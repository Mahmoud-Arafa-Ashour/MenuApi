using E_Commerce.Contracts.Auth;

namespace E_Commerce.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IAuthServices _authServices;
    private readonly ILogger<LoginController> _logger;

    public LoginController(IAuthServices authServices, ILogger<LoginController> logger)
    {
        _authServices = authServices;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody]LoginRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authServices.GetTokenAsync(request.Email, request.Password, cancellationToken);
        return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
    }
    [HttpPost]
    public async Task<IActionResult> RefreshTokenAsync([FromBody]RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var authResponse = await _authServices.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
        return authResponse.IsSuccess ?
           Ok() :
           authResponse.ToProblem();
    }
    [HttpPost]
    public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authServices.RevokeRefreshToken(request.Token, request.RefreshToken, cancellationToken);
        return authResult.IsSuccess ?
            Ok() :
            authResult.ToProblem();
    }
    [HttpPost]
    public async Task<IActionResult> registerAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authServices.RegisterAsync(request, cancellationToken);
        return result.IsSuccess ?
            Ok() :
            result.ToProblem();
    }
    [HttpGet]
    public async Task<IActionResult> ConfirmEmailGet([FromQuery] string userId, [FromQuery] string code)
    {
        var request = new ConfirmEmailRequest(userId, code);
        var result = await _authServices.ConfirmEmailAsync(request);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpPost]
    public async Task<IActionResult> ResentConfirmEmailAsync(ResentEmailConfirmationRequest request)
    {
        var result = await _authServices.ResentConfirmationEmail(request);
        return result.IsSuccess ?
            Ok() :
            result.ToProblem();
    }
}
