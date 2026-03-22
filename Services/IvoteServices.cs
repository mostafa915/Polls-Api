using SurveyBasket.Abstractions;
using SurveyBasket.Constract.Votes;

namespace SurveyBasket.Services
{
    public interface IvoteServices
    {
        Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken = default);
    }
}
