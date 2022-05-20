using System.Threading.Tasks;
using Input.Constants.InfoMessages;
using MailKit.Net.Smtp;
using MimeKit;

namespace Input.Email
{
    public static class EmailWork
    {
        public static async Task SendEmailDefault(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            
            emailMessage.From.Add(new MailboxAddress(UserInfoConstants.EmailName, UserInfoConstants.EmailService));
            emailMessage.To.Add(new MailboxAddress(string.Empty, email));
            
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(UserInfoConstants.ConnectHost, 587);
                await client.AuthenticateAsync(UserInfoConstants.EmailServiceGmail, UserInfoConstants.PasswordService);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}