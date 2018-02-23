using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Compras
{
    public class DBRequesitionType
    {
        #region CRUD

        public static List<NAVRequisitionTypeViewModel> GetAll(string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVRequisitionTypeViewModel> result = new List<NAVRequisitionTypeViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017TiposRequisicoes @DBName, @CompanyName", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVRequisitionTypeViewModel()
                        {
                            Code = (string)temp.Tipo,
                            Description = (string)temp.Description,
                        });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static NAVRequisitionTypeViewModel GetById(string code, string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVRequisitionTypeViewModel> result = new List<NAVRequisitionTypeViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017TiposRequisicoes @DBName, @CompanyName", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVRequisitionTypeViewModel()
                        {
                            Code = (string)temp.Tipo,
                            Description = (string)temp.Description,
                        });
                    }
                }

                return result.Where(x => x.Code == code).FirstOrDefault();
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        //public static TiposRequisições Create(TiposRequisições ObjectToCreate)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            ObjectToCreate.DataHoraCriação = DateTime.Now;
        //            ctx.TiposRequisições.Add(ObjectToCreate);
        //            ctx.SaveChanges();
        //        }

        //        return ObjectToCreate;
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

        //public static TiposRequisições Update(TiposRequisições ObjectToUpdate)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            ObjectToUpdate.DataHoraModificação = DateTime.Now;
        //            ctx.TiposRequisições.Update(ObjectToUpdate);
        //            ctx.SaveChanges();
        //        }

        //        return ObjectToUpdate;
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}


        //public static bool Delete(TiposRequisições ObjectToDelete)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            ctx.TiposRequisições.Remove(ObjectToDelete);
        //            ctx.SaveChanges();
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {

        //        return false;
        //    }
        //}
        #endregion
    }
}
