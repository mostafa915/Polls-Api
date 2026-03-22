using FluentValidation;

namespace SurveyBasket.Constract.Role
{
    public class RoleRequestValidator : AbstractValidator<RoleRequest>
    {
        public RoleRequestValidator() {
            RuleFor(r => r.Name)
                .NotEmpty()
                .Length(3, 200);

            RuleFor(r => r.Permissions)
                .NotNull()
                .NotEmpty();

            RuleFor(r => r.Permissions)
                .Must(r => r.Distinct().Count() == r.Count())
                .WithMessage("Must Not Dupilcated Permission!")
                .When(r => r.Permissions != null);
        }
    }
}
