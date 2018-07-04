using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data;
using static Hydra.Such.Data.Enumerations;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.NAV;
using Microsoft.Extensions.Options;
using Hydra.Such.Portal.Services;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class FaturacaoController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private BillingReceptionService billingRecService;

        public FaturacaoController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            billingRecService = new BillingReceptionService();
        }

        public IActionResult RececaoFaturas()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ReceçãoFaturação);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UserPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult DetalhesRecFatura(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ReceçãoFaturação);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Id = id;
                ViewBag.UserPermissions = UPerm;
                ViewBag.BillingReceptionStates = EnumHelper.GetItemsAsDictionary(typeof(BillingReceptionStates));
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public JsonResult GetBillingReceptions()
        {
            var billingReceptions = billingRecService.GetAllForUser(User.Identity.Name);
            return Json(billingReceptions);
        }

        [HttpGet]
        public JsonResult GetBillingReception(string id)
        {
            var billingReception = billingRecService.GetById(id);
            return Json(billingReception);
        }

        [HttpGet]
        public JsonResult GetQuestions()
        {
            List<DDMessageString> result = billingRecService.GetQuestions().Select(x => new DDMessageString()
            {
                id = x.Tipo,
                value = x.Descricao
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateBillingReception([FromBody] BillingReceptionModel item)
        {
            BillingReceptionModel createdItem = null;
            if (item != null)
            {
                item.CriadoPor = User.Identity.Name;
                createdItem = billingRecService.Create(item);
                if (createdItem != null)
                {
                    createdItem.eReasonCode = 1;
                    createdItem.eMessage = "Registo criado com sucesso";
                    item = createdItem;
                }
                else
                {
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao criar o registo";
                }
            }
            else
            {
                item.eReasonCode = 2;
                item.eMessage = "O registo não pode ser nulo";
            }
            return Json(item);
        }

        [HttpPost]
        public JsonResult UpdateBillingReception([FromBody] BillingReceptionModel item)
        {
            BillingReceptionModel updatedItem = null;
            if (item != null)
            {
                item.ModificadoPor = User.Identity.Name;
                updatedItem = billingRecService.Update(item);
                if (updatedItem != null)
                {
                    updatedItem.eReasonCode = 1;
                    updatedItem.eMessage = "Registo atualizado com sucesso";
                    item = updatedItem;
                }
            }
            else
            {
                item.eReasonCode = 2;
                item.eMessage = "O registo não pode ser nulo";
            }
            return Json(updatedItem);
        }

        [HttpPost]
        public JsonResult PostDocument([FromBody] BillingReceptionModel item)
        {
            if (item != null)
            {
                try
                {
                    var postedDocument = billingRecService.PostDocument(item, User.Identity.Name, _config, _configws);
                    item = postedDocument;
                }
                catch
                {
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao criar o documento.";
                }
            }
            else
            {
                item.eReasonCode = 2;
                item.eMessage = "O registo não pode ser nulo";
            }
            return Json(item);
        }

        [HttpPost]
        public JsonResult DocumentIsDigitized([FromBody] BillingReceptionModel item)
        {
            //if (item != null)
            //{
                
            //}
            //else
            //{
            //    item.eReasonCode = 2;
            //    item.eMessage = "O registo não pode ser nulo";
            //}
            return Json(false);
        }
    }
}