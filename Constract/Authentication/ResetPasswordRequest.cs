namespace SurveyBasket.Constract.Authentication
{
    public record ResetPasswordRequest(
        string NewPassword,
        string Email,
        string Code
        );
    
}
