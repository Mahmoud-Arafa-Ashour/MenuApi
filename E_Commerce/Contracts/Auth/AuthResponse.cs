namespace E_Commerce.Contracts.Auth
{
    public record AuthResponse
        (string id ,
        string email ,
        string Name ,
        string Adress ,
        string ResturantName ,
        string PhoneNumber,
        string token ,
        int expiresin,
        string RefreshToken,
        DateTime RefeshTokenExpiration
        );
}
