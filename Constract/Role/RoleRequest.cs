namespace SurveyBasket.Constract.Role
{
    public record RoleRequest(
        string Name,
        IList<string> Permissions
        );
    
}
