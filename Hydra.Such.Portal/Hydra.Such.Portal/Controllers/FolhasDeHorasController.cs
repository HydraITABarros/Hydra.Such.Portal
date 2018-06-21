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

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class FolhasDeHorasController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;

        public FolhasDeHorasController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
        }

        #region Home
        public IActionResult Index()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Engenharia, Enumerations.Features.FolhasHoras); //1, 6);

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

        public IActionResult FolhaDeHoras_Validacao(string folhaDeHoraNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Engenharia, Enumerations.Features.FolhasHoras);

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
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Engenharia, Enumerations.Features.FolhasHoras);

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
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Engenharia, Enumerations.Features.FolhasHoras);

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
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Engenharia, Enumerations.Features.FolhasHoras);

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

        [HttpPost]
        //Listagem das Folhas de Horas consoante o estado
        public JsonResult GetListFolhasDeHoras([FromBody] HTML_FHViewModel HTML)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Engenharia, Enumerations.Features.FolhasHoras); //1, 6);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                if (HTML.historico == 1)
                {
                    HTML.estado = 2;
                    HTML.validacao = 0;
                    HTML.integracaoajuda = 0;
                    HTML.integracaokms = 0;
                }
                else
                {
                    if (HTML.validacao == 1)
                    {
                        HTML.estado = 0;
                        HTML.validacao = 1;
                        HTML.integracaoajuda = 0;
                        HTML.integracaokms = 0;
                    }
                    else
                    {
                        if (HTML.integracaoajuda == 1)
                        {
                            HTML.estado = 1;
                            HTML.validacao = 0;
                            HTML.integracaoajuda = 1;
                            HTML.integracaokms = 0;
                        }
                        else
                        {
                            if (HTML.integracaokms == 1)
                            {
                                HTML.estado = 1;
                                HTML.validacao = 0;
                                HTML.integracaoajuda = 0;
                                HTML.integracaokms = 1;
                            }
                            else
                            {
                                if (HTML.estado == 1)
                                {
                                    HTML.estado = 1;
                                    HTML.validacao = 0;
                                    HTML.integracaoajuda = 0;
                                    HTML.integracaokms = 0;
                                }
                            }
                        }
                    }
                }

                if (HTML.validacao == 1)
                {
                    List<FolhaDeHorasViewModel> result = DBFolhasDeHoras.GetAllByValidacao(_config.NAVDatabaseName, _config.NAVCompanyName, User.Identity.Name, HTML.estado);
                    if (result != null)
                    {
                        result.ForEach(FH =>
                        {
                            //FH.AreaTexto = FH.Area == null ? "" : EnumerablesFixed.Areas.Where(y => y.Id == FH.Area).FirstOrDefault().Value;
                            FH.TipoDeslocacaoTexto = FH.TipoDeslocacao == null ? "" : EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == FH.TipoDeslocacao).FirstOrDefault().Value;
                            FH.CodigoTipoKms = FH.CodigoTipoKms == null ? "" : DBTabelaConfRecursosFh.GetDescricaoByRecurso("1", FH.CodigoTipoKms);
                            FH.DeslocacaoForaConcelho = FH.DeslocacaoForaConcelho == null ? false : FH.DeslocacaoForaConcelho;
                            FH.DeslocacaoForaConcelhoTexto = FH.DeslocacaoForaConcelho == null ? "" : FH.DeslocacaoForaConcelho == false ? "Não" : "Sim";
                            FH.Terminada = FH.Terminada == null ? false : FH.Terminada;
                            FH.TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada == false ? "Não" : "Sim";
                            //FH.Estadotexto = FH.Estado == null ? "" : EnumerablesFixed.FolhaDeHoraStatus.Where(y => y.Id == FH.Estado).FirstOrDefault().Value;
                            //FH.CriadoPor = FH.CriadoPor == null ? "" : DBUserConfigurations.GetById(FH.CriadoPor).Nome;
                            //FH.CodigoRegiao = FH.CodigoRegiao == null ? "" : FH.CodigoRegiao + " - " + DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name, FH.CodigoRegiao).FirstOrDefault().Name;
                            //FH.CodigoAreaFuncional = FH.CodigoAreaFuncional == null ? "" : FH.CodigoAreaFuncional + " - " + DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 2, User.Identity.Name, FH.CodigoAreaFuncional).FirstOrDefault().Name;
                            //FH.CodigoCentroResponsabilidade = FH.CodigoCentroResponsabilidade == null ? "" : FH.CodigoCentroResponsabilidade + " - " + DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 3, User.Identity.Name, FH.CodigoCentroResponsabilidade).FirstOrDefault().Name;
                            //FH.TerminadoPor = FH.TerminadoPor == null ? "" : DBUserConfigurations.GetById(FH.TerminadoPor).Nome;
                            //FH.Validado = FH.Validado == null ? false : FH.Validado;
                            //FH.ValidadoTexto = FH.Validado == null ? "" : FH.Validado == false ? "Não" : "Sim";
                            //FH.Validador = FH.Validador == null ? "" : DBUserConfigurations.GetById(FH.Validador).Nome;
                            //FH.IntegradorEmRH = FH.IntegradorEmRH == null ? "" : DBUserConfigurations.GetById(FH.IntegradorEmRH).Nome;
                            //FH.IntegradorEmRHKM = FH.IntegradorEmRHKM == null ? "" : DBUserConfigurations.GetById(FH.IntegradorEmRHKM).Nome;
                        });
                    }

                    return Json(result.OrderByDescending(x => x.FolhaDeHorasNo));
                }
                else
                {
                    if (HTML.integracaoajuda == 1)
                    {
                        List<FolhaDeHorasViewModel> result = DBFolhasDeHoras.GetAllByIntegracaoAjuda(_config.NAVDatabaseName, _config.NAVCompanyName, User.Identity.Name, HTML.estado);
                        if (result != null)
                        {
                            result.ForEach(FH =>
                            {
                                //FH.AreaTexto = FH.Area == null ? "" : EnumerablesFixed.Areas.Where(y => y.Id == FH.Area).FirstOrDefault().Value;
                                FH.TipoDeslocacaoTexto = FH.TipoDeslocacao == null ? "" : EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == FH.TipoDeslocacao).FirstOrDefault().Value;
                                FH.CodigoTipoKms = FH.CodigoTipoKms == null ? "" : DBTabelaConfRecursosFh.GetDescricaoByRecurso("1", FH.CodigoTipoKms);
                                FH.DeslocacaoForaConcelho = FH.DeslocacaoForaConcelho == null ? false : FH.DeslocacaoForaConcelho;
                                FH.DeslocacaoForaConcelhoTexto = FH.DeslocacaoForaConcelho == null ? "" : FH.DeslocacaoForaConcelho == false ? "Não" : "Sim";
                                FH.Terminada = FH.Terminada == null ? false : FH.Terminada;
                                FH.TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada == false ? "Não" : "Sim";
                                //FH.Estadotexto = FH.Estado == null ? "" : EnumerablesFixed.FolhaDeHoraStatus.Where(y => y.Id == FH.Estado).FirstOrDefault().Value;
                                //FH.CriadoPor = FH.CriadoPor == null ? "" : DBUserConfigurations.GetById(FH.CriadoPor).Nome;
                                //FH.CodigoRegiao = FH.CodigoRegiao == null ? "" : FH.CodigoRegiao + " - " + DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name, FH.CodigoRegiao).FirstOrDefault().Name;
                                //FH.CodigoAreaFuncional = FH.CodigoAreaFuncional == null ? "" : FH.CodigoAreaFuncional + " - " + DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 2, User.Identity.Name, FH.CodigoAreaFuncional).FirstOrDefault().Name;
                                //FH.CodigoCentroResponsabilidade = FH.CodigoCentroResponsabilidade == null ? "" : FH.CodigoCentroResponsabilidade + " - " + DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 3, User.Identity.Name, FH.CodigoCentroResponsabilidade).FirstOrDefault().Name;
                                //FH.TerminadoPor = FH.TerminadoPor == null ? "" : DBUserConfigurations.GetById(FH.TerminadoPor).Nome;
                                //FH.Validado = FH.Validado == null ? false : FH.Validado;
                                //FH.ValidadoTexto = FH.Validado == null ? "" : FH.Validado == false ? "Não" : "Sim";
                                //FH.Validador = FH.Validador == null ? "" : DBUserConfigurations.GetById(FH.Validador).Nome;
                                //FH.IntegradorEmRH = FH.IntegradorEmRH == null ? "" : DBUserConfigurations.GetById(FH.IntegradorEmRH).Nome;
                                //FH.IntegradorEmRHKM = FH.IntegradorEmRHKM == null ? "" : DBUserConfigurations.GetById(FH.IntegradorEmRHKM).Nome;
                            });
                        }

                        return Json(result.OrderByDescending(x => x.FolhaDeHorasNo));
                    }
                    else
                    {
                        if (HTML.integracaokms == 1)
                        {
                            List<FolhaDeHorasViewModel> result = DBFolhasDeHoras.GetAllByIntegracaoKMS(_config.NAVDatabaseName, _config.NAVCompanyName, User.Identity.Name, HTML.estado);
                            if (result != null)
                            {
                                result.ForEach(FH =>
                                {
                                    //FH.AreaTexto = FH.Area == null ? "" : EnumerablesFixed.Areas.Where(y => y.Id == FH.Area).FirstOrDefault().Value;
                                    FH.TipoDeslocacaoTexto = FH.TipoDeslocacao == null ? "" : EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == FH.TipoDeslocacao).FirstOrDefault().Value;
                                    FH.CodigoTipoKms = FH.CodigoTipoKms == null ? "" : DBTabelaConfRecursosFh.GetDescricaoByRecurso("1", FH.CodigoTipoKms);
                                    FH.DeslocacaoForaConcelho = FH.DeslocacaoForaConcelho == null ? false : FH.DeslocacaoForaConcelho;
                                    FH.DeslocacaoForaConcelhoTexto = FH.DeslocacaoForaConcelho == null ? "" : FH.DeslocacaoForaConcelho == false ? "Não" : "Sim";
                                    FH.Terminada = FH.Terminada == null ? false : FH.Terminada;
                                    FH.TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada == false ? "Não" : "Sim";
                                    //FH.Estadotexto = FH.Estado == null ? "" : EnumerablesFixed.FolhaDeHoraStatus.Where(y => y.Id == FH.Estado).FirstOrDefault().Value;
                                    //FH.CriadoPor = FH.CriadoPor == null ? "" : DBUserConfigurations.GetById(FH.CriadoPor).Nome;
                                    //FH.CodigoRegiao = FH.CodigoRegiao == null ? "" : FH.CodigoRegiao + " - " + DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name, FH.CodigoRegiao).FirstOrDefault().Name;
                                    //FH.CodigoAreaFuncional = FH.CodigoAreaFuncional == null ? "" : FH.CodigoAreaFuncional + " - " + DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 2, User.Identity.Name, FH.CodigoAreaFuncional).FirstOrDefault().Name;
                                    //FH.CodigoCentroResponsabilidade = FH.CodigoCentroResponsabilidade == null ? "" : FH.CodigoCentroResponsabilidade + " - " + DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 3, User.Identity.Name, FH.CodigoCentroResponsabilidade).FirstOrDefault().Name;
                                    //FH.TerminadoPor = FH.TerminadoPor == null ? "" : DBUserConfigurations.GetById(FH.TerminadoPor).Nome;
                                    //FH.Validado = FH.Validado == null ? false : FH.Validado;
                                    //FH.ValidadoTexto = FH.Validado == null ? "" : FH.Validado == false ? "Não" : "Sim";
                                    //FH.Validador = FH.Validador == null ? "" : DBUserConfigurations.GetById(FH.Validador).Nome;
                                    //FH.IntegradorEmRH = FH.IntegradorEmRH == null ? "" : DBUserConfigurations.GetById(FH.IntegradorEmRH).Nome;
                                    //FH.IntegradorEmRHKM = FH.IntegradorEmRHKM == null ? "" : DBUserConfigurations.GetById(FH.IntegradorEmRHKM).Nome;
                                });
                            }

                            return Json(result.OrderByDescending(x => x.FolhaDeHorasNo));
                        }
                        else
                        {
                            if (HTML.estado == 1)
                            {
                                List<FolhaDeHorasViewModel> result = DBFolhasDeHoras.GetAllByHistorico(_config.NAVDatabaseName, _config.NAVCompanyName, User.Identity.Name, HTML.estado);
                                if (result != null)
                                {
                                    result.ForEach(FH =>
                                    {
                                        //FH.AreaTexto = FH.Area == null ? "" : EnumerablesFixed.Areas.Where(y => y.Id == FH.Area).FirstOrDefault().Value;
                                        FH.TipoDeslocacaoTexto = FH.TipoDeslocacao == null ? "" : EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == FH.TipoDeslocacao).FirstOrDefault().Value;
                                        FH.CodigoTipoKms = FH.CodigoTipoKms == null ? "" : DBTabelaConfRecursosFh.GetDescricaoByRecurso("1", FH.CodigoTipoKms);
                                        FH.DeslocacaoForaConcelho = FH.DeslocacaoForaConcelho == null ? false : FH.DeslocacaoForaConcelho;
                                        FH.DeslocacaoForaConcelhoTexto = FH.DeslocacaoForaConcelho == null ? "" : FH.DeslocacaoForaConcelho == false ? "Não" : "Sim";
                                        FH.Terminada = FH.Terminada == null ? false : FH.Terminada;
                                        FH.TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada == false ? "Não" : "Sim";
                                        //FH.Estadotexto = FH.Estado == null ? "" : EnumerablesFixed.FolhaDeHoraStatus.Where(y => y.Id == FH.Estado).FirstOrDefault().Value;
                                        //FH.CriadoPor = FH.CriadoPor == null ? "" : DBUserConfigurations.GetById(FH.CriadoPor).Nome;
                                        //FH.CodigoRegiao = FH.CodigoRegiao == null ? "" : FH.CodigoRegiao + " - " + DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name, FH.CodigoRegiao).FirstOrDefault().Name;
                                        //FH.CodigoAreaFuncional = FH.CodigoAreaFuncional == null ? "" : FH.CodigoAreaFuncional + " - " + DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 2, User.Identity.Name, FH.CodigoAreaFuncional).FirstOrDefault().Name;
                                        //FH.CodigoCentroResponsabilidade = FH.CodigoCentroResponsabilidade == null ? "" : FH.CodigoCentroResponsabilidade + " - " + DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 3, User.Identity.Name, FH.CodigoCentroResponsabilidade).FirstOrDefault().Name;
                                        //FH.TerminadoPor = FH.TerminadoPor == null ? "" : DBUserConfigurations.GetById(FH.TerminadoPor).Nome;
                                        //FH.Validado = FH.Validado == null ? false : FH.Validado;
                                        //FH.ValidadoTexto = FH.Validado == null ? "" : FH.Validado == false ? "Não" : "Sim";
                                        //FH.Validador = FH.Validador == null ? "" : DBUserConfigurations.GetById(FH.Validador).Nome;
                                        //FH.IntegradorEmRH = FH.IntegradorEmRH == null ? "" : DBUserConfigurations.GetById(FH.IntegradorEmRH).Nome;
                                        //FH.IntegradorEmRHKM = FH.IntegradorEmRHKM == null ? "" : DBUserConfigurations.GetById(FH.IntegradorEmRHKM).Nome;
                                    });
                                }

                                return Json(result.OrderByDescending(x => x.FolhaDeHorasNo));
                            }
                            else
                            {
                                List<FolhaDeHorasViewModel> result = DBFolhasDeHoras.GetAllByDimensions(_config.NAVDatabaseName, _config.NAVCompanyName, User.Identity.Name, HTML.estado);
                                if (result != null)
                                {
                                    result.ForEach(FH =>
                                    {
                                        //FH.AreaTexto = FH.Area == null ? "" : EnumerablesFixed.Areas.Where(y => y.Id == FH.Area).FirstOrDefault().Value;
                                        FH.TipoDeslocacaoTexto = FH.TipoDeslocacao == null ? "" : EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == FH.TipoDeslocacao).FirstOrDefault().Value;
                                        FH.CodigoTipoKms = FH.CodigoTipoKms == null ? "" : DBTabelaConfRecursosFh.GetDescricaoByRecurso("1", FH.CodigoTipoKms);
                                        FH.DeslocacaoForaConcelho = FH.DeslocacaoForaConcelho == null ? false : FH.DeslocacaoForaConcelho;
                                        FH.DeslocacaoForaConcelhoTexto = FH.DeslocacaoForaConcelho == null ? "" : FH.DeslocacaoForaConcelho == false ? "Não" : "Sim";
                                        FH.Terminada = FH.Terminada == null ? false : FH.Terminada;
                                        FH.TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada == false ? "Não" : "Sim";
                                        //FH.Estadotexto = FH.Estado == null ? "" : EnumerablesFixed.FolhaDeHoraStatus.Where(y => y.Id == FH.Estado).FirstOrDefault().Value;
                                        //FH.CriadoPor = FH.CriadoPor == null ? "" : DBUserConfigurations.GetById(FH.CriadoPor).Nome;
                                        //FH.CodigoRegiao = FH.CodigoRegiao == null ? "" : FH.CodigoRegiao + " - " + DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name, FH.CodigoRegiao).FirstOrDefault().Name;
                                        //FH.CodigoAreaFuncional = FH.CodigoAreaFuncional == null ? "" : FH.CodigoAreaFuncional + " - " + DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 2, User.Identity.Name, FH.CodigoAreaFuncional).FirstOrDefault().Name;
                                        //FH.CodigoCentroResponsabilidade = FH.CodigoCentroResponsabilidade == null ? "" : FH.CodigoCentroResponsabilidade + " - " + DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 3, User.Identity.Name, FH.CodigoCentroResponsabilidade).FirstOrDefault().Name;
                                        //FH.TerminadoPor = FH.TerminadoPor == null ? "" : DBUserConfigurations.GetById(FH.TerminadoPor).Nome;
                                        //FH.Validado = FH.Validado == null ? false : FH.Validado;
                                        //FH.ValidadoTexto = FH.Validado == null ? "" : FH.Validado == false ? "Não" : "Sim";
                                        //FH.Validador = FH.Validador == null ? "" : DBUserConfigurations.GetById(FH.Validador).Nome;
                                        //FH.IntegradorEmRH = FH.IntegradorEmRH == null ? "" : DBUserConfigurations.GetById(FH.IntegradorEmRH).Nome;
                                        //FH.IntegradorEmRHKM = FH.IntegradorEmRHKM == null ? "" : DBUserConfigurations.GetById(FH.IntegradorEmRHKM).Nome;
                                    });
                                }

                                return Json(result.OrderByDescending(x => x.FolhaDeHorasNo));
                            }
                        }
                    }
                }
            }

            return Json(null);
        }
        #endregion

        #region Details
        //Criação da Folha de Horas
        public ActionResult Detalhes([FromQuery] string FHNo, [FromQuery] int area)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Engenharia, Enumerations.Features.FolhasHoras); //1, 6);

            ViewBag.reportServerURL = _config.ReportServerURL;
            ViewBag.userLogin = User.Identity.Name.ToString();

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;

                if (FHNo == null || FHNo == "")
                {
                    string id = "";

                    //Get Folha de Horas Numeration
                    Configuração Configs = DBConfigurations.GetById(1);
                    int FolhaDeHorasNumerationConfigurationId = Configs.NumeraçãoFolhasDeHoras.Value;
                    id = DBNumerationConfigurations.GetNextNumeration(FolhaDeHorasNumerationConfigurationId, true);

                    //Update Last Numeration Used
                    ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(FolhaDeHorasNumerationConfigurationId);
                    ConfigNumerations.ÚltimoNºUsado = id;
                    DBNumerationConfigurations.Update(ConfigNumerations);

                    FolhasDeHoras FH = new FolhasDeHoras()
                    {
                        NºFolhaDeHoras = id
                    };

                    FH.Área = area;
                    FH.NºEmpregado = DBUserConfigurations.GetById(User.Identity.Name).EmployeeNo == null ? "" : DBUserConfigurations.GetById(User.Identity.Name).EmployeeNo;
                    FH.NomeEmpregado = DBUserConfigurations.GetById(User.Identity.Name).EmployeeNo == null ? "" : DBUserConfigurations.GetById(User.Identity.Name).Nome;
                    FH.TipoDeslocação = 0;
                    FH.CódigoTipoKmS = "KM";
                    FH.DeslocaçãoForaConcelho = false;
                    FH.Terminada = false;
                    FH.Estado = 0;
                    FH.CriadoPor = User.Identity.Name;
                    FH.DataHoraCriação = DateTime.Now;
                    FH.CódigoRegião = DBUserConfigurations.GetById(User.Identity.Name).RegiãoPorDefeito == null ? "" : DBUserConfigurations.GetById(User.Identity.Name).RegiãoPorDefeito;
                    FH.CódigoÁreaFuncional = DBUserConfigurations.GetById(User.Identity.Name).AreaPorDefeito == null ? "" : DBUserConfigurations.GetById(User.Identity.Name).AreaPorDefeito;
                    FH.CódigoCentroResponsabilidade = DBUserConfigurations.GetById(User.Identity.Name).CentroRespPorDefeito == null ? "" : DBUserConfigurations.GetById(User.Identity.Name).CentroRespPorDefeito;
                    FH.Validado = false;
                    FH.IntegradoEmRh = false;
                    FH.IntegradoEmRhkm = false;
                    FH.CustoTotalAjudaCusto = 0;
                    FH.CustoTotalHoras = 0;
                    FH.CustoTotalKm = 0;
                    FH.NumTotalKm = 0;

                    AutorizacaoFhRh Autorizacao = DBAutorizacaoFHRH.GetAll().Where(x => x.NoEmpregado.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();

                    if (Autorizacao != null)
                    {
                        FH.Validadores = Autorizacao.NoResponsavel1 + " - " + Autorizacao.NoResponsavel2 + " - " + Autorizacao.NoResponsavel3;
                        FH.IntegradoresEmRh = Autorizacao.ValidadorRh1 + " - " + Autorizacao.ValidadorRh2 + " - " + Autorizacao.ValidadorRh3;
                        FH.IntegradoresEmRhkm = Autorizacao.ValidadorRhkm1 + " - " + Autorizacao.ValidadorRhkm2;

                        FH.NºResponsável1 = Autorizacao.NoResponsavel1;
                        FH.NºResponsável2 = Autorizacao.NoResponsavel2;
                        FH.NºResponsável3 = Autorizacao.NoResponsavel3;
                    };

                    FH.UtilizadorModificação = User.Identity.Name;
                    FH.DataHoraModificação = DateTime.Now;

                    DBFolhasDeHoras.Create(FH);

                    FHNo = FH.NºFolhaDeHoras;
                }

                ViewBag.FolhaDeHorasNo = FHNo == null ? "" : FHNo;

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
                            HoraModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("HH:mm")
                        };


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
                            DataDespesaTexto = Percurso.DataDespesa.Value.ToString("yyyy-MM-dd"),
                            Funcionario = Percurso.Funcionario,
                            CodRegiao = Percurso.CodRegiao,
                            CodArea = Percurso.CodArea,
                            CodCresp = Percurso.CodCresp,
                            CalculoAutomatico = Percurso.CalculoAutomatico,
                            Matricula = Percurso.Matricula,
                            UtilizadorCriacao = Percurso.UtilizadorCriacao,
                            DataHoraCriacao = Percurso.DataHoraCriacao,
                            DataHoraCriacaoTexto = Percurso.DataHoraCriacao.Value.ToString("yyyy-MM-dd"),
                            UtilizadorModificacao = Percurso.UtilizadorModificacao,
                            DataHoraModificacao = Percurso.DataHoraModificacao,
                            DataHoraModificacaoTexto = Percurso.DataHoraModificacao.Value.ToString("yyyy-MM-dd")
                        }).ToList();

                        //AJUDA DE CUSTO/DESPESA
                        result.FolhaDeHorasAjuda = DBLinhasFolhaHoras.GetAllByAjudaToList(data.FolhaDeHorasNo).Select(Ajuda => new LinhasFolhaHorasViewModel()
                        {
                            NoFolhaHoras = Ajuda.NoFolhaHoras,
                            NoLinha = Ajuda.NoLinha,
                            TipoCusto = Ajuda.TipoCusto,
                            CodTipoCusto = Ajuda.CodTipoCusto,
                            DescricaoTipoCusto = Ajuda.DescricaoTipoCusto,
                            DescricaoCodTipoCusto = Ajuda.CodTipoCusto + " - " + DBTabelaConfRecursosFh.GetAll().Where(y => y.CodRecurso == Ajuda.CodTipoCusto).FirstOrDefault().Descricao,
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
                            DataDespesaTexto = Ajuda.DataDespesa.Value.ToString("yyyy-MM-dd"),
                            Funcionario = Ajuda.Funcionario,
                            CodRegiao = Ajuda.CodRegiao,
                            CodArea = Ajuda.CodArea,
                            CodCresp = Ajuda.CodCresp,
                            CalculoAutomatico = Ajuda.CalculoAutomatico,
                            Matricula = Ajuda.Matricula,
                            UtilizadorCriacao = Ajuda.UtilizadorCriacao,
                            DataHoraCriacao = Ajuda.DataHoraCriacao,
                            DataHoraCriacaoTexto = Ajuda.DataHoraCriacao.Value.ToString("yyyy-MM-dd"),
                            UtilizadorModificacao = Ajuda.UtilizadorModificacao,
                            DataHoraModificacao = Ajuda.DataHoraModificacao,
                            DataHoraModificacaoTexto = Ajuda.DataHoraModificacao.Value.ToString("yyyy-MM-dd")
                        }).ToList();

                        //MÃO-DE-OBRA
                        result.FolhaDeHorasMaoDeObra = DBMaoDeObraFolhaDeHoras.GetAllByMaoDeObraToList(data.FolhaDeHorasNo).Select(MaoDeObra => new MaoDeObraFolhaDeHorasViewModel()
                        {
                            FolhaDeHorasNo = MaoDeObra.FolhaDeHorasNo,
                            LinhaNo = MaoDeObra.LinhaNo,
                            Date = MaoDeObra.Date,
                            DateTexto = MaoDeObra.Date.Value.ToString("yyyy-MM-dd"),
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
                            DataHoraCriacaoTexto = MaoDeObra.DataHoraCriacao.Value.ToString("yyyy-MM-dd"),
                            UtilizadorModificacao = MaoDeObra.UtilizadorModificacao,
                            DataHoraModificacao = MaoDeObra.DataHoraModificacao,
                            DataHoraModificacaoTexto = MaoDeObra.DataHoraModificacao.Value.ToString("yyyy-MM-dd")
                        }).ToList();

                        //PRESENÇA
                        result.FolhaDeHorasPresenca = DBPresencasFolhaDeHoras.GetAllByPresencaToList(data.FolhaDeHorasNo).Select(Presenca => new PresencasFolhaDeHorasViewModel()
                        {
                            FolhaDeHorasNo = Presenca.FolhaDeHorasNo,
                            Data = Presenca.Data,
                            DataTexto = Presenca.Data.Value.ToString("yyyy-MM-dd"),
                            Hora1Entrada = Convert.ToDateTime(string.Concat("1900/01/01 " + Presenca.Hora1Entrada)).ToShortTimeString(),
                            Hora1Saida = Convert.ToDateTime(string.Concat("1900/01/01 " + Presenca.Hora1Saida)).ToShortTimeString(),
                            Hora2Entrada = Convert.ToDateTime(string.Concat("1900/01/01 " + Presenca.Hora2Entrada)).ToShortTimeString(),
                            Hora2Saida = Convert.ToDateTime(string.Concat("1900/01/01 " + Presenca.Hora2Saida)).ToShortTimeString(),
                            Observacoes = Presenca.Observacoes,
                            UtilizadorCriacao = Presenca.UtilizadorCriacao,
                            DataHoraCriacao = Presenca.DataHoraCriacao,
                            DataHoraCriacaoTexto = Presenca.DataHoraCriacao.Value.ToString("yyyy-MM-dd"),
                            UtilizadorModificacao = Presenca.UtilizadorModificacao,
                            DataHoraModificacao = Presenca.DataHoraModificacao,
                            DataHoraModificacaoTexto = Presenca.DataHoraModificacao.Value.ToString("yyyy-MM-dd")
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
                //FolhaDeHorasViewModel FH = new FolhaDeHorasViewModel();

                if (!string.IsNullOrEmpty(folhaDeHorasNo))
                {
                    FH = DBFolhasDeHoras.GetListaValidadoresIntegradores(folhaDeHorasNo, idEmployee);
                    FH.EmpregadoNome = DBNAV2009Employees.GetAll(idEmployee, _config.NAV2009DatabaseName, _config.NAV2009CompanyName).FirstOrDefault().Name;
                }

                //AutorizacaoFhRh Autorizacao = DBAutorizacaoFHRH.GetAll().Where(x => x.NoEmpregado.ToLower() == idEmployeePortal.ToLower()).FirstOrDefault();

                //if (Autorizacao != null)
                //{
                //    FH.Responsavel1No = Autorizacao.NoResponsavel1;
                //    FH.Responsavel2No = Autorizacao.NoResponsavel2;
                //    FH.Responsavel3No = Autorizacao.NoResponsavel3;
                //    FH.Validadores = Autorizacao.NoResponsavel1 + " - " + Autorizacao.NoResponsavel2 + " - " + Autorizacao.NoResponsavel3;
                //    FH.IntegradoresEmRH = Autorizacao.ValidadorRh1 + " - " + Autorizacao.ValidadorRh2 + " - " + Autorizacao.ValidadorRh3;
                //    FH.IntegradoresEmRHKM = Autorizacao.ValidadorRhkm1 + " - " + Autorizacao.ValidadorRhkm2;
                //};
            }


            return Json(FH);
        }

        //[HttpPost]
        //public JsonResult ValidateNumeration([FromBody] FolhaDeHorasViewModel data)
        //{
        //    //Get FolhaDeHora Numeration
        //    Configuração Cfg = DBConfigurations.GetById(1);
        //    int FolhaDeHoraNumerationConfigurationId = Cfg.NumeraçãoFolhasDeHoras.Value;

        //    ConfiguraçãoNumerações CfgNumeration = DBNumerationConfigurations.GetById(FolhaDeHoraNumerationConfigurationId);

        //    //Validate if FolhaDeHorasNo is valid
        //    if (data.FolhaDeHorasNo != "" && !CfgNumeration.Manual.Value)
        //    {
        //        return Json("A numeração configurada para folha de horas não permite inserção manual.");
        //    }
        //    else if (data.FolhaDeHorasNo == "" && !CfgNumeration.Automático.Value)
        //    {
        //        return Json("É obrigatório inserir o Nº de Folha de Horas.");
        //    }

        //    return Json("");
        //}

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
                                    //if (percurso.DataDespesa > FH_DataHoraFim)
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

        //[HttpPost]
        //public JsonResult CreateFolhaDeHora([FromBody] FolhaDeHorasViewModel data)
        //{
        //    try
        //    {
        //        if (data != null)
        //        {
        //            //Get FolhaDeHora Numeration
        //            Configuração Configs = DBConfigurations.GetById(1);
        //            int FolhaDeHoraNumerationConfigurationId = Configs.NumeraçãoFolhasDeHoras.Value;
        //            data.FolhaDeHorasNo = DBNumerationConfigurations.GetNextNumeration(FolhaDeHoraNumerationConfigurationId, true);

        //            FolhasDeHoras cFolhaDeHora = new FolhasDeHoras()
        //            {
        //                NºFolhaDeHoras = data.FolhaDeHorasNo,
        //                Área = data.Area,
        //                NºProjeto = data.ProjetoNo,
        //                ProjetoDescricao = data.ProjetoDescricao,
        //                NºEmpregado = data.EmpregadoNo,
        //                NomeEmpregado = data.EmpregadoNome,
        //                DataHoraPartida = DateTime.Parse(string.Concat(data.DataPartidaTexto, " ", data.HoraPartidaTexto)),
        //                DataHoraChegada = DateTime.Parse(string.Concat(data.DataChegadaTexto, " ", data.HoraChegadaTexto)),
        //                TipoDeslocação = data.TipoDeslocacao,
        //                CódigoTipoKmS = data.CodigoTipoKms,
        //                Matrícula = data.Matricula,
        //                DeslocaçãoForaConcelho = Convert.ToBoolean(data.DeslocacaoForaConcelhoTexto),
        //                DeslocaçãoPlaneada = Convert.ToBoolean(data.DeslocacaoPlaneadaTexto),
        //                Terminada = data.Terminada,
        //                Estado = 0, // 0 = Criado
        //                CriadoPor = User.Identity.Name,
        //                DataHoraCriação = DateTime.Now,
        //                CódigoRegião = data.CodigoRegiao,
        //                CódigoÁreaFuncional = data.CodigoAreaFuncional,
        //                CódigoCentroResponsabilidade = data.CodigoCentroResponsabilidade,
        //                TerminadoPor = User.Identity.Name,
        //                DataHoraTerminado = DateTime.Now,
        //                Validado = Convert.ToBoolean(data.ValidadoTexto),
        //                Validadores = User.Identity.Name,
        //                Validador = User.Identity.Name,
        //                DataHoraValidação = DateTime.Now,
        //                IntegradorEmRh = User.Identity.Name,
        //                IntegradoresEmRh = data.IntegradoresEmRH,
        //                DataIntegraçãoEmRh = DateTime.Now,
        //                IntegradorEmRhKm = User.Identity.Name,
        //                IntegradoresEmRhkm = data.IntegradoresEmRHKM,
        //                DataIntegraçãoEmRhKm = DateTime.Now,
        //                CustoTotalAjudaCusto = data.CustoTotalAjudaCusto,
        //                CustoTotalHoras = data.CustoTotalHoras,
        //                CustoTotalKm = data.CustoTotalKM,
        //                NumTotalKm = data.NumTotalKM,
        //                Observações = data.Observacoes,
        //                NºResponsável1 = User.Identity.Name,
        //                NºResponsável2 = User.Identity.Name,
        //                NºResponsável3 = User.Identity.Name,
        //                ValidadoresRhKm = User.Identity.Name,
        //                DataHoraÚltimoEstado = DateTime.Now,
        //                UtilizadorModificação = User.Identity.Name,
        //                DataHoraModificação = DateTime.Now
        //            };

        //            //Create FolhaDeHora On Database
        //            cFolhaDeHora = DBFolhasDeHoras.Create(cFolhaDeHora);

        //            if (cFolhaDeHora == null)
        //            {
        //                data.eReasonCode = 3;
        //                data.eMessage = "Ocorreu um erro ao criar a Folha de Hora no Portal.";
        //            }
        //            else
        //            {
        //                //Update Last Numeration Used
        //                ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(FolhaDeHoraNumerationConfigurationId);
        //                ConfigNumerations.ÚltimoNºUsado = data.FolhaDeHorasNo;
        //                DBNumerationConfigurations.Update(ConfigNumerations);

        //                data.eReasonCode = 1;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        data.eReasonCode = 4;
        //        data.eMessage = "Ocorreu um erro ao criar a Folha de Hora.";
        //    }
        //    return Json(data);
        //}

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
        //Atualiza a Folha de Horas na Base de Dados
        public JsonResult UpdateFolhaDeHoras([FromBody] FolhaDeHorasViewModel data)
        {
            int result = 0;
            try
            {
                string ProjetoDescricao = "";
                string EmpregadoNome = "";

                if (data.ProjetoNo != "")
                {
                    NAVProjectsViewModel navProject = DBNAV2017Projects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, "").Where(x => x.No.ToLower() == data.ProjetoNo.ToLower()).FirstOrDefault();
                    if (navProject != null)
                    {
                        ProjetoDescricao = navProject.Description;
                    }
                }

                if (data.EmpregadoNo != "")
                {
                    NAVEmployeeViewModel employee = DBNAV2009Employees.GetAll(data.EmpregadoNo, _config.NAV2009DatabaseName, _config.NAV2009CompanyName).FirstOrDefault();
                    if (employee != null)
                    {
                        EmpregadoNome = employee.Name;
                    }
                }

                if (result == 0)
                {
                    FolhaDeHorasViewModel FH = DBFolhasDeHoras.GetListaValidadoresIntegradores(data.FolhaDeHorasNo, data.EmpregadoNo);

                    if (DBFolhasDeHoras.Update(new FolhasDeHoras()
                    {
                        NºFolhaDeHoras = data.FolhaDeHorasNo,
                        Área = data.Area,
                        NºProjeto = data.ProjetoNo == "" ? null : data.ProjetoNo,
                        ProjetoDescricao = ProjetoDescricao,
                        NºEmpregado = data.EmpregadoNo == "" ? null : data.EmpregadoNo,
                        NomeEmpregado = data.EmpregadoNo == "" ? null : DBNAV2009Employees.GetAll(data.EmpregadoNo, _config.NAV2009DatabaseName, _config.NAV2009CompanyName).FirstOrDefault().Name,
                        DataHoraPartida = DateTime.Parse(string.Concat(data.DataPartidaTexto, " ", data.HoraPartidaTexto)),
                        DataHoraChegada = DateTime.Parse(string.Concat(data.DataChegadaTexto, " ", data.HoraChegadaTexto)),
                        TipoDeslocação = data.TipoDeslocacao,
                        CódigoTipoKmS = data.CodigoTipoKms == "" ? null : data.CodigoTipoKms,
                        Matrícula = data.Matricula == "" ? null : data.Matricula,
                        DeslocaçãoForaConcelho = data.DeslocacaoForaConcelho,
                        DeslocaçãoPlaneada = data.DeslocacaoPlaneada,
                        Terminada = data.Terminada,
                        Estado = data.Estadotexto == "" ? 0 : Convert.ToInt32(data.Estadotexto),
                        CriadoPor = data.CriadoPor,
                        DataHoraCriação = data.DataHoraCriacao,
                        CódigoRegião = FH.CodigoRegiao == "" ? null : FH.CodigoRegiao,
                        CódigoÁreaFuncional = FH.CodigoAreaFuncional == "" ? null : FH.CodigoAreaFuncional,
                        CódigoCentroResponsabilidade = FH.CodigoCentroResponsabilidade == "" ? null : FH.CodigoCentroResponsabilidade,
                        TerminadoPor = data.TerminadoPor,
                        DataHoraTerminado = data.DataHoraTerminado,

                        Validado = data.ValidadoTexto == "" ? false : Convert.ToBoolean(data.ValidadoTexto),
                        Validadores = FH.Validadores == "" ? null : FH.Validadores,
                        Validador = data.Validador,
                        DataHoraValidação = data.DataHoraValidacao,

                        IntegradoEmRh = data.IntegradoEmRhTexto == "" ? false : Convert.ToBoolean(data.IntegradoEmRhTexto),
                        IntegradoresEmRh = FH.IntegradoresEmRH == "" ? null : FH.IntegradoresEmRH,
                        IntegradorEmRh = data.IntegradorEmRH,
                        DataIntegraçãoEmRh = data.DataIntegracaoEmRH,

                        IntegradoEmRhkm = data.IntegradoEmRhKmTexto == "" ? false : Convert.ToBoolean(data.IntegradoEmRhKmTexto),
                        IntegradoresEmRhkm = FH.IntegradoresEmRHKM == "" ? null : FH.IntegradoresEmRHKM,
                        IntegradorEmRhKm = data.IntegradorEmRHKM,
                        DataIntegraçãoEmRhKm = data.DataIntegracaoEmRHKM,

                        CustoTotalAjudaCusto = data.CustoTotalAjudaCusto,
                        CustoTotalHoras = data.CustoTotalHoras,
                        CustoTotalKm = data.CustoTotalKM,
                        NumTotalKm = data.NumTotalKM,
                        Observações = data.Observacoes,
                        NºResponsável1 = FH.Responsavel1No == "" ? null : FH.Responsavel1No,
                        NºResponsável2 = FH.Responsavel2No == "" ? null : FH.Responsavel2No,
                        NºResponsável3 = FH.Responsavel3No == "" ? null : FH.Responsavel3No,
                        ValidadoresRhKm = data.ValidadoresRHKM,
                        DataHoraÚltimoEstado = data.DataHoraUltimoEstado,
                        UtilizadorModificação = User.Identity.Name,
                        DataHoraModificação = DateTime.Now
                    }) == null)
                    {
                        result = 2;
                    }
                    else
                    {
                        result = 0;
                    };
                }
            }
            catch (Exception ex)
            {
                result = 99;
            }
            return Json(result);
        }

        //[HttpPost]
        //public JsonResult DeleteFolhaDeHoras([FromBody] FolhaDeHorasViewModel data)
        //{
        //    if (data != null)
        //    {
        //        ErrorHandler result = new ErrorHandler();
        //        DBFolhasDeHoras.Delete(data.FolhaDeHorasNo);
        //        result = new ErrorHandler()
        //        {
        //            eReasonCode = 0,
        //            eMessage = "Folha de Horas removida com sucesso."
        //        };
        //        return Json(result);
        //    }
        //    return Json(false);
        //}
        #endregion

        #region Job Ledger Entry

        //public IActionResult MovimentosDeFolhaDeHora(String FolhaDeHoraNo)
        //{
        //    UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 1, 6);

        //    if (UPerm != null && UPerm.Read.Value)
        //    {
        //        ViewBag.UPermissions = UPerm;
        //        ViewBag.FolhaDeHoraNo = FolhaDeHoraNo;
        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("AccessDenied", "Error");
        //    }
        //}

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

                    var dbCreateResult1 = DBLinhasFolhaHoras.CreatePercurso(Percurso1);


                    LinhasFolhaHoras Percurso2 = new LinhasFolhaHoras();

                    Percurso2.NoFolhaHoras = data.NoFolhaHoras;
                    Percurso2.NoLinha = noLinha + 1;
                    Percurso2.TipoCusto = 1; //PERCURSO
                    Percurso2.CodTipoCusto = data.CodTipoCusto;
                    Percurso2.DescricaoTipoCusto = DBTabelaConfRecursosFh.GetDescricaoByRecurso("1", data.CodTipoCusto);
                    Percurso2.Quantidade = DBDistanciaFh.GetDistanciaPrevista(data.CodOrigem, data.CodDestino);
                    Percurso2.CodOrigem = data.CodOrigem;
                    Percurso2.DescricaoOrigem = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.CodOrigem);
                    Percurso2.CodDestino = data.CodDestino;
                    Percurso2.DescricaoDestino = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.CodDestino);
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

                    var dbCreateResult1 = DBLinhasFolhaHoras.CreatePercurso(Percurso1);

                    if (dbCreateResult1 != null)
                        result = true;
                    else
                        result = false;
                }

                if (result)
                {
                    DBFolhasDeHoras.UpdateDetalhes(data.NoFolhaHoras);
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

                if (result == 0)
                {
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

                    DBLinhasFolhaHoras.UpdatePercurso(Percurso);
                }

                if (result == 0)
                {
                    DBFolhasDeHoras.UpdateDetalhes(data.NoFolhaHoras);
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
                    DBFolhasDeHoras.UpdateDetalhes(data.NoFolhaHoras);
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
            bool result = false;
            try
            {
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
                Ajuda.UtilizadorCriacao = User.Identity.Name;
                Ajuda.DataHoraCriacao = DateTime.Now;
                Ajuda.UtilizadorModificacao = User.Identity.Name;
                Ajuda.DataHoraModificacao = DateTime.Now;

                var dbCreateResult = DBLinhasFolhaHoras.CreateAjuda(Ajuda);

                if (dbCreateResult != null)
                    result = true;
                else
                    result = false;

                if (result)
                {
                    DBFolhasDeHoras.UpdateDetalhes(data.NoFolhaHoras);
                }
            }
            catch (Exception ex)
            {
                //log
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

                if (result == 0)
                {

                    LinhasFolhaHoras Ajuda = DBLinhasFolhaHoras.GetByAjudaNo(data.NoFolhaHoras, data.NoLinha);

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
                    Ajuda.UtilizadorModificacao = User.Identity.Name;
                    Ajuda.DataHoraModificacao = DateTime.Now;

                    DBLinhasFolhaHoras.UpdateAjuda(Ajuda);

                    result = 0;

                    if (result == 0)
                    {
                        DBFolhasDeHoras.UpdateDetalhes(data.NoFolhaHoras);
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
                    DBFolhasDeHoras.UpdateDetalhes(data.NoFolhaHoras);
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
                TimeSpan HoraInicio = TimeSpan.Parse(data.HoraInicio);
                TimeSpan HoraFim = TimeSpan.Parse(data.HoraFim);
                bool Almoco = Convert.ToBoolean(data.HorarioAlmoco);
                bool Jantar = Convert.ToBoolean(data.HorarioJantar);

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

                    var dbCreateResult = DBMaoDeObraFolhaDeHoras.Create(MaoDeObra);

                    if (dbCreateResult != null)
                        result = 0;
                    else
                        result = 6;

                    if (result == 0)
                    {
                        DBFolhasDeHoras.UpdateDetalhes(data.FolhaDeHorasNo);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(99);
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
                        DBFolhasDeHoras.UpdateDetalhes(data.FolhaDeHorasNo);
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
                    DBFolhasDeHoras.UpdateDetalhes(data.FolhaDeHorasNo);
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
            bool result = false;
            try
            {
                decimal NoDias = 0;
                int noLinha;

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

                if (AjudaCusto != null)
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
                            Ajuda.UtilizadorCriacao = User.Identity.Name;
                            Ajuda.DataHoraCriacao = DateTime.Now;
                            Ajuda.UtilizadorModificacao = User.Identity.Name;
                            Ajuda.DataHoraModificacao = DateTime.Now;

                            var dbCreateResult = DBLinhasFolhaHoras.CreateAjuda(Ajuda);
                        }
                    });
                }

                result = true;

                if (result)
                {
                    DBFolhasDeHoras.UpdateDetalhes(data.FolhaDeHorasNo);
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                //log
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
            int result = 0;
            try
            {
                if (data != null)
                {
                    FolhasDeHoras FH = DBFolhasDeHoras.GetById(data.FolhaDeHorasNo);
                    //if (DBFolhasDeHoras.Update(new FolhasDeHoras()
                    //{
                    //    NºFolhaDeHoras = data.FolhaDeHorasNo,
                    //    Área = FH.Área,
                    //    NºProjeto = data.ProjetoNo,
                    //    ProjetoDescricao = DBProjects.GetById(data.ProjetoNo).Descrição,
                    //    NºEmpregado = data.EmpregadoNo,
                    //    NomeEmpregado = DBNAV2009Employees.GetAll(data.EmpregadoNo, _config.NAV2009DatabaseName, _config.NAV2009CompanyName).FirstOrDefault().Name,
                    //    DataHoraPartida = DateTime.Parse(string.Concat(data.DataPartidaTexto, " ", data.HoraPartidaTexto)),
                    //    DataHoraChegada = DateTime.Parse(string.Concat(data.DataChegadaTexto, " ", data.HoraChegadaTexto)),
                    //    TipoDeslocação = data.TipoDeslocacao,
                    //    CódigoTipoKmS = data.CodigoTipoKms,
                    //    Matrícula = data.Matricula,
                    //    DeslocaçãoForaConcelho = data.DeslocacaoForaConcelho,
                    //    DeslocaçãoPlaneada = null,
                    //    Terminada = true, //TERMINADA
                    //    Estado = data.Estado,
                    //    CriadoPor = data.CriadoPor,
                    //    DataHoraCriação = FH.DataHoraCriação,
                    //    CódigoRegião = data.CodigoRegiao,
                    //    CódigoÁreaFuncional = data.CodigoAreaFuncional,
                    //    CódigoCentroResponsabilidade = data.CodigoCentroResponsabilidade,
                    //    TerminadoPor = User.Identity.Name, //TERMINADA
                    //    DataHoraTerminado = DateTime.Now, //TERMINADA
                    //    Validado = data.Validado,
                    //    Validadores = data.Validadores,
                    //    Validador = data.Validador,
                    //    DataHoraValidação = DateTime.Parse(string.Concat(data.DataValidacaoTexto, " ", data.HoraValidacaoTexto)),
                    //    IntegradoEmRh = FH.IntegradoEmRh,
                    //    IntegradoresEmRh = data.IntegradoresEmRH,
                    //    IntegradorEmRh = data.IntegradorEmRH,
                    //    DataIntegraçãoEmRh = DateTime.Parse(string.Concat(data.DataIntegracaoEmRHTexto, " ", data.HoraIntegracaoEmRHTexto)),
                    //    IntegradoEmRhkm = FH.IntegradoEmRhkm,
                    //    IntegradoresEmRhkm = data.IntegradoresEmRHKM,
                    //    IntegradorEmRhKm = data.IntegradorEmRHKM,
                    //    DataIntegraçãoEmRhKm = DateTime.Parse(string.Concat(data.DataIntegracaoEmRHKMTexto, " ", data.HoraIntegracaoEmRHKMTexto)),
                    //    CustoTotalAjudaCusto = data.CustoTotalAjudaCusto,
                    //    CustoTotalHoras = data.CustoTotalHoras,
                    //    CustoTotalKm = data.CustoTotalKM,
                    //    NumTotalKm = data.NumTotalKM,
                    //    Observações = data.Observacoes,
                    //    NºResponsável1 = FH.NºResponsável1,
                    //    NºResponsável2 = FH.NºResponsável2,
                    //    NºResponsável3 = FH.NºResponsável3,
                    //    ValidadoresRhKm = FH.ValidadoresRhKm,
                    //    DataHoraÚltimoEstado = FH.DataHoraÚltimoEstado,
                    //    UtilizadorModificação = User.Identity.Name, //TERMINADA
                    //    DataHoraModificação = DateTime.Now //TERMINADA
                    //}) == null)
                    //{
                    //    result = 6;
                    //}
                    //else
                    //{
                    //    result = 0;
                    //};
                }
            }
            catch (Exception ex)
            {
                result = 99;
            }
            return Json(result);
        }

        [HttpPost]
        //Valida uma Folha de Horas
        public JsonResult ValidarFolhaDeHoras([FromBody] FolhaDeHorasViewModel data)
        {
            int result = 0;
            try
            {
                if (string.IsNullOrEmpty(data.FolhaDeHorasNo) || string.IsNullOrEmpty(data.EmpregadoNo))
                {
                    result = 6;
                }
                else
                {
                    if ((data.Validado == null ? false : (bool)data.Validado) || (int)data.Estado != 0)
                    {
                        result = 5; //Não Pode validar pois já se encontra validada
                    }
                    else
                    {
                        if (!data.Validadores.ToLower().Contains(User.Identity.Name.ToLower()))
                        {
                            result = 1; //Não tem permissões para validar
                        }
                        else
                        {
                            using (var ctx = new SuchDBContextExtention())
                            {
                                var parameters = new[]
                                {
                                    new SqlParameter("@NoFH", data.FolhaDeHorasNo),
                                    new SqlParameter("@NoUtilizador", data.EmpregadoNo)
                                };
                                result = ctx.execStoredProcedureFH("exec FH_Validar @NoFH, @NoUtilizador", parameters);

                                if (result == 0)
                                {
                                    string EmpregadoNome = DBUserConfigurations.GetById(User.Identity.Name).Nome;
                                    string TipoDeslocação = data.TipoDeslocacaoTexto;
                                    int Estado = (int)data.Estado;
                                    int NoRegistos = 0;

                                    NoRegistos = DBLinhasFolhaHoras.GetAll().Where(x => x.NoFolhaHoras.ToLower() == data.FolhaDeHorasNo.ToLower() && x.TipoCusto == 2).Count();

                                    if (TipoDeslocação != "2" && NoRegistos == 0)
                                        Estado = 2; // 2 = Registado
                                    else
                                        Estado = 1; //VALIDADO

                                    if (DBFolhasDeHoras.Update(new FolhasDeHoras()
                                    {
                                        NºFolhaDeHoras = data.FolhaDeHorasNo,
                                        Área = data.Area,
                                        NºProjeto = data.ProjetoNo == "" ? null : data.ProjetoNo,
                                        ProjetoDescricao = data.ProjetoDescricao,
                                        NºEmpregado = data.EmpregadoNo == "" ? null : data.EmpregadoNo,
                                        NomeEmpregado = data.EmpregadoNo == "" ? null : data.EmpregadoNo,
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
                                        UtilizadorModificação = User.Identity.Name //VALIDAÇÂO
                                    }) == null)
                                    {
                                        result = 6;
                                    }
                                    else
                                    {
                                        result = 0;
                                    };

                                    //Atualiza a tabela Presenças
                                    //ATENÇÃO QUE VAI ATIVAR O TRIGGER!!!
                                    if (result == 0)
                                    {
                                        List<PresencasFolhaDeHorasViewModel> presencas = DBPresencasFolhaDeHoras.GetAllByPresencaToList(data.FolhaDeHorasNo);
                                        if (presencas != null)
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
                                                    DataHoraModificação = DateTime.Now,
                                                });
                                            });
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
                result = 99;
            }
            return Json(result);
        }

        [HttpPost]
        //Intregra as Ajudas de Custo de uma Folha de Horas
        public JsonResult IntegrarEmRHFolhaDeHoras([FromBody] FolhaDeHorasViewModel data)
        {
            int result = 0;
            try
            {
                if (string.IsNullOrEmpty(data.FolhaDeHorasNo) || string.IsNullOrEmpty(data.EmpregadoNo) || string.IsNullOrEmpty(data.ProjetoNo))
                {
                    result = 6;
                }
                else
                {
                    if (data.IntegradoEmRh == null ? false : (bool)data.IntegradoEmRh)
                    {
                        result = 5;
                    }
                    else
                    {
                        if ((int)data.Estado != 1)
                        {
                            result = 8;
                        }
                        else
                        {
                            if (!data.IntegradoresEmRH.ToLower().Contains(User.Identity.Name.ToLower()))
                            {
                                result = 1;
                            }
                            else
                            {
                                using (var ctx = new SuchDBContextExtention())
                                {
                                    var parameters = new[]
                                    {
                                        new SqlParameter("@NoFH", data.FolhaDeHorasNo),
                                        new SqlParameter("@NoUtilizador", data.EmpregadoNo)
                                    };

                                    result = ctx.execStoredProcedureFH("exec FH_IntegrarEmRH @NoFH, @NoUtilizador", parameters);

                                    if (result == 0)
                                    {
                                        string EmpregadoNome = DBUserConfigurations.GetById(User.Identity.Name).Nome;
                                        bool IntegradoEmRhKm = (bool)data.IntegradoEmRhKm;
                                        string TipoDeslocação = data.TipoDeslocacaoTexto;
                                        int Estado = (int)data.Estado;

                                        if (IntegradoEmRhKm || TipoDeslocação != "2")
                                            Estado = 2; // 2 = Registado

                                        if (DBFolhasDeHoras.Update(new FolhasDeHoras()
                                        {
                                            NºFolhaDeHoras = data.FolhaDeHorasNo,
                                            Área = data.Area,
                                            NºProjeto = data.ProjetoNo == "" ? null : data.ProjetoNo,
                                            ProjetoDescricao = data.ProjetoDescricao,
                                            NºEmpregado = data.EmpregadoNo == "" ? null : data.EmpregadoNo,
                                            NomeEmpregado = data.EmpregadoNo == "" ? null : data.EmpregadoNo,
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
                                            DataHoraModificação = DateTime.Now //INTEGRAREMRH
                                        }) == null)
                                        {
                                            result = 7;
                                        }
                                        else
                                        {
                                            result = 0;
                                        };
                                    }
                                }
                            }
                        }
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
        //Integra os KM's de uma Folha de Horas
        public JsonResult IntegrarEmRHKMFolhaDeHoras([FromBody] FolhaDeHorasViewModel data)
        {
            int result = 0;
            try
            {
                if (string.IsNullOrEmpty(data.FolhaDeHorasNo) || string.IsNullOrEmpty(data.EmpregadoNo) || string.IsNullOrEmpty(data.ProjetoNo) || data.TipoDeslocacao != 2) //2 = "Viatura Própria"
                {
                    result = 6;
                }
                else
                {
                    if (data.IntegradoEmRhKm == null ? false : (bool)data.IntegradoEmRhKm)
                    {
                        result = 5;
                    }
                    else
                    {
                        if ((int)data.Estado != 1)
                        {
                            result = 8;
                        }
                        else
                        {
                            if (!data.IntegradoresEmRHKM.ToLower().Contains(User.Identity.Name.ToLower()))
                            {
                                result = 1;
                            }
                            else
                            {
                                using (var ctx = new SuchDBContextExtention())
                                {
                                    var parameters = new[]
                                    {
                                        new SqlParameter("@NoFH", data.FolhaDeHorasNo),
                                        new SqlParameter("@NoUtilizador", data.EmpregadoNo)
                                    };

                                    result = ctx.execStoredProcedureFH("exec FH_IntegrarEmRHKM @NoFH, @NoUtilizador", parameters);

                                    if (result == 0)
                                    {
                                        string EmpregadoNome = DBUserConfigurations.GetById(User.Identity.Name).Nome;
                                        bool IntegradoEmRh = (bool)data.IntegradoEmRh;
                                        int NoRegistos = 0;
                                        int Estado = (int)data.Estado;

                                        NoRegistos = DBLinhasFolhaHoras.GetAll().Where(x => x.NoFolhaHoras.ToLower() == data.FolhaDeHorasNo.ToLower() && x.TipoCusto == 2).Count();

                                        if (IntegradoEmRh || NoRegistos == 0)
                                            Estado = 2; // 2 = Registado

                                        if (DBFolhasDeHoras.Update(new FolhasDeHoras()
                                        {
                                            NºFolhaDeHoras = data.FolhaDeHorasNo,
                                            Área = data.Area,
                                            NºProjeto = data.ProjetoNo == "" ? null : data.ProjetoNo,
                                            ProjetoDescricao = data.ProjetoDescricao,
                                            NºEmpregado = data.EmpregadoNo == "" ? null : data.EmpregadoNo,
                                            NomeEmpregado = data.EmpregadoNo == "" ? null : data.EmpregadoNo,
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
                                            DataHoraModificação = DateTime.Now //INTEGRAREMRHKM
                                        }) == null)
                                        {
                                            result = 7;
                                        }
                                        else
                                        {
                                            result = 0;
                                        };
                                    }
                                }
                            }
                        }
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
    }
}
