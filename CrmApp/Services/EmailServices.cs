using CrmApp.Models.OptionsModels;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace CrmApp.Services
{
    public class EmailServices : IEmailServices
    {

        private readonly EmailSettings _emailSettings;

        public EmailServices(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendResetPasswordEmail(string passwordLink, string toEmail)
        {
            var smtpClient = new SmtpClient();

            smtpClient.Host=_emailSettings.Host;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
            smtpClient.EnableSsl = true;
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_emailSettings.Email);
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = "LocalHost | Şifre sıfırlama linki";

            mailMessage.Body = @$"<h4>şifrenizi yenilemek için aşağıdaki linke tıklayınız</h4>
                <p><a href='{passwordLink}'>
                şifre yenileme linki
                </a></p>";

            mailMessage.IsBodyHtml = true;
            await smtpClient.SendMailAsync(mailMessage);

        }
    }
}
