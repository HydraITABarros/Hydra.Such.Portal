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
using Hydra.Such.Data.ViewModel.Projects;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data.Logic.ComprasML;

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

        public IActionResult Viaturas2Movimentos(string matricula, string noProjeto)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Viaturas);

            if (UPerm != null && UPerm.Read.Value)
            {
                if (!string.IsNullOrEmpty(noProjeto))
                {
                    ViewBag.Matricula = matricula ?? "";
                    ViewBag.ProjectNo = noProjeto ?? "";
                    return View();
                }
                else
                {
                    return RedirectToAction("PageNotFound", "Error");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult Viaturas2Custos(string matricula, string noProjeto)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Viaturas);

            if (UPerm != null && UPerm.Read.Value)
            {
                if (!string.IsNullOrEmpty(noProjeto))
                {
                    ViewBag.Matricula = matricula ?? "";
                    ViewBag.ProjectNo = noProjeto ?? "";
                    return View();
                }
                else
                {
                    return RedirectToAction("PageNotFound", "Error");
                }
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
        public JsonResult GetList2([FromBody] JObject requestParams)
        {
            Boolean EstadoAtivas = Boolean.Parse(requestParams["ativas"].ToString());

            List<Viaturas2ViewModel> result = DBViaturas2.ParseListToViewModel(DBViaturas2.GetAllAtivas(EstadoAtivas));

            //Apply User Dimensions Validations
            AcessosUtilizador UserAcess = DBUserAccesses.GetById(User.Identity.Name, (int)Enumerations.Features.Viaturas);
            if (UserAcess != null && UserAcess.VerTudo != true)
            {
                List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                    result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CodRegiao));
                if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                    result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CodAreaFuncional));
                if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                    result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CodCentroResponsabilidade));
            }

            List<ConfiguracaoTabelas> AllConfTabelas = DBConfiguracaoTabelas.GetAll();
            List<Viaturas2Marcas> AllMarcas = DBViaturas2Marcas.GetAll();
            List<Viaturas2Modelos> AllModelos = DBViaturas2Modelos.GetAll();
            List<NAVProjectsViewModel> AllProjects = DBNAV2017Projects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, "");
            List<Viaturas2Parqueamento> AllParquamentos = DBViaturas2Parqueamento.GetAll();
            List<Viaturas2ParqueamentoLocal> AllPArqueamentosLocais = DBViaturas2ParqueamentoLocal.GetAll();
            List<Viaturas2GestoresGestor>  AllResponsaveis = DBViaturas2GestoresGestor.GetByTipo(1);

            result.ForEach(x =>
            {
                if (x.IDEstado != null && x.IDEstado > 0) x.Estado = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_ESTADO" && y.ID == x.IDEstado).FirstOrDefault().Descricao;
                if (x.IDMarca != null && x.IDMarca > 0) x.Marca = AllMarcas.Where(y => y.ID == x.IDMarca).FirstOrDefault().Marca;
                if (x.IDModelo != null && x.IDModelo > 0) x.Modelo = AllModelos.Where(y => y.ID == x.IDModelo).FirstOrDefault().Modelo;
                if (x.IDTipoCaixa != null && x.IDTipoCaixa > 0) x.TipoCaixa = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_TIPO_CAIXA" && y.ID == x.IDTipoCaixa).FirstOrDefault().Descricao;
                if (x.IDCategoria != null && x.IDCategoria > 0) x.Categoria = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_CATEGORIA" && y.ID == x.IDCategoria).FirstOrDefault().Descricao;
                if (x.IDTipo != null && x.IDTipo > 0) x.Tipo = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_TIPO" && y.ID == x.IDTipo).FirstOrDefault().Descricao;
                if (x.IDCombustivel != null && x.IDCombustivel > 0) x.Combustivel = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_COMBUSTIVEL" && y.ID == x.IDCombustivel).FirstOrDefault().Descricao;
                if (x.IDTipoPropriedade != null && x.IDTipoPropriedade > 0) x.TipoPropriedade = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_TIPO_PROPRIEDADE" && y.ID == x.IDTipoPropriedade).FirstOrDefault().Descricao;
                if (x.IDPropriedade != null && x.IDPropriedade > 0) x.Propriedade = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_PROPRIEDADE" && y.ID == x.IDPropriedade).FirstOrDefault().Descricao;
                if (x.IDSegmentacao != null && x.IDSegmentacao > 0) x.Segmentacao = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_SEGMENTACAO" && y.ID == x.IDSegmentacao).FirstOrDefault().Descricao;
                if (x.AlvaraLicenca == true) x.AlvaraLicencaTexto = "Sim"; else x.AlvaraLicencaTexto = "Não";
                if (x.IDLocalParqueamento != null && x.IDLocalParqueamento > 0) x.LocalParqueamento = AllPArqueamentosLocais.Where(y => y.ID == x.IDLocalParqueamento).FirstOrDefault().Local;
                if (!string.IsNullOrEmpty(x.NoProjeto)) x.Projeto = AllProjects.Where(y => y.No == x.NoProjeto).FirstOrDefault() != null ? AllProjects.Where(y => y.No == x.NoProjeto).FirstOrDefault().Description : "";
                if (x.IDGestor != null && x.IDGestor > 0) x.Gestor = AllResponsaveis.Where(y => y.ID == x.IDGestor).FirstOrDefault() != null ? AllResponsaveis.Where(y => y.ID == x.IDGestor).FirstOrDefault().Gestor : "";

                if (x.Data1Matricula.HasValue) x.Idade = (DateTime.Now.Year - Convert.ToDateTime(x.Data1Matricula).Year).ToString() + " ano(s)";
            });

            return Json(result.OrderBy(x => x.Matricula));
        }

        [HttpPost]
        public JsonResult GetMovimentos([FromBody] JObject requestParams)
        {
            string ProjectNo = (string)requestParams.GetValue("projectno");

            List<NAVProjectsMovementsViaturasViewModel> result = DBNAV2017Projects.GetAllMovimentsByViatura(_config.NAVDatabaseName, _config.NAVCompanyName, ProjectNo);

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetCustos([FromBody] JObject requestParams)
        {
            string ProjectNo = (string)requestParams.GetValue("projectno");

            List<NAVProjectsMovementsViaturasViewModel> result = DBNAV2017Projects.GetAllCustosByViatura(_config.NAVDatabaseName, _config.NAVCompanyName, ProjectNo);

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
            if (data != null && !string.IsNullOrEmpty(data.Matricula))
            {
                int IDCondutor = 0;
                viatura = DBViaturas2.ParseToViewModel(DBViaturas2.GetByMatricula(data.Matricula));

                viatura.IDEstadoOriginalDB = viatura.IDEstado;
                viatura.DataEstadoLast = DBViaturas2Estados.GetByMatriculaRecent(data.Matricula) != null ? DBViaturas2Estados.GetByMatriculaRecent(data.Matricula).DataInicio : DateTime.MinValue;
                viatura.CodRegiaoOriginalDB = viatura.CodRegiao;
                viatura.CodAreaFuncionalOriginalDB = viatura.CodAreaFuncional;
                viatura.CodCentroResponsabilidadeOriginalDB = viatura.CodCentroResponsabilidade;
                viatura.DataDimensaoLast = DBViaturas2Dimensoes.GetByMatriculaRecent(data.Matricula) != null ? DBViaturas2Dimensoes.GetByMatriculaRecent(data.Matricula).DataInicio : DateTime.MinValue;
                viatura.IDLocalParqueamentoOriginalDB = viatura.IDLocalParqueamento;
                viatura.DataParqueamentoLast = DBViaturas2Parqueamento.GetByMatriculaRecent(data.Matricula) != null ? DBViaturas2Parqueamento.GetByMatriculaRecent(data.Matricula).DataInicio : DateTime.MinValue;
                viatura.IDPropriedadeOriginalDB = viatura.IDPropriedade;
                viatura.DataPropriedadeLast = DBViaturas2Propriedades.GetByMatriculaRecent(data.Matricula) != null ? DBViaturas2Propriedades.GetByMatriculaRecent(data.Matricula).DataInicio : DateTime.MinValue;

                IDCondutor = DBViaturas2Gestores.GetByMatriculaGestorRecent(data.Matricula, DateTime.Now.Date, 2) != null ? (int)DBViaturas2Gestores.GetByMatriculaGestorRecent(data.Matricula, DateTime.Now.Date, 2).IDGestor : 0;
                if (IDCondutor > 0) viatura.Condutor = DBViaturas2GestoresGestor.GetByID(IDCondutor) != null ? DBViaturas2GestoresGestor.GetByID(IDCondutor).Gestor : "";

                viatura.DataProximaInspecaoTexto = DBViaturas2Inspecoes.GetByMatriculaProximaInspecaoRecent(data.Matricula) != null ? DBViaturas2Inspecoes.GetByMatriculaProximaInspecaoRecent(data.Matricula).ProximaInspecao.Value.ToString("yyyy-MM-dd") : "";

                List <ConfiguracaoTabelas> AllConfTabelas = DBConfiguracaoTabelas.GetAll();
                List<Viaturas2Marcas> AllMarcas = DBViaturas2Marcas.GetAll();
                List<Viaturas2Modelos> AllModelos = DBViaturas2Modelos.GetAll();
                List<NAVProjectsViewModel> AllProjects = DBNAV2017Projects.GetAllInDB(_config.NAVDatabaseName, _config.NAVCompanyName, "");
                List<Viaturas2Parqueamento> AllParquamentos = DBViaturas2Parqueamento.GetAll();
                List<Viaturas2ParqueamentoLocal> AllPArqueamentosLocais = DBViaturas2ParqueamentoLocal.GetAll();
                List<Viaturas2GestoresGestor> AllResponsaveis = DBViaturas2GestoresGestor.GetByTipo(1);

                if (viatura.IDEstado != null && viatura.IDEstado > 0) viatura.Estado = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_ESTADO" && y.ID == viatura.IDEstado).FirstOrDefault().Descricao;
                if (viatura.IDMarca != null && viatura.IDMarca > 0) viatura.Marca = AllMarcas.Where(y => y.ID == viatura.IDMarca).FirstOrDefault().Marca;
                if (viatura.IDModelo != null && viatura.IDModelo > 0) viatura.Modelo = AllModelos.Where(y => y.ID == viatura.IDModelo).FirstOrDefault().Modelo;
                if (viatura.IDTipoCaixa != null && viatura.IDTipoCaixa > 0) viatura.TipoCaixa = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_TIPO_CAIXA" && y.ID == viatura.IDTipoCaixa).FirstOrDefault().Descricao;
                if (viatura.IDCategoria != null && viatura.IDCategoria > 0) viatura.Categoria = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_CATEGORIA" && y.ID == viatura.IDCategoria).FirstOrDefault().Descricao;
                if (viatura.IDTipo != null && viatura.IDTipo > 0) viatura.Tipo = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_TIPO" && y.ID == viatura.IDTipo).FirstOrDefault().Descricao;
                if (viatura.IDCombustivel != null && viatura.IDCombustivel > 0) viatura.Combustivel = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_COMBUSTIVEL" && y.ID == viatura.IDCombustivel).FirstOrDefault().Descricao;
                if (viatura.IDTipoPropriedade != null && viatura.IDTipoPropriedade > 0) viatura.TipoPropriedade = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_TIPO_PROPRIEDADE" && y.ID == viatura.IDTipoPropriedade).FirstOrDefault().Descricao;
                if (viatura.IDPropriedade != null && viatura.IDPropriedade > 0) viatura.Propriedade = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_PROPRIEDADE" && y.ID == viatura.IDPropriedade).FirstOrDefault().Descricao;
                if (viatura.IDSegmentacao != null && viatura.IDSegmentacao > 0) viatura.Segmentacao = AllConfTabelas.Where(y => y.Tabela == "VIATURAS2_SEGMENTACAO" && y.ID == viatura.IDSegmentacao).FirstOrDefault().Descricao;
                if (viatura.AlvaraLicenca == true) viatura.AlvaraLicencaTexto = "Sim"; else viatura.AlvaraLicencaTexto = "Não";
                if (viatura.IDLocalParqueamento != null && viatura.IDLocalParqueamento > 0) viatura.LocalParqueamento = AllPArqueamentosLocais.Where(y => y.ID == viatura.IDLocalParqueamento).FirstOrDefault().Local;
                if (!string.IsNullOrEmpty(viatura.NoProjeto)) viatura.Projeto = AllProjects.Where(y => y.No == viatura.NoProjeto).FirstOrDefault().Description;
                if (viatura.IDGestor != null && viatura.IDGestor > 0) viatura.Gestor = AllResponsaveis.Where(y => y.ID == viatura.IDGestor).FirstOrDefault() != null ? AllResponsaveis.Where(y => y.ID == viatura.IDGestor).FirstOrDefault().Gestor : "";

                if (viatura.Data1Matricula.HasValue) viatura.Idade = (DateTime.Now.Year - Convert.ToDateTime(viatura.Data1Matricula).Year).ToString() + " ano(s)";

                if (!viatura.Data1Matricula.HasValue || !viatura.NoAnosGarantia.HasValue)
                {
                    if (!viatura.Data1Matricula.HasValue)
                        viatura.GarantiaSituacao = "Falta a Data da 1ª matrícula";
                    if (!viatura.NoAnosGarantia.HasValue)
                        viatura.GarantiaSituacao = "Falta a Garantia";
                    if (!viatura.Data1Matricula.HasValue && !viatura.NoAnosGarantia.HasValue)
                        viatura.GarantiaSituacao = "Falta a Data da 1ª matrícula e Garantia";
                }
                else
                {
                    if (viatura.Data1Matricula.HasValue && viatura.NoAnosGarantia.HasValue)
                    {
                        if (Convert.ToDateTime(viatura.Data1Matricula).AddYears((int)viatura.NoAnosGarantia) >= DateTime.Now.Date)
                            viatura.GarantiaSituacao = "Com garantia";
                        else
                            viatura.GarantiaSituacao = "Sem garantia";
                    }
                }

                Viaturas2CartaVerde seguro = DBViaturas2CartaVerde.GetByMatriculaAndData(data.Matricula, DateTime.Now.Date);
                if (seguro != null)
                {
                    viatura.SeguroSituacao = "Com seguro";
                    if (seguro.DataFim.HasValue)
                        viatura.DataFimSeguro = seguro.DataFim.Value.ToString("yyyy-MM-dd");
                }
                else
                    viatura.SeguroSituacao = "Sem seguro";

                Viaturas2Inspecoes LastInspecao = DBViaturas2Inspecoes.GetByMatriculaUltimaInspecaoRecent(data.Matricula);
                if (LastInspecao != null && LastInspecao.DataInspecao.HasValue)
                    viatura.UltimaInspecao = LastInspecao.DataInspecao.Value.ToString("yyyy-MM-dd");

                if (viatura.DataMatricula.HasValue)
                {
                    if (Convert.ToDateTime(viatura.DataMatricula).AddDays(-30) <= DateTime.Now.Date && Convert.ToDateTime(viatura.DataMatricula) >= DateTime.Now.Date)
                        viatura.IUCate = viatura.DataMatricula.Value.ToString("yyyy-MM-dd");
                }

                Viaturas2Afetacao LastAfetacao = DBViaturas2Afetacao.GetByMatriculaRecent(data.Matricula);
                if (LastAfetacao != null && LastAfetacao.IDAreaReal.HasValue && LastAfetacao.DataInicio.HasValue)
                    if (Convert.ToDateTime(LastAfetacao.DataInicio) <= DateTime.Now.Date && (LastAfetacao.DataFim.HasValue ? Convert.ToDateTime(LastAfetacao.DataFim) : DateTime.Now.Date) >= DateTime.Now.Date)
                        viatura.Afetacao = DBViaturas2AfetacaoAreaReal.GetByID((int)LastAfetacao.IDAreaReal).AreaReal;
            }
            return Json(viatura);
        }

        [HttpPost]
        public JsonResult GetViaturas2TabImobilizados([FromBody] Viaturas2ImobilizadosViewModel viatura)
        {
            List<Viaturas2ImobilizadosViewModel> TabImobilizados = new List<Viaturas2ImobilizadosViewModel>();
            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabImobilizados = DBViaturas2Imobilizados.ParseListToViewModel(DBViaturas2Imobilizados.GetByMatricula(viatura.Matricula));

                List<Viaturas2ImobilizadosViewModel> AllImobilizados = DBViaturas2Imobilizados.GetAllImobilizados(_config.NAVDatabaseName, _config.NAVCompanyName, "");
                TabImobilizados.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x.NoImobilizado))
                    {
                        Viaturas2ImobilizadosViewModel Imobilizado = AllImobilizados.Where(y => y.NoImobilizado == x.NoImobilizado).FirstOrDefault();
                        if (Imobilizado != null)
                        {
                            x.Descricao = !string.IsNullOrEmpty(Imobilizado.Descricao) ? Imobilizado.Descricao : "";
                            x.DataCompraTexto = !string.IsNullOrEmpty(Imobilizado.DataCompraTexto) ? Imobilizado.DataCompraTexto : "";
                            x.DocumentoCompra = !string.IsNullOrEmpty(Imobilizado.DocumentoCompra) ? Imobilizado.DocumentoCompra : "";
                            x.ValorCompra = !string.IsNullOrEmpty(Imobilizado.ValorCompra) ? Imobilizado.ValorCompra : "";
                            x.DataIncioAmortizacaoTexto = !string.IsNullOrEmpty(Imobilizado.DataIncioAmortizacaoTexto) ? Imobilizado.DataIncioAmortizacaoTexto : "";
                            x.DataFinalAmortizacaoTexto = !string.IsNullOrEmpty(Imobilizado.DataFinalAmortizacaoTexto) ? Imobilizado.DataFinalAmortizacaoTexto : "";
                            x.ValorAmortizado = !string.IsNullOrEmpty(Imobilizado.ValorAmortizado) ? Imobilizado.ValorAmortizado : "";
                            x.VendaAbate = !string.IsNullOrEmpty(Imobilizado.VendaAbate) ? Imobilizado.VendaAbate : "";
                            x.DataVendaAbateTexto = !string.IsNullOrEmpty(Imobilizado.DataVendaAbateTexto) ? Imobilizado.DataVendaAbateTexto : "";
                            x.DocumentoVendaAbate = !string.IsNullOrEmpty(Imobilizado.DocumentoVendaAbate) ? Imobilizado.DocumentoVendaAbate : "";
                            x.ValorVendaAbate = !string.IsNullOrEmpty(Imobilizado.ValorVendaAbate) ? Imobilizado.ValorVendaAbate : "";
                            x.EstadoImobilizado = !string.IsNullOrEmpty(Imobilizado.EstadoImobilizado) ? Imobilizado.EstadoImobilizado : "";
                            x.Bloqueado = !string.IsNullOrEmpty(Imobilizado.Bloqueado) ? Imobilizado.Bloqueado : "";
                        }
                    }
                });
            }
            return Json(TabImobilizados.OrderBy(x => x.NoImobilizado));
        }

        [HttpPost]
        public JsonResult GetViaturas2TabKm([FromBody] Viaturas2InspecoesViewModel viatura)
        {
            List<Viaturas2KmViewModel> TabKm = new List<Viaturas2KmViewModel>();
            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabKm = DBViaturas2Km.ParseListToViewModel(DBViaturas2Km.GetByMatricula(viatura.Matricula));
            }
            return Json(TabKm.OrderByDescending(x => x.Km));
        }

        [HttpPost]
        public JsonResult GetViaturas2TabManutencao([FromBody] Viaturas2InspecoesViewModel viatura)
        {
            List<Viaturas2ManutencaoViewModel> TabManutencao = new List<Viaturas2ManutencaoViewModel>();
            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabManutencao = DBViaturas2Manutencao.ParseListToViewModel(DBViaturas2Manutencao.GetByMatricula(viatura.Matricula));
            }
            return Json(TabManutencao.OrderByDescending(x => x.DataInicio));
        }

        [HttpPost]
        public JsonResult GetViaturas2TabInspecao([FromBody] Viaturas2InspecoesViewModel viatura)
        {
            List<Viaturas2InspecoesViewModel> TabInspecoes = new List<Viaturas2InspecoesViewModel>();
            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabInspecoes = DBViaturas2Inspecoes.ParseListToViewModel(DBViaturas2Inspecoes.GetByMatricula(viatura.Matricula));

                List<ConfiguracaoTabelas> AllResultados = DBConfiguracaoTabelas.GetAllByTabela("VIATURAS2_INSPECAO_RESULTADO");

                TabInspecoes.ForEach(x =>
                {
                    if (x.IDResultado != null) x.Resultado = AllResultados.Where(y => y.ID == x.IDResultado).FirstOrDefault().Descricao;
                });
            }
            return Json(TabInspecoes.OrderByDescending(x => x.DataInspecao));
        }

        [HttpPost]
        public JsonResult GetViaturas2TabCartaVerde([FromBody] Viaturas2CartaVerdeViewModel viatura)
        {
            List<Viaturas2CartaVerdeViewModel> TabCartaVerde = new List<Viaturas2CartaVerdeViewModel>();
            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabCartaVerde = DBViaturas2CartaVerde.ParseListToViewModel(DBViaturas2CartaVerde.GetByMatricula(viatura.Matricula));

                List<Viaturas2CartaVerdeSeguradora> AllSeguradoras = DBViaturas2CartaVerdeSeguradora.GetAll();

                TabCartaVerde.ForEach(x =>
                {
                    if (x.IDSeguradora != null && x.IDSeguradora > 0) x.Seguradora = AllSeguradoras.Where(y => y.ID == x.IDSeguradora).FirstOrDefault().Seguradora;
                });
            }
            return Json(TabCartaVerde.OrderByDescending(x => x.DataInicio));
        }

        [HttpPost]
        public JsonResult GetViaturas2TabViaVerde([FromBody] Viaturas2ViaVerdeViewModel viatura)
        {
            List<Viaturas2ViaVerdeViewModel> TabViaVerde = new List<Viaturas2ViaVerdeViewModel>();
            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabViaVerde = DBViaturas2ViaVerde.ParseListToViewModel(DBViaturas2ViaVerde.GetByMatricula(viatura.Matricula));

                List<ConfiguracaoTabelas> AllEmpresas = DBConfiguracaoTabelas.GetAllByTabela("VIATURAS2_VIAVERDE_EMPRESA");

                TabViaVerde.ForEach(x =>
                {
                    if (x.IDEmpresa != null && x.IDEmpresa > 0) x.Empresa = AllEmpresas.Where(y => y.ID == x.IDEmpresa).FirstOrDefault().Descricao;
                });
            }
            return Json(TabViaVerde.OrderByDescending(x => x.Data));
        }

        [HttpPost]
        public JsonResult GetViaturas2TabAcidentes([FromBody] Viaturas2InspecoesViewModel viatura)
        {
            List<Viaturas2AcidentesViewModel> TabAcidentes = new List<Viaturas2AcidentesViewModel>();
            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabAcidentes = DBViaturas2Acidentes.ParseListToViewModel(DBViaturas2Acidentes.GetByMatricula(viatura.Matricula));

                List<Viaturas2GestoresGestor> AllCondutores = DBViaturas2GestoresGestor.GetByTipo(2); //Condutor
                List<ConfiguracaoTabelas> AllResponsabilidades = DBConfiguracaoTabelas.GetAllByTabela("VIATURAS2_ACIDENTES_RESPONSABILIDADE");

                TabAcidentes.ForEach(x =>
                {
                    if (x.IDCondutor != null && x.IDCondutor > 0) x.Condutor = AllCondutores.Where(y => y.ID == x.IDCondutor).FirstOrDefault().Gestor;
                    if (x.IDResponsabilidade != null && x.IDResponsabilidade > 0) x.Responsabilidade = AllResponsabilidades.Where(y => y.ID == x.IDResponsabilidade).FirstOrDefault().Descricao;
                });
            }
            return Json(TabAcidentes.OrderByDescending(x => x.Data));
        }

        [HttpPost]
        public JsonResult GetViaturas2TabContraOrdenacoes([FromBody] Viaturas2InspecoesViewModel viatura)
        {
            List<Viaturas2ContraOrdenacoesViewModel> TabContraOrdenacoes = new List<Viaturas2ContraOrdenacoesViewModel>();
            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabContraOrdenacoes = DBViaturas2ContraOrdenacoes.ParseListToViewModel(DBViaturas2ContraOrdenacoes.GetByMatricula(viatura.Matricula));

                List<Viaturas2GestoresGestor> AllCondutores = DBViaturas2GestoresGestor.GetByTipo(2); //Condutor
                List<ConfiguracaoTabelas> AllResponsabilidades = DBConfiguracaoTabelas.GetAllByTabela("VIATURAS2_CONTRAORDENACOES_RESPONSABILIDADE");

                TabContraOrdenacoes.ForEach(x =>
                {
                    if (x.IDCondutor != null && x.IDCondutor > 0) x.Condutor = AllCondutores.Where(y => y.ID == x.IDCondutor).FirstOrDefault().Gestor;
                    if (x.IDResponsabilidade != null && x.IDResponsabilidade > 0) x.Responsabilidade = AllResponsabilidades.Where(y => y.ID == x.IDResponsabilidade).FirstOrDefault().Descricao;
                });
            }
            return Json(TabContraOrdenacoes.OrderByDescending(x => x.Data));
        }

        [HttpPost]
        public JsonResult GetViaturas2TabAfetacao([FromBody] Viaturas2AfetacaoViewModel viatura)
        {
            List<Viaturas2AfetacaoViewModel> TabAfetacao = new List<Viaturas2AfetacaoViewModel>();

            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabAfetacao = DBViaturas2Afetacao.ParseListToViewModel(DBViaturas2Afetacao.GetByMatricula(viatura.Matricula));

                List<Viaturas2AfetacaoAreaReal> AllAreasReais = DBViaturas2AfetacaoAreaReal.GetAll();

                TabAfetacao.ForEach(x =>
                {
                    if (x.IDAreaReal != null && x.IDAreaReal > 0) x.AreaReal = AllAreasReais.Where(y => y.ID == x.IDAreaReal).FirstOrDefault().AreaReal;
                });
            }
            return Json(TabAfetacao.OrderByDescending(x => x.DataInicio));
        }

        [HttpPost]
        public JsonResult GetViaturas2TabCondutor([FromBody] Viaturas2InspecoesViewModel viatura)
        {
            List<Viaturas2GestoresViewModel> TabGestores = new List<Viaturas2GestoresViewModel>();
            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabGestores = DBViaturas2Gestores.ParseListToViewModel(DBViaturas2Gestores.GetByMatriculaTipo(viatura.Matricula, 2));

                List<Viaturas2GestoresGestor> AllGestores = DBViaturas2GestoresGestor.GetAll();

                TabGestores.ForEach(x =>
                {
                    if (x.IDGestor != null) x.Gestor = AllGestores.Where(y => y.ID == x.IDGestor).FirstOrDefault().Gestor;
                });
            }
            return Json(TabGestores.OrderByDescending(x => x.DataInicio));
        }

        [HttpPost]
        public JsonResult GetViaturas2TabGestor([FromBody] Viaturas2InspecoesViewModel viatura)
        {
            List<Viaturas2GestoresViewModel> TabGestores = new List<Viaturas2GestoresViewModel>();
            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabGestores = DBViaturas2Gestores.ParseListToViewModel(DBViaturas2Gestores.GetByMatriculaTipo(viatura.Matricula, 1));

                List<Viaturas2GestoresGestor> AllGestores = DBViaturas2GestoresGestor.GetAll();

                TabGestores.ForEach(x =>
                {
                    if (x.IDGestor != null) x.Gestor = AllGestores.Where(y => y.ID == x.IDGestor).FirstOrDefault().Gestor;
                });
            }
            return Json(TabGestores.OrderByDescending(x => x.DataInicio));
        }

        [HttpPost]
        public JsonResult GetViaturas2TabCartaoCombustivel([FromBody] Viaturas2CartaoCombustivelViewModel viatura)
        {
            List<Viaturas2CartaoCombustivelViewModel> TabCartaoCombustivel = new List<Viaturas2CartaoCombustivelViewModel>();
            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabCartaoCombustivel = DBViaturas2CartaoCombustivel.ParseListToViewModel(DBViaturas2CartaoCombustivel.GetByMatricula(viatura.Matricula));

                List<ConfiguracaoTabelas> AllEmpresas = DBConfiguracaoTabelas.GetAllByTabela("VIATURAS2_CARTAOCOMBUSTIVEL_EMPRESA");

                TabCartaoCombustivel.ForEach(x =>
                {
                    if (x.IDEmpresa != null && x.IDEmpresa > 0) x.Empresa = AllEmpresas.Where(y => y.ID == x.IDEmpresa).FirstOrDefault().Descricao;
                });
            }
            return Json(TabCartaoCombustivel.OrderByDescending(x => x.DataInicio));
        }

        [HttpPost]
        public JsonResult GetViaturas2TabCarTrack([FromBody] Viaturas2CarTrackViewModel viatura)
        {
            List<Viaturas2CarTrackViewModel> TabCarTrack = new List<Viaturas2CarTrackViewModel>();
            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabCarTrack = DBViaturas2CarTrack.ParseListToViewModel(DBViaturas2CarTrack.GetByMatricula(viatura.Matricula));

                List<ConfiguracaoTabelas> AllEmpresas = DBConfiguracaoTabelas.GetAllByTabela("VIATURAS2_CARTRACK_EMPRESA");

                TabCarTrack.ForEach(x =>
                {
                    if (x.IDEmpresa != null && x.IDEmpresa > 0) x.Empresa = AllEmpresas.Where(y => y.ID == x.IDEmpresa).FirstOrDefault().Descricao;
                });
            }
            return Json(TabCarTrack.OrderByDescending(x => x.DataInicio));
        }

        [HttpPost]
        public JsonResult GetViaturas2TabAbastecimentos([FromBody] Viaturas2AbastecimentosViewModel viatura)
        {
            List<Viaturas2AbastecimentosViewModel> TabAbastecimentos = new List<Viaturas2AbastecimentosViewModel>();
            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabAbastecimentos = DBViaturas2Abastecimentos.ParseListToViewModel(DBViaturas2Abastecimentos.GetByMatricula(viatura.Matricula));

                List<ConfiguracaoTabelas> AllCombustiveis = DBConfiguracaoTabelas.GetAllByTabela("VIATURAS2_ABASTECIMENTOS_COMBUSTIVEL");
                List<ConfiguracaoTabelas> AllEmpresas = DBConfiguracaoTabelas.GetAllByTabela("VIATURAS2_CARTAOCOMBUSTIVEL_EMPRESA");
                List<Viaturas2CartaoCombustivel> AllCartoes = DBViaturas2CartaoCombustivel.GetAll();
                List<Viaturas2GestoresGestor> AllCondutores = DBViaturas2GestoresGestor.GetByTipo(2);

                TabAbastecimentos.ForEach(x =>
                {
                    if (x.IDCombustivel != null && x.IDCombustivel > 0) x.Combustivel = AllCombustiveis.Where(y => y.ID == x.IDCombustivel).FirstOrDefault().Descricao;
                    if (x.IDEmpresa != null && x.IDEmpresa > 0) x.Empresa = AllEmpresas.Where(y => y.ID == x.IDEmpresa).FirstOrDefault().Descricao;
                    if (x.IDCartao != null && x.IDCartao > 0) x.Cartao = AllCartoes.Where(y => y.ID == x.IDCartao).FirstOrDefault().NoCartao;
                    if (x.IDCondutor != null && x.IDCondutor > 0) x.Condutor = AllCondutores.Where(y => y.ID == x.IDCondutor).FirstOrDefault().Gestor;
                });
            }
            return Json(TabAbastecimentos.OrderByDescending(x => x.Data));
        }

        [HttpPost]
        public JsonResult GetViaturas2TabAbate([FromBody] Viaturas2AbateViewModel viatura)
        {
            List<Viaturas2AbateViewModel> TabAbate = new List<Viaturas2AbateViewModel>();
            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabAbate = DBViaturas2Abate.ParseListToViewModel(DBViaturas2Abate.GetByMatricula(viatura.Matricula));

                List<ConfiguracaoTabelas> AllAtosAdministrativos = DBConfiguracaoTabelas.GetAllByTabela("VIATURAS2_ABATE_ATO_ADMINISTRATIVO");
                List<ConfiguracaoTabelas> AllDescricaoAtos = DBConfiguracaoTabelas.GetAllByTabela("VIATURAS2_ABATE_DESCRICAO_ATO");

                TabAbate.ForEach(x =>
                {
                    if (x.IDTipoAtoAdministrativo != null && x.IDTipoAtoAdministrativo > 0) x.TipoAtoAdministrativo = AllAtosAdministrativos.Where(y => y.ID == x.IDTipoAtoAdministrativo).FirstOrDefault().Descricao;
                    if (x.IDDescricaoAto != null && x.IDDescricaoAto > 0) x.DescricaoAto = AllDescricaoAtos.Where(y => y.ID == x.IDDescricaoAto).FirstOrDefault().Descricao;
                });
            }
            return Json(TabAbate.OrderByDescending(x => x.Data));
        }

        [HttpPost]
        public JsonResult GetViaturas2TabEstado([FromBody] Viaturas2InspecoesViewModel viatura)
        {
            List<Viaturas2EstadosViewModel> TabEstados = new List<Viaturas2EstadosViewModel>();
            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabEstados = DBViaturas2Estados.ParseListToViewModel(DBViaturas2Estados.GetByMatricula(viatura.Matricula));

                List<ConfiguracaoTabelas> AllEstados = DBConfiguracaoTabelas.GetAllByTabela("VIATURAS2_ESTADO");

                TabEstados.ForEach(x =>
                {
                    if (x.IDEstado != null) x.Estado = AllEstados.Where(y => y.ID == x.IDEstado).FirstOrDefault().Descricao;
                });
            }
            return Json(TabEstados.OrderByDescending(x => x.DataInicio));
        }

        [HttpPost]
        public JsonResult GetViaturas2TabDimensao([FromBody] Viaturas2InspecoesViewModel viatura)
        {
            List<Viaturas2DimensoesViewModel> TabDimensoes = new List<Viaturas2DimensoesViewModel>();
            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabDimensoes = DBViaturas2Dimensoes.ParseListToViewModel(DBViaturas2Dimensoes.GetByMatricula(viatura.Matricula));

            }
            return Json(TabDimensoes.OrderByDescending(x => x.DataInicio));
        }

        [HttpPost]
        public JsonResult GetViaturas2TabParqueamento([FromBody] Viaturas2InspecoesViewModel viatura)
        {
            List<Viaturas2ParqueamentoViewModel> TabParqueamentos = new List<Viaturas2ParqueamentoViewModel>();
            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabParqueamentos = DBViaturas2Parqueamento.ParseListToViewModel(DBViaturas2Parqueamento.GetByMatricula(viatura.Matricula));

                List<Viaturas2ParqueamentoLocal> AllLocais = DBViaturas2ParqueamentoLocal.GetAll();

                TabParqueamentos.ForEach(x =>
                {
                    if (x.IDLocal != null) x.Local = AllLocais.Where(y => y.ID == x.IDLocal).FirstOrDefault().Local;
                });
            }
            return Json(TabParqueamentos.OrderByDescending(x => x.DataInicio));
        }

        [HttpPost]
        public JsonResult GetViaturas2TabPropriedade([FromBody] Viaturas2InspecoesViewModel viatura)
        {
            List<Viaturas2PropriedadesViewModel> TabPropriedades = new List<Viaturas2PropriedadesViewModel>();
            if (viatura != null && !string.IsNullOrEmpty(viatura.Matricula))
            {
                TabPropriedades = DBViaturas2Propriedades.ParseListToViewModel(DBViaturas2Propriedades.GetByMatricula(viatura.Matricula));

                List<ConfiguracaoTabelas> AllTiposPropriedades = DBConfiguracaoTabelas.GetAllByTabela("VIATURAS2_TIPO_PROPRIEDADE");
                List<ConfiguracaoTabelas> AllPropriedades = DBConfiguracaoTabelas.GetAllByTabela("VIATURAS2_PROPRIEDADE");

                TabPropriedades.ForEach(x =>
                {
                    if (x.IDTipoPropriedade != null) x.TipoPropriedade = AllTiposPropriedades.Where(y => y.ID == x.IDTipoPropriedade).FirstOrDefault().Descricao;
                    if (x.IDPropriedade != null) x.Propriedade = AllPropriedades.Where(y => y.ID == x.IDPropriedade).FirstOrDefault().Descricao;
                });
            }
            return Json(TabPropriedades.OrderByDescending(x => x.DataInicio));
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
        public JsonResult CreateViatura2([FromBody] Viaturas2ViewModel data)
        {
            try
            {
                if (data != null)
                {
                    if (data.Matricula != null && !string.IsNullOrEmpty(data.Matricula))
                    {
                        if (data.DataAquisicao > DateTime.Now)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "A Data de Aquisição não pode ser superior á data atual.";
                            return Json(data);
                        }

                        data.Matricula = data.Matricula.ToUpper();
                        data.NoProjeto = "V" + data.Matricula;
                        int CounteSUCH = 0;
                        int CountNAV2017 = 0;
                        int CountNAV2009 = 0;

                        Viaturas2 Viatura = DBViaturas2.GetByMatricula(data.Matricula);
                        if (Viatura != null)
                            CounteSUCH = 1;

                        List<NAVProjectsViewModel> AllProjectsNAV2017 = DBNAV2017Projects.GetAllInDB(_config.NAVDatabaseName, _config.NAVCompanyName, data.NoProjeto);
                        if (AllProjectsNAV2017 != null)
                            CountNAV2017 = AllProjectsNAV2017.Count;

                        List<NAVProjectsViewModel> AllProjectsNAV2009 = DBNAV2009Projects.GetAll(_config.NAV2009ServerName, _config.NAV2009DatabaseName, _config.NAV2009CompanyName, data.NoProjeto);
                        if (AllProjectsNAV2009 != null)
                            CountNAV2009 = AllProjectsNAV2009.Count;

                        if (CounteSUCH == 0 && CountNAV2017 == 0 && CountNAV2009 == 0)
                        {
                            //NAV2017
                            ProjectDetailsViewModel ProjectToCreate = new ProjectDetailsViewModel()
                            {
                                ProjectNo = data.NoProjeto,
                                Description = "CONTROLO CUSTOS VIATURAS: " + data.Matricula,
                                ClientNo = "999992",
                                Status = (EstadoProjecto)1, //ENCOMENDA
                                RegionCode = data.CodRegiao,
                                FunctionalAreaCode = data.CodAreaFuncional,
                                ResponsabilityCenterCode = data.CodCentroResponsabilidade,
                                Visivel = true
                            };

                            Task<WSCreateNAVProject.Create_Result> TCreateNavProj = WSProject.CreateNavProject(ProjectToCreate, _configws);
                            try
                            {
                                TCreateNavProj.Wait();
                            }
                            catch (Exception ex)
                            {
                                data.eReasonCode = 3;
                                data.eMessage = "Ocorreu um erro ao criar o projeto no NAV2017.";
                            }

                            if (TCreateNavProj.IsCompletedSuccessfully)
                            {
                                //NAV2009
                                int resultNAV2009 = DBNAV2009Projects.Create(data.Matricula, data.CodRegiao, data.CodAreaFuncional, data.CodCentroResponsabilidade, User.Identity.Name);

                                if (resultNAV2009 == 1)
                                {
                                    //e-SUCH
                                    data.IDSegmentacao = 0;
                                    if (data.IDCategoria.HasValue && data.IDTipo.HasValue)
                                    {
                                        if (data.IDCategoria == 1) //Viatura Ligeira
                                        {
                                            if (data.IDTipo == 1) //Passageiros
                                                data.IDSegmentacao = 1; //Viaturas Ligeiras de Passageiros
                                            if (data.IDTipo == 2) //Mercadorias
                                            {
                                                if (data.PesoBruto.HasValue)
                                                {
                                                    if (data.PesoBruto <= 2200)
                                                        data.IDSegmentacao = 2; //Viaturas de Mercadorias – Peso Bruto ≤ 2.200 Kg
                                                    if (data.PesoBruto > 2200 && data.PesoBruto <= 3500)
                                                        data.IDSegmentacao = 3; //Viaturas de Mercadorias – 2.200 Kg <Peso Bruto ≤ 3.500 Kg
                                                    if (data.PesoBruto > 3500 && data.PesoBruto <= 12000)
                                                        data.IDSegmentacao = 4; //Viaturas de Mercadorias – 3.500 Kg <Peso Bruto ≤ 12.000 Kg
                                                    if (data.PesoBruto > 12000 && data.PesoBruto <= 19000)
                                                        data.IDSegmentacao = 5; //Viaturas de Mercadorias – 12.000 Kg <Peso Bruto ≤ 19.000 Kg
                                                }
                                            }
                                        }
                                    }

                                    if (data.Data1Matricula.HasValue && !data.DataMatricula.HasValue)
                                        data.DataMatricula = data.Data1Matricula;
                                    if (!data.Data1Matricula.HasValue && data.DataMatricula.HasValue)
                                        data.Data1Matricula = data.DataMatricula;

                                    Viaturas2 viaturaCreated = DBViaturas2.Create(DBViaturas2.ParseToDB(data));

                                    if (viaturaCreated == null)
                                    {
                                        data.eReasonCode = 3;
                                        data.eMessage = "Ocorreu um erro ao criar a Viatura no e-SUCH.";
                                    }
                                    else
                                    {
                                        //Viaturas2
                                        Viaturas2Estados Estado = new Viaturas2Estados
                                        {
                                            Matricula = data.Matricula,
                                            IDEstado = data.IDEstado,
                                            DataInicio = (DateTime)data.DataAquisicao,
                                            UtilizadorCriacao = User.Identity.Name
                                        };
                                        DBViaturas2Estados.Create(Estado);

                                        Viaturas2Dimensoes Dimensao = new Viaturas2Dimensoes()
                                        {
                                            Matricula = data.Matricula,
                                            Regiao = data.CodRegiao,
                                            Area = data.CodAreaFuncional,
                                            Cresp = data.CodCentroResponsabilidade,
                                            DataInicio = (DateTime)data.DataAquisicao,
                                            UtilizadorCriacao = User.Identity.Name
                                        };
                                        DBViaturas2Dimensoes.Create(Dimensao);

                                        Viaturas2Parqueamento Parqueamento = new Viaturas2Parqueamento
                                        {
                                            Matricula = data.Matricula,
                                            IDLocal = data.IDLocalParqueamento,
                                            DataInicio = (DateTime)data.DataAquisicao,
                                            UtilizadorCriacao = User.Identity.Name
                                        };
                                        DBViaturas2Parqueamento.Create(Parqueamento);

                                        Viaturas2Propriedades Propriedade = new Viaturas2Propriedades
                                        {
                                            Matricula = data.Matricula,
                                            IDTipoPropriedade = data.IDTipoPropriedade,
                                            IDPropriedade = data.IDPropriedade,
                                            DataInicio = (DateTime)data.DataAquisicao,
                                            UtilizadorCriacao = User.Identity.Name
                                        };
                                        DBViaturas2Propriedades.Create(Propriedade);

                                        data.eReasonCode = 1;
                                        data.eMessage = "Viatura criada com sucesso.";
                                    }
                                }
                                else
                                {
                                    data.eReasonCode = 3;
                                    data.eMessage = "Ocorreu um erro ao criar o projeto no NAV2009.";
                                }
                            }
                            else
                            {
                                data.eReasonCode = 3;
                                data.eMessage = "Ocorreu um erro ao criar o projeto no NAV2017.";
                            }
                        }
                        else
                        {
                            if (CounteSUCH != 0)
                            {
                                data.eReasonCode = 3;
                                data.eMessage = "Já existe uma viatura no e-SUCH com a matricula " + data.Matricula;
                            }
                            if (CountNAV2017 != 0)
                            {
                                data.eReasonCode = 3;
                                data.eMessage = "Já existe um projeto no NAV2017 com o código " + data.NoProjeto;
                            }
                            if (CountNAV2009 != 0)
                            {
                                data.eReasonCode = 3;
                                data.eMessage = "Já existe um projeto no NAV2009 com o código " + data.NoProjeto;
                            }
                        }
                    }
                    else
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "O campo matricula é de preenchimento obrigatório";
                    }
                }
                else
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Erro na obtensão dos dados.";
                }
            }
            catch (Exception e)
            {
                data.eReasonCode = 4;
                data.eMessage = "Ocorreu um erro.";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult CreateViaturas2Imobilizados([FromBody] Viaturas2ImobilizadosViewModel Imobilizados)
        {
            try
            {
                if (Imobilizados != null && !string.IsNullOrEmpty(Imobilizados.Matricula) && !string.IsNullOrEmpty(Imobilizados.NoImobilizado))
                {
                    List<Viaturas2ImobilizadosViewModel> AllImobilizados = DBViaturas2Imobilizados.GetAllImobilizados(_config.NAVDatabaseName, _config.NAVCompanyName, "").Where(x => x.PaiFilho == Imobilizados.NoImobilizado).ToList();

                    foreach (Viaturas2ImobilizadosViewModel item in AllImobilizados)
                    {
                        Viaturas2Imobilizados ImobilizadosToCreate = new Viaturas2Imobilizados();
                        ImobilizadosToCreate.Matricula = Imobilizados.Matricula;
                        ImobilizadosToCreate.NoImobilizado = item.NoImobilizado;
                        ImobilizadosToCreate.UtilizadorCriacao = User.Identity.Name;

                        DBViaturas2Imobilizados.Create(ImobilizadosToCreate);
                    };

                    Imobilizados.eReasonCode = 1;
                    Imobilizados.eMessage = "Linha Imobilizado criada com sucesso.";
                }
                else
                {
                    Imobilizados.eReasonCode = 3;
                    Imobilizados.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                Imobilizados.eReasonCode = 4;
                Imobilizados.eMessage = "Ocorreu um erro.";
            }

            return Json(Imobilizados);
        }

        [HttpPost]
        public JsonResult CreateViaturas2Km([FromBody] Viaturas2KmViewModel Km)
        {
            try
            {
                if (Km != null && !string.IsNullOrEmpty(Km.Matricula))
                {
                    Viaturas2Km KmToCreate = new Viaturas2Km();

                    KmToCreate = DBViaturas2Km.ParseToDB(Km);
                    KmToCreate.UtilizadorCriacao = User.Identity.Name;

                    if (DBViaturas2Km.Create(KmToCreate) != null)
                    {
                        Km.eReasonCode = 1;
                        Km.eMessage = "Linha Km criada com sucesso.";
                    }
                    else
                    {
                        Km.eReasonCode = 3;
                        Km.eMessage = "Ocorreu um erro ao criar a Linha Km no e-SUCH.";
                    }
                }
                else
                {
                    Km.eReasonCode = 3;
                    Km.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                Km.eReasonCode = 4;
                Km.eMessage = "Ocorreu um erro.";
            }

            return Json(Km);
        }

        [HttpPost]
        public JsonResult CreateViaturas2Manutencao([FromBody] Viaturas2ManutencaoViewModel Manutencao)
        {
            try
            {
                if (Manutencao != null && !string.IsNullOrEmpty(Manutencao.Matricula))
                {
                    Viaturas2Manutencao ManutencaoToCreate = new Viaturas2Manutencao();

                    ManutencaoToCreate = DBViaturas2Manutencao.ParseToDB(Manutencao);
                    ManutencaoToCreate.UtilizadorCriacao = User.Identity.Name;

                    if (DBViaturas2Manutencao.Create(ManutencaoToCreate) != null)
                    {
                        Manutencao.eReasonCode = 1;
                        Manutencao.eMessage = "Linha Manutencao criada com sucesso.";
                    }
                    else
                    {
                        Manutencao.eReasonCode = 3;
                        Manutencao.eMessage = "Ocorreu um erro ao criar a Linha Manutencao no e-SUCH.";
                    }
                }
                else
                {
                    Manutencao.eReasonCode = 3;
                    Manutencao.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                Manutencao.eReasonCode = 4;
                Manutencao.eMessage = "Ocorreu um erro.";
            }

            return Json(Manutencao);
        }

        [HttpPost]
        public JsonResult CreateViaturas2Inspecao([FromBody] Viaturas2InspecoesViewModel inspecao)
        {
            try
            {
                if (inspecao != null && !string.IsNullOrEmpty(inspecao.Matricula))
                {
                    Viaturas2Inspecoes inspecaoToCreate = new Viaturas2Inspecoes();

                    inspecaoToCreate = DBViaturas2Inspecoes.ParseToDB(inspecao);
                    inspecaoToCreate.UtilizadorCriacao = User.Identity.Name;

                    if (DBViaturas2Inspecoes.Create(inspecaoToCreate) != null)
                    {
                        inspecao.eReasonCode = 1;
                        inspecao.eMessage = "Inspecao criada com sucesso.";
                    }
                    else
                    {
                        inspecao.eReasonCode = 3;
                        inspecao.eMessage = "Ocorreu um erro ao criar a Inspeção no e-SUCH.";
                    }
                }
                else
                {
                    inspecao.eReasonCode = 3;
                    inspecao.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                inspecao.eReasonCode = 4;
                inspecao.eMessage = "Ocorreu um erro.";
            }

            return Json(inspecao);
        }

        [HttpPost]
        public JsonResult CreateViaturas2CartaVerde([FromBody] Viaturas2CartaVerdeViewModel CartaVerde)
        {
            try
            {
                if (CartaVerde != null && !string.IsNullOrEmpty(CartaVerde.Matricula))
                {
                    Viaturas2CartaVerde CartaVerdeToCreate = new Viaturas2CartaVerde();

                    CartaVerdeToCreate = DBViaturas2CartaVerde.ParseToDB(CartaVerde);
                    CartaVerdeToCreate.UtilizadorCriacao = User.Identity.Name;

                    if (DBViaturas2CartaVerde.Create(CartaVerdeToCreate) != null)
                    {
                        CartaVerde.eReasonCode = 1;
                        CartaVerde.eMessage = "Linha Carta Verde criada com sucesso.";
                    }
                    else
                    {
                        CartaVerde.eReasonCode = 3;
                        CartaVerde.eMessage = "Ocorreu um erro ao criar a Linha Carta Verde no e-SUCH.";
                    }
                }
                else
                {
                    CartaVerde.eReasonCode = 3;
                    CartaVerde.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                CartaVerde.eReasonCode = 4;
                CartaVerde.eMessage = "Ocorreu um erro.";
            }

            return Json(CartaVerde);
        }

        [HttpPost]
        public JsonResult CreateViaturas2Afetacao([FromBody] Viaturas2AfetacaoViewModel Afetacao)
        {
            try
            {
                if (Afetacao != null && !string.IsNullOrEmpty(Afetacao.Matricula))
                {
                    Viaturas2Afetacao AfetacaoToCreate = new Viaturas2Afetacao();

                    AfetacaoToCreate = DBViaturas2Afetacao.ParseToDB(Afetacao);
                    AfetacaoToCreate.UtilizadorCriacao = User.Identity.Name;

                    if (DBViaturas2Afetacao.Create(AfetacaoToCreate) != null)
                    {
                        Afetacao.eReasonCode = 1;
                        Afetacao.eMessage = "Linha Afetação criada com sucesso.";
                    }
                    else
                    {
                        Afetacao.eReasonCode = 3;
                        Afetacao.eMessage = "Ocorreu um erro ao criar a Linha Afetação no e-SUCH.";
                    }
                }
                else
                {
                    Afetacao.eReasonCode = 3;
                    Afetacao.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                Afetacao.eReasonCode = 4;
                Afetacao.eMessage = "Ocorreu um erro.";
            }

            return Json(Afetacao);
        }

        [HttpPost]
        public JsonResult CreateViaturas2CartaoCombustivel([FromBody] Viaturas2CartaoCombustivelViewModel CartaoCombustivel)
        {
            try
            {
                if (CartaoCombustivel != null && !string.IsNullOrEmpty(CartaoCombustivel.Matricula))
                {
                    Viaturas2CartaoCombustivel CartaoCombustivelToCreate = new Viaturas2CartaoCombustivel();

                    CartaoCombustivelToCreate = DBViaturas2CartaoCombustivel.ParseToDB(CartaoCombustivel);
                    CartaoCombustivelToCreate.UtilizadorCriacao = User.Identity.Name;

                    if (DBViaturas2CartaoCombustivel.Create(CartaoCombustivelToCreate) != null)
                    {
                        CartaoCombustivel.eReasonCode = 1;
                        CartaoCombustivel.eMessage = "Linha Cartão de Combústivel criada com sucesso.";
                    }
                    else
                    {
                        CartaoCombustivel.eReasonCode = 3;
                        CartaoCombustivel.eMessage = "Ocorreu um erro ao criar a Linha Cartão de Combústivel no e-SUCH.";
                    }
                }
                else
                {
                    CartaoCombustivel.eReasonCode = 3;
                    CartaoCombustivel.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                CartaoCombustivel.eReasonCode = 4;
                CartaoCombustivel.eMessage = "Ocorreu um erro.";
            }

            return Json(CartaoCombustivel);
        }

        [HttpPost]
        public JsonResult CreateViaturas2CarTrack([FromBody] Viaturas2CarTrackViewModel CarTrack)
        {
            try
            {
                if (CarTrack != null && !string.IsNullOrEmpty(CarTrack.Matricula))
                {
                    Viaturas2CarTrack CarTrackToCreate = new Viaturas2CarTrack();

                    CarTrackToCreate = DBViaturas2CarTrack.ParseToDB(CarTrack);
                    CarTrackToCreate.UtilizadorCriacao = User.Identity.Name;

                    if (DBViaturas2CarTrack.Create(CarTrackToCreate) != null)
                    {
                        CarTrack.eReasonCode = 1;
                        CarTrack.eMessage = "Linha CarTrack criada com sucesso.";
                    }
                    else
                    {
                        CarTrack.eReasonCode = 3;
                        CarTrack.eMessage = "Ocorreu um erro ao criar a Linha CarTrack no e-SUCH.";
                    }
                }
                else
                {
                    CarTrack.eReasonCode = 3;
                    CarTrack.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                CarTrack.eReasonCode = 4;
                CarTrack.eMessage = "Ocorreu um erro.";
            }

            return Json(CarTrack);
        }

        [HttpPost]
        public JsonResult CreateViaturas2Abastecimentos([FromBody] Viaturas2AbastecimentosViewModel Abastecimentos)
        {
            try
            {
                if (Abastecimentos != null && !string.IsNullOrEmpty(Abastecimentos.Matricula))
                {
                    Viaturas2Abastecimentos AbastecimentosToCreate = new Viaturas2Abastecimentos();

                    AbastecimentosToCreate = DBViaturas2Abastecimentos.ParseToDB(Abastecimentos);
                    AbastecimentosToCreate.UtilizadorCriacao = User.Identity.Name;

                    if (DBViaturas2Abastecimentos.Create(AbastecimentosToCreate) != null)
                    {
                        Abastecimentos.eReasonCode = 1;
                        Abastecimentos.eMessage = "Linha Abastecimento criada com sucesso.";
                    }
                    else
                    {
                        Abastecimentos.eReasonCode = 3;
                        Abastecimentos.eMessage = "Ocorreu um erro ao criar a Linha Abastecimento no e-SUCH.";
                    }
                }
                else
                {
                    Abastecimentos.eReasonCode = 3;
                    Abastecimentos.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                Abastecimentos.eReasonCode = 4;
                Abastecimentos.eMessage = "Ocorreu um erro.";
            }

            return Json(Abastecimentos);
        }

        [HttpPost]
        public JsonResult CreateViaturas2Abate([FromBody] Viaturas2AbateViewModel Abate)
        {
            try
            {
                if (Abate != null && !string.IsNullOrEmpty(Abate.Matricula))
                {
                    Viaturas2Abate AbateToCreate = new Viaturas2Abate();

                    AbateToCreate = DBViaturas2Abate.ParseToDB(Abate);
                    AbateToCreate.UtilizadorCriacao = User.Identity.Name;

                    if (DBViaturas2Abate.Create(AbateToCreate) != null)
                    {
                        Abate.eReasonCode = 1;
                        Abate.eMessage = "Linha Abate criada com sucesso.";
                    }
                    else
                    {
                        Abate.eReasonCode = 3;
                        Abate.eMessage = "Ocorreu um erro ao criar a Linha Abate no e-SUCH.";
                    }
                }
                else
                {
                    Abate.eReasonCode = 3;
                    Abate.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                Abate.eReasonCode = 4;
                Abate.eMessage = "Ocorreu um erro.";
            }

            return Json(Abate);
        }

        [HttpPost]
        public JsonResult CreateViaturas2ViaVerde([FromBody] Viaturas2ViaVerdeViewModel ViaVerde)
        {
            try
            {
                if (ViaVerde != null && !string.IsNullOrEmpty(ViaVerde.Matricula))
                {
                    Viaturas2ViaVerde ViaVerdeToCreate = new Viaturas2ViaVerde();

                    ViaVerdeToCreate = DBViaturas2ViaVerde.ParseToDB(ViaVerde);
                    ViaVerdeToCreate.UtilizadorCriacao = User.Identity.Name;

                    if (DBViaturas2ViaVerde.Create(ViaVerdeToCreate) != null)
                    {
                        ViaVerde.eReasonCode = 1;
                        ViaVerde.eMessage = "Linha Via Verde criada com sucesso.";
                    }
                    else
                    {
                        ViaVerde.eReasonCode = 3;
                        ViaVerde.eMessage = "Ocorreu um erro ao criar a Linha Via Verde no e-SUCH.";
                    }
                }
                else
                {
                    ViaVerde.eReasonCode = 3;
                    ViaVerde.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                ViaVerde.eReasonCode = 4;
                ViaVerde.eMessage = "Ocorreu um erro.";
            }

            return Json(ViaVerde);
        }

        [HttpPost]
        public JsonResult CreateViaturas2Acidentes([FromBody] Viaturas2AcidentesViewModel Acidentes)
        {
            try
            {
                if (Acidentes != null && !string.IsNullOrEmpty(Acidentes.Matricula))
                {
                    Viaturas2Acidentes AcidentesToCreate = new Viaturas2Acidentes();

                    AcidentesToCreate = DBViaturas2Acidentes.ParseToDB(Acidentes);
                    AcidentesToCreate.UtilizadorCriacao = User.Identity.Name;

                    if (DBViaturas2Acidentes.Create(AcidentesToCreate) != null)
                    {
                        Acidentes.eReasonCode = 1;
                        Acidentes.eMessage = "Linha Acidentes criada com sucesso.";
                    }
                    else
                    {
                        Acidentes.eReasonCode = 3;
                        Acidentes.eMessage = "Ocorreu um erro ao criar a Linha Acidentes no e-SUCH.";
                    }
                }
                else
                {
                    Acidentes.eReasonCode = 3;
                    Acidentes.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                Acidentes.eReasonCode = 4;
                Acidentes.eMessage = "Ocorreu um erro.";
            }

            return Json(Acidentes);
        }

        [HttpPost]
        public JsonResult CreateViaturas2ContraOrdenacoes([FromBody] Viaturas2ContraOrdenacoesViewModel ContraOrdenacoes)
        {
            try
            {
                if (ContraOrdenacoes != null && !string.IsNullOrEmpty(ContraOrdenacoes.Matricula))
                {
                    Viaturas2ContraOrdenacoes ContraOrdenacoesToCreate = new Viaturas2ContraOrdenacoes();

                    ContraOrdenacoesToCreate = DBViaturas2ContraOrdenacoes.ParseToDB(ContraOrdenacoes);
                    ContraOrdenacoesToCreate.UtilizadorCriacao = User.Identity.Name;

                    if (DBViaturas2ContraOrdenacoes.Create(ContraOrdenacoesToCreate) != null)
                    {
                        ContraOrdenacoes.eReasonCode = 1;
                        ContraOrdenacoes.eMessage = "Linha Contra Ordenações criada com sucesso.";
                    }
                    else
                    {
                        ContraOrdenacoes.eReasonCode = 3;
                        ContraOrdenacoes.eMessage = "Ocorreu um erro ao criar a Linha Contra Ordenações no e-SUCH.";
                    }
                }
                else
                {
                    ContraOrdenacoes.eReasonCode = 3;
                    ContraOrdenacoes.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                ContraOrdenacoes.eReasonCode = 4;
                ContraOrdenacoes.eMessage = "Ocorreu um erro.";
            }

            return Json(ContraOrdenacoes);
        }

        [HttpPost]
        public JsonResult CreateViaturas2Condutor([FromBody] Viaturas2GestoresViewModel condutor)
        {
            try
            {
                if (condutor != null && !string.IsNullOrEmpty(condutor.Matricula) && condutor.DataInicio.HasValue)
                {
                    Viaturas2Gestores condutorToCreate = new Viaturas2Gestores();

                    condutorToCreate = DBViaturas2Gestores.ParseToDB(condutor);
                    condutorToCreate.IDTipo = 2; //Condutor
                    condutorToCreate.UtilizadorCriacao = User.Identity.Name;

                    if (DBViaturas2Gestores.Create(condutorToCreate) != null)
                    {
                        condutor.eReasonCode = 1;
                        condutor.eMessage = "Condutor criado com sucesso.";
                    }
                    else
                    {
                        condutor.eReasonCode = 3;
                        condutor.eMessage = "Ocorreu um erro ao criar o Condutor no e-SUCH.";
                    }
                }
                else
                {
                    condutor.eReasonCode = 3;
                    condutor.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                condutor.eReasonCode = 4;
                condutor.eMessage = "Ocorreu um erro.";
            }

            return Json(condutor);
        }

        [HttpPost]
        public JsonResult CreateViaturas2Gestor([FromBody] Viaturas2GestoresViewModel gestor)
        {
            try
            {
                if (gestor != null && !string.IsNullOrEmpty(gestor.Matricula) && gestor.DataInicio.HasValue)
                {
                    Viaturas2Gestores gestorToCreate = new Viaturas2Gestores();

                    gestorToCreate = DBViaturas2Gestores.ParseToDB(gestor);
                    gestorToCreate.IDTipo = 1; //Responsável
                    gestorToCreate.UtilizadorCriacao = User.Identity.Name;

                    if (DBViaturas2Gestores.Create(gestorToCreate) != null)
                    {
                        gestor.eReasonCode = 1;
                        gestor.eMessage = "Responsável criado com sucesso.";

                        Viaturas2Gestores GestorRecent = DBViaturas2Gestores.GetByMatriculaGestorRecent(gestor.Matricula, DateTime.Now, 1);
                        Viaturas2 Viatura = DBViaturas2.GetByMatricula(gestor.Matricula);
                        if (Viatura != null)
                        {
                            Viatura.IDGestor = GestorRecent != null ? GestorRecent.IDGestor : 0;
                            Viatura.UtilizadorModificacao = User.Identity.Name;
                            DBViaturas2.Update(Viatura);
                        }
                    }
                    else
                    {
                        gestor.eReasonCode = 3;
                        gestor.eMessage = "Ocorreu um erro ao criar o Responsável no e-SUCH.";
                    }
                }
                else
                {
                    gestor.eReasonCode = 3;
                    gestor.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                gestor.eReasonCode = 4;
                gestor.eMessage = "Ocorreu um erro.";
            }

            return Json(gestor);
        }

        [HttpPost]
        public JsonResult DeleteViaturas2Imobilizados([FromBody] Viaturas2ImobilizadosViewModel Imobilizados)
        {
            try
            {
                if (Imobilizados != null && Imobilizados.ID > 0 && !string.IsNullOrEmpty(Imobilizados.Matricula))
                {
                    Viaturas2Imobilizados ImobilizadosToDelete = new Viaturas2Imobilizados();

                    ImobilizadosToDelete = DBViaturas2Imobilizados.GetByID(Imobilizados.ID);

                    if (ImobilizadosToDelete != null)
                    {
                        if (DBViaturas2Imobilizados.Delete(ImobilizadosToDelete) == true)
                        {
                            Imobilizados.eReasonCode = 1;
                            Imobilizados.eMessage = "A linha Imobilizado Eliminada com sucesso.";
                        }
                        else
                        {
                            Imobilizados.eReasonCode = 3;
                            Imobilizados.eMessage = "Ocorreu um erro ao Eliminar a linha Imobilizado no e-SUCH.";
                        }
                    }
                    else
                    {
                        Imobilizados.eReasonCode = 3;
                        Imobilizados.eMessage = "Ocorreu um erro ao Eliminar ao ler a linha Imobilizado.";
                    }
                }
                else
                {
                    Imobilizados.eReasonCode = 3;
                    Imobilizados.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                Imobilizados.eReasonCode = 4;
                Imobilizados.eMessage = "Ocorreu um erro.";
            }

            return Json(Imobilizados);
        }

        [HttpPost]
        public JsonResult DeleteViaturas2Km([FromBody] Viaturas2KmViewModel km)
        {
            try
            {
                if (km != null && km.ID > 0 && !string.IsNullOrEmpty(km.Matricula))
                {
                    Viaturas2Km kmToDelete = new Viaturas2Km();

                    kmToDelete = DBViaturas2Km.GetByID(km.ID);

                    if (kmToDelete != null)
                    {
                        if (DBViaturas2Km.Delete(kmToDelete) == true)
                        {
                            km.eReasonCode = 1;
                            km.eMessage = "A linha Km Eliminada com sucesso.";
                        }
                        else
                        {
                            km.eReasonCode = 3;
                            km.eMessage = "Ocorreu um erro ao Eliminar a linha Km no e-SUCH.";
                        }
                    }
                    else
                    {
                        km.eReasonCode = 3;
                        km.eMessage = "Ocorreu um erro ao Eliminar ao ler a linha Km.";
                    }
                }
                else
                {
                    km.eReasonCode = 3;
                    km.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                km.eReasonCode = 4;
                km.eMessage = "Ocorreu um erro.";
            }

            return Json(km);
        }

        [HttpPost]
        public JsonResult DeleteViaturas2Manutencao([FromBody] Viaturas2ManutencaoViewModel Manutencao)
        {
            try
            {
                if (Manutencao != null && Manutencao.ID > 0 && !string.IsNullOrEmpty(Manutencao.Matricula))
                {
                    Viaturas2Manutencao ManutencaoToDelete = new Viaturas2Manutencao();

                    ManutencaoToDelete = DBViaturas2Manutencao.GetByID(Manutencao.ID);

                    if (ManutencaoToDelete != null)
                    {
                        if (DBViaturas2Manutencao.Delete(ManutencaoToDelete) == true)
                        {
                            Manutencao.eReasonCode = 1;
                            Manutencao.eMessage = "Manutenção Eliminada com sucesso.";
                        }
                        else
                        {
                            Manutencao.eReasonCode = 3;
                            Manutencao.eMessage = "Ocorreu um erro ao Eliminar a Manutenção no e-SUCH.";
                        }
                    }
                    else
                    {
                        Manutencao.eReasonCode = 3;
                        Manutencao.eMessage = "Ocorreu um erro ao Eliminar ao ler a Manutenção.";
                    }
                }
                else
                {
                    Manutencao.eReasonCode = 3;
                    Manutencao.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                Manutencao.eReasonCode = 4;
                Manutencao.eMessage = "Ocorreu um erro.";
            }

            return Json(Manutencao);
        }

        [HttpPost]
        public JsonResult DeleteViaturas2Inspecao([FromBody] Viaturas2InspecoesViewModel inspecao)
        {
            try
            {
                if (inspecao != null && inspecao.ID > 0 && !string.IsNullOrEmpty(inspecao.Matricula))
                {
                    Viaturas2Inspecoes inspecaoToDelete = new Viaturas2Inspecoes();

                    inspecaoToDelete = DBViaturas2Inspecoes.GetByID(inspecao.ID);

                    if (inspecaoToDelete != null)
                    {
                        if (DBViaturas2Inspecoes.Delete(inspecaoToDelete) == true)
                        {
                            inspecao.eReasonCode = 1;
                            inspecao.eMessage = "Inspecao Eliminada com sucesso.";
                        }
                        else
                        {
                            inspecao.eReasonCode = 3;
                            inspecao.eMessage = "Ocorreu um erro ao Eliminar a Inspeção no e-SUCH.";
                        }
                    }
                    else
                    {
                        inspecao.eReasonCode = 3;
                        inspecao.eMessage = "Ocorreu um erro ao Eliminar ao ler a Inspeção.";
                    }
                }
                else
                {
                    inspecao.eReasonCode = 3;
                    inspecao.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                inspecao.eReasonCode = 4;
                inspecao.eMessage = "Ocorreu um erro.";
            }

            return Json(inspecao);
        }

        [HttpPost]
        public JsonResult DeleteViaturas2CartaVerde([FromBody] Viaturas2CartaVerdeViewModel CartaVerde)
        {
            try
            {
                if (CartaVerde != null && CartaVerde.ID > 0 && !string.IsNullOrEmpty(CartaVerde.Matricula))
                {
                    Viaturas2CartaVerde CartaVerdeToDelete = new Viaturas2CartaVerde();

                    CartaVerdeToDelete = DBViaturas2CartaVerde.GetByID(CartaVerde.ID);

                    if (CartaVerdeToDelete != null)
                    {
                        if (DBViaturas2CartaVerde.Delete(CartaVerdeToDelete) == true)
                        {
                            CartaVerde.eReasonCode = 1;
                            CartaVerde.eMessage = "Carta Verde Eliminada com sucesso.";
                        }
                        else
                        {
                            CartaVerde.eReasonCode = 3;
                            CartaVerde.eMessage = "Ocorreu um erro ao Eliminar a Carta Verde no e-SUCH.";
                        }
                    }
                    else
                    {
                        CartaVerde.eReasonCode = 3;
                        CartaVerde.eMessage = "Ocorreu um erro ao Eliminar ao ler a Carta Verde.";
                    }
                }
                else
                {
                    CartaVerde.eReasonCode = 3;
                    CartaVerde.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                CartaVerde.eReasonCode = 4;
                CartaVerde.eMessage = "Ocorreu um erro.";
            }

            return Json(CartaVerde);
        }

        [HttpPost]
        public JsonResult DeleteViaturas2Afetacao([FromBody] Viaturas2AfetacaoViewModel Afetacao)
        {
            try
            {
                if (Afetacao != null && Afetacao.ID > 0 && !string.IsNullOrEmpty(Afetacao.Matricula))
                {
                    Viaturas2Afetacao AfetacaoToDelete = new Viaturas2Afetacao();

                    AfetacaoToDelete = DBViaturas2Afetacao.GetByID(Afetacao.ID);

                    if (AfetacaoToDelete != null)
                    {
                        if (DBViaturas2Afetacao.Delete(AfetacaoToDelete) == true)
                        {
                            Afetacao.eReasonCode = 1;
                            Afetacao.eMessage = "Linha Afetação Eliminada com sucesso.";
                        }
                        else
                        {
                            Afetacao.eReasonCode = 3;
                            Afetacao.eMessage = "Ocorreu um erro ao Eliminar a linha Afetação no e-SUCH.";
                        }
                    }
                    else
                    {
                        Afetacao.eReasonCode = 3;
                        Afetacao.eMessage = "Ocorreu um erro ao Eliminar ao ler a linha Afetação.";
                    }
                }
                else
                {
                    Afetacao.eReasonCode = 3;
                    Afetacao.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                Afetacao.eReasonCode = 4;
                Afetacao.eMessage = "Ocorreu um erro.";
            }

            return Json(Afetacao);
        }

        [HttpPost]
        public JsonResult DeleteViaturas2CartaoCombustivel([FromBody] Viaturas2CartaoCombustivelViewModel CartaoCombustivel)
        {
            try
            {
                if (CartaoCombustivel != null && CartaoCombustivel.ID > 0 && !string.IsNullOrEmpty(CartaoCombustivel.Matricula))
                {
                    Viaturas2CartaoCombustivel CartaoCombustivelToDelete = new Viaturas2CartaoCombustivel();

                    CartaoCombustivelToDelete = DBViaturas2CartaoCombustivel.GetByID(CartaoCombustivel.ID);

                    if (CartaoCombustivelToDelete != null)
                    {
                        if (DBViaturas2CartaoCombustivel.Delete(CartaoCombustivelToDelete) == true)
                        {
                            CartaoCombustivel.eReasonCode = 1;
                            CartaoCombustivel.eMessage = "Linha Cartão Combústivel Eliminada com sucesso.";
                        }
                        else
                        {
                            CartaoCombustivel.eReasonCode = 3;
                            CartaoCombustivel.eMessage = "Ocorreu um erro ao Eliminar a linha Cartão Combústivel no e-SUCH.";
                        }
                    }
                    else
                    {
                        CartaoCombustivel.eReasonCode = 3;
                        CartaoCombustivel.eMessage = "Ocorreu um erro ao Eliminar ao ler a linha Cartão Combústivel.";
                    }
                }
                else
                {
                    CartaoCombustivel.eReasonCode = 3;
                    CartaoCombustivel.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                CartaoCombustivel.eReasonCode = 4;
                CartaoCombustivel.eMessage = "Ocorreu um erro.";
            }

            return Json(CartaoCombustivel);
        }

        [HttpPost]
        public JsonResult DeleteViaturas2CarTrack([FromBody] Viaturas2CarTrackViewModel CarTrack)
        {
            try
            {
                if (CarTrack != null && CarTrack.ID > 0 && !string.IsNullOrEmpty(CarTrack.Matricula))
                {
                    Viaturas2CarTrack CarTrackToDelete = new Viaturas2CarTrack();

                    CarTrackToDelete = DBViaturas2CarTrack.GetByID(CarTrack.ID);

                    if (CarTrackToDelete != null)
                    {
                        if (DBViaturas2CarTrack.Delete(CarTrackToDelete) == true)
                        {
                            CarTrack.eReasonCode = 1;
                            CarTrack.eMessage = "Linha Car Track Eliminada com sucesso.";
                        }
                        else
                        {
                            CarTrack.eReasonCode = 3;
                            CarTrack.eMessage = "Ocorreu um erro ao Eliminar a linha Car Track no e-SUCH.";
                        }
                    }
                    else
                    {
                        CarTrack.eReasonCode = 3;
                        CarTrack.eMessage = "Ocorreu um erro ao Eliminar ao ler a linha Car Track.";
                    }
                }
                else
                {
                    CarTrack.eReasonCode = 3;
                    CarTrack.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                CarTrack.eReasonCode = 4;
                CarTrack.eMessage = "Ocorreu um erro.";
            }

            return Json(CarTrack);
        }

        [HttpPost]
        public JsonResult DeleteViaturas2Abastecimentos([FromBody] Viaturas2AbastecimentosViewModel Abastecimentos)
        {
            try
            {
                if (Abastecimentos != null && Abastecimentos.ID > 0 && !string.IsNullOrEmpty(Abastecimentos.Matricula))
                {
                    Viaturas2Abastecimentos AbastecimentosToDelete = new Viaturas2Abastecimentos();

                    AbastecimentosToDelete = DBViaturas2Abastecimentos.GetByID(Abastecimentos.ID);

                    if (AbastecimentosToDelete != null)
                    {
                        if (DBViaturas2Abastecimentos.Delete(AbastecimentosToDelete) == true)
                        {
                            Abastecimentos.eReasonCode = 1;
                            Abastecimentos.eMessage = "Linha Abastecimentos Eliminada com sucesso.";
                        }
                        else
                        {
                            Abastecimentos.eReasonCode = 3;
                            Abastecimentos.eMessage = "Ocorreu um erro ao Eliminar a linha Abastecimentos no e-SUCH.";
                        }
                    }
                    else
                    {
                        Abastecimentos.eReasonCode = 3;
                        Abastecimentos.eMessage = "Ocorreu um erro ao Eliminar ao ler a linha Abastecimentos.";
                    }
                }
                else
                {
                    Abastecimentos.eReasonCode = 3;
                    Abastecimentos.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                Abastecimentos.eReasonCode = 4;
                Abastecimentos.eMessage = "Ocorreu um erro.";
            }

            return Json(Abastecimentos);
        }

        [HttpPost]
        public JsonResult DeleteViaturas2Abate([FromBody] Viaturas2AbateViewModel Abate)
        {
            try
            {
                if (Abate != null && Abate.ID > 0 && !string.IsNullOrEmpty(Abate.Matricula))
                {
                    Viaturas2Abate AbateToDelete = new Viaturas2Abate();

                    AbateToDelete = DBViaturas2Abate.GetByID(Abate.ID);

                    if (AbateToDelete != null)
                    {
                        if (DBViaturas2Abate.Delete(AbateToDelete) == true)
                        {
                            Abate.eReasonCode = 1;
                            Abate.eMessage = "Linha Abate Eliminada com sucesso.";
                        }
                        else
                        {
                            Abate.eReasonCode = 3;
                            Abate.eMessage = "Ocorreu um erro ao Eliminar a linha Abate no e-SUCH.";
                        }
                    }
                    else
                    {
                        Abate.eReasonCode = 3;
                        Abate.eMessage = "Ocorreu um erro ao Eliminar ao ler a linha Abate.";
                    }
                }
                else
                {
                    Abate.eReasonCode = 3;
                    Abate.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                Abate.eReasonCode = 4;
                Abate.eMessage = "Ocorreu um erro.";
            }

            return Json(Abate);
        }

        [HttpPost]
        public JsonResult DeleteViaturas2ViaVerde([FromBody] Viaturas2ViaVerdeViewModel ViaVerde)
        {
            try
            {
                if (ViaVerde != null && ViaVerde.ID > 0 && !string.IsNullOrEmpty(ViaVerde.Matricula))
                {
                    Viaturas2ViaVerde ViaVerdeToDelete = new Viaturas2ViaVerde();

                    ViaVerdeToDelete = DBViaturas2ViaVerde.GetByID(ViaVerde.ID);

                    if (ViaVerdeToDelete != null)
                    {
                        if (DBViaturas2ViaVerde.Delete(ViaVerdeToDelete) == true)
                        {
                            ViaVerde.eReasonCode = 1;
                            ViaVerde.eMessage = "Linha Via Verde Eliminada com sucesso.";
                        }
                        else
                        {
                            ViaVerde.eReasonCode = 3;
                            ViaVerde.eMessage = "Ocorreu um erro ao Eliminar a linha Via Verde no e-SUCH.";
                        }
                    }
                    else
                    {
                        ViaVerde.eReasonCode = 3;
                        ViaVerde.eMessage = "Ocorreu um erro ao Eliminar ao ler a linha Via Verde.";
                    }
                }
                else
                {
                    ViaVerde.eReasonCode = 3;
                    ViaVerde.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                ViaVerde.eReasonCode = 4;
                ViaVerde.eMessage = "Ocorreu um erro.";
            }

            return Json(ViaVerde);
        }

        [HttpPost]
        public JsonResult DeleteViaturas2Acidentes([FromBody] Viaturas2AcidentesViewModel Acidentes)
        {
            try
            {
                if (Acidentes != null && Acidentes.ID > 0 && !string.IsNullOrEmpty(Acidentes.Matricula))
                {
                    Viaturas2Acidentes AcidentesToDelete = new Viaturas2Acidentes();

                    AcidentesToDelete = DBViaturas2Acidentes.GetByID(Acidentes.ID);

                    if (AcidentesToDelete != null)
                    {
                        if (DBViaturas2Acidentes.Delete(AcidentesToDelete) == true)
                        {
                            Acidentes.eReasonCode = 1;
                            Acidentes.eMessage = "Acidente Eliminado com sucesso.";
                        }
                        else
                        {
                            Acidentes.eReasonCode = 3;
                            Acidentes.eMessage = "Ocorreu um erro ao Eliminar o Acidente no e-SUCH.";
                        }
                    }
                    else
                    {
                        Acidentes.eReasonCode = 3;
                        Acidentes.eMessage = "Ocorreu um erro ao Eliminar ao ler o Acidente.";
                    }
                }
                else
                {
                    Acidentes.eReasonCode = 3;
                    Acidentes.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                Acidentes.eReasonCode = 4;
                Acidentes.eMessage = "Ocorreu um erro.";
            }

            return Json(Acidentes);
        }

        [HttpPost]
        public JsonResult DeleteViaturas2ContraOrdenacoes([FromBody] Viaturas2ContraOrdenacoesViewModel ContraOrdenacoes)
        {
            try
            {
                if (ContraOrdenacoes != null && ContraOrdenacoes.ID > 0 && !string.IsNullOrEmpty(ContraOrdenacoes.Matricula))
                {
                    Viaturas2ContraOrdenacoes ContraOrdenacoesToDelete = new Viaturas2ContraOrdenacoes();

                    ContraOrdenacoesToDelete = DBViaturas2ContraOrdenacoes.GetByID(ContraOrdenacoes.ID);

                    if (ContraOrdenacoesToDelete != null)
                    {
                        if (DBViaturas2ContraOrdenacoes.Delete(ContraOrdenacoesToDelete) == true)
                        {
                            ContraOrdenacoes.eReasonCode = 1;
                            ContraOrdenacoes.eMessage = "Contra Ordenações Eliminada com sucesso.";
                        }
                        else
                        {
                            ContraOrdenacoes.eReasonCode = 3;
                            ContraOrdenacoes.eMessage = "Ocorreu um erro ao Eliminar a Contra Ordenações no e-SUCH.";
                        }
                    }
                    else
                    {
                        ContraOrdenacoes.eReasonCode = 3;
                        ContraOrdenacoes.eMessage = "Ocorreu um erro ao Eliminar ao ler a Contra Ordenações.";
                    }
                }
                else
                {
                    ContraOrdenacoes.eReasonCode = 3;
                    ContraOrdenacoes.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                ContraOrdenacoes.eReasonCode = 4;
                ContraOrdenacoes.eMessage = "Ocorreu um erro.";
            }

            return Json(ContraOrdenacoes);
        }

        [HttpPost]
        public JsonResult DeleteViaturas2Condutor([FromBody] Viaturas2InspecoesViewModel Condutor)
        {
            try
            {
                if (Condutor != null && !string.IsNullOrEmpty(Condutor.Matricula))
                {
                    Viaturas2Gestores CondutorToDelete = new Viaturas2Gestores();

                    CondutorToDelete = DBViaturas2Gestores.GetByID(Condutor.ID);

                    if (CondutorToDelete != null)
                    {
                        if (DBViaturas2Gestores.Delete(CondutorToDelete) == true)
                        {
                            Condutor.eReasonCode = 1;
                            Condutor.eMessage = "Condutor Eliminado com sucesso.";
                        }
                        else
                        {
                            Condutor.eReasonCode = 3;
                            Condutor.eMessage = "Ocorreu um erro ao Eliminar o Condutor no e-SUCH.";
                        }
                    }
                    else
                    {
                        Condutor.eReasonCode = 3;
                        Condutor.eMessage = "Ocorreu um erro ao Eliminar ao ler o Condutor.";
                    }
                }
                else
                {
                    Condutor.eReasonCode = 3;
                    Condutor.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                Condutor.eReasonCode = 4;
                Condutor.eMessage = "Ocorreu um erro.";
            }

            return Json(Condutor);
        }

        [HttpPost]
        public JsonResult DeleteViaturas2Gestor([FromBody] Viaturas2InspecoesViewModel Gestor)
        {
            try
            {
                if (Gestor != null && !string.IsNullOrEmpty(Gestor.Matricula))
                {
                    Viaturas2Gestores GestorToDelete = new Viaturas2Gestores();

                    GestorToDelete = DBViaturas2Gestores.GetByID(Gestor.ID);

                    if (GestorToDelete != null)
                    {
                        if (DBViaturas2Gestores.Delete(GestorToDelete) == true)
                        {
                            Gestor.eReasonCode = 1;
                            Gestor.eMessage = "Responsável Eliminado com sucesso.";

                            Viaturas2Gestores GestorRecent = DBViaturas2Gestores.GetByMatriculaGestorRecent(Gestor.Matricula, DateTime.Now, 1);
                            Viaturas2 Viatura = DBViaturas2.GetByMatricula(Gestor.Matricula);
                            if (Viatura != null)
                            {
                                Viatura.IDGestor = GestorRecent != null ? GestorRecent.IDGestor : 0;
                                Viatura.UtilizadorModificacao = User.Identity.Name;
                                DBViaturas2.Update(Viatura);
                            }
                        }
                        else
                        {
                            Gestor.eReasonCode = 3;
                            Gestor.eMessage = "Ocorreu um erro ao Eliminar o Responsável no e-SUCH.";
                        }
                    }
                    else
                    {
                        Gestor.eReasonCode = 3;
                        Gestor.eMessage = "Ocorreu um erro ao Eliminar ao ler o Responsável.";
                    }
                }
                else
                {
                    Gestor.eReasonCode = 3;
                    Gestor.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception e)
            {
                Gestor.eReasonCode = 4;
                Gestor.eMessage = "Ocorreu um erro.";
            }

            return Json(Gestor);
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
                if (data.IDLocalParqueamentoOriginalDB != data.IDLocalParqueamento)
                {
                    if (data.DataParqueamento > DateTime.Now)
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "A data de Parqueamento não pode ser superior á data atual.";
                        return Json(data);
                    }

                    Viaturas2Parqueamento ParqueamentoRecent = DBViaturas2Parqueamento.GetByMatriculaRecent(data.Matricula);
                    if (ParqueamentoRecent != null && ParqueamentoRecent.DataInicio > data.DataParqueamento)
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "A data de Parqueamento tem que superior ou igual à última data de Parqueamento.";
                        return Json(data);
                    }
                }

                if (!string.IsNullOrEmpty(data.NoProjeto) && (data.IDEstadoOriginalDB != data.IDEstado || data.CodRegiaoOriginalDB != data.CodRegiao ||
                    data.CodAreaFuncionalOriginalDB != data.CodAreaFuncional || data.CodCentroResponsabilidadeOriginalDB != data.CodCentroResponsabilidade))
                {
                    int CountNAV2017 = 0;
                    int CountNAV2009 = 0;

                    List<NAVProjectsViewModel> AllProjectsNAV2017 = DBNAV2017Projects.GetAllInDB(_config.NAVDatabaseName, _config.NAVCompanyName, data.NoProjeto);
                    if (AllProjectsNAV2017 != null)
                        CountNAV2017 = AllProjectsNAV2017.Count;

                    List<NAVProjectsViewModel> AllProjectsNAV2009 = DBNAV2009Projects.GetAll(_config.NAV2009ServerName, _config.NAV2009DatabaseName, _config.NAV2009CompanyName, data.NoProjeto);
                    if (AllProjectsNAV2009 != null)
                        CountNAV2009 = AllProjectsNAV2009.Count;

                    if (CountNAV2017 == 0 || CountNAV2009 == 0)
                    {
                        if (CountNAV2017 == 0)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Não é possível atualizar a Viatura " + data.Matricula + " , pois a mesma não existe no NAV2017";
                            return Json(data);
                        }
                        if (CountNAV2009 == 0)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Não é possível atualizar a Viatura " + data.Matricula + " , pois a mesma não existe no NAV2009";
                            return Json(data);
                        }
                    }
                    else
                    {
                        int EstadoProjetoNAV2009 = 0;
                        int EstadoProjetoNAV2017 = 0;
                        if (data.IDEstado == 1 || data.IDEstado == 2 || data.IDEstado == 6) //ATIVO
                        {
                            EstadoProjetoNAV2009 = 2;
                            EstadoProjetoNAV2017 = 1;
                        }
                        if (data.IDEstado == 3 || data.IDEstado == 4 || data.IDEstado == 5) //DESATIVADO
                        {
                            EstadoProjetoNAV2009 = 3;
                            EstadoProjetoNAV2017 = 2;
                        }

                        ProjectDetailsViewModel ProjectToUpdate = new ProjectDetailsViewModel()
                        {
                            ProjectNo = data.NoProjeto,
                            Description = "CONTROLO CUSTOS VIATURAS: " + data.Matricula,
                            ClientNo = "999992",
                            Status = (EstadoProjecto)EstadoProjetoNAV2017, //ENCOMENDA
                            RegionCode = data.CodRegiao,
                            FunctionalAreaCode = data.CodAreaFuncional,
                            ResponsabilityCenterCode = data.CodCentroResponsabilidade,
                            Visivel = true
                        };

                        //Read NAV2017 Project Key
                        Task<WSCreateNAVProject.Read_Result> TReadNavProj = WSProject.GetNavProject(data.NoProjeto, _configws);
                        try
                        {
                            TReadNavProj.Wait();
                        }
                        catch (Exception ex)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Erro ao atualizar: Não foi possivel obter o Projeto a partir do NAV2017.";
                            return Json(data);
                        }

                        if (TReadNavProj.IsCompletedSuccessfully)
                        {
                            if (TReadNavProj.Result.WSJob == null)
                            {
                                data.eReasonCode = 3;
                                data.eMessage = "Erro ao atualizar: O projeto não existe no NAV2017";
                                return Json(data);
                            }
                            else
                            {
                                //Update Project on NAV2017
                                Task<WSCreateNAVProject.Update_Result> TUpdateNavProj = WSProject.UpdateNavProject(TReadNavProj.Result.WSJob.Key, ProjectToUpdate, _configws);
                                try
                                {
                                    TUpdateNavProj.Wait();
                                }
                                catch (Exception ex)
                                {
                                    data.eReasonCode = 3;
                                    data.eMessage = ex.InnerException.Message;
                                    return Json(data);
                                }

                                if (!TUpdateNavProj.IsCompletedSuccessfully)
                                {
                                    data.eReasonCode = 3;
                                    data.eMessage = "Erro ao atualizar: Não foi possivel atualizar o projeto no NAV2017";
                                    return Json(data);
                                }
                            }
                        }

                        //Update Project on NAV2009
                        int resultNAV2009 = DBNAV2009Projects.Update(data.Matricula, EstadoProjetoNAV2009, data.CodRegiao, data.CodAreaFuncional, data.CodCentroResponsabilidade, User.Identity.Name);

                        if (resultNAV2009 != 1)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Erro ao atualizar: Não foi possivel atualizar o projeto no NAV2009";
                            return Json(data);
                        }
                    }
                }

                //Update Viaturas on NAV2009
                NAV2009Viaturas updateViaturaNAV2009 = new NAV2009Viaturas();
                updateViaturaNAV2009 = DBNAV2009Viaturas.Get(data.Matricula);

                if (updateViaturaNAV2009 != null)
                {
                    updateViaturaNAV2009.DataMatricula = data.DataMatricula.HasValue ? Convert.ToDateTime(data.DataMatriculaTexto) : updateViaturaNAV2009.DataMatricula;
                    updateViaturaNAV2009.NoQuadro = !string.IsNullOrEmpty(data.NoQuadro) ? Convert.ToString(data.NoQuadro) : updateViaturaNAV2009.NoQuadro;
                    updateViaturaNAV2009.PesoBruto = data.PesoBruto.HasValue ? Convert.ToString(data.PesoBruto) : updateViaturaNAV2009.PesoBruto;
                    updateViaturaNAV2009.Tara = data.Tara.HasValue ? Convert.ToString(data.Tara) : updateViaturaNAV2009.Tara;
                    updateViaturaNAV2009.Cilindrada = data.Cilindrada.HasValue ? Convert.ToString(data.Cilindrada) : updateViaturaNAV2009.Cilindrada;
                    updateViaturaNAV2009.Potencia = data.Potencia.HasValue ? Convert.ToString(data.Potencia) : updateViaturaNAV2009.Potencia;
                    updateViaturaNAV2009.NoLugares = data.NoLugares.HasValue ? Convert.ToString(data.NoLugares) : updateViaturaNAV2009.NoLugares;
                    updateViaturaNAV2009.Cor = !string.IsNullOrEmpty(data.Cor) ? Convert.ToString(data.Cor) : updateViaturaNAV2009.Cor;
                    updateViaturaNAV2009.DistanciaEntreEixos = data.DistanciaEixos.HasValue ? Convert.ToString(data.DistanciaEixos) : updateViaturaNAV2009.DistanciaEntreEixos;
                    updateViaturaNAV2009.PneumaticosFrente = !string.IsNullOrEmpty(data.PneuFrente) ? Convert.ToString(data.PneuFrente) : updateViaturaNAV2009.PneumaticosFrente;
                    updateViaturaNAV2009.PneumaticosRetaguarda = !string.IsNullOrEmpty(data.PneuRetaguarda) ? Convert.ToString(data.PneuRetaguarda) : updateViaturaNAV2009.PneumaticosRetaguarda;
                    updateViaturaNAV2009.DataAquisicao = data.DataAquisicao.HasValue ? Convert.ToDateTime(data.DataAquisicaoTexto) : updateViaturaNAV2009.DataAquisicao;
                    updateViaturaNAV2009.GlobalDimension1Code = !string.IsNullOrEmpty(data.CodRegiao) ? Convert.ToString(data.CodRegiao) : updateViaturaNAV2009.GlobalDimension1Code;
                    updateViaturaNAV2009.GlobalDimension2Code = !string.IsNullOrEmpty(data.CodAreaFuncional) ? Convert.ToString(data.CodAreaFuncional) : updateViaturaNAV2009.GlobalDimension2Code;
                    updateViaturaNAV2009.ShortcutDimension3Code = !string.IsNullOrEmpty(data.CodCentroResponsabilidade) ? Convert.ToString(data.CodCentroResponsabilidade) : updateViaturaNAV2009.ShortcutDimension3Code;
                    updateViaturaNAV2009.Observacoes = !string.IsNullOrEmpty(data.Observacoes) ? Convert.ToString(data.Observacoes) : updateViaturaNAV2009.Observacoes;
                    updateViaturaNAV2009.Utilizador = !string.IsNullOrEmpty(User.Identity.Name) ? Convert.ToString(User.Identity.Name) : updateViaturaNAV2009.Utilizador;
                    updateViaturaNAV2009.DataAlteracao = DateTime.Now;
                    updateViaturaNAV2009.IntervaloRevisoes = data.IntervaloRevisoes.HasValue ? Convert.ToInt32(data.IntervaloRevisoes) : updateViaturaNAV2009.IntervaloRevisoes;
                    updateViaturaNAV2009.ConsumoIndicativoViatura = data.ConsumoReferencia.HasValue ? Convert.ToDecimal(data.ConsumoReferencia) : updateViaturaNAV2009.ConsumoIndicativoViatura;

                    if (data.IDEstado.HasValue)
                    {
                        if (data.IDEstado == 1) //Ativo
                            updateViaturaNAV2009.Estado = 0;
                        if (data.IDEstado == 2) //Cedido
                            updateViaturaNAV2009.Estado = 3;
                        //if (data.IDEstado == 3) //Devolvido
                        //    updateViaturaNAV2009.Estado = ;
                        if (data.IDEstado == 4) //Vendido
                            updateViaturaNAV2009.Estado = 1;
                        if (data.IDEstado == 5) //Abatido
                            updateViaturaNAV2009.Estado = 2;
                        //if (data.IDEstado == 6) //Em reparação
                        //    updateViaturaNAV2009.Estado = ;
                    }

                    if (data.IDCombustivel.HasValue)
                    {
                        if (data.IDCombustivel == 1) //Gasóleo
                            updateViaturaNAV2009.Combustivel = 1;
                        if (data.IDCombustivel == 2) //Gasolina
                            updateViaturaNAV2009.Combustivel = 0;
                        if (data.IDCombustivel == 3) //Elétrico
                            updateViaturaNAV2009.Combustivel = 3;
                        if (data.IDCombustivel == 4) //GPL
                            updateViaturaNAV2009.Combustivel = 2;
                        //if (data.IDCombustivel == 5) //Outro
                        //    updateViaturaNAV2009.Combustivel = ;
                    }

                    if (data.IDTipoPropriedade.HasValue)
                    {
                        if (data.IDTipoPropriedade == 1) //SUCH
                            updateViaturaNAV2009.TipoPropriedade = 0;
                        if (data.IDTipoPropriedade == 2) //Renting
                            updateViaturaNAV2009.TipoPropriedade = 1;
                        if (data.IDTipoPropriedade == 3) //Leasing
                            updateViaturaNAV2009.TipoPropriedade = 2;
                    }

                    if (data.IDLocalParqueamento.HasValue)
                        updateViaturaNAV2009.LocalParqueamento = DBViaturas2ParqueamentoLocal.GetByID((int)data.IDLocalParqueamento).Local;

                    if (DBNAV2009Viaturas.Update(updateViaturaNAV2009) == 0)
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "Erro ao atualizar: Não foi possivel atualizar a Viatura no NAV2009";
                        return Json(data);
                    }
                }

                //UPDATE E-SUCH
                int CounteSUCH = 0;

                Viaturas2 Viatura = DBViaturas2.GetByMatricula(data.Matricula);
                if (Viatura != null)
                    CounteSUCH = 1;

                if (CounteSUCH == 0)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Não é possível atualizar a Viatura " + data.Matricula + " , pois a mesma não existe no e-SUCH";
                    return Json(data);
                }
                else
                {
                    //SEGMENTAÇÃO
                    data.IDSegmentacao = 0;
                    if (data.IDCategoria.HasValue && data.IDTipo.HasValue)
                    {
                        if (data.IDCategoria == 1) //Viatura Ligeira
                        {
                            if (data.IDTipo == 1) //Passageiros
                                data.IDSegmentacao = 1; //Viaturas Ligeiras de Passageiros
                            if (data.IDTipo == 2) //Mercadorias
                            {
                                if (data.PesoBruto.HasValue)
                                {
                                    if (data.PesoBruto <= 2200)
                                        data.IDSegmentacao = 2; //Viaturas de Mercadorias – Peso Bruto ≤ 2.200 Kg
                                    if (data.PesoBruto > 2200 && data.PesoBruto <= 3500)
                                        data.IDSegmentacao = 3; //Viaturas de Mercadorias – 2.200 Kg <Peso Bruto ≤ 3.500 Kg
                                    if (data.PesoBruto > 3500 && data.PesoBruto <= 12000)
                                        data.IDSegmentacao = 4; //Viaturas de Mercadorias – 3.500 Kg <Peso Bruto ≤ 12.000 Kg
                                    if (data.PesoBruto > 12000 && data.PesoBruto <= 19000)
                                        data.IDSegmentacao = 5; //Viaturas de Mercadorias – 12.000 Kg <Peso Bruto ≤ 19.000 Kg
                                }
                            }
                        }
                    }

                    //ESTADO
                    if (data.IDEstadoOriginalDB != data.IDEstado)
                    {
                        if (data.DataEstado <= DateTime.Now && data.DataEstado >= data.DataEstadoLast)
                        {
                            if (data.DataEstado == data.DataEstadoLast)
                            {
                                Viaturas2Estados Estado = DBViaturas2Estados.GetByMatriculaRecent(data.Matricula);
                                Estado.IDEstado = data.IDEstado;
                                Estado.UtilizadorModificacao = User.Identity.Name;
                                DBViaturas2Estados.Update(Estado);
                            }
                            else
                            {
                                Viaturas2Estados Estado = new Viaturas2Estados
                                {
                                    Matricula = data.Matricula,
                                    IDEstado = data.IDEstado,
                                    DataInicio = (DateTime)data.DataEstado,
                                    UtilizadorCriacao = User.Identity.Name
                                };
                                DBViaturas2Estados.Create(Estado);
                            }
                        }
                    }

                    //DIMENSAO
                    if (data.CodRegiaoOriginalDB != data.CodRegiao || data.CodAreaFuncionalOriginalDB != data.CodAreaFuncional || data.CodCentroResponsabilidadeOriginalDB != data.CodCentroResponsabilidade)
                    {
                        if (data.DataDimensao <= DateTime.Now && data.DataDimensao >= data.DataDimensaoLast)
                        {
                            if (data.DataDimensao == data.DataDimensaoLast)
                            {
                                Viaturas2Dimensoes Dimensao = DBViaturas2Dimensoes.GetByMatriculaRecent(data.Matricula);
                                Dimensao.Regiao = data.CodRegiao;
                                Dimensao.Area = data.CodAreaFuncional;
                                Dimensao.Cresp = data.CodCentroResponsabilidade;
                                Dimensao.UtilizadorModificacao = User.Identity.Name;
                                DBViaturas2Dimensoes.Update(Dimensao);
                            }
                            else
                            {
                                Viaturas2Dimensoes Dimensao = new Viaturas2Dimensoes
                                {
                                    Matricula = data.Matricula,
                                    Regiao = data.CodRegiao,
                                    Area = data.CodAreaFuncional,
                                    Cresp = data.CodCentroResponsabilidade,
                                    DataInicio = (DateTime)data.DataDimensao,
                                    UtilizadorCriacao = User.Identity.Name
                                };
                                DBViaturas2Dimensoes.Create(Dimensao);
                            }
                        }
                    }
                    
                    //PARQUEAMENTO
                    if (data.IDLocalParqueamentoOriginalDB != data.IDLocalParqueamento)
                    {
                        if (data.DataParqueamento <= DateTime.Now && data.DataParqueamento >= data.DataParqueamentoLast)
                        {
                            if (data.DataParqueamento == data.DataParqueamentoLast)
                            {
                                Viaturas2Parqueamento Parqueamento = DBViaturas2Parqueamento.GetByMatriculaRecent(data.Matricula);
                                Parqueamento.IDLocal = data.IDLocalParqueamento;
                                Parqueamento.UtilizadorModificacao = User.Identity.Name;
                                DBViaturas2Parqueamento.Update(Parqueamento);
                            }
                            else
                            {
                                Viaturas2Parqueamento Parqueamento = new Viaturas2Parqueamento
                                {
                                    Matricula = data.Matricula,
                                    IDLocal = data.IDLocalParqueamento,
                                    DataInicio = (DateTime)data.DataParqueamento,
                                    UtilizadorCriacao = User.Identity.Name
                                };
                                DBViaturas2Parqueamento.Create(Parqueamento);
                            }
                        }
                    }

                    //PROPRIEDADE
                    if (data.IDPropriedadeOriginalDB != data.IDPropriedade)
                    {
                        if (data.DataPropriedade <= DateTime.Now && data.DataPropriedade >= data.DataPropriedadeLast)
                        {
                            if (data.DataPropriedade == data.DataPropriedadeLast)
                            {
                                Viaturas2Propriedades Propriedade = DBViaturas2Propriedades.GetByMatriculaRecent(data.Matricula);
                                Propriedade.IDTipoPropriedade = data.IDTipoPropriedade;
                                Propriedade.IDPropriedade = data.IDPropriedade;
                                Propriedade.UtilizadorModificacao = User.Identity.Name;
                                DBViaturas2Propriedades.Update(Propriedade);
                            }
                            else
                            {
                                Viaturas2Propriedades Propriedade = new Viaturas2Propriedades
                                {
                                    Matricula = data.Matricula,
                                    IDTipoPropriedade = data.IDTipoPropriedade,
                                    IDPropriedade = data.IDPropriedade,
                                    DataInicio = (DateTime)data.DataPropriedade,
                                    UtilizadorCriacao = User.Identity.Name
                                };
                                DBViaturas2Propriedades.Create(Propriedade);
                            }
                        }
                    }
                    
                    //VIATURA
                    Viaturas2 viatura = DBViaturas2.ParseToDB(data);
                    viatura.UtilizadorModificacao = User.Identity.Name;
                    if (DBViaturas2.Update(viatura) != null)
                    {
                        data.eReasonCode = 1;
                        data.eMessage = "Viatura atualizada com sucesso.";
                    }
                    else
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "Ocorreu um erro na atualização da viatura.";
                    }
                }

                return Json(data);
            }
            else
            {
                data.eReasonCode = 3;
                data.eMessage = "Erro na obtensão dos dados.";
            }

            return Json(data);
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
        public JsonResult DeleteViatura2([FromBody] Viaturas2ViewModel data)
        {
            if (data != null && !string.IsNullOrEmpty(data.Matricula))
            {
                List<FolhasDeHoras> AllFolhasHoras = DBFolhasDeHoras.GetAll();
                List<PréRequisição> AllPreReq = DBPreRequesition.GetAll();
                List<LinhasPréRequisição> AllPreReqLines = DBPreRequesitionLines.GetAll();
                List<Requisição> AllReq = DBRequest.GetAll();
                List<LinhasRequisição> AllReqLines = DBRequestLine.GetAll();

                //FOLHAS DE HORAS
                int CountFH = 0;
                CountFH = AllFolhasHoras.Where(x => x.Matrícula == data.Matricula).Count();
                if (CountFH > 0)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Não é possível eliminar a Viatura por existirem Folhas de Horas associadas a esta Viatura " + data.Matricula;
                    return Json(data);
                }

                //PRÉ-REQUISIÇÕES
                int CountPreRQ = 0;
                CountPreRQ = AllPreReq.Where(x => x.Viatura == data.Matricula).Count();
                if (CountPreRQ > 0)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Não é possível eliminar a Viatura por existirem Pré-Requisições associadas a esta Viatura " + data.Matricula;
                    return Json(data);
                }
                int CountPreRQLines = 0;
                CountPreRQLines = AllPreReqLines.Where(x => x.Viatura == data.Matricula).Count();
                if (CountPreRQLines > 0)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Não é possível eliminar a Viatura por existirem Linhas de Pré-Requisições associadas a esta Viatura " + data.Matricula;
                    return Json(data);
                }

                //REQUISIÇÕES
                int CountRQ = 0;
                CountRQ = AllReq.Where(x => x.Viatura == data.Matricula).Count();
                if (CountRQ > 0)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Não é possível eliminar a Viatura por existirem Requisições associadas a esta Viatura " + data.Matricula;
                    return Json(data);
                }
                int CountRQLines = 0;
                CountRQLines = AllReqLines.Where(x => x.Viatura == data.Matricula).Count();
                if (CountRQLines > 0)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Não é possível eliminar a Viatura por existirem Linhas de Requisições associadas a esta Viatura " + data.Matricula;
                    return Json(data);
                }

                //PROJETOS
                if (!string.IsNullOrEmpty(data.NoProjeto))
                {
                    //PRÉ-REQUISIÇÕES
                    int CountPreRQProj = 0;
                    CountPreRQProj = AllPreReq.Where(x => x.NºProjeto == data.NoProjeto).Count();
                    if (CountPreRQProj > 0)
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "Não é possível eliminar a Viatura por existirem Pré-Requisições associadas a este Projeto " + data.NoProjeto;
                        return Json(data);
                    }
                    int CountPreRQLinesProj = 0;
                    CountPreRQLinesProj = AllPreReqLines.Where(x => x.NºProjeto == data.NoProjeto).Count();
                    if (CountPreRQLinesProj > 0)
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "Não é possível eliminar a Viatura por existirem Linhas de Pré-Requisições associadas a este Projeto " + data.NoProjeto;
                        return Json(data);
                    }

                    //REQUISIÇÕES
                    int CountRQProj = 0;
                    CountRQProj = AllReq.Where(x => x.NºProjeto == data.NoProjeto).Count();
                    if (CountRQProj > 0)
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "Não é possível eliminar a Viatura por existirem Requisições associados a este Projeto " + data.NoProjeto;
                        return Json(data);
                    }
                    int CountRQLinesProj = 0;
                    CountRQLinesProj = AllReqLines.Where(x => x.NºProjeto == data.NoProjeto).Count();
                    if (CountRQLinesProj > 0)
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "Não é possível eliminar a Viatura por existirem Linhas de Requisições associados a este Projeto " + data.NoProjeto;
                        return Json(data);
                    }
                }

                if (!string.IsNullOrEmpty(data.NoProjeto))
                {
                    int CountNAV2017 = 0;
                    List<NAVProjectsViewModel> AllProjectsNAV2017 = DBNAV2017Projects.GetAllInDB(_config.NAVDatabaseName, _config.NAVCompanyName, data.NoProjeto);
                    if (AllProjectsNAV2017 != null)
                        CountNAV2017 = AllProjectsNAV2017.Count;

                    if (CountNAV2017 > 0)
                    {
                        //Read NAV2017 Project Key
                        Task<WSCreateNAVProject.Read_Result> TReadNavProj = WSProject.GetNavProject(data.NoProjeto, _configws);
                        try
                        {
                            TReadNavProj.Wait();
                        }
                        catch (Exception ex)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Erro ao eliminar projeto: Não foi possivel obter o Projeto a partir do NAV2017.";
                            return Json(data);
                        }

                        if (TReadNavProj.IsCompletedSuccessfully)
                        {
                            if (TReadNavProj.Result.WSJob == null)
                            {
                                data.eReasonCode = 3;
                                data.eMessage = "Erro ao eliminar projeto: O projeto não existe no NAV2017";
                                return Json(data);
                            }
                            else
                            {
                                //Delete Project on NAV2017
                                Task<WSCreateNAVProject.Delete_Result> TDeleteNavProj = WSProject.DeleteNavProject(TReadNavProj.Result.WSJob.Key, _configws);
                                try
                                {
                                    TDeleteNavProj.Wait();
                                }
                                catch (Exception ex)
                                {
                                    data.eReasonCode = 3;
                                    data.eMessage = ex.InnerException.Message;
                                    return Json(data);
                                }

                                if (!TDeleteNavProj.IsCompletedSuccessfully)
                                {
                                    data.eReasonCode = 3;
                                    data.eMessage = "Erro ao eliminar projeto: Não foi possivel eliminar o projeto no NAV2017";
                                    return Json(data);
                                }
                            }
                        }
                    }

                    //Delete Project on NAV2009
                    int CountNAV2009 = 0;
                    List<NAVProjectsViewModel> AllProjectsNAV2009 = DBNAV2009Projects.GetAll(_config.NAV2009ServerName, _config.NAV2009DatabaseName, _config.NAV2009CompanyName, data.NoProjeto);
                    if (AllProjectsNAV2009 != null)
                        CountNAV2009 = AllProjectsNAV2009.Count;

                    if (CountNAV2009 > 0)
                    {
                        int resultNAV2009 = DBNAV2009Projects.Delete(data.NoProjeto);

                        if (resultNAV2009 != 1)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Erro ao eliminar: Não foi possivel eliminar o projeto no NAV2009";
                            return Json(data);
                        }
                    }
                }

                //Delete E-SUCH
                int CounteSUCH = 0;
                Viaturas2 Viatura = DBViaturas2.GetByMatricula(data.Matricula);
                if (Viatura != null)
                    CounteSUCH = 1;

                if (CounteSUCH > 0)
                {
                    try
                    {
                        //Viaturas2_Abastecimentos
                        List<Viaturas2Abastecimentos> All_Abastecimentos = DBViaturas2Abastecimentos.GetByMatricula(data.Matricula);
                        if (All_Abastecimentos != null && All_Abastecimentos.Count > 0)
                            foreach (Viaturas2Abastecimentos item in All_Abastecimentos)
                                DBViaturas2Abastecimentos.Delete(item);

                        //Viaturas2_Abate
                        List<Viaturas2Abate> All_Abate = DBViaturas2Abate.GetByMatricula(data.Matricula);
                        if (All_Abate != null && All_Abate.Count > 0)
                            foreach (Viaturas2Abate item in All_Abate)
                                DBViaturas2Abate.Delete(item);

                        //Viaturas2_Acidentes
                        List<Viaturas2Acidentes> All_Acidentes = DBViaturas2Acidentes.GetByMatricula(data.Matricula);
                        if (All_Acidentes != null && All_Acidentes.Count > 0)
                            foreach (Viaturas2Acidentes item in All_Acidentes)
                                DBViaturas2Acidentes.Delete(item);

                        //Viaturas2_Afetacao
                        List<Viaturas2Afetacao> All_Afetacao = DBViaturas2Afetacao.GetByMatricula(data.Matricula);
                        if (All_Afetacao != null && All_Afetacao.Count > 0)
                            foreach (Viaturas2Afetacao item in All_Afetacao)
                                DBViaturas2Afetacao.Delete(item);

                        //Viaturas2_CartaoCombustivel
                        List<Viaturas2CartaoCombustivel> All_CartaoCombustivel = DBViaturas2CartaoCombustivel.GetByMatricula(data.Matricula);
                        if (All_CartaoCombustivel != null && All_CartaoCombustivel.Count > 0)
                            foreach (Viaturas2CartaoCombustivel item in All_CartaoCombustivel)
                                DBViaturas2CartaoCombustivel.Delete(item);

                        //Viaturas2_CartaVerde
                        List<Viaturas2CartaVerde> All_CartaVerde = DBViaturas2CartaVerde.GetByMatricula(data.Matricula);
                        if (All_CartaVerde != null && All_CartaVerde.Count > 0)
                            foreach (Viaturas2CartaVerde item in All_CartaVerde)
                                DBViaturas2CartaVerde.Delete(item);

                        //Viaturas2_CarTrack
                        List<Viaturas2CarTrack> All_CarTrack = DBViaturas2CarTrack.GetByMatricula(data.Matricula);
                        if (All_CarTrack != null && All_CarTrack.Count > 0)
                            foreach (Viaturas2CarTrack item in All_CarTrack)
                                DBViaturas2CarTrack.Delete(item);

                        //Viaturas2_ContraOrdenacoes
                        List<Viaturas2ContraOrdenacoes> All_ContraOrdenacoes = DBViaturas2ContraOrdenacoes.GetByMatricula(data.Matricula);
                        if (All_ContraOrdenacoes != null && All_ContraOrdenacoes.Count > 0)
                            foreach (Viaturas2ContraOrdenacoes item in All_ContraOrdenacoes)
                                DBViaturas2ContraOrdenacoes.Delete(item);

                        //Viaturas2_Dimensoes
                        List<Viaturas2Dimensoes> All_Dimensoes = DBViaturas2Dimensoes.GetByMatricula(data.Matricula);
                        if (All_Dimensoes != null && All_Dimensoes.Count > 0)
                            foreach (Viaturas2Dimensoes item in All_Dimensoes)
                                DBViaturas2Dimensoes.Delete(item);

                        //Viaturas2_Estados
                        List<Viaturas2Estados> All_Estados = DBViaturas2Estados.GetByMatricula(data.Matricula);
                        if (All_Estados != null && All_Estados.Count > 0)
                            foreach (Viaturas2Estados item in All_Estados)
                                DBViaturas2Estados.Delete(item);

                        //Viaturas2_Gestores
                        List<Viaturas2Gestores> All_Gestores = DBViaturas2Gestores.GetByMatricula(data.Matricula);
                        if (All_Gestores != null && All_Gestores.Count > 0)
                            foreach (Viaturas2Gestores item in All_Gestores)
                                DBViaturas2Gestores.Delete(item);

                        //Viaturas2_Inspecao
                        List<Viaturas2Inspecoes> All_Inspecao = DBViaturas2Inspecoes.GetByMatricula(data.Matricula);
                        if (All_Inspecao != null && All_Inspecao.Count > 0)
                            foreach (Viaturas2Inspecoes item in All_Inspecao)
                                DBViaturas2Inspecoes.Delete(item);

                        //Viaturas2_Km
                        List<Viaturas2Km> All_Km = DBViaturas2Km.GetByMatricula(data.Matricula);
                        if (All_Km != null && All_Km.Count > 0)
                            foreach (Viaturas2Km item in All_Km)
                                DBViaturas2Km.Delete(item);

                        //Viaturas2_Manutencao
                        List<Viaturas2Manutencao> All_Manutencao = DBViaturas2Manutencao.GetByMatricula(data.Matricula);
                        if (All_Manutencao != null && All_Manutencao.Count > 0)
                            foreach (Viaturas2Manutencao item in All_Manutencao)
                                DBViaturas2Manutencao.Delete(item);

                        //Viaturas2_Parqueamento
                        List<Viaturas2Parqueamento> All_Parqueamento = DBViaturas2Parqueamento.GetByMatricula(data.Matricula);
                        if (All_Parqueamento != null && All_Parqueamento.Count > 0)
                            foreach (Viaturas2Parqueamento item in All_Parqueamento)
                                DBViaturas2Parqueamento.Delete(item);

                        //Viaturas2_Propriedades
                        List<Viaturas2Propriedades> All_Propriedades = DBViaturas2Propriedades.GetByMatricula(data.Matricula);
                        if (All_Propriedades != null && All_Propriedades.Count > 0)
                            foreach (Viaturas2Propriedades item in All_Propriedades)
                                DBViaturas2Propriedades.Delete(item);

                        //Viaturas2_ViaVerde
                        List<Viaturas2ViaVerde> All_ViaVerde = DBViaturas2ViaVerde.GetByMatricula(data.Matricula);
                        if (All_ViaVerde != null && All_ViaVerde.Count > 0)
                            foreach (Viaturas2ViaVerde item in All_ViaVerde)
                                DBViaturas2ViaVerde.Delete(item);

                        //Viaturas2
                        Viaturas2 viatura = DBViaturas2.ParseToDB(data);
                        if (DBViaturas2.Delete(viatura) == true)
                        {
                            data.eReasonCode = 1;
                            data.eMessage = "Viatura eliminada com sucesso.";
                            return Json(data);
                        }
                        else
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Ocorreu um erro na eliminação da viatura no e-SUCH.";
                            return Json(data);
                        }
                    }
                    catch (Exception ex)
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "Ocorreu um erro na eliminaçãonas nas tabelas auxiliares da viatura no e-SUCH.";
                        return Json(data);
                    }
                }

                return Json(data);
            }
            else
            {
                data.eReasonCode = 3;
                data.eMessage = "Erro na obtensão dos dados.";
            }

            return Json(data);
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
                if (dp["gestor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Responsável"); Col = Col + 1; }
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
                        if (dp["gestor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Gestor); Col = Col + 1; }
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

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_MovimentosViaturas2([FromBody] List<NAVProjectsMovementsViaturasViewModel> Lista)
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
                ISheet excelSheet = workbook.CreateSheet("Viaturas Movimentos");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["data"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data"); Col = Col + 1; }
                if (dp["tipo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Tipo"); Col = Col + 1; }
                if (dp["codigo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código"); Col = Col + 1; }
                if (dp["descricao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descrição"); Col = Col + 1; }
                if (dp["quantidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Quantidade"); Col = Col + 1; }
                if (dp["codigoUnidadeMedida"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Unidade Medida"); Col = Col + 1; }
                if (dp["custoUnitario"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Custo Unitário"); Col = Col + 1; }
                if (dp["custoTotal"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Custo Total"); Col = Col + 1; }
                if (dp["regiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Região"); Col = Col + 1; }
                if (dp["area"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Área Funcional"); Col = Col + 1; }
                if (dp["cresp"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Centro Responsabilidade"); Col = Col + 1; }
                if (dp["documentoNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Documento"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (NAVProjectsMovementsViaturasViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["data"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Data); Col = Col + 1; }
                        if (dp["tipo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Tipo); Col = Col + 1; }
                        if (dp["codigo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Codigo); Col = Col + 1; }
                        if (dp["descricao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Descricao); Col = Col + 1; }
                        if (dp["quantidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Quantidade.ToString()); Col = Col + 1; }
                        if (dp["codigoUnidadeMedida"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodigoUnidadeMedida); Col = Col + 1; }
                        if (dp["custoUnitario"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CustoUnitario.ToString()); Col = Col + 1; }
                        if (dp["custoTotal"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CustoTotal.ToString()); Col = Col + 1; }
                        if (dp["regiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Regiao); Col = Col + 1; }
                        if (dp["area"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Area); Col = Col + 1; }
                        if (dp["cresp"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Cresp); Col = Col + 1; }
                        if (dp["documentoNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DocumentoNo); Col = Col + 1; }
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

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_CustosViaturas2([FromBody] List<NAVProjectsMovementsViaturasViewModel> Lista)
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
                ISheet excelSheet = workbook.CreateSheet("Viaturas Movimentos");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["data"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data"); Col = Col + 1; }
                if (dp["documentoNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Documento"); Col = Col + 1; }
                if (dp["codigo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código"); Col = Col + 1; }
                if (dp["descricao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descrição"); Col = Col + 1; }
                if (dp["custoTotal"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Custo Total"); Col = Col + 1; }
                if (dp["regiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Região"); Col = Col + 1; }
                if (dp["area"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Área Funcional"); Col = Col + 1; }
                if (dp["cresp"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Centro Responsabilidade"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (NAVProjectsMovementsViaturasViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["data"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Data); Col = Col + 1; }
                        if (dp["documentoNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DocumentoNo); Col = Col + 1; }
                        if (dp["codigo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Codigo); Col = Col + 1; }
                        if (dp["descricao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Descricao); Col = Col + 1; }
                        if (dp["custoTotal"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CustoTotal.ToString()); Col = Col + 1; }
                        if (dp["regiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Regiao); Col = Col + 1; }
                        if (dp["area"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Area); Col = Col + 1; }
                        if (dp["cresp"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Cresp); Col = Col + 1; }
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
    }
}
