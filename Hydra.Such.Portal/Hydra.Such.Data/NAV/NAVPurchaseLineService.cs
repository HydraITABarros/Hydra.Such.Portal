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
using System.Linq;

namespace Hydra.Such.Data.NAV
{
    public static class NAVPurchaseLineService
    {
        static BasicHttpBinding navWSBinding;

        static NAVPurchaseLineService()
        {
            // Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows; 
        }

        public static async Task<WSPurchaseInvLine.CreateMultiple_Result> CreateMultipleAsync(PurchOrderDTO purchFromSupplier, NAVWSConfigurations WSConfigurations)
        {
            if (purchFromSupplier == null)
                throw new ArgumentNullException("purchFromSupplier");

            WSPurchaseInvLine.CreateMultiple navCreate = new WSPurchaseInvLine.CreateMultiple();
            navCreate.WSPurchInvLineInterm_List = purchFromSupplier.Lines.Select(purchLine =>
                new WSPurchaseInvLine.WSPurchInvLineInterm()
                {
                    Document_No = purchFromSupplier.NAVPrePurchOrderId,
                    Document_Type = WSPurchaseInvLine.Document_Type.Order,
                    Document_TypeSpecified = true,
                    Type = WSPurchaseInvLine.Type.Item,
                    TypeSpecified = true,
                    No = purchLine.Code,
                    Description = purchLine.Description,
                    Buy_from_Vendor_No = purchFromSupplier.SupplierId,
                    Quantity = purchLine.QuantityRequired.HasValue ? purchLine.QuantityRequired.Value : 0,
                    QuantitySpecified = true,
                    Direct_Unit_Cost = purchLine.UnitCost.HasValue ? purchLine.UnitCost.Value : 0,
                    Direct_Unit_CostSpecified = true,
                    Job_No = purchLine.ProjectNo,
                    gLocation = purchLine.LocationCode,
                    Blanket_Order_No = purchLine.OpenOrderNo,
                    Blanket_Order_Line_No = purchLine.OpenOrderLineNo.Value,
                    Blanket_Order_Line_NoSpecified = true,
                })
                .ToArray();

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_PurchaseInvLine_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSPurchaseInvLine.WSPurchInvLineInterm_PortClient ws_Client = new WSPurchaseInvLine.WSPurchInvLineInterm_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSPurchaseInvLine.CreateMultiple_Result result = await ws_Client.CreateMultipleAsync(navCreate);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
