using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Compras;
using Microsoft.EntityFrameworkCore;

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

        public static List<Requisição> GetByState(RequisitionStates state)
        {
            try
            {
                int stateValue = (int)state;

                using (var ctx = new SuchDBContext())
                {
                    return ctx.Requisição
                        .Include("LinhasRequisição")
                        .Where(x => x.Estado == stateValue)
                        .ToList();
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
                        .Include("LinhasRequisição")
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

        public static Requisição Update(Requisição ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.Requisição.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static bool Delete(Requisição ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
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
        #endregion

        #region Parse Utilities
        public static RequisitionViewModel ParseToViewModel(this Requisição item)
        {
            if (item != null)
            {
                return new RequisitionViewModel()
                {
                    RequisitionNo = item.NºRequisição,
                    Area = item.Área,
                    //State = item.Estado,
                    State = item.Estado.HasValue && Enum.IsDefined(typeof(RequisitionStates), item.Estado.Value) ? (RequisitionStates)item.Estado.Value : (RequisitionStates?)null,
                    ProjectNo = item.NºProjeto,
                    RegionCode = item.CódigoRegião,
                    FunctionalAreaCode = item.CódigoÁreaFuncional,
                    CenterResponsibilityCode = item.CódigoCentroResponsabilidade,
                    LocalCode = item.CódigoLocalização,
                    EmployeeNo = item.NºFuncionário,
                    Vehicle = item.Viatura,
                    ReceivedDate = !item.DataReceção.HasValue ? "" : item.DataReceção.Value.ToString("yyyy-MM-dd"),
                    Urgent = item.Urgente,
                    Sample = item.Amostra,
                    Attachment = item.Anexo,
                    Immobilized = item.Imobilizado,
                    BuyCash = item.CompraADinheiro,
                    LocalCollectionCode = item.CódigoLocalRecolha,
                    LocalDeliveryCode = item.CódigoLocalEntrega,
                    Comments = item.Observações,
                    RequestModel = item.ModeloDeRequisição,
                    CreateUser = item.UtilizadorCriação,
                    CreateDate = item.DataHoraCriação,
                    UpdateUser = item.UtilizadorModificação,
                    UpdateDate = item.DataHoraModificação,
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
                    ValidationDate = item.DataValidação,
                    UnitFoodProduction = item.UnidadeProdutivaAlimentação,
                    RequestNutrition = item.RequisiçãoNutrição,
                    RequestforDetergents = item.RequisiçãoDetergentes,
                    ProcedureCcpNo = item.NºProcedimentoCcp,
                    Approvers = item.Aprovadores,
                    LocalMarket = item.MercadoLocal,
                    LocalMarketRegion = item.RegiãoMercadoLocal,
                    RepairWithWarranty = item.ReparaçãoComGarantia,
                    Emm = item.Emm,
                    WarehouseDeliveryDate = item.DataEntregaArmazém,
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
                    // EstimatedValue = item.,
                    MarketInquiryNo = item.NºConsultaMercado,
                    OrderNo = item.NºEncomenda,
                    RequisitionDate = item.DataRequisição,
                    //dimension = item.,
                    //Budget = item.,
                    Lines = item.LinhasRequisição.ToList().ParseToViewModel(),
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
            if (item != null)
            {
                return new Requisição()
                {
                    NºRequisição = item.RequisitionNo,
                    Área = item.Area,
                    //Estado = item.State.HasValue ? (int)item.State.Value : (int?)null,
                    Estado = item.State.HasValue ? (int)item.State.Value : (int?)null,
                    NºProjeto = item.ProjectNo,
                    CódigoRegião = item.RegionCode,
                    CódigoÁreaFuncional = item.FunctionalAreaCode,
                    CódigoCentroResponsabilidade = item.CenterResponsibilityCode,
                    CódigoLocalização = item.LocalCode,
                    NºFuncionário = item.EmployeeNo,
                    Viatura = item.Vehicle,
                    DataReceção = string.IsNullOrEmpty(item.ReceivedDate) ? (DateTime?)null : DateTime.Parse(item.ReceivedDate),
                    Urgente = item.Urgent,
                    Amostra = item.Sample,
                    Anexo = item.Attachment,
                    Imobilizado = item.Immobilized,
                    CompraADinheiro = item.BuyCash,
                    CódigoLocalRecolha = item.LocalCollectionCode,
                    CódigoLocalEntrega = item.LocalDeliveryCode,
                    Observações = item.Comments,
                    ModeloDeRequisição = item.RequestModel,
                    UtilizadorCriação = item.CreateUser,
                    DataHoraCriação = item.CreateDate,
                    UtilizadorModificação = item.UpdateUser,
                    DataHoraModificação = item.UpdateDate,
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
                    DataEntregaArmazém = item.WarehouseDeliveryDate,
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
                    // EstimatedValue = item.,
                    NºConsultaMercado = item.MarketInquiryNo,
                    NºEncomenda = item.OrderNo,
                    DataRequisição = item.RequisitionDate,
                    //dimension = item.,
                    //Budget = item.,
                };
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
        #endregion
    }
}
