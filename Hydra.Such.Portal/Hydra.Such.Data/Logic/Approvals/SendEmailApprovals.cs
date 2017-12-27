using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Hydra.Such.Data.Logic.Approvals
{
    public class SendEmailApprovals : EmailAutomation
    {
        public EmailsAprovações EmailApproval { get; set; }
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
                    EmailApproval.ObservaçõesEnvio = "Mensagem enviada com Sucesso";
                    EmailApproval.Enviado = true;
                    DBApprovalEmails.Update(EmailApproval);
                }
            }

            if (e.Error != null)
            {
                EmailApproval.ObservaçõesEnvio = "Não foi possível enviar a mensagem " + DateTime.Now.ToString();
                DBApprovalEmails.Update(EmailApproval);
            }
        }

        public void SendEmail()
        {
            if (EmailApproval == null)
                return;
            
            if (string.IsNullOrEmpty(From) && !IsValidEmail(From))
            {
                EmailApproval.ObservaçõesEnvio = "Email do remetente inválido!";

                DBApprovalEmails.Update(EmailApproval);
                return;
            }

            if (To == null || To.Count <= 0)
            {
                EmailApproval.ObservaçõesEnvio = "Não há destinatários";
                return;
            }

            DBApprovalEmails.Update(EmailApproval);

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

            Client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            string UserState = "EmailAprovações";
            Client.SendAsync(MMessage, UserState);
        }
    }
}
