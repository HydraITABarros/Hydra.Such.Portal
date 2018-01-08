using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Hydra.Such.Data.NAV
{
    public class WSGeneric
    {
        static BasicHttpBinding navWSBinding;

        static WSGeneric()
        {
            ///Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
        }

        public static async Task<WSGenericCodeUnit.FxPostInvoice_Result> CreatePreInvoiceLineList(String HeaderNo, NAVWSConfigurations WSConfigurations)
        {
            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_Generic_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSGenericCodeUnit.WsGeneric_PortClient WS_Client = new WSGenericCodeUnit.WsGeneric_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSGenericCodeUnit.FxPostInvoice_Result result = await WS_Client.FxPostInvoiceAsync(HeaderNo);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<WSGenericCodeUnit.FxCabimento_Result> CreatePurchaseOrderFitting(String prePurchHeaderNo, NAVWSConfigurations WSConfigurations)
        {
            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_Generic_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSGenericCodeUnit.WsGeneric_PortClient ws_Client = new WSGenericCodeUnit.WsGeneric_PortClient(navWSBinding, WS_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            //try
            //{
                return await ws_Client.FxCabimentoAsync(prePurchHeaderNo);
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
        }
    }
}
