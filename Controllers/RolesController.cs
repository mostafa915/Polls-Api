using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Constract.Basic;
using SurveyBasket.Constract.Role;
using SurveyBasket.Extions;
using SurveyBasket.Jwt.Autehntication.Filiters;
using SurveyBasket.Services;

namespace SurveyBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(IRoleServices roleServices) : ControllerBase
    {
        private readonly IRoleServices _roleServices = roleServices;

        [HttpGet("")]
        [HasPermission(Permission.GetRoles)]
        public async Task<IActionResult> GetAll([FromQuery] bool? IncludeDisabled, CancellationToken cancellationToken)
        {
            var roles = await _roleServices.GetRolesAsync(IncludeDisabled, cancellationToken);
            
            return Ok(roles);
        }
        [HttpGet("{roleId}")]
        [HasPermission(Permission.GetRoles)]
        public async Task<IActionResult> Get([FromRoute] string roleId)
        {
            var result = await _roleServices.GetRoleDetailAsync(roleId);

            return result.IsSucess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpPost("")]
        [HasPermission(Permission.AddRoles)]
        public async Task<IActionResult> AddRole([FromBody] RoleRequest request, CancellationToken cancellationToken)
        {
            var result = await _roleServices.AddRoleAsync(request, cancellationToken);
            return result.IsSucess? CreatedAtAction(nameof(Get), new {result.Value.Id}, result.Value) : result.ToProblem();
        }
        [HttpPut("{roleId}")]
        [HasPermission(Permission.UpdateRoles)]
        public async Task<IActionResult> UpdateRole([FromRoute] string roleId, [FromBody]  RoleRequest request, CancellationToken cancellationToken)
        {
            var result = await _roleServices.UpdateAsync(roleId, request, cancellationToken);
            return result.IsSucess? NoContent() : result.ToProblem();
        }
        [HttpPut("{roleId}/toggle")]
        [HasPermission(Permission.UpdateRoles)]
        public async Task<IActionResult> UpdateRole([FromRoute] string roleId)
        {
            var result = await _roleServices.ToggleDeltedAsync(roleId);
            return result.IsSucess ? NoContent() : result.ToProblem();
        }
    }
}
