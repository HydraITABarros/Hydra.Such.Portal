using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Clients;
using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Such.Data.NAV
{
    public static class WSClient
    {
        static BasicHttpBinding navWSBinding;

        static WSClient()
        {
            ///Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
        }

        public static async Task<WSClientNAV.Create_Result> CreateAsync(ClientDetailsViewModel client, NAVWSConfigurations WSConfigurations)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            WSClientNAV.Create navCreate = new WSClientNAV.Create()
            {
                teste = new WSClientNAV.teste 
                {
                    No = client.No,
                    Name = client.Name,
                   // to be continue ...................
                }
            };


            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_Contacts_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSClientNAV.teste_PortClient ws_Client = new WSClientNAV.teste_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            //try
            //{
            WSClientNAV.Create_Result result = await ws_Client.CreateAsync(navCreate);
            return result;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}

        }

        public static async Task<WSContacts.Update_Result> UpdateAsync(ContactViewModel contact, NAVWSConfigurations WSConfigurations)
        {
            if (contact == null)
                throw new ArgumentNullException("contact");


            WSContacts.Update navUpdate = new WSContacts.Update()
            {
                WSContact = new WSContacts.WSContact()
                {
                    No = contact.Id,
                    gName = contact.Name,
                    City = contact.City,
                    E_Mail = contact.Email,
                    gAddress = contact.Address,
                    Mobile_Phone_No = contact.MobilePhoneContact,
                    Phone_No = contact.Phone,
                    Post_Code = contact.ZipCode,
                    VAT_Registration_No = contact.VATNumber,
                }
            };


            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_Contacts_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSContacts.WSContact_PortClient ws_Client = new WSContacts.WSContact_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            WSContacts.Read_Result resultRead = await ws_Client.ReadAsync(navUpdate.WSContact.No);
            navUpdate.WSContact.Key = resultRead.WSContact.Key;

            WSContacts.Update_Result result = await ws_Client.UpdateAsync(navUpdate);
            return result;

        }

        public static async Task<WSContacts.Delete_Result> DeleteAsync(ContactViewModel contact, NAVWSConfigurations WSConfigurations)
        {
            if (contact == null)
                throw new ArgumentNullException("contact");


            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_Contacts_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSContacts.WSContact_PortClient ws_Client = new WSContacts.WSContact_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);


            WSContacts.Read_Result resultRead = await ws_Client.ReadAsync(contact.Id);


            WSContacts.Delete_Result result = await ws_Client.DeleteAsync(resultRead.WSContact.Key);
            return result;

        }

        public static async Task<WSCreateNAVProject.Read_Result> GetAsync(string ProjectNo, NAVWSConfigurations WSConfigurations)
        {

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_Job_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreateNAVProject.WSJob_PortClient WS_Client = new WSCreateNAVProject.WSJob_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCreateNAVProject.Read_Result result = await WS_Client.ReadAsync(ProjectNo);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
