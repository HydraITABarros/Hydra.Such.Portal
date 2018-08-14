using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Compras;

namespace Hydra.Such.Data.Logic.Request
{
    public static class DBRequestTemplateLines
    {
        #region CRUD
        
        public static List<LinhasRequisição> GetByLineNo(int LineNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasRequisição.Where(x => x.NºLinha == LineNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<LinhasRequisição> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasRequisição.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static LinhasRequisição Create(LinhasRequisição ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.LinhasRequisição.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static LinhasRequisição Update(LinhasRequisição ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.LinhasRequisição.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Update(List<LinhasRequisição> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    if (items != null)
                    {
                        items.ForEach(item => item.DataHoraModificação = DateTime.Now);
                    }
                    ctx.LinhasRequisição.UpdateRange(items);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch
            {

            }
            return false;
        }

        public static void Update(List<LinhasRequisição> items, SuchDBContext ctx)
        {
            if (items != null)
            {
                items.ForEach(item => item.DataHoraModificação = DateTime.Now);
                ctx.LinhasRequisição.UpdateRange(items);
            }
        }

        public static bool Delete(LinhasRequisição ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasRequisição.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public static List<LinhasRequisição> GetAllByRequisiçãos(string requisicao)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasRequisição.Where(x=> x.NºRequisição == requisicao).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Parse Utilities
        public static void UpdateAgreedPrices(this List<RequisitionTemplateLineViewModel> reqLines, DateTime pricesDate, string respCenter, string region, string functionalArea)
        {
            if (reqLines == null || reqLines.Count == 0)
                return;

            List<LinhasAcordoPrecos> acordosPrecos = null;

            using (var ctx = new SuchDBContext())
            {
                acordosPrecos = ctx.LinhasAcordoPrecos
                    .Where(x => reqLines.Select(y => y.Code).Distinct().Contains(x.CodProduto)
                                && x.Cresp == respCenter
                                && x.Regiao == region
                                && x.Area == functionalArea
                                && x.DtValidadeInicio <= pricesDate
                                && x.DtValidadeFim >= pricesDate)
                    .ToList();
            }
            if (acordosPrecos.Count > 0)
            {
                for (int i = 0; i < reqLines.Count; i++)
                {
                    var acordo = acordosPrecos.FirstOrDefault(x => x.CodProduto == reqLines[i].Code);
                    if (acordo != null)
                    {
                        if (acordo.CustoUnitario.HasValue)
                            reqLines[i].UnitCost = acordo.CustoUnitario.Value;
                    }
                }
            }
        }

        public static RequisitionTemplateLineViewModel ParseToTemplateViewModel(this LinhasRequisição item)
        {
            if (item != null)
            {
                return new RequisitionTemplateLineViewModel()
                {
                    RequestNo = item.NºRequisição,
                    LineNo = item.NºLinha,
                    Type = item.Tipo,
                    Code = item.Código,
                    Description = item.Descrição,
                    UnitMeasureCode = item.CódigoUnidadeMedida,
                    LocalCode = item.CódigoLocalização,
                    LocalMarket = item.MercadoLocal,
                    QuantityToRequire = item.QuantidadeARequerer,
                    QuantityRequired = item.QuantidadeRequerida,
                    QuantityToProvide = item.QuantidadeADisponibilizar,
                    QuantityAvailable = item.QuantidadeDisponibilizada,
                    QuantityReceivable = item.QuantidadeAReceber,
                    QuantityReceived = item.QuantidadeRecebida,
                    QuantityPending = item.QuantidadePendente,
                    UnitCost = item.CustoUnitário,
                    ExpectedReceivingDate = !item.DataReceçãoEsperada.HasValue ? "" : item.DataReceçãoEsperada.Value.ToString("yyyy-MM-dd"),
                    Billable = item.Faturável,
                    ProjectNo = item.NºProjeto,
                    RegionCode = item.CódigoRegião,
                    FunctionalAreaCode = item.CódigoÁreaFuncional,
                    CenterResponsibilityCode = item.CódigoCentroResponsabilidade,
                    FunctionalNo = item.NºFuncionário,
                    Vehicle = item.Viatura,
                    CreateDateTime = item.DataHoraCriação,
                    CreateUser = item.UtilizadorCriação,
                    UpdateDateTime = item.DataHoraModificação,
                    UpdateUser = item.UtilizadorModificação,
                    QtyByUnitOfMeasure = item.QtdPorUnidadeDeMedida,
                    UnitCostsould = item.PreçoUnitárioVenda,
                    BudgetValue = item.ValorOrçamento,
                    MaintenanceOrderLineNo = item.NºLinhaOrdemManutenção,
                    CreateMarketSearch = item.CriarConsultaMercado,
                    SubmitPrePurchase = item.EnviarPréCompra,
                    SendPrePurchase = item.EnviadoPréCompra,
                    LocalMarketDate = !item.DataMercadoLocal.HasValue ? "" : item.DataMercadoLocal.Value.ToString("yyyy-MM-dd"),
                    LocalMarketUser = item.UserMercadoLocal,
                    SendForPurchase = item.EnviadoParaCompras,
                    SendForPurchaseDate = !item.DataEnvioParaCompras.HasValue ? "" : item.DataEnvioParaCompras.Value.ToString("yyyy-MM-dd"),
                    PurchaseValidated = item.ValidadoCompras,
                    PurchaseRefused = item.RecusadoCompras,
                    ReasonToRejectionLocalMarket = item.MotivoRecusaMercLocal,
                    RejectionLocalMarketDate = !item.DataRecusaMercLocal.HasValue ? "" : item.DataRecusaMercLocal.Value.ToString("yyyy-MM-dd"),
                    PurchaseId = item.IdCompra,
                    SupplierNo = item.NºFornecedor,
                    OpenOrderNo = item.NºEncomendaAberto,
                    OpenOrderLineNo = item.NºLinhaEncomendaAberto,
                    QueryCreatedMarketNo = item.NºDeConsultaMercadoCriada,
                    CreatedOrderNo = item.NºEncomendaCriada,
                    SupplierProductCode = item.CódigoProdutoFornecedor,
                    UnitNutritionProduction = item.UnidadeProdutivaNutrição,
                    MarketLocalRegion = item.RegiãoMercadoLocal,
                    CustomerNo = item.NºCliente,
                    Approvers = item.Aprovadores,
                };
            }
            return null;
        }

        public static List<RequisitionTemplateLineViewModel> ParseToTemplateViewModel(this List<LinhasRequisição> items)
        {
            List<RequisitionTemplateLineViewModel> parsedItems = new List<RequisitionTemplateLineViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToTemplateViewModel()));
            return parsedItems;
        }

        public static LinhasRequisição ParseToDB(this RequisitionTemplateLineViewModel item)
        {
            if (item != null)
            {
                return new LinhasRequisição()
                {
                    NºRequisição = item.RequestNo,
                    NºLinha = item.LineNo.HasValue ? item.LineNo.Value : 0,
                    Tipo = item.Type,
                    Código = item.Code,
                    Descrição = item.Description,
                    CódigoUnidadeMedida = item.UnitMeasureCode,
                    CódigoLocalização = item.LocalCode,
                    MercadoLocal = item.LocalMarket,
                    QuantidadeARequerer = item.QuantityToRequire,
                    QuantidadeRequerida = item.QuantityRequired,
                    QuantidadeADisponibilizar = item.QuantityToProvide,
                    QuantidadeDisponibilizada = item.QuantityAvailable,
                    QuantidadeAReceber = item.QuantityReceivable,
                    QuantidadeRecebida = item.QuantityReceived,
                    QuantidadePendente = item.QuantityPending,
                    CustoUnitário = item.UnitCost,
                    DataReceçãoEsperada = string.IsNullOrEmpty(item.ExpectedReceivingDate) ? (DateTime?)null : DateTime.Parse(item.ExpectedReceivingDate),
                    Faturável= item.Billable,
                    NºProjeto = item.ProjectNo,
                    CódigoRegião= item.RegionCode,
                    CódigoÁreaFuncional = item.FunctionalAreaCode,
                    CódigoCentroResponsabilidade = item.CenterResponsibilityCode,
                    NºFuncionário = item.FunctionalNo,
                    Viatura = item.Vehicle,
                    DataHoraCriação = item.CreateDateTime,
                    UtilizadorCriação = item.CreateUser,
                    DataHoraModificação = item.UpdateDateTime,
                    UtilizadorModificação = item.UpdateUser,
                    QtdPorUnidadeDeMedida = item.QtyByUnitOfMeasure,
                    PreçoUnitárioVenda = item.UnitCostsould,
                    ValorOrçamento = item.BudgetValue,
                    NºLinhaOrdemManutenção = item.MaintenanceOrderLineNo,
                    CriarConsultaMercado = item.CreateMarketSearch,
                    EnviarPréCompra = item.SubmitPrePurchase,
                    EnviadoPréCompra = item.SendPrePurchase,
                    DataMercadoLocal = string.IsNullOrEmpty(item.LocalMarketDate) ? (DateTime?)null : DateTime.Parse(item.LocalMarketDate),
                    UserMercadoLocal = item.LocalMarketUser,
                    EnviadoParaCompras= item.SendForPurchase,
                    DataEnvioParaCompras = string.IsNullOrEmpty(item.SendForPurchaseDate) ? (DateTime?)null : DateTime.Parse(item.SendForPurchaseDate),
                    ValidadoCompras = item.PurchaseValidated,
                    RecusadoCompras = item.PurchaseRefused,
                    MotivoRecusaMercLocal = item.ReasonToRejectionLocalMarket,
                    DataRecusaMercLocal = string.IsNullOrEmpty(item.RejectionLocalMarketDate) ? (DateTime?)null : DateTime.Parse(item.RejectionLocalMarketDate),
                    IdCompra= item.PurchaseId,
                    NºFornecedor= item.SupplierNo,
                    NºEncomendaAberto= item.OpenOrderNo,
                    NºLinhaEncomendaAberto= item.OpenOrderLineNo,
                    NºDeConsultaMercadoCriada= item.QueryCreatedMarketNo,
                    NºEncomendaCriada= item.CreatedOrderNo,
                    CódigoProdutoFornecedor= item.SupplierProductCode,
                    UnidadeProdutivaNutrição= item.UnitNutritionProduction,
                    RegiãoMercadoLocal= item.MarketLocalRegion,
                    NºCliente= item.CustomerNo,
                    Aprovadores= item.Approvers,
                };
            }
            return null;
        }

        public static List<LinhasRequisição> ParseToDB(this List<RequisitionTemplateLineViewModel> items)
        {
            List<LinhasRequisição> parsedItems = new List<LinhasRequisição>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
        #endregion
    }
}
