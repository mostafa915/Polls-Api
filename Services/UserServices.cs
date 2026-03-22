using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Transport.NamedPipes;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Abstractions;
using SurveyBasket.AppliacationsConfingrations;
using SurveyBasket.Constract.Basic;
using SurveyBasket.Constract.Role;
using SurveyBasket.Constract.User;
using SurveyBasket.Erorrs;
using SurveyBasket.Models;

namespace SurveyBasket.Services
{
    public class UserServices(UserManager<ApplicationUser> userManager,
        ApllicationDbContext context,
        IRoleServices roleServices) : IUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApllicationDbContext _context = context;
        private readonly IRoleServices _roleServices = roleServices;

        public async Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>

             await (from user in _context.Users
                    join roleUser in _context.UserRoles
                    on user.Id equals roleUser.UserId
                    join role in _context.Roles
                    on roleUser.RoleId equals role.Id into roles
                    where !roles.Any(r => r.Name == DefaultRole.Member)
                    select new
                            {
                                user.Id,
                                user.FirstName,
                                user.LastName,
                                user.Email,
                                user.IsDiabled,
                                Roles = roles.Select(r => r.Name!).ToList()
                            })
                    .GroupBy(u => new { u.Id, u.Email, u.FirstName, u.LastName, u.IsDiabled })
                        .Select(u => new UserResponse(
                            u.Key.Id,
                            u.Key.FirstName,
                            u.Key.LastName,
                            u.Key.Email,
                            u.Key.IsDiabled,
                            u.SelectMany(x => x.Roles)
                            ))
                    .ToListAsync();

        public async Task<Result<UserResponse>> GetAsync(string Id)
        {
            if(await _userManager.FindByIdAsync(Id) is not { } user)
            {
                return Result.Faliuer<UserResponse>(UserErorr.NotFound);
            }
            
            var userRoles = await _userManager.GetRolesAsync(user);
            var response = (user, userRoles).Adapt<UserResponse>();
            return Result.Succuess(response);
        }  


        public async Task<Result<UserResponse>> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
        {
            var EmailIsExist= await _userManager.Users.AnyAsync(u => u.Email == request.Email);
            if(EmailIsExist)
            {
                return Result.Faliuer<UserResponse>(UserErorr.DupilcatedEmail);
            }
            
            var User = request.Adapt<ApplicationUser>();

            var AllowedRoles = await _roleServices.GetRolesAsync(false, cancellationToken);
            if(request.Roles.Except(AllowedRoles.Select(x => x.Name)).Any())
            {
                return Result.Faliuer<UserResponse>(UserErorr.InvalidRoles);
            }

            var result = await _userManager.CreateAsync(User, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(User, request.Roles);
                var response = (User, request.Roles).Adapt<UserResponse>();
                return Result.Succuess(response);

            }

            var error = result.Errors.First();
            return Result.Faliuer<UserResponse>(new Erorr(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> UpdateAsync(string Id, UpdateUserRequest request, CancellationToken cancellationToken = default)
        {
            var EmailIsExist = await _userManager.Users.AnyAsync(u => u.Email == request.Email && u.Id != Id);
            if (EmailIsExist)
            {
                return Result.Faliuer(UserErorr.DupilcatedEmail);
            }

            if(await _userManager.FindByIdAsync(Id) is not { } User)
            {
                return Result.Faliuer(UserErorr.NotFound);
            }


            var AllowedRoles = await _roleServices.GetRolesAsync(false, cancellationToken);
            if (request.Roles.Except(AllowedRoles.Select(x => x.Name)).Any())
            {
                return Result.Faliuer(UserErorr.InvalidRoles);
            }
            User = request.Adapt(User);
            var result = await _userManager.UpdateAsync(User);
            
            if (result.Succeeded)
            {
                await _context.UserRoles
                    .Where(r => r.UserId == Id)
                    .ExecuteDeleteAsync(cancellationToken);

                await _userManager.AddToRolesAsync(User, request.Roles);
                return Result.Succuess();

            }

            var error = result.Errors.First();
            return Result.Faliuer(new Erorr(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result<UserProfileResponse>> InfoAsync(string UserId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users
                .Where(u => u.Id == UserId)
                .ProjectToType<UserProfileResponse>()
                .FirstAsync(cancellationToken);
            
            return Result.Succuess(user);
        }
        public async Task<Result> UpdateProfileAsync(string UserId, UpdateUserProfileRequest request)
        {
            await _userManager.Users
                .Where(u => u.Id == UserId)
                .ExecuteUpdateAsync(set =>
                set
                .SetProperty(p => p.FirstName, request.FirstName)
                .SetProperty(p => p.LastName, request.LastName));
            return Result.Succuess();
        }
        public async Task<Result> ChangePasswordAsync(string UserId, ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            var result =  await _userManager.ChangePasswordAsync(user!,request.OldPassword,request.NewPassword);

            if (result.Succeeded)
            {
                return Result.Succuess();
            }
            
            var error = result.Errors.First();

            return Result.Faliuer(new Erorr(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        public async Task<Result> ToggleDisabled(string id)
        {
            if(await _userManager.FindByIdAsync(id) is not { } user)
            {
                return Result.Faliuer(UserErorr.NotFound);
            }
            user.IsDiabled = !user.IsDiabled;
            var result =  await _userManager.UpdateAsync(user);
            if(result.Succeeded)
                return Result.Succuess();
            var error = result.Errors.First();

            return Result.Faliuer(new Erorr(error.Code, error.Description, StatusCodes.Status400BadRequest));

        }
        public async Task<Result> UnlockUser(string id)
        {
            if(await _userManager.FindByIdAsync(id) is not { } user)
                return Result.Faliuer(UserErorr.NotFound);


            var result = await _userManager.SetLockoutEndDateAsync(user, null);
            
            if(result.Succeeded)
            {
                return Result.Succuess();
            }
            var error = result.Errors.First();

            return Result.Faliuer(new Erorr(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
    }
}
