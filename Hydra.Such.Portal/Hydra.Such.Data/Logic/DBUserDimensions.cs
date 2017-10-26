using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public static class DBUserDimensions
    {
        #region CRUD
        public static AcessosDimensões GetById(string userId, int dimension, string dimensionValue)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcessosDimensões.Where(x => x.IdUtilizador == userId && x.Dimensão == dimension && x.ValorDimensão == dimensionValue).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<AcessosDimensões> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcessosDimensões.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static AcessosDimensões Create(AcessosDimensões item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.AcessosDimensões.Add(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<AcessosDimensões> Create(List<AcessosDimensões> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(x => ctx.AcessosDimensões.Add(x));
                    ctx.SaveChanges();
                }

                return items;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">Utilizar nos casos em que o id do utilizador não é definido na camada de interface</param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static List<AcessosDimensões> Create(string userId, List<AcessosDimensões> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(x =>
                        {
                            x.IdUtilizador = userId;
                            ctx.AcessosDimensões.Add(x);
                        });
                    ctx.SaveChanges();
                }

                return items;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static AcessosDimensões Update(AcessosDimensões item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.AcessosDimensões.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(string userId, int dimension, string dimensionValue)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    AcessosDimensões userDimension = ctx.AcessosDimensões.Where(x => x.IdUtilizador == userId && x.Dimensão == dimension && x.ValorDimensão == dimensionValue).FirstOrDefault();
                    if (userDimension != null)
                    {
                        ctx.AcessosDimensões.Remove(userDimension);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public static bool DeleteAllFromUser(string userId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<AcessosDimensões> userAccessesToDelete = ctx.AcessosDimensões.Where(x => x.IdUtilizador == userId).ToList();
                    ctx.AcessosDimensões.RemoveRange(userAccessesToDelete);
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

        public static List<UserDimensionsViewModel> GetByUserId(string userId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcessosDimensões.Where(x => x.IdUtilizador == userId)
                        .ToList()
                        .ParseToViewModel();
                }
            }
            catch (Exception ex)
            {
                
            }
            return new List<UserDimensionsViewModel>();
        }

        public static UserDimensionsViewModel ParseToViewModel(this AcessosDimensões dimensionAccess)
        {
            return new UserDimensionsViewModel()
            {
                UserId = dimensionAccess.IdUtilizador,
                Dimension = dimensionAccess.Dimensão,
                DimensionValue = dimensionAccess.ValorDimensão
            };
        }

        public static List<UserDimensionsViewModel> ParseToViewModel(this List<AcessosDimensões> dimensionAccess)
        {
            List<UserDimensionsViewModel> userDimensions = new List<UserDimensionsViewModel>();
            dimensionAccess.ForEach(x =>
                userDimensions.Add(x.ParseToViewModel()));
            return userDimensions;
        }

        public static AcessosDimensões ParseToDB(this UserDimensionsViewModel userDimension)
        {
            return new AcessosDimensões()
            {
                IdUtilizador = userDimension.UserId,
                Dimensão = userDimension.Dimension,
                ValorDimensão = userDimension.DimensionValue
            };
        }

        public static List<AcessosDimensões> ParseToDB(this List<UserDimensionsViewModel> userDimensions)
        {
            List<AcessosDimensões> dimensionAccess = new List<AcessosDimensões>();
            userDimensions.ForEach(x =>
                dimensionAccess.Add(x.ParseToDB()));
            return dimensionAccess;
        }
    }
}
