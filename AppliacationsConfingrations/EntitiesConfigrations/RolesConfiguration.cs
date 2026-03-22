using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.Constract.Basic;
using SurveyBasket.Models;

namespace SurveyBasket.AppliacationsConfingrations.EntitiesConfigrations
{
    public class RolesConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData([
                new ApplicationRole{
                    Id = DefaultRole.AdminRoleId,
                    Name = DefaultRole.Admin,
                    ConcurrencyStamp = DefaultRole.AdminRoleConcurrencyStamp,
                    NormalizedName = DefaultRole.Admin.ToUpper()
                },
                new ApplicationRole{
                    Id = DefaultRole.MemberRoleId,
                    Name = DefaultRole.Member,
                    ConcurrencyStamp= DefaultRole.MemberRoleConcurrencyStamp,
                    NormalizedName = DefaultRole.Member.ToUpper(),
                    IsDefault = true
                }
                ]);
        }
    }
}
