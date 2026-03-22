namespace SurveyBasket.Abstractions
{
    public static class RegexConfirm
    {
        public const string RegexPassword = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$";
    }
}
