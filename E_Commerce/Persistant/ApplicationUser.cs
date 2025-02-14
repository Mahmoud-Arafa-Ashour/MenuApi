using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Persistant
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string Adress { get; set; } = string.Empty;
        public string ResturnatName { get; set; } = string.Empty;
        public List<RefreshTokens> RefreshTokens { get; set; } = [];
    }
}
