namespace SurveyBasket.Constract.Basic
{
    public static class Permission
    {
        public static string Type { get; } = "permissions";

        public const string GetPolls = "polls:read";
        public const string AddPolls = "polls:Add";
        public const string UpdatePolls = "polls:update";
        public const string DeletePolls = "polls:delete";

        public const string GetQuestions = "questions:read";
        public const string AddQuestions = "questions:Add";
        public const string UpdateQuestions = "questions:update";

        public const string GetUsers = "users:read";
        public const string AddUsers = "users:Add";
        public const string UpdateUsers = "users:update";

        public const string GetRoles = "roles:read";
        public const string AddRoles = "roles:Add";
        public const string UpdateRoles = "roles:update";

        public const string Results = "results.read";
        public static IList<string?> GetAllPermissions() => 
            typeof(Permission).GetFields().Select(f => f.GetValue(f) as string).ToList();

    }
}
