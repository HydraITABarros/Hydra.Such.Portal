using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Approvals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Approvals
{
    public static class DBApprovalGroups
    {
        #region CRUD
        public static GruposAprovação GetById(int id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.GruposAprovação.Where(x => x.Código == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static GruposAprovação GetByDescricao(string descricao)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.GruposAprovação.Where(x => x.Descrição == descricao).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<GruposAprovação> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.GruposAprovação.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static GruposAprovação Create(GruposAprovação ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.GruposAprovação.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static GruposAprovação Update(GruposAprovação ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.GruposAprovação.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(GruposAprovação ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.GruposAprovação.Remove(ObjectToDelete);
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
        public static ApprovalGroupViewModel ParseToViewModel(this GruposAprovação x)
        {

            if (x != null)
            {
                return new ApprovalGroupViewModel()
                {
                    Code = x.Código,
                    Description = x.Descrição,
                    CreateDate = x.DataHoraCriação,
                    CreateUser = x.UtilizadorCriação,
                    UpdateDate = x.DataHoraModificação,
                    UpdateUser = x.UtilizadorModificação
                };
            }
            return null;
        }

        public static List<ApprovalGroupViewModel> ParseToViewModel(this List<GruposAprovação> items)
        {
            List<ApprovalGroupViewModel> parsedItems = new List<ApprovalGroupViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }

        public static GruposAprovação ParseToDatabase(ApprovalGroupViewModel x)
        {
            return new GruposAprovação()
            {
                Código = x.Code,
                Descrição = x.Description,
                DataHoraCriação = x.CreateDate,
                UtilizadorCriação = x.CreateUser,
                DataHoraModificação = x.UpdateDate,
                UtilizadorModificação = x.UpdateUser
            };
        }

       
        #endregion
    }
}
