using E_Commerce.Contracts.Auth;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using SurveyBasket.Helpers;
using System.Security.Cryptography;

namespace E_Commerce.Services;

public class AuthServices(UserManager<ApplicationUser> userManager
    , IJwtProvidor jwtProvidor
    , ILogger<ApplicationUser> logger
    , SignInManager<ApplicationUser> signInManager
    , IHttpContextAccessor httpContextAccessor
    , IEmailSender emailSender
    , ApplicationDbContext dbContext) : IAuthServices
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvidor _jwtProvidor = jwtProvidor;
    private readonly ILogger<ApplicationUser> _logger = logger;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly int _refreshTokenExpiration = 14;

    public async Task<Result> TestEmailAsync(string email)
    {
        try
        {
            var testMessage = "<h1>Test Email</h1><p>This is a test email from your application.</p>";
            await _emailSender.SendEmailAsync(email, "Test Email from Menu App", testMessage);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send test email");
            return Result.Failure(new Error("Failed to send test email ", ex.Message, StatusCodes.Status400BadRequest));
        }
    }

    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        //check user existance 
        if (await _userManager.FindByEmailAsync(email) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentionals);
        //check password 
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (result.Succeeded)
        {
            var (userRoles, userPermissions) = await GetRolesAndPermissions(user, cancellationToken);
            //generate token
            var (token, expiresin) = _jwtProvidor.GenerateToken(user , userRoles, userPermissions);
            //generate Refresh Token
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpirationDate = DateTime.UtcNow.AddDays(_refreshTokenExpiration);
            //save refresh token to database 
            user.RefreshTokens.Add(new RefreshTokens
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpirationDate
            });
            await _userManager.UpdateAsync(user);
            var response = new AuthResponse(user.Id, user.Email!, user.Name, user.Adress, user.ResturnatName, user.PhoneNumber, token, expiresin, refreshToken, refreshTokenExpirationDate);
            return Result.Success(response);
        }
        return Result.Failure<AuthResponse>(result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredentionals);
    }
    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken)
    {
        //check for the valid token 
        var userId = jwtProvidor.ValidateToken(token);
        if (userId is null) return Result.Failure<AuthResponse>(TokenErrors.EmptyToken);
        //check for the id
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return Result.Failure<AuthResponse>(TokenErrors.EmptyToken);
        //check for the refresh token 
        var UserrefreshToken = user.RefreshTokens?.FirstOrDefault(x => x.Token == refreshToken && x.IsActive);
        if (UserrefreshToken == null) return Result.Failure<AuthResponse>(TokenErrors.EmptyToken);
        //make it valid for one time 
        UserrefreshToken.RevokedOn = DateTime.UtcNow;
        //Make the creation of both refreshtoken and token
        var (userRoles, userPermissions) = await GetRolesAndPermissions(user, cancellationToken);
        var (newtoken, expiresin) = _jwtProvidor.GenerateToken(user, userRoles, userPermissions);
        var newrefreshToken = GenerateRefreshToken();
        var refreshTokenExpirationDate = DateTime.UtcNow.AddDays(_refreshTokenExpiration);
        //save refresh token to database 
        user.RefreshTokens!.Add(new RefreshTokens
        {
            Token = newrefreshToken,
            ExpiresOn = refreshTokenExpirationDate
        });
        await _userManager.UpdateAsync(user);
        var response = new AuthResponse(user.Id, user.Email!, user.Name, user.Adress, user.ResturnatName, user.PhoneNumber, newtoken, expiresin, newrefreshToken, refreshTokenExpirationDate);
        return Result.Success(response);
    }
    public async Task<Result> RevokeRefreshToken(string Token, string refreshToken, CancellationToken cancellationToken)
    {
        //check for the valid token 
        var userId = jwtProvidor.ValidateToken(Token);
        if (userId is null) return Result.Failure(TokenErrors.EmptyToken);
        //check for the id
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return Result.Failure(TokenErrors.EmptyToken);
        //check for the refresh token 
        var UserrefreshToken = user.RefreshTokens?.FirstOrDefault(x => x.Token == refreshToken && x.IsActive);
        if (UserrefreshToken == null) return Result.Failure(TokenErrors.EmptyToken);
        //make it valid for one time 
        UserrefreshToken.RevokedOn = DateTime.UtcNow;
        await userManager.UpdateAsync(user);
        return Result.Success();
    }
    public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        //check if the email is Duplicated
        var IsExistedEmail = await _userManager.Users.AnyAsync(x => x.Email == request.Email);
        if (IsExistedEmail)
            return Result.Failure(UserErrors.DuplicateEmail);
        var user = request.Adapt<ApplicationUser>();
        user.UserName = request.Email;
        user.Adress = request.Address;
        user.ResturnatName = request.ResturantName;
        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("User with email {Email} has been registered and the confirmation code is {code}", request.Email, code);
            await SendEmailConfirmation(user, code);
            return Result.Success();
        }
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        //check if the user exist
        if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
            return Result.Failure(UserErrors.InvalidCode);
        //check if the email is already confirmed
        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicateConfirmation);
        //getting code
        var code = request.Code;
        //Encode the code
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }
        //confirm the email
        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            _logger.LogInformation("User with email {Email} has been confirmed", user.Email);
            await _userManager.AddToRoleAsync(user, DefaultRoles.Owner);
            return Result.Success();
        }
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Result> ResentConfirmationEmail(ResentEmailConfirmationRequest request)
    {
        //check the email
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();
        //check if the email is already confirmed
        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicateConfirmation);
        // sending the code 
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        _logger.LogInformation("User with email {Email} has been resent the confirmation code {code}", request.Email, code);
        //sending the email
        await SendEmailConfirmation(user, code);
        return Result.Success();
    }
    private async Task SendEmailConfirmation(ApplicationUser user, string code)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin.ToString();
        // If origin is empty, use a default value
        if (string.IsNullOrEmpty(origin))
        {
            origin = "http://localhost:3000"; // Default value, adjust as needed
            _logger.LogWarning("Origin header is missing, using default value: {origin}", origin);
        }

        // Use the GET endpoint for automatic confirmation
        var confirmationUrl = $"{origin}/api/login/confirm-email?userId={user.Id}&code={code}";

        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
            new Dictionary<string, string>
        {
            {"{{name}}", user.Name! },
            {"{{action_url}}", confirmationUrl},
        });

        _logger.LogInformation("Sending confirmation email to {Email} with URL: {Url}",
            user.Email, confirmationUrl);

        await _emailSender.SendEmailAsync(user.Email!, "Email Confirmation : Menu App", emailBody);
    }
    private static string GenerateRefreshToken() =>
         Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    private async Task<(IEnumerable<string> Roles, IEnumerable<string> Permissions)> GetRolesAndPermissions(ApplicationUser user, CancellationToken cancellationToken)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        var userPermissions = await (from r in _dbContext.Roles
                                     join p in _dbContext.RoleClaims
                                     on r.Id equals p.RoleId
                                     where userRoles.Contains(r.Name!)
                                     select p.ClaimValue!)
                                     .Distinct()
                                     .ToListAsync(cancellationToken);
        return (userRoles, userPermissions);

    }
}
