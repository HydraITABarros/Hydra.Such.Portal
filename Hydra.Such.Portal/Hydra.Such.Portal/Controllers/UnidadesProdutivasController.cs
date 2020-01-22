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
using static Hydra.Such.Data.Enumerations;
using Hydra.Such.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Newtonsoft.Json.Linq;
using Hydra.Such.Data.Logic.ProjectMovements;

namespace Hydra.Such.Portal.Controllers
{
    public class UnidadesProdutivasController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly GeneralConfigurations _generalConfig;

        public UnidadesProdutivasController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IHostingEnvironment _hostingEnvironment, IOptions<GeneralConfigurations> appSettingsGeneral)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
            _generalConfig = appSettingsGeneral.Value;
        }


        #region Listagem Unidades Produtivas
        public IActionResult Index()
        {
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.UnidadesProdutivas);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.UserPermissions = userPerm;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        [HttpPost]
        public JsonResult GetProductivityUnits()
        {
            List<ProductivityUnitViewModel> result = DBProductivityUnits.ParseListToViewModel(DBProductivityUnits.GetAll());
            if (result != null)
            {
                result.ForEach(x =>
                {
                    x.ClientName = DBNAV2017Clients.GetClientNameByNo(x.ClientNo, _config.NAVDatabaseName, _config.NAVCompanyName);
                });

                //Apply User Dimensions Validations
                List<AcessosDimensões> CUserDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                //Regions
                if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                    result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CodeRegion));
                //FunctionalAreas
                if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                    result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CodeFunctionalArea));
                //ResponsabilityCenter
                if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                    result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CodeResponsabilityCenter));
            }
            return Json(result);
        }
        #endregion

        public IActionResult DetalhesCafetariasRefeitorios(int code)
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Cafetarias_Refeitórios);
            CafetariasRefeitórios coffeeShop = DBCoffeeShops.GetByCode(code);
            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UPermissions = userPermissions;
                ViewBag.Code = code;

                return Redirect(Url.Content("/CafetariasRefeitorios/Detalhes/" + "?productivityUnitNo=" + coffeeShop.NºUnidadeProdutiva + "&type=" + coffeeShop.Tipo + "&code=" + coffeeShop.Código + "&explorationStartDate=" + coffeeShop.DataInícioExploração));
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        #region Detalhes Unidades Produtivas
        public IActionResult Detalhes(int id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.UnidadesProdutivas);
            UserAccessesViewModel UPerm_CafetariasRefeitorios = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Cafetarias_Refeitórios);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ProductivityUnityNo = id;
                ViewBag.UPermissions = UPerm;
                ViewBag.UPermissions_CafetariasRefeitorios = UPerm_CafetariasRefeitorios;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }
        

        [HttpPost]
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
                result.ProductivityUnitNo = new int();
                result.BillingProjects = new List<DBProjectBillingViewModel>();
                result.CoffeeShops = new List<CoffeeShopViewModel>();
            }

            //Get Project Movements Values

            if (String.IsNullOrEmpty(result.ProjectKitchen))
            {
                List<MovimentosDeProjeto> KMovements = DBProjectMovements.GetByProjectNo(result.ProjectKitchen);
                result.ProjectKitchenTotalMovs = KMovements.Where(x => x.PreçoTotal.HasValue).Sum(x => x.PreçoTotal.Value);
            }

            if (String.IsNullOrEmpty(result.ProjectSubsidiaries))
            {
                List<MovimentosDeProjeto> SMovements = DBProjectMovements.GetByProjectNo(result.ProjectSubsidiaries);
                result.ProjectSubsidiariesTotalMovs = SMovements.Where(x => x.PreçoTotal.HasValue).Sum(x => x.PreçoTotal.Value);
            }

            if (String.IsNullOrEmpty(result.ProjectWasteFeedstock))
            {
                List<MovimentosDeProjeto> WMovements = DBProjectMovements.GetByProjectNo(result.ProjectWasteFeedstock);
                result.ProjectWasteFeedstockTotalMovs = WMovements.Where(x => x.PreçoTotal.HasValue).Sum(x => x.PreçoTotal.Value);
            }

            return Json(result);
        }

        [HttpPost]
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
                                x.Selected = false;
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
            catch (Exception ex)
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao criar a Unidade Produtiva.";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult UpdateProductivityUnit([FromBody] ProductivityUnitViewModel data)
        {
            try
            {
                if (data != null)
                {
                    data.CreateUser = User.Identity.Name;
                    UnidadesProdutivas CObject = DBProductivityUnits.ParseToDb(data);
                    CObject = DBProductivityUnits.GetById(CObject.NºUnidadeProdutiva);

                    if (CObject != null)
                    {
                        CObject.Descrição = data.Description;
                        CObject.Estado = data.Status;
                        CObject.NºCliente = data.ClientNo;
                        CObject.CódigoRegião = data.CodeRegion;
                        CObject.CódigoCentroResponsabilidade = data.CodeResponsabilityCenter;
                        CObject.CódigoÁreaFuncional = data.CodeFunctionalArea;
                        CObject.DataInícioExploração = data.StartDateExploration != "" && data.StartDateExploration != null ? DateTime.Parse(data.StartDateExploration) : (DateTime?)null;
                        CObject.DataFimExploração = data.EndDateExploration != "" && data.EndDateExploration != null ? DateTime.Parse(data.EndDateExploration) : (DateTime?)null;
                        CObject.Armazém = data.Warehouse;
                        CObject.ArmazémFornecedor = data.WarehouseSupplier;
                        CObject.ProjetoCozinha = data.ProjectKitchen;
                        CObject.ProjetoDesperdícios = data.ProjectWaste;
                        CObject.ProjetoDespMatPrimas = data.ProjectWasteFeedstock;
                        CObject.ProjetoMatSubsidiárias = data.ProjectSubsidiaries;
                        CObject.DataHoraModificação = DateTime.Now;
                        CObject.UtilizadorModificação = User.Identity.Name;
                        DBProductivityUnits.Update(CObject);


                        //Get Project Billing Projets
                        List<ProjetosFaturação> ExistingPBillings = DBProjectBilling.GetByNUnidadeProdutiva(CObject.NºUnidadeProdutiva);

                        List<DBProjectBillingViewModel> PBillingsToCreate = data.BillingProjects.Where(pb => !ExistingPBillings.Any(x => x.NºUnidadeProdutiva == pb.ProductivityUnitNo && x.NºProjeto == pb.ProjectNo)).ToList();
                        List<ProjetosFaturação> PBillingsToDelete = ExistingPBillings.Where(x => !data.BillingProjects.Any(pb => x.NºUnidadeProdutiva == pb.ProductivityUnitNo && x.NºProjeto == pb.ProjectNo)).ToList();

                        //Create Billing Projects
                        PBillingsToCreate.ForEach(x =>
                        {
                            x.Active = true;
                            x.ProductivityUnitNo = CObject.NºUnidadeProdutiva;
                            x.CreateUser = User.Identity.Name;
                            x.Selected = false;
                            DBProjectBilling.Create(DBProjectBilling.ParseToDB(x));
                        });

                        PBillingsToDelete.ForEach(x =>
                        {
                            DBProjectBilling.Delete(x);
                        });

                        ExistingPBillings.ForEach(x =>
                        {
                            DBProjectBillingViewModel PBToUpdate = data.BillingProjects.Where(pb => x.NºUnidadeProdutiva == pb.ProductivityUnitNo && x.NºProjeto == pb.ProjectNo).FirstOrDefault();
                            x.NºProjeto = PBToUpdate.ProjectNo;
                            x.UtilizadorModificação = User.Identity.Name;
                            x.DataHoraModificação = DateTime.Now;
                            DBProjectBilling.Update(x);
                        });

                        data = DBProductivityUnits.ParseToViewModel(CObject);
                        data.eReasonCode = 1;
                    }
                    else
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "Ocorreu um erro ao atualizar os dados na base de dados.";
                    }
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao atualizar a Unidade Produtiva.";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult DeleteProductivityUnit([FromBody] ProductivityUnitViewModel item)
        {
            ErrorHandler result = new ErrorHandler();

            try
            {
                if (item != null)
                {
                    List<ProjetosFaturação> ExistingPBillings = DBProjectBilling.GetByNUnidadeProdutiva(item.ProductivityUnitNo);
                    ExistingPBillings.ForEach(x => DBProjectBilling.Delete(DBProjectBilling.GetById(x.NºUnidadeProdutiva, x.NºProjeto)));
                    
                    DBProductivityUnits.Delete(DBProductivityUnits.ParseToDb(item));
                    result.eReasonCode = 1;
                    result.eMessage = "Unidade Produtiva removida com sucesso.";
                }
            }
            catch (Exception)
            {
                result.eReasonCode = 2;
                result.eMessage = "Ocorreu um erro ao remover a unidade produtiva.";
            }

            return Json(result);
        }
        #endregion

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_UnidadesProdutivas([FromBody] List<ProductivityUnitViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "UnidadesProdutivas\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Unidades Produtivas");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["productivityUnitNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Unidade Produtiva");
                    Col = Col + 1;
                }
                if (dp["description"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Descrição");
                    Col = Col + 1;
                }
                if (dp["codeRegion"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Região");
                    Col = Col + 1;
                }
                if (dp["codeFunctionalArea"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Centro Responsabilidade");
                    Col = Col + 1;
                }
                if (dp["codeResponsabilityCenter"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Area Funcional");
                    Col = Col + 1;
                }
                if (dp["startDateExploration"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Inicio Exploração");
                    Col = Col + 1;
                }
                if (dp["endDateExploration"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Fim Exploração");
                    Col = Col + 1;
                }
                if (dp["projectKitchen"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Proj. Cozinha");
                    Col = Col + 1;
                }
                if (dp["projectSubsidiaries"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Proj. Mat. Subsidiárias");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ProductivityUnitViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["productivityUnitNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProductivityUnitNo);
                            Col = Col + 1;
                        }
                        if (dp["description"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Description);
                            Col = Col + 1;
                        }
                        if (dp["codeRegion"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeRegion);
                            Col = Col + 1;
                        }
                        if (dp["codeFunctionalArea"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeFunctionalArea);
                            Col = Col + 1;
                        }
                        if (dp["codeResponsabilityCenter"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeResponsabilityCenter);
                            Col = Col + 1;
                        }
                        if (dp["startDateExploration"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.StartDateExploration);
                            Col = Col + 1;
                        }
                        if (dp["endDateExploration"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.EndDateExploration);
                            Col = Col + 1;
                        }
                        if (dp["projectKitchen"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProjectKitchen);
                            Col = Col + 1;
                        }
                        if (dp["projectSubsidiaries"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProjectSubsidiaries);
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
        public IActionResult ExportToExcelDownload_UnidadesProdutivas(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "UnidadesProdutivas\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Unidades Produtivas.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

    }
}