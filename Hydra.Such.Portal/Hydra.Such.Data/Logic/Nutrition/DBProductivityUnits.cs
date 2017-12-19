using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Nutrition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public static class DBProductivityUnits
    {
        #region CRUD
        public static UnidadesProdutivas GetById(int NºUnidadeProdutiva)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UnidadesProdutivas.Where(x => x.NºUnidadeProdutiva == NºUnidadeProdutiva).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        

        public static List<UnidadesProdutivas> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UnidadesProdutivas.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static UnidadesProdutivas Create(UnidadesProdutivas ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //Add Profile User
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.UnidadesProdutivas.Add(ObjectToCreate);
                }
                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static UnidadesProdutivas Update(UnidadesProdutivas ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.UnidadesProdutivas.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(UnidadesProdutivas objectToDelete)
        {
            return Delete(new List<UnidadesProdutivas>() { objectToDelete });
        }

        public static bool Delete(List<UnidadesProdutivas> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.UnidadesProdutivas.RemoveRange(items);
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




        public static UnidadesProdutivas ParseToDb(ProductivityUnitViewModel x)
        {
            return new UnidadesProdutivas()
            {
                NºUnidadeProdutiva = x.ProductivityUnitNo,
                Descrição = x.Description,
                Estado = x.Status,
                NºCliente = x.ClientNo,
                CódigoRegião = x.CodeRegion,
                CódigoCentroResponsabilidade = x.CodeResponsabilityCenter,
                CódigoÁreaFuncional = x.CodeFunctionalArea,
                DataInícioExploração = x.StartDateExploration != "" ? DateTime.Parse(x.StartDateExploration) : (DateTime?)null,
                DataFimExploração = x.EndDateExploration != "" ? DateTime.Parse(x.EndDateExploration) : (DateTime?)null,
                Armazém = x.Warehouse,
                ArmazémFornecedor = x.WarehouseSupplier,
                ProjetoCozinha = x.ProjectKitchen,
                ProjetoDesperdícios = x.ProjectWaste,
                ProjetoDespMatPrimas = x.ProjectWasteFeedstock,
                ProjetoMatSubsidiárias = x.ProjectSubsidiaries,
                DataHoraCriação = x.CreateDate,
                DataHoraModificação = x.UpdateDate,
                UtilizadorCriação = x.CreateUser,
                UtilizadorModificação = x.UpdateUser
            };
        }

        public static List<ProductivityUnitViewModel> ParseListToViewModel(List<UnidadesProdutivas> x)
        {
            List<ProductivityUnitViewModel> result = new List<ProductivityUnitViewModel>();

            x.ForEach(y => result.Add(ParseToViewModel(y)));

            return result;
        }
        //public static ProductivityUnitViewModel ParseToViewModel(UnidadesProdutivas x)
        //{
        //    return new ProductivityUnitViewModel()
        //    {
        //        ProductivityUnitNo = x.NºUnidadeProdutiva,
        //        Description = x.Descrição,
        //        Status = x.Estado,
        //        ClientNo = x.NºCliente,
        //        CodeRegion = x.CódigoRegião,
        //        CodeResponsabilityCenter = x.CódigoCentroResponsabilidade,
        //        CodeFunctionalArea = x.CódigoÁreaFuncional,
        //        StartDateExploration = x.DataInícioExploração.HasValue ? x.DataInícioExploração.Value.ToString("yyyy-MM-dd") : "",
        //        EndDateExploration = x.DataFimExploração.HasValue ? x.DataFimExploração.Value.ToString("yyyy-MM-dd") : "",
        //        Warehouse = x.Armazém,
        //        WarehouseSupplier = x.ArmazémFornecedor,
        //        ProjectKitchen = x.ProjetoCozinha,
        //        ProjectWaste = x.ProjetoDesperdícios,
        //        ProjectWasteFeedstock = x.ProjetoDespMatPrimas,
        //        ProjectSubsidiaries = x.ProjetoMatSubsidiárias,
        //        CreateDate = x.DataHoraCriação,
        //        UpdateDate = x.DataHoraModificação,
        //        CreateUser = x.UtilizadorCriação,
        //        UpdateUser = x.UtilizadorModificação
        //    };
        //}

        public static ProductivityUnitViewModel ParseToViewModel(this UnidadesProdutivas item)
        {
            if (item != null)
            {
                return new ProductivityUnitViewModel()
                {
                    ProductivityUnitNo = item.NºUnidadeProdutiva,
                    Description = item.Descrição,
                    Status = item.Estado,
                    ClientNo = item.NºCliente,
                    CodeRegion = item.CódigoRegião,
                    CodeResponsabilityCenter = item.CódigoCentroResponsabilidade,
                    CodeFunctionalArea = item.CódigoÁreaFuncional,
                    StartDateExploration = item.DataInícioExploração.HasValue ? item.DataInícioExploração.Value.ToString("yyyy-MM-dd") : "",
                    EndDateExploration = item.DataFimExploração.HasValue ? item.DataFimExploração.Value.ToString("yyyy-MM-dd") : "",
                    Warehouse = item.Armazém,
                    WarehouseSupplier = item.ArmazémFornecedor,
                    ProjectKitchen = item.ProjetoCozinha,
                    ProjectWaste = item.ProjetoDesperdícios,
                    ProjectWasteFeedstock = item.ProjetoDespMatPrimas,
                    ProjectSubsidiaries = item.ProjetoMatSubsidiárias,
                    CreateDate = item.DataHoraCriação,
                    UpdateDate = item.DataHoraModificação,
                    CreateUser = item.UtilizadorCriação,
                    UpdateUser = item.UtilizadorModificação
                };
            }
            return null;
        }

        public static List<ProductivityUnitViewModel> ParseToViewModel(this List<UnidadesProdutivas> items)
        {
            List<ProductivityUnitViewModel> parsedItems = new List<ProductivityUnitViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }
    }
}
