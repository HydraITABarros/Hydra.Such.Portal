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
    public static class NAVPurchaseService
    {
        static BasicHttpBinding navWSBinding;

        static NAVPurchaseService()
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
                    No = contact.Id,
                    Name = contact.Name
                }
            };

        
            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_Contacts_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSContacts.WSContact_PortClient ws_Client = new WSContacts.WSContact_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSContacts.Create_Result result = await ws_Client.CreateAsync(navCreate);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
