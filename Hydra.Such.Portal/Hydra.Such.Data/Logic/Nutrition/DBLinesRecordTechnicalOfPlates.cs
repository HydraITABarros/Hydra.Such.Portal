using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Nutrition;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public static class DBLinesRecordTechnicalOfPlates
    {
        public static List<LinhasFichasTécnicasPratos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasFichasTécnicasPratos.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<LinhasFichasTécnicasPratos> GetByLineNo(int LineNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasFichasTécnicasPratos.Where(x => x.NºLinha == LineNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static LinhasFichasTécnicasPratos Create(LinhasFichasTécnicasPratos ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.LinhasFichasTécnicasPratos.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static LinhasFichasTécnicasPratos Update(LinhasFichasTécnicasPratos ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.LinhasFichasTécnicasPratos.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static List<LinhasFichasTécnicasPratos> GetAllbyPlateNo(string PlateNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasFichasTécnicasPratos.Where(x => x.NºPrato == PlateNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool Delete(LinhasFichasTécnicasPratos ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasFichasTécnicasPratos.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public static LinesRecordTechnicalOfPlatesViewModel ParseToViewModel(this LinhasFichasTécnicasPratos item)
        {
            if (item != null)
            {
                return new LinesRecordTechnicalOfPlatesViewModel()
                {
                    PlateNo = item.NºPrato,
                    LineNo = item.NºLinha,
                    Type = item.Tipo,
                    Code = item.Código,
                    Description = item.Descrição,
                    Quantity = item.Quantidade,
                    UnitMeasureCode = item.CódUnidadeMedida,
                    QuantityOfProduction = item.QuantidadeDeProdução,
                    EnergeticValue = item.ValorEnergético,
                    Proteins = item.Proteínas,
                    HydratesOfCarbon = item.HidratosDeCarbono,
                    Lipids = item.Lípidos,
                    Fibers = item.Fibras,
                    ExpectedCostPrice = item.PreçoCustoEsperado,
                    CurrentCostPrice = item.PreçoCustoAtual,
                    TimeExpectedCostPrice = item.TpreçoCustoEsperado,
                    TimeCurrentCostPrice = item.TpreçoCustoAtual,
                    LocalizationCode = item.CódLocalização,
                    ProteinsByQuantity = item.ProteínasPorQuantidade,
                    GlicansByQuantity = item.GlícidosPorQuantidade,
                    LipidsByQuantity = item.LípidosPorQuantidade,
                    FibersByQuantity = item.FibasPorQuantidade,
                    EnergeticValue2 = item.ValorEnergético2,
                    VitaminA = item.VitaminaA,
                    VitaminD = item.VitaminaD,
                    Cholesterol = item.Colesterol,
                    Sodium = item.Sódio,
                    Potassium = item.Potássio,
                    Calcium = item.Cálcio,
                    Iron = item.Ferro,
                    Edivel = item.Edivel,
                    VitaminAByQuantity = item.VitaminaAPorQuantidade,
                    VitaminDByQuantity = item.VitaminaDPorQuantidade,
                    CholesterolByQuantity = item.ColesterolPorQuantidade,
                    SodiumByQuantity = item.SódioPorQuantidade,
                    PotassiumByQuantity = item.PotássioPorQuantidade,
                    IronByQuantity = item.FerroPorQuantidade,
                    CalciumByQuantity = item.CálcioPorQuantidade,
                    SaturatedFattyAcids = item.ÁcidosGordosSaturados,
                    SugarCane = item.Açucares,
                    Salt = item.Sal,
                    QuantityPlates = item.QuantidadePrato,
                    Preparation = item.Preparação,
                    CreateDateTime = item.DataHoraCriação,
                    CreateUser = item.UtilizadorCriação,
                    UpdateDateTime = item.DataHoraModificação,
                    UpdateUser = item.UtilizadorModificação
                };
            }
            return null;
        }
        public static List<LinesRecordTechnicalOfPlatesViewModel> ParseToViewModel(this List<LinhasFichasTécnicasPratos> items)
        {
            List<LinesRecordTechnicalOfPlatesViewModel> parsedItems = new List<LinesRecordTechnicalOfPlatesViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }
        public static LinhasFichasTécnicasPratos ParseToDB(this LinesRecordTechnicalOfPlatesViewModel item)
        {
            if (item != null)
            {
                return new LinhasFichasTécnicasPratos()
                {
                    NºPrato = item.PlateNo,
                    NºLinha = item.LineNo,
                    Tipo = item.Type,
                    Código = item.Code,
                    Descrição = item.Description,
                    Quantidade = item.Quantity,
                    CódUnidadeMedida = item.UnitMeasureCode,
                    QuantidadeDeProdução = item.QuantityOfProduction,
                    ValorEnergético  = item.EnergeticValue,
                    Proteínas  = item.Proteins,
                    HidratosDeCarbono  = item.HydratesOfCarbon,
                    Lípidos  = item.Lipids,
                    Fibras  = item.Fibers,
                    PreçoCustoEsperado  = item.ExpectedCostPrice,
                    PreçoCustoAtual  = item.CurrentCostPrice,
                    TpreçoCustoEsperado  = item.TimeExpectedCostPrice,
                    TpreçoCustoAtual  = item.TimeCurrentCostPrice,
                    CódLocalização  = item.LocalizationCode,
                    ProteínasPorQuantidade  = item.ProteinsByQuantity,
                    GlícidosPorQuantidade  = item.GlicansByQuantity,
                    LípidosPorQuantidade  = item.LipidsByQuantity,
                    FibasPorQuantidade  = item.FibersByQuantity,
                    ValorEnergético2  = item.EnergeticValue2,
                    VitaminaA  = item.VitaminA,
                    VitaminaD  = item.VitaminD,
                    Colesterol  = item.Cholesterol,
                    Sódio  = item.Sodium,
                    Potássio  = item.Potassium,
                    Cálcio  = item.Calcium,
                    Ferro  = item.Iron,
                    Edivel = item.Edivel,
                    VitaminaAPorQuantidade  = item.VitaminAByQuantity,
                    VitaminaDPorQuantidade  = item.VitaminDByQuantity,
                    ColesterolPorQuantidade  = item.CholesterolByQuantity,
                    SódioPorQuantidade  = item.SodiumByQuantity,
                    PotássioPorQuantidade  = item.PotassiumByQuantity,
                    FerroPorQuantidade  = item.IronByQuantity,
                    CálcioPorQuantidade  = item.CalciumByQuantity,
                    ÁcidosGordosSaturados  = item.SaturatedFattyAcids,
                    Açucares  = item.SugarCane,
                    Sal  = item.Salt,
                    QuantidadePrato  = item.QuantityPlates,
                    Preparação  = item.Preparation,
                    DataHoraCriação  = item.CreateDateTime,
                    UtilizadorCriação  = item.CreateUser,
                    DataHoraModificação  = item.UpdateDateTime,
                    UtilizadorModificação  = item.UpdateUser
                };
            }
            return null;
        }
        public static List<LinhasFichasTécnicasPratos> ParseToDB(this List<LinesRecordTechnicalOfPlatesViewModel> items)
        {
            List<LinhasFichasTécnicasPratos> parsedItems = new List<LinhasFichasTécnicasPratos>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
    }
}
