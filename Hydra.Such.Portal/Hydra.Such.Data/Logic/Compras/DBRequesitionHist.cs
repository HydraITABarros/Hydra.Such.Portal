using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Compras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.Logic.ComprasML
{
    public class DBRequesitionHist
    {
        public static RequisiçãoHist GetByNo(string RequesitionNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RequisiçãoHist.Where(x => x.NºRequisição == RequesitionNo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static string GetByNoAndArea(string RequesitionNo, int area)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RequisiçãoHist.Where(x => x.NºRequisição == RequesitionNo).FirstOrDefault().NºRequisição;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<RequisiçãoHist> GetReqByUserAreaStatus(string UserName, List<RequisitionStates> status)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    var statusValues = status.Cast<int>().ToList();

                    return ctx.RequisiçãoHist.Where(x => x.UtilizadorCriação == UserName && statusValues.Contains(x.Estado.Value) && !x.ModeloDeRequisição.HasValue || !x.ModeloDeRequisição.Value).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<RequisiçãoHist> GetAll(string User, int area)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RequisiçãoHist.Where(x => x.UtilizadorCriação == User && x.Área == area).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static RequisiçãoHist Create(RequisiçãoHist ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.RequisiçãoHist.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool DeleteByRequesitionNo(string RequesitionNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.RequisiçãoHist.RemoveRange(ctx.RequisiçãoHist.Where(x => x.NºRequisição == RequesitionNo));
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static RequisiçãoHist Update(RequisiçãoHist ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.RequisiçãoHist.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }



        public static RequisiçãoHist ParseToDB(RequisitionHistViewModel x)
        {
            RequisiçãoHist result = new RequisiçãoHist()
            {
                NºRequisição = x.RequisitionNo,
                Área = x.Area,
                Estado = (int?)x.State,
                NºProjeto = x.ProjectNo,
                CódigoRegião = x.RegionCode,
                CódigoÁreaFuncional = x.FunctionalAreaCode,
                CódigoCentroResponsabilidade = x.CenterResponsibilityCode,
                CódigoLocalização = x.LocalCode,
                NºFuncionário = x.EmployeeNo,
                Viatura = x.Vehicle,
                DataReceção = x.ReceivedDate != null ? DateTime.Parse(x.ReceivedDate) : (DateTime?)null,
                Urgente = x.Urgent,
                Amostra = x.Sample,
                Anexo = x.Attachment,
                Imobilizado = x.Immobilized,
                CompraADinheiro = x.BuyCash,
                CódigoLocalRecolha = x.LocalCollectionCode,
                CódigoLocalEntrega = x.LocalDeliveryCode,
                Observações = x.Comments,
                ModeloDeRequisição = x.RequestModel,
                DataHoraCriação = x.CreateDate != null ? DateTime.Parse(x.CreateDate) : (DateTime?)null,
                UtilizadorCriação = x.CreateUser,
                DataHoraModificação = x.UpdateDate,
                UtilizadorModificação = x.UpdateUser,
                CabimentoOrçamental = x.RelatedSearches,
                Exclusivo = x.Exclusive,
                JáExecutado = x.AlreadyPerformed,
                Equipamento = x.Equipment,
                ReposiçãoDeStock = x.StockReplacement,
                Reclamação = x.Reclamation,
                NºRequisiçãoReclamada = x.RequestReclaimNo,
                ResponsávelCriação = x.ResponsibleCreation,
                ResponsávelAprovação = x.ResponsibleApproval,
                ResponsávelValidação = x.ResponsibleValidation,
                ResponsávelReceção = x.ResponsibleReception,
                DataAprovação = x.ApprovalDate,
                DataValidação = x.ValidationDate,
                UnidadeProdutivaAlimentação = x.UnitFoodProduction,
                RequisiçãoNutrição = x.RequestNutrition,
                RequisiçãoDetergentes = x.RequestforDetergents,
                NºProcedimentoCcp = x.ProcedureCcpNo,
                Aprovadores = x.Approvers,
                MercadoLocal = x.LocalMarket,
                RegiãoMercadoLocal = x.LocalMarketRegion,
                ReparaçãoComGarantia = x.RepairWithWarranty,
                Emm = x.Emm,
                DataEntregaArmazém = x.WarehouseDeliveryDate != null ? DateTime.Parse(x.WarehouseDeliveryDate) : (DateTime?)null,
                LocalDeRecolha = x.LocalCollection,
                MoradaRecolha = x.CollectionAddress,
                Morada2Recolha = x.Collection2Address,
                CódigoPostalRecolha = x.CollectionPostalCode,
                LocalidadeRecolha = x.CollectionLocality,
                ContatoRecolha = x.CollectionContact,
                ResponsávelReceçãoRecolha = x.CollectionResponsibleReception,
                LocalEntrega = x.LocalDelivery,
                MoradaEntrega = x.DeliveryAddress,
                Morada2Entrega = x.Delivery2Address,
                CódigoPostalEntrega = x.DeliveryPostalCode,
                LocalidadeEntrega = x.LocalityDelivery,
                ContatoEntrega = x.DeliveryContact,
                ResponsávelReceçãoReceção = x.ResponsibleReceptionReception,
                NºFatura = x.InvoiceNo,
                DataMercadoLocal = x.LocalMarketDate,
                DataRequisição = x.RequisitionDate != null ? DateTime.Parse(x.RequisitionDate) : (DateTime?)null,
                NºConsultaMercado = x.MarketInquiryNo,
                NºEncomenda = x.OrderNo,
                Orçamento = x.SentReqToAprove,
                ValorEstimado = x.EstimatedValue
            };
            return result;
        }

        public static RequisitionHistViewModel ParseToViewModel(RequisiçãoHist x)
        {
            RequisitionHistViewModel result = new RequisitionHistViewModel()
            {
                RequisitionNo = x.NºRequisição,
                Area = x.Área,
                State = (RequisitionStates?)x.Estado,
                ProjectNo = x.NºProjeto,
                RegionCode = x.CódigoRegião,
                FunctionalAreaCode = x.CódigoÁreaFuncional,
                CenterResponsibilityCode = x.CódigoCentroResponsabilidade,
                LocalCode = x.CódigoLocalização,
                EmployeeNo = x.NºFuncionário,
                Vehicle = x.Viatura,
                ReceivedDate = x.DataReceção.HasValue ? x.DataReceção.Value.ToString("yyyy-MM-dd") : "",
                Urgent = x.Urgente,
                Sample = x.Amostra,
                Attachment = x.Anexo,
                Immobilized = x.Imobilizado,
                BuyCash = x.CompraADinheiro,
                LocalCollectionCode = x.CódigoLocalRecolha,
                LocalDeliveryCode = x.CódigoLocalEntrega,
                Comments = x.Observações,
                RequestModel = x.ModeloDeRequisição,
                CreateUser = x.UtilizadorCriação,
                CreateDate = x.DataHoraCriação.HasValue ? x.DataHoraCriação.Value.ToString("yyyy-MM-dd") : "",
                UpdateUser = x.UtilizadorModificação,
                UpdateDate = x.DataHoraModificação,
                RelatedSearches = x.CabimentoOrçamental,
                Exclusive = x.Exclusivo,
                AlreadyPerformed = x.JáExecutado,
                Equipment = x.Equipamento,
                StockReplacement = x.ReposiçãoDeStock,
                Reclamation = x.Reclamação,
                RequestReclaimNo = x.NºRequisiçãoReclamada,
                ResponsibleCreation = x.ResponsávelCriação,
                ResponsibleApproval = x.ResponsávelAprovação,
                ResponsibleValidation = x.ResponsávelValidação,
                ResponsibleReception = x.ResponsávelReceção,
                ApprovalDate = x.DataAprovação,
                ValidationDate = x.DataValidação,
                UnitFoodProduction = x.UnidadeProdutivaAlimentação,
                RequestNutrition = x.RequisiçãoNutrição,
                RequestforDetergents = x.RequisiçãoDetergentes,
                ProcedureCcpNo = x.NºProcedimentoCcp,
                Approvers = x.Aprovadores,
                LocalMarket = x.MercadoLocal,
                LocalMarketRegion = x.RegiãoMercadoLocal,
                RepairWithWarranty = x.ReparaçãoComGarantia,
                Emm = x.Emm,
                WarehouseDeliveryDate = x.DataEntregaArmazém.HasValue ? x.DataEntregaArmazém.Value.ToString("yyyy-MM-dd") : "",
                LocalCollection = x.LocalDeRecolha,
                CollectionAddress = x.MoradaRecolha,
                Collection2Address = x.Morada2Recolha,
                CollectionPostalCode = x.CódigoPostalRecolha,
                CollectionLocality = x.LocalidadeRecolha,
                CollectionContact = x.ContatoRecolha,
                CollectionResponsibleReception = x.ResponsávelReceçãoRecolha,
                LocalDelivery = x.LocalEntrega,
                DeliveryAddress = x.MoradaEntrega,
                Delivery2Address = x.Morada2Entrega,
                DeliveryPostalCode = x.CódigoPostalEntrega,
                LocalityDelivery = x.LocalidadeEntrega,
                DeliveryContact = x.ContatoEntrega,
                ResponsibleReceptionReception = x.ResponsávelReceçãoReceção,
                InvoiceNo = x.NºFatura,
                LocalMarketDate = x.DataMercadoLocal,
                EstimatedValue = x.ValorEstimado,
                MarketInquiryNo = x.NºConsultaMercado,
                OrderNo = x.NºEncomenda,
                RequisitionDate = x.DataRequisição.HasValue ? x.DataRequisição.Value.ToString("yyyy-MM-dd") : ""
            };

            return result;
        }

        public static Requisição TransferToRequisition(RequisiçãoHist item)
        {
            if (item != null)
            {
                return new Requisição()
                {
                    NºRequisição = item.NºRequisição,
                    Área = item.Área,
                    Estado = item.Estado.HasValue ? (int)item.Estado.Value : (int?)null,
                    NºProjeto = item.NºProjeto,
                    CódigoRegião = item.CódigoRegião,
                    CódigoÁreaFuncional = item.CódigoÁreaFuncional,
                    CódigoCentroResponsabilidade = item.CódigoCentroResponsabilidade,
                    CódigoLocalização = item.CódigoLocalização,
                    NºFuncionário = item.NºFuncionário,
                    Viatura = item.Viatura,
                    DataReceção = item.DataReceção,
                    Urgente = item.Urgente,
                    Amostra = item.Amostra,
                    Anexo = item.Anexo,
                    Imobilizado = item.Imobilizado,
                    CompraADinheiro = item.CompraADinheiro,
                    CódigoLocalRecolha = item.CódigoLocalRecolha,
                    CódigoLocalEntrega = item.CódigoLocalEntrega,
                    Observações = item.Observações,
                    ModeloDeRequisição = item.ModeloDeRequisição,
                    DataHoraCriação = item.DataHoraCriação,
                    UtilizadorCriação = item.UtilizadorCriação,
                    DataHoraModificação = item.DataHoraModificação,
                    UtilizadorModificação = item.UtilizadorModificação,
                    CabimentoOrçamental = item.CabimentoOrçamental,
                    Exclusivo = item.Exclusivo,
                    JáExecutado = item.JáExecutado,
                    Equipamento = item.Equipamento,
                    ReposiçãoDeStock = item.ReposiçãoDeStock,
                    Reclamação = item.Reclamação,
                    NºRequisiçãoReclamada = item.NºRequisiçãoReclamada,
                    ResponsávelCriação = item.ResponsávelCriação,
                    ResponsávelAprovação = item.ResponsávelAprovação,
                    ResponsávelValidação = item.ResponsávelValidação,
                    ResponsávelReceção = item.ResponsávelReceção,
                    DataAprovação = item.DataAprovação,
                    DataValidação = item.DataValidação,
                    UnidadeProdutivaAlimentação = item.UnidadeProdutivaAlimentação,
                    RequisiçãoNutrição = item.RequisiçãoNutrição,
                    RequisiçãoDetergentes = item.RequisiçãoDetergentes,
                    NºProcedimentoCcp = item.NºProcedimentoCcp,
                    Aprovadores = item.Aprovadores,
                    MercadoLocal = item.MercadoLocal,
                    RegiãoMercadoLocal = item.RegiãoMercadoLocal,
                    ReparaçãoComGarantia = item.ReparaçãoComGarantia,
                    Emm = item.Emm,
                    DataEntregaArmazém = item.DataEntregaArmazém,
                    LocalDeRecolha = item.LocalDeRecolha,
                    MoradaRecolha = item.MoradaRecolha,
                    Morada2Recolha = item.Morada2Recolha,
                    CódigoPostalRecolha = item.CódigoPostalRecolha,
                    LocalidadeRecolha = item.LocalidadeRecolha,
                    ContatoRecolha = item.ContatoRecolha,
                    ResponsávelReceçãoRecolha = item.ResponsávelReceçãoRecolha,
                    LocalEntrega = item.LocalEntrega,
                    MoradaEntrega = item.MoradaEntrega,
                    Morada2Entrega = item.Morada2Entrega,
                    CódigoPostalEntrega = item.CódigoPostalEntrega,
                    LocalidadeEntrega = item.LocalidadeEntrega,
                    ContatoEntrega = item.ContatoEntrega,
                    ResponsávelReceçãoReceção = item.ResponsávelReceçãoReceção,
                    NºFatura = item.NºFatura,
                    DataMercadoLocal = item.DataMercadoLocal,
                    DataRequisição = item.DataRequisição,
                    NºConsultaMercado = item.NºConsultaMercado,
                    NºEncomenda = item.NºEncomenda,
                    Orçamento = item.Orçamento,
                    ValorEstimado = item.ValorEstimado,
                    PrecoIvaincluido = item.PrecoIvaincluido,
                    Adiantamento = item.Adiantamento,
                };
            }
            return null;
        }

    }
}
