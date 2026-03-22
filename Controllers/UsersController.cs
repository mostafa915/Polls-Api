using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Constract.Basic;
using SurveyBasket.Constract.User;
using SurveyBasket.Extions;
using SurveyBasket.Jwt.Autehntication.Filiters;
using SurveyBasket.Services;

namespace SurveyBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserServices userServices) : ControllerBase
    {
        private readonly IUserServices _userServices = userServices;

        [HttpGet("")]
        [HasPermission(Permission.GetUsers)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await _userServices.GetAllAsync(cancellationToken));
        }

        [HttpGet("{id}")]
        [HasPermission(Permission.GetUsers)]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _userServices.GetAsync(id);
            return result.IsSucess?  Ok(result.Value) : result.ToProblem();
        }
        
        
        [HttpPost("")]
        [HasPermission(Permission.AddUsers)]
        public async Task<IActionResult> Add([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _userServices.CreateAsync(request, cancellationToken);
            return result.IsSucess ? CreatedAtAction(nameof(Get), new { result.Value.Id }, result.Value) : result.ToProblem();
        }

        [HttpPut("{id}")]
        [HasPermission(Permission.UpdateUsers)]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _userServices.UpdateAsync(id, request, cancellationToken);
            return result.IsSucess ? NoContent() : result.ToProblem();

        }

        [HttpPut("{id}/toogle-status")]
        [HasPermission(Permission.UpdateUsers)]
        public async Task<IActionResult> UpdateStatus([FromRoute] string id)
        {
            var result = await _userServices.ToggleDisabled(id);
            return result.IsSucess ? NoContent() : result.ToProblem();

        }

        [HttpPut("{id}/unlock")]
        [HasPermission(Permission.UpdateUsers)]
        public async Task<IActionResult> Unlock([FromRoute] string id)
        {
            var result = await _userServices.UnlockUser(id);
            return result.IsSucess ? NoContent() : result.ToProblem();

        }
    }
}
