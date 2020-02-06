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
                int IDGestor = 0;
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

                IDGestor = DBViaturas2Gestores.GetByMatriculaGestorRecent(data.Matricula, DateTime.Now, 1) != null ? (int)DBViaturas2Gestores.GetByMatriculaGestorRecent(data.Matricula, DateTime.Now, 1).IDGestor : 0;
                if (IDGestor > 0) viatura.Gestor = DBViaturas2GestoresGestor.GetByID(IDGestor) != null ? DBViaturas2GestoresGestor.GetByID(IDGestor).Gestor : "";
                IDCondutor = DBViaturas2Gestores.GetByMatriculaGestorRecent(data.Matricula, DateTime.Now, 2) != null ? (int)DBViaturas2Gestores.GetByMatriculaGestorRecent(data.Matricula, DateTime.Now, 2).IDGestor : 0;
                if (IDCondutor > 0) viatura.Condutor = DBViaturas2GestoresGestor.GetByID(IDCondutor) != null ? DBViaturas2GestoresGestor.GetByID(IDCondutor).Gestor : "";

                viatura.DataProximaInspecaoTexto = DBViaturas2Inspecoes.GetByMatriculaProximaInspecaoRecent(data.Matricula) != null ? DBViaturas2Inspecoes.GetByMatriculaProximaInspecaoRecent(data.Matricula).ProximaInspecao.Value.ToString("yyyy-MM-dd") : "";

                List <ConfiguracaoTabelas> AllConfTabelas = DBConfiguracaoTabelas.GetAll();
                List<Viaturas2Marcas> AllMarcas = DBViaturas2Marcas.GetAll();
                List<Viaturas2Modelos> AllModelos = DBViaturas2Modelos.GetAll();
                List<NAVProjectsViewModel> AllProjects = DBNAV2017Projects.GetAllInDB(_config.NAVDatabaseName, _config.NAVCompanyName, "");
                List<Viaturas2Parqueamento> AllParquamentos = DBViaturas2Parqueamento.GetAll();
                List<Viaturas2ParqueamentoLocal> AllPArqueamentosLocais = DBViaturas2ParqueamentoLocal.GetAll();

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

                if (viatura.Data1Matricula.HasValue) viatura.Idade = (DateTime.Now.Year - Convert.ToDateTime(viatura.Data1Matricula).Year).ToString() + " ano(s)";
            }
            return Json(viatura);
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
        public JsonResult GetViaturas2TabCartaVerde([FromBody] Viaturas2InspecoesViewModel viatura)
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
        public JsonResult DeleteViaturas2Inspecao([FromBody] Viaturas2InspecoesViewModel inspecao)
        {
            try
            {
                if (inspecao != null && !string.IsNullOrEmpty(inspecao.Matricula))
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
                    data.CodAreaFuncionalOriginalDB != data.CodCentroResponsabilidade || data.CodCentroResponsabilidadeOriginalDB != data.CodCentroResponsabilidade))
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
