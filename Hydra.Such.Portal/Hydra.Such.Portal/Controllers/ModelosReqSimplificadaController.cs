using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.Logic.Nutrition;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Database;



using Hydra.Such.Data.ViewModel.Nutrition;

using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
//using Hydra.Such.Portal.Configurations;
//using Microsoft.Extensions.Options;

namespace Hydra.Such.Portal.Areas.Nutricao.Controllers
{
    public class ModelosReqSimplificadaController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ModelosReqSimplificadaController(IHostingEnvironment _hostingEnvironment)
        {
            this._hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult Index()
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ModelosRequisiçõesSimplificadas);
            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UserPermissions = userPermissions;
                return View();
            }
            else
            {
                //return RedirectToAction("AccessDenied", "~/Error");
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        [HttpPost]
        
        public JsonResult GetAllReqTemplates()
        {
            var items = DBSimplifiedReqTemplates.ParseToViewModel(DBSimplifiedReqTemplates.GetAll());

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (userDimensions.Where(y => y.Dimensão == (int)Enumerations.Dimensions.Region).Count() > 0)
                items.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Enumerations.Dimensions.Region && (y.ValorDimensão == x.CodeRegion || string.IsNullOrEmpty(x.CodeRegion))));
            //FunctionalAreas
            if (userDimensions.Where(y => y.Dimensão == (int)Enumerations.Dimensions.FunctionalArea).Count() > 0)
                items.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Enumerations.Dimensions.FunctionalArea && (y.ValorDimensão == x.CodeFunctionalArea || string.IsNullOrEmpty(x.CodeFunctionalArea))));
            //ResponsabilityCenter
            if (userDimensions.Where(y => y.Dimensão == (int)Enumerations.Dimensions.ResponsabilityCenter).Count() > 0)
                items.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Enumerations.Dimensions.ResponsabilityCenter && (y.ValorDimensão == x.CodeResponsabilityCenter || string.IsNullOrEmpty(x.CodeResponsabilityCenter))));

            return Json(items);
        }

        
        public IActionResult Detalhes(string id)
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ModelosRequisiçõesSimplificadas);

            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UPermissions = userPermissions;
                ViewBag.RequisitionTemplateId = id;
                
                //FormType values (0 - Create, 1 - Update); 
                ViewBag.FormType = (id != null && !string.IsNullOrEmpty(id)) ? "1" : "0";

                return View();
            }
            else
            {
                //return RedirectToAction("AccessDenied", "Error");
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        [HttpPost]
        
        public JsonResult GetReqTemplate([FromBody] string requisitionTemplateId)
        {
            SimplifiedReqTemplateViewModel item;
            if (!string.IsNullOrEmpty(requisitionTemplateId) && requisitionTemplateId != "0")
                item = DBSimplifiedReqTemplates.GetById(requisitionTemplateId).ParseToViewModel();
            else
                item = new SimplifiedReqTemplateViewModel();

            return Json(item);
        }

        [HttpPost]
        
        public JsonResult ValidateNumeration([FromBody] SimplifiedReqTemplateViewModel item)
        {
            //Get Project Numeration
            Configuração conf = DBConfigurations.GetById(1);
            if (conf != null)
            {
                int numModReqSimplificadas = conf.NumeraçãoModReqSimplificadas.Value;

                ConfiguraçãoNumerações numConf = DBNumerationConfigurations.GetById(numModReqSimplificadas);

                //Validate if id is valid
                if (!(item.RequisitionTemplateId == "" || item.RequisitionTemplateId == null) && !numConf.Manual.Value)
                {
                    return Json("A numeração configurada para os modelos de requisição simplificada não permite inserção manual.");
                }
                else if (item.RequisitionTemplateId == "" && !numConf.Automático.Value)
                {
                    return Json("É obrigatório inserir o Nº Modelo.");
                }
            }
            else
            {
                return Json("Não foi possivel obter as configurações base de numeração.");
            }
            return Json("");
        }

        [HttpPost]
        
        public JsonResult CreateReqTemplate([FromBody] SimplifiedReqTemplateViewModel item)
        {
            try
            {
                if (item != null)
                {
                    //Get Numeration
                    bool autoGenId = false;
                    Configuração conf = DBConfigurations.GetById(1);
                    int entityNumerationConfId = conf.NumeraçãoModReqSimplificadas.Value;

                    if (item.RequisitionTemplateId == "" || item.RequisitionTemplateId == null)
                    {
                        autoGenId = true;
                        item.RequisitionTemplateId = DBNumerationConfigurations.GetNextNumeration(entityNumerationConfId, autoGenId, false);
                    }
                    if (item.RequisitionTemplateId != null)
                    {
                        item.CreateUser = User.Identity.Name; 
                        var createdItem = DBSimplifiedReqTemplates.Create(item.ParseToDB());
                        if (createdItem != null)
                        {
                            item = createdItem.ParseToViewModel();
                            if (autoGenId)
                            {
                                ConfiguraçãoNumerações configNum = DBNumerationConfigurations.GetById(entityNumerationConfId);
                                configNum.ÚltimoNºUsado = item.RequisitionTemplateId;
                                configNum.UtilizadorModificação = User.Identity.Name;
                                DBNumerationConfigurations.Update(configNum);
                            }
                            item.eReasonCode = 1;
                            item.eMessage = "Registo criado com sucesso.";
                        }
                        else
                        {
                            item.eReasonCode = 3;
                            item.eMessage = "Ocorreu um erro ao inserir os dados na base de dados.";
                        }
                    }
                    else
                    {
                        item.eReasonCode = 5;
                        item.eMessage = "A numeração configurada não é compativel com a inserida.";
                    }
                }
            }
            catch (Exception)
            {
                item.eReasonCode = 2;
                item.eMessage = "Ocorreu um erro ao criar o modelo de requisição simplificado.";
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult UpdateReqTemplate([FromBody] SimplifiedReqTemplateViewModel item)
        {
            if (item != null)
            {
                item.UpdateUser = User.Identity.Name;
                var updatedItem = DBSimplifiedReqTemplates.Update(item.ParseToDB());

                if (updatedItem != null)
                {
                    item = updatedItem.ParseToViewModel();
                    item.eReasonCode = 1;
                    item.eMessage = "Registo atualizado com sucesso.";
                }
                else
                {
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao atualizar o modelo de requisição simplificada.";
                }
            }
            else
            {
                item = new SimplifiedReqTemplateViewModel()
                {
                    eReasonCode = 3,
                    eMessage = "Ocorreu um erro ao atualizar. O modelo de requisição simplificado não pode ser nulo."
                };
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult DeleteReqTemplate([FromBody] SimplifiedReqTemplateViewModel item)
        {
            ErrorHandler errorHandler = new ErrorHandler()
            {
                eReasonCode = 2,
                eMessage = "Ocorreu um erro ao eliminar o registo."
            };

            try
            {
                if (item != null)
                {
                    bool sucess = DBSimplifiedReqTemplates.Delete(item.ParseToDB());
                    if (sucess)
                    {
                        item.eReasonCode = 1;
                        item.eMessage = "Registo eliminado com sucesso.";
                    }
                }
                else
                {
                    item = new SimplifiedReqTemplateViewModel();
                }
            }
            catch
            {
                item.eReasonCode = errorHandler.eReasonCode;
                item.eMessage = errorHandler.eMessage;
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult CreateReqTemplateLine([FromBody] SimplifiedReqTemplateLinesViewModel item)
        {
            if (item != null)
            {
                item.CreateUser = User.Identity.Name;
                var createdItem = DBSimplifiedReqTemplateLines.Create(item.ParseToDB());
                if (createdItem != null)
                {
                    item = createdItem.ParseToViewModel();
                    item.eReasonCode = 1;
                    item.eMessage = "Registo criado com sucesso.";
                }
                else
                {
                    item = new SimplifiedReqTemplateLinesViewModel();
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao criar o registo.";
                }
            }
            else
            {
                item = new SimplifiedReqTemplateLinesViewModel();
                item.eReasonCode = 2;
                item.eMessage = "A linha não pode ser nula.";
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult DeleteReqTemplateLine([FromBody] SimplifiedReqTemplateLinesViewModel item)
        {
            if (item != null)
            {
                if (DBSimplifiedReqTemplateLines.Delete(item.ParseToDB()))
                {
                    item.eReasonCode = 1;
                    item.eMessage = "Registo eliminado com sucesso.";
                }
                else
                {
                    item = new SimplifiedReqTemplateLinesViewModel();
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao eliminar o registo.";
                }
            }
            else
            {
                item = new SimplifiedReqTemplateLinesViewModel();
                item.eReasonCode = 2;
                item.eMessage = "Ocorreu um erro: a linha não pode ser nula.";
            }

            return Json(item);
        }

        [HttpPost]
        
        public JsonResult UpdateReqTemplateLines([FromBody] List<SimplifiedReqTemplateLinesViewModel> items)
        {
            ErrorHandler updateResponse = new ErrorHandler()
            {
                eReasonCode = 2,
                eMessage = "Ocorreu um erro ao eliminar o registo.",
            };

            string reqTemplateId = string.Empty;
            if (items != null && items.Count > 0)
            {
                reqTemplateId = items.Select(x => x.RequisitionTemplateId).First();

                //Get existing lines from db
                var reqTemplateLines = DBSimplifiedReqTemplateLines.GetLinesForTemplate(reqTemplateId);
                if (reqTemplateLines != null)
                {
                    List<LinhasRequisiçõesSimplificadas> itemsToUpdate = new List<LinhasRequisiçõesSimplificadas>();

                    //Update existing
                    reqTemplateLines.ForEach(itemToUpdate =>
                    {
                        var item = items.SingleOrDefault(x => x.RequisitionTemplateId == itemToUpdate.NºRequisição &&
                            x.RequisitionTemplateLineId == itemToUpdate.NºLinha);

                        if (item != null)
                        {
                            //Update
                            itemToUpdate = item.ParseToDB();
                            itemToUpdate.UtilizadorModificação = User.Identity.Name;
                        }
                        itemsToUpdate.Add(itemToUpdate);
                    });

                    var updatedItems = DBSimplifiedReqTemplateLines.Update(itemsToUpdate);
                    if (updatedItems != null)
                    {
                        updateResponse.eReasonCode = 1;
                        updateResponse.eMessage = "Linhas atualizadas com sucesso.";
                    }
                }
                else
                {
                    updateResponse.eMessage = "Ocorreu um erro ao obter as linhas.";
                }
            }
            else
            {
                updateResponse.eMessage = "Não existem linhas para atualizar.";
            }
            return Json(updateResponse);
        }

        [HttpPost]
        public JsonResult GetServices()
        {
            List<Portal.Controllers.DDMessageString> result = Data.Logic.ProjectDiary.DBServices.GetAll().Select(x => new Portal.Controllers.DDMessageString()
            {
                id = x.Código,
                value = x.Descrição
            }).ToList();

            return Json(result);
        }

        //1
        [HttpPost]
        public async Task<JsonResult> ExportToExcel_ModelosRequisicaoSimplificada([FromBody] List<SimplifiedReqTemplateViewModel> dp)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\Upload\\temp";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + ".xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Modelos de Requisição Simplificada");
                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("Nº Modelo");
                row.CreateCell(1).SetCellValue("Descrição");
                row.CreateCell(2).SetCellValue("Região");
                row.CreateCell(3).SetCellValue("CÁrea Funcional");
                row.CreateCell(4).SetCellValue("Centro Responsabilidade");

                if (dp != null)
                {
                    int count = 1;
                    foreach (SimplifiedReqTemplateViewModel item in dp)
                    {
                        row = excelSheet.CreateRow(count);
                        row.CreateCell(0).SetCellValue(item.RequisitionTemplateId);
                        row.CreateCell(1).SetCellValue(item.Description);
                        row.CreateCell(2).SetCellValue(item.CodeRegion);
                        row.CreateCell(3).SetCellValue(item.CodeFunctionalArea);
                        row.CreateCell(4).SetCellValue(item.CodeResponsabilityCenter);
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
        public IActionResult ExportToExcelDownload_ModelosRequisicaoSimplificada(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Modelos de Requisição Simplificada.xlsx");
        }

    }
}