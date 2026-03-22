using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using SurveyBasket.Constract.Basic;
using SurveyBasket.Constract.Votes;
using SurveyBasket.Erorrs;
using SurveyBasket.Extions;
using SurveyBasket.Services;
using System.Security.Claims;

namespace SurveyBasket.Controllers
{
    [Route("api/polls/{pollId}/vote")]
    [ApiController]
    [Authorize(Roles = DefaultRole.Member)]
    public class VotesController(IQuestionServices questionServices, IvoteServices ivoteServices) : ControllerBase
    {
        private readonly IQuestionServices _questionServices = questionServices;
        private readonly IvoteServices _ivoteServices = ivoteServices;

        [HttpGet("")]
        public async Task<IActionResult> start([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var result = await _questionServices.GetAvailableAsync(pollId, User.GetUserId()!,cancellationToken);
            return result.IsSucess ? Ok(result.Value) : result.ToProblem();

        }
        [HttpPost("")]
        public async Task<IActionResult> Vote([FromRoute] int pollId, [FromBody] VoteRequest request,CancellationToken cancellationToken)
        {
            var result = await ivoteServices.AddAsync(pollId, User.GetUserId()!, request, cancellationToken);
            return result.IsSucess ? Created() : result.ToProblem();


        }
    }
}
