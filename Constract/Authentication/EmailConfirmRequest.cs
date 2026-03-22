namespace SurveyBasket.Constract.Authentication
{
    public record EmailConfirmRequest(
        string UserId,
        string Code
        );
    
}
