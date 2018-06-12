using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Extensions;
namespace Hydra.Such.Data.Logic.Contracts
{
    public static class DBFetcUnit
    {
        #region CRUD
        public static List<Contratos> GetByNo(string ContractNo, bool Archived)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Contratos.Where(x => x.NºDeContrato == ContractNo && x.Arquivado == Archived).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<UnidadePrestação> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UnidadePrestação.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static UnidadePrestação Create(UnidadePrestação ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.UnidadePrestação.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(UnidadePrestação ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.UnidadePrestação.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static UnidadePrestação Update(UnidadePrestação ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.UnidadePrestação.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

     
        #endregion

      




        public static UnidadePrestação ParseToDB(FetcUnitViewModel x)
        {
            UnidadePrestação result = new UnidadePrestação()
            {
                Código = x.Code,
                Descrição = x.Description,
                DataHoraCriação = string.IsNullOrEmpty(x.CreateDate) ? (DateTime?)null : DateTime.Parse(x.CreateDate),
                DataHoraModificação = string.IsNullOrEmpty(x.UpdateDate) ? (DateTime?)null : DateTime.Parse(x.UpdateDate),
                UtilizadorCriação = x.CreateUser,
                UtilizadorModificação = x.UpdateUser,
              
            };

            return result;

        }

        public static FetcUnitViewModel ParseToViewModel(UnidadePrestação x)
        {
            if (x == null)
                return null;
            FetcUnitViewModel result = new FetcUnitViewModel()
            {
                Code = x.Código,
                Description = x.Descrição,            
                CreateDate = x.DataHoraCriação.HasValue ? x.DataHoraCriação.Value.ToString("yyyy-MM-dd") : "",
                UpdateDate = x.DataHoraModificação.HasValue ? x.DataHoraModificação.Value.ToString("yyyy-MM-dd") : "",
                CreateUser = x.UtilizadorCriação,
                UpdateUser = x.UtilizadorModificação,
               
            };

            return result;

        }
    }
}
