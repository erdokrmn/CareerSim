using System.Net;
using System.Net.Mail;
using CareerSim.Services.IServices;
using Microsoft.Extensions.Configuration;

namespace CareerSim.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public SmtpEmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var fromEmail = _config["EmailSettings:From"];
            var password = _config["EmailSettings:Password"];
            var smtpHost = _config["EmailSettings:Smtp"];
            var smtpPort = int.Parse(_config["EmailSettings:Port"]);

            var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true
            };

            var message = new MailMessage(fromEmail, toEmail, subject, body);
            message.IsBodyHtml = true;

            await client.SendMailAsync(message);
        }
    }
}
