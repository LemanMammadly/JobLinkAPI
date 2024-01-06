using System;
using JobLink.Business.ExternalServices.Interfaces;
using JobLink.Core.Entities;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace JobLink.Business.ExternalServices.Implements
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailSenderService(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public string GetEmailConfirmationTemplate(string templatePath)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", templatePath);
            return File.ReadAllText(path);
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, SecureSocketOptions.StartTls);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfiguration.UserName, _emailConfiguration.Password);

                client.Send(mailMessage);
            }
            catch
            {
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}

