using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Approvals
{
    public static class DBUserApprovalMovements
    {
        #region CRUD
        public static UtilizadoresMovimentosDeAprovação GetById(int NºMovimento, string UserId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UtilizadoresMovimentosDeAprovação.Where(x => x.NºMovimento == NºMovimento && x.Utilizador == UserId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<UtilizadoresMovimentosDeAprovação> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UtilizadoresMovimentosDeAprovação.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static UtilizadoresMovimentosDeAprovação Create(UtilizadoresMovimentosDeAprovação ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.UtilizadoresMovimentosDeAprovação.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static UtilizadoresMovimentosDeAprovação Update(UtilizadoresMovimentosDeAprovação ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.UtilizadoresMovimentosDeAprovação.Update(ObjectToUpdate);
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
