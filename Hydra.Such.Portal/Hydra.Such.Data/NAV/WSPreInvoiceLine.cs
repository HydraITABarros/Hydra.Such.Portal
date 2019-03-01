using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;
using Hydra.Such.Data.Logic.ProjectDiary;

namespace Hydra.Such.Data.NAV
{
    public class WSPreInvoiceLine
    {
        static BasicHttpBinding navWSBinding;

        static WSPreInvoiceLine()
        {
            ///Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
        }

        public static async Task<WSCreatePreInvoiceLine.Create_Result> CreatePreInvoiceLine(ProjectDiaryViewModel PreInvoiceLineToCreate, NAVWSConfigurations WSConfigurations, string PKey)
        {
            //Mapping Object
            WSCreatePreInvoiceLine.Type TypeValue;
            switch (PreInvoiceLineToCreate.Type)
            {
                case 1:
                    TypeValue = WSCreatePreInvoiceLine.Type.Resource;
                    break;
                case 2:
                    TypeValue = WSCreatePreInvoiceLine.Type.Item;
                    break;
                case 3:
                    TypeValue = WSCreatePreInvoiceLine.Type.G_L_Account;
                    break;
                case 4:
                    TypeValue = WSCreatePreInvoiceLine.Type.Fixed_Asset;
                    break;
                case 5:
                    TypeValue = WSCreatePreInvoiceLine.Type.Charge_Item;
                    break;
                default:
                    TypeValue = WSCreatePreInvoiceLine.Type._blank_;
                    break;

            }

            WSCreatePreInvoiceLine.Create NAVCreate = new WSCreatePreInvoiceLine.Create()
            {
                WsPreInvoiceLine = new WSCreatePreInvoiceLine.WsPreInvoiceLine()
                {
                    Unit_PriceSpecified = true,
                    Unit_Cost_LCYSpecified = true,
                    Document_Type = WSCreatePreInvoiceLine.Document_Type.Invoice,
                    Document_TypeSpecified = true,
                    Document_No = PKey,
                    Type = TypeValue,
                    No = PreInvoiceLineToCreate.Code,
                    Description100 = PreInvoiceLineToCreate.Description,
                    QuantitySpecified = true,
                    Quantity = (int)PreInvoiceLineToCreate.Quantity,
                    TypeSpecified = true,
                    Unit_of_Measure = PreInvoiceLineToCreate.MeasurementUnitCode,
                    Location_Code = PreInvoiceLineToCreate.LocationCode,
                    Unit_Price = (decimal)PreInvoiceLineToCreate.UnitPrice,
                    Unit_Cost_LCY = (decimal)PreInvoiceLineToCreate.UnitCost,
                    Job_Journal_Line_No_Portal = PreInvoiceLineToCreate.LineNo,
                    Job_Journal_Line_No_PortalSpecified = true,
                }
            };

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_PreInvoiceLine_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreatePreInvoiceLine.WsPreInvoiceLine_PortClient WS_Client = new WSCreatePreInvoiceLine.WsPreInvoiceLine_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCreatePreInvoiceLine.Create_Result result = await WS_Client.CreateAsync(NAVCreate);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<WSCreatePreInvoiceLine.CreateMultiple_Result> CreatePreInvoiceLineList(List<LinhasFaturaçãoContrato> LinesList, String HeaderNo, NAVWSConfigurations WSConfigurations)
        {
            int counter = 0;
            WSCreatePreInvoiceLine.WsPreInvoiceLine[] parsedList = LinesList.Select(
               x => new WSCreatePreInvoiceLine.WsPreInvoiceLine
               {
                   
                   Document_No = HeaderNo,
                   Line_No = counter+=10000,
                   Line_NoSpecified = true,
                   Document_Type = WSCreatePreInvoiceLine.Document_Type.Invoice,
                   Document_TypeSpecified = true,
                   No = x.Código,
                   TypeSpecified = true,
                   
                   Type = ConvertToSaleslineType(x.Tipo.Replace(" ", String.Empty)),
                   Description = x.Descrição.Length > 50 ? x.Descrição.Substring(0, 50) : !string.IsNullOrEmpty(x.Descrição) ? x.Descrição : "",
                   Description_2 = x.Descricao2.Length > 50 ? x.Descricao2.Substring(0, 50) : !string.IsNullOrEmpty(x.Descricao2) ? x.Descricao2 : "",
                                            //Quantity = x.Quantidade.Value,
                   Quantity = x.Quantidade.HasValue ? x.Quantidade.Value : 0,
                   QuantitySpecified = true,
                   Unit_of_Measure = x.CódUnidadeMedida,
                   Unit_Price = x.PreçoUnitário.HasValue ? x.PreçoUnitário.Value : 0,
                                            //Unit_Price = x.PreçoUnitário.Value,
                   Unit_PriceSpecified = true,
                                            //Amount = x.ValorVenda.Value,
                                            //AmountSpecified = true,
                   Service_Contract_No = x.NºContrato,
                   Contract_No = x.NºContrato,
                                            //Job_No = x.NºContrato,//Não definir para não obrigar a ter movimentos
                   gJobDimension = x.NºContrato,
                   RegionCode20 = x.CódigoRegião,
                   FunctionAreaCode20 = x.CódigoÁreaFuncional,
                   ResponsabilityCenterCode20 = x.CódigoCentroResponsabilidade
               }).ToArray();

            WSCreatePreInvoiceLine.CreateMultiple NAVCreate = new WSCreatePreInvoiceLine.CreateMultiple(parsedList);

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_PreInvoiceLine_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreatePreInvoiceLine.WsPreInvoiceLine_PortClient WS_Client = new WSCreatePreInvoiceLine.WsPreInvoiceLine_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCreatePreInvoiceLine.CreateMultiple_Result result = await WS_Client.CreateMultipleAsync(NAVCreate);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static async Task<WSCreatePreInvoiceLine.CreateMultiple_Result> CreatePreInvoiceLineListProject(List<SPInvoiceListViewModel> LinesList, String HeaderNo, string OptionInvoice, NAVWSConfigurations WSConfigurations)
        {
            int counter = 0;
            int array = 0;
            WSCreatePreInvoiceLine.WsPreInvoiceLine[] parsedList = new WSCreatePreInvoiceLine.WsPreInvoiceLine[LinesList.Count];
  
            foreach (var x in LinesList)
            {
                TiposRefeição refeicao = DBMealTypes.GetById(x.MealType??0);

                WSCreatePreInvoiceLine.WsPreInvoiceLine line = new WSCreatePreInvoiceLine.WsPreInvoiceLine();
                
                line.Document_Type = OptionInvoice.Replace(" ", String.Empty) == "4" ? WSCreatePreInvoiceLine.Document_Type.Credit_Memo : WSCreatePreInvoiceLine.Document_Type.Invoice;
                line.Document_TypeSpecified = true;
                line.Document_No = HeaderNo;
                line.Type = ConvertInvoiceLineType(x.Type.ToString());
                line.No = x.Code;
                line.Description100 = x.Description;
                line.QuantitySpecified = true;
                line.Quantity = x.Quantity.HasValue ? x.Quantity.Value : 0;
                line.TypeSpecified = true;
                line.Unit_of_Measure = x.MeasurementUnitCode;
                line.Location_Code = x.LocationCode;
                line.Unit_Price = x.UnitPrice.HasValue ? x.UnitPrice.Value : 0;
                line.Unit_PriceSpecified = true;
                line.Unit_Cost_LCY = x.UnitCost.HasValue ? x.UnitCost.Value : 0;
                line.Unit_Cost_LCYSpecified = true;
                line.Line_No = counter += 10000;
                line.Line_NoSpecified = true;
                line.Job_No = x.ProjectNo;
                line.gJobDimension = x.ProjectDimension;
                line.Service_Contract_No = x.ContractNo;
                line.Contract_No_Portal = x.ContractNo;
                line.Contract_No = x.ContractNo;
                line.Tipo_Refeicao = x.MealType.HasValue ? x.MealType.Value.ToString() : string.Empty;// (refeicao!=null) ? refeicao.Código.ToString() : "";
                line.Gen_Prod_Posting_Group = (refeicao != null) ? refeicao.GrupoContabProduto : x.ProjectContabGroup;
                line.Cod_Serv_Cliente = x.ServiceClientCode;
                line.Consumption_Date = !string.IsNullOrEmpty(x.ConsumptionDate) ? DateTime.Parse(x.ConsumptionDate) : DateTime.MinValue;
                line.Consumption_DateSpecified = !string.IsNullOrEmpty(x.ConsumptionDate);

                line.Grupo_Serviço = x.ServiceGroupCode;
                line.Nº_Guia_Externa = x.ExternalGuideNo;
                line.Nº_Guia_Resíduos_GAR = x.WasteGuideNo_GAR;
                line.RegionCode20 = x.RegionCode;
                line.FunctionAreaCode20 = x.FunctionalAreaCode;
                line.ResponsabilityCenterCode20 = x.ResponsabilityCenterCode;
                line.Des_Serv_Cliente = !string.IsNullOrEmpty(x.ServiceClientCode) ? DBServices.GetById(x.ServiceClientCode) != null ? DBServices.GetById(x.ServiceClientCode).Descrição : "" : "";

                line.Data_Registo_Diario = !string.IsNullOrEmpty(x.ConsumptionDate) ? DateTime.Parse(x.ConsumptionDate) : DateTime.MinValue;
                line.Data_Registo_DiarioSpecified = !string.IsNullOrEmpty(x.ConsumptionDate);


                if (x.ResourceType.HasValue)
                {
                    line.Tipo_Recurso = (WSCreatePreInvoiceLine.Tipo_Recurso)x.ResourceType.Value;
                    line.Tipo_RecursoSpecified = true;
                }
                parsedList[array] = line;
                array++;

                //Job_Journal_Line_No_Portal = x.LineNo,
                //Job_Journal_Line_No_PortalSpecified = true,

            };

            WSCreatePreInvoiceLine.CreateMultiple NAVCreate = new WSCreatePreInvoiceLine.CreateMultiple(parsedList);

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_PreInvoiceLine_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreatePreInvoiceLine.WsPreInvoiceLine_PortClient WS_Client = new WSCreatePreInvoiceLine.WsPreInvoiceLine_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            //try
            //{
            return await WS_Client.CreateMultipleAsync(NAVCreate);
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
        }
        
        private static WSCreatePreInvoiceLine.Type ConvertToSaleslineType (string type)
        {
            switch (type)
            {
                case "1":
                    return WSCreatePreInvoiceLine.Type.Item;
                case "2":
                    return WSCreatePreInvoiceLine.Type.Resource;
                case "3":
                    return WSCreatePreInvoiceLine.Type.G_L_Account;
                default:
                    throw new Exception("O tipo selecionado não é válido.");
            }
        }


        //problem with order
        private static WSCreatePreInvoiceLine.Type ConvertInvoiceLineType(string type)
        {
            switch (type)
            {
                case "1":
                    return WSCreatePreInvoiceLine.Type.Item;
                case "2":
                    return WSCreatePreInvoiceLine.Type.Resource;
                case "3":
                    return WSCreatePreInvoiceLine.Type.G_L_Account;
                case "4":
                    return WSCreatePreInvoiceLine.Type.Fixed_Asset;
                case "5":
                    return WSCreatePreInvoiceLine.Type.Charge_Item;
                default:
                    return WSCreatePreInvoiceLine.Type._blank_;
            }
        }

    }
}
