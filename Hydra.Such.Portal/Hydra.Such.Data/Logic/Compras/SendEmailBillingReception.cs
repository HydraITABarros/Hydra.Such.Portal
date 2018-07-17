using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Hydra.Such.Data.Logic.Request
{
    public class SendEmailBillingReception : EmailAutomation
    {     
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
            MMessage.Body = Body;
            MMessage.IsBodyHtml = IsBodyHtml;
            MMessage.Subject = Subject;

            //Client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            string UserState = "EmailFacturas";
            Client.SendAsync(MMessage, UserState);
        }
    }
}
