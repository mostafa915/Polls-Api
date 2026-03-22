namespace SurveyBasket.Constract.User
{
    public record UserProfileResponse(
        string UserName,
        string FirstName,
        string LastName,
        string Email
        );
    
}
