using SurveyBasket.Models;
using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.Constract.Poll
{
    public record PollRequest (
        string Title,
        string Summary,
        DateTime StartAt,
        DateTime EndAt);
    
}
