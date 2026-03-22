using SurveyBasket.Abstractions;

namespace SurveyBasket.Erorrs
{
    public static class VoteErorrs
    {
        public static readonly Erorr NotFound = new Erorr("Vote.NotFound", "Vote With This Id Is Not Found, Please Insert Correct Id", StatusCodes.Status404NotFound);
        public static readonly Erorr DuplicatedVote = new Erorr("Vote.DuplicatedTitle", "You Already Voted", StatusCodes.Status409Conflict);

    }
}
