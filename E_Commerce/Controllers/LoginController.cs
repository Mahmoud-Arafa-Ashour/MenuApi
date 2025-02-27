namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(IAuthServices authServices) : ControllerBase
    {
        private readonly IAuthServices _authServices = authServices;

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody]LoginRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _authServices.GetTokenAsync(request.Email, request.Password, cancellationToken);
            return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody]RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var authResponse = await _authServices.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
            return authResponse.IsSuccess ?
               Ok() :
               authResponse.ToProblem();
        }
        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _authServices.RevokeRefreshToken(request.Token, request.RefreshToken, cancellationToken);
            return authResult.IsSuccess ?
                Ok() :
                authResult.ToProblem();
        }
        [HttpPost("register")]
        public async Task<IActionResult> registerAsync(RegisterRequest request, CancellationToken cancellationToken)
        {
            var result = await _authServices.RegisterAsync(request , cancellationToken);
            return result.IsSuccess ?
                Ok() :
                result.ToProblem();
        }
    }
}
