using FluentValidation;
using SurveyBasket.Abstractions;

namespace SurveyBasket.Constract.User
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(c => c.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(c => c.FirstName)
                .NotEmpty()
                .Length(3, 100);

            RuleFor(c => c.LastName)
                .NotEmpty()
                .Length(3, 100);

            RuleFor(c => c.Password)
                .Matches(RegexConfirm.RegexPassword)
                .WithMessage("Password must be at least has 8 characters and contains Digit, Lowercase, Uppercase, UniqueChars, NonAlphanumeric");
                
            RuleFor(c => c.Roles)
                .NotNull()
                .NotEmpty();

            RuleFor(c => c.Roles)
                .Must(c => c.Distinct().Count() == c.Count)
                .WithMessage("Must Not Dupilcated Role")
                .When(c => c != null);
        }
    }
}
