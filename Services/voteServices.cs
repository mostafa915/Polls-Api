using Mapster;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Abstractions;
using SurveyBasket.AppliacationsConfingrations;
using SurveyBasket.Constract.Votes;
using SurveyBasket.Erorrs;
using SurveyBasket.Models;

namespace SurveyBasket.Services
{
    public class voteServices(ApllicationDbContext dbContext) : IvoteServices
    {
        private readonly ApllicationDbContext _dbContext = dbContext;

        public async Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken = default)
        {
            var hasVote = await _dbContext.votes.AnyAsync(v => v.UserId == userId && v.PollId == pollId, cancellationToken);
            if (hasVote)
            {
                return Result.Faliuer(VoteErorrs.DuplicatedVote);
            }
            var pollIsExist = await _dbContext.Polls.AnyAsync(p => p.Id == pollId && p.IsPublished && p.StartAt <= DateTime.UtcNow && p.EndAt >= DateTime.UtcNow) ;
            if (!pollIsExist)
            {
                return Result.Faliuer(PollErorr.NotFound);
            }
            var avaliableQuestion = await _dbContext.Questions
                .Where(q => q.PollId == pollId && q.IsActive)
                .Select(q => q.Id)
                .ToListAsync(cancellationToken);
            if(!request.Answers.Select(a => a.QuestionId).SequenceEqual(avaliableQuestion))
            {
                return Result.Faliuer(QuestionErorr.InvalidQuestion);
            }
            var vote = new Vote
            {
                PollId = pollId,
                UserId = userId,
                voteAnswers = request.Answers.Adapt<IEnumerable<VoteAnswer>>().ToList()
            };
            await _dbContext.AddAsync(vote, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Succuess();
        }
    }
}
