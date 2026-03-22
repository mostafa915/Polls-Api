namespace SurveyBasket.Abstractions
{
    public record Erorr(string code,string description, int? statusCode) {

        public static readonly Erorr None = new(string.Empty, string.Empty, null);
    }
    
}
