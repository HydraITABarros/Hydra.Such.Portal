using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Nutrition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public static class DBUnidadeMedida
    {
        public static List<UnidadeMedida> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UnidadeMedida.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static UnidadeMedida Create(UnidadeMedida ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.UnidadeMedida.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static UnidadeMedida Update(UnidadeMedida ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.UnidadeMedida.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(UnidadeMedida ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.UnidadeMedida.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static UnidadeMedida GetById(string Code)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UnidadeMedida
                        .FirstOrDefault(x => x.Code == Code);
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        #region Parses

        public static UnidadeMedidaViewModel ParseToViewModel(UnidadeMedida x)
        {
            if (x != null)
            {
                return new UnidadeMedidaViewModel()
                {
                    Code = x.Code,
                    Description = x.Description,
                    CreateDate = x.DataHoraCriação,
                    CreateUser = x.UtilizadorCriação,
                    UpdateDate = x.DataHoraModificação,
                    UpdateUser = x.UtilizadorCriação
                };
            }
            return null;
        }

        public static List<UnidadeMedidaViewModel> ParseToViewModel(this List<UnidadeMedida> items)
        {
            List<UnidadeMedidaViewModel> locations = new List<UnidadeMedidaViewModel>();
            if (items != null)
                items.ForEach(x =>
                    locations.Add(ParseToViewModel(x)));
            return locations;
        }

        public static UnidadeMedida ParseToDatabase(UnidadeMedidaViewModel x)
        {
            return new UnidadeMedida()
            {
                Code = x.Code,
                Description = x.Description,
                DataHoraCriação = x.CreateDate,
                UtilizadorCriação = x.CreateUser,
                DataHoraModificação = x.UpdateDate,
                UtilizadorModificação = x.UpdateUser
            };
        }

        public static List<UnidadeMedida> ParseToDatabase(this List<UnidadeMedidaViewModel> items)
        {
            List<UnidadeMedida> locations = new List<UnidadeMedida>();
            if (items != null)
                items.ForEach(x =>
                    locations.Add(ParseToDatabase(x)));
            return locations;
        }

        #endregion
    }
}
