namespace E_Commerce.Contracts.login
{
    public record RefreshTokenRequest
        (
        string Token , 
        string RefreshToken
        );
}
