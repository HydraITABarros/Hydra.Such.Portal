using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Nutrition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public class DBCoffeeShopsDiary
    {
        public static DiárioCafetariaRefeitório Create(DiárioCafetariaRefeitório ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.DiárioCafetariaRefeitório.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static DiárioCafetariaRefeitório Update(DiárioCafetariaRefeitório ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.DiárioCafetariaRefeitório.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(DiárioCafetariaRefeitório ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.DiárioCafetariaRefeitório.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static List<DiárioCafetariaRefeitório> GetByIdsList(int NºUnidadeProdutiva, int CódigoCafetariaRefeitório, string User)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioCafetariaRefeitório.Where(x => x.NºUnidadeProdutiva == NºUnidadeProdutiva && x.CódigoCafetariaRefeitório == CódigoCafetariaRefeitório && x.Utilizador == User).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        public static CoffeeShopDiaryViewModel ParseToViewModel(DiárioCafetariaRefeitório x)
        {
            CoffeeShopDiaryViewModel result = new CoffeeShopDiaryViewModel()
            {
                LineNo = x.NºLinha,
                CoffeShopCode = x.CódigoCafetariaRefeitório,
                RegistryDate = x.DataRegisto,
                ResourceNo = x.NºRecurso,
                Description = x.Descrição,
                Value = x.Valor,
                ProjectNo = x.NºProjeto,
                RegionCode = x.CódigoRegião,
                FunctionalAreaCode = x.CódigoÁreaFuncional,
                ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                Quantity = x.Quantidade,
                ProdutiveUnityNo = x.NºUnidadeProdutiva,
                MealType = x.TipoRefeição,
                MovementType = x.TipoMovimento,
                User = x.Utilizador,
                CreateDateTime = x.DataHoraCriação,
                UpdateDateTime = x.DataHoraModificação,
                CreateUser = x.UtilizadorCriação,
                UpdateUser = x.UtilizadorModificação,
                DateToday = DateTime.Today.ToString("yyyy-MM-dd")
            };
            

            return result;

        }
    }
}
