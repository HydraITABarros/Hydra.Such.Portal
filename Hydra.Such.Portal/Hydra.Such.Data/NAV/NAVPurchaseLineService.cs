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
                    Document_Type = WSPurchaseInvLine.Document_Type.Order,
                    Document_TypeSpecified = true,
                    Document_No = purchFromSupplier.NAVPrePurchOrderId,
                    No = purchLine.Code,
                    Buy_from_Vendor_No = purchFromSupplier.SupplierId,
                    Pay_to_Vendor_No = purchFromSupplier.SupplierId,
                    Quantity = purchLine.QuantityRequired.HasValue ? purchLine.QuantityRequired.Value : 0,
                    QuantitySpecified = true,
                    gLocation = purchLine.LocationCode,
                    Job_No = purchLine.ProjectNo,
                    RegionCode20 = purchLine.RegionCode,
                    FunctionAreaCode20 = purchLine.FunctionalAreaCode,
                    ResponsabilityCenterCode20 = purchLine.CenterResponsibilityCode,
                    Type = WSPurchaseInvLine.Type.Item,
                    TypeSpecified = true,
                    Description100 = purchLine.Description,
                    Line_Discount_Percent = purchLine.DiscountPercentage.HasValue ? purchLine.DiscountPercentage.Value : 0,
                    Line_Discount_PercentSpecified = purchLine.DiscountPercentage.HasValue,
                    VAT_Bus_Posting_Group = purchLine.VATBusinessPostingGroup,
                    VAT_Prod_Posting_Group = purchLine.VATProductPostingGroup,
                    Requisition_No = purchFromSupplier.RequisitionId,
                    Requisition_Line_No = purchLine.LineId.HasValue ? purchLine.LineId.Value : 0,
                    Requisition_Line_NoSpecified = true,
                    Unit_of_Measure_Code = purchLine.UnitMeasureCode,
                    Direct_Unit_Cost = purchLine.UnitCost.HasValue ? purchLine.UnitCost.Value : 0,
                    Direct_Unit_CostSpecified = true,
                    Blanket_Order_No = string.IsNullOrEmpty(purchLine.OpenOrderNo) ? string.Empty : purchLine.OpenOrderNo,
                    Blanket_Order_Line_No = purchLine.OpenOrderLineNo.HasValue ? purchLine.OpenOrderLineNo.Value : 0,
                    Blanket_Order_Line_NoSpecified = purchLine.OpenOrderLineNo.HasValue,
                })
                .ToArray();

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_PurchaseInvLine_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSPurchaseInvLine.WSPurchInvLineInterm_PortClient ws_Client = new WSPurchaseInvLine.WSPurchInvLineInterm_PortClient(navWSBinding, ws_URL);
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

        /// <summary>
        /// Para criar encomendas com referência a encomendas de compra abertas.
        /// Necessário para contornar validação do NAV no campo Pay-to Vendor No.". Primeiro criar as linhas para gerar as chaves, etc e depois atualizar e preencher com os dados pretendidos.
        /// </summary>
        /// <param name="purchFromSupplier"></param>
        /// <param name="WSConfigurations"></param>
        /// <returns></returns>
        public static bool CreateAndUpdateMultipleAsync(PurchOrderDTO purchFromSupplier, NAVWSConfigurations WSConfigurations)
        {
            if (purchFromSupplier == null)
                throw new ArgumentNullException("purchFromSupplier");

            WSPurchaseInvLine.CreateMultiple navCreate = new WSPurchaseInvLine.CreateMultiple();
            navCreate.WSPurchInvLineInterm_List = purchFromSupplier.Lines.Select(purchLine =>
                new WSPurchaseInvLine.WSPurchInvLineInterm()
                {
                    Document_Type = WSPurchaseInvLine.Document_Type.Order,
                    Document_TypeSpecified = true,
                    Document_No = purchFromSupplier.NAVPrePurchOrderId,
                    Buy_from_Vendor_No = purchFromSupplier.SupplierId,
                    Requisition_No = purchFromSupplier.RequisitionId,
                    Requisition_Line_No = purchLine.LineId.HasValue ? purchLine.LineId.Value : 0,
                    Requisition_Line_NoSpecified = true,
                })
                .ToArray();

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_PurchaseInvLine_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSPurchaseInvLine.WSPurchInvLineInterm_PortClient ws_Client = new WSPurchaseInvLine.WSPurchInvLineInterm_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            Task<WSPurchaseInvLine.CreateMultiple_Result> createPurchaseLinesTask = ws_Client.CreateMultipleAsync(navCreate);
            createPurchaseLinesTask.Wait();
            if (createPurchaseLinesTask.IsCompletedSuccessfully)
            {
                Task<WSPurchaseInvLine.UpdateMultiple_Result> updatePurchaseLinesTask = UpdateMultipleAsync(purchFromSupplier, createPurchaseLinesTask.Result.WSPurchInvLineInterm_List.ToList(), WSConfigurations);
                updatePurchaseLinesTask.Wait();
                return updatePurchaseLinesTask.IsCompletedSuccessfully;
            }
            return false;
            //try
            //{
            //return await ws_Client.CreateMultipleAsync(navCreate);
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }
        private static async Task<WSPurchaseInvLine.UpdateMultiple_Result> UpdateMultipleAsync(PurchOrderDTO purchFromSupplier, List<WSPurchaseInvLine.WSPurchInvLineInterm> itemsToUpdate, NAVWSConfigurations WSConfigurations)
        {
            if (purchFromSupplier == null)
                throw new ArgumentNullException("purchFromSupplier");

            itemsToUpdate.ForEach(purchInvLine =>
            {
                var item = purchFromSupplier.Lines.Single(x => x.LineId == (int)purchInvLine.Requisition_Line_No);
                //purchInvLine.Document_Type = WSPurchaseInvLine.Document_Type.Order;
                //purchInvLine.Document_TypeSpecified = true;
                purchInvLine.Document_No = purchFromSupplier.NAVPrePurchOrderId;
                purchInvLine.No = item.Code;
                //purchInvLine.Buy_from_Vendor_No = purchFromSupplier.SupplierId;
                //purchInvLine.Pay_to_Vendor_No = purchFromSupplier.SupplierId;
                purchInvLine.Quantity = item.QuantityRequired.HasValue ? item.QuantityRequired.Value : 0;
                purchInvLine.QuantitySpecified = true;
                purchInvLine.gLocation = item.LocationCode;
                purchInvLine.Job_No = item.ProjectNo;
                purchInvLine.RegionCode20 = item.RegionCode;
                purchInvLine.FunctionAreaCode20 = item.FunctionalAreaCode;
                purchInvLine.ResponsabilityCenterCode20 = item.CenterResponsibilityCode;
                purchInvLine.Type = WSPurchaseInvLine.Type.Item;
                purchInvLine.TypeSpecified = true;
                purchInvLine.Description100 = item.Description;
                purchInvLine.Line_Discount_Percent = item.DiscountPercentage.HasValue ? item.DiscountPercentage.Value : 0;
                purchInvLine.Line_Discount_PercentSpecified = item.DiscountPercentage.HasValue;
                purchInvLine.VAT_Bus_Posting_Group = item.VATBusinessPostingGroup;
                purchInvLine.VAT_Prod_Posting_Group = item.VATProductPostingGroup;
                purchInvLine.Requisition_No = purchFromSupplier.RequisitionId;
                purchInvLine.Requisition_Line_No = item.LineId.HasValue ? item.LineId.Value : 0;
                purchInvLine.Requisition_Line_NoSpecified = true;
                purchInvLine.Unit_of_Measure_Code = item.UnitMeasureCode;
                purchInvLine.Direct_Unit_Cost = item.UnitCost.HasValue ? item.UnitCost.Value : 0;
                purchInvLine.Direct_Unit_CostSpecified = true;
                purchInvLine.Blanket_Order_No = string.IsNullOrEmpty(item.OpenOrderNo) ? string.Empty : item.OpenOrderNo;
                purchInvLine.Blanket_Order_Line_No = item.OpenOrderLineNo.HasValue ? item.OpenOrderLineNo.Value : 0;
                purchInvLine.Blanket_Order_Line_NoSpecified = item.OpenOrderLineNo.HasValue;
            });

            WSPurchaseInvLine.UpdateMultiple navUpdate = new WSPurchaseInvLine.UpdateMultiple();
            navUpdate.WSPurchInvLineInterm_List = itemsToUpdate.ToArray();

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_PurchaseInvLine_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSPurchaseInvLine.WSPurchInvLineInterm_PortClient ws_Client = new WSPurchaseInvLine.WSPurchInvLineInterm_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            //try
            //{
            return await ws_Client.UpdateMultipleAsync(navUpdate);
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}

        }
    }
}
