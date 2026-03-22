using Hangfire;
using Mapster;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using SurveyBasket.Abstractions;
using SurveyBasket.AppliacationsConfingrations;
using SurveyBasket.Constract.Poll;
using SurveyBasket.Erorrs;
using SurveyBasket.Models;
using System.Threading;

namespace SurveyBasket.Services
{
    public class Service(ApllicationDbContext context, HybridCache hybridCache, INotification notification) : IService
    {
        private readonly ApllicationDbContext _context = context;
        private readonly HybridCache _hybridCache = hybridCache;
        private readonly INotification _notification = notification;
        private const string _cachePrefix = "poll";
        public async Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var pollsResponses = await _context.Polls
                .AsNoTracking()
                .ProjectToType<PollResponse>()
                .ToListAsync(cancellationToken);
                
            return pollsResponses;
        }

        public async Task<IEnumerable<PollResponse>> GetCurrentAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Polls
                .Where(p => p.IsPublished && p.StartAt <= DateTime.UtcNow && p.EndAt >= DateTime.UtcNow)
                .AsNoTracking()
                .ProjectToType<PollResponse>()
                .ToListAsync();
        }

        public async Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            //var poll = await _context.Polls.FindAsync(id, cancellationToken);
            var key = $"{_cachePrefix}-{id}";
            var poll = await _hybridCache.GetOrCreateAsync<Poll>(
                key,
                async cacheEntry => await _context.Polls.FindAsync(id, cancellationToken)
                );
            return poll is null ? 
                Result.Faliuer<PollResponse>(PollErorr.NotFound) : 
                Result.Succuess(poll.Adapt<PollResponse>());
            
        }
        

        public async Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellationToken = default)
        {
            var IsTitleExistBefore = await _context.Polls.AnyAsync( p => p.Title == request.Title, cancellationToken);
            if(IsTitleExistBefore)
            {
                return Result.Faliuer<PollResponse>(PollErorr.DuplicatedTitle);
            }
            var poll = request.Adapt<Poll>();
            await _context.Polls.AddAsync(poll ,cancellationToken );
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Succuess(poll.Adapt<PollResponse>());
        }



        public async Task<Result> UpdateAsync(int id, PollRequest request, CancellationToken cancellationToken = default)
        {
            var CurrentPoll = await _context.Polls.FindAsync(id, cancellationToken);
            if (CurrentPoll is null)
            {
                return Result.Faliuer(PollErorr.NotFound);
            }

            var IsTitleExistBefore = await _context.Polls.AnyAsync(p => p.Title == request.Title && p.Id != id, cancellationToken);
            if (IsTitleExistBefore)
            {
                return Result.Faliuer<PollResponse>(PollErorr.DuplicatedTitle);
            }

            CurrentPoll.Title = request.Title;
            CurrentPoll.Summary = request.Summary;
            CurrentPoll.StartAt = request.StartAt;
            CurrentPoll.EndAt = request.EndAt;
            await _context.SaveChangesAsync();
            await _hybridCache.RemoveAsync($"{_cachePrefix}-{id}",cancellationToken);

            return Result.Succuess();
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var poll = await _context.Polls.FindAsync(id, cancellationToken);
            if(poll is null)
            {
                return Result.Faliuer(PollErorr.NotFound);
            }
            _context.Remove(poll);
           await _context.SaveChangesAsync(cancellationToken);
            await _hybridCache.RemoveAsync($"{_cachePrefix}-{id}", cancellationToken);

            return Result.Succuess();
        }



        public async Task<Result> TooglePublishAsync(int id, CancellationToken cancellationToken = default)
        {
            var poll = await _context.Polls.FindAsync(id , cancellationToken);
                if( poll is null) {
                return Result.Faliuer(PollErorr.NotFound); 
            }
               poll.IsPublished =  !poll.IsPublished;
            await _context.SaveChangesAsync(cancellationToken);
            await _hybridCache.RemoveAsync($"{_cachePrefix}-{id}", cancellationToken);
            if(poll.IsPublished && poll.StartAt ==  DateTime.UtcNow)
            {
                BackgroundJob.Enqueue(() => _notification.SendNotficationByNewPollAsync(poll.Id));
            }
            return Result.Succuess();
        }

        
    }
}
