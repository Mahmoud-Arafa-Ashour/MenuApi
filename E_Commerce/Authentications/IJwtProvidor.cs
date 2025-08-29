namespace E_Commerce.Authentications
{
    public interface IJwtProvidor
    {
        (string token, int expirein) GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions);
        string? ValidateToken(string token);
    }
}
