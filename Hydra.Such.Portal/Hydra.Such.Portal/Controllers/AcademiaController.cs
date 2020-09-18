using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

using Hydra.Such.Data.Extensions;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.ViewModel.Academia;

namespace Hydra.Such.Portal.Controllers
{
    public class AcademiaController : Controller
    {
        private int ClickSource
        {
            get
            {
                return ClickSource;
            }

            set
            {
                ClickSource = value;
            }
        }

        

        [HttpGet]
        public JsonResult GetMeusPedidos([FromBody] JObject requestParams)
        {
            ClickSource = 0;

            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            if(UPerm != null && UPerm.Read.Value)
            {
                bool apenasActivos = requestParams["apenasActivos"] == null ? false : (bool)requestParams["apenasActivos"];
                List<PedidoParticipacaoFormacao> meusPedidos = DBAcademia.__GetAllPedidosFormacao(User.Identity.Name, apenasActivos);

                List<PedidoParticipacaoFormacaoView> meusPedidosView = new List<PedidoParticipacaoFormacaoView>();

                foreach(var p in meusPedidos)
                {
                    meusPedidosView.Add(new PedidoParticipacaoFormacaoView(p));
                }

                return Json(meusPedidosView);
            }
            return Json(null);
        }

        
        #region SGPPF actions
        public ActionResult MeusPedidos()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            if (UPerm != null && UPerm.Read.Value)
            {
                ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
                if(userConfig.TipoUtilizadorFormacao == null)
                {
                    // os utilizadores deverão ter um tipo de utilizador definido. 
                    // O tipo por defeito deverá ser o de Formando (1-Formando)
                    return RedirectToAction("AccessDenied", "Error");
                }

                ViewBag.OnlyActive = true;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public ActionResult AprovacaoChefia()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
            if (UPerm != null && UPerm.Read.Value &&
                userConfig.TipoUtilizadorFormacao.Value == (int)Enumerations.TipoUtilizadorFluxoPedidoFormacao.AprovadorChefia)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public ActionResult AprovacaoDireccao()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
            if (UPerm != null && UPerm.Read.Value &&
                userConfig.TipoUtilizadorFormacao.Value == (int)Enumerations.TipoUtilizadorFluxoPedidoFormacao.AprovadorDireccao)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public ActionResult AutorizacaoCA()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);

            if (UPerm != null && UPerm.Read.Value &&
                userConfig.TipoUtilizadorFormacao.Value == (int)Enumerations.TipoUtilizadorFluxoPedidoFormacao.ConselhoAdministracao)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public ActionResult GestaoPedidos()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);

            if (UPerm != null && UPerm.Read.Value &&
                userConfig.TipoUtilizadorFormacao.Value == (int)Enumerations.TipoUtilizadorFluxoPedidoFormacao.GestorFormacao)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        #endregion

        #region Default actions
        //// GET: Academia
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: Academia/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Academia/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Academia/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Academia/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Academia/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Academia/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Academia/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        #endregion

    }
}