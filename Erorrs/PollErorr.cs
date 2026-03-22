using SurveyBasket.Abstractions;

namespace SurveyBasket.Erorrs
{
    public static class PollErorr
    {
        public static readonly Erorr NotFound = new Erorr("Poll.NotFound", "Poll With This Id Is Not Found, Please Insert Correct Id", StatusCodes.Status404NotFound);
        public static readonly Erorr DuplicatedTitle = new Erorr("Poll.DuplicatedTitle", "There is Same Title In Athor Poll", StatusCodes.Status409Conflict);

    }
}
