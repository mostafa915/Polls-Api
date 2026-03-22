using Microsoft.EntityFrameworkCore;

namespace SurveyBasket.Models
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpireOn { get; set; }
        public DateTime CreationOn { get; set; } = DateTime.UtcNow;
        public DateTime? RevokeOn { get; set; }
        public bool IsExpired  => DateTime.UtcNow >= ExpireOn;
        public bool IsActive => !IsExpired && RevokeOn is null;

    }
}
