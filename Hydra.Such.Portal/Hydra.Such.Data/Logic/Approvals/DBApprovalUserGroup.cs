using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Approvals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Approvals
{
    public static class DBApprovalUserGroup
    {
        #region CRUD
        public static UtilizadoresGruposAprovação GetById(int group, string user)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UtilizadoresGruposAprovação.Where(x => x.GrupoAprovação == group && x.UtilizadorAprovação == user).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<UtilizadoresGruposAprovação> GetByGroup(int group)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UtilizadoresGruposAprovação.Where(x => x.GrupoAprovação == group).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<UtilizadoresGruposAprovação> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UtilizadoresGruposAprovação.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static UtilizadoresGruposAprovação Create(UtilizadoresGruposAprovação ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.UtilizadoresGruposAprovação.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static UtilizadoresGruposAprovação Update(UtilizadoresGruposAprovação ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.UtilizadoresGruposAprovação.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static bool Delete(UtilizadoresGruposAprovação ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.UtilizadoresGruposAprovação.Remove(ObjectToDelete);
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

        public static List<string> GetAllFromGroup(int groupId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UtilizadoresGruposAprovação.Where(x => x.GrupoAprovação == groupId).Select(x => x.UtilizadorAprovação).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        #region Parses
        public static UtilizadoresGruposAprovação ParseToDb(ApprovalUserGroupViewModel x)
        {
            return new UtilizadoresGruposAprovação()
            {
                UtilizadorAprovação=x.ApprovalUser,
                GrupoAprovação=x.ApprovalGroup,
                DataHoraCriação=x.CreateDate,
                DataHoraModificação=x.UpdateDate,
                UtilizadorCriação=x.CreateUser,
                UtilizadorModificação=x.UpdateUser
            };
        }

        public static ApprovalUserGroupViewModel ParseToViewModel(this UtilizadoresGruposAprovação x)
        {
            if (x != null)
            {
                return new ApprovalUserGroupViewModel()
                {
                    ApprovalGroup = x.GrupoAprovação,
                    ApprovalUser = x.UtilizadorAprovação,
                    CreateDate = x.DataHoraCriação,
                    CreateUser = x.UtilizadorCriação,
                    UpdateDate = x.DataHoraModificação,
                    UpdateUser = x.UtilizadorModificação
                };
            }
            return null;
        }

       
        public static List<ApprovalUserGroupViewModel> ParseToViewModel(this List<UtilizadoresGruposAprovação> items)
        {
            List<ApprovalUserGroupViewModel> parsedItems = new List<ApprovalUserGroupViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }
        #endregion
    }
}
