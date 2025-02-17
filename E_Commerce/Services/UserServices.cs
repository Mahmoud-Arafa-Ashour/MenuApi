namespace E_Commerce.Services
{
    public class UserServices(UserManager<ApplicationUser> userManager) : IUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result<UserProfileResponse>> GetUserInfo(string userid)
        {
            var user = await _userManager.Users
                .Where(x => x.Id == userid)
                .ProjectToType<UserProfileResponse>()
                .SingleAsync();
            return Result.Success(user);
        }
        public async Task<Result> UpdateProfile(string userid, UpdateProfileRequest request)
        {
            var user = await _userManager.FindByIdAsync(userid);
            user = request.Adapt(user);
            await _userManager.UpdateAsync(user!);
            return Result.Success();
        }
        public async Task<Result> ChangePassword(string userid , ChangePasswordRequset requset)
        {
            var user = await _userManager.FindByIdAsync(userid);
            var result = await _userManager.ChangePasswordAsync(user!, requset.CurrentPassword, requset.NewPassword);
            if(result.Succeeded) 
                return Result.Success();
            var errors = result.Errors.First();
            return Result.Failure(new Error(errors.Code, errors.Description, StatusCodes.Status400BadRequest));
        }
    }
}
