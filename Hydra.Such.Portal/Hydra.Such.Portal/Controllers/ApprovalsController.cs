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
                        x.NumberLink = "/Compras/GestaoRequisicoes/DetalhesReqAprovada/"+x.Number;
                        break;
                    case 2:
                        x.NumberLink = "/Compras/Nutricao/ModelosReqSimplificada";
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

            int PMovementNo = int.Parse(requestParams["movementNo"].ToString());
            int PMovementStatus = int.Parse(requestParams["status"].ToString());
            string PRejectReason = requestParams["rejectReason"].ToString();

            //Get Approval Movement
            ApprovalMovementsViewModel CAMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetById(PMovementNo));

            if (CAMovement.Status == 1)
            {
                //Check Approval Type
                if (CAMovement.Type == 1)
                {
                    //Get Requistion and verify if exists
                    RequisitionViewModel CRequesition = DBRequest.ParseToViewModel(DBRequest.GetById(CAMovement.Number));
                    if (CRequesition != null)
                    {
                        //Check if is to approve or reject
                        if (PMovementStatus == 1)
                        {
                            //Get Requistion Lines
                            List<RequisitionLineViewModel> CRequesitionLines = DBRequestLine.ParseToViewModel(DBRequestLine.GetAllByRequisiçãos(CRequesition.RequisitionNo));
                            if (CRequesitionLines != null && CRequesitionLines.Count > 0)
                            {
                                //Check if requisition have Request Nutrition a false and all lines have ProjectNo
                                if ((!CRequesitionLines.Any(x => x.ProjectNo == null || x.ProjectNo == "") && (CRequesition.RequestNutrition.HasValue && CRequesition.RequestNutrition.Value)) || !CRequesition.RequestNutrition.HasValue || !CRequesition.RequestNutrition.Value)
                                {
                                    //Approve Movement
                                    ErrorHandler ApproveResult = ApprovalMovementsManager.ApproveMovement(CAMovement.MovementNo, User.Identity.Name);

                                    //Check Approve Status
                                    if (ApproveResult.eReasonCode == 103)
                                    {
                                        //Update Requisiton Data
                                        CRequesition.State = RequisitionStates.Approved;
                                        CRequesition.ResponsibleApproval = User.Identity.Name;
                                        CRequesition.ApprovalDate = DateTime.Now;
                                        CRequesition.UpdateDate = DateTime.Now;
                                        CRequesition.UpdateUser = User.Identity.Name;
                                        DBRequest.Update(DBRequest.ParseToDB(CRequesition));

                                        //Update Requisition Lines Data
                                        CRequesitionLines.ForEach(line => {
                                            if (line.QuantityToRequire.HasValue && line.QuantityToRequire.Value > 0)
                                            {
                                                line.QuantityRequired = line.QuantityToRequire;
                                                DBRequestLine.Update(DBRequestLine.ParseToDB(line));
                                            }
                                        });

                                        result.eReasonCode = 100;
                                        result.eMessage = "A requisição foi aprovada com sucesso.";
                                    }
                                    else if (ApproveResult.eReasonCode == 100)
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
                                result.eMessage = "A requisição que tentou não possui linhas.";
                            }
                        }
                        else if (PMovementStatus == 2)
                        {
                            //????????????????????????????????????
                            //????????????????????????????????????
                            // A AGUARDAR INFORMAÇÃO DO CLIENTE
                            //????????????????????????????????????
                            //????????????????????????????????????
                        }
                    }
                    else
                    {
                        result.eReasonCode = 200;
                        result.eMessage = "A requisição que tentou aprovar já não existe.";
                    }
                }
            }
            else if (CAMovement.Status == 1)
            {
                result.eReasonCode = 101;
                result.eMessage = "Esta linha já foi aprovada pelo utilizador " + CAMovement.UserUpdate + ".";
            }
            else if (CAMovement.Status == 2)
            {
                result.eReasonCode = 102;
                result.eMessage = "Esta linha foi rejeitada pelo utilizador " + CAMovement.UserUpdate + ".";
            }
            return Json(result);
        }
    }
}