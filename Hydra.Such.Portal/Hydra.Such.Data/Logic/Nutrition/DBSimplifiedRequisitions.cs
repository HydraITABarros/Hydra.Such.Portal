using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Nutrition;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public static class DBSimplifiedRequisitions
    {
        #region CRUD
        public static List<RequisiçõesSimplificadas> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RequisiçõesSimplificadas.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static RequisiçõesSimplificadas Create(RequisiçõesSimplificadas ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.RequisiçõesSimplificadas.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static RequisiçõesSimplificadas Update(RequisiçõesSimplificadas ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.RequisiçõesSimplificadas.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(RequisiçõesSimplificadas ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.RequisiçõesSimplificadas.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static RequisiçõesSimplificadas GetById(string Code)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RequisiçõesSimplificadas
                        .FirstOrDefault(x => x.NºRequisição == Code);
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        #endregion

        public static List<RequisiçõesSimplificadas> GetByCreateResponsible(string CreateResponsible)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RequisiçõesSimplificadas.Where(x => x.ResponsávelCriação == CreateResponsible && x.Estado!=3).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<RequisiçõesSimplificadas> GetByApprovals(int approvalTrue)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RequisiçõesSimplificadas.Where(x => x.Estado == approvalTrue).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region Parses
        public static SimplifiedRequisitionViewModel ParseToViewModel(RequisiçõesSimplificadas x)
        {
            return new SimplifiedRequisitionViewModel()
            {
                RequisitionNo = x.NºRequisição,
                Status = x.Estado ,
                RequisitionDate = x.DataHoraRequisição.HasValue ? x.DataHoraRequisição.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                RequisitionTime = x.DataHoraRequisição.HasValue ? x.DataHoraRequisição.Value.ToString("HH:mm", CultureInfo.InvariantCulture) : "",
                RegistrationDate = x.DataRegisto.HasValue ? x.DataRegisto.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                LocationCode = x.CódLocalização,
                RegionCode = x.CódigoRegião,
                FunctionalAreaCode = x.CódigoÁreaFuncional,
                ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                ProjectNo = x.NºProjeto,
                MealType= x.TipoRefeição,
                ApprovalDate = x.DataHoraAprovação.HasValue ? x.DataHoraAprovação.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                ApprovalTime = x.DataHoraAprovação.HasValue ? x.DataHoraAprovação.Value.ToString("HH:mm", CultureInfo.InvariantCulture) : "",
                ShipDate = x.DataHoraEnvio.HasValue ? x.DataHoraEnvio.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                ShipTime = x.DataHoraEnvio.HasValue ? x.DataHoraEnvio.Value.ToString("HH:mm", CultureInfo.InvariantCulture) : "",
                AvailabilityDate = x.DataHoraDisponibilização.HasValue ? x.DataHoraDisponibilização.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                AvailabilityTime = x.DataHoraDisponibilização.HasValue ? x.DataHoraDisponibilização.Value.ToString("HH:mm", CultureInfo.InvariantCulture) : "",
                CreateResponsible = x.ResponsávelCriação,
                ApprovalResponsible = x.ResponsávelAprovação,
                ShipResponsible = x.ResponsávelEnvio,
                ReceiptResponsible = x.ResponsávelReceção,
                Print = x.Imprimir,
                Atach = x.Anexo,
                EmployeeNo = x.NºFuncionário,
                Urgent = x.Urgente,
                ProductivityNo = x.NºUnidadeProdutiva,
                Observations = x.Observações,
                Finished = x.Terminada,
                AimResponsible = x.ResponsávelVisar,
                AimDate = x.DataHoraVisar.HasValue ? x.DataHoraVisar.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                AimTime = x.DataHoraVisar.HasValue ? x.DataHoraVisar.Value.ToString("HH:mm", CultureInfo.InvariantCulture) : "",
                Authorized = x.Autorizada,
                AuthorizedResponsible = x.ResponsávelAutorização,
                AuthorizedDate = x.DataHoraAutorização.HasValue ? x.DataHoraAutorização.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                AuthorizedTime = x.DataHoraAutorização.HasValue ? x.DataHoraAutorização.Value.ToString("HH:mm", CultureInfo.InvariantCulture) : "",
                Visor = x.Visadores,
                ReceiptLinesDate = x.DataReceçãoLinhas,
                NutritionRequisition = x.RequisiçãoNutrição,
                ReceiptPreviewDate = x.DataReceçãoEsperada.HasValue ? x.DataReceçãoEsperada.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                ModelRequisition = x.RequisiçãoModelo,
                CreateDate = x.DataHoraCriação,
                CreateUser = x.UtilizadorCriação,
                UpdateDate = x.DataHoraModificação,
                UpdateUser = x.UtilizadorModificação
            };
        }

        public static List<SimplifiedRequisitionViewModel> ParseToViewModel(this List<RequisiçõesSimplificadas> items)
        {
            List<SimplifiedRequisitionViewModel> locations = new List<SimplifiedRequisitionViewModel>();
            if (items != null)
                items.ForEach(x =>
                    locations.Add(ParseToViewModel(x)));
            return locations;
        }

        public static RequisiçõesSimplificadas ParseToDatabase(SimplifiedRequisitionViewModel x)
        {
            RequisiçõesSimplificadas result = new RequisiçõesSimplificadas()
            {
                NºRequisição = x.RequisitionNo,
                Estado = x.Status,
                DataHoraRequisição = x.RequisitionDate != "" && x.RequisitionDate != null ? DateTime.Parse(x.RequisitionDate) : (DateTime?)null,
                DataRegisto = x.RegistrationDate != "" && x.RegistrationDate != null ? DateTime.Parse(x.RegistrationDate) : (DateTime?)null,
                CódLocalização = x.LocationCode,
                CódigoRegião = x.RegionCode,
                CódigoÁreaFuncional = x.FunctionalAreaCode,
                CódigoCentroResponsabilidade = x.ResponsabilityCenterCode,
                NºProjeto = x.ProjectNo,
                TipoRefeição= x.MealType,
                DataHoraAprovação = x.ApprovalDate != "" && x.ApprovalDate != null ? DateTime.Parse(x.ApprovalDate) : (DateTime?)null,
                DataHoraEnvio = x.ShipDate != "" && x.ShipDate != null ? DateTime.Parse(x.ShipDate) : (DateTime?)null,
                DataHoraDisponibilização = x.AvailabilityDate != "" && x.AvailabilityDate != null ? DateTime.Parse(x.AvailabilityDate) : (DateTime?)null,
                ResponsávelCriação = x.CreateResponsible,
                ResponsávelAprovação = x.ApprovalResponsible,
                ResponsávelEnvio = x.ShipResponsible,
                ResponsávelReceção = x.ReceiptResponsible,
                Imprimir = x.Print,
                Anexo = x.Atach,
                NºFuncionário = x.EmployeeNo,
                Urgente = x.Urgent,
                NºUnidadeProdutiva = x.ProductivityNo,
                Observações = x.Observations,
                Terminada = x.Finished,
                ResponsávelVisar = x.AimResponsible,
                DataHoraVisar = x.AimDate != "" && x.AimDate != null ? DateTime.Parse(x.AimDate) : (DateTime?)null ,
                Autorizada = x.Authorized,
                ResponsávelAutorização = x.AuthorizedResponsible,
                DataHoraAutorização = x.AuthorizedDate != "" && x.AuthorizedDate != null ? DateTime.Parse(x.AuthorizedDate) : (DateTime?)null ,
                Visadores = x.Visor,
                DataReceçãoLinhas = x.ReceiptLinesDate,
                RequisiçãoNutrição = x.NutritionRequisition,
                DataReceçãoEsperada = x.ReceiptPreviewDate != "" && x.ReceiptPreviewDate != null ? DateTime.Parse(x.ReceiptPreviewDate) : (DateTime?)null ,
                RequisiçãoModelo = x.ModelRequisition,
                DataHoraCriação = x.CreateDate,
                UtilizadorCriação = x.CreateUser,
                DataHoraModificação = x.UpdateDate,
                UtilizadorModificação = x.UpdateUser
            };

            if (result.DataHoraRequisição != null)
            {
                result.DataHoraRequisição = result.DataHoraRequisição.Value.Date;
                result.DataHoraRequisição = result.DataHoraRequisição.Value.Add(TimeSpan.Parse(x.RequisitionTime));
            }

            if (result.DataHoraAprovação != null)
            {
                result.DataHoraAprovação = result.DataHoraAprovação.Value.Date;
                result.DataHoraAprovação = result.DataHoraAprovação.Value.Add(TimeSpan.Parse(x.ApprovalTime));
            }

            if (result.DataHoraEnvio != null)
            {
                result.DataHoraEnvio = result.DataHoraEnvio.Value.Date;
                result.DataHoraEnvio = result.DataHoraEnvio.Value.Add(TimeSpan.Parse(x.ShipTime));
            }

            if (result.DataHoraDisponibilização != null)
            {
                result.DataHoraDisponibilização = result.DataHoraDisponibilização.Value.Date;
                result.DataHoraDisponibilização = result.DataHoraDisponibilização.Value.Add(TimeSpan.Parse(x.AvailabilityTime));
            }

            if (result.DataHoraVisar != null)
            {
                result.DataHoraVisar = result.DataHoraVisar.Value.Date;
                result.DataHoraVisar = result.DataHoraVisar.Value.Add(TimeSpan.Parse(x.AimTime));
            }

            if (result.DataHoraAutorização != null)
            {
                result.DataHoraAutorização = result.DataHoraAutorização.Value.Date;
                result.DataHoraAutorização = result.DataHoraAutorização.Value.Add(TimeSpan.Parse(x.AuthorizedTime));
            }
            return result; 
        }

        public static List<RequisiçõesSimplificadas> ParseToDatabase(this List<SimplifiedRequisitionViewModel> items)
        {
            List<RequisiçõesSimplificadas> locations = new List<RequisiçõesSimplificadas>();
            if (items != null)
                items.ForEach(x =>
                    locations.Add(ParseToDatabase(x)));
            return locations;
        }

        #endregion
    }
}
