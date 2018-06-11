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
            List<ApprovalMovementsViewModel> result = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetAllAssignedToUserFilteredByStatus(User.Identity.Name,1));

            this.session.SetString("totalPendingApprovals", result.Count.ToString());

            result.ForEach(x => {
                x.TypeText = EnumerablesFixed.ApprovalTypes.Where(y => y.Id == x.Type).FirstOrDefault().Value;
                x.AreaText = EnumerablesFixed.Areas.Where(y => y.Id == x.Area).FirstOrDefault().Value;
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
                        x.NumberLink = "/GestaoRequisicoes/DetalhesReqAprovada/"+x.Number;
                        break;
                    case 2:
                        x.NumberLink = "/ModelosReqSimplificada";
                        break;
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
                            if(requisition.Lines.Count > 0)
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
                                        requisition.Lines.ForEach(line => {
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