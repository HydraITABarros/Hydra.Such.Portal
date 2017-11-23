using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.Logic.Nutrition;
using Hydra.Such.Data.ViewModel.Nutrition;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Portal.Areas.Nutricao.Controllers
{
    public class CafetariasRefeitoriosController : Controller
    {
        [Area("Nutricao")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult GetCoffeeShops()
        {
            ////Apply User Dimensions Validations
            //List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            ////Regions
            //if (userDimensions.Where(y => y.Dimensão == 1).Count() > 0)
            //    result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == 1 && y.ValorDimensão == x.CodeRegion));
            ////FunctionalAreas
            //if (userDimensions.Where(y => y.Dimensão == 2).Count() > 0)
            //    result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == 2 && y.ValorDimensão == x.CodeFunctionalArea));
            ////ResponsabilityCenter
            //if (userDimensions.Where(y => y.Dimensão == 3).Count() > 0)
            //    result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == 3 && y.ValorDimensão == x.CodeResponsabilityCenter));
            var items = DBCoffeeShops.GetAll().ParseToViewModel();
            return Json(items);
        }

        [Area("Nutricao")]
        public IActionResult Detalhes(int productivityUnitNo, int type, int code, string explorationStartDate)
        {
            UserAccessesViewModel userPermissions = new UserAccessesViewModel() {
                Create = true,
                Delete = true,
                Read = true,
                Update = true
            }; //DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 2, 2);

            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UPermissions = userPermissions;

                ViewBag.ProductivityUnityNo = productivityUnitNo;
                ViewBag.Type = type;
                ViewBag.Code = code;
                ViewBag.ExplorationStartDate = explorationStartDate;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult GetCoffeeShop([FromBody] Newtonsoft.Json.Linq.JObject requestParams)
        {
            CoffeeShopViewModel item;
            if (requestParams != null)
            {
                int productivityUnitNo = int.Parse(requestParams["productivityUnitNo"].ToString());
                int type = int.Parse(requestParams["type"].ToString());
                int code = int.Parse(requestParams["code"].ToString());
                string explorationStartDate = requestParams["explorationStartDate"].ToString();

                DateTime date;                
                if (DateTime.TryParse(explorationStartDate, out date))
                {
                    item = DBCoffeeShops.GetById(productivityUnitNo, type, code, date)
                        .ParseToViewModel();
                }
                else
                {
                    item = new CoffeeShopViewModel();
                }
            }
            else
            {
                item = new CoffeeShopViewModel();
            }


            ////Apply User Dimensions Validations
            //List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            ////Regions
            //if (userDimensions.Where(y => y.Dimensão == 1).Count() > 0)
            //    result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == 1 && y.ValorDimensão == x.CodeRegion));
            ////FunctionalAreas
            //if (userDimensions.Where(y => y.Dimensão == 2).Count() > 0)
            //    result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == 2 && y.ValorDimensão == x.CodeFunctionalArea));
            ////ResponsabilityCenter
            //if (userDimensions.Where(y => y.Dimensão == 3).Count() > 0)
            //    result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == 3 && y.ValorDimensão == x.CodeResponsabilityCenter));

            return Json(item);
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult CreateCoffeeShop([FromBody] CoffeeShopViewModel data)
        {
            try
            {
                if (data != null)
                {
                    data.CreateUser = User.Identity.Name;
                    CafetariasRefeitórios itemToCreate = DBCoffeeShops.ParseToDB(data);

                    itemToCreate = DBCoffeeShops.Create(itemToCreate);

                    if (itemToCreate != null)
                    {
                        data = DBCoffeeShops.ParseToViewModel(itemToCreate);
                        data.eReasonCode = 1;
                        data.eMessage = "Registo criado com sucesso.";
                    }
                    else
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "Ocorreu um erro ao inserir os dados na base de dados.";
                    }
                }
            }
            catch (Exception)
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao criar a cafetaria / refeitório.";
            }
            return Json(data);
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult UpdateCoffeeShop([FromBody] CoffeeShopViewModel item)
        {
            if (item != null)
            {
                item.UpdateUser = User.Identity.Name;
                CafetariasRefeitórios updatedItem = DBCoffeeShops.ParseToDB(item);
                updatedItem = DBCoffeeShops.Update(updatedItem);
                
                if (updatedItem != null)
                {
                    item = DBCoffeeShops.ParseToViewModel(updatedItem);
                    item.eReasonCode = 1;
                    item.eMessage = "Cafetaria / refeitório atualizado com sucesso.";
                }
                else
                {
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao atualizar a cafetaria / refeitório.";
                }
            }
            else
            {
                item = new CoffeeShopViewModel()
                {
                    eReasonCode = 3,
                    eMessage = "Ocorreu um erro ao atualizar. A cafetaria / refeitório não pode ser nulo."
                };
            }
            return Json(item);
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult DeleteCoffeeShop([FromBody] CoffeeShopViewModel item)
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
                    bool sucess = DBCoffeeShops.Delete(DBCoffeeShops.ParseToDB(item));
                    if (sucess)
                    {
                        item.eReasonCode = 1;
                        item.eMessage = "Registo eliminado com sucesso.";
                    }
                }
                else
                {
                    item = new CoffeeShopViewModel();
                    throw new ArgumentNullException("item");
                }
            }
            catch {
                item.eReasonCode = errorHandler.eReasonCode;
                item.eMessage = errorHandler.eMessage;
            }
            return Json(item);
        }
    }
}