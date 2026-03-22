using FluentValidation;

namespace SurveyBasket.Constract.Poll
{
    public class PollRquestValidator : AbstractValidator<PollRequest>
    {
        public PollRquestValidator() {

            RuleFor(p => p.Title)
                .Length(3, 100)
                .NotEmpty();

            RuleFor(p => p.Summary)
                .Length(3, 1500)
                .NotEmpty();

            RuleFor(p => p.StartAt.Day)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Now.Day);

            RuleFor(p => p.EndAt.Day)
                .NotEmpty();

            RuleFor(p => p)
            .Must(HasValidDate)
            .WithName(nameof(PollRequest.EndAt))
            .WithMessage("{PropertyName} Must Be Greater Than Or Equal Start At");
        }
        private bool HasValidDate(PollRequest request) {
            return request.EndAt >= request.StartAt;
        }
    }
}
