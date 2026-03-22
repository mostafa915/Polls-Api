using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Constract.User;
using SurveyBasket.Extions;
using SurveyBasket.Services;

namespace SurveyBasket.Controllers
{
    [Route("me")]
    [ApiController]
    public class AccountsController(IUserServices userServices) : ControllerBase
    {
        private readonly IUserServices _userServices = userServices;

        [HttpGet("")]
        public async Task<IActionResult> Info(CancellationToken cancellationToken)
        {
            var result = await _userServices.InfoAsync(User.GetUserId()!);
            return Ok(result.Value);
        }
        [HttpPut("info")]
        public async Task<IActionResult> UpdateInfo([FromBody] UpdateUserProfileRequest request)
        {
            var result = await _userServices.UpdateProfileAsync(User.GetUserId()!, request);
            return NoContent();
        }
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = await _userServices.ChangePasswordAsync(User.GetUserId()!, request);
            return result.IsSucess? NoContent() : result.ToProblem();
        }
    }
}
