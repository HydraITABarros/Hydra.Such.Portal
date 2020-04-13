using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Contracts;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Contracts;
using Hydra.Such.Data.ViewModel.Projects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Hydra.Such.Data.NAV
{
    public static class WSPreInvoice
    {
        static BasicHttpBinding navWSBinding;

        static WSPreInvoice()
        {
            ///Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
        }
        
        public static async Task<WSCreatePreInvoice.Create_Result> CreatePreInvoice(SPInvoiceListViewModel preInvoiceToCreate, NAVWSConfigurations WSConfigurations)
        {
            WSCreatePreInvoice.Document_Type tipo;
            bool notaDebito=false;
            string PostingNoSeries = "";

            ConfigUtilizadores CUsers = DBUserConfigurations.GetById(preInvoiceToCreate.CreateUser);

            if (preInvoiceToCreate.MovementType == 2)//Nota de débito
            {
                tipo = WSCreatePreInvoice.Document_Type.Invoice;
                notaDebito = true;
                PostingNoSeries = CUsers.NumSerieNotasDebito;
                

            }
            else if (preInvoiceToCreate.MovementType == 4)//Nota de crédito
            {
                tipo = WSCreatePreInvoice.Document_Type.Credit_Memo;
                notaDebito = false;
                PostingNoSeries = CUsers.NumSerieNotasCredito;
            }
            else// Fatura
            {
                tipo = WSCreatePreInvoice.Document_Type.Invoice;
                notaDebito = false;
                PostingNoSeries = CUsers.NumSerieFaturas;
            }

            string username = preInvoiceToCreate.CreateUser.ToUpper();
            int index = username.IndexOf("@");
            if (index > 0)
                username = username.Substring(0, index);

            WSCreatePreInvoice.Create NAVCreate = new WSCreatePreInvoice.Create()
            {
                WSPreInvoice = new WSCreatePreInvoice.WSPreInvoice() {

                    Document_Type = tipo,
                    Document_TypeSpecified = true,
                    Sell_to_Customer_No = preInvoiceToCreate.InvoiceToClientNo,
                    VAT_Registration_No = preInvoiceToCreate.ClientVATReg,
                    Contract_No = preInvoiceToCreate.ContractNo,
                    Debit_Memo = notaDebito,
                    Debit_MemoSpecified = true,
                    Posting_No_Series = PostingNoSeries,
                    Codigo_Pedido = preInvoiceToCreate.ClientRequest,
                    Currency_Code = preInvoiceToCreate.Currency,
                    Data_Serv_Prestado = preInvoiceToCreate.ServiceDate,
                    Data_Encomenda = !string.IsNullOrEmpty(preInvoiceToCreate.DataPedido) ? DateTime.Parse(preInvoiceToCreate.DataPedido) : DateTime.MinValue,
                    Data_EncomendaSpecified = !string.IsNullOrEmpty(preInvoiceToCreate.DataPedido),
                    //Document_Date = preInvoiceToCreate.dat
                    //Due_Date
                    //Document_Date
                    //External_Document_No
                    RegionCode20 = preInvoiceToCreate.RegionCode,
                    FunctionAreaCode20 = preInvoiceToCreate.FunctionalAreaCode,
                    ResponsabilityCenterCode20 = preInvoiceToCreate.ResponsabilityCenterCode,
                    Location_Code = preInvoiceToCreate.LocationCode,
                    No_Compromisso = preInvoiceToCreate.CommitmentNumber,
                    Observacoes = !string.IsNullOrEmpty(preInvoiceToCreate.Comments) ? preInvoiceToCreate.Comments : "",
                    Observações_1 = !string.IsNullOrEmpty(preInvoiceToCreate.Comments2) ? preInvoiceToCreate.Comments2 : "",
                    Responsibility_Center = CUsers.CentroDeResponsabilidade,
                    //Order_Date
                    Payment_Method_Code = preInvoiceToCreate.CodMetodoPagamento,
                    Payment_Terms_Code = preInvoiceToCreate.CodTermosPagamento,
                    //Posting_Date
                    Posting_Date = !string.IsNullOrEmpty(preInvoiceToCreate.Posting_Date.ToString()) ? DateTime.Parse(preInvoiceToCreate.Posting_Date.ToString()) : DateTime.MinValue,
                    Posting_DateSpecified = !string.IsNullOrEmpty(preInvoiceToCreate.Posting_Date.ToString()),
                    Document_Date = !string.IsNullOrEmpty(preInvoiceToCreate.Posting_Date.ToString()) ? DateTime.Parse(preInvoiceToCreate.Posting_Date.ToString()) : DateTime.MinValue,
                    Document_DateSpecified = !string.IsNullOrEmpty(preInvoiceToCreate.Posting_Date.ToString()),
                    External_Document_No = preInvoiceToCreate.ProjectNo,
                    Grupo_Fatura = (int)preInvoiceToCreate.InvoiceGroup,
                    Grupo_FaturaSpecified = true,

                    User_pre_registo_2009 = username,

                    Ship_to_Address = preInvoiceToCreate.Ship_to_Address,
                    Ship_to_Address_2 = preInvoiceToCreate.Ship_to_Address_2,
                    Ship_to_City = preInvoiceToCreate.Ship_to_City,
                    Ship_to_Code = preInvoiceToCreate.Ship_to_Code,
                    Ship_to_Contact = preInvoiceToCreate.Ship_to_Contact,
                    Ship_to_Country_Region_Code = preInvoiceToCreate.Ship_to_Country_Region_Code,
                    Ship_to_County = preInvoiceToCreate.Ship_to_County,
                    Ship_to_Name = preInvoiceToCreate.Ship_to_Name,
                    Ship_to_Name_2 = preInvoiceToCreate.Ship_to_Name_2,
                    Ship_to_Post_Code = preInvoiceToCreate.Ship_to_Post_Code,

                    Prices_Including_VAT = preInvoiceToCreate.FaturaPrecosIvaIncluido.HasValue ? (bool)preInvoiceToCreate.FaturaPrecosIvaIncluido : false,
                    Prices_Including_VATSpecified = true
                }

            };

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_PreInvoice_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreatePreInvoice.WSPreInvoice_PortClient WS_Client = new WSCreatePreInvoice.WSPreInvoice_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);


            return await WS_Client.CreateAsync(NAVCreate);
            
        }

        public static async Task<WSCreatePreInvoice.Create_Result> CreatePreInvoice(AuthorizedCustomerBillingHeader billingHeader, NAVWSConfigurations WSConfigurations)
        {
            SPInvoiceListViewModel invoiceHeader = new SPInvoiceListViewModel();
            invoiceHeader.InvoiceToClientNo = billingHeader.InvoiceToClientNo;
            invoiceHeader.Date = billingHeader.Date;
            invoiceHeader.CommitmentNumber = billingHeader.CommitmentNumber;
            invoiceHeader.ClientRequest = billingHeader.ClientRequest;
            invoiceHeader.ClientVATReg = billingHeader.ClientVATReg;
            invoiceHeader.ContractNo = billingHeader.ContractNo;
            invoiceHeader.Currency = billingHeader.Currency;
            invoiceHeader.ServiceDate = billingHeader.ServiceDate;
            invoiceHeader.UpdateDate = billingHeader.UpdateDate;
            invoiceHeader.RegionCode = billingHeader.RegionCode;
            invoiceHeader.FunctionalAreaCode = billingHeader.FunctionalAreaCode;
            invoiceHeader.ResponsabilityCenterCode = billingHeader.ResponsabilityCenterCode;
            invoiceHeader.LocationCode = billingHeader.LocationCode;
            invoiceHeader.Comments = billingHeader.Comments;
            invoiceHeader.CodTermosPagamento = billingHeader.CodTermosPagamento;
            invoiceHeader.CodMetodoPagamento = billingHeader.CodMetodoPagamento;
            invoiceHeader.CreateUser = billingHeader.CreateUser;
            invoiceHeader.FaturaPrecosIvaIncluido = billingHeader.FaturaPrecosIvaIncluido.HasValue ? (bool)billingHeader.FaturaPrecosIvaIncluido : false;

            return await CreatePreInvoice(invoiceHeader, WSConfigurations);

        }

        public static async Task<WSCreatePreInvoice.Create_Result> CreatePreInvoice(AuthorizedCustomerBillingHeader billingHeader, NAVWSConfigurations WSConfigurations, string dataFormulario, string projeto, SPInvoiceListViewModel Ship, int GrupoFatura)
        {
            string Observacoes = billingHeader.Comments;
            string Observacoes1 = billingHeader.Comments;
            string Observacoes2 = "";
            int ultimoSpace = 0;
            int tamanhoFinal = 0;

            if (Observacoes.Length > 250)
            {
                Observacoes1 = Observacoes.Substring(0, 250);
                ultimoSpace = Observacoes1.LastIndexOf(' ');

                Observacoes1 = Observacoes.Substring(0, ultimoSpace);

                if ((Observacoes.Length - ultimoSpace) > 250)
                    tamanhoFinal = 250;
                else
                    tamanhoFinal = Observacoes.Length - ultimoSpace;

                Observacoes2 = Observacoes.Substring(ultimoSpace + 1, tamanhoFinal);
            }


            SPInvoiceListViewModel invoiceHeader = new SPInvoiceListViewModel();
            invoiceHeader.InvoiceToClientNo = billingHeader.InvoiceToClientNo;
            invoiceHeader.Date = billingHeader.Date;
            invoiceHeader.DataPedido = billingHeader.DataPedido;
            invoiceHeader.CommitmentNumber = billingHeader.CommitmentNumber;
            invoiceHeader.ClientRequest = billingHeader.ClientRequest;
            invoiceHeader.ClientVATReg = billingHeader.ClientVATReg;
            invoiceHeader.ContractNo = billingHeader.ContractNo;
            invoiceHeader.Currency = billingHeader.Currency;
            invoiceHeader.ServiceDate = billingHeader.ServiceDate;
            invoiceHeader.UpdateDate = billingHeader.UpdateDate;
            invoiceHeader.RegionCode = billingHeader.RegionCode;
            invoiceHeader.FunctionalAreaCode = billingHeader.FunctionalAreaCode;
            invoiceHeader.ResponsabilityCenterCode = billingHeader.ResponsabilityCenterCode;
            invoiceHeader.LocationCode = billingHeader.LocationCode;
            invoiceHeader.Comments = !string.IsNullOrEmpty(Observacoes1) ? Observacoes1 : "";
            invoiceHeader.Comments2 = !string.IsNullOrEmpty(Observacoes2) ? Observacoes2 : "";
            invoiceHeader.CodTermosPagamento = billingHeader.CodTermosPagamento;
            invoiceHeader.CodMetodoPagamento = billingHeader.CodMetodoPagamento;
            invoiceHeader.CreateUser = billingHeader.CreateUser;
            invoiceHeader.Posting_Date = Convert.ToDateTime(dataFormulario);
            invoiceHeader.ProjectNo = projeto;
            invoiceHeader.MovementType = billingHeader.MovementType;
            invoiceHeader.InvoiceGroup = GrupoFatura;

            invoiceHeader.Ship_to_Code = Ship.Ship_to_Code;
            //invoiceHeader.Ship_to_Address = Ship.Ship_to_Address;
            //invoiceHeader.Ship_to_Address_2 = Ship.Ship_to_Address_2;
            //invoiceHeader.Ship_to_City = Ship.Ship_to_City;
            //invoiceHeader.Ship_to_Contact = Ship.Ship_to_Contact;
            //invoiceHeader.Ship_to_Country_Region_Code = Ship.Ship_to_Country_Region_Code;
            //invoiceHeader.Ship_to_County = Ship.Ship_to_County;
            //invoiceHeader.Ship_to_Name = Ship.Ship_to_Name;
            //invoiceHeader.Ship_to_Name_2 = Ship.Ship_to_Name_2;
            //invoiceHeader.Ship_to_Post_Code = Ship.Ship_to_Post_Code;

            invoiceHeader.FaturaPrecosIvaIncluido = billingHeader.FaturaPrecosIvaIncluido.HasValue ? (bool)billingHeader.FaturaPrecosIvaIncluido : false;

            return await CreatePreInvoice(invoiceHeader, WSConfigurations);

        }

        public static async Task<WSCreatePreInvoice.Create_Result> CreatePreInvoiceHeader(NAVSalesHeaderViewModel PreInvoiceToCreate, NAVWSConfigurations WSConfigurations) //, int GrupoFatura)
        {
            string Observacoes = !string.IsNullOrEmpty(PreInvoiceToCreate.Observacoes) ? PreInvoiceToCreate.Observacoes : "";
            string Observacoes1 = !string.IsNullOrEmpty(PreInvoiceToCreate.Observacoes) ? PreInvoiceToCreate.Observacoes : "";
            string Observacoes2 = "";
            int ultimoSpace = 0;
            int tamanhoFinal = 0;

            if (!string.IsNullOrEmpty(Observacoes) && Observacoes.Length > 250)
            {
                Observacoes1 = Observacoes.Substring(0, 250);
                ultimoSpace = Observacoes1.LastIndexOf(' ');

                Observacoes1 = Observacoes.Substring(0, ultimoSpace);

                if ((Observacoes.Length - ultimoSpace) > 250)
                    tamanhoFinal = 250;
                else
                    tamanhoFinal = Observacoes.Length - ultimoSpace;

                Observacoes2 = Observacoes.Substring(ultimoSpace + 1, tamanhoFinal);
            }

            WSCreatePreInvoice.Create NAVCreate = new WSCreatePreInvoice.Create()
            {

                WSPreInvoice = new WSCreatePreInvoice.WSPreInvoice()
                {
                    Sell_to_Customer_No = PreInvoiceToCreate.Sell_toCustomerNo,
                    Document_Date = PreInvoiceToCreate.DocumentDate,
                    Shipment_Date = PreInvoiceToCreate.ShipmentDate,
                    Periodo_de_Fact_Contrato = PreInvoiceToCreate.PeriododeFact_Contrato,
                    Valor_Contrato = PreInvoiceToCreate.ValorContrato,

                    Ship_to_Code = PreInvoiceToCreate.Ship_toCode,
                    //Ship_to_Address = PreInvoiceToCreate.Ship_toAddress.Length >= 50 ? PreInvoiceToCreate.Ship_toAddress.Substring(0, 49) : PreInvoiceToCreate.Ship_toAddress,
                    //Ship_to_Address_2 = PreInvoiceToCreate.Ship_toAddress2,
                    //Ship_to_City = PreInvoiceToCreate.Ship_toCity,
                    //Ship_to_Contact = PreInvoiceToCreate.Ship_toContact,
                    //Ship_to_Country_Region_Code = PreInvoiceToCreate.Ship_toCountryRegionCode,
                    //Ship_to_County = PreInvoiceToCreate.Ship_toCounty,
                    //Ship_to_Name = PreInvoiceToCreate.Ship_toName,
                    //Ship_to_Name_2 = PreInvoiceToCreate.Ship_toName2,
                    //Ship_to_Post_Code = PreInvoiceToCreate.Ship_toPostCode,

                    Currency_Code = PreInvoiceToCreate.CurrencyCode,
                    Due_Date = PreInvoiceToCreate.DueDate,
                    Payment_Terms_Code = PreInvoiceToCreate.PaymentTermsCode,
                    Payment_Method_Code = PreInvoiceToCreate.PaymentMethodCode,
                    Responsibility_Center = PreInvoiceToCreate.ResponsibilityCenter,
                    Posting_No_Series = PreInvoiceToCreate.PostingNoSeries,
                    No_Compromisso = PreInvoiceToCreate.No_Compromisso,
                    Codigo_Pedido = PreInvoiceToCreate.CodigoPedido,
                    Data_Encomenda = PreInvoiceToCreate.DataEncomenda,
                    Data_Serv_Prestado = PreInvoiceToCreate.DataServ_Prestado,
                    Observacoes = Observacoes1,
                    Observações_1 = Observacoes2,
                    Contract_No = PreInvoiceToCreate.ContractNo,
                    Document_Type = WSCreatePreInvoice.Document_Type.Invoice,
                    Factura_CAF= PreInvoiceToCreate.FacturaCAF,
                    User_pre_registo_2009=PreInvoiceToCreate.Userpreregisto2009,
                    Posting_Date= PreInvoiceToCreate.PostingDate,                   
                    //Grupo_Fatura= GrupoFatura,
                    Document_TypeSpecified = true,
                    Posting_DateSpecified=true,
                    Shipment_DateSpecified=true,
                    Shipment_Start_TimeSpecified=true,
                    ReportID_OriginalSpecified=true,
                    Valor_ContratoSpecified=true,
                    Document_DateSpecified=true,
                    Factura_CAFSpecified=true,
                    Due_DateSpecified=true,
                    Order_DateSpecified=true,
                    Data_EncomendaSpecified=true,
                    CommentSpecified=true,
                    Debit_MemoSpecified=true,
                    //Grupo_FaturaSpecified=true,
                    
                    //Dimensions
                    ResponsabilityCenterCode20 = PreInvoiceToCreate.ResponsabilityCenterCode20,
                    FunctionAreaCode20 = PreInvoiceToCreate.FunctionAreaCode20,
                    RegionCode20 = PreInvoiceToCreate.RegionCode20,

                    Prices_Including_VAT = PreInvoiceToCreate.PricesIncludingVAT == 1 ? true : false,
                    Prices_Including_VATSpecified = true
                }
            };

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_PreInvoice_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreatePreInvoice.WSPreInvoice_PortClient WS_Client = new WSCreatePreInvoice.WSPreInvoice_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCreatePreInvoice.Create_Result result = await WS_Client.CreateAsync(NAVCreate);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<WSCreatePreInvoice.Create_Result> CreateContractInvoice(AutorizarFaturaçãoContratos CreateInvoice, NAVWSConfigurations WSConfigurations, string ContractInvoicePeriod, string InvoiceBorrowed, string CodTermosPagamento, string MetdoPagamento, bool PricesIncludingVAT, string Ship_to_Code)
        {
            DateTime now = DateTime.Now;
            string PostingNoSeries = "";
            string Observacoes = "";
            string Mes = InvoiceBorrowed.Substring(0, InvoiceBorrowed.IndexOf("/"));
            string Ano = InvoiceBorrowed.Substring(InvoiceBorrowed.IndexOf("/") + 1, 4);
            ConfigUtilizadores CUsers = DBUserConfigurations.GetById(CreateInvoice.UtilizadorCriação);
            Contratos Contrato = DBContracts.GetByIdLastVersion(CreateInvoice.NºContrato);
            WSCreatePreInvoice.Create_Result result = new WSCreatePreInvoice.Create_Result();


            if (Contrato != null && Contrato.TipoContrato == 3 && Contrato.Tipo == 3) //Contrato Quotas
            {
                ConfiguracaoParametros Parametro = DBConfiguracaoParametros.GetByParametro("QuotasNoSeries");

                if (Parametro != null && !string.IsNullOrEmpty(Parametro.Valor))
                    PostingNoSeries = Parametro.Valor;

                if (Contrato != null && !string.IsNullOrEmpty(Contrato.TextoFatura))
                {
                    Observacoes = Contrato.TextoFatura;
                    Observacoes = Observacoes.Replace("<MES>", Mes);
                    Observacoes = Observacoes.Replace("<ANO>", Ano);
                }
            }
            else
            {
                PostingNoSeries = !string.IsNullOrEmpty(CUsers.NumSerieFaturas) ? CUsers.NumSerieFaturas : "";
                Observacoes = !string.IsNullOrEmpty(CreateInvoice.Descrição) ? CreateInvoice.Descrição : "";
            }

            WSCreatePreInvoice.Create NAVCreate = new WSCreatePreInvoice.Create()
            {
                WSPreInvoice = new WSCreatePreInvoice.WSPreInvoice()
                {
                    Sell_to_Customer_No = !string.IsNullOrEmpty(CreateInvoice.NºCliente) ? CreateInvoice.NºCliente : "",
                    Document_Date = DateTime.Today,
                    Document_DateSpecified = true,
                    Shipment_Date = now,
                    Shipment_DateSpecified = true,
                    Shipment_Start_Time = now.AddHours(1),
                    Shipment_Start_TimeSpecified = true,
                    Document_Type = WSCreatePreInvoice.Document_Type.Invoice,
                    Document_TypeSpecified = true,
                    Posting_Date = CreateInvoice.DataDeRegisto ?? DateTime.Now,
                    Posting_DateSpecified = true,
                    Periodo_de_Fact_Contrato = !string.IsNullOrEmpty(ContractInvoicePeriod) ? ContractInvoicePeriod : "",
                    Data_Serv_Prestado = !string.IsNullOrEmpty(InvoiceBorrowed) ? InvoiceBorrowed : "",
                    Responsibility_Center = !string.IsNullOrEmpty(CUsers.CentroDeResponsabilidade) ? CUsers.CentroDeResponsabilidade : "",

                    Posting_No_Series = PostingNoSeries,
                    Due_Date = (DateTime)CreateInvoice.DataDeExpiração,
                    Due_DateSpecified = true,
                    Payment_Terms_Code = CodTermosPagamento,

                    //Amaro
                    Observacoes = Observacoes,
                    Contract_No = !string.IsNullOrEmpty(CreateInvoice.NºContrato) ? CreateInvoice.NºContrato : "",
                    Factura_CAF = true,
                    Factura_CAFSpecified = true,
                    Codigo_Pedido = !string.IsNullOrEmpty(CreateInvoice.NoRequisicaoDoCliente) ? CreateInvoice.NoRequisicaoDoCliente : "",
                    No_Compromisso = !string.IsNullOrEmpty(CreateInvoice.NoCompromisso) ? CreateInvoice.NoCompromisso : "",
                    Data_Encomenda = CreateInvoice.DataRececaoRequisicao ?? DateTime.MinValue,
                    Data_EncomendaSpecified = true,

                    RegionCode20 = !string.IsNullOrEmpty(CreateInvoice.CódigoRegião) ? CreateInvoice.CódigoRegião : "",
                    FunctionAreaCode20 = !string.IsNullOrEmpty(CreateInvoice.CódigoÁreaFuncional) ? CreateInvoice.CódigoÁreaFuncional : "",
                    ResponsabilityCenterCode20 = !string.IsNullOrEmpty(CreateInvoice.CódigoCentroResponsabilidade) ? CreateInvoice.CódigoCentroResponsabilidade : "",

                    Prices_Including_VAT = PricesIncludingVAT,
                    Prices_Including_VATSpecified = true,

                    Ship_to_Code = !string.IsNullOrEmpty(Ship_to_Code) ? Ship_to_Code : "",

                    //Contratos Quotas
                    Payment_Method_Code = MetdoPagamento
                }
            };

            // Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_PreInvoice_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreatePreInvoice.WSPreInvoice_PortClient WS_Client = new WSCreatePreInvoice.WSPreInvoice_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                result = await WS_Client.CreateAsync(NAVCreate);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<WSCreatePreInvoice.CreateMultiple_Result> CreateMultipleContractInvoice(List<AutorizarFaturaçãoContratos> CreateList, NAVWSConfigurations WSConfigurations)
        {
            WSCreatePreInvoice.WSPreInvoice[] parsedList = CreateList.Select(
                x => new WSCreatePreInvoice.WSPreInvoice
                {
                    Sell_to_Customer_No = x.NºCliente,
                    Document_Date = x.DataDeRegisto.Value,
                    Due_Date = x.DataDeExpiração.Value
                }).ToArray();

            WSCreatePreInvoice.CreateMultiple NAVCreate = new WSCreatePreInvoice.CreateMultiple(parsedList);


            // Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_PreInvoice_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreatePreInvoice.WSPreInvoice_PortClient WS_Client = new WSCreatePreInvoice.WSPreInvoice_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCreatePreInvoice.CreateMultiple_Result result = await WS_Client.CreateMultipleAsync(NAVCreate);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<WSCreatePreInvoice.Delete_Result> DeletePreInvoiceLineList(String HeaderNo, NAVWSConfigurations WSConfigurations)
        {

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_PreInvoiceLine_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreatePreInvoice.WSPreInvoice_PortClient WS_Client = new WSCreatePreInvoice.WSPreInvoice_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCreatePreInvoice.Delete_Result result = await WS_Client.DeleteAsync(HeaderNo);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<WSSuchNav2017.WSgetNumPreRegisto_Result> GetPreInvoice(string CodProjeto, int GrupoFactura, NAVWSConfigurations WSConfigurations)
        {

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.Ws_SuchNav2017_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSSuchNav2017.WSNAV2017_PortClient WS_Client = new WSSuchNav2017.WSNAV2017_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSSuchNav2017.WSgetNumPreRegisto_Result result = await WS_Client.WSgetNumPreRegistoAsync(CodProjeto, GrupoFactura);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<WSCreatePreInvoice.Read_Result> GetNavPreInvoice(string PreInvoice, string DocumentType, NAVWSConfigurations WSConfigurations)
        {

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_PreInvoice_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreatePreInvoice.WSPreInvoice_PortClient WS_Client = new WSCreatePreInvoice.WSPreInvoice_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCreatePreInvoice.Read_Result result = await WS_Client.ReadAsync(DocumentType, PreInvoice);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<WSCreatePreInvoice.Delete_Result> DeletePreInvoice(string PreInvoiceKey, NAVWSConfigurations WSConfigurations)
        {
            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_PreInvoice_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreatePreInvoice.WSPreInvoice_PortClient WS_Client = new WSCreatePreInvoice.WSPreInvoice_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCreatePreInvoice.Delete_Result result = await WS_Client.DeleteAsync(PreInvoiceKey);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
