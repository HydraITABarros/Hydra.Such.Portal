using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Compras;
using Hydra.Such.Data.ViewModel.Compras;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Compras
{
    public class DBCompras
    {
        public static List<ComprasModel> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Compras.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ComprasModel GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Compras.Where(x => x.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ComprasModel Create(ComprasModel ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Compras.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ComprasModel Update(ComprasModel ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Compras.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool Delete(ComprasModel ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Compras.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}