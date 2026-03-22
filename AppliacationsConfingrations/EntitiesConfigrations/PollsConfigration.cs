using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.Models;

namespace SurveyBasket.AppliacationsConfingrations.EntitiesConfigrations
{
    public class PollsConfigration : IEntityTypeConfiguration<Poll>
    {
        public void Configure(EntityTypeBuilder<Poll> builder)
        {
            builder.HasIndex(e => e.Title)
                .IsUnique();
            
            builder.Property(e => e.Title).
                HasColumnType("varchar")
                .HasMaxLength(100);
            
            builder.Property(e => e.Summary)
                .HasColumnType("varchar")
                .HasMaxLength(1500);

        }
    }
}
