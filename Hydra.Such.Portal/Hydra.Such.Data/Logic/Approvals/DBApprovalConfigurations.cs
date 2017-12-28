using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Approvals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Approvals
{
    public static class DBApprovalConfigurations
    {

        #region CRUD
        public static ConfiguraçãoAprovações GetById(int id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguraçãoAprovações.Where(x => x.Id == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ConfiguraçãoAprovações> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguraçãoAprovações.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConfiguraçãoAprovações Create(ConfiguraçãoAprovações ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.ConfiguraçãoAprovações.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConfiguraçãoAprovações Update(ConfiguraçãoAprovações ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.ConfiguraçãoAprovações.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(ConfiguraçãoAprovações ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ConfiguraçãoAprovações.Remove(ObjectToDelete);
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


        public static List<ConfiguraçãoAprovações> GetByTypeAreaValue(int type, int area, decimal value)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<ConfiguraçãoAprovações> result = ctx.ConfiguraçãoAprovações.Where(x => x.Tipo == type && x.Área == area && (x.ValorAprovação >= value || x.ValorAprovação == 0)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }






        #region Parses
        public static ApprovalConfigurationsViewModel ParseToViewModel(ConfiguraçãoAprovações x)
        {
            return new ApprovalConfigurationsViewModel()
            {
                Id = x.Id,
                Type = x.Tipo,
                Area = x.Área,
                Level = x.NívelAprovação,
                ApprovalValue = x.ValorAprovação,
                ApprovalUser = x.UtilizadorAprovação,
                ApprovalGroup = x.GrupoAprovação,
                CreateDate = x.DataHoraCriação,
                CreateUser = x.UtilizadorCriação,
                UpdateDate = x.DataHoraModificação,
                UpdateUser = x.UtilizadorModificação
            };
        }
        public static ConfiguraçãoAprovações ParseToDatabase(ApprovalConfigurationsViewModel x)
        {
            return new ConfiguraçãoAprovações()
            {
                Id = x.Id,
                Tipo = x.Type,
                Área = x.Area,
                NívelAprovação = x.Level,
                ValorAprovação = x.ApprovalValue,
                UtilizadorAprovação = x.ApprovalUser,
                GrupoAprovação = x.ApprovalGroup,
                DataHoraCriação = x.CreateDate,
                UtilizadorCriação = x.CreateUser,
                DataHoraModificação = x.UpdateDate,
                UtilizadorModificação = x.UpdateUser
            };
        }
        #endregion
    }
}
