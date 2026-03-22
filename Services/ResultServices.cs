using Microsoft.EntityFrameworkCore;
using SurveyBasket.Abstractions;
using SurveyBasket.AppliacationsConfingrations;
using SurveyBasket.Constract.Result;
using SurveyBasket.Erorrs;
using SurveyBasket.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SurveyBasket.Services
{
    public class ResultServices(ApllicationDbContext context) : IResultServices
    {
        private readonly ApllicationDbContext _context = context;

        public async Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken = default)
        {
            var pollVotes = await _context.Polls
                .Where(p => p.Id == pollId)
                .Select(p => new PollVotesResponse(
                     p.Title,
                     p.Votes.Select(v => new VoteResponse (
                         v.User.FirstName + " " + v.User.LastName,
                         v.SubmittedOn,
                        v.voteAnswers.Select(va => new QuestionAnswerResponse(
                            va.Question.Content,
                            va.Answers.Content
                            ))
                         )) 
                    ))
                .SingleOrDefaultAsync(cancellationToken);
           
            return pollVotes is null ? Result.Faliuer<PollVotesResponse>(PollErorr.NotFound) : Result.Succuess(pollVotes);
        }

        public async Task<Result<IEnumerable<VotePerDaysResponse>>> GetVotesPerDaysAsync(int pollId, CancellationToken cancellationToken = default)
        {
            var pollIsExist = await _context.Polls.AnyAsync(p => p.Id == pollId);
            if (!pollIsExist)
                return Result.Faliuer<IEnumerable<VotePerDaysResponse>>(PollErorr.NotFound);
            var votes = await _context.votes
                .Where(v => v.PollId == pollId)
                .GroupBy(p => new {Date =  DateOnly.FromDateTime(p.SubmittedOn)})
                .Select(g => new VotePerDaysResponse(
                    g.Key.Date,
                    g.Count()
                    ))
                .ToListAsync(cancellationToken);
            return Result.Succuess<IEnumerable<VotePerDaysResponse>>(votes);
        }
        public async Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionsAsync(int pollId, CancellationToken cancellationToken = default)
        {
            var pollIsExist = await _context.Polls.AnyAsync(p => p.Id == pollId);
            if (!pollIsExist)
                return Result.Faliuer<IEnumerable<VotesPerQuestionResponse>>(PollErorr.NotFound);
            var VotesPerQuestion = await _context.voteAnswers
                .Where(x => x.vote.PollId == pollId)
                .Select(s => new VotesPerQuestionResponse(
                    s.Question.Content,
                    s.Question.voteAnswers
                    .GroupBy(p => new {AnswerId = p.Answers.Id , AnswerContent = p.Answers.Content })
                    .Select(s => new VotesPerAnswerResponse(
                        s.Key.AnswerContent,
                        s.Count()
                        ))
                    ))
                .ToListAsync();
            return Result.Succuess<IEnumerable<VotesPerQuestionResponse>>(VotesPerQuestion);
        }
    }
}
