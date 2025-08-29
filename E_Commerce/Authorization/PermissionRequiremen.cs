namespace E_Commerce.Authorization;

public class PermissionRequiremen(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
