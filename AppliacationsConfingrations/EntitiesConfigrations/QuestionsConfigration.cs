using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.Models;

namespace SurveyBasket.AppliacationsConfingrations.EntitiesConfigrations
{
    public class QuestionsConfigration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasIndex(q => new { q.Content, q.PollId });
            builder.Property(q => q.Content).HasMaxLength(1000);
        }
    }
}
