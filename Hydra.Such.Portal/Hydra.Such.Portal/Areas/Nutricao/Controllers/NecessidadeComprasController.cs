using System;
using System.Collections.Generic;
using System.Linq;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Nutrition;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Data.ViewModel.Nutrition;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Portal.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hydra.Such.Portal.Areas.Nutricao.Controllers
{
    public class NecessidadeComprasController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;

        public NecessidadeComprasController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
        }

        [Area("Nutricao")]
        public IActionResult Detalhes(int? id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 3, 41);
            if (UPerm != null && UPerm.Read.Value)
            {  
                ViewBag.UPermissions = UPerm;
                if (id != null && id > 0)
                {
                    ViewBag.ProductivityUnityNo = id;
                    UnidadesProdutivas ProductivityUnitDB = DBProductivityUnits.GetById((int) id);
                    ViewBag.ProductivityUnitId = ProductivityUnitDB.NºUnidadeProdutiva;
                    ViewBag.ProductivityUnitDesc = ProductivityUnitDB.Descrição;
                }
                else
                {
                    ViewBag.ProductivityUnitId =0;
                    ViewBag.ProductivityUnitDesc = "";
                }
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [Area("Nutricao")]
        public IActionResult NecessidadeCompraDireta(int? id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 3, 44);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                if (id != null && id > 0)
                {
                    ViewBag.ProductivityUnityNo = id;
                    UnidadesProdutivas ProductivityUnitDB = DBProductivityUnits.GetById((int)id);
                    ViewBag.ProductivityUnitId = ProductivityUnitDB.NºUnidadeProdutiva;
                    ViewBag.ProductivityUnitDesc = ProductivityUnitDB.Descrição;
                }
                else
                {
                    ViewBag.ProductivityUnitId = 0;
                    ViewBag.ProductivityUnitDesc = "";
                }
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        [HttpPost]
        [Area("Nutricao")]
        public JsonResult GetGridValues([FromBody]int id)
        {
            List<DailyRequisitionProductiveUnitViewModel> result = DBShoppingNecessity.GetAllById(id).ParseToViewModel();
            return Json(result);
        }
        [HttpPost]
        [Area("Nutricao")]
        public JsonResult GetGridValuesWithoutDatePriceSup([FromBody]int id)
        {
            List<DailyRequisitionProductiveUnitViewModel> result = DBShoppingNecessity.GetAllDirectById(id).ParseToViewModel();
            return Json(result);
        }


        [HttpPost]
        [Area("Nutricao")]
        public JsonResult GetModelRequisition()
            {
                List<RequisitionViewModel> result = DBRequest.GetAllModelRequest().ParseToViewModel();
                //Apply User Dimensions Validations
                List<AcessosDimensões> CUserDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                //Regions
                if (CUserDimensions.Where(y => y.Dimensão == 1).Count() > 0)
                    result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == 1 && y.ValorDimensão == x.RegionCode));
                //FunctionalAreas
                if (CUserDimensions.Where(y => y.Dimensão == 2).Count() > 0)
                    result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == 2 && y.ValorDimensão == x.FunctionalAreaCode));
                //ResponsabilityCenter
                if (CUserDimensions.Where(y => y.Dimensão == 3).Count() > 0)
                    result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == 3 && y.ValorDimensão == x.CenterResponsibilityCode));
            return Json(result);
        }
        [HttpPost]
        [Area("Nutricao")]
        public JsonResult UpdateDirectShoppingNecessity([FromBody] List<DailyRequisitionProductiveUnitViewModel> dp)
        {
            ErrorHandler result = new ErrorHandler();
            string notupdate = "";
            if (dp != null)
            {
                List<DiárioRequisiçãoUnidProdutiva> previousList;
                // Get All
                previousList = DBShoppingNecessity.GetAllWithoutPriceSup();
                foreach (DiárioRequisiçãoUnidProdutiva line in previousList)
                {
                    if (!dp.Any(x => x.LineNo == line.NºLinha))
                    {
                        DBShoppingNecessity.Delete(line);
                    }
                }

                //Update or Create
                try
                {
                    dp.ForEach(x =>
                    {
                        List<DiárioRequisiçãoUnidProdutiva> dpObject = DBShoppingNecessity.GetByLineNoWithoutPriceSup(x.LineNo);

                        if (dpObject.Count > 0)
                        {
                            DiárioRequisiçãoUnidProdutiva newdp = dpObject.FirstOrDefault();

                            newdp.NºLinha = x.LineNo;
                            newdp.NºUnidadeProdutiva = x.ProductionUnitNo;
                            newdp.NºProduto = x.ProductNo;
                            newdp.Descrição = x.Description;
                            newdp.CódUnidadeMedida = x.UnitMeasureCode;
                            newdp.Quantidade = x.Quantity;
                            newdp.CustoUnitárioDireto = x.DirectUnitCost;
                            newdp.Valor = x.TotalValue;
                            newdp.NºFornecedor = x.SupplierNo;
                            newdp.DataHoraModificação = DateTime.Now;
                            newdp.UtilizadorModificação = User.Identity.Name;
                            newdp.CodigoLocalização = x.LocalCode;
                            newdp.NomeFornecedor = x.SupplierName;
                            newdp.NºEncomendaAberto = x.OpenOrderNo;
                            newdp.NºLinhaEncomendaAberto = x.OrderLineOpenNo;
                            newdp.DescriçãoUnidadeProduto = x.ProductUnitDescription;
                            newdp.DataReceçãoEsperada = string.IsNullOrEmpty(x.ExpectedReceptionDate)
                                ? (DateTime?)null
                                : DateTime.Parse(x.ExpectedReceptionDate);
                            newdp.Observações = x.Observation;
                            newdp = DBShoppingNecessity.Update(newdp);
                            if (newdp == null)
                            {
                                result.eReasonCode = 3;
                                result.eMessage =
                                    "Ocorreu um erro ao Atualizar a Diário Requisição Unid. Produtiva";
                            }
                            else
                            {
                                result.eReasonCode = 1;
                                result.eMessage = "Diário requisição Unid. Produtiva foi atualizado";
                            }
                        }
                        else
                        {
                            DiárioRequisiçãoUnidProdutiva newdp = new DiárioRequisiçãoUnidProdutiva()
                            {
                                NºLinha = x.LineNo,
                                NºUnidadeProdutiva = x.ProductionUnitNo,
                                NºProduto = x.ProductNo,
                                Descrição = x.Description,
                                CódUnidadeMedida = x.UnitMeasureCode,
                                Quantidade = x.Quantity,
                                Valor = x.TotalValue,
                                NºFornecedor = x.SupplierNo,
                                CodigoLocalização = x.LocalCode,
                                NomeFornecedor = x.SupplierName,
                                NºEncomendaAberto = x.OpenOrderNo,
                                NºLinhaEncomendaAberto = x.OrderLineOpenNo,
                                DescriçãoUnidadeProduto = x.ProductUnitDescription,
                                DataReceçãoEsperada = string.IsNullOrEmpty(x.ExpectedReceptionDate)
                                    ? (DateTime?)null
                                    : DateTime.Parse(x.ExpectedReceptionDate),
                                Observações = x.Observation
                            };
                            newdp.UtilizadorCriação = User.Identity.Name;
                            newdp.DataHoraCriação = DateTime.Now;
                            newdp = DBShoppingNecessity.Create(newdp);
                            if (newdp == null)
                            {
                                result.eReasonCode = 4;
                                result.eMessage =
                                    "Ocorreu um erro ao criar a Diário Requisição Unid. Produtiva";
                            }
                            else
                            {
                                result.eReasonCode = 1;
                                result.eMessage = "O registo Diário requisição Unid. Produtiva foi criado";
                            }
                        }

                    });
                }
                catch (Exception e)
                {
                    result.eReasonCode = 5;
                    result.eMessage =
                        "Ocorreu um erro não com Diário Requisição Unid. Produtiva";
                }
            }
            else
            {
                result.eReasonCode = 2;
                result.eMessage = "Occorreu um erro ao atualizar o Diário de requisição Unid. Produtiva.";
            }
            return Json(result);
        }


        [HttpPost]
        [Area("Nutricao")]
        public JsonResult UpdateShoppingNecessity([FromBody] List<DailyRequisitionProductiveUnitViewModel> dp)
        {
            ErrorHandler result = new ErrorHandler();
            string notupdate = "";
            if (dp != null)
            {
                List<DiárioRequisiçãoUnidProdutiva> previousList;
                // Get All
                previousList = DBShoppingNecessity.GetAll();
                foreach (DiárioRequisiçãoUnidProdutiva line in previousList)
                {
                    if (!dp.Any(x => x.LineNo == line.NºLinha))
                    {
                        DBShoppingNecessity.Delete(line);
                    }
                }
           
                //Update or Create
                try
                {
                    dp.ForEach(x =>
                    {
                        List<DiárioRequisiçãoUnidProdutiva> dpObject = DBShoppingNecessity.GetByLineNo(x.LineNo);

                        if (dpObject.Count > 0)
                        {
                            DiárioRequisiçãoUnidProdutiva newdp = dpObject.FirstOrDefault();

                            newdp.NºLinha = x.LineNo;
                            newdp.NºUnidadeProdutiva = x.ProductionUnitNo;
                            newdp.NºProduto = x.ProductNo;
                            newdp.Descrição = x.Description;
                            newdp.CódUnidadeMedida = x.UnitMeasureCode;
                            newdp.Quantidade = x.Quantity;
                            newdp.CustoUnitárioDireto = x.DirectUnitCost;
                            newdp.Valor = x.TotalValue;
                            newdp.NºProjeto = x.ProjectNo;
                            newdp.NºFornecedor = x.SupplierNo;
                            newdp.TipoRefeição = x.MealType;
                            newdp.TabelaPreçosFornecedor = x.TableSupplierPrice;
                            newdp.DataHoraCriação = x.CreateDateTime;
                            newdp.DataHoraModificação = DateTime.Now;
                            newdp.UtilizadorCriação = x.CreateUser;
                            newdp.UtilizadorModificação = User.Identity.Name;
                            newdp.DataReceçãoEsperada = string.IsNullOrEmpty(x.ExpectedReceptionDate)
                                ? (DateTime?) null
                                : DateTime.Parse(x.ExpectedReceptionDate);
                            newdp.DataPPreçoFornecedor = string.IsNullOrEmpty(x.DateByPriceSupplier)
                                ? (DateTime?) null
                                : DateTime.Parse(x.DateByPriceSupplier);
                            newdp.CodigoLocalização = x.LocalCode;
                            newdp.QuantidadePorUnidMedida = x.QuantitybyUnitMeasure;
                            newdp.CodigoProdutoFornecedor = x.SupplierProductCode;
                            newdp.DescriçãoProdutoFornecedor = x.SupplierProductDescription;
                            newdp.NomeFornecedor = x.SupplierName;
                            newdp.NºEncomendaAberto = x.OpenOrderNo;
                            newdp.NºLinhaEncomendaAberto = x.OrderLineOpenNo;
                            newdp.DescriçãoUnidadeProduto = x.ProductUnitDescription;
                            newdp.NºDocumento = x.DocumentNo;
                            newdp = DBShoppingNecessity.Update(newdp);
                            if (newdp == null)
                            {
                                result.eReasonCode = 3;
                                result.eMessage =
                                    "Ocorreu um erro ao Atualizar a Diário Requisição Unid. Produtiva";
                            }
                            else
                            {
                                result.eReasonCode = 1;
                                result.eMessage = "Diário requisição Unid. Produtiva foi atualizado";
                            }
                        }
                        else
                        {
                            DiárioRequisiçãoUnidProdutiva newdp = new DiárioRequisiçãoUnidProdutiva()
                            {
                                NºLinha = x.LineNo,
                                NºUnidadeProdutiva = x.ProductionUnitNo,
                                NºProduto = x.ProductNo,
                                Descrição = x.Description,
                                CódUnidadeMedida = x.UnitMeasureCode,
                                Quantidade = x.Quantity,
                                CustoUnitárioDireto = x.DirectUnitCost,
                                Valor = x.TotalValue,
                                NºProjeto = x.ProjectNo,
                                NºFornecedor = x.SupplierNo,
                                TipoRefeição = x.MealType,
                                TabelaPreçosFornecedor = x.TableSupplierPrice,
                                DataHoraModificação = x.UpdateDateTime,
                                UtilizadorModificação = User.Identity.Name,
                                DataReceçãoEsperada = string.IsNullOrEmpty(x.ExpectedReceptionDate)
                                    ? (DateTime?) null
                                    : DateTime.Parse(x.ExpectedReceptionDate),
                                DataPPreçoFornecedor = string.IsNullOrEmpty(x.DateByPriceSupplier)
                                    ? (DateTime?) null
                                    : DateTime.Parse(x.DateByPriceSupplier),
                                CodigoLocalização = x.LocalCode,
                                QuantidadePorUnidMedida = x.QuantitybyUnitMeasure,
                                CodigoProdutoFornecedor = x.SupplierProductCode,
                                DescriçãoProdutoFornecedor = x.SupplierProductDescription,
                                NomeFornecedor = x.SupplierName,
                                NºEncomendaAberto = x.OpenOrderNo,
                                NºLinhaEncomendaAberto = x.OrderLineOpenNo,
                                DescriçãoUnidadeProduto = x.ProductUnitDescription,
                                NºDocumento = x.DocumentNo
                            };
                            newdp.UtilizadorCriação = User.Identity.Name;

                            newdp.DataHoraCriação = DateTime.Now;
                            newdp = DBShoppingNecessity.Create(newdp);
                            if (newdp == null)
                            {
                                result.eReasonCode = 4;
                                result.eMessage =
                                    "Ocorreu um erro ao criar a Diário Requisição Unid. Produtiva";
                            }
                            else
                            {
                                result.eReasonCode = 1;
                                result.eMessage = "O registo Diário requisição Unid. Produtiva foi criado";
                            }
                        }
                       
                    });
                }
                catch (Exception e)
                {
                    result.eReasonCode = 5;
                    result.eMessage =
                        "Ocorreu um erro não com Diário Requisição Unid. Produtiva";
                }
            }
            else
            {
                result.eReasonCode = 2;
                result.eMessage = "Occorreu um erro ao atualizar o Diário de requisição Unid. Produtiva.";
            }
            return Json(result);
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult CreateShoppingNecessity([FromBody] List<DailyRequisitionProductiveUnitViewModel> dp)
        {
            ErrorHandler result = new ErrorHandler();
            string notupdate = "";
            if (dp != null)
            {
                //Update or Create
                try
                {
                    dp.ForEach(x =>
                    {
                        List<DiárioRequisiçãoUnidProdutiva> dpObject = DBShoppingNecessity.GetByLineNo(x.LineNo);

                        if (dpObject.Count > 0)
                        {
                            DiárioRequisiçãoUnidProdutiva newdp = dpObject.FirstOrDefault();

                            newdp.NºLinha = x.LineNo;
                            newdp.NºUnidadeProdutiva = x.ProductionUnitNo;
                            newdp.NºProduto = x.ProductNo;
                            newdp.Descrição = x.Description;
                            newdp.CódUnidadeMedida = x.UnitMeasureCode;
                            newdp.Quantidade = x.Quantity;
                            newdp.CustoUnitárioDireto = x.DirectUnitCost;
                            newdp.Valor = x.TotalValue;
                            newdp.NºProjeto = x.ProjectNo;
                            newdp.NºFornecedor = x.SupplierNo;
                            newdp.TipoRefeição = x.MealType;
                            newdp.TabelaPreçosFornecedor = x.TableSupplierPrice;
                            newdp.DataHoraCriação = x.CreateDateTime;
                            newdp.DataHoraModificação = DateTime.Now;
                            newdp.UtilizadorCriação = x.CreateUser;
                            newdp.UtilizadorModificação = User.Identity.Name;
                            newdp.DataReceçãoEsperada = string.IsNullOrEmpty(x.ExpectedReceptionDate)
                                ? (DateTime?)null
                                : DateTime.Parse(x.ExpectedReceptionDate);
                            newdp.DataPPreçoFornecedor = string.IsNullOrEmpty(x.DateByPriceSupplier)
                                ? (DateTime?)null
                                : DateTime.Parse(x.DateByPriceSupplier);
                            newdp.CodigoLocalização = x.LocalCode;
                            newdp.QuantidadePorUnidMedida = x.QuantitybyUnitMeasure;
                            newdp.CodigoProdutoFornecedor = x.SupplierProductCode;
                            newdp.DescriçãoProdutoFornecedor = x.SupplierProductDescription;
                            newdp.NomeFornecedor = x.SupplierName;
                            newdp.NºEncomendaAberto = x.OpenOrderNo;
                            newdp.NºLinhaEncomendaAberto = x.OrderLineOpenNo;
                            newdp.DescriçãoUnidadeProduto = x.ProductUnitDescription;
                            newdp.NºDocumento = x.DocumentNo;
                            newdp = DBShoppingNecessity.Update(newdp);
                            if (newdp == null)
                            {
                                result.eReasonCode = 3;
                                result.eMessage =
                                    "Ocorreu um erro ao Atualizar a Diário Requisição Unid. Produtiva";
                            }
                            else
                            {
                                result.eReasonCode = 1;
                                result.eMessage = "Diário requisição Unid. Produtiva foi atualizado";
                            }
                        }
                        else
                        {
                            DiárioRequisiçãoUnidProdutiva newdp = new DiárioRequisiçãoUnidProdutiva()
                            {
                                NºLinha = x.LineNo,
                                NºUnidadeProdutiva = x.ProductionUnitNo,
                                NºProduto = x.ProductNo,
                                Descrição = x.Description,
                                CódUnidadeMedida = x.UnitMeasureCode,
                                Quantidade = x.Quantity,
                                CustoUnitárioDireto = x.DirectUnitCost,
                                Valor = x.TotalValue,
                                NºProjeto = x.ProjectNo,
                                NºFornecedor = x.SupplierNo,
                                TipoRefeição = x.MealType,
                                TabelaPreçosFornecedor = x.TableSupplierPrice,
                                DataHoraModificação = x.UpdateDateTime,
                                UtilizadorModificação = User.Identity.Name,
                                DataReceçãoEsperada = string.IsNullOrEmpty(x.ExpectedReceptionDate)
                                    ? (DateTime?)null
                                    : DateTime.Parse(x.ExpectedReceptionDate),
                                DataPPreçoFornecedor = string.IsNullOrEmpty(x.DateByPriceSupplier)
                                    ? (DateTime?)null
                                    : DateTime.Parse(x.DateByPriceSupplier),
                                CodigoLocalização = x.LocalCode,
                                QuantidadePorUnidMedida = x.QuantitybyUnitMeasure,
                                CodigoProdutoFornecedor = x.SupplierProductCode,
                                DescriçãoProdutoFornecedor = x.SupplierProductDescription,
                                NomeFornecedor = x.SupplierName,
                                NºEncomendaAberto = x.OpenOrderNo,
                                NºLinhaEncomendaAberto = x.OrderLineOpenNo,
                                DescriçãoUnidadeProduto = x.ProductUnitDescription,
                                NºDocumento = x.DocumentNo
                            };
                            newdp.UtilizadorCriação = User.Identity.Name;

                            newdp.DataHoraCriação = DateTime.Now;
                            newdp = DBShoppingNecessity.Create(newdp);
                            if (newdp == null)
                            {
                                result.eReasonCode = 4;
                                result.eMessage =
                                    "Ocorreu um erro ao criar a Diário Requisição Unid. Produtiva";
                            }
                            else
                            {
                                result.eReasonCode = 1;
                                result.eMessage = "O registo Diário requisição Unid. Produtiva foi criado";
                            }
                        }

                    });
                }
                catch (Exception e)
                {
                    result.eReasonCode = 5;
                    result.eMessage =
                        "Ocorreu um erro não com Diário Requisição Unid. Produtiva";
                }
            }
            else
            {
                result.eReasonCode = 2;
                result.eMessage = "Occorreu um erro ao atualizar o Diário de requisição Unid. Produtiva.";
            }
            return Json(result);
        }
        [HttpPost]
        [Area("Nutricao")]
        public JsonResult GetProductivityUnits()
        {
            List<ProductivityUnitViewModel> result = DBProductivityUnits.ParseListToViewModel(DBProductivityUnits.GetAll());
            return Json(result);
        }


        //Generate Requisitions and Requisitions Lines by Shopping Necessity lines
        [HttpPost]
        [Area("Nutricao")]
        public JsonResult GenerateRequesition([FromBody] List<DailyRequisitionProductiveUnitViewModel> data)
        {
            Requisição resultRq = new Requisição();
            LinhasRequisição resultRqLines = new LinhasRequisição();
            ErrorHandler result = new ErrorHandler();
            int rLinesCreated = 0;
            int rLinesNotCreated = 0;

            //Get Contract Numeration
            Configuração Configs = DBConfigurations.GetById(1);
            int ProjectNumerationConfigurationId = 0;
            ProjectNumerationConfigurationId = Configs.NumeraçãoRequisições.Value;
            try
            {
                //get formulary values
                DailyRequisitionProductiveUnitViewModel valrProd = new DailyRequisitionProductiveUnitViewModel();
                foreach (DailyRequisitionProductiveUnitViewModel getform in data)
                {
                    if (getform.LineNo<=0)
                    {
                        valrProd = getform;
                    }
                }
                //get all lines with quantity > 0
                List<DailyRequisitionProductiveUnitViewModel> allprod = new List<DailyRequisitionProductiveUnitViewModel>();
                foreach (DailyRequisitionProductiveUnitViewModel getlines in data)
                {
                    if (getlines.Quantity > 0)
                    {
                        allprod.Add(getlines);
                    }
                }
                if (allprod != null && allprod.Count > 0)
                {
                    UnidadesProdutivas ProductivityUnitDB =
                        DBProductivityUnits.GetById((int) valrProd.ProductionUnitNo);
                    if (valrProd != null && ProductivityUnitDB != null)
                    {
                        // CREATE requisition
                        resultRq.NºRequisição =
                            DBNumerationConfigurations.GetNextNumeration(ProjectNumerationConfigurationId,
                                (resultRq.NºRequisição == "" || resultRq.NºRequisição == null));
                        resultRq.CódigoÁreaFuncional = ProductivityUnitDB.CódigoÁreaFuncional;
                        resultRq.CódigoCentroResponsabilidade = ProductivityUnitDB.CódigoCentroResponsabilidade;
                        resultRq.CódigoRegião = ProductivityUnitDB.CódigoRegião;
                        resultRq.CódigoLocalização = valrProd.LocalCode;
                        resultRq.UnidadeProdutivaAlimentação = Convert.ToString(ProductivityUnitDB.NºUnidadeProdutiva);
                        resultRq.RequisiçãoNutrição = true;
                        resultRq.DataReceção = string.IsNullOrEmpty(valrProd.ExpectedReceptionDate)
                            ? (DateTime?) null
                            : DateTime.Parse(valrProd.ExpectedReceptionDate);
                        resultRq.Aprovadores = "";
                        resultRq.UtilizadorCriação = User.Identity.Name;
                        resultRq.DataHoraCriação = DateTime.Now;
                        resultRq.Estado = (int) RequisitionStates.Pending;
                        resultRq = DBRequest.Create(resultRq);
                        if (resultRq == null)
                        {
                            result.eReasonCode = 3;
                            result.eMessage = "Ocorreu um erro ao criar a Requisição.";
                        }
                        else
                        {
                            //Update Last Numeration Used
                            ConfiguraçãoNumerações ConfigNumerations =
                                DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);
                            ConfigNumerations.ÚltimoNºUsado = resultRq.NºRequisição;
                            ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                            DBNumerationConfigurations.Update(ConfigNumerations);
                            result.eReasonCode = 1;
                        }

                        if (allprod != null && allprod.Count > 0 && resultRq.NºRequisição != null)
                        {

                            // CREATE requisition Lines
                            foreach (DailyRequisitionProductiveUnitViewModel rpu in allprod)
                            {
                                try
                                {
                                    if (ProductivityUnitDB != null)
                                    {
                                        resultRqLines.NºRequisição = resultRq.NºRequisição;
                                        resultRqLines.Tipo = 2;
                                        resultRqLines.Código = rpu.ProductNo;
                                        resultRqLines.Descrição = rpu.Description;
                                        resultRqLines.CódigoUnidadeMedida = rpu.UnitMeasureCode;
                                        resultRqLines.QtdPorUnidadeDeMedida = rpu.QuantitybyUnitMeasure;
                                        resultRqLines.CódigoLocalização = rpu.LocalCode;
                                        resultRqLines.QuantidadeARequerer = rpu.Quantity;
                                        resultRqLines.CustoUnitário = rpu.DirectUnitCost;
                                        resultRqLines.NºFornecedor = rpu.SupplierNo;
                                        resultRqLines.DataReceçãoEsperada =
                                            string.IsNullOrEmpty(rpu.ExpectedReceptionDate)
                                                ? (DateTime?) null
                                                : DateTime.Parse(rpu.ExpectedReceptionDate);
                                        resultRqLines.CódigoProdutoFornecedor = rpu.SupplierProductCode;
                                        resultRqLines.NºEncomendaAberto = rpu.OpenOrderNo;
                                        resultRqLines.NºLinhaEncomendaAberto = Convert.ToInt32(rpu.OrderLineOpenNo);
                                        resultRqLines.CódigoÁreaFuncional = ProductivityUnitDB.CódigoÁreaFuncional;
                                        resultRqLines.CódigoCentroResponsabilidade =
                                            ProductivityUnitDB.CódigoCentroResponsabilidade;
                                        resultRqLines.CódigoRegião = ProductivityUnitDB.CódigoRegião;
                                        resultRqLines.UtilizadorCriação = User.Identity.Name;
                                        resultRqLines = DBRequestLine.Create(resultRqLines);
                                        if (resultRqLines == null)
                                        {
                                            rLinesCreated++;
                                        }
                                        else
                                        {
                                            rLinesNotCreated++;
                                        }
                                    }
                                    if (rLinesCreated > 0 && rLinesNotCreated > 0)
                                    {
                                        result.eReasonCode = 6;
                                        result.eMessage = "Algumas linhas de requisição não foram criadas";
                                    }
                                }
                                catch (Exception e)
                                {
                                    result.eReasonCode = 4;
                                    result.eMessage = "Ocorreu um erro ao criar as linhas de requisição";
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.eReasonCode = 7;
                    result.eMessage = "Só é possivel gerar Requisições quando existem linhas com a quantidade superior a 0.";
                }
               
            }
            catch (Exception e)
            {
                result.eReasonCode = 5;
                result.eMessage = "Ocorreu um erro ao gerar a requisição";
            }

            if (result == null || result.eReasonCode==1)
            {
                result.eReasonCode = 1;
                result.eMessage = "Gerou a requisição com sucesso!";
            }
            return Json(result);
        }

        //Create Shopping Necessity lines by copying Requisitions Lines 
        [Area("Nutricao")]
        public JsonResult GenerateByRequesition([FromBody] List<RequisitionViewModel> data)
        {
            ErrorHandler resultValidation = new ErrorHandler();
            string rqWithOutLines = "";
            int CountRqWithOutLines = 0;
            string supVal = "";
            string prodVal = "";
            if (data != null && data.Count > 0)
            {
                foreach (RequisitionViewModel rpu in data)
                {
                    List<LinhasRequisição> result = new List<LinhasRequisição>();
                    result = DBRequestLine.GetAllByRequisiçãos(rpu.RequisitionNo);
                    if (result != null && result.Count > 0)
                    {
                        foreach (LinhasRequisição lr in result)
                        {
                            UnidadesProdutivas ProductivityUnitDB =
                                DBProductivityUnits.GetById(Convert.ToInt32(rpu.UnitFoodProduction));

                            //Get Supplier by Code 
                            List<DDMessageString> supplierval = DBNAV2017Supplier
                                .GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, lr.NºFornecedor).Select(
                                    x => new DDMessageString()
                                    {
                                        id = x.No_,
                                        value = x.Name
                                    }).ToList();


                            //Get product by code
                            List<DDMessageString> products = DBNAV2017Products
                                .GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, lr.Código).Select(
                                    x => new DDMessageString()
                                    {
                                        id = x.Code,
                                        value = x.Name
                                    }).ToList();

                            //Get supplier value
                            if (supplierval.Count == 1)
                            {
                                var ddMessageString = supplierval.FirstOrDefault();
                                if (ddMessageString != null)
                                {
                                    supVal = ddMessageString.value;
                                }
                            }

                            //Get product value
                            if (products.Count == 1)
                            {
                                var ddMessageString = products.FirstOrDefault();
                                if (ddMessageString != null)
                                {
                                    prodVal = ddMessageString.value;
                                }
                            }
                            else
                            {
                                if (products.Count > 0)
                                {
                                    foreach (DDMessageString VARIABLE in products)
                                    {
                                        if (VARIABLE.id == lr.Código)
                                        {
                                            prodVal = VARIABLE.value;
                                        }
                                    }
                                }

                            }

                            DiárioRequisiçãoUnidProdutiva newdp = new DiárioRequisiçãoUnidProdutiva()
                            {
                                NºUnidadeProdutiva = ProductivityUnitDB.NºUnidadeProdutiva,
                                Quantidade = 0,
                                DataReceçãoEsperada = string.IsNullOrEmpty(rpu.ReceivedDate)
                                    ? (DateTime?) null
                                    : DateTime.Parse(rpu.ReceivedDate),
                                CodigoLocalização = rpu.LocalCode,
                                NºProduto = lr.Código,
                                Descrição = prodVal,
                                CódUnidadeMedida = lr.CódigoUnidadeMedida,
                                CustoUnitárioDireto = lr.CustoUnitário,
                                NºProjeto = lr.NºProjeto,
                                NºFornecedor = lr.NºFornecedor,
                                QuantidadePorUnidMedida = lr.QtdPorUnidadeDeMedida,
                                CodigoProdutoFornecedor = lr.CódigoProdutoFornecedor,
                                NomeFornecedor = supVal,
                                NºEncomendaAberto = lr.NºEncomendaAberto,
                                NºLinhaEncomendaAberto = Convert.ToString(lr.NºLinhaEncomendaAberto),
                                DescriçãoUnidadeProduto = ProductivityUnitDB.Descrição
                            };
                            newdp.UtilizadorCriação = User.Identity.Name;
                            newdp.DataHoraCriação = DateTime.Now;
                            newdp = DBShoppingNecessity.Create(newdp);
                            if (newdp == null)
                            {
                                resultValidation.eReasonCode = 5;
                                resultValidation.eMessage =
                                    "Ocorreu um erro ao criar a Diário Requisição Unid. Produtiva";
                            }
                        }
                    }
                    else
                    {
                        if (rqWithOutLines == "")
                        {
                            rqWithOutLines = rpu.RequisitionNo;
                            CountRqWithOutLines++;
                        }
                        else
                        {
                                rqWithOutLines = rqWithOutLines + ", " + rpu.RequisitionNo;
                                CountRqWithOutLines++;
                        }
                    }
                }
                if (rqWithOutLines != "" && CountRqWithOutLines == 1)
                {
                    resultValidation.eReasonCode = 3;
                    resultValidation.eMessage = "A Requisição " + rqWithOutLines +
                                                " não tem linhas de requisição associadas";
                }
                else
                {
                    if (rqWithOutLines != "" && CountRqWithOutLines >1)
                    {
                        resultValidation.eReasonCode = 4;
                        resultValidation.eMessage = "As Requisições " + rqWithOutLines +
                                                    " não têm linhas de requisição associadas";
                    }
                }
            }
            else
            {
                resultValidation.eReasonCode = 2;
                resultValidation.eMessage = "É necessario selecionar o(s) Modelo(s) de requisição para poder ser Copiado";

            }
            if (string.IsNullOrEmpty(resultValidation.eMessage))
            {
                resultValidation.eReasonCode = 1;
                resultValidation.eMessage = "Diário requisição Unid. Produtiva foi atualizado";
            }
            return Json(resultValidation);
        }

    }
}