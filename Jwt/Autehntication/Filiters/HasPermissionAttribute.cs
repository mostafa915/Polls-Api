using Microsoft.AspNetCore.Authorization;

namespace SurveyBasket.Jwt.Autehntication.Filiters
{
    public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
    {
    }
}
