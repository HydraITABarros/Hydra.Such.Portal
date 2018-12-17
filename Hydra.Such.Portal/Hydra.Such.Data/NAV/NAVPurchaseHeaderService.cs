using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using Hydra.Such.Data.ViewModel.Compras;

namespace Hydra.Such.Data.NAV
{
    public static class NAVPurchaseHeaderService
    {
        static BasicHttpBinding navWSBinding;

        static NAVPurchaseHeaderService()
        {
            // Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows; 
        }

        public static async Task<WsPrePurchaseDocs.Create_Result> CreateAsync(BillingReceptionModel purchDoc, string prePurchInvoiceNoSerie, NAVWSConfigurations WSConfigurations)
        {
            if (purchDoc == null)
                throw new ArgumentNullException("purchDoc");

            WsPrePurchaseDocs.WSPrePurchaseDocs itemToCreate = new WsPrePurchaseDocs.WSPrePurchaseDocs();
            itemToCreate.Document_Type = (WsPrePurchaseDocs.Document_Type)purchDoc.TipoDocumento;
            itemToCreate.Document_TypeSpecified = true;
            itemToCreate.RegionCode20 = purchDoc.CodRegiao;
            itemToCreate.FunctionAreaCode20 = purchDoc.CodAreaFuncional;
            itemToCreate.ResponsabilityCenterCode20 = purchDoc.CodCentroResponsabilidade;
            itemToCreate.Buy_from_Vendor_No = purchDoc.CodFornecedor;
            itemToCreate.Rececao_Faturacao = purchDoc.Id;
            itemToCreate.No_Series = string.Empty;
            itemToCreate.Posting_No_Series = prePurchInvoiceNoSerie;
            if (purchDoc.Valor.HasValue)
            {
                itemToCreate.Valor_Factura = purchDoc.Valor.Value;
                itemToCreate.Valor_FacturaSpecified = true;
            }
            if (purchDoc.TipoDocumento == Enumerations.BillingDocumentTypes.Fatura)
                itemToCreate.Vendor_Invoice_No = purchDoc.NumDocFornecedor;
            else
                itemToCreate.Vendor_Cr_Memo_No = purchDoc.NumDocFornecedor;

            WsPrePurchaseDocs.Create navCreate = new WsPrePurchaseDocs.Create(itemToCreate);

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_PurchaseHeaderDocs_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WsPrePurchaseDocs.WSPrePurchaseDocs_PortClient ws_Client = new WsPrePurchaseDocs.WSPrePurchaseDocs_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            //try
            //{
            return await ws_Client.CreateAsync(navCreate);
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }
    }
}
