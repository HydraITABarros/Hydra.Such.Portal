using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Hydra.Such.Data.ViewModel.Nutrition;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public class DBCoffeeShopMovements
    {
        public static MovimentosCafetariaRefeitório Create(MovimentosCafetariaRefeitório ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.MovimentosCafetariaRefeitório.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static MovimentosCafetariaRefeitório GetById(int movementNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosCafetariaRefeitório.SingleOrDefault(x => x.NºMovimento == movementNo);
                }
            }
            catch { }

            return null;
        }

        public static List<MovimentosCafetariaRefeitório> GetByUnitAndCoffeeShop(int productivityUnitNo, int coffeeShopCode)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosCafetariaRefeitório.Where(mov => mov.NºUnidadeProdutiva == productivityUnitNo &&
                                                                            mov.CódigoCafetariaRefeitório.Value == coffeeShopCode).ToList();
                }
            }
            catch { }

            return null;
        }

        public static decimal GetTotalRevenuesFor(int productivityUnitNo, int coffeeShopCode, int coffeeShopType)
        {
            decimal? totalRevenues = null;
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    totalRevenues = ctx.MovimentosCafetariaRefeitório.Where(mov => mov.NºUnidadeProdutiva == productivityUnitNo &&
                                                                            mov.CódigoCafetariaRefeitório.Value == coffeeShopCode &&
                                                                            mov.Tipo.Value == coffeeShopType)
                                                                     .Sum(total => total.Valor);
                }
            }
            catch { }

            return totalRevenues.HasValue ? totalRevenues.Value : 0;
        }

        public static decimal GetTotalMealsFor(int productivityUnitNo, int coffeeShopCode, int coffeeShopType)
        {
            decimal? totalRevenues = null;
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    totalRevenues = ctx.MovimentosCafetariaRefeitório.Where(mov => mov.NºUnidadeProdutiva == productivityUnitNo &&
                                                                            mov.CódigoCafetariaRefeitório.Value == coffeeShopCode &&
                                                                            mov.Tipo.Value == coffeeShopType)
                                                                     .Sum(total => total.Quantidade);
                }
            }
            catch { }

            return totalRevenues.HasValue ? totalRevenues.Value : 0;
        }

        public static CoffeeShopMovimentsViewModel ParseToViewModel(MovimentosCafetariaRefeitório x)
        {
            CoffeeShopMovimentsViewModel result = new CoffeeShopMovimentsViewModel()
            {
                MovimentNo = x.NºMovimento,
                CoffeeShopCode = x.CódigoCafetariaRefeitório,
                ProdutiveUnityNo = x.NºUnidadeProdutiva,
                Type = x.Tipo,
                RegistryDate = x.DataRegisto.HasValue ? x.DataRegisto.Value.ToString("yyyy-MM-dd") : "",
                ResourceNo = x.NºRecurso,
                Description = x.Descrição,
                Value = x.Valor,
                MovementType = x.TipoMovimento,
                RegionCode = x.CódigoRegião,
                FunctionalAreaCode = x.CódigoÁreaFuncional,
                ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                User = x.Utilizador,
                SystemCreateDateTime = x.DataHoraSistemaRegisto.HasValue ? x.DataHoraSistemaRegisto.Value.ToString("yyyy-MM-dd") : "",
                CreateDateTime = x.DataHoraCriação.HasValue ? x.DataHoraCriação.Value.ToString("yyyy-MM-dd") : "",
                CreateUser = x.UtilizadorCriação,
                UpdateDateTime = x.DataHoraModificação.HasValue ? x.DataHoraModificação.Value.ToString("yyyy-MM-dd") : "",
                UpdateUser = x.UtilizadorModificação
            };
            return result;
        }



    }
}
