using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.Constract.Basic;
using SurveyBasket.Models;

namespace SurveyBasket.AppliacationsConfingrations.EntitiesConfigrations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(p => p.FirstName)
                .HasMaxLength(100);
            
            builder.Property(p => p.LastName)
                .HasMaxLength(100);

            builder.OwnsMany(u => u.RefreshTokens)
                .ToTable("RefreshTokens")
                .WithOwner()
                .HasForeignKey("UserId");

            var PassHasher = new PasswordHasher<ApplicationUser>();
            builder.HasData(new ApplicationUser
            {
                Id = DefaultUser.AdminId,
                FirstName = DefaultUser.AdminFirstName,
                LastName = DefaultUser.AdminLastName,
                Email = DefaultUser.AdminEmail,
                UserName = DefaultUser.AdminUserName,
                ConcurrencyStamp = DefaultUser.AdminConcurrencyStamp,
                SecurityStamp = DefaultUser.AdminSecurityStamp,
                PasswordHash = PassHasher.HashPassword(null!, DefaultUser.AdminPassword),
                EmailConfirmed = true,
                NormalizedUserName = DefaultUser.AdminUserName.ToUpper(),
                NormalizedEmail = DefaultUser.AdminEmail.ToUpper(),

            });
        }
    }
}
