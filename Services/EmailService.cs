using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using TroyPortfolio.Models;

namespace TroyPortfolio.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(ContactFormModel model)
        {
            var email = new MimeMessage();

            // Sender
            email.From.Add(MailboxAddress.Parse(_settings.From));

            // Recipient (your Gmail)
            email.To.Add(MailboxAddress.Parse(_settings.From));

            // Subject
            email.Subject = model.Subject;

            // Replying goes to the sender
            email.ReplyTo.Add(MailboxAddress.Parse(model.Email));

            // Email body
            email.Body = new TextPart("html")
            {
                Text = $@"
                    <h2>New Portfolio Contact</h2>

                    <p><strong>Name:</strong> {model.Name}</p>

                    <p><strong>Email:</strong> {model.Email}</p>

                    <p><strong>Subject:</strong> {model.Subject}</p>

                    <hr>

                    <p>{model.Message}</p>"
            };

            using var smtp = new SmtpClient();



            await smtp.ConnectAsync(
                _settings.SmtpServer,
                _settings.Port,
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _settings.Username,
                _settings.Password);

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}