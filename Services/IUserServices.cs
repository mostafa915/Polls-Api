using SurveyBasket.Abstractions;
using SurveyBasket.Constract.User;

namespace SurveyBasket.Services
{
    public interface IUserServices
    {
        Task<Result<UserProfileResponse>> InfoAsync(string UserId, CancellationToken cancellationToken = default);
        Task<Result> UpdateProfileAsync(string UserId, UpdateUserProfileRequest request);
        Task<Result> ChangePasswordAsync(string UserId, ChangePasswordRequest request);
        Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<UserResponse>> GetAsync(string Id);
        Task<Result<UserResponse>> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(string Id, UpdateUserRequest request, CancellationToken cancellationToken = default);
        Task<Result> ToggleDisabled(string id);
        Task<Result> UnlockUser(string id);
    }
}
