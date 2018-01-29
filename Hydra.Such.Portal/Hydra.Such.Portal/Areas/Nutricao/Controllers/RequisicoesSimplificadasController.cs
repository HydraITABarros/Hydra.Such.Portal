using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Nutrition;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Nutrition;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Portal.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hydra.Such.Portal.Areas.Nutricao.Controllers
{

    public class RequisicoesSimplificadasController : Controller
    {
        ProjetosController register;
        private readonly NAVWSConfigurations configws;
        private ErrorHandler mensage = new ErrorHandler();


        public RequisicoesSimplificadasController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs)
        {
            this.configws = NAVWSConfigs.Value;
            register = new ProjetosController(appSettings, NAVWSConfigs);
        }


        [Area("Nutricao")]
        public IActionResult Index(int option)
        {
            HttpContext.Session.Remove("aprovadoSession");
            ViewBag.RequisitionsApprovals = "false";
            //‘Requisições simplificadas para Registar’ com estado aprovado
            if (option == 1)
            {
                ViewBag.Option = "resgitar";
            }
            //Histórico Requisições simplificadas
            else if (option == 2)
            {
                ViewBag.Option = "historico";
            }
            return View();
        }

        [Area("Nutricao")]
        [HttpPost]
        public JsonResult GetSimplifiedRequisitions([FromBody] string option)
        {
            List<SimplifiedRequisitionViewModel> result;

            //‘Requisições simplificadas para Registar’ com estado aprovado
            if (option == "resgitar")
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
                result = DBSimplifiedRequisitions.ParseToViewModel(DBSimplifiedRequisitions.GetByCreateResponsible(User.Identity.Name));
            }
            return Json(result);
        }

        [Area("Nutricao")]
        [HttpPost]
        public JsonResult GetSimplifiedRequisitionLines([FromBody] SimplifiedRequisitionLineViewModel item)
        {
            List<SimplifiedRequisitionLineViewModel> result = new List<SimplifiedRequisitionLineViewModel>();
            if (item != null)
            {
                result = DBSimplifiedRequisitionLines.ParseToViewModel(DBSimplifiedRequisitionLines.GetById(item.RequisitionNo));
                if (result.Count() != 0 && result[0].MealType >0)
                {
                    TiposRefeição typeMeal = DBMealTypes.GetById(result[0].MealType ?? 0 );
                    result.ForEach(x =>
                        x.DescriptionMeal = typeMeal.Descrição

                    );
                }

                return Json(result);
            }
            return Json(result);
        }

        [Area("Nutricao")]
        public IActionResult Detalhes(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 3, 40);

            if (UPerm != null && UPerm.Read.Value)
            {
              
                ViewBag.Approval = HttpContext.Session.GetString("aprovadoSession") ?? "";
                ViewBag.User = User.Identity.Name;
                //Registar requisições aprovadas
                if (ViewBag.Approval == "resgitar" )
                {
                    ViewBag.LockFields = true;
                    UPerm.Create = false;
                    ViewBag.Option = "resgitar";
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
                }

                ViewBag.RequestNo = id ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [Area("Nutricao")]
        [HttpPost]
        public JsonResult GetSimplifiedRequisitionData([FromBody] SimplifiedRequisitionViewModel item)
        {
            ConfigUtilizadores utilizador = DBUserConfigurations.GetById(User.Identity.Name);
            SimplifiedRequisitionViewModel result = new SimplifiedRequisitionViewModel();
            result.Status = 1;
            result.EmployeeNo = utilizador.EmployeeNo;
            if (item != null && !string.IsNullOrEmpty(item.RequisitionNo))
                result = DBSimplifiedRequisitions.ParseToViewModel(DBSimplifiedRequisitions.GetById(item.RequisitionNo));
            return Json(result);
        }

        // 100 - Sucesso
        // 101 - Ocorreu um erro desconhecido
        // 102 - 
        [Area("Nutricao")]
        [HttpPost]
        public JsonResult CreateSimplifiedRequisition([FromBody] SimplifiedRequisitionViewModel item)
        {
            if (item != null)
            {
                item.CreateUser = User.Identity.Name;
                item.CreateResponsible = User.Identity.Name;

                if (DBSimplifiedRequisitions.GetById(item.RequisitionNo) != null)
                {
                    item.eReasonCode = 101;
                    item.eMessage = "O Nº "+ item.RequisitionNo+ " já existe!";
                }
                else
                {
                    if (DBSimplifiedRequisitions.Create(DBSimplifiedRequisitions.ParseToDatabase(item)) != null)
                    {
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

        [Area("Nutricao")]
        [HttpPost]
        public JsonResult CreateLinesSimplifiedRequisition([FromBody] SimplifiedRequisitionLineViewModel item)
        {
            UnidadeDeArmazenamento product = DBStockkeepingUnit.GetById(item.Code);
            SimplifiedRequisitionLineViewModel result = new SimplifiedRequisitionLineViewModel();
            if (item != null)
            {
              
                item.CreateUser = User.Identity.Name;
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


        [Area("Nutricao")]
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

        [Area("Nutricao")]
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


        [Area("Nutricao")]
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
            
        [Area("Nutricao")]
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
                    Task<WSGenericCodeUnit.FxGetStock_ItemLocation_Result> TGetNavLocationProduct = WSGeneric.GetALLNavLocationProduct(item.Code, item.LocationCode, configws);
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

        [Area("Nutricao")]
        [HttpPost]
        public JsonResult UpdateSimplifiedRequisitionLines([FromBody] List<SimplifiedRequisitionLineViewModel> items)
        {
            if (items != null)
            {
                items.ForEach(x =>
                {
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

        [Area("Nutricao")]
        [HttpPost]
        public JsonResult UpdateSimplifiedRequisition([FromBody] SimplifiedRequisitionViewModel item)
        {
            SimplifiedRequisitionViewModel result = new SimplifiedRequisitionViewModel();

            try
            {
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
                        List<LinhasRequisiçõesSimplificadas> lines =  DBSimplifiedRequisitionLines.GetById(CLocation.NºRequisição);
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
        [Area("Nutricao")]
        [HttpPost]
        public JsonResult DeleteSimplifiedRequisition([FromBody] SimplifiedRequisitionViewModel item)
        {
            SimplifiedRequisitionViewModel result = new SimplifiedRequisitionViewModel();
            if (item != null)
            {
                // Delete Lines Requisitions
                List<LinhasRequisiçõesSimplificadas> CLinhas = DBSimplifiedRequisitionLines.GetById(item.RequisitionNo);
                CLinhas.ForEach(x=> DBSimplifiedRequisitionLines.Delete(x));

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
        [Area("Nutricao")]
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
    }
}