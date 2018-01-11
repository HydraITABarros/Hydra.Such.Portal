using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Nutrition;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public static class DBRecordTechnicalOfPlates
    {
        #region CRUD
        public static List<FichasTécnicasPratos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.FichasTécnicasPratos.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<FichasTécnicasPratos> GetByPlateNo(string PlateNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.FichasTécnicasPratos.Where(x => x.NºPrato == PlateNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static FichasTécnicasPratos Create(FichasTécnicasPratos ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.FichasTécnicasPratos.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static FichasTécnicasPratos Update(FichasTécnicasPratos ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.FichasTécnicasPratos.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static bool Delete(FichasTécnicasPratos ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.FichasTécnicasPratos.Remove(ObjectToDelete);
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
        public static RecordTechnicalOfPlatesModelView ParseToViewModel(this FichasTécnicasPratos item)
        {
            if (item != null)
            {
                return new RecordTechnicalOfPlatesModelView()
                {
                    PlateNo = item.NºPrato,
                    Description = item.Descrição,
                    UnitMeasureCode = item.CódUnidadeMedida,
                    State = item.Estado,
                    RecordTechnicalName = item.NomeFichaTécnica,
                    LocalizationCode = item.CódLocalização,
                    PreparationTime = item.TempoPreparação,
                    TechnicalCooking = item.TécnicaCulinária,
                    Group = item.Grupo,
                    Epoch = item.Época,
                    DosesNo = item.NºDeDoses,
                    PreparationTemperature = item.TemperaturaPreparação,
                    FinalTemperatureConfection = item.TemperaturaFinalConfeção,
                    ServeTemperature = item.TemperaturaAServir,
                    Image = item.Image,
                    VariationPriceCost = item.VariaçãoPreçoCusto,
                    ClassFt1 = item.ClassFt1,
                    ClassFt2 = item.ClassFt2,
                    ClassFt3 = item.ClassFt3,
                    ClassFt4 = item.ClassFt4,
                    ClassFt5 = item.ClassFt5,
                    ClassFt6 = item.ClassFt6,
                    ClassFt7 = item.ClassFt7,
                    ClassFt8 = item.ClassFt8,
                    CenterResponsibilityCode = item.CódigoCentroResponsabilidade,
                    Observations = item.Observações,
                    CreateDateTime = item.DataHoraCriação,
                    UpdateDateTime = item.DataHoraModificação,
                    CreateUser = item.UtilizadorCriação,
                    UpdateUser = item.UtilizadorModificação
                };
            }
            return null;
        }
        public static List<RecordTechnicalOfPlatesModelView> ParseToViewModel(this List<FichasTécnicasPratos> items)
        {
            List<RecordTechnicalOfPlatesModelView> parsedItems = new List<RecordTechnicalOfPlatesModelView>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }
        public static FichasTécnicasPratos ParseToDB(this RecordTechnicalOfPlatesModelView item)
        {
            if (item != null)
            {
                return new FichasTécnicasPratos()
                {
                    NºPrato = item.PlateNo,
                    Descrição = item.Description,
                    CódUnidadeMedida = item.UnitMeasureCode,
                    Estado = item.State,
                    NomeFichaTécnica = item.RecordTechnicalName,
                    CódLocalização = item.LocalizationCode,
                    TempoPreparação = item.PreparationTime,
                    TécnicaCulinária = item.TechnicalCooking,
                    Grupo = item.Group,
                    Época = item.Epoch,
                    NºDeDoses = item.DosesNo,
                    TemperaturaPreparação = item.PreparationTemperature,
                    TemperaturaFinalConfeção = item.FinalTemperatureConfection,
                    TemperaturaAServir = item.ServeTemperature,
                    Image = item.Image,
                    VariaçãoPreçoCusto = item.VariationPriceCost,
                    ClassFt1 = item.ClassFt1,
                    ClassFt2 = item.ClassFt2,
                    ClassFt3 = item.ClassFt3,
                    ClassFt4 = item.ClassFt4,
                    ClassFt5 = item.ClassFt5,
                    ClassFt6 = item.ClassFt6,
                    ClassFt7 = item.ClassFt7,
                    ClassFt8 = item.ClassFt8,
                    CódigoCentroResponsabilidade = item.CenterResponsibilityCode,
                    Observações = item.Observations,
                    DataHoraModificação = item.UpdateDateTime,
                    UtilizadorModificação = item.UpdateUser,
                };
            }
            return null;
        }

        public static List<FichasTécnicasPratos> ParseToDB(this List<RecordTechnicalOfPlatesModelView> items)
        {
            List<FichasTécnicasPratos> parsedItems = new List<FichasTécnicasPratos>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
    }
}
