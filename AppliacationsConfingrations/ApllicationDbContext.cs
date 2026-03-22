using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SurveyBasket.Extions;
using SurveyBasket.Models;
using System.Reflection;
using System.Security.Claims;

namespace SurveyBasket.AppliacationsConfingrations
{
    public class ApllicationDbContext(DbContextOptions<ApllicationDbContext> options, IHttpContextAccessor httpContextAccessor)
        :  IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public DbSet<Poll> Polls { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answers> Answers { get; set; }
        public DbSet<Vote> votes { get; set; }
        public DbSet<VoteAnswer> voteAnswers { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var CascadeFKs = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())
                .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

            foreach (var fk in CascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var CurrentUserId = _httpContextAccessor.HttpContext?.User.GetUserId()!;
            var Entries = ChangeTracker.Entries<EuidtableEntity>();
            foreach (var EntriesEntry in Entries)
            {
                if (EntriesEntry.State == EntityState.Added)
                {
                    EntriesEntry.Property(x => x.CreatedById).CurrentValue = CurrentUserId;
                }
                else if (EntriesEntry.State == EntityState.Modified)
                {
                    EntriesEntry.Property(x => x.UpdatedById).CurrentValue = CurrentUserId;
                    EntriesEntry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);

        }

    }
}
