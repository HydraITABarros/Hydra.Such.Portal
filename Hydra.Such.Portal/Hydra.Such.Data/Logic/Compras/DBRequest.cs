using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Data.ViewModel.ProjectView;
using Microsoft.EntityFrameworkCore;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.Logic.Request
{
    public static class DBRequest
    {
        #region CRUD

        public static List<Requisição> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Requisição.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Requisição> GetAll(int TipoReq)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Requisição.Where(x => x.TipoReq == TipoReq).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Requisição> GetAllByViatura(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Requisição.Where(x => x.Viatura == Matricula).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Requisição> GetAllByProjeto(string Projeto)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Requisição.Where(x => x.NºProjeto == Projeto).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Requisição> GetByState(int TipoReq, RequisitionStates state)
        {
            try
            {
                List<RequisitionStates> states = new List<RequisitionStates>() { state };
                return GetByState(TipoReq, states);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Requisição> GetByState(int TipoReq, List<RequisitionStates> states)
        {
            try
            {
                List<int> stateValues = states.Cast<int>().ToList();

                using (var ctx = new SuchDBContext())
                {
                    return ctx.Requisição
                        .Include("LinhasRequisição")
                        .Include(x => x.RequisicoesRegAlteracoes)
                        .Where(x => stateValues.Contains(x.Estado.Value) && x.TipoReq == TipoReq)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Requisição> GetByStateSimple(int TipoReq, List<RequisitionStates> states)
        {
            try
            {
                List<int> stateValues = states.Cast<int>().ToList();

                using (var ctx = new SuchDBContext())
                {
                    return ctx.Requisição
                        .Where(x => stateValues.Contains(x.Estado.Value) && x.TipoReq == TipoReq)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Requisição> GetAllHistoric(int TipoReq)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Requisição
                        //.Include("LinhasRequisição")
                        //.Include(x => x.RequisicoesRegAlteracoes)
                        .Where(x => x.Estado == (int)RequisitionStates.Archived && x.TipoReq == TipoReq)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Requisição> GetByState(List<RequisitionStates> states, List<AcessosDimensões> userDims, string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<int> stateValues = states.Cast<int>().ToList();

                string userRegions = "";
                string userAreas = "";
                string userCresps = "";

                if (userDims != null && userDims.Count > 0)
                {
                    userDims.ForEach(x =>
                    {
                       if(x.Dimensão == 1)
                        {
                            if (userRegions == "")
                                userRegions = x.ValorDimensão;
                            else
                                userRegions = userRegions + "," + x.ValorDimensão;
                        }

                        if (x.Dimensão == 2)
                        {
                            if (userAreas == "")
                                userAreas = x.ValorDimensão;
                            else
                                userAreas = userAreas + "," + x.ValorDimensão;
                        }

                        if (x.Dimensão == 3)
                        {
                            if (userCresps == "")
                                userCresps = x.ValorDimensão;
                            else
                                userCresps = userCresps + "," + x.ValorDimensão;
                        }
                    });
                }               

                if (userRegions == "")
                {
                    //List<NAVDimValueViewModel> nav2017Regions = DBNAV2017DimensionValues.GetByDimType(NAVDatabaseName, NAVCompanyName, 1);

                    //if(nav2017Regions != null)
                    //{
                    //    nav2017Regions.ForEach(x => { 
                    //       if (userRegions == "")
                    //            userRegions = x.Code;
                    //        else
                    //            userRegions = userRegions + "," + x.Code;
                    //    });
                    //}
                }

                if (userAreas == "")
                {
                    //List<NAVDimValueViewModel> nav2017Areas = DBNAV2017DimensionValues.GetByDimType(NAVDatabaseName, NAVCompanyName, 2);

                    //if (nav2017Areas != null)
                    //{
                    //    nav2017Areas.ForEach(x => {
                    //        if (userAreas == "")
                    //            userAreas = x.Code;
                    //        else
                    //            userAreas = userAreas + "," + x.Code;
                    //    });
                    //}
                }

                if (userCresps == "")
                {
                    //List<NAVDimValueViewModel> nav2017Cresps = DBNAV2017DimensionValues.GetByDimType(NAVDatabaseName, NAVCompanyName, 3);

                    //if (userCresps != null)
                    //{
                    //    nav2017Cresps.ForEach(x => {
                    //        if (userCresps == "")
                    //            userCresps = x.Code;
                    //        else
                    //            userCresps = userCresps + "," + x.Code;
                    //    });
                    //}
                }


                using (var ctx = new SuchDBContext())
                {

                    List<Requisição> reqList = ctx.Requisição.Where(x =>
                        stateValues.Contains(x.Estado.Value)
                    ).Select(Rq => new Requisição()
                    {
                        NºRequisição = Rq.NºRequisição,
                        Estado = Rq.Estado,
                        CódigoRegião = Rq.CódigoRegião,
                        CódigoCentroResponsabilidade = Rq.CódigoCentroResponsabilidade,
                        CódigoÁreaFuncional = Rq.CódigoÁreaFuncional,
                        CódigoLocalEntrega = Rq.CódigoLocalEntrega,
                        CódigoLocalRecolha = Rq.CódigoLocalRecolha,
                        CódigoLocalização = Rq.CódigoLocalização,
                        NºProjeto = Rq.NºProjeto,
                        ResponsávelAprovação = Rq.ResponsávelAprovação,
                        ResponsávelCriação = Rq.ResponsávelCriação
                    }).ToList();


                    foreach(var r in reqList)
                    {
                        r.CódigoRegião = string.IsNullOrEmpty(r.CódigoRegião) ? "" : r.CódigoRegião;
                        r.CódigoÁreaFuncional = string.IsNullOrEmpty(r.CódigoÁreaFuncional) ? "" : r.CódigoÁreaFuncional;
                        r.CódigoCentroResponsabilidade = string.IsNullOrEmpty(r.CódigoCentroResponsabilidade) ? "" : r.CódigoCentroResponsabilidade;
                    }

                    if (userRegions != "")
                    {
                        reqList = reqList.Where(r => userRegions.Contains(r.CódigoRegião) || r.CódigoRegião == null || r.CódigoRegião == "").ToList();
                    }

                    if(userAreas != "")
                    {
                        reqList = reqList.Where(a => userAreas.Contains(a.CódigoÁreaFuncional) || a.CódigoÁreaFuncional == null || a.CódigoÁreaFuncional == "").ToList();
                    }

                    if(userCresps != "")
                    {
                        reqList = reqList.Where(c => userCresps.Contains(c.CódigoCentroResponsabilidade) || c.CódigoCentroResponsabilidade == null || c.CódigoCentroResponsabilidade == "").ToList();
                    }

                    return reqList;
                    
                    //return ctx.Requisição.Where(x =>
                    //    (stateValues.Contains(x.Estado.Value)) &&
                    //    (userRegions.ToLower().Contains(x.CódigoRegião.ToLower()) || x.CódigoRegião == null) &&
                    //    (userAreas.ToLower().Contains(x.CódigoÁreaFuncional.ToLower()) || x.CódigoÁreaFuncional == null) &&
                    //    (userCresps.ToLower().Contains(x.CódigoCentroResponsabilidade.ToLower()) || x.CódigoCentroResponsabilidade == null)
                    //).Select(Rq => new Requisição()
                    //{
                    //           NºRequisição = Rq.NºRequisição,
                    //           Estado = Rq.Estado,
                    //           CódigoRegião = Rq.CódigoRegião,
                    //           CódigoCentroResponsabilidade = Rq.CódigoCentroResponsabilidade,
                    //           CódigoÁreaFuncional = Rq.CódigoÁreaFuncional,
                    //           CódigoLocalEntrega = Rq.CódigoLocalEntrega,
                    //           CódigoLocalRecolha = Rq.CódigoLocalRecolha,
                    //           CódigoLocalização = Rq.CódigoLocalização,
                    //           NºProjeto = Rq.NºProjeto,
                    //           ResponsávelAprovação = Rq.ResponsávelAprovação,
                    //           ResponsávelCriação = Rq.ResponsávelCriação
                    //}).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Requisição GetById(string requestId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Requisição
                        .Include(x => x.LinhasRequisição)//("LinhasRequisição")
                        .Include(x => x.RequisicoesRegAlteracoes)
                        .SingleOrDefault(x => x.NºRequisição == requestId);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Requisição Create(Requisição ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.Requisição.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Requisição Update(Requisição objectToUpdate, bool updateLines = false, bool addLogEntry = false)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    if (updateLines && objectToUpdate.LinhasRequisição != null)
                        DBRequestLine.Update(objectToUpdate.LinhasRequisição.ToList(), ctx);
                    if (addLogEntry)
                    {
                        var logEntry = new RequisicoesRegAlteracoes();
                        logEntry.ModificadoEm = DateTime.Now;
                        logEntry.ModificadoPor = objectToUpdate.UtilizadorModificação;
                        logEntry.NºRequisição = objectToUpdate.NºRequisição;
                        if (objectToUpdate.Estado.HasValue)
                            logEntry.Estado = objectToUpdate.Estado.Value;

                        ctx.RequisicoesRegAlteracoes.Add(logEntry);
                    }
                    objectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.Requisição.Update(objectToUpdate);
                    ctx.SaveChanges();

                    objectToUpdate = GetById(objectToUpdate.NºRequisição);
                }

                return objectToUpdate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Requisição UpdateHeaderAndLines(Requisição item, bool addLogEntry)
        {
            return Update(item, true, addLogEntry);
        }

        public static bool Delete(Requisição ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //if (ObjectToDelete.DiárioDeProjeto.Count() > 0)
                    //    ctx.DiárioDeProjeto.RemoveRange(ObjectToDelete.DiárioDeProjeto);

                    //if (ObjectToDelete.LinhasPEncomendaProcedimentosCcp.Count() > 0)
                    //    ctx.LinhasPEncomendaProcedimentosCcp.RemoveRange(ObjectToDelete.LinhasPEncomendaProcedimentosCcp);

                    if (ObjectToDelete.LinhasRequisição.Count() > 0)
                        ctx.LinhasRequisição.RemoveRange(ObjectToDelete.LinhasRequisição);

                    if (ObjectToDelete.LinhasRequisiçãoHist.Count() > 0)
                        ctx.LinhasRequisiçãoHist.RemoveRange(ObjectToDelete.LinhasRequisiçãoHist);

                    if (ObjectToDelete.LinhasRequisiçõesSimplificadas.Count() > 0)
                        ctx.LinhasRequisiçõesSimplificadas.RemoveRange(ObjectToDelete.LinhasRequisiçõesSimplificadas);

                    //if (ObjectToDelete.MovimentosDeProjeto.Count() > 0)
                    //    ctx.MovimentosDeProjeto.RemoveRange(ObjectToDelete.MovimentosDeProjeto);

                    //if (ObjectToDelete.PréMovimentosProjeto.Count() > 0)
                    //    ctx.PréMovimentosProjeto.RemoveRange(ObjectToDelete.PréMovimentosProjeto);

                    if (ObjectToDelete.RequisicoesRegAlteracoes.Count() > 0)
                        ctx.RequisicoesRegAlteracoes.RemoveRange(ObjectToDelete.RequisicoesRegAlteracoes);

                    ctx.Requisição.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static List<Requisição> GetByProcedimento(int TipoReq, string procedimentoNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Requisição.Where(x => x.NºProcedimentoCcp == procedimentoNo && x.TipoReq == TipoReq).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        public static List<Requisição> GetReqByUserAreaStatus(int TipoReq, string userName, RequisitionStates status)
        {
            return GetReqByUserAreaStatus(TipoReq, userName, new List<RequisitionStates> { status });
        }

        public static List<Requisição> GetReqByUserAreaStatus(int TipoReq, string UserName, List<RequisitionStates> status)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    var statusValues = status.Cast<int>().ToList();
                    
                    return ctx.Requisição.Where(x => x.TipoReq == TipoReq &&
                    (x.ResponsávelCriação.ToLower() == UserName.ToLower() || x.ResponsávelAprovação.ToLower() == UserName.ToLower() ||
                    x.ResponsávelValidação.ToLower() == UserName.ToLower() || x.ResponsávelReceção.ToLower() == UserName.ToLower()) &&
                    statusValues.Contains(x.Estado.Value) && !x.ModeloDeRequisição.HasValue || !x.ModeloDeRequisição.Value).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<Requisição> GetReqByUser(int TipoReq, string UserName)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Requisição.Where(x => x.TipoReq == TipoReq && x.UtilizadorCriação == UserName && !x.ModeloDeRequisição.HasValue || !x.ModeloDeRequisição.Value).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Requisição> GetReqByUserResponsibleForApproval(int TipoReq, string UserName)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Requisição.Where(x => x.TipoReq == TipoReq && x.ResponsávelAprovação == UserName && !x.ModeloDeRequisição.HasValue || !x.ModeloDeRequisição.Value).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        #region Parse Utilities
        public static RequisiçãoHist TransferToRequisitionHist(this RequisitionViewModel item)
        {
            if (item != null)
            {
                return new RequisiçãoHist()
                {
                    NºRequisição = item.RequisitionNo,
                    Área = item.Area,
                    Estado = item.State.HasValue ? (int)item.State.Value : (int?)null,
                    NºProjeto = item.ProjectNo,
                    CódigoRegião = item.RegionCode,
                    CódigoÁreaFuncional = item.FunctionalAreaCode,
                    CódigoCentroResponsabilidade = item.CenterResponsibilityCode,
                    CódigoLocalização = item.LocalCode,
                    NºFuncionário = item.EmployeeNo,
                    Viatura = item.Vehicle,
                    DataReceção = !string.IsNullOrEmpty(item.ReceivedDate) ? Convert.ToDateTime(item.ReceivedDate) : (DateTime?)null,
                    Urgente = item.Urgent,
                    Amostra = item.Sample,
                    Anexo = item.Attachment,
                    Imobilizado = item.Immobilized,
                    CompraADinheiro = item.BuyCash,
                    CódigoLocalRecolha = item.LocalCollectionCode,
                    CódigoLocalEntrega = item.LocalDeliveryCode,
                    Observações = item.Comments,
                    RejeicaoMotivo = item.RejeicaoMotivo,
                    ModeloDeRequisição = item.RequestModel,
                    DataHoraCriação = !string.IsNullOrEmpty(item.CreateDate) ? Convert.ToDateTime(item.CreateDate) : (DateTime?)null,
                    UtilizadorCriação = item.CreateUser,
                    DataHoraModificação = item.UpdateDate,
                    UtilizadorModificação = item.UpdateUser,
                    CabimentoOrçamental = item.RelatedSearches,
                    Exclusivo = item.Exclusive,
                    JáExecutado = item.AlreadyPerformed,
                    Equipamento = item.Equipment,
                    ReposiçãoDeStock = item.StockReplacement,
                    Reclamação = item.Reclamation,
                    NºRequisiçãoReclamada = item.RequestReclaimNo,
                    ResponsávelCriação = item.ResponsibleCreation,
                    ResponsávelAprovação = item.ResponsibleApproval,
                    ResponsávelValidação = item.ResponsibleValidation,
                    ResponsávelReceção = item.ResponsibleReception,
                    DataAprovação = item.ApprovalDate,
                    DataValidação = item.ValidationDate,
                    UnidadeProdutivaAlimentação = item.UnitFoodProduction,
                    RequisiçãoNutrição = item.RequestNutrition,
                    RequisiçãoDetergentes = item.RequestforDetergents,
                    NºProcedimentoCcp = item.ProcedureCcpNo,
                    Aprovadores = item.Approvers,
                    MercadoLocal = item.LocalMarket,
                    RegiãoMercadoLocal = item.LocalMarketRegion,
                    ReparaçãoComGarantia = item.RepairWithWarranty,
                    Emm = item.Emm,
                    DataEntregaArmazém = !string.IsNullOrEmpty(item.WarehouseDeliveryDate) ? Convert.ToDateTime(item.WarehouseDeliveryDate) : (DateTime?)null,
                    LocalDeRecolha = item.LocalCollection,
                    MoradaRecolha = item.CollectionAddress,
                    Morada2Recolha = item.Collection2Address,
                    CódigoPostalRecolha = item.CollectionPostalCode,
                    LocalidadeRecolha = item.CollectionLocality,
                    ContatoRecolha = item.CollectionContact,
                    ResponsávelReceçãoRecolha = item.CollectionResponsibleReception,
                    LocalEntrega = item.LocalDelivery,
                    MoradaEntrega = item.DeliveryAddress,
                    Morada2Entrega = item.Delivery2Address,
                    CódigoPostalEntrega = item.DeliveryPostalCode,
                    LocalidadeEntrega = item.LocalityDelivery,
                    ContatoEntrega = item.DeliveryContact,
                    ResponsávelReceçãoReceção = item.ResponsibleReceptionReception,
                    NºFatura = item.InvoiceNo,
                    DataMercadoLocal = item.LocalMarketDate,
                    DataRequisição = !string.IsNullOrEmpty(item.RequisitionDate) ? Convert.ToDateTime(item.RequisitionDate) : (DateTime?)null,
                    NºConsultaMercado = item.MarketInquiryNo,
                    NºEncomenda = item.OrderNo,
                    Orçamento = item.Budget,
                    ValorEstimado = item.EstimatedValue,
                    PrecoIvaincluido = item.PricesIncludingVAT,
                    Adiantamento = item.InAdvance,
                    PedirOrcamento = item.PedirOrcamento,
                };
            }
            return null;
        }

        public static List<LinhasRequisiçãoHist> TransferToRequisitionLinesHist(this List<RequisitionLineViewModel> Linhas)
        {
            List<LinhasRequisiçãoHist> LinhasREQHist = new List<LinhasRequisiçãoHist>();
            if (Linhas.Count > 0)
            {
                Linhas.ForEach(Linha =>
                {
                    LinhasRequisiçãoHist LinhaHist = new LinhasRequisiçãoHist()
                    {
                        NºRequisição = Linha.RequestNo,
                        NºLinha = (int)Linha.LineNo,
                        Tipo = Linha.Type,
                        Código = Linha.Code,
                        Descrição = Linha.Description,
                        Descrição2 = Linha.Description2,
                        CódigoUnidadeMedida = Linha.UnitMeasureCode,
                        CódigoLocalização = Linha.LocalCode,
                        MercadoLocal = Linha.LocalMarket,
                        QuantidadeARequerer = Linha.QuantityToRequire,
                        QuantidadeRequerida = Linha.QuantityRequired,
                        QuantidadeDisponibilizada = Linha.QuantityToProvide,
                        QuantidadeAReceber = Linha.QuantityAvailable,
                        QuantidadeRecebida = Linha.QuantityReceivable,
                        QuantidadePendente = Linha.QuantityPending,
                        CustoUnitário = Linha.UnitCost,
                        DataReceçãoEsperada = !string.IsNullOrEmpty(Linha.ExpectedReceivingDate) ? Convert.ToDateTime(Linha.ExpectedReceivingDate) : (DateTime?)null,
                        Faturável = Linha.Billable,
                        NºProjeto = Linha.ProjectNo,
                        CódigoRegião = Linha.RegionCode,
                        CódigoÁreaFuncional = Linha.FunctionalAreaCode,
                        CódigoCentroResponsabilidade = Linha.CenterResponsibilityCode,
                        NºFuncionário = Linha.FunctionalNo,
                        Viatura = Linha.Vehicle,
                        DataHoraCriação = Linha.CreateDateTime,
                        UtilizadorCriação = Linha.CreateUser,
                        DataHoraModificação = Linha.UpdateDateTime,
                        UtilizadorModificação = Linha.UpdateUser,
                        QtdPorUnidadeDeMedida = Linha.QtyByUnitOfMeasure,
                        PreçoUnitárioVenda = Linha.UnitCostsould,
                        ValorOrçamento = Linha.BudgetValue,
                        NºLinhaOrdemManutenção = Linha.MaintenanceOrderLineNo,
                        CriarConsultaMercado = Linha.CreateMarketSearch,
                        EnviarPréCompra = Linha.SubmitPrePurchase,
                        EnviadoPréCompra = Linha.SendPrePurchase,
                        DataMercadoLocal = !string.IsNullOrEmpty(Linha.LocalMarketDate) ? Convert.ToDateTime(Linha.LocalMarketDate) : (DateTime?)null,
                        UserMercadoLocal = Linha.LocalMarketUser,
                        EnviadoParaCompras = Linha.SendForPurchase,
                        DataEnvioParaCompras = !string.IsNullOrEmpty(Linha.SendForPurchaseDate) ? Convert.ToDateTime(Linha.SendForPurchaseDate) : (DateTime?)null,
                        ValidadoCompras = Linha.PurchaseValidated,
                        RecusadoCompras = Linha.PurchaseRefused,
                        MotivoRecusaMercLocal = Linha.ReasonToRejectionLocalMarket,
                        DataRecusaMercLocal = !string.IsNullOrEmpty(Linha.RejectionLocalMarketDate) ? Convert.ToDateTime(Linha.RejectionLocalMarketDate) : (DateTime?)null,
                        IdCompra = Linha.PurchaseId,
                        NºFornecedor = Linha.SupplierNo,
                        NºEncomendaAberto = Linha.OpenOrderNo,
                        NºLinhaEncomendaAberto = Linha.OpenOrderLineNo,
                        NºDeConsultaMercadoCriada = Linha.QueryCreatedMarketNo,
                        NºEncomendaCriada = Linha.CreatedOrderNo,
                        CódigoProdutoFornecedor = Linha.SupplierProductCode,
                        UnidadeProdutivaNutrição = Linha.UnitNutritionProduction,
                        RegiãoMercadoLocal = Linha.MarketLocalRegion,
                        NºCliente = Linha.CustomerNo,
                        Aprovadores = Linha.Approvers,
                        Urgente = Linha.Urgent,
                        GrupoRegistoIvanegocio = Linha.VATBusinessPostingGroup,
                        GrupoRegistoIvaproduto = Linha.VATProductPostingGroup,
                        PercentagemDesconto = Linha.DiscountPercentage
                    };

                    LinhasREQHist.Add(LinhaHist);
                });
            }
            return LinhasREQHist;
        }

        public static RequisitionViewModel ParseToViewModel(this Requisição item)
        {
            if (item != null)
            {
                return new RequisitionViewModel()
                {
                    RequisitionNo = item.NºRequisição,
                    TipoReq = item.TipoReq,
                    Area = item.Área,
                    State = item.Estado.HasValue && Enum.IsDefined(typeof(RequisitionStates), item.Estado.Value) ? (RequisitionStates)item.Estado.Value : (RequisitionStates?)null,
                    ProjectNo = item.NºProjeto,
                    RegionCode = item.CódigoRegião,
                    FunctionalAreaCode = item.CódigoÁreaFuncional,
                    CenterResponsibilityCode = item.CódigoCentroResponsabilidade,
                    LocalCode = item.CódigoLocalização,
                    EmployeeNo = item.NºFuncionário,
                    Vehicle = item.Viatura,
                    ReceivedDate = !item.DataReceção.HasValue ? "" : item.DataReceção.Value.ToString("yyyy-MM-dd"),
                    ReceivedDateHour = !item.DataReceção.HasValue ? "" : item.DataReceção.Value.ToString("HH:mm"),
                    Urgent = item.Urgente,
                    Sample = item.Amostra,
                    Attachment = item.Anexo,
                    Immobilized = item.Imobilizado,
                    BuyCash = item.CompraADinheiro,
                    LocalCollectionCode = item.CódigoLocalRecolha,
                    LocalDeliveryCode = item.CódigoLocalEntrega,
                    Comments = item.Observações,
                    NoDocumento = item.NoDocumento,
                    RejeicaoMotivo = item.RejeicaoMotivo,
                    RequestModel = item.ModeloDeRequisição,
                    CreateDate = !item.DataHoraCriação.HasValue ? "" : item.DataHoraCriação.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    CreateUser = item.UtilizadorCriação,
                    UpdateDate = item.DataHoraModificação,
                    UpdateUser = item.UtilizadorModificação,
                    RelatedSearches = item.CabimentoOrçamental,
                    Exclusive = item.Exclusivo,
                    AlreadyPerformed = item.JáExecutado,
                    Equipment = item.Equipamento,
                    StockReplacement = item.ReposiçãoDeStock,
                    Reclamation = item.Reclamação,
                    RequestReclaimNo = item.NºRequisiçãoReclamada,
                    ResponsibleCreation = item.ResponsávelCriação,
                    ResponsibleApproval = item.ResponsávelAprovação,
                    ResponsibleValidation = item.ResponsávelValidação,
                    ResponsibleReception = item.ResponsávelReceção,
                    ApprovalDate = item.DataAprovação,
                    ApprovalDateText = !item.DataAprovação.HasValue ? "" : item.DataAprovação.Value.ToString("yyyy-MM-dd"),
                    ApprovalDateHour = !item.DataAprovação.HasValue ? "" : item.DataAprovação.Value.ToString("HH:mm"),
                    ValidationDate = item.DataValidação,
                    ValidationDateText = !item.DataValidação.HasValue ? "" : item.DataValidação.Value.ToString("yyyy-MM-dd"),
                    ValidationDateHour = !item.DataValidação.HasValue ? "" : item.DataValidação.Value.ToString("HH:mm"),
                    UnitFoodProduction = item.UnidadeProdutivaAlimentação,
                    RequestNutrition = item.RequisiçãoNutrição,
                    RequestNutritionText = !item.RequisiçãoNutrição.HasValue ? "" : item.RequisiçãoNutrição == true ? "Sim" : "Não",
                    RequestforDetergents = item.RequisiçãoDetergentes,
                    ProcedureCcpNo = item.NºProcedimentoCcp,
                    Approvers = item.Aprovadores,
                    LocalMarket = item.MercadoLocal,
                    LocalMarketRegion = item.RegiãoMercadoLocal,
                    RepairWithWarranty = item.ReparaçãoComGarantia,
                    Emm = item.Emm,
                    WarehouseDeliveryDate = !item.DataEntregaArmazém.HasValue ? "" : item.DataEntregaArmazém.Value.ToString("yyyy-MM-dd"),
                    LocalCollection = item.LocalDeRecolha,
                    CollectionAddress = item.MoradaRecolha,
                    Collection2Address = item.Morada2Recolha,
                    CollectionPostalCode = item.CódigoPostalRecolha,
                    CollectionLocality = item.LocalidadeRecolha,
                    CollectionContact = item.ContatoRecolha,
                    CollectionResponsibleReception = item.ResponsávelReceçãoRecolha,
                    LocalDelivery = item.LocalEntrega,
                    DeliveryAddress = item.MoradaEntrega,
                    Delivery2Address = item.Morada2Entrega,
                    DeliveryPostalCode = item.CódigoPostalEntrega,
                    LocalityDelivery = item.LocalidadeEntrega,
                    DeliveryContact = item.ContatoEntrega,
                    ResponsibleReceptionReception = item.ResponsávelReceçãoReceção,
                    InvoiceNo = item.NºFatura,
                    LocalMarketDate = item.DataMercadoLocal,
                    RequisitionDate = !item.DataRequisição.HasValue ? "" : item.DataRequisição.Value.ToString("yyyy-MM-dd"),
                    MarketInquiryNo = item.NºConsultaMercado,
                    OrderNo = item.NºEncomenda,
                    Budget = item.Orçamento,
                    EstimatedValue = item.ValorEstimado,
                    PricesIncludingVAT = item.PrecoIvaincluido.HasValue ? item.PrecoIvaincluido.Value : false,
                    InAdvance = item.Adiantamento.HasValue ? item.Adiantamento.Value : false,
                    PedirOrcamento = item.PedirOrcamento,
                    ValorTotalDocComIVA = item.ValorTotalDocComIVA,
                    TipoAlteracaoSISLOG = item.TipoAlteracaoSISLOG,
                    DataAlteracaoSISLOG = item.DataAlteracaoSISLOG,
                    EnviarSISLOG = item.EnviarSISLOG,
                    SISLOG = item.SISLOG,
                    DataEnvioSISLOG = item.DataEnvioSISLOG,

                    Lines = item.LinhasRequisição.ToList().ParseToViewModel(),
                    //AROMAO 01/10/2018
                    ChangeLog = item.RequisicoesRegAlteracoes.ToList().ParseToViewModel()
                };
            }
            return null;
        }

        public static List<RequisitionViewModel> ParseToViewModel(this List<Requisição> items)
        {
            List<RequisitionViewModel> parsedItems = new List<RequisitionViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }

        public static Requisição ParseToDB(this RequisitionViewModel item)
        {
            try
            {
                if (item != null)
                {
                    return new Requisição()
                    {
                        NºRequisição = item.RequisitionNo,
                        TipoReq = item.TipoReq,
                        Área = item.Area,
                        Estado = item.State.HasValue ? (int)item.State.Value : (int?)null,
                        NºProjeto = item.ProjectNo,
                        CódigoRegião = item.RegionCode,
                        CódigoÁreaFuncional = item.FunctionalAreaCode,
                        CódigoCentroResponsabilidade = item.CenterResponsibilityCode,
                        CódigoLocalização = item.LocalCode,
                        NºFuncionário = item.EmployeeNo,
                        Viatura = item.Vehicle,
                        DataReceção = !string.IsNullOrEmpty(item.ReceivedDate) ? Convert.ToDateTime(item.ReceivedDate) : (DateTime?)null,
                        Urgente = item.Urgent,
                        Amostra = item.Sample,
                        Anexo = item.Attachment,
                        Imobilizado = item.Immobilized,
                        CompraADinheiro = item.BuyCash,
                        CódigoLocalRecolha = item.LocalCollectionCode,
                        CódigoLocalEntrega = item.LocalDeliveryCode,
                        Observações = item.Comments,
                        NoDocumento = item.NoDocumento,
                        RejeicaoMotivo = item.RejeicaoMotivo,
                        ModeloDeRequisição = item.RequestModel,
                        DataHoraCriação = !string.IsNullOrEmpty(item.CreateDate) ? Convert.ToDateTime(item.CreateDate) : (DateTime?)null,
                        UtilizadorCriação = item.CreateUser,
                        DataHoraModificação = item.UpdateDate,
                        UtilizadorModificação = item.UpdateUser,
                        CabimentoOrçamental = item.RelatedSearches,
                        Exclusivo = item.Exclusive,
                        JáExecutado = item.AlreadyPerformed,
                        Equipamento = item.Equipment,
                        ReposiçãoDeStock = item.StockReplacement,
                        Reclamação = item.Reclamation,
                        NºRequisiçãoReclamada = item.RequestReclaimNo,
                        ResponsávelCriação = item.ResponsibleCreation,
                        ResponsávelAprovação = item.ResponsibleApproval,
                        ResponsávelValidação = item.ResponsibleValidation,
                        ResponsávelReceção = item.ResponsibleReception,
                        DataAprovação = item.ApprovalDate,
                        DataValidação = item.ValidationDate,
                        UnidadeProdutivaAlimentação = item.UnitFoodProduction,
                        RequisiçãoNutrição = item.RequestNutrition,
                        RequisiçãoDetergentes = item.RequestforDetergents,
                        NºProcedimentoCcp = item.ProcedureCcpNo,
                        Aprovadores = item.Approvers,
                        MercadoLocal = item.LocalMarket,
                        RegiãoMercadoLocal = item.LocalMarketRegion,
                        ReparaçãoComGarantia = item.RepairWithWarranty,
                        Emm = item.Emm,
                        DataEntregaArmazém = !string.IsNullOrEmpty(item.WarehouseDeliveryDate) ? Convert.ToDateTime(item.WarehouseDeliveryDate) : (DateTime?)null,
                        LocalDeRecolha = item.LocalCollection,
                        MoradaRecolha = item.CollectionAddress,
                        Morada2Recolha = item.Collection2Address,
                        CódigoPostalRecolha = item.CollectionPostalCode,
                        LocalidadeRecolha = item.CollectionLocality,
                        ContatoRecolha = item.CollectionContact,
                        ResponsávelReceçãoRecolha = item.CollectionResponsibleReception,
                        LocalEntrega = item.LocalDelivery,
                        MoradaEntrega = item.DeliveryAddress,
                        Morada2Entrega = item.Delivery2Address,
                        CódigoPostalEntrega = item.DeliveryPostalCode,
                        LocalidadeEntrega = item.LocalityDelivery,
                        ContatoEntrega = item.DeliveryContact,
                        ResponsávelReceçãoReceção = item.ResponsibleReceptionReception,
                        NºFatura = item.InvoiceNo,
                        DataMercadoLocal = item.LocalMarketDate,
                        DataRequisição = !string.IsNullOrEmpty(item.RequisitionDate) ? Convert.ToDateTime(item.RequisitionDate) : (DateTime?)null,
                        NºConsultaMercado = item.MarketInquiryNo,
                        NºEncomenda = item.OrderNo,
                        Orçamento = item.Budget,
                        ValorEstimado = item.EstimatedValue,
                        PrecoIvaincluido = item.PricesIncludingVAT,
                        Adiantamento = item.InAdvance,
                        PedirOrcamento = item.PedirOrcamento,
                        ValorTotalDocComIVA = item.ValorTotalDocComIVA,
                        TipoAlteracaoSISLOG = item.TipoAlteracaoSISLOG,
                        DataAlteracaoSISLOG = item.DataAlteracaoSISLOG,
                        EnviarSISLOG = item.EnviarSISLOG,
                        SISLOG = item.SISLOG,
                        DataEnvioSISLOG = item.DataEnvioSISLOG,

                        LinhasRequisição = item.Lines.ParseToDB(),
                        RequisicoesRegAlteracoes = item.ChangeLog.ParseToDB()
                    };
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        public static List<Requisição> ParseToDB(this List<RequisitionViewModel> items)
        {
            List<Requisição> parsedItems = new List<Requisição>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }

        
        public static RequisitionChangeLog ParseToViewModel(this RequisicoesRegAlteracoes item)
        {
            if (item != null)
            {
                return new RequisitionChangeLog()
                {
                    Id = item.Id,
                    RequisitionNo = item.NºRequisição,
                    State = (RequisitionStates)item.Estado,
                    StateDescription = EnumHelper.GetDescriptionFor(typeof(RequisitionStates), item.Estado),
                    ModifiedAt = item.ModificadoEm,
                    ModifiedAtAsString = item.ModificadoEm.ToString("yyyy-MM-dd HH:mm:ss"),
                    ModifiedBy = item.ModificadoPor
                };
            }
            return null;
        }

        public static List<RequisitionChangeLog> ParseToViewModel(this List<RequisicoesRegAlteracoes> items)
        {
            List<RequisitionChangeLog> parsedItems = new List<RequisitionChangeLog>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }

        public static RequisicoesRegAlteracoes ParseToDB(this RequisitionChangeLog item)
        {
            if (item != null)
            {
                return new RequisicoesRegAlteracoes()
                {
                    Id = item.Id,
                    NºRequisição = item.RequisitionNo,
                    Estado = (int)item.State,
                    ModificadoEm = item.ModifiedAt,
                    ModificadoPor = item.ModifiedBy
                };
            }
            return null;
        }

        public static List<RequisicoesRegAlteracoes> ParseToDB(this List<RequisitionChangeLog> items)
        {
            List<RequisicoesRegAlteracoes> parsedItems = new List<RequisicoesRegAlteracoes>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
        #endregion
    }
}
