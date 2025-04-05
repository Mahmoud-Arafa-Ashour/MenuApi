using E_Commerce.Abstractions;
using E_Commerce.Contracts.Auth;

namespace E_Commerce.Services
{
    public interface IAuthServices
    {
        Task<Result<AuthResponse>> GetTokenAsync(string email , string password , CancellationToken cancellationToken = default);
        Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken);
        Task<Result> RevokeRefreshToken(string Token, string refreshToken, CancellationToken cancellationToken);
        Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
        Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
        Task<Result> ResentConfirmationEmail(ResentEmailConfirmationRequest request);
        Task<Result> TestEmailAsync(string email);
    }
}
