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
        protected int RequestOrigin { get; set; }
        protected int TrainingRequestApprovalType = EnumerablesFixed.ApprovalTypes.Where(e => e.Value.Contains("Pedido Formação")).FirstOrDefault().Id;
        

        [HttpPost]
        public JsonResult GetMeusPedidos([FromBody] JObject requestParams)
        {
            //ClickSource = 0;
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
            ConfiguracaoAprovacaoUtilizador cfgUser = new ConfiguracaoAprovacaoUtilizador(User.Identity.Name, userConfig.TipoUtilizadorFormacao.Value, TrainingRequestApprovalType);
            List<Formando> formandos = DBAcademia.__GetAllFormandos(cfgUser, Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuChefia);

            Formando formando = DBAcademia.__GetDetailsFormando("27960");


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

        [HttpPost]
        public JsonResult GetMeusPedidosAprovacao([FromBody] JObject requestParams)
        {
            bool apenasCompletos = requestParams["apenasCompletos"] == null ? false : (bool)requestParams["apenasCompletos"];
            try
            {
                RequestOrigin = requestParams["requestOrigin"] == null ? 0 : (int)requestParams["requestOrigin"];
            }
            catch (Exception)
            {
                return Json(-1);
            }            

            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
            ConfiguracaoAprovacaoUtilizador cfgUser = new ConfiguracaoAprovacaoUtilizador(User.Identity.Name, userConfig.TipoUtilizadorFormacao.Value, TrainingRequestApprovalType);
            List<PedidoParticipacaoFormacaoView> meusPedidosView = new List<PedidoParticipacaoFormacaoView>();

            switch (RequestOrigin)
            {
                case (int)Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuChefia:
                    if(UPerm != null && UPerm.Read.Value && cfgUser.IsChief())
                    {
                        List<PedidoParticipacaoFormacao> meusPedidos = DBAcademia.__GetAllPedidosFormacao(cfgUser, Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuChefia, apenasCompletos);
                        if (meusPedidos != null && meusPedidos.Count > 0)
                        {
                            foreach (var p in meusPedidos)
                            {
                                meusPedidosView.Add(new PedidoParticipacaoFormacaoView(p));
                            }
                        }
                    }
                    break;
                case (int)Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuDirector:
                    if(UPerm != null && UPerm.Read.Value && cfgUser.IsDirector())
                    {
                        List<PedidoParticipacaoFormacao> meusPedidos = DBAcademia.__GetAllPedidosFormacao(cfgUser, Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuDirector, apenasCompletos);
                        if (meusPedidos != null && meusPedidos.Count > 0)
                        {
                            foreach (var p in meusPedidos)
                            {
                                meusPedidosView.Add(new PedidoParticipacaoFormacaoView(p));
                            }

                            return Json(meusPedidosView);
                        }
                    }
                    break;
            }

            return Json(meusPedidosView);
        }

        [HttpPost]
        public JsonResult GetCatalogo()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);

            if (UPerm != null && userConfig.TipoUtilizadorFormacao == (int)Enumerations.TipoUtilizadorFluxoPedidoFormacao.GestorFormacao)
            {
                List<TemaFormacao> temas = DBAcademia.__GetCatalogo();
                return Json(temas);
            }
            return Json(null);
        }
                

        #region SGPPF actions
        public ActionResult MeusPedidos()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
            

            if (UPerm != null && UPerm.Read.Value)
            {
                if(userConfig.TipoUtilizadorFormacao == null)
                {                    
                    // os utilizadores deverão ter um tipo de utilizador definido. 
                    // O tipo por defeito deverá ser o de Formando (1-Formando)
                    return RedirectToAction("AccessDenied", "Error");
                }

                ViewBag.OnlyActive = true;
                ViewBag.RequestOrigin = (int)Enumerations.AcademiaOrigemAcessoFuncionalidade.MeusPedidos;
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
            ConfiguracaoAprovacaoUtilizador cfgUser = new ConfiguracaoAprovacaoUtilizador(User.Identity.Name, userConfig.TipoUtilizadorFormacao.Value, TrainingRequestApprovalType);

            if (UPerm != null && UPerm.Read.Value && cfgUser.IsChief())
            {
                ViewBag.OnlyCompleted = true;
                ViewBag.RequestOrigin = (int)Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuChefia;
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

            ConfiguracaoAprovacaoUtilizador cfgUser = new ConfiguracaoAprovacaoUtilizador(User.Identity.Name, userConfig.TipoUtilizadorFormacao.Value, TrainingRequestApprovalType);

            bool isDirector = cfgUser.IsDirector();

            if (UPerm != null && UPerm.Read.Value && isDirector)
            {
                ViewBag.OnlyCompleted = false;
                ViewBag.RequestOrigin = (int)Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuDirector;
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
                ViewBag.OnlyCompleted = false;
                ViewBag.RequestOrigin = (int)Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuCA;
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
                ViewBag.OnlyCompleted = false;
                ViewBag.RequestOrigin = (int)Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuGestao;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public ActionResult GestaoCatalogo()
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

        public ActionResult DetalhesTema(string id, string codInterno)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);

            if (UPerm != null && UPerm.Read.Value &&
                userConfig.TipoUtilizadorFormacao.Value == (int)Enumerations.TipoUtilizadorFluxoPedidoFormacao.GestorFormacao)
            {
                ViewBag.idTema = id;
                ViewBag.codInterno = codInterno; 
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