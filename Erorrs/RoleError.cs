using SurveyBasket.Abstractions;

namespace SurveyBasket.Erorrs
{
    public static class RoleError
    {
        public static readonly Erorr NotFound = new Erorr("Role.NotFound", "Role With This Id Is Not Found, Please Insert Correct Id", StatusCodes.Status404NotFound);
        public readonly static Erorr DupilcatedRole = new Erorr("Role.DupilcatedRole", "This Role Is Duplicated", StatusCodes.Status409Conflict);
        public readonly static Erorr InvalidPermissions = new Erorr("User.InvalidPermissions", "Invalid Permissions, Please Check of Inserted Permissions Is Exist", StatusCodes.Status400BadRequest);

    }
}
