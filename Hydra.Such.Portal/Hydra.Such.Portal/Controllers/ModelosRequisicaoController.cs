using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Portal.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Portal.Controllers
{
    public class ModelosRequisicaoController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;

        public ModelosRequisicaoController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
        }

        
        public IActionResult Index()
        {
            UserAccessesViewModel userPermissions =
                DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ModelosRequisicao);
            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UserPermissions = userPermissions;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        
        public IActionResult Detalhes(string id)
        {
            UserAccessesViewModel userPermissions =
                DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ModelosRequisicao);
            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UserPermissions = userPermissions;
                ViewBag.RequisitionId = id;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        [HttpPost]
        
        public JsonResult GetRequisitionTemplates()
        {
            List<RequisitionTemplateViewModel> result = DBRequestTemplates.GetAll().ParseToTemplateViewModel();

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.RegionCode));
            //FunctionalAreas
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.FunctionalAreaCode));
            //ResponsabilityCenter
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CenterResponsibilityCode));
            return Json(result);
        }

        [HttpPost]
        
        public JsonResult GetRequisition([FromBody] string id)
        {
            RequisitionTemplateViewModel item;
            if (!string.IsNullOrEmpty(id) && id != "0")
            {
                item = DBRequestTemplates.GetById(id).ParseToTemplateViewModel();
            }
            else
                item = new RequisitionTemplateViewModel();

            return Json(item);
        }

        [HttpPost]
        
        public JsonResult CreateRequisition([FromBody] RequisitionTemplateViewModel item)
        {
            if (item != null)
            {
                //Get Numeration
                bool autoGenId = false;
                Configuração conf = DBConfigurations.GetById(1);
                int entityNumerationConfId = conf.NumeracaoModelosRequisicao.Value;

                if (item.RequisitionNo == "" || item.RequisitionNo == null)
                {
                    autoGenId = true;
                    item.RequisitionNo = DBNumerationConfigurations.GetNextNumeration(entityNumerationConfId, autoGenId);
                }
                if (item.RequisitionNo != null)
                {
                    item.CreateUser = User.Identity.Name;
                    var createdItem = DBRequestTemplates.Create(item.ParseToDB());
                    if (createdItem != null)
                    {
                        item = createdItem.ParseToTemplateViewModel();
                        if (autoGenId)
                        {
                            ConfiguraçãoNumerações configNum = DBNumerationConfigurations.GetById(entityNumerationConfId);
                            configNum.ÚltimoNºUsado = item.RequisitionNo;
                            configNum.UtilizadorModificação = User.Identity.Name;
                            DBNumerationConfigurations.Update(configNum);
                        }
                        item.eReasonCode = 1;
                        item.eMessage = "Registo criado com sucesso.";
                    }
                    else
                    {
                        item = new RequisitionTemplateViewModel();
                        item.eReasonCode = 2;
                        item.eMessage = "Ocorreu um erro ao criar o registo.";
                    }
                }
                else
                {
                    item.eReasonCode = 5;
                    item.eMessage = "A numeração configurada não é compativel com a inserida.";
                }
            }
            else
            {
                item = new RequisitionTemplateViewModel();
                item.eReasonCode = 2;
                item.eMessage = "Ocorreu um erro: o modelo de requisição não pode ser nulo.";
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult UpdateRequisition([FromBody] RequisitionTemplateViewModel item)
        {
            if (item != null)
            {
                item.CreateUser = User.Identity.Name;
                var updatedItem = DBRequestTemplates.Update(item.ParseToDB());
                if (updatedItem != null)
                {
                    item = updatedItem.ParseToTemplateViewModel();
                    item.eReasonCode = 1;
                    item.eMessage = "Registo atualizado com sucesso.";
                }
                else
                {
                    item = new RequisitionTemplateViewModel();
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao atualizar o registo.";
                }
            }
            else
            {
                item = new RequisitionTemplateViewModel();
                item.eReasonCode = 2;
                item.eMessage = "Ocorreu um erro: o modelo de requisição não pode ser nulo.";
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult DeleteRequisition([FromBody] RequisitionTemplateViewModel item)
        {
            if (item != null)
            {
                if (DBRequestTemplates.Delete(item.ParseToDB()))
                {
                    item.eReasonCode = 1;
                    item.eMessage = "Registo eliminado com sucesso.";
                }
                else
                {
                    item = new RequisitionTemplateViewModel();
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao eliminar o registo.";
                }
            }
            else
            {
                item = new RequisitionTemplateViewModel();
                item.eReasonCode = 2;
                item.eMessage = "Ocorreu um erro: o modelo de requisição não pode ser nulo.";
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult ValidateNumeration([FromBody] RequisitionTemplateViewModel item)
        {
            //Get Project Numeration
            Configuração conf = DBConfigurations.GetById(1);
            if (conf != null)
            {
                int numRequisitionTemplate = conf.NumeracaoModelosRequisicao.Value;

                ConfiguraçãoNumerações numConf = DBNumerationConfigurations.GetById(numRequisitionTemplate);

                //Validate if id is valid
                if (!(item.RequisitionNo == "" || item.RequisitionNo == null) && !numConf.Manual.Value)
                {
                    return Json("A numeração configurada para os modelos de requisição não permite inserção manual.");
                }
                else if (item.RequisitionNo == "" && !numConf.Automático.Value)
                {
                    return Json("É obrigatório inserir o Nº Modelo.");
                }
            }
            else
            {
                return Json("Não foi possivel obter as configurações base de numeração.");
            }
            return Json("");
        }

        public JsonResult GetPlaces([FromBody] int placeId)
        {
            PlacesViewModel PlacesData = DBPlaces.ParseToViewModel(DBPlaces.GetById(placeId));
            return Json(PlacesData);
        }

        [HttpPost]
        
        public JsonResult CreateRequisitionLine([FromBody] RequisitionTemplateLineViewModel item)
        {
            if (item != null)
            {
                item.CreateUser = User.Identity.Name;
                var createdItem = DBRequestTemplateLines.Create(item.ParseToDB());
                if (createdItem != null)
                {
                    item = createdItem.ParseToTemplateViewModel();
                    item.eReasonCode = 1;
                    item.eMessage = "Registo criado com sucesso.";
                }
                else
                {
                    item = new RequisitionTemplateLineViewModel();
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao criar o registo.";
                }
            }
            else
            {
                item = new RequisitionTemplateLineViewModel();
                item.eReasonCode = 2;
                item.eMessage = "Ocorreu um erro: a linha não pode ser nula.";
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult UpdateRequisitionLines([FromBody] RequisitionTemplateViewModel item)
        {
            try
            {
                if (item != null && item.Lines != null)
                {
                    if (DBRequestTemplateLines.Update(item.Lines.ParseToDB()))
                    {
                        item.Lines.ForEach(x => x.Selected = false);
                        item.eReasonCode = 1;
                        item.eMessage = "Linhas atualizadas com sucesso.";
                        return Json(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //item.eReasonCode = 2;
                //item.eMessage = "Ocorreu um erro ao atualizar as linhas.";
            }
            item.eReasonCode = 2;
            item.eMessage = "Ocorreu um erro ao atualizar as linhas.";
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult DeleteRequisitionLine([FromBody] RequisitionTemplateLineViewModel item)
        {
            if (item != null)
            {
                if (DBRequestTemplateLines.Delete(item.ParseToDB()))
                {
                    item.eReasonCode = 1;
                    item.eMessage = "Registo eliminado com sucesso.";
                }
                else
                {
                    item = new RequisitionTemplateLineViewModel();
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao eliminar o registo.";
                }
            }
            else
            {
                item = new RequisitionTemplateLineViewModel();
                item.eReasonCode = 2;
                item.eMessage = "Ocorreu um erro: a linha não pode ser nula.";
            }
            return Json(item);
        }
    }
}