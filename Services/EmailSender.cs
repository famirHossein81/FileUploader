using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using FileUploader.Models;
namespace FileUploader.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailSender(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            using (var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
            {
                client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                    Subject = mailRequest.Subject,
                    Body = mailRequest.Body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(mailRequest.ToEmail);
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}