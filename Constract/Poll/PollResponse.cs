namespace SurveyBasket.Constract.Poll
{
    public record PollResponse(
        int Id,
        string Title,
        string Summary,
        bool IsPublished,
        DateTime StartAt,
        DateTime EndAt
        );
    
    
}
