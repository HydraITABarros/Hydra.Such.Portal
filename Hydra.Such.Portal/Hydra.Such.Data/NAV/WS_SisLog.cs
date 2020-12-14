using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Projects;
using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using WSSisLog;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.NAV
{
    public static class WS_SisLog
    {
        static BasicHttpBinding navWSBinding;

        static WS_SisLog()
        {
            ///Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
        }

        public static async Task<getStockResponse> GetSTOCK(string Armazem, string Produto, NAVWSConfigurations WSConfigurations)
        {
            try
            {
                InputParametersStockActual input = new InputParametersStockActual();
                input.almacen = Armazem;
                input.articulo = Produto;
                getStock stock = new getStock(input);

                EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_SisLog_URL);
                StockActualSOAPClient WS_Client = new StockActualSOAPClient(navWSBinding, WS_URL);
                WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
                WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential("", "");

                getStockResponse result = await WS_Client.getStockAsync(stock);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
