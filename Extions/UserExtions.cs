using System.Security.Claims;

namespace SurveyBasket.Extions
{
    public static class UserExtions
    {
        public static string? GetUserId(this ClaimsPrincipal User) => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
