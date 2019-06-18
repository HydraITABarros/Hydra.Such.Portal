using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Portal.Configurations;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Hydra.Such.Data.ViewModel.CCP;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.CCP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.NAV;

using System.IO;
using System.Net.Mail;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data.ViewModel.Compras;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Hydra.Such.Data;

namespace Hydra.Such.Portal.Controllers
{
    public class AreaEmailRecipients
    {
        public int AreaID { get; set; }
        public string AreaName { get; set; }
        public string ToAddress { get; set; }
        public string CCAddress { get; set; }

        public AreaEmailRecipients(string AreaFuncionalCode, ConfiguracaoCcp Addresses)
        {
            List<EnumData> Areas = EnumerablesFixed.Areas;

            try
            {
                AreaID = Convert.ToInt32(AreaFuncionalCode.Substring(0, 2));
                //AreaID = Convert.ToInt32(AreaFuncionalCode);
                AreaName = Areas.Find(a => a.Id == AreaID).Value ?? "";
                switch (AreaID)
                {
                    case 0:
                        ToAddress = Addresses.Email3Compras;
                        CCAddress = "";
                        break;
                    case 1:
                        ToAddress = Addresses.Email5Compras;
                        CCAddress = Addresses.Email6Compras;
                        break;
                    case 2:
                        ToAddress = Addresses.Email7Compras;
                        CCAddress = Addresses.Email8Compras;
                        break;
                    case 5:
                        ToAddress = Addresses.Email4Compras;
                        CCAddress = "";
                        break;
                    default:
                        if (Convert.ToInt32(AreaFuncionalCode.Substring(0, 2)) == 32)
                        {
                            AreaID = 32;
                            AreaName = "Gestão de Parques de Estacionamento";
                            ToAddress = Addresses.Email7Compras;
                            CCAddress = Addresses.Email8Compras;
                        }
                        else
                        {
                            AreaName = "";
                            ToAddress = Addresses.EmailCompras;
                            CCAddress = Addresses.Email2Compras;
                        }
                        break;
                }
            }
            catch (Exception)
            {

                AreaID = -1;
                AreaName = "";
                ToAddress = "";
                CCAddress = "";
            }
        }

    }

    //Nuno Rato - 20180525 --> Criação de uma class que seja constituida por uma coleção de listas de ProcedimentosCcp, por estados, para apresentar num quadro de bordo
    public class CCP_QuadroBordo
    {
        public List<ProcedimentoCCPView> estado_0 { get; set; }
        public List<ProcedimentoCCPView> estado_1 { get; set; }
        public List<ProcedimentoCCPView> estado_2 { get; set; }
        public List<ProcedimentoCCPView> estado_3 { get; set; }
        public List<ProcedimentoCCPView> estado_4 { get; set; }
        public List<ProcedimentoCCPView> estado_5 { get; set; }
        public List<ProcedimentoCCPView> estado_6 { get; set; }
        public List<ProcedimentoCCPView> estado_7 { get; set; }
        public List<ProcedimentoCCPView> estado_8 { get; set; }
        public List<ProcedimentoCCPView> estado_9 { get; set; }
        public List<ProcedimentoCCPView> estado_10 { get; set; }
        public List<ProcedimentoCCPView> estado_11 { get; set; }
        public List<ProcedimentoCCPView> estado_12 { get; set; }
        public List<ProcedimentoCCPView> estado_13 { get; set; }
        public List<ProcedimentoCCPView> estado_14 { get; set; }
        public List<ProcedimentoCCPView> estado_15 { get; set; }
        public List<ProcedimentoCCPView> estado_16 { get; set; }
        public List<ProcedimentoCCPView> estado_17 { get; set; }
        public List<ProcedimentoCCPView> estado_18 { get; set; }
        public List<ProcedimentoCCPView> estado_19 { get; set; }
        public List<ProcedimentoCCPView> estado_20 { get; set; }
        public List<ProcedimentoCCPView> estado_todos { get; set; }
        public List<ProcedimentoCCPView> estado_ratificarCA { get; set; }
        public List<ProcedimentoCCPView> estado_processos_suspensos { get; set; }

    }

    
    public class ReferenceProcedimento
    {
        public int ReferenceYear { get; set; }
        public int ProcReference { get; set; }

        public ReferenceProcedimento(int type_proc, int type, int year)
        {
            ReferenceYear = year;
            ProcReference = DBProcedimentosCCP.ObtainProcedimentoReference(type_proc, type, year);
        }
    }

    [Authorize]
    public class ProcedimentosCcpsController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly IHostingEnvironment _hostingEnvironment;
        //private readonly GeneralConfigurations _generalConfig;
        private string __UploadPath { get; set; }

        public ProcedimentosCcpsController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            //_generalConfig = appSettingsGeneral.Value;
            __UploadPath = "wwwroot/Upload/Procedimentos/";
            this._hostingEnvironment = _hostingEnvironment;
        }

        #region Views
        public IActionResult Index()
        {
            return View();
        }

        // zpgm.< view that will return a Pedidos Simplificados list
        public IActionResult PedidoSimplificado()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.PedidoSimplificado);
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
        // zpgm.>

        // zpgm.< view that will return a Pedidos de Aquisição list
        public IActionResult PedidoAquisicao()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.PedidoAquisição);
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
        // zpgm.>
        public IActionResult Detalhes(string id)
        {
            ViewBag.No = id == null ? "" : id;
            return View();
        }

        public IActionResult DetalhePedidoAquisicao(string id, bool isHistoric = false)
        {
            ViewBag.reportServerURL = _config.ReportServerURL;

            ViewBag.No = id == null ? "" : id;

            if (isHistoric == true)
            {
                ViewBag.Historic = "(Histórico) ";
                ViewBag.ifHistoric = true;
            }
            else
            {
                ViewBag.Historic = "";
                ViewBag.ifHistoric = false;
            }

            return View();
        }

        public IActionResult DetalhePedidoSimplificado(string id, bool isHistoric = false)
        {
            ViewBag.No = id == null ? "" : id;
            ViewBag.TipoProcedimento = 2;

            if (isHistoric == true)
            {
                ViewBag.Historic = "(Histórico) ";
                ViewBag.ifHistoric = true;
            }
            else
            {
                ViewBag.Historic = "";
                ViewBag.ifHistoric = false;
            }

            return View();
        }

        public IActionResult QuadroBordo()
        {
            return View();
        }
        public IActionResult QuadroBordoSimplificado()
        {
            return View();
        }
        public IActionResult QuadroBordoCA()
        {
            return View();
        }
        #endregion

        [HttpPost]
        public JsonResult GetAllProcedimentos()
        {
            List<ProcedimentoCCPView> result = DBProcedimentosCCP.GetAllProcedimentosByViewToList();


            return Json(result);
        }
        [HttpPost]
        public JsonResult GetProcedimentosByProcedimentoType([FromBody] JObject requestParams/*[FromBody] int id*/)
        {
            int id = int.Parse(requestParams["id"].ToString());
            int Historic = 0;
            if (requestParams["Historic"] != null)
                Historic = int.Parse(requestParams["Historic"].ToString());

            List<ProcedimentoCCPView> result = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoTypeToList(id, Historic);
            
            return Json(result);
        }

        [HttpPost]
        public JsonResult InsertAcordoPreco_Procedimentos([FromBody] JObject requestParams)
        {
            string _NoProc = requestParams["NoProcedimento"].ToString();
            DateTime? _DtIni = requestParams["dtIni"].ToString() != string.Empty ? DateTime.Parse(requestParams["dtIni"].ToString()) : (DateTime?)null;
            DateTime? _DtFim = requestParams["dtFim"].ToString() != string.Empty ? DateTime.Parse(requestParams["dtFim"].ToString()) : (DateTime?)null;
            decimal? _ValorTotal = requestParams["valorTotal"].ToString() != string.Empty ? decimal.Parse(requestParams["valorTotal"].ToString()) : (decimal?)null;

            AcordoPrecos AP = DBAcordoPrecos.GetById(_NoProc);

            if (AP == null)
            {
                AcordoPrecos toCreate = DBAcordoPrecos.Create(new AcordoPrecos()
                {
                    NoProcedimento = _NoProc,
                    DtInicio = _DtIni,
                    DtFim = _DtFim,
                    ValorTotal = _ValorTotal
                });

                if (toCreate != null)
                    return Json(ReturnHandlers.Success);
                else
                    return Json(ReturnHandlers.NoData);
            }
            else
            {
                return Json(ReturnHandlers.ExistsData);
            }


            //return DetalhePedidoAquisicao(_NoProc,false);
        }


        [HttpPost]
        public JsonResult GetAllProcedimentos_Estados()
        {
            var model = new CCP_QuadroBordo();
            model.estado_0 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(0);
            model.estado_1 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(1);
            model.estado_2 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(2);
            model.estado_3 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(3);
            model.estado_4 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(4);
            model.estado_5 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(5);
            model.estado_6 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(6);
            model.estado_7 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(7);
            model.estado_8 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(8);
            model.estado_9 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(9);
            model.estado_10 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(10);
            model.estado_11 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(11);
            model.estado_12 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(12);
            model.estado_13 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(13);
            model.estado_14 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(14);
            model.estado_15 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(15);
            model.estado_16 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(16);
            model.estado_17 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(17);
            model.estado_18 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(18);
            model.estado_19 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(19);
            model.estado_20 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(20);

            model.estado_todos = DBProcedimentosCCP.GetAllProcedimentosByView_QuadroBordo_ToList();
            model.estado_ratificarCA = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoRatificarCAToList();
            model.estado_processos_suspensos = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoProcessosSuspensosToList();

            return Json(model);
        }

        [HttpPost]
        public JsonResult GetAllProcedimentosSimplificados_Estados()
        {
            var model = new CCP_QuadroBordo();
            model.estado_1 = DBProcedimentosCCP.GetAllProcedimentosSimplificadosViewByProcedimentoEstadoToList(1);
            model.estado_2 = DBProcedimentosCCP.GetAllProcedimentosSimplificadosViewByProcedimentoEstadoToList(2);
            model.estado_4 = DBProcedimentosCCP.GetAllProcedimentosSimplificadosViewByProcedimentoEstadoToList(4);
            model.estado_5 = DBProcedimentosCCP.GetAllProcedimentosSimplificadosViewByProcedimentoEstadoToList(5);
            model.estado_16 = DBProcedimentosCCP.GetAllProcedimentosSimplificadosViewByProcedimentoEstadoToList(16);
            model.estado_17 = DBProcedimentosCCP.GetAllProcedimentosSimplificadosViewByProcedimentoEstadoToList(17);

            /*              !!!!!!!!!   NOTA    !!!!!!!!!           */
            //estado_processos_suspensos equivale a autorizados nos Procedimentos Simplificados, apenas para não alterar a class!!!!
            model.estado_processos_suspensos = DBProcedimentosCCP.GetAllProcedimentosSimplificadosViewByProcedimentoProcessosAutorizadosToList();

            return Json(model);
        }


        [HttpPost]
        public JsonResult GetAllProcedimentosCA_Estados()
        {
            var model = new CCP_QuadroBordo();
            model.estado_3 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(3);
            model.estado_8 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(8);
            model.estado_17 = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoEstadoToList(17);
            model.estado_ratificarCA = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoRatificarCAToList();

            //Estes são usados para obter os Simplificados, mas que têm os mesmos estados que os PA's
            model.estado_7 = DBProcedimentosCCP.GetAllProcedimentosSimplificadosViewByProcedimentoEstadoToList(17);
            model.estado_processos_suspensos = DBProcedimentosCCP.GetAllProcedimentosSimplificadosViewByProcedimentoRatificarCAToList();
            
            return Json(model);
        }

        [HttpPost]
        public JsonResult GetProcedimentoDetails([FromBody] ProcedimentoCCPView data)
        {
            try
            {
                    if (data != null)
                {
                    ProcedimentosCcp proc = DBProcedimentosCCP.GetProcedimentoById(data.No);
                    if (proc != null)
                    {
                        ProcedimentoCCPView result = CCPFunctions.CastProcedimentoCcpToProcedimentoCcpView(proc);

                        if (result.LinhasPEncomendaProcedimentosCcp != null)
                        {
                            foreach (LinhasParaEncomendaCCPView Linha in result.LinhasPEncomendaProcedimentosCcp)
                            {
                                string Linha_DESC = string.Empty;

                                try
                                {
                                    Linha.CodLocalizacaoText = DBNAV2017Locations.GetAllLocations(_config.NAVDatabaseName, _config.NAVCompanyName).Where(x => x.Code == Linha.CodLocalizacao).FirstOrDefault().Name;
                                }
                                catch
                                {
                                    Linha.CodLocalizacaoText = "";
                                }

                            }
                        }
                        GetChecklists(result);

                        ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                        result.EmailDestinoCA = UserDetails.ProcedimentosEmailEnvioParaCa == null ? string.Empty : UserDetails.ProcedimentosEmailEnvioParaCa;

                        result.Nome_Utilizador_Logado = User.Identity.Name;
                        result.Nome_Utilizador = UserDetails.Nome;

                        //ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                        //result.Nome_Utilizador_Logado = UserDetails.Nome;

                        return Json(result);
                    }

                    return Json(new ProcedimentoCCPView());
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return Json(false);
        }
        [HttpPost]
        public JsonResult CreateProcedimento([FromBody] ProcedimentoCCPView data)
        {
            try
            {
                if (data != null)
                {
                    data.UtilizadorCriacao = User.Identity.Name;
                    ProcedimentosCcp procedimento = DBProcedimentosCCP.__CreateProcedimento(data);
                    if (procedimento == null)
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "Ocorreu um erro ao criar o Procedimento";
                    }
                    else
                    {
                        data.eReasonCode = 1;
                        data.eMessage = "Procedimento criado com sucesso";
                    }
                }

            }
            catch (Exception e)
            {
                data.eReasonCode = 4;
                data.eMessage = "Ocorreu um erro ao criar o Procedimento";
            }

            return Json(data);
        }

        [HttpPost]
        public JsonResult CreateProcedimentoByProcedimentoType([FromBody] int id)
        {
            List<EnumData> ProcedimentoTypes = EnumerablesFixed.ProcedimentosCcpProcedimentoType;
            bool TypeFound = false;
            foreach (var pt in ProcedimentoTypes)
            {
                if (pt.Id == id && !TypeFound)
                    TypeFound = true;
            }

            if (!TypeFound)
                return Json("");

            ProcedimentosCcp Procedimento = DBProcedimentosCCP.__CreateProcedimento(id, User.Identity.Name);

            if (Procedimento == null || Procedimento.Nº == "")
                return Json("");

            return Json(Procedimento.Nº);

        }
        [HttpPost]
        public JsonResult UpdateProcedimento([FromBody] ProcedimentoCCPView data)
        {
            try
            {
                if (data != null)
                {
                    data.UtilizadorModificacao = User.Identity.Name;
                    ProcedimentosCcp proc = DBProcedimentosCCP.__UpdateProcedimento(data);
                    if (proc == null)
                    {
                        return Json(ReturnHandlers.UnableToUpdateProcedimento);
                    }
                    return Json(ReturnHandlers.Success);
                } else
                    return Json(ReturnHandlers.NoData);
            }
            catch (Exception e)
            {
                return Json(ReturnHandlers.UnableToUpdateProcedimento);
            }

        }
        [HttpPost]
        public JsonResult DeleteProcedimento([FromBody] ProcedimentoCCPView data)
        {
            ErrorHandler result = new ErrorHandler();
            if (data != null)
            {
                if (DBProcedimentosCCP.__DeleteProcedimento(data.No))
                {
                    result = new ErrorHandler()
                    {
                        eReasonCode = 0,
                        eMessage = "Procedimento removido com sucesso"
                    };
                }
                else
                {
                    result = new ErrorHandler()
                    {
                        eReasonCode = 4,
                        eMessage = "Não foi possível remover o Procedimento"
                    };
                }
                return Json(result);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        [HttpPost]
        public JsonResult GetUsersWhoAreElementosJuri()
        {
            List<ConfigUtilizadores> Users = DBProcedimentosCCP.GetAllUsersElementosJuri().GroupBy(u => new { u.IdUtilizador, u.Nome }).Select(u => u.First()).ToList();
            List<DDMessageString> result = Users.Select(cu => new DDMessageString()
            {
                id = cu.IdUtilizador,
                value = cu.Nome
            }).ToList();

            return Json(result);
        }
        [HttpPost]
        public JsonResult CreateElementoJuri([FromBody] ElementosJuriView data)
        {
            List<ElementosJuri> SearchForDuplicates = DBProcedimentosCCP.GetAllElementosJuriProcedimento(data.NoProcedimento);
            foreach (var ej in SearchForDuplicates)
            {
                // search if is user is already an Elemento Juri
                if (ej.NºProcedimento == data.NoProcedimento && ej.Utilizador == data.Utilizador)
                {
                    ErrorHandler DuplicatedUser = new ErrorHandler()
                    {
                        eReasonCode = 3,
                        eMessage = "Utilizador já existe como Elemento do Juri!"
                    };
                    return Json(DuplicatedUser);
                }

                // search if there is already a Presidente
                if (ej.Presidente.HasValue && ej.Presidente.Value && data.Presidente.HasValue && data.Presidente.Value)
                {
                    ErrorHandler PresidentInserted = new ErrorHandler()
                    {
                        eReasonCode = 4,
                        eMessage = "Já existe um Presidente!"
                    };

                    return Json(PresidentInserted);
                }
            }

            if (data.EnviarEmail.HasValue && data.EnviarEmail.Value)
                data.Email = data.Utilizador;

            if (data.EnviarEmail.HasValue && !data.EnviarEmail.Value)
                data.Email = "";

            data.UtilizadorCriacao = User.Identity.Name;
            data.DataHoraCriacao = DateTime.Now;
            ElementosJuri NewElemento = DBProcedimentosCCP.__CreateElementoJuri(data);

            bool created = NewElemento != null ? true : false;

            if (created)
            {
                ErrorHandler Success = new ErrorHandler()
                {
                    eReasonCode = 0,
                    eMessage = "Sucesso ao criar o utilizador!"
                };

                return Json(Success);
            }
            else
            {
                ErrorHandler UnknownError = new ErrorHandler()
                {
                    eReasonCode = 3,
                    eMessage = "Impossivel inserir Elemento Juri na Base de Dados"
                };

                return Json(UnknownError);
            }

        }
        [HttpPost]
        public JsonResult DeleteElementoJuri([FromBody] ElementosJuriView data)
        {
            return Json(DBProcedimentosCCP.__DeleteElementoJuri(data.NoProcedimento, data.NoLinha));
        }

        //NR 20180226
        [HttpPost]
        public JsonResult CreateLinhaProdutoServico([FromBody] LinhasPEncomendaProcedimentosCcp data)
        {
            bool result = false;
            try
            {
                //int noLinha;
                //noLinha = DBProcedimentosCCP.GetMaxByLinhaParaEncomenda(data.NºProcedimento);

                LinhasPEncomendaProcedimentosCcp LinhaProdutoServico = new LinhasPEncomendaProcedimentosCcp();
                LinhaProdutoServico.CustoUnitário = data.CustoUnitário;
                LinhaProdutoServico.Código = data.Código;
                LinhaProdutoServico.CódigoCentroResponsabilidade = data.CódigoCentroResponsabilidade;
                LinhaProdutoServico.CódigoRegião = data.CódigoRegião;
                LinhaProdutoServico.CódigoÁreaFuncional = data.CódigoÁreaFuncional;
                LinhaProdutoServico.CódLocalização = data.CódLocalização;
                LinhaProdutoServico.CódUnidadeMedida = data.CódUnidadeMedida;
                LinhaProdutoServico.CustoUnitário = data.CustoUnitário;
                LinhaProdutoServico.DataHoraCriação = DateTime.Now;
                LinhaProdutoServico.DataHoraModificação = data.DataHoraModificação;
                LinhaProdutoServico.Descrição = data.Descrição;
                //AROMAO 01/10/2018
                //LinhaProdutoServico.Nº = data.Nº;
                //LinhaProdutoServico.NºLinha = noLinha;
                LinhaProdutoServico.NºLinhaRequisição = data.NºLinhaRequisição;
                LinhaProdutoServico.NºProcedimento = data.NºProcedimento;
                LinhaProdutoServico.NºProcedimentoNavigation = data.NºProcedimentoNavigation;
                LinhaProdutoServico.NºProjeto = data.NºProjeto;
                LinhaProdutoServico.NºRequisição = data.NºRequisição;
                //AROMAO 01/10/2018
                LinhaProdutoServico.NºRequisiçãoNavigation = data.NºRequisiçãoNavigation;
                LinhaProdutoServico.QuantARequerer = data.QuantARequerer;
                LinhaProdutoServico.Tipo = data.Tipo;
                LinhaProdutoServico.UtilizadorCriação = User.Identity.Name;
                LinhaProdutoServico.UtilizadorModificação = data.UtilizadorModificação;

                var dbCreateResult = DBProcedimentosCCP.CreateLinhaProdutoServico(LinhaProdutoServico);

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
        public JsonResult DeleteLinhaProdutoServico([FromBody] LinhasPEncomendaProcedimentosCcp data)
        {
            bool result = false;
            try
            {
                bool dbDeleteResult = DBProcedimentosCCP.__DeleteLinhaProdutoServico(data.NºProcedimento, data.NºLinha);

                result = dbDeleteResult;
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }

        #region Get the FluxoTrabalhoListaControlo propertie according with the view where it should be displayed
        public void GetChecklists(ProcedimentoCCPView data)
        {
            if (data.FluxoTrabalhoListaControlo != null && data.FluxoTrabalhoListaControlo.Count > 0)
            {
                // 0. Used in the "Unidade Produtiva - Avaliação Técnica" paper-tab
                if (data.Estado != 0)
                {
                    FluxoTrabalhoListaControlo Fluxo = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 0).LastOrDefault();

                    if (Fluxo != null)
                    {
                        data.DataAreaChecklist = Fluxo.Data;
                        data.HoraAreaChecklist = Fluxo.Hora;

                        data.ComentarioArea = Fluxo.Comentario;
                        data.ResponsavelArea = Fluxo.User;
                        data.NomeResponsavelArea = Fluxo.NomeUser;
                        data.DataResponsavel = Fluxo.Data;
                    }

                }

                // 1. Used in the "Imobilizado" paper-tab "Contabilidade" area
                if (data.Estado != 1)
                {
                    FluxoTrabalhoListaControlo Fluxo = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 1).LastOrDefault();
                    if (Fluxo != null)
                    {
                        data.ImobAreaDataChecklist = Fluxo.Data;
                        data.ImobAreaHoraChecklist = Fluxo.Hora;

                        data.ComentarioImobContabilidade = Fluxo.Comentario;
                        data.ComentarioImobContabilidade2 = Fluxo.Comentario2;
                        data.ImobilizadoSimNao = Fluxo.ImobSimNao ?? false;
                        data.ResponsavelImobContabilidade = Fluxo.User;
                        data.NomeResponsavelImobContabilidade = Fluxo.NomeUser;
                        data.DataImobContabilidade = Fluxo.Data;
                    }
                }

                // 2. Used in the "Imobilizado" paper-tab "Área" area
                if (data.Estado != 2)
                {
                    FluxoTrabalhoListaControlo Fluxo = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 2).LastOrDefault();
                    if (Fluxo != null)
                    {
                        data.ImobAreaDataChecklist = Fluxo.Data;
                        data.ImobAreaHoraChecklist = Fluxo.Hora;
                        data.ComentarioImobArea = Fluxo.Comentario;
                        data.ResponsavelImobArea = Fluxo.User;
                        data.NomeResponsavelImobArea = Fluxo.NomeUser;
                        data.DataImobArea = Fluxo.Data;

                        //ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                        //data.EmailDestinoCA = UserDetails.ProcedimentosEmailEnvioParaCa == null ? string.Empty: UserDetails.ProcedimentosEmailEnvioParaCa;
                    }
                }

                // 3. Used in the "CA" paper-tab "Autorizar Imobilizado" area 
                if (data.Estado != 3)
                {
                    FluxoTrabalhoListaControlo Fluxo = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 3).LastOrDefault();
                    if (Fluxo != null)
                    {
                        data.ImobCADataChecklist = Fluxo.Data;
                        data.ImobCAHoraChecklist = Fluxo.Hora;

                        data.ComentarioImobCA = Fluxo.Comentario;
                        data.ResponsavelImobCA = Fluxo.User;
                        data.NomeResponsavelImobCA = Fluxo.NomeUser;
                        data.DataImobAprovisionamentoCA = Fluxo.Data;
                    }
                }

                // 4. Used in the "Fundamentos Decisão" paper-tab in the "A Preencher pelas compras" area
                if (data.Estado != 4)
                {
                    FluxoTrabalhoListaControlo Fluxo = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 4).LastOrDefault();
                    if (Fluxo != null)
                    {
                        data.FundamentoComprasDataChecklist = Fluxo.Data;
                        data.FundamentoComprasHoraChecklist = Fluxo.Hora;
                        data.ComentarioFundamentoCompras = Fluxo.Comentario;
                        data.ResponsavelFundamentoCompras = Fluxo.User;
                        data.NomeResponsavelFundamentoCompras = Fluxo.NomeUser;
                        data.DataEnvio = Fluxo.Data;
                    }
                }

                // 5. Used in the "Fundamentos Decisão" paper-tab in the "A Preencher pelos Serviços Financeiros" area
                if (data.Estado != 5)
                {
                    FluxoTrabalhoListaControlo Fluxo = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 5).LastOrDefault();
                    if (Fluxo != null)
                    {
                        data.FundamentoFinDataChecklist = Fluxo.Data;
                        data.FundamentoFinHoraChecklist = Fluxo.Hora;
                        data.ComentarioFundamentoFinanceiros = Fluxo.Comentario;
                        data.ComentarioFundamentoFinanceiros2 = Fluxo.Comentario2;
                        data.ResponsavelFundamentoFinanceiros = Fluxo.User;
                        data.NomeResponsavelFundamentoFinanceiros = Fluxo.NomeUser;
                        data.DataFinanceiros = Fluxo.Data;
                    }
                }

                // 6. Used in the "Juridicos" paper-tab returns a List of ElementosChecklist according to ProcedimentosCcp.Estado (!= 6 or != 14)
                if (data.Estado != 6)
                {
                    FluxoTrabalhoListaControlo Fluxo6 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 6).LastOrDefault();
                    if (Fluxo6 != null)
                    {
                        data.JuridicosDataChecklist6 = Fluxo6.Data;
                        data.JuridicosHoraChecklist6 = Fluxo6.Hora;

                        data.ComentarioJuridico6 = Fluxo6.Comentario;
                        data.ResponsavelJuridico6 = Fluxo6.User;
                        data.NomeResponsavelJuridico6 = Fluxo6.NomeUser;
                        data.DataJuridico6 = Fluxo6.Data;
                    }
                }

                if (data.Estado != 14)
                {
                    FluxoTrabalhoListaControlo Fluxo14 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 14).LastOrDefault();
                    if (Fluxo14 != null)
                    {
                        data.JuridicosDataChecklist14 = Fluxo14.Data;
                        data.JuridicosHoraChecklist14 = Fluxo14.Hora;

                        data.ComentarioJuridico14 = Fluxo14.Comentario;
                        data.ResponsavelJuridico14 = Fluxo14.User;
                        data.NomeResponsavelJuridico14 = Fluxo14.NomeUser;
                        data.DataJuridico14 = Fluxo14.Data;
                    }
                }

                // 7. Used in the "Fundamentos Decisão" paper-tab in the "A preencher pela Área(...)" area 
                if (data.Estado != 7)
                {
                    FluxoTrabalhoListaControlo Fluxo = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 7).LastOrDefault();

                    if (Fluxo != null)
                    {
                        data.FundamentacaoAreaDataChecklist = Fluxo.Data;
                        data.FundamentacaoAreaHoraChecklist = Fluxo.Hora;
                        data.FundamentacaoAreaComentario = Fluxo.Comentario;
                        data.FundamentacaoAreaResponsavel = Fluxo.User;
                        data.FundamentacaoAreaNomeResponsavel = Fluxo.NomeUser;
                        data.FundamentacaoAreaDataResponsavel = Fluxo.Data;
                    }
                }

                // 8. Used in the "CA" paper-tab returns a List of ElementosChecklist according to ProcedimentosCcp.Estado (!= 8 or != 17)
                if (data.Estado != 8)
                {
                    FluxoTrabalhoListaControlo Fluxo8 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 8).LastOrDefault();
                    if (Fluxo8 != null)
                    {
                        data.AberturaCADataChecklist8 = Fluxo8.Data;
                        data.AberturaCAHoraChecklist8 = Fluxo8.Hora;
                        data.ComentarioCA8 = Fluxo8.Comentario;
                        data.ResponsavelCA8 = Fluxo8.User;
                        data.NomeResponsavelCA8 = Fluxo8.NomeUser;
                        data.DataAberturaCA8 = Fluxo8.Data;
                    }
                }

                if (data.Estado != 17)
                {
                    FluxoTrabalhoListaControlo Fluxo17 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 17).LastOrDefault();
                    if (Fluxo17 != null)
                    {
                        data.AberturaCADataChecklist17 = Fluxo17.Data;
                        data.AberturaCAHoraChecklist17 = Fluxo17.Hora;
                        data.ComentarioCA17 = Fluxo17.Comentario;
                        data.ResponsavelCA17 = Fluxo17.User;
                        data.NomeResponsavelCA17 = Fluxo17.NomeUser;
                        data.DataAberturaCA17 = Fluxo17.Data;
                    }
                }

                // 9. Used in the "Valores Adjudicação" paper-tab returns a List of ElementosChecklist according to ProcedimentosCcp.Estado (!= 15 or != 16)
                if (data.Estado != 15)
                {
                    FluxoTrabalhoListaControlo Fluxo15 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 15).LastOrDefault();
                    if (Fluxo15 != null)
                    {
                        data.AdjudicacaoDataChecklist15 = Fluxo15.Data;
                        data.AdjudicacaoHoraChecklist15 = Fluxo15.Hora;

                        data.ComentarioAdjudicacao15 = Fluxo15.Comentario;
                        data.ResponsavelAdjudicacao15 = Fluxo15.User;
                        data.NomeResponsavelAdjudicacao15 = Fluxo15.NomeUser;
                        data.DataAdjudicacao15 = Fluxo15.Data;
                    }
                }

                if (data.Estado != 16)
                {
                    FluxoTrabalhoListaControlo Fluxo16 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 16).LastOrDefault();
                    if (Fluxo16 != null)
                    {
                        data.AdjudicacaoDataChecklist16 = Fluxo16.Data;
                        data.AdjudicacaoHoraChecklist16 = Fluxo16.Hora;

                        data.ComentarioAdjudicacao16 = Fluxo16.Comentario;
                        data.ResponsavelAdjudicacao16 = Fluxo16.User;
                        data.NomeResponsavelAdjudicacao16 = Fluxo16.NomeUser;
                        data.DataAdjudicacao16 = Fluxo16.Data;
                    }
                }

            }
        }
        #endregion

        #region zpgm.ALT_CCP_#001.y2019
        [HttpPost]
        public JsonResult GetTiposProcedimento()
        {
            List<TipoProcedimentoCcp> tipos = DBConfiguracaoCCP.GetAllTypes(false);
            List<TipoProcedimentoCcpView> result = new List<TipoProcedimentoCcpView>();
            foreach(var t in tipos)
            {
                result.Add(new TipoProcedimentoCcpView(t));
            }

            return Json(result);
        }
        
        [HttpPost]
        public JsonResult GetFundamentosTipoProcedimento([FromBody] JObject requestParam)
        {
            int idTipo = requestParam["idTipo"] == null ? 0: (int)requestParam["idTipo"];

            if (idTipo == 0)
                return Json(null);

            List<FundamentoLegalTipoProcedimentoCcp> result = DBConfiguracaoCCP.GetReasonsForType(idTipo);

            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateLote([FromBody]LoteProcedimentoCcp data)
        {
            if (data == null)
                return Json(false);

            data.UtilizadorCriacao = User.Identity.Name;
            data.DataCriacao = DateTime.Now;

            return Json(DBProcedimentosCCP.__CreateBatch(data));
        }
        [HttpPost]
        public JsonResult UpdateLote([FromBody]LoteProcedimentoCcp data)
        {
            if (data == null)
                return Json(false);

            data.UtilizadorModificacao = User.Identity.Name;
            data.DataModificacao = DateTime.Now;

            return Json(DBProcedimentosCCP.__UpdateBatch(data));

            //return Json(null);
        }

        
        [HttpPost]
        public JsonResult DeleteLote([FromBody]JObject requestParam)
        {
            if (requestParam == null)
                return Json(false);

            string noProcedimento = requestParam["noProcedimento"] == null ? "" : (string)requestParam["noProcedimento"];
            int idLote = requestParam["idLote"] == null ? 0 : (int)requestParam["idLote"];

            if (noProcedimento == "")
                return Json(false);

            if (idLote == 0)
                return Json(false);

            if (idLote == -1)
            {
                return Json(DBProcedimentosCCP.__DeleteAllBatches(noProcedimento));
            }

            return Json(DBProcedimentosCCP.__DeleteBatch(noProcedimento, idLote));
        }

        [HttpPost]
        public JsonResult CBVAEnviarLoteArea([FromBody] LoteProcedimentoCcp data)
        {
            if (data != null)
            {
                ProcedimentosCcp procedimento = DBProcedimentosCCP.GetProcedimentoById(data.NoProcedimento);
                if (procedimento == null)
                    return Json(ReturnHandlers.NoData);

                bool IsElementCompras = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCompras);
                bool jaAdjudicado = false;

                if (!string.IsNullOrEmpty(data.UtilizadorAdjudicacao) && !string.IsNullOrWhiteSpace(data.UtilizadorAdjudicacao))
                    jaAdjudicado = true;

                if (!IsElementCompras)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (jaAdjudicado)
                {
                    return Json(ReturnHandlers.BatchAlreadyAwarded);
                }

                data.EstadoLote = 15;

                data.UtilizadorAdjudicacao = User.Identity.Name;
                data.DataAdjudicacao = DateTime.Now;
                data.HoraAdjudicacao = DateTime.Now;

                FluxoTrabalhoListaControlo fluxo0 = new FluxoTrabalhoListaControlo();

                if (procedimento.FluxoTrabalhoListaControlo != null)
                {
                    fluxo0 = procedimento.FluxoTrabalhoListaControlo.Where(f => f.Estado == 0).LastOrDefault();
                }
                else
                {
                    fluxo0 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.NoProcedimento, 0);
                }


                ConfigUtilizadores UserDetails_TO = DBProcedimentosCCP.GetUserDetails(fluxo0.User);
                string UserEmail_TO = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO.IdUtilizador))
                {
                    UserEmail_TO = UserDetails_TO.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";
                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }

                if (!EmailAutomation.IsValidEmail(EmailList.EmailCompras))
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // process batch and change batch state
                ErrorHandler DecisionTaken = DBProcedimentosCCP.AwardBatch(procedimento, data, UserDetails, 1);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }


                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = procedimento.Nº,
                    Assunto = data.NoProcedimento + "-" + procedimento.NomeProcesso + " Lote nº " + data.IdLote +  " -  Abertura de Procedimento",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = data.ComentarioAutorizacaoAdjudicacao,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                procedimento.EmailsProcedimentosCcp.Add(ProcedimentoEmail);

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);
                Email.CC.Add(EmailList.EmailCompras);
                Email.CC.Add(EmailList.Email2Compras);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }
        #endregion

        [HttpPost]
        public JsonResult GetUserFeatures()
        {
            List<AcessosUtilizador> UserAccesses = DBProcedimentosCCP.GetUserAccesses(User.Identity.Name);
            if(UserAccesses != null)
            {
                List<string> UAccess = new List<string>();

                foreach(var ua in UserAccesses)
                {
                    switch (ua.Funcionalidade)
                    {
                        case DBProcedimentosCCP._ElementoArea:
                            UAccess.Add("IsElementArea");
                            break;
                        case DBProcedimentosCCP._ElementoPreArea0:
                            UAccess.Add("IsElementPreArea0");
                            break;
                        case DBProcedimentosCCP._ElementoPreArea:
                            UAccess.Add("IsElementPreArea");
                            break;
                        case DBProcedimentosCCP._ElementoCompras:
                            UAccess.Add("IsElementCompras");
                            break;
                        case DBProcedimentosCCP._ElementoJuri:
                            UAccess.Add("IsElementJuri");
                            break;
                        case DBProcedimentosCCP._ElementoContabilidade:
                            UAccess.Add("IsElementContabilidade");
                            break;
                        case DBProcedimentosCCP._ElementoJuridico:
                            UAccess.Add("IsElementJuridico");
                            break;
                        case DBProcedimentosCCP._ElementoCA:
                            UAccess.Add("IsElementCA");
                            break;
                        case DBProcedimentosCCP._GestorProcesso:
                            UAccess.Add("IsGestorProcesso");
                            break;
                        case DBProcedimentosCCP._SecretariadoCA:
                            UAccess.Add("IsSecretariadoCA");
                            break;
                        case DBProcedimentosCCP._FechoProcesso:
                            UAccess.Add("IsFechoProcesso");
                            break;
                    }
                }

                if (UAccess == null || UAccess.Count == 0)
                    return Json(null);

                return Json(UAccess.Distinct().ToList());
            }

            return Json(null);
        }

        //NR 20180329
        [HttpPost]
        public JsonResult CreateNota([FromBody] NotasProcedimentosCcp data)
        {
            bool result = false;
            try
            {
                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);


                NotasProcedimentosCcp Notas = new NotasProcedimentosCcp();

                Notas.NºProcedimento = data.NºProcedimento;
                Notas.Nota = data.Nota;
                Notas.Utilizador = UserDetails.IdUtilizador;
                Notas.UtilizadorCriação = UserDetails.IdUtilizador;
                Notas.UtilizadorModificação = UserDetails.IdUtilizador;
                Notas.DataHora = DateTime.Now;
                Notas.DataHoraCriação = DateTime.Now;
                Notas.DataHoraModificação = DateTime.Now;

                var dbCreateResult = DBProcedimentosCCP.CreateNota(Notas);

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

        //NR 20180329
        [HttpPost]
        public JsonResult DeleteNota([FromBody] NotasProcedimentosCcp data)
        {
            bool result = false;
            try
            {
                bool dbDeleteResult = DBProcedimentosCCP.__DeleteNotaProcedimento(data.NºProcedimento, data.NºLinha);

                result = dbDeleteResult;
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }

        //NR 20180329
        [HttpPost]
        public JsonResult CreateAta([FromBody] RegistoDeAtas data)
        {
            JsonResult result = Json(ReturnHandlers.Error);

            bool IsElementSecretariadoCA = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._SecretariadoCA);

            if (!IsElementSecretariadoCA)
            {
                return Json(ReturnHandlers.UserNotAllowed);
            }

            bool AtaExiste = DBProcedimentosCCP.CheckAtaNumber(data.NºProcedimento, data.NºAta);

            if (AtaExiste)
            {
                return Json(ReturnHandlers.ProcedimentoAtaNumberExists);
            }

            try
            {
                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);

                RegistoDeAtas Atas = new RegistoDeAtas();

                Atas.NºProcedimento = data.NºProcedimento;
                Atas.NºAta = data.NºAta;
                Atas.DataDaAta = data.DataDaAta;
                Atas.Observações = data.Observações;
                Atas.DataHoraCriação = DateTime.Now;
                Atas.UtilizadorCriação = UserDetails.IdUtilizador;
                Atas.DataHoraModificação = DateTime.Now;
                Atas.UtilizadorModificação = UserDetails.IdUtilizador;

                var dbCreateResult = DBProcedimentosCCP.CreateAta(Atas);

                if (dbCreateResult != null)
                    result = Json(ReturnHandlers.Success);
                else
                    result = Json(ReturnHandlers.Error);
            }
            catch (Exception ex)
            {
                result = Json(ReturnHandlers.Error);
            }

            return result;
        }




        [HttpPost]
        public JsonResult CreateLigacaoRequisicao([FromBody] RequisitionViewModel requisition/*string procedureNo, string requisitionNo*/)
        {
            bool result = false;
            try
            {
                Requisição reqDB = DBRequest.GetById(requisition.RequisitionNo/*requisitionNo*/);

                reqDB.NºProcedimentoCcp = requisition.ProcedureCcpNo/*procedureNo*/;

                RequisitionViewModel updatedRequisition = DBRequest.Update(reqDB).ParseToViewModel();
                
                List<LinhasRequisição> linhasReq = DBRequestLine.GetByRequisitionId(requisition.RequisitionNo/*requisitionNo*/);
                for (int i = 0; i < linhasReq.Count; i++)
                {
                    LinhasPEncomendaProcedimentosCcp LinhaProdutoServico = new LinhasPEncomendaProcedimentosCcp();
                    LinhaProdutoServico.CustoUnitário = linhasReq[i].CustoUnitário;
                    LinhaProdutoServico.Código = linhasReq[i].Código;
                    LinhaProdutoServico.CódigoCentroResponsabilidade = linhasReq[i].CódigoCentroResponsabilidade;
                    LinhaProdutoServico.CódigoRegião = linhasReq[i].CódigoRegião;
                    LinhaProdutoServico.CódigoÁreaFuncional = linhasReq[i].CódigoÁreaFuncional;
                    LinhaProdutoServico.CódLocalização = linhasReq[i].CódigoLocalização;
                    LinhaProdutoServico.CódUnidadeMedida = linhasReq[i].CódigoUnidadeMedida;
                    LinhaProdutoServico.CustoUnitário = linhasReq[i].CustoUnitário;
                    LinhaProdutoServico.DataHoraCriação = DateTime.Now;
                    LinhaProdutoServico.DataHoraModificação = linhasReq[i].DataHoraModificação;
                    LinhaProdutoServico.Descrição = linhasReq[i].Descrição;
                    //LinhaProdutoServico.Nº = linhasReq[i].NºLinha;
                    //LinhaProdutoServico.NºLinha = noLinha;
                    LinhaProdutoServico.NºLinhaRequisição = linhasReq[i].NºLinha;
                    LinhaProdutoServico.NºProcedimento = requisition.ProcedureCcpNo/*procedureNo*/;
                    //LinhaProdutoServico.NºProcedimentoNavigation = procedureNo;
                    LinhaProdutoServico.NºProjeto = linhasReq[i].NºProjeto;
                    LinhaProdutoServico.NºRequisição = linhasReq[i].NºRequisição;
                    //AROMAO 01/10/2018
                    LinhaProdutoServico.NºRequisiçãoNavigation = linhasReq[i].NºRequisiçãoNavigation;
                    LinhaProdutoServico.QuantARequerer = linhasReq[i].QuantidadeARequerer;
                    LinhaProdutoServico.Tipo = linhasReq[i].Tipo;
                    LinhaProdutoServico.UtilizadorCriação = User.Identity.Name;
                    LinhaProdutoServico.UtilizadorModificação = linhasReq[i].UtilizadorModificação;

                    var dbCreateResult = DBProcedimentosCCP.CreateLinhaProdutoServico(LinhaProdutoServico);

                    if (dbCreateResult != null)
                        result = true;
                    else
                        result = false;
                }

                if (updatedRequisition != null)
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



        /*
         *      In the following methods the ErrorHandler will return:
         *          0 -> SUCCESS
         *          != 0 -> Error
         *          
         */

        //  This method reflects the CommandButton "Submeter Processo" in the "Unidade Produtiva" tab of the NAV form
        [HttpPost]
        public JsonResult SubmitProcedimento([FromBody] ProcedimentoCCPView data)
        {
            if(data != null)
            {
                // 1. Get the latest version in the Database
                ProcedimentosCcp Procedimento = DBProcedimentosCCP.GetProcedimentoById(data.No);
                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                bool IsElementPreArea0 = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(UserDetails.IdUtilizador, DBProcedimentosCCP._ElementoPreArea0);
                bool IsElementPreArea = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(UserDetails.IdUtilizador, DBProcedimentosCCP._ElementoPreArea);
                string UserEmail = "";

                if(EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                    UserEmail = UserDetails.IdUtilizador;

                // 2.a Check if Procedimento has been already submitted
                if (IsElementPreArea0 && Procedimento.PréÁrea.HasValue && Procedimento.PréÁrea.Value)
                {
                    return Json(ReturnHandlers.ProcedimentoAlreadySubmitted);
                }

                // 2.b Check if Procedimento has been already submitted
                if(IsElementPreArea && Procedimento.SubmeterPréÁrea.HasValue && Procedimento.SubmeterPréÁrea.Value)
                {
                    return Json(ReturnHandlers.ProcedimentoAlreadySubmitted);
                }

                // 3. Create Fluxo Trabalho
                if (!data.Imobilizado.HasValue)
                    data.Imobilizado = false;

                
                FluxoTrabalhoListaControlo Fluxo = new FluxoTrabalhoListaControlo()
                {
                    No = data.No,
                    Estado = 0,
                    Data = DateTime.Now.Date,
                    Hora = DateTime.Now.TimeOfDay,
                    TipoEstado = 1,
                    User = UserDetails.IdUtilizador,
                    NomeUser = UserDetails.Nome,
                    Comentario = data.ComentarioArea,
                    EstadoSeguinte = data.Imobilizado.Value ? 1 : 4,
                };

                data.ResponsavelArea = Fluxo.User;
                data.NomeResponsavelArea = Fluxo.NomeUser;
                data.DataResponsavel = Fluxo.Data;

                if (IsElementPreArea || IsElementPreArea0)
                    Fluxo.EstadoSeguinte = 0;

                if(DBProcedimentosCCP.__CreateFluxoTrabalho(Fluxo) == null)
                {
                    return Json(ReturnHandlers.UnableToCreateFluxo);
                }

                //List<FluxoTrabalhoListaControlo> Fluxos = new List<FluxoTrabalhoListaControlo>
                //{
                //    Fluxo
                //};

                data.FluxoTrabalhoListaControlo = new List<FluxoTrabalhoListaControlo>
                {
                    Fluxo
                };



                data.Estado = data.Imobilizado.Value ? 1 : 4;
                data.DataHoraEstado = Fluxo.Data + Fluxo.Hora;
                data.UtilizadorEstado = UserDetails.IdUtilizador;
                data.UtilizadorModificacao = UserDetails.IdUtilizador;

                if (DBProcedimentosCCP.__UpdateProcedimento(data) == null)
                {
                    return Json(ReturnHandlers.UnableToUpdateProcedimento);
                }

                if (!EmailAutomation.IsValidEmail(UserEmail))
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                }

                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }

                // 4. Send emails and updates the data object
                if (!(IsElementPreArea || IsElementPreArea0))
                {
                    // Prepare emails
                    if (data.Imobilizado.Value)
                    {
                        if (!EmailAutomation.IsValidEmail(EmailList.EmailContabilidade))
                        {
                            return Json(ReturnHandlers.InvalidEmailAddress);
                        }

                        EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                        {
                            NºProcedimento = data.No,
                            EmailDestinatário = EmailList.EmailContabilidade,
                            Assunto = data.No + " - Aquisição de Imobilizado",
                            TextoEmail = data.ComentarioArea,    
                            UtilizadorEmail = UserEmail,
                            DataHoraEmail = DateTime.Now,
                            UtilizadorCriação = UserDetails.IdUtilizador,
                            DataHoraCriação = DateTime.Now
                        };

                        if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                        {
                            return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                        }

                        //data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));
                        data.EmailsProcedimentosCcp = DBProcedimentosCCP.GetAllEmailsView(data.No);

                        SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                        {
                            DisplayName = UserDetails.Nome,
                            Subject = ProcedimentoEmail.Assunto,
                            From = DBProcedimentosCCP._EmailSender
                        };
                        
                        Email.To.Add(EmailList.EmailContabilidade);

                        if (EmailAutomation.IsValidEmail(EmailList.Email2Contabilidade))
                            Email.CC.Add(EmailList.Email2Contabilidade);

                        if (EmailAutomation.IsValidEmail(EmailList.Email3Contabilidade))
                            Email.CC.Add(EmailList.Email3Contabilidade);

                        Email.BCC.Add(UserEmail);
                        
                        Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                        
                        Email.IsBodyHtml = true;
                        Email.EmailProcedimento = ProcedimentoEmail;

                        Email.SendEmail();
                    }
                    else
                    {
                        AreaEmailRecipients DestinationEmails = new AreaEmailRecipients(data.CodigoAreaFuncional, EmailList);

                        if(DestinationEmails.AreaID == -1)
                        {
                            return Json(ReturnHandlers.UnknownArea);
                        }

                        if (!EmailAutomation.IsValidEmail(DestinationEmails.ToAddress))
                        {
                            return Json(ReturnHandlers.InvalidEmailAddress);
                        }

                        EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                        {
                            NºProcedimento = data.No,
                            EmailDestinatário = DestinationEmails.ToAddress,
                            Assunto = data.No + " - Novo pedido de aquisição",
                            TextoEmail = data.ComentarioArea, 
                            UtilizadorEmail = UserEmail,
                            DataHoraEmail = DateTime.Now,
                            UtilizadorCriação = UserDetails.IdUtilizador,
                            DataHoraCriação = DateTime.Now
                        };

                        if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                        {
                            return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                        }

                        //data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));
                        data.EmailsProcedimentosCcp = DBProcedimentosCCP.GetAllEmailsView(data.No);

                        SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                        {
                            DisplayName = UserDetails.Nome,
                            Subject = ProcedimentoEmail.Assunto,
                            From = DBProcedimentosCCP._EmailSender
                        };

                        Email.To.Add(DestinationEmails.ToAddress);

                        if (EmailAutomation.IsValidEmail(DestinationEmails.CCAddress))
                            Email.CC.Add(DestinationEmails.CCAddress);

                        Email.BCC.Add(UserEmail);
                        Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);

                        Email.IsBodyHtml = true;
                        Email.EmailProcedimento = ProcedimentoEmail;

                        Email.SendEmail();
                    }
                }
                else
                {
                    if (IsElementPreArea)
                        data.SubmeterPreArea = true;
                    else
                        data.PreArea = true;

                    if (DBProcedimentosCCP.__UpdateProcedimento(data) == null)
                    {
                        return Json(ReturnHandlers.UnableToUpdateProcedimento);
                    }

                    if (!EmailAutomation.IsValidEmail(UserDetails.ProcedimentosEmailEnvioParaArea))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    }

                    EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                    {
                        NºProcedimento = data.No,
                        EmailDestinatário = UserDetails.ProcedimentosEmailEnvioParaArea,
                        Assunto = data.No + " - Novo pedido de aquisição",
                        TextoEmail = "Foi registado um pedido de aquisição, que necessita ser submetido para aprovação",
                        UtilizadorEmail = UserEmail,
                        DataHoraEmail = DateTime.Now,
                        UtilizadorCriação = UserDetails.IdUtilizador,
                        DataHoraCriação = DateTime.Now
                    };

                    if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                    {
                        return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                    }

                    //data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));
                    data.EmailsProcedimentosCcp = DBProcedimentosCCP.GetAllEmailsView(data.No);

                    SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                    {
                        DisplayName = UserDetails.Nome,
                        Subject = ProcedimentoEmail.Assunto,
                        From = DBProcedimentosCCP._EmailSender
                    };

                    Email.To.Add(UserDetails.ProcedimentosEmailEnvioParaArea);
                    if (EmailAutomation.IsValidEmail(UserDetails.ProcedimentosEmailEnvioParaArea2))
                        Email.CC.Add(UserDetails.ProcedimentosEmailEnvioParaArea2);

                    Email.BCC.Add(UserEmail);
                    Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);

                    Email.IsBodyHtml = true;
                    Email.EmailProcedimento = ProcedimentoEmail;

                    Email.SendEmail();
                }

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        #region The following methods map the MenuButton actions, named "Acções" in the "Imobilizado (1-2)" tab on the NAV form

        #region "Acções" MenuItem in the "Contabilidade (1)" section
        [HttpPost]
        public JsonResult ConfirmProcedimento([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                if (data.Estado != 1)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                };

                bool IsElementContabilidade = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoContabilidade);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementContabilidade && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                //ProcedimentosCcp Procedimento = DBProcedimentosCCP.GetProcedimentoById(data.No);
                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure ImobContabConfirmar.b
                ErrorHandler UnableToConfirmAssetPurchase = DBProcedimentosCCP.AccountingConfirmsAssetPurchase(data, UserDetails, 1);
                if (UnableToConfirmAssetPurchase.eReasonCode != 0)
                {
                    return Json(UnableToConfirmAssetPurchase);
                }
                // NAV ImobContabConfirmar.e

                // send emails.b
                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }

                AreaEmailRecipients DestinationEmails = new AreaEmailRecipients(data.CodigoAreaFuncional, EmailList);

                if (DestinationEmails.AreaID == -1)
                {
                    return Json(ReturnHandlers.UnknownArea);
                }
                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " - Aquisição de Imobilizado",
                    UtilizadorEmail = UserEmail,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = UserDetails.IdUtilizador,
                    DataHoraCriação = DateTime.Now
                };

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = data.No + " - Aquisição de Imobilizado",
                    From = DBProcedimentosCCP._EmailSender
                };


                data.ImobilizadoSimNao = data.ImobilizadoSimNao.HasValue ? data.ImobilizadoSimNao : false;
                if (data.ImobilizadoSimNao.Value)
                {
                    if (EmailAutomation.IsValidEmail(DestinationEmails.ToAddress))
                    {
                        Email.To.Add(DestinationEmails.ToAddress);
                        ProcedimentoEmail.EmailDestinatário = DestinationEmails.ToAddress;
                    }
                    else
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    }

                    if (EmailAutomation.IsValidEmail(DestinationEmails.CCAddress))
                        Email.CC.Add(DestinationEmails.CCAddress);
                }
                else
                {
                    FluxoTrabalhoListaControlo Fluxo = data.FluxoTrabalhoListaControlo.Where(e => e.Estado == 0).LastOrDefault();
                    if (Fluxo != null)
                    {
                        ConfigUtilizadores FluxoUser = DBProcedimentosCCP.GetUserDetails(Fluxo.User);
                        if (FluxoUser != null && EmailAutomation.IsValidEmail(FluxoUser.IdUtilizador))
                        {
                            Email.To.Add(FluxoUser.IdUtilizador);
                            ProcedimentoEmail.EmailDestinatário = FluxoUser.IdUtilizador;

                            if (EmailAutomation.IsValidEmail(FluxoUser.ProcedimentosEmailEnvioParaArea))
                                Email.CC.Add(FluxoUser.ProcedimentosEmailEnvioParaArea);
                        }
                        else
                        {
                            return Json(ReturnHandlers.InvalidEmailAddress);
                        }
                    }
                    else
                    {
                        Email.To.Add(UserEmail);
                        ProcedimentoEmail.EmailDestinatário = UserEmail;

                        if (EmailAutomation.IsValidEmail(UserDetails.ProcedimentosEmailEnvioParaArea))
                            Email.CC.Add(UserDetails.ProcedimentosEmailEnvioParaArea);
                    }
                }

                ProcedimentoEmail.TextoEmail = data.ComentarioImobContabilidade;
                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                Email.BCC.Add(UserEmail);
                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;
                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();
                // send emails.e

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }
        [HttpPost]
        public JsonResult ReturnToArea([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementContabilidade = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoContabilidade);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementContabilidade && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);

                // NAV Procedure ImobContabConfirmar.b
                ErrorHandler UnableToConfirmAssetPurchase = DBProcedimentosCCP.AccountingConfirmsAssetPurchase(data, UserDetails, 0);
                if (UnableToConfirmAssetPurchase.eReasonCode != 0)
                {
                    return Json(UnableToConfirmAssetPurchase);
                }
                // NAV ImobContabConfirmar.e

                if (data.FluxoTrabalhoListaControlo == null)
                    data.FluxoTrabalhoListaControlo = DBProcedimentosCCP.GetAllCheklistControloProcedimento(data.No);

                FluxoTrabalhoListaControlo Fluxo0 = data.FluxoTrabalhoListaControlo.Where(e => e.Estado == 0).LastOrDefault();

                string UserEmail = "";

                if (Fluxo0 != null)
                {
                    ConfigUtilizadores UserDetailsAux = DBProcedimentosCCP.GetUserDetails(Fluxo0.User);
                    if (UserDetailsAux != null)
                        UserDetails = UserDetailsAux;
                }

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    EmailDestinatário = UserEmail,
                    Assunto = data.No + " - Aquisição de Imobilizado (Devolução)",
                    TextoEmail = data.ComentarioImobContabilidade,
                    UtilizadorEmail = UserEmail,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = UserDetails.IdUtilizador,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.To.Add(UserEmail);
                if (EmailAutomation.IsValidEmail(UserDetails.ProcedimentosEmailEnvioParaArea))
                    Email.CC.Add(UserDetails.ProcedimentosEmailEnvioParaArea);

                string BCCAddress = DBProcedimentosCCP.GetUserEmail(User.Identity.Name);
                if (BCCAddress != null)
                    Email.BCC.Add(BCCAddress);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }
        #endregion

        #region "Acções" MenuItem in the "Area (2)" section
        public JsonResult GetPermission([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementArea = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoArea);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementArea && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 2)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure ImobAreaConfirmar.b
                ErrorHandler PermissionDenied = DBProcedimentosCCP.AreaConfirmsAssetPurchase(data, UserDetails, 1);
                if (PermissionDenied.eReasonCode != 0)
                {
                    return Json(PermissionDenied);
                }
                // NAV ImobAreaConfirmar.e

                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }

                if (!EmailAutomation.IsValidEmail(EmailList.EmailCa))
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " - Autorização de aquisição de Imobilizado",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = EmailList.EmailCa,
                    //TextoEmail = data.ElementosChecklist.ChecklistImobilizadoArea.ComentarioImobArea,
                    TextoEmail = data.ComentarioImobArea,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = UserDetails.IdUtilizador,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(EmailList.EmailCa);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.Success);
            }
        }

        public JsonResult ReturnToAccounting([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementArea = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoArea);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementArea && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 2)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);

                // NAV Procedure ImobAreaConfirmar.b
                ErrorHandler PermissionDenied = DBProcedimentosCCP.AreaConfirmsAssetPurchase(data, UserDetails, 0);
                if (PermissionDenied.eReasonCode != 0)
                {
                    return Json(PermissionDenied);
                }
                // NAV ImobAreaConfirmar.e

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult CloseProcedimentoArea([FromBody] ProcedimentoCCPView data)
        {
            // zpgm.28112017 - this action doens't have an implementation in NAV. Decide whether is relevant or not.
            return Json(null);
        }
        #endregion

        #endregion

        #region The following methods map the MenuButtons, named "Acções" in the "Fundamentos Decisão (4, 5 e 7)" tab on the NAV form

        #region MenuButton "Acções" in the "A Preencher pelas Compras (4)" section
        [HttpPost]
        public JsonResult SubmitToAccounting([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCompras = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCompras);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementCompras && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado < 4 || data.Estado > 6)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                if (string.IsNullOrEmpty(data.NomeProcesso))
                {
                    return Json(ReturnHandlers.ProcessNameNotSet);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure FDComprasConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.DecisionGroundsToBuy(data, UserDetails, 1);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV FDComprasConfirmar.e

                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }

                if (!EmailAutomation.IsValidEmail(EmailList.EmailFinanceiros))
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Parecer Financeiro",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = EmailList.EmailFinanceiros,
                    //TextoEmail = data.ElementosChecklist.ChecklistFundamentoCompras.ComentarioFundamentoCompras,
                    TextoEmail = data.ComentarioFundamentoCompras,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = UserDetails.IdUtilizador,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(EmailList.EmailFinanceiros);

                if (EmailAutomation.IsValidEmail(EmailList.Email2Financeiros))
                    Email.CC.Add(EmailList.Email2Financeiros);

                //NR 20180301 - Enviar 
                if (EmailAutomation.IsValidEmail(data.EmailDestinoCA))
                    Email.CC.Add(data.EmailDestinoCA);

                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        [HttpPost]
        public JsonResult SubmitToLegalDepartment([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCompras = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCompras);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementCompras && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado < 4 || data.Estado > 6)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                if (string.IsNullOrEmpty(data.NomeProcesso))
                {
                    return Json(ReturnHandlers.ProcessNameNotSet);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure FDComprasConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.DecisionGroundsToBuy(data, UserDetails, 2);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV FDComprasConfirmar.e

                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                }

                if (!EmailAutomation.IsValidEmail(EmailList.EmailJurididos))
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Parecer Juridico",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = EmailList.EmailFinanceiros,
                    //TextoEmail = data.ElementosChecklist.ChecklistFundamentoCompras.ComentarioFundamentoCompras,
                    TextoEmail = data.ComentarioFundamentoCompras,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = UserDetails.IdUtilizador,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(EmailList.EmailFinanceiros);

                if (EmailAutomation.IsValidEmail(EmailList.Email2Juridicos))
                    Email.CC.Add(EmailList.Email2Juridicos);

                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        [HttpPost]
        public JsonResult SubmitToArea([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCompras = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCompras);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementCompras && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 4)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                if (string.IsNullOrEmpty(data.NomeProcesso))
                {
                    return Json(ReturnHandlers.ProcessNameNotSet);
                }

                FluxoTrabalhoListaControlo Fluxo0 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo0 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 0).LastOrDefault();
                }
                else
                {
                    Fluxo0 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 0);
                }

                //É para enviar ao utilizador que mudou o último estado = 0
                ConfigUtilizadores UserDetails_TO = DBProcedimentosCCP.GetUserDetails(Fluxo0.User);
                string UserEmail_TO = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO.IdUtilizador))
                {
                    UserEmail_TO = UserDetails_TO.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };


                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure FDComprasConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.DecisionGroundsToBuy(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 3);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV FDComprasConfirmar.e

                //ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                //if (EmailList == null)
                //{
                //    return Json(ReturnHandlers.AddressListIsEmpty);
                //}

                //if (!EmailAutomation.IsValidEmail(EmailList.EmailJurididos))
                //{
                //    return Json(ReturnHandlers.InvalidEmailAddres);
                //};

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Abertura de Procedimento",
                    UtilizadorEmail = UserEmail,
                    //EmailDestinatário = EmailList.EmailFinanceiros,
                    //TextoEmail = data.ElementosChecklist.ChecklistFundamentoCompras.ComentarioFundamentoCompras,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = data.ComentarioFundamentoCompras,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);

                if (EmailAutomation.IsValidEmail(UserDetails_TO.ProcedimentosEmailEnvioParaArea2))
                    Email.CC.Add(UserDetails_TO.ProcedimentosEmailEnvioParaArea2);

                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        [HttpPost]
        public JsonResult ReturnToAreaToJustifyProcess([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCompras = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCompras);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementCompras && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 4)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                FluxoTrabalhoListaControlo Fluxo0 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo0 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 0).LastOrDefault();
                }
                else
                {
                    Fluxo0 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 0);
                }

                //É para enviar ao utilizador que mudou o último estado = 0
                ConfigUtilizadores UserDetails_TO = DBProcedimentosCCP.GetUserDetails(Fluxo0.User);
                string UserEmail_TO = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO.IdUtilizador))
                {
                    UserEmail_TO = UserDetails_TO.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure FDComprasConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.DecisionGroundsToBuy(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 0);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV FDComprasConfirmar.e

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Devolucao do Procedimento",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = data.ComentarioFundamentoCompras,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);

                if (EmailAutomation.IsValidEmail(UserDetails_TO.ProcedimentosEmailEnvioParaArea2))
                    Email.CC.Add(UserDetails_TO.ProcedimentosEmailEnvioParaArea2);

                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        [HttpPost]
        public JsonResult SubmitToBoardForOpening([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCompras = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCompras);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementCompras && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado < 4 || data.Estado > 6)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                if (data.Tipo == 0)
                {
                    return Json(ReturnHandlers.UnknownTipoProcedimento);
                }

                if (string.IsNullOrEmpty(data.EmailDestinoCA))
                {
                    return Json(ReturnHandlers.EmptyCAEmailAddress);
                }

                if (string.IsNullOrEmpty(data.NomeProcesso))
                {
                    return Json(ReturnHandlers.ProcessNameNotSet);
                }
                
                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                //Para substituir o email 'pdias@such.pt' que está martelado no NAV2009
                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                }

                if (!EmailAutomation.IsValidEmail(EmailList.EmailCa))
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };


                // NAV Procedure FDComprasConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.DecisionGroundsToBuy(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 4);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV FDComprasConfirmar.e

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Autorização de abertura",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = data.EmailDestinoCA,
                    TextoEmail = data.ComentarioFundamentoCompras,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(data.EmailDestinoCA);

                if (EmailAutomation.IsValidEmail(UserDetails.ProcedimentosEmailEnvioParaCa))
                    Email.CC.Add(UserDetails.ProcedimentosEmailEnvioParaCa);

                Email.BCC.Add(UserEmail);
                Email.BCC.Add(EmailList.EmailCa);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        [HttpPost]
        public JsonResult SubmitToBoardForGranting([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCompras = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCompras);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementCompras && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado < 4 || data.Estado > 6)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                if (data.Tipo == 0)
                {
                    return Json(ReturnHandlers.UnknownTipoProcedimento);
                }

                if (string.IsNullOrEmpty(data.EmailDestinoCA))
                {
                    return Json(ReturnHandlers.EmptyCAEmailAddress);
                }

                if (string.IsNullOrEmpty(data.NomeProcesso))
                {
                    return Json(ReturnHandlers.ProcessNameNotSet);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                //Para substituir o email 'pdias@such.pt' que está martelado no NAV2009
                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                }

                if (!EmailAutomation.IsValidEmail(EmailList.EmailCa))
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };


                // NAV Procedure FDComprasConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.DecisionGroundsToBuy(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 5);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV FDComprasConfirmar.e

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Autorização Adjudicação",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = data.EmailDestinoCA,
                    TextoEmail = data.ComentarioFundamentoCompras,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(data.EmailDestinoCA);

                if (EmailAutomation.IsValidEmail(UserDetails.ProcedimentosEmailEnvioParaCa))
                    Email.CC.Add(UserDetails.ProcedimentosEmailEnvioParaCa);

                Email.BCC.Add(UserEmail);
                Email.BCC.Add(EmailList.EmailCa);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        #endregion

        #region MenuButton "Acções" in the "A Preencher pelos Serviços Financeiros (5)" section
        public JsonResult SubmitToArea_5([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementContabilidade = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoContabilidade);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementContabilidade && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 5)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                FluxoTrabalhoListaControlo Fluxo0 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo0 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 0).LastOrDefault();
                }
                else
                {
                    Fluxo0 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 0);
                }

                //É para enviar ao utilizador que mudou o último estado = 0
                ConfigUtilizadores UserDetails_TO = DBProcedimentosCCP.GetUserDetails(Fluxo0.User);
                string UserEmail_TO = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO.IdUtilizador))
                {
                    UserEmail_TO = UserDetails_TO.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure FDFinancConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.FinancialDecisionGroundsToBuy(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 1);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV FDFinancConfirmar.e

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Fundamento decisão financeira",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = data.ComentarioFundamentoFinanceiros,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);
                
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult ReturnToAcquisition([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementContabilidade = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoContabilidade);

                if (!IsElementContabilidade)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (!(data.WorkflowFinanceiros.HasValue && data.WorkflowFinanceiros.Value))
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if ((data.Estado != 5) && (data.Estado != 6))
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                FluxoTrabalhoListaControlo Fluxo4 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo4 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 4).LastOrDefault();
                }
                else
                {
                    Fluxo4 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 4);
                }

                //É para enviar ao utilizador que mudou o último estado = 0
                ConfigUtilizadores UserDetails_TO = DBProcedimentosCCP.GetUserDetails(Fluxo4.User);
                string UserEmail_TO = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO.IdUtilizador))
                {
                    UserEmail_TO = UserDetails_TO.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };


                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };


                // NAV Procedure FDFinancConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.FinancialDecisionGroundsToBuy(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 1);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV FDFinancConfirmar.e

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Fundamento decisão financeira",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = data.ComentarioFundamentoFinanceiros,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);
                Email.CC.Add(UserEmail_TO);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        #endregion

        #region MenuButton "Acções" in the "A Preencher pela área - Pedido de Abertura de Procedimento (7)" section
        public JsonResult SubmitForAuthorization([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementArea = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoArea);

                if (!IsElementArea)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 7)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                if (string.IsNullOrEmpty(data.EmailDestinoCA))
                {
                    return Json(ReturnHandlers.EmptyCAEmailAddress);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure FDAreaConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.AreaDecisionGroundsToBuy(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 1);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV FDAreaConfirmar.e

                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }

                if (!EmailAutomation.IsValidEmail(EmailList.EmailCa))
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Pedido Abertura Procedimento",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = EmailList.EmailCa,
                    TextoEmail = data.FundamentacaoAreaComentario,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                //Email.To.Add(data.EmailDestinoCA);
                Email.To.Add(EmailList.EmailCa); 
                Email.CC.Add(EmailList.EmailCa);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult ReturnPurchase([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementArea = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoArea);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementArea && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 7)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                FluxoTrabalhoListaControlo Fluxo4 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo4 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 4).LastOrDefault();
                }
                else
                {
                    Fluxo4 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 4);
                }

                //É para enviar ao utilizador que mudou o último estado = 0
                ConfigUtilizadores UserDetails_TO = DBProcedimentosCCP.GetUserDetails(Fluxo4.User);
                string UserEmail_TO = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO.IdUtilizador))
                {
                    UserEmail_TO = UserDetails_TO.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure FDAreaConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.AreaDecisionGroundsToBuy(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 0);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV FDAreaConfirmar.e

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " -  Abertura de procedimento (Devolução)",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = data.FundamentacaoAreaComentario,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };
                
                Email.To.Add(UserEmail_TO);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult CloseProcess([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementArea = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoArea);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementArea && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 7)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                FluxoTrabalhoListaControlo Fluxo4 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo4 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 4).LastOrDefault();
                }
                else
                {
                    Fluxo4 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 4);
                }

                //É para enviar ao utilizador que mudou o último estado = 0
                ConfigUtilizadores UserDetails_TO = DBProcedimentosCCP.GetUserDetails(Fluxo4.User);
                string UserEmail_TO = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO.IdUtilizador))
                {
                    UserEmail_TO = UserDetails_TO.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure FDAreaConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.AreaDecisionGroundsToBuy(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 3);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV FDAreaConfirmar.e

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " -  Fecho do procedimento",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = data.FundamentacaoAreaComentario,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        #endregion

        #endregion

        #region The following methods map the MenuButtons, named "Acções" in the "Juridicos" tab on the NAV form

        #region MenuButton "Acções" in the "1ª Fase - Avaliação das Peças do Procedimento (A preencher na abertura do procedimento) (6)" section

        public JsonResult ReturnPurchase_6([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementJuridico = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoJuridico);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementJuridico)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (!(data.WorkflowJuridicos.HasValue && data.WorkflowJuridicos.Value))
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if ((data.Estado != 5) && (data.Estado != 6))
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                FluxoTrabalhoListaControlo Fluxo4 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo4 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 4).LastOrDefault();
                }
                else
                {
                    Fluxo4 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 4);
                }

                //É para enviar ao utilizador que mudou o último estado = 0
                ConfigUtilizadores UserDetails_TO = DBProcedimentosCCP.GetUserDetails(Fluxo4.User);
                string UserEmail_TO = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO.IdUtilizador))
                {
                    UserEmail_TO = UserDetails_TO.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }

                if (!EmailAutomation.IsValidEmail(EmailList.EmailFinanceiros))
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };


                // NAV Procedure Juridico1Fase.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.LegalFirstPhase(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 1);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV Juridico1Fase.e

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " -  Avaliação peças procedimento",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = data.ComentarioJuridico6,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);
                Email.CC.Add(EmailList.EmailFinanceiros);
                Email.CC.Add(EmailList.Email2Financeiros);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        #endregion

        #region MenuButton "Acções" in the "2ª Fase - Avaliação da minuta do Contrato (A preencher Após adjudicação) (14)" section

        public JsonResult SendToPurchase_14([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementJuridico = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoJuridico);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementJuridico)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 14)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                FluxoTrabalhoListaControlo Fluxo4 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo4 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 4).LastOrDefault();
                }
                else
                {
                    Fluxo4 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 4);
                }

                //É para enviar ao utilizador que mudou o último estado = 0
                ConfigUtilizadores UserDetails_TO = DBProcedimentosCCP.GetUserDetails(Fluxo4.User);
                string UserEmail_TO = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO.IdUtilizador))
                {
                    UserEmail_TO = UserDetails_TO.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure Juridico2Fase.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.LegalSecondPhase(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 1);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV Juridico2Fase.e

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " -  Avaliação minuta contrato",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = data.ComentarioJuridico14,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        #endregion

        #endregion

        #region The following methods map the Buttons, named "Confirmar", "Acções", "Enviar para Avaliação (Juri)" and "Enviar" in the "CCP" tab on the NAV form

        public JsonResult Confirm_9([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCompras = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCompras);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementCompras)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 9)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                if (!data.DataPublicacao.HasValue)
                {
                    return Json(ReturnHandlers.UnknownDatePublicacao);
                }

                // NAV Procedure CBPP_Confirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.CBPP_Confirmar(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name));
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV CBPP_Confirmar.e

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult Confirm_10([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCompras = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCompras);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementCompras)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 10)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                if (data.UtilizadorPublicacao == "")
                {
                    return Json(ReturnHandlers.ProcedimentoNotPublished);
                }

                if (!data.DataRecolha.HasValue)
                {
                    return Json(ReturnHandlers.UnknownDatePublicacao);
                }

                // NAV Procedure CBRP_Confirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.CBRP_Confirmar(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name));
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV CBRP_Confirmar.e

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult Confirm_11([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                if (data.Estado != 11)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                if (data.UtilizadorRecolha == "")
                {
                    return Json(ReturnHandlers.ProcedimentoPlatformNotGathering);
                }

                if (!data.DataValidRelatorioPreliminar.HasValue)
                {
                    return Json(ReturnHandlers.UnknownDatePublicacao);
                }

                // NAV Procedure CBVR_Confirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.CBVR_Confirmar(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name));
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV CBVR_Confirmar.e

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult Confirm_12([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                if (data.Estado != 12)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                if (data.UtilizadorValidRelatorioPreliminar == "")
                {
                    return Json(ReturnHandlers.ProcedimentoPreliminaryReportNotValidated);
                }

                if (!data.DataAudienciaPrevia.HasValue)
                {
                    return Json(ReturnHandlers.UnknownDatePublicacao);
                }

                // NAV Procedure CBAP_Confirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.CBAP_Confirmar(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name));
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV CBAP_Confirmar.e

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult MIEnvJuridicos([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                if (data.UtilizadorAudienciaPrevia == "")
                {
                    return Json(ReturnHandlers.ProcedimentoPriorHearingNotRegistered);
                }

                if (!data.DataRelatorioFinal.HasValue)
                {
                    return Json(ReturnHandlers.UnknownDatePublicacao);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure MIEnvJuridicos.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.MIEnvJuridicos(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name));
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV MIEnvJuridicos.e

                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                }

                if (!EmailAutomation.IsValidEmail(EmailList.EmailJurididos))
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                }

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Avaliação minuta contrato",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = EmailList.EmailFinanceiros,
                    //TextoEmail = data.ElementosChecklist.ChecklistFundamentoCompras.ComentarioFundamentoCompras,
                    TextoEmail = data.ComentarioRelatorioFinal,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = UserDetails.IdUtilizador,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(EmailList.EmailFinanceiros);

                if (EmailAutomation.IsValidEmail(EmailList.Email2Juridicos))
                    Email.CC.Add(EmailList.Email2Juridicos);

                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult MIEnvCompras([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                if (data.UtilizadorAudienciaPrevia == "")
                {
                    return Json(ReturnHandlers.ProcedimentoPriorHearingNotRegistered);
                }

                if (!data.DataRelatorioFinal.HasValue)
                {
                    return Json(ReturnHandlers.UnknownDatePublicacao);
                }

                // NAV Procedure MIEnvCompras.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.MIEnvCompras(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name));
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV MIEnvCompras.e

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }



        //TO DO!!!
        public JsonResult SendEmail_ToJuriAproval([FromBody] ProcedimentoCCPView data)
        {

            //  DEIXAR PARA O FIM, POIS PODE NÃO ESTAR A SER USADA A FUNCIONALIDADE NO NAV2009


            //if (data != null)
            //{
                
            //    ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
            //    string UserEmail = "";

            //    if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
            //    {
            //        UserEmail = UserDetails.IdUtilizador;
            //    }
            //    else
            //    {
            //        return Json(ReturnHandlers.InvalidEmailAddres);
            //    };
                

            //    EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
            //    {
            //        NºProcedimento = data.No,
            //        Assunto = data.No + " " + data.NomeProcesso + " -  Avaliação minuta contrato",
            //        UtilizadorEmail = UserEmail,
            //        //EmailDestinatário = UserEmail_TO,
            //        TextoEmail = data.ComentarioJuridico14,
            //        DataHoraEmail = DateTime.Now,
            //        UtilizadorCriação = User.Identity.Name,
            //        DataHoraCriação = DateTime.Now
            //    };

            //    if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
            //    {
            //        return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
            //    }

            //    data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

            //    SendEmailsProcedimentos Email = new SendEmailsProcedimentos
            //    {
            //        DisplayName = UserDetails.Nome,
            //        Subject = ProcedimentoEmail.Assunto,
            //        From = DBProcedimentosCCP._EmailSender
            //    };

            //    //Email.To.Add(UserEmail_TO);
            //    Email.BCC.Add(UserEmail);

            //    Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
            //    Email.IsBodyHtml = true;

            //    Email.EmailProcedimento = ProcedimentoEmail;

            //    Email.SendEmail();

            //    return Json(ReturnHandlers.Success);
            //}
            //else
            //{
            //    return Json(ReturnHandlers.NoData);
            //}

            return Json(ReturnHandlers.NoData);
        }

        #endregion

        #region The following methods map the Buttons, named "Enviar para Área", "Submeter o Processo" in the "Valores Adjudicação" tab on the NAV form

        public JsonResult CBVAEnviarArea([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCompras = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCompras);

                if (!IsElementCompras)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 15)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                if ((data.ResponsavelAdjudicacao15 != "") && (data.ResponsavelAdjudicacao15 != null))
                {
                    return Json(ReturnHandlers.ProcedimentoAlreadyConfirmed);
                }

                FluxoTrabalhoListaControlo Fluxo0 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo0 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 0).LastOrDefault();
                }
                else
                {
                    Fluxo0 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 0);
                }

                //É para enviar ao utilizador que mudou o último estado = 0
                ConfigUtilizadores UserDetails_TO = DBProcedimentosCCP.GetUserDetails(Fluxo0.User);
                string UserEmail_TO = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO.IdUtilizador))
                {
                    UserEmail_TO = UserDetails_TO.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }

                if (!EmailAutomation.IsValidEmail(EmailList.EmailCompras))
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure VAComprasConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.VAComprasConfirmar(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 1);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV VAComprasConfirmar.e

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " -  Abertura de Procedimento",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = data.ComentarioAdjudicacao15,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);
                Email.CC.Add(EmailList.EmailCompras);
                Email.CC.Add(EmailList.Email2Compras);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult CBVASubmetAprov([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementArea = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoArea);

                if (!IsElementArea)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 16)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                if (string.IsNullOrEmpty(data.EmailDestinoCA))
                {
                    return Json(ReturnHandlers.EmptyCAEmailAddress);
                }

                if (string.IsNullOrEmpty(data.ComentarioAdjudicacao16))
                {
                    return Json(ReturnHandlers.UncompletedDecisionProposal);
                }
                
                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }

                if (!EmailAutomation.IsValidEmail(EmailList.EmailCa))
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure VAAreaConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.VAAreaConfirmar(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 1);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV VAAreaConfirmar.e

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Pedido de adjudicação",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = data.EmailDestinoCA,
                    TextoEmail = data.ComentarioAdjudicacao16,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(data.EmailDestinoCA);
                Email.CC.Add(EmailList.EmailCa);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }
        #endregion

        #region The following methods map the Buttons, named "Autorizar", "Suspender Processo" and "Não Autorizar" in the "CA" tab on the NAV form

        #region Buttons in the "Autorizar Imobilizado (3)"

        public JsonResult CBRejeitarImobCA([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCA = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCA);

                if (!IsElementCA)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (!data.Imobilizado.HasValue && !data.Imobilizado.Value)
                {
                    return Json(ReturnHandlers.ProcedimentoNotImmobilized);
                }

                if (data.Estado > 8)
                {
                    return Json(ReturnHandlers.ProcedimentoAlreadyAuthorized);
                }


                if ((data.ResponsavelImobCA != "") && (data.ResponsavelImobCA != null))
                {
                    return Json(ReturnHandlers.ProcedimentoAlreadyTreated);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure CAConfirmarImob.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.BoardOfManagementConfirmImmobilized(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 0);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV CAConfirmarImob.e

                FluxoTrabalhoListaControlo Fluxo0 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo0 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 0).LastOrDefault();
                }
                else
                {
                    Fluxo0 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 0);
                }

                ConfigUtilizadores UserDetails_TO = DBProcedimentosCCP.GetUserDetails(Fluxo0.User);
                string UserEmail_TO = "";
                string UserEmail_TO_Area = "";
                string UserEmail_TO_2Area = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO.IdUtilizador))
                {
                    UserEmail_TO = UserDetails_TO.IdUtilizador;
                    UserEmail_TO_Area = UserDetails_TO.ProcedimentosEmailEnvioParaArea == null ? UserEmail_TO_Area : UserDetails_TO.ProcedimentosEmailEnvioParaArea;
                    UserEmail_TO_2Area = UserDetails_TO.ProcedimentosEmailEnvioParaArea2 == null ? UserEmail_TO_2Area : UserDetails_TO.ProcedimentosEmailEnvioParaArea2;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };
                
                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Autorização Imobilizado (Rejeitado)",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = data.ComentarioImobCA == "" || data.ComentarioImobCA == null ? "Não Autorizado" : data.ComentarioImobCA,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome == "" ? "Conselho de Administração" : UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);
                Email.BCC.Add(UserEmail_TO_Area);
                Email.BCC.Add(UserEmail_TO_2Area);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult CBDevolverImobCA([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCA = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCA);

                if (!IsElementCA)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (!data.Imobilizado.HasValue && !data.Imobilizado.Value)
                {
                    return Json(ReturnHandlers.ProcedimentoNotImmobilized);
                }

                if (data.Estado > 8)
                {
                    return Json(ReturnHandlers.ProcedimentoAlreadyAuthorized);
                }

                if ((data.ResponsavelImobCA != "") && (data.ResponsavelImobCA != null))
                {
                    return Json(ReturnHandlers.ProcedimentoAlreadyTreated);
                }

                //Colocar o Procedimento como suspenso!
                data.CaSuspenso = true;

                if (DBProcedimentosCCP.__UpdateProcedimento(data) == null)
                {
                    return Json(ReturnHandlers.UnableToUpdateProcedimento);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }

                if (!EmailAutomation.IsValidEmail(EmailList.EmailCompras))
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };


                string UserEmail_TO = "pdias@such.pt";
                //string UserEmail_TO = EmailList.EmailCompras;

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Suspensão abertura procedimento",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = (data.ComentarioImobCA == "" || data.ComentarioImobCA == null) ? "O processo está suspenso." : data.ComentarioImobCA,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome == "" ? "Conselho de Administração" : UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult CBAutorizarImobCA([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCA = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCA);

                if (!IsElementCA)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (!data.Imobilizado.HasValue && !data.Imobilizado.Value)
                {
                    return Json(ReturnHandlers.ProcedimentoNotImmobilized);
                }

                if (data.Estado > 8)
                {
                    return Json(ReturnHandlers.ProcedimentoAlreadyAuthorized);
                }

                if (data.Estado != 8)
                {
                    return Json(ReturnHandlers.ProcedimentoNotPossibleAuth);
                }

                if ((data.ResponsavelImobCA != "") && (data.ResponsavelImobCA != null))
                {
                    return Json(ReturnHandlers.ProcedimentoAlreadyTreated);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure CAConfirmarImob.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.BoardOfManagementConfirmImmobilized(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 1);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV CAConfirmarImob.e

                FluxoTrabalhoListaControlo Fluxo4 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo4 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 4).LastOrDefault();
                }
                else
                {
                    Fluxo4 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 4);
                }

                FluxoTrabalhoListaControlo Fluxo0 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo0 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 0).LastOrDefault();
                }
                else
                {
                    Fluxo0 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 0);
                }

                string UserEmail_TO = "";
                string UserEmail_TO_Area = "";
                string UserEmail_TO_2Area = "";

                ConfigUtilizadores UserDetails_TO = DBProcedimentosCCP.GetUserDetails(Fluxo4.User);
                ConfigUtilizadores UserDetails_CC = DBProcedimentosCCP.GetUserDetails(Fluxo0.User);

                if (EmailAutomation.IsValidEmail(UserDetails_TO.IdUtilizador))
                {
                    UserEmail_TO = UserDetails_TO.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                UserEmail_TO_Area = UserDetails_CC.ProcedimentosEmailEnvioParaArea == null ? UserEmail_TO_Area : UserDetails_CC.ProcedimentosEmailEnvioParaArea;
                UserEmail_TO_2Area = UserDetails_CC.ProcedimentosEmailEnvioParaArea2 == null ? UserEmail_TO_2Area : UserDetails_CC.ProcedimentosEmailEnvioParaArea2;

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Autorização Imobilizado",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = data.ComentarioImobCA == "" || data.ComentarioImobCA == null ? "Autorizado" : data.ComentarioImobCA,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome == "" ? "Conselho de Administração" : UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);
                Email.BCC.Add(UserEmail_TO_Area);
                Email.BCC.Add(UserEmail_TO_2Area);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        #endregion

        #region Buttons in the "1º Fase - Autorização de abertura do Procedimento (8)"

        public JsonResult CBRejeitar1CA([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCA = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCA);

                if (!IsElementCA)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if ((data.ResponsavelCA8 != "") && (data.ResponsavelCA8 != null))
                {
                    return Json(ReturnHandlers.ProcedimentoAlreadyTreated);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure CAConfirmarAbertura.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.BoardOfManagementConfirmOpening(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 2);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV CAConfirmarAbertura.e

                FluxoTrabalhoListaControlo Fluxo7 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo7 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 7).LastOrDefault();
                }
                else
                {
                    Fluxo7 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 7);
                }

                ConfigUtilizadores UserDetails_TO = DBProcedimentosCCP.GetUserDetails(Fluxo7.User);
                string UserEmail_TO = "";
                string UserEmail_TO_Area = "";
                string UserEmail_TO_2Area = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO.IdUtilizador))
                {
                    UserEmail_TO = UserDetails_TO.IdUtilizador;
                    UserEmail_TO_Area = UserDetails_TO.ProcedimentosEmailEnvioParaArea == null ? UserEmail_TO_Area : UserDetails_TO.ProcedimentosEmailEnvioParaArea;
                    UserEmail_TO_2Area = UserDetails_TO.ProcedimentosEmailEnvioParaArea2 == null ? UserEmail_TO_2Area : UserDetails_TO.ProcedimentosEmailEnvioParaArea2;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                FluxoTrabalhoListaControlo Fluxo4 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo4 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 4).LastOrDefault();
                }
                else
                {
                    Fluxo4 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 4);
                }

                ConfigUtilizadores UserDetails_CC = DBProcedimentosCCP.GetUserDetails(Fluxo4.User);
                string UserEmail_CC1 = "";

                if (EmailAutomation.IsValidEmail(UserDetails_CC.IdUtilizador))
                {
                    UserEmail_CC1 = UserDetails_CC.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Rejeição abertura procedimento",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = data.ComentarioCA8 == "" || data.ComentarioCA8 == null ? "Não Autorizado" : data.ComentarioCA8,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome == "" ? "Conselho de Administração" : UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);
                Email.BCC.Add(UserEmail_TO_Area);
                Email.BCC.Add(UserEmail_TO_2Area);
                Email.BCC.Add(UserEmail_CC1);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult CBDevolverAberturaCA([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCA = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCA);

                if (!IsElementCA)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if ((data.ResponsavelCA8 != "") && (data.ResponsavelCA8 != null))
                {
                    return Json(ReturnHandlers.ProcedimentoAlreadyTreated);
                }

                //Colocar o Procedimento como suspenso!
                data.CaSuspenso = true;

                if (DBProcedimentosCCP.__UpdateProcedimento(data) == null)
                {
                    return Json(ReturnHandlers.UnableToUpdateProcedimento);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }

                if (!EmailAutomation.IsValidEmail(EmailList.EmailCompras))
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };


                //string UserEmail_TO = "pdias@such.pt";
                string UserEmail_TO = EmailList.EmailCompras;

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Suspensão abertura procedimento",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = (data.ComentarioCA8 == "" || data.ComentarioCA8 == null) ? "O processo está suspenso." : data.ComentarioCA8,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome == "" ? "Conselho de Administração" : UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult CBAutorizar1CA([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCA = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCA);

                if (!IsElementCA)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 8)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                if ((data.ResponsavelCA8 != "") && (data.ResponsavelCA8 != null))
                {
                    return Json(ReturnHandlers.ProcedimentoAlreadyTreated);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                string gComent2CA = data.ComentarioCA8 == "" ? "Autorizado" : data.ComentarioCA8;
                string gComentarioEmail = gComent2CA;

                // NAV Procedure CAConfirmarAbertura.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.BoardOfManagementConfirmOpening(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 1);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV CAConfirmarAbertura.e

                FluxoTrabalhoListaControlo Fluxo4 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo4 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 4).LastOrDefault();
                }
                else
                {
                    Fluxo4 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 4);
                }

                ConfigUtilizadores UserDetails_TO_Compras = DBProcedimentosCCP.GetUserDetails(Fluxo4.User);
                string UserEmail_TO_Compras = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO_Compras.IdUtilizador))
                {
                    UserEmail_TO_Compras = UserDetails_TO_Compras.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }

                string gEmailPara = string.Empty;
                string gEmailCC1 = string.Empty;
                string gEmailCC2 = string.Empty;
                string gEmailCC3 = string.Empty;
                string gEmailBCC = string.Empty;
                string gAssunto = string.Empty;

                if (data.CodigoRegiao.StartsWith("1"))  // Engenharia
                {
                    if (!EmailAutomation.IsValidEmail(EmailList.Email5Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };
                    if (!EmailAutomation.IsValidEmail(EmailList.Email6Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };

                    gEmailPara = EmailList.Email5Compras;
                    gEmailCC1 = EmailList.Email6Compras;
                }
                else if (data.CodigoRegiao.StartsWith("0")) // Apoio
                {
                    if (!EmailAutomation.IsValidEmail(EmailList.Email3Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };

                    gEmailPara = EmailList.Email3Compras;
                }
                else if (data.CodigoRegiao.StartsWith("5")) // Nutrição
                {
                    if (!EmailAutomation.IsValidEmail(EmailList.Email4Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };

                    gEmailPara = EmailList.Email4Compras;
                }
                else if (data.CodigoRegiao.StartsWith("2")) // Ambiente
                {
                    if (!EmailAutomation.IsValidEmail(EmailList.Email7Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };
                    if (!EmailAutomation.IsValidEmail(EmailList.Email8Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };

                    gEmailPara = EmailList.Email7Compras;
                    gEmailCC1 = EmailList.Email8Compras;
                }
                else if (data.CodigoRegiao.StartsWith("72"))    // Gestão de Parques de Estacionamento
                {
                    if (!EmailAutomation.IsValidEmail(EmailList.Email7Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };
                    if (!EmailAutomation.IsValidEmail(EmailList.Email8Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };

                    gEmailPara = EmailList.Email7Compras;
                    gEmailCC1 = EmailList.Email8Compras;
                }

                FluxoTrabalhoListaControlo Fluxo0 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo0 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 0).LastOrDefault();
                }
                else
                {
                    Fluxo0 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 0);
                }

                ConfigUtilizadores UserDetails_Area = DBProcedimentosCCP.GetUserDetails(Fluxo0.User);
                string UserEmail_Area = "";
                string UserEmail_2Area = "";

                UserEmail_Area = EmailAutomation.IsValidEmail(UserDetails_Area.ProcedimentosEmailEnvioParaArea) ? UserDetails_Area.ProcedimentosEmailEnvioParaArea : string.Empty;
                UserEmail_2Area = EmailAutomation.IsValidEmail(UserDetails_Area.ProcedimentosEmailEnvioParaArea2) ? UserDetails_Area.ProcedimentosEmailEnvioParaArea2 : string.Empty;

                gEmailCC2 = UserEmail_Area;
                gEmailCC3 = UserEmail_2Area;
                gEmailBCC = UserEmail;

                gAssunto = data.No + " " + data.NomeProcesso + " - Autorizacao abertura procedimento";

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = gAssunto,
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = gEmailPara,
                    TextoEmail = gComentarioEmail,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email_1 = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome == "" ? "Conselho de Administração" : UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email_1.To.Add(gEmailPara);
                Email_1.CC.Add(gEmailCC1);
                Email_1.CC.Add(gEmailCC2);
                Email_1.CC.Add(gEmailCC3);
                Email_1.BCC.Add(UserEmail);

                Email_1.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email_1.IsBodyHtml = true;

                Email_1.EmailProcedimento = ProcedimentoEmail;

                Email_1.SendEmail();

                // Email de Alerta para elemento do CA
                if (data.RatificarCaAdjudicacao.HasValue && data.RatificarCaAdjudicacao.Value)
                {
                    gAssunto = data.No + " " + data.NomeProcesso + " - Ratificação CA Abertura";

                    EmailsProcedimentosCcp ProcedimentoEmail_3 = new EmailsProcedimentosCcp
                    {
                        NºProcedimento = data.No,
                        Assunto = gAssunto,
                        UtilizadorEmail = UserEmail,
                        EmailDestinatário = "CACompras@such.pt",
                        TextoEmail = gComentarioEmail,
                        DataHoraEmail = DateTime.Now,
                        UtilizadorCriação = User.Identity.Name,
                        DataHoraCriação = DateTime.Now
                    };

                    if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail_3))
                    {
                        return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                    }

                    data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail_3));

                    SendEmailsProcedimentos Email_3 = new SendEmailsProcedimentos
                    {
                        DisplayName = UserDetails.Nome == "" ? "Conselho de Administração" : UserDetails.Nome,
                        Subject = ProcedimentoEmail_3.Assunto,
                        From = DBProcedimentosCCP._EmailSender
                    };

                    Email_3.To.Add(UserEmail);
                    Email_3.CC.Add("CACompras@such.pt");

                    Email_3.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail_3.TextoEmail, UserDetails.Nome);
                    Email_3.IsBodyHtml = true;

                    Email_3.EmailProcedimento = ProcedimentoEmail_3;

                    Email_3.SendEmail();
                }

                // Email para pdias@such.pt
                if (data.CriticoAdjudicacao.HasValue && data.CriticoAdjudicacao.Value)
                {
                    gAssunto = data.No + " " + data.NomeProcesso + " - Procedimento Crítico";

                    EmailsProcedimentosCcp ProcedimentoEmail_4 = new EmailsProcedimentosCcp
                    {
                        NºProcedimento = data.No,
                        Assunto = gAssunto,
                        UtilizadorEmail = UserEmail,
                        EmailDestinatário = "pdias@such.pt",
                        TextoEmail = gComentarioEmail,
                        DataHoraEmail = DateTime.Now,
                        UtilizadorCriação = User.Identity.Name,
                        DataHoraCriação = DateTime.Now
                    };

                    if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail_4))
                    {
                        return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                    }

                    data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail_4));

                    SendEmailsProcedimentos Email_4 = new SendEmailsProcedimentos
                    {
                        DisplayName = UserDetails.Nome == "" ? "Conselho de Administração" : UserDetails.Nome,
                        Subject = ProcedimentoEmail_4.Assunto,
                        From = DBProcedimentosCCP._EmailSender
                    };

                    Email_4.To.Add("pdias@such.pt");

                    Email_4.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail_4.TextoEmail, UserDetails.Nome);
                    Email_4.IsBodyHtml = true;

                    Email_4.EmailProcedimento = ProcedimentoEmail_4;

                    Email_4.SendEmail();
                }

                // Envio email para os elementos do Juri
                gComentarioEmail = "Foi autorizado a abertura do procedimento, do qual faz parte como membro do juri";

                EmailsProcedimentosCcp ProcedimentoEmail_Juri = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = gAssunto,
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail,
                    TextoEmail = gComentarioEmail,
                    Email = true,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail_Juri))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail_Juri));

                SendEmailsProcedimentos Email_Juri = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome == "" ? "Conselho de Administração" : UserDetails.Nome,
                    Subject = ProcedimentoEmail_Juri.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                foreach (var elem_juri in data.ElementosJuri.Where(el => el.EnviarEmail == true).ToList())
                {
                    Email_Juri.To.Add(elem_juri.Email);
                }

                Email_Juri.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail_Juri.TextoEmail, UserDetails.Nome);
                Email_Juri.IsBodyHtml = true;

                Email_Juri.EmailProcedimento = ProcedimentoEmail_Juri;

                Email_Juri.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }



        #endregion

        #region Buttons in the "2ª Fase - Autorização de Adjudicação (17)"

        public JsonResult CBRejeitar2CA([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCA = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCA);

                if (!IsElementCA)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if ((data.ResponsavelCA17 != "") && (data.ResponsavelCA17 != null))
                {
                    return Json(ReturnHandlers.ProcedimentoAlreadyTreated);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure CAConfirmarAutorizacao.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.BoardOfManagementConfirmAuthorization(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 2);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV CAConfirmarAutorizacao.e

                FluxoTrabalhoListaControlo Fluxo7 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo7 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 7).LastOrDefault();
                }
                else
                {
                    Fluxo7 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 7);
                }

                ConfigUtilizadores UserDetails_TO = DBProcedimentosCCP.GetUserDetails(Fluxo7.User);
                string UserEmail_TO = "";
                string UserEmail_TO_Area = "";
                string UserEmail_TO_2Area = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO.IdUtilizador))
                {
                    UserEmail_TO = UserDetails_TO.IdUtilizador;
                    UserEmail_TO_Area = UserDetails_TO.ProcedimentosEmailEnvioParaArea == null ? UserEmail_TO_Area : UserDetails_TO.ProcedimentosEmailEnvioParaArea;
                    UserEmail_TO_2Area = UserDetails_TO.ProcedimentosEmailEnvioParaArea2 == null ? UserEmail_TO_2Area : UserDetails_TO.ProcedimentosEmailEnvioParaArea2;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                FluxoTrabalhoListaControlo Fluxo4 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo4 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 4).LastOrDefault();
                }
                else
                {
                    Fluxo4 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 4);
                }

                ConfigUtilizadores UserDetails_CC = DBProcedimentosCCP.GetUserDetails(Fluxo4.User);
                string UserEmail_CC1 = "";

                if (EmailAutomation.IsValidEmail(UserDetails_CC.IdUtilizador))
                {
                    UserEmail_CC1 = UserDetails_CC.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Rejeição Adjudicação",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = data.ComentarioCA17 == "" ? "Não Autorizado" : data.ComentarioCA17,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome == "" ? "Conselho de Administração" : UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);
                Email.BCC.Add(UserEmail_CC1);
                Email.BCC.Add(UserEmail_TO_Area);
                Email.BCC.Add(UserEmail_TO_2Area);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult CBDevolverAutorizacaoCA([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCA = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCA);

                if (!IsElementCA)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if ((data.ResponsavelCA17 != "") && (data.ResponsavelCA17 != null))
                {
                    return Json(ReturnHandlers.ProcedimentoAlreadyTreated);
                }

                //Colocar o Procedimento como suspenso!
                data.CaSuspenso = true;

                if (DBProcedimentosCCP.__UpdateProcedimento(data) == null)
                {
                    return Json(ReturnHandlers.UnableToUpdateProcedimento);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }

                if (!EmailAutomation.IsValidEmail(EmailList.EmailCompras))
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };


                //string UserEmail_TO = "pdias@such.pt";
                string UserEmail_TO = EmailList.EmailCompras;

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Suspensão abertura procedimento",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = (data.ComentarioCA17 == "" || data.ComentarioCA17 == null)? "O processo está suspenso." : data.ComentarioCA17,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome == "" ? "Conselho de Administração" : UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult CBAutorizar2CA([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCA = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCA);

                if (!IsElementCA)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 17)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                if ((data.ResponsavelCA17 != "") && (data.ResponsavelCA17 != null))
                {
                    return Json(ReturnHandlers.ProcedimentoAlreadyTreated);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                string gComent2CA = data.ComentarioCA17 == "" ? "Autorizado" : data.ComentarioCA17;
                string gComentarioEmail = gComent2CA;
                string gComentarioEmail2 = string.Empty;

                if ((data.AutorizacaoAdjudicacao.HasValue) && (data.AutorizacaoAdjudicacao.Value))
                {
                    // NAV Procedure CAConfirmarAutorizacao.b
                    ErrorHandler DecisionTaken = DBProcedimentosCCP.BoardOfManagementConfirmAuthorization(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 1);
                    if (DecisionTaken.eReasonCode != 0)
                    {
                        return Json(DecisionTaken);
                    }
                    // NAV CAConfirmarAutorizacao.e
                }

                if ((data.NaoAdjudicacaoEEncerramento.HasValue) && (data.NaoAdjudicacaoEEncerramento.Value))
                {
                    gComent2CA = "Não Autorizado";

                    // NAV Procedure CAConfirmarAutorizacao.b
                    ErrorHandler DecisionTaken = DBProcedimentosCCP.BoardOfManagementConfirmAuthorization(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 2);
                    if (DecisionTaken.eReasonCode != 0)
                    {
                        return Json(DecisionTaken);
                    }
                    // NAV CAConfirmarAutorizacao.e
                }

                if ((data.NaoAdjudicacaoESuspensao.HasValue) && (data.NaoAdjudicacaoESuspensao.Value))
                {
                    //Colocar o Procedimento como suspenso!
                    data.CaSuspenso = true;

                    if (DBProcedimentosCCP.__UpdateProcedimento(data) == null)
                    {
                        return Json(ReturnHandlers.UnableToUpdateProcedimento);
                    }

                    ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                    if (EmailList == null)
                    {
                        return Json(ReturnHandlers.AddressListIsEmpty);
                    }

                    if (!EmailAutomation.IsValidEmail(EmailList.EmailCompras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };

                    //string UserEmail_TO = "pdias@such.pt";
                    string UserEmail_TO = EmailList.EmailCompras;

                    EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                    {
                        NºProcedimento = data.No,
                        Assunto = data.No + " " + data.NomeProcesso + " - Suspensão abertura procedimento",
                        UtilizadorEmail = UserEmail,
                        EmailDestinatário = UserEmail_TO,
                        TextoEmail = (data.ComentarioCA17 == "" || data.ComentarioCA17 == null) ? "O processo está suspenso a pedido da área." : data.ComentarioCA17,
                        DataHoraEmail = DateTime.Now,
                        UtilizadorCriação = User.Identity.Name,
                        DataHoraCriação = DateTime.Now
                    };

                    if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                    {
                        return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                    }

                    data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                    SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                    {
                        DisplayName = UserDetails.Nome == "" ? "Conselho de Administração" : UserDetails.Nome,
                        Subject = ProcedimentoEmail.Assunto,
                        From = DBProcedimentosCCP._EmailSender
                    };

                    Email.To.Add(UserEmail_TO);
                    Email.BCC.Add(UserEmail);

                    Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                    Email.IsBodyHtml = true;

                    Email.EmailProcedimento = ProcedimentoEmail;

                    Email.SendEmail();

                    return Json(ReturnHandlers.Success);
                }

                // email para Compras e Área

                FluxoTrabalhoListaControlo Fluxo4 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo4 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 4).LastOrDefault();
                }
                else
                {
                    Fluxo4 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 4);
                }

                ConfigUtilizadores UserDetails_TO_Compras = DBProcedimentosCCP.GetUserDetails(Fluxo4.User);
                string UserEmail_TO_Compras = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO_Compras.IdUtilizador))
                {
                    UserEmail_TO_Compras = UserDetails_TO_Compras.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfiguracaoCcp EmailList_2 = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList_2 == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }
                
                string gEmailPara = string.Empty;
                string gEmailCC1 = string.Empty;
                string gEmailCC2 = string.Empty;
                string gEmailCC3 = string.Empty;
                string gEmailCC4 = string.Empty;
                string gEmailCC5 = string.Empty;
                string gEmailBCC = string.Empty;
                string gAssunto = string.Empty;

                if (data.CodigoRegiao.StartsWith("1"))  // Engenharia
                {
                    if (!EmailAutomation.IsValidEmail(EmailList_2.Email5Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };
                    if (!EmailAutomation.IsValidEmail(EmailList_2.Email6Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };

                    gEmailPara = EmailList_2.Email5Compras;
                    gEmailCC1 = EmailList_2.Email6Compras;
                }
                else if (data.CodigoRegiao.StartsWith("0")) // Apoio
                {
                    if (!EmailAutomation.IsValidEmail(EmailList_2.Email3Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };

                    gEmailPara = EmailList_2.Email3Compras;
                }
                else if (data.CodigoRegiao.StartsWith("5")) // Nutrição
                {
                    if (!EmailAutomation.IsValidEmail(EmailList_2.Email4Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };

                    gEmailPara = EmailList_2.Email4Compras;
                }
                else if (data.CodigoRegiao.StartsWith("2")) // Ambiente
                {
                    if (!EmailAutomation.IsValidEmail(EmailList_2.Email7Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };
                    if (!EmailAutomation.IsValidEmail(EmailList_2.Email8Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };

                    gEmailPara = EmailList_2.Email7Compras;
                    gEmailCC1 = EmailList_2.Email8Compras;
                }
                else if (data.CodigoRegiao.StartsWith("72"))    // Gestão de Parques de Estacionamento
                {
                    if (!EmailAutomation.IsValidEmail(EmailList_2.Email7Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };
                    if (!EmailAutomation.IsValidEmail(EmailList_2.Email8Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };

                    gEmailPara = EmailList_2.Email7Compras;
                    gEmailCC1 = EmailList_2.Email8Compras;
                }

                FluxoTrabalhoListaControlo Fluxo0 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo0 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 0).LastOrDefault();
                }
                else
                {
                    Fluxo0 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 0);
                }

                ConfigUtilizadores UserDetails_Area = DBProcedimentosCCP.GetUserDetails(Fluxo0.User);
                string UserEmail_Area = "";
                string UserEmail_2Area = "";

                UserEmail_Area = EmailAutomation.IsValidEmail(UserDetails_Area.ProcedimentosEmailEnvioParaArea) ? UserDetails_Area.ProcedimentosEmailEnvioParaArea : string.Empty;
                UserEmail_2Area = EmailAutomation.IsValidEmail(UserDetails_Area.ProcedimentosEmailEnvioParaArea2) ? UserDetails_Area.ProcedimentosEmailEnvioParaArea2 : string.Empty;

                gEmailCC2 = UserEmail_Area;
                gEmailCC3 = UserEmail_2Area;
                gEmailCC4 = EmailAutomation.IsValidEmail(EmailList_2.EmailFinanceiros) ? EmailList_2.EmailFinanceiros : string.Empty;
                gEmailCC5 = EmailAutomation.IsValidEmail(EmailList_2.Email2Financeiros) ? EmailList_2.Email2Financeiros : string.Empty;
                gEmailBCC = UserEmail;

                gAssunto = data.No + " " + data.NomeProcesso + " - Autorização Adjudicação";
                if (data.NaoAdjudicacaoEEncerramento.HasValue && data.NaoAdjudicacaoEEncerramento.Value)
                {
                    gAssunto = data.No + " " + data.NomeProcesso + " - Rejeição Adjudicação a pedido da área";
                }

                if (data.Fornecedor != null && data.Fornecedor != string.Empty)
                {
                    gComentarioEmail2 = "Fornecedor : " + data.Fornecedor.ToString();
                }

                if (data.ValorAdjudicacaoAtual.HasValue && data.ValorAdjudicacaoAtual.Value != 0)
                {
                    if (gComentarioEmail2.ToString() != string.Empty)
                        gComentarioEmail2 = gComentarioEmail2 + Environment.NewLine + String.Format("Valor Adjudicação : {0:N2}", data.ValorAdjudicacaoAtual.Value);
                    else
                        gComentarioEmail2 = String.Format("Valor Adjudicação : {0:N2}", data.ValorAdjudicacaoAtual.Value);
                }

                EmailsProcedimentosCcp ProcedimentoEmail_2 = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = gAssunto,
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = gEmailPara,
                    TextoEmail = gComentarioEmail + gComentarioEmail2 != string.Empty ? Environment.NewLine + gComentarioEmail2 : string.Empty,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail_2))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail_2));

                SendEmailsProcedimentos Email_2 = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome == "" ? "Conselho de Administração" : UserDetails.Nome,
                    Subject = ProcedimentoEmail_2.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email_2.To.Add(gEmailPara);
                Email_2.CC.Add(gEmailCC1);
                Email_2.CC.Add(gEmailCC2);
                Email_2.CC.Add(gEmailCC3);
                Email_2.CC.Add(gEmailCC4);
                Email_2.CC.Add(gEmailCC5);
                Email_2.BCC.Add(UserEmail);

                Email_2.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail_2.TextoEmail, UserDetails.Nome);
                Email_2.IsBodyHtml = true;

                Email_2.EmailProcedimento = ProcedimentoEmail_2;

                Email_2.SendEmail();

                // Email de Alerta para elemento do CA
                if (data.RatificarCaAdjudicacao.HasValue && data.RatificarCaAdjudicacao.Value)
                {
                    gAssunto = data.No + " " + data.NomeProcesso + " - Ratificação CA Adjudicação";

                    EmailsProcedimentosCcp ProcedimentoEmail_3 = new EmailsProcedimentosCcp
                    {
                        NºProcedimento = data.No,
                        Assunto = gAssunto,
                        UtilizadorEmail = UserEmail,
                        EmailDestinatário = "CACompras@such.pt",
                        TextoEmail = gComentarioEmail + gComentarioEmail2 != string.Empty ? Environment.NewLine + gComentarioEmail2 : string.Empty,
                        DataHoraEmail = DateTime.Now,
                        UtilizadorCriação = User.Identity.Name,
                        DataHoraCriação = DateTime.Now
                    };

                    if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail_3))
                    {
                        return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                    }

                    data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail_3));

                    SendEmailsProcedimentos Email_3 = new SendEmailsProcedimentos
                    {
                        DisplayName = UserDetails.Nome == "" ? "Conselho de Administração" : UserDetails.Nome,
                        Subject = ProcedimentoEmail_3.Assunto,
                        From = DBProcedimentosCCP._EmailSender
                    };

                    Email_3.To.Add(UserEmail);
                    Email_3.CC.Add("CACompras@such.pt");

                    Email_3.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail_3.TextoEmail, UserDetails.Nome);
                    Email_3.IsBodyHtml = true;

                    Email_3.EmailProcedimento = ProcedimentoEmail_3;

                    Email_3.SendEmail();
                }

                // Email para pdias@such.pt
                if (data.CriticoAdjudicacao.HasValue && data.CriticoAdjudicacao.Value)
                {
                    gAssunto = data.No + " " + data.NomeProcesso + " - Procedimento Crítico (Adjudicação)";

                    EmailsProcedimentosCcp ProcedimentoEmail_4 = new EmailsProcedimentosCcp
                    {
                        NºProcedimento = data.No,
                        Assunto = gAssunto,
                        UtilizadorEmail = UserEmail,
                        EmailDestinatário = "pdias@such.pt",
                        TextoEmail = gComentarioEmail + gComentarioEmail2 != string.Empty ? Environment.NewLine + gComentarioEmail2 : string.Empty,
                        DataHoraEmail = DateTime.Now,
                        UtilizadorCriação = User.Identity.Name,
                        DataHoraCriação = DateTime.Now
                    };

                    if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail_4))
                    {
                        return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                    }

                    data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail_4));

                    SendEmailsProcedimentos Email_4 = new SendEmailsProcedimentos
                    {
                        DisplayName = UserDetails.Nome == "" ? "Conselho de Administração" : UserDetails.Nome,
                        Subject = ProcedimentoEmail_4.Assunto,
                        From = DBProcedimentosCCP._EmailSender
                    };

                    Email_4.To.Add("pdias@such.pt");

                    Email_4.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail_4.TextoEmail, UserDetails.Nome);
                    Email_4.IsBodyHtml = true;

                    Email_4.EmailProcedimento = ProcedimentoEmail_4;

                    Email_4.SendEmail();
                }

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        /// <summary>
        /// Necessário por causa de um procedimento que necessitava de efectuar várias perguntas ao user. Precisei de criar um iron-ajax com referência a este procedimento
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public JsonResult Dummy([FromBody] ProcedimentoCCPView data)
        {
            return Json(ReturnHandlers.Success);
        }


        #endregion

        #endregion

        #region The following methods map the Buttons, named "Confirmar", "Criar Novo Contrato/Acordo", "Criar Contrato" and "Criar Encomenda" in the "Adjudicação" tab on the NAV form

        public JsonResult Confirm_18([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCompras = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCompras);

                if (!IsElementCompras)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 18)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                if (!data.DataNotificacao.HasValue)
                {
                    return Json(ReturnHandlers.UnknownDatePublicacao);
                }

                // NAV Procedure CBAP_Confirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.CBConfirmaNotif(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name));
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV CBAP_Confirmar.e

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        #endregion

        #region The following methods map the Buttons, named "Confirmar" in the "Workflow" tab on the NAV form

        public JsonResult MudarEstado([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                if (data.NovoEstado.Value >= data.Estado.Value)
                {
                    return Json(ReturnHandlers.ProcedimentoNotPossibleChangeToUpper);
                }

                if ((data.NovoEstado.Value == 1 || data.NovoEstado.Value == 2) && (data.Imobilizado.HasValue && data.Imobilizado.Value))
                {
                    return Json(ReturnHandlers.ProcedimentoNotPossibleChangeToImob);
                }

                if (data.NovoEstado.Value < data.Estado.Value)
                {
                    // NAV Procedure <Control1000000484> - OnPush()
                    ErrorHandler DecisionTaken = DBProcedimentosCCP.MudarEstado(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name));
                    if (DecisionTaken.eReasonCode != 0)
                    {
                        return Json(DecisionTaken);
                    }
                    // NAV <Control1000000484> - OnPush()
                }






                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        #endregion


        #region The following methods map the MenuButton actions, named "Acções" in the "Imobilizado (1-2)" tab on the NAV form Procedimentos Simplificados
        #region "Acções" MenuItem in the "Contabilidade (1)" section
        [HttpPost]
        public JsonResult ConfirmProcedimento_Simplificado([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                if (data.Estado != 1)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                };

                bool IsElementContabilidade = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoContabilidade);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementContabilidade && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                //ProcedimentosCcp Procedimento = DBProcedimentosCCP.GetProcedimentoById(data.No);
                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure ImobContabConfirmar.b
                ErrorHandler UnableToConfirmAssetPurchase = DBProcedimentosCCP.AccountingConfirmsAssetPurchase_Simplificado(data, UserDetails, 1);
                if (UnableToConfirmAssetPurchase.eReasonCode != 0)
                {
                    return Json(UnableToConfirmAssetPurchase);
                }
                // NAV ImobContabConfirmar.e

                // send emails.b
                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }

                
                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " - Aquisição de Imobilizado",
                    UtilizadorEmail = UserEmail,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = UserDetails.IdUtilizador,
                    DataHoraCriação = DateTime.Now
                };

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = data.No + " - Aquisição de Imobilizado",
                    From = DBProcedimentosCCP._EmailSender
                };


                string gEmailPara = string.Empty;
                string gEmailCC1 = string.Empty;
                string gEmailBCC = string.Empty;
                string gAssunto = string.Empty;

                data.ImobilizadoSimNao = data.ImobilizadoSimNao.HasValue ? data.ImobilizadoSimNao : false;

                if (data.ImobilizadoSimNao.Value)
                {
                    if (data.CodigoRegiao.StartsWith("1"))  // Engenharia
                    {
                        if (!EmailAutomation.IsValidEmail(EmailList.Email10Compras))
                        {
                            return Json(ReturnHandlers.InvalidEmailAddress);
                        };
                        if (!EmailAutomation.IsValidEmail(EmailList.Email11Compras))
                        {
                            return Json(ReturnHandlers.InvalidEmailAddress);
                        };

                        gEmailPara = EmailList.Email10Compras;
                        gEmailCC1 = EmailList.Email11Compras;
                    }
                    else if (data.CodigoRegiao.StartsWith("0")) // Apoio
                    {
                        if (!EmailAutomation.IsValidEmail(EmailList.Email3Compras))
                        {
                            return Json(ReturnHandlers.InvalidEmailAddress);
                        };

                        gEmailPara = EmailList.Email3Compras;
                    }
                    else if (data.CodigoRegiao.StartsWith("5")) // Nutrição
                    {
                        if (!EmailAutomation.IsValidEmail(EmailList.Email4Compras))
                        {
                            return Json(ReturnHandlers.InvalidEmailAddress);
                        };

                        gEmailPara = EmailList.Email4Compras;
                    }
                    else if (data.CodigoRegiao.StartsWith("2")) // Ambiente
                    {
                        if (!EmailAutomation.IsValidEmail(EmailList.Email9Compras))
                        {
                            return Json(ReturnHandlers.InvalidEmailAddress);
                        };
                        if (!EmailAutomation.IsValidEmail(EmailList.Email8Compras))
                        {
                            return Json(ReturnHandlers.InvalidEmailAddress);
                        };

                        gEmailPara = EmailList.Email9Compras;
                        gEmailCC1 = EmailList.Email8Compras;
                    }
                    else if (data.CodigoRegiao.StartsWith("72"))    // Gestão de Parques de Estacionamento
                    {
                        if (!EmailAutomation.IsValidEmail(EmailList.Email9Compras))
                        {
                            return Json(ReturnHandlers.InvalidEmailAddress);
                        };
                        if (!EmailAutomation.IsValidEmail(EmailList.Email8Compras))
                        {
                            return Json(ReturnHandlers.InvalidEmailAddress);
                        };

                        gEmailPara = EmailList.Email9Compras;
                        gEmailCC1 = EmailList.Email8Compras;
                    }
                }
                else
                {
                    if (EmailAutomation.IsValidEmail(UserDetails.ProcedimentosEmailEnvioParaArea2))
                    {
                        gEmailPara = UserDetails.ProcedimentosEmailEnvioParaArea2;
                    }
                    else
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };
                }

                Email.To.Add(gEmailPara);
                Email.CC.Add(gEmailCC1);
                ProcedimentoEmail.EmailDestinatário = gEmailPara;

                ProcedimentoEmail.TextoEmail = data.ComentarioImobContabilidade;
                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                Email.BCC.Add(UserEmail);
                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;
                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();
                // send emails.e

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        #endregion

        #region "Acções" MenuItem in the "Area (2)" section

        public JsonResult GetPermission_Simplificado([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementArea = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoArea);
                //bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementArea /*&& !IsGestorProcesso*/)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 2)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure ImobAreaConfirmar.b
                ErrorHandler PermissionDenied = DBProcedimentosCCP.AreaConfirmsAssetPurchase_Simplificado(data, UserDetails, 1);
                if (PermissionDenied.eReasonCode != 0)
                {
                    return Json(PermissionDenied);
                }
                // NAV ImobAreaConfirmar.e

                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }

                if (!EmailAutomation.IsValidEmail(EmailList.EmailCa))
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " - Autorização de aquisição de Imobilizado",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = EmailList.EmailCa,
                    //TextoEmail = data.ElementosChecklist.ChecklistImobilizadoArea.ComentarioImobArea,
                    TextoEmail = data.ComentarioImobArea,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = UserDetails.IdUtilizador,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(EmailList.EmailCa);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.Success);
            }
        }

        public JsonResult ReturnToAccounting_Simplificado([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementArea = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoArea);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementArea && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 2)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);

                // NAV Procedure ImobAreaConfirmar.b
                ErrorHandler PermissionDenied = DBProcedimentosCCP.AreaConfirmsAssetPurchase_Simplificado(data, UserDetails, 0);
                if (PermissionDenied.eReasonCode != 0)
                {
                    return Json(PermissionDenied);
                }
                // NAV ImobAreaConfirmar.e

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult CloseProcedimentoArea_Simplificado([FromBody] ProcedimentoCCPView data)
        {
            // nr.20180627 - this action doens't have an implementation in NAV. Decide whether is relevant or not.
            return Json(null);
        }

        #endregion

        #endregion

        #region The following methods map the MenuButtons, named "Acções" in the "Fundamentos Decisão (4, 5 e 16)" tab on the NAV form Procedimentos Simplificados

        #region MenuButton "Acções" in the "A Preencher pelas Compras (4)" section
        [HttpPost]
        public JsonResult SubmitToAccounting_Simplificado([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCompras = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCompras);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementCompras && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado < 4 || data.Estado > 6)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                if (string.IsNullOrEmpty(data.NomeProcesso))
                {
                    return Json(ReturnHandlers.ProcessNameNotSet);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure FDComprasConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.DecisionGroundsToBuy_Simplificado(data, UserDetails, 1);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV FDComprasConfirmar.e

                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }

                if (!EmailAutomation.IsValidEmail(EmailList.EmailFinanceiros))
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Parecer Financeiro",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = EmailList.EmailFinanceiros,
                    //TextoEmail = data.ElementosChecklist.ChecklistFundamentoCompras.ComentarioFundamentoCompras,
                    TextoEmail = data.ComentarioFundamentoCompras,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = UserDetails.IdUtilizador,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(EmailList.EmailFinanceiros);

                if (EmailAutomation.IsValidEmail(EmailList.Email2Financeiros))
                    Email.CC.Add(EmailList.Email2Financeiros);

                //NR 20180301 - Enviar 
                if (EmailAutomation.IsValidEmail(data.EmailDestinoCA))
                    Email.CC.Add(data.EmailDestinoCA);

                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        [HttpPost]
        public JsonResult SubmitToArea_Simplificado([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCompras = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCompras);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementCompras && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 4)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                if (string.IsNullOrEmpty(data.NomeProcesso))
                {
                    return Json(ReturnHandlers.ProcessNameNotSet);
                }

                FluxoTrabalhoListaControlo Fluxo0 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo0 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 0).LastOrDefault();
                }
                else
                {
                    Fluxo0 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 0);
                }

                //É para enviar ao utilizador que mudou o último estado = 0
                ConfigUtilizadores UserDetails_TO = DBProcedimentosCCP.GetUserDetails(Fluxo0.User);
                string UserEmail_TO = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO.IdUtilizador))
                {
                    UserEmail_TO = UserDetails_TO.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };


                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure FDComprasConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.DecisionGroundsToBuy_Simplificado(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 3);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV FDComprasConfirmar.e

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Abertura de Procedimento",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = data.ComentarioFundamentoCompras,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);

                if (EmailAutomation.IsValidEmail(UserDetails_TO.ProcedimentosEmailEnvioParaArea2))
                    Email.CC.Add(UserDetails_TO.ProcedimentosEmailEnvioParaArea2);

                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        [HttpPost]
        public JsonResult ReturnToArea_Simplificado([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCompras = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCompras);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if (!IsElementCompras && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 4)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                FluxoTrabalhoListaControlo Fluxo0 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo0 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 0).LastOrDefault();
                }
                else
                {
                    Fluxo0 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 0);
                }

                //É para enviar ao utilizador que mudou o último estado = 0
                ConfigUtilizadores UserDetails_TO = DBProcedimentosCCP.GetUserDetails(Fluxo0.User);
                string UserEmail_TO = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO.IdUtilizador))
                {
                    UserEmail_TO = UserDetails_TO.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure FDComprasConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.DecisionGroundsToBuy_Simplificado(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 0);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV FDComprasConfirmar.e

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Devolução de Procedimento",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = data.ComentarioFundamentoCompras,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);

                if (EmailAutomation.IsValidEmail(UserDetails_TO.ProcedimentosEmailEnvioParaArea2))
                    Email.CC.Add(UserDetails_TO.ProcedimentosEmailEnvioParaArea2);

                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }
        #endregion

        #region MenuButton "Acções" in the "A Preencher pelos Serviços Financeiros (5)" section

        public JsonResult ReturnToAcquisition_Simplificado([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementContabilidade = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoContabilidade);

                if (!IsElementContabilidade)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }
                
                FluxoTrabalhoListaControlo Fluxo4 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo4 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 4).LastOrDefault();
                }
                else
                {
                    Fluxo4 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 4);
                }

                //É para enviar ao utilizador que mudou o último estado = 0
                ConfigUtilizadores UserDetails_TO = DBProcedimentosCCP.GetUserDetails(Fluxo4.User);
                string UserEmail_TO = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO.IdUtilizador))
                {
                    UserEmail_TO = UserDetails_TO.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };


                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };


                // NAV Procedure FDFinancConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.FinancialDecisionGroundsToBuy_Simplificado(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 1);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV FDFinancConfirmar.e

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Fundamento decisão financeira",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = data.ComentarioFundamentoFinanceiros,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);
                Email.CC.Add(UserEmail_TO);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }
        #endregion

        #region MenuButton "Acções" in the "A Preencher pelos Serviços Financeiros (16)" section

        public JsonResult CBVASubmetAprov_Simplificado([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementArea = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoArea);

                if (!IsElementArea)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if (data.Estado != 16)
                {
                    return Json(ReturnHandlers.StateNotAllowed);
                }

                if (string.IsNullOrEmpty(data.EmailDestinoCA))
                {
                    return Json(ReturnHandlers.EmptyCAEmailAddress);
                }
                
                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }

                if (!EmailAutomation.IsValidEmail(EmailList.EmailCa))
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure VAAreaConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.VAAreaConfirmar_Simplificado(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 1);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV VAAreaConfirmar.e

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Pedido de adjudicação",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = data.EmailDestinoCA,
                    TextoEmail = data.ComentarioAdjudicacao16,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(data.EmailDestinoCA);
                Email.CC.Add(EmailList.EmailCa);
                Email.BCC.Add(UserEmail);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        #endregion

        #endregion

        #region The following methods map the Buttons, named "Autorizar", "Devolver Processo" and "Não Autorizar" in the "CA" tab on the NAV form Procedimentos Simplificados

        public JsonResult CBAutorizar2CA_Simplificado([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCA = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCA);

                if (!IsElementCA)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if ((data.ResponsavelCA17 != "") && (data.ResponsavelCA17 != null))
                {
                    return Json(ReturnHandlers.ProcedimentoAlreadyTreated);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                string gComent2CA = data.ComentarioCA17 == "" ? "Autorizado" : data.ComentarioCA17;
                string gComentarioEmail = gComent2CA;
                string gComentarioEmail2 = string.Empty;

                // NAV Procedure CAConfirmarAutorizacao.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.BoardOfManagementConfirmAuthorization_Simplificado(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 1);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV CAConfirmarAutorizacao.e

                // email para Compras e Área
                FluxoTrabalhoListaControlo Fluxo4 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo4 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 4).LastOrDefault();
                }
                else
                {
                    Fluxo4 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 4);
                }

                ConfigUtilizadores UserDetails_TO_Compras = DBProcedimentosCCP.GetUserDetails(Fluxo4.User);
                string UserEmail_TO_Compras = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO_Compras.IdUtilizador))
                {
                    UserEmail_TO_Compras = UserDetails_TO_Compras.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                ConfiguracaoCcp EmailList_2 = DBProcedimentosCCP.GetConfiguracaoCCP();
                if (EmailList_2 == null)
                {
                    return Json(ReturnHandlers.AddressListIsEmpty);
                }

                string gEmailPara = string.Empty;
                string gEmailCC1 = string.Empty;
                string gEmailCC2 = string.Empty;
                string gEmailCC3 = string.Empty;
                string gEmailBCC = string.Empty;
                string gAssunto = string.Empty;

                if (data.CodigoRegiao.StartsWith("1"))  // Engenharia
                {
                    if (!EmailAutomation.IsValidEmail(EmailList_2.Email10Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };
                    if (!EmailAutomation.IsValidEmail(EmailList_2.Email11Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };

                    gEmailPara = EmailList_2.Email10Compras;
                    gEmailCC1 = EmailList_2.Email11Compras;
                }
                else if (data.CodigoRegiao.StartsWith("0")) // Apoio
                {
                    if (!EmailAutomation.IsValidEmail(EmailList_2.Email3Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };

                    gEmailPara = EmailList_2.Email3Compras;
                }
                else if (data.CodigoRegiao.StartsWith("5")) // Nutrição
                {
                    if (!EmailAutomation.IsValidEmail(EmailList_2.Email4Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };

                    gEmailPara = EmailList_2.Email4Compras;
                }
                else if (data.CodigoRegiao.StartsWith("2")) // Ambiente
                {
                    if (!EmailAutomation.IsValidEmail(EmailList_2.Email9Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };
                    if (!EmailAutomation.IsValidEmail(EmailList_2.Email8Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };

                    gEmailPara = EmailList_2.Email9Compras;
                    gEmailCC1 = EmailList_2.Email8Compras;
                }
                else if (data.CodigoRegiao.StartsWith("72"))    // Gestão de Parques de Estacionamento
                {
                    if (!EmailAutomation.IsValidEmail(EmailList_2.Email9Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };
                    if (!EmailAutomation.IsValidEmail(EmailList_2.Email8Compras))
                    {
                        return Json(ReturnHandlers.InvalidEmailAddress);
                    };

                    gEmailPara = EmailList_2.Email9Compras;
                    gEmailCC1 = EmailList_2.Email8Compras;
                }

                FluxoTrabalhoListaControlo Fluxo0 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo0 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 0).LastOrDefault();
                }
                else
                {
                    Fluxo0 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 0);
                }

                ConfigUtilizadores UserDetails_Area = DBProcedimentosCCP.GetUserDetails(Fluxo0.User);
                string UserEmail_Area = "";
                string UserEmail_2Area = "";

                UserEmail_Area = EmailAutomation.IsValidEmail(UserDetails_Area.ProcedimentosEmailEnvioParaArea) ? UserDetails_Area.ProcedimentosEmailEnvioParaArea : string.Empty;
                UserEmail_2Area = EmailAutomation.IsValidEmail(UserDetails_Area.ProcedimentosEmailEnvioParaArea2) ? UserDetails_Area.ProcedimentosEmailEnvioParaArea2 : string.Empty;

                gEmailCC2 = UserEmail_Area;
                gEmailCC3 = UserEmail_2Area;
                gEmailBCC = UserEmail;

                gAssunto = data.No + " " + data.NomeProcesso + " - Autorização Adjudicação";
                
                EmailsProcedimentosCcp ProcedimentoEmail_2 = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = gAssunto,
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = gEmailPara,
                    TextoEmail = gComentarioEmail,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail_2))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail_2));

                SendEmailsProcedimentos Email_2 = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome == "" ? "Conselho de Administração" : UserDetails.Nome,
                    Subject = ProcedimentoEmail_2.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email_2.To.Add(gEmailPara);
                Email_2.CC.Add(gEmailCC1);
                Email_2.CC.Add(gEmailCC2);
                Email_2.CC.Add(gEmailCC3);
                Email_2.BCC.Add(UserEmail);

                Email_2.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail_2.TextoEmail, UserDetails.Nome);
                Email_2.IsBodyHtml = true;

                Email_2.EmailProcedimento = ProcedimentoEmail_2;

                Email_2.SendEmail();
                
                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult CBDevolverAutorizacaoCA_Simplificado([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCA = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCA);

                if (!IsElementCA)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if ((data.ResponsavelCA17 != "") && (data.ResponsavelCA17 != null))
                {
                    return Json(ReturnHandlers.ProcedimentoAlreadyTreated);
                }
                
                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure CAConfirmarAutorizacao.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.BoardOfManagementConfirmAuthorization_Simplificado(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 0);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV CAConfirmarAutorizacao.e

                FluxoTrabalhoListaControlo Fluxo0 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo0 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 0).LastOrDefault();
                }
                else
                {
                    Fluxo0 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 0);
                }

                ConfigUtilizadores UserDetails_Area = DBProcedimentosCCP.GetUserDetails(Fluxo0.User);
                string UserEmail_Area = "";
                string UserEmail_2Area = "";

                UserEmail_Area = EmailAutomation.IsValidEmail(UserDetails_Area.ProcedimentosEmailEnvioParaArea) ? UserDetails_Area.ProcedimentosEmailEnvioParaArea : string.Empty;
                UserEmail_2Area = EmailAutomation.IsValidEmail(UserDetails_Area.ProcedimentosEmailEnvioParaArea2) ? UserDetails_Area.ProcedimentosEmailEnvioParaArea2 : string.Empty;

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Devolução Autorização Adjudicação",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserDetails_Area.IdUtilizador,
                    TextoEmail = (data.ComentarioCA17 == "" || data.ComentarioCA17 == null) ? "Devolução do Procedimento" : data.ComentarioCA17,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome == "" ? "Conselho de Administração" : UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserDetails_Area.IdUtilizador);
                Email.To.Add(UserEmail_Area);
                Email.BCC.Add(UserEmail);
                Email.CC.Add(UserEmail_Area);
                Email.CC.Add(UserEmail_2Area);

                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;

                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        public JsonResult CBRejeitar2CA_Simplificado([FromBody] ProcedimentoCCPView data)
        {
            if (data != null)
            {
                bool IsElementCA = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoCA);

                if (!IsElementCA)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if ((data.ResponsavelCA17 != "") && (data.ResponsavelCA17 != null))
                {
                    return Json(ReturnHandlers.ProcedimentoAlreadyTreated);
                }

                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                // NAV Procedure CAConfirmarAutorizacao.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.BoardOfManagementConfirmAuthorization_Simplificado(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 2);
                if (DecisionTaken.eReasonCode != 0)
                {
                    return Json(DecisionTaken);
                }
                // NAV CAConfirmarAutorizacao.e

                FluxoTrabalhoListaControlo Fluxo7 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo7 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 7).LastOrDefault();
                }
                else
                {
                    Fluxo7 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 7);
                }

                ConfigUtilizadores UserDetails_TO = DBProcedimentosCCP.GetUserDetails(Fluxo7.User);
                string UserEmail_TO = "";
                string UserEmail_TO_Area = "";
                string UserEmail_TO_2Area = "";

                if (EmailAutomation.IsValidEmail(UserDetails_TO.IdUtilizador))
                {
                    UserEmail_TO = UserDetails_TO.IdUtilizador;
                    UserEmail_TO_Area = UserDetails_TO.ProcedimentosEmailEnvioParaArea == null ? UserEmail_TO_Area : UserDetails_TO.ProcedimentosEmailEnvioParaArea;
                    UserEmail_TO_2Area = UserDetails_TO.ProcedimentosEmailEnvioParaArea2 == null ? UserEmail_TO_2Area : UserDetails_TO.ProcedimentosEmailEnvioParaArea2;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                FluxoTrabalhoListaControlo Fluxo4 = new FluxoTrabalhoListaControlo();

                if (data.FluxoTrabalhoListaControlo != null)
                {
                    Fluxo4 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 4).LastOrDefault();
                }
                else
                {
                    Fluxo4 = DBProcedimentosCCP.GetChecklistControloProcedimento(data.No, 4);
                }

                ConfigUtilizadores UserDetails_CC = DBProcedimentosCCP.GetUserDetails(Fluxo4.User);
                string UserEmail_CC1 = "";

                if (EmailAutomation.IsValidEmail(UserDetails_CC.IdUtilizador))
                {
                    UserEmail_CC1 = UserDetails_CC.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddress);
                };

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Rejeição Adjudicação",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = UserEmail_TO,
                    TextoEmail = data.ComentarioCA17 == "" ? "Não Autorizado" : data.ComentarioCA17,
                    DataHoraEmail = DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraCriação = DateTime.Now
                };

                if (!DBProcedimentosCCP.__CreateEmailProcedimento(ProcedimentoEmail))
                {
                    return Json(ReturnHandlers.UnableToCreateEmailProcedimento);
                }

                data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                {
                    DisplayName = UserDetails.Nome == "" ? "Conselho de Administração" : UserDetails.Nome,
                    Subject = ProcedimentoEmail.Assunto,
                    From = DBProcedimentosCCP._EmailSender
                };

                Email.To.Add(UserEmail_TO);
                Email.BCC.Add(UserEmail_CC1);
                Email.BCC.Add(UserEmail_TO_Area);
                Email.BCC.Add(UserEmail_TO_2Area);
                Email.BCC.Add(UserEmail);
                Email.Body = CCPFunctions.MakeEmailBodyContent(ProcedimentoEmail.TextoEmail, UserDetails.Nome);
                Email.IsBodyHtml = true;
                Email.EmailProcedimento = ProcedimentoEmail;

                Email.SendEmail();

                return Json(ReturnHandlers.Success);
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }
        }

        #endregion

        #region the following methods map the NAV 2009 "Funções" MenuButton on the PA form
        public JsonResult CloseProcedimentoByRequest([FromBody] ProcedimentoCCPView data)
        {
            ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(User.Identity.Name);
            if (data != null)
            {
                return Json(DBProcedimentosCCP.CloseProcedimento(data, UserDetails));
            }
            else
            {
                return Json(ReturnHandlers.NoData);
            }

        }
        public JsonResult GetProcedimentoReference([FromBody] JObject requestParams)
        {
            int TipoProc = requestParams["tipoProc"].ToString() != string.Empty ? int.Parse(requestParams["tipoProc"].ToString()): 0;
            int Tipo = requestParams["tipo"].ToString() != string.Empty ? int.Parse(requestParams["tipo"].ToString()) : 0;

            if(TipoProc == 0 || Tipo == 0)
            {
                return Json(ReturnHandlers.Error);
            }

            ReferenceProcedimento rproc = new ReferenceProcedimento(TipoProc, Tipo, DateTime.Now.Year);
            if (rproc != null)
                return Json(rproc);

            return Json(ReturnHandlers.Error);
        }
        
        #endregion


        //1
        [HttpPost]
        public async Task<JsonResult> ExportToExcel_PedidosAquisicao([FromBody] List<ProcedimentoCCPView> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\Upload\\temp";
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
                ISheet excelSheet = workbook.CreateSheet("Pedidos de Aquisição");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["no"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº");
                    Col = Col + 1;
                }
                if (dp["estado"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Estado");
                    Col = Col + 1;
                }
                if (dp["nomeProcesso"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nome Processo");
                    Col = Col + 1;
                }
                if (dp["gestorProcesso"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Gestor Processo");
                    Col = Col + 1;
                }
                if (dp["valorPrecoBase"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Valor Preço Base");
                    Col = Col + 1;
                }
                if (dp["codigoRegiao"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Região");
                    Col = Col + 1;
                }
                if (dp["codigoAreaFuncional"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Codigo Área Funcional");
                    Col = Col + 1;
                }
                if (dp["codigoCentroResponsabilidade"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Centro Responsabilidade");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ProcedimentoCCPView item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["no"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.No);
                            Col = Col + 1;
                        }
                        if (dp["estado"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Estado.ToString());
                            Col = Col + 1;
                        }
                        if (dp["nomeProcesso"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.NomeProcesso);
                            Col = Col + 1;
                        }
                        if (dp["gestorProcesso"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.GestorProcesso);
                            Col = Col + 1;
                        }
                        if (dp["valorPrecoBase"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ValorPrecoBase.ToString());
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
        public IActionResult ExportToExcelDownload_PedidosAquisicao(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Pedidos de Aquisição.xlsx");
        }

        #region ANEXOS
        
        [HttpPost]
        [Route("Procedimentos/FileUpload")]
        [Route("Procedimentos/FileUpload/{id}/{linha}")]
        public JsonResult FileUpload(string id, int linha)
        {
            try
            {
                var files = Request.Form.Files;
                string full_filename;
                foreach (var file in files)
                {
                    try
                    {
                        string filename = Path.GetFileName(file.FileName);
                                                    
                        full_filename = filename;
                        full_filename = DateTime.Now.ToString("yyyyMMddHmmssf") + "_" + full_filename;

                        // class propertiy to define Procedimentos path 
                        __UploadPath += id;

                        if (!System.IO.Directory.Exists(__UploadPath))
                        {
                            System.IO.Directory.CreateDirectory(__UploadPath);
                        }

                        var path = Path.Combine(__UploadPath, full_filename);

                        if (System.IO.File.Exists(path))
                        {    
                            return Json("Ficheiro já existe!");
                        }

                        using (FileStream dd = new FileStream(path, FileMode.CreateNew))
                        {
                            
                            file.CopyTo(dd);
                            dd.Dispose();

                            Anexos newfile = new Anexos();
                            newfile.NºOrigem = id;
                            newfile.UrlAnexo = full_filename;

                            //TipoOrigem: 1-PréRequisição; 2-Requisição; 3-Contratos; 4-Procedimentos;5-ConsultaMercado 
                            newfile.TipoOrigem = TipoOrigemAnexos.Procedimentos;

                            newfile.DataHoraCriação = DateTime.Now;
                            newfile.UtilizadorCriação = User.Identity.Name;

                            DBAttachments.Create(newfile);
                            if (newfile.NºLinha == 0)
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json("0");
        }

        [HttpPost]
        public JsonResult LoadAttachments([FromBody] JObject requestParams)
        {
            string id = requestParams["id"].ToString();

            List<Anexos> list = DBAttachments.GetById(TipoOrigemAnexos.Procedimentos, id);
            List<AttachmentsViewModel> attach = new List<AttachmentsViewModel>();
            list.ForEach(x => attach.Add(DBAttachments.ParseToViewModel(x)));
            return Json(attach);
        }

        [HttpPost]
        public JsonResult DeleteAttachments([FromBody] AttachmentsViewModel requestParams)
        {
            try
            {
                System.IO.File.Delete(__UploadPath + requestParams.DocNumber + '/' + requestParams.Url);
                DBAttachments.Delete(DBAttachments.ParseToDB(requestParams));

                return Json(ReturnHandlers.Success);

            }
            catch (Exception ex)
            {
                return Json(ReturnHandlers.Error);
            }
            
        }
        #endregion

    }
    
}
