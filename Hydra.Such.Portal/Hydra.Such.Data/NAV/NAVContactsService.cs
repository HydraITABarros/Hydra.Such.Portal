//using System;
//using System.Collections.Generic;
//using System.ServiceModel;
//using System.Text;
//using System.Threading.Tasks;
//using System.Net;

using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.NAV
{
    public static class NAVContactsService
    {
        static BasicHttpBinding navWSBinding;

        static NAVContactsService()
        {
            // Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
        }

        public static async Task<WSContacts.Create_Result> CreateAsync(ContactViewModel contact, NAVWSConfigurations WSConfigurations)
        {
            if (contact == null)
                throw new ArgumentNullException("contact");

            WSContacts.Create navCreate = new WSContacts.Create()
            {
                WSContact = new WSContacts.WSContact()
                {
                    No = contact.No,
                    gName = contact.Nome,
                    City = contact.ClienteCidade,
                    E_Mail = contact.Email,
                    gAddress = contact.ClienteEndereco,
                    Mobile_Phone_No = contact.Telemovel,
                    Phone_No = contact.Telefone,
                    Post_Code = contact.ClienteCodigoPostal,
                    VAT_Registration_No = contact.ClienteNIF,
                }
            };

        
            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_Contacts_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSContacts.WSContact_PortClient ws_Client = new WSContacts.WSContact_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            //try
            //{
                WSContacts.Create_Result result = await ws_Client.CreateAsync(navCreate);
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
                    No = contact.No,
                    gName = contact.Nome,
                    City = contact.ClienteCidade,
                    E_Mail = contact.Email,
                    gAddress = contact.ClienteEndereco,
                    Mobile_Phone_No = contact.Telemovel,
                    Phone_No = contact.Telefone,
                    Post_Code = contact.ClienteCodigoPostal,
                    VAT_Registration_No = contact.ClienteNIF,
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

            
            WSContacts.Read_Result resultRead = await ws_Client.ReadAsync(contact.No);
            

            WSContacts.Delete_Result result = await ws_Client.DeleteAsync(resultRead.WSContact.Key);
            return result;

        }
    }
}
