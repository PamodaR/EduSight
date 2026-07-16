using ESCHOOLING.Shared;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Threading.Tasks;

namespace ECOMSYSTEM.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string toName, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_config["Smtp:FromName"], _config["Smtp:FromAddress"]));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var client = new SmtpClient();

            _logger.LogInformation("SendEmailAsync: connecting to {Host}:{Port} to send '{Subject}' to {ToEmail}", _config["Smtp:Host"], _config["Smtp:Port"], subject, toEmail);

            await client.ConnectAsync(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"]!), SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_config["Smtp:Username"], _config["Smtp:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
