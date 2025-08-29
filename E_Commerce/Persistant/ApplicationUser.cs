namespace E_Commerce.Persistant;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Adress { get; set; } = string.Empty;
    [Required]
    public string ResturnatName { get; set; } = string.Empty;
    public List<RefreshTokens> RefreshTokens { get; set; } = [];
}
// Adress ResturnatName Name 
