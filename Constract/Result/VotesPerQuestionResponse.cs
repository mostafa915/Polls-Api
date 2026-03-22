namespace SurveyBasket.Constract.Result
{
    public record VotesPerQuestionResponse(
        string Question,
        IEnumerable<VotesPerAnswerResponse> SelectedAnswers 
        );
    
}
