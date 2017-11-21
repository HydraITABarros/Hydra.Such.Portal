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
                            CkecklistImobilizadoArea = new ElementosChecklistImobilizadoArea(Fluxo)
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

        // 6. Used in the "Juridicos" paper-tab returns a List of ElementosChecklist according to ProcedimentosCcp Estado (!= 6 or != 14)
        [HttpPost]
        public JsonResult GetChecklistJuridicos([FromBody] ProcedimentoCCPView data)
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

        // 7. Used in the "Fundamentos DEcisão" paper-tab in the "A preencher pela Área(...)" area 
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
        #endregion
    }
}
