using System;
using System.Collections.Generic;
using System.Linq;
using Hydra.Such.Data;
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
using Hydra.Such.Portal.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Portal.Controllers
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

        #region Necessidade de Compras
        
        public IActionResult Detalhes(int id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.NecessidadeCompras);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ProductivityUnitViewModel productivityUnit;
                if (id > 0)
                {
                    productivityUnit = DBProductivityUnits.GetById(id).ParseToViewModel();

                    ViewBag.ProductivityUnityNo = id;
                    ViewBag.ProductivityUnitId = productivityUnit.ProductivityUnitNo;//.NºUnidadeProdutiva;
                    ViewBag.ProductivityUnitDesc = productivityUnit.Description;//.Descrição;
                    ViewBag.ProductivityArea = "10";// ProductivityUnitDB.CódigoÁreaFuncional;
                    ViewBag.ProductivityUnit = productivityUnit;
                }
                else
                {
                    ViewBag.ProductivityUnitId = 0;
                    ViewBag.ProductivityUnitDesc = "";
                    ViewBag.ProductivityUnit = new ProductivityUnitViewModel();
                }
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }
        [HttpPost]
        
        public JsonResult GetGridValues([FromBody]int id)
        {
            List<DailyRequisitionProductiveUnitViewModel> result = DBShoppingNecessity.GetAllById(id).ParseToViewModel();
            return Json(result);
        }
        [HttpPost]
        
        public JsonResult GetModelRequisition()
        {
            List<RequisitionViewModel> result = DBRequestTemplates.GetAll().ParseToViewModel();
            //Apply User Dimensions Validations
            List<AcessosDimensões> CUserDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.RegionCode));
            //FunctionalAreas
            if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.FunctionalAreaCode));
            //ResponsabilityCenter
            if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CenterResponsibilityCode));
            return Json(result);
        }
        [HttpPost]
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


                if (dp.Count == 0)
                {
                    result.eReasonCode = 1;
                    result.eMessage = "Diário requisição Unid. Produtiva foi atualizado";
                    return Json(result);
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
                            string[] tokens = x.id.Split(' ');

                            if (tokens[0] != x.OpenOrderNo)
                            {
                                x.OpenOrderNo = tokens[0];
                            }
                            if (tokens[1] != x.OrderLineOpenNo)
                            {
                                x.OrderLineOpenNo = tokens[1];
                            }
                            if (tokens[2] != x.ProductNo)
                            {
                                x.ProductNo = tokens[2];
                            }
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
        public JsonResult CreateShoppingNecessity([FromBody] DailyRequisitionProductiveUnitViewModel dp)
        {
            ErrorHandler result = new ErrorHandler();
            if (dp != null)
            {
                try
                {
                    dp.CreateUser = User.Identity.Name;
                    var newdp = DBShoppingNecessity.Create(dp.ParseToDB()).ParseToViewModel();
                    if (newdp == null)
                    {
                        result.eReasonCode = 4;
                        result.eMessage = "Ocorreu um erro ao criar a Diário Requisição Unid. Produtiva";
                    }
                    else
                    {
                        result.eReasonCode = 1;
                        result.eMessage = "Registo criado com sucesso";
                    }

                }
                catch (Exception e)
                {
                    result.eReasonCode = 5;
                    result.eMessage = "Ocorreu um erro não com Diário Requisição Unid. Produtiva";
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
        
        public JsonResult GetProductivityUnits()
        {
            List<ProductivityUnitViewModel> result = DBProductivityUnits.ParseListToViewModel(DBProductivityUnits.GetAll());
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateAgreementVendor([FromBody] FornecedoresAcordoPrecos acordoFornecedor)
        {
            FornecedoresAcordoPrecos result = new FornecedoresAcordoPrecos();
            result = DBFornecedoresAcordoPrecos.Update(acordoFornecedor);
            if (acordoFornecedor != null)
            {
                result = DBFornecedoresAcordoPrecos.Update(acordoFornecedor);
                if (result != null)
                {
                    return Json(result);
                }
                else
                {
                    result = DBFornecedoresAcordoPrecos.Create(acordoFornecedor);
                    return Json(result);
                }
                
            }
            return null;
        }
        //Create Shopping Necessity lines by copying Requisitions Lines 

        public JsonResult GenerateByRequesition([FromBody] Newtonsoft.Json.Linq.JObject requestParams)
        {
            ProductivityUnitViewModel prodUnit = requestParams["prodUnit"].ToObject<ProductivityUnitViewModel>();
            List<RequisitionViewModel> data = requestParams["reqModels"].ToObject<List<RequisitionViewModel>>();
            string pricesDateValue = requestParams["pricesDate"].ToObject<string>();

            DateTime pricesDate;
            if (!DateTime.TryParse(pricesDateValue, out pricesDate))
                pricesDate = DateTime.Now;

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
                    result = DBRequestLine.GetByRequisitionId(rpu.RequisitionNo);

                    //Obter preços do acordo de preços caso existam
                    var tmpUpdatePrices = result.ParseToTemplateViewModel();
                    tmpUpdatePrices.UpdateAgreedPrices(pricesDate, prodUnit.CodeResponsabilityCenter, prodUnit.CodeRegion, prodUnit.CodeFunctionalArea);
                    result = tmpUpdatePrices.ParseToDB();

                    if (result != null && result.Count > 0)
                    {
                        foreach (LinhasRequisição lr in result)
                        {
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

                            DiárioRequisiçãoUnidProdutiva newdp = new DiárioRequisiçãoUnidProdutiva();
                            newdp.NºUnidadeProdutiva = prodUnit.ProductivityUnitNo;
                            newdp.Quantidade = 0;
                            newdp.DataReceçãoEsperada = string.IsNullOrEmpty(rpu.ReceivedDate)
                                ? (DateTime?)null
                                : DateTime.Parse(rpu.ReceivedDate);
                            newdp.CodigoLocalização = rpu.LocalCode;
                            newdp.NºProduto = lr.Código;
                            newdp.Descrição = prodVal;
                            newdp.CódUnidadeMedida = lr.CódigoUnidadeMedida;
                            newdp.CustoUnitárioDireto = lr.CustoUnitário;
                            newdp.NºProjeto = lr.NºProjeto;
                            newdp.NºFornecedor = lr.NºFornecedor;
                            newdp.QuantidadePorUnidMedida = lr.QtdPorUnidadeDeMedida;
                            newdp.CodigoProdutoFornecedor = lr.CódigoProdutoFornecedor;
                            newdp.NomeFornecedor = supVal;
                            newdp.NºEncomendaAberto = lr.NºEncomendaAberto;
                            newdp.NºLinhaEncomendaAberto = Convert.ToString(lr.NºLinhaEncomendaAberto);
                            newdp.DescriçãoUnidadeProduto = prodUnit.Description;
                            newdp.DataPPreçoFornecedor = pricesDate;
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
                    if (rqWithOutLines != "" && CountRqWithOutLines > 1)
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

        public JsonResult GenerateRequesition([FromBody] List<DailyRequisitionProductiveUnitViewModel> data)
        {
            ErrorHandler result = new ErrorHandler();
            int rLinesNotCreated = 0;
            if (data != null && data.Count > 0)
            {
                int? productivityUnitId = data.Where(x => x.ProductionUnitNo.HasValue).Select(x => x.ProductionUnitNo).FirstOrDefault();
                DateTime expextedDate = data.Where(x => !string.IsNullOrEmpty(x.ExpectedReceptionDate)).Select(x => DateTime.Parse(x.ExpectedReceptionDate)).OrderBy(x => x).FirstOrDefault();
                if (productivityUnitId.HasValue)
                {
                    UnidadesProdutivas productivityUnit = DBProductivityUnits.GetById(productivityUnitId.Value);

                    if (productivityUnit != null)
                    {
                        RequisitionViewModel req = new RequisitionViewModel();
                        req.FunctionalAreaCode = productivityUnit.CódigoÁreaFuncional;
                        req.CenterResponsibilityCode = productivityUnit.CódigoCentroResponsabilidade;
                        req.RegionCode = productivityUnit.CódigoRegião;
                        req.LocalCode = productivityUnit.Armazém;
                        req.UnitFoodProduction = Convert.ToString(productivityUnit.NºUnidadeProdutiva);
                        req.RequestNutrition = true;
                        req.RequisitionDate = DateTime.Now.ToString();
                        req.ReceivedDate = expextedDate != DateTime.MinValue ? expextedDate.ToString() : string.Empty;
                        req.CreateUser = User.Identity.Name;
                        req.CreateDate = DateTime.Now.ToString();
                        req.State = RequisitionStates.Pending;

                        data.ForEach(item =>
                        {
                            if (item.Quantity.HasValue && item.Quantity > 0)
                            {
                                RequisitionLineViewModel line = new RequisitionLineViewModel();
                                line.Type = 2;
                                line.Code = item.ProductNo;
                                line.Description = item.Description;
                                line.UnitMeasureCode = item.UnitMeasureCode;
                                line.QtyByUnitOfMeasure = item.QuantitybyUnitMeasure;
                                line.QuantityToRequire = item.Quantity;
                                line.UnitCost = item.DirectUnitCost;
                                line.SupplierNo = item.SupplierNo;
                                line.ExpectedReceivingDate = item.ExpectedReceptionDate;
                                line.SupplierProductCode = item.SupplierProductCode;
                                line.FunctionalAreaCode = req.FunctionalAreaCode;
                                line.CenterResponsibilityCode = req.CenterResponsibilityCode;
                                line.RegionCode = req.RegionCode;
                                line.CreateUser = User.Identity.Name;
                                line.ProjectNo = productivityUnit.ProjetoCozinha;
                                line.LocalCode = productivityUnit.Armazém;
                                
                                req.Lines.Add(line);
                            }
                        });

                        try
                        {
                            //Get VATPostingGroup Info
                            List<string> productsInRequisitionIds = req.Lines.Select(y => y.Code).Distinct().ToList();
                            var productsInRequisition = DBNAV2017Products.GetProductsById(_config.NAVDatabaseName, _config.NAVCompanyName, productsInRequisitionIds);
                            var vendors = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName);
                            
                            //Set VATPostingGroup Info
                            req.Lines.ForEach(line =>
                            {
                                line.VATBusinessPostingGroup = vendors.FirstOrDefault(x => x.No_ == line.SupplierNo)?.VATBusinessPostingGroup;
                                line.VATProductPostingGroup = productsInRequisition.FirstOrDefault(x => x.Code == line.Code)?.VATProductPostingGroup;
                            });
                        }
                        catch { }


                        try
                        {
                            RequisitionViewModel createdRequisition = CreateRequesition(req);
                            if (createdRequisition.eReasonCode == 1)
                            {
                                bool deletedSuccessfully = DBShoppingNecessity.Delete(data.ParseToDB());
                                if (deletedSuccessfully)
                                {
                                    result.eReasonCode = 1;
                                    result.eMessage = createdRequisition.eMessage;
                                }
                                else
                                {
                                    result.eReasonCode = 2;
                                    result.eMessage = "A requisição foi criada com sucesso, no entanto ocorreu um erro ao eliminar as linhas do diário. Por favor, elimine as linhas manualmente.";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            result.eReasonCode = 2;
                            result.eMessage = "Ocorreu um erro ao criar a requisição";
                            result.eMessages.Add(new TraceInformation(TraceType.Exception, ex.Message));
                        }
                    }
                }
                else
                {
                    result.eReasonCode = 2;
                    result.eMessage = "Não foi possivel identificar a unidade produtiva.";
                }
            }
            else
            {
                result.eReasonCode = 2;
                result.eMessage = "Ocorreu um erro: A lista esta vazia";
            }
            return Json(result);
        }

        public RequisitionViewModel CreateRequesition(RequisitionViewModel requisition)
        {
            //ErrorHandler data = new ErrorHandler();
            if(requisition != null)
            {
                //Get Contract Numeration
                Configuração Configs = DBConfigurations.GetById(1);
                int ProjectNumerationConfigurationId = 0;
                ProjectNumerationConfigurationId = Configs.NumeraçãoRequisições.Value;

                string RequisitionNo = DBNumerationConfigurations.GetNextNumeration(ProjectNumerationConfigurationId, true, false);
                if (!string.IsNullOrEmpty(RequisitionNo))
                {
                    //Update Last Numeration Used
                    ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);
                    ConfigNumerations.ÚltimoNºUsado = RequisitionNo;
                    ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                    DBNumerationConfigurations.Update(ConfigNumerations);

                    requisition.RequisitionNo = RequisitionNo;
                    Requisição createReq = DBRequest.ParseToDB(requisition);

                    createReq = DBRequest.Create(createReq);
                    if (createReq != null)
                    {
                        //Create Workflow
                        var ctx = new SuchDBContext();
                        var logEntry = new RequisicoesRegAlteracoes();
                        logEntry.NºRequisição = createReq.NºRequisição;
                        logEntry.Estado = (int)RequisitionStates.Pending;
                        logEntry.ModificadoEm = DateTime.Now;
                        logEntry.ModificadoPor = User.Identity.Name;
                        ctx.RequisicoesRegAlteracoes.Add(logEntry);
                        ctx.SaveChanges();

                        var totalValue = requisition.GetTotalValue();
                        //Start Approval
                        ErrorHandler result = ApprovalMovementsManager.StartApprovalMovement(1, createReq.CódigoÁreaFuncional, createReq.CódigoCentroResponsabilidade, createReq.CódigoRegião, totalValue, createReq.NºRequisição, User.Identity.Name, "");
                        if (result.eReasonCode != 100)
                        {
                            requisition.eMessages.Add(new TraceInformation(TraceType.Error, result.eMessage));
                        }

                        requisition.eReasonCode = 1;
                        requisition.eMessage = "Requisição criada com sucesso: " + RequisitionNo;
                        requisition.eMessages.Add(new TraceInformation(TraceType.Success, RequisitionNo));
                    }
                    else
                    {
                        requisition.eReasonCode = 2;
                        requisition.eMessage = "Ocorreu um erro ao criar a requisição.";
                    }
                }
                else
                {
                    requisition.eReasonCode = 2;
                    requisition.eMessage = "Não foi possivel gerar numeração para a requisição.";
                }

            }
            return requisition;
        }
        #endregion

        #region Necessidade de Compras Diretas

        public IActionResult NecessidadeCompraDireta(int? id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.NecessidadeComprasDireta);
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
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }
        [HttpPost]
        
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
                            string[] tokens = x.id.Split(' ');

                            if (tokens[0] != x.OpenOrderNo)
                            {
                                x.OpenOrderNo = tokens[0];
                            }
                            if (tokens[1] != x.OrderLineOpenNo)
                            {
                                x.OrderLineOpenNo = tokens[1];
                            }
                            if (tokens[2] != x.ProductNo)
                            {
                                x.ProductNo = tokens[2];
                            }
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
                                CustoUnitárioDireto = x.DirectUnitCost,
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
        
        public JsonResult GetGridValuesWithoutDatePriceSup([FromBody]int id)
        {
            List<DailyRequisitionProductiveUnitViewModel> result = DBShoppingNecessity.GetAllDirectById(id).ParseToViewModel();
            return Json(result);
        }
        //Generate Requisitions and Requisitions Lines by Shopping Necessity lines
        [HttpPost]
        
        public JsonResult GenerateRequesitionByDirectShopNeclines([FromBody] List<DailyRequisitionProductiveUnitViewModel> data)
        {
            ErrorHandler result = new ErrorHandler();
            int rLinesNotCreated = 0;
            if (data != null && data.Count>0)
            {
                //Get Numeration
                Configuração conf = DBConfigurations.GetById(1);
                int entityNumerationConfId = conf.NumeraçãoRequisições.Value;

                foreach (DailyRequisitionProductiveUnitViewModel NecShopDirect in data)
                {
                    Requisição resultRq = new Requisição();
                    LinhasRequisição resultRqLines = new LinhasRequisição();

                    try
                    {
                        if (NecShopDirect.Quantity > 0 && NecShopDirect.LineNo > 0)
                        {
                            UnidadesProdutivas ProductivityUnitDB = DBProductivityUnits.GetById((int)NecShopDirect.ProductionUnitNo);
                            if (ProductivityUnitDB != null)
                            {
                                // CREATE requisition
                                resultRq.NºRequisição = DBNumerationConfigurations.GetNextNumeration(entityNumerationConfId, true, false);
                                if (resultRq.NºRequisição != null)
                                {

                                    resultRq.CódigoÁreaFuncional = ProductivityUnitDB.CódigoÁreaFuncional;
                                    resultRq.CódigoCentroResponsabilidade = ProductivityUnitDB.CódigoCentroResponsabilidade;
                                    resultRq.CódigoRegião = ProductivityUnitDB.CódigoRegião;
                                    resultRq.Aprovadores = User.Identity.Name;
                                    resultRq.CódigoLocalização = NecShopDirect.LocalCode;
                                    resultRq.UnidadeProdutivaAlimentação = Convert.ToString(ProductivityUnitDB.NºUnidadeProdutiva);
                                    resultRq.RequisiçãoNutrição = true;
                                    resultRq.CompraADinheiro = true;
                                    resultRq.Observações = NecShopDirect.Observation;
                                    resultRq.DataReceção = string.IsNullOrEmpty(NecShopDirect.ExpectedReceptionDate) ? (DateTime?)null : DateTime.Parse(NecShopDirect.ExpectedReceptionDate);
                                    resultRq.UtilizadorCriação = User.Identity.Name;
                                    resultRq.DataHoraCriação = DateTime.Now;
                                    resultRq.Estado = (int)RequisitionStates.Pending;
                                    resultRq = DBRequest.Create(resultRq);
                                    if (resultRq == null)
                                    {
                                        result.eReasonCode = 3;
                                        result.eMessage = "Ocorreu um erro ao criar a Requisição.";
                                    }
                                    else
                                    {
                                        //Update Last Numeration Used
                                        ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(entityNumerationConfId);
                                        ConfigNumerations.ÚltimoNºUsado = resultRq.NºRequisição;
                                        ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                                        DBNumerationConfigurations.Update(ConfigNumerations);
                                        result.eReasonCode = 1;
                                    }
                                    if (resultRq.NºRequisição != null)
                                    {
                                        // CREATE requisition Lines
                                        try
                                        {
                                            if (ProductivityUnitDB != null)
                                            {
                                                resultRqLines.NºRequisição = resultRq.NºRequisição;
                                                resultRqLines.Tipo = 2;
                                                resultRqLines.Código = NecShopDirect.ProductNo;
                                                resultRqLines.Descrição = NecShopDirect.Description;
                                                resultRqLines.CódigoUnidadeMedida = NecShopDirect.UnitMeasureCode;
                                                resultRqLines.CódigoLocalização = NecShopDirect.LocalCode;
                                                resultRqLines.QuantidadeARequerer = NecShopDirect.Quantity;
                                                resultRqLines.CustoUnitário = NecShopDirect.DirectUnitCost;
                                                resultRqLines.NºFornecedor = NecShopDirect.SupplierNo;
                                                resultRqLines.DataReceçãoEsperada =
                                                string.IsNullOrEmpty(NecShopDirect.ExpectedReceptionDate)
                                                    ? (DateTime?)null
                                                    : DateTime.Parse(NecShopDirect.ExpectedReceptionDate);
                                                resultRqLines.CódigoRegião = ProductivityUnitDB.CódigoRegião;
                                                resultRqLines.CódigoÁreaFuncional = ProductivityUnitDB.CódigoÁreaFuncional;
                                                resultRqLines.CódigoCentroResponsabilidade = ProductivityUnitDB.CódigoCentroResponsabilidade;
                                                resultRqLines.UtilizadorCriação = User.Identity.Name;
                                                resultRqLines = DBRequestLine.Create(resultRqLines);
                                                if (resultRqLines == null)
                                                {
                                                    rLinesNotCreated++;
                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            result.eReasonCode = 4;
                                            result.eMessage = "Ocorreu um erro ao criar as linhas de requisição";
                                        }
                                    }
                                }
                                else
                                {
                                    result.eReasonCode = 5;
                                    result.eMessage = "Não foi possivel gerar a numeração.";
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        result.eReasonCode = 5;
                        result.eMessage = "Ocorreu um erro ao gerar a requisição";
                    }

                }
            }
            else
            {
                result.eReasonCode = 2;
                result.eMessage = "Ocorreu um erro: A lista esta vazia";
            }
            if (rLinesNotCreated>0)
            {
                if (rLinesNotCreated == data.Count)
                {
                    result.eReasonCode = 6;
                    result.eMessage = "Ocorreu um erro: As linhas de requisição não foram criadas";
                }
                else
                {
                    result.eReasonCode = 7;
                    result.eMessage = "Ocorreu um erro: Algumas linhas de requisição não foram criadas";
                }
            }
            if (result == null || result.eReasonCode == 1)
            {
                result.eReasonCode = 1;
                result.eMessage = "Gerou a requisição com sucesso!";
            }
            return Json(result);
        }
        #endregion
    }
}