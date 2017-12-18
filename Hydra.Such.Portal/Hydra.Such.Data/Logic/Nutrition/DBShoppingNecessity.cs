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
      

        public static List<DiárioRequisiçãoUnidProdutiva> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioRequisiçãoUnidProdutiva.ToList();
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
        public static bool Delete(DiárioRequisiçãoUnidProdutiva ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.DiárioRequisiçãoUnidProdutiva.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static List<DiárioRequisiçãoUnidProdutiva> GetByLineNo(int LineNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioRequisiçãoUnidProdutiva.Where(x => x.NºLinha == LineNo).ToList();
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
                    DocumentNo = item.NºDocumento
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
        #endregion
    }
}
