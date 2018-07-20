using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Logic;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.FolhaDeHora;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.NAV;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.Logic.Viatura;
using Hydra.Such.Data.ViewModel;
using static Hydra.Such.Data.Enumerations;
using Hydra.Such.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class ViaturasController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ViaturasController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult Viaturas()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.Viaturas);

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

        public IActionResult DetalhesViatura(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.Viaturas);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.Matricula = id == null ? "" : id;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetList()
        {

            List<ViaturasViewModel> result = DBViatura.ParseListToViewModel(DBViatura.GetAllToList());

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CodigoRegiao));
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CodigoAreaFuncional));
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CodigoCentroResponsabilidade));

            result.ForEach(x =>
            {

                if (x.Estado != null) x.EstadoDescricao = EnumerablesFixed.ViaturasEstado.Where(y => y.Id == x.Estado).FirstOrDefault().Value;
                if (x.TipoCombustivel != null) x.TipoCombustivelDescricao = EnumerablesFixed.ViaturasTipoCombustivel.Where(y => y.Id == x.TipoCombustivel).FirstOrDefault().Value;
                if (x.TipoPropriedade != null) x.TipoPropriedadeDescricao = EnumerablesFixed.ViaturasTipoPropriedade.Where(y => y.Id == x.TipoPropriedade).FirstOrDefault().Value;
                if (x.CodigoMarca != null) x.Marca = DBMarcas.ParseToViewModel(DBMarcas.GetById(Int32.Parse(x.CodigoMarca)));
                if (x.CodigoModelo != null) x.Modelo = DBModelos.ParseToViewModel(DBModelos.GetById(Int32.Parse(x.CodigoModelo)));
                if (x.CodigoTipoViatura != null) x.TipoViatura = DBTiposViaturas.ParseToViewModel(DBTiposViaturas.GetById(Int32.Parse(x.CodigoTipoViatura)));
            });

            return Json(result);
        }
        

        [HttpPost]
        public JsonResult GetViaturaDetails([FromBody] ViaturasViewModel data)
        {
            if (data != null)
            {
                Viaturas viatura = DBViatura.GetByMatricula(data.Matricula);

                if (viatura != null)
                {
                    ViaturasViewModel result = DBViatura.ParseToViewModel(viatura);

                    return Json(result);
                }

                return Json(new ViaturasViewModel());
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult CreateViatura([FromBody] ViaturasViewModel data)
        {

            try
            {
                if (data != null)
                {

                    if (data.Matricula != null)
                    {
                        Viaturas viatura = DBViatura.ParseToDB(data);

                        viatura.UtilizadorCriação = User.Identity.Name;

                        viatura = DBViatura.Create(viatura);
                        data.eReasonCode = 1;

                        if (viatura == null)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Ocorreu um erro ao criar a viatura no portal.";
                        }
                    }

                }
            }
            catch (Exception e)
            {
                data.eReasonCode = 4;
                data.eMessage = "Ocorreu um erro ao criar a viatura";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult UpdateViatura([FromBody] ViaturasViewModel data)
        {

            if (data != null)
            {
                Viaturas viatura = DBViatura.ParseToDB(data);
                viatura.UtilizadorModificação = User.Identity.Name;

                DBViatura.Update(viatura);
                return Json(data);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult DeleteViatura([FromBody] ViaturasViewModel data)
        {

            if (data != null)
            {
                ErrorHandler result = new ErrorHandler();
                DBViatura.Delete(DBViatura.ParseToDB(data));
                result = new ErrorHandler()
                {
                    eReasonCode = 0,
                    eMessage = "Viatura removida com sucesso."
                };
                return Json(result);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult UploadImage([FromBody] Object data)
        {
            return Json(data);
        }

        //1
        [HttpPost]
        public async Task<JsonResult> ExportToExcel_Viaturas([FromBody] List<ViaturasViewModel> dp)
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
                ISheet excelSheet = workbook.CreateSheet("Viaturas");
                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("Matrícula");
                row.CreateCell(1).SetCellValue("Data Matrícula");
                row.CreateCell(2).SetCellValue("Estado");
                row.CreateCell(3).SetCellValue("Tipo Viatura");
                row.CreateCell(4).SetCellValue("Código Marca");
                row.CreateCell(5).SetCellValue("Marca");
                row.CreateCell(6).SetCellValue("Código Modelo");
                row.CreateCell(7).SetCellValue("Modelo");
                row.CreateCell(8).SetCellValue("Código Região");
                row.CreateCell(9).SetCellValue("Código Área Funcional");
                row.CreateCell(10).SetCellValue("Código Centro Responsabilidade");
                row.CreateCell(11).SetCellValue("Utilizador Criação");
                row.CreateCell(12).SetCellValue("Data/Hora Modificação");
                row.CreateCell(13).SetCellValue("Utilizador Modificação");

                if (dp != null)
                {
                    int count = 1;
                    foreach (ViaturasViewModel item in dp)
                    {
                        row = excelSheet.CreateRow(count);
                        row.CreateCell(0).SetCellValue(item.Matricula);
                        row.CreateCell(1).SetCellValue(item.DataMatricula);
                        row.CreateCell(2).SetCellValue(item.EstadoDescricao);
                        row.CreateCell(3).SetCellValue(item.TipoViatura.Descricao);
                        row.CreateCell(4).SetCellValue(item.CodigoMarca);
                        row.CreateCell(5).SetCellValue(item.Marca.Descricao);
                        row.CreateCell(6).SetCellValue(item.CodigoModelo);
                        row.CreateCell(7).SetCellValue(item.Modelo.Descricao);
                        row.CreateCell(8).SetCellValue(item.CodigoRegiao);
                        row.CreateCell(9).SetCellValue(item.CodigoAreaFuncional);
                        row.CreateCell(10).SetCellValue(item.CodigoCentroResponsabilidade);
                        row.CreateCell(11).SetCellValue(item.UtilizadorCriacao);
                        row.CreateCell(12).SetCellValue(item.DataHoraModificacao);
                        row.CreateCell(13).SetCellValue(item.UtilizadorModificacao);
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
        public IActionResult ExportToExcelDownload_Viaturas(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Viaturas.xlsx");
        }

    }
}
