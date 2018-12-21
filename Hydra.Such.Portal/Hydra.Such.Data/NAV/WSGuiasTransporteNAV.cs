using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.GuiaTransporte;
using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using WSSuchNav2017;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.NAV
{
    public static class WSGuiasTransporteNAV
    {
        static BasicHttpBinding navWSBinding;

        static WSGuiasTransporteNAV()
        {
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
        }

        //public static async Task<WSSuchNav2017.WSNovaGuiaTransporte_Result> CreateGuia(NAVWSConfigurations WSConfig)
        //{
        //    WSSuchNav2017.WSNovaGuiaTransporte newGuia = new WSNovaGuiaTransporte();

        //    EndpointAddress WS_URL = new EndpointAddress(WSConfig.Ws_SuchNav2017_URL.Replace);
        //}
    }
}
