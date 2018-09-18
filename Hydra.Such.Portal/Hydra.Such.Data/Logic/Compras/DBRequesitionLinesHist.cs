using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Compras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.ComprasML
{
    public class DBRequesitionLinesHist
    {
        #region CRUD
        public static LinhasRequisiçãoHist GetById(string RequisicionNo, int LineNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasRequisiçãoHist.Where(x => x.NºRequisição == RequisicionNo && x.NºLinha == LineNo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasRequisiçãoHist> GetByRequisitionId(string requisicao)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasRequisiçãoHist.Where(x => x.NºRequisição == requisicao).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<LinhasRequisiçãoHist> GetReqLinesByUserAreaStatus(string UserName)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasRequisiçãoHist.Where(x => x.UtilizadorCriação == UserName).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<LinhasRequisiçãoHist> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasRequisiçãoHist.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasRequisiçãoHist> GetAllByNo(string RequisicionNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasRequisiçãoHist.Where(x => x.NºRequisição == RequisicionNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static LinhasRequisiçãoHist Create(LinhasRequisiçãoHist ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasRequisiçãoHist.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool CreateMultiple(List<LinhasRequisiçãoHist> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(x => x.DataHoraCriação = DateTime.Now);
                    ctx.LinhasRequisiçãoHist.AddRange(items);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static LinhasRequisiçãoHist Update(LinhasRequisiçãoHist ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.LinhasRequisiçãoHist.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(LinhasRequisiçãoHist ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasRequisiçãoHist.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        #endregion

        public static bool DeleteAllFromReqNo(string RequisicionNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasRequisiçãoHist.RemoveRange(ctx.LinhasRequisiçãoHist.Where(x => x.NºRequisição == RequisicionNo).ToList());
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static RequisitionLineHistViewModel ParseToViewModel(LinhasRequisiçãoHist x)
        {
            return new RequisitionLineHistViewModel()
            {
                RequestNo = x.NºRequisição,
                LineNo = x.NºLinha,
                Type = x.Tipo,
                Code = x.Código,
                Description = x.Descrição,
                UnitMeasureCode = x.CódigoUnidadeMedida,
                LocalCode = x.CódigoLocalização,
                LocalMarket = x.MercadoLocal,
                QuantityToRequire = x.QuantidadeARequerer,
                QuantityRequired = x.QuantidadeRequerida,
                QuantityToProvide = x.QuantidadeADisponibilizar,
                QuantityAvailable = x.QuantidadeDisponibilizada,
                QuantityReceivable = x.QuantidadeAReceber,
                QuantityReceived = x.QuantidadeRecebida,
                QuantityPending = x.QuantidadePendente,
                UnitCost = x.CustoUnitário,
                ExpectedReceivingDate = x.DataReceçãoEsperada.HasValue ? x.DataReceçãoEsperada.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                Billable = x.Faturável,
                ProjectNo = x.NºProjeto,
                RegionCode = x.CódigoRegião,
                FunctionalAreaCode = x.CódigoÁreaFuncional,
                CenterResponsibilityCode = x.CódigoCentroResponsabilidade,
                FunctionalNo = x.NºFuncionário,
                Vehicle = x.Viatura,
                CreateDateTime = x.DataHoraCriação,
                CreateUser = x.UtilizadorCriação,
                UpdateDateTime = x.DataHoraModificação,
                UpdateUser = x.UtilizadorModificação,
                QtyByUnitOfMeasure = x.QtdPorUnidadeDeMedida,
                UnitCostsould = x.PreçoUnitárioVenda,
                BudgetValue = x.ValorOrçamento,
                MaintenanceOrderLineNo = x.NºLinhaOrdemManutenção,
                CreateMarketSearch = x.CriarConsultaMercado,
                SubmitPrePurchase = x.EnviarPréCompra,
                SendPrePurchase = x.EnviadoPréCompra,
                LocalMarketDate = x.DataMercadoLocal.HasValue ? x.DataMercadoLocal.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                LocalMarketUser = x.UserMercadoLocal,
                SendForPurchase = x.EnviadoParaCompras,
                SendForPurchaseDate = x.DataEnvioParaCompras.HasValue ? x.DataEnvioParaCompras.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                PurchaseValidated = x.ValidadoCompras,
                PurchaseRefused = x.RecusadoCompras,
                ReasonToRejectionLocalMarket = x.MotivoRecusaMercLocal,
                RejectionLocalMarketDate = x.DataRecusaMercLocal.HasValue ? x.DataRecusaMercLocal.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                PurchaseId = x.IdCompra,
                SupplierNo = x.NºFornecedor,
                OpenOrderNo = x.NºEncomendaAberto,
                OpenOrderLineNo = x.NºLinhaEncomendaAberto,
                QueryCreatedMarketNo = x.NºDeConsultaMercadoCriada,
                CreatedOrderNo = x.NºEncomendaCriada,
                SupplierProductCode = x.CódigoProdutoFornecedor,
                UnitNutritionProduction = x.UnidadeProdutivaNutrição,
                MarketLocalRegion = x.RegiãoMercadoLocal,
                CustomerNo = x.NºCliente,
                Approvers = x.Aprovadores,
                Selected = x.Urgente
            };
        }

        public static LinhasRequisiçãoHist ParseToDB(RequisitionLineHistViewModel x)
        {
            return new LinhasRequisiçãoHist()
            {
                 NºRequisição = x.RequestNo,
                 NºLinha = (int)x.LineNo,
                 Tipo = x.Type,
                 Código = x.Code,
                 Descrição = x.Description,
                 CódigoUnidadeMedida = x.UnitMeasureCode,
                 CódigoLocalização = x.LocalCode,
                 MercadoLocal = x.LocalMarket,
                 QuantidadeARequerer = x.QuantityToRequire,
                 QuantidadeRequerida = x.QuantityRequired,
                 QuantidadeADisponibilizar = x.QuantityToProvide,
                 QuantidadeDisponibilizada = x.QuantityAvailable,
                 QuantidadeAReceber = x.QuantityReceivable,
                 QuantidadeRecebida = x.QuantityReceived,
                 QuantidadePendente = x.QuantityPending,
                 CustoUnitário = x.UnitCost,
                 DataReceçãoEsperada = x.ExpectedReceivingDate != null && x.ExpectedReceivingDate != "" ? DateTime.Parse(x.ExpectedReceivingDate) : (DateTime?)null,
                 Faturável = x.Billable,
                 NºProjeto = x.ProjectNo,
                 CódigoRegião = x.RegionCode,
                 CódigoÁreaFuncional = x.FunctionalAreaCode,
                 CódigoCentroResponsabilidade = x.CenterResponsibilityCode,
                 NºFuncionário = x.FunctionalNo,
                 Viatura = x.Vehicle,
                 DataHoraCriação = x.CreateDateTime,
                 UtilizadorCriação = x.CreateUser,
                 DataHoraModificação = x.UpdateDateTime,
                 UtilizadorModificação = x.UpdateUser,
                 QtdPorUnidadeDeMedida = x.QtyByUnitOfMeasure,
                 PreçoUnitárioVenda = x.UnitCostsould,
                 ValorOrçamento = x.BudgetValue,
                 NºLinhaOrdemManutenção = x.MaintenanceOrderLineNo,
                 CriarConsultaMercado = x.CreateMarketSearch,
                 EnviarPréCompra = x.SubmitPrePurchase,
                 EnviadoPréCompra = x.SendPrePurchase,
                 DataMercadoLocal = x.LocalMarketDate != null && x.LocalMarketDate != "" ? DateTime.Parse(x.LocalMarketDate) : (DateTime?)null,
                 UserMercadoLocal = x.LocalMarketUser,
                 EnviadoParaCompras = x.SendForPurchase,
                 DataEnvioParaCompras = x.SendForPurchaseDate != null && x.SendForPurchaseDate != "" ? DateTime.Parse(x.SendForPurchaseDate) : (DateTime?)null,
                 ValidadoCompras = x.PurchaseValidated,
                 RecusadoCompras = x.PurchaseRefused,
                 MotivoRecusaMercLocal = x.ReasonToRejectionLocalMarket,
                 DataRecusaMercLocal = x.RejectionLocalMarketDate != null && x.RejectionLocalMarketDate != "" ? DateTime.Parse(x.RejectionLocalMarketDate) : (DateTime?)null,
                 IdCompra = x.PurchaseId,
                 NºFornecedor = x.SupplierNo,
                 NºEncomendaAberto = x.OpenOrderNo,
                 NºLinhaEncomendaAberto = x.OpenOrderLineNo,
                 NºDeConsultaMercadoCriada = x.QueryCreatedMarketNo,
                 NºEncomendaCriada = x.CreatedOrderNo,
                 CódigoProdutoFornecedor = x.SupplierProductCode,
                 UnidadeProdutivaNutrição = x.UnitNutritionProduction,
                 RegiãoMercadoLocal = x.MarketLocalRegion,
                 NºCliente = x.CustomerNo,
                 Aprovadores = x.Approvers,
                 Urgente = x.Selected
            };
        }
    }
}
