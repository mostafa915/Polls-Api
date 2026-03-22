namespace SurveyBasket.Constract.User
{
    public record ChangePasswordRequest(
        string OldPassword,
        string NewPassword
        );
}
