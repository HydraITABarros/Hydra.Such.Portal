using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Portal.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Portal.Controllers
{
    public class ModelosRequisicaoController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ModelosRequisicaoController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
        }


        public IActionResult Index()
        {
            UserAccessesViewModel userPermissions =
                DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ModelosRequisicao);
            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UserPermissions = userPermissions;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        
        public IActionResult Detalhes(string id)
        {
            UserAccessesViewModel userPermissions =
                DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ModelosRequisicao);
            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UserPermissions = userPermissions;
                ViewBag.RequisitionId = id;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        [HttpPost]
        
        public JsonResult GetRequisitionTemplates()
        {
            List<RequisitionTemplateViewModel> result = DBRequestTemplates.GetAll().ParseToTemplateViewModel();

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.RegionCode));
            //FunctionalAreas
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.FunctionalAreaCode));
            //ResponsabilityCenter
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CenterResponsibilityCode));
            return Json(result);
        }

        [HttpPost]
        
        public JsonResult GetRequisition([FromBody] string id)
        {
            RequisitionTemplateViewModel item;
            if (!string.IsNullOrEmpty(id) && id != "0")
            {
                item = DBRequestTemplates.GetById(id).ParseToTemplateViewModel();
            }
            else
                item = new RequisitionTemplateViewModel();

            return Json(item);
        }

        [HttpPost]
        
        public JsonResult CreateRequisition([FromBody] RequisitionTemplateViewModel item)
        {
            if (item != null)
            {
                //Get Numeration
                bool autoGenId = false;
                Configuração conf = DBConfigurations.GetById(1);
                int entityNumerationConfId = conf.NumeracaoModelosRequisicao.Value;

                if (item.RequisitionNo == "" || item.RequisitionNo == null)
                {
                    autoGenId = true;
                    item.RequisitionNo = DBNumerationConfigurations.GetNextNumeration(entityNumerationConfId, autoGenId, false);
                }
                if (item.RequisitionNo != null)
                {
                    item.CreateUser = User.Identity.Name;
                    var createdItem = DBRequestTemplates.Create(item.ParseToDB());
                    if (createdItem != null)
                    {
                        item = createdItem.ParseToTemplateViewModel();
                        if (autoGenId)
                        {
                            ConfiguraçãoNumerações configNum = DBNumerationConfigurations.GetById(entityNumerationConfId);
                            configNum.ÚltimoNºUsado = item.RequisitionNo;
                            configNum.UtilizadorModificação = User.Identity.Name;
                            DBNumerationConfigurations.Update(configNum);
                        }
                        item.eReasonCode = 1;
                        item.eMessage = "Registo criado com sucesso.";
                    }
                    else
                    {
                        item = new RequisitionTemplateViewModel();
                        item.eReasonCode = 2;
                        item.eMessage = "Ocorreu um erro ao criar o registo.";
                    }
                }
                else
                {
                    item.eReasonCode = 5;
                    item.eMessage = "A numeração configurada não é compativel com a inserida.";
                }
            }
            else
            {
                item = new RequisitionTemplateViewModel();
                item.eReasonCode = 2;
                item.eMessage = "Ocorreu um erro: o modelo de requisição não pode ser nulo.";
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult UpdateRequisition([FromBody] RequisitionTemplateViewModel item)
        {
            if (item != null)
            {
                item.CreateUser = User.Identity.Name;
                var updatedItem = DBRequestTemplates.Update(item.ParseToDB());
                if (updatedItem != null)
                {
                    item = updatedItem.ParseToTemplateViewModel();
                    item.eReasonCode = 1;
                    item.eMessage = "Registo atualizado com sucesso.";
                }
                else
                {
                    item = new RequisitionTemplateViewModel();
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao atualizar o registo.";
                }
            }
            else
            {
                item = new RequisitionTemplateViewModel();
                item.eReasonCode = 2;
                item.eMessage = "Ocorreu um erro: o modelo de requisição não pode ser nulo.";
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult DeleteRequisition([FromBody] RequisitionTemplateViewModel item)
        {
            if (item != null)
            {
                if (DBRequestTemplates.Delete(item.ParseToDB()))
                {
                    item.eReasonCode = 1;
                    item.eMessage = "Registo eliminado com sucesso.";
                }
                else
                {
                    item = new RequisitionTemplateViewModel();
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao eliminar o registo.";
                }
            }
            else
            {
                item = new RequisitionTemplateViewModel();
                item.eReasonCode = 2;
                item.eMessage = "Ocorreu um erro: o modelo de requisição não pode ser nulo.";
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult ValidateNumeration([FromBody] RequisitionTemplateViewModel item)
        {
            //Get Project Numeration
            Configuração conf = DBConfigurations.GetById(1);
            if (conf != null)
            {
                int numRequisitionTemplate = conf.NumeracaoModelosRequisicao.Value;

                ConfiguraçãoNumerações numConf = DBNumerationConfigurations.GetById(numRequisitionTemplate);

                //Validate if id is valid
                if (!(item.RequisitionNo == "" || item.RequisitionNo == null) && !numConf.Manual.Value)
                {
                    return Json("A numeração configurada para os modelos de requisição não permite inserção manual.");
                }
                else if (item.RequisitionNo == "" && !numConf.Automático.Value)
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

        public JsonResult GetPlaces([FromBody] int placeId)
        {
            PlacesViewModel PlacesData = DBPlaces.ParseToViewModel(DBPlaces.GetById(placeId));
            return Json(PlacesData);
        }

        [HttpPost]
        
        public JsonResult CreateRequisitionLine([FromBody] RequisitionTemplateLineViewModel item)
        {
            if (item != null)
            {
                item.CreateUser = User.Identity.Name;
                var createdItem = DBRequestTemplateLines.Create(item.ParseToDB());
                if (createdItem != null)
                {
                    item = createdItem.ParseToTemplateViewModel();
                    item.eReasonCode = 1;
                    item.eMessage = "Registo criado com sucesso.";
                }
                else
                {
                    item = new RequisitionTemplateLineViewModel();
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao criar o registo.";
                }
            }
            else
            {
                item = new RequisitionTemplateLineViewModel();
                item.eReasonCode = 2;
                item.eMessage = "Ocorreu um erro: a linha não pode ser nula.";
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult UpdateRequisitionLines([FromBody] RequisitionTemplateViewModel item)
        {
            try
            {
                if (item != null && item.Lines != null)
                {
                    if (DBRequestTemplateLines.Update(item.Lines.ParseToDB()))
                    {
                        item.Lines.ForEach(x => x.Selected = false);
                        item.eReasonCode = 1;
                        item.eMessage = "Linhas atualizadas com sucesso.";
                        return Json(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //item.eReasonCode = 2;
                //item.eMessage = "Ocorreu um erro ao atualizar as linhas.";
            }
            item.eReasonCode = 2;
            item.eMessage = "Ocorreu um erro ao atualizar as linhas.";
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult DeleteRequisitionLine([FromBody] RequisitionTemplateLineViewModel item)
        {
            if (item != null)
            {
                if (DBRequestTemplateLines.Delete(item.ParseToDB()))
                {
                    item.eReasonCode = 1;
                    item.eMessage = "Registo eliminado com sucesso.";
                }
                else
                {
                    item = new RequisitionTemplateLineViewModel();
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao eliminar o registo.";
                }
            }
            else
            {
                item = new RequisitionTemplateLineViewModel();
                item.eReasonCode = 2;
                item.eMessage = "Ocorreu um erro: a linha não pode ser nula.";
            }
            return Json(item);
        }

        //1
        [HttpPost]
        public async Task<JsonResult> ExportToExcel_ModelosRequisicao([FromBody] List<RequisitionViewModel> dp)
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
                ISheet excelSheet = workbook.CreateSheet("Modelos de Requisição");
                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("Nº Requisição");
                row.CreateCell(1).SetCellValue("ERegião Mercado Local");
                row.CreateCell(2).SetCellValue("Data Mercado Local");
                row.CreateCell(3).SetCellValue("Código Região");
                row.CreateCell(4).SetCellValue("Código Área Funcional");
                row.CreateCell(5).SetCellValue("Código Centro Responsabilidade");
                row.CreateCell(6).SetCellValue("Nº Consulta Mercado");
                row.CreateCell(7).SetCellValue("Nº Encomenda");
                row.CreateCell(8).SetCellValue("Nº Requisição Reclamada");
                row.CreateCell(9).SetCellValue("Data requisição");

                if (dp != null)
                {
                    int count = 1;
                    foreach (RequisitionViewModel item in dp)
                    {
                        row = excelSheet.CreateRow(count);
                        row.CreateCell(0).SetCellValue(item.RequisitionNo);
                        row.CreateCell(1).SetCellValue(item.LocalMarketRegion);
                        row.CreateCell(2).SetCellValue(item.LocalMarketDate.ToString());
                        row.CreateCell(3).SetCellValue(item.RegionCode);
                        row.CreateCell(4).SetCellValue(item.FunctionalAreaCode);
                        row.CreateCell(5).SetCellValue(item.CenterResponsibilityCode);
                        row.CreateCell(6).SetCellValue(item.MarketInquiryNo);
                        row.CreateCell(7).SetCellValue(item.OrderNo);
                        row.CreateCell(8).SetCellValue(item.RequestReclaimNo);
                        row.CreateCell(9).SetCellValue(item.RequisitionDate);
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
        public IActionResult ExportToExcelDownload_ModelosRequisicao(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Modelos de Requisição.xlsx");
        }
    }
}