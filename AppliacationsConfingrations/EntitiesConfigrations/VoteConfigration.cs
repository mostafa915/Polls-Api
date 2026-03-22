using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.Models;

namespace SurveyBasket.AppliacationsConfingrations.EntitiesConfigrations
{
    public class VoteConfigration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.HasIndex(v => new { v.PollId, v.UserId }).IsUnique();
        }
    }
}
