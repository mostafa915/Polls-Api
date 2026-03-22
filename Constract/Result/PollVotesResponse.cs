namespace SurveyBasket.Constract.Result
{
    public record PollVotesResponse(
        string title,
        IEnumerable<VoteResponse> Votes
        );
    
}
