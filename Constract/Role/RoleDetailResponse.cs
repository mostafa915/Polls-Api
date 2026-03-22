namespace SurveyBasket.Constract.Role
{
    public record RoleDetailResponse(
        string Name,
        string Id,
        bool IsDeleted,
        IEnumerable<string> Permissions
        );
    
}
