using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MailKit.Net.Smtp;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.IdentityModel.Tokens;
using MimeKit;

namespace FLY.Business.Services.Implements
{
    public class EmailService : IEmailService
    {
        private readonly string? _mailboxEmail;
        private readonly string? _mailboxPassword;

        public EmailService()
        {
            _mailboxEmail = Environment.GetEnvironmentVariable("MAILBOX_EMAIL");
            _mailboxPassword = Environment.GetEnvironmentVariable("MAILBOX_PASSWORD");
        }

        public async Task SendVerificationEmailAsync(string email, string verificationCode)
        {
            try
            {
                string smtpServer = "smtp.gmail.com";
                int port = 465;

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("FLY", _mailboxEmail));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = "FLY: Email Verification Code";
                message.Body = new TextPart("plain")
                {
                    Text = $"Your verification code is: {verificationCode}.\nPlease enter this code to verify, it will be expired after 1 hour.\n\nBest regards\nFLY Team"
                };

                var gmailMessage = new Message
                {
                    Raw = Base64UrlEncoder.Encode(message.ToString())
                };

                using (var client = new SmtpClient())
                {
                    try
                    {
                        client.Connect(smtpServer, port, true);
                        client.Authenticate(_mailboxEmail, _mailboxPassword);

                        client.Send(message);
                        client.Disconnect(true);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }

                //await service.Users.Messages.Send(gmailMessage, "me").ExecuteAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send verification email with error: {ex.Message}");
            }
        }

        
    }
}
