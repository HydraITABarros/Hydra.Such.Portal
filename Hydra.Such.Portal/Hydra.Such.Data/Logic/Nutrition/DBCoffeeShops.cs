using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Nutrition;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public static class DBCoffeeShops
    {
        public static List<CafetariasRefeitórios> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.CafetariasRefeitórios.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static CafetariasRefeitórios Create(CafetariasRefeitórios ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.CafetariasRefeitórios.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static CafetariasRefeitórios Update(CafetariasRefeitórios ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.CafetariasRefeitórios.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static CafetariasRefeitórios GetById(int productivityUnitNo, int type, int code, DateTime explorationStartDate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.CafetariasRefeitórios
                        .FirstOrDefault(x => x.NºUnidadeProdutiva == productivityUnitNo && 
                        x.Tipo == type &&
                        x.Código == code &&
                        x.DataInícioExploração == explorationStartDate);
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static CafetariasRefeitórios GetById(int NºUnidadeProdutiva, string NºProjeto)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.CafetariasRefeitórios.FirstOrDefault(x => x.NºUnidadeProdutiva == NºUnidadeProdutiva && x.NºProjeto == NºProjeto);
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static CafetariasRefeitórios GetByCode(int Code)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.CafetariasRefeitórios.FirstOrDefault(x => x.Código == Code);
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static CafetariasRefeitórios GetByIdDiary(int NºUnidadeProdutiva, int CódigoCafetariaRefeitório)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.CafetariasRefeitórios.FirstOrDefault(x => x.NºUnidadeProdutiva == NºUnidadeProdutiva && x.Código == CódigoCafetariaRefeitório);
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(CafetariasRefeitórios ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.CafetariasRefeitórios.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static List<CafetariasRefeitórios> GetByNUnidadeProdutiva(int NºUnidadeProdutiva)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.CafetariasRefeitórios.Where(x => x.NºUnidadeProdutiva == NºUnidadeProdutiva).ToList(); ;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static CafetariasRefeitórios ParseToDB(CoffeeShopViewModel x)
        {
            return new CafetariasRefeitórios()
            {
                NºUnidadeProdutiva = x.ProductivityUnitNo,
                Tipo = x.Type,
                Código = x.Code,
                DataInícioExploração = DateTime.Parse(x.StartDateExploration),
                DataFimExploração = x.EndDateExploration != "" ? DateTime.Parse(x.EndDateExploration) : (DateTime?)null,
                Descrição = x.Description,
                CódResponsável = x.CodeResponsible,
                CódigoRegião = x.CodeRegion,
                CódigoÁreaFuncional = x.CodeFunctionalArea,
                CódigoCentroResponsabilidade = x.CodeResponsabilityCenter,
                Armazém = x.Warehouse,
                ArmazémLocal = x.WarehouseSupplier,
                NºProjeto = x.ProjectNo,
                Ativa = x.Active,
                DataHoraCriação = x.CreateDate,
                UtilizadorCriação = x.CreateUser,
                DataHoraModificação = x.UpdateDate,
                UtilizadorModificação = x.UpdateUser
            };
        }

        public static CoffeeShopViewModel ParseToViewModel(CafetariasRefeitórios x)
        {
            return new CoffeeShopViewModel()
            {
                ProductivityUnitNo = x.NºUnidadeProdutiva,
                Type = x.Tipo,
                Code = x.Código,
                StartDateExploration = x.DataInícioExploração.ToString("yyyy-MM-dd"),
                EndDateExploration = x.DataFimExploração.HasValue ? x.DataFimExploração.Value.ToString("yyyy-MM-dd") : "",
                Description = x.Descrição,
                CodeResponsible = x.CódResponsável,
                CodeRegion = x.CódigoRegião,
                CodeFunctionalArea = x.CódigoÁreaFuncional,
                CodeResponsabilityCenter = x.CódigoCentroResponsabilidade,
                Warehouse = x.Armazém,
                WarehouseSupplier = x.ArmazémLocal,
                ProjectNo = x.NºProjeto,
                Active = x.Ativa,
                CreateDate = x.DataHoraCriação,
                CreateUser = x.UtilizadorCriação,
                UpdateDate = x.DataHoraModificação,
                UpdateUser = x.UtilizadorModificação
            };
        }

        public static List<CoffeeShopViewModel> ParseListToViewModel(List<CafetariasRefeitórios> x)
        {
            List<CoffeeShopViewModel> result = new List<CoffeeShopViewModel>();

            x.ForEach(y => result.Add(ParseToViewModel(y)));
            return result;
        }
        
        public static CoffeeShopViewModel ParseToViewModel(this CafetariasRefeitórios item)
        {
            if (item != null)
            {
                return new CoffeeShopViewModel()
                {
                    ProductivityUnitNo = item.NºUnidadeProdutiva,
                    Type = item.Tipo,
                    Code = item.Código,
                    StartDateExploration = item.DataInícioExploração.ToString("yyyy-MM-dd"),
                    EndDateExploration = item.DataFimExploração.HasValue ? item.DataFimExploração.Value.ToString("yyyy-MM-dd") : "",
                    Description = item.Descrição,
                    CodeResponsible = item.CódResponsável,
                    CodeRegion = item.CódigoRegião,
                    CodeFunctionalArea = item.CódigoÁreaFuncional,
                    CodeResponsabilityCenter = item.CódigoCentroResponsabilidade,
                    Warehouse = item.Armazém,
                    WarehouseSupplier = item.ArmazémLocal,
                    ProjectNo = item.NºProjeto,
                    Active = item.Ativa,
                    CreateDate = item.DataHoraCriação,
                    CreateUser = item.UtilizadorCriação,
                    UpdateDate = item.DataHoraModificação,
                    UpdateUser = item.UtilizadorModificação
                };
            }
            return null;
        }

        public static List<CoffeeShopViewModel> ParseToViewModel(this List<CafetariasRefeitórios> items)
        {
            List<CoffeeShopViewModel> coffeeShops = new List<CoffeeShopViewModel>();
            if (items != null)
                items.ForEach(x =>
                    coffeeShops.Add(x.ParseToViewModel()));
            return coffeeShops;
        }
    }
}
