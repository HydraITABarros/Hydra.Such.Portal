using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBAccessProfiles
    {
        #region CRUD
        public static AcessosPerfil GetById(int IdPerfil, int Área, int Funcionalidade)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcessosPerfil.Where(x => x.IdPerfil == IdPerfil && x.Área == Área && x.Funcionalidade == Funcionalidade).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<AcessosPerfil> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcessosPerfil.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static AcessosPerfil Create(AcessosPerfil ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.AcessosPerfil.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static AcessosPerfil Update(AcessosPerfil ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.AcessosPerfil.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(AcessosPerfil item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //List<AcessosPerfil> ProfileAccessesToDelete = ctx.AcessosPerfil.Where(x => x.IdPerfil == ProfileId).ToList();
                    ctx.AcessosPerfil.Remove(item);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool DeleteAllFromProfile(int ProfileId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<AcessosPerfil> ProfileAccessesToDelete = ctx.AcessosPerfil.Where(x => x.IdPerfil == ProfileId).ToList();
                    ctx.AcessosPerfil.RemoveRange(ProfileAccessesToDelete);
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

        public static List<AcessosPerfil> GetByProfileModelId(int ProfileModelId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcessosPerfil.Where(x => x.IdPerfil == ProfileModelId).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        #region Parse Utilities
        public static AccessProfileModelView ParseToViewModel(this AcessosPerfil item)
        {
            if (item != null)
            {
                return new AccessProfileModelView()
                {
                    IdProfile = item.IdPerfil,
                    Feature = item.Funcionalidade,
                    Create = item.Inserção,
                    Delete = item.Eliminação,
                    Read = item.Leitura,
                    Update = item.Leitura,
                    CreateDate = item.DataHoraCriação,
                    Area = item.Área,
                    CreateUser = item.UtilizadorCriação,
                    UpdateDate = item.DataHoraModificação,
                    UpdateUser = item.UtilizadorModificação,
                };
            }
            return null;
        }

        public static List<AccessProfileModelView> ParseToViewModel(this List<AcessosPerfil> items)
        {
            List<AccessProfileModelView> parsedItems = new List<AccessProfileModelView>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }

        public static AcessosPerfil ParseToDB(this AccessProfileModelView item)
        {
            if (item != null)
            {
                return new AcessosPerfil()
                {
                    IdPerfil = item.IdProfile,
                    Funcionalidade = item.Feature,
                    Eliminação = item.Delete,
                    Inserção = item.Create,
                    Leitura = item.Read,
                    Modificação = item.Update,
                    DataHoraCriação = item.CreateDate,
                    DataHoraModificação = item.UpdateDate,
                    UtilizadorCriação = item.CreateUser,
                    UtilizadorModificação = item.UpdateUser,
                    Área = item.Area,
                };
            }
            return null;
        }

        public static List<AcessosPerfil> ParseToDB(this List<AccessProfileModelView> items)
        {
            List<AcessosPerfil> parsedItems = new List<AcessosPerfil>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
        #endregion
    }
}
