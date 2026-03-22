using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Constract.Basic;
using SurveyBasket.Extions;
using SurveyBasket.Jwt.Autehntication.Filiters;
using SurveyBasket.Services;

namespace SurveyBasket.Controllers
{
    [Route("api/polls/{pollId}/[controller]")]
    [ApiController]
    [HasPermission(Permission.Results)]
    public class ResultsController(IResultServices resultServices) : ControllerBase
    {
        private readonly IResultServices _resultServices = resultServices;

        [HttpGet("raw-data")]
        public async Task<IActionResult> PollVotes([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var result = await _resultServices.GetPollVotesAsync(pollId, cancellationToken);
            return result.IsSucess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("votes-per-day")]
        public async Task<IActionResult> VotesPerDay([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var result = await _resultServices.GetVotesPerDaysAsync(pollId, cancellationToken);
            return result.IsSucess? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("votes-per-question")]
        public async Task<IActionResult> VotesPerQuestion([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var result = await _resultServices.GetVotesPerQuestionsAsync(pollId, cancellationToken);
            return result.IsSucess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
