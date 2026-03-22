using FluentValidation;

namespace SurveyBasket.Constract.Authentication
{
    public class EmailResetPasswordRequestValidator : AbstractValidator<EmailResetPasswordRequest>
    {
        public EmailResetPasswordRequestValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
