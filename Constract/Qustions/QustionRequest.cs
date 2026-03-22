namespace SurveyBasket.Constract.Qustions
{
    public record QustionRequest(
        string Content,
        List<string> Answers
        );
    
}
