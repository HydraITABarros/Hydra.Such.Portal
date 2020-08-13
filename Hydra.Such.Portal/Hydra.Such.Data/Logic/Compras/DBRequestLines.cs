using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Compras;

namespace Hydra.Such.Data.Logic.Request
{
    public static class DBRequestLine
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

        public static LinhasRequisição GetByRequisicaoNoAndLineNo(string RequisicaoNo, int LineNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasRequisição.Where(x => x.NºRequisição == RequisicaoNo && x.NºLinha == LineNo).FirstOrDefault();
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

        public static List<LinhasRequisição> GetAllByViatura(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasRequisição.Where(x => x.Viatura == Matricula).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<LinhasRequisição> GetAllByProjeto(string Projeto)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasRequisição.Where(x => x.NºProjeto == Projeto).ToList();
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
        public static List<LinhasRequisição> GetByRequisitionId(string requisicao)
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

        #region Logic extensions
        public static void UpdateAgreedPrices(this List<RequisitionLineViewModel> reqLines)
        {
            if (reqLines == null || reqLines.Count == 0)
                return;

            List<LinhasAcordoPrecos> acordosPrecos = null;

            using (var ctx = new SuchDBContext())
            {
                acordosPrecos = ctx.LinhasAcordoPrecos
                    .Where(x => reqLines.Select(y => y.SupplierNo).Distinct().Contains(x.NoFornecedor)
                                && x.DtValidadeInicio <= DateTime.Now
                                && x.DtValidadeFim >= DateTime.Now)
                    .ToList();
            }
            if (acordosPrecos.Count > 0)
            {
                for (int i = 0; i < reqLines.Count; i++)
                {
                    var acordo = acordosPrecos.FirstOrDefault(x => x.NoFornecedor == reqLines[i].SupplierNo && x.CodProduto == reqLines[i].Code);
                    if (acordo != null)
                    {
                        if (acordo.CustoUnitario.HasValue)
                            reqLines[i].UnitCost = acordo.CustoUnitario.Value;
                    }
                }
            }
        }
        #endregion

        #region Parse Utilities
        public static RequisitionLineViewModel ParseToViewModel(this LinhasRequisição item)
        {
            if (item != null)
            {
                return new RequisitionLineViewModel()
                {
                    RequestNo = item.NºRequisição,
                    LineNo = item.NºLinha,
                    Type = item.Tipo,
                    Code = item.Código,
                    Description = item.Descrição,
                    Description2 = item.Descrição2,
                    UnitMeasureCode = item.CódigoUnidadeMedida,
                    LocalCode = item.CódigoLocalização,
                    LocalMarket = item.MercadoLocal == null ? false : item.MercadoLocal,
                    QuantityToRequire = item.QuantidadeARequerer,
                    QuantityRequired = item.QuantidadeRequerida,
                    QuantityToProvide = item.QuantidadeADisponibilizar,
                    QuantityAvailable = item.QuantidadeDisponibilizada,
                    QuantityReceivable = item.QuantidadeAReceber,
                    QuantityReceived = item.QuantidadeRecebida,
                    QuantityPending = item.QuantidadePendente,
                    QuantidadeDisponivel = item.QuantidadeDisponivel,
                    QuantidadeReservada = item.QuantidadeReservada,
                    UnitCost = item.CustoUnitário,
                    UnitCostWithIVA = item.CustoUnitarioComIVA,
                    ExpectedReceivingDate = !item.DataReceçãoEsperada.HasValue ? "" : item.DataReceçãoEsperada.Value.ToString("yyyy-MM-dd"),
                    Billable = item.Faturável == null ? false : item.Faturável,
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
                    CriarNotaEncomenda = item.CriarNotaEncomenda == null ? false : item.CriarNotaEncomenda,
                    CreateMarketSearch = item.CriarConsultaMercado == null ? false : item.CriarConsultaMercado,
                    SubmitPrePurchase = item.EnviadoPréCompra == null ? false : item.EnviadoPréCompra,
                    SendPrePurchase = item.EnviarPréCompra == null ? false : item.EnviarPréCompra,
                    LocalMarketDate = !item.DataMercadoLocal.HasValue ? "" : item.DataMercadoLocal.Value.ToString("yyyy-MM-dd"),
                    LocalMarketUser = item.UserMercadoLocal,
                    SendForPurchase = item.EnviadoParaCompras == null ? false : item.EnviadoParaCompras,
                    SendForPurchaseDate = !item.DataEnvioParaCompras.HasValue ? "" : item.DataEnvioParaCompras.Value.ToString("yyyy-MM-dd"),
                    PurchaseValidated = item.ValidadoCompras == null ? false : item.ValidadoCompras,
                    PurchaseRefused = item.RecusadoCompras == null ? false : item.RecusadoCompras,
                    ReasonToRejectionLocalMarket = item.MotivoRecusaMercLocal,
                    RejectionLocalMarketDate = !item.DataRecusaMercLocal.HasValue ? "" : item.DataRecusaMercLocal.Value.ToString("yyyy-MM-dd"),
                    PurchaseId = item.IdCompra,
                    SupplierNo = item.NºFornecedor,
                    SubSupplierNo = item.NoSubFornecedor,
                    OpenOrderNo = item.NºEncomendaAberto,
                    OpenOrderLineNo = item.NºLinhaEncomendaAberto,
                    QueryCreatedMarketNo = item.NºDeConsultaMercadoCriada,
                    CreatedOrderNo = item.NºEncomendaCriada,
                    SupplierProductCode = item.CódigoProdutoFornecedor,
                    UnitNutritionProduction = item.UnidadeProdutivaNutrição,
                    MarketLocalRegion = item.RegiãoMercadoLocal,
                    CustomerNo = item.NºCliente,
                    Approvers = item.Aprovadores,
                    Urgent = item.Urgente,
                    VATBusinessPostingGroup = item.GrupoRegistoIvanegocio,
                    VATProductPostingGroup = item.GrupoRegistoIvaproduto,
                    DiscountPercentage = item.PercentagemDesconto.HasValue ? item.PercentagemDesconto.Value : 0,
                    QuantidadeInicial = item.QuantidadeInicial.HasValue ? item.QuantidadeInicial.Value : 0,
                    SemEfeito = item.SemEfeito == null ? false : item.SemEfeito,
                    CustoUnitarioSubFornecedor = item.CustoUnitarioSubFornecedor
                };
            }
            return null;
        }

        public static List<RequisitionLineViewModel> ParseToViewModel(this List<LinhasRequisição> items)
        {
            List<RequisitionLineViewModel> parsedItems = new List<RequisitionLineViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }

        public static LinhasRequisição ParseToDB(this RequisitionLineViewModel item)
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
                    Descrição2 = item.Description2,
                    CódigoUnidadeMedida = item.UnitMeasureCode,
                    CódigoLocalização = item.LocalCode,
                    MercadoLocal = item.LocalMarket == null ? false : item.LocalMarket,
                    QuantidadeARequerer = item.QuantityToRequire,
                    QuantidadeRequerida = item.QuantityRequired,
                    QuantidadeADisponibilizar = item.QuantityToProvide,
                    QuantidadeDisponibilizada = item.QuantityAvailable,
                    QuantidadeAReceber = item.QuantityReceivable,
                    QuantidadeRecebida = item.QuantityReceived,
                    QuantidadePendente = item.QuantityPending,
                    QuantidadeDisponivel = item.QuantidadeDisponivel,
                    QuantidadeReservada = item.QuantidadeReservada,
                    CustoUnitário = item.UnitCost,
                    CustoUnitarioComIVA = item.UnitCostWithIVA,
                    DataReceçãoEsperada = string.IsNullOrEmpty(item.ExpectedReceivingDate) ? (DateTime?)null : DateTime.Parse(item.ExpectedReceivingDate),
                    Faturável= item.Billable == null ? false : item.Billable,
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
                    CriarNotaEncomenda = item.CriarNotaEncomenda == null ? false : item.CriarNotaEncomenda,
                    CriarConsultaMercado = item.CreateMarketSearch == null ? false : item.CreateMarketSearch,
                    EnviarPréCompra = item.SendPrePurchase == null ? false : item.SendPrePurchase,
                    EnviadoPréCompra = item.SubmitPrePurchase == null ? false : item.SubmitPrePurchase,
                    DataMercadoLocal = string.IsNullOrEmpty(item.LocalMarketDate) ? (DateTime?)null : DateTime.Parse(item.LocalMarketDate),
                    UserMercadoLocal = item.LocalMarketUser,
                    EnviadoParaCompras= item.SendForPurchase == null ? false : item.SendForPurchase,
                    DataEnvioParaCompras = string.IsNullOrEmpty(item.SendForPurchaseDate) ? (DateTime?)null : DateTime.Parse(item.SendForPurchaseDate),
                    ValidadoCompras = item.PurchaseValidated == null ? false : item.PurchaseValidated,
                    RecusadoCompras = item.PurchaseRefused == null ? false : item.PurchaseRefused,
                    MotivoRecusaMercLocal = item.ReasonToRejectionLocalMarket,
                    DataRecusaMercLocal = string.IsNullOrEmpty(item.RejectionLocalMarketDate) ? (DateTime?)null : DateTime.Parse(item.RejectionLocalMarketDate),
                    IdCompra= item.PurchaseId,
                    NºFornecedor= item.SupplierNo,
                    NoSubFornecedor = item.SubSupplierNo,
                    NºEncomendaAberto= item.OpenOrderNo,
                    NºLinhaEncomendaAberto= item.OpenOrderLineNo,
                    NºDeConsultaMercadoCriada= item.QueryCreatedMarketNo,
                    NºEncomendaCriada= item.CreatedOrderNo,
                    CódigoProdutoFornecedor= item.SupplierProductCode,
                    UnidadeProdutivaNutrição= item.UnitNutritionProduction,
                    RegiãoMercadoLocal= item.MarketLocalRegion,
                    NºCliente= item.CustomerNo,
                    Aprovadores= item.Approvers,
                    Urgente = item.Urgent,
                    GrupoRegistoIvanegocio = item.VATBusinessPostingGroup,
                    GrupoRegistoIvaproduto = item.VATProductPostingGroup,
                    PercentagemDesconto = item.DiscountPercentage.HasValue ? item.DiscountPercentage.Value : (decimal?)null,
                    QuantidadeInicial = item.QuantidadeInicial.HasValue ? item.QuantidadeInicial.Value : (decimal?)null,
                    SemEfeito = item.SemEfeito == null ? false : item.SemEfeito,
                    CustoUnitarioSubFornecedor = item.CustoUnitarioSubFornecedor
                };
            }
            return null;
        }

        public static List<LinhasRequisição> ParseToDB(this List<RequisitionLineViewModel> items)
        {
            List<LinhasRequisição> parsedItems = new List<LinhasRequisição>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
        #endregion








        #region HISTORICO
        public static RequisitionLineHistViewModel ParseHistToViewModel(this LinhasRequisiçãoHist item)
        {
            if (item != null)
            {
                return new RequisitionLineHistViewModel()
                {
                    RequestNo = item.NºRequisição,
                    LineNo = item.NºLinha,
                    Type = item.Tipo,
                    Code = item.Código,
                    Description = item.Descrição,
                    Description2 = item.Descrição2,
                    UnitMeasureCode = item.CódigoUnidadeMedida,
                    LocalCode = item.CódigoLocalização,
                    LocalMarket = item.MercadoLocal == null ? false : item.MercadoLocal,
                    QuantityToRequire = item.QuantidadeARequerer,
                    QuantityRequired = item.QuantidadeRequerida,
                    QuantityToProvide = item.QuantidadeADisponibilizar,
                    QuantityAvailable = item.QuantidadeDisponibilizada,
                    QuantityReceivable = item.QuantidadeAReceber,
                    QuantityReceived = item.QuantidadeRecebida,
                    QuantityPending = item.QuantidadePendente,
                    UnitCost = item.CustoUnitário,
                    ExpectedReceivingDate = !item.DataReceçãoEsperada.HasValue ? "" : item.DataReceçãoEsperada.Value.ToString("yyyy-MM-dd"),
                    Billable = item.Faturável == null ? false : item.Faturável,
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
                    CriarNotaEncomenda = item.CriarNotaEncomenda == null ? false : item.CriarNotaEncomenda,
                    CreateMarketSearch = item.CriarConsultaMercado == null ? false : item.CriarConsultaMercado,
                    SubmitPrePurchase = item.EnviadoPréCompra == null ? false : item.EnviadoPréCompra,
                    SendPrePurchase = item.EnviarPréCompra == null ? false : item.EnviarPréCompra,
                    LocalMarketDate = !item.DataMercadoLocal.HasValue ? "" : item.DataMercadoLocal.Value.ToString("yyyy-MM-dd"),
                    LocalMarketUser = item.UserMercadoLocal,
                    SendForPurchase = item.EnviadoParaCompras == null ? false : item.EnviadoParaCompras,
                    SendForPurchaseDate = !item.DataEnvioParaCompras.HasValue ? "" : item.DataEnvioParaCompras.Value.ToString("yyyy-MM-dd"),
                    PurchaseValidated = item.ValidadoCompras == null ? false : item.ValidadoCompras,
                    PurchaseRefused = item.RecusadoCompras == null ? false : item.RecusadoCompras,
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
                    Urgent = item.Urgente,
                    VATBusinessPostingGroup = item.GrupoRegistoIvanegocio,
                    VATProductPostingGroup = item.GrupoRegistoIvaproduto,
                    DiscountPercentage = item.PercentagemDesconto.HasValue ? item.PercentagemDesconto.Value : 0
                };
            }
            return null;
        }

        public static List<RequisitionLineHistViewModel> ParseHistToViewModel(this List<LinhasRequisiçãoHist> items)
        {
            List<RequisitionLineHistViewModel> parsedItems = new List<RequisitionLineHistViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseHistToViewModel()));
            return parsedItems;
        }

        public static LinhasRequisiçãoHist ParseHistToDB(this RequisitionLineHistViewModel item)
        {
            if (item != null)
            {
                return new LinhasRequisiçãoHist()
                {
                    NºRequisição = item.RequestNo,
                    NºLinha = item.LineNo.HasValue ? item.LineNo.Value : 0,
                    Tipo = item.Type,
                    Código = item.Code,
                    Descrição = item.Description,
                    Descrição2 = item.Description2,
                    CódigoUnidadeMedida = item.UnitMeasureCode,
                    CódigoLocalização = item.LocalCode,
                    MercadoLocal = item.LocalMarket == null ? false : item.LocalMarket,
                    QuantidadeARequerer = item.QuantityToRequire,
                    QuantidadeRequerida = item.QuantityRequired,
                    QuantidadeADisponibilizar = item.QuantityToProvide,
                    QuantidadeDisponibilizada = item.QuantityAvailable,
                    QuantidadeAReceber = item.QuantityReceivable,
                    QuantidadeRecebida = item.QuantityReceived,
                    QuantidadePendente = item.QuantityPending,
                    CustoUnitário = item.UnitCost,
                    DataReceçãoEsperada = string.IsNullOrEmpty(item.ExpectedReceivingDate) ? (DateTime?)null : DateTime.Parse(item.ExpectedReceivingDate),
                    Faturável = item.Billable == null ? false : item.Billable,
                    NºProjeto = item.ProjectNo,
                    CódigoRegião = item.RegionCode,
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
                    CriarNotaEncomenda = item.CriarNotaEncomenda == null ? false : item.CriarNotaEncomenda,
                    CriarConsultaMercado = item.CreateMarketSearch == null ? false : item.CreateMarketSearch,
                    EnviarPréCompra = item.SendPrePurchase == null ? false : item.SendPrePurchase,
                    EnviadoPréCompra = item.SubmitPrePurchase == null ? false : item.SubmitPrePurchase,
                    DataMercadoLocal = string.IsNullOrEmpty(item.LocalMarketDate) ? (DateTime?)null : DateTime.Parse(item.LocalMarketDate),
                    UserMercadoLocal = item.LocalMarketUser,
                    EnviadoParaCompras = item.SendForPurchase == null ? false : item.SendForPurchase,
                    DataEnvioParaCompras = string.IsNullOrEmpty(item.SendForPurchaseDate) ? (DateTime?)null : DateTime.Parse(item.SendForPurchaseDate),
                    ValidadoCompras = item.PurchaseValidated == null ? false : item.PurchaseValidated,
                    RecusadoCompras = item.PurchaseRefused == null ? false : item.PurchaseRefused,
                    MotivoRecusaMercLocal = item.ReasonToRejectionLocalMarket,
                    DataRecusaMercLocal = string.IsNullOrEmpty(item.RejectionLocalMarketDate) ? (DateTime?)null : DateTime.Parse(item.RejectionLocalMarketDate),
                    IdCompra = item.PurchaseId,
                    NºFornecedor = item.SupplierNo,
                    NºEncomendaAberto = item.OpenOrderNo,
                    NºLinhaEncomendaAberto = item.OpenOrderLineNo,
                    NºDeConsultaMercadoCriada = item.QueryCreatedMarketNo,
                    NºEncomendaCriada = item.CreatedOrderNo,
                    CódigoProdutoFornecedor = item.SupplierProductCode,
                    UnidadeProdutivaNutrição = item.UnitNutritionProduction,
                    RegiãoMercadoLocal = item.MarketLocalRegion,
                    NºCliente = item.CustomerNo,
                    Aprovadores = item.Approvers,
                    GrupoRegistoIvanegocio = item.VATBusinessPostingGroup,
                    GrupoRegistoIvaproduto = item.VATProductPostingGroup,
                    PercentagemDesconto = item.DiscountPercentage.HasValue ? item.DiscountPercentage.Value : (decimal?)null,
                    Urgente = item.Urgent
                };
            }
            return null;
        }

        public static List<LinhasRequisiçãoHist> ParseHistToDB(this List<RequisitionLineHistViewModel> items)
        {
            List<LinhasRequisiçãoHist> parsedItems = new List<LinhasRequisiçãoHist>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseHistToDB()));
            return parsedItems;
        }
        #endregion
    }
}
