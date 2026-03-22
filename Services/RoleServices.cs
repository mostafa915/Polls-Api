using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Abstractions;
using SurveyBasket.AppliacationsConfingrations;
using SurveyBasket.Constract.Basic;
using SurveyBasket.Constract.Role;
using SurveyBasket.Erorrs;
using SurveyBasket.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SurveyBasket.Services
{
    public class RoleServices(
        RoleManager<ApplicationRole> roleManager, ApllicationDbContext context) : IRoleServices
    {
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly ApllicationDbContext _context = context;

        public async Task<IEnumerable<RoleResponse>> GetRolesAsync(bool? IncludeDisabled, CancellationToken cancellationToken = default)
        {
            return await _roleManager.Roles
                .Where(r => !r.IsDefault && (!r.IsDeleted || IncludeDisabled == true))
                .ProjectToType<RoleResponse>()
                .ToListAsync(cancellationToken);
        }
        public async Task<Result<RoleDetailResponse>> GetRoleDetailAsync(string roleId)
        {
            if(await _roleManager.FindByIdAsync(roleId) is not { } role)
            {
                return Result.Faliuer<RoleDetailResponse>(RoleError.NotFound);
            }
            var permissions = await _roleManager.GetClaimsAsync(role);
            var response = new RoleDetailResponse(role.Name!, roleId, role.IsDefault, permissions.Select(x => x.Value));

            return Result.Succuess(response);

        }
        public async Task<Result<RoleDetailResponse>> AddRoleAsync(RoleRequest request, CancellationToken cancellationToken = default)
        {
            var roleIsDipulcaited = await _roleManager.Roles.AnyAsync(r => r.Name == request.Name);
            if(roleIsDipulcaited)
            {
                return Result.Faliuer<RoleDetailResponse>(RoleError.DupilcatedRole);
            }
            var permissionsIsExist = request.Permissions.All(x => Permission.GetAllPermissions().Contains(x));
            if(!permissionsIsExist)
            {
                return Result.Faliuer<RoleDetailResponse>(RoleError.InvalidPermissions);
            }
            var role = new ApplicationRole
            {
                Name = request.Name,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                var permissions = request.Permissions.Select(p => new IdentityRoleClaim<string>
                {
                    ClaimType = Permission.Type,
                    RoleId = role.Id,
                    ClaimValue = p
                });
                await _context.AddRangeAsync(permissions);
                await _context.SaveChangesAsync(cancellationToken);
                var response = new RoleDetailResponse(role.Name, role.Id, role.IsDeleted, request.Permissions);
                return Result.Succuess(response);
            }
            var error = result.Errors.First();
            return Result.Faliuer<RoleDetailResponse>(new Erorr(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        public async Task<Result> UpdateAsync(string Id, RoleRequest request, CancellationToken cancellationToken = default)
        {
            var RoleIsExist = await _roleManager.Roles.AnyAsync(r => r.Name == request.Name && r.Id != Id);
            if (RoleIsExist)
            {
                return Result.Faliuer<RoleDetailResponse>(RoleError.DupilcatedRole);
            }

            if(await _roleManager.FindByIdAsync(Id) is not { } role)
            {
                return Result.Faliuer<RoleDetailResponse>(RoleError.NotFound);
            }
           
            var permissionsIsExist = request.Permissions.All(x => Permission.GetAllPermissions().Contains(x));
            if (!permissionsIsExist)
            {
                return Result.Faliuer<RoleDetailResponse>(RoleError.InvalidPermissions);
            }

            role.Name = request.Name;
            var result = await _roleManager.UpdateAsync(role);
            if(result.Succeeded)
            {
                var currentPeermissions = await _context.RoleClaims
                    .Where(r => r.RoleId == Id && r.ClaimType == Permission.Type)
                    .Select(r => r.ClaimValue)
                    .ToListAsync();
                 
                var newPermissions = request.Permissions
                    .Except(currentPeermissions)
                    .Select(x => new IdentityRoleClaim<string>
                    {
                        ClaimType = Permission.Type,
                        RoleId = Id,
                        ClaimValue = x
                    });
                var deletedPermissions = currentPeermissions.Except(request.Permissions);
                await _context.RoleClaims
                    .Where(r => r.RoleId == Id && deletedPermissions.Contains(r.ClaimValue))
                    .ExecuteDeleteAsync(cancellationToken);
                
                _context.AddRange(newPermissions);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Succuess();
            }
            
            var error = result.Errors.First();
            return Result.Faliuer<RoleDetailResponse>(new Erorr(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        public async Task<Result> ToggleDeltedAsync(string Id)
        {
            if(await _roleManager.FindByIdAsync(Id) is not { } role)
            {
                return Result.Faliuer<RoleDetailResponse>(RoleError.NotFound);
            }
            role.IsDeleted = !role.IsDeleted;
            await _roleManager.UpdateAsync(role);
            return Result.Succuess();
        }
    }
}
