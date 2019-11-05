using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Nutrition;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Nutrition;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Portal.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Hydra.Such.Portal.Controllers
{

    public class RequisicoesSimplificadasController : Controller
    {
        ProjetosController register;
        private readonly NAVWSConfigurations configws;
        private ErrorHandler mensage = new ErrorHandler();
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly GeneralConfigurations _generalConfig;

        public RequisicoesSimplificadasController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IHostingEnvironment _hostingEnvironment, IOptions<GeneralConfigurations> appSettingsGeneral)
        {
            this.configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
            _generalConfig = appSettingsGeneral.Value;

            register = new ProjetosController(appSettings, NAVWSConfigs, _hostingEnvironment, appSettingsGeneral);
        }

        public IActionResult Index(int option)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.RequisiçõesSimplificadas);

            if (UPerm != null && UPerm.Read.Value)
            {
                HttpContext.Session.Remove("aprovadoSession");
                ViewBag.RequisitionsApprovals = "false";
                //‘Requisições simplificadas para Registar’ com estado aprovado
                if (option == 1)
                {
                    ViewBag.Option = "registar";
                }
                //Histórico Requisições simplificadas
                else if (option == 2)
                {
                    ViewBag.Option = "historico";
                }
                else
                {
                    ViewBag.Option = "";
                }
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                //return RedirectToAction("AccessDenied", "Error");
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        public IActionResult Detalhes(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.RequisiçõesSimplificadas);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Approval = HttpContext.Session.GetString("aprovadoSession") ?? "";
                ViewBag.User = User.Identity.Name;
                //Registar requisições aprovadas
                if (ViewBag.Approval == "registar")
                {
                    ViewBag.LockFields = true;
                    UPerm.Create = false;
                    ViewBag.Option = "registar";
                }
                //Histórico requisições aprovadas
                else if (ViewBag.Approval == "historico")
                {
                    ViewBag.LockFields = true;
                    UPerm.Update = false;
                    UPerm.Create = false;
                    UPerm.Delete = false;
                    ViewBag.Option = "historico";
                }
                else
                {
                    ViewBag.LockFields = false;
                    ViewBag.Option = "";
                }
                HttpContext.Session.Remove("aprovadoSession");
                ViewBag.RequestNo = id ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                //return RedirectToAction("AccessDenied", "Error");
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        [HttpPost]
        public JsonResult SimplifiedRequisitionsPage([FromBody] string option)
        {
            List<SimplifiedRequisitionViewModel> result;

            //‘Requisições simplificadas para Registar’ com estado aprovado
            if (option == "registar")
            {
                result = DBSimplifiedRequisitions.ParseToViewModel(DBSimplifiedRequisitions.GetByApprovals(2));
                HttpContext.Session.SetString("aprovadoSession", option);
            }
            //Histórico de Requisições simplificadas
            else if (option == "historico")
            {
                result = DBSimplifiedRequisitions.ParseToViewModel(DBSimplifiedRequisitions.GetByApprovals(3));
                HttpContext.Session.SetString("aprovadoSession", option);
            }
            //‘Requisições simplificadas’ com utilizador
            else
            {
                HttpContext.Session.SetString("aprovadoSession", "");
                result = DBSimplifiedRequisitions.ParseToViewModel(DBSimplifiedRequisitions.GetByCreateResponsiblePendente(User.Identity.Name));
            }
            if (result != null)
            {
                //Apply User Dimensions Validations
                List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                //Regions
                if (userDimensions.Where(y => y.Dimensão == (int)Enumerations.Dimensions.Region).Count() > 0)
                    result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Enumerations.Dimensions.Region && (y.ValorDimensão == x.RegionCode || string.IsNullOrEmpty(x.RegionCode))));
                //FunctionalAreas
                if (userDimensions.Where(y => y.Dimensão == (int)Enumerations.Dimensions.FunctionalArea).Count() > 0)
                    result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Enumerations.Dimensions.FunctionalArea && (y.ValorDimensão == x.FunctionalAreaCode || string.IsNullOrEmpty(x.FunctionalAreaCode))));
                //ResponsabilityCenter
                if (userDimensions.Where(y => y.Dimensão == (int)Enumerations.Dimensions.ResponsabilityCenter).Count() > 0)
                    result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Enumerations.Dimensions.ResponsabilityCenter && (y.ValorDimensão == x.ResponsabilityCenterCode || string.IsNullOrEmpty(x.ResponsabilityCenterCode))));
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidateNumeration([FromBody] SimplifiedRequisitionViewModel data)
        {
            //Get Project Numeration
            Configuração Cfg = DBConfigurations.GetById(1);
            int ProjectNumerationConfigurationId = 0;
            ProjectNumerationConfigurationId = Cfg.NumeraçãoRequisiçõesSimplificada.Value;

            ConfiguraçãoNumerações CfgNumeration = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);

            //Validate if ProjectNo is valid
            if (!CfgNumeration.Automático.Value)
            {
                return Json("É obrigatório inserir o Nº Requisição.");
            }

            return Json("");
        }

        #region Gets
        [HttpPost]
        public JsonResult GetSimplifiedRequisitionLinesModels([FromBody] JObject requestParams)
        {
            string requestNo = requestParams["requestNo"].ToString();
            string requestNoNew = requestParams["requestNoNew"].ToString();
            ConfigUtilizadores utilizador = DBUserConfigurations.GetById(User.Identity.Name);
            List<SimplifiedRequisitionLineViewModel> result = new List<SimplifiedRequisitionLineViewModel>();
            if (requestNo != null)
            {
                result = DBSimplifiedRequisitionLines.ParseToViewModel(DBSimplifiedRequisitionLines.GetById(requestNo));
                result.ForEach(x => {
                    x.RequisitionNo = requestNoNew;
                    x.Status = 1;
                    x.LineNo = 0;
                    x.QuantityApproved = 0;
                    x.RequisitionDate= DateTime.Now.ToString();
                    x.EmployeeNo = utilizador.EmployeeNo;                 
                });
            }
            HttpContext.Session.Remove("aprovadoSession");
            return Json(result);
        } 

        [HttpPost]
        public JsonResult GetSimplifiedRequisitionLinesData([FromBody] SimplifiedRequisitionLineViewModel item)
        {
            List<SimplifiedRequisitionLineViewModel> result = new List<SimplifiedRequisitionLineViewModel>();
    
            if (item != null)
            {
                result = DBSimplifiedRequisitionLines.ParseToViewModel(DBSimplifiedRequisitionLines.GetById(item.RequisitionNo));
                if (result.Count() != 0 && result[0].MealType > 0)
                {
                    TiposRefeição typeMeal = DBMealTypes.GetById(result[0].MealType ?? 0);
                    result.ForEach(x => 
                        x.DescriptionMeal = typeMeal.Descrição

                    );
                }
            }
            return Json(result);
        }
        
        [HttpPost]
        public JsonResult GetSimplifiedRequisitionData([FromBody] SimplifiedRequisitionViewModel item)
        {
            ConfigUtilizadores utilizador = DBUserConfigurations.GetById(User.Identity.Name);
            SimplifiedRequisitionViewModel result = new SimplifiedRequisitionViewModel();
            result.Status = 1;
            result.EmployeeNo = utilizador.EmployeeNo;
            if (item != null && !string.IsNullOrEmpty(item.RequisitionNo))
            {

                result = DBSimplifiedRequisitions.ParseToViewModel(DBSimplifiedRequisitions.GetById(item.RequisitionNo));
            }
            else
            {
                //Get Numeration
                string entityId = "";
                bool autoGenId = false;
                Configuração conf = DBConfigurations.GetById(1);
                int entityNumerationConfId = conf.NumeraçãoRequisiçõesSimplificada.Value;
                autoGenId = true;
                entityId = DBNumerationConfigurations.GetNextNumeration(entityNumerationConfId, autoGenId, false);
                result.RequisitionNo = entityId;

            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetSimplifiedRequisitionModel([FromBody] SimplifiedRequisitionViewModel item)
        {
            ConfigUtilizadores utilizador = DBUserConfigurations.GetById(User.Identity.Name);
            SimplifiedRequisitionViewModel result = new SimplifiedRequisitionViewModel();
        
            if (item != null && !string.IsNullOrEmpty(item.RequisitionNo))
            {

                result = DBSimplifiedRequisitions.ParseToViewModel(DBSimplifiedReqTemplates.GetById(item.RequisitionNo));           
                result.Status = 1;
                result.Finished = false;
                result.CreateResponsible = null;
                result.CreateDate = null;
                result.RequisitionDate = null;
                result.RequisitionTime = null;
                result.RegistrationDate = null;
                result.ApprovalResponsible = null;
                result.ApprovalDate = null;
                result.ApprovalTime = null;
                result.EmployeeNo = utilizador.EmployeeNo;
                result.ReceiptPreviewDate=DateTime.Now.ToString();
            }
            return Json(result);
        }

        #endregion


        // 100 - Sucesso
        // 101 - Ocorreu um erro desconhecido
        // 102 - 
        [HttpPost]
        public JsonResult CreateSimplifiedRequisition([FromBody] SimplifiedRequisitionViewModel item)
        {
            if (item != null)
            {
                item.CreateUser = User.Identity.Name;
                item.CreateResponsible = User.Identity.Name;
                item.RequisitionDate = DateTime.Now.ToString();
                item.RegistrationDate = DateTime.Now.ToString("dd/MM/yyyy");
                item.RequisitionTime = DateTime.Now.ToString("HH:mm:ss");

                if (DBSimplifiedRequisitions.GetById(item.RequisitionNo) != null)
                {
                    item.eReasonCode = 101;
                    item.eMessage = "O Nº "+ item.RequisitionNo+ " já existe!";
                }
                else
                {

                    if (DBSimplifiedRequisitions.Create(DBSimplifiedRequisitions.ParseToDatabase(item)) != null)
                    {
                        //Update Last Numeration Used
                        Configuração conf = DBConfigurations.GetById(1);
                        int entityNumerationConfId = conf.NumeraçãoRequisiçõesSimplificada.Value;
                        ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(entityNumerationConfId);
                        ConfigNumerations.ÚltimoNºUsado = item.RequisitionNo;
                        ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                        DBNumerationConfigurations.Update(ConfigNumerations);

                        item.eReasonCode = 100;
                        item.eMessage = "Requisição Simplificada criada com sucesso.";
                    }
                    else
                    {
                        item.eReasonCode = 101;
                        item.eMessage = "Ocorreu um erro ao criar a Requisição Simplificada.";
                    }
                }
            }
            return Json(item);
        }

        // 100 - Sucesso
        // 101 - Ocorreu um erro desconhecido
        [HttpPost]
        public JsonResult CreateLinesSimplifiedRequisition([FromBody] SimplifiedRequisitionLineViewModel item)
        {
            UnidadeDeArmazenamento product = DBStockkeepingUnit.GetById(item.Code);
            SimplifiedRequisitionLineViewModel result = new SimplifiedRequisitionLineViewModel();
            if (item != null)
            {
              
                item.CreateUser = User.Identity.Name;
                item.CreateDate = DateTime.Now;
                
                result = DBSimplifiedRequisitionLines.ParseToViewModel(DBSimplifiedRequisitionLines.Create(DBSimplifiedRequisitionLines.ParseToDatabase(item)));

                if (result != null)
                {  

                    result.eReasonCode = 100;
                    result.eMessage = "Linha de Requisição Simplificada criada com sucesso.";
                }
                else
                {
                    result.eReasonCode = 101;
                    result.eMessage = "Ocorreu um erro ao criar a Requisição Simplificada.";
                }
            }
            return Json(result);
        }

        // 100 - Sucesso
        // 101 - Ocorreu um erro desconhecido
        [HttpPost]
        public JsonResult CreateMultiLinesSimplifiedRequisition([FromBody] List<SimplifiedRequisitionLineViewModel> item)
        {
            List<SimplifiedRequisitionLineViewModel> result = new List<SimplifiedRequisitionLineViewModel>();
            if (item != null)
            {
                item.ForEach(x =>
                {
                    x.CreateUser = User.Identity.Name;
                    result.Add(DBSimplifiedRequisitionLines.ParseToViewModel(DBSimplifiedRequisitionLines.Create(DBSimplifiedRequisitionLines.ParseToDatabase(x))));
                });
               
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateSimplifiedRequisitionLines([FromBody] List<SimplifiedRequisitionLineViewModel> items)
        {
            if (items != null)
            {
                items.ForEach(x =>
                {
                    x.UpdateUser = User.Identity.Name;
                    DBSimplifiedRequisitionLines.Update(DBSimplifiedRequisitionLines.ParseToDatabase(x));

                });
                mensage.eReasonCode = 100;
                mensage.eMessage = "Linhas de Requisição Simplificada actualizadas com sucesso.";
            }
            else
            {
                mensage.eReasonCode = 101;
                mensage.eMessage = "Ocorreu um erro ao actualizar as Linhas de Requisição Simplificada.";
            }

            return Json(mensage);
        }

        [HttpPost]
        public JsonResult UpdateSimplifiedRequisition([FromBody] SimplifiedRequisitionViewModel item)
        {
            SimplifiedRequisitionViewModel result = new SimplifiedRequisitionViewModel();

            try
            {
                item.RegistrationDate = DateTime.Now.ToString("dd/MM/yyyy");
                if (item != null)
                {
                    RequisiçõesSimplificadas CLocation = DBSimplifiedRequisitions.GetById(item.RequisitionNo);
                    CLocation.Estado = item.Status;
                    CLocation.DataHoraRequisição = item.RequisitionDate != "" && item.RequisitionDate != null ? DateTime.Parse(item.RequisitionDate) : (DateTime?)null;
                    CLocation.DataRegisto = item.RegistrationDate != "" && item.RegistrationDate != null ? DateTime.Parse(item.RegistrationDate) : (DateTime?)null;
                    CLocation.CódLocalização = item.LocationCode;
                    CLocation.CódigoRegião = item.RegionCode;
                    CLocation.CódigoÁreaFuncional = item.FunctionalAreaCode;
                    CLocation.CódigoCentroResponsabilidade = item.ResponsabilityCenterCode;
                    CLocation.NºProjeto = item.ProjectNo;
                    CLocation.TipoRefeição = item.MealType;
                    CLocation.DataHoraAprovação = item.ApprovalDate != "" && item.ApprovalDate != null ? DateTime.Parse(item.ApprovalDate) : (DateTime?)null;
                    CLocation.DataHoraEnvio = item.ShipDate != "" && item.ShipDate != null ? DateTime.Parse(item.ShipDate) : (DateTime?)null;
                    CLocation.DataHoraDisponibilização = item.AvailabilityDate != "" && item.AvailabilityDate != null ? DateTime.Parse(item.AvailabilityDate) : (DateTime?)null;
                    CLocation.ResponsávelCriação = item.CreateResponsible;
                    CLocation.ResponsávelAprovação = item.ApprovalResponsible;
                    CLocation.ResponsávelEnvio = item.ShipResponsible;
                    CLocation.ResponsávelReceção = item.ReceiptResponsible;
                    CLocation.Imprimir = item.Print;
                    CLocation.Anexo = item.Atach;
                    CLocation.NºFuncionário = item.EmployeeNo;
                    CLocation.Urgente = item.Urgent;
                    CLocation.NºUnidadeProdutiva = item.ProductivityNo;
                    CLocation.Observações = item.Observations;
                    CLocation.Terminada = item.Finished;
                    CLocation.ResponsávelVisar = item.AimResponsible;
                    CLocation.DataHoraVisar = item.AimDate != "" && item.AimDate != null ? DateTime.Parse(item.AimDate) : (DateTime?)null;
                    CLocation.Autorizada = item.Authorized;
                    CLocation.ResponsávelAutorização = item.AuthorizedResponsible;
                    CLocation.DataHoraAutorização = item.AuthorizedDate != "" && item.AuthorizedDate != null ? DateTime.Parse(item.AuthorizedDate) : (DateTime?)null;
                    CLocation.Visadores = item.Visor;
                    CLocation.DataReceçãoLinhas = item.ReceiptLinesDate;
                    CLocation.RequisiçãoNutrição = item.NutritionRequisition;
                    CLocation.DataReceçãoEsperada = string.IsNullOrEmpty(item.ReceiptPreviewDate) ? (DateTime?)null : DateTime.Parse(item.ReceiptPreviewDate);
                    CLocation.RequisiçãoModelo = item.ModelRequisition;
                    CLocation.DataHoraModificação = DateTime.Now;
                    CLocation.UtilizadorModificação = User.Identity.Name;

                    if (CLocation.DataHoraRequisição != null)
                    {
                        CLocation.DataHoraRequisição = CLocation.DataHoraRequisição.Value.Date;
                        CLocation.DataHoraRequisição = CLocation.DataHoraRequisição.Value.Add(TimeSpan.Parse(item.RequisitionTime));
                    }

                    if (CLocation.DataHoraAprovação != null)
                    {
                        CLocation.DataHoraAprovação = CLocation.DataHoraAprovação.Value.Date;
                        CLocation.DataHoraAprovação = CLocation.DataHoraAprovação.Value.Add(TimeSpan.Parse(item.ApprovalTime));
                    }

                    if (CLocation.DataHoraEnvio != null)
                    {
                        CLocation.DataHoraEnvio = CLocation.DataHoraEnvio.Value.Date;
                        CLocation.DataHoraEnvio = CLocation.DataHoraEnvio.Value.Add(TimeSpan.Parse(item.ShipTime));
                    }

                    if (CLocation.DataHoraDisponibilização != null)
                    {
                        CLocation.DataHoraDisponibilização = CLocation.DataHoraDisponibilização.Value.Date;
                        CLocation.DataHoraDisponibilização = CLocation.DataHoraDisponibilização.Value.Add(TimeSpan.Parse(item.AvailabilityTime));
                    }

                    if (CLocation.DataHoraVisar != null)
                    {
                        CLocation.DataHoraVisar = CLocation.DataHoraVisar.Value.Date;
                        CLocation.DataHoraVisar = CLocation.DataHoraVisar.Value.Add(TimeSpan.Parse(item.AimTime));
                    }

                    if (CLocation.DataHoraAutorização != null)
                    {
                        CLocation.DataHoraAutorização = CLocation.DataHoraAutorização.Value.Date;
                        CLocation.DataHoraAutorização = CLocation.DataHoraAutorização.Value.Add(TimeSpan.Parse(item.AuthorizedTime));
                    }


                    result = DBSimplifiedRequisitions.ParseToViewModel(DBSimplifiedRequisitions.Update(CLocation));

                    if (result != null)
                    {
                        List<LinhasRequisiçõesSimplificadas> lines = DBSimplifiedRequisitionLines.GetById(CLocation.NºRequisição);
                        lines.ForEach(x =>
                        {
                            x.NºProjeto = CLocation.NºProjeto;
                            x.TipoRefeição = CLocation.TipoRefeição;
                            x.CódLocalização = CLocation.CódLocalização;
                            x.CódigoRegião = CLocation.CódigoRegião;
                            x.CódigoÁreaFuncional = CLocation.CódigoÁreaFuncional;
                            x.CódigoCentroResponsabilidade = CLocation.CódigoCentroResponsabilidade;
                            DBSimplifiedRequisitionLines.Update(x);
                        });

                        return Json(DBSimplifiedRequisitionLines.ParseToViewModel(lines));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        // 100 - Sucesso
        // 101 - Ocorreu um erro desconhecido
        [HttpPost]
        public JsonResult DeleteSimplifiedRequisition([FromBody] SimplifiedRequisitionViewModel item)
        {
            SimplifiedRequisitionViewModel result = new SimplifiedRequisitionViewModel();
            if (item != null)
            {
                // Delete Lines Requisitions
                List<LinhasRequisiçõesSimplificadas> CLinhas = DBSimplifiedRequisitionLines.GetById(item.RequisitionNo);
                CLinhas.ForEach(x => DBSimplifiedRequisitionLines.Delete(x));

                RequisiçõesSimplificadas CLocation = DBSimplifiedRequisitions.GetById(item.RequisitionNo);
                if (DBSimplifiedRequisitions.Delete(CLocation))
                {
                    result.eReasonCode = 100;
                    result.eMessage = "Requisição Simplificada removida com sucesso.";
                }
                else
                {
                    result.eReasonCode = 101;
                    result.eMessage = "Ocorreu um erro ao remover a Requisição Simplificada.";
                }
            }
            return Json(result);
        }

        // 100 - Sucesso
        // 101 - Ocorreu um erro desconhecido
        [HttpPost]
        public JsonResult DeleteSimplifiedRequisitionLines([FromBody] SimplifiedRequisitionLineViewModel data)
        {
            //mensage
            if (data != null)
            {

                if (DBSimplifiedRequisitionLines.Delete(DBSimplifiedRequisitionLines.ParseToDatabase(data)))
                {
                    data.eReasonCode = 100;
                    data.eMessage = "Linha Requisição Simplificada removida com sucesso.";
                }
                else
                {
                    data.eReasonCode = 101;
                    data.eMessage = "Ocorreu um erro ao remover a Linha Requisição Simplificada.";
                }

            }
            return Json(data);
        }
       
        // 100 - Sucesso
        // 101 - Ocorreu um erro desconhecido

        
        [HttpPost]
        public JsonResult FinishSimplifiedRequisition([FromBody] List<SimplifiedRequisitionLineViewModel> items)
        {
            if (items != null && items.Count>0)
            {
                mensage.eReasonCode = 100;
                foreach (var item in items)
                {
                    UnidadeDeArmazenamento product = DBStockkeepingUnit.GetById(item.Code);
                    if (product.Bloqueado == true)
                    {
                        mensage.eReasonCode = 101;
                        mensage.eMessage = "A linha Nº:" + item.LineNo + " contem o produto " + item.Description + " bloqueado";
                        break;
                    }
                    item.QuantityApproved = item.QuantityToRequire;
                    item.Status = 2;
    
                }
                if (mensage.eReasonCode == 100)
                {
                    items.ForEach(x =>
                    {
                        x.QuantityApproved = x.QuantityToRequire;
                        x.Status = 2;
                        DBSimplifiedRequisitionLines.Update(DBSimplifiedRequisitionLines.ParseToDatabase(x));

                    });

                    SimplifiedRequisitionViewModel requisitionSimpli = DBSimplifiedRequisitions.ParseToViewModel(DBSimplifiedRequisitions.GetById(items[0].RequisitionNo));
                    requisitionSimpli.Status = 2;
                    requisitionSimpli.Finished = true;
                    requisitionSimpli.ApprovalResponsible = User.Identity.Name;
                    requisitionSimpli.ApprovalDate = DateTime.Now.ToString();
                    requisitionSimpli.ApprovalTime = DateTime.Now.ToString("HH:mm:ss");
                    DBSimplifiedRequisitions.Update(DBSimplifiedRequisitions.ParseToDatabase(requisitionSimpli));

                    mensage.eReasonCode = 100;
                    mensage.eMessage = "Requisição terminada com Sucesso";
                }
            }
            else
            {
                mensage.eReasonCode = 101;
                mensage.eMessage = "Nessecita de linhas de requisição ao terminar!!";
            }
            
            return Json(mensage);           
        }

        // 100 - Sucesso
        // 101 - Ocorreu um erro desconhecido

        
        [HttpPost]
        public JsonResult HistorySimplifiedRequisition([FromBody] List<SimplifiedRequisitionLineViewModel> items)
        {
            if (items != null)
            {
                SimplifiedRequisitionViewModel requisitionSimpli = DBSimplifiedRequisitions.ParseToViewModel(DBSimplifiedRequisitions.GetById(items[0].RequisitionNo));
                requisitionSimpli.Status = 3;
                DBSimplifiedRequisitions.Update(DBSimplifiedRequisitions.ParseToDatabase(requisitionSimpli));
                foreach (var item in items)
                {
                    item.Status = 3;
                    DBSimplifiedRequisitionLines.Update(DBSimplifiedRequisitionLines.ParseToDatabase(item));
                }
                mensage.eReasonCode = 100;
                mensage.eMessage = "Requisição arquivada com Sucesso";
            }
            else
            {
                mensage.eReasonCode = 101;
                mensage.eMessage = "Erro na arquivação";
            }
            return Json(mensage);
        }


        
        [HttpPost]
        public JsonResult DisapproveSimplifiedRequisition([FromBody] List<SimplifiedRequisitionLineViewModel> items)
        {
            bool disapprove = true;

            foreach (var item in items)
            {
                if(item.QuantityReceipt!=0 && item.QuantityReceipt!=null)
                {
                    disapprove = false;
                    mensage.eReasonCode = 101;
                    mensage.eMessage = "Impossivel desaprovar a requisição nº"+ item.RequisitionNo;
                    break;
                }
            }
            if (disapprove == true)
            {
                SimplifiedRequisitionViewModel requisitionSimpli = DBSimplifiedRequisitions.ParseToViewModel(DBSimplifiedRequisitions.GetById(items[0].RequisitionNo));
                requisitionSimpli.Status = 1;
                requisitionSimpli.ApprovalResponsible = "";
                requisitionSimpli.ApprovalDate = "";
                requisitionSimpli.ApprovalTime = "";
                DBSimplifiedRequisitions.Update(DBSimplifiedRequisitions.ParseToDatabase(requisitionSimpli));
                foreach (var item in items)
                {
                    item.Status = 1;
                    item.QuantityApproved = 0;
                    DBSimplifiedRequisitionLines.Update(DBSimplifiedRequisitionLines.ParseToDatabase(item));
                }
                mensage.eReasonCode = 100;
                mensage.eMessage = "Requisição desaprovada com Sucesso";
            }

            return Json(mensage);
        }
            
        
        [HttpPost]
        public JsonResult RegisterRequisition([FromBody] List<SimplifiedRequisitionLineViewModel> items)
        {
            string dataRegisto;
            bool historyRequisition = true;
            SimplifiedRequisitionViewModel requisitionSimpli = DBSimplifiedRequisitions.ParseToViewModel(DBSimplifiedRequisitions.GetById(items[0].RequisitionNo));
            List<ProjectDiaryViewModel> ListDp = new List<ProjectDiaryViewModel>();

            //Receipt Lines Date
            if (requisitionSimpli.ReceiptLinesDate == true && requisitionSimpli.RegistrationDate!=null)
               dataRegisto = requisitionSimpli.RegistrationDate;
            else
               dataRegisto = DateTime.Now.ToString();

            foreach (var item in items)
            {
                if (item.QuantityReceipt == 0 || item.QuantityReceipt== null)
                {
                    Task<WSGenericCodeUnit.FxGetStock_ItemLocation_Result> TGetNavLocationProduct = WSGeneric.GetNAVProductQuantityInStockFor(item.Code, item.LocationCode, configws);
                    TGetNavLocationProduct.Wait();
                    if (TGetNavLocationProduct.IsCompletedSuccessfully)
                    {
                        if (TGetNavLocationProduct.Result.return_value >0 && TGetNavLocationProduct.Result.return_value >= item.QuantityApproved)
                        {
                            //Update Simplified Requisition lines
                            item.QuantityReceipt = item.QuantityApproved;
                            DBSimplifiedRequisitionLines.Update(DBSimplifiedRequisitionLines.ParseToDatabase(item));

                            //Create Diary Project
                            DiárioDeProjeto newdp = new DiárioDeProjeto()
                            {
                              
                                NºProjeto = requisitionSimpli.ProjectNo,
                                Data = requisitionSimpli.RegistrationDate == "" || requisitionSimpli.RegistrationDate == null ? (DateTime?)null : DateTime.Parse(requisitionSimpli.RegistrationDate),
                                TipoMovimento = 1, //Consumo
                                Tipo = item.Type,
                                Código = item.Code,
                                Descrição = item.Description,
                                Quantidade = item.QuantityApproved,
                                CódUnidadeMedida = item.MeasureUnitNo,
                                CódLocalização = item.LocationCode,
                                //GrupoContabProjeto = x.ProjectContabGroup,
                                CódigoRegião = item.RegionCode,
                                CódigoÁreaFuncional = item.FunctionAreaCode,
                                CódigoCentroResponsabilidade = item.ResponsabilityCenterCode,
                                Utilizador = User.Identity.Name,
                                CustoUnitário = item.UnitCost,
                                CustoTotal = item.TotalCost,
                                //PreçoUnitário = x.UnitPrice,
                                //PreçoTotal = x.TotalPrice,
                                //Faturável = x.Billable,
                                Registado = true,
                                //FaturaANºCliente = x.InvoiceToClientNo,
                                //Moeda = x.Currency,
                                //ValorUnitárioAFaturar = x.UnitValueToInvoice,
                                TipoRefeição = item.MealType,
                                //CódGrupoServiço = x.ServiceGroupCode,
                                //NºGuiaResíduos = x.ResidueGuideNo,
                                //NºGuiaExterna = x.ExternalGuideNo,
                                //DataConsumo = x.ConsumptionDate == "" || x.ConsumptionDate == null ? (DateTime?)null : DateTime.Parse(x.ConsumptionDate),
                                //CódServiçoCliente = x.ServiceClientCode
                            };
                            newdp.Faturada = false;
                            newdp.DataHoraCriação = DateTime.Now;
                            newdp.UtilizadorCriação = User.Identity.Name;

                            //Insert List Regist Project
                            ListDp.Add(DBProjectDiary.ParseToViewModel(newdp));
                        }
                        else
                        {
                            historyRequisition=false;
                            requisitionSimpli.eReasonCode = 101;
                            requisitionSimpli.eMessage = requisitionSimpli.eMessage + " Quantidade existente em stock: " + TGetNavLocationProduct.Result.return_value + ", no Produto "+item.Description+ " \r\n"; 
                        }
                    }                  
                }
            }
            //Register diary project
            if (ListDp.Count() != 0)
            {
                register.UpdateProjectDiaryRequisition(ListDp, requisitionSimpli.RequisitionNo, User.Identity.Name);
            }

            if (historyRequisition == true)
            {
                //Register lines of diary project
                ListDp.Clear();
                items.ForEach(x => 
                {
                    x.Status = 3;
                    DBSimplifiedRequisitionLines.Update(DBSimplifiedRequisitionLines.ParseToDatabase(x));
                    DiárioDeProjeto newdp = new DiárioDeProjeto()
                    {
                        NºProjeto = requisitionSimpli.ProjectNo,
                        Data = requisitionSimpli.RegistrationDate == "" || requisitionSimpli.RegistrationDate == null ? (DateTime?)null : DateTime.Parse(requisitionSimpli.RegistrationDate),
                        TipoMovimento = 1, //Consumo
                        Tipo = x.Type,
                        Código = x.Code,
                        Descrição = x.Description,
                        Quantidade = x.QuantityApproved,
                        CódUnidadeMedida = x.MeasureUnitNo,
                        CódLocalização = x.LocationCode,
                        //GrupoContabProjeto = x.ProjectContabGroup,
                        CódigoRegião = x.RegionCode,
                        CódigoÁreaFuncional = x.FunctionAreaCode,
                        CódigoCentroResponsabilidade = x.ResponsabilityCenterCode,
                        Utilizador = User.Identity.Name,
                        CustoUnitário = x.UnitCost,
                        CustoTotal = x.TotalCost,
                        //PreçoUnitário = x.UnitPrice,
                        //PreçoTotal = x.TotalPrice,
                        //Faturável = x.Billable,
                        Registado = true,
                        //FaturaANºCliente = x.InvoiceToClientNo,
                        //Moeda = x.Currency,
                        //ValorUnitárioAFaturar = x.UnitValueToInvoice,
                        TipoRefeição = x.MealType,
                        //CódGrupoServiço = x.ServiceGroupCode,
                        //NºGuiaResíduos = x.ResidueGuideNo,
                        //NºGuiaExterna = x.ExternalGuideNo,
                        //DataConsumo = x.ConsumptionDate == "" || x.ConsumptionDate == null ? (DateTime?)null : DateTime.Parse(x.ConsumptionDate),
                        //CódServiçoCliente = x.ServiceClientCode
                    };
                    newdp.Faturada = false;
                    newdp.DataHoraCriação = DateTime.Now;
                    newdp.UtilizadorCriação = User.Identity.Name;

                    //Insert List Regist Project
                    ListDp.Add(DBProjectDiary.ParseToViewModel(newdp));
                });

                register.RegisterDiaryLinesRequisition(ListDp, User.Identity.Name);

                //Update Simplified Requisition Header
                requisitionSimpli.Status = 3;
                requisitionSimpli.RegistrationDate = DateTime.Now.ToString();
                requisitionSimpli.ReceiptResponsible = User.Identity.Name;
                DBSimplifiedRequisitions.Update(DBSimplifiedRequisitions.ParseToDatabase(requisitionSimpli));
                
                requisitionSimpli.eReasonCode = 100;
                requisitionSimpli.eMessage = "Registo em Diário de Projeto com sucesso";
            }
    
            return Json(requisitionSimpli);
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_RequisicoesSimplificadas([FromBody] List<SimplifiedRequisitionViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "RequisicoesSimplificadas\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Requisições Simplificadas");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["requisitionNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº");
                    Col = Col + 1;
                }
                if (dp["status"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Estado");
                    Col = Col + 1;
                }
                if (dp["requisitionDate"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Requisição");
                    Col = Col + 1;
                }
                if (dp["requisitionTime"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Hora Requisição");
                    Col = Col + 1;
                }
                if (dp["locationCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Localização");
                    Col = Col + 1;
                }
                if (dp["regionCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Região");
                    Col = Col + 1;
                }
                if (dp["functionalAreaCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Area");
                    Col = Col + 1;
                }
                if (dp["responsabilityCenterCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Centro Responsabilidade");
                    Col = Col + 1;
                }
                if (dp["projectNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Projeto");
                    Col = Col + 1;
                }
                if (dp["observations"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Observações");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (SimplifiedRequisitionViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["requisitionNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisitionNo);
                            Col = Col + 1;
                        }
                        if (dp["status"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Status.ToString());
                            Col = Col + 1;
                        }
                        if (dp["requisitionDate"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisitionDate);
                            Col = Col + 1;
                        }
                        if (dp["requisitionTime"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisitionTime);
                            Col = Col + 1;
                        }
                        if (dp["locationCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.LocationCode);
                            Col = Col + 1;
                        }
                        if (dp["regionCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RegionCode);
                            Col = Col + 1;
                        }
                        if (dp["functionalAreaCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.FunctionalAreaCode);
                            Col = Col + 1;
                        }
                        if (dp["responsabilityCenterCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ResponsabilityCenterCode);
                            Col = Col + 1;
                        }
                        if (dp["projectNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProjectNo);
                            Col = Col + 1;
                        }
                        if (dp["observations"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Observations);
                            Col = Col + 1;
                        }
                        count++;
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_RequisicoesSimplificadas(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "RequisicoesSimplificadas\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Requisições Simplificadas.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

    }
}