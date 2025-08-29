namespace E_Commerce.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class AccountController(IUserServices userServices) : ControllerBase
{
    private readonly IUserServices _userServices = userServices;
    [HttpGet]
    public async Task<IActionResult> Info()
    {
        var result = await _userServices.GetUserInfo(User.GetUserID()!);
        return Ok(result);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateInfo(UpdateProfileRequest request)
    {
        var result = await _userServices.UpdateProfile(User.GetUserID()!, request);
        return NoContent();
    }
    [HttpPut]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequset requset)
    {
        var result = await _userServices.ChangePassword(User.GetUserID()!, requset);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
