using FluentValidation;
using SurveyBasket.Abstractions;

namespace SurveyBasket.Constract.Authentication
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress();
            
            RuleFor(p => p.NewPassword)
                .NotEmpty()
                .Matches(RegexConfirm.RegexPassword)
                .WithMessage("Password must be at least has 8 characters and contains Digit, Lowercase, Uppercase, UniqueChars, NonAlphanumeric");

            RuleFor(p => p.Code)
                .NotEmpty();
        }
    }
}
