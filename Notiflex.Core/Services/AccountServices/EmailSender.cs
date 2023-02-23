using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Notiflex.Core.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Services.AccountServices
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public EmailSender(IConfiguration config)
        {
            _config= config;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            var email = _config.GetSection("EmailSender")["FromEmail"];
            var name = _config.GetSection("EmailSender")["FromName"];
            message.From.Add(new MailboxAddress(name, email));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };
#pragma warning disable IDE0063
            using var client = new SmtpClient();
#pragma warning restore IDE0063
            await client.ConnectAsync("smtp.gmail.com", 465, true);

            // Note: only needed if the SMTP server requires authentication
            await client.AuthenticateAsync(email, _config.GetSection("EmailSender")["AuthPassword"]);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
