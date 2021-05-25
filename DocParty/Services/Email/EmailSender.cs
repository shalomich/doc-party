using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Services.Email
{
    class EmailSender
    {
        private const string FeedbackConfigSectionName = "Feedback";
        private readonly string Email;

        public EmailSender(IConfiguration configuration)
        {
            Email = configuration.GetSection(FeedbackConfigSectionName)[nameof(Email)];
        }

        public async Task SendEmailAsync(string recipientEmail, EmailMessage message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("DocParty", Email));
            emailMessage.To.Add(new MailboxAddress("", recipientEmail));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = message.Text
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.yandex.ru", 25, false);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
