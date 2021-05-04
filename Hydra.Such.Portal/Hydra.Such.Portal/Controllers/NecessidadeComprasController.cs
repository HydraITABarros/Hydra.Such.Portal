using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult Detalhes(int productivityUnitNo, int type)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.NecessidadeCompras);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ProductivityUnitViewModel productivityUnit;
                if (productivityUnitNo > 0)
                {
                    productivityUnit = DBProductivityUnits.GetById(productivityUnitNo).ParseToViewModel();

                    if (type == 1)
                    {
                        ViewBag.ProductivityUnityNo = productivityUnitNo;
                        ViewBag.ProductivityUnitId = productivityUnit.ProductivityUnitNo;//.NºUnidadeProdutiva;
                        ViewBag.ProductivityUnitDesc = productivityUnit.Description;//.Descrição;
                        ViewBag.ProductivityArea = "10";// ProductivityUnitDB.CódigoÁreaFuncional;
                        ViewBag.ProductivityUnit = productivityUnit;
                        ViewBag.ProductivityArmazem = productivityUnit.WarehouseSupplier;

                        ViewBag.Type = type; // 1 = Matéria Prima
                    }
                    else
                    {
                        ViewBag.ProductivityUnityNo = productivityUnitNo;
                        ViewBag.ProductivityUnitId = productivityUnit.ProductivityUnitNo;//.NºUnidadeProdutiva;
                        ViewBag.ProductivityUnitDesc = productivityUnit.Description;//.Descrição;
                        ViewBag.ProductivityArea = "10";// ProductivityUnitDB.CódigoÁreaFuncional;
                        ViewBag.ProductivityUnit = productivityUnit;
                        ViewBag.ProductivityArmazem = productivityUnit.WarehouseSupplier;

                        ViewBag.Type = type; // 2 = Matéria Subsidiária
                    }
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
        public JsonResult GetGridValues([FromBody]int id, int type)
        {
            List<DailyRequisitionProductiveUnitViewModel> result = DBShoppingNecessity.GetAllByIdAndType(id, type).ParseToViewModel();
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
        public JsonResult DeleteShoppingNecessity([FromBody] int linha)
        {
            ErrorHandler result = new ErrorHandler();

            DiárioRequisiçãoUnidProdutiva toDelete = DBShoppingNecessity.GetByOnlyLineNo(linha);

            if (toDelete != null)
            {
                if (DBShoppingNecessity.Delete(toDelete) == true)
                {
                    result.eReasonCode = 1;
                    result.eMessage = "A linha do Diário requisição Unid. Produtiva foi eliminada com sucesso.";

                    return Json(result);
                }
            }

            result.eReasonCode = 2;
            result.eMessage = "Ocorreu um erro ao eliminar a linha do Diário requisição Unid. Produtiva";

            return Json(result);
        }


        [HttpPost]
        public JsonResult UpdateShoppingNecessity([FromBody] List<DailyRequisitionProductiveUnitViewModel> dp)
        {
            ErrorHandler result = new ErrorHandler();
            if (dp != null)
            {
                //List<DiárioRequisiçãoUnidProdutiva> previousList;
                //// Get All
                //previousList = DBShoppingNecessity.GetAll();
                //foreach (DiárioRequisiçãoUnidProdutiva line in previousList)
                //{
                //    if (!dp.Any(x => x.LineNo == line.NºLinha))
                //    {
                //        DBShoppingNecessity.Delete(line);
                //    }
                //}


                //if (dp.Count == 0)
                //{
                //    result.eReasonCode = 1;
                //    result.eMessage = "Diário requisição Unid. Produtiva foi atualizado";
                //    return Json(result);
                //}

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
                            newdp.Descricao2 = x.Description2;
                            newdp.CódUnidadeMedida = x.UnitMeasureCode;
                            newdp.Quantidade = x.Quantity;
                            newdp.QuantidadeDisponivel = x.QuantidadeDisponivel;
                            newdp.QuantidadeReservada = x.QuantidadeReservada;
                            newdp.CustoUnitárioDireto = x.DirectUnitCost;
                            newdp.Valor = x.TotalValue;
                            newdp.NºProjeto = x.ProjectNo;
                            newdp.NºFornecedor = x.SupplierNo;
                            newdp.NoSubFornecedor = x.SubSupplierNo;
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
                            newdp.NomeSubFornecedor = x.SubSupplierName;
                            newdp.NºEncomendaAberto = x.OpenOrderNo;
                            newdp.NºLinhaEncomendaAberto = x.OrderLineOpenNo;
                            newdp.DescriçãoUnidadeProduto = x.ProductUnitDescription;
                            newdp.GrupoRegistoIvaProduto = x.GrupoRegistoIvaProduto;
                            newdp.NºDocumento = x.DocumentNo;
                            newdp.Tipo = x.Tipo;
                            newdp.Interface = x.Interface;
                            newdp.CustoUnitarioSubFornecedor = x.DirectUnitCostSubSupplier;
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
                                Descricao2 = x.Description2,
                                CódUnidadeMedida = x.UnitMeasureCode,
                                Quantidade = x.Quantity,
                                QuantidadeDisponivel = x.QuantidadeDisponivel,
                                QuantidadeReservada = x.QuantidadeReservada,
                                CustoUnitárioDireto = x.DirectUnitCost,
                                Valor = x.TotalValue,
                                NºProjeto = x.ProjectNo,
                                NºFornecedor = x.SupplierNo,
                                NoSubFornecedor = x.SubSupplierNo,
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
                                NomeSubFornecedor = x.SubSupplierName,
                                NºEncomendaAberto = x.OpenOrderNo,
                                NºLinhaEncomendaAberto = x.OrderLineOpenNo,
                                DescriçãoUnidadeProduto = x.ProductUnitDescription,
                                GrupoRegistoIvaProduto = x.GrupoRegistoIvaProduto,
                                NºDocumento = x.DocumentNo,
                                Tipo = x.Tipo,
                                Interface = x.Interface,
                                CustoUnitarioSubFornecedor = x.DirectUnitCostSubSupplier,
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
                    UnidadesProdutivas UP = DBProductivityUnits.GetById((int)dp.ProductionUnitNo);
                    if (UP != null && dp.Tipo == 1)
                    {
                        dp.ProjectNo = UP.ProjetoCozinha;
                    }
                    else
                    {
                        if (UP != null && dp.Tipo == 2)
                        {
                            dp.ProjectNo = UP.ProjetoMatSubsidiárias;
                        }
                    }

                    if (dp.Description.Length >= 100)
                    {
                        result.eReasonCode = 6;
                        result.eMessage = "O campo Descrição Produto Fornecedor não pode ter mais de 100 carateres.";
                        return Json(result);
                    }

                    if (dp.Description2.Length >= 50)
                    {
                        result.eReasonCode = 7;
                        result.eMessage = "O campo Descrição 2 Produto não pode ter mais de 50 carateres.";
                        return Json(result);
                    }

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
            string pricesDateValue = requestParams["pricesDate"].ToObject<string>(); // Data p/ Preço Fornecedor
            string expectedReceipDateValue = requestParams["expectedReceipDate"].ToObject<string>();
            string tipovalue = requestParams["Tipo"].ToObject<string>();

            if (!DateTime.TryParse(pricesDateValue, out DateTime pricesDate))
                pricesDate = DateTime.Now;
            if (!DateTime.TryParse(expectedReceipDateValue, out DateTime expectedReceipDate))
                expectedReceipDate = DateTime.MinValue;
            int tipo = Convert.ToInt32(tipovalue);

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
                            supVal = string.Empty;
                            prodVal = string.Empty;

                            //RUI DESENVOLVIMENTO: Get Supplier and Unit Cost  
                            LinhasAcordoPrecos linhaAcordo = new LinhasAcordoPrecos();
                            if (tipo == 1)
                            {
                                linhaAcordo = DBLinhasAcordoPrecos.GetAll().Where(x => (pricesDate >= x.DtValidadeInicio && pricesDate <= x.DtValidadeFim) && (prodUnit.Warehouse == x.Localizacao) && (x.CodProduto == lr.Código)).FirstOrDefault();
                            }
                            if (tipo == 2)
                            {
                                linhaAcordo = DBLinhasAcordoPrecos.GetAll().Where(x => (pricesDate >= x.DtValidadeInicio && pricesDate <= x.DtValidadeFim) && (prodUnit.Warehouse == x.Localizacao) && (prodUnit.CodeResponsabilityCenter == x.Cresp) && (x.CodProduto == lr.Código)).FirstOrDefault();
                            }

                            //Get Supplier by Code //ACORDO DE PREÇOS
                            //List<DDMessageString> supplierval = DBNAV2017Supplier
                            //    .GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, lr.NºFornecedor).Select(
                            //        x => new DDMessageString()
                            //        {
                            //            id = x.No_,
                            //            value = x.Name
                            //        }).ToList();


                            //Get product by code
                            List<DDMessageString> products = DBNAV2017Products
                                .GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, lr.Código).Select(
                                    x => new DDMessageString()
                                    {
                                        id = x.Code,
                                        value = x.Name
                                    }).ToList();

                            //Get fornecedor by code
                            NAVSupplierViewModels fornecedor = new NAVSupplierViewModels();
                            if (!string.IsNullOrEmpty(lr.NºFornecedor))
                                fornecedor = DBNAV2017Supplier.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, lr.NºFornecedor).FirstOrDefault();

                            //Get supplier value //ACORDO DE PREÇOS
                            //if (supplierval.Count == 1)
                            //{
                            //    var ddMessageString = supplierval.FirstOrDefault();
                            //    if (ddMessageString != null)
                            //    {
                            //        supVal = ddMessageString.value;
                            //    }
                            //}

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

                            string projeto = "";
                            if (tipo == 1)
                                projeto = prodUnit.ProjectKitchen;
                            if (tipo == 2)
                                projeto = prodUnit.ProjectSubsidiaries;

                            QuantidadesViewModel Quantidades = DBNAV2017Products.GetQuantidades(_config.NAVDatabaseName, _config.NAVCompanyName, lr.Código, prodUnit.Warehouse);

                            DiárioRequisiçãoUnidProdutiva newdp = new DiárioRequisiçãoUnidProdutiva();
                            newdp.NºUnidadeProdutiva = prodUnit.ProductivityUnitNo;
                            newdp.CodigoLocalização = prodUnit.Warehouse;
                            newdp.NºProjeto = projeto;
                            newdp.DescriçãoUnidadeProduto = prodUnit.Description;
                            newdp.Quantidade = 0;
                            newdp.QuantidadeDisponivel = Quantidades != null ? Quantidades.QuantDisponivel : 0;
                            newdp.QuantidadeReservada = Quantidades != null ? Quantidades.QuantReservada : 0;
                            newdp.DataReceçãoEsperada = expectedReceipDate.CompareTo(DateTime.MinValue) > 0 ? expectedReceipDate : (DateTime?)null;
                            newdp.NºProduto = lr.Código;
                            newdp.Descrição = prodVal;
                            newdp.Descricao2 = lr.Descrição2;
                            newdp.CódUnidadeMedida = lr.CódigoUnidadeMedida;
                            //newdp.CustoUnitárioDireto = lr.CustoUnitário;
                            newdp.NºFornecedor = !string.IsNullOrEmpty(lr.NºFornecedor) ? lr.NºFornecedor : "";
                            newdp.NomeFornecedor = fornecedor != null && !string.IsNullOrEmpty(fornecedor.Name) ? fornecedor.Name : "";
                            newdp.CodigoProdutoFornecedor = lr.CódigoProdutoFornecedor;
                            newdp.QuantidadePorUnidMedida = lr.QtdPorUnidadeDeMedida;
                            newdp.NºEncomendaAberto = lr.NºEncomendaAberto;
                            newdp.NºLinhaEncomendaAberto = Convert.ToString(lr.NºLinhaEncomendaAberto);
                            newdp.DataPPreçoFornecedor = pricesDate;
                            newdp.Valor = (newdp.Quantidade == null ? 0 : newdp.Quantidade) * (newdp.CustoUnitárioDireto == null ? 0 : newdp.CustoUnitárioDireto);
                            newdp.UtilizadorCriação = User.Identity.Name;
                            newdp.DataHoraCriação = DateTime.Now;
                            newdp.Tipo = tipo;

                            if (newdp == null)
                            {
                                resultValidation.eReasonCode = 5;
                                resultValidation.eMessage =
                                    "Ocorreu um erro ao criar a Diário Requisição Unid. Produtiva";
                            }

                            if (linhaAcordo != null) //LINHA ACORDO DE PREÇOS
                            {
                                newdp.Descrição = linhaAcordo.DescricaoProduto;
                                newdp.NºFornecedor = linhaAcordo.NoFornecedor;
                                newdp.NomeFornecedor = linhaAcordo.NomeFornecedor;
                                newdp.NoSubFornecedor = !string.IsNullOrEmpty(linhaAcordo.NoSubFornecedor) ? linhaAcordo.NoSubFornecedor : "";
                                newdp.NomeSubFornecedor = !string.IsNullOrEmpty(linhaAcordo.NomeSubFornecedor) ? linhaAcordo.NomeSubFornecedor : "";
                                newdp.CódUnidadeMedida = linhaAcordo.Um;
                                newdp.CustoUnitárioDireto = linhaAcordo.CustoUnitario;
                                newdp.CodigoProdutoFornecedor = linhaAcordo.CodProdutoFornecedor;
                                newdp.DescriçãoProdutoFornecedor = linhaAcordo.DescricaoProdFornecedor;
                                newdp.QuantidadePorUnidMedida = linhaAcordo.QtdPorUm;
                                newdp.Valor = (newdp.Quantidade == null ? 0 : newdp.Quantidade) * (newdp.CustoUnitárioDireto == null ? 0 : newdp.CustoUnitárioDireto);
                                newdp.GrupoRegistoIvaProduto = linhaAcordo.GrupoRegistoIvaProduto;
                                newdp.Interface = linhaAcordo.Interface;
                                newdp.CustoUnitarioSubFornecedor = linhaAcordo.CustoUnitarioSubFornecedor;
                            }
                            else
                            {
                                resultValidation.eReasonCode = 1;
                                resultValidation.eMessage = "Verificar acordo de preços! Não existe linhas de acordo para o produto " + lr.Código + ", na data " + expectedReceipDate + " a " + pricesDate;
                            }
                            newdp = DBShoppingNecessity.Create(newdp);
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
            //ErrorHandler result = new ErrorHandler();

            //if (data != null && data.Count > 0)
            //{
            //    if (data.Where(x => x.DirectUnitCost == null || x.SupplierNo == null || x.SupplierNo == "").Count() > 0)
            //    {
            //        result.eReasonCode = 2;
            //        result.eMessage = "Existe linhas sem custo unitário ou Cód Fornecedor preenchidos.";

            //        return Json(result);
            //    }

            //    List<NAVProductsViewModel> AllProducts = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, string.Empty);
            //    data.ForEach(item =>
            //    {
            //        NAVProductsViewModel Product = AllProducts.Where(x => x.Code == item.ProductNo).FirstOrDefault();

            //        if (Product == null)
            //        {
            //            result.eReasonCode = 22;
            //            result.eMessage = "Não é possivel criar a Requisição, por o produto Nº " + item.ProductNo + " - " + item.Description + " " + item.Description2 + " estar bloqueado no NAV2017.";
            //        }
            //    });
            //    if (result.eReasonCode == 22)
            //        return Json(result);

            //    int? productivityUnitId = data.Where(x => x.ProductionUnitNo.HasValue).Select(x => x.ProductionUnitNo).FirstOrDefault();
            //    DateTime expextedDate = data.Where(x => !string.IsNullOrEmpty(x.ExpectedReceptionDate)).Select(x => DateTime.Parse(x.ExpectedReceptionDate)).OrderBy(x => x).FirstOrDefault();
            //    if (productivityUnitId.HasValue)
            //    {
            //        UnidadesProdutivas productivityUnit = DBProductivityUnits.GetById(productivityUnitId.Value);

            //        int Tipo = (int)data.FirstOrDefault().Tipo;
            //        string Projeto = "";
            //        if (Tipo == 1)
            //            Projeto = productivityUnit.ProjetoCozinha;
            //        if (Tipo == 2)
            //            Projeto = productivityUnit.ProjetoMatSubsidiárias;

            //        if (productivityUnit != null)
            //        {
            //            RequisitionViewModel req = new RequisitionViewModel();
            //            req.TipoReq = (int)RequisitionTypes.Normal;
            //            req.FunctionalAreaCode = productivityUnit.CódigoÁreaFuncional;
            //            req.CenterResponsibilityCode = productivityUnit.CódigoCentroResponsabilidade;
            //            req.RegionCode = productivityUnit.CódigoRegião;
            //            req.LocalCode = productivityUnit.Armazém;
            //            req.UnitFoodProduction = Convert.ToString(productivityUnit.NºUnidadeProdutiva);
            //            req.RequestNutrition = true;
            //            req.RequisitionDate = DateTime.Now.ToString();
            //            req.ReceivedDate = expextedDate != DateTime.MinValue ? expextedDate.ToString() : string.Empty;
            //            req.ProjectNo = Projeto;
            //            req.CreateUser = User.Identity.Name;
            //            req.CreateDate = DateTime.Now.ToString();
            //            req.State = RequisitionStates.Pending;

            //            data.ForEach(item =>
            //            {
            //                if (item.Quantity.HasValue && item.Quantity > 0)
            //                {
            //                    var productsInReq = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, item.ProductNo);

            //                    RequisitionLineViewModel line = new RequisitionLineViewModel
            //                    {
            //                        Type = 2,
            //                        Code = item.ProductNo,
            //                        Description = !string.IsNullOrEmpty(item.Description) ? item.Description.Length >= 100 ? item.Description.Substring(0, 100) : item.Description : "",
            //                        Description2 = !string.IsNullOrEmpty(item.Description2) ? item.Description2.Length >= 50 ? item.Description2.Substring(0, 50) : item.Description2 : "",
            //                        UnitMeasureCode = item.UnitMeasureCode,
            //                        QtyByUnitOfMeasure = item.QuantitybyUnitMeasure,
            //                        QuantityToRequire = item.Quantity,
            //                        QuantidadeDisponivel = item.QuantidadeDisponivel,
            //                        QuantidadeReservada = item.QuantidadeReservada,
            //                        UnitCost = item.DirectUnitCost,
            //                        SupplierNo = item.SupplierNo,
            //                        SubSupplierNo = item.SubSupplierNo,
            //                        ExpectedReceivingDate = item.ExpectedReceptionDate,
            //                        SupplierProductCode = item.SupplierProductCode,
            //                        FunctionalAreaCode = req.FunctionalAreaCode,
            //                        CenterResponsibilityCode = req.CenterResponsibilityCode,
            //                        RegionCode = req.RegionCode,
            //                        CreateUser = User.Identity.Name,
            //                        ProjectNo = Projeto,
            //                        LocalCode = productivityUnit.Armazém,
            //                        VATProductPostingGroup = string.IsNullOrEmpty(item.GrupoRegistoIvaProduto) ? productsInReq.Where(x => x.Code == item.ProductNo).FirstOrDefault().VATProductPostingGroup : item.GrupoRegistoIvaProduto,
            //                        CriarNotaEncomenda = true
            //                    };

            //                    req.Lines.Add(line);
            //                }
            //            });

            //            try
            //            {
            //                //Get VATPostingGroup Info
            //                List<string> productsInRequisitionIds = req.Lines.Select(y => y.Code).Distinct().ToList();
            //                var productsInRequisition = DBNAV2017Products.GetProductsById(_config.NAVDatabaseName, _config.NAVCompanyName, productsInRequisitionIds);
            //                var vendors = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName);

            //                //Set VATPostingGroup Info
            //                req.Lines.ForEach(line =>
            //                {
            //                    line.VATBusinessPostingGroup = vendors.FirstOrDefault(x => x.No_ == line.SupplierNo)?.VATBusinessPostingGroup;

            //                    if (string.IsNullOrEmpty(line.VATProductPostingGroup))
            //                        line.VATProductPostingGroup = productsInRequisition.Where(x => x.Code == line.Code).FirstOrDefault().VATProductPostingGroup;

            //                    if (string.IsNullOrEmpty(line.Description2))
            //                        line.Description2 = productsInRequisition.Where(x => x.Code == line.Code).FirstOrDefault().Name2;

            //                });
            //            }
            //            catch { }


            //            try
            //            {
            //                RequisitionViewModel createdRequisition = CreateRequesition(req);
            //                if (createdRequisition.eReasonCode == 1)
            //                {
            //                    bool deletedSuccessfully = DBShoppingNecessity.Delete(data.ParseToDB());
            //                    if (deletedSuccessfully)
            //                    {
            //                        result.eReasonCode = 1;
            //                        result.eMessage = createdRequisition.eMessage;
            //                    }
            //                    else
            //                    {
            //                        result.eReasonCode = 2;
            //                        result.eMessage = "A requisição foi criada com sucesso, no entanto ocorreu um erro ao eliminar as linhas do diário. Por favor, elimine as linhas manualmente.";
            //                    }
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                result.eReasonCode = 55;
            //                result.eMessage = "Ocorreu um erro ao criar a requisição";
            //                result.eMessages.Add(new TraceInformation(TraceType.Exception, ex.Message));
            //            }
            //        }
            //    }
            //    else
            //    {
            //        result.eReasonCode = 2;
            //        result.eMessage = "Não foi possivel identificar a unidade produtiva.";
            //    }
            //}
            //else
            //{
            //    result.eReasonCode = 2;
            //    result.eMessage = "Ocorreu um erro: A lista esta vazia";
            //}
            //return Json(result);













            ErrorHandler result = new ErrorHandler();

            if (data != null && data.Count > 0)
            {
                List<NAVProductsViewModel> AllProducts = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, string.Empty);
                List<NAVVendorViewModel> AllVendors = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName);

                if (data.Where(x => x.DirectUnitCost == null || x.SupplierNo == null || x.SupplierNo == "").Count() > 0)
                {
                    result.eReasonCode = 2;
                    result.eMessage = "Existe linhas sem custo unitário ou Cód Fornecedor preenchidos.";
                    result.eMessages.Add(new TraceInformation(TraceType.Error, "Existe linhas sem custo unitário ou Cód Fornecedor preenchidos."));
                    return Json(result);
                }

                data.ForEach(item =>
                {
                    NAVProductsViewModel Product = AllProducts.Where(x => x.Code == item.ProductNo).FirstOrDefault();

                    if (Product == null)
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Não é possivel criar a Requisição, por o produto Nº " + item.ProductNo + " - " + item.Description + " " + item.Description2 + " estar bloqueado no NAV2017.";
                        result.eMessages.Add(new TraceInformation(TraceType.Error, "Não é possivel criar a Requisição, por o produto Nº " + item.ProductNo + " - " + item.Description + " " + item.Description2 + " estar bloqueado no NAV2017."));
                    }
                });

                if (result.eReasonCode == 2)
                {
                    return Json(result);
                }

                int? productivityUnitId = data.Where(x => x.ProductionUnitNo.HasValue).Select(x => x.ProductionUnitNo).FirstOrDefault();
                DateTime expextedDate = data.Where(x => !string.IsNullOrEmpty(x.ExpectedReceptionDate)).Select(x => DateTime.Parse(x.ExpectedReceptionDate)).OrderBy(x => x).FirstOrDefault();

                if (productivityUnitId != null && productivityUnitId.HasValue)
                {
                    UnidadesProdutivas productivityUnit = DBProductivityUnits.GetById(productivityUnitId.Value);

                    int Tipo = (int)data.FirstOrDefault().Tipo;
                    string Projeto = "";
                    if (Tipo == 1)
                        Projeto = productivityUnit.ProjetoCozinha;
                    if (Tipo == 2)
                        Projeto = productivityUnit.ProjetoMatSubsidiárias;

                    if (productivityUnit != null)
                    {
                        List<RequisitionViewModel> RequisicoesGroupInterface = new List<RequisitionViewModel>();

                        try
                        {
                            RequisicoesGroupInterface = data.GroupBy(x =>
                            x.Interface,
                            x => x,
                            (key, items) => new RequisitionViewModel
                            {
                                Interface = key,
                                TipoReq = (int)RequisitionTypes.Normal,
                                FunctionalAreaCode = productivityUnit.CódigoÁreaFuncional,
                                CenterResponsibilityCode = productivityUnit.CódigoCentroResponsabilidade,
                                RegionCode = productivityUnit.CódigoRegião,
                                LocalCode = productivityUnit.Armazém,
                                UnitFoodProduction = Convert.ToString(productivityUnit.NºUnidadeProdutiva),
                                RequestNutrition = true,
                                RequisitionDate = DateTime.Now.ToString(),
                                ReceivedDate = expextedDate != DateTime.MinValue ? expextedDate.ToString() : string.Empty,
                                ProjectNo = Projeto,
                                CreateUser = User.Identity.Name,
                                CreateDate = DateTime.Now.ToString(),
                                State = RequisitionStates.Pending,

                                Lines = items.Select(line => new RequisitionLineViewModel()
                                {
                                    Type = 2,
                                    Code = line.ProductNo,
                                    Description = !string.IsNullOrEmpty(line.Description) ? line.Description.Length >= 100 ? line.Description.Substring(0, 100) : line.Description : "",
                                    Description2 = !string.IsNullOrEmpty(line.Description2) ? line.Description2.Length >= 50 ? line.Description2.Substring(0, 50) : line.Description2 : "",
                                    UnitMeasureCode = line.UnitMeasureCode,
                                    QtyByUnitOfMeasure = line.QuantitybyUnitMeasure,
                                    QuantityToRequire = line.Quantity,
                                    QuantidadeDisponivel = line.QuantidadeDisponivel,
                                    QuantidadeReservada = line.QuantidadeReservada,
                                    UnitCost = line.DirectUnitCost,
                                    SupplierNo = line.SupplierNo,
                                    SubSupplierNo = line.SubSupplierNo,
                                    ExpectedReceivingDate = line.ExpectedReceptionDate,
                                    SupplierProductCode = line.SupplierProductCode,
                                    FunctionalAreaCode = productivityUnit.CódigoÁreaFuncional,
                                    CenterResponsibilityCode = productivityUnit.CódigoCentroResponsabilidade,
                                    RegionCode = productivityUnit.CódigoRegião,
                                    CreateUser = User.Identity.Name,
                                    ProjectNo = Projeto,
                                    LocalCode = productivityUnit.Armazém,
                                    VATProductPostingGroup = !string.IsNullOrEmpty(line.GrupoRegistoIvaProduto) ? line.GrupoRegistoIvaProduto : AllProducts.FirstOrDefault(x => x.Code == line.ProductNo)?.VATProductPostingGroup,
                                    VATBusinessPostingGroup = AllVendors.FirstOrDefault(x => x.No_ == line.SupplierNo)?.VATBusinessPostingGroup,
                                    CriarNotaEncomenda = true,
                                    CustoUnitarioSubFornecedor = line.DirectUnitCostSubSupplier,
                                    NoLinhaDiarioRequisicaoUnidProdutiva = line.LineNo
                                }).ToList()
                            }).ToList();
                        }
                        catch
                        {
                            throw new Exception("Ocorreu um erro ao agrupar as linhas.");
                        }

                        if (RequisicoesGroupInterface.Count() > 0)
                        {
                            RequisicoesGroupInterface.ForEach(RequisicaoGroupInterface =>
                            {
                                try
                                {
                                    if (RequisicaoGroupInterface.Interface == 0) //0 = Normal
                                    {
                                        List<RequisitionViewModel> RequisicoesGroupLocalCode = new List<RequisitionViewModel>();

                                        try
                                        {
                                            RequisicoesGroupLocalCode = RequisicaoGroupInterface.Lines.GroupBy(x => 
                                            x.LocalCode,
                                            x => x,
                                            (key, items) => new RequisitionViewModel
                                            {
                                                LocalCode = key,
                                                Interface = RequisicaoGroupInterface.Interface,
                                                SupplierCode = RequisicaoGroupInterface.SupplierCode,
                                                NoSubFornecedor = RequisicaoGroupInterface.NoSubFornecedor,
                                                TipoReq = (int)RequisitionTypes.Normal,
                                                FunctionalAreaCode = productivityUnit.CódigoÁreaFuncional,
                                                CenterResponsibilityCode = productivityUnit.CódigoCentroResponsabilidade,
                                                RegionCode = productivityUnit.CódigoRegião,
                                                UnitFoodProduction = Convert.ToString(productivityUnit.NºUnidadeProdutiva),
                                                RequestNutrition = true,
                                                RequisitionDate = DateTime.Now.ToString(),
                                                ReceivedDate = expextedDate != DateTime.MinValue ? expextedDate.ToString() : string.Empty,
                                                ProjectNo = Projeto,
                                                CreateUser = User.Identity.Name,
                                                CreateDate = DateTime.Now.ToString(),
                                                State = RequisitionStates.Pending,

                                                Lines = items.Select(line => new RequisitionLineViewModel()
                                                {
                                                    Type = 2,
                                                    Code = line.Code,
                                                    Description = !string.IsNullOrEmpty(line.Description) ? line.Description.Length >= 100 ? line.Description.Substring(0, 100) : line.Description : "",
                                                    Description2 = !string.IsNullOrEmpty(line.Description2) ? line.Description2 : AllProducts.FirstOrDefault(x => x.Code == line.Code)?.Name2,
                                                    UnitMeasureCode = line.UnitMeasureCode,
                                                    QtyByUnitOfMeasure = line.QtyByUnitOfMeasure,
                                                    QuantityToRequire = line.QuantityToRequire,
                                                    QuantidadeDisponivel = line.QuantidadeDisponivel,
                                                    QuantidadeReservada = line.QuantidadeReservada,
                                                    UnitCost = line.UnitCost,
                                                    SupplierNo = line.SupplierNo,
                                                    SubSupplierNo = line.SubSupplierNo,
                                                    ExpectedReceivingDate = line.ExpectedReceivingDate,
                                                    SupplierProductCode = line.SupplierProductCode,
                                                    FunctionalAreaCode = productivityUnit.CódigoÁreaFuncional,
                                                    CenterResponsibilityCode = productivityUnit.CódigoCentroResponsabilidade,
                                                    RegionCode = productivityUnit.CódigoRegião,
                                                    CreateUser = User.Identity.Name,
                                                    ProjectNo = Projeto,
                                                    LocalCode = productivityUnit.Armazém,
                                                    VATProductPostingGroup = !string.IsNullOrEmpty(line.VATProductPostingGroup) ? line.VATProductPostingGroup : AllProducts.FirstOrDefault(x => x.Code == line.Code)?.VATProductPostingGroup,
                                                    VATBusinessPostingGroup = AllVendors.FirstOrDefault(x => x.No_ == line.SupplierNo)?.VATBusinessPostingGroup,
                                                    CriarNotaEncomenda = true,
                                                    CustoUnitarioSubFornecedor = line.CustoUnitarioSubFornecedor,
                                                    NoLinhaDiarioRequisicaoUnidProdutiva = line.NoLinhaDiarioRequisicaoUnidProdutiva
                                                }).ToList()
                                            }).ToList();
                                        }
                                        catch
                                        {
                                            throw new Exception("Ocorreu um erro ao agrupar as linhas.");
                                        }

                                        if (RequisicoesGroupLocalCode.Count() > 0)
                                        {
                                            RequisicoesGroupLocalCode.ForEach(RequisicaoGroupLocalCode =>
                                            {
                                                try
                                                {
                                                    RequisicaoGroupLocalCode.NoSubFornecedor = null;
                                                    RequisicaoGroupLocalCode.NoEncomendaFornecedor = null;
                                                    RequisicaoGroupLocalCode.DataEncomendaSubfornecedor = null;

                                                    RequisitionViewModel createdRequisition = CreateRequesition(RequisicaoGroupLocalCode);
                                                    if (createdRequisition.eReasonCode == 1)
                                                    {
                                                        bool deletedSuccessfully = DBShoppingNecessity.Delete(RequisicaoGroupLocalCode.Lines);
                                                        if (deletedSuccessfully)
                                                        {
                                                            result.eReasonCode = 1;
                                                            result.eMessage = result.eMessage + Environment.NewLine + createdRequisition.eMessage;
                                                            result.eMessages.Add(new TraceInformation(TraceType.Success, createdRequisition.eMessage));

                                                        }
                                                        else
                                                        {
                                                            result.eReasonCode = 2;
                                                            result.eMessage = result.eMessage + Environment.NewLine + "A requisição foi criada com sucesso, no entanto ocorreu um erro ao eliminar as linhas do diário. Por favor, elimine as linhas manualmente.";
                                                            result.eMessages.Add(new TraceInformation(TraceType.Error, "A requisição foi criada com sucesso, no entanto ocorreu um erro ao eliminar as linhas do diário. Por favor, elimine as linhas manualmente."));
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    result.eReasonCode = 2;
                                                    result.eMessage = result.eMessage + Environment.NewLine + "Ocorreu um erro ao criar a requisição";
                                                    result.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                                                }
                                            });
                                        }
                                    }

                                    if (RequisicaoGroupInterface.Interface == 1) //1 = NeoValor
                                    {
                                        List<RequisitionViewModel> RequisicoesGroupSupplier = new List<RequisitionViewModel>();

                                        try
                                        {
                                            RequisicoesGroupSupplier = RequisicaoGroupInterface.Lines.GroupBy(x => new
                                            { x.SupplierNo, x.SubSupplierNo, x.LocalCode },
                                            x => x,
                                            (key, items) => new RequisitionViewModel
                                            {
                                                Interface = RequisicaoGroupInterface.Interface,
                                                SupplierCode = key.SupplierNo,
                                                NoSubFornecedor = key.SubSupplierNo,
                                                LocalCode = key.LocalCode,
                                                TipoReq = (int)RequisitionTypes.Normal,
                                                FunctionalAreaCode = productivityUnit.CódigoÁreaFuncional,
                                                CenterResponsibilityCode = productivityUnit.CódigoCentroResponsabilidade,
                                                RegionCode = productivityUnit.CódigoRegião,
                                                UnitFoodProduction = Convert.ToString(productivityUnit.NºUnidadeProdutiva),
                                                RequestNutrition = true,
                                                RequisitionDate = DateTime.Now.ToString(),
                                                ReceivedDate = expextedDate != DateTime.MinValue ? expextedDate.ToString() : string.Empty,
                                                ProjectNo = Projeto,
                                                CreateUser = User.Identity.Name,
                                                CreateDate = DateTime.Now.ToString(),
                                                State = RequisitionStates.Pending,

                                                Lines = items.Select(line => new RequisitionLineViewModel()
                                                {
                                                    Type = 2,
                                                    Code = line.Code,
                                                    Description = !string.IsNullOrEmpty(line.Description) ? line.Description.Length >= 100 ? line.Description.Substring(0, 100) : line.Description : "",
                                                    Description2 = !string.IsNullOrEmpty(line.Description2) ? line.Description2 : AllProducts.FirstOrDefault(x => x.Code == line.Code)?.Name2,
                                                    UnitMeasureCode = line.UnitMeasureCode,
                                                    QtyByUnitOfMeasure = line.QtyByUnitOfMeasure,
                                                    QuantityToRequire = line.QuantityToRequire,
                                                    QuantidadeDisponivel = line.QuantidadeDisponivel,
                                                    QuantidadeReservada = line.QuantidadeReservada,
                                                    UnitCost = line.UnitCost,
                                                    SupplierNo = line.SupplierNo,
                                                    SubSupplierNo = line.SubSupplierNo,
                                                    ExpectedReceivingDate = line.ExpectedReceivingDate,
                                                    SupplierProductCode = line.SupplierProductCode,
                                                    FunctionalAreaCode = productivityUnit.CódigoÁreaFuncional,
                                                    CenterResponsibilityCode = productivityUnit.CódigoCentroResponsabilidade,
                                                    RegionCode = productivityUnit.CódigoRegião,
                                                    CreateUser = User.Identity.Name,
                                                    ProjectNo = Projeto,
                                                    LocalCode = productivityUnit.Armazém,
                                                    VATProductPostingGroup = !string.IsNullOrEmpty(line.VATProductPostingGroup) ? line.VATProductPostingGroup : AllProducts.FirstOrDefault(x => x.Code == line.Code)?.VATProductPostingGroup,
                                                    VATBusinessPostingGroup = AllVendors.FirstOrDefault(x => x.No_ == line.SupplierNo)?.VATBusinessPostingGroup,
                                                    CriarNotaEncomenda = true,
                                                    CustoUnitarioSubFornecedor = line.CustoUnitarioSubFornecedor,
                                                    NoLinhaDiarioRequisicaoUnidProdutiva = line.NoLinhaDiarioRequisicaoUnidProdutiva
                                                }).ToList()
                                            }).ToList();
                                        }
                                        catch
                                        {
                                            throw new Exception("Ocorreu um erro ao agrupar as linhas.");
                                        }

                                        if (RequisicoesGroupSupplier.Count() > 0)
                                        {
                                            RequisicoesGroupSupplier.ForEach(RequisicaoGroupSupplier =>
                                            {
                                                try
                                                {
                                                    RequisitionViewModel createdRequisition = CreateRequesition(RequisicaoGroupSupplier);
                                                    if (createdRequisition.eReasonCode == 1)
                                                    {
                                                        //bool deletedSuccessfully = true;
                                                        bool deletedSuccessfully = DBShoppingNecessity.Delete(RequisicaoGroupSupplier.Lines);
                                                        if (deletedSuccessfully)
                                                        {
                                                            result.eReasonCode = 1;
                                                            result.eMessage = result.eMessage + Environment.NewLine + createdRequisition.eMessage;
                                                            result.eMessages.Add(new TraceInformation(TraceType.Success, createdRequisition.eMessage));
                                                        }
                                                        else
                                                        {
                                                            result.eReasonCode = 2;
                                                            result.eMessage = result.eMessage + Environment.NewLine + "A requisição foi criada com sucesso, no entanto ocorreu um erro ao eliminar as linhas do diário. Por favor, elimine as linhas manualmente.";
                                                            result.eMessages.Add(new TraceInformation(TraceType.Error, "A requisição foi criada com sucesso, no entanto ocorreu um erro ao eliminar as linhas do diário. Por favor, elimine as linhas manualmente."));
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    result.eReasonCode = 2;
                                                    result.eMessage = result.eMessage + Environment.NewLine + "Ocorreu um erro ao criar a requisição";
                                                    result.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                                                }
                                            });
                                        };
                                    };
                                }
                                catch (Exception ex)
                                {
                                    result.eReasonCode = 2;
                                    result.eMessage = result.eMessage + Environment.NewLine + "Ocorreu um erro ao criar a requisição";
                                    result.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                                };
                            });
                        }
                        else
                        {
                            result.eReasonCode = 2;
                            result.eMessage = "Ocorreu um erro ao agrupar as linhas.";
                            result.eMessages.Add(new TraceInformation(TraceType.Error, "Ocorreu um erro ao agrupar as linhas."));
                        }
                    }
                    else
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Não foi possivel obter a Unidade Produtiva.";
                        result.eMessages.Add(new TraceInformation(TraceType.Error, "Não foi possivel obter a Unidade Produtiva."));
                    }
                }
                else
                {
                    result.eReasonCode = 2;
                    result.eMessage = "Não foi possivel identificar a Unidade Produtiva.";
                    result.eMessages.Add(new TraceInformation(TraceType.Error, "Não foi possivel identificar a Unidade Produtiva."));
                }
            }
            else
            {
                result.eReasonCode = 2;
                result.eMessage = "Ocorreu um erro: A lista esta vazia";
                result.eMessages.Add(new TraceInformation(TraceType.Error, "Ocorreu um erro: A lista esta vazia"));
            }

            return Json(result);
        }

        public RequisitionViewModel CreateRequesition(RequisitionViewModel requisition)
        {
            //ErrorHandler data = new ErrorHandler();
            if (requisition != null)
            {
                //Get Contract Numeration
                Configuração Configs = DBConfigurations.GetById(1);
                int ProjectNumerationConfigurationId = 0;
                if (requisition.Interface == null || requisition.Interface == 0)
                    ProjectNumerationConfigurationId = Configs.NumeraçãoRequisições.Value;
                if (requisition.Interface == 1)
                    ProjectNumerationConfigurationId = Configs.NumeracaoRequisicaoFornecedor.Value;

                string RequisitionNo = DBNumerationConfigurations.GetNextNumeration(ProjectNumerationConfigurationId, true, false);
                if (!string.IsNullOrEmpty(RequisitionNo))
                {
                    //Update Last Numeration Used
                    ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);
                    ConfigNumerations.ÚltimoNºUsado = RequisitionNo;
                    ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                    DBNumerationConfigurations.Update(ConfigNumerations);

                    requisition.RequisitionNo = RequisitionNo;
                    requisition.ResponsibleCreation = User.Identity.Name;
                    requisition.EstimatedValue = requisition.Lines.Sum(x => x.QuantityToRequire * x.UnitCost);
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
                        requisition.eMessage = "Requisição criada com sucesso Nº " + RequisitionNo;
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
                            newdp.Descricao2 = x.Description2;
                            newdp.CódUnidadeMedida = x.UnitMeasureCode;
                            newdp.Quantidade = x.Quantity;
                            newdp.QuantidadeDisponivel = x.QuantidadeDisponivel;
                            newdp.QuantidadeReservada = x.QuantidadeReservada;
                            newdp.CustoUnitárioDireto = x.DirectUnitCost;
                            newdp.Valor = x.TotalValue;
                            newdp.NºFornecedor = x.SupplierNo;
                            newdp.NoSubFornecedor = x.SubSupplierNo;
                            newdp.DataHoraModificação = DateTime.Now;
                            newdp.UtilizadorModificação = User.Identity.Name;
                            newdp.CodigoLocalização = x.LocalCode;
                            newdp.NomeFornecedor = x.SupplierName;
                            newdp.NomeSubFornecedor = x.SubSupplierName;
                            newdp.NºEncomendaAberto = x.OpenOrderNo;
                            newdp.NºLinhaEncomendaAberto = x.OrderLineOpenNo;
                            newdp.DescriçãoUnidadeProduto = x.ProductUnitDescription;
                            newdp.GrupoRegistoIvaProduto = x.GrupoRegistoIvaProduto;
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
                                Descricao2 = x.Description2,
                                CódUnidadeMedida = x.UnitMeasureCode,
                                Quantidade = x.Quantity,
                                QuantidadeDisponivel = x.QuantidadeDisponivel,
                                QuantidadeReservada = x.QuantidadeReservada,
                                Valor = x.TotalValue,
                                NºFornecedor = x.SupplierNo,
                                NoSubFornecedor = x.SubSupplierNo,
                                CodigoLocalização = x.LocalCode,
                                NomeFornecedor = x.SupplierName,
                                NomeSubFornecedor = x.SubSupplierName,
                                NºEncomendaAberto = x.OpenOrderNo,
                                NºLinhaEncomendaAberto = x.OrderLineOpenNo,
                                DescriçãoUnidadeProduto = x.ProductUnitDescription,
                                GrupoRegistoIvaProduto = x.GrupoRegistoIvaProduto,
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
            if (data != null && data.Count > 0)
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
                                    resultRq.TipoReq = (int)RequisitionTypes.Normal;
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
                                    resultRq.ResponsávelCriação = User.Identity.Name;
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
                                                resultRqLines.QuantidadeDisponivel = NecShopDirect.QuantidadeDisponivel;
                                                resultRqLines.QuantidadeReservada = NecShopDirect.QuantidadeReservada;
                                                resultRqLines.CustoUnitário = NecShopDirect.DirectUnitCost;
                                                resultRqLines.NºFornecedor = NecShopDirect.SupplierNo;
                                                resultRqLines.NoSubFornecedor = NecShopDirect.SubSupplierNo;
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
            if (rLinesNotCreated > 0)
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


        [HttpPost]
        public JsonResult GetQuantidades(string produto, string armazem)
        {
            //QuantidadesViewModel Quantidades = new QuantidadesViewModel();
            QuantidadesViewModel Quantidades = DBNAV2017Products.GetQuantidades(_config.NAVDatabaseName, _config.NAVCompanyName, produto, armazem);

            //if (!string.IsNullOrEmpty(produto) && !string.IsNullOrEmpty(armazem) && armazem == "4300")
            //{
            //    Task<WSSisLogProd.getStockResponse> TReadStock = WS_SisLogProd.GetSTOCK(armazem, produto);
            //    try
            //    {
            //        TReadStock.Wait();
            //    }
            //    catch (Exception ex)
            //    {
            //        Quantidades.QuantDisponivel = 0;
            //    }
            //    if (TReadStock.Result.stocksActuales != null && TReadStock.Result.stocksActuales.Length > 0)
            //        Quantidades.QuantDisponivel = TReadStock.Result.stocksActuales.FirstOrDefault().cantdisponible;
            //    else
            //        Quantidades.QuantDisponivel = 0;
            //}

            if (Quantidades != null)
                return Json(Quantidades);
            else
                return Json(null);
        }
    }
}