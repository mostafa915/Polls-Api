using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.Constract.Basic;

namespace SurveyBasket.AppliacationsConfingrations.EntitiesConfigrations
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
        {
            var Permissions = Permission.GetAllPermissions();
            var AdminClaims = new List<IdentityRoleClaim<string>>();
            for(int i = 0; i < Permissions.Count; i++)
            {
                AdminClaims.Add(new IdentityRoleClaim<string>
                {
                    Id = i + 1,
                    ClaimType = Permission.Type,
                    ClaimValue = Permissions[i],
                    RoleId = DefaultRole.AdminRoleId
                });
            }
            builder.HasData(AdminClaims);
        }
    }
}
