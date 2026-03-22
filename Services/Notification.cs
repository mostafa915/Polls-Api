
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.AppliacationsConfingrations;
using SurveyBasket.Helpers;
using SurveyBasket.Models;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace SurveyBasket.Services
{
    public class Notification
        (
        ApllicationDbContext apllicationDbContext,
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        IEmailSender emailSender
        ) : INotification
    {
        private readonly ApllicationDbContext _context = apllicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IEmailSender _emailSender = emailSender;

        public async Task SendNotficationByNewPollAsync(int? pollId = null)
        {
            IEnumerable<Poll> polls = [];
            if(pollId.HasValue)
            {
                var poll = _context.Polls.SingleOrDefault(p => p.Id == pollId);
                polls = [poll!];
            }
            else
            {
                polls = await _context.Polls
                    .Where(p => p.IsPublished && p.StartAt == DateTime.UtcNow)
                    .AsNoTracking()
                    .ToListAsync();
            }

            var users = await _userManager.Users.ToListAsync();
            foreach(var poll in polls)
            {
                foreach(var user in users)
                {
                    var Orign = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
                    var placeholder = new Dictionary<string, string>
                    {
                        {"{{name}}", user.FirstName },
                        {"{{pollTitle}}", poll.Title },
                        {"{{endDate}}", poll.EndAt.ToString() },
                        {"{{poll_url}}", $"{Orign}/polls/start/{poll.Id}" },
                    };
                    var body = EmailBodyBuilder.GenerateEmailBody("Notfication", placeholder);
                    await _emailSender.SendEmailAsync(user.Email!, $"Survey Basket: New Poll - {poll.Title}", body);

                }
            }
        }
    }
}
