using System;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Hydra.Such.Data.NAV
{
    public static class WSPaymentJournal
    {
        static BasicHttpBinding navWSBinding;

        static WSPaymentJournal()
        {
            ///Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
        }

        public static async Task<WSPaymentJournalNAV.Create_Result> CreatePaymentJournalNAV(WSPaymentJournalNAV.WSPaymentJournal PaymentJournalToCreate, NAVWSConfigurations WSConfigurations)
        {
            WSPaymentJournalNAV.Create NAVCreate = new WSPaymentJournalNAV.Create()
            {
                WSPaymentJournal = new WSPaymentJournalNAV.WSPaymentJournal()
                {
                    Journal_Template_Name = PaymentJournalToCreate.Journal_Template_Name,
                    Journal_Batch_Name = PaymentJournalToCreate.Journal_Batch_Name,
                    Line_No = PaymentJournalToCreate.Line_No,
                    Line_NoSpecified = true,
                    Account_Type = PaymentJournalToCreate.Account_Type,
                    Account_TypeSpecified = true,
                    Account_No = PaymentJournalToCreate.Account_No,
                    Description = PaymentJournalToCreate.Description,
                    Posting_Date = PaymentJournalToCreate.Posting_Date,
                    Posting_DateSpecified = true,
                    Document_Type = PaymentJournalToCreate.Document_Type,
                    Document_TypeSpecified = true,
                    Amount = PaymentJournalToCreate.Amount,
                    AmountSpecified = true,
                    
                }
            };
            //NAVCreate.CurrentJnlBatchName = NAVCreate.WSPaymentJournal.Journal_Batch_Name;

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_PaymentJournalNAV_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSPaymentJournalNAV.WSPaymentJournal_PortClient WS_Client = new WSPaymentJournalNAV.WSPaymentJournal_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSPaymentJournalNAV.Create_Result result = await WS_Client.CreateAsync(NAVCreate);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
