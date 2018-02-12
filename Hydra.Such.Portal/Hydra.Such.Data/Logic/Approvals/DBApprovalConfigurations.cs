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


        public static List<ConfiguraçãoAprovações> GetByTypeAreaValueDateAndDimensions(int type, int area, string functionalArea, string responsabiltyCenter, string region, decimal value,DateTime fDate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<ConfiguraçãoAprovações> result = ctx.ConfiguraçãoAprovações.Where(x => x.Tipo == type && x.Área == area && x.CódigoÁreaFuncional == functionalArea && x.CódigoCentroResponsabilidade == x.CódigoCentroResponsabilidade && x.CódigoRegião == region && (x.ValorAprovação >= value || x.ValorAprovação == 0) && (x.DataInicial <= fDate && x.DataFinal >= fDate)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }






        #region Parses
        public static ApprovalConfigurationsViewModel ParseToViewModel(this ConfiguraçãoAprovações x)
        {
            if (x != null)
            {

                return new ApprovalConfigurationsViewModel()
                {
                    Id = x.Id,
                    Type = x.Tipo,
                    Area = x.Área,
                    FunctionalArea = x.CódigoÁreaFuncional,
                    Region = x.CódigoRegião,
                    ResponsabilityCenter = x.CódigoCentroResponsabilidade,
                    Level = x.NívelAprovação,
                    ApprovalValue = x.ValorAprovação,
                    ApprovalUser = x.UtilizadorAprovação,
                    ApprovalGroup = x.GrupoAprovação,
                    CreateDate = x.DataHoraCriação,
                    CreateUser = x.UtilizadorCriação,
                    UpdateDate = x.DataHoraModificação,
                    UpdateUser = x.UtilizadorModificação,
                    StartDate = x.DataInicial.HasValue ? x.DataInicial.Value.ToString("yyyy-MM-dd") : "",
                    EndDate = x.DataFinal.HasValue ? x.DataFinal.Value.ToString("yyyy-MM-dd") : ""

                };
            }
            return null;
        }
        public static List<ApprovalConfigurationsViewModel> ParseToViewModel(this List<ConfiguraçãoAprovações> items)
        {
            List<ApprovalConfigurationsViewModel> parsedItems = new List<ApprovalConfigurationsViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }
        public static ConfiguraçãoAprovações ParseToDatabase(ApprovalConfigurationsViewModel x)
        {
            return new ConfiguraçãoAprovações()
            {
                Id = x.Id,
                Tipo = x.Type,
                Área = x.Area,
                CódigoÁreaFuncional = x.FunctionalArea,
                CódigoRegião = x.Region,
                CódigoCentroResponsabilidade = x.ResponsabilityCenter,
                NívelAprovação = x.Level,
                ValorAprovação = x.ApprovalValue,
                UtilizadorAprovação = x.ApprovalUser,
                GrupoAprovação = x.ApprovalGroup,
                DataHoraCriação = x.CreateDate,
                UtilizadorCriação = x.CreateUser,
                DataHoraModificação = x.UpdateDate,
                UtilizadorModificação = x.UpdateUser,
                DataInicial = string.IsNullOrEmpty(x.StartDate) ? (DateTime?)null : DateTime.Parse(x.StartDate),
                DataFinal = string.IsNullOrEmpty(x.EndDate) ? (DateTime?)null : DateTime.Parse(x.EndDate)

            };
        }
        #endregion
    }
}
