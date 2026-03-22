using Mapster;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Abstractions;
using SurveyBasket.AppliacationsConfingrations;
using SurveyBasket.Constract.Answers;
using SurveyBasket.Constract.Pagination;
using SurveyBasket.Constract.Qustions;
using SurveyBasket.Erorrs;
using SurveyBasket.Models;
using System.Linq.Dynamic.Core;

namespace SurveyBasket.Services
{
    public class QuestionServices(ApllicationDbContext dbContext) : IQuestionServices
    {
        private readonly ApllicationDbContext _dbContext = dbContext;
        public async Task<Result<PaginaedList<QustionResponse>>> GetAllAysnc(int PollId, FiliterRequest request ,CancellationToken cancellationToken = default)
        {
            var pollIsExist = await _dbContext.Polls.AnyAsync(p => p.Id == PollId);
            if(!pollIsExist)
            {
                Result.Faliuer<IEnumerable<QustionResponse>>(PollErorr.NotFound);
            }

            var query = _dbContext.Questions
                .Where(q => q.PollId == PollId);
            if (!string.IsNullOrEmpty(request.SearchValue))
            {
                query = query.Where(q => q.Content.Contains(request.SearchValue));  
            }
            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                query = query.OrderBy($"{request.SortColumn} {request.SortDirection}"); 
            }
              var source =  query.Include(q => q.Answers)
                .ProjectToType<QustionResponse>()
                .AsNoTracking();

            var Questions = await PaginaedList<QustionResponse>.CreateAsync(source, request.PageNumber, request.PageSize, cancellationToken);

            return Result.Succuess(Questions);

        }

        public async Task<Result<QustionResponse>> GetQuestionAsync(int PollId, int QuestionId, CancellationToken cancellationToken = default)
        {
            var Question = await _dbContext.Questions
                .Where(q => q.Id == QuestionId && PollId == q.PollId)
                .Include(q => q.Answers)
                .ProjectToType<QustionResponse>()
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken);
            if(Question is null)
            {
                return Result.Faliuer<QustionResponse>(QuestionErorr.NotFound);
            }
            return Result.Succuess(Question);
        }

        public async Task<Result<QustionResponse>> AddAsync(int pollId, QustionRequest request, CancellationToken cancellationToken = default)
        {
            var PollIsExist = await _dbContext.Polls.AnyAsync(p => p.Id == pollId);
            if(!PollIsExist)
            {
                return Result.Faliuer<QustionResponse>(PollErorr.NotFound);
            }
            var ContentIsDupilcated = await _dbContext.Questions.AnyAsync(q => q.Content == request.Content && q.PollId == pollId);
            if(ContentIsDupilcated)
            {
                return Result.Faliuer<QustionResponse>(QuestionErorr.DuplicatedContent);
            }
            var Question = request.Adapt<Question>();
            Question.PollId = pollId;
            request.Answers.ForEach(answer => Question.Answers.Add(new Answers { Content = answer }));
            await _dbContext.AddAsync(Question,cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Succuess(Question.Adapt<QustionResponse>());

        }

        public async Task<Result> UpdateAsync(int PollId, int Id, QustionRequest request, CancellationToken cancellationToken = default)
        {
            var isDupilicateContentQuestion = await _dbContext.Questions.AnyAsync(
                q => q.Content == request.Content
                && q.Id != Id
                && q.PollId == PollId
                , cancellationToken);
            if(isDupilicateContentQuestion)
            {
                return Result.Faliuer(QuestionErorr.DuplicatedContent);
            }
            var Question = await _dbContext.Questions.Include(q => q.Answers).SingleOrDefaultAsync(
                q => q.Id == Id
                && q.PollId == PollId, 
                cancellationToken);
            if(Question is null)
            {
                return Result.Faliuer(QuestionErorr.NotFound);
            }
            Question.Content = request.Content;
            var currentAnswers = Question.Answers.Select(a => a.Content).ToList();
            var newAnswers = request.Answers.Except(currentAnswers).ToList();
            newAnswers.ForEach(a =>
            {
                Question.Answers.Add(new Answers { Content = a });
            });

            Question.Answers.ToList().ForEach(a =>
            {
                a.IsActive = request.Answers.Contains(a.Content);
            });

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Succuess();
        }

        public async Task<Result> ToggleActiveAsync(int PollId, int Id, CancellationToken cancellationToken = default)
        {
            var question = await _dbContext.Questions.SingleOrDefaultAsync(q => q.Id == Id && q.PollId == PollId, cancellationToken);
            if(question is null)
            {
                return Result.Faliuer(QuestionErorr.NotFound);
            }
            question.IsActive = !question.IsActive;
            await _dbContext.SaveChangesAsync();
            return Result.Succuess();
        }

        public async Task<Result<IEnumerable<QustionResponse>>> GetAvailableAsync(int pollId, string userId, CancellationToken cancellationToken = default)
        {
            var HasVoted = await _dbContext.votes.AnyAsync(v => v.UserId == userId && v.PollId == pollId);
            if(HasVoted)
            {
                return Result.Faliuer<IEnumerable<QustionResponse>>(VoteErorrs.DuplicatedVote);
            }
            var PollIsExist = await _dbContext.Polls.AnyAsync(p => p.Id == pollId && p.IsPublished && p.StartAt <= DateTime.UtcNow && p.EndAt >= DateTime.UtcNow);
            if(!PollIsExist)
            {
                return Result.Faliuer<IEnumerable<QustionResponse>>(PollErorr.NotFound);
            }
            var questions = await _dbContext.Questions
                .Where(q => q.PollId == pollId && q.IsActive)
                .Select(q => new QustionResponse(
                    q.Id,
                    q.Content,
                    q.Answers.Where(a => a.IsActive).Select(a => new AnswerResponse(a.Id, a.Content))
                    ))
                .AsNoTracking()
                .ToListAsync();
            return Result.Succuess<IEnumerable<QustionResponse>>(questions);
        }
    }
}
