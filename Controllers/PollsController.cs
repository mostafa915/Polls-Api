using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.OutputCaching;
using SurveyBasket.Constract.Basic;
using SurveyBasket.Constract.Poll;
using SurveyBasket.Erorrs;
using SurveyBasket.Extions;
using SurveyBasket.Jwt.Autehntication.Filiters;
using SurveyBasket.Models;
using SurveyBasket.Services;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SurveyBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class PollsController: ControllerBase
    {
        private readonly IService service;
        public PollsController(IService service)
        {
            this.service = service; 
        }

        [HttpGet("")]
        [HasPermission(Permission.GetPolls)]

        public async Task <IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await service.GetAllAsync(cancellationToken));
        }
        
        [HttpGet("Current")]
        [HasPermission(Permission.GetPolls)]
        public async Task<IActionResult> GetAllCurrent(CancellationToken cancellationToken)
        {
            return Ok(await service.GetCurrentAllAsync(cancellationToken));
        }

        [HttpGet("{id}")]
        [HasPermission(Permission.GetPolls)]

        public async Task <IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var result = await service.GetByIdAsync(id, cancellationToken);
            return result.IsSucess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("")]
        [HasPermission(Permission.AddPolls)]

        public async Task <IActionResult> Create([FromBody] PollRequest poll, CancellationToken cancellationToken)
        {
            var result = await service.AddAsync(poll, cancellationToken);
            return result.IsSucess ? NoContent() : result.ToProblem();
        }


        [HttpPut("{id}")]
        [HasPermission(Permission.UpdatePolls)]

        public async Task <IActionResult> update([FromRoute] int id, [FromBody] PollRequest Poll, CancellationToken cancellationToken )
        {
            
            var result = await service.UpdateAsync(id, Poll, cancellationToken);
            return result.IsSucess ? NoContent() : result.ToProblem();

        }


        [HttpDelete("{id}")]
        [HasPermission(Permission.DeletePolls)]

        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken )
        {
            var result = await service.DeleteAsync(id,cancellationToken);
            return result.IsSucess ? NoContent() : result.ToProblem();

        }


        [HttpPut("{id}/togglePublish")]
        [HasPermission(Permission.UpdatePolls)]

        public async Task<IActionResult> TogglePublish([FromRoute] int id , CancellationToken cancellationToken)
        {
            var result = await service.TooglePublishAsync(id,cancellationToken);
            return result.IsSucess ? NoContent() : result.ToProblem();

        }
    }

}
