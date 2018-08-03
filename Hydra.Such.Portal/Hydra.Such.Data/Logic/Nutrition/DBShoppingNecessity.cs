using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Nutrition;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public static class DBShoppingNecessity
    {
        #region CRUD
        public static List<DiárioRequisiçãoUnidProdutiva> GetAllById(int NºUnidadeProdutiva)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioRequisiçãoUnidProdutiva.Where(x => x.NºUnidadeProdutiva == NºUnidadeProdutiva).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static List<DiárioRequisiçãoUnidProdutiva> GetAllDirectById(int NºUnidadeProdutiva)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioRequisiçãoUnidProdutiva.Where(x => x.NºUnidadeProdutiva == NºUnidadeProdutiva && x.DataPPreçoFornecedor == null).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static List<DiárioRequisiçãoUnidProdutiva> GetAllWithoutPriceSup()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioRequisiçãoUnidProdutiva.Where(x => x.DataPPreçoFornecedor == null).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<DiárioRequisiçãoUnidProdutiva> GetByLineNoWithoutPriceSup(int LineNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioRequisiçãoUnidProdutiva.Where(x => x.NºLinha == LineNo && x.DataPPreçoFornecedor == null).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<DiárioRequisiçãoUnidProdutiva> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioRequisiçãoUnidProdutiva.Where(x => x.DataPPreçoFornecedor != null).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DiárioRequisiçãoUnidProdutiva Create(DiárioRequisiçãoUnidProdutiva ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.DiárioRequisiçãoUnidProdutiva.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static DiárioRequisiçãoUnidProdutiva Update(DiárioRequisiçãoUnidProdutiva ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.DiárioRequisiçãoUnidProdutiva.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(List<DiárioRequisiçãoUnidProdutiva> itemsToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.DiárioRequisiçãoUnidProdutiva.RemoveRange(itemsToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static bool Delete(DiárioRequisiçãoUnidProdutiva ObjectToDelete)
        {
            List<DiárioRequisiçãoUnidProdutiva> itemsToDelete = new List<DiárioRequisiçãoUnidProdutiva>();
            itemsToDelete.Add(ObjectToDelete);
            return Delete(itemsToDelete);
        }

        public static List<DiárioRequisiçãoUnidProdutiva> GetByLineNo(int LineNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioRequisiçãoUnidProdutiva.Where(x => x.NºLinha == LineNo && x.DataPPreçoFornecedor != null).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion


        #region Parse Utilities
        public static DailyRequisitionProductiveUnitViewModel ParseToViewModel(this DiárioRequisiçãoUnidProdutiva item)
        {
            if (item != null)
            {
                return new DailyRequisitionProductiveUnitViewModel()
                {
                    id = item.NºEncomendaAberto + " " + item.NºLinhaEncomendaAberto + " " + item.NºProduto,
                    LineNo = item.NºLinha,
                    Description = item.Descrição,
                    CreateDateTime = item.DataHoraCriação,
                    CreateUser = item.UtilizadorCriação,
                    DateByPriceSupplier = !item.DataPPreçoFornecedor.HasValue ? "" : item.DataPPreçoFornecedor.Value.ToString("yyyy-MM-dd"),
                    DirectUnitCost = item.CustoUnitárioDireto,
                    ExpectedReceptionDate = !item.DataReceçãoEsperada.HasValue ? "" : item.DataReceçãoEsperada.Value.ToString("yyyy-MM-dd"),
                    LocalCode = item.CodigoLocalização,
                    MealType = item.TipoRefeição,
                    OpenOrderNo = item.NºEncomendaAberto,
                    OrderLineOpenNo = item.NºLinhaEncomendaAberto,
                    ProductNo = item.NºProduto,
                    ProductUnitDescription = item.DescriçãoUnidadeProduto,
                    ProductionUnitNo = item.NºUnidadeProdutiva,
                    ProjectNo = item.NºProjeto,
                    Quantity = item.Quantidade,
                    QuantitybyUnitMeasure = item.QuantidadePorUnidMedida,
                    SupplierName = item.NomeFornecedor,
                    SupplierNo = item.NºFornecedor,
                    SupplierProductCode = item.CodigoProdutoFornecedor,
                    SupplierProductDescription = item.DescriçãoProdutoFornecedor,
                    TableSupplierPrice = item.TabelaPreçosFornecedor,
                    TotalValue = item.Valor,
                    UnitMeasureCode = item.CódUnidadeMedida,
                    UpdateDateTime = item.DataHoraModificação,
                    UpdateUser = item.UtilizadorCriação,
                    DocumentNo = item.NºDocumento,
                    Observation = item.Observações
                };
            }
            return null;
        }
        public static List<DailyRequisitionProductiveUnitViewModel> ParseToViewModel(this List<DiárioRequisiçãoUnidProdutiva> items)
        {
            List<DailyRequisitionProductiveUnitViewModel> parsedItems = new List<DailyRequisitionProductiveUnitViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }


        public static DiárioRequisiçãoUnidProdutiva ParseToDB(this DailyRequisitionProductiveUnitViewModel item)
        {
            if (item != null)
            {
                return new DiárioRequisiçãoUnidProdutiva()
                {
                    //id = item.NºEncomendaAberto + " " + item.NºLinhaEncomendaAberto + " " + item.NºProduto,
                    NºLinha = item.LineNo,
                    Descrição = item.Description,
                    DataHoraCriação = item.CreateDateTime,
                    UtilizadorCriação = item.CreateUser,
                    DataPPreçoFornecedor =  DateTime.Parse(item.DateByPriceSupplier),
                    CustoUnitárioDireto = item.DirectUnitCost,
                    DataReceçãoEsperada = DateTime.Parse(item.ExpectedReceptionDate),
                    CodigoLocalização = item.LocalCode,
                    TipoRefeição = item.MealType,
                    NºEncomendaAberto = item.OpenOrderNo,
                    NºLinhaEncomendaAberto = item.OrderLineOpenNo,
                    NºProduto = item.ProductNo,
                    DescriçãoUnidadeProduto = item.ProductUnitDescription,
                    NºUnidadeProdutiva = item.ProductionUnitNo,
                    NºProjeto = item.ProjectNo,
                    Quantidade = item.Quantity,
                    QuantidadePorUnidMedida = item.QuantitybyUnitMeasure,
                    NomeFornecedor = item.SupplierName,
                    NºFornecedor = item.SupplierNo,
                    CodigoProdutoFornecedor = item.SupplierProductCode,
                    DescriçãoProdutoFornecedor = item.SupplierProductDescription,
                    TabelaPreçosFornecedor = item.TableSupplierPrice,
                    Valor = item.TotalValue,
                    CódUnidadeMedida = item.UnitMeasureCode,
                    DataHoraModificação = item.UpdateDateTime,
                    UtilizadorModificação = item.UpdateUser,
                    NºDocumento = item.DocumentNo,
                    Observações = item.Observation
                };
            }
            return null;
        }

        public static List<DiárioRequisiçãoUnidProdutiva> ParseToDB(this List<DailyRequisitionProductiveUnitViewModel> items)
        {
            List<DiárioRequisiçãoUnidProdutiva> parsedItems = new List<DiárioRequisiçãoUnidProdutiva>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }


        #endregion
    }
}
