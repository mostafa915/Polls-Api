namespace SurveyBasket.Constract.User
{
    public record UserResponse(
        string Id,
        string FirstName,
        string LastName,
        string Email,
        bool IsDiabled,
        IEnumerable<string> Roles
        );
    
}
