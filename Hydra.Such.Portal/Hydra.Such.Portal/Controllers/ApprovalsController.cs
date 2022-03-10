﻿using System;
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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Hydra.Such.Portal.Services;
using Hydra.Such.Data.NAV;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.ViewModel.Projects;

namespace Hydra.Such.Portal.Controllers
{
    public class ApprovalsController : Controller
    {
        private readonly ISession session;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly NAVWSConfigurations configws;
        private readonly GeneralConfigurations _generalConfig;
        private readonly NAVConfigurations _config;

        public ApprovalsController(IOptions<NAVConfigurations> appSettings, IHttpContextAccessor httpContextAccessor, IOptions<NAVWSConfigurations> NAVWSConfigs, IHostingEnvironment _hostingEnvironment, IOptions<GeneralConfigurations> appSettingsGeneral)
        {
            this.session = httpContextAccessor.HttpContext.Session;
            this._hostingEnvironment = _hostingEnvironment;
            configws = NAVWSConfigs.Value;
            _generalConfig = appSettingsGeneral.Value;
            _config = appSettings.Value;
        }

        public IActionResult Index()
        {
            List<ApprovalMovementsViewModel> result = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetAllAssignedToUserFilteredByStatus(User.Identity.Name, 1));

            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.Region));
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.FunctionalArea));
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.ResponsabilityCenter));

            int totalPendingApprovals = result != null ? result.Count : 0;
            this.session.SetString("totalPendingApprovals", totalPendingApprovals.ToString());
            return View();

            //int totalPendingApprovals = DBApprovalMovements.GetAllAssignedToUserFilteredByStatus(User.Identity.Name, 1).Count;
            //this.session.SetString("totalPendingApprovals", totalPendingApprovals.ToString());
            //return View();
        }

        [HttpPost]
        public JsonResult GetListApprovals()
        {
            List<ApprovalMovementsViewModel> result = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetAllAssignedToUserFilteredByStatus(User.Identity.Name, 1));

            //List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
            //    result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.Region));
            //if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
            //    result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.FunctionalArea));
            //if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
            //    result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.ResponsabilityCenter));

            this.session.SetString("totalPendingApprovals", result != null ? result.Count.ToString() : 0.ToString());

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
                        x.NumberLink = "/GestaoRequisicoes/MinhaRequisicao/" + x.Number; //"/GestaoRequisicoes/DetalhesReqAprovada/"
                        break;
                    case 2:
                        x.NumberLink = "/ModelosReqSimplificada";
                        break;
                    case 3:
                        x.NumberLink = "/FolhasDeHoras/Detalhes/?FHNo=" + x.Number;
                        break;
                    case 4:
                        x.NumberLink = "/GestaoRequisicoes/MinhaRequisicao_CD/" + x.Number;
                        break;
                    case 5:
                        x.NumberLink = "/Projetos/Detalhes/" + x.Number;
                        break;
                }

                if (x.Type == 3)
                {
                    FolhasDeHoras FH = DBFolhasDeHoras.GetById(x.Number);
                    if (FH != null)
                    {
                        x.FHDatePartida = FH.DataHoraPartida.HasValue ? Convert.ToDateTime(FH.DataHoraPartida).ToShortDateString() : "";
                        x.FHDateChegada = FH.DataHoraChegada.HasValue ? Convert.ToDateTime(FH.DataHoraChegada).ToShortDateString() : "";
                        x.RequisicaoProjectNo = !string.IsNullOrEmpty(FH.NºProjeto) ? FH.NºProjeto : "";
                    }

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

                if (x.Type == 1) //Requisições
                {
                    Requisição REQ = DBRequest.GetById(x.Number);

                    if (REQ != null)
                    {
                        switch (REQ.TipoReq)
                        {
                            case 0:
                                x.NumberLink = "/GestaoRequisicoes/MinhaRequisicao/" + x.Number; //"/GestaoRequisicoes/DetalhesReqAprovada/"
                                break;
                            case 1:
                                x.NumberLink = "/GestaoRequisicoes/MinhaRequisicao_CD/" + x.Number; //"/GestaoRequisicoes/DetalhesReqAprovada/"
                                break;
                        }

                        x.RequisicaoProjectNo = REQ.NºProjeto;
                        x.RequisicaoClientNo = REQ.ClientNo;
                        x.RequisicaoClientName = REQ.ClientName;
                        x.RequisicaoAcordosPrecos = REQ.RequisiçãoNutrição.HasValue ? REQ.RequisiçãoNutrição == true ? "Sim" : "Não" : "";
                        x.RequisicaoUrgente = REQ.Urgente.HasValue ? REQ.Urgente == true ? "Sim" : "Não" : "";
                        x.RequisicaoOrcamentoEmAnexo = REQ.Orçamento.HasValue ? REQ.Orçamento == true ? "Sim" : "Não" : "";
                        x.RequisicaoImobilizado = REQ.Imobilizado.HasValue ? REQ.Imobilizado == true ? "Sim" : "Não" : "";
                        x.RequisicaoExclusivo = REQ.Exclusivo.HasValue ? REQ.Exclusivo == true ? "Sim" : "Não" : "";
                        x.RequisicaoJaExecutado = REQ.JáExecutado.HasValue ? REQ.JáExecutado == true ? "Sim" : "Não" : "";
                        x.RequisicaoAmostra = REQ.Amostra.HasValue ? REQ.Amostra == true ? "Sim" : "Não" : "";
                        x.RequisicaoEquipamento = REQ.Equipamento.HasValue ? REQ.Equipamento == true ? "Sim" : "Não" : "";
                        x.RequisicaoReposicaoDeStock = REQ.ReposiçãoDeStock.HasValue ? REQ.ReposiçãoDeStock == true ? "Sim" : "Não" : "";
                        x.RequisicaoPrecoIvaIncluido = REQ.PrecoIvaincluido.HasValue ? REQ.PrecoIvaincluido == true ? "Sim" : "Não" : "";
                        x.RequisicaoAdiantamento = REQ.Adiantamento.HasValue ? REQ.Adiantamento == true ? "Sim" : "Não" : "";
                        x.RequisicaoPedirOrcamento = REQ.PedirOrcamento.HasValue ? REQ.PedirOrcamento == true ? "Sim" : "Não" : "";
                        x.RequisicaoRoupaManutencao = REQ.RoupaManutencao.HasValue ? REQ.RoupaManutencao == true ? "Sim" : "Não" : "";
                    }
                }
            });

            return Json(result.OrderByDescending(x => x.MovementNo));
        }

        [HttpPost]
        public JsonResult GetREQListApprovals()
        {
            List<ApprovalMovementsViewModel> result = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetAllREQAssignedToUserFilteredByStatus(User.Identity.Name, 1));

            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.Region));
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.FunctionalArea));
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.ResponsabilityCenter));

            this.session.SetString("totalPendingApprovals", result != null ? result.Count.ToString() : 0.ToString());

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

                if (x.Type == 1) //Requisições
                {
                    Requisição REQ = DBRequest.GetById(x.Number);

                    if (REQ != null)
                        x.RequisicaoAcordosPrecos = REQ.RequisiçãoNutrição.HasValue ? REQ.RequisiçãoNutrição == true ? "Sim" : "Não" : "";
                }
            });

            return Json(result.OrderByDescending(x => x.MovementNo));
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
                    if (requisition != null && requisition.State == RequisitionStates.Pending)
                    {
                        //Check if is to approve or reject
                        if (movementStatus == 1)
                        {
                            //Get Requistion Lines
                            //List<RequisitionLineViewModel> requesitionLines = DBRequestLine.ParseToViewModel(DBRequestLine.GetAllByRequisiçãos(requisition.RequisitionNo));
                            //if (requesitionLines != null && requesitionLines.Count > 0)
                            if (requisition.Lines != null && requisition.Lines.Count > 0)
                            {
                                //Check if requisition have Request Nutrition a false and all lines have ProjectNo
                                if ((!requisition.Lines.Any(x => x.ProjectNo == null || x.ProjectNo == "") && (requisition.RequestNutrition.HasValue && requisition.RequestNutrition.Value)) || !requisition.RequestNutrition.HasValue || !requisition.RequestNutrition.Value)
                                {
                                    //Approve Movement
                                    ErrorHandler approvalResult = new ErrorHandler();

                                    //Update Requisiton Data
                                    requisition.State = RequisitionStates.Approved;
                                    requisition.ResponsibleApproval = User.Identity.Name;
                                    requisition.ApprovalDate = DateTime.Now;
                                    requisition.UpdateDate = DateTime.Now;
                                    requisition.UpdateUser = User.Identity.Name;

                                    //if (DBRequest.Update(requisition.ParseToDB()) != null)
                                    //Requisição ReqDB = new Requisição();
                                    //ReqDB = requisition.ParseToDB();
                                    if (DBRequest.Update(requisition.ParseToDB()) != null)
                                    {
                                        //Update Requisition Lines Data
                                        if (requisition.Lines != null && requisition.Lines.Count > 0)
                                        {
                                            requisition.Lines.ForEach(line =>
                                            {
                                                if (line.QuantityToRequire.HasValue && line.QuantityToRequire.Value > 0)
                                                {
                                                    line.QuantityRequired = line.QuantityToRequire;
                                                    DBRequestLine.Update(line.ParseToDB());
                                                }
                                            });
                                        }

                                        //SISLOGRESERVAS Update Lines Data
                                        if (requisition.Lines != null && requisition.Lines.Count > 0)
                                        {
                                            requisition.Lines.ForEach(line =>
                                            {
                                                if (!string.IsNullOrEmpty(line.LocalCode) && (line.LocalCode == "2300" || line.LocalCode == "3300" || line.LocalCode == "4300"))
                                                {
                                                    SISLOGReservas Reserva = DBSISLOGReservas.GetById(line.RequestNo, (int)line.LineNo);

                                                    if (Reserva != null)
                                                    {
                                                        Reserva.Reservado = false;
                                                        Reserva.UtilizadorModificacao = User.Identity.Name;
                                                        Reserva.DataHoraModificacao = DateTime.Now;

                                                        DBSISLOGReservas.Update(Reserva);
                                                    }
                                                }
                                            });
                                        }

                                        //Create Workflow de Aprovação
                                        var ctx = new SuchDBContext();
                                        var logEntry = new RequisicoesRegAlteracoes();
                                        logEntry.NºRequisição = requisition.RequisitionNo;
                                        logEntry.Estado = (int)RequisitionStates.Approved; //APROVADO = 4
                                        logEntry.ModificadoEm = DateTime.Now;
                                        logEntry.ModificadoPor = User.Identity.Name;
                                        ctx.RequisicoesRegAlteracoes.Add(logEntry);
                                        ctx.SaveChanges();


                                        //Se a requesição tiver o campo "Requisição Nutrição" a true faz automaticamente a validação
                                        if (requisition != null && requisition.RequestNutrition.HasValue && requisition.RequestNutrition == true)
                                        {
                                            try
                                            {
                                                requisition.eReasonCode = 204;
                                                requisition.eMessage = "Ocorreu um erro na Validação da Requisição.";

                                                RequisitionService serv = new RequisitionService(configws, HttpContext.User.Identity.Name);
                                                requisition = serv.ValidateRequisition(requisition);
                                                if (requisition != null && requisition.eReasonCode == 1)
                                                {
                                                    //Create Workflow
                                                    //ctx = new SuchDBContext();
                                                    //logEntry = new RequisicoesRegAlteracoes();
                                                    //logEntry.NºRequisição = requisition.RequisitionNo;
                                                    //logEntry.Estado = (int)RequisitionStates.Validated; //VALIDADO = 3
                                                    //logEntry.ModificadoEm = DateTime.Now;
                                                    //logEntry.ModificadoPor = User.Identity.Name;
                                                    //ctx.RequisicoesRegAlteracoes.Add(logEntry);
                                                    //ctx.SaveChanges();

                                                    Requisição REQTMP = DBRequest.GetById(approvalMovement.Number);
                                                    if (REQTMP != null && REQTMP.Estado.HasValue && REQTMP.Estado == (int)RequisitionStates.Validated)
                                                    {
                                                        approvalResult = ApprovalMovementsManager.ApproveMovement(approvalMovement.MovementNo, User.Identity.Name);

                                                        if (approvalResult != null && approvalResult.eReasonCode == 103)
                                                        {
                                                            result.eReasonCode = 100;
                                                            result.eMessage = "A requisição foi aprovada e validada com sucesso.";
                                                        }
                                                        else
                                                        {
                                                            result.eReasonCode = 204;
                                                            result.eMessage = "Ocorreu um erro no Movimento de Aprovação da Requisição.";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        result.eReasonCode = 204;
                                                        result.eMessage = "Ocorreu um erro na Validação da Requisição.";
                                                    }
                                                }
                                                else
                                                {
                                                    result.eReasonCode = requisition.eReasonCode;
                                                    result.eMessage = requisition.eMessage;
                                                }

                                                return Json(result);
                                            }
                                            catch (Exception ex)
                                            {
                                                result.eReasonCode = 204;
                                                result.eMessage = "Ocorreu um erro inesperado na Validação da Requisição.";

                                                return Json(result);
                                            }
                                        }

                                        approvalResult = ApprovalMovementsManager.ApproveMovement(approvalMovement.MovementNo, User.Identity.Name);

                                        if (approvalResult.eReasonCode == 103)
                                        {
                                            result.eReasonCode = 100;
                                            result.eMessage = "A requisição foi aprovada com sucesso.";

                                        }
                                        else if (approvalResult.eReasonCode == 100)
                                        {
                                            result.eReasonCode = 100;
                                            result.eMessage = "Requisição aprovada com sucesso, encontra-se a aguardar aprovação do nivel seguinte.";
                                        }
                                    }
                                    else
                                    {
                                        result.eReasonCode = 204;
                                        result.eMessage = "Ocorreu um erro na atualização da Requisição.";
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
                            //Update Requisiton Data
                            requisition.State = RequisitionStates.Rejected;
                            requisition.ResponsibleApproval = User.Identity.Name;
                            requisition.ApprovalDate = DateTime.Now;
                            requisition.UpdateDate = DateTime.Now;
                            requisition.UpdateUser = User.Identity.Name;
                            requisition.RejeicaoMotivo = rejectionComments;
                            if (DBRequest.Update(requisition.ParseToDB()) != null)
                            {
                                //Create Workflow
                                var ctx = new SuchDBContext();
                                var logEntry = new RequisicoesRegAlteracoes();
                                logEntry.NºRequisição = requisition.RequisitionNo;
                                logEntry.Estado = (int)RequisitionStates.Rejected; //REJEITADO = 5
                                logEntry.ModificadoEm = DateTime.Now;
                                logEntry.ModificadoPor = User.Identity.Name;
                                ctx.RequisicoesRegAlteracoes.Add(logEntry);
                                ctx.SaveChanges();

                                //Reject Movement
                                ErrorHandler approveResult = ApprovalMovementsManager.RejectMovement(approvalMovement.MovementNo, User.Identity.Name, rejectionComments);
                                if (approveResult.eReasonCode == 100)
                                {
                                    result.eReasonCode = 100;
                                    result.eMessage = "A requisição foi rejeitada com sucesso.";
                                }
                                else
                                {
                                    result.eReasonCode = 199;
                                    result.eMessage = "Ocorreu um erro desconhecido ao rejeitar a requisição.";
                                }
                            }
                            else
                            {
                                result.eReasonCode = 11;
                                result.eMessage = "Ocorreu um erro ao rejeitar a Requisição.";
                            }
                        }
                    }
                    else
                    {
                        result.eReasonCode = 200;
                        result.eMessage = "A requisição não existe ou já aprovada.";
                    }
                }
                //COMPRAS A DINHEIRO
                else if (approvalMovement.Type == 4)
                {
                    //Get Requistion and verify if exists
                    RequisitionViewModel requisition = DBRequest.ParseToViewModel(DBRequest.GetById(approvalMovement.Number));
                    if (requisition != null && requisition.State == RequisitionStates.Pending)
                    {
                        //Check if is to approve or reject
                        if (movementStatus == 1)
                        {
                            //Get Requistion Lines
                            if (requisition.Lines.Count > 0)
                            {
                                //Check if requisition have Request Nutrition a false and all lines have ProjectNo
                                if ((!requisition.Lines.Any(x => x.ProjectNo == null || x.ProjectNo == "") && (requisition.RequestNutrition.HasValue && requisition.RequestNutrition.Value)) || !requisition.RequestNutrition.HasValue || !requisition.RequestNutrition.Value)
                                {
                                    //Approve Movement
                                    ErrorHandler approvalResult = new ErrorHandler();

                                    //Update Requisiton Data
                                    requisition.State = RequisitionStates.Approved;
                                    requisition.ResponsibleApproval = User.Identity.Name;
                                    requisition.ApprovalDate = DateTime.Now;
                                    requisition.UpdateDate = DateTime.Now;
                                    requisition.UpdateUser = User.Identity.Name;
                                    if (DBRequest.Update(requisition.ParseToDB()) != null)
                                    {
                                        //Create Workflow
                                        var ctx = new SuchDBContext();
                                        var logEntry = new RequisicoesRegAlteracoes();
                                        logEntry.NºRequisição = requisition.RequisitionNo;
                                        logEntry.Estado = (int)RequisitionStates.Approved; //APROVADO = 4
                                        logEntry.ModificadoEm = DateTime.Now;
                                        logEntry.ModificadoPor = User.Identity.Name;
                                        ctx.RequisicoesRegAlteracoes.Add(logEntry);
                                        ctx.SaveChanges();

                                        //Update Requisition Lines Data
                                        requisition.Lines.ForEach(line =>
                                        {
                                            if (line.QuantityToRequire.HasValue && line.QuantityToRequire.Value > 0)
                                            {
                                                line.QuantityRequired = line.QuantityToRequire;
                                                DBRequestLine.Update(line.ParseToDB());
                                            }
                                        });

                                        approvalResult = ApprovalMovementsManager.ApproveMovement(approvalMovement.MovementNo, User.Identity.Name);

                                        //Check Approve Status
                                        if (approvalResult.eReasonCode == 103)
                                        {
                                            result.eReasonCode = 100;
                                            result.eMessage = "A Compras Dinheiro foi aprovada com sucesso.";
                                        }
                                        else if (approvalResult.eReasonCode == 100)
                                        {
                                            result.eReasonCode = 100;
                                            result.eMessage = "Compras Dinheiro aprovada com sucesso, encontra-se a aguardar aprovação do nivel seguinte.";
                                        }
                                        else
                                        {
                                            result.eReasonCode = 199;
                                            result.eMessage = "Ocorreu um erro desconhecido ao aprovar a Compras Dinheiro.";
                                        }
                                    }
                                    else
                                    {
                                        result.eReasonCode = 11;
                                        result.eMessage = "Ocorreu um erro na atualização da Compras a Dinheiro.";
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
                                result.eMessage = "A Compras Dinheiro não possui linhas.";
                            }
                        }
                        else if (movementStatus == 2)
                        {
                            //Update Requisiton Data
                            requisition.State = RequisitionStates.Rejected;
                            requisition.ResponsibleApproval = User.Identity.Name;
                            requisition.ApprovalDate = DateTime.Now;
                            requisition.UpdateDate = DateTime.Now;
                            requisition.UpdateUser = User.Identity.Name;
                            //requisition.Comments += rejectionComments;
                            requisition.RejeicaoMotivo = rejectionComments;
                            if (DBRequest.Update(requisition.ParseToDB()) != null)
                            {
                                //Create Workflow
                                var ctx = new SuchDBContext();
                                var logEntry = new RequisicoesRegAlteracoes();
                                logEntry.NºRequisição = requisition.RequisitionNo;
                                logEntry.Estado = (int)RequisitionStates.Rejected; //REJEITADO = 5
                                logEntry.ModificadoEm = DateTime.Now;
                                logEntry.ModificadoPor = User.Identity.Name;
                                ctx.RequisicoesRegAlteracoes.Add(logEntry);
                                ctx.SaveChanges();

                                //Reject Movement
                                ErrorHandler approveResult = ApprovalMovementsManager.RejectMovement(approvalMovement.MovementNo, User.Identity.Name, rejectionComments);

                                //Check Approve Status
                                if (approveResult.eReasonCode == 100)
                                {
                                    result.eReasonCode = 100;
                                    result.eMessage = "A Compras Dinheiro foi rejeitada com sucesso.";
                                }
                                else
                                {
                                    result.eReasonCode = 199;
                                    result.eMessage = "Ocorreu um erro desconhecido ao rejeitar a Compras Dinheiro.";
                                }
                            }
                            else
                            {
                                result.eReasonCode = 11;
                                result.eMessage = "Ocorreu um erro na atualização da Compra a Dinheiro.";
                            }
                        }
                    }
                    else
                    {
                        result.eReasonCode = 200;
                        result.eMessage = "A Compras Dinheiro não existe ou já aprovada.";
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
                        if (movementStatus == 1) //APROVAR
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
                                        int Resultado = 0;
                                        using (var ctx = new SuchDBContextExtention())
                                        {
                                            var parameters = new[]
                                            {
                                                new SqlParameter("@NoFH", FolhaHoras.NºFolhaDeHoras),
                                                new SqlParameter("@NoUtilizador", FolhaHoras.NºEmpregado),
                                                new SqlParameter("@NoValidador", User.Identity.Name)
                                            };
                                            Resultado = ctx.execStoredProcedureFH("exec FH_Validar @NoFH, @NoUtilizador, @NoValidador", parameters);
                                        }

                                        if (Resultado == 0)
                                        {
                                            int Estado = (int)FolhaHoras.Estado;
                                            int NoRegistosAjC = DBLinhasFolhaHoras.GetAll().Where(x => x.NoFolhaHoras.ToLower() == FolhaHoras.NºFolhaDeHoras.ToLower() && x.TipoCusto == 2).Count();
                                            int NoRegistoskm = DBLinhasFolhaHoras.GetAll().Where(x => x.NoFolhaHoras.ToLower() == FolhaHoras.NºFolhaDeHoras.ToLower() && x.TipoCusto == 1).Count();

                                            if ((NoRegistosAjC > 0) || (FolhaHoras.TipoDeslocação == 2 && NoRegistoskm > 0))
                                                Estado = 1; //VALIDADO
                                            else
                                                Estado = 2; // 2 = Registado

                                            FolhaHoras.Estado = Estado;
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
                                                if (Presencas != null && Presencas.Count() > 0)
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

                                                if (FolhaHoras.Estado == 1) //VALIDADO
                                                {
                                                    bool integrarRH = false;

                                                    if (FolhaHoras.IntegradoresEmRh.ToLower().Contains(User.Identity.Name.ToLower()))
                                                    {
                                                        if (NoRegistosAjC > 0)
                                                        {
                                                            using (var ctxRH = new SuchDBContextExtention())
                                                            {
                                                                var parametersRH = new[]
                                                                {
                                                                    new SqlParameter("@NoFH", FolhaHoras.NºFolhaDeHoras),
                                                                    new SqlParameter("@NoUtilizador", FolhaHoras.NºEmpregado),
                                                                    new SqlParameter("@NoValidador", User.Identity.Name)
                                                                };
                                                                result.eReasonCode = ctxRH.execStoredProcedureFH("exec FH_IntegrarEmRH @NoFH, @NoUtilizador, @NoValidador", parametersRH);
                                                            }

                                                            if (result.eReasonCode == 0)
                                                            {
                                                                FolhaHoras.Estado = Estado; //INTEGRAREMRH
                                                                FolhaHoras.IntegradoEmRh = true; //INTEGRAREMRH
                                                                FolhaHoras.IntegradorEmRh = User.Identity.Name; //INTEGRAREMRH
                                                                FolhaHoras.DataIntegraçãoEmRh = DateTime.Now; //INTEGRAREMRH
                                                                FolhaHoras.UtilizadorModificação = User.Identity.Name; //INTEGRAREMRH
                                                                FolhaHoras.DataHoraModificação = DateTime.Now; //INTEGRAREMRH

                                                                if (DBFolhasDeHoras.Update(FolhaHoras) != null)
                                                                {
                                                                    result.eReasonCode = 0;
                                                                    integrarRH = true;
                                                                }
                                                                else
                                                                {
                                                                    result.eReasonCode = 30;
                                                                    result.eMessage = "Ocorreu um erro ao Integrar Ajudas de Custo.";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (result.eReasonCode == 1)
                                                                {
                                                                    result.eReasonCode = 101;
                                                                    result.eMessage = "Não tem permissões para validar.";
                                                                }
                                                                if (result.eReasonCode == 5)
                                                                {
                                                                    result.eReasonCode = 105;
                                                                    result.eMessage = "Não Pode validar pois já se encontra integrada em RH.";
                                                                }
                                                                if (result.eReasonCode == 8)
                                                                {
                                                                    result.eReasonCode = 108;
                                                                    result.eMessage = "A Folha de Horas tem que estar no estado Validado.";
                                                                }
                                                                if (result.eReasonCode == 9)
                                                                {
                                                                    result.eReasonCode = 109;
                                                                    result.eMessage = "Já existe Ajudas de Custo Integradas para o trabalhador nessa data.";
                                                                }
                                                            }
                                                        }
                                                        else
                                                            integrarRH = true;
                                                    }
                                                    else
                                                    {
                                                        if (NoRegistosAjC == 0)
                                                            integrarRH = true;
                                                    }

                                                    if (FolhaHoras.IntegradoresEmRhkm.ToLower().Contains(User.Identity.Name.ToLower()))
                                                    {
                                                        if (FolhaHoras.TipoDeslocação == 2 && NoRegistoskm > 0)
                                                        {
                                                            using (var ctxKM = new SuchDBContextExtention())
                                                            {
                                                                var parametersKM = new[]
                                                                {
                                                                    new SqlParameter("@NoFH", FolhaHoras.NºFolhaDeHoras),
                                                                    new SqlParameter("@NoUtilizador", FolhaHoras.NºEmpregado),
                                                                    new SqlParameter("@NoValidador", User.Identity.Name)
                                                                };
                                                                result.eReasonCode = ctxKM.execStoredProcedureFH("exec FH_IntegrarEmRHKM @NoFH, @NoUtilizador, @NoValidador", parametersKM);
                                                            }

                                                            if (result.eReasonCode == 0)
                                                            {
                                                                if (integrarRH == true)
                                                                    Estado = 2;

                                                                FolhaHoras.Estado = Estado; //INTEGRAREMRHKM
                                                                FolhaHoras.IntegradoEmRhkm = true; //INTEGRAREMRHKM
                                                                FolhaHoras.IntegradorEmRhKm = User.Identity.Name; //INTEGRAREMRHKM
                                                                FolhaHoras.DataIntegraçãoEmRhKm = DateTime.Now; //INTEGRAREMRHKM
                                                                FolhaHoras.UtilizadorModificação = User.Identity.Name; //INTEGRAREMRHKM
                                                                FolhaHoras.DataHoraModificação = DateTime.Now; //INTEGRAREMRHKM

                                                                if (DBFolhasDeHoras.Update(FolhaHoras) != null)
                                                                {
                                                                    result.eReasonCode = 0;
                                                                }
                                                                else
                                                                {
                                                                    result.eReasonCode = 31;
                                                                    result.eMessage = "Ocorreu um erro ao Integrar km.";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (result.eReasonCode == 1)
                                                                {
                                                                    result.eReasonCode = 101;
                                                                    result.eMessage = "Não tem permissões para validar.";
                                                                }
                                                                if (result.eReasonCode == 5)
                                                                {
                                                                    result.eReasonCode = 105;
                                                                    result.eMessage = "Não Pode validar pois já se encontra integrada em RH KM.";
                                                                }
                                                                if (result.eReasonCode == 6)
                                                                {
                                                                    result.eReasonCode = 106;
                                                                    result.eMessage = "Para integrar KMs o campo Tipo de Deslocação tem que ser Viatura Própria.";
                                                                }
                                                                if (result.eReasonCode == 8)
                                                                {
                                                                    result.eReasonCode = 108;
                                                                    result.eMessage = "A Folha de Horas tem que estar no estado Validado.";
                                                                }
                                                                if (result.eReasonCode == 9)
                                                                {
                                                                    result.eReasonCode = 109;
                                                                    result.eMessage = "Já existe Ajudas de Custo Integradas para o trabalhador nessa data.";
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (result.eReasonCode == 0)
                                                            {
                                                                if (integrarRH == true)
                                                                    Estado = 2;

                                                                FolhaHoras.Estado = Estado;
                                                                FolhaHoras.UtilizadorModificação = User.Identity.Name;
                                                                FolhaHoras.DataHoraModificação = DateTime.Now;

                                                                if (DBFolhasDeHoras.Update(FolhaHoras) != null)
                                                                {
                                                                    result.eReasonCode = 0;
                                                                }
                                                                else
                                                                {
                                                                    result.eReasonCode = 31;
                                                                    result.eMessage = "Ocorreu um erro ao Integrar km.";
                                                                }
                                                            }
                                                        }

                                                    }
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

                                                FolhasDeHoras FHFinal = DBFolhasDeHoras.GetById(FolhaHoras.NºFolhaDeHoras);
                                                if (FHFinal.Estado == 1 && FHFinal.TipoDeslocação != 2 && FHFinal.IntegradoEmRh == true)
                                                {
                                                    FHFinal.Estado = 2;
                                                    DBFolhasDeHoras.Update(FHFinal);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            result.eReasonCode = 199;
                                            result.eMessage = "Ocorreu no script SQL de Validação.";

                                            if (Resultado == 1)
                                            {
                                                result.eReasonCode = 101;
                                                result.eMessage = "Não tem permissões para validar.";
                                            }
                                            if (Resultado == 2)
                                            {
                                                result.eReasonCode = 102;
                                                result.eMessage = "O projecto não existe no eSUCH e no Evolution.";
                                            }
                                            if (Resultado == 3)
                                            {
                                                result.eReasonCode = 103;
                                                result.eMessage = "O projecto na Mão de Obra não existe no eSUCH e no Evolution.";
                                            }
                                            if (Resultado == 5)
                                            {
                                                result.eReasonCode = 105;
                                                result.eMessage = "Não Pode validar pois já se encontra validada.";
                                            }
                                            if (Resultado == 6)
                                            {
                                                result.eReasonCode = 106;
                                                result.eMessage = "Já existem movimentos inseridos na tabela Movimentos De Projeto para esta Folha de Horas.";
                                            }
                                            if (Resultado == 7)
                                            {
                                                result.eReasonCode = 107;
                                                result.eMessage = "Já existem movimentos inseridos na tabela Job Ledger Entry para esta Folha de Horas.";
                                            }
                                            if (Resultado == 8)
                                            {
                                                result.eReasonCode = 108;
                                                result.eMessage = "Não foi possivel obter o ID do Empregado da Folha de Horas.";
                                            }
                                            if (Resultado == 9)
                                            {
                                                result.eReasonCode = 109;
                                                result.eMessage = "Não foi possivel obter o código do Projecto/Ordem da Folha de Horas.";
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
                                            int Resultado = 0;
                                            using (var ctx = new SuchDBContextExtention())
                                            {
                                                var parameters = new[]
                                                {
                                                    new SqlParameter("@NoFH", FolhaHoras.NºFolhaDeHoras),
                                                    new SqlParameter("@NoUtilizador", FolhaHoras.NºEmpregado),
                                                    new SqlParameter("@NoValidador", User.Identity.Name)
                                                };
                                                Resultado = ctx.execStoredProcedureFH("exec FH_IntegrarEmRH @NoFH, @NoUtilizador, @NoValidador", parameters);
                                            }

                                            if (Resultado == 0)
                                            {
                                                bool IntegradoEmRhKm = (bool)FolhaHoras.IntegradoEmRhkm;
                                                int TipoDeslocação = (int)FolhaHoras.TipoDeslocação;
                                                int Estado = (int)FolhaHoras.Estado;
                                                int NoRegistoskm = DBLinhasFolhaHoras.GetAll().Where(x => x.NoFolhaHoras.ToLower() == FolhaHoras.NºFolhaDeHoras.ToLower() && x.TipoCusto == 1).Count();

                                                if (IntegradoEmRhKm == true || NoRegistoskm == 0)
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
                                                    if (Estado == 1)
                                                    {
                                                        if (FolhaHoras.IntegradoresEmRhkm.ToLower().Contains(User.Identity.Name.ToLower()))
                                                        {
                                                            if (FolhaHoras.TipoDeslocação == 2 && NoRegistoskm > 0)
                                                            {
                                                                using (var ctx = new SuchDBContextExtention())
                                                                {
                                                                    var parameters = new[]
                                                                    {
                                                                        new SqlParameter("@NoFH", FolhaHoras.NºFolhaDeHoras),
                                                                        new SqlParameter("@NoUtilizador", FolhaHoras.NºEmpregado),
                                                                        new SqlParameter("@NoValidador", User.Identity.Name)
                                                                    };
                                                                    result.eReasonCode = ctx.execStoredProcedureFH("exec FH_IntegrarEmRHKM @NoFH, @NoUtilizador, @NoValidador", parameters);
                                                                }

                                                                if (result.eReasonCode == 0)
                                                                {
                                                                    Estado = 2;

                                                                    FolhaHoras.Estado = Estado; //INTEGRAREMRHKM
                                                                    FolhaHoras.IntegradoEmRhkm = true; //INTEGRAREMRHKM
                                                                    FolhaHoras.IntegradorEmRhKm = User.Identity.Name; //INTEGRAREMRHKM
                                                                    FolhaHoras.DataIntegraçãoEmRhKm = DateTime.Now; //INTEGRAREMRHKM
                                                                    FolhaHoras.UtilizadorModificação = User.Identity.Name; //INTEGRAREMRHKM
                                                                    FolhaHoras.DataHoraModificação = DateTime.Now; //INTEGRAREMRHKM

                                                                    if (DBFolhasDeHoras.Update(FolhaHoras) != null)
                                                                    {
                                                                        result.eReasonCode = 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        result.eReasonCode = 31;
                                                                        result.eMessage = "Ocorreu um erro ao Integrar km.";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (result.eReasonCode == 1)
                                                                    {
                                                                        result.eReasonCode = 101;
                                                                        result.eMessage = "Não tem permissões para validar.";
                                                                    }
                                                                    if (result.eReasonCode == 5)
                                                                    {
                                                                        result.eReasonCode = 105;
                                                                        result.eMessage = "Não Pode validar pois já se encontra integrada em RH KM.";
                                                                    }
                                                                    if (result.eReasonCode == 6)
                                                                    {
                                                                        result.eReasonCode = 106;
                                                                        result.eMessage = "Para integrar KMs o campo Tipo de Deslocação tem que ser Viatura Própria.";
                                                                    }
                                                                    if (result.eReasonCode == 8)
                                                                    {
                                                                        result.eReasonCode = 108;
                                                                        result.eMessage = "A Folha de Horas tem que estar no estado Validado.";
                                                                    }
                                                                    if (result.eReasonCode == 9)
                                                                    {
                                                                        result.eReasonCode = 109;
                                                                        result.eMessage = "Já existe Km's integrados para esta Folha de Hora.";
                                                                    }
                                                                }
                                                            }
                                                        }
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

                                                    FolhasDeHoras FHFinal = DBFolhasDeHoras.GetById(FolhaHoras.NºFolhaDeHoras);
                                                    if (FHFinal.Estado == 1 && FHFinal.TipoDeslocação != 2 && FHFinal.IntegradoEmRh == true)
                                                    {
                                                        FHFinal.Estado = 2;
                                                        DBFolhasDeHoras.Update(FHFinal);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (Resultado == 1)
                                                {
                                                    result.eReasonCode = 101;
                                                    result.eMessage = "Não tem permissões para validar.";
                                                }
                                                if (Resultado == 5)
                                                {
                                                    result.eReasonCode = 105;
                                                    result.eMessage = "Não Pode validar pois já se encontra integrada em RH.";
                                                }
                                                if (Resultado == 8)
                                                {
                                                    result.eReasonCode = 108;
                                                    result.eMessage = "A Folha de Horas tem que estar no estado Validado.";
                                                }
                                                if (result.eReasonCode == 9)
                                                {
                                                    result.eReasonCode = 109;
                                                    result.eMessage = "Já existe Ajudas de Custo Integradas para o trabalhador nessa data.";
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
                                            int Resultado = 0;
                                            using (var ctx = new SuchDBContextExtention())
                                            {
                                                var parameters = new[]
                                                {
                                                    new SqlParameter("@NoFH", FolhaHoras.NºFolhaDeHoras),
                                                    new SqlParameter("@NoUtilizador", FolhaHoras.NºEmpregado),
                                                    new SqlParameter("@NoValidador", User.Identity.Name)
                                                };
                                                Resultado = ctx.execStoredProcedureFH("exec FH_IntegrarEmRHKM @NoFH, @NoUtilizador, @NoValidador", parameters);
                                            }

                                            if (Resultado == 0)
                                            {
                                                bool IntegradoEmRh = (bool)FolhaHoras.IntegradoEmRh;
                                                int NoRegistos = 0;
                                                int Estado = (int)FolhaHoras.Estado;

                                                NoRegistos = DBLinhasFolhaHoras.GetAll().Where(x => x.NoFolhaHoras.ToLower() == FolhaHoras.NºFolhaDeHoras.ToLower() && x.TipoCusto == 2).Count();

                                                if (IntegradoEmRh == true || NoRegistos == 0)
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
                                                    if (Estado == 1)
                                                    {
                                                        if (result.eReasonCode == 0 && FolhaHoras.IntegradoresEmRh.ToLower().Contains(User.Identity.Name.ToLower()))
                                                        {
                                                            if (NoRegistos > 0)
                                                            {
                                                                using (var ctxRH = new SuchDBContextExtention())
                                                                {
                                                                    var parametersRH = new[]
                                                                    {
                                                                        new SqlParameter("@NoFH", FolhaHoras.NºFolhaDeHoras),
                                                                        new SqlParameter("@NoUtilizador", FolhaHoras.NºEmpregado),
                                                                        new SqlParameter("@NoValidador", User.Identity.Name)
                                                                    };
                                                                    result.eReasonCode = ctxRH.execStoredProcedureFH("exec FH_IntegrarEmRH @NoFH, @NoUtilizador, @NoValidador", parametersRH);
                                                                }

                                                                if (result.eReasonCode == 0)
                                                                {
                                                                    Estado = 2; //REGISTADO

                                                                    FolhaHoras.Estado = Estado; //INTEGRAREMRH
                                                                    FolhaHoras.IntegradoEmRh = true; //INTEGRAREMRH
                                                                    FolhaHoras.IntegradorEmRh = User.Identity.Name; //INTEGRAREMRH
                                                                    FolhaHoras.DataIntegraçãoEmRh = DateTime.Now; //INTEGRAREMRH
                                                                    FolhaHoras.UtilizadorModificação = User.Identity.Name; //INTEGRAREMRH
                                                                    FolhaHoras.DataHoraModificação = DateTime.Now; //INTEGRAREMRH

                                                                    if (DBFolhasDeHoras.Update(FolhaHoras) != null)
                                                                    {
                                                                        result.eReasonCode = 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        result.eReasonCode = 30;
                                                                        result.eMessage = "Ocorreu um erro ao Integrar Ajudas de Custo.";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (result.eReasonCode == 1)
                                                                    {
                                                                        result.eReasonCode = 101;
                                                                        result.eMessage = "Não tem permissões para validar.";
                                                                    }
                                                                    if (result.eReasonCode == 5)
                                                                    {
                                                                        result.eReasonCode = 105;
                                                                        result.eMessage = "Não Pode validar pois já se encontra integrada em RH.";
                                                                    }
                                                                    if (result.eReasonCode == 8)
                                                                    {
                                                                        result.eReasonCode = 108;
                                                                        result.eMessage = "A Folha de Horas tem que estar no estado Validado.";
                                                                    }
                                                                    if (result.eReasonCode == 9)
                                                                    {
                                                                        result.eReasonCode = 109;
                                                                        result.eMessage = "Já existe Ajudas de Custo Integradas para o trabalhador nessa data.";
                                                                    }

                                                                }
                                                            }
                                                        }
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
                                                if (Resultado == 1)
                                                {
                                                    result.eReasonCode = 101;
                                                    result.eMessage = "Não tem permissões para validar.";
                                                }
                                                if (Resultado == 5)
                                                {
                                                    result.eReasonCode = 105;
                                                    result.eMessage = "Não Pode validar pois já se encontra integrada em RH KM.";
                                                }
                                                if (Resultado == 6)
                                                {
                                                    result.eReasonCode = 106;
                                                    result.eMessage = "Para integrar KMs o campo Tipo de Deslocação tem que ser Viatura Própria.";
                                                }
                                                if (Resultado == 8)
                                                {
                                                    result.eReasonCode = 108;
                                                    result.eMessage = "A Folha de Horas tem que estar no estado Validado.";
                                                }
                                                if (Resultado == 9)
                                                {
                                                    result.eReasonCode = 109;
                                                    result.eMessage = "Já existe Km's integrados para esta Folha de Hora.";
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
                else if (approvalMovement.Type == 5) //PROJETOS
                {
                    if (movementStatus == 1) //APROVAR
                    {
                        string EmployeeNo = DBUserConfigurations.GetById(User.Identity.Name).EmployeeNo;
                        if (!string.IsNullOrEmpty(EmployeeNo))
                        {
                            ProjectDetailsViewModel data = DBProjects.GetById(approvalMovement.Number).ParseToViewModel();

                            data.Status = (EstadoProjecto)1; //ENCOMENDA
                            data.ProjectResponsible = EmployeeNo;
                            data.UpdateDate = DateTime.Now;
                            data.UpdateUser = User.Identity.Name;
                            data.Visivel = true;

                            NAVClientsViewModel client = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, data.ClientNo);
                            if (client != null && (client.Blocked == WSCustomerNAV.Blocked.Invoice || client.Blocked == WSCustomerNAV.Blocked.All))
                            {
                                result.eReasonCode = 503;
                                result.eMessage = "Não é possivel aprovar o Projeto, por o Cliente Nº " + data.ClientNo + "estar bloqueado no NAV2017.";
                            }
                            else
                            {
                                Task<WSCreateNAVProject.Create_Result> TCreateNavProj = WSProject.CreateNavProject(data, configws);
                                try
                                {
                                    TCreateNavProj.Wait();
                                }
                                catch (Exception ex)
                                {
                                    result.eReasonCode = 503;
                                    result.eMessage = "Ocorreu um erro ao criar o projeto no NAV.";
                                }
                                if (!TCreateNavProj.IsCompletedSuccessfully)
                                {
                                    result.eReasonCode = 503;
                                    result.eMessage = "Ocorreu um erro ao criar o projeto no NAV.";

                                    if (TCreateNavProj.Exception != null)
                                        result.eMessages.Add(new TraceInformation(TraceType.Exception, TCreateNavProj.Exception.Message));

                                    if (TCreateNavProj.Exception.InnerException != null)
                                        result.eMessages.Add(new TraceInformation(TraceType.Exception, TCreateNavProj.Exception.InnerException.ToString()));
                                }
                                else
                                {
                                    if (DBProjects.Update(DBProjects.ParseToDB(data)) != null)
                                    {
                                        //Update Old Movement
                                        ApprovalMovementsViewModel ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetById(movementNo));
                                        ApprovalMovement.Status = 2;
                                        ApprovalMovement.DateTimeApprove = DateTime.Now;
                                        ApprovalMovement.DateTimeUpdate = DateTime.Now;
                                        ApprovalMovement.UserUpdate = User.Identity.Name;
                                        ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.Update(DBApprovalMovements.ParseToDatabase(ApprovalMovement)));

                                        //Delete All User Approval Movements
                                        if (DBUserApprovalMovements.DeleteFromMovementExcept(ApprovalMovement.MovementNo, User.Identity.Name) == true)
                                        {
                                            result.eReasonCode = 100;
                                            result.eMessage = "O Projeto foi aprovado com sucesso.";
                                        }
                                        else
                                        {
                                            result.eReasonCode = 504;
                                            result.eMessage = "Ocorreu um erro ao apagar os Movimentos de Aprovação.";
                                        }
                                    }
                                    else
                                    {
                                        result.eReasonCode = 505;
                                        result.eMessage = "Ocorreu um erro ao atualizar o movimento de aprovação.";
                                    }
                                }
                            }
                        }
                        else
                        {
                            result.eReasonCode = 506;
                            result.eMessage = "Não pode aprovar o movimento pois não têm atribuído um número mecanográfico.";
                        }
                    }
                    else if (movementStatus == 2) //REJEITAR
                    {
                        //Reject Movement
                        ErrorHandler approveResult = ApprovalMovementsManager.RejectMovement_Projeto(approvalMovement.MovementNo, User.Identity.Name, rejectionComments);

                        //Check Approve Status
                        if (approveResult.eReasonCode == 100)
                        {
                            result.eReasonCode = 100;
                            result.eMessage = "O Movimento de Aprovação foi rejeitado com sucesso.";
                        }
                        else
                        {
                            result.eReasonCode = 599;
                            result.eMessage = "Ocorreu um erro desconhecido ao rejeitar o Movimento de Aprovação.";
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

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_Approvals([FromBody] List<ApprovalMovementsViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Aprovacoes\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Pedidos de Aprovação");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["movementNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº");
                    Col = Col + 1;
                }
                if (dp["typeText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo");
                    Col = Col + 1;
                }
                if (dp["number"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Associado");
                    Col = Col + 1;
                }
                if (dp["fhDatePartida"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("FH Dia da Partida");
                    Col = Col + 1;
                }
                if (dp["fhDateChegada"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("FH Dia da Chegada");
                    Col = Col + 1;
                }
                if (dp["requisicaoProjectNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Ordem/Projeto");
                    Col = Col + 1;
                }
                if (dp["requisicaoClientNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Req Cliente Nº");
                    Col = Col + 1;
                }
                if (dp["requisicaoClientName"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Req Cliente Nome");
                    Col = Col + 1;
                }
                if (dp["requisicaoAcordosPrecos"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Req Acordos Preços");
                    Col = Col + 1;
                }
                if (dp["requisicaoUrgente"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Req Urgente");
                    Col = Col + 1;
                }
                if (dp["requisicaoOrcamentoEmAnexo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Req Oçamento Em Anexo");
                    Col = Col + 1;
                }
                if (dp["requisicaoImobilizado"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Req Imobilizado");
                    Col = Col + 1;
                }
                if (dp["requisicaoExclusivo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Req Exclusivo");
                    Col = Col + 1;
                }
                if (dp["requisicaoJaExecutado"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Req Já Executado");
                    Col = Col + 1;
                }
                if (dp["requisicaoAmostra"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Req Amostra");
                    Col = Col + 1;
                }
                if (dp["requisicaoEquipamento"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Req Equipamento");
                    Col = Col + 1;
                }
                if (dp["requisicaoReposicaoDeStock"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Req Reposição De Stock");
                    Col = Col + 1;
                }
                if (dp["requisicaoPrecoIvaIncluido"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Req Preço IVA Incluído");
                    Col = Col + 1;
                }
                if (dp["requisicaoAdiantamento"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Req Adiantamento");
                    Col = Col + 1;
                }
                if (dp["requisicaoPedirOrcamento"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Req Pedir Orçamento");
                    Col = Col + 1;
                }
                if (dp["requisicaoRoupaManutencao"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Req Roupa Manutenção");
                    Col = Col + 1;
                }
                if (dp["requestUser"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("CSolicitado Por");
                    Col = Col + 1;
                }
                if (dp["value"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Valor");
                    Col = Col + 1;
                }
                if (dp["statusText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Estado");
                    Col = Col + 1;
                }
                if (dp["level"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nivel");
                    Col = Col + 1;
                }
                if (dp["region"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Região");
                    Col = Col + 1;
                }
                if (dp["responsabilityCenter"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Cresp");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ApprovalMovementsViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["movementNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.MovementNo);
                            Col = Col + 1;
                        }
                        if (dp["typeText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TypeText);
                            Col = Col + 1;
                        }
                        if (dp["number"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Number);
                            Col = Col + 1;
                        }
                        if (dp["fhDatePartida"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.FHDatePartida);
                            Col = Col + 1;
                        }
                        if (dp["fhDateChegada"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.FHDateChegada);
                            Col = Col + 1;
                        }
                        if (dp["requisicaoProjectNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisicaoProjectNo);
                            Col = Col + 1;
                        }
                        if (dp["requisicaoClientNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisicaoClientNo);
                            Col = Col + 1;
                        }
                        if (dp["requisicaoClientName"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisicaoClientName);
                            Col = Col + 1;
                        }
                        if (dp["requisicaoAcordosPrecos"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisicaoAcordosPrecos);
                            Col = Col + 1;
                        }
                        if (dp["requisicaoUrgente"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisicaoUrgente);
                            Col = Col + 1;
                        }
                        if (dp["requisicaoOrcamentoEmAnexo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisicaoOrcamentoEmAnexo);
                            Col = Col + 1;
                        }
                        if (dp["requisicaoImobilizado"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisicaoImobilizado);
                            Col = Col + 1;
                        }
                        if (dp["requisicaoExclusivo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisicaoExclusivo);
                            Col = Col + 1;
                        }
                        if (dp["requisicaoJaExecutado"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisicaoJaExecutado);
                            Col = Col + 1;
                        }
                        if (dp["requisicaoAmostra"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisicaoAmostra);
                            Col = Col + 1;
                        }
                        if (dp["requisicaoEquipamento"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisicaoEquipamento);
                            Col = Col + 1;
                        }
                        if (dp["requisicaoReposicaoDeStock"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisicaoReposicaoDeStock);
                            Col = Col + 1;
                        }
                        if (dp["requisicaoPrecoIvaIncluido"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisicaoPrecoIvaIncluido);
                            Col = Col + 1;
                        }
                        if (dp["requisicaoAdiantamento"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisicaoAdiantamento);
                            Col = Col + 1;
                        }
                        if (dp["requisicaoPedirOrcamento"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisicaoPedirOrcamento);
                            Col = Col + 1;
                        }
                        if (dp["requisicaoRoupaManutencao"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequisicaoRoupaManutencao);
                            Col = Col + 1;
                        }
                        if (dp["requestUser"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequestUser);
                            Col = Col + 1;
                        }
                        if (dp["value"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Value.ToString());
                            Col = Col + 1;
                        }
                        if (dp["statusText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.StatusText);
                            Col = Col + 1;
                        }
                        if (dp["level"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Level);
                            Col = Col + 1;
                        }
                        if (dp["region"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Region);
                            Col = Col + 1;
                        }
                        if (dp["responsabilityCenter"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ResponsabilityCenter);
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
        public IActionResult ExportToExcelDownload_Approvals(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Aprovacoes\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Pedidos de Aprovação.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

    }
}