using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.Logic
{
    public class DBUserAccesses
    {
        #region CRUD
        public static AcessosUtilizador GetById(string IdUtilizador, int Funcionalidade)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcessosUtilizador.Where(x => x.IdUtilizador == IdUtilizador && x.Funcionalidade == Funcionalidade).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<AcessosUtilizador> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcessosUtilizador.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static AcessosUtilizador Create(AcessosUtilizador ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.AcessosUtilizador.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static AcessosUtilizador Update(AcessosUtilizador ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.AcessosUtilizador.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(AcessosUtilizador item)
        {
            return Delete(new List<AcessosUtilizador>() { item });
        }

        public static bool Delete(List<AcessosUtilizador> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.AcessosUtilizador.RemoveRange(items);
                    ctx.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static bool DeleteAllFromUser(string UserId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<AcessosUtilizador> UserAccessesToDelete = ctx.AcessosUtilizador.Where(x => x.IdUtilizador == UserId).ToList();
                    ctx.AcessosUtilizador.RemoveRange(UserAccessesToDelete);
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

        public static List<AcessosUtilizador> GetByUserId(string UserId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcessosUtilizador.Where(x => x.IdUtilizador == UserId).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static UserAccessesViewModel GetByUserAreaFunctionality(string userId, Features feature)
        {
            try
            {
                return GetByUserAreaFunctionality(userId, (int)feature);
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static UserAccessesViewModel GetByUserAreaFunctionality(string userId, List<Features> features)
        {
            try
            {
                var items = features.Select(x => (int)x).ToList();
                return GetByUserAreaFunctionality(userId, items);
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        private static UserAccessesViewModel GetByUserAreaFunctionality(string userId, int featureId)
        {
            return GetByUserAreaFunctionality(userId, new List<int>() { featureId });
        }

        private static UserAccessesViewModel GetByUserAreaFunctionality(string UserId, List<int> features)
        {
            UserAccessesViewModel userAccess = new UserAccessesViewModel()
            {
                IdUser = UserId,
                Feature = 0,
                Create = false,
                Read = false,
                Update = false,
                Delete = false
            };

            try
            {
                //TODO: Remover area
                using (var ctx = new SuchDBContext())
                {
                    ConfigUtilizadores CUser = DBUserConfigurations.GetById(UserId);
                    if (CUser.Administrador)
                    {
                        userAccess.Feature = features.FirstOrDefault();
                        userAccess.Create = true;
                        userAccess.Read = true;
                        userAccess.Update = true;
                        userAccess.Delete = true;
                    }
                    else
                    {
                        var userAccessess = ctx.AcessosUtilizador.Where(x => x.IdUtilizador.ToLower() == UserId.ToLower()).Where(x => features.Contains(x.Funcionalidade)).ToList();
                        if (userAccessess.Count > 0)
                        {
                            userAccess = ParseToViewModel(userAccessess.FirstOrDefault());
                            userAccess.Create = userAccessess.Any(x => x.Inserção.Value);
                            userAccess.Read = userAccessess.Any(x => x.Leitura.Value);
                            userAccess.Update = userAccessess.Any(x => x.Modificação.Value);
                            userAccess.Delete = userAccessess.Any(x => x.Eliminação.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return userAccess;
        }

        public static UserAccessesViewModel ParseToViewModel(AcessosUtilizador x)
        {
            if (x == null)
            {
                return new UserAccessesViewModel()
                {
                    Create = false,
                    Read = false,
                    Update = false,
                    Delete = false
                };
            }
            else
            {
                return new UserAccessesViewModel()
                {
                    IdUser = x.IdUtilizador,
                    Area = (int?)x.Área,
                    Feature = x.Funcionalidade,
                    Create = x.Inserção,
                    Read = x.Leitura,
                    Update = x.Modificação,
                    Delete = x.Eliminação
                };
            }

        }
    }
}
