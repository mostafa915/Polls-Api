using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Constract.Basic;
using SurveyBasket.Constract.Pagination;
using SurveyBasket.Constract.Qustions;
using SurveyBasket.Erorrs;
using SurveyBasket.Extions;
using SurveyBasket.Jwt.Autehntication.Filiters;
using SurveyBasket.Models;
using SurveyBasket.Services;
using System.Threading.Tasks;

namespace SurveyBasket.Controllers
{
    [Route("api/Polls/{pollId}/[controller]")]
    [ApiController]
    public class QuestionController(IQuestionServices questionServices) : ControllerBase
    {
        private readonly IQuestionServices _questionServices = questionServices;
        
        
        [HttpGet("")]
        [HasPermission(Permission.GetQuestions)]

        public async Task<IActionResult> GetAll([FromRoute] int pollId, [FromQuery] FiliterRequest filiters,  CancellationToken cancellationToken)
        {
            var result = await _questionServices.GetAllAysnc(pollId, filiters, cancellationToken);
            return result.IsSucess ? Ok(result.Value) : result.ToProblem();

        }

        [HttpGet("{id}")]
        [HasPermission(Permission.GetQuestions)]

        public async Task<IActionResult> Get([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _questionServices.GetQuestionAsync(pollId, id, cancellationToken);
            return result.IsSucess ? Ok(result.Value) : result.ToProblem();


        }


        [HttpPost("")]
        [HasPermission(Permission.AddQuestions)]


        public async Task<IActionResult> Create([FromRoute] int pollId, [FromBody] QustionRequest request, CancellationToken cancellationToken)
        {
            var result = await _questionServices.AddAsync(pollId, request, cancellationToken);
            return result.IsSucess ? NoContent() : result.ToProblem();


        }
        [HttpPut("{id}")]
        [HasPermission(Permission.UpdateQuestions)]
        public async Task<IActionResult> Update([FromRoute] int pollId, [FromRoute] int id, [FromBody] QustionRequest request, CancellationToken cancellationToken)
        {
            var result = await _questionServices.UpdateAsync(pollId, id, request, cancellationToken);
            return result.IsSucess ? NoContent() : result.ToProblem();


        }
        
        [HttpPut("{id}/toggleActive")]
        [HasPermission(Permission.UpdateQuestions)]
        public async Task<IActionResult> ChangeActive([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _questionServices.ToggleActiveAsync(pollId, id, cancellationToken);
            return result.IsSucess ? NoContent() : result.ToProblem();

        }
    }
}
