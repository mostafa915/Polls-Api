using FluentValidation;
using SurveyBasket.Abstractions;

namespace SurveyBasket.Constract.Authentication
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator() {
            RuleFor(r => r.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(r => r.FirstName)
                .NotEmpty()
                .Length(3, 100);

            RuleFor(r => r.LastName)
                .NotEmpty()
                .Length(3, 100);

            RuleFor(r => r.Password)
                .NotEmpty()
                .Matches(RegexConfirm.RegexPassword)
                .WithMessage("Password must be at least has 8 characters and contains Digit, Lowercase, Uppercase, UniqueChars, NonAlphanumeric");
        }
    }
}
