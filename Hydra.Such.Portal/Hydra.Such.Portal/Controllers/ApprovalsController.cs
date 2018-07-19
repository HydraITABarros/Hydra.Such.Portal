using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Logic.Approvals;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Approvals;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Portal.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using static Hydra.Such.Data.Enumerations;

using Hydra.Such.Data.ViewModel.FH;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.FolhaDeHora;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.Database;
using System.Data.SqlClient;

namespace Hydra.Such.Portal.Controllers
{
    public class ApprovalsController : Controller
    {
        private readonly ISession session;
        public ApprovalsController(IHttpContextAccessor httpContextAccessor)
        {
            this.session = httpContextAccessor.HttpContext.Session;
        }

        public IActionResult Index()
        {
            int totalPendingApprovals = DBApprovalMovements.GetAllAssignedToUserFilteredByStatus(User.Identity.Name, 1).Count;
            this.session.SetString("totalPendingApprovals", totalPendingApprovals.ToString());
            return View();
        }

        [HttpPost]
        public JsonResult GetListApprovals()
        {
            List<ApprovalMovementsViewModel> result = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetAllAssignedToUserFilteredByStatus(User.Identity.Name, 1));

            this.session.SetString("totalPendingApprovals", result.Count.ToString());

            result.ForEach(x =>
            {
                x.TypeText = EnumerablesFixed.ApprovalTypes.Where(y => y.Id == x.Type).FirstOrDefault().Value;

                switch (x.Status)
                {
                    case 1:
                        x.StatusText = "Pendente";
                        break;
                    case 2:
                        x.StatusText = "Aprovado";
                        break;
                    case 3:
                        x.StatusText = "Pendente";
                        break;
                }

                switch (x.Type)
                {
                    case 1:
                        x.NumberLink = "/GestaoRequisicoes/DetalhesReqAprovada/" + x.Number;
                        break;
                    case 2:
                        x.NumberLink = "/ModelosReqSimplificada";
                        break;
                    case 3:
                        x.NumberLink = "/FolhasDeHoras/Detalhes/?FHNo=" + x.Number;
                        break;
                }

                if (x.Type == 3)
                {
                    switch (x.Level)
                    {
                        case 1:
                            x.TypeText = "Folha de Horas - Validar";
                            break;
                        case 2:
                            x.TypeText = "Folha de Horas - Integrar Ajudas de Custo";
                            break;
                        case 3:
                            x.TypeText = "Folha de Horas - Integrar km's";
                            break;
                    }
                }
            });

            return Json(result);
        }


        //100 - Sucesso
        //101 - Movimento já foi aprovado por alguém
        //102 - Movimento já foi rejeitado por alguém
        //199 - Ocorreu um erro desconhecido
        //REQUISIÇÕES
        //200 - Requisição não encontrada
        //201 - Requisição não possui linhas
        //202 - Requisição não possui linhas
        [HttpPost]
        public JsonResult UpdateMovementStatus([FromBody] JObject requestParams)
        {
            ErrorHandler result = new ErrorHandler();

            int movementNo = int.Parse(requestParams["movementNo"].ToString());
            int movementStatus = int.Parse(requestParams["status"].ToString());
            string rejectionComments = requestParams["rejectReason"].ToString();

            //Get Approval Movement
            ApprovalMovementsViewModel approvalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetById(movementNo));

            if (approvalMovement.Status == 1)
            {
                //Check Approval Type
                if (approvalMovement.Type == 1)
                {
                    //Get Requistion and verify if exists
                    RequisitionViewModel requisition = DBRequest.ParseToViewModel(DBRequest.GetById(approvalMovement.Number));
                    if (requisition != null)
                    {
                        //Check if is to approve or reject
                        if (movementStatus == 1)
                        {
                            //Get Requistion Lines
                            //List<RequisitionLineViewModel> requesitionLines = DBRequestLine.ParseToViewModel(DBRequestLine.GetAllByRequisiçãos(requisition.RequisitionNo));
                            //if (requesitionLines != null && requesitionLines.Count > 0)
                            if (requisition.Lines.Count > 0)
                            {
                                //Check if requisition have Request Nutrition a false and all lines have ProjectNo
                                if ((!requisition.Lines.Any(x => x.ProjectNo == null || x.ProjectNo == "") && (requisition.RequestNutrition.HasValue && requisition.RequestNutrition.Value)) || !requisition.RequestNutrition.HasValue || !requisition.RequestNutrition.Value)
                                {
                                    //Approve Movement
                                    ErrorHandler approvalResult = ApprovalMovementsManager.ApproveMovement(approvalMovement.MovementNo, User.Identity.Name);

                                    //Check Approve Status
                                    if (approvalResult.eReasonCode == 103)
                                    {
                                        //Update Requisiton Data
                                        requisition.State = RequisitionStates.Approved;
                                        requisition.ResponsibleApproval = User.Identity.Name;
                                        requisition.ApprovalDate = DateTime.Now;
                                        requisition.UpdateDate = DateTime.Now;
                                        requisition.UpdateUser = User.Identity.Name;
                                        DBRequest.Update(requisition.ParseToDB());

                                        //Update Requisition Lines Data
                                        requisition.Lines.ForEach(line =>
                                        {
                                            if (line.QuantityToRequire.HasValue && line.QuantityToRequire.Value > 0)
                                            {
                                                line.QuantityRequired = line.QuantityToRequire;
                                                DBRequestLine.Update(line.ParseToDB());
                                            }
                                        });

                                        result.eReasonCode = 100;
                                        result.eMessage = "A requisição foi aprovada com sucesso.";
                                    }
                                    else if (approvalResult.eReasonCode == 100)
                                    {
                                        result.eReasonCode = 100;
                                        result.eMessage = "Requisição aprovada com sucesso, encontra-se a aguardar aprovação do nivel seguinte.";
                                    }
                                    else
                                    {
                                        result.eReasonCode = 199;
                                        result.eMessage = "Ocorreu um erro desconhecido ao aprovar a requisição.";
                                    }
                                }
                                else
                                {
                                    result.eReasonCode = 202;
                                    result.eMessage = "Todas as linhas necessitam de possuir NºOrdem/Projeto.";
                                }
                            }
                            else
                            {
                                result.eReasonCode = 201;
                                result.eMessage = "A requisição não possui linhas.";
                            }
                        }
                        else if (movementStatus == 2)
                        {
                            //Reject Movement
                            ErrorHandler approveResult = ApprovalMovementsManager.RejectMovement(approvalMovement.MovementNo, User.Identity.Name, rejectionComments);

                            //Check Approve Status
                            if (approveResult.eReasonCode == 100)
                            {
                                //Update Requisiton Data
                                requisition.State = RequisitionStates.Rejected;
                                requisition.ResponsibleApproval = User.Identity.Name;
                                requisition.ApprovalDate = DateTime.Now;
                                requisition.UpdateDate = DateTime.Now;
                                requisition.UpdateUser = User.Identity.Name;
                                requisition.Comments += rejectionComments;
                                DBRequest.Update(requisition.ParseToDB());

                                result.eReasonCode = 100;
                                result.eMessage = "A requisição foi rejeitada com sucesso.";
                            }
                            else
                            {
                                result.eReasonCode = 199;
                                result.eMessage = "Ocorreu um erro desconhecido ao rejeitar a requisição.";
                            }
                        }
                    }
                    else
                    {
                        result.eReasonCode = 200;
                        result.eMessage = "A requisição já não existe.";
                    }
                }
                //Folhas de Horas - Validar
                else if (approvalMovement.Type == 3 && approvalMovement.Level == 1)
                {
                    //Get Folha de Horas and verify if exists
                    FolhasDeHoras FolhaHoras = DBFolhasDeHoras.GetById(approvalMovement.Number);

                    if (FolhaHoras != null)
                    {
                        //Aprovar
                        if (movementStatus == 1)
                        {
                            if (string.IsNullOrEmpty(FolhaHoras.NºFolhaDeHoras) || string.IsNullOrEmpty(FolhaHoras.NºEmpregado))
                            {
                                result.eReasonCode = 101;
                                result.eMessage = "Faltam preencher os campos obrigatórios.";
                            }
                            else
                            {
                                if ((FolhaHoras.Validado == null ? false : (bool)FolhaHoras.Validado) || (int)FolhaHoras.Estado != 0)
                                {
                                    result.eReasonCode = 101;
                                    result.eMessage = "A Folha de Horas já se encontra validada.";
                                }
                                else
                                {
                                    if (!FolhaHoras.Validadores.ToLower().Contains(User.Identity.Name.ToLower()))
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
                                                new SqlParameter("@NoFH", FolhaHoras.NºFolhaDeHoras),
                                                new SqlParameter("@NoUtilizador", FolhaHoras.NºEmpregado),
                                                new SqlParameter("@NoValidador", User.Identity.Name)
                                            };

                                            int Resultado = ctx.execStoredProcedureFH("exec FH_Validar @NoFH, @NoUtilizador, @NoValidador", parameters);

                                            if (Resultado == 0)
                                            {

                                                int NoAjudasCusto = DBLinhasFolhaHoras.GetAll().Where(x => x.NoFolhaHoras.ToLower() == FolhaHoras.NºFolhaDeHoras.ToLower() && x.TipoCusto == 2).Count();

                                                if (FolhaHoras.TipoDeslocação != 2 && NoAjudasCusto == 0)
                                                    FolhaHoras.Estado = 2; // 2 = Registado
                                                else
                                                    FolhaHoras.Estado = 1; //VALIDADO

                                                FolhaHoras.Validado = true;
                                                FolhaHoras.Validador = User.Identity.Name;
                                                FolhaHoras.DataHoraValidação = DateTime.Now;
                                                FolhaHoras.DataHoraÚltimoEstado = DateTime.Now;
                                                FolhaHoras.DataHoraModificação = DateTime.Now;
                                                FolhaHoras.UtilizadorModificação = User.Identity.Name;

                                                if (DBFolhasDeHoras.Update(FolhaHoras) == null)
                                                {
                                                    result.eReasonCode = 101; //Houve erro no Update na Folha de Horas
                                                    result.eMessage = "Ocorreu um erro ao atulizar a Folha de Horas.";
                                                }
                                                else
                                                {
                                                    List<PresencasFolhaDeHorasViewModel> Presencas = DBPresencasFolhaDeHoras.GetAllByPresencaToList(FolhaHoras.NºFolhaDeHoras);
                                                    if (Presencas != null)
                                                    {
                                                        Presencas.ForEach(x =>
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

                                                    if (FolhaHoras.Estado == 1)
                                                    {
                                                        //Approve Movement
                                                        ErrorHandler approvalResult = ApprovalMovementsManager.ApproveMovement_FH(approvalMovement.MovementNo, User.Identity.Name);

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

                                                    if (FolhaHoras.Estado == 2)
                                                    {
                                                        //Update Old Movement
                                                        ApprovalMovementsViewModel ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetById(movementNo));
                                                        if (ApprovalMovement != null)
                                                        {
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
                                            else
                                            {
                                                result.eReasonCode = 101;
                                                result.eMessage = "Ocorreu um erro no script SQL de Validaçãodo na Folha de Horas.";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //Rejeitar
                        else if (movementStatus == 2)
                        {
                            //Reject Movement
                            ErrorHandler approveResult = ApprovalMovementsManager.RejectMovement_FH(approvalMovement.MovementNo, User.Identity.Name, rejectionComments);

                            //Check Approve Status
                            if (approveResult.eReasonCode == 100)
                            {
                                //Update Folha de Horas Data
                                FolhaHoras.Estado = 0;
                                FolhaHoras.Terminada = false;
                                FolhaHoras.TerminadoPor = null;
                                FolhaHoras.DataHoraTerminado = null;
                                FolhaHoras.Validado = false;
                                FolhaHoras.Validador = null;
                                FolhaHoras.DataHoraValidação = null;
                                FolhaHoras.Observações = FolhaHoras.Observações + "\r\nRejeição: " + rejectionComments + " - Data: " + DateTime.Now.ToString() + " - Utilizador: " + User.Identity.Name;
                                FolhaHoras.DataHoraÚltimoEstado = DateTime.Now;
                                FolhaHoras.UtilizadorModificação = User.Identity.Name;
                                FolhaHoras.DataHoraModificação = DateTime.Now;
                                DBFolhasDeHoras.Update(FolhaHoras);

                                result.eReasonCode = 100;
                                result.eMessage = "A Folha de Horas foi rejeitada com sucesso.";
                            }
                            else
                            {
                                result.eReasonCode = 199;
                                result.eMessage = "Ocorreu um erro desconhecido ao rejeitar a Folha de Horas.";
                            }
                        }
                    }
                }
                //Folhas de Horas - Integrar Aj. Custo RH
                else if (approvalMovement.Type == 3 && approvalMovement.Level == 2)
                {
                    //Get Folha de Horas and verify if exists
                    FolhasDeHoras FolhaHoras = DBFolhasDeHoras.GetById(approvalMovement.Number);

                    if (FolhaHoras != null)
                    {
                        //Aprovar
                        if (movementStatus == 1)
                        {
                            if (string.IsNullOrEmpty(FolhaHoras.NºFolhaDeHoras) || string.IsNullOrEmpty(FolhaHoras.NºEmpregado) || string.IsNullOrEmpty(FolhaHoras.NºProjeto))
                            {
                                result.eReasonCode = 101;
                                result.eMessage = "Faltam preencher algum campo obrigatório na Folha de Horas.";
                            }
                            else
                            {
                                if (FolhaHoras.IntegradoEmRh == null ? false : (bool)FolhaHoras.IntegradoEmRh)
                                {
                                    result.eReasonCode = 101;
                                    result.eMessage = "Já foram integradas as Ajudas de Custo na Folha de Horas.";
                                }
                                else
                                {
                                    if ((int)FolhaHoras.Estado != 1)
                                    {
                                        result.eReasonCode = 101;
                                        result.eMessage = "É necessário primeiro validar a Folha de Horas.";
                                    }
                                    else
                                    {
                                        if (!FolhaHoras.IntegradoresEmRh.ToLower().Contains(User.Identity.Name.ToLower()))
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
                                                    new SqlParameter("@NoFH", FolhaHoras.NºFolhaDeHoras),
                                                    new SqlParameter("@NoUtilizador", FolhaHoras.NºEmpregado),
                                                    new SqlParameter("@NoValidador", User.Identity.Name)
                                                };

                                                int Resultado = ctx.execStoredProcedureFH("exec FH_IntegrarEmRH @NoFH, @NoUtilizador, @NoValidador", parameters);

                                                if (Resultado == 0)
                                                {
                                                    bool IntegradoEmRhKm = (bool)FolhaHoras.IntegradoEmRhkm;
                                                    int TipoDeslocação = (int)FolhaHoras.TipoDeslocação;
                                                    int Estado = (int)FolhaHoras.Estado;

                                                    if (IntegradoEmRhKm || TipoDeslocação != 2)
                                                        Estado = 2; // 2 = Registado

                                                    FolhaHoras.Estado = Estado; //INTEGRAREMRH
                                                    FolhaHoras.IntegradoEmRh = true; //INTEGRAREMRH
                                                    FolhaHoras.IntegradorEmRh = User.Identity.Name; //INTEGRAREMRH
                                                    FolhaHoras.DataIntegraçãoEmRh = DateTime.Now; //INTEGRAREMRH
                                                    FolhaHoras.UtilizadorModificação = User.Identity.Name; //INTEGRAREMRH
                                                    FolhaHoras.DataHoraModificação = DateTime.Now; //INTEGRAREMRH

                                                    if (DBFolhasDeHoras.Update(FolhaHoras) == null)
                                                    {
                                                        result.eReasonCode = 101; //Houve erro no Update na Folha de Horas
                                                        result.eMessage = "Houve erro na atualização da Folha de Horas.";
                                                    }
                                                    else
                                                    {
                                                        if (FolhaHoras.Estado == 1)
                                                        {
                                                            //Approve Movement
                                                            ErrorHandler approvalResult = ApprovalMovementsManager.ApproveMovement_FH(approvalMovement.MovementNo, User.Identity.Name);

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

                                                        if (FolhaHoras.Estado == 2)
                                                        {
                                                            //Update Old Movement
                                                            ApprovalMovementsViewModel ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetById(movementNo));
                                                            if (ApprovalMovement != null)
                                                            {
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
                                                else
                                                {
                                                    result.eReasonCode = 101;
                                                    result.eMessage = "Ocorreu um erro no script SQL de Validaçãodo na Folha de Horas.";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //Rejeitar
                        else if (movementStatus == 2)
                        {
                            result.eReasonCode = 101;
                            result.eMessage = "A Folha de Horas não pode ser rejeitada neste nível.";
                        }
                    }
                }
                //Folhas de Horas - Integrar kms RH
                else if (approvalMovement.Type == 3 && approvalMovement.Level == 3)
                {
                    //Get Folha de Horas and verify if exists
                    FolhasDeHoras FolhaHoras = DBFolhasDeHoras.GetById(approvalMovement.Number);

                    if (FolhaHoras != null)
                    {
                        //Aprovar
                        if (movementStatus == 1)
                        {
                            if (string.IsNullOrEmpty(FolhaHoras.NºFolhaDeHoras) || string.IsNullOrEmpty(FolhaHoras.NºEmpregado) || string.IsNullOrEmpty(FolhaHoras.NºProjeto) || FolhaHoras.TipoDeslocação != 2) //2 = "Viatura Própria"
                            {
                                result.eReasonCode = 101;
                                result.eMessage = "Faltam preencher algum campo obrigatório na Folha de Horas.";
                            }
                            else
                            {
                                if (FolhaHoras.IntegradoEmRhkm == null ? false : (bool)FolhaHoras.IntegradoEmRhkm)
                                {
                                    result.eReasonCode = 101;
                                    result.eMessage = "Já foram integradas os km's na Folha de Horas.";
                                }
                                else
                                {
                                    if ((int)FolhaHoras.Estado != 1)
                                    {
                                        result.eReasonCode = 101;
                                        result.eMessage = "A Folha de Horas não está num estado possível de Integrar os km's.";
                                    }
                                    else
                                    {
                                        if (!FolhaHoras.IntegradoresEmRhkm.ToLower().Contains(User.Identity.Name.ToLower()))
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
                                                    new SqlParameter("@NoFH", FolhaHoras.NºFolhaDeHoras),
                                                    new SqlParameter("@NoUtilizador", FolhaHoras.NºEmpregado),
                                                    new SqlParameter("@NoValidador", User.Identity.Name)
                                                };

                                                int Resultado = ctx.execStoredProcedureFH("exec FH_IntegrarEmRHKM @NoFH, @NoUtilizador, @NoValidador", parameters);

                                                if (Resultado == 0)
                                                {
                                                    bool IntegradoEmRh = (bool)FolhaHoras.IntegradoEmRh;
                                                    int NoRegistos = 0;
                                                    int Estado = (int)FolhaHoras.Estado;

                                                    NoRegistos = DBLinhasFolhaHoras.GetAll().Where(x => x.NoFolhaHoras.ToLower() == FolhaHoras.NºFolhaDeHoras.ToLower() && x.TipoCusto == 2).Count();

                                                    if (IntegradoEmRh || NoRegistos == 0)
                                                        Estado = 2; // 2 = Registado

                                                    FolhaHoras.Estado = Estado; //INTEGRAREMRHKM
                                                    FolhaHoras.IntegradoEmRhkm = true; //INTEGRAREMRHKM
                                                    FolhaHoras.IntegradorEmRhKm = User.Identity.Name; //INTEGRAREMRHKM
                                                    FolhaHoras.DataIntegraçãoEmRhKm = DateTime.Now; //INTEGRAREMRHKM
                                                    FolhaHoras.UtilizadorModificação = User.Identity.Name; //INTEGRAREMRHKM
                                                    FolhaHoras.DataHoraModificação = DateTime.Now; //INTEGRAREMRHKM

                                                    if (DBFolhasDeHoras.Update(FolhaHoras) == null)
                                                    {
                                                        result.eReasonCode = 101;
                                                        result.eMessage = "Houve erro na atualização da Folha de Horas.";
                                                    }
                                                    else
                                                    {
                                                        if (FolhaHoras.Estado == 1)
                                                        {
                                                            //Approve Movement
                                                            ErrorHandler approvalResult = ApprovalMovementsManager.ApproveMovement_FH(approvalMovement.MovementNo, User.Identity.Name);

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

                                                        if (FolhaHoras.Estado == 2)
                                                        {
                                                            //Update Old Movement
                                                            ApprovalMovementsViewModel ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetById(movementNo));

                                                            if (ApprovalMovement != null)
                                                            {
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
                                                else
                                                {
                                                    result.eReasonCode = 101;
                                                    result.eMessage = "Ocorreu um erro no script SQL de Validaçãodo na Folha de Horas.";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //Rejeitar
                        else if (movementStatus == 2)
                        {
                            result.eReasonCode = 101;
                            result.eMessage = "A Folha de Horas não pode ser rejeitada neste nível.";
                        }
                    }
                }
            }
            else if (approvalMovement.Status == 1)
            {
                result.eReasonCode = 101;
                result.eMessage = "Esta linha já foi aprovada pelo utilizador " + approvalMovement.UserUpdate + ".";
            }
            else if (approvalMovement.Status == 2)
            {
                result.eReasonCode = 102;
                result.eMessage = "Esta linha foi rejeitada pelo utilizador " + approvalMovement.UserUpdate + ".";
            }
            return Json(result);
        }
    }
}