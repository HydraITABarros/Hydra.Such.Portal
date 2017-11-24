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


namespace Hydra.Such.Data.ViewModel
{
    public class __Configurations
    {
        private const string __Host = "10.19.247.98";
        private const string __Username = "navision@such.pt";
        private const string __Password = "";
        private const int __Port = 25;
        private const bool __SSL = false;


        public string Host
        {
            get { return __Host; }
        }
        public string Username
        {
            get { return __Username; }
        }

        public int Port
        {
            get { return __Port; }
        }
        public string Password
        {
            get { return __Password; }
        }

        public bool SSL
        {
            get { return __SSL; }
        }
    }
    public class EmailAutomation
    {
        // Properties declaration
        #region Properties declaration
        public string From { get; set; }
        public string DisplayName { get; set; }
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        private __Configurations Config { get; set; }
        private EmailsProcedimentosCcp EmailProcedimento
        {
            set { EmailProcedimento = value; }
        }

        private static bool MailSent = false; 
        #endregion

        #region Class constructors & Set accessor for the To property
        public EmailAutomation()
        {
            To = new List<string>();
            Config = new __Configurations();
        }
        public EmailAutomation(string from, string displayName, string subject, string body, bool isBodyHtml)
        {
            From = from;
            To = new List<string>();
            DisplayName = displayName;
            Subject = subject;
            Body = body;
            IsBodyHtml = isBodyHtml;
            Config = new __Configurations();
        }

        public void SetTo(List<string> DestinationAddresses)
        {
            if(DestinationAddresses != null && DestinationAddresses.Count > 0)
            {
                foreach(var da in DestinationAddresses)
                {
                    if (IsValidEmail(da))
                    {
                        To.Add(da);
                    }
                }
            }
        }
        #endregion

        #region Check if email is valid
        /*
         *      The next methods was taken from Microsoft website
         *      URL: https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
         */
        static bool invalid = false;
        public static bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
        #endregion

        #region Function that will be use in the EventHandler
        /* 
        * URL: https://msdn.microsoft.com/en-us/library/system.net.mail.smtpclient(v=vs.110).aspx
        *      To send the e-mail message and block while waiting for the e-mail to be transmitted to the SMTP server, use one of the synchronous Send methods. 
        *      To allow your program's main thread to continue executing while the e-mail is transmitted, use one of the asynchronous SendAsync methods. 
        *      The SendCompleted event is raised when a SendAsync operation completes. To receive this event, you must add a SendCompletedEventHandler delegate to SendCompleted. 
        *      The SendCompletedEventHandler delegate must reference a callback method that handles notification of SendCompleted events. 
        *      To cancel an asynchronous e-mail transmission, use the SendAsyncCancel method. 
        *      
        *      The SendCompletedCallback is the method used by the SendAsync SMTP method and will perform the database updates when email has been successfuly sent
        */
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            string Token = (string)e.UserState;
            if (!e.Cancelled && e.Error == null)
            {
                MailSent = true;
                if (MailSent)
                {
                    // Log the success
                }
            }

            if(e.Error != null)
            {
                // Log the error
            }
        }
        private static void SendCompletedCallbackForProcedimento(object sender, AsyncCompletedEventArgs e)
        {
            string Token = (string)e.UserState;
            if (!e.Cancelled && e.Error == null)
            {
                MailSent = true;
                if (MailSent)
                {
                    // Do database update
                }
            }

            if (e.Error != null)
            {
                // Log the erros to the EmailProcedimento table
            }
        }
        #endregion

        public void SendEmail()
        {
            if (To == null || string.IsNullOrEmpty(From))
                return;

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
                if(IsValidEmail(t))
                    MMessage.To.Add(new MailAddress(t));
            }

            MMessage.Subject = Subject;
            MMessage.Body = Body;
            MMessage.IsBodyHtml = IsBodyHtml;

            Client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            string UserState = "EmailProcedimentos";

            Client.SendAsync(MMessage, UserState);
        }

        public void SendEmailProcedimento()
        {

        }
    }
}
