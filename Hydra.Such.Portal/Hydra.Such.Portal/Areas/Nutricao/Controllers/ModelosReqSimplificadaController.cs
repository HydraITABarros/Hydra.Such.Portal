using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.Logic.Nutrition;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Database;



using Hydra.Such.Data.ViewModel.Nutrition;

using Hydra.Such.Data.ViewModel;
//using Hydra.Such.Portal.Configurations;
//using Microsoft.Extensions.Options;

namespace Hydra.Such.Portal.Areas.Nutricao.Controllers
{
    public class ModelosReqSimplificadaController : Controller
    {
        [Area("Nutricao")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult GetAllReqTemplates()
        {
            var items = DBSimplifiedReqTemplates.GetAll().ParseToViewModel();

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (userDimensions.Where(y => y.Dimensão == 1).Count() > 0)
                items.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == 1 && (y.ValorDimensão == x.CodeRegion || string.IsNullOrEmpty(x.CodeRegion))));
            //FunctionalAreas
            if (userDimensions.Where(y => y.Dimensão == 2).Count() > 0)
                items.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == 2 && (y.ValorDimensão == x.CodeFunctionalArea || string.IsNullOrEmpty(x.CodeFunctionalArea))));
            //ResponsabilityCenter
            if (userDimensions.Where(y => y.Dimensão == 3).Count() > 0)
                items.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == 3 && (y.ValorDimensão == x.CodeResponsabilityCenter || string.IsNullOrEmpty(x.CodeResponsabilityCenter))));

            return Json(items);
        }

        [Area("Nutricao")]
        public IActionResult Detalhes(string id)
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 3, 38);

            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UPermissions = userPermissions;
                ViewBag.RequisitionTemplateId = id;
                
                //FormType values (0 - Create, 1 - Update); 
                ViewBag.FormType = (id != null && !string.IsNullOrEmpty(id)) ? "1" : "0";

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult GetReqTemplate([FromBody] string requisitionTemplateId)
        {
            SimplifiedReqTemplateViewModel item;
            if (!string.IsNullOrEmpty(requisitionTemplateId) && requisitionTemplateId != "0")
                item = DBSimplifiedReqTemplates.GetById(requisitionTemplateId).ParseToViewModel();
            else
                item = new SimplifiedReqTemplateViewModel();

            return Json(item);
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult CreateReqTemplate([FromBody] SimplifiedReqTemplateViewModel item)
        {
            try
            {
                if (item != null)
                {
                    item.CreateUser = User.Identity.Name;

                    var createdItem = DBSimplifiedReqTemplates.Create(item.ParseToDB());

                    if (createdItem != null)
                    {
                        item = createdItem.ParseToViewModel();
                        item.eReasonCode = 1;
                        item.eMessage = "Registo criado com sucesso.";
                    }
                    else
                    {
                        item.eReasonCode = 3;
                        item.eMessage = "Ocorreu um erro ao inserir os dados na base de dados.";
                    }
                }
            }
            catch (Exception)
            {
                item.eReasonCode = 2;
                item.eMessage = "Ocorreu um erro ao criar o modelo de requisição simplificado.";
            }
            return Json(item);
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult UpdateReqTemplate([FromBody] SimplifiedReqTemplateViewModel item)
        {
            if (item != null)
            {
                item.UpdateUser = User.Identity.Name;
                var updatedItem = DBSimplifiedReqTemplates.Update(item.ParseToDB());

                if (updatedItem != null)
                {
                    item = updatedItem.ParseToViewModel();
                    item.eReasonCode = 1;
                    item.eMessage = "Registo atualizado com sucesso.";
                }
                else
                {
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao atualizar o modelo de requisição simplificada.";
                }
            }
            else
            {
                item = new SimplifiedReqTemplateViewModel()
                {
                    eReasonCode = 3,
                    eMessage = "Ocorreu um erro ao atualizar. O modelo de requisição simplificado não pode ser nulo."
                };
            }
            return Json(item);
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult DeleteReqTemplate([FromBody] SimplifiedReqTemplateViewModel item)
        {
            ErrorHandler errorHandler = new ErrorHandler()
            {
                eReasonCode = 2,
                eMessage = "Ocorreu um erro ao eliminar o registo."
            };

            try
            {
                if (item != null)
                {
                    bool sucess = DBSimplifiedReqTemplates.Delete(item.ParseToDB());
                    if (sucess)
                    {
                        item.eReasonCode = 1;
                        item.eMessage = "Registo eliminado com sucesso.";
                    }
                }
                else
                {
                    item = new SimplifiedReqTemplateViewModel();
                }
            }
            catch
            {
                item.eReasonCode = errorHandler.eReasonCode;
                item.eMessage = errorHandler.eMessage;
            }
            return Json(item);
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult CreateReqTemplateLine([FromBody] SimplifiedReqTemplateLinesViewModel item)
        {
            if (item != null)
            {
                item.CreateUser = User.Identity.Name;
                var createdItem = DBSimplifiedReqTemplateLines.Create(item.ParseToDB());
                if (createdItem != null)
                {
                    item = createdItem.ParseToViewModel();
                    item.eReasonCode = 1;
                    item.eMessage = "Registo criado com sucesso.";
                }
                else
                {
                    item = new SimplifiedReqTemplateLinesViewModel();
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao criar o registo.";
                }
            }
            else
            {
                item = new SimplifiedReqTemplateLinesViewModel();
                item.eReasonCode = 2;
                item.eMessage = "A linha não pode ser nula.";
            }
            return Json(item);
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult DeleteReqTemplateLine([FromBody] SimplifiedReqTemplateLinesViewModel item)
        {
            ErrorHandler requestResponse = new ErrorHandler()
            {
                eReasonCode = 2,
                eMessage = "Ocorreu um erro ao eliminar o registo.",
            };

            if (item != null)
            {
                if (DBSimplifiedReqTemplateLines.Delete(item.ParseToDB()))
                {
                    requestResponse.eReasonCode = 1;
                    requestResponse.eMessage = "Registo eliminado com sucesso.";
                }
            }
            return Json(requestResponse);
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult UpdateReqTemplateLines([FromBody] List<SimplifiedReqTemplateLinesViewModel> items)
        {
            ErrorHandler updateResponse = new ErrorHandler()
            {
                eReasonCode = 2,
                eMessage = "Ocorreu um erro ao eliminar o registo.",
            };

            string reqTemplateId = string.Empty;
            if (items != null && items.Count > 0)
            {
                reqTemplateId = items.Select(x => x.RequisitionTemplateId).First();

                //Get existing lines from db
                var reqTemplateLines = DBSimplifiedReqTemplateLines.GetLinesForTemplate(reqTemplateId);
                if (reqTemplateLines != null)
                {
                    List<LinhasRequisiçõesSimplificadas> itemsToUpdate = new List<LinhasRequisiçõesSimplificadas>();

                    //Update existing
                    reqTemplateLines.ForEach(itemToUpdate =>
                    {
                        var item = items.SingleOrDefault(x => x.RequisitionTemplateId == itemToUpdate.NºRequisição &&
                            x.RequisitionTemplateLineId == itemToUpdate.NºLinha);

                        if (item != null)
                        {
                            //Update
                            itemToUpdate = item.ParseToDB();
                            itemToUpdate.UtilizadorModificação = User.Identity.Name;
                        }
                        itemsToUpdate.Add(itemToUpdate);
                    });

                    var updatedItems = DBSimplifiedReqTemplateLines.Update(itemsToUpdate);
                    if (updatedItems != null)
                    {
                        updateResponse.eReasonCode = 1;
                        updateResponse.eMessage = "Linhas atualizadas com sucesso.";
                    }
                }
                else
                {
                    updateResponse.eMessage = "Ocorreu um erro ao obter as linhas.";
                }
            }
            else
            {
                updateResponse.eMessage = "Não existem linhas para atualizar.";
            }
            return Json(updateResponse);
        }

        [HttpPost]
        public JsonResult GetServices()
        {
            List<Portal.Controllers.DDMessage> result = Data.Logic.ProjectDiary.DBServices.GetAll().Select(x => new Portal.Controllers.DDMessage()
            {
                id = x.Código,
                value = x.Descrição
            }).ToList();

            return Json(result);
        }
    }
}