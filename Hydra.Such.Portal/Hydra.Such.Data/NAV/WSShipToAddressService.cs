using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Hydra.Such.Data.NAV
{
    public static class WSShipToAddressService
    {
        static BasicHttpBinding navWSBinding;

        static WSShipToAddressService()
        {
            ///Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
            navWSBinding.MaxReceivedMessageSize = 20971520;
            /*navWSBinding.MaxBufferSize = 20971520;
            navWSBinding.MaxBufferPoolSize = 20971520;*/
        }
    }
}
