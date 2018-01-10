using Hydra.Such.Data.ViewModel.Compras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Such.Data.NAV
{
    public class NAVTransferShipmentService
    {

        static BasicHttpBinding navWSBinding;

        static NAVTransferShipmentService()
        {
            // Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
        }

        public static async Task<WSTransferShipmentHeader.Create_Result> CreateHeaderAsync(TransferShipment transferShipment, NAVWSConfigurations WSConfigurations)
        {
            if (transferShipment == null)
                throw new ArgumentNullException("transferShipment");

            WSTransferShipmentHeader.Create navCreate = new WSTransferShipmentHeader.Create()
            {
                WSCabGuiaTransporte = new WSTransferShipmentHeader.WSCabGuiaTransporte()
                {
                    Nº_Projecto = transferShipment.ProjectNo,
                    Global_Dimension_1_Code = transferShipment.RegionNo,
                    Global_Dimension_2_Code = transferShipment.FunctionalAreaNo,
                    Global_Dimension_3_Code = transferShipment.ResponsibilityCenterNo,
                    Nº_Requisição = transferShipment.RequisitionNo,
                    Observações = transferShipment.Comments,
                }
            };

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_TransferShipmentHeader_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSTransferShipmentHeader.WSCabGuiaTransporte_PortClient ws_Client = new WSTransferShipmentHeader.WSCabGuiaTransporte_PortClient(navWSBinding, ws_URL);
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

        public static async Task<WSTransferShipmentLine.CreateMultiple_Result> CreateLinesAsync(TransferShipment transferShipment, NAVWSConfigurations WSConfigurations)
        {
            if (transferShipment == null)
                throw new ArgumentNullException("transferShipment");

            WSTransferShipmentLine.CreateMultiple navCreate = new WSTransferShipmentLine.CreateMultiple();
            navCreate.WSLinGuiaTransporte_List = transferShipment.Lines.Select(line =>
                new WSTransferShipmentLine.WSLinGuiaTransporte()
                {
                    No_projecto = transferShipment.ProjectNo,
                    No = line.ProductNo,
                    Descricao = line.ProductDescription,
                    Nº_Documento = transferShipment.TransferShipmentNo,
                    Quantidade = line.Quantity.HasValue ? line.Quantity.Value : 0,
                    QuantidadeSpecified = true,
                    Unit_Cost = line.UnitCost.HasValue ? line.UnitCost.Value : 0,
                    Unit_CostSpecified = true,
                })
                .ToArray();

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_TransferShipmentLine_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSTransferShipmentLine.WSLinGuiaTransporte_PortClient ws_Client = new WSTransferShipmentLine.WSLinGuiaTransporte_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            //try
            //{
            return await ws_Client.CreateMultipleAsync(navCreate);
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }
    }
}
