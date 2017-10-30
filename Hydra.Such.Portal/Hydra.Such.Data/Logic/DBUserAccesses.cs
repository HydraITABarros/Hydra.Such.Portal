using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBUserAccesses
    {
        #region CRUD
        public static AcessosUtilizador GetById(string IdUtilizador, int Área, int Funcionalidade)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcessosUtilizador.Where(x => x.IdUtilizador == IdUtilizador && x.Área == Área && x.Funcionalidade == Funcionalidade).FirstOrDefault();
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


        public static UserAccessesViewModel ParseToViewModel(AcessosUtilizador x)
        {
            return new UserAccessesViewModel()
            {
                IdUser = x.IdUtilizador,
                Area = x.Área,
                Feature = x.Funcionalidade,
                Create = x.Inserção,
                Read = x.Leitura,
                Update = x.Modificação,
                Delete = x.Eliminação
            };
        }
    }
}
