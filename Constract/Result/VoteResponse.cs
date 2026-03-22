namespace SurveyBasket.Constract.Result
{
    public record VoteResponse(
        string VoterName,
        DateTime VoteDate,
        IEnumerable<QuestionAnswerResponse> SelectedAnswers
        );
    
}
