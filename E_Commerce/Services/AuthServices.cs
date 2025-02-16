using E_Commerce.Abstractions;
using E_Commerce.Authentications;
using E_Commerce.Contracts.Auth;
using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using static E_Commerce.Abstractions.Errors;

namespace E_Commerce.Services
{
    public class AuthServices(UserManager<ApplicationUser> userManager 
        , IJwtProvidor jwtProvidor 
        , ILogger<ApplicationUser> logger) : IAuthServices
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtProvidor _jwtProvidor = jwtProvidor;
        private readonly ILogger<ApplicationUser> _logger = logger;
        private readonly int _refreshTokenExpiration = 14;
        public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            //check user existance 
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentionals);
            //check password 
            var isvalidPass = await _userManager.CheckPasswordAsync(user, password);
            if (!isvalidPass) 
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentionals);
            //generate token
            var (token, expiresin) = _jwtProvidor.GenerateToken(user);
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
            var (newtoken, expiresin) = _jwtProvidor.GenerateToken(user);
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
        public async Task<Result> RegisterAsync(RegisterRequest request , CancellationToken cancellationToken)
        {
            //check if the email is Duplicated
            var IsExistedEmail = await _userManager.Users.AnyAsync(x=>x.Email == request.Email);
            if (IsExistedEmail)
                return Result.Failure(UserErrors.DuplicateEmail);
            var user = request.Adapt<ApplicationUser>();
            user.UserName = request.Email;
            user.Adress = request.Address;
            user.ResturnatName = request.ResturantName;
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return Result.Failure<AuthResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
            }
            return Result.Success();
        }
        private static string GenerateRefreshToken() =>
             Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
