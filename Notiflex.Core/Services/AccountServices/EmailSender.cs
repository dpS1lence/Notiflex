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
            message.From.Add(new MailboxAddress(_config["FromName"], _config["FromEmail"]));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);

                // Note: only needed if the SMTP server requires authentication
                await client.AuthenticateAsync(_config["FromEmail"], _config["AuthPassword"]);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
