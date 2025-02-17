using System.Text.RegularExpressions;

namespace E_Commerce.Contracts.User
{
    public record ChangePasswordRequset(string CurrentPassword ,string NewPassword);
}
