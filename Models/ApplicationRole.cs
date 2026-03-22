using Microsoft.AspNetCore.Identity;

namespace SurveyBasket.Models
{
    public class ApplicationRole : IdentityRole
    {
        public bool IsDeleted { get; set; }
        public bool IsDefault { get; set; }
    }
}
