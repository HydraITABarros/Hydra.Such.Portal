using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Contracts
{
    public static class DBContracts
    {
        #region CRUD
        public static Contratos GetById(string ContractNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Contratos.Where(x => x.NºContrato == ContractNo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<Contratos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Contratos.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Contratos Create(Contratos ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.Contratos.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Contratos Update(Contratos ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.Contratos.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        #endregion
    }
}
