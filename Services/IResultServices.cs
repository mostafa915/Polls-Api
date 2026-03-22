using SurveyBasket.Abstractions;
using SurveyBasket.Constract.Result;

namespace SurveyBasket.Services
{
    public interface IResultServices
    {
        Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<VotePerDaysResponse>>> GetVotesPerDaysAsync(int pollId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionsAsync(int pollId, CancellationToken cancellationToken = default);


    }
}
