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
                List<FichasTécnicasPratos> result;
                using (var ctx = new SuchDBContext())
                {
                    result = ctx.FichasTécnicasPratos.Select(x => new FichasTécnicasPratos()
                    {
                        NºPrato = x.NºPrato,
                        Descrição = x.Descrição,
                        CódUnidadeMedida = x.CódUnidadeMedida,
                        Estado = x.Estado,
                        NomeFichaTécnica = x.NomeFichaTécnica,
                        CódLocalização = x.CódLocalização,
                        TempoPreparação = x.TempoPreparação,
                        TécnicaCulinária = x.TécnicaCulinária,
                        Grupo = x.Grupo,
                        Época = x.Época,
                        NºDeDoses = x.NºDeDoses,
                        TemperaturaPreparação = x.TemperaturaPreparação,
                        TemperaturaFinalConfeção = x.TemperaturaFinalConfeção,
                        TemperaturaAServir = x.TemperaturaAServir,
                        VariaçãoPreçoCusto = x.VariaçãoPreçoCusto,
                        ClassFt1 = x.ClassFt1,
                        ClassFt2 = x.ClassFt2,
                        ClassFt3 = x.ClassFt3,
                        ClassFt4 = x.ClassFt4,
                        ClassFt5 = x.ClassFt5,
                        ClassFt6 = x.ClassFt6,
                        ClassFt7 = x.ClassFt7,
                        ClassFt8 = x.ClassFt8,
                        CódigoCentroResponsabilidade = x.CódigoCentroResponsabilidade,
                        Observações = x.Observações,
                        DataHoraCriação = x.DataHoraCriação,
                        DataHoraModificação = x.DataHoraModificação,
                        UtilizadorCriação = x.UtilizadorCriação,
                        UtilizadorModificação = x.UtilizadorModificação,
                        ContêmGlúten = x.ContêmGlúten,
                        ÁBaseCrustáceos = x.ÁBaseCrustáceos,
                        ÁBaseOvos = x.ÁBaseOvos,
                        ÁBasePeixes = x.ÁBasePeixes,
                        ÁBaseAmendoins = x.ÁBaseAmendoins,
                        ÁBaseSoja = x.ÁBaseSoja,
                        ÁBaseLeite = x.ÁBaseLeite,
                        ÁBaseFrutosCascaRija = x.ÁBaseFrutosCascaRija,
                        ÁBaseAipo = x.ÁBaseAipo,
                        ÁBaseMostarda = x.ÁBaseMostarda,
                        ÁBaseSementesDeSésamo = x.ÁBaseSementesDeSésamo,
                        ÁBaseEnxofreESulfitos = x.ÁBaseEnxofreESulfitos,
                        ÁBaseTremoço = x.ÁBaseTremoço,
                        ÁBaseMoluscos = x.ÁBaseMoluscos,
                        VitaminaA = x.VitaminaA,
                        VitaminaD = x.VitaminaD,
                        Colesterol = x.Colesterol,
                        Sódio = x.Sódio,
                        Potássio = x.Potássio,
                        Cálcio = x.Cálcio,
                        Ferro = x.Ferro,
                        Proteínas = x.Proteínas,
                        HidratosDeCarbono = x.HidratosDeCarbono,
                        Lípidos = x.Lípidos,
                        FibraAlimentar = x.FibraAlimentar,
                        PreçoCustoEsperado = x.PreçoCustoEsperado,
                        PreçoCustoActual = x.PreçoCustoActual,
                        ValorEnergético = x.ValorEnergético,
                        ValorEnergético2 = x.ValorEnergético2,
                        
                    }).ToList();
                    return result;
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
                List<FichasTécnicasPratos> result;
                using (var ctx = new SuchDBContext())
                {
                    result = ctx.FichasTécnicasPratos.Where(x => x.NºPrato == PlateNo).Select(x => new FichasTécnicasPratos()
                    {
                        NºPrato = x.NºPrato,
                        Descrição = x.Descrição,
                        CódUnidadeMedida = x.CódUnidadeMedida,
                        Estado = x.Estado,
                        NomeFichaTécnica = x.NomeFichaTécnica,
                        CódLocalização = x.CódLocalização,
                        TempoPreparação = x.TempoPreparação,
                        TécnicaCulinária = x.TécnicaCulinária,
                        Grupo = x.Grupo,
                        Época = x.Época,
                        NºDeDoses = x.NºDeDoses,
                        TemperaturaPreparação = x.TemperaturaPreparação,
                        TemperaturaFinalConfeção = x.TemperaturaFinalConfeção,
                        TemperaturaAServir = x.TemperaturaAServir,
                        VariaçãoPreçoCusto = x.VariaçãoPreçoCusto,
                        ClassFt1 = x.ClassFt1,
                        ClassFt2 = x.ClassFt2,
                        ClassFt3 = x.ClassFt3,
                        ClassFt4 = x.ClassFt4,
                        ClassFt5 = x.ClassFt5,
                        ClassFt6 = x.ClassFt6,
                        ClassFt7 = x.ClassFt7,
                        ClassFt8 = x.ClassFt8,
                        CódigoCentroResponsabilidade = x.CódigoCentroResponsabilidade,
                        Observações = x.Observações,
                        DataHoraCriação = x.DataHoraCriação,
                        DataHoraModificação = x.DataHoraModificação,
                        UtilizadorCriação = x.UtilizadorCriação,
                        UtilizadorModificação = x.UtilizadorModificação,
                        ContêmGlúten = x.ContêmGlúten,
                        ÁBaseCrustáceos = x.ÁBaseCrustáceos,
                        ÁBaseOvos = x.ÁBaseOvos,
                        ÁBasePeixes = x.ÁBasePeixes,
                        ÁBaseAmendoins = x.ÁBaseAmendoins,
                        ÁBaseSoja = x.ÁBaseSoja,
                        ÁBaseLeite = x.ÁBaseLeite,
                        ÁBaseFrutosCascaRija = x.ÁBaseFrutosCascaRija,
                        ÁBaseAipo = x.ÁBaseAipo,
                        ÁBaseMostarda = x.ÁBaseMostarda,
                        ÁBaseSementesDeSésamo = x.ÁBaseSementesDeSésamo,
                        ÁBaseEnxofreESulfitos = x.ÁBaseEnxofreESulfitos,
                        ÁBaseTremoço = x.ÁBaseTremoço,
                        ÁBaseMoluscos = x.ÁBaseMoluscos,
                        VitaminaA = x.VitaminaA,
                        VitaminaD = x.VitaminaD,
                        Colesterol = x.Colesterol,
                        Sódio = x.Sódio,
                        Potássio = x.Potássio,
                        Cálcio = x.Cálcio,
                        Ferro = x.Ferro,
                        Proteínas = x.Proteínas,
                        HidratosDeCarbono = x.HidratosDeCarbono,
                        Lípidos = x.Lípidos,
                        FibraAlimentar = x.FibraAlimentar,
                        PreçoCustoEsperado = x.PreçoCustoEsperado,
                        PreçoCustoActual = x.PreçoCustoActual,
                        ValorEnergético = x.ValorEnergético,
                        ValorEnergético2 = x.ValorEnergético2
                    }).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static RecordTechnicalOfPlatesModelView GetOnlyImageByPlateNo(string PlateNo)
        {
            try
            {
                RecordTechnicalOfPlatesModelView result;
                using (var ctx = new SuchDBContext())
                {
                    result = ctx.FichasTécnicasPratos.Where(x => x.NºPrato == PlateNo).Select(x => new RecordTechnicalOfPlatesModelView()
                    {
                        Image = x.Image
                    }).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<FichasTécnicasPratos> GetWithImageByPlateNo(string PlateNo)
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
                    UpdateUser = item.UtilizadorModificação,
                    WithGluten = item.ContêmGlúten,
                    BasedCrustaceans = item.ÁBaseCrustáceos,
                    BasedEggs = item.ÁBaseOvos,
                    BasedFish = item.ÁBasePeixes,
                    BasedPeanuts = item.ÁBaseAmendoins,
                    BasedSoy = item.ÁBaseSoja,
                    BasedMilk = item.ÁBaseLeite,
                    BasedFruitShardShell = item.ÁBaseFrutosCascaRija,
                    BasedCelery = item.ÁBaseAipo,
                    BasedMustard = item.ÁBaseMostarda,
                    BasedSesameSeeds = item.ÁBaseSementesDeSésamo,
                    BasedSulfurDioxeAndSulphites = item.ÁBaseEnxofreESulfitos,
                    BasedLupine = item.ÁBaseTremoço,
                    BasedMolluscs = item.ÁBaseMoluscos,
                    VitaminA = item.VitaminaA,
                    VitaminD = item.VitaminaD,
                    Cholesterol = item.Colesterol,
                    Sodium = item.Sódio,
                    Potassium = item.Potássio,
                    Calcium = item.Cálcio,
                    Iron = item.Ferro,
                    Proteins = item.Proteínas,
                    Carbohydrates = item.HidratosDeCarbono,
                    Lipids = item.Lípidos,
                    FoodFiber = item.FibraAlimentar,
                    PriceCostExpected = item.PreçoCustoEsperado,
                    PriceCostCurrent = item.PreçoCustoActual,
                    Energeticvalue = item.ValorEnergético,
                    Energeticvalue2 = item.ValorEnergético2,
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
                    ContêmGlúten = item.WithGluten,
                    ÁBaseCrustáceos = item.BasedCrustaceans,
                    ÁBaseOvos = item.BasedEggs,
                    ÁBasePeixes = item.BasedFish,
                    ÁBaseAmendoins = item.BasedPeanuts,
                    ÁBaseSoja = item.BasedSoy,
                    ÁBaseLeite = item.BasedMilk,
                    ÁBaseFrutosCascaRija = item.BasedFruitShardShell,
                    ÁBaseAipo = item.BasedCelery,
                    ÁBaseMostarda = item.BasedMustard,
                    ÁBaseSementesDeSésamo = item.BasedSesameSeeds,
                    ÁBaseEnxofreESulfitos = item.BasedSulfurDioxeAndSulphites,
                    ÁBaseTremoço = item.BasedLupine,
                    ÁBaseMoluscos = item.BasedMolluscs,
                    VitaminaA = item.VitaminA,
                    VitaminaD = item.VitaminD,
                    Colesterol= item.Cholesterol,
                    Sódio= item.Sodium,
                    Potássio= item.Potassium,
                    Cálcio= item.Calcium,
                    Ferro= item.Iron,
                    Proteínas= item.Proteins,
                    HidratosDeCarbono = item.Carbohydrates,
                    Lípidos = item.Lipids,
                    FibraAlimentar = item.FoodFiber,
                    PreçoCustoEsperado = item.PriceCostExpected,
                    PreçoCustoActual = item.PriceCostCurrent,
                    ValorEnergético = item.Energeticvalue,
                    ValorEnergético2 = item.Energeticvalue2,
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
