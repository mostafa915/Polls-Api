namespace SurveyBasket.Constract.Votes
{
    public record VoteRequest(
        IEnumerable<VoteAnswerRequest> Answers
        );
    
   
}
