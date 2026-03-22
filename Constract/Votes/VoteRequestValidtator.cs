using FluentValidation;

namespace SurveyBasket.Constract.Votes
{
    public class VoteRequestValidtator : AbstractValidator<VoteRequest>
    {
        public VoteRequestValidtator()
        {
            RuleFor(v => v.Answers)
                .NotEmpty();

            RuleForEach(v => v.Answers).SetInheritanceValidator(v => v.Add(new VoteAnswerRequestValidator()));
                
        }
    }
}
