using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Api.Config;
using Api.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;

namespace Api.Services
{
    public class EmailService : IEmailService
    {

        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> mailSettings)
        {
            _emailSettings = mailSettings.Value;
        }

        public async Task<bool> SendAsync(string toEmail, string subject, string body)
        {

            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Stockify", _emailSettings.From));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.SslOnConnect);
                await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }


        }



    }
}