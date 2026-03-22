using FluentValidation;
using SurveyBasket.Abstractions;

namespace SurveyBasket.Constract.User
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator() {
            RuleFor(p => p.OldPassword)
                .NotEmpty();

            RuleFor(p => p.NewPassword)
                .Matches(RegexConfirm.RegexPassword)
                .WithMessage("Password must be at least has 8 characters and contains Digit, Lowercase, Uppercase, UniqueChars, NonAlphanumeric")
                .NotEqual(p => p.OldPassword)
                .WithMessage("New Password Must Be Not Equal To Old Password");
                
        }
    }
}
