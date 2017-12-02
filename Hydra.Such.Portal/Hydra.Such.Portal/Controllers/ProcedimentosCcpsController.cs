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
                AreaID = Convert.ToInt32(AreaFuncionalCode.Substring(0, 1));
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
                        if (Convert.ToInt32(AreaFuncionalCode.Substring(0, 2)) == 72)
                        {
                            AreaID = 72;
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

    [Authorize]
    public class ProcedimentosCcpsController : Controller
    {
        #region Views
        public IActionResult Index()
        {
            return View();
        }

        // zpgm.< view that will return a Pedidos Simplificados list
        public IActionResult PedidoSimplificado()
        {
            return View();
        }
        // zpgm.>

        // zpgm.< view that will return a Pedidos de Aquisição list
        public IActionResult PedidoAquisicao()
        {
            return View();
        }
        // zpgm.>
        public IActionResult Detalhes(string id)
        {
            ViewBag.No = id == null ? "" : id;
            return View();
        }

        public IActionResult DetalhePedidoAquisicao(string id)
        {
            ViewBag.No = id == null ? "" : id;
            return View();
        }

        public IActionResult DetalhePedidoSimplificado(string id)
        {
            ViewBag.No = id == null ? "" : id;
            ViewBag.TipoProcedimento = 2;
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
        public JsonResult GetProcedimentosByProcedimentoType([FromBody] int id)
        {
            List<ProcedimentoCCPView> result = DBProcedimentosCCP.GetAllProcedimentosViewByProcedimentoTypeToList(id);

            return Json(result);
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
                        data.eReasonCode = 3;
                        data.eMessage = "Ocorreu um erro ao actualizar o procedimento";
                    }
                }
            }
            catch (Exception e)
            {
                data.eReasonCode = 4;
                data.eMessage = "Ocorreu um erro ao actualizar o procedimento";
            }

            return Json(data);
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
            return Json(false);
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


        #region Get the FluxoTrabalhoListaControlo propertie according with the view where it should be displayed
        // 0. Used in the "Unidade Produtiva - Avaliação Técnica" paper-tab
        [HttpPost]
        public JsonResult GetChecklistArea([FromBody] ProcedimentoCCPView data)
        {
            if (data.FluxoTrabalhoListaControlo != null && data.FluxoTrabalhoListaControlo.Count > 0)
            {
                if (data.Estado != 0)
                {
                    FluxoTrabalhoListaControlo Fluxo = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 0).LastOrDefault();

                    if (Fluxo != null)
                    {
                        ElementosChecklist Checklist = new ElementosChecklist()
                        {
                            ProcedimentoID = Fluxo.No,
                            Estado = Fluxo.Estado,
                            DataChecklist = Fluxo.Data,
                            HoraChecklist = Fluxo.Hora,
                            ChecklistArea = new ElementosChecklistArea(Fluxo)
                        };

                        return Json(Checklist);
                    }
                    else
                        return Json(null);
                }
                else
                {
                    return Json(null);
                }


            }
            else
            {
                return Json(null);
            }
        }

        // 1. Used in the "Imobilizado" paper-tab "Contabilidade" area
        [HttpPost]
        public JsonResult GetChecklistImobilizadoContabilidade([FromBody] ProcedimentoCCPView data)
        {
            if (data.FluxoTrabalhoListaControlo != null && data.FluxoTrabalhoListaControlo.Count > 0)
            {
                if (data.Estado != 1)
                {
                    FluxoTrabalhoListaControlo Fluxo = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 1).LastOrDefault();
                    if (Fluxo != null)
                    {
                        ElementosChecklist Checklist = new ElementosChecklist()
                        {
                            ProcedimentoID = Fluxo.No,
                            Estado = Fluxo.Estado,
                            DataChecklist = Fluxo.Data,
                            HoraChecklist = Fluxo.Hora,
                            ChecklistImobilizadoContabilidade = new ElementosChecklistImobilizadoContabilidade(Fluxo)
                        };

                        return Json(Checklist);
                    }
                    else
                    {
                        return Json(null);
                    }
                }
                else
                {
                    return Json(null);
                }

            }
            else
            {
                return Json(null);
            }
        }

        // 2. Used in the "Imobilizado" paper-tab "Área" area
        [HttpPost]
        public JsonResult GetChecklistImobilizadoArea([FromBody] ProcedimentoCCPView data)
        {
            if (data.FluxoTrabalhoListaControlo != null && data.FluxoTrabalhoListaControlo.Count > 0)
            {
                if (data.Estado != 2)
                {
                    FluxoTrabalhoListaControlo Fluxo = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 2).LastOrDefault();
                    if (Fluxo != null)
                    {
                        ElementosChecklist Checklist = new ElementosChecklist()
                        {
                            ProcedimentoID = Fluxo.No,
                            Estado = Fluxo.Estado,
                            DataChecklist = Fluxo.Data,
                            HoraChecklist = Fluxo.Hora,
                            ChecklistImobilizadoArea = new ElementosChecklistImobilizadoArea(Fluxo)
                        };

                        return Json(Checklist);
                    }
                    else
                    {
                        return Json(null);
                    }
                }
                else
                {
                    return Json(null);
                }

            }
            else
            {
                return Json(null);
            }
        }

        // 3. Used in the "CA" paper-tab "Autorizar Imobilizado" area 
        [HttpPost]
        public JsonResult GetChecklistImobilizadoCA([FromBody] ProcedimentoCCPView data)
        {
            if (data.FluxoTrabalhoListaControlo != null && data.FluxoTrabalhoListaControlo.Count > 0)
            {
                if (data.Estado != 3)
                {
                    FluxoTrabalhoListaControlo Fluxo = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 3).LastOrDefault();
                    if (Fluxo != null)
                    {
                        ElementosChecklist Checklist = new ElementosChecklist()
                        {
                            ProcedimentoID = Fluxo.No,
                            Estado = Fluxo.Estado,
                            DataChecklist = Fluxo.Data,
                            HoraChecklist = Fluxo.Hora,
                            ChecklistImobilizadoCA = new ElementosChecklistImobilizadoCA(Fluxo)
                        };

                        return Json(Checklist);
                    }
                    else
                    {
                        return Json(null);
                    }
                }
                else
                {
                    return Json(null);
                }

            }
            else
            {
                return Json(null);
            }
        }

        // 4. Used in the "Fundamentos Decisão" paper-tab in the "A Preencher pelas compras" area
        [HttpPost]
        public JsonResult GetChecklistFundamentoCompras([FromBody] ProcedimentoCCPView data)
        {
            if (data.FluxoTrabalhoListaControlo != null && data.FluxoTrabalhoListaControlo.Count > 0)
            {
                if (data.Estado != 4)
                {
                    FluxoTrabalhoListaControlo Fluxo = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 4).LastOrDefault();
                    if (Fluxo != null)
                    {
                        ElementosChecklist Checklist = new ElementosChecklist()
                        {
                            ProcedimentoID = Fluxo.No,
                            Estado = Fluxo.Estado,
                            DataChecklist = Fluxo.Data,
                            HoraChecklist = Fluxo.Hora,
                            ChecklistFundamentoCompras = new ElementosChecklistFundamentoCompras(Fluxo)
                        };

                        return Json(Checklist);
                    }
                    else
                    {
                        return Json(null);
                    }
                }
                else
                {
                    return Json(null);
                }

            }
            else
            {
                return Json(null);
            }
        }

        // 5. Used in the "Fundamentos Decisão" paper-tab in the "A Preencher pelos Serviços Financeiros" area
        [HttpPost]
        public JsonResult GetChecklistFundamentoFinanceiros([FromBody] ProcedimentoCCPView data)
        {
            if (data.FluxoTrabalhoListaControlo != null && data.FluxoTrabalhoListaControlo.Count > 0)
            {
                if (data.Estado != 5)
                {
                    FluxoTrabalhoListaControlo Fluxo = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 5).LastOrDefault();
                    if (Fluxo != null)
                    {
                        ElementosChecklist Checklist = new ElementosChecklist()
                        {
                            ProcedimentoID = Fluxo.No,
                            Estado = Fluxo.Estado,
                            DataChecklist = Fluxo.Data,
                            HoraChecklist = Fluxo.Hora,
                            ChecklistFundamentoFinanceiros = new ElementosChecklistFundamentoFinanceiros(Fluxo)
                        };

                        return Json(Checklist);
                    }
                    else
                    {
                        return Json(null);
                    }
                }
                else
                {
                    return Json(null);
                }

            }
            else
            {
                return Json(null);
            }
        }

        // 6. Used in the "Juridicos" paper-tab returns a List of ElementosChecklist according to ProcedimentosCcp.Estado (!= 6 or != 14)
        [HttpPost]
        public JsonResult GetChecklistJuridicoAvaliacaoPecasOrContrato([FromBody] ProcedimentoCCPView data)
        {
            if (data.FluxoTrabalhoListaControlo != null && data.FluxoTrabalhoListaControlo.Count > 0)
            {
                List<ElementosChecklist> ElementosChecklistJuridico = new List<ElementosChecklist>();
                ElementosChecklistJuridico = null;

                if(data.Estado != 6 || data.Estado != 14)
                {
                    if(data.Estado != 6)
                    {
                        FluxoTrabalhoListaControlo Fluxo6 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 6).LastOrDefault();
                        if (Fluxo6 != null)
                        {
                            ElementosChecklistJuridico.Add(new ElementosChecklist()
                            {
                                ProcedimentoID = Fluxo6.No,
                                Estado = Fluxo6.Estado,
                                DataChecklist = Fluxo6.Data,
                                HoraChecklist = Fluxo6.Hora,
                                ChecklistJuridico = new ElementosChecklistJuridico(Fluxo6)
                            });
                        }
                    }
                       
                    if(data.Estado != 14)
                    {
                        FluxoTrabalhoListaControlo Fluxo14 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 14).LastOrDefault();
                        if (Fluxo14 != null)
                        {
                            ElementosChecklistJuridico.Add(new ElementosChecklist()
                            {
                                ProcedimentoID = Fluxo14.No,
                                Estado = Fluxo14.Estado,
                                DataChecklist = Fluxo14.Data,
                                HoraChecklist = Fluxo14.Hora,
                                ChecklistJuridico = new ElementosChecklistJuridico(Fluxo14)
                            });
                        }
                    }           
                }
               
                return Json(ElementosChecklistJuridico);
            }
            else
            {
                return Json(null);
            }
        }

        // 7. Used in the "Fundamentos Decisão" paper-tab in the "A preencher pela Área(...)" area 
        [HttpPost]
        public JsonResult GetChecklistFundamentacaoArea([FromBody] ProcedimentoCCPView data)
        {
            if (data.FluxoTrabalhoListaControlo != null && data.FluxoTrabalhoListaControlo.Count > 0)
            {
                if (data.Estado != 7)
                {
                    FluxoTrabalhoListaControlo Fluxo = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 7).LastOrDefault();

                    if (Fluxo != null)
                    {
                        ElementosChecklist Checklist = new ElementosChecklist()
                        {
                            ProcedimentoID = Fluxo.No,
                            Estado = Fluxo.Estado,
                            DataChecklist = Fluxo.Data,
                            HoraChecklist = Fluxo.Hora,
                            ChecklistArea = new ElementosChecklistArea(Fluxo)
                        };

                        return Json(Checklist);
                    }
                    else
                        return Json(null);
                }
                else
                {
                    return Json(null);
                }


            }
            else
            {
                return Json(null);
            }
        }

        // 8. Used in the "CA" paper-tab returns a List of ElementosChecklist according to ProcedimentosCcp.Estado (!= 8 or != 17)
        [HttpPost]
        public JsonResult GetChecklistAberturaOrAutorizacao([FromBody] ProcedimentoCCPView data)
        {
            if (data.FluxoTrabalhoListaControlo != null && data.FluxoTrabalhoListaControlo.Count > 0)
            {
                List<ElementosChecklist> ElementosCA = new List<ElementosChecklist>();
                ElementosCA = null;

                if (data.Estado != 8 || data.Estado != 17)
                {
                    if (data.Estado != 8)
                    {
                        FluxoTrabalhoListaControlo Fluxo8 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 8).LastOrDefault();
                        if (Fluxo8 != null)
                        {
                            ElementosCA.Add(new ElementosChecklist()
                            {
                                ProcedimentoID = Fluxo8.No,
                                Estado = Fluxo8.Estado,
                                DataChecklist = Fluxo8.Data,
                                HoraChecklist = Fluxo8.Hora,
                                ChecklistAberturaCA = new ElementosChecklistAberturaCA(Fluxo8)
                            });
                        }
                    }

                    if (data.Estado != 17)
                    {
                        FluxoTrabalhoListaControlo Fluxo17 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 17).LastOrDefault();
                        if (Fluxo17 != null)
                        {
                            ElementosCA.Add(new ElementosChecklist()
                            {
                                ProcedimentoID = Fluxo17.No,
                                Estado = Fluxo17.Estado,
                                DataChecklist = Fluxo17.Data,
                                HoraChecklist = Fluxo17.Hora,
                                ChecklistAberturaCA = new ElementosChecklistAberturaCA(Fluxo17)
                            });
                        }
                    }
                }

                return Json(ElementosCA);
            }
            else
            {
                return Json(null);
            }
        }

        // 9. Used in the "Valores Adjudicação" paper-tab returns a List of ElementosChecklist according to ProcedimentosCcp.Estado (!= 15 or != 16)
        [HttpPost]
        public JsonResult GetChecklistAdjudicacao([FromBody] ProcedimentoCCPView data)
        {
            if (data.FluxoTrabalhoListaControlo != null && data.FluxoTrabalhoListaControlo.Count > 0)
            {
                List<ElementosChecklist> ElementosAdjudicacao = new List<ElementosChecklist>();
                ElementosAdjudicacao = null;

                if (data.Estado != 15 || data.Estado != 16)
                {
                    if (data.Estado != 15)
                    {
                        FluxoTrabalhoListaControlo Fluxo15 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 15).LastOrDefault();
                        if (Fluxo15 != null)
                        {
                            ElementosAdjudicacao.Add(new ElementosChecklist()
                            {
                                ProcedimentoID = Fluxo15.No,
                                Estado = Fluxo15.Estado,
                                DataChecklist = Fluxo15.Data,
                                HoraChecklist = Fluxo15.Hora,
                                ChecklistAdjudicacao = new ElementosChecklistAdjudicacaoCompras(Fluxo15)
                            });
                        }
                    }

                    if (data.Estado != 16)
                    {
                        FluxoTrabalhoListaControlo Fluxo16 = data.FluxoTrabalhoListaControlo.Where(f => f.Estado == 16).LastOrDefault();
                        if (Fluxo16 != null)
                        {
                            ElementosAdjudicacao.Add(new ElementosChecklist()
                            {
                                ProcedimentoID = Fluxo16.No,
                                Estado = Fluxo16.Estado,
                                DataChecklist = Fluxo16.Data,
                                HoraChecklist = Fluxo16.Hora,
                                ChecklistAdjudicacao = new ElementosChecklistAdjudicacaoCompras(Fluxo16)
                            });
                        }
                    }
                }

                return Json(ElementosAdjudicacao);
            }
            else
            {
                return Json(null);
            }
        }
        #endregion

        [HttpPost]
        public JsonResult GetUserFeatures()
        {
            List<AcessosUtilizador> UserAccesses = DBProcedimentosCCP.GetUserAccesses(User.Identity.Name);
            if(UserAccesses != null)
            {
                List<string> UAccess = null;

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

                
                return Json(UAccess.Distinct().ToList());
            }

            return Json(null);
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
                    Comentario = data.ElementosChecklist.ChecklistArea.ComentarioArea,
                    EstadoSeguinte = data.Imobilizado.Value ? 1 : 4,
                };

                data.ElementosChecklist.ChecklistArea.ResponsavelArea = Fluxo.User;
                data.ElementosChecklist.ChecklistArea.NomeResponsavelArea = Fluxo.NomeUser;
                data.ElementosChecklist.ChecklistArea.DataResponsavel = Fluxo.Data;

                if (IsElementPreArea || IsElementPreArea0)
                    Fluxo.EstadoSeguinte = 0;

                if(DBProcedimentosCCP.__CreateFluxoTrabalho(Fluxo) == null)
                {
                    return Json(ReturnHandlers.UnableToCreateFluxo);
                }

                data.FluxoTrabalhoListaControlo.Add(Fluxo);

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
                    return Json(ReturnHandlers.InvalidEmailAddres);
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
                            return Json(ReturnHandlers.InvalidEmailAddres);
                        }

                        EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                        {
                            NºProcedimento = data.No,
                            EmailDestinatário = EmailList.EmailContabilidade,
                            Assunto = data.No + " - Aquisção de Imobilizado",
                            TextoEmail = data.ElementosChecklist.ChecklistArea.ComentarioArea,
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
                            From = "CCP_NAV@such.pt"
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
                            return Json(ReturnHandlers.InvalidEmailAddres);
                        }

                        EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                        {
                            NºProcedimento = data.No,
                            EmailDestinatário = DestinationEmails.ToAddress,
                            Assunto = data.No + " - Novo pedido de aquisição",
                            TextoEmail = data.ElementosChecklist.ChecklistArea.ComentarioArea,
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
                            From = "CCP_NAV@such.pt"
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
                        return Json(ReturnHandlers.InvalidEmailAddres);
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

                    data.EmailsProcedimentosCcp.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ProcedimentoEmail));

                    SendEmailsProcedimentos Email = new SendEmailsProcedimentos
                    {
                        DisplayName = UserDetails.Nome,
                        Subject = ProcedimentoEmail.Assunto,
                        From = "CCP_NAV@such.pt"
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

        #region The following methods map the MenuButton actions, named "Acções" in the "Imobilizado" tab on the NAV form
        #region "Acções" MenuItem in the "Contabilidade" section
        [HttpPost]
        public JsonResult ConfirmProcedimento([FromBody] ProcedimentoCCPView data)
        {
            if(data != null)
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
                    return Json(ReturnHandlers.InvalidEmailAddres);
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
                    From = "CCP_NAV@such.pt"
                };

                if (data.ElementosChecklist.ChecklistImobilizadoContabilidade.ImobilizadoSimNao)
                {
                    if (EmailAutomation.IsValidEmail(DestinationEmails.ToAddress))
                    {
                        Email.To.Add(DestinationEmails.ToAddress);
                        ProcedimentoEmail.EmailDestinatário = DestinationEmails.ToAddress;
                    }
                    else
                    {
                        return Json(ReturnHandlers.InvalidEmailAddres);
                    }

                    if (EmailAutomation.IsValidEmail(DestinationEmails.CCAddress))
                        Email.CC.Add(DestinationEmails.CCAddress);
                }
                else
                {
                    FluxoTrabalhoListaControlo Fluxo = data.FluxoTrabalhoListaControlo.Where(e => e.Estado == 0).LastOrDefault();
                    if(Fluxo != null)
                    {
                        ConfigUtilizadores FluxoUser = DBProcedimentosCCP.GetUserDetails(Fluxo.User);
                        if(FluxoUser != null && EmailAutomation.IsValidEmail(FluxoUser.IdUtilizador))
                        {
                            Email.To.Add(FluxoUser.IdUtilizador);
                            ProcedimentoEmail.EmailDestinatário = FluxoUser.IdUtilizador;

                            if (EmailAutomation.IsValidEmail(FluxoUser.ProcedimentosEmailEnvioParaArea))
                                Email.CC.Add(FluxoUser.ProcedimentosEmailEnvioParaArea);
                        }
                        else
                        {
                            return Json(ReturnHandlers.InvalidEmailAddres);
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

                ProcedimentoEmail.TextoEmail = data.ElementosChecklist.ChecklistImobilizadoContabilidade.ComentarioImobContabilidade;
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
            if(data != null)
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

                if(Fluxo0 != null)
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
                    return Json(ReturnHandlers.InvalidEmailAddres);
                };

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    EmailDestinatário  = UserEmail,
                    Assunto = data.No + " - Aquisição de Imobilizado (Devolução)",
                    TextoEmail = data.ElementosChecklist.ChecklistImobilizadoContabilidade.ComentarioImobContabilidade,
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
                    From = "CCP_NAV@such.pt"
                };

                Email.To.Add(UserEmail);
                if(EmailAutomation.IsValidEmail(UserDetails.ProcedimentosEmailEnvioParaArea))
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
        
        #region "Acções" MenuItem in the "Area" section
        public JsonResult GetPermission([FromBody] ProcedimentoCCPView data)
        {
            if(data != null)
            {
                bool IsElementArea = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._ElementoArea);
                bool IsGestorProcesso = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(User.Identity.Name, DBProcedimentosCCP._GestorProcesso);

                if(!IsElementArea && !IsGestorProcesso)
                {
                    return Json(ReturnHandlers.UserNotAllowed);
                }

                if(data.Estado != 2)
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
                    return Json(ReturnHandlers.InvalidEmailAddres);
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
                    return Json(ReturnHandlers.InvalidEmailAddres);
                };

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " - Autorização de aquisição de Imobilizado",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = EmailList.EmailCa,
                    TextoEmail = data.ElementosChecklist.ChecklistImobilizadoArea.ComentarioImobArea,
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
                    From = "CCP_NAV@such.pt"
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
            if(data != null)
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

        #region The following methods map the MenuButtons, named "Acções" in the "Fundamentos Decisão" tab on the NAV form
        #region MenuButton "Acções" in the "A preencher pelas compras" section
        [HttpPost]
        public JsonResult SubmitToAccounting([FromBody] ProcedimentoCCPView data)
        {
            if(data != null)
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
                    return Json(ReturnHandlers.InvalidEmailAddres);
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
                    return Json(ReturnHandlers.InvalidEmailAddres);
                };

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Parecer Financeiro",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = EmailList.EmailFinanceiros,
                    TextoEmail = data.ElementosChecklist.ChecklistFundamentoCompras.ComentarioFundamentoCompras,
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
                    From = "CCP_NAV@such.pt"
                };

                Email.To.Add(EmailList.EmailFinanceiros);

                if (EmailAutomation.IsValidEmail(EmailList.Email2Financeiros))
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
                    return Json(ReturnHandlers.InvalidEmailAddres);
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
                    return Json(ReturnHandlers.InvalidEmailAddres);
                }

                if (!EmailAutomation.IsValidEmail(EmailList.EmailJurididos))
                {
                    return Json(ReturnHandlers.InvalidEmailAddres);
                };

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Parecer Juridico",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = EmailList.EmailFinanceiros,
                    TextoEmail = data.ElementosChecklist.ChecklistFundamentoCompras.ComentarioFundamentoCompras,
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
                    From = "CCP_NAV@such.pt"
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

                if (data.Estado < 4 || data.Estado > 6)
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

                
                ConfigUtilizadores UserDetails = DBProcedimentosCCP.GetUserDetails(Fluxo0.User);
                string UserEmail = "";

                if (EmailAutomation.IsValidEmail(UserDetails.IdUtilizador))
                {
                    UserEmail = UserDetails.IdUtilizador;
                }
                else
                {
                    return Json(ReturnHandlers.InvalidEmailAddres);
                };

                // NAV Procedure FDComprasConfirmar.b
                ErrorHandler DecisionTaken = DBProcedimentosCCP.DecisionGroundsToBuy(data, DBProcedimentosCCP.GetUserDetails(User.Identity.Name), 3);
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

                if (!EmailAutomation.IsValidEmail(EmailList.EmailJurididos))
                {
                    return Json(ReturnHandlers.InvalidEmailAddres);
                };

                EmailsProcedimentosCcp ProcedimentoEmail = new EmailsProcedimentosCcp
                {
                    NºProcedimento = data.No,
                    Assunto = data.No + " " + data.NomeProcesso + " - Parecer Juridico",
                    UtilizadorEmail = UserEmail,
                    EmailDestinatário = EmailList.EmailFinanceiros,
                    TextoEmail = data.ElementosChecklist.ChecklistFundamentoCompras.ComentarioFundamentoCompras,
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
                    From = "CCP_NAV@such.pt"
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
        public JsonResult ReturnToAreaToJustifyProcess([FromBody] ProcedimentoCCPView data)
        {
            return Json(null);
        }

        [HttpPost]
        public JsonResult SubmitToBoardForOpening([FromBody] ProcedimentoCCPView data)
        {
            return Json(null);
        }

        [HttpPost]
        public JsonResult SubmitToBoardForGranting([FromBody] ProcedimentoCCPView data)
        {
            return Json(null);
        } 
        #endregion
        #endregion
    }

    
}
