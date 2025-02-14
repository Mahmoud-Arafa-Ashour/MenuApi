namespace E_Commerce.Authentications
{
    public interface IJwtProvidor
    {
        (string token, int expirein) GenerateToken(ApplicationUser user);
        string? ValidateToken(string token);
    }
}
