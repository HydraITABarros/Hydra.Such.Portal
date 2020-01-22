using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Approvals;
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

        public static List<UtilizadoresMovimentosDeAprovação> GetById(int NºMovimento)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UtilizadoresMovimentosDeAprovação.Where(x => x.NºMovimento == NºMovimento).ToList();
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

        public static bool Delete(UtilizadoresMovimentosDeAprovação ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.UtilizadoresMovimentosDeAprovação.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool DeleteFromMovementExcept(int movementNo, string user)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.UtilizadoresMovimentosDeAprovação.RemoveRange(ctx.UtilizadoresMovimentosDeAprovação.Where(y => y.NºMovimento == movementNo && y.Utilizador != user));
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



        #region Parses
        public static UserApprovalMovementViewModel ParseToViewModel(UtilizadoresMovimentosDeAprovação x)
        {
            return new UserApprovalMovementViewModel()
            {
                MovementNo = x.NºMovimento,
                UserId = x.Utilizador
            };
        }
        public static UtilizadoresMovimentosDeAprovação ParseToDatabase(UserApprovalMovementViewModel x)
        {
            return new UtilizadoresMovimentosDeAprovação()
            {
                NºMovimento = x.MovementNo,
                Utilizador = x.UserId
            };
        }
        #endregion
    }
}
