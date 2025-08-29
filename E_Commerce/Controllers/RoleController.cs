using E_Commerce.Contracts.Role;

namespace E_Commerce.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class RoleController(IRoleService roleService) : ControllerBase
{
    public IRoleService RoleService { get; } = roleService;
    [HttpGet]
    public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
    {
        var roles = await RoleService.GetAllRolesAsync(cancellationToken);
        return Ok(roles);
    }
    [HttpGet]
    public async Task<IActionResult> GetActiveRoles(CancellationToken cancellationToken)
    {
        var roles = await RoleService.GetActiveRoles(cancellationToken);
        return Ok(roles);
    }
    [HttpPost]
    public async Task<IActionResult> AddRole([FromBody] RoleRequest roleRequest, CancellationToken cancellationToken)
    {
        var result = await RoleService.AddRoleAsync(roleRequest, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
