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
        public static List<ConfiguraçãoAprovações> GetAllByType(int type)
        {
            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    return _ctx.ConfiguraçãoAprovações.Where(c => c.Tipo == type).ToList();
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

        public static List<ConfiguraçãoAprovações> GetByTypeAreaValueDateAndDimensions(int type, string functionalArea, string responsabiltyCenter, string region, decimal value, DateTime fDate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<ConfiguraçãoAprovações> approvalConfigs = ctx.ConfiguraçãoAprovações
                        .Where(x => x.Tipo == type &&
                                    //x.Área == area && 
                                    (x.CódigoÁreaFuncional == functionalArea || x.CódigoÁreaFuncional == string.Empty) &&
                                    (x.CódigoCentroResponsabilidade == responsabiltyCenter || x.CódigoCentroResponsabilidade == string.Empty) &&
                                    (x.CódigoRegião == region || x.CódigoRegião == string.Empty) &&
                                    (x.ValorAprovação >= value || x.ValorAprovação == 0 || !x.ValorAprovação.HasValue) &&
                                    (x.DataInicial <= fDate && x.DataFinal >= fDate))
                        .ToList();

                    //Set empty to the max
                    approvalConfigs
                        .Where(x => !x.ValorAprovação.HasValue)
                        .ToList()
                        .ForEach(x => x.ValorAprovação = decimal.MaxValue);

                    //Order by importance: approval limits first then dimension value strength
                    List<DimensionStrengthItem> orderedItems = approvalConfigs
                        .OrderBy(x => x.NívelAprovação).ThenBy(x => x.ValorAprovação)
                        .Distinct()
                        .Select(x => new DimensionStrengthItem(x))
                        .OrderByDescending(x => x.DimensionsStrength)
                        .ThenByDescending(x => x.DimensionsTypeStrength)
                        .ToList();

                    return orderedItems.ToList<ConfiguraçãoAprovações>();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<ConfiguraçãoAprovações> GetByTypeAreaValueDateAndDimensionsAndNivel(int type, string functionalArea, string responsabiltyCenter, string region, decimal value, DateTime fDate, int nivel)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<ConfiguraçãoAprovações> approvalConfigs = ctx.ConfiguraçãoAprovações
                        .Where(x => x.Tipo == type &&
                                    //x.Área == area && 
                                    (x.CódigoÁreaFuncional == functionalArea || x.CódigoÁreaFuncional == string.Empty) &&
                                    (x.CódigoCentroResponsabilidade == responsabiltyCenter || x.CódigoCentroResponsabilidade == string.Empty) &&
                                    (x.CódigoRegião == region || x.CódigoRegião == string.Empty) &&
                                    (x.ValorAprovação >= value || x.ValorAprovação == 0 || !x.ValorAprovação.HasValue) &&
                                    (x.DataInicial <= fDate && x.DataFinal >= fDate) &&
                                    (x.NívelAprovação == nivel))
                        .ToList();

                    //Set empty to the max
                    approvalConfigs
                        .Where(x => !x.ValorAprovação.HasValue)
                        .ToList()
                        .ForEach(x => x.ValorAprovação = decimal.MaxValue);

                    //Order by importance: approval limits first then dimension value strength
                    List<DimensionStrengthItem> orderedItems = approvalConfigs
                        .OrderBy(x => x.NívelAprovação).ThenBy(x => x.ValorAprovação)
                        .Distinct()
                        .Select(x => new DimensionStrengthItem(x))
                        .OrderByDescending(x => x.DimensionsStrength)
                        .ThenByDescending(x => x.DimensionsTypeStrength)
                        .ToList();

                    return orderedItems.ToList<ConfiguraçãoAprovações>();
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

        public class DimensionStrengthItem : ConfiguraçãoAprovações
        {
            public DimensionStrengthItem(ConfiguraçãoAprovações item)
            {
                this.CódigoCentroResponsabilidade = item.CódigoCentroResponsabilidade;
                this.CódigoRegião = item.CódigoRegião;
                this.CódigoÁreaFuncional = item.CódigoÁreaFuncional;
                this.DataFinal = item.DataFinal;
                this.DataHoraCriação = item.DataHoraCriação;
                this.DataHoraModificação = item.DataHoraModificação;
                this.DataInicial = item.DataInicial;
                this.GrupoAprovação = item.GrupoAprovação;
                this.Id = item.Id;
                this.NívelAprovação = item.NívelAprovação;
                this.Tipo = item.Tipo;
                this.UtilizadorAprovação = item.UtilizadorAprovação;
                this.UtilizadorCriação = item.UtilizadorCriação;
                this.UtilizadorModificação = item.UtilizadorModificação;
                this.ValorAprovação = item.ValorAprovação;
                this.Área = item.Área;
            }

            public int DimensionsStrength
            {
                get
                {
                    int value = 0;
                    if (this.CódigoÁreaFuncional != string.Empty)
                        value++;
                    if (this.CódigoCentroResponsabilidade != string.Empty)
                        value++;
                    if (this.CódigoRegião != string.Empty)
                        value++;
                    return value;
                }
            }

            public int DimensionsTypeStrength
            {
                get
                {
                    int value = 0;
                    if (this.CódigoÁreaFuncional != string.Empty)
                        value += 3;
                    if (this.CódigoCentroResponsabilidade != string.Empty)
                        value += 2;
                    if (this.CódigoRegião != string.Empty)
                        value += 1;
                    return value;
                }
            }
        }
    }
}
