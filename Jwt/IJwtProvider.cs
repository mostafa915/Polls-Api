using Microsoft.Extensions.Options;
using SurveyBasket.Constract.Authentication;
using SurveyBasket.Models;

namespace SurveyBasket.Jwt
{
    public interface IJwtProvider
    {
        (string token, int expireIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions);
        string? ValidateToken(string token);
    }
}
