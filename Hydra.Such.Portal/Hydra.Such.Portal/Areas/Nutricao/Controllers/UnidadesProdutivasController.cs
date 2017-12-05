using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel.Nutrition;
using Hydra.Such.Data.Logic.Nutrition;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.NAV;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Projects;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Portal.Areas.Nutricao.Controllers
{
    public class UnidadesProdutivasController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;

        public UnidadesProdutivasController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
        }


        #region Listagem Unidades Produtivas
        [Area("Nutricao")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult GetProductivityUnits()
        {
            List<ProductivityUnitViewModel> result = DBProductivityUnits.ParseListToViewModel(DBProductivityUnits.GetAll());

            result.ForEach(x =>
            {
                x.ClientName = DBNAV2017Clients.GetClientNameByNo(x.ClientNo, _config.NAVDatabaseName, _config.NAVCompanyName);
            });

            //Apply User Dimensions Validations
            List<AcessosDimensões> CUserDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (CUserDimensions.Where(y => y.Dimensão == 1).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == 1 && y.ValorDimensão == x.CodeRegion));
            //FunctionalAreas
            if (CUserDimensions.Where(y => y.Dimensão == 2).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == 2 && y.ValorDimensão == x.CodeFunctionalArea));
            //ResponsabilityCenter
            if (CUserDimensions.Where(y => y.Dimensão == 3).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == 3 && y.ValorDimensão == x.CodeResponsabilityCenter));

            return Json(result);
        }
        #endregion

        #region Detalhes Unidades Produtivas
        [Area("Nutricao")]
        public IActionResult Detalhes(int id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 2, 2);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ProductivityUnityNo = id;
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult GetProductivityUnitData([FromBody] int ProductivityUnitNo)
        {
            UnidadesProdutivas ProductivityUnitDB = DBProductivityUnits.GetById(ProductivityUnitNo);
            ProductivityUnitViewModel result = new ProductivityUnitViewModel();

            if (ProductivityUnitDB != null)
            {
                result = DBProductivityUnits.ParseToViewModel(ProductivityUnitDB);
                
                result.BillingProjects =  DBProjectBilling.ParseListToViewModel(DBProjectBilling.GetByNUnidadeProdutiva(result.ProductivityUnitNo));
                result.CoffeeShops = DBCoffeeShops.ParseListToViewModel(DBCoffeeShops.GetByNUnidadeProdutiva(result.ProductivityUnitNo), _config.NAVDatabaseName, _config.NAVCompanyName);
            }
            else
            {
                result.BillingProjects = new List<DBProjectBillingViewModel>();
                result.CoffeeShops = new List<CoffeeShopViewModel>();
            }
            return Json(result);
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult GetProjectBillingInfo([FromBody] string ProjectNo )
        {
            Projetos CProject = DBProjects.GetById(ProjectNo);
            List<DiárioDeProjeto> CPRojectLines = DBProjectDiary.GetByProjectNo(ProjectNo).Where(x => x.Faturada.HasValue && x.Faturada.Value && x.PreçoTotal.HasValue).ToList();

            return Json(new DBProjectBillingViewModel()
            {
                ClientNo = CProject.NºCliente,
                ClientName = DBNAV2017Clients.GetClientNameByNo(CProject.NºCliente, _config.NAVDatabaseName, _config.NAVCompanyName),
                Active = true,
                ProjectNo = ProjectNo,
                TotalSales = CPRojectLines.Sum(x => x.PreçoTotal.Value)
            });
        }


        //RETURN Response
        [HttpPost]
        [Area("Nutricao")]
        public JsonResult CreateProductivityUnit([FromBody] ProductivityUnitViewModel data)
        {
            try
            {
                if (data != null)
                {
                    data.CreateUser = User.Identity.Name;
                    UnidadesProdutivas CObject = DBProductivityUnits.ParseToDb(data);
                    CObject = DBProductivityUnits.Create(CObject);

                    if (CObject != null)
                    {
                        List<DBProjectBillingViewModel> CreatedPBillings = new List<DBProjectBillingViewModel>();
                        //Create Billing Projects
                        if (data.BillingProjects.Count > 0)
                        {
                            data.BillingProjects.ForEach(x =>
                            {
                                x.Active = true;
                                x.ProductivityUnitNo = CObject.NºUnidadeProdutiva;
                                x.CreateUser = User.Identity.Name;
                                CreatedPBillings.Add(DBProjectBilling.ParseToViewModel(DBProjectBilling.Create(DBProjectBilling.ParseToDB(x))));
                            });
                        }

                        data = DBProductivityUnits.ParseToViewModel(CObject);
                        data.BillingProjects = CreatedPBillings;
                        data.eReasonCode = 1;
                    } else
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "Ocorreu um erro ao inserir os dados na base de dados.";
                    }
                }
            }
            catch (Exception)
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao criar a Unidade Produtiva.";
            }
            return Json(data);
        }



        
        #endregion  
    }
}