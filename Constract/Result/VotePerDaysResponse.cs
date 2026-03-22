namespace SurveyBasket.Constract.Result
{
    public record VotePerDaysResponse(
        DateOnly Date,
        int NumberOfVotes
        );
    
}
