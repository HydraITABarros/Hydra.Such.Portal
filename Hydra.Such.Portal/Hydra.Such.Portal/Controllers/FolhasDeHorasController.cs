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
            return View();
        }

        [HttpPost]
        public JsonResult GetListFolhasDeHorasByArea([FromBody] int id)
        {
            List<FolhaDeHorasViewModel> result = DBFolhasDeHoras.GetAllByAreaToList(id);

            result.ForEach(FH =>
            {
                FH.AreaTexto = EnumerablesFixed.Areas.Where(y => y.Id == FH.Area).FirstOrDefault().Value;
                FH.TipoDeslocacaoTexto = EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == FH.TipoDeslocacao).FirstOrDefault().Value;
                if (FH.DeslocacaoForaConcelho.Value) FH.DeslocacaoForaConcelhoTexto = "Sim"; else FH.DeslocacaoForaConcelhoTexto = "Não";
                FH.Estadotexto = EnumerablesFixed.FolhaDeHoraStatus.Where(y => y.Id == FH.Estado).FirstOrDefault().Value;
                FH.Validadores = DBUserConfigurations.GetById(FH.Validadores).Nome;
            });
            return Json(result);
        }
        #endregion

        #region Details
        public IActionResult Detalhes(String id)
        {
            ViewBag.FolhaDeHorasNo = id == null ? "" : id;
            return View();
        }

        [HttpPost]
        public JsonResult GetFolhaDeHoraDetails([FromBody] FolhaDeHorasViewModel data)
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
                        AreaTexto = FH.Área.ToString(),
                        ProjetoNo = FH.NºProjeto,
                        ProjetoDescricao = "",
                        EmpregadoNo = FH.NºEmpregado,
                        DataHoraPartida = FH.DataHoraPartida,
                        DataPartidaTexto = FH.DataHoraPartida.Value.ToString("yyyy-MM-dd"),
                        HoraPartidaTexto = FH.DataHoraPartida.Value.ToShortTimeString(),
                        DataHoraChegada = FH.DataHoraChegada,
                        DataChegadaTexto = FH.DataHoraChegada.Value.ToString("yyyy-MM-dd"),
                        HoraChegadaTexto = FH.DataHoraChegada.Value.ToShortTimeString(),
                        TipoDeslocacao = FH.TipoDeslocação,
                        TipoDeslocacaoTexto = FH.TipoDeslocação.ToString(),
                        CodigoTipoKms = FH.CódigoTipoKmS,
                        DeslocacaoForaConcelho = FH.DeslocaçãoForaConcelho,
                        DeslocacaoForaConcelhoTexto = FH.DeslocaçãoForaConcelho.ToString(),
                        Validadores = FH.Validadores,
                        Estado = FH.Estado,
                        Estadotexto = FH.Estado.ToString(),
                        CriadoPor = FH.CriadoPor,
                        DataHoraCriacao = FH.DataHoraCriação,
                        DataCriacaoTexto = FH.DataHoraCriação.Value.ToString("yyyy-MM-dd"),
                        HoraCriacaoTexto = FH.DataHoraCriação.Value.ToShortTimeString(),
                        DataHoraUltimoEstado = FH.DataHoraÚltimoEstado,
                        DataUltimoEstadoTexto = FH.DataHoraÚltimoEstado.Value.ToString("yyyy-MM-dd"),
                        HoraUltimoEstadoTexto = FH.DataHoraÚltimoEstado.Value.ToShortTimeString(),
                        //abarros_
                        //UtilizadorCriacao = FH.UtilizadorCriação,
                        DataHoraModificacao = FH.DataHoraModificação,
                        DataModificacaoTexto = FH.DataHoraModificação.Value.ToString("yyyy-MM-dd"),
                        HoraModificacaoTexto = FH.DataHoraModificação.Value.ToShortTimeString(),
                        UtilizadorModificacao = FH.UtilizadorModificação,
                        EmpregadoNome = FH.NomeEmpregado,
                        Matricula = FH.Matrícula,
                        Terminada = FH.Terminada,
                        TerminadaTexto = FH.Terminada.ToString(),
                        TerminadoPor = FH.TerminadoPor,
                        DataHoraTerminado = FH.DataHoraTerminado,
                        DataTerminadoTexto = FH.DataHoraTerminado.Value.ToString("yyyy-MM-dd"),
                        HoraTerminadoTexto = FH.DataHoraTerminado.Value.ToShortTimeString(),
                        Validado = FH.Validado,
                        ValidadoTexto = FH.Validado.ToString(),
                        DeslocacaoPlaneada = FH.DeslocaçãoPlaneada,
                        DeslocacaoPlaneadaTexto = FH.DeslocaçãoPlaneada.ToString(),
                        Observacoes = FH.Observações,
                        Responsavel1No = FH.NºResponsável1,
                        Responsavel2No = FH.NºResponsável2,
                        Responsavel3No = FH.NºResponsável3,
                        ValidadoresRHKM = FH.ValidadoresRhKm,
                        CodigoRegiao = FH.CódigoRegião,
                        CodigoAreaFuncional = FH.CódigoÁreaFuncional,
                        CodigoCentroResponsabilidade = FH.CódigoCentroResponsabilidade,
                        Validador = FH.Validador,
                        DataHoraValidacao = FH.DataHoraValidação,
                        DataValidacaoTexto = FH.DataHoraValidação.Value.ToString("yyyy-MM-dd"),
                        HoraValidacaoTexto = FH.DataHoraValidação.Value.ToShortTimeString(),
                        IntegradorEmRH = FH.IntegradorEmRh,
                        DataIntegracaoEmRH = FH.DataIntegraçãoEmRh,
                        DataIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh.Value.ToString("yyyy-MM-dd"),
                        HoraIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh.Value.ToShortTimeString(),
                        IntegradorEmRHKM = FH.IntegradorEmRhKm,
                        DataIntegracaoEmRHKM = FH.DataIntegraçãoEmRhKm,
                        DataIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm.Value.ToString("yyyy-MM-dd"),
                        HoraIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm.Value.ToShortTimeString()
                    };

                    Projetos cProject = DBProjects.GetById(FH.NºProjeto);
                    result.ProjetoDescricao = cProject.Descrição;

                    List<NAVEmployeeViewModel> employee = DBNAV2009Employees.GetAll(FH.NºEmpregado, _config.NAVDatabaseName, _config.NAVCompanyName);
                    result.EmpregadoNome = employee[0].Name;

                    //PERCURSO
                    result.FolhaDeHorasPercurso = DBPercursosEAjudasCustoDespesasFolhaDeHoras.GetAllByPercursoToList(data.FolhaDeHorasNo).Select(Percurso => new PercursosEAjudasCustoDespesasFolhaDeHorasViewModel()
                    {
                        FolhaDeHorasNo = Percurso.FolhaDeHorasNo,
                        TipoCusto = Percurso.TipoCusto,
                        LinhaNo = Percurso.LinhaNo,
                        Descricao = Percurso.Descricao,
                        Origem = Percurso.Origem,
                        Destino = Percurso.Destino,
                        DataViagem = Percurso.DataViagem,
                        DataViagemTexto = Percurso.DataViagem.Value.ToString("yyyy-MM-dd"),
                        Distancia = Convert.ToDecimal(Percurso.Distancia),
                        Quantidade = Convert.ToDecimal(Percurso.Quantidade),
                        CustoUnitario = Convert.ToDecimal(Percurso.CustoUnitario),
                        CustoTotal = Convert.ToDecimal(Percurso.CustoTotal),
                        PrecoUnitario = Convert.ToDecimal(Percurso.PrecoUnitario),
                        Justificacao = Percurso.Justificacao,
                        RubricaSalarial = Percurso.RubricaSalarial,
                        DataHoraCriacao = Percurso.DataHoraCriacao,
                        DataHoraCriacaoTexto = Percurso.DataHoraCriacao.Value.ToString("yyyy-MM-dd"),
                        UtilizadorCriacao = Percurso.UtilizadorCriacao,
                        DataHoraModificacao = Percurso.DataHoraModificacao,
                        DataHoraModificacaoTexto = Percurso.DataHoraModificacao.Value.ToString("yyyy-MM-dd"),
                        UtilizadorModificacao = Percurso.UtilizadorModificacao
                    }).ToList();

                    //AJUDA DE CUSTO/DESPESA
                    result.FolhaDeHorasAjuda = DBPercursosEAjudasCustoDespesasFolhaDeHoras.GetAllByAjudaToList(data.FolhaDeHorasNo).Select(Ajuda => new PercursosEAjudasCustoDespesasFolhaDeHorasViewModel()
                    {
                        FolhaDeHorasNo = Ajuda.FolhaDeHorasNo,
                        TipoCusto = Ajuda.TipoCusto,
                        LinhaNo = Ajuda.LinhaNo,
                        Descricao = Ajuda.Descricao,
                        Origem = Ajuda.Origem,
                        Destino = Ajuda.Destino,
                        DataViagem = Ajuda.DataViagem,
                        DataViagemTexto = Ajuda.DataViagem.Value.ToString("yyyy-MM-dd"),
                        Distancia = Convert.ToDecimal(Ajuda.Distancia),
                        Quantidade = Convert.ToDecimal(Ajuda.Quantidade),
                        CustoUnitario = Convert.ToDecimal(Ajuda.CustoUnitario),
                        CustoTotal = Convert.ToDecimal(Ajuda.CustoTotal),
                        PrecoUnitario = Convert.ToDecimal(Ajuda.PrecoUnitario),
                        Justificacao = Ajuda.Justificacao,
                        RubricaSalarial = Ajuda.RubricaSalarial,
                        DataHoraCriacao = Ajuda.DataHoraCriacao,
                        DataHoraCriacaoTexto = Ajuda.DataHoraCriacao.Value.ToString("yyyy-MM-dd"),
                        UtilizadorCriacao = Ajuda.UtilizadorCriacao,
                        DataHoraModificacao = Ajuda.DataHoraModificacao,
                        DataHoraModificacaoTexto = Ajuda.DataHoraModificacao.Value.ToString("yyyy-MM-dd"),
                        UtilizadorModificacao = Ajuda.UtilizadorModificacao
                    }).ToList();

                    //MÃO-DE-OBRA
                    result.FolhaDeHorasMaoDeObra = DBMaoDeObraFolhaDeHoras.GetAllByMaoDeObraToList(data.FolhaDeHorasNo).Select(MaoDeObra => new MaoDeObraFolhaDeHorasViewModel()
                    {
                        FolhaDeHorasNo = MaoDeObra.FolhaDeHorasNo,
                        LinhaNo = MaoDeObra.LinhaNo,
                        Date = MaoDeObra.Date,
                        EmpregadoNo = MaoDeObra.EmpregadoNo,
                        ProjetoNo = MaoDeObra.ProjetoNo,
                        CodigoTipoTrabalho = MaoDeObra.CodigoTipoTrabalho,
                        //HoraInicio = Convert.ToDateTime("1753-01-01 " + MaoDeObra.HoraInicio),
                        HoraInicioTexto = MaoDeObra.HoraInicio.ToString(),
                        //HoraFim = Convert.ToDateTime("1753-01-01 " + MaoDeObra.HoraFim),
                        HoraFimTexto = MaoDeObra.HoraFim.ToString(),
                        HorarioAlmoco = MaoDeObra.HorarioAlmoco,
                        HorarioJantar = MaoDeObra.HorarioJantar,
                        CodigoFamiliaRecurso = MaoDeObra.CodigoFamiliaRecurso,
                        RecursoNo = MaoDeObra.RecursoNo,
                        CodigoUnidadeMedida = MaoDeObra.CodigoUnidadeMedida,
                        CodigoTipoOM = MaoDeObra.CodigoTipoOM,
                        //HorasNo = Convert.ToDateTime("1753-01-01 " + MaoDeObra.HorasNo),
                        HorasNoTexto = MaoDeObra.HorasNo.ToString(),
                        CustoUnitarioDireto = Convert.ToDecimal(MaoDeObra.CustoUnitarioDireto),
                        PrecoDeCusto = Convert.ToDecimal(MaoDeObra.PrecoDeCusto),
                        PrecoDeVenda = Convert.ToDecimal(MaoDeObra.PrecoDeVenda),
                        PrecoTotal = Convert.ToDecimal(MaoDeObra.PrecoTotal),
                        DataHoraCriacao = MaoDeObra.DataHoraCriacao,
                        DataHoraCriacaoTexto = MaoDeObra.DataHoraCriacao.Value.ToString("yyyy-MM-dd"),
                        UtilizadorCriacao = MaoDeObra.UtilizadorCriacao,
                        DataHoraModificacao = MaoDeObra.DataHoraModificacao,
                        DataHoraModificacaoTexto = MaoDeObra.DataHoraModificacao.Value.ToString("yyyy-MM-dd"),
                        UtilizadorModificacao = MaoDeObra.UtilizadorModificacao
                    }).ToList();

                    //PRESENÇA
                    result.FolhaDeHorasPresenca = DBPresencasFolhaDeHoras.GetAllByPresencaToList(data.FolhaDeHorasNo).Select(Presenca => new PresencasFolhaDeHorasViewModel()
                    {
                        FolhaDeHorasNo = Presenca.FolhaDeHorasNo,
                        Data = Presenca.Data,
                        DataTexto = Presenca.Data.Value.ToString("yyyy-MM-dd"),
                        Hora1Entrada = Presenca.Hora1Entrada.ToString(),
                        Hora1Saida = Presenca.Hora1Saida.ToString(),
                        Hora2Entrada = Presenca.Hora2Entrada.ToString(),
                        Hora2Saida = Presenca.Hora2Saida.ToString(),
                        DataHoraCriacao = Presenca.DataHoraCriacao,
                        DataHoraCriacaoTexto = Presenca.DataHoraCriacao.Value.ToString("yyyy-MM-dd"),
                        UtilizadorCriacao = Presenca.UtilizadorCriacao,
                        DataHoraModificacao = Presenca.DataHoraModificacao,
                        DataHoraModificacaoTexto = Presenca.DataHoraModificacao.Value.ToString("yyyy-MM-dd"),
                        UtilizadorModificacao = Presenca.UtilizadorModificacao
                    }).ToList();

                    return Json(result);
                }

                return Json(new FolhaDeHorasViewModel());
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult ValidateNumeration([FromBody] FolhaDeHorasViewModel data)
        {
            //Get FolhaDeHora Numeration
            Configuração Cfg = DBConfigurations.GetById(1);
            int FolhaDeHoraNumerationConfigurationId = Cfg.NumeraçãoFolhasDeHoras.Value;

            ConfiguraçãoNumerações CfgNumeration = DBNumerationConfigurations.GetById(FolhaDeHoraNumerationConfigurationId);

            //Validate if FolhaDeHorasNo is valid
            if (data.FolhaDeHorasNo != "" && !CfgNumeration.Manual.Value)
            {
                return Json("A numeração configurada para folha de horas não permite inserção manual.");
            }
            else if (data.FolhaDeHorasNo == "" && !CfgNumeration.Automático.Value)
            {
                return Json("É obrigatório inserir o Nº de Folha de Horas.");
            }

            return Json("");
        }

        //eReason = 1 -> Sucess
        //eReason = 2 -> Error creating Project on Databse 
        //eReason = 3 -> Error creating Project on NAV 
        //eReason = 4 -> Unknow Error 
        [HttpPost]
        public JsonResult CreateFolhaDeHora([FromBody] FolhaDeHorasViewModel data)
        {
            try
            {
                if (data != null)
                {
                    //Get FolhaDeHora Numeration
                    Configuração Configs = DBConfigurations.GetById(1);
                    int FolhaDeHoraNumerationConfigurationId = Configs.NumeraçãoFolhasDeHoras.Value;
                    data.FolhaDeHorasNo = DBNumerationConfigurations.GetNextNumeration(FolhaDeHoraNumerationConfigurationId, true);

                    FolhasDeHoras cFolhaDeHora = new FolhasDeHoras()
                    {
                        NºFolhaDeHoras = data.FolhaDeHorasNo,
                        Área = data.Area,
                        NºProjeto = data.ProjetoNo,
                        NºEmpregado = data.EmpregadoNo,
                        DataHoraPartida = DateTime.Parse(string.Concat(data.DataPartidaTexto, " ", data.HoraPartidaTexto)),
                        DataHoraChegada = DateTime.Parse(string.Concat(data.DataChegadaTexto, " ", data.HoraChegadaTexto)),
                        TipoDeslocação = data.TipoDeslocacao,
                        CódigoTipoKmS = data.CodigoTipoKms,
                        DeslocaçãoForaConcelho = Convert.ToBoolean(data.DeslocacaoForaConcelhoTexto),
                        Validadores = User.Identity.Name,
                        Estado = 1,
                        CriadoPor = User.Identity.Name,
                        DataHoraCriação = DateTime.Now,
                        DataHoraÚltimoEstado = DateTime.Now,
                        //abarros_
                        //UtilizadorCriação = User.Identity.Name,
                        DataHoraModificação = DateTime.Now,
                        UtilizadorModificação = User.Identity.Name,
                        NomeEmpregado = data.EmpregadoNome,
                        Matrícula = data.Matricula,
                        Terminada = data.Terminada,
                        TerminadoPor = User.Identity.Name,
                        DataHoraTerminado = DateTime.Now,
                        Validado = Convert.ToBoolean(data.ValidadoTexto),
                        DeslocaçãoPlaneada = Convert.ToBoolean(data.DeslocacaoPlaneadaTexto),
                        Observações = data.Observacoes,
                        NºResponsável1 = User.Identity.Name,
                        NºResponsável2 = User.Identity.Name,
                        NºResponsável3 = User.Identity.Name,
                        ValidadoresRhKm = User.Identity.Name,
                        CódigoRegião = data.CodigoRegiao,
                        CódigoÁreaFuncional = data.CodigoAreaFuncional,
                        CódigoCentroResponsabilidade = data.CodigoCentroResponsabilidade,
                        Validador = User.Identity.Name,
                        DataHoraValidação = DateTime.Now,
                        IntegradorEmRh = User.Identity.Name,
                        DataIntegraçãoEmRh = DateTime.Now,
                        IntegradorEmRhKm = User.Identity.Name,
                        DataIntegraçãoEmRhKm = DateTime.Now
                    };

                    //Create FolhaDeHora On Database
                    cFolhaDeHora = DBFolhasDeHoras.Create(cFolhaDeHora);

                    if (cFolhaDeHora == null)
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "Ocorreu um erro ao criar a Folha de Hora no Portal.";
                    }
                    else
                    {
                        //Update Last Numeration Used
                        ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(FolhaDeHoraNumerationConfigurationId);
                        ConfigNumerations.ÚltimoNºUsado = data.FolhaDeHorasNo;
                        DBNumerationConfigurations.Update(ConfigNumerations);

                        data.eReasonCode = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 4;
                data.eMessage = "Ocorreu um erro ao criar a Folha de Hora.";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult UpdateFolhaDeHora([FromBody] FolhaDeHorasViewModel data)
        {
            FolhasDeHoras FHUpdate = DBFolhasDeHoras.GetById(data.FolhaDeHorasNo);
            if (FHUpdate == null)
            {
                data.eReasonCode = 1;
                data.eMessage = "Não foi possivel obter a Folha de Horas.";
            }
            else
            {
                FHUpdate.CriadoPor = data.CriadoPor;
                FHUpdate.CódigoCentroResponsabilidade = data.CodigoCentroResponsabilidade;
                FHUpdate.CódigoRegião = data.CodigoRegiao;
                FHUpdate.CódigoTipoKmS = data.CodigoTipoKms;
                FHUpdate.CódigoÁreaFuncional = data.CodigoAreaFuncional;
                FHUpdate.DataHoraChegada = data.DataHoraChegada;
                FHUpdate.DataHoraCriação = data.DataHoraCriacao;
                FHUpdate.DataHoraModificação = System.DateTime.Now;
                FHUpdate.DataHoraPartida = data.DataHoraPartida;
                FHUpdate.DataHoraTerminado = data.DataHoraTerminado;
                FHUpdate.DataHoraValidação = data.DataHoraValidacao;
                FHUpdate.DataHoraÚltimoEstado = data.DataHoraUltimoEstado;
                FHUpdate.DataIntegraçãoEmRh = data.DataIntegracaoEmRH;
                FHUpdate.DataIntegraçãoEmRhKm = data.DataIntegracaoEmRHKM;
                FHUpdate.DeslocaçãoForaConcelho = data.DeslocacaoForaConcelho;
                FHUpdate.DeslocaçãoPlaneada = data.DeslocacaoPlaneada;
                //FHUpdate.DistribuiçãoCustoFolhaDeHoras = data.;
                FHUpdate.Estado = data.Estado;
                FHUpdate.IntegradorEmRh = data.IntegradorEmRH;
                FHUpdate.IntegradorEmRhKm = data.IntegradorEmRHKM;
                FHUpdate.Matrícula = data.Matricula;
                //FHUpdate.MãoDeObraFolhaDeHoras = data.;
                FHUpdate.NomeEmpregado = data.EmpregadoNome;
                FHUpdate.NºEmpregado = data.EmpregadoNo;
                FHUpdate.NºFolhaDeHoras = data.FolhaDeHorasNo;
                FHUpdate.NºProjeto = data.ProjetoNo;
                //FHUpdate.NºProjetoNavigation = data.;
                FHUpdate.NºResponsável1 = data.Responsavel1No;
                FHUpdate.NºResponsável2 = data.Responsavel2No;
                FHUpdate.NºResponsável3 = data.Responsavel3No;
                FHUpdate.Observações = data.Observacoes;
                //FHUpdate.PercursosEAjudasCustoDespesasFolhaDeHoras = data.;
                //FHUpdate.PresençasFolhaDeHoras = data.;
                FHUpdate.Terminada = data.Terminada;
                FHUpdate.TerminadoPor = data.TerminadoPor;
                FHUpdate.TipoDeslocação = data.TipoDeslocacao;
                //abarros_
                //FHUpdate.UtilizadorCriação = data.UtilizadorCriacao;
                FHUpdate.UtilizadorModificação = User.Identity.Name;
                FHUpdate.Validado = data.Validado;
                FHUpdate.Validador = data.Validador;
                FHUpdate.Validadores = data.Validadores;
                FHUpdate.ValidadoresRhKm = data.ValidadoresRHKM;
                FHUpdate.Área = data.Area;

                DBFolhasDeHoras.Update(FHUpdate);
            }






            if (data != null)
            {
                FolhasDeHoras cFolhaDeHora = new FolhasDeHoras()
                {
                    NºFolhaDeHoras = data.FolhaDeHorasNo,
                    Área = Convert.ToInt16(data.AreaTexto),
                    NºProjeto = data.ProjetoNo,
                    NºEmpregado = data.EmpregadoNo,
                    DataHoraPartida = DateTime.Parse(string.Concat(data.DataPartidaTexto, " ", data.HoraPartidaTexto)),
                    DataHoraChegada = DateTime.Parse(string.Concat(data.DataChegadaTexto, " ", data.HoraChegadaTexto)),
                    TipoDeslocação = Convert.ToInt16(data.TipoDeslocacaoTexto),
                    CódigoTipoKmS = data.CodigoTipoKms,
                    DeslocaçãoForaConcelho = Convert.ToBoolean(data.DeslocacaoForaConcelhoTexto),
                    Validadores = data.Validadores,
                    Estado = Convert.ToUInt16(data.Estadotexto),
                    CriadoPor = User.Identity.Name,
                    DataHoraCriação = DateTime.Now,
                    DataHoraÚltimoEstado = DateTime.Now,
                    //UserCreation = User.Identity.Name,
                    DataHoraModificação = DateTime.Now,
                    UtilizadorModificação = User.Identity.Name,
                    NomeEmpregado = data.EmpregadoNo,
                    Matrícula = data.Matricula,
                    Terminada = data.Terminada,
                    TerminadoPor = User.Identity.Name,
                    DataHoraTerminado = DateTime.Now,
                    Validado = Convert.ToBoolean(data.ValidadoTexto),
                    DeslocaçãoPlaneada = Convert.ToBoolean(data.DeslocacaoPlaneadaTexto),
                    Observações = data.Observacoes,
                    NºResponsável1 = data.Responsavel1No,
                    NºResponsável2 = data.Responsavel2No,
                    NºResponsável3 = data.Responsavel3No,
                    ValidadoresRhKm = data.ValidadoresRHKM,
                    CódigoRegião = data.CodigoRegiao,
                    CódigoÁreaFuncional = data.CodigoAreaFuncional,
                    CódigoCentroResponsabilidade = data.CodigoCentroResponsabilidade,
                    Validador = data.Validador,
                    DataHoraValidação = DateTime.Now,
                    IntegradorEmRh = data.IntegradorEmRH,
                    DataIntegraçãoEmRh = DateTime.Now,
                    IntegradorEmRhKm = data.IntegradorEmRHKM,
                    DataIntegraçãoEmRhKm = DateTime.Now
                };

                DBFolhasDeHoras.Update(cFolhaDeHora);
                return Json(data);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult DeleteFolhaDeHoras([FromBody] FolhaDeHorasViewModel data)
        {

            if (data != null)
            {
                ErrorHandler result = new ErrorHandler();
                DBFolhasDeHoras.Delete(data.FolhaDeHorasNo);
                result = new ErrorHandler()
                {
                    eReasonCode = 0,
                    eMessage = "Folha de Horas removida com sucesso."
                };
                return Json(result);
            }
            return Json(false);
        }
        #endregion

        #region Job Ledger Entry

        public IActionResult MovimentosDeFolhaDeHora(String FolhaDeHoraNo)
        {
            ViewBag.FolhaDeHoraNo = FolhaDeHoraNo;
            return View();
        }

        #endregion

        #region PERCURSO

        [HttpPost]
        public JsonResult PercursoGetAllByFolhaHoraNoToList([FromBody] string FolhaHoraNo)
        {
            try
            {
                List<PercursosEAjudasCustoDespesasFolhaDeHorasViewModel> result = DBPercursosEAjudasCustoDespesasFolhaDeHoras.GetAllByPercursoToList(FolhaHoraNo);

                result.ForEach(x =>
                {
                    //x.AreaText = EnumerablesFixed.Areas.Where(y => y.Id == x.Area).FirstOrDefault().Value;
                    //x.TypeDeslocationText = EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == x.TypeDeslocation).FirstOrDefault().Value;
                    //if (x.DisplacementOutsideCity.Value) x.DisplacementOutsideCityText = "Sim"; else x.DisplacementOutsideCityText = "Não";
                    //x.StatusText = EnumerablesFixed.FolhaDeHoraStatus.Where(y => y.Id == x.Status).FirstOrDefault().Value;
                    //x.Validators = DBUserConfigurations.GetById(x.Validators).Nome;
                });

                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region AJUDA

        [HttpPost]
        public JsonResult AjudaGetAllByFolhaHoraNoToList([FromBody] string FolhaHoraNo)
        {
            try
            {
                List<PercursosEAjudasCustoDespesasFolhaDeHorasViewModel> result = DBPercursosEAjudasCustoDespesasFolhaDeHoras.GetAllByAjudaToList(FolhaHoraNo);

                result.ForEach(x =>
                {
                    //x.AreaText = EnumerablesFixed.Areas.Where(y => y.Id == x.Area).FirstOrDefault().Value;
                    //x.TypeDeslocationText = EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == x.TypeDeslocation).FirstOrDefault().Value;
                    //if (x.DisplacementOutsideCity.Value) x.DisplacementOutsideCityText = "Sim"; else x.DisplacementOutsideCityText = "Não";
                    //x.StatusText = EnumerablesFixed.FolhaDeHoraStatus.Where(y => y.Id == x.Status).FirstOrDefault().Value;
                    //x.Validators = DBUserConfigurations.GetById(x.Validators).Nome;
                });

                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        #endregion

        #region MÃO-DE-OBRA

        [HttpPost]
        public JsonResult MaoDeObraGetAllByFolhaHoraNoToList([FromBody] string FolhaHoraNo)
        {
            try
            {
                List<MaoDeObraFolhaDeHorasViewModel> result = DBMaoDeObraFolhaDeHoras.GetAllByMaoDeObraToList(FolhaHoraNo);

                result.ForEach(x =>
                {
                    //x.AreaText = EnumerablesFixed.Areas.Where(y => y.Id == x.Area).FirstOrDefault().Value;
                    //x.TypeDeslocationText = EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == x.TypeDeslocation).FirstOrDefault().Value;
                    //if (x.DisplacementOutsideCity.Value) x.DisplacementOutsideCityText = "Sim"; else x.DisplacementOutsideCityText = "Não";
                    //x.StatusText = EnumerablesFixed.FolhaDeHoraStatus.Where(y => y.Id == x.Status).FirstOrDefault().Value;
                    //x.Validators = DBUserConfigurations.GetById(x.Validators).Nome;
                });

                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region Presença

        [HttpPost]
        public JsonResult PresencasGetAllByFolhaHoraNoToList([FromBody] string FolhaHoraNo)
        {
            try
            {
                List<PresencasFolhaDeHorasViewModel> result = DBPresencasFolhaDeHoras.GetAllByPresencaToList(FolhaHoraNo);

                result.ForEach(x =>
                {
                    //x.AreaText = EnumerablesFixed.Areas.Where(y => y.Id == x.Area).FirstOrDefault().Value;
                    //x.TypeDeslocationText = EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == x.TypeDeslocation).FirstOrDefault().Value;
                    //if (x.DisplacementOutsideCity.Value) x.DisplacementOutsideCityText = "Sim"; else x.DisplacementOutsideCityText = "Não";
                    //x.StatusText = EnumerablesFixed.FolhaDeHoraStatus.Where(y => y.Id == x.Status).FirstOrDefault().Value;
                    //x.Validators = DBUserConfigurations.GetById(x.Validators).Nome;
                });

                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}
