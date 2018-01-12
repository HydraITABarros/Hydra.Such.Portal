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
                WSShipmentDocHeader = new WSTransferShipmentHeader.WSShipmentDocHeader()
                {
                    Nº_Projecto = transferShipment.ProjectNo,
                   
                    FunctionAreaCode20 = transferShipment.FunctionalAreaNo,
                    RegionCode20 = transferShipment.RegionNo,
                    ResponsabilityCenterCode20 = transferShipment.ResponsibilityCenterNo,
                    Nº_Requisição = transferShipment.RequisitionNo,
                    Observações = transferShipment.Comments,
                    Cod_Postal_Descarga = "4700-301",
                    Post_Code = "4700-301",
                    Local_Descarga1 = "Local_Descarga1",
                    Morada_Cliente = "Morada_Cliente",
                    Data_Descarga = DateTime.Now,
                    Data_DescargaSpecified = true
                }
            };

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_TransferShipmentHeader_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSTransferShipmentHeader.WSShipmentDocHeader_PortClient ws_Client = new WSTransferShipmentHeader.WSShipmentDocHeader_PortClient(navWSBinding, ws_URL);
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
            int counter = 0;
            WSTransferShipmentLine.CreateMultiple navCreate = new WSTransferShipmentLine.CreateMultiple();
            navCreate.WSShipmentDocLine_List = transferShipment.Lines.Select(line =>
                new WSTransferShipmentLine.WSShipmentDocLine()
                {
                    Nº_Linha = (counter+=10000),
                    Nº_LinhaSpecified = true,
                    Tipo = WSTransferShipmentLine.Tipo.Produto,
                    TipoSpecified = true,
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
            WSTransferShipmentLine.WSShipmentDocLine_PortClient ws_Client = new WSTransferShipmentLine.WSShipmentDocLine_PortClient(navWSBinding, ws_URL);
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
