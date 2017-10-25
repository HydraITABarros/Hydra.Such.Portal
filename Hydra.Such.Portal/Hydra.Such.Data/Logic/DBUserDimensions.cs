using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public class DBUserDimensions
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

        public static AcessosDimensões Create(AcessosDimensões objectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.AcessosDimensões.Add(objectToCreate);
                    ctx.SaveChanges();
                }

                return objectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static AcessosDimensões Update(AcessosDimensões objectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.AcessosDimensões.Update(objectToCreate);
                    ctx.SaveChanges();
                }

                return objectToCreate;
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

        public static List<AcessosDimensões> GetByUserId(string userId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcessosDimensões.Where(x => x.IdUtilizador == userId).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static UserDimensionsViewModel ParseToViewModel(AcessosDimensões x)
        {
            return new UserDimensionsViewModel()
            {
                UserId = x.IdUtilizador,
                Dimension = x.Dimensão,
                DimensionValue = x.ValorDimensão
            };
        }

        public static AcessosDimensões ParseToDB(UserDimensionsViewModel x)
        {
            return new AcessosDimensões()
            {
                IdUtilizador = x.UserId,
                Dimensão = x.Dimension,
                ValorDimensão = x.DimensionValue
            };
        }
    }
}
