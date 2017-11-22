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
                FH.AreaTexto = FH.Area == null ? "" : EnumerablesFixed.Areas.Where(y => y.Id == FH.Area).FirstOrDefault().Value;
                FH.TipoDeslocacaoTexto = FH.TipoDeslocacao == null ? "" : EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == FH.TipoDeslocacao).FirstOrDefault().Value;
                FH.DeslocacaoForaConcelhoTexto = FH.DeslocacaoForaConcelho == null ? "" : EnumerablesFixed.FolhaDeHoraDisplacementOutsideCity.Where(y => y.Id == Convert.ToInt32(FH.DeslocacaoForaConcelho)).FirstOrDefault().Value;
                FH.Estadotexto = FH.Estado == null ? "" : EnumerablesFixed.FolhaDeHoraStatus.Where(y => y.Id == FH.Estado).FirstOrDefault().Value;
                FH.Validadores = FH.Validadores == "" ? "" : DBUserConfigurations.GetById(FH.Validadores).Nome;
            });

            return Json(result);
        }
        #endregion

        #region Details
        //public IActionResult Detalhes(string id)
        public IActionResult Detalhes([FromQuery] string FHNo, [FromQuery] int area)
        {
            //String id = HttpContext.Request.Query["FHNo"].ToString();
            //String area = HttpContext.Request.Query["area"].ToString();
            string id = "";

            if (FHNo == null || FHNo == "")
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

                FH.Área = area;
                FH.CriadoPor = User.Identity.Name;
                FH.DataHoraCriação = DateTime.Now;
                FH.UtilizadorModificação = User.Identity.Name;
                FH.DataHoraModificação = DateTime.Now;

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
                            AreaTexto = FH.Área == null ? "" : FH.Área.ToString(),
                            ProjetoNo = FH.NºProjeto,
                            ProjetoDescricao = FH.ProjetoDescricao,
                            EmpregadoNo = FH.NºEmpregado,
                            EmpregadoNome = FH.NomeEmpregado,
                            DataHoraPartida = FH.DataHoraPartida,
                            DataPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("yyyy-MM-dd"),
                            HoraPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("HH:mm:ss"),
                            DataHoraChegada = FH.DataHoraChegada,
                            DataChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("yyyy-MM-dd"),
                            HoraChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("HH:mm:ss"),
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
                            HoraCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("HH:mm:ss"),
                            CodigoRegiao = FH.CódigoRegião,
                            CodigoAreaFuncional = FH.CódigoÁreaFuncional,
                            CodigoCentroResponsabilidade = FH.CódigoCentroResponsabilidade,
                            TerminadoPor = FH.TerminadoPor,
                            DataHoraTerminado = FH.DataHoraTerminado,
                            DataTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("yyyy-MM-dd"),
                            HoraTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("HH:mm:ss"),
                            Validado = FH.Validado,
                            ValidadoTexto = FH.Validado == null ? "" : FH.Validado.ToString(),
                            Validadores = FH.Validadores == null ? "" : FH.Validadores,
                            Validador = FH.Validador,
                            DataHoraValidacao = FH.DataHoraValidação,
                            DataValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("yyyy-MM-dd"),
                            HoraValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("HH:mm:ss"),
                            IntegradorEmRH = FH.IntegradorEmRh,
                            IntegradoresEmRH = FH.IntegradoresEmRh,
                            DataIntegracaoEmRH = FH.DataIntegraçãoEmRh,
                            DataIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("yyyy-MM-dd"),
                            HoraIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("HH:mm:ss"),
                            IntegradorEmRHKM = FH.IntegradorEmRhKm,
                            IntegradoresEmRHKM = FH.IntegradoresEmRhkm,
                            DataIntegracaoEmRHKM = FH.DataIntegraçãoEmRhKm,
                            DataIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("yyyy-MM-dd"),
                            HoraIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("HH:mm:ss"),
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
                            HoraUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("HH:mm:ss"),
                            UtilizadorModificacao = FH.UtilizadorModificação,
                            DataHoraModificacao = FH.DataHoraModificação,
                            DataModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("yyyy-MM-dd"),
                            HoraModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("HH:mm:ss")
                        };

                        Projetos cProject = DBProjects.GetById(FH.NºProjeto);
                        result.ProjetoDescricao = FH.NºProjeto == null ? "" : cProject.Descrição;

                        List<NAVEmployeeViewModel> employee = DBNAV2009Employees.GetAll(FH.NºEmpregado, _config.NAVDatabaseName, _config.NAVCompanyName);
                        result.EmpregadoNome = FH.NºEmpregado == null ? "" : employee[0].Name;

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
        public JsonResult UpdateFolhaDeHoras([FromBody] FolhaDeHorasViewModel data)
        {
            bool result = false;
            try
            {
                DBFolhasDeHoras.Update(new FolhasDeHoras()
                {
                    NºFolhaDeHoras = data.FolhaDeHorasNo,
                    Área = data.Area,
                    NºProjeto = data.ProjetoNo,
                    NºEmpregado = data.EmpregadoNo,
                    DataHoraPartida = DateTime.Parse(string.Concat(data.DataPartidaTexto, " ", data.HoraPartidaTexto)),
                    DataHoraChegada = DateTime.Parse(string.Concat(data.DataChegadaTexto, " ", data.HoraChegadaTexto)),
                    TipoDeslocação = Convert.ToInt16(data.TipoDeslocacaoTexto),
                    CódigoTipoKmS = data.CodigoTipoKms,
                    DeslocaçãoForaConcelho = Convert.ToBoolean(data.DeslocacaoForaConcelhoTexto),
                    Validadores = data.Validadores,
                    Estado = Convert.ToUInt16(data.Estadotexto),
                    CriadoPor = data.CriadoPor,
                    DataHoraCriação = DateTime.Parse(string.Concat(data.DataCriacaoTexto, " ", data.HoraCriacaoTexto)),
                    DataHoraÚltimoEstado = data.DataHoraUltimoEstado,
                    DataHoraModificação = DateTime.Now,
                    UtilizadorModificação = User.Identity.Name,
                    NomeEmpregado = data.EmpregadoNo,
                    Matrícula = data.Matricula,
                    Terminada = data.Terminada,
                    TerminadoPor = data.TerminadoPor,
                    DataHoraTerminado = data.DataHoraTerminado,
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
                    DataHoraValidação = data.DataHoraValidacao,
                    IntegradorEmRh = data.IntegradorEmRH,
                    DataIntegraçãoEmRh = data.DataIntegracaoEmRH,
                    IntegradorEmRhKm = data.IntegradorEmRHKM,
                    DataIntegraçãoEmRhKm = data.DataIntegracaoEmRHKM
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
                List<LinhasFolhaHorasViewModel> result = DBLinhasFolhaHoras.GetAllByPercursoToList(FolhaHoraNo);

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
        public JsonResult CreatePercurso([FromBody] LinhasFolhaHorasViewModel data)
        {
            bool result = false;
            try
            {
                int noPercursos;
                noPercursos = DBLinhasFolhaHoras.GetPercursoByFolhaHoraNo(data.NoFolhaHoras).Count();

                int noLinha;
                noLinha = DBLinhasFolhaHoras.GetMaxPercursoByFolhaHoraNo(data.NoFolhaHoras);

                if (noPercursos == 0)
                {
                    LinhasFolhaHoras Percurso1 = new LinhasFolhaHoras();

                    Percurso1.NoFolhaHoras = data.NoFolhaHoras;
                    Percurso1.NoLinha = noLinha + 1;
                    Percurso1.TipoCusto = 1; //PERCURSO
                    Percurso1.CodOrigem = data.CodOrigem;
                    Percurso1.DescricaoOrigem = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.CodOrigem);
                    Percurso1.CodDestino = data.CodDestino;
                    Percurso1.DescricaoDestino = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.CodDestino);
                    Percurso1.DataDespesa = data.DataDespesa;
                    Percurso1.Observacao = data.Observacao;
                    Percurso1.Distancia = data.Distancia;
                    Percurso1.DistanciaPrevista = DBDistanciaFh.GetDistanciaPrevista(data.CodOrigem, data.CodDestino);
                    Percurso1.CustoUnitario = data.CustoUnitario;
                    Percurso1.CustoTotal = data.Distancia * data.CustoUnitario;
                    Percurso1.UtilizadorCriacao = User.Identity.Name;
                    Percurso1.DataHoraCriacao = DateTime.Now;
                    Percurso1.UtilizadorModificacao = User.Identity.Name;
                    Percurso1.DataHoraModificacao = DateTime.Now;

                    var dbCreateResult1 = DBLinhasFolhaHoras.CreatePercurso(Percurso1);


                    LinhasFolhaHoras Percurso2 = new LinhasFolhaHoras();

                    Percurso2.NoFolhaHoras = data.NoFolhaHoras;
                    Percurso2.NoLinha = noLinha + 2;
                    Percurso2.TipoCusto = 1; //PERCURSO
                    Percurso2.CodOrigem = data.CodOrigem;
                    Percurso2.DescricaoOrigem = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.CodOrigem);
                    Percurso2.CodDestino = data.CodDestino;
                    Percurso2.DescricaoDestino = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.CodDestino);
                    Percurso2.DataDespesa = data.DataDespesa;
                    Percurso2.Observacao = data.Observacao;
                    Percurso2.Distancia = data.Distancia;
                    Percurso2.DistanciaPrevista = DBDistanciaFh.GetDistanciaPrevista(data.CodOrigem, data.CodDestino);
                    Percurso2.CustoUnitario = data.CustoUnitario;
                    Percurso2.CustoTotal = data.Distancia * data.CustoUnitario;
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
                    Percurso1.NoLinha = noLinha + 1;
                    Percurso1.TipoCusto = 1; //PERCURSO
                    Percurso1.CodOrigem = data.CodOrigem;
                    Percurso1.DescricaoOrigem = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.CodOrigem);
                    Percurso1.CodDestino = data.CodDestino;
                    Percurso1.DescricaoDestino = DBOrigemDestinoFh.GetOrigemDestinoDescricao(data.CodDestino);
                    Percurso1.DataDespesa = data.DataDespesa;
                    Percurso1.Observacao = data.Observacao;
                    Percurso1.Distancia = data.Distancia;
                    Percurso1.DistanciaPrevista = DBDistanciaFh.GetDistanciaPrevista(data.CodOrigem, data.CodDestino);
                    Percurso1.CustoUnitario = data.CustoUnitario;
                    Percurso1.CustoTotal = data.Distancia * data.CustoUnitario;
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
                    DBLinhasFolhaHoras.UpdatePercurso(new LinhasFolhaHoras()
                    {
                        NoFolhaHoras = x.NoFolhaHoras,
                        NoLinha = x.NoLinha,
                        TipoCusto = 1, //PERCURSO
                        CodOrigem = x.CodOrigem,
                        DescricaoOrigem = DBOrigemDestinoFh.GetOrigemDestinoDescricao(x.CodOrigem),
                        CodDestino = x.CodDestino,
                        DescricaoDestino = DBOrigemDestinoFh.GetOrigemDestinoDescricao(x.CodDestino),
                        DataDespesa = x.DataDespesa,
                        Observacao = x.Observacao,
                        Distancia = x.Distancia,
                        DistanciaPrevista = DBDistanciaFh.GetDistanciaPrevista(x.CodOrigem, x.CodDestino),
                        CustoUnitario = x.CustoUnitario,
                        CustoTotal = x.Distancia * x.CustoUnitario,
                        UtilizadorCriacao = x.UtilizadorCriacao,
                        DataHoraCriacao = x.DataHoraCriacao,
                        UtilizadorModificacao = User.Identity.Name,
                        DataHoraModificacao = DateTime.Now
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
                bool dbDeleteResult = DBLinhasFolhaHoras.DeletePercurso(linhaNo);

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
                List<LinhasFolhaHorasViewModel> result = DBLinhasFolhaHoras.GetAllByAjudaToList(FolhaHoraNo);

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
        public JsonResult CreateAjuda([FromBody] LinhasFolhaHorasViewModel data)
        {
            bool result = false;
            try
            {
                int noLinha;
                noLinha = DBLinhasFolhaHoras.GetMaxAjudaByFolhaHoraNo(data.NoFolhaHoras);

                LinhasFolhaHoras Ajuda = new LinhasFolhaHoras();

                Ajuda.NoFolhaHoras = data.NoFolhaHoras;
                Ajuda.NoLinha = noLinha + 1;
                Ajuda.TipoCusto = data.TipoCusto;
                Ajuda.CodTipoCusto = data.CodTipoCusto;
                Ajuda.Quantidade = data.Quantidade;
                Ajuda.CustoUnitario = data.CustoUnitario;
                Ajuda.CustoTotal = data.Quantidade * data.CustoUnitario;
                Ajuda.PrecoUnitario = data.PrecoUnitario;
                Ajuda.PrecoVenda = data.Quantidade * data.PrecoUnitario;
                Ajuda.DataDespesa = data.DataDespesa;
                Ajuda.Observacao = data.Observacao;
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
                    DBLinhasFolhaHoras.UpdateAjuda(new LinhasFolhaHoras()
                    {
                        NoFolhaHoras = x.NoFolhaHoras,
                        NoLinha = x.NoLinha,
                        TipoCusto = x.TipoCusto,
                        CodTipoCusto = x.CodTipoCusto,
                        Quantidade = x.Quantidade,
                        CustoUnitario = x.CustoUnitario,
                        CustoTotal = x.Quantidade * x.CustoUnitario,
                        PrecoUnitario = x.PrecoUnitario,
                        PrecoVenda = x.Quantidade * x.PrecoUnitario,
                        DataDespesa = x.DataDespesa,
                        Observacao = x.Observacao,
                        UtilizadorCriacao = x.UtilizadorCriacao,
                        DataHoraCriacao = x.DataHoraCriacao,
                        UtilizadorModificacao = User.Identity.Name,
                        DataHoraModificacao = DateTime.Now,
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
                bool dbDeleteResult = DBLinhasFolhaHoras.DeleteAjuda(Convert.ToInt32(linhaNo));

                result = dbDeleteResult;
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult CalcularAjudasCusto([FromBody] FolhaDeHorasViewModel data)
        {
            bool result = false;
            try
            {
                decimal NoDias = 0;
                int noLinha;

                //APAGAR TODOS OS REGISTOS DAS LINHAS DE FOLHAS DE HORAS ONDE Calculo_Automatico = true

                List<LinhasFolhaHoras> LinhasFH = DBLinhasFolhaHoras.GetAjudaByFolhaHoraNo(data.FolhaDeHorasNo).Where(x => (x.NoFolhaHoras == data.FolhaDeHorasNo) && (x.CalculoAutomatico == true)).ToList();
                LinhasFH.ForEach(x =>
                {
                    DBLinhasFolhaHoras.DeleteAjuda(x.NoLinha);
                });





                List<ConfiguracaoAjudaCusto> AjudaCusto = DBConfiguracaoAjudaCusto.GetAll().Where(x =>
                    (x.DataChegadaDataPartida == false) &&
                    (x.DistanciaMinima <= GetSUMDistancia(data.FolhaDeHorasNo)) &&
                    (x.TipoCusto != 1)
                    ).ToList();

                NoDias = Convert.ToInt32((Convert.ToDateTime(data.DataHoraChegada) - Convert.ToDateTime(data.DataHoraPartida)).TotalDays);

                AjudaCusto.ForEach(x =>
                {
                    if (x.CodigoRefCusto == 1) //ALMOCO
                    {
                        if (TimeSpan.Parse(data.HoraPartidaTexto) <= x.LimiteHoraPartida) NoDias = NoDias + 1;

                        if ((TimeSpan.Parse(data.HoraChegadaTexto) >= x.LimiteHoraChegada) || data.DataPartidaTexto != data.DataChegadaTexto) NoDias = NoDias + 1;
                    }

                    if (x.CodigoRefCusto == 2) //JANTAR
                    {
                        if ((TimeSpan.Parse(data.HoraPartidaTexto) >= x.LimiteHoraPartida) || data.DataPartidaTexto != data.DataChegadaTexto) NoDias = NoDias + 1;

                        if (TimeSpan.Parse(data.HoraChegadaTexto) >= x.LimiteHoraChegada) NoDias = NoDias + 1;
                    }

                    noLinha = DBLinhasFolhaHoras.GetMaxAjudaByFolhaHoraNo(data.FolhaDeHorasNo);

                    LinhasFolhaHoras Ajuda = new LinhasFolhaHoras();

                    Ajuda.NoFolhaHoras = data.FolhaDeHorasNo;
                    Ajuda.NoLinha = noLinha + 1;
                    Ajuda.CodTipoCusto = x.CodigoTipoCusto.Trim();
                    Ajuda.TipoCusto = x.TipoCusto;
                    Ajuda.DescricaoTipoCusto = EnumerablesFixed.FolhaDeHoraAjudaTipoCusto.Where(y => y.Id == x.TipoCusto).FirstOrDefault().Value;
                    Ajuda.Quantidade = Convert.ToDecimal(NoDias);
                    Ajuda.CustoUnitario = Convert.ToDecimal(DBTabelaConfRecursosFH.GetAll().Where(y => y.Tipo == x.TipoCusto.ToString() && y.CodRecurso == x.CodigoTipoCusto.Trim()).FirstOrDefault().PrecoUnitarioCusto);
                    Ajuda.PrecoUnitario = Convert.ToDecimal(DBTabelaConfRecursosFH.GetAll().Where(y => y.Tipo == x.TipoCusto.ToString() && y.CodRecurso == x.CodigoTipoCusto.Trim()).FirstOrDefault().PrecoUnitarioVenda);
                    Ajuda.CustoTotal = NoDias * Convert.ToDecimal(DBTabelaConfRecursosFH.GetAll().Where(y => y.Tipo == x.TipoCusto.ToString() && y.CodRecurso == x.CodigoTipoCusto.Trim()).FirstOrDefault().PrecoUnitarioCusto);
                    Ajuda.PrecoVenda = NoDias * Convert.ToDecimal(DBTabelaConfRecursosFH.GetAll().Where(y => y.Tipo == x.TipoCusto.ToString() && y.CodRecurso == x.CodigoTipoCusto.Trim()).FirstOrDefault().PrecoUnitarioVenda);
                    Ajuda.DataDespesa = data.DataHoraPartida;
                    Ajuda.CalculoAutomatico = true;
                    Ajuda.CodRegiao = data.CodigoRegiao;
                    Ajuda.CodArea = data.CodigoAreaFuncional;
                    Ajuda.CodCresp = data.CodigoCentroResponsabilidade;
                    Ajuda.RubricaSalarial = DBTabelaConfRecursosFH.GetAll().Where(y => y.Tipo == x.TipoCusto.ToString() && y.CodRecurso == x.CodigoTipoCusto.Trim()).FirstOrDefault().RubricaSalarial;
                    Ajuda.UtilizadorCriacao = User.Identity.Name;
                    Ajuda.DataHoraCriacao = DateTime.Now;
                    Ajuda.UtilizadorModificacao = User.Identity.Name;
                    Ajuda.DataHoraModificacao = DateTime.Now;

                    var dbCreateResult = DBLinhasFolhaHoras.CreateAjuda(Ajuda);
                });

                result = true;

                return Json(result);
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }


        public decimal GetSUMDistancia(string noFH)
        {
            decimal SUMDistancia = 0;
            try
            {
                List<LinhasFolhaHoras> Linhas = DBLinhasFolhaHoras.GetPercursoByFolhaHoraNo(noFH).Where(x => x.TipoCusto == 1).ToList();

                Linhas.ForEach(x =>
                {
                    SUMDistancia = SUMDistancia + Convert.ToDecimal(x.Distancia);
                });

                return SUMDistancia;
            }
            catch (Exception ex)
            {
                //log
            }
            return SUMDistancia;
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
                MaoDeObra.NºDeHoras = TimeSpan.Parse(data.HoraFim) - TimeSpan.Parse(data.HoraInicio);
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

        [HttpPost]
        public JsonResult CreatePresenca([FromBody] PresencasFolhaDeHorasViewModel data)
        {
            bool result = false;
            try
            {
                PresençasFolhaDeHoras Presenca = new PresençasFolhaDeHoras();

                Presenca.NºFolhaDeHoras = data.FolhaDeHorasNo;
                Presenca.Data = Convert.ToDateTime(data.Data);
                Presenca.Hora1ªEntrada = TimeSpan.Parse(data.Hora1Entrada);
                Presenca.Hora1ªSaída = TimeSpan.Parse(data.Hora1Saida);
                Presenca.Hora2ªEntrada = TimeSpan.Parse(data.Hora2Entrada);
                Presenca.Hora2ªSaída = TimeSpan.Parse(data.Hora2Saida);
                Presenca.Observacoes = data.Observacoes;
                Presenca.UtilizadorCriação = User.Identity.Name;
                Presenca.DataHoraCriação = DateTime.Now;
                Presenca.UtilizadorModificação = User.Identity.Name;
                Presenca.DataHoraModificação = DateTime.Now;

                var dbCreateResult = DBPresencasFolhaDeHoras.Create(Presenca);

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
        public JsonResult UpdatePresenca([FromBody] FolhaDeHorasViewModel data)
        {
            bool result = false;

            try
            {
                data.FolhaDeHorasPresenca.ForEach(x =>
                {
                    DBPresencasFolhaDeHoras.Update(new PresençasFolhaDeHoras()
                    {
                        NºFolhaDeHoras = x.FolhaDeHorasNo,
                        Data = Convert.ToDateTime(x.Data),
                        Hora1ªEntrada = TimeSpan.Parse(x.Hora1Entrada),
                        Hora1ªSaída = TimeSpan.Parse(x.Hora1Saida),
                        Hora2ªEntrada = TimeSpan.Parse(x.Hora2Entrada),
                        Hora2ªSaída = TimeSpan.Parse(x.Hora2Saida),
                        Observacoes = x.Observacoes,
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
    }
}
