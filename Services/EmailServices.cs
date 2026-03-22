using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using SurveyBasket.MailSetting;

namespace SurveyBasket.Services
{
    public class EmailServices(IOptions<MailConfirmation> options, ILogger<EmailServices> logger) : IEmailSender
    {
        private readonly MailConfirmation _mailConfirmation = options.Value;
        private readonly ILogger<EmailServices> _logger = logger;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var Message = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailConfirmation.Mail),
                Subject = subject
            };

            Message.To.Add(MailboxAddress.Parse(email));

            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            Message.Body = builder.ToMessageBody();

            using var Smtp = new SmtpClient();
            _logger.LogInformation("Sending Email To {email}", email);
            Smtp.Connect(_mailConfirmation.host, _mailConfirmation.port, SecureSocketOptions.StartTls);
            Smtp.Authenticate(_mailConfirmation.Mail, _mailConfirmation.PassWord);
            await Smtp.SendAsync(Message);
            Smtp.Disconnect(true);
        }
    }
}
