using Hydra.Such.Data.Logic.ComprasML;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data.Logic.PedidoCotacao;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Compras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Hydra.Such.Data.Enumerations;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Portal.Configurations;
using Microsoft.Extensions.Options;

namespace Hydra.Such.Portal.Services
{
    public class GenericResult
    {
        public bool CompletedSuccessfully { get; set; }
        public string ResultValue { get; set; }
        public string ErrorMessage { get; set; }

        public GenericResult()
        {
            this.CompletedSuccessfully = false;
        }
    }

    public class RequisitionService
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations configws;
        private readonly string changedByUserName;

        public RequisitionService(NAVWSConfigurations NAVWSConfigs, string logChangesAsUserName)
        {
            this.configws = NAVWSConfigs;
            this.changedByUserName = logChangesAsUserName;
        }

        public RequisitionService(NAVConfigurations appSettings, NAVWSConfigurations NAVWSConfigs, string logChangesAsUserName)
        {
            _config = appSettings;
            this.configws = NAVWSConfigs;
            this.changedByUserName = logChangesAsUserName;
        }

        public RequisitionViewModel ValidateRequisition(RequisitionViewModel requisition)
        {
            if (requisition != null && requisition.Lines != null && requisition.Lines.Count > 0 && requisition.State == RequisitionStates.Approved)
            {
                var linesToValidate = requisition.Lines
                    .Where(x => x.QuantityRequired != null && x.QuantityRequired.Value > 0)
                    .ToList();

                if (linesToValidate.Count() > 0)
                {
                    requisition.State = RequisitionStates.Validated;
                    requisition.ResponsibleValidation = this.changedByUserName;
                    requisition.ValidationDate = DateTime.Now;
                    requisition.UpdateUser = this.changedByUserName;

                    linesToValidate.ForEach(item =>
                    {
                        item.QuantityToProvide = item.QuantityRequired;
                        item.UpdateUser = this.changedByUserName;
                    });

                    var updatedReq = DBRequest.UpdateHeaderAndLines(requisition.ParseToDB(), true);
                    if (updatedReq != null)
                    {
                        requisition = updatedReq.ParseToViewModel();
                        requisition.eReasonCode = 1;
                        requisition.eMessage = "Requisição validada com sucesso.";
                    }
                    else
                    {
                        requisition.eReasonCode = 3;
                        requisition.eMessage = "Ocorreu um erro ao validar a requisição.";
                    }
                }
                else
                {
                    requisition.eReasonCode = 3;
                    requisition.eMessage = "Não existem linhas que cumpram os requisitos de validação.";
                }
            }
            else
            {
                requisition = new RequisitionViewModel()
                {
                    eReasonCode = 3,
                    eMessage = " O estado da requisição e / ou linhas não cumprem os requisitos de validação.",
                };
            }
            return requisition;
        }

        public RequisitionViewModel ValidateLocalMarketFor(RequisitionViewModel requisition)
        {            
            if (requisition != null && requisition.Lines != null && requisition.Lines.Count > 0 && requisition.State == RequisitionStates.Approved)
            {
                //use for database update later
                var requisitionLines = requisition.Lines
                    .Where(x =>
                        x.LocalMarket != null
                        && x.PurchaseValidated != null
                        && x.QuantityRequired != null
                        && x.LocalMarket.Value
                        && !x.PurchaseValidated.Value
                        && x.QuantityRequired.Value > 0)
                    .ToList();

                List<PurchOrderDTO> purchOrders = new List<PurchOrderDTO>();

                try
                {
                    purchOrders = requisitionLines.GroupBy(x =>
                            x.SupplierNo,
                            x => x,
                            (key, items) => new PurchOrderDTO
                            {
                                SupplierId = key,
                                RequisitionId = requisition.RequisitionNo,
                                CenterResponsibilityCode = requisition.CenterResponsibilityCode,
                                FunctionalAreaCode = requisition.FunctionalAreaCode,
                                RegionCode = requisition.RegionCode,
                                LocalMarketRegion = requisition.LocalMarketRegion,
                                InAdvance = requisition.InAdvance.HasValue ? requisition.InAdvance.Value : false,
                                PricesIncludingVAT = requisition.PricesIncludingVAT.HasValue ? requisition.PricesIncludingVAT.Value : false,
                                Lines = items.Select(line => new PurchOrderLineDTO()
                                {
                                    LineId = line.LineNo.Value,
                                    Type = line.Type,
                                    Code = line.Code,
                                    Description = line.Description,
                                    ProjectNo = line.ProjectNo,
                                    QuantityRequired = line.QuantityRequired,
                                    UnitCost = line.UnitCost,
                                    LocationCode = line.LocalCode,
                                    OpenOrderNo = line.OpenOrderNo,
                                    OpenOrderLineNo = line.OpenOrderLineNo,
                                    CenterResponsibilityCode = line.CenterResponsibilityCode,
                                    FunctionalAreaCode = line.FunctionalAreaCode,
                                    RegionCode = line.RegionCode,
                                    UnitMeasureCode = line.UnitMeasureCode,
                                    VATBusinessPostingGroup = line.VATBusinessPostingGroup,
                                    VATProductPostingGroup = line.VATProductPostingGroup,
                                    DiscountPercentage = line.DiscountPercentage.HasValue ? line.DiscountPercentage.Value : 0,
                                })
                                .ToList()
                            })
                    .ToList();
                }
                catch
                {
                    throw new Exception("Ocorreu um erro ao agrupar as linhas.");
                }

                if (purchOrders.Count() > 0)
                {
                    purchOrders.ForEach(purchOrder =>
                    {
                        try
                        {
                            var result = CreateNAVPurchaseOrderFor(purchOrder, Convert.ToDateTime(requisition.ReceivedDate));
                            if (result.CompletedSuccessfully)
                            {
                                //Update Requisition Lines
                                requisitionLines.ForEach(line =>
                                   line.CreatedOrderNo = result.ResultValue);

                                bool linesUpdated = DBRequestLine.Update(requisitionLines.ParseToDB());
                                if (linesUpdated)
                                {
                                    requisition.eMessages.Add(new TraceInformation(TraceType.Success, "Criada encomenda para o fornecedor núm. " + purchOrder.SupplierId + "; "));
                                }
                            }
                        }
                        catch
                        {
                            requisition.eMessages.Add(new TraceInformation(TraceType.Error, "Ocorreu um erro ao criar encomenda para o fornecedor núm. " + purchOrder.SupplierId + "; "));
                        }
                    });

                    if (requisition.eMessages.Any(x => x.Type == TraceType.Success))
                    {
                        //Refresh lines - Get from db
                        var updatedLines = DBRequestLine.GetByRequisitionId(requisition.RequisitionNo);
                        if (updatedLines != null)
                        {
                            requisition.Lines = updatedLines.ParseToViewModel();
                        }
                    }

                    if (requisition.eMessages.Any(x => x.Type == TraceType.Error))
                    {
                        requisition.eReasonCode = 2;
                        requisition.eMessage = "Ocorram erros ao validar o mercado local.";
                    }
                    else
                    {
                        requisition.eReasonCode = 1;
                        requisition.eMessage = "Mercado local validado com sucesso.";
                    }
                }
                else
                {
                    requisition.eReasonCode = 3;
                    requisition.eMessage = "Não existem linhas que cumpram os requisitos de validação.";
                }
            }
            else
            {
                requisition.eReasonCode = 3;
                requisition.eMessage = "O estado da requisição e / ou linhas não cumprem os requisitos de validação.";
            }
            return requisition;
        }

        public RequisitionViewModel CreatePurchaseOrderFor(RequisitionViewModel requisition)
        {
            if (requisition != null && requisition.Lines != null && requisition.Lines.Count > 0)
            {
                //use for database update later
                var requisitionLines = requisition.Lines;

                //AMARO TESTES COMENTAR
                //if (!string.IsNullOrEmpty(requisition.OrderNo))
                //    throw new Exception("A Encomenda de Compra já foi criada para esta Requisição com o Nº " + requisition.OrderNo);
                //FIM

                if (requisitionLines.Any(x => string.IsNullOrEmpty(x.SupplierNo) || !x.UnitCost.HasValue || x.UnitCost.Value == 0))
                    throw new Exception("É obrigatório o preenchimento do fornecedor e do custo unitário nas linhas");

                //AMARO TESTES DESCOMENTAR
                requisitionLines.RemoveAll(x => (x.CriarNotaEncomenda == null || x.CriarNotaEncomenda == false) && x.CreatedOrderNo != "");
                //FIM

                List<PurchOrderDTO> purchOrders = new List<PurchOrderDTO>();
                List<DBNAV2017SupplierProductRef.SuppliersProductsRefs> supplierProductRef = new List<DBNAV2017SupplierProductRef.SuppliersProductsRefs>();
                try
                {
                    purchOrders = requisitionLines.GroupBy(x =>
                                x.SupplierNo,
                                x => x,
                                (key, items) => new PurchOrderDTO
                                {
                                    SupplierId = key,
                                    RequisitionId = requisition.RequisitionNo,
                                    CenterResponsibilityCode = requisition.CenterResponsibilityCode,
                                    FunctionalAreaCode = requisition.FunctionalAreaCode,
                                    RegionCode = requisition.RegionCode,
                                    LocalMarketRegion = requisition.LocalMarketRegion,
                                    InAdvance = requisition.InAdvance.HasValue ? requisition.InAdvance.Value : false,
                                    PricesIncludingVAT = requisition.PricesIncludingVAT.HasValue ? requisition.PricesIncludingVAT.Value : false,
                                    LocationCode = requisition.LocalCode,
                                    
                                    Lines = items.Select(line => new PurchOrderLineDTO()
                                    {
                                        LineId = line.LineNo,
                                        Type = line.Type,
                                        Code = line.Code,
                                        Description = line.Description,
                                        Description2 = line.Description2,
                                        ProjectNo = line.ProjectNo,
                                        QuantityRequired = line.QuantityRequired,
                                        UnitCost = line.UnitCost,
                                        LocationCode = line.LocalCode,
                                        OpenOrderNo = line.OpenOrderNo,
                                        OpenOrderLineNo = line.OpenOrderLineNo,
                                        CenterResponsibilityCode = line.CenterResponsibilityCode,
                                        FunctionalAreaCode = line.FunctionalAreaCode,
                                        RegionCode = line.RegionCode,
                                        UnitMeasureCode = line.UnitMeasureCode,
                                        VATBusinessPostingGroup = line.VATBusinessPostingGroup,
                                        VATProductPostingGroup = line.VATProductPostingGroup,
                                        DiscountPercentage = line.DiscountPercentage.HasValue ? line.DiscountPercentage.Value : 0,
                                    })
                                    .ToList()
                                })
                        .ToList();

                    supplierProductRef = DBNAV2017SupplierProductRef.GetSuplierProductRefsForRequisition(_config.NAVDatabaseName, _config.NAVCompanyName, requisition.RequisitionNo);
                }
                catch
                {
                    throw new Exception("Ocorreu um erro ao agrupar as linhas.");
                }

                if (purchOrders.Count() > 0)
                {
                    purchOrders.ForEach(purchOrder =>
                    {
                        try
                        {
                            purchOrder.Lines.ForEach(line =>
                                line.SupplierProductCode = supplierProductRef
                                    .Where(x => x.ProductId == line.Code
                                                && x.SupplierNo == purchOrder.SupplierId
                                                && x.UnitOfMeasureCode == line.UnitMeasureCode)
                                    .FirstOrDefault()
                                    ?.SupplierProductId
                            );
                            //var result = CreateNAVPurchaseOrderFor(purchOrder, Convert.ToDateTime(requisition.ReceivedDate), requisition.Comments);
                            var result = CreateNAVPurchaseOrderFor(purchOrder, Convert.ToDateTime(requisition.ReceivedDate));
                            if (result.CompletedSuccessfully)
                            {
                                //Update req
                                requisition.OrderNo = result.ResultValue;

                                //Update Requisition Lines
                                requisition.Lines.ForEach(line =>
                                {
                                    line.CreatedOrderNo = result.ResultValue;
                                    line.UpdateUser = this.changedByUserName;
                                });
                                //Commit to DB
                                var updatedReq = DBRequest.Update(requisition.ParseToDB(), true);
                                //bool linesUpdated = DBRequestLine.Update(requisition.Lines.ParseToDB());
                                //if (linesUpdated)
                                if (updatedReq != null)
                                {
                                    requisition.eMessages.Add(new TraceInformation(TraceType.Success, "Criada encomenda para o fornecedor núm. " + purchOrder.SupplierId + "; "));
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            requisition.eMessages.Add(new TraceInformation(TraceType.Error, "Ocorreu um erro ao criar encomenda para o fornecedor núm. " + purchOrder.SupplierId + ": " + ex.Message));
                            //requisition.eMessages.Add(new TraceInformation(TraceType.Exception, purchOrder.SupplierId + " " + ex.Message));
                        }
                    });

                    if (requisition.eMessages.Any(x => x.Type == TraceType.Success))
                    {
                        //Refresh lines - Get from db
                        var updatedLines = DBRequestLine.GetByRequisitionId(requisition.RequisitionNo);
                        if (updatedLines != null)
                        {
                            requisition.Lines = updatedLines.ParseToViewModel();
                        }
                    }

                    if (requisition.eMessages.Any(x => x.Type == TraceType.Error))
                    {
                        requisition.eReasonCode = 2;
                        requisition.eMessage = "Ocorram erros ao criar encomenda de compra.";
                    }
                    else
                    {
                        requisition.eReasonCode = 1;
                        requisition.eMessage = "Encomenda de compra criada com sucesso.";
                    }
                }
                else
                {
                    requisition.eReasonCode = 3;
                    requisition.eMessage = "Não existem linhas que cumpram os requisitos de validação do mercado local.";
                }
            }
            return requisition;
        }

        public RequisitionViewModel SendPrePurchaseFor(RequisitionViewModel requisition)
        {
            if (requisition != null && requisition.Lines != null && requisition.Lines.Count > 0 && requisition.State == RequisitionStates.Validated)
            {
                //use for later database update
                var requisitionLines = requisition.Lines
                    .Where(x =>
                        x.SendPrePurchase.Value == true  //Enviar Pré Compra
                        && (x.SubmitPrePurchase == null || x.SubmitPrePurchase.Value == false))  //Enviado Pré Compra
                    .ToList();

                var prePurchOrderLines = requisitionLines
                    .Select(line => new PrePurchOrderLineViewModel()
                    {
                        RequisitionNo = line.RequestNo,
                        RequisitionLineNo = line.LineNo,
                        ProductCode = line.Code,
                        ProductDescription = line.Description,
                        UnitOfMeasureCode = line.UnitMeasureCode,
                        LocationCode = line.LocalCode,
                        QuantityAvailable = line.QuantityAvailable,
                        UnitCost = line.UnitCost,
                        ProjectNo = line.ProjectNo,
                        RegionCode = line.RegionCode,
                        FunctionalAreaCode = line.FunctionalAreaCode,
                        CenterResponsibilityCode = line.CenterResponsibilityCode,
                        CreateUser = this.changedByUserName,
                        SupplierNo = line.SupplierNo,
                    })
                    .ToList();

                if (prePurchOrderLines.Count() > 0)
                {
                    bool success = false;
                    try
                    {
                        //Update Requisition Lines
                        requisitionLines.ForEach(line =>
                        {
                            line.SubmitPrePurchase = true;
                            line.UpdateUser = this.changedByUserName;
                        });

                        var createdLines = DBPrePurchOrderLines.CreateAndUpdateReqLines(prePurchOrderLines.ParseToDB(), requisitionLines.ParseToDB());
                        if (createdLines != null)
                        {
                            var updatedLines = DBRequestLine.GetByRequisitionId(requisition.RequisitionNo);
                            if (updatedLines != null)
                            {
                                requisition.Lines = updatedLines.ParseToViewModel();
                            }
                            success = true;
                        }
                    }
                    catch { }

                    if (success)
                    {
                        requisition.eReasonCode = 1;
                        requisition.eMessage = "Pré-Compra enviada com sucesso";
                    }
                    else
                    {
                        requisition.eReasonCode = 2;
                        requisition.eMessage = "Ocorreu um erro ao enviar a Pré-Compra.";
                    }
                }
                else
                {
                    requisition.eReasonCode = 2;
                    requisition.eMessage = " Não existem linhas para enviar.";
                }
            }
            else
            {
                requisition.eReasonCode = 2;
                requisition.eMessage = " O estado da requisição e / ou linhas não cumprem os requisitos.";
            }
            return requisition;
        }

        public RequisitionViewModel CreateMarketConsultFor(RequisitionViewModel requisition)
        {
            try
            {
                //Verificar se pode criar uma consulta de mercado
                if (requisition.Lines.Where(p => p.CreateMarketSearch == true).Where(p => p.QueryCreatedMarketNo == null).Count() <= 0)
                {
                    requisition.eReasonCode = -1;
                    requisition.eMessage = "Consulta ao Mercado não pode ser criada! As linhas devem estar marcadas com 'Criar Consulta Mercado' e não ter 'Nº de Consulta Mercado Criada'";
                    return requisition;
                }

                //Criar nova Consulta Mercado - Obtenção do novo NumConsultaMercado e incrementar Numerações
                ConsultaMercado consultaMercado = DBConsultaMercado.Create(changedByUserName);

                //Actualizar o registo com os dados possiveis
                consultaMercado.CodProjecto = requisition.ProjectNo == "" ? null : requisition.ProjectNo;
                consultaMercado.Descricao = "Consulta Mercado - " + requisition.RequisitionNo;
                consultaMercado.CodRegiao = requisition.RegionCode;
                consultaMercado.CodAreaFuncional = requisition.FunctionalAreaCode;
                consultaMercado.CodCentroResponsabilidade = requisition.CenterResponsibilityCode;
                consultaMercado.DataPedidoCotacao = DateTime.Now;
                consultaMercado.CodLocalizacao = requisition.LocalCode;
                consultaMercado.Destino = 0;
                consultaMercado.Estado = 0;
                consultaMercado.UtilizadorRequisicao = requisition.CreateUser;
                consultaMercado.Fase = 0;
                consultaMercado.Modalidade = 0;
                consultaMercado.PedidoCotacaoCriadoEm = DateTime.Now;
                consultaMercado.PedidoCotacaoCriadoPor = changedByUserName;
                consultaMercado.NumRequisicao = requisition.RequisitionNo;
                consultaMercado.Urgente = requisition.Urgent;

                consultaMercado = DBConsultaMercado.Update(consultaMercado);

                //Para cada linha da requisição
                foreach (RequisitionLineViewModel requisitionLine in requisition.Lines.Where(p => p.CreateMarketSearch == true).Where(p => p.QueryCreatedMarketNo == null))
                {
                    decimal _qty = requisitionLine.QuantityToRequire != null ? requisitionLine.QuantityToRequire.Value : 0;
                    decimal _custo = requisitionLine.UnitCost != null ? requisitionLine.UnitCost.Value : 0;
                    decimal _custoTotalPrev = Math.Round(_qty * _custo * 100) / 100;

                    DateTime? _dataEntrega;

                    try
                    {
                        _dataEntrega = DateTime.Parse(requisitionLine.ExpectedReceivingDate);
                    }
                    catch
                    {
                        _dataEntrega = null;
                    }

                    //Inserir Linhas na tabela "Linhas_Consulta_Mercado"
                    LinhasConsultaMercado linhasConsultaMercado = new LinhasConsultaMercado()
                    {
                        NumConsultaMercado = consultaMercado.NumConsultaMercado,
                        CodProduto = requisitionLine.Code,
                        Descricao = requisitionLine.Description,
                        Descricao2 = requisitionLine.Description2,
                        NumProjecto = requisitionLine.ProjectNo,
                        CodRegiao = requisitionLine.RegionCode,
                        CodAreaFuncional = requisitionLine.FunctionalAreaCode,
                        CodCentroResponsabilidade = requisitionLine.CenterResponsibilityCode,
                        CodLocalizacao = requisitionLine.LocalCode,
                        Quantidade = requisitionLine.QuantityToRequire,
                        CustoUnitarioPrevisto = requisitionLine.UnitCost,
                        CustoTotalPrevisto = _custoTotalPrev,
                        CodUnidadeMedida = requisitionLine.UnitMeasureCode,
                        DataEntregaPrevista = _dataEntrega,
                        NumRequisicao = requisition.RequisitionNo,
                        LinhaRequisicao = requisitionLine.LineNo,
                        CriadoEm = DateTime.Now,
                        CriadoPor = changedByUserName
                    };
                    linhasConsultaMercado = DBConsultaMercado.Create(linhasConsultaMercado);


                    //Verificar se tem Fornecedor identificado
                    if (requisitionLine.SupplierNo != null)
                    {
                        //Verificar se na tabela "Seleccao_Entidades" já temos este Fornecedor para esta Consulta Mercado
                        SeleccaoEntidades seleccaoEntidades = DBConsultaMercado.GetSeleccaoEntidadesPorNumConsultaFornecedor(consultaMercado.NumConsultaMercado, requisitionLine.SupplierNo);

                        if (seleccaoEntidades == null)
                        {
                            seleccaoEntidades = new SeleccaoEntidades() {
                                NumConsultaMercado = consultaMercado.NumConsultaMercado,
                                CodFornecedor = requisitionLine.SupplierNo,
                                NomeFornecedor = !string.IsNullOrEmpty(requisitionLine.SupplierNo) ? DBNAV2017Supplier.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, requisitionLine.SupplierNo).FirstOrDefault().Name : "",
                                Selecionado = true,
                                Preferencial = true
                            };

                            seleccaoEntidades = DBConsultaMercado.Create(seleccaoEntidades);
                        }
                    }

                    requisitionLine.QueryCreatedMarketNo = consultaMercado.NumConsultaMercado;

                    DBRequestLine.Update(DBRequestLine.ParseToDB(requisitionLine));
                }

                requisition.MarketInquiryNo = consultaMercado.NumConsultaMercado;

                Requisição requisição = DBRequest.ParseToDB(requisition);
                DBRequest.Update(requisição);
                requisition = DBRequest.ParseToViewModel(requisição);

                requisition.eReasonCode = 1;
                requisition.eMessage = "Consulta ao Mercado " + consultaMercado.NumConsultaMercado + " criada com sucesso";

            }
            catch (Exception ex)
            {
                requisition.eReasonCode = -1;
                requisition.eMessage = ex.Message;
            }

            return requisition;
        }

        public GenericResult CreateTransferShipmentFor(string requisitionId)
        {
            GenericResult response = new GenericResult();
            RequisitionViewModel requisition = null;

            if (!string.IsNullOrEmpty(requisitionId))
            {
                var tempReq = DBRequest.GetById(requisitionId);
                if (tempReq != null)
                    requisition = tempReq.ParseToViewModel();
            }
            return CreateTransferShipmentFor(requisition);
        }

        public GenericResult CreateTransferShipmentFor(RequisitionViewModel requisition)
        {
            GenericResult response = new GenericResult();

            if (requisition != null && requisition.Lines != null && requisition.Lines.Count > 0)
            {
                try
                {
                    TransferShipment transferShipment = new TransferShipment();
                    transferShipment.ProjectNo = requisition.ProjectNo;
                    transferShipment.Comments = requisition.Comments;
                    transferShipment.FunctionalAreaNo = requisition.FunctionalAreaCode;
                    transferShipment.RequisitionNo = requisition.RequisitionNo;
                    transferShipment.Lines = requisition.Lines.Select(line => new TransferShipmentLine()
                    {
                        ProductNo = line.Code,
                        ProductDescription = line.Description,
                        Quantity = line.QuantityToProvide,
                        UnitOfMeasureNo = line.UnitMeasureCode,
                        UnitCost = line.UnitCost,
                        RegionNo = line.RegionCode,
                        FunctionalAreaNo = line.FunctionalAreaCode,
                        CenterResponsibilityNo = line.CenterResponsibilityCode
                    }).ToList();

                    Task<WSTransferShipmentHeader.Create_Result> createTransferShipHeaderTask = NAVTransferShipmentService.CreateHeaderAsync(transferShipment, configws);
                    createTransferShipHeaderTask.Wait();
                    if (createTransferShipHeaderTask.IsCompletedSuccessfully)
                    {
                        transferShipment.TransferShipmentNo = createTransferShipHeaderTask.Result.WSShipmentDocHeader.Nº_Guia_Transporte;

                        Task<WSTransferShipmentLine.CreateMultiple_Result> createTransferShipLinesTask = NAVTransferShipmentService.CreateLinesAsync(transferShipment, configws);
                        createTransferShipLinesTask.Wait();
                        if (createTransferShipLinesTask.IsCompletedSuccessfully)
                        {
                            Task<WSGenericCodeUnit.FxPostShipmentDoc_Result> createTransferShipDocTask = WSGeneric.CreateTransferShipment(transferShipment.TransferShipmentNo, configws);
                            createTransferShipDocTask.Wait();
                            if (createTransferShipDocTask.IsCompletedSuccessfully)
                            {
                                response.CompletedSuccessfully = true;
                                response.ResultValue = createTransferShipDocTask.Result.return_value;
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    response.ErrorMessage = ex.Message;
                }
            }
            else
            {
                response.ErrorMessage = "A requisição é nula ou não tem linhas.";
            }
            return response;
        }

        //private GenericResult CreateNAVPurchaseOrderFor(PurchOrderDTO purchOrder)
        //{
        //    GenericResult createPrePurchOrderResult = new GenericResult();

        //    if (!string.IsNullOrEmpty(purchOrder.SupplierId) && !string.IsNullOrEmpty(purchOrder.CenterResponsibilityCode))
        //    {
        //        ConfiguraçãoEmailFornecedores ConfigEmailForne = DBConfigEmailFornecedores.GetById(purchOrder.SupplierId, purchOrder.CenterResponsibilityCode);

        //        if (ConfigEmailForne != null && !string.IsNullOrEmpty(ConfigEmailForne.Email))
        //            purchOrder.Vendor_Mail = ConfigEmailForne.Email;
        //    }

        //    Task<WSPurchaseInvHeader.Create_Result> createPurchaseHeaderTask = NAVPurchaseHeaderIntermService.CreateAsync(purchOrder, configws);
        //    createPurchaseHeaderTask.Wait();
        //    if (createPurchaseHeaderTask.IsCompletedSuccessfully)
        //    {
        //        createPrePurchOrderResult.ResultValue = createPurchaseHeaderTask.Result.WSPurchInvHeaderInterm.No;
        //        purchOrder.NAVPrePurchOrderId = createPrePurchOrderResult.ResultValue;

        //        bool createPurchaseLinesTask = NAVPurchaseLineService.CreateAndUpdateMultipleAsync(purchOrder, configws);
        //        if (createPurchaseLinesTask)
        //        {
        //            try
        //            {
        //                /*
        //                 *  Swallow errors at this stage as they will be managed in NAV
        //                 */
        //                //Task<WSGenericCodeUnit.FxCabimento_Result> createPurchOrderTask = WSGeneric.CreatePurchaseOrder(purchOrder.NAVPrePurchOrderId, configws);
        //                //createPurchOrderTask.Start();
        //                ////if (createPurchOrderTask.IsCompletedSuccessfully)
        //                ////{
        //                ////    createPrePurchOrderResult.CompletedSuccessfully = true;
        //                ////}
        //            }
        //            catch (Exception ex) { }
        //            createPrePurchOrderResult.CompletedSuccessfully = true;
        //        }
        //    }
        //    return createPrePurchOrderResult;
        //}

        private GenericResult CreateNAVPurchaseOrderFor(PurchOrderDTO purchOrder, DateTime DataRececao)
        {
            GenericResult createPrePurchOrderResult = new GenericResult();

            if (!string.IsNullOrEmpty(purchOrder.SupplierId) && !string.IsNullOrEmpty(purchOrder.CenterResponsibilityCode))
            {
                ConfiguraçãoEmailFornecedores ConfigEmailForne = DBConfigEmailFornecedores.GetById(purchOrder.SupplierId, purchOrder.CenterResponsibilityCode);

                if (ConfigEmailForne != null && !string.IsNullOrEmpty(ConfigEmailForne.Email))
                    purchOrder.Vendor_Mail = ConfigEmailForne.Email;
            }

            Task<WSPurchaseInvHeader.Create_Result> createPurchaseHeaderTask = NAVPurchaseHeaderIntermService.CreateAsync(purchOrder, configws, DataRececao);
            createPurchaseHeaderTask.Wait();
            if (createPurchaseHeaderTask.IsCompletedSuccessfully)
            {
                createPrePurchOrderResult.ResultValue = createPurchaseHeaderTask.Result.WSPurchInvHeaderInterm.No;
                purchOrder.NAVPrePurchOrderId = createPrePurchOrderResult.ResultValue;

                bool createPurchaseLinesTask = NAVPurchaseLineService.CreateAndUpdateMultipleAsync(purchOrder, configws);
                if (createPurchaseLinesTask)
                {
                    try
                    {
                        /*
                         *  Swallow errors at this stage as they will be managed in NAV
                         */
                        //Task<WSGenericCodeUnit.FxCabimento_Result> createPurchOrderTask = WSGeneric.CreatePurchaseOrder(purchOrder.NAVPrePurchOrderId, configws);
                        //createPurchOrderTask.Start();
                        ////if (createPurchOrderTask.IsCompletedSuccessfully)
                        ////{
                        ////    createPrePurchOrderResult.CompletedSuccessfully = true;
                        ////}
                    }
                    catch (Exception ex) { }
                    createPrePurchOrderResult.CompletedSuccessfully = true;
                }
            }
            return createPrePurchOrderResult;
        }
    }
}
