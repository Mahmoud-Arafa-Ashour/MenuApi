using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Contracts.Auth
{
    public class TestEmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
} 