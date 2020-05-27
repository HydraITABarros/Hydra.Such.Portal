using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hydra.Such.Data;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Data.ViewModel.ProjectView;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Portal.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
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
        private readonly GeneralConfigurations _generalConfig;

        public ModelosRequisicaoController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IHostingEnvironment _hostingEnvironment, IOptions<GeneralConfigurations> appSettingsGeneral)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
            _generalConfig = appSettingsGeneral.Value;
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

            return Json(result.OrderByDescending(x => x.RequisitionNo));
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
        public JsonResult CopyRequisition([FromBody] RequisitionTemplateViewModel itemOriginal)
        {
            try
            {
                if (itemOriginal != null && !string.IsNullOrEmpty(itemOriginal.RequisitionNo))
                {
                    //Get Numeration
                    bool autoGenId = true;
                    Configuração conf = DBConfigurations.GetById(1);
                    int entityNumerationConfId = conf.NumeracaoModelosRequisicao.Value;
                    string RequisitionNoOriginal = itemOriginal.RequisitionNo;

                    RequisitionTemplateViewModel copyItem = new RequisitionTemplateViewModel();
                    copyItem = itemOriginal;
                    copyItem.RequisitionNo = string.Empty;

                    if (autoGenId)
                    {
                        copyItem.RequisitionNo = DBNumerationConfigurations.GetNextNumeration(entityNumerationConfId, autoGenId, false);

                        if (copyItem != null && !string.IsNullOrEmpty(copyItem.RequisitionNo))
                        {
                            ConfiguraçãoNumerações configNum = DBNumerationConfigurations.GetById(entityNumerationConfId);
                            configNum.ÚltimoNºUsado = copyItem.RequisitionNo;
                            configNum.UtilizadorModificação = User.Identity.Name;
                            DBNumerationConfigurations.Update(configNum);
                        }
                    }

                    if (copyItem != null && !string.IsNullOrEmpty(copyItem.RequisitionNo))
                    {
                        copyItem.CreateUser = User.Identity.Name;
                        copyItem.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        copyItem.UpdateUser = string.Empty;
                        copyItem.UpdateDate = (DateTime?)null;
                        copyItem.Lines.ForEach(line =>
                        {
                            line.LineNo = 0;
                            line.CreateUser = User.Identity.Name;
                            line.CreateDateTime = DateTime.Now;
                            line.UpdateUser = string.Empty;
                            line.UpdateDateTime = (DateTime?)null;
                        });

                        copyItem = DBRequestTemplates.Create(copyItem.ParseToDB()).ParseToTemplateViewModel();
                        if (copyItem != null && !string.IsNullOrEmpty(copyItem.RequisitionNo))
                        {
                            itemOriginal.RequisitionNo = RequisitionNoOriginal;
                            itemOriginal.eReasonCode = 1;
                            itemOriginal.eMessage = "Novo Modelo de Requisição criado com sucesso com o Nº " + copyItem.RequisitionNo;
                        }
                        else
                        {
                            itemOriginal.RequisitionNo = RequisitionNoOriginal;
                            itemOriginal.eReasonCode = 2;
                            itemOriginal.eMessage = "Ocorreu um erro ao copiar o Modelo de Requisição.";
                        }
                    }
                    else
                    {
                        itemOriginal.RequisitionNo = RequisitionNoOriginal;
                        itemOriginal.eReasonCode = 3;
                        itemOriginal.eMessage = "A numeração configurada não é compativel com a inserida.";
                    }
                }
                else
                {
                    itemOriginal.eReasonCode = 4;
                    itemOriginal.eMessage = "Ocorreu um erro: o modelo de requisição não pode ser nulo.";
                }
            }
            catch (Exception ex)
            {
                itemOriginal.eReasonCode = 99;
                itemOriginal.eMessage = "Ocorreu um erro ao copiar o Modelo de Requisição.";
            }

            return Json(itemOriginal);
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
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_ModelosRequisicao([FromBody] List<RequisitionViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "ModelosRequisicao\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Modelos de Requisição");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["requisitionNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Requisição");
                    Col = Col + 1;
                }
                //if (dp["localMarketRegion"]["hidden"].ToString() == "False")
                //{
                //    row.CreateCell(Col).SetCellValue("ERegião Mercado Local");
                //    Col = Col + 1;
                //}
                //if (dp["localMarketDate"]["hidden"].ToString() == "False")
                //{
                //    row.CreateCell(Col).SetCellValue("Data Mercado Local");
                //    Col = Col + 1;
                //}
                if (dp["regionCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Região");
                    Col = Col + 1;
                }
                if (dp["functionalAreaCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Área Funcional");
                    Col = Col + 1;
                }
                if (dp["centerResponsibilityCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Centro Responsabilidade");
                    Col = Col + 1;
                }
                if (dp["comments"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Observações");
                    Col = Col + 1;
                }
                //if (dp["marketInquiryNo"]["hidden"].ToString() == "False")
                //{
                //    row.CreateCell(Col).SetCellValue("Nº Consulta Mercado");
                //    Col = Col + 1;
                //}
                //if (dp["orderNo"]["hidden"].ToString() == "False")
                //{
                //    row.CreateCell(Col).SetCellValue("Nº Encomenda");
                //    Col = Col + 1;
                //}
                //if (dp["requestReclaimNo"]["hidden"].ToString() == "False")
                //{
                //    row.CreateCell(Col).SetCellValue("Nº Requisição Reclamada");
                //    Col = Col + 1;
                //}
                //if (dp["requisitionDate"]["hidden"].ToString() == "False")
                //{
                //    row.CreateCell(Col).SetCellValue("Data requisição");
                //    Col = Col + 1;
                //}

                if (dp != null)
                {
                    int count = 1;
                    foreach (RequisitionViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["requisitionNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisitionNo);
                            Col = Col + 1;
                        }
                        //if (dp["localMarketRegion"]["hidden"].ToString() == "False")
                        //{
                        //    row.CreateCell(Col).SetCellValue(item.LocalMarketRegion);
                        //    Col = Col + 1;
                        //}
                        //if (dp["localMarketDate"]["hidden"].ToString() == "False")
                        //{
                        //    row.CreateCell(Col).SetCellValue(item.LocalMarketDate.ToString());
                        //    Col = Col + 1;
                        //}
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
                        if (dp["centerResponsibilityCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CenterResponsibilityCode);
                            Col = Col + 1;
                        }
                        if (dp["comments"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Comments);
                            Col = Col + 1;
                        }
                        //if (dp["marketInquiryNo"]["hidden"].ToString() == "False")
                        //{
                        //    row.CreateCell(Col).SetCellValue(item.MarketInquiryNo);
                        //    Col = Col + 1;
                        //}
                        //if (dp["orderNo"]["hidden"].ToString() == "False")
                        //{
                        //    row.CreateCell(Col).SetCellValue(item.OrderNo);
                        //    Col = Col + 1;
                        //}
                        //if (dp["requestReclaimNo"]["hidden"].ToString() == "False")
                        //{
                        //    row.CreateCell(Col).SetCellValue(item.RequestReclaimNo);
                        //    Col = Col + 1;
                        //}
                        //if (dp["requisitionDate"]["hidden"].ToString() == "False")
                        //{
                        //    row.CreateCell(Col).SetCellValue(item.RequisitionDate);
                        //    Col = Col + 1;
                        //}
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
            sFileName = _generalConfig.FileUploadFolder + "ModelosRequisicao\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Modelos de Requisição.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_RequisitionModelDetails([FromBody] List<RequisitionTemplateLineViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].Colunas;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "ModelosRequisicao\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            //var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Modelo de Requisição Linhas");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                row.CreateCell(Col).SetCellValue("Cód. Requisição"); Col = Col + 1;
                if (dp["localCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Localização"); Col = Col + 1; }
                if (dp["code"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Produto"); Col = Col + 1; }
                if (dp["description"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descrição"); Col = Col + 1; }
                if (dp["description2"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descrição 2"); Col = Col + 1; }
                if (dp["unitMeasureCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Unid. Medida"); Col = Col + 1; }
                if (dp["quantityToRequire"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Quantidade a Requerer"); Col = Col + 1; }
                if (dp["unitCost"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Custo Unitário"); Col = Col + 1; }
                if (dp["projectNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Ordem/Projeto"); Col = Col + 1; }
                if (dp["regionCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Região"); Col = Col + 1; }
                if (dp["functionalAreaCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Área Funcional"); Col = Col + 1; }
                if (dp["centerResponsibilityCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Centro Responsabilidade"); Col = Col + 1; }
                if (dp["supplierNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Fornecedor"); Col = Col + 1; }

                if (Lista != null)
                {
                    int count = 1;
                    foreach (RequisitionTemplateLineViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        row.CreateCell(Col).SetCellValue(item.RequestNo); Col = Col + 1;
                        if (dp["localCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.LocalCode); Col = Col + 1; }
                        if (dp["code"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Code); Col = Col + 1; }
                        if (dp["description"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Description); Col = Col + 1; }
                        if (dp["description2"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Description2); Col = Col + 1; }
                        if (dp["unitMeasureCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.UnitMeasureCode); Col = Col + 1; }
                        if (dp["quantityToRequire"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.QuantityToRequire.ToString()); Col = Col + 1; }
                        if (dp["unitCost"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.UnitCost.ToString()); Col = Col + 1; }
                        if (dp["projectNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ProjectNo); Col = Col + 1; }
                        if (dp["regionCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.RegionCode); Col = Col + 1; }
                        if (dp["functionalAreaCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.FunctionalAreaCode); Col = Col + 1; }
                        if (dp["centerResponsibilityCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CenterResponsibilityCode); Col = Col + 1; }
                        if (dp["supplierNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.SupplierNo); Col = Col + 1; }

                        count++;
                    }
                }
                workbook.Write(fs);
            }
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_RequisitionModelDetails(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "ModelosRequisicao\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Modelo de Requisição Linhas.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        #region Upload Excel
        [HttpPost]
        public JsonResult OnPostImport()
        {
            var files = Request.Form.Files;
            RequisitionTemplateViewModel Requisition = new RequisitionTemplateViewModel();

            try
            {
                if (files != null && files.Count > 0)
                {
                    List<NAVProductsViewModel> AllProducts = DBNAV2017Products.GetAllProductsCompras(_config.NAVDatabaseName, _config.NAVCompanyName, "");
                    List<NAVUnitOfMeasureViewModel> AllUnitMeasures = DBNAV2017UnitOfMeasure.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName);
                    List<NAVLocationsViewModel> AllLocations = DBNAV2017Locations.GetAllLocations(_config.NAVDatabaseName, _config.NAVCompanyName);
                    List<NAVProjectsViewModel> AllProjects = DBNAV2017Projects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, "");
                    List<NAVDimValueViewModel> AllRegions = DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name);
                    List<NAVDimValueViewModel> AllAreas = DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 2, User.Identity.Name);
                    List<NAVDimValueViewModel> AllCresps = DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 3, User.Identity.Name);
                    List<NAVVendorViewModel> AllVendors = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName);

                    RequisitionTemplateLineViewModel nrow = new RequisitionTemplateLineViewModel();
                    for (int i = 0; i < files.Count; i++)
                    {
                        IFormFile file = files[i];
                        string folderName = "Upload";
                        string webRootPath = _generalConfig.FileUploadFolder + "ModelosRequisicao\\" + "tmp\\";
                        string newPath = Path.Combine(webRootPath, folderName);
                        StringBuilder sb = new StringBuilder();
                        if (!Directory.Exists(newPath))
                        {
                            Directory.CreateDirectory(newPath);
                        }
                        if (file.Length > 0)
                        {
                            string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                            ISheet sheet;
                            string fullPath = Path.Combine(newPath, file.FileName);
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                                stream.Position = 0;
                                if (sFileExtension == ".xls")
                                {
                                    HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                                }
                                else
                                {
                                    XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                                }

                                IRow linha1 = sheet.GetRow(sheet.FirstRowNum + 1);
                                string RequisitionNo = linha1.GetCell(0).ToString();
                                Requisition = DBRequestTemplates.GetById(RequisitionNo).ParseToTemplateViewModel();

                                if (Requisition != null)
                                {
                                    decimal value;
                                    for (int j = (sheet.FirstRowNum + 1); j <= sheet.LastRowNum; j++)
                                    {
                                        IRow row = sheet.GetRow(j);
                                        if (row != null && !string.IsNullOrEmpty(row.GetCell(0).ToString()))
                                        {
                                            nrow = new RequisitionTemplateLineViewModel();

                                            NAVProductsViewModel Product = AllProducts.Where(x => x.Code == row.GetCell(2).ToString()).FirstOrDefault();
                                            NAVUnitOfMeasureViewModel UnitMeasure = AllUnitMeasures.Where(x => x.code == row.GetCell(5).ToString()).FirstOrDefault();
                                            NAVLocationsViewModel Location = AllLocations.Where(x => x.Code == row.GetCell(1).ToString()).FirstOrDefault();
                                            NAVProjectsViewModel Project = AllProjects.Where(x => x.No == row.GetCell(8).ToString()).FirstOrDefault();
                                            NAVDimValueViewModel Region = AllRegions.Where(x => x.Code == row.GetCell(9).ToString()).FirstOrDefault();
                                            NAVDimValueViewModel Area = AllAreas.Where(x => x.Code == row.GetCell(10).ToString()).FirstOrDefault();
                                            NAVDimValueViewModel Cresp = AllCresps.Where(x => x.Code == row.GetCell(11).ToString()).FirstOrDefault();
                                            NAVVendorViewModel Vendor = AllVendors.Where(x => x.No_ == row.GetCell(12).ToString()).FirstOrDefault();

                                            nrow.RequestNo = Requisition.RequisitionNo;
                                            nrow.LineNo = 0;
                                            nrow.Code = row.GetCell(2) == null ? "" : Product != null ? Product.Code : "";
                                            nrow.Description = row.GetCell(2) == null ? "" : Product != null ? Product.Name : "";
                                            nrow.Description2 = row.GetCell(4) == null ? "" : row.GetCell(4).ToString();
                                            nrow.UnitMeasureCode = row.GetCell(5) == null ? "" : UnitMeasure != null ? UnitMeasure.code : "";
                                            nrow.LocalCode = row.GetCell(1) == null ? "" : Location != null ? Location.Code : "";
                                            if (row.GetCell(6) != null && Decimal.TryParse(row.GetCell(6).ToString().Replace(".", ","), out value))
                                                nrow.QuantityToRequire = row.GetCell(6) == null ? (Decimal?)null : Convert.ToDecimal(row.GetCell(6).ToString().Replace(".", ","));
                                            if (row.GetCell(7) != null && decimal.TryParse(row.GetCell(7).ToString().Replace(".", ","), out value))
                                                nrow.UnitCost = row.GetCell(7) == null ? (Decimal?)null : Convert.ToDecimal(row.GetCell(7).ToString().Replace(".", ","));
                                            nrow.ProjectNo = row.GetCell(8) == null ? "" : Project != null ? Project.No : "";
                                            nrow.RegionCode = row.GetCell(9) == null ? "" : Region != null ? Region.Code : "";
                                            nrow.FunctionalAreaCode = row.GetCell(10) == null ? "" : Area != null ? Area.Code : "";
                                            nrow.CenterResponsibilityCode = row.GetCell(11) == null ? "" : Cresp != null ? Cresp.Code : "";
                                            nrow.CreateDateTime = DateTime.Now;
                                            nrow.CreateUser = User.Identity.Name;
                                            nrow.SupplierNo = row.GetCell(2) == null ? "" : Vendor != null ? Vendor.No_ : "";

                                            Requisition.Lines.Add(nrow);
                                        }
                                    }
                                }
                                else
                                    return Json(null);
                            }
                        }
                        else
                            return Json(null);
                    }
                }
                else
                    return Json(null);
            }
            catch (Exception ex)
            {
                return Json(null);
            }

            return Json(Requisition);
        }
        #endregion






    }
}