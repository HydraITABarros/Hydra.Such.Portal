using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Approvals
{
    public static class DBApprovalMovements
    {
        #region CRUD
        public static MovimentosDeAprovação GetById(int NºMovimento)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosDeAprovação.Where(x => x.NºMovimento == NºMovimento).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<MovimentosDeAprovação> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosDeAprovação.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static MovimentosDeAprovação Create(MovimentosDeAprovação ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.MovimentosDeAprovação.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static MovimentosDeAprovação Update(MovimentosDeAprovação ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.MovimentosDeAprovação.Update(ObjectToUpdate);
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
