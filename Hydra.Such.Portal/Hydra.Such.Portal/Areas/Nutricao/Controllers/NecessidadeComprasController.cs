using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Nutrition;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.ViewModel.Nutrition;
using Hydra.Such.Data.ViewModel.ProjectView;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Such.Portal.Areas.Nutricao.Controllers
{
    public class NecessidadeComprasController : Controller
    {
        [Area("Nutricao")]
        public IActionResult Detalhes(int id)
        {
            if (id != null && id >0)
            {
                UnidadesProdutivas ProductivityUnitDB = DBProductivityUnits.GetById(id);
                ViewBag.ProductivityUnitId = ProductivityUnitDB.NºUnidadeProdutiva;
                ViewBag.ProductivityUnitDesc = ProductivityUnitDB.Descrição;
            }
            return View();
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult GetGridValues()
        {
            List<DailyRequisitionProductiveUnitViewModel> result = DBShoppingNecessity.GetAll().Select(x => new DailyRequisitionProductiveUnitViewModel()
            {
                LineNo = x.NºLinha,
                Description = x.Descrição,
                CreateDateTime = x.DataHoraCriação,
                CreateUser = x.UtilizadorCriação,
                DateByPriceSupplier = !x.DataPPreçoFornecedor.HasValue ? "" : x.DataPPreçoFornecedor.Value.ToString("yyyy-MM-dd"),
                DirectUnitCost = x.CustoUnitárioDireto,
                ExpectedReceptionDate = !x.DataReceçãoEsperada.HasValue ? "" : x.DataReceçãoEsperada.Value.ToString("yyyy-MM-dd"),
                LocalCode = x.CodigoLocalização,
                MealType = x.TipoRefeição,
                OpenOrderNo = x.NºEncomendaAberto,
                OrderLineOpenNo = x.NºLinhaEncomendaAberto,
                ProductNo = x.NºProduto,
                ProductUnitDescription = x.DescriçãoUnidadeProduto,
                ProductionUnitNo = x.NºUnidadeProdutiva,
                ProjectNo = x.NºProjeto,
                Quantity = x.Quantidade,
                QuantitybyUnitMeasure = x.QuantidadePorUnidMedida,
                SupplierName = x.NomeFornecedor,
                SupplierNo = x.NºFornecedor,
                SupplierProductCode = x.CodigoProdutoFornecedor,
                SupplierProductDescription = x.DescriçãoProdutoFornecedor,
                TableSupplierPrice = x.TabelaPreçosFornecedor,
                TotalValue = x.Valor,
                UnitMeasureCode = x.CódUnidadeMedida,
                UpdateDateTime = x.DataHoraModificação,
                UpdateUser = x.UtilizadorCriação,
                DocumentNo = x.NºDocumento
            }).ToList();
            return Json(result);
        }

    
        [Area("Nutricao")]
        public IActionResult GeracaodeRequisicoes()
        {
            return View();
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult UpdateShoppingNecessity([FromBody] List<DailyRequisitionProductiveUnitViewModel> dp)
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
                        newdp.DataReceçãoEsperada = string.IsNullOrEmpty(x.ExpectedReceptionDate) ? (DateTime?)null : DateTime.Parse(x.ExpectedReceptionDate);
                        newdp.DataPPreçoFornecedor = string.IsNullOrEmpty(x.DateByPriceSupplier) ? (DateTime?)null : DateTime.Parse(x.DateByPriceSupplier);
                        newdp.CodigoLocalização = x.LocalCode;
                        newdp.QuantidadePorUnidMedida = x.QuantitybyUnitMeasure;
                        newdp.CodigoProdutoFornecedor = x.SupplierProductCode;
                        newdp.DescriçãoProdutoFornecedor = x.SupplierProductDescription;
                        newdp.NomeFornecedor = x.SupplierName;
                        newdp.NºEncomendaAberto = x.OpenOrderNo;
                        newdp.NºLinhaEncomendaAberto = x.OrderLineOpenNo;
                        newdp.DescriçãoUnidadeProduto = x.ProductUnitDescription;
                        newdp.NºDocumento = x.DocumentNo;
                        DBShoppingNecessity.Update(newdp);
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
                            DataReceçãoEsperada = string.IsNullOrEmpty(x.ExpectedReceptionDate) ? (DateTime?)null : DateTime.Parse(x.ExpectedReceptionDate),
                            DataPPreçoFornecedor = string.IsNullOrEmpty(x.DateByPriceSupplier) ? (DateTime?)null : DateTime.Parse(x.DateByPriceSupplier),
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
                        DBShoppingNecessity.Create(newdp);
                    }


                });
            }
            catch (Exception e)
            {
                throw;
            }


            return Json(dp);
        }
    }
}