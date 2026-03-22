using SurveyBasket.Abstractions;
using SurveyBasket.Constract.Role;

namespace SurveyBasket.Services
{
    public interface IRoleServices
    {
        Task<IEnumerable<RoleResponse>> GetRolesAsync(bool? IncludeDisabled, CancellationToken cancellationToken = default);
        Task<Result<RoleDetailResponse>> GetRoleDetailAsync(string roleId);
        Task<Result<RoleDetailResponse>> AddRoleAsync(RoleRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(string Id, RoleRequest request, CancellationToken cancellationToken = default);
        Task<Result> ToggleDeltedAsync(string Id);
    }
}
