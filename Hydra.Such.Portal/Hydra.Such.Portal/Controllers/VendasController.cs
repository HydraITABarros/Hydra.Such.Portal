using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Contracts;
using Hydra.Such.Data;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class VendasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region Contratos
        public IActionResult Contratos(int? archived, string contractNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Vendas, Enumerations.Features.Contratos); //4, 2);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Archived = archived == null ? 0 : 1;
                ViewBag.ContractNo = contractNo ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult DetalhesContrato(string id, string version = "")
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Vendas, Enumerations.Features.Contratos); //4, 2);
            if (UPerm != null && UPerm.Read.Value)
            {
                Contratos cContract = null;
                if (version != "")
                    cContract = DBContracts.GetByIdAndVersion(id, int.Parse(version));
                else
                    cContract = DBContracts.GetByIdLastVersion(id);

                if (cContract != null && cContract.Arquivado == true)
                {
                    UPerm.Update = false;
                }
                ViewBag.ContractNo = id ?? "";
                ViewBag.VersionNo = version ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        #endregion

        #region Oportunidades
        public IActionResult Oportunidades(int? archived, string contractNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Vendas, Enumerations.Features.Oportunidades); //4, 20);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Archived = archived == null ? 0 : 1;
                ViewBag.ContractNo = contractNo ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult DetalhesOportunidade(string id, string version)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Vendas, Enumerations.Features.Oportunidades); //4, 20);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ContractNo = id ?? "";
                ViewBag.VersionNo = version ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        #endregion

        #region Propostas
        public IActionResult Propostas(int? archived, string contractNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Vendas, Enumerations.Features.Propostas); //4, 20);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Archived = archived == null ? 0 : 1;
                ViewBag.ContractNo = contractNo ?? "";
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult DetalhesProposta(string id, string version)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Vendas, Enumerations.Features.Propostas); //4, 20);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ContractNo = id ?? "";
                ViewBag.VersionNo = version ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        #endregion

        #region Projetos
        public IActionResult Projetos()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Vendas, Enumerations.Features.Projetos); //4, 1);
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

        public IActionResult DetalhesProjeto(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Vendas, Enumerations.Features.Projetos); //4, 1);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ProjectNo = id == null ? "" : id;
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult ProjetosContrato(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Vendas, Enumerations.Features.Projetos); //4, 1);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.ContractId = string.IsNullOrEmpty(id) ? string.Empty : id;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        #endregion

        #region DiárioProjetos
        public IActionResult DiarioProjeto(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Vendas, Enumerations.Features.FolhasHoras); //4, 19);
            if (UPerm != null && UPerm.Read.Value)
            {
                // UPerm.Update = false;

                ViewBag.ProjectNo = id ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult AutorizacaoFaturacao(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Vendas, Enumerations.Features.AutorizaçãoFaturação); //4, 22);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ProjectNo = id ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        #endregion

        #region Clientes
        public IActionResult Clientes()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Vendas, Enumerations.Features.Clientes); //4, 47);
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

        public IActionResult DetalhesCliente(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Vendas, Enumerations.Features.Clientes); //4, 47);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.No = id ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        #endregion


        public IActionResult Requisicoes()
        {
            return View();
        }

        public IActionResult Administracao()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Vendas, Enumerations.Features.Administração); //4, 18);
            if (UPerm != null && UPerm.Read.Value)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        #region Pré-Requisições

        public IActionResult PreRequisicoesLista()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Vendas, Enumerations.Features.PréRequisições); //4, 3);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Area = 4;
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult PreRequisicoesDetalhes(string PreRequesitionNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Vendas, Enumerations.Features.PréRequisições); //4, 3);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Area = 4;
                ViewBag.PreRequesitionNo = User.Identity.Name;
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        #endregion

        #region Pending Requesitions
        public IActionResult RequisicoesPendentes()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, (int)Enumerations.Areas.Vendas, 0);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Area = 4;
                ViewBag.UPermissions = UPerm.Read;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        #endregion

        #region history Requesitions
        public IActionResult RequisicoesHistorico()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, (int)Enumerations.Areas.Vendas, 0);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Area = 4;
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        #endregion
    }
}