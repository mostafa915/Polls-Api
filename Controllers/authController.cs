using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SurveyBasket.Abstractions;
using SurveyBasket.Constract.Authentication;
using SurveyBasket.Erorrs;
using SurveyBasket.Extions;
using SurveyBasket.Services;

namespace SurveyBasket.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthentication authentication, IOptions<JwtOptions> options) : ControllerBase
    {
        private readonly IAuthentication _authentication = authentication;

        public JwtOptions _JwtOptions { get; } = options.Value;

        [HttpPost]
        public  async Task<IActionResult> Login([FromBody] AuthenRequest request, CancellationToken cancellationToken)
        {
            var result = await _authentication.GetTokenAsync(request.email, request.password, cancellationToken);
            return result.IsSucess ?Ok(result.Value) : result.ToProblem();
        }
        //[HttpPost("refresh")]
        //public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        //{
        //    var result  = await _authentication.GetNewRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
        //    return result.IsSucess? Ok(result.Value) : result.ToProblem();
        //}
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            var result = await _authentication.RegisterAsync(request, cancellationToken);
            return result.IsSucess? Ok() : result.ToProblem();
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmRequest request, CancellationToken cancellationToken)
        {
            var result = await _authentication.ConfirmEmailAsync(request);
            return result.IsSucess? Ok() :result.ToProblem();
        }

        [HttpPost("resend-confirm-email")]
        public async Task<IActionResult> ResendConfirmEmail([FromBody] ResendConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            var result = await _authentication.ResendEmailConfirAsync(request,cancellationToken);
            return result.IsSucess ? Ok() : result.ToProblem();
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] EmailResetPasswordRequest request)
        {
            var result = await _authentication.ResetPasswordEmailCodeAsync(request.Email);
            return result.IsSucess ? Ok() : result.ToProblem();
        }
        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authentication.ResetPasswordConfirmAsync(request);
            return result.IsSucess? Ok() : result.ToProblem();   
        }

        [HttpPut("invoke-refresh-token")]
        public async Task<IActionResult> invokeRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await _authentication.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
            return result.IsSucess ? Ok() : result.ToProblem();

        }
    }
}
