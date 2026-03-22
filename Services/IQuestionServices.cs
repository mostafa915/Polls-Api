using SurveyBasket.Abstractions;
using SurveyBasket.Constract.Pagination;
using SurveyBasket.Constract.Poll;
using SurveyBasket.Constract.Qustions;

namespace SurveyBasket.Services
{
    public interface IQuestionServices
    {
        Task<Result<PaginaedList<QustionResponse>>> GetAllAysnc(int PollId, FiliterRequest request, CancellationToken cancellationToken = default);
        Task<Result<QustionResponse>> GetQuestionAsync(int PollId, int QuestionId, CancellationToken cancellationToken = default);
        Task<Result<QustionResponse>> AddAsync(int PollId ,QustionRequest request, CancellationToken cancellationToken = default);
        Task<Result> ToggleActiveAsync(int PollId, int Id, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int PollId, int Id, QustionRequest request, CancellationToken cancellationToken = default);
        public  Task<Result<IEnumerable<QustionResponse>>> GetAvailableAsync(int pollId, string userId, CancellationToken cancellationToken = default);
    }
}
