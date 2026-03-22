using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.Models;

namespace SurveyBasket.AppliacationsConfingrations.EntitiesConfigrations
{
    public class AnswersConfigration : IEntityTypeConfiguration<Answers>
    {
        public void Configure(EntityTypeBuilder<Answers> builder)
        {
            builder.HasIndex(a => new { a.QuestionId, a.Content });
            builder.Property(a => a.Content).HasMaxLength(1000);
        }
    }
}
