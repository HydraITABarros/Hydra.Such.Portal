﻿using Hydra.Such.Data.Logic.ComprasML;
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
using Hydra.Such.Data.Logic.Nutrition;

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
            requisition.eReasonCode = 99;
            requisition.eMessage = "Ocorreu um erro na Validação da Requisição.";

            try
            {
                if (requisition != null)
                {
                    if (requisition.State == RequisitionStates.Approved)
                    {
                        if (requisition.Lines != null && requisition.Lines.Count > 0)
                        {
                            var linesToValidate = requisition.Lines
                                .Where(x => x.QuantityRequired != null && x.QuantityRequired.HasValue && x.QuantityRequired.Value > 0)
                                .ToList();

                            if (linesToValidate != null && linesToValidate.Count > 0)
                            {
                                linesToValidate.ForEach(item =>
                                {
                                    item.QuantityToProvide = item.QuantityRequired; // QuantidadeADisponibilizar = QuantidadeRequerida
                                    item.UpdateUser = this.changedByUserName;
                                    item.UpdateDateTime = DateTime.Now;

                                    if (DBRequestLine.Update(item.ParseToDB()) == null)
                                    {
                                        requisition.eReasonCode = 2;
                                        requisition.eMessage = "Ocorreu um erro ao atualizar as linhas na Validação da Requisição.";
                                    }
                                });

                                if (requisition.eReasonCode == 99)
                                {
                                    requisition.State = RequisitionStates.Validated;
                                    requisition.ResponsibleValidation = this.changedByUserName;
                                    requisition.ValidationDate = DateTime.Now;
                                    requisition.UpdateUser = this.changedByUserName;

                                    //SISLOG
                                    if (requisition.TipoReq != null && requisition.TipoReq == 0)
                                    {
                                        if (!string.IsNullOrEmpty(requisition.LocalCode) && requisition.LocalCode == "4300")
                                        {
                                            if (requisition.StockReplacement == null || requisition.StockReplacement == false)
                                            {
                                                requisition.TipoAlteracaoSISLOG = 1;
                                                requisition.DataAlteracaoSISLOG = DateTime.Now;
                                                requisition.EnviarSISLOG = true;
                                                requisition.SISLOG = false;
                                            }
                                        }
                                    }

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
                            }
                            else
                            {
                                requisition.eReasonCode = 4;
                                requisition.eMessage = "Não existem linhas com Qt. Requerida superior a zero.";
                            }
                        }
                        else
                        {
                            requisition.eReasonCode = 5;
                            requisition.eMessage = "Não existem linhas para validar na Requisição.";
                        }
                    }
                    else
                    {
                        requisition.eReasonCode = 6;
                        requisition.eMessage = "A Requisição não está no estado Aprovado.";
                    }
                }
                else
                {
                    requisition = new RequisitionViewModel()
                    {
                        eReasonCode = 7,
                        eMessage = "Erro na obtenção da Requisição.",
                    };
                }
            }
            catch
            {
                requisition.eReasonCode = 99;
                requisition.eMessage = "Ocorreu um erro na Validação da Requisição.";
            };

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
                                Purchaser_Code = requisition.NumeroMecanografico,

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
                if (string.IsNullOrEmpty(requisition.ReceivedDate))
                {
                    requisition.eReasonCode = 4;
                    requisition.eMessage = "É obrigatório o preenchimento do campo Data Receção no Geral.";
                    return requisition;
                }

                //use for database update later
                var requisitionLines = requisition.Lines;

                requisitionLines.RemoveAll(x => x.CriarNotaEncomenda == null || x.CriarNotaEncomenda == false);
                requisitionLines.RemoveAll(x => x.CreatedOrderNo != "" && x.CreatedOrderNo != null);
                //FIM

                if (requisitionLines.Any(x => string.IsNullOrEmpty(x.SupplierNo) || !x.UnitCost.HasValue || x.UnitCost.Value == 0 || string.IsNullOrEmpty(x.VATBusinessPostingGroup)))
                    throw new Exception("É obrigatório o preenchimento do Fornecedor, do Custo Unitário e do Grupo Registo IVA Negócio nas linhas");

                if (!string.IsNullOrEmpty(requisition.ProjectNo) && (string.IsNullOrEmpty(requisition.RegionCode) || string.IsNullOrEmpty(requisition.FunctionalAreaCode) || string.IsNullOrEmpty(requisition.CenterResponsibilityCode)))
                {
                    NAVProjectsViewModel REQProject = DBNAV2017Projects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, requisition.ProjectNo).FirstOrDefault();
                    if (REQProject != null)
                    {
                        requisition.RegionCode = REQProject.RegionCode;
                        requisition.FunctionalAreaCode = REQProject.AreaCode;
                        requisition.CenterResponsibilityCode = REQProject.CenterResponsibilityCode;
                    }
                }

                List<PurchOrderDTO> purchOrders = new List<PurchOrderDTO>();
                List<DBNAV2017SupplierProductRef.SuppliersProductsRefs> supplierProductRef = new List<DBNAV2017SupplierProductRef.SuppliersProductsRefs>();

                requisitionLines.Where(x => Convert.IsDBNull(x.SubSupplierNo)).ToList().ForEach(line => line.SubSupplierNo = "");

                try
                {
                    purchOrders = requisitionLines.GroupBy(x => new
                    { x.SupplierNo, x.SubSupplierNo },
                        x => x,
                        (key, items) => new PurchOrderDTO
                        {
                            SupplierId = key.SupplierNo,
                            SubSupplierId = key.SubSupplierNo,
                            RequisitionId = requisition.RequisitionNo,
                            CenterResponsibilityCode = requisition.CenterResponsibilityCode,
                            FunctionalAreaCode = requisition.FunctionalAreaCode,
                            RegionCode = requisition.RegionCode,
                            LocalMarketRegion = requisition.LocalMarketRegion,
                            InAdvance = requisition.InAdvance.HasValue ? requisition.InAdvance.Value : false,
                            PricesIncludingVAT = requisition.PricesIncludingVAT.HasValue ? requisition.PricesIncludingVAT.Value : false,
                            LocationCode = requisition.LocalCode,
                            Purchaser_Code = requisition.NumeroMecanografico,

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
                                NoContrato = line.NoContrato
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
                                                //&& x.SubSupplierNo == purchOrder.SubSupplierId
                                                && x.UnitOfMeasureCode == line.UnitMeasureCode)
                                    .FirstOrDefault()
                                    ?.SupplierProductId
                            );

                            //Novo código que adiciona + linhas mas só para requisições do tipo nutrição
                            if (requisition.RequestNutrition == true)
                            {
                                string codFornecedor = purchOrder.SupplierId;
                                List<ConfigLinhasEncFornecedor> LinhasEncFornecedor = DBConfigLinhasEncFornecedor.GetAll().Where(x => x.VendorNo == codFornecedor).ToList();

                                if (LinhasEncFornecedor != null && LinhasEncFornecedor.Count > 0)
                                {
                                    foreach (ConfigLinhasEncFornecedor linha in LinhasEncFornecedor)
                                    {
                                        string ProjectNo = string.Empty;
                                        string RegionCode = string.Empty;
                                        string FunctionalAreaCode = string.Empty;
                                        string CenterResponsibilityCode = string.Empty;
                                        string ArmazemCompraDireta = string.Empty;

                                        Configuração Config = DBConfigurations.GetById(1);
                                        if (Config != null)
                                            ArmazemCompraDireta = Config.ArmazemCompraDireta;

                                        UnidadesProdutivas UnidProd = DBProductivityUnits.GetAll().Where(x => x.NºCliente == codFornecedor).FirstOrDefault();
                                        if (UnidProd != null)
                                            ProjectNo = UnidProd.ProjetoMatSubsidiárias;

                                        if (!string.IsNullOrEmpty(ProjectNo))
                                        {
                                            NAVProjectsViewModel Project = DBNAV2017Projects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, ProjectNo).FirstOrDefault();
                                            if (Project != null)
                                            {
                                                RegionCode = Project.RegionCode;
                                                FunctionalAreaCode = Project.AreaCode;
                                                CenterResponsibilityCode = Project.CenterResponsibilityCode;
                                            }
                                        }

                                        PurchOrderLineDTO purchOrderLine = new PurchOrderLineDTO()
                                        {
                                            LineId = null,
                                            Type = 2, //PRODUTO
                                            Code = linha.No,
                                            Description = linha.Description,
                                            Description2 = linha.Description2,
                                            ProjectNo = ProjectNo,
                                            QuantityRequired = linha.Quantity,
                                            UnitCost = linha.Valor,
                                            LocationCode = ArmazemCompraDireta,
                                            OpenOrderNo = "", //line.OpenOrderNo,
                                            OpenOrderLineNo = null, //line.OpenOrderLineNo,
                                            CenterResponsibilityCode = CenterResponsibilityCode,
                                            FunctionalAreaCode = FunctionalAreaCode,
                                            RegionCode = RegionCode,
                                            UnitMeasureCode = linha.UnitOfMeasure,
                                            VATBusinessPostingGroup = "", //line.VATBusinessPostingGroup,
                                            VATProductPostingGroup = "", //line.VATProductPostingGroup,
                                            DiscountPercentage = 0 //line.DiscountPercentage.HasValue ? line.DiscountPercentage.Value : 0,
                                        };
                                        purchOrder.Lines.Add(purchOrderLine);
                                    }
                                }
                            }

                            //var result = CreateNAVPurchaseOrderFor(purchOrder, Convert.ToDateTime(requisition.ReceivedDate), requisition.Comments);
                            var result = CreateNAVPurchaseOrderFor(purchOrder, Convert.ToDateTime(requisition.ReceivedDate));
                            if (result.CompletedSuccessfully)
                            {
                                //Update req
                                requisition.OrderNo = result.ResultValue;

                                //Update Requisition Lines
                                requisition.Lines.ForEach(line =>
                                {
                                    if (line.SupplierNo == purchOrder.SupplierId && line.SubSupplierNo == purchOrder.SubSupplierId)
                                    {
                                        line.CreatedOrderNo = result.ResultValue;
                                        line.UpdateUser = this.changedByUserName;
                                    }
                                });
                                //Commit to DB
                                var updatedReq = DBRequest.Update(requisition.ParseToDB(), true);
                                //bool linesUpdated = DBRequestLine.Update(requisition.Lines.ParseToDB());
                                //if (linesUpdated)
                                if (updatedReq != null)
                                {
                                    requisition.eMessages.Add(new TraceInformation(TraceType.Success, "Criada encomenda para o fornecedor núm. " + purchOrder.SupplierId + " ;"));
                                }
                            }
                        }
                        catch (Exception ex)
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
                    requisition.eMessage = "Não existem linhas que cumpram os requisitos de criação de encomenda.";
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
                if (requisition.Lines.Where(p => p.CreateMarketSearch == true).Where(p => string.IsNullOrEmpty(p.QueryCreatedMarketNo)).Count() <= 0)
                {
                    requisition.eReasonCode = -1;
                    requisition.eMessage = "Consulta ao Mercado não pode ser criada! As linhas devem estar marcadas com 'Criar Consulta Mercado' e não ter 'Nº de Consulta Mercado Criada'";
                    return requisition;
                }

                //Criar nova Consulta Mercado - Obtenção do novo NumConsultaMercado e incrementar Numerações
                ConsultaMercado consultaMercado = DBConsultaMercado.Create(changedByUserName);

                //Ir Buscar o Nº Mecanográfico do utilizado
                ConfigUtilizadores UC = DBUserConfigurations.GetById(changedByUserName);

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
                consultaMercado.CodComprador = !string.IsNullOrEmpty(UC.EmployeeNo) ? UC.EmployeeNo : null;

                consultaMercado = DBConsultaMercado.Update(consultaMercado);

                //Para cada linha da requisição
                foreach (RequisitionLineViewModel requisitionLine in requisition.Lines.Where(p => p.CreateMarketSearch == true).Where(p => string.IsNullOrEmpty(p.QueryCreatedMarketNo)))
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
                            seleccaoEntidades = new SeleccaoEntidades()
                            {
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
                catch (Exception ex)
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
