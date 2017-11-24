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

        /*
         *      In the following method the ErrorHandler will return:
         *          0 -> SUCCESS
         *          1 -> Procedimento already submitted
         *          2 -> Unable to create Fluxo Lista Trabalho
         *          3 -> Unable to update Procedimento
         *          
         */
        [HttpPost]
        public JsonResult SubmitProcedimento([FromBody] ProcedimentoCCPView data)
        {
            if(data != null)
            {
                // 1. Get the latest version in the Database
                ProcedimentosCcp Procedimento = DBProcedimentosCCP.GetProcedimentoById(data.No);
                string UserID = User.Identity.Name;
                bool UserElementPreArea0 = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(UserID, DBProcedimentosCCP._ElementoPreArea0);
                bool UserElementPreArea = DBProcedimentosCCP.CheckUserRoleRelatedToCCP(UserID, DBProcedimentosCCP._ElementoPreArea);

                int errorCount = 1;

                string UserEmail = DBProcedimentosCCP.GetUserEmail(UserID);

                // 2.a Check if Procedimento has been already submitted
                if (UserElementPreArea0 && Procedimento.PréÁrea.HasValue && Procedimento.PréÁrea.Value)
                {
                    ErrorHandler ProcedimentoAlreadySubmitted = new ErrorHandler()
                    {
                        eReasonCode = errorCount,
                        eMessage = "Procedimento já submetido!"
                    };

                    errorCount += 1;
                    return Json(ProcedimentoAlreadySubmitted);
                }

                // 2.b Check if Procedimento has been already submitted
                if(UserElementPreArea && Procedimento.SubmeterPréÁrea.HasValue && Procedimento.SubmeterPréÁrea.Value)
                {
                    ErrorHandler ProcedimentoAlreadySubmitted = new ErrorHandler()
                    {
                        eReasonCode = errorCount,
                        eMessage = "Procedimento já submetido!"
                    };

                    errorCount += 1;
                    return Json(ProcedimentoAlreadySubmitted);
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
                    User = User.Identity.Name,
                    NomeUser = DBProcedimentosCCP.GetUserName(UserID),
                    Comentario = data.ElementosChecklist.ChecklistArea.ComentarioArea,
                    EstadoSeguinte = data.Imobilizado.Value ? 1 : 4,
                };

                data.ElementosChecklist.ChecklistArea.ResponsavelArea = Fluxo.User;
                data.ElementosChecklist.ChecklistArea.NomeResponsavelArea = Fluxo.NomeUser;
                data.ElementosChecklist.ChecklistArea.DataResponsavel = Fluxo.Data;

                if (UserElementPreArea || UserElementPreArea0)
                    Fluxo.EstadoSeguinte = 0;

                if(DBProcedimentosCCP.__CreateFluxoTrabalho(Fluxo) == null)
                {
                    ErrorHandler UnableToCreateFluxo = new ErrorHandler()
                    {
                        eReasonCode = errorCount,
                        eMessage = "Não foi possível criar o Fluxo Trabalho Lista Controlo!"
                    };

                    errorCount += 1;
                    return Json(UnableToCreateFluxo);
                }

                // 4. Send emails and updates the data object
                if(!(UserElementPreArea || UserElementPreArea0))
                {
                    data.Estado = data.Imobilizado.Value ? 1 : 4;
                    data.DataHoraEstado = Fluxo.Data + Fluxo.Hora;
                    data.UtilizadorEstado = UserID;
                    data.DataHoraEstado = DateTime.Now;
                    data.UtilizadorModificacao = UserID;

                    if (DBProcedimentosCCP.__UpdateProcedimento(data) == null)
                    {
                        ErrorHandler UnableUpdatingProcedimento = new ErrorHandler()
                        {
                            eReasonCode = errorCount,
                            eMessage = "Não foi possível actualizar o Procedimento!"
                        };

                        errorCount += 1;
                        return Json(UnableUpdatingProcedimento);
                    }

                    if (string.IsNullOrEmpty(UserEmail))
                    {
                        ErrorHandler InvalidUserEmailAddress = new ErrorHandler()
                        {
                            eReasonCode = 4,
                            eMessage = "Utilizador sem endereço de email válido"
                        };

                        return Json(InvalidUserEmailAddress);
                    }
                    ConfiguracaoCcp EmailList = DBProcedimentosCCP.GetConfiguracaoCCP();
                    if (EmailList == null)
                    {
                        ErrorHandler DestinationEmailsAreEmpty = new ErrorHandler()
                        {
                            eReasonCode = errorCount,
                            eMessage = "Falta configuração dos destinatários de emails!"
                        };

                        errorCount += 1;
                        return Json(DestinationEmailsAreEmpty);
                    }
                    // Prepare emails
                    if (data.Imobilizado.Value)
                    {
                        if (!EmailAutomation.IsValidEmail(EmailList.EmailContabilidade))
                        {
                            ErrorHandler InvalidDestinationAddress = new ErrorHandler()
                            {
                                eReasonCode = errorCount,
                                eMessage = "Verifique as configurações: Endereço de Email Contabilidade não preenchido"
                            };

                            errorCount += 1;
                            return Json(InvalidDestinationAddress);
                        }

                        EmailsProcedimentosCcp EmailProcedimento = new EmailsProcedimentosCcp
                        {
                            NºProcedimento = data.No,
                            EmailDestinatário = EmailList.EmailContabilidade,
                            Assunto = data.No + " - Aquisção de Imobilizado",
                            TextoEmail = data.ElementosChecklist.ChecklistArea.ComentarioArea,
                            UtilizadorEmail = UserEmail,
                            DataHoraEmail = DateTime.Now,
                            UtilizadorCriação = UserID
                        };

                        if (!DBProcedimentosCCP.__CreateEmailProcedimento(EmailProcedimento))
                        {
                            ErrorHandler UnableToCreateEmailProcedimento = new ErrorHandler()
                            {
                                eReasonCode = errorCount,
                                eMessage = "Não foi possível escrever na Base de Dados o Email!"
                            };

                            errorCount += 1;
                            return Json(UnableToCreateEmailProcedimento);
                        }

                        EmailAutomation Email = new EmailAutomation
                        {
                            DisplayName = DBProcedimentosCCP.GetUserName(UserID),
                            From = "CCP_NAV@such.pt"
                        };

                        Email.To.Add(EmailList.EmailContabilidade);

                        if (EmailAutomation.IsValidEmail(EmailList.Email2Contabilidade))
                            Email.CC.Add(EmailList.Email2Contabilidade);

                        if (EmailAutomation.IsValidEmail(EmailList.Email3Contabilidade))
                            Email.CC.Add(EmailList.Email3Contabilidade);

                        Email.BCC.Add(UserEmail);

                        Email.Body = EmailProcedimento.TextoEmail;
                        Email.IsBodyHtml = false;

                        Email.SendEmailProcedimento();
                    }
                    else
                    {

                    }
                }

                
            }

            return Json(null);
        }
    }
}
