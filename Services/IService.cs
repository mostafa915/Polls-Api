using SurveyBasket.Abstractions;
using SurveyBasket.Constract.Poll;
using SurveyBasket.Models;

namespace SurveyBasket.Services
{
    public interface IService
    {
        Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<PollResponse>> GetCurrentAllAsync(CancellationToken cancellationToken = default);
        Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<PollResponse>> AddAsync(PollRequest request , CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int id ,PollRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<Result> TooglePublishAsync(int id, CancellationToken cancellationToken= default);
    }
}
