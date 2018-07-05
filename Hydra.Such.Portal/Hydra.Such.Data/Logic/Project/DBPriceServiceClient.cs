using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Project
{
    public static class DBPriceServiceClient
    {
        #region CRUD
        //
        public static List<PreçosServiçosCliente> GetByC_SC_R(string client, string ServClient, string Resource)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PreçosServiçosCliente.Where(x=> x.Cliente == client && x.CodServCliente == ServClient && x.Recurso == Resource).ToList();
                }
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static List<PreçosServiçosCliente> GetAll ()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PreçosServiçosCliente.ToList();
                }
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static PreçosServiçosCliente Create (PreçosServiçosCliente ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.PreçosServiçosCliente.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception)
            {

               return null;
            }
        }
        public static PreçosServiçosCliente Update(PreçosServiçosCliente ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PreçosServiçosCliente.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static bool Delete(PreçosServiçosCliente ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PreçosServiçosCliente.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public static PriceServiceClientViewModel ParseToViewModel(PreçosServiçosCliente x)
        {

            return new PriceServiceClientViewModel()
            {
                 Client = x.Cliente,
                 Name = x.Nome,
                 Name2 = x.Nome2,
                 CompleteName = x.Nome + " " + x.Nome2,
                 CodServClient = x.CodServCliente,
                 ServiceDescription = x.DescriçãoServiço,
                 SalePrice = x.PreçoVenda,
                 PriceCost = x.PreçoDeCusto,
                 Date = x.Data.HasValue ? x.Data.Value.ToString("yyyy-MM-dd") : "",
                 Resource = x.Recurso,
                 ResourceDescription = x.DescriçãoDoRecurso,
                 UnitMeasure = x.UnidadeMedida,
                 TypeMeal = x.TipoRefeição,
                 TypeMealDescription = x.DescriçãoTipoRefeição,
                 RegionCode = x.CodigoRegião,
                 FunctionalAreaCode = x.CodigoArea,
                 ResponsabilityCenterCode = x.CodigoCentroResponsabilidade,
                 CreateDateTime = x.DataHoraCriação.HasValue ? x.DataHoraCriação.Value.ToString("yyyy-MM-dd") : "",
                 UpdateDateTime = x.DataHoraModificação.HasValue ? x.DataHoraModificação.Value.ToString("yyyy-MM-dd") : "",
                 CreateUser = x.UtilizadorCriação,
                 UpdateUser = x.UtilizadorModificação,
                 Selected = false
            };
        }

        public static List<PriceServiceClientViewModel> ParseToViewModel(List<PreçosServiçosCliente> items)
        {
            List<PriceServiceClientViewModel> parsedItems = new List<PriceServiceClientViewModel>();
            if (items != null && items.Count > 0)
            {
                items.ForEach(x =>
                    parsedItems.Add(ParseToViewModel(x))
                );
            }
            return parsedItems;
        }
        public static PreçosServiçosCliente ParseToDatabase(PriceServiceClientViewModel x)
        {
            return new PreçosServiçosCliente()
            {
                Cliente = x.Client,
                Nome = x.Name,
                Nome2 = x.Name2,
                CodServCliente = x.CodServClient,
                DescriçãoServiço = x.ServiceDescription,
                PreçoVenda = x.SalePrice,
                PreçoDeCusto = x.PriceCost,
                Data = x.Date == "" || x.Date == null ? (DateTime?)null : DateTime.Parse(x.Date),
                Recurso = x.Resource,
                DescriçãoDoRecurso = x.ResourceDescription,
                UnidadeMedida = x.UnitMeasure,
                TipoRefeição = x.TypeMeal,
                DescriçãoTipoRefeição = x.TypeMealDescription,
                CodigoRegião = x.RegionCode,
                CodigoArea = x.FunctionalAreaCode,
                CodigoCentroResponsabilidade = x.ResponsabilityCenterCode,
                DataHoraCriação = x.CreateDateTime == "" || x.CreateDateTime == null ? (DateTime?)null : DateTime.Parse(x.CreateDateTime),
                DataHoraModificação = x.UpdateDateTime == "" || x.UpdateDateTime == null ? (DateTime?)null : DateTime.Parse(x.UpdateDateTime),
                UtilizadorCriação = x.CreateUser,
                UtilizadorModificação = x.UpdateUser
            };
        }

        public static List<PreçosServiçosCliente> ParseToDatabase(this List<PriceServiceClientViewModel> items)
        {
            List<PreçosServiçosCliente> PriceServiceClient = new List<PreçosServiçosCliente>();
            if (items != null)
                items.ForEach(x =>
                    PriceServiceClient.Add(ParseToDatabase(x)));
            return PriceServiceClient;
        }
        #endregion
    }
}
