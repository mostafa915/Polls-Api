using SurveyBasket.Constract.Answers;
using SurveyBasket.Models;

namespace SurveyBasket.Constract.Qustions
{
    public record QustionResponse(    
        int Id,
        string Content,
        IEnumerable<AnswerResponse> Answers
    );
    
    
}
