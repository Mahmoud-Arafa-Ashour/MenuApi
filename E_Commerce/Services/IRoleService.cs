using E_Commerce.Contracts.Role;
namespace E_Commerce.Services;
public interface IRoleService
{
    Task<IEnumerable<RoleResponse>> GetAllRolesAsync(CancellationToken cancellationToken);
    Task<IEnumerable<RoleResponse>> GetActiveRoles(CancellationToken cancellationToken);
    Task<Result<RoleResponse>> AddRoleAsync(RoleRequest roleRequest, CancellationToken cancellationToken);
}
