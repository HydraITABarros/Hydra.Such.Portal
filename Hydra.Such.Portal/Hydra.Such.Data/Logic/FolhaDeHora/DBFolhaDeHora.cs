using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBFolhaDeHora
    {
        #region CRUD

        public static List<FolhasDeHoras> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.FolhasDeHoras.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static FolhasDeHoras Create(FolhasDeHoras ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.FolhasDeHoras.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static FolhasDeHoras Update(FolhasDeHoras ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.FolhasDeHoras.Update(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch
            {
                return null;
            }
        }

        public static bool Delete(FolhasDeHoras ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.FolhasDeHoras.Remove(ObjectToCreate);
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
    }
}
