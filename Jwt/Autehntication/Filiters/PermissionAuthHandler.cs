using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Constract.Basic;

namespace SurveyBasket.Jwt.Autehntication.Filiters
{
    public class PermissionAuthHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var user = context.User.Identity;
            if (user is null || !user.IsAuthenticated)
                return;
           
            var hasPermission = context.User.Claims.Any(x => x.Value == requirement.Permission && x.Type == Permission.Type);
            if (!hasPermission)
                return;

            context.Succeed(requirement);
            return;
        }
    }
}
