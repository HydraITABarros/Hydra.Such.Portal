using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBNAV2009Projects
    {
        public static List<NAVProjectsViewModel> GetAll(string NAVServerName, string NAVDatabaseName, string NAVCompanyName, string ProjectNo)
        {
            try
            {
                List<NAVProjectsViewModel> result = new List<NAVProjectsViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@ServerName", NAVServerName),
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@ProjectNo", ProjectNo),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2009ProjetosGetAll @ServerName, @DBName, @CompanyName, @ProjectNo", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVProjectsViewModel()
                        {
                            No = temp.No_ != null ? (string)temp.No_ : (string)"",
                            Description = temp.Description != null ? (string)temp.Description : (string)"",
                            CustomerNo = temp.BillToCustomerNo_ != null ? (string)temp.BillToCustomerNo_ : (string)"",
                            CustomerName = temp.BillToCustomerName != null ? (string)temp.BillToCustomerName : (string)"",
                            RegionCode = temp.GlobalDimension1Code != null ? (string)temp.GlobalDimension1Code : (string)"",
                            AreaCode = temp.GlobalDimension2Code != null ? (string)temp.GlobalDimension2Code : (string)"",
                            CenterResponsibilityCode = temp.GlobalDimension3Code != null ? (string)temp.GlobalDimension3Code : (string)""
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

        public static int Create(string Matricula, string RegionCode, string FunctionalAreaCode, string ResponsabilityCenterCode, string UserID)
        {
            try
            {
                int result = 0;
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@Matricula", Matricula),
                        new SqlParameter("@GlobalDimension1Code", RegionCode),
                        new SqlParameter("@GlobalDimension2Code", FunctionalAreaCode),
                        new SqlParameter("@GlobalDimension3Code", ResponsabilityCenterCode),
                        new SqlParameter("@Utilizador", UserID),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2009ProjetosCreate @Matricula, @GlobalDimension1Code, @GlobalDimension2Code, @GlobalDimension3Code, @Utilizador", parameters);

                    foreach (dynamic temp in data)
                    {
                        result = temp.Result != null ? (int)temp.Result : (int)0;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int Update(string Matricula, int Estado, string RegionCode, string FunctionalAreaCode, string ResponsabilityCenterCode, string UserID)
        {
            try
            {
                int result = 0;
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@Matricula", Matricula),
                        new SqlParameter("@Estado", Estado),
                        new SqlParameter("@GlobalDimension1Code", RegionCode),
                        new SqlParameter("@GlobalDimension2Code", FunctionalAreaCode),
                        new SqlParameter("@GlobalDimension3Code", ResponsabilityCenterCode),
                        new SqlParameter("@Utilizador", UserID),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2009ProjetosUpdate @Matricula, @Estado, @GlobalDimension1Code, @GlobalDimension2Code, @GlobalDimension3Code, @Utilizador", parameters);

                    foreach (dynamic temp in data)
                    {
                        result = temp.Result != null ? (int)temp.Result : (int)0;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int Delete(string ProjectNo)
        {
            try
            {
                int result = 0;
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@ProjectNo", ProjectNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2009ProjetosDelete @ProjectNo", parameters);

                    foreach (dynamic temp in data)
                    {
                        result = temp.Result != null ? (int)temp.Result : (int)0;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
