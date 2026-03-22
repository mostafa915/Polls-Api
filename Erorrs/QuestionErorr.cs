using SurveyBasket.Abstractions;

namespace SurveyBasket.Erorrs
{
    public class QuestionErorr
    {
        public static readonly Erorr NotFound = new Erorr("Question.NotFound", "Question With This Id Is Not Found, Please Insert Correct Id", StatusCodes.Status404NotFound);
        public static readonly Erorr DuplicatedContent = new Erorr("Question.DuplicatedContent", "There is Same Content Question In The Same Poll", StatusCodes.Status409Conflict);
        public static readonly Erorr InvalidQuestion = new Erorr("Question.InvalidQuestion", "Invalid Question, Please Insert Correct Id", StatusCodes.Status400BadRequest);

    }
}
