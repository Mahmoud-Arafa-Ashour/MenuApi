using System.Security.Claims;

namespace E_Commerce.Abstractions
{
    public static class UserExtentions
    {
        public static string? GetUserID(this ClaimsPrincipal user) =>
            user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
