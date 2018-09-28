using Hydra.Such.Data.ViewModel;
using System.Net;
using System.Net.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Hydra.Such.Data.Logic.PedidoCotacao
{
    public class SendEmailsPedidoCotacao : EmailAutomation
    {
        private bool MailSent = false;


        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            MailSent = false;
            string Token = (string)e.UserState;
            if (!e.Cancelled && e.Error == null)
            {
                MailSent = true;
                if (MailSent)
                {
                   
                }
            }

            if (e.Error != null)
            {
            }
        }


        public void SendEmail()
        {
            SmtpClient Client = new SmtpClient(Config.Host, Config.Port);
            NetworkCredential Credentials = new NetworkCredential(Config.Username, Config.Password);
            Client.UseDefaultCredentials = true;
            Client.Credentials = Credentials;
            Client.EnableSsl = Config.SSL;

            MailMessage MMessage = new MailMessage
            {
                From = new MailAddress(From, DisplayName)
            };

            foreach (var t in To)
            {
                MMessage.To.Add(new MailAddress(t));
            }

            foreach (var cc in CC)
            {
                if (IsValidEmail(cc))
                    MMessage.CC.Add(cc);
            }

            foreach (var bcc in BCC)
            {
                if (IsValidEmail(bcc))
                    MMessage.Bcc.Add(bcc);
            }

            MMessage.Subject = Subject;
            MMessage.Body = Body;
            MMessage.IsBodyHtml = IsBodyHtml;

            Attachment attachment = new Attachment(Anexo);

            MMessage.Attachments.Add(attachment);

            Client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            string UserState = "EmailsPedidoCotacao";
            Client.SendAsync(MMessage, UserState);
        }



    }
}
