using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Nutrition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public static class DBStockkeepingUnit
    {
        #region CRUD
        public static UnidadeDeArmazenamento GetById(string NºProduto)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UnidadeDeArmazenamento.Where(x => x.NºProduto == NºProduto).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
     

        public static List<UnidadeDeArmazenamento> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UnidadeDeArmazenamento.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static UnidadeDeArmazenamento Create(UnidadeDeArmazenamento ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    
                    ctx.UnidadeDeArmazenamento.Add(ObjectToCreate);
                    ctx.SaveChanges();

                }
                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static UnidadeDeArmazenamento Update(UnidadeDeArmazenamento ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.UnidadeDeArmazenamento.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

   

        public static bool Delete(UnidadeDeArmazenamento items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.UnidadeDeArmazenamento.RemoveRange(items);
                    ctx.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        public static UnidadeDeArmazenamento ParseToDb(StockkeepingUnitViewModel x)
        {
            if (x != null)
            {
                return new UnidadeDeArmazenamento()
                {
                    NºProduto = x.ProductNo,
                    CódLocalização = x.Code,
                    Descrição = x.Description,
                    Inventário = x.Inventory,
                    Bloqueado = x.Blocked,
                    CódUnidadeMedidaProduto = x.CodeUnitMeasure,
                    CustoUnitário = x.UnitCost,
                    ValorEmArmazem = x.WareHouseValue,
                    ArmazémPrincipal = x.CodeWareHouse,
                    NºPrateleira = x.ShelfNo,
                    NºFornecedor = x.VendorNo,
                    CódProdForn = x.VendorItemNo,
                    UltimoCustoDirecto = x.LastCostDirect,
                    CódCategoriaProduto = x.CodeProcuctCategory,
                    CódGrupoProduto = x.CodeProcuctGroup,
                    PreçoDeVenda = x.PriceSale
                };
            }
            else
                return null;
        }

        public static StockkeepingUnitViewModel ParseToViewModel(this UnidadeDeArmazenamento item)
        {
            if (item != null)
            {
                return new StockkeepingUnitViewModel()
                {
                    ProductNo = item.NºProduto,
                    Code = item.CódLocalização,
                    Description = item.Descrição,
                    Inventory = item.Inventário,
                    Blocked = item.Bloqueado,
                    CodeUnitMeasure = item.CódUnidadeMedidaProduto,
                    UnitCost = item.CustoUnitário,
                    WareHouseValue = item.ValorEmArmazem,
                    CodeWareHouse = item.ArmazémPrincipal,
                    ShelfNo = item.NºPrateleira,
                    VendorNo = item.NºFornecedor,
                    VendorItemNo = item.CódProdForn,
                    LastCostDirect = item.UltimoCustoDirecto,
                    CodeProcuctCategory = item.CódCategoriaProduto,
                    CodeProcuctGroup = item.CódGrupoProduto,
                    PriceSale = item.PreçoDeVenda
                };
            }
            return null;
        }

        public static List<StockkeepingUnitViewModel> ParseToViewModel(this List<UnidadeDeArmazenamento> items)
        {
            List<StockkeepingUnitViewModel> parsedItems = new List<StockkeepingUnitViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }
    }
}
