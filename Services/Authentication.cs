using Hangfire;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Options;
using SurveyBasket.Abstractions;
using SurveyBasket.AppliacationsConfingrations;
using SurveyBasket.Constract.Authentication;
using SurveyBasket.Constract.Basic;
using SurveyBasket.Erorrs;
using SurveyBasket.Helpers;
using SurveyBasket.Jwt;
using SurveyBasket.Models;
using System.Security.Cryptography;
using System.Text;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace SurveyBasket.Services
{
    public class Authentication(
        UserManager<ApplicationUser> userManager,
        IJwtProvider jwtProvider,
        SignInManager<ApplicationUser> signInManager,
        ILogger<Authentication> logger,
        IEmailSender emailSender,
        IHttpContextAccessor httpContextAccessor,
        ApllicationDbContext context) 
        : IAuthentication
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly ILogger<Authentication> _logger = logger;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ApllicationDbContext _context = context;
        private readonly int refreshTokenExpireDays = 14;
        public async Task<Result<AutheResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
               return Result.Faliuer<AutheResponse>(UserErorr.InvalidCredintails);
            }
            if(user.IsDiabled)
            {
                return Result.Faliuer<AutheResponse>(UserErorr.DisabledUser);
            }
            var result = await _signInManager.PasswordSignInAsync(user!, password , false , true);
            if(result.Succeeded) {
                
                var (roles, permissions) = await GetRolesAndPermissions(user, cancellationToken);


                var (token, experIn) = _jwtProvider.GenerateToken(user!, roles, permissions!);
                var RefreshToken = GenerateRefreshToken();
                var RefreshTokenExpire = DateTime.UtcNow.AddDays(refreshTokenExpireDays);
                user.RefreshTokens.Add(new RefreshToken
                {
                    Token = RefreshToken,
                    ExpireOn = RefreshTokenExpire
                });
                await _userManager.UpdateAsync(user);
                var Response = new AutheResponse(user.Id, user.FirstName, user.LastName, user.Email, token, experIn * 60, RefreshToken, RefreshTokenExpire);
                return Result.Succuess(Response);
            }
            var error = result.IsNotAllowed
                ? UserErorr.EmailNotConfirmed
                : result.IsLockedOut
                ? UserErorr.MaxiamumTrySignIn
                : UserErorr.InvalidCredintails;

            return Result.Faliuer<AutheResponse>(error);
        }
        public async Task<Result<AutheResponse>> GetNewRefreshTokenAsync(string Token, string RefershToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(Token);
            if (userId is null)
            {
                return Result.Faliuer<AutheResponse>(UserErorr.InvalidId);
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return Result.Faliuer<AutheResponse>(UserErorr.InvalidId);
            }
            if (user.IsDiabled)
            {
                return Result.Faliuer<AutheResponse>(UserErorr.DisabledUser);
            }
            if(user.LockoutEnd > DateTime.UtcNow)
            {
                return Result.Faliuer<AutheResponse>(UserErorr.MaxiamumTrySignIn);
            }
            var userRefershToken = user.RefreshTokens.FirstOrDefault(x => x.Token == RefershToken && x.IsActive);
            if (userRefershToken is null)
            {
                return Result.Faliuer<AutheResponse>(UserErorr.HaveAlreadyToken);
            }
            userRefershToken.RevokeOn = DateTime.UtcNow;
            var (roles, permissions) = await GetRolesAndPermissions(user, cancellationToken);
            var (NewToken, experIn) = _jwtProvider.GenerateToken(user, roles, permissions);
            var NewRefreshToken = GenerateRefreshToken();
            var RefreshTokenExpire = DateTime.UtcNow.AddDays(refreshTokenExpireDays);
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = NewRefreshToken,
                ExpireOn = RefreshTokenExpire
            });
            await _userManager.UpdateAsync(user);
            var response = new AutheResponse(user.Id, user.FirstName, user.LastName, user.Email, NewToken, experIn * 60, NewRefreshToken, RefreshTokenExpire);
            return Result.Succuess(response);
        }


        public async Task<Result> RevokeRefreshTokenAsync(string token, string RefershToken, CancellationToken cancellationToken)
        {
            var userId = _jwtProvider.ValidateToken(token);

            if(userId is null)
            {
                return Result.Faliuer(UserErorr.InvalidId); ;
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return Result.Faliuer(UserErorr.InvalidId); ;
            }

            var refreshToken = user.RefreshTokens.FirstOrDefault(u => u.Token == RefershToken && u.IsActive);
            if (refreshToken is null)
            {

                return Result.Faliuer(UserErorr.HaveAlreadyToken); 
            }
            refreshToken.RevokeOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);
            return Result.Succuess();
        }
        public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var EmailIsExistBefore = await _userManager.Users.AnyAsync(u => u.Email == request.Email);
            if(EmailIsExistBefore)
            {
                return Result.Faliuer<AutheResponse>(UserErorr.DupilcatedEmail);
            }
            var user = request.Adapt<ApplicationUser>();
            user.UserName = $"{request.FirstName}{request.LastName}";
            var result = await _userManager.CreateAsync(user, request.Password);
            if(result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                _logger.LogInformation("Confirmation Code {code}", code);
                await SendConfirmationEmail(user, code);
                return Result.Succuess();
            }
            var erorr = result.Errors.First();
            return Result.Faliuer<AutheResponse>(new Erorr(erorr.Code, erorr.Description, StatusCodes.Status400BadRequest));
        }
        public async Task<Result> ConfirmEmailAsync(EmailConfirmRequest request, CancellationToken cancellationToken = default)
        {
            if(await _userManager.FindByIdAsync(request.UserId) is not { } user)
            {
                return Result.Faliuer(UserErorr.InvalidCode);
            }
            if(await _userManager.IsEmailConfirmedAsync(user))
            {
                return Result.Faliuer(UserErorr.DupilcatedConfirmation);
            }
            var code = request.Code;
            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch (FormatException)
            {
                return Result.Faliuer(UserErorr.InvalidCode);
            } 
            var result = await _userManager.ConfirmEmailAsync(user, code);
            
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, DefaultRole.Member); 
                return Result.Succuess(); 
            }
            
            var error = result.Errors.First();
            return Result.Faliuer(new Erorr(error.Code, error.Description, StatusCodes.Status400BadRequest));

        }
        public async Task<Result> ResendEmailConfirAsync(ResendConfirmEmailRequest request, CancellationToken cancellationToken = default)
        {
            if(await _userManager.FindByEmailAsync(request.Email) is not { } user)
            {
                return Result.Succuess();
            }
            if (user.EmailConfirmed)
            {
                return Result.Faliuer(UserErorr.DupilcatedConfirmation);
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("Confirmation Code {code}", code);
            await SendConfirmationEmail(user, code);

            return Result.Succuess();  
        }
       
        
        public async Task<Result> ResetPasswordEmailCodeAsync(string email)
        {
            if(await _userManager.FindByEmailAsync(email) is not { } user)
            {
                return Result.Succuess();
            }
            if (!user.EmailConfirmed)
            {
                return Result.Faliuer(UserErorr.EmailNotConfirmed);
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("Reset Password Code {code}", code);
            await SendConfirmationResetPassword(user, code);

            return Result.Succuess();
        }

       public async Task<Result> ResetPasswordConfirmAsync(ResetPasswordRequest request)
        {
            var User = await _userManager.FindByEmailAsync(request.Email);
            if(User is null || !User.EmailConfirmed)
            {
                return Result.Faliuer(UserErorr.InvalidCode);
            }
            IdentityResult result;
            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
                result = await _userManager.ResetPasswordAsync(User, code, request.NewPassword);
            }
            catch (FormatException)
            {
                result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
            }

            if(result.Succeeded)
            {
                return Result.Succuess();
            }
            var error = result.Errors.First();
            return Result.Faliuer(new Erorr(error.Code, error.Description, StatusCodes.Status401Unauthorized));
        }
        
        
        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
        private async Task SendConfirmationEmail(ApplicationUser user, string code)
        {

            var Orign = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
            var EmailBody = EmailBodyBuilder.GenerateEmailBody("emailConfrim", new Dictionary<string, string>
                {
                    { "{{name}}" , user.FirstName },
                    { "{{action_url}}", $"{Orign}/auth/emailconfirmation?userId={user.Id}&code={code}"}
                });
            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "Survey Basket: Email Confirmation", EmailBody));

            //await _emailSender.SendEmailAsync(user.Email!, "Survey Basket: Email Confirmation", EmailBody);
            await Task.CompletedTask;
        }
        private async Task SendConfirmationResetPassword(ApplicationUser user, string code)
        {

            var Orign = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
            var EmailBody = EmailBodyBuilder.GenerateEmailBody("ResetPasswordEmail", new Dictionary<string, string>
                {
                    { "{{name}}" , user.FirstName },
                    { "{{action_url}}", $"{Orign}/auth/emailconfirmation?email={user.Email}&code={code}"}
                });
            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "Survey Basket: Change Password", EmailBody));

            //await _emailSender.SendEmailAsync(user.Email!, "Survey Basket: Email Confirmation", EmailBody);
            await Task.CompletedTask;
        }
        private async Task<(IEnumerable<string> roles, IEnumerable<string> permissions)> GetRolesAndPermissions(ApplicationUser user, CancellationToken cancellationToken)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var permissions = await (from role in _context.Roles
                              join roleClaim in _context.RoleClaims
                              on role.Id equals roleClaim.RoleId
                              where roles.Contains(role.Name!)
                              select roleClaim.ClaimValue)
                              .Distinct()
                              .ToListAsync();

            var (token, experIn) = _jwtProvider.GenerateToken(user!, roles, permissions!);
            return (roles, permissions!);

        }
    }
}
