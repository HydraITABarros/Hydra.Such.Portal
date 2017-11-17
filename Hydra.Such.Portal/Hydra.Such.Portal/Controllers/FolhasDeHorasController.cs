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
            if (id == null || id == "")
            {
                //Get Project Numeration
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

                DBFolhasDeHoras.Create(FH);

            }

            ViewBag.FolhaDeHorasNo = id == null ? "" : id;
            return View();
        }

        [HttpPost]
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
                            AreaTexto = FH.Área.ToString(),
                            ProjetoNo = FH.NºProjeto,
                            ProjetoDescricao = FH.ProjetoDescricao,
                            EmpregadoNo = FH.NºEmpregado,
                            EmpregadoNome = FH.NomeEmpregado,
                            DataHoraPartida = FH.DataHoraPartida,
                            DataPartidaTexto = FH.DataHoraPartida.Value.ToString("yyyy-MM-dd"),
                            HoraPartidaTexto = FH.DataHoraPartida.Value.ToString("HH:mm:ss"),
                            DataHoraChegada = FH.DataHoraChegada,
                            DataChegadaTexto = FH.DataHoraChegada.Value.ToString("yyyy-MM-dd"),
                            HoraChegadaTexto = FH.DataHoraChegada.Value.ToString("HH:mm:ss"),
                            TipoDeslocacao = FH.TipoDeslocação,
                            TipoDeslocacaoTexto = FH.TipoDeslocação.ToString(),
                            CodigoTipoKms = FH.CódigoTipoKmS,
                            Matricula = FH.Matrícula,
                            DeslocacaoForaConcelho = FH.DeslocaçãoForaConcelho,
                            DeslocacaoForaConcelhoTexto = FH.DeslocaçãoForaConcelho.ToString(),
                            DeslocacaoPlaneada = FH.DeslocaçãoPlaneada,
                            DeslocacaoPlaneadaTexto = FH.DeslocaçãoPlaneada.ToString(),
                            Terminada = FH.Terminada,
                            TerminadaTexto = FH.Terminada.ToString(),
                            Estado = FH.Estado,
                            Estadotexto = FH.Estado.ToString(),
                            CriadoPor = FH.CriadoPor,
                            DataHoraCriacao = FH.DataHoraCriação,
                            DataCriacaoTexto = FH.DataHoraCriação.Value.ToString("yyyy-MM-dd"),
                            HoraCriacaoTexto = FH.DataHoraCriação.Value.ToString("HH:mm:ss"),
                            CodigoRegiao = FH.CódigoRegião,
                            CodigoAreaFuncional = FH.CódigoÁreaFuncional,
                            CodigoCentroResponsabilidade = FH.CódigoCentroResponsabilidade,
                            TerminadoPor = FH.TerminadoPor,
                            DataHoraTerminado = FH.DataHoraTerminado,
                            DataTerminadoTexto = FH.DataHoraTerminado.Value.ToString("yyyy-MM-dd"),
                            HoraTerminadoTexto = FH.DataHoraTerminado.Value.ToString("HH:mm:ss"),
                            Validado = FH.Validado,
                            ValidadoTexto = FH.Validado.ToString(),
                            Validadores = FH.Validadores,
                            Validador = FH.Validador,
                            DataHoraValidacao = FH.DataHoraValidação,
                            DataValidacaoTexto = FH.DataHoraValidação.Value.ToString("yyyy-MM-dd"),
                            HoraValidacaoTexto = FH.DataHoraValidação.Value.ToString("HH:mm:ss"),
                            IntegradorEmRH = FH.IntegradorEmRh,
                            IntegradoresEmRH = FH.IntegradoresEmRh,
                            DataIntegracaoEmRH = FH.DataIntegraçãoEmRh,
                            DataIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh.Value.ToString("yyyy-MM-dd"),
                            HoraIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh.Value.ToString("HH:mm:ss"),
                            IntegradorEmRHKM = FH.IntegradorEmRhKm,
                            IntegradoresEmRHKM = FH.IntegradoresEmRhkm,
                            DataIntegracaoEmRHKM = FH.DataIntegraçãoEmRhKm,
                            DataIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm.Value.ToString("yyyy-MM-dd"),
                            HoraIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm.Value.ToString("HH:mm:ss"),
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
                            DataUltimoEstadoTexto = FH.DataHoraÚltimoEstado.Value.ToString("yyyy-MM-dd"),
                            HoraUltimoEstadoTexto = FH.DataHoraÚltimoEstado.Value.ToString("HH:mm:ss"),
                            UtilizadorModificacao = FH.UtilizadorModificação,
                            DataHoraModificacao = FH.DataHoraModificação,
                            DataModificacaoTexto = FH.DataHoraModificação.Value.ToString("yyyy-MM-dd"),
                            HoraModificacaoTexto = FH.DataHoraModificação.Value.ToString("HH:mm:ss")
                        };

                        Projetos cProject = DBProjects.GetById(FH.NºProjeto);
                        result.ProjetoDescricao = cProject.Descrição;

                        List<NAVEmployeeViewModel> employee = DBNAV2009Employees.GetAll(FH.NºEmpregado, _config.NAVDatabaseName, _config.NAVCompanyName);
                        result.EmpregadoNome = employee[0].Name;

                        //PERCURSO
                        result.FolhaDeHorasPercurso = DBPercursosEAjudasCustoDespesasFolhaDeHoras.GetAllByPercursoToList(data.FolhaDeHorasNo).Select(Percurso => new PercursosEAjudasCustoDespesasFolhaDeHorasViewModel()
                        {
                            FolhaDeHorasNo = Percurso.FolhaDeHorasNo,
                            CodPercursoAjuda = 1,
                            LinhaNo = Percurso.LinhaNo,
                            Origem = Percurso.Origem,
                            OrigemDescricao = Percurso.OrigemDescricao,
                            Destino = Percurso.Destino,
                            DestinoDescricao = Percurso.DestinoDescricao,
                            DataViagem = Percurso.DataViagem,
                            DataViagemTexto = Percurso.DataViagem.Value.ToString("yyyy-MM-dd"),
                            Justificacao = Percurso.Justificacao,
                            Distancia = Convert.ToDecimal(Percurso.Distancia),
                            DistanciaPrevista = Convert.ToDecimal(Percurso.DistanciaPrevista),
                            CustoUnitario = Convert.ToDecimal(Percurso.CustoUnitario),
                            CustoTotal = Convert.ToDecimal(Percurso.CustoTotal),
                            TipoCusto = Percurso.TipoCusto,
                            CodTipoCusto = Percurso.CodTipoCusto,
                            Descricao = Percurso.Descricao,
                            Quantidade = Convert.ToDecimal(Percurso.Quantidade),
                            PrecoUnitario = Convert.ToDecimal(Percurso.PrecoUnitario),
                            PrecoVenda = Convert.ToDecimal(Percurso.PrecoVenda),
                            RubricaSalarial = Percurso.RubricaSalarial,
                            UtilizadorCriacao = Percurso.UtilizadorCriacao,
                            DataHoraCriacao = Percurso.DataHoraCriacao,
                            DataHoraCriacaoTexto = Percurso.DataHoraCriacao.Value.ToString("yyyy-MM-dd"),
                            UtilizadorModificacao = Percurso.UtilizadorModificacao,
                            DataHoraModificacao = Percurso.DataHoraModificacao,
                            DataHoraModificacaoTexto = Percurso.DataHoraModificacao.Value.ToString("yyyy-MM-dd")
                        }).ToList();

                        //AJUDA DE CUSTO/DESPESA
                        result.FolhaDeHorasAjuda = DBPercursosEAjudasCustoDespesasFolhaDeHoras.GetAllByAjudaToList(data.FolhaDeHorasNo).Select(Ajuda => new PercursosEAjudasCustoDespesasFolhaDeHorasViewModel()
                        {
                            FolhaDeHorasNo = Ajuda.FolhaDeHorasNo,
                            CodPercursoAjuda = 2,
                            LinhaNo = Ajuda.LinhaNo,
                            Origem = Ajuda.Origem,
                            OrigemDescricao = Ajuda.OrigemDescricao,
                            Destino = Ajuda.Destino,
                            DestinoDescricao = Ajuda.DestinoDescricao,
                            DataViagem = Ajuda.DataViagem,
                            DataViagemTexto = Ajuda.DataViagem.Value.ToString("yyyy-MM-dd"),
                            Justificacao = Ajuda.Justificacao,
                            Distancia = Convert.ToDecimal(Ajuda.Distancia),
                            DistanciaPrevista = Convert.ToDecimal(Ajuda.DistanciaPrevista),
                            CustoUnitario = Convert.ToDecimal(Ajuda.CustoUnitario),
                            CustoTotal = Convert.ToDecimal(Ajuda.CustoTotal),
                            TipoCusto = Ajuda.TipoCusto,
                            CodTipoCusto = Ajuda.CodTipoCusto,
                            Descricao = Ajuda.Descricao,
                            Quantidade = Convert.ToDecimal(Ajuda.Quantidade),
                            PrecoUnitario = Convert.ToDecimal(Ajuda.PrecoUnitario),
                            PrecoVenda = Convert.ToDecimal(Ajuda.PrecoVenda),
                            RubricaSalarial = Ajuda.RubricaSalarial,
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
                            HoraInicioTexto = MaoDeObra.HoraInicio,
                            HorarioAlmoco = MaoDeObra.HorarioAlmoco,
                            HoraFim = MaoDeObra.HoraFim,
                            HoraFimTexto = MaoDeObra.HoraFimTexto,
                            HorarioJantar = MaoDeObra.HorarioJantar,
                            CodigoFamiliaRecurso = MaoDeObra.CodigoFamiliaRecurso,
                            CodigoTipoOM = MaoDeObra.CodigoTipoOM,
                            HorasNo = MaoDeObra.HorasNo,
                            HorasNoTexto = MaoDeObra.HorasNoTexto,
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
                        ProjetoDescricao = data.ProjetoDescricao,
                        NºEmpregado = data.EmpregadoNo,
                        NomeEmpregado = data.EmpregadoNome,
                        DataHoraPartida = DateTime.Parse(string.Concat(data.DataPartidaTexto, " ", data.HoraPartidaTexto)),
                        DataHoraChegada = DateTime.Parse(string.Concat(data.DataChegadaTexto, " ", data.HoraChegadaTexto)),
                        TipoDeslocação = data.TipoDeslocacao,
                        CódigoTipoKmS = data.CodigoTipoKms,
                        Matrícula = data.Matricula,
                        DeslocaçãoForaConcelho = Convert.ToBoolean(data.DeslocacaoForaConcelhoTexto),
                        DeslocaçãoPlaneada = Convert.ToBoolean(data.DeslocacaoPlaneadaTexto),
                        Terminada = data.Terminada,
                        Estado = 1,
                        CriadoPor = User.Identity.Name,
                        DataHoraCriação = DateTime.Now,
                        CódigoRegião = data.CodigoRegiao,
                        CódigoÁreaFuncional = data.CodigoAreaFuncional,
                        CódigoCentroResponsabilidade = data.CodigoCentroResponsabilidade,
                        TerminadoPor = User.Identity.Name,
                        DataHoraTerminado = DateTime.Now,
                        Validado = Convert.ToBoolean(data.ValidadoTexto),
                        Validadores = User.Identity.Name,
                        Validador = User.Identity.Name,
                        DataHoraValidação = DateTime.Now,
                        IntegradorEmRh = User.Identity.Name,
                        IntegradoresEmRh = data.IntegradoresEmRH,
                        DataIntegraçãoEmRh = DateTime.Now,
                        IntegradorEmRhKm = User.Identity.Name,
                        IntegradoresEmRhkm = data.IntegradoresEmRHKM,
                        DataIntegraçãoEmRhKm = DateTime.Now,
                        CustoTotalAjudaCusto = data.CustoTotalAjudaCusto,
                        CustoTotalHoras = data.CustoTotalHoras,
                        CustoTotalKm = data.CustoTotalKM,
                        NumTotalKm = data.NumTotalKM,
                        Observações = data.Observacoes,
                        NºResponsável1 = User.Identity.Name,
                        NºResponsável2 = User.Identity.Name,
                        NºResponsável3 = User.Identity.Name,
                        ValidadoresRhKm = User.Identity.Name,
                        DataHoraÚltimoEstado = DateTime.Now,
                        UtilizadorModificação = User.Identity.Name,
                        DataHoraModificação = DateTime.Now
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

        [HttpPost]
        public JsonResult CreatePercurso([FromBody] PercursosEAjudasCustoDespesasFolhaDeHorasViewModel data)
        {
            bool result = false;
            try
            {
                int noPercursos;

                noPercursos = DBPercursosEAjudasCustoDespesasFolhaDeHoras.GetAllByPercursoToList(data.FolhaDeHorasNo).Count;

                if (noPercursos == 0)
                {
                    PercursosEAjudasCustoDespesasFolhaDeHoras Percurso1 = new PercursosEAjudasCustoDespesasFolhaDeHoras();

                    Percurso1.NºFolhaDeHoras = data.FolhaDeHorasNo;
                    Percurso1.CodPercursoAjuda = 1; //PERCURSO
                    Percurso1.Origem = data.Origem;
                    Percurso1.OrigemDescricao = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.Origem);
                    Percurso1.Destino = data.Destino;
                    Percurso1.DestinoDescricao = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.Destino);
                    Percurso1.DataViagem = Convert.ToDateTime(data.DataViagem);
                    Percurso1.Justificação = data.Justificacao;
                    Percurso1.Distância = Convert.ToDecimal(data.Distancia);
                    Percurso1.DistanciaPrevista = DBDistanciaFh.GetDistanciaPrevista(data.Origem, data.Destino);
                    Percurso1.CustoUnitário = Convert.ToDecimal(data.CustoUnitario);
                    Percurso1.CustoTotal = Convert.ToDecimal(Convert.ToDecimal(data.Distancia) * Convert.ToDecimal(data.CustoUnitario));
                    Percurso1.UtilizadorCriação = User.Identity.Name;
                    Percurso1.DataHoraCriação = DateTime.Now;
                    Percurso1.UtilizadorModificação = User.Identity.Name;
                    Percurso1.DataHoraModificação = DateTime.Now;

                    var dbCreateResult1 = DBPercursosEAjudasCustoDespesasFolhaDeHoras.CreatePercurso(Percurso1);


                    PercursosEAjudasCustoDespesasFolhaDeHoras Percurso2 = new PercursosEAjudasCustoDespesasFolhaDeHoras();

                    Percurso2.NºFolhaDeHoras = data.FolhaDeHorasNo;
                    Percurso2.CodPercursoAjuda = 1; //PERCURSO
                    Percurso2.Origem = data.Destino;
                    Percurso2.OrigemDescricao = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.Destino);
                    Percurso2.Destino = data.Origem;
                    Percurso2.DestinoDescricao = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.Origem);
                    Percurso2.DataViagem = Convert.ToDateTime(data.DataViagem);
                    Percurso2.Justificação = data.Justificacao;
                    Percurso2.Distância = Convert.ToDecimal(data.Distancia);
                    Percurso2.DistanciaPrevista = DBDistanciaFh.GetDistanciaPrevista(data.Origem, data.Destino);
                    Percurso2.CustoUnitário = Convert.ToDecimal(data.CustoUnitario);
                    Percurso2.CustoTotal = Convert.ToDecimal(Convert.ToDecimal(data.Distancia) * Convert.ToDecimal(data.CustoUnitario));
                    Percurso2.UtilizadorCriação = User.Identity.Name;
                    Percurso2.DataHoraCriação = DateTime.Now;
                    Percurso2.UtilizadorModificação = User.Identity.Name;
                    Percurso2.DataHoraModificação = DateTime.Now;

                    var dbCreateResult2 = DBPercursosEAjudasCustoDespesasFolhaDeHoras.CreatePercurso(Percurso2);

                    if (dbCreateResult1 != null && dbCreateResult2 != null)
                        result = true;
                    else
                        result = false;
                }
                else
                {
                    PercursosEAjudasCustoDespesasFolhaDeHoras Percurso1 = new PercursosEAjudasCustoDespesasFolhaDeHoras();

                    Percurso1.NºFolhaDeHoras = data.FolhaDeHorasNo;
                    Percurso1.CodPercursoAjuda = 1; //PERCURSO
                    Percurso1.Origem = data.Origem;
                    Percurso1.OrigemDescricao = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.Origem);
                    Percurso1.Destino = data.Destino;
                    Percurso1.DestinoDescricao = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.Destino);
                    Percurso1.DataViagem = Convert.ToDateTime(data.DataViagem);
                    Percurso1.Justificação = data.Justificacao;
                    Percurso1.Distância = Convert.ToDecimal(data.Distancia);
                    Percurso1.DistanciaPrevista = DBDistanciaFh.GetDistanciaPrevista(data.Origem, data.Destino);
                    Percurso1.CustoUnitário = Convert.ToDecimal(data.CustoUnitario);
                    Percurso1.CustoTotal = Convert.ToDecimal(Convert.ToDecimal(data.Distancia) * Convert.ToDecimal(data.CustoUnitario));
                    Percurso1.UtilizadorCriação = User.Identity.Name;
                    Percurso1.DataHoraCriação = DateTime.Now;
                    Percurso1.UtilizadorModificação = User.Identity.Name;
                    Percurso1.DataHoraModificação = DateTime.Now;

                    var dbCreateResult1 = DBPercursosEAjudasCustoDespesasFolhaDeHoras.CreatePercurso(Percurso1);

                    if (dbCreateResult1 != null)
                        result = true;
                    else
                        result = false;
                }
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdatePercurso([FromBody] FolhaDeHorasViewModel data)
        {
            bool result = false;
            try
            {
                data.FolhaDeHorasPercurso.ForEach(x =>
                {
                    DBPercursosEAjudasCustoDespesasFolhaDeHoras.UpdatePercurso(new PercursosEAjudasCustoDespesasFolhaDeHoras()
                    {
                        NºFolhaDeHoras = x.FolhaDeHorasNo,
                        NºLinha = Convert.ToInt32(x.LinhaNo),
                        CodPercursoAjuda = 1, //PERCURSO
                        Origem = x.Destino,
                        OrigemDescricao = DBOrigemDestinoFh.GetOrigemDestinoDescricao(x.Destino),
                        Destino = x.Origem,
                        DestinoDescricao = DBOrigemDestinoFh.GetOrigemDestinoDescricao(x.Origem),
                        DataViagem = Convert.ToDateTime(x.DataViagem),
                        Justificação = x.Justificacao,
                        Distância = Convert.ToDecimal(x.Distancia),
                        DistanciaPrevista = DBDistanciaFh.GetDistanciaPrevista(x.Origem, x.Destino),
                        CustoUnitário = Convert.ToDecimal(x.CustoUnitario),
                        CustoTotal = Convert.ToDecimal(Convert.ToDecimal(x.Distancia) * Convert.ToDecimal(x.CustoUnitario)),
                        UtilizadorCriação = x.UtilizadorCriacao,
                        DataHoraCriação = x.DataHoraCriacao,
                        UtilizadorModificação = User.Identity.Name,
                        DataHoraModificação = DateTime.Now
                    });
                });

                result = true;
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeletePercurso([FromBody] int linhaNo)
        {
            bool result = false;
            try
            {
                bool dbDeleteResult = DBPercursosEAjudasCustoDespesasFolhaDeHoras.DeletePercurso(Convert.ToInt32(linhaNo));

                result = dbDeleteResult;
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

        [HttpPost]
        public JsonResult CreateAjuda([FromBody] PercursosEAjudasCustoDespesasFolhaDeHorasViewModel data)
        {
            bool result = false;
            try
            {
                PercursosEAjudasCustoDespesasFolhaDeHoras Ajuda = new PercursosEAjudasCustoDespesasFolhaDeHoras();

                Ajuda.NºFolhaDeHoras = data.FolhaDeHorasNo;
                Ajuda.CodPercursoAjuda = 2; //AJUDA
                Ajuda.TipoCusto = data.TipoCusto;
                Ajuda.CodTipoCusto = data.CodTipoCusto;
                Ajuda.Descrição = data.Descricao;
                Ajuda.Quantidade = data.Quantidade;
                Ajuda.CustoUnitário = data.CustoUnitario;
                Ajuda.CustoTotal = data.Quantidade * data.CustoUnitario;
                Ajuda.PreçoUnitário = data.PrecoUnitario;
                Ajuda.PrecoVenda = data.Quantidade * data.PrecoUnitario;
                Ajuda.DataViagem = data.DataViagem;
                Ajuda.Justificação = data.Justificacao;
                Ajuda.UtilizadorCriação = User.Identity.Name;
                Ajuda.DataHoraCriação = DateTime.Now;
                Ajuda.UtilizadorModificação = User.Identity.Name;
                Ajuda.DataHoraModificação = DateTime.Now;

                var dbCreateResult = DBPercursosEAjudasCustoDespesasFolhaDeHoras.CreateAjuda(Ajuda);

                if (dbCreateResult != null)
                    result = true;
                else
                    result = false;
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateAjuda([FromBody] FolhaDeHorasViewModel data)
        {
            bool result = false;
            try
            {
                data.FolhaDeHorasAjuda.ForEach(x =>
                {
                    DBPercursosEAjudasCustoDespesasFolhaDeHoras.UpdateAjuda(new PercursosEAjudasCustoDespesasFolhaDeHoras()
                    {
                        NºFolhaDeHoras = x.FolhaDeHorasNo,
                        NºLinha = Convert.ToInt32(x.LinhaNo),
                        CodPercursoAjuda = 2, //AJUDA
                        TipoCusto = x.TipoCusto,
                        CodTipoCusto = x.CodTipoCusto,
                        Descrição = x.Descricao,
                        Quantidade = x.Quantidade,
                        CustoUnitário = x.CustoUnitario,
                        CustoTotal = x.Quantidade * x.CustoUnitario,
                        PreçoUnitário = x.PrecoUnitario,
                        PrecoVenda = x.Quantidade * x.PrecoUnitario,
                        DataViagem = x.DataViagem,
                        Justificação = x.Justificacao,
                        UtilizadorCriação = x.UtilizadorCriacao,
                        DataHoraCriação = x.DataHoraCriacao,
                        UtilizadorModificação = User.Identity.Name,
                        DataHoraModificação = DateTime.Now,
                    });
                });

                result = true;
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteAjuda([FromBody] int linhaNo)
        {
            bool result = false;
            try
            {
                bool dbDeleteResult = DBPercursosEAjudasCustoDespesasFolhaDeHoras.DeleteAjuda(Convert.ToInt32(linhaNo));

                result = dbDeleteResult;
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

        [HttpPost]
        public JsonResult CreateMaoDeObra([FromBody] MaoDeObraFolhaDeHorasViewModel data)
        {
            bool result = false;
            try
            {
                MãoDeObraFolhaDeHoras MaoDeObra = new MãoDeObraFolhaDeHoras();

                MaoDeObra.NºFolhaDeHoras = data.FolhaDeHorasNo;
                MaoDeObra.Date = data.Date;
                MaoDeObra.NºProjeto = data.ProjetoNo;
                MaoDeObra.NºEmpregado = data.EmpregadoNo;
                MaoDeObra.CódigoTipoTrabalho = data.CodigoTipoTrabalho;
                MaoDeObra.HoraInício = TimeSpan.Parse(data.HoraInicio);
                MaoDeObra.HorárioAlmoço = data.HorarioAlmoco;
                MaoDeObra.HoraFim = TimeSpan.Parse(data.HoraFim);
                MaoDeObra.HorárioJantar = data.HorarioJantar;
                MaoDeObra.CódigoFamíliaRecurso = data.CodigoFamiliaRecurso;
                MaoDeObra.CódigoTipoOm = data.CodigoTipoOM;
                //MaoDeObra.NºDeHoras = TimeSpan.Parse(data.HoraFim) - TimeSpan.Parse(data.HoraInicio); //TimeSpan.Parse(data.HorasNo);
                MaoDeObra.CustoUnitárioDireto = data.CustoUnitarioDireto;
                MaoDeObra.CodigoCentroResponsabilidade = data.CodigoCentroResponsabilidade;
                MaoDeObra.PreçoTotal = data.PrecoTotal;
                MaoDeObra.Descricao = data.Descricao;
                MaoDeObra.NºRecurso = data.RecursoNo;
                MaoDeObra.CódUnidadeMedida = data.CodigoUnidadeMedida;
                MaoDeObra.PreçoDeCusto = data.PrecoDeCusto;
                MaoDeObra.PreçoDeVenda = data.PrecoDeVenda;
                MaoDeObra.UtilizadorCriação = User.Identity.Name;
                MaoDeObra.DataHoraCriação = DateTime.Now;
                MaoDeObra.UtilizadorModificação = User.Identity.Name;
                MaoDeObra.DataHoraModificação = DateTime.Now;

                var dbCreateResult = DBMaoDeObraFolhaDeHoras.Create(MaoDeObra);

                if (dbCreateResult != null)
                    result = true;
                else
                    result = false;
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateMaoDeObra([FromBody] FolhaDeHorasViewModel data)
        {
            bool result = false;

            
            try
            {
                data.FolhaDeHorasMaoDeObra.ForEach(x =>
                {
                    //TimeSpan Hora;
                    //TimeSpan.TryParse(x.HoraInicioTexto, out Hora);

                    DBMaoDeObraFolhaDeHoras.Update(new MãoDeObraFolhaDeHoras()
                    {
                        NºFolhaDeHoras = x.FolhaDeHorasNo,
                        NºLinha = Convert.ToInt32(x.LinhaNo),
                        Date = x.Date,
                        NºProjeto = x.ProjetoNo,
                        NºEmpregado = x.EmpregadoNo,
                        CódigoTipoTrabalho = x.CodigoTipoTrabalho,
                        HoraInício = TimeSpan.Parse(x.HoraInicioTexto),
                        HorárioAlmoço = x.HorarioAlmoco,
                        HoraFim = TimeSpan.Parse(x.HoraFimTexto),
                        HorárioJantar = x.HorarioJantar,
                        CódigoFamíliaRecurso = x.CodigoFamiliaRecurso,
                        CódigoTipoOm = x.CodigoTipoOM,
                        NºDeHoras = TimeSpan.Parse(x.HorasNoTexto),
                        CustoUnitárioDireto = x.CustoUnitarioDireto,
                        CodigoCentroResponsabilidade = x.CodigoCentroResponsabilidade,
                        PreçoTotal = x.PrecoTotal,
                        Descricao = x.Descricao,
                        NºRecurso = x.RecursoNo,
                        CódUnidadeMedida = x.CodigoUnidadeMedida,
                        PreçoDeCusto = x.PrecoDeCusto,
                        PreçoDeVenda = x.PrecoDeVenda,
                        UtilizadorCriação = x.UtilizadorCriacao,
                        DataHoraCriação = x.DataHoraCriacao,
                        UtilizadorModificação = User.Identity.Name,
                        DataHoraModificação = DateTime.Now,
                    });
                });

                result = true;
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteMaoDeObra([FromBody] int linhaNo)
        {
            bool result = false;
            try
            {
                bool dbDeleteResult = DBMaoDeObraFolhaDeHoras.Delete(linhaNo);

                result = dbDeleteResult;
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
