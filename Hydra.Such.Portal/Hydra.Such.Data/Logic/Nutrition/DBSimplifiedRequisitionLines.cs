using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Nutrition;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public static class DBSimplifiedRequisitionLines
    {
        #region CRUD
        public static List<LinhasRequisiçõesSimplificadas> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasRequisiçõesSimplificadas.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static LinhasRequisiçõesSimplificadas Create(LinhasRequisiçõesSimplificadas ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.LinhasRequisiçõesSimplificadas.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static LinhasRequisiçõesSimplificadas Update(LinhasRequisiçõesSimplificadas ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.LinhasRequisiçõesSimplificadas.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(LinhasRequisiçõesSimplificadas ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasRequisiçõesSimplificadas.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static LinhasRequisiçõesSimplificadas GetById(string Code)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasRequisiçõesSimplificadas
                        .FirstOrDefault(x => x.NºRequisição == Code);
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        #endregion

        #region Parses
        public static SimplifiedRequisitionLineViewModel ParseToViewModel(LinhasRequisiçõesSimplificadas x)
        {
            return new SimplifiedRequisitionLineViewModel()
            {
                RequisitionNo = x.NºRequisição,
                LineNo = x.NºLinha,
                Type = x.Tipo,
                Code = x.Código,
                LocationCode = x.CódLocalização,
                Status = x.Estado,
                Description = x.Descrição,
                MeasureUnitNo = x.CódUnidadeMedida,
                QuantityToRequire = x.QuantidadeARequerer,
                QuantityApproved = x.QuantidadeAprovada,
                QuantityReceipt = x.QuantidadeRecebida,
                QuantityToApprove = x.QuantidadeAAprovar,
                TotalCost = x.CustoTotal,
                ProjectNo = x.NºProjeto,
                MealType = x.TipoRefeição,
                RegionCode = x.CódigoRegião,
                FunctionAreaCode = x.CódigoÁreaFuncional,
                ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                EmployeeNo = x.NºFuncionário,
                UnitCost = x.CustoUnitário,
                RequisitionDate = x.DataRequisição.HasValue ? x.DataRequisição.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                CreateDate = x.DataHoraCriação,
                CreateUser = x.UtilizadorCriação,
                UpdateDate = x.DataHoraModificação,
                UpdateUser = x.UtilizadorModificação
            };
        }

        public static List<SimplifiedRequisitionLineViewModel> ParseToViewModel(this List<LinhasRequisiçõesSimplificadas> items)
        {
            List<SimplifiedRequisitionLineViewModel> locations = new List<SimplifiedRequisitionLineViewModel>();
            if (items != null)
                items.ForEach(x =>
                    locations.Add(ParseToViewModel(x)));
            return locations;
        }

        public static LinhasRequisiçõesSimplificadas ParseToDatabase(SimplifiedRequisitionLineViewModel x)
        {
            return new LinhasRequisiçõesSimplificadas()
            {
                NºRequisição = x.RequisitionNo,
                NºLinha = x.LineNo,
                Tipo = x.Type,
                Código = x.Code,
                CódLocalização = x.LocationCode,
                Estado = x.Status,
                Descrição = x.Description,
                CódUnidadeMedida = x.MeasureUnitNo,
                QuantidadeARequerer = x.QuantityToRequire,
                QuantidadeAprovada = x.QuantityApproved,
                QuantidadeRecebida = x.QuantityReceipt,
                QuantidadeAAprovar = x.QuantityToApprove,
                CustoTotal = x.TotalCost,
                NºProjeto = x.ProjectNo,
                TipoRefeição = x.MealType,
                CódigoRegião = x.RegionCode,
                CódigoÁreaFuncional = x.FunctionAreaCode,
                CódigoCentroResponsabilidade = x.ResponsabilityCenterCode,
                NºFuncionário = x.EmployeeNo,
                CustoUnitário = x.UnitCost,
                DataRequisição = x.RequisitionDate != "" && x.RequisitionDate != null ? DateTime.Parse(x.RequisitionDate) : (DateTime?)null,
                DataHoraCriação = x.CreateDate,
                UtilizadorCriação = x.CreateUser,
                DataHoraModificação = x.UpdateDate,
                UtilizadorModificação = x.UpdateUser
            };
        }

        public static List<LinhasRequisiçõesSimplificadas> ParseToDatabase(this List<SimplifiedRequisitionLineViewModel> items)
        {
            List<LinhasRequisiçõesSimplificadas> locations = new List<LinhasRequisiçõesSimplificadas>();
            if (items != null)
                items.ForEach(x =>
                    locations.Add(ParseToDatabase(x)));
            return locations;
        }

        #endregion
    }
}
