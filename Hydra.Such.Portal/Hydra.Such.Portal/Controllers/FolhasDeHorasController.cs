using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel.FH;
using Hydra.Such.Data.Logic;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.FolhaDeHora;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.NAV;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.Logic.Project;
using System.Data.SqlClient;
using Hydra.Such.Data;
using Hydra.Such.Data.Logic.Approvals;
using Hydra.Such.Portal.Extensions;
using Hydra.Such.Data.ViewModel.Approvals;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Newtonsoft.Json.Linq;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class FolhasDeHorasController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly GeneralConfigurations _generalConfig;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FolhasDeHorasController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IOptions<GeneralConfigurations> appSettingsGeneral, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            _generalConfig = appSettingsGeneral.Value;
            this._hostingEnvironment = _hostingEnvironment;
        }

        #region Home
        public IActionResult Index()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.FolhasHoras); //1, 6);
            UserAccessesViewModel UPermReportAjCustosRH = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.FHReportAjCustoRH); //65);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.UPermissionsRH = UPermReportAjCustosRH;
                ViewBag.reportServerURL = _config.ReportServerURL;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult FolhaDeHoras_Validacao(string folhaDeHoraNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.FolhasHoras);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.FolhaDeHorasNo = folhaDeHoraNo ?? "";
                ViewBag.UPermissions = UPerm;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult FolhaDeHoras_IntegracaoKMS(string folhaDeHoraNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.FolhasHoras);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.FolhaDeHorasNo = folhaDeHoraNo ?? "";
                ViewBag.UPermissions = UPerm;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult FolhaDeHoras_Historico(string folhaDeHoraNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.FolhasHoras);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.FolhaDeHorasNo = folhaDeHoraNo ?? "";
                ViewBag.UPermissions = UPerm;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult FolhaDeHoras_IntegracaoAjudaCusto(string folhaDeHoraNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.FolhasHoras);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.FolhaDeHorasNo = folhaDeHoraNo ?? "";
                ViewBag.UPermissions = UPerm;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult FolhaDeHoras_Pendentes()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.FolhasHoras);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.FolhaDeHorasNo = "";
                ViewBag.UPermissions = UPerm;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        //Listagem das Folhas de Horas consoante o estado
        public JsonResult GetListFolhasDeHoras([FromBody] HTML_FHViewModel HTML)
        {
            try
            {
                UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.FolhasHoras); //1, 6);

                List<TabelaConfRecursosFh>  AllRecursos = DBTabelaConfRecursosFh.GetAll();

                if (UPerm != null && UPerm.Read.Value)
                {
                    if (HTML.todas == 1)
                    {
                        List<FolhaDeHorasViewModel> result = DBFolhasDeHoras.GetAllByTodas(_config.NAVDatabaseName, _config.NAVCompanyName, User.Identity.Name);
                        if (result != null)
                        {
                            result.ForEach(FH =>
                            {
                                FH.TipoDeslocacaoTexto = FH.TipoDeslocacao == null ? "" : EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == FH.TipoDeslocacao).FirstOrDefault().Value;
                                FH.CodigoTipoKms = FH.CodigoTipoKms == null ? "" : AllRecursos.Where(x => x.Tipo == "1" && x.CodRecurso == FH.CodigoTipoKms).FirstOrDefault().Descricao;
                                FH.DeslocacaoForaConcelho = FH.DeslocacaoForaConcelho == null ? false : FH.DeslocacaoForaConcelho;
                                FH.DeslocacaoForaConcelhoTexto = FH.DeslocacaoForaConcelho == null ? "" : FH.DeslocacaoForaConcelho == false ? "Não" : "Sim";
                                FH.Terminada = FH.Terminada == null ? false : FH.Terminada;
                                FH.TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada == false ? "Não" : "Sim";
                            });
                        }

                        return Json(result.OrderByDescending(x => x.FolhaDeHorasNo));
                    }
                    else
                    {
                        if (HTML.pendentes == 1)
                        {
                            List<FolhaDeHorasViewModel> result = DBFolhasDeHoras.GetAllByPendentes(_config.NAVDatabaseName, _config.NAVCompanyName, User.Identity.Name);
                            if (result != null)
                            {
                                result.ForEach(FH =>
                                {
                                    if (string.IsNullOrEmpty(FH.Validadores) || string.IsNullOrEmpty(FH.IntegradoresEmRH) || string.IsNullOrEmpty(FH.IntegradoresEmRHKM))
                                        FH.Estadotexto = "Faltam Validadores";
                                    else
                                        FH.Estadotexto = "Não está Terminada";

                                    FH.TipoDeslocacaoTexto = FH.TipoDeslocacao == null ? "" : EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == FH.TipoDeslocacao).FirstOrDefault().Value;
                                    FH.CodigoTipoKms = FH.CodigoTipoKms == null ? "" : AllRecursos.Where(x => x.Tipo == "1" && x.CodRecurso == FH.CodigoTipoKms).FirstOrDefault().Descricao;
                                    FH.DeslocacaoForaConcelho = FH.DeslocacaoForaConcelho == null ? false : FH.DeslocacaoForaConcelho;
                                    FH.DeslocacaoForaConcelhoTexto = FH.DeslocacaoForaConcelho == null ? "" : FH.DeslocacaoForaConcelho == false ? "Não" : "Sim";
                                    FH.Terminada = FH.Terminada == null ? false : FH.Terminada;
                                    FH.TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada == false ? "Não" : "Sim";
                                });
                            }

                            return Json(result.OrderByDescending(x => x.FolhaDeHorasNo));
                        }
                        else
                        {
                            if (HTML.validacao == 1)
                            {
                                List<FolhaDeHorasViewModel> result = DBFolhasDeHoras.GetAllByValidacao(_config.NAVDatabaseName, _config.NAVCompanyName, User.Identity.Name);
                                if (result != null)
                                {
                                    result.ForEach(FH =>
                                    {
                                        FH.TipoDeslocacaoTexto = FH.TipoDeslocacao == null ? "" : EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == FH.TipoDeslocacao).FirstOrDefault().Value;
                                        FH.CodigoTipoKms = FH.CodigoTipoKms == null ? "" : AllRecursos.Where(x => x.Tipo == "1" && x.CodRecurso == FH.CodigoTipoKms).FirstOrDefault().Descricao;
                                        FH.DeslocacaoForaConcelho = FH.DeslocacaoForaConcelho == null ? false : FH.DeslocacaoForaConcelho;
                                        FH.DeslocacaoForaConcelhoTexto = FH.DeslocacaoForaConcelho == null ? "" : FH.DeslocacaoForaConcelho == false ? "Não" : "Sim";
                                        FH.Terminada = FH.Terminada == null ? false : FH.Terminada;
                                        FH.TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada == false ? "Não" : "Sim";
                                    });
                                }

                                return Json(result.OrderByDescending(x => x.FolhaDeHorasNo));
                            }
                            else
                            {
                                if (HTML.integracaoajuda == 1)
                                {
                                    List<FolhaDeHorasViewModel> result = DBFolhasDeHoras.GetAllByIntegracaoAjuda(_config.NAVDatabaseName, _config.NAVCompanyName, User.Identity.Name);
                                    if (result != null)
                                    {
                                        result.ForEach(FH =>
                                        {
                                            FH.TipoDeslocacaoTexto = FH.TipoDeslocacao == null ? "" : EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == FH.TipoDeslocacao).FirstOrDefault().Value;
                                            FH.CodigoTipoKms = FH.CodigoTipoKms == null ? "" : AllRecursos.Where(x => x.Tipo == "1" && x.CodRecurso == FH.CodigoTipoKms).FirstOrDefault().Descricao;
                                            FH.DeslocacaoForaConcelho = FH.DeslocacaoForaConcelho == null ? false : FH.DeslocacaoForaConcelho;
                                            FH.DeslocacaoForaConcelhoTexto = FH.DeslocacaoForaConcelho == null ? "" : FH.DeslocacaoForaConcelho == false ? "Não" : "Sim";
                                            FH.Terminada = FH.Terminada == null ? false : FH.Terminada;
                                            FH.TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada == false ? "Não" : "Sim";
                                        });
                                    }

                                    return Json(result.OrderByDescending(x => x.FolhaDeHorasNo));
                                }
                                else
                                {
                                    if (HTML.integracaokms == 1)
                                    {
                                        List<FolhaDeHorasViewModel> result = DBFolhasDeHoras.GetAllByIntegracaoKMS(_config.NAVDatabaseName, _config.NAVCompanyName, User.Identity.Name);
                                        if (result != null)
                                        {
                                            result.ForEach(FH =>
                                            {
                                                FH.TipoDeslocacaoTexto = FH.TipoDeslocacao == null ? "" : EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == FH.TipoDeslocacao).FirstOrDefault().Value;
                                                FH.CodigoTipoKms = FH.CodigoTipoKms == null ? "" : AllRecursos.Where(x => x.Tipo == "1" && x.CodRecurso == FH.CodigoTipoKms).FirstOrDefault().Descricao;
                                                FH.DeslocacaoForaConcelho = FH.DeslocacaoForaConcelho == null ? false : FH.DeslocacaoForaConcelho;
                                                FH.DeslocacaoForaConcelhoTexto = FH.DeslocacaoForaConcelho == null ? "" : FH.DeslocacaoForaConcelho == false ? "Não" : "Sim";
                                                FH.Terminada = FH.Terminada == null ? false : FH.Terminada;
                                                FH.TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada == false ? "Não" : "Sim";
                                            });
                                        }

                                        return Json(result.OrderByDescending(x => x.FolhaDeHorasNo));
                                    }
                                    else
                                    {
                                        if (HTML.historico == 1)
                                        {
                                            List<FolhaDeHorasViewModel> result = DBFolhasDeHoras.GetAllByHistorico(_config.NAVDatabaseName, _config.NAVCompanyName, User.Identity.Name);
                                            if (result != null)
                                            {
                                                result.ForEach(FH =>
                                                {
                                                    FH.TipoDeslocacaoTexto = FH.TipoDeslocacao == null ? "" : EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == FH.TipoDeslocacao).FirstOrDefault().Value;
                                                    FH.CodigoTipoKms = FH.CodigoTipoKms == null ? "" : AllRecursos.Where(x => x.Tipo == "1" && x.CodRecurso == FH.CodigoTipoKms).FirstOrDefault().Descricao;
                                                    FH.DeslocacaoForaConcelho = FH.DeslocacaoForaConcelho == null ? false : FH.DeslocacaoForaConcelho;
                                                    FH.DeslocacaoForaConcelhoTexto = FH.DeslocacaoForaConcelho == null ? "" : FH.DeslocacaoForaConcelho == false ? "Não" : "Sim";
                                                    FH.Terminada = FH.Terminada == null ? false : FH.Terminada;
                                                    FH.TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada == false ? "Não" : "Sim";
                                                });
                                            }

                                            return Json(result.OrderByDescending(x => x.FolhaDeHorasNo));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(null);
            }

            return Json(null);
        }
        #endregion

        #region Details
        //Criação da Folha de Horas
        public ActionResult Detalhes([FromQuery] string FHNo, [FromQuery] int area)
        {
            int Estado = 0;
            ConfigUtilizadores Utilizador = DBUserConfigurations.GetById(User.Identity.Name);
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.FolhasHoras); //1, 6);

            ViewBag.reportServerURL = _config.ReportServerURL;
            ViewBag.userLogin = User.Identity.Name.ToString();

            //if (Utilizador.Administrador == true)
            //    ViewBag.mostrarDistribuicaoPercursos = true;
            //else
            //    ViewBag.mostrarDistribuicaoPercursos = false;

            //ViewBag.mostrarDistribuicaoPercursos = "NAO";

            if (UPerm != null && UPerm.Read.Value)
            {
                if (FHNo == null || FHNo == "")
                {
                    string id = "";

                    //Get Folha de Horas Numeration
                    Configuração Configs = DBConfigurations.GetById(1);
                    int FolhaDeHorasNumerationConfigurationId = Configs.NumeraçãoFolhasDeHoras.Value;
                    id = DBNumerationConfigurations.GetNextNumeration(FolhaDeHorasNumerationConfigurationId, true, false);

                    //Update Last Numeration Used
                    ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(FolhaDeHorasNumerationConfigurationId);
                    ConfigNumerations.ÚltimoNºUsado = id;
                    DBNumerationConfigurations.Update(ConfigNumerations);

                    FolhasDeHoras FH = new FolhasDeHoras();

                    FH.NºFolhaDeHoras = id;
                    FH.Área = 1;
                    FH.NºProjeto = "";
                    FH.ProjetoDescricao = "";
                    FH.NºEmpregado = DBUserConfigurations.GetById(User.Identity.Name).EmployeeNo == null ? "" : DBUserConfigurations.GetById(User.Identity.Name).EmployeeNo;
                    FH.NomeEmpregado = DBUserConfigurations.GetById(User.Identity.Name).EmployeeNo == null ? "" : DBUserConfigurations.GetById(User.Identity.Name).Nome;
                    FH.DataHoraPartida = null;
                    FH.DataHoraChegada = null;
                    FH.TipoDeslocação = 0;
                    FH.CódigoTipoKmS = "AJC0003";
                    FH.Matrícula = "";
                    FH.DeslocaçãoForaConcelho = false;
                    FH.DeslocaçãoPlaneada = false;
                    FH.Terminada = false;
                    FH.Estado = 0;
                    FH.CriadoPor = User.Identity.Name;
                    FH.DataHoraCriação = DateTime.Now;
                    FH.CódigoRegião = DBUserConfigurations.GetById(User.Identity.Name).RegiãoPorDefeito == null ? "" : DBUserConfigurations.GetById(User.Identity.Name).RegiãoPorDefeito;
                    FH.CódigoÁreaFuncional = DBUserConfigurations.GetById(User.Identity.Name).AreaPorDefeito == null ? "" : DBUserConfigurations.GetById(User.Identity.Name).AreaPorDefeito;
                    FH.CódigoCentroResponsabilidade = DBUserConfigurations.GetById(User.Identity.Name).CentroRespPorDefeito == null ? "" : DBUserConfigurations.GetById(User.Identity.Name).CentroRespPorDefeito;
                    FH.TerminadoPor = "";
                    FH.DataHoraTerminado = null;
                    FH.Validado = false;
                    FH.Validadores = "";
                    FH.Validador = "";
                    FH.DataHoraValidação = null;
                    FH.IntegradoEmRh = false;
                    FH.IntegradoresEmRh = "";
                    FH.IntegradorEmRh = "";
                    FH.DataIntegraçãoEmRh = null;
                    FH.IntegradoEmRhkm = false;
                    FH.IntegradoresEmRhkm = "";
                    FH.IntegradorEmRhKm = "";
                    FH.DataIntegraçãoEmRhKm = null;
                    FH.CustoTotalAjudaCusto = 0;
                    FH.CustoTotalHoras = 0;
                    FH.CustoTotalKm = 0;
                    FH.NumTotalKm = 0;
                    FH.Observações = "";
                    FH.NºResponsável1 = "";
                    FH.NºResponsável2 = "";
                    FH.NºResponsável3 = "";
                    FH.ValidadoresRhKm = "";
                    FH.DataHoraÚltimoEstado = null;
                    FH.UtilizadorModificação = "";
                    FH.DataHoraModificação = null;
                    FH.Eliminada = false;
                    FH.Intervenientes = " CRIADOPOR: " + FH.CriadoPor + " EMPREGADO: " + FH.NºEmpregado + " VALIDADORES: " + FH.Validadores + " INTEGRADORESEMRH: " + FH.IntegradoresEmRh + " INTEGRADORESEMRHKM: " + FH.IntegradoresEmRhkm;

                    DBFolhasDeHoras.Create(FH);

                    FH.Intervenientes = " CRIADOPOR: " + FH.CriadoPor;
                    ConfigUtilizadores ConfigUser = DBUserConfigurations.GetByEmployeeNo(FH.NºEmpregado);
                    if (ConfigUser != null)
                        FH.Intervenientes = FH.Intervenientes + " EMPREGADO: " + ConfigUser.IdUtilizador;

                    FolhaDeHorasViewModel Autorizacao = DBFolhasDeHoras.GetListaValidadoresIntegradores(id, User.Identity.Name, _config.NAV2009DatabaseName, _config.NAV2009CompanyName);
                    if (Autorizacao != null)
                    {
                        FH.Validadores = !string.IsNullOrEmpty(Autorizacao.Validadores) ? Autorizacao.Validadores : "";
                        FH.IntegradoresEmRh = !string.IsNullOrEmpty(Autorizacao.IntegradoresEmRH) ? Autorizacao.IntegradoresEmRH : "";
                        FH.IntegradoresEmRhkm = !string.IsNullOrEmpty(Autorizacao.IntegradoresEmRHKM) ? Autorizacao.IntegradoresEmRHKM : "";

                        FH.NºResponsável1 = !string.IsNullOrEmpty(Autorizacao.Responsavel1No) ? Autorizacao.Responsavel1No : "";
                        FH.NºResponsável2 = !string.IsNullOrEmpty(Autorizacao.Responsavel2No) ? Autorizacao.Responsavel2No : "";
                        FH.NºResponsável3 = !string.IsNullOrEmpty(Autorizacao.Responsavel3No) ? Autorizacao.Responsavel3No : "";

                        FH.Intervenientes = !string.IsNullOrEmpty(Autorizacao.Intervenientes) ? Autorizacao.Intervenientes : "";
                    };

                    FH.UtilizadorModificação = User.Identity.Name;
                    FH.DataHoraModificação = DateTime.Now;

                    DBFolhasDeHoras.Update(FH);

                    FHNo = FH.NºFolhaDeHoras;
                }
                else
                {
                    FolhasDeHoras Folha = DBFolhasDeHoras.GetById(FHNo);
                    ConfigUtilizadores CriadorFH = DBUserConfigurations.GetById(Folha.CriadoPor);
                    string SuperiorHierarquico = CriadorFH.SuperiorHierarquico == null ? "" : CriadorFH.SuperiorHierarquico;

                    if (Folha.Terminada == true)
                    {
                        if (User.Identity.Name.ToLower() != Folha.CriadoPor.ToLower() &&
                            User.Identity.Name.ToLower() != SuperiorHierarquico.ToLower() &&
                            !Folha.Validadores.ToLower().Contains(User.Identity.Name.ToLower()) &&
                            !Folha.IntegradoresEmRh.ToLower().Contains(User.Identity.Name.ToLower()) &&
                            !Folha.IntegradoresEmRhkm.ToLower().Contains(User.Identity.Name.ToLower()))
                        {
                            UPerm.Create = false;
                            UPerm.Delete = false;
                            UPerm.Update = false;
                        }
                    }

                    Estado = (int)Folha.Estado;
                }

                ViewBag.UPermissions = UPerm;
                ViewBag.FolhaDeHorasNo = FHNo == null ? "" : FHNo;
                ViewBag.Estado = Estado;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        //Carrega todos os dados de uma Folha de Horas
        public JsonResult GetFolhaDeHoraDetails([FromBody] FolhaDeHorasViewModel data)
        {
            try
            {
                if (data != null)
                {
                    FolhasDeHoras FH = DBFolhasDeHoras.GetById(data.FolhaDeHorasNo);

                    if (FH != null)
                    {
                        FolhaDeHorasViewModel result = new FolhaDeHorasViewModel()
                        {
                            FolhaDeHorasNo = FH.NºFolhaDeHoras,
                            Area = FH.Área,
                            AreaTexto = FH.Área == null ? "" : FH.Área.ToString(),
                            ProjetoNo = FH.NºProjeto == null ? "" : FH.NºProjeto.ToString(),
                            ProjetoDescricao = FH.ProjetoDescricao,
                            EmpregadoNo = FH.NºEmpregado,
                            EmpregadoNome = FH.NomeEmpregado,
                            DataHoraPartida = FH.DataHoraPartida,
                            DataPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("yyyy-MM-dd"),
                            HoraPartidaTexto = FH.DataHoraPartida == null ? "00:00" : FH.DataHoraPartida.Value.ToString("HH:mm"),
                            DataHoraChegada = FH.DataHoraChegada,
                            DataChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("yyyy-MM-dd"),
                            HoraChegadaTexto = FH.DataHoraChegada == null ? "00:00" : FH.DataHoraChegada.Value.ToString("HH:mm"),
                            TipoDeslocacao = FH.TipoDeslocação,
                            TipoDeslocacaoTexto = FH.TipoDeslocação == null ? "" : FH.TipoDeslocação == null ? "" : FH.TipoDeslocação.ToString(),
                            CodigoTipoKms = FH.CódigoTipoKmS,
                            Matricula = FH.Matrícula,
                            DeslocacaoForaConcelho = FH.DeslocaçãoForaConcelho,
                            DeslocacaoForaConcelhoTexto = FH.DeslocaçãoForaConcelho == null ? "" : FH.DeslocaçãoForaConcelho.ToString(),
                            DeslocacaoPlaneada = FH.DeslocaçãoPlaneada,
                            DeslocacaoPlaneadaTexto = FH.DeslocaçãoPlaneada == null ? "" : FH.DeslocaçãoPlaneada.ToString(),
                            Terminada = FH.Terminada,
                            TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada.ToString(),
                            Estado = FH.Estado,
                            Estadotexto = FH.Estado == null ? "" : FH.Estado.ToString(),
                            CriadoPor = FH.CriadoPor,
                            DataHoraCriacao = FH.DataHoraCriação,
                            DataCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("yyyy-MM-dd"),
                            HoraCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("HH:mm"),
                            CodigoRegiao = FH.CódigoRegião,
                            CodigoAreaFuncional = FH.CódigoÁreaFuncional,
                            CodigoCentroResponsabilidade = FH.CódigoCentroResponsabilidade,
                            TerminadoPor = FH.TerminadoPor,
                            DataHoraTerminado = FH.DataHoraTerminado,
                            DataTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("yyyy-MM-dd"),
                            HoraTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("HH:mm"),

                            Validado = FH.Validado,
                            ValidadoTexto = FH.Validado == null ? "" : FH.Validado.ToString(),
                            Validadores = FH.Validadores == null ? "" : FH.Validadores,
                            Validador = FH.Validador,
                            DataHoraValidacao = FH.DataHoraValidação,
                            DataValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("yyyy-MM-dd"),
                            HoraValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("HH:mm"),

                            IntegradoEmRh = FH.IntegradoEmRh,
                            IntegradoEmRhTexto = FH.IntegradoEmRh == null ? "" : FH.IntegradoEmRh.ToString(),
                            IntegradoresEmRH = FH.IntegradoresEmRh == null ? "" : FH.IntegradoresEmRh,
                            IntegradorEmRH = FH.IntegradorEmRh,
                            DataIntegracaoEmRH = FH.DataIntegraçãoEmRh,
                            DataIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("yyyy-MM-dd"),
                            HoraIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("HH:mm"),

                            IntegradoEmRhKm = FH.IntegradoEmRhkm,
                            IntegradoEmRhKmTexto = FH.IntegradoEmRhkm == null ? "" : FH.IntegradoEmRhkm.ToString(),
                            IntegradoresEmRHKM = FH.IntegradoresEmRhkm == null ? "" : FH.IntegradoresEmRhkm,
                            IntegradorEmRHKM = FH.IntegradorEmRhKm,
                            DataIntegracaoEmRHKM = FH.DataIntegraçãoEmRhKm,
                            DataIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("yyyy-MM-dd"),
                            HoraIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("HH:mm"),

                            CustoTotalAjudaCusto = Convert.ToDecimal(FH.CustoTotalAjudaCusto),
                            CustoTotalHoras = Convert.ToDecimal(FH.CustoTotalHoras),
                            CustoTotalKM = Convert.ToDecimal(FH.CustoTotalKm),
                            NumTotalKM = Convert.ToDecimal(FH.NumTotalKm),
                            Observacoes = FH.Observações,
                            Responsavel1No = FH.NºResponsável1,
                            Responsavel2No = FH.NºResponsável2,
                            Responsavel3No = FH.NºResponsável3,
                            ValidadoresRHKM = FH.ValidadoresRhKm,
                            DataHoraUltimoEstado = FH.DataHoraÚltimoEstado,
                            DataUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("yyyy-MM-dd"),
                            HoraUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("HH:mm"),
                            UtilizadorModificacao = FH.UtilizadorModificação,
                            DataHoraModificacao = FH.DataHoraModificação,
                            DataModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("yyyy-MM-dd"),
                            HoraModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("HH:mm"),
                            Eliminada = FH.Eliminada,
                            Intervenientes = FH.Intervenientes
                        };
                        if (result.Estado == 0 && result.Terminada == true)
                        {
                            if (result.Validadores.ToLower().Contains(User.Identity.Name.ToLower()) || result.CriadoPor.ToLower().Contains(User.Identity.Name.ToLower()))
                                result.MostrarBotoes = true;
                            else
                                result.MostrarBotoes = false;
                        }

                        //PERCURSO
                        result.FolhaDeHorasPercurso = DBLinhasFolhaHoras.GetAllByPercursoToList(data.FolhaDeHorasNo).Select(Percurso => new LinhasFolhaHorasViewModel()
                        {
                            NoFolhaHoras = Percurso.NoFolhaHoras,
                            NoLinha = Percurso.NoLinha,
                            TipoCusto = Percurso.TipoCusto,
                            CodTipoCusto = Percurso.CodTipoCusto,
                            DescricaoTipoCusto = Percurso.DescricaoTipoCusto,
                            Quantidade = Percurso.Quantidade,
                            CustoUnitario = Percurso.CustoUnitario,
                            CustoTotal = Percurso.CustoTotal,
                            PrecoUnitario = Percurso.PrecoUnitario,
                            PrecoVenda = Percurso.PrecoVenda,
                            CodOrigem = Percurso.CodOrigem,
                            DescricaoOrigem = Percurso.DescricaoOrigem,
                            CodDestino = Percurso.CodDestino,
                            DescricaoDestino = Percurso.DescricaoDestino,
                            Distancia = Percurso.Distancia,
                            DistanciaPrevista = Percurso.DistanciaPrevista,
                            RubricaSalarial = Percurso.RubricaSalarial,
                            RegistarSubsidiosPremios = Percurso.RegistarSubsidiosPremios,
                            Observacao = Percurso.Observacao,
                            RubricaSalarial2 = Percurso.RubricaSalarial2,
                            DataDespesa = Percurso.DataDespesa,
                            DataDespesaTexto = Percurso.DataDespesa.HasValue ? Percurso.DataDespesa.Value.ToString("yyyy-MM-dd") : "",
                            Funcionario = Percurso.Funcionario,
                            CodRegiao = Percurso.CodRegiao,
                            CodArea = Percurso.CodArea,
                            CodCresp = Percurso.CodCresp,
                            CalculoAutomatico = Percurso.CalculoAutomatico,
                            Matricula = Percurso.Matricula,
                            NoProjeto = Percurso.NoProjeto,
                            ProjetoDescricao = Percurso.ProjetoDescricao,
                            UtilizadorCriacao = Percurso.UtilizadorCriacao,
                            DataHoraCriacao = Percurso.DataHoraCriacao,
                            DataHoraCriacaoTexto = Percurso.DataHoraCriacao.HasValue ? Percurso.DataHoraCriacao.Value.ToString("yyyy-MM-dd") : "",
                            UtilizadorModificacao = Percurso.UtilizadorModificacao,
                            DataHoraModificacao = Percurso.DataHoraModificacao,
                            DataHoraModificacaoTexto = Percurso.DataHoraModificacao.HasValue ? Percurso.DataHoraModificacao.Value.ToString("yyyy-MM-dd") : ""
                        }).ToList();

                        //AJUDA DE CUSTO/DESPESA
                        result.FolhaDeHorasAjuda = DBLinhasFolhaHoras.GetAllByAjudaToList(data.FolhaDeHorasNo).Select(Ajuda => new LinhasFolhaHorasViewModel()
                        {
                            NoFolhaHoras = Ajuda.NoFolhaHoras,
                            NoLinha = Ajuda.NoLinha,
                            TipoCusto = Ajuda.TipoCusto,
                            CodTipoCusto = Ajuda.CodTipoCusto,
                            DescricaoTipoCusto = Ajuda.DescricaoTipoCusto,
                            //DescricaoCodTipoCusto = Ajuda.CodTipoCusto + " - " + DBTabelaConfRecursosFh.GetAll().Where(y => y.CodRecurso == Ajuda.CodTipoCusto).FirstOrDefault().Descricao,
                            DescricaoCodTipoCusto = DBTabelaConfRecursosFh.GetAll().Where(y => y.CodRecurso == Ajuda.CodTipoCusto).FirstOrDefault().Descricao,
                            Quantidade = Ajuda.Quantidade,
                            CustoUnitario = Ajuda.CustoUnitario,
                            CustoTotal = Ajuda.CustoTotal,
                            PrecoUnitario = Ajuda.PrecoUnitario,
                            PrecoVenda = Ajuda.PrecoVenda,
                            CodOrigem = Ajuda.CodOrigem,
                            DescricaoOrigem = Ajuda.DescricaoOrigem,
                            CodDestino = Ajuda.CodDestino,
                            DescricaoDestino = Ajuda.DescricaoDestino,
                            Distancia = Ajuda.Distancia,
                            DistanciaPrevista = Ajuda.DistanciaPrevista,
                            RubricaSalarial = Ajuda.RubricaSalarial,
                            RegistarSubsidiosPremios = Ajuda.RegistarSubsidiosPremios,
                            Observacao = Ajuda.Observacao,
                            RubricaSalarial2 = Ajuda.RubricaSalarial2,
                            DataDespesa = Ajuda.DataDespesa,
                            DataDespesaTexto = Ajuda.DataDespesa.HasValue ? Ajuda.DataDespesa.Value.ToString("yyyy-MM-dd") : "",
                            Funcionario = Ajuda.Funcionario,
                            CodRegiao = Ajuda.CodRegiao,
                            CodArea = Ajuda.CodArea,
                            CodCresp = Ajuda.CodCresp,
                            CalculoAutomatico = Ajuda.CalculoAutomatico,
                            Matricula = Ajuda.Matricula,
                            NoProjeto = Ajuda.NoProjeto,
                            ProjetoDescricao = Ajuda.ProjetoDescricao,
                            UtilizadorCriacao = Ajuda.UtilizadorCriacao,
                            DataHoraCriacao = Ajuda.DataHoraCriacao,
                            DataHoraCriacaoTexto = Ajuda.DataHoraCriacao.HasValue ? Ajuda.DataHoraCriacao.Value.ToString("yyyy-MM-dd") : "",
                            UtilizadorModificacao = Ajuda.UtilizadorModificacao,
                            DataHoraModificacao = Ajuda.DataHoraModificacao,
                            DataHoraModificacaoTexto = Ajuda.DataHoraModificacao.HasValue ? Ajuda.DataHoraModificacao.Value.ToString("yyyy-MM-dd") : ""
                        }).ToList();

                        //MÃO-DE-OBRA
                        result.FolhaDeHorasMaoDeObra = DBMaoDeObraFolhaDeHoras.GetAllByMaoDeObraToList(data.FolhaDeHorasNo).Select(MaoDeObra => new MaoDeObraFolhaDeHorasViewModel()
                        {
                            FolhaDeHorasNo = MaoDeObra.FolhaDeHorasNo,
                            LinhaNo = MaoDeObra.LinhaNo,
                            Date = MaoDeObra.Date,
                            DateTexto = MaoDeObra.Date.HasValue ? MaoDeObra.Date.Value.ToString("yyyy-MM-dd") : "",
                            ProjetoNo = MaoDeObra.ProjetoNo,
                            EmpregadoNo = MaoDeObra.EmpregadoNo,
                            CodigoTipoTrabalho = MaoDeObra.CodigoTipoTrabalho,
                            HoraInicio = MaoDeObra.HoraInicio,
                            HoraInicioTexto = MaoDeObra.HoraInicio == "00:00" ? "" : MaoDeObra.HoraInicio,
                            HorarioAlmoco = MaoDeObra.HorarioAlmoco,
                            HoraFim = MaoDeObra.HoraFim,
                            HoraFimTexto = MaoDeObra.HoraFim == "00:00" ? "" : MaoDeObra.HoraFim,
                            HorarioJantar = MaoDeObra.HorarioJantar,
                            CodigoFamiliaRecurso = MaoDeObra.CodigoFamiliaRecurso,
                            CodigoTipoOM = MaoDeObra.CodigoTipoOM,
                            HorasNo = MaoDeObra.HorasNo,
                            HorasNoTexto = MaoDeObra.HorasNo,
                            CustoUnitarioDireto = Convert.ToDecimal(MaoDeObra.CustoUnitarioDireto),
                            CodigoCentroResponsabilidade = MaoDeObra.CodigoCentroResponsabilidade,
                            PrecoTotal = Convert.ToDecimal(MaoDeObra.PrecoTotal),
                            Descricao = MaoDeObra.Descricao,
                            RecursoNo = MaoDeObra.RecursoNo,
                            CodigoUnidadeMedida = MaoDeObra.CodigoUnidadeMedida,
                            PrecoDeCusto = Convert.ToDecimal(MaoDeObra.PrecoDeCusto),
                            PrecoDeVenda = Convert.ToDecimal(MaoDeObra.PrecoDeVenda),
                            UtilizadorCriacao = MaoDeObra.UtilizadorCriacao,
                            DataHoraCriacao = MaoDeObra.DataHoraCriacao,
                            DataHoraCriacaoTexto = MaoDeObra.DataHoraCriacao.HasValue ? MaoDeObra.DataHoraCriacao.Value.ToString("yyyy-MM-dd") : "",
                            UtilizadorModificacao = MaoDeObra.UtilizadorModificacao,
                            DataHoraModificacao = MaoDeObra.DataHoraModificacao,
                            DataHoraModificacaoTexto = MaoDeObra.DataHoraModificacao.HasValue ? MaoDeObra.DataHoraModificacao.Value.ToString("yyyy-MM-dd") : ""
                        }).ToList();

                        //PRESENÇA
                        result.FolhaDeHorasPresenca = DBPresencasFolhaDeHoras.GetAllByPresencaToList(data.FolhaDeHorasNo).Select(Presenca => new PresencasFolhaDeHorasViewModel()
                        {
                            FolhaDeHorasNo = Presenca.FolhaDeHorasNo,
                            Data = Presenca.Data,
                            DataTexto = Presenca.Data.HasValue ? Presenca.Data.Value.ToString("yyyy-MM-dd")  : "",
                            Hora1Entrada = Convert.ToDateTime(string.Concat("1900/01/01 " + Presenca.Hora1Entrada)).ToShortTimeString(),
                            Hora1Saida = Convert.ToDateTime(string.Concat("1900/01/01 " + Presenca.Hora1Saida)).ToShortTimeString(),
                            Hora2Entrada = Convert.ToDateTime(string.Concat("1900/01/01 " + Presenca.Hora2Entrada)).ToShortTimeString(),
                            Hora2Saida = Convert.ToDateTime(string.Concat("1900/01/01 " + Presenca.Hora2Saida)).ToShortTimeString(),
                            Observacoes = Presenca.Observacoes,
                            UtilizadorCriacao = Presenca.UtilizadorCriacao,
                            DataHoraCriacao = Presenca.DataHoraCriacao,
                            DataHoraCriacaoTexto = Presenca.DataHoraCriacao.HasValue ? Presenca.DataHoraCriacao.Value.ToString("yyyy-MM-dd") : "",
                            UtilizadorModificacao = Presenca.UtilizadorModificacao,
                            DataHoraModificacao = Presenca.DataHoraModificacao,
                            DataHoraModificacaoTexto = Presenca.DataHoraModificacao.HasValue ? Presenca.DataHoraModificacao.Value.ToString("yyyy-MM-dd") : ""
                        }).ToList();

                        return Json(result);
                    }

                    return Json(new FolhaDeHorasViewModel());
                }
                return Json(false);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost]
        //Buscar o Nome de Empregado, Região, Área, CRESP, Responsáveis, Validadores e Integradores
        //public JsonResult GetEmployeeNome([FromBody] string idEmployee, [FromBody] string folhaDeHorasNo)
        public JsonResult GetEmployeeNome([FromBody] FolhaDeHorasViewModel data)
        {
            string idEmployee;
            string folhaDeHorasNo;
            FolhaDeHorasViewModel FH = new FolhaDeHorasViewModel();

            if (data != null)
            {
                idEmployee = data.EmpregadoNo;
                folhaDeHorasNo = data.FolhaDeHorasNo;

                if (!string.IsNullOrEmpty(folhaDeHorasNo))
                {
                    FH.Intervenientes = " CRIADOPOR: " + FH.CriadoPor;
                    ConfigUtilizadores ConfigUser = DBUserConfigurations.GetByEmployeeNo(idEmployee);
                    if (ConfigUser != null)
                        FH.Intervenientes = FH.Intervenientes + " EMPREGADO: " + ConfigUser.IdUtilizador;

                    FH = DBFolhasDeHoras.GetListaValidadoresIntegradores(folhaDeHorasNo, idEmployee, _config.NAV2009DatabaseName, _config.NAV2009CompanyName);

                    FH.Validadores = !string.IsNullOrEmpty(FH.Validadores) ? FH.Validadores : "";
                    FH.IntegradoresEmRH = !string.IsNullOrEmpty(FH.IntegradoresEmRH) ? FH.IntegradoresEmRH : "";
                    FH.IntegradoresEmRHKM = !string.IsNullOrEmpty(FH.IntegradoresEmRHKM) ? FH.IntegradoresEmRHKM : "";

                    FH.Responsavel1No = !string.IsNullOrEmpty(FH.Responsavel1No) ? FH.Responsavel1No : "";
                    FH.Responsavel2No = !string.IsNullOrEmpty(FH.Responsavel2No) ? FH.Responsavel2No : "";
                    FH.Responsavel3No = !string.IsNullOrEmpty(FH.Responsavel3No) ? FH.Responsavel3No : "";

                    FH.Intervenientes = !string.IsNullOrEmpty(FH.Intervenientes) ? FH.Intervenientes : "";

                    FH.EmpregadoNo = idEmployee;
                    FH.EmpregadoNome = DBNAV2009Employees.GetAll(idEmployee, _config.NAV2009DatabaseName, _config.NAV2009CompanyName).FirstOrDefault().Name;

                    data.UtilizadorModificacao = User.Identity.Name;
                    DBFolhasDeHoras.Update(DBFolhasDeHoras.ParseToFolhaHoras(data));
                }
            }

            return Json(FH);
        }

        [HttpPost]
        //Validação da Data e Hora Inicial e Final em todas as tabelas
        public JsonResult ValidarDataHoraInicio_DataHoraFim([FromBody] FolhaDeHorasViewModel data)
        {
            int result = 0;
            try
            {
                DateTime FH_DataHoraInicio = DateTime.Parse(string.Concat(data.DataPartidaTexto, " ", data.HoraPartidaTexto));
                DateTime FH_DataHoraFim = DateTime.Parse(string.Concat(data.DataChegadaTexto, " ", data.HoraChegadaTexto));

                if (result == 0)
                {
                    if (FH_DataHoraFim < FH_DataHoraInicio)
                        result = 1;
                    if (FH_DataHoraInicio > DateTime.Now)
                        result = 2;
                    if (FH_DataHoraFim > DateTime.Now)
                        result = 3;

                    if (string.IsNullOrEmpty(data.FolhaDeHorasNo))
                        result = 20;
                    if (string.IsNullOrEmpty(data.ProjetoNo))
                        result = 21;
                    if (string.IsNullOrEmpty(data.EmpregadoNo))
                        result = 22;
                    if (string.IsNullOrEmpty(data.DataPartidaTexto))
                        result = 23;
                    if (string.IsNullOrEmpty(data.HoraPartidaTexto))
                        result = 24;
                    if (string.IsNullOrEmpty(data.DataChegadaTexto))
                        result = 25;
                    if (string.IsNullOrEmpty(data.HoraChegadaTexto))
                        result = 26;
                    if (data.TipoDeslocacao == 0)
                        result = 27;
                    if (string.IsNullOrEmpty(data.CodigoTipoKms))
                        result = 28;
                    if (data.TipoDeslocacao == 1 && string.IsNullOrEmpty(data.Matricula))
                        result = 29;
                    if (string.IsNullOrEmpty(data.CodigoRegiao))
                        result = 30;
                    if (string.IsNullOrEmpty(data.CodigoAreaFuncional))
                        result = 31;
                    if (string.IsNullOrEmpty(data.CodigoCentroResponsabilidade))
                        result = 32;
                }

                //PERCURSOS
                //result = 0;
                if (result == 0)
                {
                    List<LinhasFolhaHorasViewModel> PERCURSOS = DBLinhasFolhaHoras.GetAllByPercursoToList(data.FolhaDeHorasNo);
                    if (PERCURSOS.Count() > 0)
                    {
                        PERCURSOS.ForEach(percurso =>
                        {
                            if (result == 0)
                            {
                                if (Convert.ToDateTime(Convert.ToDateTime(percurso.DataDespesa).ToShortDateString()) < Convert.ToDateTime(FH_DataHoraInicio.ToShortDateString()))
                                    result = 4;
                                if (Convert.ToDateTime(Convert.ToDateTime(percurso.DataDespesa).ToShortDateString()) > Convert.ToDateTime(FH_DataHoraFim.ToShortDateString()))
                                    result = 5;
                            }
                        });
                    }
                }

                //AJUDAS
                //result = 0;
                if (result == 0)
                {
                    List<LinhasFolhaHorasViewModel> AJUDAS = DBLinhasFolhaHoras.GetAllByAjudaToList(data.FolhaDeHorasNo);
                    if (AJUDAS.Count() > 0)
                    {
                        AJUDAS.ForEach(ajuda =>
                        {
                            if (result == 0)
                            {
                                if (Convert.ToDateTime(Convert.ToDateTime(ajuda.DataDespesa).ToShortDateString()) < Convert.ToDateTime(FH_DataHoraInicio.ToShortDateString()))
                                    result = 6;
                                if (Convert.ToDateTime(Convert.ToDateTime(ajuda.DataDespesa).ToShortDateString()) > Convert.ToDateTime(FH_DataHoraFim.ToShortDateString()))
                                    result = 7;
                            }
                        });
                    }
                }

                //MAODEOBRAS
                //result = 0;
                if (result == 0)
                {
                    List<MãoDeObraFolhaDeHoras> MAODEOBRAS = DBMaoDeObraFolhaDeHoras.GetByFolhaHoraNo(data.FolhaDeHorasNo);
                    if (MAODEOBRAS.Count() > 0)
                    {
                        DateTime maodeobraInicio;
                        DateTime maodeobraFim;
                        MAODEOBRAS.ForEach(maodeobra =>
                        {
                            if (result == 0)
                            {
                                maodeobraInicio = DateTime.Parse(string.Concat(Convert.ToDateTime(maodeobra.Date).ToShortDateString(), " ", maodeobra.HoraInício.Value.ToString()));
                                maodeobraFim = DateTime.Parse(string.Concat(Convert.ToDateTime(maodeobra.Date).ToShortDateString(), " ", maodeobra.HoraFim.Value.ToString()));

                                if (maodeobraInicio < FH_DataHoraInicio)
                                    result = 8;
                                if (maodeobraInicio > FH_DataHoraFim)
                                    result = 9;
                                if (maodeobraFim < FH_DataHoraInicio)
                                    result = 10;
                                if (maodeobraFim > FH_DataHoraFim)
                                    result = 11;
                            }
                        });
                    }
                }

                //PRESENCAS
                //result = 0;
                if (result == 0)
                {
                    List<PresençasFolhaDeHoras> PRESENCAS = DBPresencasFolhaDeHoras.GetByFolhaHoraNo(data.FolhaDeHorasNo);
                    if (PRESENCAS.Count() > 0)
                    {
                        DateTime presencaEntrada1;
                        DateTime presencaSaida1;
                        DateTime presencaEntrada2;
                        DateTime presencaSaida2;
                        PRESENCAS.ForEach(presenca =>
                        {
                            if (result == 0)
                            {
                                presencaEntrada1 = DateTime.Parse(string.Concat(presenca.Data.ToShortDateString(), " ", presenca.Hora1ªEntrada.ToString()));
                                presencaSaida1 = DateTime.Parse(string.Concat(presenca.Data.ToShortDateString(), " ", presenca.Hora1ªSaída.ToString()));
                                presencaEntrada2 = DateTime.Parse(string.Concat(presenca.Data.ToShortDateString(), " ", presenca.Hora2ªEntrada.ToString()));
                                presencaSaida2 = DateTime.Parse(string.Concat(presenca.Data.ToShortDateString(), " ", presenca.Hora2ªSaída.ToString()));

                                if (presencaEntrada1 < FH_DataHoraInicio)
                                    result = 12;
                                if (presencaEntrada1 > FH_DataHoraFim)
                                    result = 13;
                                if (presencaSaida1 < FH_DataHoraInicio)
                                    result = 14;
                                if (presencaSaida1 > FH_DataHoraFim)
                                    result = 15;
                                if (presencaEntrada2 < FH_DataHoraInicio)
                                    result = 16;
                                if (presencaEntrada2 > FH_DataHoraFim)
                                    result = 17;
                                if (presencaSaida2 < FH_DataHoraInicio)
                                    result = 18;
                                if (presencaSaida2 > FH_DataHoraFim)
                                    result = 19;
                            }
                        });
                    }
                }

                if (result == 0)
                {
                    List<FolhasDeHoras> FH = DBFolhasDeHoras.GetAll().ToList();
                    if (FH.Count() > 0)
                    {
                        FH.ForEach(x =>
                        {
                            if (x.NºFolhaDeHoras != null && x.NºEmpregado != null && x.DataHoraPartida != null && x.DataHoraChegada != null)
                            {
                                if (x.NºFolhaDeHoras != data.FolhaDeHorasNo && x.NºEmpregado == data.EmpregadoNo && FH_DataHoraInicio >= x.DataHoraPartida && FH_DataHoraFim <= x.DataHoraChegada)
                                {
                                    result = 88;
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                result = 99;
            }

            return Json(result);
        }

        [HttpPost]
        //Verifica se já existe alguma Folha de Horas no mesmo períoda para o mesmo Empregado
        public JsonResult UpdateFolhaDeHorasValidacao([FromBody] FolhaDeHorasViewModel data)
        {
            int result = 1;
            try
            {
                DateTime DataHoraInicio = Convert.ToDateTime(string.Concat(data.DataPartidaTexto, " ", data.HoraPartidaTexto));
                DateTime DataHoraFim = Convert.ToDateTime(string.Concat(data.DataChegadaTexto, " ", data.HoraChegadaTexto));

                List<FolhasDeHoras> FH = DBFolhasDeHoras.GetAll().ToList();
                if (FH.Count() > 0)
                {
                    FH.ForEach(x =>
                    {
                        if (result == 1)
                        {
                            if (x.NºFolhaDeHoras != null && x.NºEmpregado != null && x.DataHoraPartida != null && x.DataHoraChegada != null)
                            {
                                if (x.NºFolhaDeHoras != data.FolhaDeHorasNo && x.NºEmpregado == data.EmpregadoNo && DataHoraInicio >= x.DataHoraPartida && DataHoraFim <= x.DataHoraChegada)
                                {
                                    result = 0;
                                }
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                result = 99;
            }
            return Json(result);
        }

        [HttpPost]
        //Verifica se já existe alguma Folha de Horas no mesmo períoda para o mesmo Empregado
        public JsonResult DeleteFolhaDeHoras([FromBody] FolhaDeHorasViewModel data)
        {
            int result = 0;
            try
            {
                if (data.Validado == null || data.Validado == false)
                {
                    data.DataHoraPartida = DateTime.Parse(string.Concat(data.DataPartidaTexto, " ", data.HoraPartidaTexto));
                    data.DataHoraChegada = DateTime.Parse(string.Concat(data.DataChegadaTexto, " ", data.HoraChegadaTexto));

                    data.Eliminada = true; //ELIMINADA
                    data.UtilizadorModificacao = User.Identity.Name; //ELIMINADA
                    data.DataHoraModificacao = DateTime.Now; //ELIMINADA

                    FolhasDeHoras FH = DBFolhasDeHoras.ParseToFolhaHoras(data);

                    if (DBFolhasDeHoras.Update(FH) == null)
                    {
                        result = 2;
                    };

                    if (result == 0)
                    {
                        List<MovimentosDeAprovação> Aprovacoes = DBApprovalMovements.GetAll().Where(x => x.Número == data.FolhaDeHorasNo).ToList();

                        if (Aprovacoes.Count > 0)
                        {
                            Aprovacoes.ForEach(Aprovacao =>
                            {
                                Aprovacao.Estado = 3;

                                DBApprovalMovements.Update(Aprovacao);
                            });
                        }
                    }
                }
                else
                {
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                result = 99;
            }
            return Json(result);
        }

        [HttpPost]
        //Verifica se já existe alguma Folha de Horas no mesmo períoda para o mesmo Empregado
        public JsonResult ToHistoricFolhaDeHoras([FromBody] FolhaDeHorasViewModel data)
        {
            int result = 0;
            try
            {
                if (data.Estado == 1)
                {
                    data.Estado = 2; //HISTÓRICO
                    data.UtilizadorModificacao = User.Identity.Name; //HISTÓRICO
                    data.DataHoraModificacao = DateTime.Now; //HISTÓRICO

                    FolhasDeHoras FH = DBFolhasDeHoras.ParseToFolhaHoras(data);

                    if (DBFolhasDeHoras.Update(FH) == null)
                    {
                        result = 2;
                    };

                    if (result == 0)
                    {
                        List<MovimentosDeAprovação> Aprovacoes = DBApprovalMovements.GetAll().Where(x => x.Número == data.FolhaDeHorasNo).ToList();

                        if (Aprovacoes.Count > 0)
                        {
                            Aprovacoes.ForEach(Aprovacao =>
                            {
                                Aprovacao.Estado = 3;

                                DBApprovalMovements.Update(Aprovacao);
                            });
                        }
                    }
                }
                else
                {
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                result = 99;
            }
            return Json(result);
        }

        [HttpPost]
        //Atualiza a Folha de Horas na Base de Dados
        public JsonResult UpdateFolhaDeHoras([FromBody] FolhaDeHorasViewModel data)
        {
            int result = 0;
            try
            {
                if (data != null)
                {
                    data.DataHoraPartida = DateTime.Parse(string.Concat(data.DataPartidaTexto, " ", data.HoraPartidaTexto));
                    data.DataHoraChegada = DateTime.Parse(string.Concat(data.DataChegadaTexto, " ", data.HoraChegadaTexto));

                    data.UtilizadorModificacao = User.Identity.Name; //UPDATE
                    data.DataHoraModificacao = DateTime.Now; //UPDATE

                    if (!string.IsNullOrEmpty(data.ProjetoNo))
                    {
                        Projetos proj = new Projetos();
                        proj = DBProjects.GetById(data.ProjetoNo);
                        if (proj != null)
                        {
                            data.ProjetoDescricao = string.IsNullOrEmpty(proj.Descrição) ? "" : proj.Descrição;
                        }
                        else
                        {
                            NAVProjectsViewModel NAVproj = new NAVProjectsViewModel();
                            NAVproj = DBNAV2017Projects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, data.ProjetoNo).FirstOrDefault();
                            if (NAVproj != null)
                            {
                                data.ProjetoDescricao = string.IsNullOrEmpty(NAVproj.Description) ? "" : NAVproj.Description;
                            }
                        }

                        //data.ProjetoDescricao = DBProjects.GetById(data.ProjetoNo) != null ? string.IsNullOrEmpty(DBProjects.GetById(data.ProjetoNo).Descrição) ? "" : DBProjects.GetById(data.ProjetoNo).Descrição : string.IsNullOrEmpty(DBNAV2017Projects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, data.ProjetoNo).FirstOrDefault().Description) ? "" : DBNAV2017Projects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, data.ProjetoNo).FirstOrDefault().Description;
                    }

                    if (!string.IsNullOrEmpty(data.EmpregadoNo))
                        data.EmpregadoNome = DBNAV2009Employees.GetAll(data.EmpregadoNo, _config.NAV2009DatabaseName, _config.NAV2009CompanyName).FirstOrDefault().Name;


                    data.Intervenientes = " CRIADOPOR: ";
                    if (!string.IsNullOrEmpty(data.CriadoPor))
                        data.Intervenientes = data.Intervenientes + data.CriadoPor;

                    data.Intervenientes = data.Intervenientes + " EMPREGADO: ";
                    if (!string.IsNullOrEmpty(data.EmpregadoNo))
                    {
                        ConfigUtilizadores ConfigUser = DBUserConfigurations.GetByEmployeeNo(data.EmpregadoNo);
                        if (ConfigUser != null)
                            data.Intervenientes = data.Intervenientes + " EMPREGADO: " + ConfigUser.IdUtilizador;
                    }

                    data.Intervenientes = data.Intervenientes + " VALIDADORES: ";
                    if (!string.IsNullOrEmpty(data.Validadores))
                        data.Intervenientes = data.Intervenientes + data.Validadores;

                    data.Intervenientes = data.Intervenientes + " INTEGRADORESEMRH: ";
                    if (!string.IsNullOrEmpty(data.IntegradoresEmRH))
                        data.Intervenientes = data.Intervenientes + data.IntegradoresEmRH;

                    data.Intervenientes = data.Intervenientes + " INTEGRADORESEMRHKM: ";
                    if (!string.IsNullOrEmpty(data.IntegradoresEmRHKM))
                        data.Intervenientes = data.Intervenientes + data.IntegradoresEmRHKM;


                    FolhasDeHoras FH = DBFolhasDeHoras.ParseToFolhaHoras(data);

                    if (DBFolhasDeHoras.Update(FH) == null)
                    {
                        result = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                result = 99;
            }
            return Json(result);
        }

        [HttpPost]
        //Atualiza a Folha de Horas na Base de Dados
        public JsonResult AutoUpdateFolhaDeHoras([FromBody] FolhaDeHorasViewModel data)
        {
            int result = 0;
            try
            {
                if (data != null)
                {
                    if (!string.IsNullOrEmpty(data.ProjetoNo))
                        data.ProjetoDescricao = DBProjects.GetById(data.ProjetoNo) != null ? DBProjects.GetById(data.ProjetoNo).Descrição : "";

                    if (!string.IsNullOrEmpty(data.DataPartidaTexto))
                        data.DataHoraPartida = Convert.ToDateTime(data.DataPartidaTexto);
                    if (!string.IsNullOrEmpty(data.DataPartidaTexto) && !string.IsNullOrEmpty(data.HoraPartidaTexto))
                        data.DataHoraPartida = Convert.ToDateTime(data.DataPartidaTexto + " " + data.HoraPartidaTexto);

                    if (!string.IsNullOrEmpty(data.DataChegadaTexto))
                        data.DataHoraChegada = Convert.ToDateTime(data.DataChegadaTexto);
                    if (!string.IsNullOrEmpty(data.DataChegadaTexto) && !string.IsNullOrEmpty(data.HoraChegadaTexto))
                        data.DataHoraChegada = Convert.ToDateTime(data.DataChegadaTexto + " " + data.HoraChegadaTexto);

                    data.Intervenientes = " CRIADOPOR: ";
                    if (!string.IsNullOrEmpty(data.CriadoPor))
                        data.Intervenientes = data.Intervenientes + data.CriadoPor;
                    data.Intervenientes = data.Intervenientes + " EMPREGADO: ";
                    if (!string.IsNullOrEmpty(data.EmpregadoNo))
                    {
                        ConfigUtilizadores ConfigUser = DBUserConfigurations.GetByEmployeeNo(data.EmpregadoNo);
                        if (ConfigUser != null)
                            data.Intervenientes = data.Intervenientes + " EMPREGADO: " + ConfigUser.IdUtilizador;
                    }
                    data.Intervenientes = data.Intervenientes + " VALIDADORES: ";
                    if (!string.IsNullOrEmpty(data.Validadores))
                        data.Intervenientes = data.Intervenientes + data.Validadores;
                    data.Intervenientes = data.Intervenientes + " INTEGRADORESEMRH: ";
                    if (!string.IsNullOrEmpty(data.IntegradoresEmRH))
                        data.Intervenientes = data.Intervenientes + data.IntegradoresEmRH;
                    data.Intervenientes = data.Intervenientes + " INTEGRADORESEMRHKM: ";
                    if (!string.IsNullOrEmpty(data.IntegradoresEmRHKM))
                        data.Intervenientes = data.Intervenientes + data.IntegradoresEmRHKM;

                    data.UtilizadorModificacao = User.Identity.Name;
                    if (DBFolhasDeHoras.Update(DBFolhasDeHoras.ParseToFolhaHoras(data)) == null)
                    {
                        result = 2;
                    }
                }
                else
                {
                    result = 3;
                }
            }
            catch (Exception ex)
            {
                result = 99;
            }
            return Json(result);
        }

        [HttpPost]
        //Atualiza a Folha de Horas na Base de Dados
        public JsonResult UpdateFolhaDeHorasCreated([FromBody] FolhaDeHorasViewModel data)
        {
            int result = 0;
            try
            {
                if (!string.IsNullOrEmpty(data.FolhaDeHorasNo))
                {
                    data.DataHoraPartida = DateTime.Parse(string.Concat(data.DataPartidaTexto, " ", data.HoraPartidaTexto));
                    data.DataHoraChegada = DateTime.Parse(string.Concat(data.DataChegadaTexto, " ", data.HoraChegadaTexto));

                    data.UtilizadorModificacao = User.Identity.Name; //UPDATE
                    data.DataHoraModificacao = DateTime.Now; //UPDATE

                    if (!string.IsNullOrEmpty(data.ProjetoNo))
                    {
                        Projetos proj = new Projetos();
                        proj = DBProjects.GetById(data.ProjetoNo);
                        if (proj != null)
                        {
                            data.ProjetoDescricao = string.IsNullOrEmpty(proj.Descrição) ? "" : proj.Descrição;
                        }
                        else
                        {
                            NAVProjectsViewModel NAVproj = new NAVProjectsViewModel();
                            NAVproj = DBNAV2017Projects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, data.ProjetoNo).FirstOrDefault();
                            if (NAVproj != null)
                            {
                                data.ProjetoDescricao = string.IsNullOrEmpty(NAVproj.Description) ? "" : NAVproj.Description;
                            }
                        }

                        //data.ProjetoDescricao = DBProjects.GetById(data.ProjetoNo) != null ? string.IsNullOrEmpty(DBProjects.GetById(data.ProjetoNo).Descrição) ? "" : DBProjects.GetById(data.ProjetoNo).Descrição : string.IsNullOrEmpty(DBNAV2017Projects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, data.ProjetoNo).FirstOrDefault().Description) ? "" : DBNAV2017Projects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, data.ProjetoNo).FirstOrDefault().Description;
                    }

                    if (!string.IsNullOrEmpty(data.EmpregadoNo))
                        data.EmpregadoNome = DBNAV2009Employees.GetAll(data.EmpregadoNo, _config.NAV2009DatabaseName, _config.NAV2009CompanyName).FirstOrDefault().Name;

                    data.Intervenientes = " CRIADOPOR: ";
                    if (!string.IsNullOrEmpty(data.CriadoPor))
                        data.Intervenientes = data.Intervenientes + data.CriadoPor;
                    data.Intervenientes = data.Intervenientes + " EMPREGADO: ";
                    if (!string.IsNullOrEmpty(data.EmpregadoNo))
                    {
                        ConfigUtilizadores ConfigUser = DBUserConfigurations.GetByEmployeeNo(data.EmpregadoNo);
                        if (ConfigUser != null)
                            data.Intervenientes = data.Intervenientes + " EMPREGADO: " + ConfigUser.IdUtilizador;
                    }
                    data.Intervenientes = data.Intervenientes + " VALIDADORES: ";
                    if (!string.IsNullOrEmpty(data.Validadores))
                        data.Intervenientes = data.Intervenientes + data.Validadores;
                    data.Intervenientes = data.Intervenientes + " INTEGRADORESEMRH: ";
                    if (!string.IsNullOrEmpty(data.IntegradoresEmRH))
                        data.Intervenientes = data.Intervenientes + data.IntegradoresEmRH;
                    data.Intervenientes = data.Intervenientes + " INTEGRADORESEMRHKM: ";
                    if (!string.IsNullOrEmpty(data.IntegradoresEmRHKM))
                        data.Intervenientes = data.Intervenientes + data.IntegradoresEmRHKM;

                    FolhasDeHoras FH = DBFolhasDeHoras.ParseToFolhaHoras(data);

                    if (DBFolhasDeHoras.Update(FH) == null)
                    {
                        result = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                result = 99;
            }
            return Json(result);
        }

        #endregion

        #region PERCURSO

        [HttpPost]
        //Obtem todos os Percursos de uma Folha de Horas
        public JsonResult PercursoGetAllByFolhaHoraNoToList([FromBody] string FolhaHoraNo)
        {
            try
            {
                List<LinhasFolhaHorasViewModel> result = DBLinhasFolhaHoras.GetAllByPercursoToList(FolhaHoraNo);

                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        //Cria um novo Percurso
        public JsonResult CreatePercurso([FromBody] LinhasFolhaHorasViewModel data)
        {
            bool result = false;
            try
            {
                int noPercursos;
                noPercursos = DBLinhasFolhaHoras.GetPercursoByFolhaHoraNo(data.NoFolhaHoras).Count();

                int noLinha;
                noLinha = DBLinhasFolhaHoras.GetMaxByFolhaHoraNo(data.NoFolhaHoras);

                string ProjetoDescricao;
                if (!string.IsNullOrEmpty(data.NoProjeto))
                    ProjetoDescricao = DBNAV2017Projects.GetAll(_config.NAV2009DatabaseName, _config.NAV2009CompanyName, data.NoProjeto) != null ? DBNAV2017Projects.GetAll(_config.NAV2009DatabaseName, _config.NAV2009CompanyName, data.NoProjeto).FirstOrDefault().Description : "";
                else
                    ProjetoDescricao = "";

                if (noPercursos == 0)
                {
                    LinhasFolhaHoras Percurso1 = new LinhasFolhaHoras();

                    Percurso1.NoFolhaHoras = data.NoFolhaHoras;
                    Percurso1.NoLinha = noLinha;
                    Percurso1.TipoCusto = 1; //PERCURSO
                    Percurso1.CodTipoCusto = data.CodTipoCusto;
                    Percurso1.DescricaoTipoCusto = DBTabelaConfRecursosFh.GetDescricaoByRecurso("1", data.CodTipoCusto);
                    Percurso1.Quantidade = DBDistanciaFh.GetDistanciaPrevista(data.CodOrigem, data.CodDestino);
                    Percurso1.CodOrigem = data.CodOrigem;
                    Percurso1.DescricaoOrigem = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.CodOrigem);
                    Percurso1.CodDestino = data.CodDestino;
                    Percurso1.DescricaoDestino = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.CodDestino);
                    Percurso1.DataDespesa = data.DataDespesa;
                    Percurso1.Observacao = data.Observacao;
                    Percurso1.Distancia = DBDistanciaFh.GetDistanciaPrevista(data.CodOrigem, data.CodDestino);
                    Percurso1.DistanciaPrevista = DBDistanciaFh.GetDistanciaPrevista(data.CodOrigem, data.CodDestino);
                    Percurso1.CustoUnitario = DBTabelaConfRecursosFh.GetPrecoUnitarioCusto("1", data.CodTipoCusto);
                    Percurso1.CustoTotal = Percurso1.Distancia * Percurso1.CustoUnitario;
                    Percurso1.RubricaSalarial = DBTabelaConfRecursosFh.GetRubricaSalarial("1", data.CodTipoCusto);
                    Percurso1.Funcionario = data.Funcionario;
                    Percurso1.CodRegiao = data.CodRegiao;
                    Percurso1.CodArea = data.CodArea;
                    Percurso1.CodCresp = data.CodCresp;
                    Percurso1.Matricula = data.Matricula == "" ? null : data.Matricula;
                    Percurso1.UtilizadorCriacao = User.Identity.Name;
                    Percurso1.DataHoraCriacao = DateTime.Now;
                    Percurso1.UtilizadorModificacao = User.Identity.Name;
                    Percurso1.DataHoraModificacao = DateTime.Now;
                    Percurso1.NoProjeto = data.NoProjeto;
                    Percurso1.ProjetoDescricao = ProjetoDescricao;

                    var dbCreateResult1 = DBLinhasFolhaHoras.CreatePercurso(Percurso1);


                    LinhasFolhaHoras Percurso2 = new LinhasFolhaHoras();

                    Percurso2.NoFolhaHoras = data.NoFolhaHoras;
                    Percurso2.NoLinha = noLinha + 1;
                    Percurso2.TipoCusto = 1; //PERCURSO
                    Percurso2.CodTipoCusto = data.CodTipoCusto;
                    Percurso2.DescricaoTipoCusto = DBTabelaConfRecursosFh.GetDescricaoByRecurso("1", data.CodTipoCusto);
                    Percurso2.Quantidade = DBDistanciaFh.GetDistanciaPrevista(data.CodOrigem, data.CodDestino);
                    Percurso2.CodOrigem = data.CodDestino;
                    Percurso2.DescricaoOrigem = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.CodDestino);
                    Percurso2.CodDestino = data.CodOrigem;
                    Percurso2.DescricaoDestino = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.CodOrigem);
                    Percurso2.DataDespesa = data.DataDespesa;
                    Percurso2.Observacao = data.Observacao;
                    Percurso2.Distancia = DBDistanciaFh.GetDistanciaPrevista(data.CodOrigem, data.CodDestino);
                    Percurso2.DistanciaPrevista = DBDistanciaFh.GetDistanciaPrevista(data.CodOrigem, data.CodDestino);
                    Percurso2.CustoUnitario = DBTabelaConfRecursosFh.GetPrecoUnitarioCusto("1", data.CodTipoCusto);
                    Percurso2.CustoTotal = Percurso1.Distancia * Percurso1.CustoUnitario;
                    Percurso2.RubricaSalarial = DBTabelaConfRecursosFh.GetRubricaSalarial("1", data.CodTipoCusto);
                    Percurso2.Funcionario = data.Funcionario;
                    Percurso2.CodRegiao = data.CodRegiao;
                    Percurso2.CodArea = data.CodArea;
                    Percurso2.CodCresp = data.CodCresp;
                    Percurso2.Matricula = data.Matricula == "" ? null : data.Matricula;
                    Percurso2.UtilizadorCriacao = User.Identity.Name;
                    Percurso2.DataHoraCriacao = DateTime.Now;
                    Percurso2.UtilizadorModificacao = User.Identity.Name;
                    Percurso2.DataHoraModificacao = DateTime.Now;
                    Percurso2.NoProjeto = data.NoProjeto;
                    Percurso2.ProjetoDescricao = ProjetoDescricao;


                    var dbCreateResult2 = DBLinhasFolhaHoras.CreatePercurso(Percurso2);

                    if (dbCreateResult1 != null && dbCreateResult2 != null)
                        result = true;
                    else
                        result = false;
                }
                else
                {
                    LinhasFolhaHoras Percurso1 = new LinhasFolhaHoras();

                    Percurso1.NoFolhaHoras = data.NoFolhaHoras;
                    Percurso1.NoLinha = noLinha;
                    Percurso1.TipoCusto = 1; //PERCURSO
                    Percurso1.CodTipoCusto = data.CodTipoCusto;
                    Percurso1.DescricaoTipoCusto = DBTabelaConfRecursosFh.GetDescricaoByRecurso("1", data.CodTipoCusto);
                    Percurso1.Quantidade = DBDistanciaFh.GetDistanciaPrevista(data.CodOrigem, data.CodDestino);
                    Percurso1.CodOrigem = data.CodOrigem;
                    Percurso1.DescricaoOrigem = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.CodOrigem);
                    Percurso1.CodDestino = data.CodDestino;
                    Percurso1.DescricaoDestino = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.CodDestino);
                    Percurso1.DataDespesa = data.DataDespesa;
                    Percurso1.Observacao = data.Observacao;
                    Percurso1.Distancia = DBDistanciaFh.GetDistanciaPrevista(data.CodOrigem, data.CodDestino);
                    Percurso1.DistanciaPrevista = DBDistanciaFh.GetDistanciaPrevista(data.CodOrigem, data.CodDestino);
                    Percurso1.CustoUnitario = DBTabelaConfRecursosFh.GetPrecoUnitarioCusto("1", data.CodTipoCusto);
                    Percurso1.CustoTotal = Percurso1.Distancia * Percurso1.CustoUnitario;
                    Percurso1.RubricaSalarial = DBTabelaConfRecursosFh.GetRubricaSalarial("1", data.CodTipoCusto);
                    Percurso1.Funcionario = data.Funcionario;
                    Percurso1.CodRegiao = data.CodRegiao;
                    Percurso1.CodArea = data.CodArea;
                    Percurso1.CodCresp = data.CodCresp;
                    Percurso1.Matricula = data.Matricula == "" ? null : data.Matricula;
                    Percurso1.UtilizadorCriacao = User.Identity.Name;
                    Percurso1.DataHoraCriacao = DateTime.Now;
                    Percurso1.UtilizadorModificacao = User.Identity.Name;
                    Percurso1.DataHoraModificacao = DateTime.Now;
                    Percurso1.NoProjeto = data.NoProjeto;
                    Percurso1.ProjetoDescricao = ProjetoDescricao;


                    var dbCreateResult1 = DBLinhasFolhaHoras.CreatePercurso(Percurso1);

                    if (dbCreateResult1 != null)
                        result = true;
                    else
                        result = false;
                }

                if (result)
                {
                    DBFolhasDeHoras.UpdateDetalhes(data.NoFolhaHoras, _config.NAV2009DatabaseName, _config.NAV2009CompanyName);
                }

            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }

        //[HttpPost]
        //public JsonResult UpdatePercurso([FromBody] FolhaDeHorasViewModel data)
        //{
        //    bool result = false;
        //    try
        //    {
        //        if (data.FolhaDeHorasPercurso != null)
        //        {
        //            data.FolhaDeHorasPercurso.ForEach(x =>
        //            {
        //                DBLinhasFolhaHoras.UpdatePercurso(new LinhasFolhaHoras()
        //                {
        //                    NoFolhaHoras = x.NoFolhaHoras,
        //                    NoLinha = x.NoLinha,
        //                    TipoCusto = 1, //PERCURSO
        //                    CodOrigem = x.CodOrigem,
        //                    DescricaoOrigem = DBOrigemDestinoFh.GetOrigemDestinoDescricao(x.CodOrigem),
        //                    CodDestino = x.CodDestino,
        //                    DescricaoDestino = DBOrigemDestinoFh.GetOrigemDestinoDescricao(x.CodDestino),
        //                    DataDespesa = x.DataDespesa,
        //                    Observacao = x.Observacao,
        //                    Distancia = x.Distancia,
        //                    DistanciaPrevista = DBDistanciaFh.GetDistanciaPrevista(x.CodOrigem, x.CodDestino),
        //                    CustoUnitario = x.CustoUnitario,
        //                    CustoTotal = x.Distancia * x.CustoUnitario,
        //                    UtilizadorCriacao = x.UtilizadorCriacao,
        //                    DataHoraCriacao = x.DataHoraCriacao,
        //                    UtilizadorModificacao = User.Identity.Name,
        //                    DataHoraModificacao = DateTime.Now
        //                });
        //            });
        //        }

        //        result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //log
        //    }
        //    return Json(result);
        //}

        [HttpPost]
        //Atualiza um percurso
        public JsonResult UpdateLinhaPercurso([FromBody] LinhasFolhaHorasViewModel data)
        {
            int result = 0;

            try
            {
                if ((decimal)data.Distancia < 0 || (decimal)data.Distancia > 9999)
                    result = 1;

                FolhasDeHoras FH = DBFolhasDeHoras.GetById(data.NoFolhaHoras);
                DateTime Partida = Convert.ToDateTime(Convert.ToDateTime(FH.DataHoraPartida).ToShortDateString());
                DateTime Chegada = Convert.ToDateTime(Convert.ToDateTime(FH.DataHoraChegada).ToShortDateString());

                if (data.DataDespesa < Partida)
                    result = 2;
                if (data.DataDespesa > Chegada)
                    result = 3;

                if (result == 0)
                {
                    string ProjetoDescricao;
                    if (!string.IsNullOrEmpty(data.NoProjeto))
                        ProjetoDescricao = DBNAV2017Projects.GetAll(_config.NAV2009DatabaseName, _config.NAV2009CompanyName, data.NoProjeto) != null ? DBNAV2017Projects.GetAll(_config.NAV2009DatabaseName, _config.NAV2009CompanyName, data.NoProjeto).FirstOrDefault().Description : "";
                    else
                        ProjetoDescricao = "";

                    LinhasFolhaHoras Percurso = DBLinhasFolhaHoras.GetByPercursoNo(data.NoFolhaHoras, data.NoLinha);

                    Percurso.CodOrigem = data.CodOrigem;
                    Percurso.DescricaoOrigem = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.CodOrigem);
                    Percurso.CodDestino = data.CodDestino;
                    Percurso.DescricaoDestino = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.CodDestino);
                    Percurso.DataDespesa = data.DataDespesa;
                    Percurso.Observacao = data.Observacao;
                    Percurso.Distancia = data.Distancia;
                    Percurso.DistanciaPrevista = DBDistanciaFh.GetDistanciaPrevista(data.CodOrigem, data.CodDestino);
                    Percurso.CustoUnitario = data.CustoUnitario;
                    Percurso.CustoTotal = data.Distancia * data.CustoUnitario;
                    Percurso.UtilizadorModificacao = User.Identity.Name;
                    Percurso.DataHoraModificacao = DateTime.Now;
                    Percurso.NoProjeto = data.NoProjeto;
                    Percurso.ProjetoDescricao = ProjetoDescricao;

                    DBLinhasFolhaHoras.UpdatePercurso(Percurso);
                }

                if (result == 0)
                {
                    DBFolhasDeHoras.UpdateDetalhes(data.NoFolhaHoras, _config.NAV2009DatabaseName, _config.NAV2009CompanyName);
                }
            }
            catch (Exception ex)
            {
                //log
                result = 99;
            }
            return Json(result);
        }

        [HttpPost]
        //Copia um percurso
        public JsonResult CopiarLinhaPercurso([FromBody] LinhasFolhaHorasViewModel data)
        {
            int result = 0;

            try
            {
                LinhasFolhaHoras PercursoOriginal = DBLinhasFolhaHoras.GetByPercursoNo(data.NoFolhaHoras, data.NoLinha);
                LinhasFolhaHoras PercursoCopia = new LinhasFolhaHoras();

                int noLinha;
                noLinha = DBLinhasFolhaHoras.GetMaxByFolhaHoraNo(data.NoFolhaHoras);

                PercursoCopia.NoFolhaHoras = PercursoOriginal.NoFolhaHoras;
                PercursoCopia.NoLinha = noLinha;
                PercursoCopia.TipoCusto = PercursoOriginal.TipoCusto;
                PercursoCopia.CodTipoCusto = PercursoOriginal.CodTipoCusto;
                PercursoCopia.DescricaoTipoCusto = PercursoOriginal.DescricaoTipoCusto;
                PercursoCopia.Quantidade = PercursoOriginal.Quantidade;
                PercursoCopia.CodOrigem = PercursoOriginal.CodOrigem;
                PercursoCopia.DescricaoOrigem = PercursoOriginal.DescricaoOrigem;
                PercursoCopia.CodDestino = PercursoOriginal.CodDestino;
                PercursoCopia.DescricaoDestino = PercursoOriginal.DescricaoDestino;
                PercursoCopia.DataDespesa = PercursoOriginal.DataDespesa;
                PercursoCopia.Observacao = PercursoOriginal.Observacao;
                PercursoCopia.Distancia = PercursoOriginal.Distancia;
                PercursoCopia.DistanciaPrevista = PercursoOriginal.DistanciaPrevista;
                PercursoCopia.CustoUnitario = PercursoOriginal.CustoUnitario;
                PercursoCopia.CustoTotal = PercursoOriginal.CustoTotal;
                PercursoCopia.RubricaSalarial = PercursoOriginal.RubricaSalarial;
                PercursoCopia.Funcionario = PercursoOriginal.Funcionario;
                PercursoCopia.CodRegiao = PercursoOriginal.CodRegiao;
                PercursoCopia.CodArea = PercursoOriginal.CodArea;
                PercursoCopia.CodCresp = PercursoOriginal.CodCresp;
                PercursoCopia.Matricula = PercursoOriginal.Matricula;
                PercursoCopia.NoProjeto = PercursoOriginal.NoProjeto;
                PercursoCopia.ProjetoDescricao = PercursoOriginal.ProjetoDescricao;
                PercursoCopia.UtilizadorCriacao = User.Identity.Name;
                PercursoCopia.DataHoraCriacao = DateTime.Now;
                PercursoCopia.UtilizadorModificacao = User.Identity.Name;
                PercursoCopia.DataHoraModificacao = DateTime.Now;

                if (DBLinhasFolhaHoras.CreatePercurso(PercursoCopia) == null)
                    result = 1;

                if (result == 0)
                {
                    DBFolhasDeHoras.UpdateDetalhes(data.NoFolhaHoras, _config.NAV2009DatabaseName, _config.NAV2009CompanyName);
                }
            }
            catch (Exception ex)
            {
                //log
                result = 99;
            }
            return Json(result);
        }

        [HttpPost]
        //Apaga um Percurso
        public JsonResult DeletePercurso([FromBody] LinhasFolhaHorasViewModel data)
        {
            bool result = false;
            try
            {
                bool dbDeleteResult = DBLinhasFolhaHoras.DeletePercurso(data.NoFolhaHoras, data.NoLinha);

                result = dbDeleteResult;

                if (result)
                {
                    DBFolhasDeHoras.UpdateDetalhes(data.NoFolhaHoras, _config.NAV2009DatabaseName, _config.NAV2009CompanyName);
                }
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }

        #endregion

        #region AJUDA

        [HttpPost]
        //Obtem todas as Ajudas de uma Folha de Horas
        public JsonResult AjudaGetAllByFolhaHoraNoToList([FromBody] string FolhaHoraNo)
        {
            try
            {
                List<LinhasFolhaHorasViewModel> result = DBLinhasFolhaHoras.GetAllByAjudaToList(FolhaHoraNo);
                if (result != null)
                {
                    result.ForEach(x =>
                    {
                        x.DescricaoTipoCusto = EnumerablesFixed.FolhaDeHoraAjudaTipoCusto.Where(y => y.Id == x.TipoCusto).FirstOrDefault().Value;
                        x.DescricaoCodTipoCusto = x.CodTipoCusto + " - " + DBTabelaConfRecursosFh.GetAll().Where(y => y.CodRecurso == x.CodTipoCusto).FirstOrDefault().Descricao;
                    });
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        //Cria uma Ajuda
        public JsonResult CreateAjuda([FromBody] LinhasFolhaHorasViewModel data)
        {
            int result = 99;
            try
            {
                TabelaConfRecursosFh Recurso = DBTabelaConfRecursosFh.GetAll().Where(x => x.Tipo == data.TipoCusto.ToString() && x.CodRecurso == data.CodTipoCusto).FirstOrDefault();

                if (Recurso != null)
                {
                    if (Recurso.CalculoAutomatico != true)
                    {
                        string ProjetoDescricao;
                        if (!string.IsNullOrEmpty(data.NoProjeto))
                            ProjetoDescricao = DBNAV2017Projects.GetAll(_config.NAV2009DatabaseName, _config.NAV2009CompanyName, data.NoProjeto) != null ? DBNAV2017Projects.GetAll(_config.NAV2009DatabaseName, _config.NAV2009CompanyName, data.NoProjeto).FirstOrDefault().Description : "";
                        else
                            ProjetoDescricao = "";

                        int noLinha;
                        noLinha = DBLinhasFolhaHoras.GetMaxByFolhaHoraNo(data.NoFolhaHoras);

                        LinhasFolhaHoras Ajuda = new LinhasFolhaHoras();

                        Ajuda.NoFolhaHoras = data.NoFolhaHoras;
                        Ajuda.NoLinha = noLinha;
                        Ajuda.TipoCusto = data.TipoCusto;
                        Ajuda.CodTipoCusto = data.CodTipoCusto;
                        Ajuda.DescricaoTipoCusto = EnumerablesFixed.FolhaDeHoraAjudaTipoCusto.Where(y => y.Id == data.TipoCusto).FirstOrDefault().Value;
                        Ajuda.Quantidade = data.Quantidade;
                        Ajuda.CustoUnitario = data.CustoUnitario;
                        Ajuda.CustoTotal = data.Quantidade * data.CustoUnitario;
                        Ajuda.PrecoUnitario = data.PrecoUnitario;
                        Ajuda.PrecoVenda = data.Quantidade * data.PrecoUnitario;
                        Ajuda.DataDespesa = data.DataDespesa;
                        Ajuda.Observacao = data.Observacao;
                        Ajuda.Funcionario = data.Funcionario;
                        Ajuda.CodRegiao = data.CodRegiao;
                        Ajuda.CodArea = data.CodArea;
                        Ajuda.CodCresp = data.CodCresp;
                        Ajuda.CalculoAutomatico = false;
                        Ajuda.NoProjeto = data.NoProjeto;
                        Ajuda.ProjetoDescricao = ProjetoDescricao;
                        Ajuda.UtilizadorCriacao = User.Identity.Name;
                        Ajuda.DataHoraCriacao = DateTime.Now;
                        Ajuda.UtilizadorModificacao = User.Identity.Name;
                        Ajuda.DataHoraModificacao = DateTime.Now;

                        var dbCreateResult = DBLinhasFolhaHoras.CreateAjuda(Ajuda);

                        if (dbCreateResult != null)
                            result = 1;
                        else
                            result = 2;

                        if (result == 1)
                        {
                            if (DBFolhasDeHoras.UpdateDetalhes(data.NoFolhaHoras, _config.NAV2009DatabaseName, _config.NAV2009CompanyName) != null)
                                result = 1;
                            else
                                result = 3;
                        }
                    }
                    else
                        result = 4;
                }
                else
                    result = 5;
            }
            catch (Exception ex)
            {
                result = 99;
            }

            return Json(result);
        }

        //[HttpPost]
        //public JsonResult UpdateAjuda([FromBody] FolhaDeHorasViewModel data)
        //{
        //    bool result = false;
        //    try
        //    {
        //        if (data.FolhaDeHorasAjuda != null)
        //        {
        //            data.FolhaDeHorasAjuda.ForEach(x =>
        //            {
        //                DBLinhasFolhaHoras.UpdateAjuda(new LinhasFolhaHoras()
        //                {
        //                    NoFolhaHoras = x.NoFolhaHoras,
        //                    NoLinha = x.NoLinha,
        //                    TipoCusto = x.TipoCusto,
        //                    CodTipoCusto = x.CodTipoCusto,
        //                    DescricaoTipoCusto = EnumerablesFixed.FolhaDeHoraAjudaTipoCusto.Where(y => y.Id == x.TipoCusto).FirstOrDefault().Value,
        //                    Quantidade = x.Quantidade,
        //                    CustoUnitario = x.CustoUnitario,
        //                    CustoTotal = x.Quantidade * x.CustoUnitario,
        //                    PrecoUnitario = x.PrecoUnitario,
        //                    PrecoVenda = x.Quantidade * x.PrecoUnitario,
        //                    DataDespesa = x.DataDespesa,
        //                    Observacao = x.Observacao,
        //                    UtilizadorCriacao = x.UtilizadorCriacao,
        //                    DataHoraCriacao = x.DataHoraCriacao,
        //                    UtilizadorModificacao = User.Identity.Name,
        //                    DataHoraModificacao = DateTime.Now,

        //                });
        //            });
        //        }

        //        result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //log
        //    }
        //    return Json(result);
        //}

        [HttpPost]
        //Atualiza uma Ajuda
        public JsonResult UpdateLinhaAjuda([FromBody] LinhasFolhaHorasViewModel data)
        {
            int result = 0;
            try
            {
                if ((decimal)data.Quantidade < 0 || (decimal)data.Quantidade > 9999)
                    result = 1;

                if ((decimal)data.CustoUnitario < 0)
                    result = 2;

                if ((decimal)data.PrecoUnitario < 0)
                    result = 3;

                if ((decimal)data.PrecoVenda < 0)
                    result = 4;

                FolhasDeHoras FH = DBFolhasDeHoras.GetById(data.NoFolhaHoras);
                DateTime Partida = Convert.ToDateTime(Convert.ToDateTime(FH.DataHoraPartida).ToShortDateString());
                DateTime Chegada = Convert.ToDateTime(Convert.ToDateTime(FH.DataHoraChegada).ToShortDateString());
                if (data.DataDespesa < Partida)
                    result = 5;
                if (data.DataDespesa > Chegada)
                    result = 6;

                if (result == 0)
                {
                    string ProjetoDescricao;
                    if (!string.IsNullOrEmpty(data.NoProjeto))
                        ProjetoDescricao = DBNAV2017Projects.GetAll(_config.NAV2009DatabaseName, _config.NAV2009CompanyName, data.NoProjeto) != null ? DBNAV2017Projects.GetAll(_config.NAV2009DatabaseName, _config.NAV2009CompanyName, data.NoProjeto).FirstOrDefault().Description : "";
                    else
                        ProjetoDescricao = "";

                    LinhasFolhaHoras Ajuda = DBLinhasFolhaHoras.GetByAjudaNo(data.NoFolhaHoras, data.NoLinha);

                    if (Ajuda.CalculoAutomatico == true)
                    {
                        Ajuda.NoProjeto = data.NoProjeto;
                        Ajuda.ProjetoDescricao = ProjetoDescricao;
                        Ajuda.UtilizadorModificacao = User.Identity.Name;
                        Ajuda.DataHoraModificacao = DateTime.Now;
                    }
                    else
                    {
                        Ajuda.TipoCusto = data.TipoCusto;
                        Ajuda.CodTipoCusto = data.CodTipoCusto;
                        Ajuda.DescricaoTipoCusto = EnumerablesFixed.FolhaDeHoraAjudaTipoCusto.Where(y => y.Id == data.TipoCusto).FirstOrDefault().Value;
                        Ajuda.Quantidade = data.Quantidade;
                        Ajuda.CustoUnitario = data.CustoUnitario;
                        Ajuda.CustoTotal = data.Quantidade * data.CustoUnitario;
                        Ajuda.PrecoUnitario = data.PrecoUnitario;
                        Ajuda.PrecoVenda = data.Quantidade * data.PrecoUnitario;
                        Ajuda.DataDespesa = data.DataDespesa;
                        Ajuda.Observacao = data.Observacao;
                        Ajuda.NoProjeto = data.NoProjeto;
                        Ajuda.ProjetoDescricao = ProjetoDescricao;
                        Ajuda.UtilizadorModificacao = User.Identity.Name;
                        Ajuda.DataHoraModificacao = DateTime.Now;
                    }

                    DBLinhasFolhaHoras.UpdateAjuda(Ajuda);

                    result = 0;

                    if (result == 0)
                    {
                        DBFolhasDeHoras.UpdateDetalhes(data.NoFolhaHoras, _config.NAV2009DatabaseName, _config.NAV2009CompanyName);
                    }
                }
            }
            catch (Exception ex)
            {
                result = 99;
            }
            return Json(result);
        }

        [HttpPost]
        //Copia um percurso
        public JsonResult CopiarLinhaAjuda([FromBody] LinhasFolhaHorasViewModel data)
        {
            int result = 0;

            try
            {
                LinhasFolhaHoras AjudaOriginal = DBLinhasFolhaHoras.GetByAjudaNo(data.NoFolhaHoras, data.NoLinha);
                if (AjudaOriginal.CalculoAutomatico == true)
                {
                    result = 2;
                }
                else
                { 
                    LinhasFolhaHoras AjudaCopia = new LinhasFolhaHoras();

                    int noLinha;
                    noLinha = DBLinhasFolhaHoras.GetMaxByFolhaHoraNo(data.NoFolhaHoras);

                    AjudaCopia.NoFolhaHoras = AjudaOriginal.NoFolhaHoras;
                    AjudaCopia.NoLinha = noLinha;
                    AjudaCopia.TipoCusto = AjudaOriginal.TipoCusto;
                    AjudaCopia.CodTipoCusto = AjudaOriginal.CodTipoCusto;
                    AjudaCopia.DescricaoTipoCusto = AjudaOriginal.DescricaoTipoCusto;
                    AjudaCopia.Quantidade = AjudaOriginal.Quantidade;
                    AjudaCopia.CustoUnitario = AjudaOriginal.CustoUnitario;
                    AjudaCopia.CustoTotal = AjudaOriginal.CustoTotal;
                    AjudaCopia.PrecoUnitario = AjudaOriginal.PrecoUnitario;
                    AjudaCopia.PrecoVenda = AjudaOriginal.PrecoVenda;
                    AjudaCopia.DataDespesa = AjudaOriginal.DataDespesa;
                    AjudaCopia.Observacao = AjudaOriginal.Observacao;
                    AjudaCopia.Funcionario = AjudaOriginal.Funcionario;
                    AjudaCopia.CodRegiao = AjudaOriginal.CodRegiao;
                    AjudaCopia.CodArea = AjudaOriginal.CodArea;
                    AjudaCopia.CodCresp = AjudaOriginal.CodCresp;
                    AjudaCopia.CalculoAutomatico = AjudaOriginal.CalculoAutomatico;
                    AjudaCopia.NoProjeto = AjudaOriginal.NoProjeto;
                    AjudaCopia.ProjetoDescricao = AjudaOriginal.ProjetoDescricao;
                    AjudaCopia.UtilizadorCriacao = User.Identity.Name;
                    AjudaCopia.DataHoraCriacao = DateTime.Now;
                    AjudaCopia.UtilizadorModificacao = User.Identity.Name;
                    AjudaCopia.DataHoraModificacao = DateTime.Now;

                    if (DBLinhasFolhaHoras.CreateAjuda(AjudaCopia) == null)
                        result = 1;

                    if (result == 0)
                    {
                        DBFolhasDeHoras.UpdateDetalhes(data.NoFolhaHoras, _config.NAV2009DatabaseName, _config.NAV2009CompanyName);
                    }
                }
            }
            catch (Exception ex)
            {
                //log
                result = 99;
            }
            return Json(result);
        }

        [HttpPost]
        //Apaga uma Ajuda
        public JsonResult DeleteAjuda([FromBody] LinhasFolhaHorasViewModel data)
        {
            bool result = false;
            try
            {
                bool dbDeleteResult = DBLinhasFolhaHoras.DeleteAjuda(data.NoFolhaHoras, data.NoLinha);

                result = dbDeleteResult;

                if (result)
                {
                    DBFolhasDeHoras.UpdateDetalhes(data.NoFolhaHoras, _config.NAV2009DatabaseName, _config.NAV2009CompanyName);
                }
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }

        #endregion

        #region MÃO-DE-OBRA

        [HttpPost]
        //Obtem todas as Mão de Obras de uma Folha de Horas
        public JsonResult MaoDeObraGetAllByFolhaHoraNoToList([FromBody] string FolhaHoraNo)
        {
            try
            {
                List<MaoDeObraFolhaDeHorasViewModel> result = DBMaoDeObraFolhaDeHoras.GetAllByMaoDeObraToList(FolhaHoraNo);

                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        //Cria uma Mão de Obra
        public JsonResult CreateMaoDeObra([FromBody] MaoDeObraFolhaDeHorasViewModel data)
        {
            int result = 0;
            try
            {
                FolhasDeHoras FH = DBFolhasDeHoras.GetById(data.FolhaDeHorasNo);

                string FHDataPartida = FH.DataHoraPartida.HasValue ? Convert.ToDateTime(FH.DataHoraPartida).ToShortDateString() : "";
                string FHDataChegada = FH.DataHoraChegada.HasValue ? Convert.ToDateTime(FH.DataHoraChegada).ToShortDateString() : "";
                TimeSpan FHHoraPartida = TimeSpan.Parse(Convert.ToDateTime(FH.DataHoraPartida).ToShortTimeString());
                TimeSpan FHHoraChegada = TimeSpan.Parse(Convert.ToDateTime(FH.DataHoraChegada).ToShortTimeString());


                TimeSpan HoraMeiaNoite23 = TimeSpan.Parse("23:59");
                TimeSpan HoraMeiaNoite00 = TimeSpan.Parse("00:00");
                TimeSpan HoraInicio = TimeSpan.Parse(data.HoraInicio);
                TimeSpan HoraFim = TimeSpan.Parse(data.HoraFim);
                bool Almoco = Convert.ToBoolean(data.HorarioAlmoco);
                bool Jantar = Convert.ToBoolean(data.HorarioJantar);

                Configuração Configuracao = DBConfigurations.GetAll().Where(x => x.Id == 1).FirstOrDefault();

                TimeSpan InicioHoraAlmoco = (TimeSpan)Configuracao.InicioHoraAlmoco;
                TimeSpan FimHoraAlmoco = (TimeSpan)Configuracao.FimHoraAlmoco;
                TimeSpan InicioHoraJantar = (TimeSpan)Configuracao.InicioHoraJantar;
                TimeSpan FimHoraJantar = (TimeSpan)Configuracao.FimHoraJantar;

                if (FHDataPartida == FHDataChegada)
                {
                    if (Almoco)
                        if (HoraFim > InicioHoraAlmoco && HoraFim < FimHoraAlmoco)
                            result = 1;

                    if (Almoco)
                        if (HoraInicio > InicioHoraAlmoco && HoraInicio <= FimHoraAlmoco)
                            result = 2;

                    if (Jantar)
                        if (HoraFim > InicioHoraJantar && HoraFim < FimHoraJantar)
                            result = 3;

                    if (Jantar)
                        if (HoraInicio > InicioHoraJantar && HoraInicio <= FimHoraJantar)
                            result = 4;

                    if ((HoraInicio > HoraFim) || (HoraInicio < FHHoraPartida) || (HoraFim > FHHoraChegada))
                        result = 5;
                }
                else
                {
                    if (Almoco)
                        if (HoraFim > InicioHoraAlmoco && HoraFim < FimHoraAlmoco)
                            result = 1;

                    if (Almoco)
                        if (HoraInicio > InicioHoraAlmoco && HoraInicio <= FimHoraAlmoco)
                            result = 2;

                    if (Jantar)
                        if (HoraFim > InicioHoraJantar && HoraFim < FimHoraJantar)
                            result = 3;

                    if (Jantar)
                        if (HoraInicio > InicioHoraJantar && HoraInicio <= FimHoraJantar)
                            result = 4;

                    if (HoraMeiaNoite00 > HoraFim)
                        result = 5;

                    if (HoraInicio >= HoraFim)
                    {
                        if (Convert.ToDateTime(data.Date) >= Convert.ToDateTime(Convert.ToDateTime(FH.DataHoraChegada).ToShortDateString()))
                        {
                            result = 6;
                        }
                    }
                }

                if (result == 0)
                {
                    MãoDeObraFolhaDeHoras MaoDeObra = new MãoDeObraFolhaDeHoras();

                    //TABELA NAV2017JOB
                    MaoDeObra.CodigoRegiao = data.CodigoRegiao;
                    MaoDeObra.CodigoArea = data.CodigoArea;
                    MaoDeObra.CodigoCentroResponsabilidade = data.CodigoCentroResponsabilidade;

                    //TABELA RHRECURSOSFH
                    RhRecursosFh Recurso = DBRHRecursosFH.GetAll().Where(x => x.NoEmpregado.ToLower() == data.EmpregadoNo.ToLower()).FirstOrDefault();
                    if (Recurso != null)
                    {
                        MaoDeObra.NºRecurso = Recurso.Recurso;
                        MaoDeObra.CódigoFamíliaRecurso = Recurso.FamiliaRecurso;
                    }
                    else
                    {
                        result = 8;
                        return Json(result);
                    }

                    //TABELA PRECOVENDARECURSOFH
                    PrecoVendaRecursoFh PrecoVendaRecurso = DBPrecoVendaRecursoFH.GetAll().Where(x => x.Code.ToLower() == MaoDeObra.NºRecurso.ToLower() && x.CodTipoTrabalho.ToLower() == data.CodigoTipoTrabalho.ToString().ToLower() && Convert.ToDateTime(x.StartingDate) <= DateTime.Now && Convert.ToDateTime(x.EndingDate) >= DateTime.Now).FirstOrDefault();
                    if (PrecoVendaRecurso != null)
                    {
                        MaoDeObra.PreçoDeVenda = PrecoVendaRecurso.PrecoUnitario;
                        MaoDeObra.PreçoDeCusto = PrecoVendaRecurso.CustoUnitario;
                        MaoDeObra.CustoUnitárioDireto = PrecoVendaRecurso.PrecoUnitario;
                    }
                    else
                    {
                        result = 9;
                        return Json(result);
                    }

                    //CALCULAR PRECO TOTAL
                    TimeSpan HorasTotal;
                    if (FHDataPartida == FHDataChegada)
                    {
                        TimeSpan H_Almoco = FimHoraAlmoco.Subtract(InicioHoraAlmoco);
                        TimeSpan H_Jantar = FimHoraJantar.Subtract(InicioHoraJantar);

                        double Num_Horas_Aux = (HoraFim - HoraInicio).TotalHours;
                        HorasTotal = TimeSpan.Parse(data.HoraFim) - TimeSpan.Parse(data.HoraInicio);

                        if (data.HorarioAlmoco == true)
                        {
                            if (HoraFim >= FimHoraAlmoco && HoraInicio < InicioHoraAlmoco)
                            {
                                Num_Horas_Aux = Num_Horas_Aux - H_Almoco.TotalHours;
                                HorasTotal = HorasTotal.Subtract(H_Almoco);
                            }
                        }

                        if (data.HorarioJantar == true)
                        {
                            if (HoraFim >= FimHoraJantar && HoraInicio < InicioHoraJantar)
                            {
                                Num_Horas_Aux = Num_Horas_Aux - H_Jantar.TotalHours;
                                HorasTotal = HorasTotal.Subtract(H_Jantar);
                            }
                        }
                    }
                    else
                    {
                        double Num_Horas_Aux = 0;
                        TimeSpan H_Almoco = FimHoraAlmoco.Subtract(InicioHoraAlmoco);
                        TimeSpan H_Jantar = FimHoraJantar.Subtract(InicioHoraJantar);

                        if (HoraInicio < HoraFim)
                        {
                            Num_Horas_Aux = (HoraFim - HoraInicio).TotalHours;
                            HorasTotal = TimeSpan.Parse(data.HoraFim) - TimeSpan.Parse(data.HoraInicio);
                        }
                        else
                        {
                            Num_Horas_Aux = (HoraMeiaNoite23 - HoraInicio).Add(TimeSpan.Parse("00:01")).TotalHours + (HoraFim - HoraMeiaNoite00).TotalHours;
                            HorasTotal = HoraMeiaNoite23.Subtract(TimeSpan.Parse(data.HoraInicio)).Add(TimeSpan.Parse("00:01")) + (TimeSpan.Parse(data.HoraFim).Subtract(TimeSpan.Parse(HoraMeiaNoite00.ToString())));
                        }


                        if (data.HorarioAlmoco == true)
                        {
                            if (HoraInicio < HoraFim)
                            {
                                if ((HoraFim >= FimHoraAlmoco && HoraInicio < InicioHoraAlmoco))
                                {
                                    Num_Horas_Aux = Num_Horas_Aux - H_Almoco.TotalHours;
                                    HorasTotal = HorasTotal.Subtract(H_Almoco);
                                }
                            }
                            else
                            {
                                if ((HoraMeiaNoite23 >= FimHoraAlmoco && HoraInicio < InicioHoraAlmoco))
                                {
                                    Num_Horas_Aux = Num_Horas_Aux - H_Almoco.TotalHours;
                                    HorasTotal = HorasTotal.Subtract(H_Almoco);
                                }
                            }
                        }

                        if (data.HorarioJantar == true)
                        {
                            if (HoraInicio <= HoraFim)
                            {
                                if ((HoraFim >= FimHoraJantar && HoraInicio < InicioHoraJantar))
                                {
                                    Num_Horas_Aux = Num_Horas_Aux - H_Jantar.TotalHours;
                                    HorasTotal = HorasTotal.Subtract(H_Jantar);
                                }
                            }
                            else
                            {
                                if ((HoraMeiaNoite23 >= FimHoraJantar && HoraInicio < InicioHoraJantar))
                                {
                                    Num_Horas_Aux = Num_Horas_Aux - H_Jantar.TotalHours;
                                    HorasTotal = HorasTotal.Subtract(H_Jantar);
                                }
                            }
                        }
                    }

                    MaoDeObra.NºDeHoras = HorasTotal;

                    decimal HorasMinutosDecimal = Convert.ToDecimal(HorasTotal.TotalMinutes / 60);
                    MaoDeObra.PreçoTotal = HorasMinutosDecimal * Convert.ToDecimal(MaoDeObra.PreçoDeVenda);

                    MaoDeObra.NºFolhaDeHoras = data.FolhaDeHorasNo;
                    MaoDeObra.Date = data.Date;
                    MaoDeObra.NºProjeto = data.ProjetoNo;
                    MaoDeObra.NºEmpregado = data.EmpregadoNo;
                    MaoDeObra.CódigoTipoTrabalho = data.CodigoTipoTrabalho;
                    MaoDeObra.HoraInício = TimeSpan.Parse(data.HoraInicio);
                    MaoDeObra.HorárioAlmoço = data.HorarioAlmoco;
                    MaoDeObra.HoraFim = TimeSpan.Parse(data.HoraFim);
                    MaoDeObra.HorárioJantar = data.HorarioJantar;
                    MaoDeObra.CódigoTipoOm = null; //?????
                    MaoDeObra.Descricao = null; //?????
                    MaoDeObra.CódUnidadeMedida = data.CodigoUnidadeMedida;
                    MaoDeObra.UtilizadorCriação = User.Identity.Name;
                    MaoDeObra.DataHoraCriação = DateTime.Now;
                    MaoDeObra.UtilizadorModificação = User.Identity.Name;
                    MaoDeObra.DataHoraModificação = DateTime.Now;

                    var dbCreateResult = DBMaoDeObraFolhaDeHoras.Create(MaoDeObra);

                    if (dbCreateResult != null)
                        result = 0;
                    else
                        result = 7;

                    if (result == 0)
                    {
                        DBFolhasDeHoras.UpdateDetalhes(data.FolhaDeHorasNo, _config.NAV2009DatabaseName, _config.NAV2009CompanyName);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(99);
            }
            return Json(result);
        }

        [HttpPost]
        //Copia um percurso
        public JsonResult CopiarLinhaMaoDeObra([FromBody] MaoDeObraFolhaDeHorasViewModel data)
        {
            int result = 0;

            try
            {
                MãoDeObraFolhaDeHoras MaoDeObraOriginal = DBMaoDeObraFolhaDeHoras.GetByMaoDeObraNo((int)data.LinhaNo);
                MãoDeObraFolhaDeHoras MaoDeObraCopia = new MãoDeObraFolhaDeHoras();

                MaoDeObraCopia.NºFolhaDeHoras = MaoDeObraOriginal.NºFolhaDeHoras;
                MaoDeObraCopia.Date = MaoDeObraOriginal.Date;
                MaoDeObraCopia.NºProjeto = MaoDeObraOriginal.NºProjeto;
                MaoDeObraCopia.NºEmpregado = MaoDeObraOriginal.NºEmpregado;
                MaoDeObraCopia.CódigoTipoTrabalho = MaoDeObraOriginal.CódigoTipoTrabalho;
                MaoDeObraCopia.HoraInício = MaoDeObraOriginal.HoraInício;
                MaoDeObraCopia.HorárioAlmoço = MaoDeObraOriginal.HorárioAlmoço;
                MaoDeObraCopia.HoraFim = MaoDeObraOriginal.HoraFim;
                MaoDeObraCopia.HorárioJantar = MaoDeObraOriginal.HorárioJantar;
                MaoDeObraCopia.CódigoFamíliaRecurso = MaoDeObraOriginal.CódigoFamíliaRecurso;
                MaoDeObraCopia.CódigoTipoOm = MaoDeObraOriginal.CódigoTipoOm;
                MaoDeObraCopia.NºDeHoras = MaoDeObraOriginal.NºDeHoras;
                MaoDeObraCopia.CustoUnitárioDireto = MaoDeObraOriginal.CustoUnitárioDireto;
                MaoDeObraCopia.CodigoRegiao = MaoDeObraOriginal.CodigoRegiao;
                MaoDeObraCopia.CodigoArea = MaoDeObraOriginal.CodigoArea;
                MaoDeObraCopia.CodigoCentroResponsabilidade = MaoDeObraOriginal.CodigoCentroResponsabilidade;
                MaoDeObraCopia.PreçoTotal = MaoDeObraOriginal.PreçoTotal;
                MaoDeObraCopia.Descricao = MaoDeObraOriginal.Descricao;
                MaoDeObraCopia.NºRecurso = MaoDeObraOriginal.NºRecurso;
                MaoDeObraCopia.CódUnidadeMedida = MaoDeObraOriginal.CódUnidadeMedida;
                MaoDeObraCopia.PreçoDeCusto = MaoDeObraOriginal.PreçoDeCusto;
                MaoDeObraCopia.PreçoDeVenda = MaoDeObraOriginal.PreçoDeVenda;
                MaoDeObraCopia.UtilizadorCriação = User.Identity.Name;
                MaoDeObraCopia.DataHoraCriação = DateTime.Now;
                MaoDeObraCopia.UtilizadorModificação = User.Identity.Name;
                MaoDeObraCopia.DataHoraModificação = DateTime.Now;

                if (DBMaoDeObraFolhaDeHoras.Create(MaoDeObraCopia) == null)
                    result = 1;

                if (result == 0)
                {
                    DBFolhasDeHoras.UpdateDetalhes(data.FolhaDeHorasNo, _config.NAV2009DatabaseName, _config.NAV2009CompanyName);
                }
            }
            catch (Exception ex)
            {
                //log
                result = 99;
            }
            return Json(result);
        }

        //[HttpPost]
        //public JsonResult UpdateMaoDeObra([FromBody] FolhaDeHorasViewModel data)
        //{
        //    int result = 0;
        //    try
        //    {
        //        if (data.FolhaDeHorasMaoDeObra != null)
        //        {
        //            data.FolhaDeHorasMaoDeObra.ForEach(x =>
        //            {
        //                TimeSpan HoraInicio = TimeSpan.Parse(x.HoraInicio);
        //                TimeSpan HoraFim = TimeSpan.Parse(x.HoraFim);
        //                bool Almoco = Convert.ToBoolean(x.HorarioAlmoco);
        //                bool Jantar = Convert.ToBoolean(x.HorarioJantar);

        //                Configuração Configuracao = DBConfigurations.GetAll().Where(y => y.Id == 1).FirstOrDefault();

        //                TimeSpan InicioHoraAlmoco = (TimeSpan)Configuracao.InicioHoraAlmoco;
        //                TimeSpan FimHoraAlmoco = (TimeSpan)Configuracao.FimHoraAlmoco;
        //                TimeSpan InicioHoraJantar = (TimeSpan)Configuracao.InicioHoraJantar;
        //                TimeSpan FimHoraJantar = (TimeSpan)Configuracao.FimHoraJantar;

        //                if (Almoco)
        //                    if (HoraFim > InicioHoraAlmoco && HoraFim < FimHoraAlmoco)
        //                        result = 1;

        //                if (Almoco)
        //                    if (HoraInicio > InicioHoraAlmoco && HoraInicio <= FimHoraAlmoco)
        //                        result = 2;

        //                if (Jantar)
        //                    if (HoraFim > InicioHoraJantar && HoraFim < FimHoraJantar)
        //                        result = 3;

        //                if (Jantar)
        //                    if (HoraInicio > InicioHoraJantar && HoraInicio <= FimHoraJantar)
        //                        result = 4;

        //                if (HoraInicio > HoraFim)
        //                    result = 5;

        //                if (result == 0)
        //                {
        //                    //CALCULAR PRECO TOTAL
        //                    TimeSpan H_Almoco = FimHoraAlmoco.Subtract(InicioHoraAlmoco);
        //                    TimeSpan H_Jantar = FimHoraJantar.Subtract(InicioHoraJantar);

        //                    double Num_Horas_Aux = (HoraFim - HoraInicio).TotalHours;
        //                    TimeSpan HorasTotal = TimeSpan.Parse(x.HoraFim) - TimeSpan.Parse(x.HoraInicio);

        //                    if (x.HorarioAlmoco == true)
        //                    {
        //                        if (HoraFim >= FimHoraAlmoco && HoraInicio < InicioHoraAlmoco)
        //                        {
        //                            Num_Horas_Aux = Num_Horas_Aux - H_Almoco.TotalHours;
        //                            HorasTotal = HorasTotal.Subtract(H_Almoco);
        //                        }
        //                    }

        //                    if (x.HorarioJantar == true)
        //                    {
        //                        if (HoraFim >= FimHoraJantar && HoraInicio < InicioHoraJantar)
        //                        {
        //                            Num_Horas_Aux = Num_Horas_Aux - H_Jantar.TotalHours;
        //                            HorasTotal = HorasTotal.Subtract(H_Jantar);
        //                        }
        //                    }

        //                    x.HorasNo = HorasTotal.ToString();

        //                    decimal HorasMinutosDecimal = Convert.ToDecimal(HorasTotal.TotalMinutes / 60);
        //                    x.PrecoTotal = HorasMinutosDecimal * Convert.ToDecimal(x.PrecoDeVenda);

        //                    var dbUpdateResult = DBMaoDeObraFolhaDeHoras.Update(new MãoDeObraFolhaDeHoras()
        //                    {
        //                        NºFolhaDeHoras = x.FolhaDeHorasNo,
        //                        NºLinha = Convert.ToInt32(x.LinhaNo),
        //                        Date = x.Date,
        //                        NºProjeto = x.ProjetoNo,
        //                        NºEmpregado = x.EmpregadoNo,
        //                        CódigoTipoTrabalho = x.CodigoTipoTrabalho,
        //                        HoraInício = TimeSpan.Parse(x.HoraInicioTexto),
        //                        HorárioAlmoço = x.HorarioAlmoco,
        //                        HoraFim = TimeSpan.Parse(x.HoraFimTexto),
        //                        HorárioJantar = x.HorarioJantar,
        //                        CódigoFamíliaRecurso = x.CodigoFamiliaRecurso,
        //                        CódigoTipoOm = x.CodigoTipoOM,
        //                        NºDeHoras = TimeSpan.Parse(x.HorasNoTexto),
        //                        CustoUnitárioDireto = x.CustoUnitarioDireto,
        //                        CodigoRegiao = x.CodigoRegiao,
        //                        CodigoArea = x.CodigoArea,
        //                        CodigoCentroResponsabilidade = x.CodigoCentroResponsabilidade,
        //                        PreçoTotal = x.PrecoTotal,
        //                        Descricao = x.Descricao,
        //                        NºRecurso = x.RecursoNo,
        //                        CódUnidadeMedida = x.CodigoUnidadeMedida,
        //                        PreçoDeCusto = x.PrecoDeCusto,
        //                        PreçoDeVenda = x.PrecoDeVenda,
        //                        UtilizadorCriação = x.UtilizadorCriacao,
        //                        DataHoraCriação = x.DataHoraCriacao,
        //                        UtilizadorModificação = User.Identity.Name,
        //                        DataHoraModificação = DateTime.Now,
        //                    });

        //                    if (dbUpdateResult != null)
        //                        result = 0;
        //                    else
        //                        result = 6;

        //                    if (result == 0)
        //                    {
        //                        DBFolhasDeHoras.UpdateDetalhes(data.FolhaDeHorasNo);
        //                    }
        //                }
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(99);
        //    }
        //    return Json(result);
        //}

        [HttpPost]
        //Atualiza uma Mão de Obra
        public JsonResult UpdateLinhaMaoDeObra([FromBody] MaoDeObraFolhaDeHorasViewModel data)
        {
            int result = 0;
            try
            {
                FolhasDeHoras FH = DBFolhasDeHoras.GetById(data.FolhaDeHorasNo);
                DateTime FHDataHoraInicio = Convert.ToDateTime(FH.DataHoraPartida);
                DateTime FHDataHoraFIM = Convert.ToDateTime(FH.DataHoraChegada);

                MãoDeObraFolhaDeHoras MaoDeObra = DBMaoDeObraFolhaDeHoras.GetByMaoDeObraNo(Convert.ToInt32(data.LinhaNo));
                TimeSpan HoraInicio = TimeSpan.Parse(data.HoraInicio);
                TimeSpan HoraFim = TimeSpan.Parse(data.HoraFim);
                bool Almoco = Convert.ToBoolean(data.HorarioAlmoco);
                bool Jantar = Convert.ToBoolean(data.HorarioJantar);
                DateTime DataHoraInicio = Convert.ToDateTime(string.Concat(Convert.ToDateTime(data.Date).ToShortDateString(), " ", data.HoraInicio));
                DateTime DataHoraFim = Convert.ToDateTime(string.Concat(Convert.ToDateTime(data.Date).ToShortDateString(), " ", data.HoraFim));

                Configuração Configuracao = DBConfigurations.GetAll().Where(x => x.Id == 1).FirstOrDefault();
                TimeSpan InicioHoraAlmoco = (TimeSpan)Configuracao.InicioHoraAlmoco;
                TimeSpan FimHoraAlmoco = (TimeSpan)Configuracao.FimHoraAlmoco;
                TimeSpan InicioHoraJantar = (TimeSpan)Configuracao.InicioHoraJantar;
                TimeSpan FimHoraJantar = (TimeSpan)Configuracao.FimHoraJantar;

                if (Almoco)
                    if (HoraFim > InicioHoraAlmoco && HoraFim < FimHoraAlmoco)
                        result = 1;

                if (Almoco)
                    if (HoraInicio > InicioHoraAlmoco && HoraInicio <= FimHoraAlmoco)
                        result = 2;

                if (Jantar)
                    if (HoraFim > InicioHoraJantar && HoraFim < FimHoraJantar)
                        result = 3;

                if (Jantar)
                    if (HoraInicio > InicioHoraJantar && HoraInicio <= FimHoraJantar)
                        result = 4;

                if (HoraInicio > HoraFim)
                    result = 5;

                if (DataHoraInicio < FHDataHoraInicio)
                    result = 6;

                if (DataHoraInicio > FHDataHoraFIM)
                    result = 7;

                if (DataHoraFim < FHDataHoraInicio)
                    result = 8;

                if (DataHoraFim > FHDataHoraFIM)
                    result = 9;

                if (result == 0)
                {
                    //TABELA NAV2017JOB
                    MaoDeObra.CodigoRegiao = data.CodigoRegiao;
                    MaoDeObra.CodigoArea = data.CodigoArea;
                    MaoDeObra.CodigoCentroResponsabilidade = data.CodigoCentroResponsabilidade;

                    //TABELA RHRECURSOSFH
                    RhRecursosFh Recurso = DBRHRecursosFH.GetAll().Where(x => x.NoEmpregado.ToLower() == data.EmpregadoNo.ToLower()).FirstOrDefault();
                    if (Recurso != null)
                    {
                        MaoDeObra.NºRecurso = Recurso.Recurso;
                        MaoDeObra.CódigoFamíliaRecurso = Recurso.FamiliaRecurso;
                    }

                    //TABELA PRECOVENDARECURSOFH
                    PrecoVendaRecursoFh PrecoVendaRecurso = DBPrecoVendaRecursoFH.GetAll().Where(x => x.Code.ToLower() == MaoDeObra.NºRecurso.ToLower() && x.CodTipoTrabalho.ToLower() == data.CodigoTipoTrabalho.ToString().ToLower() && Convert.ToDateTime(x.StartingDate) <= DateTime.Now && Convert.ToDateTime(x.EndingDate) >= DateTime.Now).FirstOrDefault();
                    if (PrecoVendaRecurso != null)
                    {
                        MaoDeObra.PreçoDeVenda = PrecoVendaRecurso.PrecoUnitario;
                        MaoDeObra.PreçoDeCusto = PrecoVendaRecurso.CustoUnitario;
                        MaoDeObra.CustoUnitárioDireto = PrecoVendaRecurso.PrecoUnitario;
                    }

                    //CALCULAR PRECO TOTAL
                    TimeSpan H_Almoco = FimHoraAlmoco.Subtract(InicioHoraAlmoco);
                    TimeSpan H_Jantar = FimHoraJantar.Subtract(InicioHoraJantar);

                    double Num_Horas_Aux = (HoraFim - HoraInicio).TotalHours;
                    TimeSpan HorasTotal = TimeSpan.Parse(data.HoraFim) - TimeSpan.Parse(data.HoraInicio);

                    if (data.HorarioAlmoco == true)
                    {
                        if (HoraFim >= FimHoraAlmoco && HoraInicio < InicioHoraAlmoco)
                        {
                            Num_Horas_Aux = Num_Horas_Aux - H_Almoco.TotalHours;
                            HorasTotal = HorasTotal.Subtract(H_Almoco);
                        }
                    }

                    if (data.HorarioJantar == true)
                    {
                        if (HoraFim >= FimHoraJantar && HoraInicio < InicioHoraJantar)
                        {
                            Num_Horas_Aux = Num_Horas_Aux - H_Jantar.TotalHours;
                            HorasTotal = HorasTotal.Subtract(H_Jantar);
                        }
                    }

                    MaoDeObra.NºDeHoras = HorasTotal;

                    decimal HorasMinutosDecimal = Convert.ToDecimal(HorasTotal.TotalMinutes / 60);
                    MaoDeObra.PreçoTotal = HorasMinutosDecimal * Convert.ToDecimal(MaoDeObra.PreçoDeVenda);

                    MaoDeObra.NºFolhaDeHoras = data.FolhaDeHorasNo;
                    MaoDeObra.Date = data.Date;
                    MaoDeObra.NºProjeto = data.ProjetoNo;
                    MaoDeObra.NºEmpregado = data.EmpregadoNo;
                    MaoDeObra.CódigoTipoTrabalho = data.CodigoTipoTrabalho;
                    MaoDeObra.HoraInício = TimeSpan.Parse(data.HoraInicio);
                    MaoDeObra.HorárioAlmoço = data.HorarioAlmoco;
                    MaoDeObra.HoraFim = TimeSpan.Parse(data.HoraFim);
                    MaoDeObra.HorárioJantar = data.HorarioJantar;
                    MaoDeObra.CódigoTipoOm = null; //?????
                    MaoDeObra.Descricao = null; //?????
                    MaoDeObra.CódUnidadeMedida = data.CodigoUnidadeMedida;
                    MaoDeObra.UtilizadorCriação = User.Identity.Name;
                    MaoDeObra.DataHoraCriação = DateTime.Now;
                    MaoDeObra.UtilizadorModificação = User.Identity.Name;
                    MaoDeObra.DataHoraModificação = DateTime.Now;

                    var dbUpdateResult = DBMaoDeObraFolhaDeHoras.Update(MaoDeObra);

                    if (dbUpdateResult != null)
                        result = 0;
                    else
                        result = 6;

                    if (result == 0)
                    {
                        DBFolhasDeHoras.UpdateDetalhes(data.FolhaDeHorasNo, _config.NAV2009DatabaseName, _config.NAV2009CompanyName);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(99);
            }

            return Json(result);
        }

        [HttpPost]
        //Apaga uma Mão de Obra
        public JsonResult DeleteMaoDeObra([FromBody] MaoDeObraFolhaDeHorasViewModel data)
        {
            bool result = false;
            try
            {
                bool dbDeleteResult = DBMaoDeObraFolhaDeHoras.Delete(data.FolhaDeHorasNo, (int)data.LinhaNo);

                result = dbDeleteResult;

                if (result)
                {
                    DBFolhasDeHoras.UpdateDetalhes(data.FolhaDeHorasNo, _config.NAV2009DatabaseName, _config.NAV2009CompanyName);
                }
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }

        [HttpPost]
        //Verifica a Data e Hora de uma Mão de Obra
        public JsonResult MaoDeObraHoraInicioFim([FromBody] MaoDeObraFolhaDeHorasViewModel data)
        {
            int result = 0;
            TimeSpan HoraInicio = TimeSpan.Parse(data.HoraInicio);
            TimeSpan HoraFim = TimeSpan.Parse(data.HoraFim);
            bool Almoco = Convert.ToBoolean(data.HorarioAlmoco);
            bool Jantar = Convert.ToBoolean(data.HorarioJantar);

            Configuração Configuracao = DBConfigurations.GetAll().Where(x => x.Id == 1).FirstOrDefault();

            TimeSpan InicioHoraAlmoco = (TimeSpan)Configuracao.InicioHoraAlmoco;
            TimeSpan FimHoraAlmoco = (TimeSpan)Configuracao.FimHoraAlmoco;
            TimeSpan InicioHoraJantar = (TimeSpan)Configuracao.InicioHoraJantar;
            TimeSpan FimHoraJantar = (TimeSpan)Configuracao.FimHoraJantar;

            try
            {
                if (Almoco)
                {
                    if (HoraFim > InicioHoraAlmoco && HoraFim < FimHoraAlmoco)
                        result = 1;
                }

                if (Almoco)
                {
                    if (HoraInicio > InicioHoraAlmoco && HoraInicio <= FimHoraAlmoco)
                        result = 2;
                }

                if (Jantar)
                {
                    if (HoraFim > InicioHoraJantar && HoraFim < FimHoraJantar)
                        result = 3;
                }

                if (Jantar)
                {
                    if (HoraInicio > InicioHoraJantar && HoraInicio <= FimHoraJantar)
                        result = 4;
                }

                if (HoraInicio > HoraFim)
                {
                    result = 5;
                }
            }
            catch (Exception ex)
            {
                //log
            }

            return Json(result);
        }

        #endregion

        #region Presença

        [HttpPost]
        //Obtem todas as Presenças de uma Folha de Horas
        public JsonResult PresencasGetAllByFolhaHoraNoToList([FromBody] string FolhaHoraNo)
        {
            try
            {
                List<PresencasFolhaDeHorasViewModel> result = DBPresencasFolhaDeHoras.GetAllByPresencaToList(FolhaHoraNo);

                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        //Cria uma Presença
        public JsonResult CreatePresenca([FromBody] PresencasFolhaDeHorasViewModel data)
        {
            int result = 0;
            try
            {
                TimeSpan HoraInicio = TimeSpan.Parse(Convert.ToDateTime(DBFolhasDeHoras.GetById(data.FolhaDeHorasNo).DataHoraPartida).ToShortTimeString());
                TimeSpan HoraFim = TimeSpan.Parse(Convert.ToDateTime(DBFolhasDeHoras.GetById(data.FolhaDeHorasNo).DataHoraChegada).ToShortTimeString());

                TimeSpan Hora1Entrada = TimeSpan.Parse(data.Hora1Entrada);
                TimeSpan Hora1Saida = TimeSpan.Parse(data.Hora1Saida);
                TimeSpan Hora2Entrada = TimeSpan.Parse(data.Hora2Entrada);
                TimeSpan Hora2Saida = TimeSpan.Parse(data.Hora2Saida);

                if (Hora1Entrada < HoraInicio || Hora1Entrada > HoraFim)
                {
                    result = 2;
                    return Json(result);
                }
                if (Hora1Saida < HoraInicio || Hora1Saida > HoraFim)
                {
                    result = 3;
                    return Json(result);
                }
                if (Hora2Entrada < HoraInicio || Hora2Entrada > HoraFim)
                {
                    result = 4;
                    return Json(result);
                }
                if (Hora2Saida < HoraInicio || Hora2Saida > HoraFim)
                {
                    result = 5;
                    return Json(result);
                }

                if (Hora1Entrada > Hora1Saida || Hora1Entrada > Hora2Entrada || Hora1Entrada > Hora2Saida)
                {
                    result = 6;
                    return Json(result);
                }
                if (Hora1Saida < Hora1Entrada || Hora1Saida > Hora2Entrada || Hora1Saida > Hora2Saida)
                {
                    result = 7;
                    return Json(result);
                }
                if (Hora2Entrada < Hora1Entrada || Hora2Entrada < Hora1Saida || Hora2Entrada > Hora2Saida)
                {
                    result = 8;
                    return Json(result);
                }
                if (Hora2Saida < Hora1Entrada || Hora2Saida < Hora1Saida || Hora2Saida < Hora2Entrada)
                {
                    result = 9;
                    return Json(result);
                }

                if (result == 0)
                {
                    PresençasFolhaDeHoras Presenca = new PresençasFolhaDeHoras();

                    Presenca.NºFolhaDeHoras = data.FolhaDeHorasNo;
                    Presenca.Data = Convert.ToDateTime(data.Data);
                    Presenca.NoEmpregado = data.NoEmpregado;
                    Presenca.Hora1ªEntrada = TimeSpan.Parse(data.Hora1Entrada);
                    Presenca.Hora1ªSaída = TimeSpan.Parse(data.Hora1Saida);
                    Presenca.Hora2ªEntrada = TimeSpan.Parse(data.Hora2Entrada);
                    Presenca.Hora2ªSaída = TimeSpan.Parse(data.Hora2Saida);
                    Presenca.Observacoes = data.Observacoes;
                    Presenca.Validado = 0;
                    Presenca.IntegradoTr = 0;
                    Presenca.DataIntTr = null;
                    Presenca.UtilizadorCriação = User.Identity.Name;
                    Presenca.DataHoraCriação = DateTime.Now;
                    Presenca.UtilizadorModificação = User.Identity.Name;
                    Presenca.DataHoraModificação = DateTime.Now;

                    var dbCreateResult = DBPresencasFolhaDeHoras.Create(Presenca);

                    if (dbCreateResult != null)
                        result = 0;
                    else
                        result = 1;
                }
            }
            catch (Exception ex)
            {
                result = 99;
            }
            return Json(result);
        }

        //[HttpPost]
        //public JsonResult UpdatePresenca([FromBody] FolhaDeHorasViewModel data)
        //{
        //    bool result = false;

        //    try
        //    {
        //        if (data.FolhaDeHorasPresenca != null)
        //        {
        //            data.FolhaDeHorasPresenca.ForEach(x =>
        //            {
        //                DBPresencasFolhaDeHoras.Update(new PresençasFolhaDeHoras()
        //                {
        //                    NºFolhaDeHoras = x.FolhaDeHorasNo,
        //                    Data = Convert.ToDateTime(x.Data),
        //                    NoEmpregado = x.NoEmpregado,
        //                    Hora1ªEntrada = TimeSpan.Parse(x.Hora1Entrada),
        //                    Hora1ªSaída = TimeSpan.Parse(x.Hora1Saida),
        //                    Hora2ªEntrada = TimeSpan.Parse(x.Hora2Entrada),
        //                    Hora2ªSaída = TimeSpan.Parse(x.Hora2Saida),
        //                    Observacoes = x.Observacoes,
        //                    Validado = x.Validado,
        //                    IntegradoTr = x.IntegradoTR,
        //                    DataIntTr = Convert.ToDateTime(x.DataIntTR),
        //                    UtilizadorCriação = x.UtilizadorCriacao,
        //                    DataHoraCriação = x.DataHoraCriacao,
        //                    UtilizadorModificação = User.Identity.Name,
        //                    DataHoraModificação = DateTime.Now,
        //                });
        //            });
        //        }

        //        result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //log
        //    }
        //    return Json(result);
        //}

        [HttpPost]
        //Atualiza uma Presença
        public JsonResult UpdateLinhaPresenca([FromBody] PresencasFolhaDeHorasViewModel data)
        {
            int result = 0;
            try
            {
                TimeSpan HoraInicio = TimeSpan.Parse(Convert.ToDateTime(DBFolhasDeHoras.GetById(data.FolhaDeHorasNo).DataHoraPartida).ToShortTimeString());
                TimeSpan HoraFim = TimeSpan.Parse(Convert.ToDateTime(DBFolhasDeHoras.GetById(data.FolhaDeHorasNo).DataHoraChegada).ToShortTimeString());

                TimeSpan Hora1Entrada = TimeSpan.Parse(data.Hora1Entrada);
                TimeSpan Hora1Saida = TimeSpan.Parse(data.Hora1Saida);
                TimeSpan Hora2Entrada = TimeSpan.Parse(data.Hora2Entrada);
                TimeSpan Hora2Saida = TimeSpan.Parse(data.Hora2Saida);

                if (Hora1Entrada < HoraInicio || Hora1Entrada > HoraFim)
                {
                    result = 2;
                    return Json(result);
                }
                if (Hora1Saida < HoraInicio || Hora1Saida > HoraFim)
                {
                    result = 3;
                    return Json(result);
                }
                if (Hora2Entrada < HoraInicio || Hora2Entrada > HoraFim)
                {
                    result = 4;
                    return Json(result);
                }
                if (Hora2Saida < HoraInicio || Hora2Saida > HoraFim)
                {
                    result = 5;
                    return Json(result);
                }

                if (Hora1Entrada > Hora1Saida || Hora1Entrada > Hora2Entrada || Hora1Entrada > Hora2Saida)
                {
                    result = 6;
                    return Json(result);
                }
                if (Hora1Saida < Hora1Entrada || Hora1Saida > Hora2Entrada || Hora1Saida > Hora2Saida)
                {
                    result = 7;
                    return Json(result);
                }
                if (Hora2Entrada < Hora1Entrada || Hora2Entrada < Hora1Saida || Hora2Entrada > Hora2Saida)
                {
                    result = 8;
                    return Json(result);
                }
                if (Hora2Saida < Hora1Entrada || Hora2Saida < Hora1Saida || Hora2Saida < Hora2Entrada)
                {
                    result = 9;
                    return Json(result);
                }

                if (result == 0)
                {
                    PresençasFolhaDeHoras Presenca = DBPresencasFolhaDeHoras.GetAll().Where(x => x.NºFolhaDeHoras.ToLower() == data.FolhaDeHorasNo.ToLower() && x.Data == data.Data).FirstOrDefault();

                    if (Presenca != null)
                    {
                        Presenca.NºFolhaDeHoras = data.FolhaDeHorasNo;
                        Presenca.Data = Convert.ToDateTime(data.Data);
                        Presenca.NoEmpregado = data.NoEmpregado;
                        Presenca.Hora1ªEntrada = TimeSpan.Parse(data.Hora1Entrada);
                        Presenca.Hora1ªSaída = TimeSpan.Parse(data.Hora1Saida);
                        Presenca.Hora2ªEntrada = TimeSpan.Parse(data.Hora2Entrada);
                        Presenca.Hora2ªSaída = TimeSpan.Parse(data.Hora2Saida);
                        Presenca.Observacoes = data.Observacoes;
                        Presenca.Validado = Presenca.Validado;
                        Presenca.IntegradoTr = Presenca.IntegradoTr;
                        Presenca.DataIntTr = Presenca.DataIntTr;
                        Presenca.UtilizadorCriação = Presenca.UtilizadorCriação;
                        Presenca.DataHoraCriação = Presenca.DataHoraCriação;
                        Presenca.UtilizadorModificação = User.Identity.Name;
                        Presenca.DataHoraModificação = DateTime.Now;

                        DBPresencasFolhaDeHoras.Update(Presenca);

                        result = 0;
                    }
                    else
                        result = 1;
                }
            }
            catch (Exception ex)
            {
                result = 99;
            }

            return Json(result);
        }

        [HttpPost]
        //Apaga uma presença
        public JsonResult DeletePresenca([FromBody] PresencasFolhaDeHorasViewModel data)
        {
            bool result = false;
            try
            {
                bool dbDeleteResult = DBPresencasFolhaDeHoras.Delete(data.FolhaDeHorasNo, data.Data.ToString());

                result = dbDeleteResult;
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }
        #endregion

        #region Botões

        [HttpPost]
        //Calcula as Ajudas de Custo
        public JsonResult CalcularAjudasCusto([FromBody] FolhaDeHorasViewModel data)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 90;
            result.eMessage = "Não existem Despesas válidas para calcular as Ajudas de Custo.";
            try
            {
                decimal NoDias = 0;
                int noLinha;

                //Só Calcula as Ajudas de Custo se a Deslocação Fora do Concelho estiver ativa
                if (data.DeslocacaoForaConcelho == true)
                {
                    //APAGAR TODOS OS REGISTOS DAS LINHAS DE FOLHAS DE HORAS ONDE Calculo_Automatico = true
                    List<LinhasFolhaHoras> LinhasFH = DBLinhasFolhaHoras.GetAjudaByFolhaHoraNo(data.FolhaDeHorasNo).Where(x => (x.NoFolhaHoras.ToLower() == data.FolhaDeHorasNo.ToLower()) && (x.CalculoAutomatico == true)).ToList();
                    if (LinhasFH != null)
                    {
                        LinhasFH.ForEach(x =>
                        {
                            DBLinhasFolhaHoras.DeleteAjuda(x.NoFolhaHoras, x.NoLinha);
                        });
                    }

                    List<ConfiguracaoAjudaCusto> AjudaCusto = DBConfiguracaoAjudaCusto.GetAll().Where(x =>
                        (x.DataChegadaDataPartida == false) &&
                        (x.DistanciaMinima <= GetSUMDistancia(data.FolhaDeHorasNo)) &&
                        (x.TipoCusto != 1)
                        ).ToList();

                    if (AjudaCusto != null && AjudaCusto.Count() > 0)
                    {
                        AjudaCusto.ForEach(x =>
                        {
                            NoDias = Convert.ToInt32((Convert.ToDateTime(data.DataChegadaTexto) - Convert.ToDateTime(data.DataPartidaTexto)).TotalDays);
                            NoDias = NoDias + 1;

                            if (Convert.ToDateTime(data.DataPartidaTexto) == Convert.ToDateTime(data.DataChegadaTexto))
                            {

                                if (x.CodigoRefCusto == 1) //ALMOCO
                                {
                                    if (TimeSpan.Parse(data.HoraPartidaTexto) <= x.LimiteHoraPartida && TimeSpan.Parse(data.HoraChegadaTexto) > x.LimiteHoraPartida)
                                        NoDias = NoDias;
                                    else
                                        NoDias = NoDias - 1;
                                }

                                if (x.CodigoRefCusto == 2) //JANTAR
                                {
                                    if (TimeSpan.Parse(data.HoraChegadaTexto) >= x.LimiteHoraChegada && TimeSpan.Parse(data.HoraPartidaTexto) < x.LimiteHoraChegada)
                                        NoDias = NoDias;
                                    else
                                        NoDias = NoDias - 1;
                                }
                            }
                            else
                            {
                                if (x.CodigoRefCusto == 1) //ALMOCO
                                {
                                    if (TimeSpan.Parse(data.HoraPartidaTexto) <= x.LimiteHoraPartida)
                                        NoDias = NoDias;
                                    else
                                        NoDias = NoDias - 1;

                                    if ((TimeSpan.Parse(data.HoraChegadaTexto) >= x.LimiteHoraChegada) || data.DataPartidaTexto != data.DataChegadaTexto)
                                        NoDias = NoDias;
                                    else
                                        NoDias = NoDias - 1;
                                }

                                if (x.CodigoRefCusto == 2) //JANTAR
                                {
                                    if ((TimeSpan.Parse(data.HoraPartidaTexto) >= x.LimiteHoraPartida) || data.DataPartidaTexto != data.DataChegadaTexto)
                                        NoDias = NoDias;
                                    else
                                        NoDias = NoDias - 1;

                                    if (TimeSpan.Parse(data.HoraChegadaTexto) >= x.LimiteHoraChegada)
                                        NoDias = NoDias;
                                    else
                                        NoDias = NoDias - 1;
                                }
                            }

                            if (NoDias > 0)
                            {
                                noLinha = DBLinhasFolhaHoras.GetMaxByFolhaHoraNo(data.FolhaDeHorasNo);

                                LinhasFolhaHoras Ajuda = new LinhasFolhaHoras();

                                Ajuda.NoFolhaHoras = data.FolhaDeHorasNo;
                                Ajuda.NoLinha = noLinha;
                                Ajuda.CodTipoCusto = x.CodigoTipoCusto.Trim();
                                Ajuda.TipoCusto = x.TipoCusto;
                                Ajuda.DescricaoTipoCusto = EnumerablesFixed.FolhaDeHoraAjudaTipoCusto.Where(y => y.Id == x.TipoCusto).FirstOrDefault().Value;
                                Ajuda.Quantidade = Convert.ToDecimal(NoDias);
                                Ajuda.CustoUnitario = Convert.ToDecimal(DBTabelaConfRecursosFh.GetAll().Where(y => y.Tipo == x.TipoCusto.ToString() && y.CodRecurso == x.CodigoTipoCusto.Trim()).FirstOrDefault().PrecoUnitarioCusto);
                                Ajuda.PrecoUnitario = Convert.ToDecimal(DBTabelaConfRecursosFh.GetAll().Where(y => y.Tipo.ToLower() == x.TipoCusto.ToString().ToLower() && y.CodRecurso.ToLower() == x.CodigoTipoCusto.ToLower().Trim()).FirstOrDefault().PrecoUnitarioVenda);
                                Ajuda.CustoTotal = NoDias * Convert.ToDecimal(DBTabelaConfRecursosFh.GetAll().Where(y => y.Tipo.ToLower() == x.TipoCusto.ToString().ToLower() && y.CodRecurso.ToLower() == x.CodigoTipoCusto.ToLower().Trim()).FirstOrDefault().PrecoUnitarioCusto);
                                Ajuda.PrecoVenda = NoDias * Convert.ToDecimal(DBTabelaConfRecursosFh.GetAll().Where(y => y.Tipo.ToLower() == x.TipoCusto.ToString().ToLower() && y.CodRecurso.ToLower() == x.CodigoTipoCusto.ToLower().Trim()).FirstOrDefault().PrecoUnitarioVenda);
                                Ajuda.DataDespesa = Convert.ToDateTime(data.DataPartidaTexto + " " + data.HoraPartidaTexto);
                                Ajuda.CalculoAutomatico = true;
                                Ajuda.Funcionario = data.EmpregadoNo;
                                Ajuda.CodRegiao = data.CodigoRegiao == "" ? null : data.CodigoRegiao;
                                Ajuda.CodArea = data.CodigoAreaFuncional == "" ? null : data.CodigoAreaFuncional;
                                Ajuda.CodCresp = data.CodigoCentroResponsabilidade == null ? null : data.CodigoCentroResponsabilidade;
                                Ajuda.RubricaSalarial = DBTabelaConfRecursosFh.GetAll().Where(y => y.Tipo.ToLower() == x.TipoCusto.ToString().ToLower() && y.CodRecurso.ToLower() == x.CodigoTipoCusto.Trim().ToLower()).FirstOrDefault().RubricaSalarial;
                                Ajuda.NoProjeto = data.ProjetoNo;
                                Ajuda.ProjetoDescricao = data.ProjetoDescricao;
                                Ajuda.UtilizadorCriacao = User.Identity.Name;
                                Ajuda.DataHoraCriacao = DateTime.Now;
                                Ajuda.UtilizadorModificacao = User.Identity.Name;
                                Ajuda.DataHoraModificacao = DateTime.Now;

                                LinhasFolhaHoras dbCreateResult = DBLinhasFolhaHoras.CreateAjuda(Ajuda);
                                if (dbCreateResult != null)
                                {
                                    result.eReasonCode = 0;
                                    result.eMessage = "Foram calculados as Ajudas de Custo com sucesso.";
                                }
                                else
                                {
                                    result.eReasonCode = 1;
                                    result.eMessage = "Ocorreu um erro na no calculo das Ajudas de Custo.";
                                }
                            }
                        });
                    }
                    else
                    {
                        result.eReasonCode = 1;
                        result.eMessage = "Com os dados preenchidos nesta Folha de Horas não é possível calcular Ajudas de Custo.";
                    }

                    FolhasDeHoras dbUpdateResult = DBFolhasDeHoras.UpdateDetalhes(data.FolhaDeHorasNo, _config.NAV2009DatabaseName, _config.NAV2009CompanyName);

                    return Json(result);
                }
                else
                {
                    result.eReasonCode = 2;
                    result.eMessage = "As Ajudas de Custo só podem ser calculadas se for uma Deslocação Fora do Concelho.";
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro na atualização da Folha de Horas.";
            }

            return Json(result);
        }

        //Calcula o Somatória das Distâncias
        public decimal GetSUMDistancia(string noFH)
        {
            decimal SUMDistancia = 0;
            try
            {
                List<LinhasFolhaHoras> Linhas = DBLinhasFolhaHoras.GetPercursoByFolhaHoraNo(noFH).Where(x => x.TipoCusto == 1).ToList();
                if (Linhas != null)
                {
                    Linhas.ForEach(x =>
                    {
                        SUMDistancia = SUMDistancia + Convert.ToDecimal(x.Distancia);
                    });
                }

                return SUMDistancia;
            }
            catch (Exception ex)
            {
                //log
            }
            return SUMDistancia;
        }

        //[HttpPost]
        //public JsonResult ValidarBotaoFolhaDeHoras([FromBody] FolhaDeHorasViewModel data)
        //{
        //    bool result = false;
        //    try
        //    {
        //        string EmpregadoNome = DBUserConfigurations.GetById(User.Identity.Name).Nome;

        //        if (data.Estado == 0 && data.Validadores.ToLower().Contains(EmpregadoNome.ToLower()))
        //        {
        //            result = true;
        //        }

        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        [HttpPost]
        //Terminar uma Folha de Horas
        public JsonResult TerminarFolhaDeHoras([FromBody] FolhaDeHorasViewModel data)
        {
            ErrorHandler resultApprovalMovement = new ErrorHandler();
            try
            {
                if (data != null)
                {
                    List<LinhasFolhaHorasViewModel> Percursos = DBLinhasFolhaHoras.GetAllByPercursoToList(data.FolhaDeHorasNo);
                    List<LinhasFolhaHorasViewModel> Ajudas = DBLinhasFolhaHoras.GetAllByAjudaToList(data.FolhaDeHorasNo);
                    List<MaoDeObraFolhaDeHorasViewModel> MaoDeObra = DBMaoDeObraFolhaDeHoras.GetAllByMaoDeObraToList(data.FolhaDeHorasNo);

                    if ((Percursos == null || Percursos.Count() == 0) && (Ajudas == null || Ajudas.Count() == 0) && (MaoDeObra == null || MaoDeObra.Count() == 0))
                    {
                        resultApprovalMovement.eReasonCode = 9;
                        resultApprovalMovement.eMessage = "Não pode Terminar a Folha de Horas, pois não existe nenhum movimento associado á mesma.";
                        return Json(resultApprovalMovement);
                    }
                    else
                    {
                        if (data.DeslocacaoForaConcelho == true && (Percursos == null || Percursos.Count() == 0))
                        {
                            resultApprovalMovement.eReasonCode = 8;
                            resultApprovalMovement.eMessage = "Não pode Terminar a Folha de Horas, pois para Deslocação Fora do Concelho tem que existir pelo menos um Percurso.";
                            return Json(resultApprovalMovement);
                        }
                        else
                        {
                            using (var ctx = new SuchDBContextExtention())
                            {
                                var parameters = new[]
                                {
                                    new SqlParameter("@NoFH", data.FolhaDeHorasNo),
                            };
                                resultApprovalMovement.eReasonCode = ctx.execStoredProcedureFH("exec FH_Validar_Terminar @NoFH", parameters);

                                if (resultApprovalMovement.eReasonCode == 1)
                                {
                                    resultApprovalMovement.eMessage = "Ocorreu um erro na validação de Terminar.";
                                    return Json(resultApprovalMovement);
                                }
                                if (resultApprovalMovement.eReasonCode == 2)
                                {
                                    resultApprovalMovement.eMessage = "A Folha de Horas não pode ser terminada pois o Projeto não existe no eSUCH.";
                                    return Json(resultApprovalMovement);
                                }
                                if (resultApprovalMovement.eReasonCode == 3)
                                {
                                    resultApprovalMovement.eMessage = "A Folha de Horas não pode ser terminada pois a Ordem Manutenção não existe no Evolution.";
                                    return Json(resultApprovalMovement);
                                }
                            }

                            if (data.Terminada == true)
                            {
                                data.Intervenientes = " CRIADOPOR: " + data.CriadoPor;
                                ConfigUtilizadores ConfigUser = DBUserConfigurations.GetByEmployeeNo(data.EmpregadoNo);
                                if (ConfigUser != null)
                                    data.Intervenientes = data.Intervenientes + " EMPREGADO: " + ConfigUser.IdUtilizador;

                                FolhaDeHorasViewModel Autorizacao = DBFolhasDeHoras.GetListaValidadoresIntegradores(data.FolhaDeHorasNo, data.EmpregadoNo, _config.NAV2009DatabaseName, _config.NAV2009CompanyName);
                                if (Autorizacao != null)
                                {
                                    data.Validadores = !string.IsNullOrEmpty(Autorizacao.Validadores) ? Autorizacao.Validadores : "";
                                    data.IntegradoresEmRH = !string.IsNullOrEmpty(Autorizacao.IntegradoresEmRH) ? Autorizacao.IntegradoresEmRH : "";
                                    data.IntegradoresEmRHKM = !string.IsNullOrEmpty(Autorizacao.IntegradoresEmRHKM) ? Autorizacao.IntegradoresEmRHKM : "";

                                    data.Responsavel1No = !string.IsNullOrEmpty(Autorizacao.Responsavel1No) ? Autorizacao.Responsavel1No : "";
                                    data.Responsavel2No = !string.IsNullOrEmpty(Autorizacao.Responsavel2No) ? Autorizacao.Responsavel2No : "";
                                    data.Responsavel3No = !string.IsNullOrEmpty(Autorizacao.Responsavel3No) ? Autorizacao.Responsavel3No : "";

                                    data.Intervenientes = !string.IsNullOrEmpty(Autorizacao.Intervenientes) ? Autorizacao.Intervenientes : "";
                                };

                                data.DataHoraPartida = DateTime.Parse(string.Concat(data.DataPartidaTexto, " ", data.HoraPartidaTexto));
                                data.DataHoraChegada = DateTime.Parse(string.Concat(data.DataChegadaTexto, " ", data.HoraChegadaTexto));

                                data.Terminada = true; //TERMINADA
                                data.TerminadoPor = User.Identity.Name; //TERMINADA
                                data.DataHoraTerminado = DateTime.Now; //TERMINADA
                                data.UtilizadorModificacao = User.Identity.Name; //TERMINADA
                                data.DataHoraModificacao = DateTime.Now; //TERMINADA

                                FolhasDeHoras FH = DBFolhasDeHoras.ParseToFolhaHoras(data);

                                if (DBFolhasDeHoras.Update(FH) == null)
                                {
                                    resultApprovalMovement.eReasonCode = 6;
                                    resultApprovalMovement.eMessage = "Ocorreu um erro na atualização da Folha de Horas.";
                                }
                                else
                                {
                                    if (FH.Terminada == true)
                                    {
                                        decimal CustoTotal = (decimal)FH.CustoTotalAjudaCusto;

                                        if (DBFolhasDeHoras.GetById(data.FolhaDeHorasNo).Estado == 0) //CRIADO
                                        {
                                            if (DBApprovalMovements.GetAll().Where(x => x.Tipo == 3 && x.Número == FH.NºFolhaDeHoras && x.Estado == 1).ToList().Count() == 0)
                                                resultApprovalMovement = ApprovalMovementsManager.StartApprovalMovement_FH(3, FH.CódigoÁreaFuncional, FH.CódigoCentroResponsabilidade, FH.CódigoRegião, CustoTotal, FH.NºFolhaDeHoras, User.Identity.Name);
                                        }
                                        else
                                        {
                                            resultApprovalMovement.eReasonCode = 7;
                                            resultApprovalMovement.eMessage = "Não pode terminar a Folha de Horas pois a mesma já não está no estado Criado.";
                                        }

                                    }
                                    else
                                    {
                                        resultApprovalMovement.eReasonCode = 1;
                                        resultApprovalMovement.eMessage = "";
                                    }
                                };
                            }
                        }
                    }
                }
                else
                {
                    resultApprovalMovement.eReasonCode = 2;
                    resultApprovalMovement.eMessage = "Ocorreu um erro no processamento dados.";
                }
            }
            catch (Exception ex)
            {
                resultApprovalMovement.eReasonCode = 99;
                resultApprovalMovement.eMessage = "Ocorreu um erro no processamento dados.";
            }
            return Json(resultApprovalMovement);
        }

        [HttpPost]
        //Valida uma Folha de Horas
        public JsonResult ValidarFolhaDeHoras([FromBody] FolhaDeHorasViewModel data)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                if (string.IsNullOrEmpty(data.FolhaDeHorasNo) || string.IsNullOrEmpty(data.EmpregadoNo))
                {
                    result.eReasonCode = 101;
                    result.eMessage = "Faltam preencher os campos obrigatórios.";
                }
                else
                {

                    if (data.Terminada == null ? false : !(bool)data.Terminada)
                    {
                        result.eReasonCode = 101;
                        result.eMessage = "A Folha de Horas tem que ser Terminada antes de ser Validada.";
                    }
                    else
                    {
                        if ((data.Validado == null ? false : (bool)data.Validado) || (int)data.Estado != 0)
                        {
                            result.eReasonCode = 101;
                            result.eMessage = "A Folha de Horas já se encontra validada.";
                        }
                        else
                        {
                            if (!data.Validadores.ToLower().Contains(User.Identity.Name.ToLower()))
                            {
                                result.eReasonCode = 101;
                                result.eMessage = "Não tem permissões para validar a Folha de Horas.";
                            }
                            else
                            {
                                using (var ctx = new SuchDBContextExtention())
                                {
                                    var parameters = new[]
                                    {
                                        new SqlParameter("@NoFH", data.FolhaDeHorasNo),
                                        new SqlParameter("@NoUtilizador", data.EmpregadoNo),
                                        new SqlParameter("@NoValidador", User.Identity.Name)
                                    };
                                    result.eReasonCode = ctx.execStoredProcedureFH("exec FH_Validar @NoFH, @NoUtilizador, @NoValidador", parameters);
                                }

                                if (result.eReasonCode == 0)
                                {
                                    string EmpregadoNome = DBUserConfigurations.GetById(User.Identity.Name).Nome;
                                    int TipoDeslocação = (int)data.TipoDeslocacao;
                                    int Estado = (int)data.Estado;
                                    int NoRegistosAjC = 0;
                                    int NoRegistoskm = 0;

                                    NoRegistosAjC = DBLinhasFolhaHoras.GetAll().Where(x => x.NoFolhaHoras.ToLower() == data.FolhaDeHorasNo.ToLower() && x.TipoCusto == 2).Count();
                                    NoRegistoskm = DBLinhasFolhaHoras.GetAll().Where(x => x.NoFolhaHoras.ToLower() == data.FolhaDeHorasNo.ToLower() && x.TipoCusto == 1).Count();

                                    if ((NoRegistosAjC > 0) || (TipoDeslocação == 2 && NoRegistoskm > 0))
                                        Estado = 1; //VALIDADO
                                    else
                                        Estado = 2; // 2 = Registado

                                    if (DBFolhasDeHoras.Update(new FolhasDeHoras()
                                    {
                                        NºFolhaDeHoras = data.FolhaDeHorasNo,
                                        Área = data.Area,
                                        NºProjeto = data.ProjetoNo == "" ? null : data.ProjetoNo,
                                        ProjetoDescricao = data.ProjetoDescricao,
                                        NºEmpregado = data.EmpregadoNo == "" ? null : data.EmpregadoNo,
                                        NomeEmpregado = data.EmpregadoNome == "" ? null : data.EmpregadoNome,
                                        DataHoraPartida = DateTime.Parse(string.Concat(data.DataPartidaTexto, " ", data.HoraPartidaTexto)),
                                        DataHoraChegada = DateTime.Parse(string.Concat(data.DataChegadaTexto, " ", data.HoraChegadaTexto)),
                                        TipoDeslocação = data.TipoDeslocacaoTexto == "" ? 1 : Convert.ToInt32(data.TipoDeslocacaoTexto),
                                        CódigoTipoKmS = data.CodigoTipoKms == "" ? null : data.CodigoTipoKms,
                                        Matrícula = data.Matricula == "" ? null : data.Matricula,
                                        DeslocaçãoForaConcelho = data.DeslocacaoForaConcelho,
                                        DeslocaçãoPlaneada = data.DeslocacaoPlaneada,
                                        Terminada = data.Terminada,
                                        Estado = Estado, //VALIDAÇÂO
                                        CriadoPor = data.CriadoPor,
                                        DataHoraCriação = data.DataHoraCriacao,
                                        CódigoRegião = data.CodigoRegiao == "" ? null : data.CodigoRegiao,
                                        CódigoÁreaFuncional = data.CodigoAreaFuncional == "" ? null : data.CodigoAreaFuncional,
                                        CódigoCentroResponsabilidade = data.CodigoCentroResponsabilidade == "" ? null : data.CodigoCentroResponsabilidade,
                                        TerminadoPor = data.TerminadoPor,
                                        DataHoraTerminado = data.DataHoraTerminado,
                                        Validado = true, //VALIDAÇÂO
                                        Validadores = data.Validadores == "" ? null : data.Validadores,
                                        Validador = User.Identity.Name, //VALIDAÇÂO
                                        DataHoraValidação = DateTime.Now, //VALIDAÇÂO
                                        IntegradoEmRh = data.IntegradoEmRh,
                                        IntegradoresEmRh = data.IntegradoresEmRH,
                                        IntegradorEmRh = data.IntegradorEmRH,
                                        DataIntegraçãoEmRh = data.DataIntegracaoEmRH,
                                        IntegradoEmRhkm = data.IntegradoEmRhKm,
                                        IntegradoresEmRhkm = data.IntegradoresEmRHKM,
                                        IntegradorEmRhKm = data.IntegradorEmRHKM,
                                        DataIntegraçãoEmRhKm = data.DataIntegracaoEmRHKM,
                                        CustoTotalAjudaCusto = data.CustoTotalAjudaCusto,
                                        CustoTotalHoras = data.CustoTotalHoras,
                                        CustoTotalKm = data.CustoTotalKM,
                                        NumTotalKm = data.NumTotalKM,
                                        Observações = data.Observacoes,
                                        NºResponsável1 = data.Responsavel1No,
                                        NºResponsável2 = data.Responsavel2No,
                                        NºResponsável3 = data.Responsavel3No,
                                        ValidadoresRhKm = data.ValidadoresRHKM,
                                        DataHoraÚltimoEstado = DateTime.Now, //VALIDAÇÂO
                                        DataHoraModificação = DateTime.Now, //VALIDAÇÂO
                                        UtilizadorModificação = User.Identity.Name, //VALIDAÇÂO
                                        Eliminada = false,
                                        Intervenientes = data.Intervenientes
                                    }) != null)
                                    {
                                        result.eReasonCode = 0;
                                    }
                                    else
                                    {
                                        result.eReasonCode = 6;
                                        result.eMessage = "Ocorreu um erro ao atualizar a Folha de Horas.";
                                    };

                                    //Atualiza a tabela Presenças
                                    //ATENÇÃO QUE VAI ATIVAR O TRIGGER!!!
                                    List<PresencasFolhaDeHorasViewModel> presencas = DBPresencasFolhaDeHoras.GetAllByPresencaToList(data.FolhaDeHorasNo);
                                    if (presencas != null && presencas.Count() > 0)
                                    {
                                        presencas.ForEach(x =>
                                        {
                                            DBPresencasFolhaDeHoras.Update(new PresençasFolhaDeHoras()
                                            {
                                                NºFolhaDeHoras = x.FolhaDeHorasNo,
                                                Data = Convert.ToDateTime(x.Data),
                                                NoEmpregado = x.NoEmpregado,
                                                Hora1ªEntrada = TimeSpan.Parse(x.Hora1Entrada),
                                                Hora1ªSaída = TimeSpan.Parse(x.Hora1Saida),
                                                Hora2ªEntrada = TimeSpan.Parse(x.Hora2Entrada),
                                                Hora2ªSaída = TimeSpan.Parse(x.Hora2Saida),
                                                Observacoes = x.Observacoes,
                                                Validado = 1,
                                                IntegradoTr = 1,
                                                DataIntTr = DateTime.Now,
                                                UtilizadorCriação = x.UtilizadorCriacao,
                                                DataHoraCriação = x.DataHoraCriacao,
                                                UtilizadorModificação = User.Identity.Name,
                                                DataHoraModificação = DateTime.Now
                                            });
                                        });
                                    }

                                    if (result.eReasonCode == 0)
                                    {
                                        if (Estado == 1) //VALIDADO
                                        {
                                            bool integrarRH = false;

                                            if (data.IntegradoresEmRH.ToLower().Contains(User.Identity.Name.ToLower()))
                                            {
                                                if (NoRegistosAjC > 0)
                                                {
                                                    using (var ctxRH = new SuchDBContextExtention())
                                                    {
                                                        var parametersRH = new[]
                                                        {
                                                            new SqlParameter("@NoFH", data.FolhaDeHorasNo),
                                                            new SqlParameter("@NoUtilizador", data.EmpregadoNo),
                                                            new SqlParameter("@NoValidador", User.Identity.Name)
                                                        };
                                                        result.eReasonCode = ctxRH.execStoredProcedureFH("exec FH_IntegrarEmRH @NoFH, @NoUtilizador, @NoValidador", parametersRH);
                                                    }

                                                    if (result.eReasonCode == 0)
                                                    {
                                                        FolhasDeHoras FHIntegrarRH = DBFolhasDeHoras.GetById(data.FolhaDeHorasNo);

                                                        FHIntegrarRH.Estado = Estado; //INTEGRAREMRH
                                                        FHIntegrarRH.IntegradoEmRh = true; //INTEGRAREMRH
                                                        FHIntegrarRH.IntegradorEmRh = User.Identity.Name; //INTEGRAREMRH
                                                        FHIntegrarRH.DataIntegraçãoEmRh = DateTime.Now; //INTEGRAREMRH
                                                        FHIntegrarRH.UtilizadorModificação = User.Identity.Name; //INTEGRAREMRH
                                                        FHIntegrarRH.DataHoraModificação = DateTime.Now; //INTEGRAREMRH

                                                        if (DBFolhasDeHoras.Update(FHIntegrarRH) != null)
                                                        {
                                                            result.eReasonCode = 0;
                                                            integrarRH = true;
                                                        }
                                                        else
                                                        {
                                                            result.eReasonCode = 30;
                                                            result.eMessage = "Ocorreu um erro ao Integrar Ajudas de Custo.";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (result.eReasonCode == 1)
                                                        {
                                                            result.eReasonCode = 101;
                                                            result.eMessage = "Não tem permissões para validar.";
                                                        }
                                                        else
                                                        {
                                                            if (result.eReasonCode == 2)
                                                            {
                                                                result.eReasonCode = 102;
                                                                result.eMessage = "O projecto não existe no eSUCH e no Evolution.";
                                                            }
                                                            else
                                                            {
                                                                if (result.eReasonCode == 3)
                                                                {
                                                                    result.eReasonCode = 103;
                                                                    result.eMessage = "O projecto na Mão de Obra não existe no eSUCH e no Evolution.";
                                                                }
                                                                else
                                                                {
                                                                    if (result.eReasonCode == 5)
                                                                    {
                                                                        result.eReasonCode = 105;
                                                                        result.eMessage = "Não Pode validar pois já se encontra validada.";
                                                                    }
                                                                    else
                                                                    {
                                                                        result.eReasonCode = 199;
                                                                        result.eMessage = "Ocorreu um erro no script SQL de Validaçãodo na Folha de Horas.";
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                    integrarRH = true;
                                            }
                                            else
                                            {
                                                if (NoRegistosAjC == 0)
                                                    integrarRH = true;
                                            }

                                            if (data.IntegradoresEmRHKM.ToLower().Contains(User.Identity.Name.ToLower()))
                                            {
                                                if (data.TipoDeslocacao == 2 && NoRegistoskm > 0)
                                                {
                                                    using (var ctxKM = new SuchDBContextExtention())
                                                    {
                                                        var parametersKM = new[]
                                                        {
                                                            new SqlParameter("@NoFH", data.FolhaDeHorasNo),
                                                            new SqlParameter("@NoUtilizador", data.EmpregadoNo),
                                                            new SqlParameter("@NoValidador", User.Identity.Name)
                                                        };
                                                        result.eReasonCode = ctxKM.execStoredProcedureFH("exec FH_IntegrarEmRHKM @NoFH, @NoUtilizador, @NoValidador", parametersKM);
                                                    }

                                                    if (result.eReasonCode == 0)
                                                    {
                                                        FolhasDeHoras FHIntegrarRHKM = DBFolhasDeHoras.GetById(data.FolhaDeHorasNo);

                                                        if (integrarRH == true)
                                                            Estado = 2;

                                                        FHIntegrarRHKM.Estado = Estado; //INTEGRAREMRHKM
                                                        FHIntegrarRHKM.IntegradoEmRhkm = true; //INTEGRAREMRHKM
                                                        FHIntegrarRHKM.IntegradorEmRhKm = User.Identity.Name; //INTEGRAREMRHKM
                                                        FHIntegrarRHKM.DataIntegraçãoEmRhKm = DateTime.Now; //INTEGRAREMRHKM
                                                        FHIntegrarRHKM.UtilizadorModificação = User.Identity.Name; //INTEGRAREMRHKM
                                                        FHIntegrarRHKM.DataHoraModificação = DateTime.Now; //INTEGRAREMRHKM

                                                        if (DBFolhasDeHoras.Update(FHIntegrarRHKM) != null)
                                                        {
                                                            result.eReasonCode = 0;
                                                        }
                                                        else
                                                        {
                                                            result.eReasonCode = 31;
                                                            result.eMessage = "Ocorreu um erro ao Integrar km.";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (result.eReasonCode == 0)
                                                    {
                                                        FolhasDeHoras FHIntegrarRHKM = DBFolhasDeHoras.GetById(data.FolhaDeHorasNo);

                                                        if (integrarRH == true)
                                                            Estado = 2;

                                                        FHIntegrarRHKM.Estado = Estado;
                                                        FHIntegrarRHKM.UtilizadorModificação = User.Identity.Name;
                                                        FHIntegrarRHKM.DataHoraModificação = DateTime.Now;

                                                        if (DBFolhasDeHoras.Update(FHIntegrarRHKM) != null)
                                                        {
                                                            result.eReasonCode = 0;
                                                        }
                                                        else
                                                        {
                                                            result.eReasonCode = 31;
                                                            result.eMessage = "Ocorreu um erro ao Integrar km.";
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (Estado == 1)
                                        { 
                                            ErrorHandler approvalResult = new ErrorHandler();

                                            //Approve Movement
                                            MovimentosDeAprovação approvalMovement = DBApprovalMovements.GetAll().Where(x => x.Tipo == 3 && x.CódigoÁreaFuncional == data.CodigoAreaFuncional &&
                                                    x.CódigoRegião == data.CodigoRegiao && x.CódigoCentroResponsabilidade == data.CodigoCentroResponsabilidade && x.Número == data.FolhaDeHorasNo &&
                                                    x.Estado == 1 && x.Nivel == 1).FirstOrDefault();

                                            if (approvalMovement != null)
                                                approvalResult = ApprovalMovementsManager.ApproveMovement_FH(approvalMovement.NºMovimento, User.Identity.Name);

                                            //Check Approve Status
                                            if (approvalResult.eReasonCode == 353)
                                            {
                                                result.eReasonCode = 100;
                                                result.eMessage = "A Folha de Horas foi aprovada com sucesso.";
                                            }
                                            else if (approvalResult.eReasonCode == 350)
                                            {
                                                result.eReasonCode = 100;
                                                result.eMessage = "A Folha de Horas aprovada com sucesso, encontra-se a aguardar aprovação do nivel seguinte.";
                                            }
                                            else
                                            {
                                                result.eReasonCode = 199;
                                                result.eMessage = "Ocorreu um erro desconhecido ao aprovar a Folha de Horas.";
                                            }
                                        }

                                        if (Estado == 2) //PASSA PARA HISTÓRICO
                                        {
                                            //Update Old Movement
                                            MovimentosDeAprovação approvalMovement = DBApprovalMovements.GetAll().Where(x => x.Tipo == 3 && x.CódigoÁreaFuncional == data.CodigoAreaFuncional &&
                                                x.CódigoRegião == data.CodigoRegiao && x.CódigoCentroResponsabilidade == data.CodigoCentroResponsabilidade && x.Número == data.FolhaDeHorasNo &&
                                                x.Estado == 1 && x.Nivel == 1).FirstOrDefault();

                                            if (approvalMovement != null)
                                            {
                                                ApprovalMovementsViewModel ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetById(approvalMovement.NºMovimento));
                                                ApprovalMovement.Status = 2;
                                                ApprovalMovement.DateTimeApprove = DateTime.Now;
                                                ApprovalMovement.DateTimeUpdate = DateTime.Now;
                                                ApprovalMovement.UserUpdate = User.Identity.Name;
                                                ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.Update(DBApprovalMovements.ParseToDatabase(ApprovalMovement)));

                                                //Delete All User Approval Movements
                                                DBUserApprovalMovements.DeleteFromMovementExcept(ApprovalMovement.MovementNo, User.Identity.Name);
                                            }

                                            result.eReasonCode = 100;
                                            result.eMessage = "A Folha de Horas foi aprovada e encerrada com sucesso.";
                                        }

                                        FolhasDeHoras FHFinal = DBFolhasDeHoras.GetById(data.FolhaDeHorasNo);
                                        if (FHFinal.Estado == 1 && FHFinal.TipoDeslocação != 2 && FHFinal.IntegradoEmRh == true)
                                        {
                                            FHFinal.Estado = 2;
                                            DBFolhasDeHoras.Update(FHFinal);
                                        }
                                    }
                                    else
                                    {
                                        if (result.eReasonCode == 1)
                                        {
                                            result.eReasonCode = 101;
                                            result.eMessage = "Não tem permissões para validar.";
                                        }
                                        else
                                        {
                                            if (result.eReasonCode == 2)
                                            {
                                                result.eReasonCode = 102;
                                                result.eMessage = "O projecto não existe no eSUCH e no Evolution.";
                                            }
                                            else
                                            {
                                                if (result.eReasonCode == 3)
                                                {
                                                    result.eReasonCode = 103;
                                                    result.eMessage = "O projecto na Mão de Obra não existe no eSUCH e no Evolution.";
                                                }
                                                else
                                                {
                                                    if (result.eReasonCode == 5)
                                                    {
                                                        result.eReasonCode = 105;
                                                        result.eMessage = "Não Pode validar pois já se encontra validada.";
                                                    }
                                                    else
                                                    {
                                                        result.eReasonCode = 199;
                                                        result.eMessage = "Ocorreu um erro no script SQL de Validaçãodo na Folha de Horas.";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (result.eReasonCode == 1)
                                    {
                                        result.eReasonCode = 101;
                                        result.eMessage = "Não tem permissões para validar.";
                                    }
                                    else
                                    {
                                        if (result.eReasonCode == 2)
                                        {
                                            result.eReasonCode = 102;
                                            result.eMessage = "O projecto nas Linhas da Folha de Horas não existe no eSUCH e no Evolution.";
                                        }
                                        else
                                        {
                                            if (result.eReasonCode == 3)
                                            {
                                                result.eReasonCode = 103;
                                                result.eMessage = "O projecto na Mão de Obra não existe no eSUCH e no Evolution.";
                                            }
                                            else
                                            {
                                                if (result.eReasonCode == 5)
                                                {
                                                    result.eReasonCode = 105;
                                                    result.eMessage = "Não Pode validar pois já se encontra validada.";
                                                }
                                                else
                                                {
                                                    if (result.eReasonCode == 6)
                                                    {
                                                        result.eReasonCode = 106;
                                                        result.eMessage = "Já existem movimentos inseridos na tabela Movimentos De Projeto para esta Folha de Horas.";
                                                    }
                                                    else
                                                    {
                                                        if (result.eReasonCode == 7)
                                                        {
                                                            result.eReasonCode = 107;
                                                            result.eMessage = "Já existem movimentos inseridos na tabela Job Ledger Entry para esta Folha de Horas.";
                                                        }
                                                        else
                                                        {
                                                            result.eReasonCode = 199;
                                                            result.eMessage = "Ocorreu um erro no script SQL de Validaçãodo na Folha de Horas.";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro";
            }
            return Json(result);
        }

        [HttpPost]
        //Intregra as Ajudas de Custo de uma Folha de Horas
        public JsonResult IntegrarEmRHFolhaDeHoras([FromBody] FolhaDeHorasViewModel data)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                if (string.IsNullOrEmpty(data.FolhaDeHorasNo) || string.IsNullOrEmpty(data.EmpregadoNo) || string.IsNullOrEmpty(data.ProjetoNo))
                {
                    result.eReasonCode = 101;
                    result.eMessage = "Faltam preencher algum campo obrigatório na Folha de Horas.";
                }
                else
                {
                    if (data.IntegradoEmRh == null ? false : (bool)data.IntegradoEmRh)
                    {
                        result.eReasonCode = 101;
                        result.eMessage = "Já foram integradas as Ajudas de Custo na Folha de Horas.";
                    }
                    else
                    {
                        if ((int)data.Estado != 1)
                        {
                            result.eReasonCode = 101;
                            result.eMessage = "É necessário primeiro validar a Folha de Horas.";
                        }
                        else
                        {
                            if (!data.IntegradoresEmRH.ToLower().Contains(User.Identity.Name.ToLower()))
                            {
                                result.eReasonCode = 101;
                                result.eMessage = "Não tem permissões para validar a Folha de Horas";
                            }
                            else
                            {
                                using (var ctx = new SuchDBContextExtention())
                                {
                                    var parameters = new[]
                                    {
                                        new SqlParameter("@NoFH", data.FolhaDeHorasNo),
                                        new SqlParameter("@NoUtilizador", data.EmpregadoNo),
                                        new SqlParameter("@NoValidador", User.Identity.Name)
                                    };
                                    result.eReasonCode = ctx.execStoredProcedureFH("exec FH_IntegrarEmRH @NoFH, @NoUtilizador, @NoValidador", parameters);
                                }

                                if (result.eReasonCode == 0)
                                {
                                    string EmpregadoNome = DBUserConfigurations.GetById(User.Identity.Name).Nome;
                                    bool IntegradoEmRhKm = (bool)data.IntegradoEmRhKm;
                                    string TipoDeslocação = data.TipoDeslocacaoTexto;
                                    int Estado = (int)data.Estado;
                                    int NoRegistoskm = DBLinhasFolhaHoras.GetAll().Where(x => x.NoFolhaHoras.ToLower() == data.FolhaDeHorasNo.ToLower() && x.TipoCusto == 1).Count();

                                    if (IntegradoEmRhKm == true || NoRegistoskm == 0)
                                        Estado = 2; // 2 = Registado

                                    if (DBFolhasDeHoras.Update(new FolhasDeHoras()
                                    {
                                        NºFolhaDeHoras = data.FolhaDeHorasNo,
                                        Área = data.Area,
                                        NºProjeto = data.ProjetoNo == "" ? null : data.ProjetoNo,
                                        ProjetoDescricao = data.ProjetoDescricao,
                                        NºEmpregado = data.EmpregadoNo == "" ? null : data.EmpregadoNo,
                                        NomeEmpregado = data.EmpregadoNome == "" ? null : data.EmpregadoNome,
                                        DataHoraPartida = DateTime.Parse(string.Concat(data.DataPartidaTexto, " ", data.HoraPartidaTexto)),
                                        DataHoraChegada = DateTime.Parse(string.Concat(data.DataChegadaTexto, " ", data.HoraChegadaTexto)),
                                        TipoDeslocação = data.TipoDeslocacaoTexto == "" ? 1 : Convert.ToInt32(data.TipoDeslocacaoTexto),
                                        CódigoTipoKmS = data.CodigoTipoKms == "" ? null : data.CodigoTipoKms,
                                        Matrícula = data.Matricula == "" ? null : data.Matricula,
                                        DeslocaçãoForaConcelho = data.DeslocacaoForaConcelho,
                                        DeslocaçãoPlaneada = data.DeslocacaoPlaneada,
                                        Terminada = data.Terminada,
                                        Estado = Estado, //INTEGRAREMRH
                                        CriadoPor = data.CriadoPor,
                                        DataHoraCriação = data.DataHoraCriacao,
                                        CódigoRegião = data.CodigoRegiao == "" ? null : data.CodigoRegiao,
                                        CódigoÁreaFuncional = data.CodigoAreaFuncional == "" ? null : data.CodigoAreaFuncional,
                                        CódigoCentroResponsabilidade = data.CodigoCentroResponsabilidade == "" ? null : data.CodigoCentroResponsabilidade,
                                        TerminadoPor = data.TerminadoPor,
                                        DataHoraTerminado = data.DataHoraTerminado,
                                        Validado = data.Validado,
                                        Validadores = data.Validadores == "" ? null : data.Validadores,
                                        Validador = data.Validador,
                                        DataHoraValidação = data.DataHoraValidacao,
                                        IntegradoEmRh = true, //INTEGRAREMRH
                                        IntegradoresEmRh = data.IntegradoresEmRH,
                                        IntegradorEmRh = User.Identity.Name, //INTEGRAREMRH
                                        DataIntegraçãoEmRh = DateTime.Now, //INTEGRAREMRH
                                        IntegradoEmRhkm = data.IntegradoEmRhKm,
                                        IntegradoresEmRhkm = data.IntegradoresEmRHKM,
                                        IntegradorEmRhKm = data.IntegradorEmRHKM,
                                        DataIntegraçãoEmRhKm = data.DataIntegracaoEmRHKM,
                                        CustoTotalAjudaCusto = data.CustoTotalAjudaCusto,
                                        CustoTotalHoras = data.CustoTotalHoras,
                                        CustoTotalKm = data.CustoTotalKM,
                                        NumTotalKm = data.NumTotalKM,
                                        Observações = data.Observacoes,
                                        NºResponsável1 = data.Responsavel1No,
                                        NºResponsável2 = data.Responsavel2No,
                                        NºResponsável3 = data.Responsavel3No,
                                        ValidadoresRhKm = data.ValidadoresRHKM,
                                        DataHoraÚltimoEstado = data.DataHoraUltimoEstado,
                                        UtilizadorModificação = User.Identity.Name, //INTEGRAREMRH
                                        DataHoraModificação = DateTime.Now, //INTEGRAREMRH
                                        Eliminada = false,
                                        Intervenientes = data.Intervenientes
                                    }) == null)
                                    {
                                        result.eReasonCode = 7;
                                        result.eMessage = "Ocorreu um erro ao Integrar a Folha de Horas.";
                                    }
                                    else
                                    {
                                        result.eReasonCode = 0;

                                        if (Estado == 1)
                                        {
                                            if (data.IntegradoresEmRHKM.ToLower().Contains(User.Identity.Name.ToLower()))
                                            {
                                                if (data.TipoDeslocacao == 2 && NoRegistoskm > 0)
                                                {
                                                    using (var ctx = new SuchDBContextExtention())
                                                    {
                                                        var parameters = new[]
                                                        {
                                                            new SqlParameter("@NoFH", data.FolhaDeHorasNo),
                                                            new SqlParameter("@NoUtilizador", data.EmpregadoNo),
                                                            new SqlParameter("@NoValidador", User.Identity.Name)
                                                        };
                                                        result.eReasonCode = ctx.execStoredProcedureFH("exec FH_IntegrarEmRHKM @NoFH, @NoUtilizador, @NoValidador", parameters);
                                                    }

                                                    if (result.eReasonCode == 0)
                                                    {
                                                        FolhasDeHoras FHIntegrarRHKM = DBFolhasDeHoras.GetById(data.FolhaDeHorasNo);
                                                        Estado = 2;

                                                        FHIntegrarRHKM.Estado = Estado; //INTEGRAREMRHKM
                                                        FHIntegrarRHKM.IntegradoEmRhkm = true; //INTEGRAREMRHKM
                                                        FHIntegrarRHKM.IntegradorEmRhKm = User.Identity.Name; //INTEGRAREMRHKM
                                                        FHIntegrarRHKM.DataIntegraçãoEmRhKm = DateTime.Now; //INTEGRAREMRHKM
                                                        FHIntegrarRHKM.UtilizadorModificação = User.Identity.Name; //INTEGRAREMRHKM
                                                        FHIntegrarRHKM.DataHoraModificação = DateTime.Now; //INTEGRAREMRHKM

                                                        if (DBFolhasDeHoras.Update(FHIntegrarRHKM) != null)
                                                        {
                                                            result.eReasonCode = 0;
                                                        }
                                                        else
                                                        {
                                                            result.eReasonCode = 31;
                                                            result.eMessage = "Ocorreu um erro ao Integrar km.";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        result.eReasonCode = 101;
                                                        result.eMessage = "Ocorreu um erro no script SQL de Integração KM na Folha de Horas.";
                                                    }
                                                }
                                            }
                                        }

                                        if (Estado == 1)
                                        { 
                                            ErrorHandler approvalResult = new ErrorHandler();

                                            //Approve Movement
                                            MovimentosDeAprovação approvalMovement = DBApprovalMovements.GetAll().Where(x => x.Tipo == 3 && x.CódigoÁreaFuncional == data.CodigoAreaFuncional &&
                                                x.CódigoRegião == data.CodigoRegiao && x.CódigoCentroResponsabilidade == data.CodigoCentroResponsabilidade && x.Número == data.FolhaDeHorasNo &&
                                                x.Estado == 1 && x.Nivel == 2).FirstOrDefault();

                                            if (approvalMovement != null)
                                                approvalResult = ApprovalMovementsManager.ApproveMovement_FH(approvalMovement.NºMovimento, User.Identity.Name);

                                            //Check Approve Status
                                            if (approvalResult.eReasonCode == 353)
                                            {
                                                result.eReasonCode = 100;
                                                result.eMessage = "A Folha de Horas foi aprovada com sucesso.";
                                            }
                                            else if (approvalResult.eReasonCode == 350)
                                            {
                                                result.eReasonCode = 100;
                                                result.eMessage = "A Folha de Horas aprovada com sucesso, encontra-se a aguardar aprovação do nivel seguinte.";
                                            }
                                            else
                                            {
                                                result.eReasonCode = 199;
                                                result.eMessage = "Ocorreu um erro desconhecido ao aprovar a Folha de Horas.";
                                            }
                                        }

                                        if (Estado == 2)
                                        {
                                            //Update Old Movement
                                            MovimentosDeAprovação approvalMovement = DBApprovalMovements.GetAll().Where(x => x.Tipo == 3 && x.CódigoÁreaFuncional == data.CodigoAreaFuncional &&
                                                x.CódigoRegião == data.CodigoRegiao && x.CódigoCentroResponsabilidade == data.CodigoCentroResponsabilidade && x.Número == data.FolhaDeHorasNo &&
                                                x.Estado == 1 && x.Nivel == 2).FirstOrDefault();

                                            if (approvalMovement != null)
                                            {
                                                ApprovalMovementsViewModel ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetById(approvalMovement.NºMovimento));
                                                ApprovalMovement.Status = 2;
                                                ApprovalMovement.DateTimeApprove = DateTime.Now;
                                                ApprovalMovement.DateTimeUpdate = DateTime.Now;
                                                ApprovalMovement.UserUpdate = User.Identity.Name;
                                                ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.Update(DBApprovalMovements.ParseToDatabase(ApprovalMovement)));

                                                //Delete All User Approval Movements
                                                DBUserApprovalMovements.DeleteFromMovementExcept(ApprovalMovement.MovementNo, User.Identity.Name);
                                            }

                                            result.eReasonCode = 100;
                                            result.eMessage = "A Folha de Horas foi aprovada e encerrada com sucesso.";
                                        }

                                        FolhasDeHoras FHFinal = DBFolhasDeHoras.GetById(data.FolhaDeHorasNo);
                                        if (FHFinal.Estado == 1 && FHFinal.TipoDeslocação != 2 && FHFinal.IntegradoEmRh == true)
                                        {
                                            FHFinal.Estado = 2;
                                            DBFolhasDeHoras.Update(FHFinal);
                                        }
                                    }
                                }
                                else
                                {
                                    result.eReasonCode = 101;
                                    result.eMessage = "Ocorreu um erro no script SQL de Integrar RH na Folha de Horas.";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(result);
        }

        [HttpPost]
        //Integra os KM's de uma Folha de Horas
        public JsonResult IntegrarEmRHKMFolhaDeHoras([FromBody] FolhaDeHorasViewModel data)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                if (string.IsNullOrEmpty(data.FolhaDeHorasNo) || string.IsNullOrEmpty(data.EmpregadoNo) || string.IsNullOrEmpty(data.ProjetoNo))
                {
                    result.eReasonCode = 101;
                    result.eMessage = "Faltam preencher algum campo obrigatório na Folha de Horas.";
                }
                else
                {
                    if (data.TipoDeslocacao != 2) //2 = "Viatura Própria")
                    {
                        result.eReasonCode = 101;
                        result.eMessage = "Não é possível Integrar km, devido ao Tipo de Deslocação não ser uma Viatura Própria.";
                    }
                    else
                    {
                        if (data.IntegradoEmRhKm == null ? false : (bool)data.IntegradoEmRhKm)
                        {
                            result.eReasonCode = 101;
                            result.eMessage = "Já foram integradas os km's na Folha de Horas.";
                        }
                        else
                        {
                            if ((int)data.Estado != 1)
                            {
                                result.eReasonCode = 101;
                                result.eMessage = "A Folha de Horas não está num estado possível de Integrar os km's.";
                            }
                            else
                            {
                                if (!data.IntegradoresEmRHKM.ToLower().Contains(User.Identity.Name.ToLower()))
                                {
                                    result.eReasonCode = 101;
                                    result.eMessage = "Não tem permissões para validar a Folha de Horas";
                                }
                                else
                                {
                                    using (var ctx = new SuchDBContextExtention())
                                    {
                                        var parameters = new[]
                                        {
                                            new SqlParameter("@NoFH", data.FolhaDeHorasNo),
                                            new SqlParameter("@NoUtilizador", data.EmpregadoNo),
                                            new SqlParameter("@NoValidador", User.Identity.Name)
                                        };
                                        result.eReasonCode = ctx.execStoredProcedureFH("exec FH_IntegrarEmRHKM @NoFH, @NoUtilizador, @NoValidador", parameters);
                                    }

                                    if (result.eReasonCode == 0)
                                    {
                                        string EmpregadoNome = DBUserConfigurations.GetById(User.Identity.Name).Nome;
                                        bool IntegradoEmRh = (bool)data.IntegradoEmRh;
                                        int Estado = (int)data.Estado;
                                        int NoRegistosAjC = DBLinhasFolhaHoras.GetAll().Where(x => x.NoFolhaHoras.ToLower() == data.FolhaDeHorasNo.ToLower() && x.TipoCusto == 2).Count();

                                        if (IntegradoEmRh == true || NoRegistosAjC == 0)
                                            Estado = 2; // 2 = Registado

                                        if (DBFolhasDeHoras.Update(new FolhasDeHoras()
                                        {
                                            NºFolhaDeHoras = data.FolhaDeHorasNo,
                                            Área = data.Area,
                                            NºProjeto = data.ProjetoNo == "" ? null : data.ProjetoNo,
                                            ProjetoDescricao = data.ProjetoDescricao,
                                            NºEmpregado = data.EmpregadoNo == "" ? null : data.EmpregadoNo,
                                            NomeEmpregado = data.EmpregadoNome == "" ? null : data.EmpregadoNome,
                                            DataHoraPartida = DateTime.Parse(string.Concat(data.DataPartidaTexto, " ", data.HoraPartidaTexto)),
                                            DataHoraChegada = DateTime.Parse(string.Concat(data.DataChegadaTexto, " ", data.HoraChegadaTexto)),
                                            TipoDeslocação = data.TipoDeslocacaoTexto == "" ? 1 : Convert.ToInt32(data.TipoDeslocacaoTexto),
                                            CódigoTipoKmS = data.CodigoTipoKms == "" ? null : data.CodigoTipoKms,
                                            Matrícula = data.Matricula == "" ? null : data.Matricula,
                                            DeslocaçãoForaConcelho = data.DeslocacaoForaConcelho,
                                            DeslocaçãoPlaneada = data.DeslocacaoPlaneada,
                                            Terminada = data.Terminada,
                                            Estado = Estado, //INTEGRAREMRHKM
                                            CriadoPor = data.CriadoPor,
                                            DataHoraCriação = data.DataHoraCriacao,
                                            CódigoRegião = data.CodigoRegiao == "" ? null : data.CodigoRegiao,
                                            CódigoÁreaFuncional = data.CodigoAreaFuncional == "" ? null : data.CodigoAreaFuncional,
                                            CódigoCentroResponsabilidade = data.CodigoCentroResponsabilidade == "" ? null : data.CodigoCentroResponsabilidade,
                                            TerminadoPor = data.TerminadoPor,
                                            DataHoraTerminado = data.DataHoraTerminado,
                                            Validado = data.Validado,
                                            Validadores = data.Validadores == "" ? null : data.Validadores,
                                            Validador = data.Validador,
                                            DataHoraValidação = data.DataHoraValidacao,
                                            IntegradoEmRh = data.IntegradoEmRh,
                                            IntegradoresEmRh = data.IntegradoresEmRH,
                                            IntegradorEmRh = data.IntegradorEmRH,
                                            DataIntegraçãoEmRh = data.DataIntegracaoEmRH,
                                            IntegradoEmRhkm = true, //INTEGRAREMRHKM
                                            IntegradoresEmRhkm = data.IntegradoresEmRHKM,
                                            IntegradorEmRhKm = User.Identity.Name, //INTEGRAREMRHKM
                                            DataIntegraçãoEmRhKm = DateTime.Now, //INTEGRAREMRHKM
                                            CustoTotalAjudaCusto = data.CustoTotalAjudaCusto,
                                            CustoTotalHoras = data.CustoTotalHoras,
                                            CustoTotalKm = data.CustoTotalKM,
                                            NumTotalKm = data.NumTotalKM,
                                            Observações = data.Observacoes,
                                            NºResponsável1 = data.Responsavel1No,
                                            NºResponsável2 = data.Responsavel2No,
                                            NºResponsável3 = data.Responsavel3No,
                                            ValidadoresRhKm = data.ValidadoresRHKM,
                                            DataHoraÚltimoEstado = data.DataHoraUltimoEstado,
                                            UtilizadorModificação = User.Identity.Name, //INTEGRAREMRHKM
                                            DataHoraModificação = DateTime.Now, //INTEGRAREMRHKM
                                            Eliminada = false,
                                            Intervenientes = data.Intervenientes
                                        }) == null)
                                        {
                                            result.eReasonCode = 101;
                                            result.eMessage = "Houve erro na atualização da Folha de Horas.";
                                        }
                                        else
                                        {
                                            if (Estado == 1)
                                            {
                                                if (result.eReasonCode == 0 && data.IntegradoresEmRH.ToLower().Contains(User.Identity.Name.ToLower()))
                                                {
                                                    if (NoRegistosAjC > 0)
                                                    {
                                                        using (var ctxRH = new SuchDBContextExtention())
                                                        {
                                                            var parametersRH = new[]
                                                            {
                                                                new SqlParameter("@NoFH", data.FolhaDeHorasNo),
                                                                new SqlParameter("@NoUtilizador", data.EmpregadoNo),
                                                                new SqlParameter("@NoValidador", User.Identity.Name)
                                                            };
                                                            result.eReasonCode = ctxRH.execStoredProcedureFH("exec FH_IntegrarEmRH @NoFH, @NoUtilizador, @NoValidador", parametersRH);
                                                        }

                                                        if (result.eReasonCode == 0)
                                                        {
                                                            FolhasDeHoras FHIntegrarRH = DBFolhasDeHoras.GetById(data.FolhaDeHorasNo);
                                                            Estado = 2; //REGISTADO

                                                            FHIntegrarRH.Estado = Estado; //INTEGRAREMRH
                                                            FHIntegrarRH.IntegradoEmRh = true; //INTEGRAREMRH
                                                            FHIntegrarRH.IntegradorEmRh = User.Identity.Name; //INTEGRAREMRH
                                                            FHIntegrarRH.DataIntegraçãoEmRh = DateTime.Now; //INTEGRAREMRH
                                                            FHIntegrarRH.UtilizadorModificação = User.Identity.Name; //INTEGRAREMRH
                                                            FHIntegrarRH.DataHoraModificação = DateTime.Now; //INTEGRAREMRH

                                                            if (DBFolhasDeHoras.Update(FHIntegrarRH) != null)
                                                            {
                                                                result.eReasonCode = 0;
                                                            }
                                                            else
                                                            {
                                                                result.eReasonCode = 30;
                                                                result.eMessage = "Ocorreu um erro ao Integrar Ajudas de Custo.";
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            if (result.eReasonCode == 0)
                                            {
                                                if (Estado == 1)
                                                {
                                                    ErrorHandler approvalResult = new ErrorHandler();

                                                    //Approve Movement
                                                    MovimentosDeAprovação approvalMovement = DBApprovalMovements.GetAll().Where(x => x.Tipo == 3 && x.CódigoÁreaFuncional == data.CodigoAreaFuncional &&
                                                        x.CódigoRegião == data.CodigoRegiao && x.CódigoCentroResponsabilidade == data.CodigoCentroResponsabilidade && x.Número == data.FolhaDeHorasNo &&
                                                        x.Estado == 1 && x.Nivel == 3).FirstOrDefault();

                                                    if (approvalMovement != null)
                                                        approvalResult = ApprovalMovementsManager.ApproveMovement_FH(approvalMovement.NºMovimento, User.Identity.Name);

                                                    //Check Approve Status
                                                    if (approvalResult.eReasonCode == 353)
                                                    {
                                                        result.eReasonCode = 100;
                                                        result.eMessage = "A Folha de Horas foi aprovada com sucesso.";
                                                    }
                                                    else if (approvalResult.eReasonCode == 350)
                                                    {
                                                        result.eReasonCode = 100;
                                                        result.eMessage = "A Folha de Horas aprovada com sucesso, encontra-se a aguardar aprovação do nivel seguinte.";
                                                    }
                                                    else
                                                    {
                                                        result.eReasonCode = 199;
                                                        result.eMessage = "Ocorreu um erro desconhecido ao aprovar a Folha de Horas.";
                                                    }
                                                }

                                                if (Estado == 2)
                                                {
                                                    //Update Old Movement
                                                    MovimentosDeAprovação approvalMovement = DBApprovalMovements.GetAll().Where(x => x.Tipo == 3 && x.CódigoÁreaFuncional == data.CodigoAreaFuncional &&
                                                        x.CódigoRegião == data.CodigoRegiao && x.CódigoCentroResponsabilidade == data.CodigoCentroResponsabilidade && x.Número == data.FolhaDeHorasNo &&
                                                        x.Estado == 1 && x.Nivel == 3).FirstOrDefault();

                                                    if (approvalMovement != null)
                                                    {
                                                        ApprovalMovementsViewModel ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetById(approvalMovement.NºMovimento));
                                                        ApprovalMovement.Status = 2;
                                                        ApprovalMovement.DateTimeApprove = DateTime.Now;
                                                        ApprovalMovement.DateTimeUpdate = DateTime.Now;
                                                        ApprovalMovement.UserUpdate = User.Identity.Name;
                                                        ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.Update(DBApprovalMovements.ParseToDatabase(ApprovalMovement)));

                                                        //Delete All User Approval Movements
                                                        DBUserApprovalMovements.DeleteFromMovementExcept(ApprovalMovement.MovementNo, User.Identity.Name);
                                                    }

                                                    result.eReasonCode = 100;
                                                    result.eMessage = "A Folha de Horas foi aprovada e encerrada com sucesso.";
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        result.eReasonCode = 101;
                                        result.eMessage = "Ocorreu um erro no script SQL de Integrar Km na Folha de Horas.";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult EnviarParaAprovacao([FromBody] string FolhaHorasNo)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 99;
            try
            {
                FolhasDeHoras FH = DBFolhasDeHoras.GetById(FolhaHorasNo);

                if (string.IsNullOrEmpty(FolhaHorasNo))
                {
                    result.eReasonCode = 1;
                    result.eMessage = "Não foi possivel ler o Nº da Folha de Horas.";
                }
                else
                {
                    if (FH == null)
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Não foi possível obter a Folha de Horas";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(FH.NºProjeto))
                        {
                            result.eReasonCode = 3;
                            result.eMessage = "Falta preencher o campo Nº Ordem/Projecto.";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(FH.NºEmpregado))
                            {
                                result.eReasonCode = 4;
                                result.eMessage = "Falta preencher o campo Nº Empregado - Nome.";
                            }
                            else
                            {
                                if (FH.DataHoraPartida == null)
                                {
                                    result.eReasonCode = 5;
                                    result.eMessage = "Falta preencher o campo Data/Hora Início.";
                                }
                                else
                                {
                                    if (FH.DataHoraChegada == null)
                                    {
                                        result.eReasonCode = 6;
                                        result.eMessage = "Falta preencher o campo Data/Hora Fim.";
                                    }
                                    else
                                    {
                                        if (FH.TipoDeslocação == null || FH.TipoDeslocação == 0)
                                        {
                                            result.eReasonCode = 7;
                                            result.eMessage = "Falta preencher o campo Tipo Deslocação.";
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(FH.CódigoTipoKmS))
                                            {
                                                result.eReasonCode = 8;
                                                result.eMessage = "Falta preencher o campo Tipo km.";
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(FH.CódigoRegião))
                                                {
                                                    result.eReasonCode = 9;
                                                    result.eMessage = "Falta preencher o campo Região.";
                                                }
                                                else
                                                {
                                                    if (string.IsNullOrEmpty(FH.CódigoÁreaFuncional))
                                                    {
                                                        result.eReasonCode = 10;
                                                        result.eMessage = "Falta preencher o campo Área Funcional.";
                                                    }
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(FH.CódigoCentroResponsabilidade))
                                                        {
                                                            result.eReasonCode = 11;
                                                            result.eMessage = "Falta preencher o campo Centro Responsabilidade.";
                                                        }
                                                        else
                                                        {
                                                            if (string.IsNullOrEmpty(FH.Validadores))
                                                            {
                                                                result.eReasonCode = 12;
                                                                result.eMessage = "Não existem Validadores.";
                                                            }
                                                            else
                                                            {
                                                                if (string.IsNullOrEmpty(FH.IntegradoresEmRh))
                                                                {
                                                                    result.eReasonCode = 13;
                                                                    result.eMessage = "Não existem Integradores Aj. Custo.";
                                                                }
                                                                else
                                                                {
                                                                    if (string.IsNullOrEmpty(FH.IntegradoresEmRhkm))
                                                                    {
                                                                        result.eReasonCode = 14;
                                                                        result.eMessage = "Não existem integradores km RH.";
                                                                    }
                                                                    else
                                                                    {
                                                                        if (FH.DataHoraChegada < FH.DataHoraPartida)
                                                                        {
                                                                            result.eReasonCode = 15;
                                                                            result.eMessage = "A Data/Hora Início tem que Inferior á Data/Hora Fim.";
                                                                        }
                                                                        else
                                                                        {
                                                                            if (FH.DataHoraPartida > DateTime.Now)
                                                                            {
                                                                                result.eReasonCode = 16;
                                                                                result.eMessage = "A Data/Hora Início não pode ser superior á data/hora atual.";
                                                                            }
                                                                            else
                                                                            {
                                                                                if (FH.DataHoraChegada > DateTime.Now)
                                                                                {
                                                                                    result.eReasonCode = 17;
                                                                                    result.eMessage = "A Data/Hora Fim não pode ser superior á data/hora atual.";
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (FH.TipoDeslocação == 1)
                                                                                    {
                                                                                        if (!string.IsNullOrEmpty(FH.Matrícula))
                                                                                        {
                                                                                            result.eReasonCode = 100;
                                                                                            result.eMessage = "Fluxo Iniciado com sucesso";
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            result.eReasonCode = 18;
                                                                                            result.eMessage = "Falta preencher o campo Matrícula.";
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        result.eReasonCode = 100;
                                                                                        result.eMessage = "Fluxo Iniciado com sucesso";
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (result.eReasonCode == 100)
                {
                    FH.Terminada = true; //TERMINADA
                    FH.TerminadoPor = User.Identity.Name; //TERMINADA
                    FH.DataHoraTerminado = DateTime.Now; //TERMINADA
                    FH.UtilizadorModificação = User.Identity.Name; //TERMINADA
                    FH.DataHoraModificação = DateTime.Now; //TERMINADA

                    if (DBFolhasDeHoras.Update(FH) == null)
                    {
                        result.eReasonCode = 19;
                        result.eMessage = "Ocorreu um erro ao atualizar a Folha de Horas.";
                    }
                    else
                    {
                        decimal CustoTotal = (decimal)FH.CustoTotalAjudaCusto;

                        result = ApprovalMovementsManager.StartApprovalMovement_FH(3, FH.CódigoÁreaFuncional, FH.CódigoCentroResponsabilidade, FH.CódigoRegião, CustoTotal, FH.NºFolhaDeHoras, User.Identity.Name);
                    };
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(result);
        }

        //1
        [HttpPost]
        public async Task<JsonResult> ExportToExcel_FolhasHoras([FromBody] List<FolhaDeHorasViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].Colunas;

            string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\Upload\\temp";
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
                ISheet excelSheet = workbook.CreateSheet("Folha de Horas");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["folhaDeHorasNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Folha Horas");
                    Col = Col + 1;
                }
                if (dp["projetoNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Ordem/Projecto");
                    Col = Col + 1;
                }
                if (dp["projetoDescricao"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Projecto Descrição");
                    Col = Col + 1;
                }
                if (dp["empregadoNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Empregado");
                    Col = Col + 1;
                }
                if (dp["empregadoNome"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nome Empregado");
                    Col = Col + 1;
                }
                if (dp["dataPartidaTexto"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Início");
                    Col = Col + 1;
                }
                if (dp["horaPartidaTexto"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Hora Início");
                    Col = Col + 1;
                }
                if (dp["dataChegadaTexto"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Fim");
                    Col = Col + 1;
                }
                if (dp["horaChegadaTexto"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Hora Fim");
                    Col = Col + 1;
                }
                if (dp["tipoDeslocacaoTexto"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo Deslocação");
                    Col = Col + 1;
                }
                if (dp["codigoTipoKms"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo km");
                    Col = Col + 1;
                }
                if (dp["matricula"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Matrícula");
                    Col = Col + 1;
                }
                if (dp["deslocacaoForaConcelhoTexto"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Desloc. Fora do Concelho");
                    Col = Col + 1;
                }
                if (dp["terminadaTexto"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Terminado");
                    Col = Col + 1;
                }
                if (dp["custoTotalAjudaCusto"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Custo Total Ajuda Custo");
                    Col = Col + 1;
                }
                if (dp["custoTotalHoras"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Custo Total Horas");
                    Col = Col + 1;
                }
                if (dp["custoTotalKM"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Custo Total km");
                    Col = Col + 1;
                }
                if (dp["numTotalKM"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Total km");
                    Col = Col + 1;
                }
                if (dp["codigoRegiao"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Região");
                    Col = Col + 1;
                }
                if (dp["codigoAreaFuncional"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Área Funcional");
                    Col = Col + 1;
                }
                if (dp["codigoCentroResponsabilidade"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Centro Respon.");
                    Col = Col + 1;
                }
                if (dp["observacoes"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Observações");
                    Col = Col + 1;
                }

                if (Lista != null)
                {
                    int count = 1;
                    foreach (FolhaDeHorasViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["folhaDeHorasNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.FolhaDeHorasNo);
                            Col = Col + 1;
                        }
                        if (dp["projetoNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProjetoNo);
                            Col = Col + 1;
                        }
                        if (dp["projetoDescricao"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProjetoDescricao);
                            Col = Col + 1;
                        }
                        if (dp["empregadoNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.EmpregadoNo);
                            Col = Col + 1;
                        }
                        if (dp["empregadoNome"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.EmpregadoNome);
                            Col = Col + 1;
                        }
                        if (dp["dataPartidaTexto"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DataPartidaTexto);
                            Col = Col + 1;
                        }
                        if (dp["horaPartidaTexto"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.HoraPartidaTexto);
                            Col = Col + 1;
                        }
                        if (dp["dataChegadaTexto"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DataChegadaTexto);
                            Col = Col + 1;
                        }
                        if (dp["horaChegadaTexto"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.HoraChegadaTexto);
                            Col = Col + 1;
                        }
                        if (dp["tipoDeslocacaoTexto"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TipoDeslocacaoTexto);
                            Col = Col + 1;
                        }
                        if (dp["codigoTipoKms"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodigoTipoKms);
                            Col = Col + 1;
                        }
                        if (dp["matricula"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Matricula);
                            Col = Col + 1;
                        }
                        if (dp["deslocacaoForaConcelhoTexto"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DeslocacaoForaConcelhoTexto);
                            Col = Col + 1;
                        }
                        if (dp["terminadaTexto"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TerminadaTexto);
                            Col = Col + 1;
                        }
                        if (dp["custoTotalAjudaCusto"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CustoTotalAjudaCusto.ToString());
                            Col = Col + 1;
                        }
                        if (dp["custoTotalHoras"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CustoTotalHoras.ToString());
                            Col = Col + 1;
                        }
                        if (dp["custoTotalKM"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CustoTotalKM.ToString());
                            Col = Col + 1;
                        }
                        if (dp["numTotalKM"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.NumTotalKM.ToString());
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
                        if (dp["observacoes"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Observacoes);
                            Col = Col + 1;
                        }

                        count++;
                    }
                }
                workbook.Write(fs);
            }
            //using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            //{
            //    await stream.CopyToAsync(memory);
            //}
            //memory.Position = 0;
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_FolhasHoras(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Folha de Horas.xlsx");
        }

        #endregion


        [HttpPost]
        //Cria uma Ajuda
        public JsonResult GetDistribuicaoPercursos([FromBody] LinhasFolhaHorasViewModel data)
        {
            int result = 99;
            try
            {
                List<DistribuiçãoCustoFolhaDeHoras> Distribuicao = DBDistribuicaoCustoFolhaDeHoras.GetDistribuiçãoByLinha(data.NoFolhaHoras, data.NoLinha);

                if (Distribuicao == null || Distribuicao.Count() == 0)
                {
                    FolhasDeHoras FH = DBFolhasDeHoras.GetById(data.NoFolhaHoras);

                    DistribuiçãoCustoFolhaDeHoras NewDistribuicao = new DistribuiçãoCustoFolhaDeHoras
                    {
                        NºFolhasDeHoras = data.NoFolhaHoras,
                        NºLinhaPercursosEAjudasCustoDespesas = data.NoLinha,
                        TipoObra = data.TipoCusto,
                        NºObra = FH.NºProjeto,
                        PercentagemValor = 100,
                        Valor = data.CustoTotal,
                        TotalValor = data.CustoTotal,
                        TotalPercentagemValor = 100,
                        KmTotais = data.Distancia,
                        KmDistancia = data.Distancia,
                        Quantidade = 0,
                        CódigoRegião = data.CodRegiao,
                        CódigoÁreaFuncional = data.CodArea,
                        CódigoCentroResponsabilidade = data.CodCresp,
                        DataHoraCriação = DateTime.Now,
                        UtilizadorCriação = User.Identity.Name
                    };

                    if (DBDistribuicaoCustoFolhaDeHoras.Create(NewDistribuicao) != null)
                    {
                        return Json(DBDistribuicaoCustoFolhaDeHoras.ParseToViewModel(NewDistribuicao));
                    }
                }

                return Json(DBDistribuicaoCustoFolhaDeHoras.ParseToViewModel(Distribuicao));
            }
            catch (Exception ex)
            {
                result = 99;
            }

            return Json(null);
        }

    }
}
