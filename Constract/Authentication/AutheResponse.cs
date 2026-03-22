namespace SurveyBasket.Constract.Authentication
{
    public record AutheResponse(
        string id,
        string FirstName,
        string LastName,
        string? Email,
        string Token,
        int ExpireIn,
        string RefreshToken,
        DateTime RefreshTokenExpiretion
        );
    
}
