namespace E_Commerce.Services
{
    public interface IUserServices
    {
        Task<Result<UserProfileResponse>> GetUserInfo(string userid);
        Task<Result> UpdateProfile(string userid, UpdateProfileRequest request);
        Task<Result> ChangePassword(string userid, ChangePasswordRequset requset);
    }
}
