using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.Constract.Basic;

namespace SurveyBasket.AppliacationsConfingrations.EntitiesConfigrations
{
    public class UserRolesConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(new IdentityUserRole<string>
            {
                RoleId = DefaultRole.AdminRoleId,
                UserId = DefaultUser.AdminId,
            });
        }
    }
}
