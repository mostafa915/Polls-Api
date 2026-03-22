using FluentValidation;

namespace SurveyBasket.Constract.Authentication
{
    public class EmailConfirmRequestValidator : AbstractValidator<EmailConfirmRequest>
    {
        public EmailConfirmRequestValidator()
        {
            RuleFor(e => e.Code)
                .NotEmpty();
            
            RuleFor(e => e.UserId)
                .NotEmpty();
        }
    }
}
