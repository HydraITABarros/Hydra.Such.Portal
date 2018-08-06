using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Nutrition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public static class DBUnitMeasureProduct
    {
        #region CRUD
        public static UnidadeMedidaProduto GetById(string NºProduto)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UnidadeMedidaProduto.Where(x => x.NºProduto == NºProduto).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static UnidadeMedidaProduto GetByProdCode(string NºProduto, string Cod)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UnidadeMedidaProduto.Where(x => x.NºProduto == NºProduto && x.Código==Cod).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<UnidadeMedidaProduto> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UnidadeMedidaProduto.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static UnidadeMedidaProduto Create(UnidadeMedidaProduto ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.UnidadeMedidaProduto.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }
                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static UnidadeMedidaProduto Update(UnidadeMedidaProduto ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.UnidadeMedidaProduto.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool Delete(UnidadeMedidaProduto objectToDelete)
        {
            return Delete(new List<UnidadeMedidaProduto>() { objectToDelete });
        }

        public static bool Delete(List<UnidadeMedidaProduto> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.UnidadeMedidaProduto.RemoveRange(items);
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

        public static UnidadeMedidaProduto ParseToDb(UnitMeasureProductViewModel x)
        {
            return new UnidadeMedidaProduto()
            {
                NºProduto = x.ProductNo,
                Código = x.Code,
                QtdPorUnidadeMedida = x.QtdUnitMeasure,
                Comprimento = x.Length,
                Largura = x.Width,
                Altura = x.Heigth,
                Cubagem = x.Cubage,
                Peso = x.Weight,
                DataHoraCriação = x.CreateDate,
                UtilizadorCriação = x.CreateUser,
                DataHoraModificação = x.UpdateDate,
                UtilizadorModificação = x.UpdateUser
            };
        }

        public static UnitMeasureProductViewModel ParseToViewModel(this UnidadeMedidaProduto item)
        {
            if (item != null)
            {
                return new UnitMeasureProductViewModel()
                {
                   ProductNo = item.NºProduto,
                   Code = item.Código,
                   QtdUnitMeasure = item.QtdPorUnidadeMedida,
                   Length = item.Comprimento,
                   Width = item.Largura,
                   Heigth = item.Altura,
                   Cubage = item.Cubagem,
                   Weight = item.Peso,
                   CreateDate = item.DataHoraCriação,
                   CreateUser = item.UtilizadorCriação,
                   UpdateDate = item.DataHoraModificação,
                   UpdateUser = item.UtilizadorModificação
                };
            }
            return null;
        }

        public static List<UnitMeasureProductViewModel> ParseToViewModel(this List<UnidadeMedidaProduto> items)
        {
            List<UnitMeasureProductViewModel> parsedItems = new List<UnitMeasureProductViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(ParseToViewModel(x)));
            return parsedItems;
        }
    }
}
