using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Web;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.IO;
using System.Net;
using System.Configuration;

using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.CCP;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic.CCP
{
    public class SendEmailsProcedimentos : EmailAutomation
    {
        public EmailsProcedimentosCcp EmailProcedimento { get; set; }
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
                    EmailProcedimento.UtilizadorModificação = UserID;
                    EmailProcedimento.DataHoraModificação = DateTime.Now;
                    EmailProcedimento.ObservacoesEnvio = "Mensagem enviada com Sucesso";
                    EmailProcedimento.Email = true;
                    DBProcedimentosCCP.__UpdateEmailProcedimento(EmailProcedimento);
                }
            }

            if (e.Error != null)
            {
                EmailProcedimento.ObservacoesEnvio = "Não foi possível enviar a mensagem " + DateTime.Now.ToString();
                DBProcedimentosCCP.__UpdateEmailProcedimento(EmailProcedimento);
            }
        }

        public void SendEmail()
        {
            if (EmailProcedimento == null)
                return;

            // the From property must be set and be a valid email address
            if (string.IsNullOrEmpty(From) && !IsValidEmail(From))
            {
                EmailProcedimento.ObservacoesEnvio = "Email do remetente inválido!";

                DBProcedimentosCCP.__UpdateEmailProcedimento(EmailProcedimento);
                return;
            }
            

            if (To == null || To.Count <= 0)
            {
                EmailProcedimento.ObservacoesEnvio = "Não há destinatários";
                return;
            }

            DBProcedimentosCCP.__UpdateEmailProcedimento(EmailProcedimento);

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

            //MMessage.Attachments.Add

            Client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            string UserState = "EmailProcedimentos";
            Client.SendAsync(MMessage, UserState);
        }

        public void SendEmail_ToJuri_With_Attachment()
        {
            
        }
    }
}
