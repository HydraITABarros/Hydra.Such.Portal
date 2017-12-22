using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;


namespace Hydra.Such.Data.Logic
{
    public static class DBApprovalsConfigurations
    {
        #region CRUD

        public static ConfiguraçãoAprovações GetById(int Cod)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguraçãoAprovações.Where(x => x.Tipo == Cod).FirstOrDefault();
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

        public static ConfiguraçãoAprovações Create(ConfiguraçãoAprovações item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraCriação = DateTime.Now;
                    ctx.ConfiguraçãoAprovações.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<ConfiguraçãoAprovações> Create(List<ConfiguraçãoAprovações> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(x =>
                    {
                        x.DataHoraCriação = DateTime.Now;
                        ctx.ConfiguraçãoAprovações.Add(x);
                    });
                    ctx.SaveChanges();
                }

                return items;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ConfiguraçãoAprovações Update(ConfiguraçãoAprovações item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraModificação = DateTime.Now;
                    ctx.ConfiguraçãoAprovações.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(int cod)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ConfiguraçãoAprovações approval = ctx.ConfiguraçãoAprovações.Where(x => x.Id == cod).FirstOrDefault();
                    if (approval != null)
                    {
                        ctx.ConfiguraçãoAprovações.Remove(approval);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public static bool Delete(ConfiguraçãoAprovações item)
        {
            return Delete(new List<ConfiguraçãoAprovações> { item });
        }

        public static bool Delete(List<ConfiguraçãoAprovações> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ConfiguraçãoAprovações.RemoveRange(items);
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

        public static ApprovalViewModel ParseToViewModel(this ConfiguraçãoAprovações item)
        {
            if (item != null)
            {
                return new ApprovalViewModel()
                {
                    Id=item.Id,
                    Type=item.Tipo,
                    Area=item.Área,
                    ValueApproval=item.ValorAprovação,
                    GroupApproval = item.GrupoAprovação,
                    LevelApproval=item.NívelAprovação,
                    UserApproval=item.UtilizadorAprovação,
                    CreateDate = item.DataHoraCriação.HasValue ? item.DataHoraCriação.Value.ToString("yyyy-MM-dd") : "",
                    UpdateDate = item.DataHoraModificação.HasValue ? item.DataHoraModificação.Value.ToString("yyyy-MM-dd") : "",
                    CreateUser = item.UtilizadorCriação,
                    UpdateUser = item.UtilizadorModificação
                };
            }
            return null;
        }

        public static List<ApprovalViewModel> ParseToViewModel(this List<ConfiguraçãoAprovações> items)
        {
            List<ApprovalViewModel> approval = new List<ApprovalViewModel>();
            if (items != null)
                items.ForEach(x =>
                    approval.Add(x.ParseToViewModel()));
            return approval;
        }

        public static ConfiguraçãoAprovações ParseToDB(this ApprovalViewModel item)
        {
            if (item != null)
            {
                return new ConfiguraçãoAprovações()
                {
                    Id= item.Id,
                    Tipo= item.Type,
                    Área=item.Area,
                    ValorAprovação= item.ValueApproval,
                    GrupoAprovação=item.GroupApproval,
                    NívelAprovação= item.LevelApproval,
                    UtilizadorAprovação= item.UserApproval,
                    DataHoraCriação = string.IsNullOrEmpty(item.CreateDate) ? (DateTime?)null : DateTime.Parse(item.CreateDate),
                    DataHoraModificação = string.IsNullOrEmpty(item.UpdateDate) ? (DateTime?)null : DateTime.Parse(item.UpdateDate),
                    UtilizadorCriação = item.CreateUser,
                    UtilizadorModificação= item.UpdateUser
                };
            }
            return null;
        }

        public static List<ConfiguraçãoAprovações> ParseToDB(this List<ApprovalViewModel> items)
        {
            List<ConfiguraçãoAprovações> approval = new List<ConfiguraçãoAprovações>();
            if (items != null)
                items.ForEach(x =>
                    approval.Add(x.ParseToDB()));
            return approval;
        }
    }
}
