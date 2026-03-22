using FluentValidation;

namespace SurveyBasket.Constract.Authentication
{
    public class ResendConfirmEmailRequestValidator : AbstractValidator<ResendConfirmEmailRequest>
    {
        public ResendConfirmEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
