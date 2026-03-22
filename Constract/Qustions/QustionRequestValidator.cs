using FluentValidation;

namespace SurveyBasket.Constract.Qustions
{
    public class QustionRequestValidator : AbstractValidator<QustionRequest>
    {
        public QustionRequestValidator()
        {
            RuleFor(q => q.Content)
                .NotEmpty()
                .Length(3, 1000);

            RuleFor(q => q.Answers)
                .NotNull();

            RuleFor(q => q.Answers)
                .Must(q => q.Count > 1)
                .WithMessage("Answers Should Be At Least 2")
                .When(q => q.Answers != null);

            RuleFor(q => q.Answers)
                .Must(q => q.Distinct().Count() == q.Count)
                .WithMessage("Must Not Dupilcate Answer")
                .When(q => q.Answers != null);
        }
    }
}
