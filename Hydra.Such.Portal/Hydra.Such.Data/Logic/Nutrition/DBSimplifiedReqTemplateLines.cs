using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Hydra.Such.Data.ViewModel.Nutrition;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public static class DBSimplifiedReqTemplateLines
    {
        #region CRUD
        public static LinhasRequisiçõesSimplificadas GetById(string reqTemplateId, int reqTemplateLineId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return GetTemplateBaseQuery(ctx)
                        .Where(x => x.NºRequisição == reqTemplateId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<LinhasRequisiçõesSimplificadas> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return GetTemplateBaseQuery(ctx)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Ensures Template Header
        /// </summary>
        /// <returns></returns>
        private static IQueryable<LinhasRequisiçõesSimplificadas> GetTemplateBaseQuery(SuchDBContext ctx)
        {
            return ctx.LinhasRequisiçõesSimplificadas
                    .Join(ctx.RequisiçõesSimplificadas, rsl => rsl.NºRequisição, rs => rs.NºRequisição,
                        (rsl, rs) => new { Lines = rsl, Header = rs })
                    .Where(x => x.Header.RequisiçãoModelo.Value)
                    .Select(x => x.Lines);

        }

        public static List<LinhasRequisiçõesSimplificadas> GetLinesForTemplate(string reqTemplateId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return GetTemplateBaseQuery(ctx)
                        .Where(x => x.NºRequisição == reqTemplateId).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static LinhasRequisiçõesSimplificadas Create(LinhasRequisiçõesSimplificadas item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraCriação = DateTime.Now;
                    ctx.LinhasRequisiçõesSimplificadas.Add(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static LinhasRequisiçõesSimplificadas Update(LinhasRequisiçõesSimplificadas item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraModificação = DateTime.Now;
                    ctx.LinhasRequisiçõesSimplificadas.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasRequisiçõesSimplificadas> Update(List<LinhasRequisiçõesSimplificadas> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(item =>
                        item.DataHoraModificação = DateTime.Now);
                    ctx.LinhasRequisiçõesSimplificadas.UpdateRange(items);
                    ctx.SaveChanges();
                }

                return items;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(LinhasRequisiçõesSimplificadas item)
        {
            try
            {
                if (item != null)
                {
                    using (var ctx = new SuchDBContext())
                    {
                        ctx.LinhasRequisiçõesSimplificadas.Remove(item);
                        ctx.SaveChanges();
                    }

                    return true;
                }
            }
            catch(Exception ex)
            {  }

            return false;
        }

        public static bool Delete(List<LinhasRequisiçõesSimplificadas> items)
        {
            try
            {
                if (items != null)
                {
                    using (var ctx = new SuchDBContext())
                    {
                        ctx.LinhasRequisiçõesSimplificadas.RemoveRange(items);
                        ctx.SaveChanges();
                    }

                    return true;
                }
            }
            catch { }

            return false;
        }
        #endregion

        #region Parse Utilities

        public static SimplifiedReqTemplateLinesViewModel ParseToViewModel(this LinhasRequisiçõesSimplificadas item)
        {
            if (item != null)
            {
                return new SimplifiedReqTemplateLinesViewModel()
                {
                    RequisitionTemplateId = item.NºRequisição,
                    RequisitionTemplateLineId = item.NºLinha,
                    Description = "Não implementado",//item.Descrição
                    CodeRegion = item.CódigoRegião,
                    CodeFunctionalArea = item.CódigoÁreaFuncional,
                    CodeResponsabilityCenter = item.CódigoCentroResponsabilidade,
                    CreateDate = item.DataHoraCriação.HasValue ? item.DataHoraCriação.Value.ToString("yyyy-MM-dd") : "",
                    UpdateDate = item.DataHoraModificação.HasValue ? item.DataHoraModificação.Value.ToString("yyyy-MM-dd") : "",
                    CreateUser = item.UtilizadorCriação,
                    UpdateUser = item.UtilizadorModificação,
                    //TemplateHeader = item.NºRequisiçãoNavigation.ParseToViewModel()
                };
            }
            return null;
        }

        public static List<SimplifiedReqTemplateLinesViewModel> ParseToViewModel(this List<LinhasRequisiçõesSimplificadas> items)
        {
            List<SimplifiedReqTemplateLinesViewModel> parsedItems = new List<SimplifiedReqTemplateLinesViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }

        public static LinhasRequisiçõesSimplificadas ParseToDB(this SimplifiedReqTemplateLinesViewModel item)
        {
            if (item != null)
            {
                return new LinhasRequisiçõesSimplificadas()
                {
                    NºRequisição = item.RequisitionTemplateId,
                    NºLinha = item.RequisitionTemplateLineId,
                    Descrição = item.Description,
                    CódigoRegião = item.CodeRegion,
                    CódigoÁreaFuncional = item.CodeFunctionalArea,
                    CódigoCentroResponsabilidade = item.CodeResponsabilityCenter,
                    DataHoraCriação = string.IsNullOrEmpty(item.CreateDate) ? (DateTime?)null : DateTime.Parse(item.CreateDate),
                    DataHoraModificação = string.IsNullOrEmpty(item.UpdateDate) ? (DateTime?)null : DateTime.Parse(item.UpdateDate),
                    UtilizadorCriação = item.CreateUser,
                    UtilizadorModificação = item.UpdateUser,
                };
            }
            return null;
        }

        public static List<LinhasRequisiçõesSimplificadas> ParseToDB(this List<SimplifiedReqTemplateLinesViewModel> items)
        {
            List<LinhasRequisiçõesSimplificadas> parsedItems = new List<LinhasRequisiçõesSimplificadas>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
        #endregion
    }
}
