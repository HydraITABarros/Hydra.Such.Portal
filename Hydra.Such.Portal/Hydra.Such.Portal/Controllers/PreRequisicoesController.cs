using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Data.Logic.Compras;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Request;
using Newtonsoft.Json.Linq;
using System.IO;
using Hydra.Such.Portal.Configurations;
using Microsoft.Extensions.Options;

namespace Hydra.Such.Portal.Controllers
{
    public class PreRequisicoesController : Controller
    {
        private readonly GeneralConfigurations _config;

        public PreRequisicoesController(IOptions<GeneralConfigurations> appSettings)
        {
            _config = appSettings.Value;
        }


        public IActionResult Index()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 10, 3);
            if (UPerm != null && UPerm.Read.Value)
            {

                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public JsonResult GetPreReqList([FromBody] int Area)
        {

            List<PréRequisição> PreRequisition = null;
            PreRequisition = DBPreRequesition.GetAll(User.Identity.Name, Area);
            
            List<PreRequesitionsViewModel> result = new List<PreRequesitionsViewModel>();

            PreRequisition.ForEach(x => result.Add(DBPreRequesition.ParseToViewModel(x)));
            return Json(result);
        }

        #region Pre Requesition Details
        [HttpPost]
        public JsonResult GetPreRequesitionDetails([FromBody] PreRequesitionsViewModel data)
        {
            if (data != null)
            {
                PreRequesitionsViewModel result = new PreRequesitionsViewModel();
                if (data.PreRequesitionsNo != "")
                {
                    PréRequisição PreRequisition = DBPreRequesition.GetByNo(data.PreRequesitionsNo);
                    result = DBPreRequesition.ParseToViewModel(PreRequisition);

                }
                return Json(result);

            }
            return Json(false);
        }
        
        public IActionResult PréRequisiçõesDetalhes(string PreRequesitionNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 10, 3);
            if (UPerm != null && UPerm.Read.Value)
            {
                
                ViewBag.PreRequesitionNo = PreRequesitionNo ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        #endregion

        #region Pre Requesition Lines
        [HttpPost]
        public JsonResult GetPreReqLines([FromBody] PreRequesitionsViewModel data)
        {
            if (data != null)
            {
                List<LinhasPréRequisição> PreRequesitionLines = DBPreRequesitionLines.GetAllByNo(data.PreRequesitionsNo);

                PreRequesitionLineHelperViewModel result = new PreRequesitionLineHelperViewModel
                {
                    PreRequisitionNo = data.PreRequesitionsNo,
                    Lines = new List<PreRequisitionLineViewModel>()
                };

                if (PreRequesitionLines != null)
                {
                    PreRequesitionLines.ForEach(x => result.Lines.Add(DBPreRequesitionLines.ParseToViewModel(x)));
                }
                return Json(result);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult UpdateContractLines([FromBody] PreRequesitionLineHelperViewModel data)
        {
            try
            {
                if (data != null)
                {
                    List<LinhasPréRequisição> PreRequesitionLines = DBPreRequesitionLines.GetAllByNo(data.PreRequisitionNo);
                    List<LinhasPréRequisição> CLToDelete = PreRequesitionLines.Where(y => !data.Lines.Any(x => x.PreRequisitionLineNo == y.NºPréRequisição && x.LineNo == y.NºLinha)).ToList();

                    CLToDelete.ForEach(x => DBPreRequesitionLines.Delete(x));

                    data.Lines.ForEach(x =>
                    {
                        LinhasPréRequisição CLine = PreRequesitionLines.Where(y => x.PreRequisitionLineNo == y.NºPréRequisição && x.LineNo == y.NºLinha).FirstOrDefault();

                        if (CLine != null)
                        {
                            CLine.NºPréRequisição = x.PreRequisitionLineNo;
                            CLine.NºLinha = x.LineNo;
                            CLine.Tipo = x.Type;
                            CLine.Código = x.Code;
                            CLine.Descrição = x.Description;
                            CLine.CódigoLocalização = x.LocalCode;
                            CLine.CódigoUnidadeMedida = x.UnitMeasureCode;
                            CLine.QuantidadeARequerer = x.QuantityToRequire;
                            CLine.CódigoRegião = x.RegionCode;
                            CLine.CódigoÁreaFuncional = x.FunctionalAreaCode;
                            CLine.CódigoCentroResponsabilidade = x.CenterResponsibilityCode;
                            CLine.NºProjeto = x.ProjectNo;
                            CLine.DataHoraCriação = x.CreateDateTime != null && x.CreateDateTime != "" ? DateTime.Parse(x.CreateDateTime) : (DateTime?)null;
                            CLine.UtilizadorCriação = x.CreateUser;
                            CLine.DataHoraModificação = DateTime.Now;
                            CLine.UtilizadorModificação = User.Identity.Name;
                            CLine.QtdPorUnidadeMedida = x.QtyByUnitOfMeasure;
                            CLine.QuantidadeRequerida = x.QuantityRequired;
                            CLine.QuantidadePendente = x.QuantityPending;
                            CLine.CustoUnitário = x.UnitCost;
                            CLine.PreçoUnitárioVenda = x.SellUnityPrice;
                            CLine.ValorOrçamento = x.BudgetValue;
                            CLine.DataReceçãoEsperada = x.ExpectedReceivingDate != null && x.ExpectedReceivingDate != "" ? DateTime.Parse(x.ExpectedReceivingDate) : (DateTime?)null;
                            CLine.Faturável = x.Billable;
                            CLine.NºLinhaOrdemManutenção = x.MaintenanceOrderLineNo;
                            CLine.NºFuncionário = x.EmployeeNo;
                            CLine.Viatura = x.Vehicle;
                            CLine.NºFornecedor = x.SupplierNo;
                            CLine.CódigoProdutoFornecedor = x.SupplierProductCode;
                            
                            CLine.UnidadeProdutivaNutrição = x.UnitNutritionProduction;
                            CLine.NºCliente = x.CustomerNo;
                            CLine.NºEncomendaAberto = x.OpenOrderNo;
                            CLine.NºLinhaEncomendaAberto = x.OpenOrderLineNo;

                            DBPreRequesitionLines.Update(CLine);
                        }
                        else
                        {
                            x = DBPreRequesitionLines.ParseToViewModel(DBPreRequesitionLines.Create(DBPreRequesitionLines.ParseToDB(x)));
                        }
                    });

                    data.eReasonCode = 1;
                    data.eMessage = "Linhas de Pré-Requisição atualizadas com sucesso.";
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao atualizar as linhas de Pré-Requisição.";
            }
            return Json(data);
        }
        #endregion

        #region CRUD
        [HttpPost]
        public JsonResult CreatePreRequesition([FromBody] PreRequesitionsViewModel data)
        {
            try
            {
                if (data != null)
                {
                    //Get Contract Numeration
                    Configuração Configs = DBConfigurations.GetById(1);
                    int ProjectNumerationConfigurationId = 0;
                    ProjectNumerationConfigurationId = Configs.NumeraçãoPréRequisições.Value;


                    data.PreRequesitionsNo = DBNumerationConfigurations.GetNextNumeration(ProjectNumerationConfigurationId, (data.PreRequesitionsNo == "" || data.PreRequesitionsNo == null));

                    if (data.PreRequesitionsNo != null)
                    {
                        PréRequisição pPreRequisicao = DBPreRequesition.ParseToDB(data);
                        pPreRequisicao.UtilizadorCriação = User.Identity.Name;
                        pPreRequisicao.DataHoraCriação = DateTime.Now;

                        //Create Contract On Database

                        pPreRequisicao = DBPreRequesition.Create(pPreRequisicao);

                        if (pPreRequisicao == null)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Ocorreu um erro ao criar o contrato.";
                        }
                        else
                        {
                            //Update Last Numeration Used
                            ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);
                            ConfigNumerations.ÚltimoNºUsado = data.PreRequesitionsNo;
                            ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                            DBNumerationConfigurations.Update(ConfigNumerations);

                            data.eReasonCode = 1;
                        }
                    }
                    else
                    {
                        data.eReasonCode = 5;
                        data.eMessage = "A numeração configurada não é compativel com a inserida.";
                    }
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 4;
                data.eMessage = "Ocorreu um erro ao criar o contrato";
            }
            return Json(data);

        }

        [HttpPost]
        public JsonResult UpdatePreRequesition([FromBody] PreRequesitionsViewModel data)
        {
            try
            {
                if (data != null)
                {
                    if (data.PreRequesitionsNo != null)
                    {
                        //Contratos cContract = DBContracts.ParseToDB(data);
                        PréRequisição PreRequisicaoDB = DBPreRequesition.GetByNo(data.PreRequesitionsNo);


                        if (PreRequisicaoDB != null)
                        {
                            PreRequisicaoDB.NºPréRequisição = data.PreRequesitionsNo;
                            PreRequisicaoDB.Área = data.Area;
                            PreRequisicaoDB.TipoRequisição = data.RequesitionType;
                            PreRequisicaoDB.NºProjeto = data.ProjectNo;
                            PreRequisicaoDB.CódigoRegião = data.RegionCode;
                            PreRequisicaoDB.CódigoÁreaFuncional = data.FunctionalAreaCode;
                            PreRequisicaoDB.CódigoCentroResponsabilidade = data.ResponsabilityCenterCode;
                            PreRequisicaoDB.Urgente = data.Urgent;
                            PreRequisicaoDB.Amostra = data.Sample;
                            PreRequisicaoDB.Anexo = data.Attachment;
                            PreRequisicaoDB.Imobilizado = data.Immobilized;
                            PreRequisicaoDB.CompraADinheiro = data.MoneyBuy;
                            PreRequisicaoDB.CódigoLocalRecolha = data.LocalCollectionCode;
                            PreRequisicaoDB.CódigoLocalEntrega = data.LocalDeliveryCode;
                            PreRequisicaoDB.Observações = data.Notes;
                            PreRequisicaoDB.ModeloDePréRequisição = data.PreRequesitionModel;
                            PreRequisicaoDB.DataHoraCriação = data.CreateDateTime;
                            PreRequisicaoDB.UtilizadorCriação = data.CreateUser;
                            PreRequisicaoDB.DataHoraModificação = data.UpdateDateTime;
                            PreRequisicaoDB.UtilizadorModificação = data.UpdateUser;
                            PreRequisicaoDB.Exclusivo = data.Exclusive;
                            PreRequisicaoDB.JáExecutado = data.AlreadyExecuted;
                            PreRequisicaoDB.Equipamento = data.Equipment;
                            PreRequisicaoDB.ReposiçãoDeStock = data.StockReplacement;
                            PreRequisicaoDB.Reclamação = data.Complaint;
                            PreRequisicaoDB.CódigoLocalização = data.LocationCode;
                            PreRequisicaoDB.CabimentoOrçamental = data.FittingBudget;
                            PreRequisicaoDB.NºFuncionário = data.EmployeeNo;
                            PreRequisicaoDB.Viatura = data.Vehicle;
                            PreRequisicaoDB.NºRequesiçãoReclamada = data.ClaimedRequesitionNo;
                            PreRequisicaoDB.ResponsávelCriação = data.CreateResponsible;
                            PreRequisicaoDB.ResponsávelAprovação = data.ApprovalResponsible;
                            PreRequisicaoDB.ResponsávelValidação = data.ValidationResponsible;
                            PreRequisicaoDB.ResponsávelReceção = data.ReceptionResponsible;
                            PreRequisicaoDB.DataAprovação = data.ApprovalDate;
                            PreRequisicaoDB.DataValidação = data.ValidationDate;
                            PreRequisicaoDB.DataReceção = data.ReceptionDate != "" && data.ReceptionDate != null ? DateTime.Parse(data.ReceptionDate) : (DateTime?)null;
                            PreRequisicaoDB.UnidadeProdutivaAlimentação = data.FoodProductionUnit;
                            PreRequisicaoDB.RequisiçãoNutrição = data.NutritionRequesition;
                            PreRequisicaoDB.RequisiçãoDetergentes = data.DetergentsRequisition;
                            PreRequisicaoDB.NºProcedimentoCcp = data.CCPProcedureNo;
                            PreRequisicaoDB.Aprovadores = data.Approvers;
                            PreRequisicaoDB.MercadoLocal = data.LocalMarket;
                            PreRequisicaoDB.RegiãoMercadoLocal = data.LocalMarketRegion;
                            PreRequisicaoDB.ReparaçãoComGarantia = data.WarrantyRepair;
                            PreRequisicaoDB.Emm = data.EMM;
                            PreRequisicaoDB.DataEntregaArmazém = data.DeliveryWarehouseDate != "" && data.DeliveryWarehouseDate != null ? DateTime.Parse(data.DeliveryWarehouseDate) : (DateTime?)null;
                            PreRequisicaoDB.LocalDeRecolha = data.CollectionLocal;
                            PreRequisicaoDB.MoradaRecolha = data.CollectionAddress;
                            PreRequisicaoDB.Morada2Recolha = data.CollectionAddress2;
                            PreRequisicaoDB.CodigoPostalRecolha = data.CollectionPostalCode;
                            PreRequisicaoDB.LocalidadeRecolha = data.CollectionLocality;
                            PreRequisicaoDB.ContatoRecolha = data.CollectionContact;
                            PreRequisicaoDB.ResponsávelReceçãoRecolha = data.CollectionReceptionResponsible;
                            PreRequisicaoDB.LocalEntrega = data.DeliveryLocal;
                            PreRequisicaoDB.MoradaEntrega = data.DeliveryAddress;
                            PreRequisicaoDB.Morada2Entrega = data.DeliveryAddress2;
                            PreRequisicaoDB.CódigoPostalEntrega = data.DeliveryPostalCode;
                            PreRequisicaoDB.LocalidadeEntrega = data.DeliveryLocality;
                            PreRequisicaoDB.ContatoEntrega = data.DeliveryContact;
                            PreRequisicaoDB.ResponsávelReceçãoReceção = data.ReceptionReceptionResponsible;
                            PreRequisicaoDB.NºFatura = data.InvoiceNo;

                            PreRequisicaoDB = DBPreRequesition.Update(PreRequisicaoDB);
                        }
                        data.eReasonCode = 1;
                        data.eMessage = "Contrato atualizado com sucesso.";
                    }
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao atualizar o contrato.";
            }
            return Json(data);

        }

        [HttpPost]
        public JsonResult DeletePreRequesition([FromBody] PreRequesitionsViewModel data)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                if (data != null)
                {
                    //Verify if contract have Invoices Or Projects
                    List<LinhasPréRequisição> lines = DBPreRequesitionLines.GetAllByNo(data.PreRequesitionsNo);
                    foreach(var linestodelete in lines)
                    {
                        DBPreRequesitionLines.Delete(linestodelete);
                    }
                    // Delete Contract 
                    DBPreRequesition.DeleteByPreRequesitionNo(data.PreRequesitionsNo);
                    
                    result.eReasonCode = 1;
                    result.eMessage = "Contrato eliminado com sucesso.";
                    //}

                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 2;
                result.eMessage = "Ocorreu um erro ao eliminar o contrato.";
            }
            return Json(result);

        }
        #endregion

        #region Populate CB
        public JsonResult CBVehicleFleetBool([FromBody] int id)
        {
            bool? FleetBool = DBRequesitionType.GetById(id).Frota;
            return Json(FleetBool);
        }

        public JsonResult GetPlaceData([FromBody] int placeId)
        {
            PlacesViewModel PlacesData = DBPlaces.ParseToViewModel(DBPlaces.GetById(placeId));
            return Json(PlacesData);
        }
        #endregion

        #region Numeração Pré-Requisição
        [HttpPost]
        public JsonResult ValidateNumeration([FromBody] PreRequesitionsViewModel data)
        {
            //Get Project Numeration
            Configuração Cfg = DBConfigurations.GetById(1);
            int ProjectNumerationConfigurationId = 0;
            ProjectNumerationConfigurationId = Cfg.NumeraçãoPréRequisições.Value;

            ConfiguraçãoNumerações CfgNumeration = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);

            //Validate if ProjectNo is valid
            if (!(data.PreRequesitionsNo == "" || data.PreRequesitionsNo == null) && !CfgNumeration.Manual.Value)
            {
                return Json("A numeração configurada para pré-requisições não permite inserção manual.");
            }
            else if (data.PreRequesitionsNo == "" && !CfgNumeration.Automático.Value)
            {
                return Json("É obrigatório inserir o Nº Pré-Requisição.");
            }

            return Json("");
        }
        #endregion

        #region Requesition Model Lines
        
        public IActionResult RequisiçõesModeloLista(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 10, 3);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.PreReqNo = id;
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public JsonResult GetReqModelList()
        {
            List<Requisição> RequisitionModel = null;
            RequisitionModel = DBRequest.GetReqModel();


            List<RequisitionViewModel> result = new List<RequisitionViewModel>();

            RequisitionModel.ForEach(x => result.Add(DBRequest.ParseToViewModel(x)));
            return Json(result);
        }

        public JsonResult CopyReqModelLines([FromBody] RequisitionViewModel data, string id)
        {
            List<LinhasRequisição> RequesitionLines = new List<LinhasRequisição>();
            RequesitionLines = DBRequestLine.GetAllByRequisiçãos(data.RequisitionNo);

            List<RequisitionLineViewModel> result = new List<RequisitionLineViewModel>();
            RequesitionLines.ForEach(x => result.Add(DBRequestLine.ParseToViewModel(x)));
            
            try
            {
                foreach (var line in result)
                {
                    LinhasPréRequisição newlines = new LinhasPréRequisição();

                    newlines.NºPréRequisição = data.PreRequisitionNo;
                    newlines.CódigoLocalização = line.LocalCode;
                    newlines.CódigoProdutoFornecedor = line.SupplierProductCode;
                    newlines.Descrição = line.Description;
                    newlines.CódigoUnidadeMedida = line.UnitMeasureCode;
                    newlines.QuantidadeARequerer = line.QuantityToRequire;
                    newlines.CustoUnitário = line.UnitCost;
                    newlines.NºProjeto = line.ProjectNo;
                    newlines.NºLinhaOrdemManutenção = line.MaintenanceOrderLineNo;
                    newlines.Viatura = line.Vehicle;
                    newlines.NºFornecedor = line.SupplierNo;
                    newlines.CódigoRegião = line.RegionCode;
                    newlines.CódigoÁreaFuncional = line.FunctionalAreaCode;
                    newlines.CódigoCentroResponsabilidade = line.CenterResponsibilityCode;
                    newlines.NºEncomendaAberto = line.OpenOrderNo;
                    newlines.NºLinhaEncomendaAberto = line.OpenOrderLineNo;
                    DBPreRequesitionLines.Create(newlines);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
            return Json(result);
        }

        public JsonResult GetPendingReq([FromBody] JObject requestParams)
        {
            int AreaNo = int.Parse(requestParams["AreaNo"].ToString());

            List<Requisição> RequisitionModel = null;
            RequisitionModel = DBRequest.GetReqByUserAreaStatus(User.Identity.Name, AreaNo, (int)RequisitionStates.Pending);
            
            List<RequisitionViewModel> result = new List<RequisitionViewModel>();

            RequisitionModel.ForEach(x => result.Add(DBRequest.ParseToViewModel(x)));
            return Json(result);

        }

        public JsonResult GetPendingReqLines([FromBody] JObject requestParams)
        {
            string ReqNo = requestParams["ReqNo"].ToString();

            List<LinhasRequisição> RequisitionLines = null;
            RequisitionLines = DBRequestLine.GetAllByRequisiçãos(ReqNo);

            List<RequisitionLineViewModel> result = new List<RequisitionLineViewModel>();

            RequisitionLines.ForEach(x => result.Add(DBRequestLine.ParseToViewModel(x)));
            return Json(result);

        }
        #endregion

        #region Requisition
        
        [HttpPost]
        public JsonResult CreateRequesition([FromBody] PreRequesitionsViewModel data)
        {
            try
            {
                if (data != null)
                {
                    List<LinhasPréRequisição> PreRequesitionLines = DBPreRequesitionLines.GetAllByNo(data.PreRequesitionsNo);
                    data.eMessage = "";

                    if (data.Complaint == true && (data.ClaimedRequesitionNo == "" || data.ClaimedRequesitionNo == null))
                    {
                        data.eReasonCode = 2;
                        data.eMessage = "O campo Nº Requisição Reclamada deve ser preenchido.";
                        return Json(data);
                    }
                   
                    if (data.MoneyBuy == true)
                    {
                        if(PreRequesitionLines != null)
                        {
                            foreach (var lines in PreRequesitionLines)
                            {
                                if (lines.CustoUnitário == null || (lines.NºFornecedor == null || lines.NºFornecedor == ""))
                                {
                                    data.eReasonCode = 3;
                                    data.eMessage = "Os campos Custo Unitário e Nº Fornecedor das linhas devem ser todos preenchidos.";
                                    return Json(data);
                                }
                            }
                        }
                    }

                    if (data.Equipment == true)
                    {
                        if (data.CollectionLocal == null || String.IsNullOrEmpty(data.CollectionAddress) || String.IsNullOrEmpty(data.CollectionPostalCode) || String.IsNullOrEmpty(data.CollectionLocality) || String.IsNullOrEmpty(data.CollectionContact) || String.IsNullOrEmpty(data.CollectionReceptionResponsible))
                        {
                            data.eReasonCode = 4;
                            data.eMessage = "Os campos de Recolha devem ser todos preenchidos.";
                            return Json(data);
                        }
                        else if (data.DeliveryLocal == null || String.IsNullOrEmpty(data.DeliveryAddress) || String.IsNullOrEmpty(data.DeliveryPostalCode) || String.IsNullOrEmpty(data.DeliveryLocality) || String.IsNullOrEmpty(data.CollectionReceptionResponsible) || String.IsNullOrEmpty(data.InvoiceNo))
                        {
                            data.eReasonCode = 4;
                            data.eMessage = "Os campos de Entrega (Fornecedor) devem ser todos preenchidos.";
                            return Json(data);
                        }
                    }

                    List<PreRequisitionLineViewModel> GroupedList = new List<PreRequisitionLineViewModel>();
                    PreRequesitionLines.ForEach(x => GroupedList.Add(DBPreRequesitionLines.ParseToViewModel(x)));
                                        
                    List<RequisitionViewModel> newlist = GroupedList.GroupBy(
                        x => x.LocalCode,
                        x => x,
                        (key, items) => new RequisitionViewModel
                        {
                            RequestReclaimNo = data.ClaimedRequesitionNo,
                            Urgent = data.Urgent,
                            Area = data.Area,
                            Immobilized = data.Immobilized,
                            Exclusive = data.Exclusive,
                            AlreadyPerformed = data.AlreadyExecuted,
                            Sample = data.Sample,
                            Equipment = data.Equipment,
                            BuyCash = data.MoneyBuy,
                            StockReplacement = data.StockReplacement,
                            Reclamation = data.Complaint,
                            RegionCode = data.RegionCode,
                            FunctionalAreaCode = data.FunctionalAreaCode,
                            CenterResponsibilityCode = data.ResponsabilityCenterCode,
                            Vehicle = data.Vehicle,
                            ProjectNo = data.ProjectNo,
                            ReceivedDate = data.ReceptionDate,
                            Comments = data.Notes,
                            RepairWithWarranty = data.WarrantyRepair,
                            Emm = data.EMM,
                            WarehouseDeliveryDate = data.DeliveryWarehouseDate,
                            LocalCollection = data.CollectionLocal,
                            CollectionAddress = data.CollectionAddress,
                            CollectionPostalCode = data.CollectionPostalCode,
                            CollectionLocality = data.CollectionLocality,
                            CollectionContact = data.CollectionContact,
                            CollectionResponsibleReception = data.CollectionReceptionResponsible,
                            LocalDelivery = data.DeliveryLocal,
                            DeliveryAddress = data.DeliveryAddress,
                            DeliveryPostalCode = data.DeliveryPostalCode, 
                            LocalityDelivery = data.DeliveryLocality,
                            ResponsibleReceptionReception = data.ReceptionReceptionResponsible,
                            InvoiceNo = data.InvoiceNo,
                            State = RequisitionStates.Pending,
                            CreateUser = User.Identity.Name,
                            
                            Lines = items.Select(line => new RequisitionLineViewModel()
                            {
                                
                                LocalCode = line.LocalCode,
                                SupplierProductCode = line.SupplierProductCode,
                                Description = line.Description,
                                UnitMeasureCode = line.UnitMeasureCode,
                                QuantityToRequire = line.QuantityToRequire,
                                UnitCost = line.UnitCost,
                                ProjectNo = line.ProjectNo,
                                MaintenanceOrderLineNo = line.MaintenanceOrderLineNo,
                                Vehicle = line.Vehicle,
                                SupplierNo = line.SupplierNo,
                                RegionCode = line.RegionCode,
                                FunctionalAreaCode = line.FunctionalAreaCode,
                                CenterResponsibilityCode = line.CenterResponsibilityCode,
                                OpenOrderNo = line.OpenOrderNo,
                                OpenOrderLineNo = line.OpenOrderLineNo,
                            }).ToList()
                        }).ToList();
                    
                    foreach(var req in newlist)
                    {
                        //Get Contract Numeration
                        Configuração Configs = DBConfigurations.GetById(1);
                        int ProjectNumerationConfigurationId = 0;
                        ProjectNumerationConfigurationId = Configs.NumeraçãoRequisições.Value;

                        string RequisitionNo = DBNumerationConfigurations.GetNextNumeration(ProjectNumerationConfigurationId, true);
                        if (RequisitionNo != null)
                        {
                            req.RequisitionNo = RequisitionNo;
                            Requisição createReq = DBRequest.ParseToDB(req);

                            DBRequest.Create(createReq);
                            if(createReq.NºRequisição != null)
                            {
                                //Update Last Numeration Used
                                ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);
                                ConfigNumerations.ÚltimoNºUsado = RequisitionNo;
                                ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                                DBNumerationConfigurations.Update(ConfigNumerations);

                                data.eReasonCode = 1;
                                data.eMessage = "Requisições criadas com sucesso";
                                
                            }
                            else
                            {
                                DBRequest.Delete(createReq);
                                data.eReasonCode = 0;
                                data.eMessage = "Ocorreu um erro ao criar o requisição.";
                            }
                        }
                        else
                        {
                            data.eReasonCode = 0;
                            data.eMessage = "A numeração configurada não é compativel com a inserida.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 0;
                data.eMessage = "Ocorreu um erro ao criar o requisição.";
            }

            return Json(data);
        }

        #endregion

        #region Numeração Requisição
        [HttpPost]
        public JsonResult ValidateNumerationReq([FromBody] PreRequesitionsViewModel data)
        {
            //Get Project Numeration
            Configuração Cfg = DBConfigurations.GetById(1);
            int ProjectNumerationConfigurationId = 0;
            ProjectNumerationConfigurationId = Cfg.NumeraçãoRequisições.Value;

            ConfiguraçãoNumerações CfgNumeration = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);

            //Validate if ProjectNo is valid
            if (!CfgNumeration.Automático.Value)
            {
                return Json("É obrigatório inserir o Nº Requisição.");
            }

            return Json("");
        }


        #endregion

        #region Attachments
        [HttpPost]
        [Route("PreRequisicoes/FileUpload")]
        [Route ("PreRequisicoes/FileUpload/{id}/{linha}")]
        public JsonResult FileUpload(string id, int linha)
        {
            try
            {
                var files = Request.Form.Files;
                string full_filename;
                foreach (var file in files)
                {
                    try
                    {
                        string filename = Path.GetFileName(file.FileName);
                        full_filename = id + "_" + filename;
                        var path = Path.Combine(_config.FileUploadFolder, full_filename);
                        using (FileStream dd = new FileStream(path, FileMode.CreateNew))
                        {
                            file.CopyTo(dd);

                            Anexos newfile = new Anexos();
                            newfile.NºOrigem = id;
                            newfile.UrlAnexo = full_filename;
                            newfile.TipoOrigem = 1;
                            newfile.DataHoraCriação = DateTime.Now;
                            newfile.UtilizadorCriação = User.Identity.Name;

                            DBAttachments.Create(newfile);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json("");
        }

        [HttpPost]
        public JsonResult LoadAttachments([FromBody] JObject requestParams)
        {
            string id = requestParams["id"].ToString();
            //string line = requestParams["linha"].ToString();
            //int lineNo = Int32.Parse(line);

            List<Anexos> list = DBAttachments.GetById(id);
            List<AttachmentsViewModel> attach = new List<AttachmentsViewModel>();
            list.ForEach(x => attach.Add(DBAttachments.ParseToViewModel(x)));
            return Json(attach);
        }

        [HttpGet]
        public FileStreamResult DownloadFile(string id)
        {
            return new FileStreamResult(new FileStream(_config.FileUploadFolder + id, FileMode.Open), "application/xlsx");
        }

        
        [HttpPost]
        public JsonResult DeleteAttachments([FromBody] AttachmentsViewModel requestParams)
        {
            try
            {
                System.IO.File.Delete(_config.FileUploadFolder + requestParams.Url);
                DBAttachments.Delete(DBAttachments.ParseToDB(requestParams));
                requestParams.eReasonCode = 1;

            }
            catch (Exception ex)
            {
                requestParams.eReasonCode = 2;
                return Json(requestParams);
            }
            return Json(requestParams);
        }
        #endregion
    }
}