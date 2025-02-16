using System.Net;
using System.Net.Mail;

namespace FMISaliAPI.Services.EmailService
{
    public class EmailSender(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
        : IEmailSender
    {
        private readonly SmtpClient _client = new(smtpServer, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUsername, smtpPassword),
            EnableSsl = true
        };

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(smtpUsername);
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;

            mailMessage.To.Add(email);
            await _client.SendMailAsync(mailMessage);
        }
    }
}