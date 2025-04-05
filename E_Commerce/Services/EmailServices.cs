using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Services
{
    public class EmailServices : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailServices> _logger;

        public EmailServices(IOptions<EmailSettings> emailSettings, ILogger<EmailServices> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                _logger.LogInformation($"Attempting to send email to {email} with subject: {subject}");
                
                var message = new MimeMessage()
                {
                    Sender = MailboxAddress.Parse(_emailSettings.Username),
                    Subject = subject
                };
                message.To.Add(MailboxAddress.Parse(email));
                
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlMessage
                };
                message.Body = bodyBuilder.ToMessageBody();

                using var smtp = new SmtpClient();
                
                _logger.LogInformation($"Connecting to SMTP server: {_emailSettings.Host}:{_emailSettings.Port}");
                await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                
                _logger.LogInformation("Authenticating with SMTP server");
                await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                
                _logger.LogInformation("Sending email");
                await smtp.SendAsync(message);
                
                _logger.LogInformation("Email sent successfully");
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {email}. Error: {ex.Message}");
                throw;
            }
        }
    }
}
