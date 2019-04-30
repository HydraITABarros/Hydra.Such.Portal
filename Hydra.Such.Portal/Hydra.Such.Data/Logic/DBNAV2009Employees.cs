using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBNAV2009Employees
    {
        public static List<NAVEmployeeViewModel> GetAll(string NoEmpregado, string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVEmployeeViewModel> result = new List<NAVEmployeeViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoEmpregado", NoEmpregado),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2009Empregados @DBName, @CompanyName, @NoEmpregado", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVEmployeeViewModel()
                        {
                            No = temp.No_ != null ? (string)temp.No_ : (string)"",
                            Name = temp.Name != null ? (string)temp.Name : (string)"",
                            Regiao = temp.Regiao != null ? (string)temp.Regiao : (string)"",
                            Area = temp.Area != null ? (string)temp.Area : (string)"",
                            Cresp = temp.Cresp != null ? (string)temp.Cresp : (string)"",
                            Responsavel1 = temp.Responsavel1 != null ? (string)temp.Responsavel1 : (string)"",
                            Responsavel2 = temp.Responsavel2 != null ? (string)temp.Responsavel2 : (string)"",
                            Responsavel3 = temp.Responsavel3 != null ? (string)temp.Responsavel3 : (string)"",
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

        /// <summary>
        /// Obter Gestores de Processo,para Procedimentos CCP, consultando a tabela NAV2009 "User Setup" e a view "RH_Employee", filtrando por "[CkList Gestor Processo] = 1 AND [CkList Elemento Juri] = 1"
        /// NR 20180228
        /// </summary>
        /// <param name="NAVDatabaseName"></param>
        /// <param name="NAVCompanyName"></param>
        /// <returns></returns>
        public static List<NAVEmployeeViewModel> GetAllGestorProcesso(string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVEmployeeViewModel> result = new List<NAVEmployeeViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2009GestorProcesso @DBName, @CompanyName", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVEmployeeViewModel()
                        {
                            No = (string)temp.No_,
                            Name = (string)temp.Name,
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
    }
}
