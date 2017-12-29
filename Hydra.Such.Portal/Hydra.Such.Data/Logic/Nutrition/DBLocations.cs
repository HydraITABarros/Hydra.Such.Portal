using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Nutrition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public static class DBLocations
    {
        public static List<Localizações> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Localizações.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Localizações Create(Localizações ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.Localizações.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Localizações Update(Localizações ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.Localizações.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Localizações GetById(string Code)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Localizações
                        .FirstOrDefault(x => x.Código == Code);
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        
        #region Parses

        public static LocationViewModel ParseToViewModel(Localizações x)
        {
            return new LocationViewModel()
            {
                Code = x.Código,
                Name = x.Nome,
                Address = x.Endereço,
                City = x.Cidade,
                MobilePhone = x.Telefone,
                Fax = x.NºFax,
                Contact = x.Contato,
                ZipCode = x.CódPostal,
                Email = x.Email,
                Locked = x.Bloqueado,
                Region = x.Região,
                Area = x.Área,
                ResponsabilityCenter = x.CentroResponsabilidade,
                SupplierLocation = x.LocalFornecedor,
                ShipLocationCode = x.CódigoLocalEntrega,
                WarehouseManager = x.ResponsávelArmazém,
                WarehouseEnvironment = x.ArmazémAmbiente,
                CreateDate = x.DataHoraCriação,
                CreateUser = x.UtilizadorCriação,
                UpdateDate = x.DataHoraModificação,
                UpdateUser = x.UtilizadorCriação
            };
        }

        public static List<LocationViewModel> ParseToViewModel(this List<Localizações> items)
        {
            List<LocationViewModel> locations = new List<LocationViewModel>();
            if (items != null)
                items.ForEach(x =>
                    locations.Add(ParseToViewModel(x)));
            return locations;
        }

        public static Localizações ParseToDatabase(LocationViewModel x)
        {
            return new Localizações()
            {
                Código = x.Code,
                Nome = x.Name,
                Endereço = x.Address,
                Cidade = x.City,
                Telefone = x.MobilePhone,
                NºFax = x.Fax,
                Contato = x.Contact,
                CódPostal = x.ZipCode,
                Email = x.Email,
                Bloqueado = x.Locked,
                Região = x.Region,
                Área = x.Area,
                CentroResponsabilidade = x.ResponsabilityCenter,
                LocalFornecedor = x.SupplierLocation,
                CódigoLocalEntrega = x.ShipLocationCode,
                ResponsávelArmazém = x.WarehouseManager,
                ArmazémAmbiente = x.WarehouseEnvironment,
                DataHoraCriação = x.CreateDate,
                UtilizadorCriação = x.CreateUser,
                DataHoraModificação = x.UpdateDate,
                UtilizadorModificação = x.UpdateUser
            };
        }

        public static List<Localizações> ParseToDatabase(this List<LocationViewModel> items)
        {
            List<Localizações> locations = new List<Localizações>();
            if (items != null)
                items.ForEach(x =>
                    locations.Add(ParseToDatabase(x)));
            return locations;
        }

        #endregion
    }
}
