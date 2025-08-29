using E_Commerce.Contracts.Role;

namespace E_Commerce.Services;

public class RoleService(RoleManager<ApplicationRole> roleManager , ApplicationDbContext dbContext) : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<RoleResponse>> GetAllRolesAsync(CancellationToken cancellationToken) =>
        await _roleManager.Roles
        .Where(x => !x.isDefault)
        .ProjectToType<RoleResponse>()
        .ToListAsync(cancellationToken);

    public async Task<IEnumerable<RoleResponse>> GetActiveRoles(CancellationToken cancellationToken) =>
        await _roleManager.Roles
        .Where(x => !x.IsDeleted)
        .ProjectToType<RoleResponse>()
        .ToListAsync(cancellationToken);

    public async Task<Result<RoleResponse>> AddRoleAsync(RoleRequest roleRequest , CancellationToken cancellationToken)
    {
        var isExistRole = await _roleManager.RoleExistsAsync(roleRequest.Name);
        if (isExistRole)
            return Result.Failure<RoleResponse>(RoleErrors.DuplicateRole);
        var ExistedPermissions = Permissions.GetAllPermissions;
        if(ExistedPermissions.Except(roleRequest.Permissions).Any())
            return Result.Failure<RoleResponse>(RoleErrors.NotValid);
        var role = new ApplicationRole()
        {
            Name = roleRequest.Name,
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };
        var result = await _roleManager.CreateAsync(role);
        if(result.Succeeded)
        {
            var Permission = roleRequest.Permissions.Select(x => new IdentityRoleClaim<string>()
            {
                ClaimType = Permissions.Type,
                ClaimValue = x ,
                RoleId = role.Id
            }).ToList();
            await _dbContext.AddRangeAsync(Permission, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            var response = new RoleResponse(
                role.Id,
                role.Name,
                false);
            return Result.Success(response);
        }
        var error = result.Errors.FirstOrDefault();
        return Result.Failure<RoleResponse>(new Error(error!.Code,error.Description,StatusCodes.Status409Conflict));
    }
}
