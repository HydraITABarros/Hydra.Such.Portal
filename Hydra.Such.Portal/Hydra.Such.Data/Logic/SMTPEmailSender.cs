using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Such.Data.Logic
{
    public class SMTPEmailSender : EmailAutomation
    {
        public async Task SendMailAsync(string from, string to, string subject, string htmlMessage)
        {
            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(from, DisplayName);
                mailMessage.To.Add(new MailAddress(to));

                mailMessage.Subject = subject;
                mailMessage.Body = htmlMessage;
                mailMessage.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient(Config.Host, Config.Port))
                {
                    NetworkCredential Credentials = new NetworkCredential(Config.Username, Config.Password);
                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.Credentials = Credentials;
                    smtpClient.EnableSsl = Config.SSL;
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }
    }
}
