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

        public static DiárioCafetariaRefeitório GetById(int LineNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioCafetariaRefeitório.Where(x => x.NºLinha == LineNo).FirstOrDefault();
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
                RegistryDate = x.DataRegisto.HasValue ? x.DataRegisto.Value.ToString("yyyy-MM-dd") : "",
                ResourceNo = x.NºRecurso,
                Description = x.Descrição,
                Value = x.Valor,
                ProjectNo = x.NºProjeto,
                RegionCode = x.CódigoRegião,
                FunctionalAreaCode = x.CódigoÁreaFuncional,
                ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                Quantity = x.Quantidade,
                ProdutiveUnityNo = x.NºUnidadeProdutiva,
                MealType = x.TipoRefeição ?? 0,
                MovementType = x.TipoMovimento,
                User = x.Utilizador,
                CreateDateTime = x.DataHoraCriação.HasValue ? x.DataHoraCriação.Value.ToString("yyyy-MM-dd") : "",
                UpdateDateTime = x.DataHoraModificação.HasValue ? x.DataHoraModificação.Value.ToString("yyyy-MM-dd") : "",
                CreateUser = x.UtilizadorCriação,
                UpdateUser = x.UtilizadorModificação,
                DateToday = DateTime.Today.ToString("yyyy-MM-dd"),
            };
            return result;

        }

        public static DiárioCafetariaRefeitório ParseToDB( CoffeeShopDiaryViewModel x)
        {
            DiárioCafetariaRefeitório result = new DiárioCafetariaRefeitório()
            {
                NºLinha = x.LineNo,
                CódigoCafetariaRefeitório = x.CoffeShopCode,
                DataRegisto = !string.IsNullOrEmpty(x.RegistryDate) ? DateTime.Parse(x.RegistryDate) : (DateTime?)null,
                NºRecurso = x.ResourceNo,
                Descrição = x.Description,
                Valor = x.Value,
                NºProjeto = x.ProjectNo,
                CódigoRegião = x.RegionCode,
                CódigoÁreaFuncional = x.FunctionalAreaCode,
                CódigoCentroResponsabilidade = x.ResponsabilityCenterCode,
                Quantidade = x.Quantity,
                NºUnidadeProdutiva = x.ProdutiveUnityNo,
                TipoRefeição = x.MealType,
                TipoMovimento = x.MovementType,
                Utilizador = x.User,
                DataHoraCriação = x.CreateDateTime != null ? DateTime.Parse(x.CreateDateTime) : (DateTime?)null,
                DataHoraModificação = x.UpdateDateTime != null ? DateTime.Parse(x.UpdateDateTime) : (DateTime?)null,
                UtilizadorCriação = x.CreateUser,
                UtilizadorModificação = x.UpdateUser,
            };
            return result;
        }

        public static List<DiárioCafetariaRefeitório> ParseToDBList(List<CoffeeShopDiaryViewModel> data)
        {
            List<DiárioCafetariaRefeitório> updatedLines = new List<DiárioCafetariaRefeitório>();
            foreach (var x in data)
            {
                DiárioCafetariaRefeitório LineToUpdate = new DiárioCafetariaRefeitório();
                LineToUpdate.NºLinha = x.LineNo;
                LineToUpdate.CódigoCafetariaRefeitório = x.CoffeShopCode;
                LineToUpdate.DataRegisto = !string.IsNullOrEmpty(x.RegistryDate) ? DateTime.Parse(x.RegistryDate) : (DateTime?)null;
                LineToUpdate.NºRecurso = x.ResourceNo;
                LineToUpdate.Descrição = x.Description;
                LineToUpdate.Valor = x.Value;
                LineToUpdate.NºProjeto = x.ProjectNo;
                LineToUpdate.CódigoRegião = x.RegionCode;
                LineToUpdate.CódigoÁreaFuncional = x.FunctionalAreaCode;
                LineToUpdate.CódigoCentroResponsabilidade = x.ResponsabilityCenterCode;
                LineToUpdate.Quantidade = x.Quantity;
                LineToUpdate.NºUnidadeProdutiva = x.ProdutiveUnityNo;
                LineToUpdate.TipoRefeição = x.MealType;
                LineToUpdate.TipoMovimento = x.MovementType;
                LineToUpdate.Utilizador = x.User;
                LineToUpdate.DataHoraCriação = x.CreateDateTime != null ? DateTime.Parse(x.CreateDateTime) : (DateTime?)null;
                LineToUpdate.UtilizadorCriação = x.CreateUser;
                LineToUpdate.UtilizadorModificação = x.UpdateUser;
                updatedLines.Add(LineToUpdate);
            };
            return updatedLines;
            
        }
    }
}
