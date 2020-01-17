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
using Newtonsoft.Json.Linq;
using System.Drawing;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class ViaturasController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly GeneralConfigurations _generalConfig;

        public ViaturasController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IHostingEnvironment _hostingEnvironment, IOptions<GeneralConfigurations> appSettingsGeneral)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
            _generalConfig = appSettingsGeneral.Value;
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

        public IActionResult Viaturas2()
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

        public IActionResult DetalhesViatura2(string id)
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
                if (x.TipoCombustivel != null && x.TipoCombustivel != 0) x.TipoCombustivelDescricao = EnumerablesFixed.ViaturasTipoCombustivel.Where(y => y.Id == x.TipoCombustivel).FirstOrDefault().Value;
                if (x.TipoPropriedade != null && x.TipoPropriedade != 0) x.TipoPropriedadeDescricao = EnumerablesFixed.ViaturasTipoPropriedade.Where(y => y.Id == x.TipoPropriedade).FirstOrDefault().Value;
                if (!string.IsNullOrEmpty(x.CodigoMarca)) x.Marca = DBMarcas.ParseToViewModel(DBMarcas.GetById(Int32.Parse(x.CodigoMarca)));
                if (!string.IsNullOrEmpty(x.CodigoModelo)) x.Modelo = DBModelos.ParseToViewModel(DBModelos.GetById(Int32.Parse(x.CodigoModelo)));
                if (!string.IsNullOrEmpty(x.CodigoTipoViatura)) x.TipoViatura = DBTiposViaturas.ParseToViewModel(DBTiposViaturas.GetById(Int32.Parse(x.CodigoTipoViatura)));
            });

            return Json(result);
        }


        [HttpPost]
        public JsonResult GetList2()
        {
            List<Viaturas2ViewModel> result = DBViaturas2.ParseListToViewModel(DBViaturas2.GetAllToList());

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CodRegiao));
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CodAreaFuncional));
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CodCentroResponsabilidade));

            List<ConfiguracaoTabelas> AllConfTabelas = DBConfiguracaoTabelas.GetAll();
            List<Viaturas2Marcas> AllMarcas = DBViaturas2Marcas.GetAll();
            List<Viaturas2Modelos> AllModelos = DBViaturas2Modelos.GetAll();
            List<NAVProjectsViewModel> AllProjects = DBNAV2017Projects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, "");
            List<Viaturas2Parqueamento> AllParquamentos = DBViaturas2Parqueamento.GetAll();
            List<Viaturas2ParqueamentoLocal> AllPArqueamentosLocais = DBViaturas2ParqueamentoLocal.GetAll();

            result.ForEach(x =>
            {
                if (x.IDEstado != null) x.Estado = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_ESTADO" && y.ID == x.IDEstado).FirstOrDefault().Descricao;
                if (x.IDMarca != null) x.Marca = AllMarcas.Where(y => y.ID == x.IDMarca).FirstOrDefault().Marca;
                if (x.IDModelo != null) x.Modelo = AllModelos.Where(y => y.ID == x.IDModelo).FirstOrDefault().Modelo;
                if (x.IDTipoCaixa != null) x.TipoCaixa = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_TIPO_CAIXA" && y.ID == x.IDTipoCaixa).FirstOrDefault().Descricao;
                if (x.IDCategoria != null) x.Categoria = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_CATEGORIA" && y.ID == x.IDCategoria).FirstOrDefault().Descricao;
                if (x.IDTipo != null) x.Tipo = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_TIPO" && y.ID == x.IDTipo).FirstOrDefault().Descricao;
                if (x.IDCombustivel != null) x.Combustivel = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_COMBUSTIVEL" && y.ID == x.IDCombustivel).FirstOrDefault().Descricao;
                if (x.IDTipoPropriedade != null) x.TipoPropriedade = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_TIPO_PROPRIEDADE" && y.ID == x.IDTipoPropriedade).FirstOrDefault().Descricao;
                if (x.IDPropriedade != null) x.Propriedade = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_PROPRIEDADE" && y.ID == x.IDPropriedade).FirstOrDefault().Descricao;
                if (x.IDSegmentacao != null) x.Segmentacao = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_SEGMENTACAO" && y.ID == x.IDSegmentacao).FirstOrDefault().Descricao;
                if (x.AlvaraLicenca == true) x.AlvaraLicencaTexto = "Sim"; else x.AlvaraLicencaTexto = "Não";
                if (x.IDLocalParqueamento != null) x.LocalParqueamento = AllPArqueamentosLocais.Where(y => y.ID == AllParquamentos.Where(z => z.ID == x.IDLocalParqueamento).FirstOrDefault().IDLocal).FirstOrDefault().Local;
                if (!string.IsNullOrEmpty(x.NoProjeto)) x.Projeto = AllProjects.Where(y => y.No == x.NoProjeto).FirstOrDefault().Description;
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

                    if (DBViaturaImagem.GetByMatricula(data.Matricula) != null)
                        result.Imagem = DBViaturaImagem.GetByMatricula(result.Matricula).Imagem;

                    return Json(result);
                }

                return Json(new ViaturasViewModel());
            }
            return Json(false);
        }

        [Route("Viaturas/loadimage/{matricula}")]
        public ActionResult loadimage(string matricula)
        {
            //string matricula = "00-AA-00";
            string defaultImg = "_default";
            string imgPath = _generalConfig.FileUploadFolder + "Viaturas\\" + matricula + ".jpg";
            string imgPathDefault = _generalConfig.FileUploadFolder + "Viaturas\\" + defaultImg + ".jpg";

            if (System.IO.File.Exists(imgPath))
                return new FileStreamResult(new FileStream(imgPath, FileMode.Open), "image/jpeg");
            else
                return new FileStreamResult(new FileStream(imgPathDefault, FileMode.Open), "image/jpeg");
        }

        [Route("Viaturas/UploadImage/{matricula}")]
        public JsonResult UploadImage(string matricula)
        {
            try
            {
                var files = Request.Form.Files;
                foreach (var file in files)
                {
                    try
                    {
                        string extension = Path.GetExtension(file.FileName);
                        if (extension.ToLower() == ".jpg")
                        {
                            string imgPath = _generalConfig.FileUploadFolder + "Viaturas\\" + matricula + ".jpg";
                            if (System.IO.File.Exists(imgPath))
                                System.IO.File.Delete(imgPath);

                            using (FileStream dd = new FileStream(imgPath, FileMode.CreateNew))
                            {
                                file.CopyTo(dd);
                                dd.Dispose();

                                return Json(true);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult GetViatura2Details([FromBody] Viaturas2ViewModel data)
        {
            Viaturas2ViewModel viatura = new Viaturas2ViewModel();
            if (data != null)
            {
                viatura = DBViaturas2.ParseToViewModel(DBViaturas2.GetByMatricula(data.Matricula));

                viatura.IDEstadoOrinalDB = viatura.IDEstado;
                viatura.IDLocalParqueamentoOriginalDB = viatura.IDLocalParqueamento;
                viatura.CodRegiaoOriginalDB = viatura.CodRegiao;
                viatura.CodAreaFuncionalOriginalDB = viatura.CodAreaFuncional;
                viatura.CodCentroResponsabilidadeOriginalDB = viatura.CodCentroResponsabilidade;

                List<ConfiguracaoTabelas> AllConfTabelas = DBConfiguracaoTabelas.GetAll();
                List<Viaturas2Marcas> AllMarcas = DBViaturas2Marcas.GetAll();
                List<Viaturas2Modelos> AllModelos = DBViaturas2Modelos.GetAll();
                List<NAVProjectsViewModel> AllProjects = DBNAV2017Projects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, "");
                List<Viaturas2Parqueamento> AllParquamentos = DBViaturas2Parqueamento.GetAll();
                List<Viaturas2ParqueamentoLocal> AllPArqueamentosLocais = DBViaturas2ParqueamentoLocal.GetAll();

                if (viatura.IDEstado != null) viatura.Estado = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_ESTADO" && y.ID == viatura.IDEstado).FirstOrDefault().Descricao;
                if (viatura.IDMarca != null) viatura.Marca = AllMarcas.Where(y => y.ID == viatura.IDMarca).FirstOrDefault().Marca;
                if (viatura.IDModelo != null) viatura.Modelo = AllModelos.Where(y => y.ID == viatura.IDModelo).FirstOrDefault().Modelo;
                if (viatura.IDTipoCaixa != null) viatura.TipoCaixa = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_TIPO_CAIXA" && y.ID == viatura.IDTipoCaixa).FirstOrDefault().Descricao;
                if (viatura.IDCategoria != null) viatura.Categoria = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_CATEGORIA" && y.ID == viatura.IDCategoria).FirstOrDefault().Descricao;
                if (viatura.IDTipo != null) viatura.Tipo = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_TIPO" && y.ID == viatura.IDTipo).FirstOrDefault().Descricao;
                if (viatura.IDCombustivel != null) viatura.Combustivel = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_COMBUSTIVEL" && y.ID == viatura.IDCombustivel).FirstOrDefault().Descricao;
                if (viatura.IDTipoPropriedade != null) viatura.TipoPropriedade = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_TIPO_PROPRIEDADE" && y.ID == viatura.IDTipoPropriedade).FirstOrDefault().Descricao;
                if (viatura.IDPropriedade != null) viatura.Propriedade = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_PROPRIEDADE" && y.ID == viatura.IDPropriedade).FirstOrDefault().Descricao;
                if (viatura.IDSegmentacao != null) viatura.Segmentacao = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_SEGMENTACAO" && y.ID == viatura.IDSegmentacao).FirstOrDefault().Descricao;
                if (viatura.AlvaraLicenca == true) viatura.AlvaraLicencaTexto = "Sim"; else viatura.AlvaraLicencaTexto = "Não";
                if (viatura.IDLocalParqueamento != null) viatura.LocalParqueamento = AllPArqueamentosLocais.Where(y => y.ID == AllParquamentos.Where(z => z.ID == viatura.IDLocalParqueamento).FirstOrDefault().IDLocal).FirstOrDefault().Local;
                if (viatura.IDLocalParqueamento != null) viatura.IDLocal = AllParquamentos.Where(z => z.ID == viatura.IDLocalParqueamento).FirstOrDefault().IDLocal;
                if (!string.IsNullOrEmpty(viatura.NoProjeto)) viatura.Projeto = AllProjects.Where(y => y.No == viatura.NoProjeto).FirstOrDefault().Description;
            }
            return Json(viatura);
        }

        [HttpPost]
        public JsonResult CreateViatura([FromBody] ViaturasViewModel data)
        {

            try
            {
                if (data != null)
                {
                    if (data.Matricula != null && !string.IsNullOrEmpty(data.Matricula))
                    {
                        Viaturas viatura = DBViatura.GetByMatricula(data.Matricula);

                        if (viatura == null)
                        {
                            Viaturas viaturaToCreate = DBViatura.ParseToDB(data);
                            viaturaToCreate.UtilizadorCriação = User.Identity.Name;

                            if (string.IsNullOrEmpty(viaturaToCreate.NoProjeto))
                            {
                                string projectToSearch = "V" + viaturaToCreate.Matrícula;

                                List<NAVProjectsViewModel> AllProjects = DBNAV2017Projects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, projectToSearch).ToList();
                                if (AllProjects != null && AllProjects.Count > 0)
                                {
                                    viaturaToCreate.NoProjeto = projectToSearch;
                                }
                            }

                            viaturaToCreate = DBViatura.Create(viaturaToCreate);

                            if (data.Imagem != null)
                            {
                                ViaturasImagens ViaturaImagem = new ViaturasImagens
                                {
                                    Matricula = data.Matricula,
                                    Imagem = data.Imagem,
                                    UtilizadorCriacao = User.Identity.Name
                                };
                                DBViaturaImagem.Create(ViaturaImagem);
                            }

                            if (viaturaToCreate == null)
                            {
                                data.eReasonCode = 3;
                                data.eMessage = "Ocorreu um erro ao criar a viatura no portal.";
                            }
                            else
                                data.eReasonCode = 1;
                        }
                        else
                        {
                            data.eReasonCode = 4;
                            data.eMessage = "Não pode criar a Viatura, porque já existe uma Viatura com esta matrícula no eSUCH.";
                        }
                    }
                    else
                    {
                        data.eReasonCode = 5;
                        data.eMessage = "O campo Matrícula é de preenchimento obrigatório.";
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

                if (data.Imagem != null)
                {
                    ViaturasImagens ViaturaImagem = new ViaturasImagens
                    {
                        Matricula = data.Matricula,
                        Imagem = data.Imagem,
                        UtilizadorModificacao = User.Identity.Name
                    };
                    if (DBViaturaImagem.GetByMatricula(data.Matricula) != null)
                        DBViaturaImagem.Update(ViaturaImagem);
                    else
                        DBViaturaImagem.Create(ViaturaImagem);
                }


                return Json(data);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult UpdateViatura2([FromBody] Viaturas2ViewModel data)
        {

            if (data != null)
            {
                Viaturas2 viatura = DBViaturas2.ParseToDB(data);
                viatura.UtilizadorModificacao = User.Identity.Name;
                DBViaturas2.Update(viatura);

                //if (data.Imagem != null)
                //{
                //    ViaturasImagens ViaturaImagem = new ViaturasImagens
                //    {
                //        Matricula = data.Matricula,
                //        Imagem = data.Imagem,
                //        UtilizadorModificacao = User.Identity.Name
                //    };
                //    if (DBViaturaImagem.GetByMatricula(data.Matricula) != null)
                //        DBViaturaImagem.Update(ViaturaImagem);
                //    else
                //        DBViaturaImagem.Create(ViaturaImagem);
                //}


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

                if (DBViaturaImagem.GetByMatricula(data.Matricula) != null)
                {
                    ViaturasImagens ViaturaImagem = DBViaturaImagem.GetByMatricula(data.Matricula);
                    DBViaturaImagem.Delete(ViaturaImagem);
                }

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
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_Viaturas([FromBody] List<ViaturasViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Viaturas\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Viaturas");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["matricula"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Matrícula");
                    Col = Col + 1;
                }
                if (dp["dataMatricula"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Matrícula");
                    Col = Col + 1;
                }
                if (dp["estadoDescricao"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Estado");
                    Col = Col + 1;
                }
                if (dp["noProjeto"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Projeto");
                    Col = Col + 1;
                }
                if (dp["tipoViatura"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo Viatura");
                    Col = Col + 1;
                }
                if (dp["codigoMarca"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Marca");
                    Col = Col + 1;
                }
                if (dp["marca"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Marca");
                    Col = Col + 1;
                }
                if (dp["codigoModelo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Modelo");
                    Col = Col + 1;
                }
                if (dp["modelo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Modelo");
                    Col = Col + 1;
                }
                if (dp["codigoRegiao"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Região");
                    Col = Col + 1;
                }
                if (dp["codigoAreaFuncional"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Área Funcional");
                    Col = Col + 1;
                }
                if (dp["codigoCentroResponsabilidade"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Centro Responsabilidade");
                    Col = Col + 1;
                }
                if (dp["utilizadorCriacao"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Utilizador Criação");
                    Col = Col + 1;
                }
                if (dp["dataHoraModificacao"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data/Hora Modificação");
                    Col = Col + 1;
                }
                if (dp["utilizadorModificacao"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Utilizador Modificação");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ViaturasViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["matricula"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Matricula);
                            Col = Col + 1;
                        }
                        if (dp["dataMatricula"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DataMatricula);
                            Col = Col + 1;
                        }
                        if (dp["estadoDescricao"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.EstadoDescricao);
                            Col = Col + 1;
                        }
                        if (dp["noProjeto"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.NoProjeto);
                            Col = Col + 1;
                        }
                        if (dp["tipoViatura"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TipoViatura != null ? item.TipoViatura.Descricao : "");
                            Col = Col + 1;
                        }
                        if (dp["codigoMarca"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodigoMarca);
                            Col = Col + 1;
                        }
                        if (dp["marca"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Marca != null ? item.Marca.Descricao : "");
                            Col = Col + 1;
                        }
                        if (dp["codigoModelo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodigoModelo);
                            Col = Col + 1;
                        }
                        if (dp["modelo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Modelo != null ? item.Modelo.Descricao : "");
                            Col = Col + 1;
                        }
                        if (dp["codigoRegiao"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodigoRegiao);
                            Col = Col + 1;
                        }
                        if (dp["codigoAreaFuncional"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodigoAreaFuncional);
                            Col = Col + 1;
                        }
                        if (dp["codigoCentroResponsabilidade"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodigoCentroResponsabilidade);
                            Col = Col + 1;
                        }
                        if (dp["utilizadorCriacao"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.UtilizadorCriacao);
                            Col = Col + 1;
                        }
                        if (dp["dataHoraModificacao"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DataHoraModificacao);
                            Col = Col + 1;
                        }
                        if (dp["utilizadorModificacao"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.UtilizadorModificacao);
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
        public IActionResult ExportToExcelDownload_Viaturas(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Viaturas\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Viaturas.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_Viaturas2([FromBody] List<Viaturas2ViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Viaturas\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Viaturas");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["matricula"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Matrícula"); Col = Col + 1; }
                if (dp["estado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Estado"); Col = Col + 1; }
                if (dp["marca"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Marca"); Col = Col + 1; }
                if (dp["modelo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Modelo"); Col = Col + 1; }
                if (dp["data1MatriculaTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data 1ª Matrícula"); Col = Col + 1; }
                if (dp["cor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cor"); Col = Col + 1; }
                if (dp["dataMatriculaTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Matrícula"); Col = Col + 1; }
                if (dp["categoria"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Categoria"); Col = Col + 1; }
                if (dp["tipo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Tipo"); Col = Col + 1; }
                if (dp["classificacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Classificacao"); Col = Col + 1; }
                if (dp["cilindrada"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cilíndrada"); Col = Col + 1; }
                if (dp["combustivel"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Combústivel"); Col = Col + 1; }
                if (dp["consumoReferencia"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Consumo Referência"); Col = Col + 1; }
                if (dp["capacidadeDeposito"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Capacidade Depósito"); Col = Col + 1; }
                if (dp["autonomia"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Autonomia"); Col = Col + 1; }
                if (dp["pesoBruto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Peso Bruto"); Col = Col + 1; }
                if (dp["cargaMaxima"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Carga Máxima"); Col = Col + 1; }
                if (dp["tara"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Tara"); Col = Col + 1; }
                if (dp["potencia"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Potência"); Col = Col + 1; }
                if (dp["distanciaEixos"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Distância Eixos"); Col = Col + 1; }
                if (dp["noLugares"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Lugares"); Col = Col + 1; }
                if (dp["noAnosGarantia"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Anos Garantia"); Col = Col + 1; }
                if (dp["noQuadro"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Quadro"); Col = Col + 1; }
                if (dp["tipoCaixa"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Tipo Caixa"); Col = Col + 1; }
                if (dp["pneuFrente"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Pneu Frente"); Col = Col + 1; }
                if (dp["pneuRetaguarda"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Pneu Retaguarda"); Col = Col + 1; }
                if (dp["observacoes"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Observações"); Col = Col + 1; }
                if (dp["tipoPropriedade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Tipo Propriedade"); Col = Col + 1; }
                if (dp["propriedade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Propriedade"); Col = Col + 1; }
                if (dp["segmentacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Segmentação"); Col = Col + 1; }
                if (dp["dataProximaInspecaoTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Próxima Inspeção"); Col = Col + 1; }
                if (dp["intervaloRevisoes"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Intervalo Revisões"); Col = Col + 1; }
                if (dp["localParqueamento"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Local Parqueamento"); Col = Col + 1; }
                if (dp["alvaraLicencaTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Alvará Licença"); Col = Col + 1; }
                if (dp["codRegiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código Região"); Col = Col + 1; }
                if (dp["codAreaFuncional"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código Área Funcional"); Col = Col + 1; }
                if (dp["codCentroResponsabilidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código Centro Responsabilidade"); Col = Col + 1; }
                if (dp["noProjeto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Projeto"); Col = Col + 1; }
                if (dp["projeto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Projeto"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (Viaturas2ViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["matricula"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Matricula); Col = Col + 1; }
                        if (dp["estado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Estado); Col = Col + 1; }
                        if (dp["marca"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Marca); Col = Col + 1; }
                        if (dp["modelo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Modelo); Col = Col + 1; }
                        if (dp["data1MatriculaTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Data1MatriculaTexto); Col = Col + 1; }
                        if (dp["cor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Cor); Col = Col + 1; }
                        if (dp["dataMatriculaTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataMatriculaTexto); Col = Col + 1; }
                        if (dp["categoria"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Categoria); Col = Col + 1; }
                        if (dp["tipo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Tipo); Col = Col + 1; }
                        if (dp["classificacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Classificacao); Col = Col + 1; }
                        if (dp["cilindrada"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Cilindrada.ToString()); Col = Col + 1; }
                        if (dp["combustivel"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Combustivel); Col = Col + 1; }
                        if (dp["consumoReferencia"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ConsumoReferencia.ToString()); Col = Col + 1; }
                        if (dp["capacidadeDeposito"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CapacidadeDeposito.ToString()); Col = Col + 1; }
                        if (dp["autonomia"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Autonomia.ToString()); Col = Col + 1; }
                        if (dp["pesoBruto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.PesoBruto.ToString()); Col = Col + 1; }
                        if (dp["cargaMaxima"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CargaMaxima.ToString()); Col = Col + 1; }
                        if (dp["tara"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Tara.ToString()); Col = Col + 1; }
                        if (dp["potencia"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Potencia.ToString()); Col = Col + 1; }
                        if (dp["distanciaEixos"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DistanciaEixos.ToString()); Col = Col + 1; }
                        if (dp["noLugares"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NoLugares.ToString()); Col = Col + 1; }
                        if (dp["noAnosGarantia"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NoAnosGarantia.ToString()); Col = Col + 1; }
                        if (dp["noQuadro"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NoQuadro); Col = Col + 1; }
                        if (dp["tipoCaixa"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.TipoCaixa); Col = Col + 1; }
                        if (dp["pneuFrente"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.PneuFrente); Col = Col + 1; }
                        if (dp["pneuRetaguarda"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.PneuRetaguarda); Col = Col + 1; }
                        if (dp["observacoes"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Observacoes); Col = Col + 1; }
                        if (dp["tipoPropriedade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.TipoPropriedade); Col = Col + 1; }
                        if (dp["propriedade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Propriedade); Col = Col + 1; }
                        if (dp["segmentacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Segmentacao); Col = Col + 1; }
                        if (dp["dataProximaInspecaoTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataProximaInspecaoTexto); Col = Col + 1; }
                        if (dp["intervaloRevisoes"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.IntervaloRevisoes.ToString()); Col = Col + 1; }
                        if (dp["localParqueamento"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.LocalParqueamento); Col = Col + 1; }
                        if (dp["alvaraLicencaTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.AlvaraLicencaTexto); Col = Col + 1; }
                        if (dp["codRegiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodRegiao); Col = Col + 1; }
                        if (dp["codAreaFuncional"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodAreaFuncional); Col = Col + 1; }
                        if (dp["codCentroResponsabilidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodCentroResponsabilidade); Col = Col + 1; }
                        if (dp["noProjeto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NoProjeto); Col = Col + 1; }
                        if (dp["projeto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Projeto); Col = Col + 1; }
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
        public IActionResult ExportToExcelDownload_Viaturas2(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Viaturas\\" + "tmp\\" + sFileName;
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
