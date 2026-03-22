using FluentValidation;
using System.Text.RegularExpressions;

namespace SurveyBasket.Constract.Authentication
{
    public class AuthenRequestValidation : AbstractValidator<AuthenRequest>
    {
        public AuthenRequestValidation() {
            RuleFor(a => a.email)
                .NotEmpty()
                .EmailAddress();
            
            RuleFor(a => a.password)
                .NotEmpty();
        
        }
    }
}
