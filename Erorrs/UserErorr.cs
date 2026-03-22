using SurveyBasket.Abstractions;

namespace SurveyBasket.Erorrs
{
    public static class UserErorr
    {
        public readonly static Erorr InvalidCredintails = new Erorr("User.InvalidCredintails", "Invalid Email Or PassWord", StatusCodes.Status401Unauthorized);
        public readonly static Erorr InvalidId = new Erorr("User.InvalidId", "Invalid Id, Please Check of Inserted Id", StatusCodes.Status404NotFound);
        public readonly static Erorr HaveAlreadyToken = new Erorr("User.HaveAlreadyToken", "You Have Already Token Valid", StatusCodes.Status409Conflict);
        public readonly static Erorr DupilcatedEmail = new Erorr("User.DupilcatedEmail", "This Email Is Duplicated", StatusCodes.Status409Conflict);
        public readonly static Erorr EmailNotConfirmed = new Erorr("User.EmailNotConfirmed", "Email Not Confirmed", StatusCodes.Status401Unauthorized);
        public readonly static Erorr InvalidCode = new Erorr("User.InvalidCode", "Invalid Code, Please Check of Inserted Code", StatusCodes.Status401Unauthorized);
        public readonly static Erorr DupilcatedConfirmation = new Erorr("User.DupilcatedConfirmation ", "You Have Alearey Confirmed Email", StatusCodes.Status400BadRequest);
        public readonly static Erorr DisabledUser = new Erorr("User.DisabledUser", "Disabled User, Please Concact With Admin", StatusCodes.Status401Unauthorized);
        public readonly static Erorr MaxiamumTrySignIn = new Erorr("User.MaxiamumTrySignIn", "You Arriave Maxiamum Try, Please Try Again After 5 Minutes", StatusCodes.Status401Unauthorized);
        public static readonly Erorr NotFound = new Erorr("User.NotFound", "User With This Id Is Not Found, Please Insert Correct Id", StatusCodes.Status404NotFound);
        public readonly static Erorr InvalidRoles = new Erorr("User.InvalidRoles", "Invalid Roles, Please Check of Inserted Roles If It Exists", StatusCodes.Status401Unauthorized);


    }
}
