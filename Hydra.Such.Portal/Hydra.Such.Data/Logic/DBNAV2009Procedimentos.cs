using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Hydra.Such.Data.ViewModel.Fornecedores;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2009Procedimentos
    {
        public static List<ProcedimentosViewModel> ListProcedimentos2Years(string NAVServerName, string NAVDatabaseName, string NAVCompanyName, string procedimentoNo = null)
        {
            try
            {
                List<ProcedimentosViewModel> result = new List<ProcedimentosViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@ServerName", NAVServerName),
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@ProcedimentoNo", procedimentoNo )
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2009ProcedimentosGetAll2Years @ServerName, @DBName, @CompanyName, @ProcedimentoNo", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new ProcedimentosViewModel()
                        {
                            No = temp.No.Equals(DBNull.Value) ? "" : (string)temp.No,
                            Local = temp.Local.Equals(DBNull.Value) ? "" : (string)temp.Local,
                            Regiao = temp.Regiao.Equals(DBNull.Value) ? "" : (string)temp.Regiao,
                            Area = temp.Area.Equals(DBNull.Value) ? "" : (string)temp.Area,
                            Cresp = temp.Cresp.Equals(DBNull.Value) ? "" : (string)temp.Cresp
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

        public static List<DDMessageString> ListProcedimentosFornecedores(string NAVServerName, string NAVDatabaseName, string NAVCompanyName, string procedimentoNo = null)
        {
            try
            {
                List<DDMessageString> result = new List<DDMessageString>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@ServerName", NAVServerName),
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@ProcedimentoNo", procedimentoNo )
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2009ProcedimentosGetAllFornecedores @ServerName, @DBName, @CompanyName, @ProcedimentoNo", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new DDMessageString()
                        {
                            id = temp.No.Equals(DBNull.Value) ? "" : (string)temp.No,
                            value = temp.Name.Equals(DBNull.Value) ? "" : (string)temp.Name
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

        public class DDMessageString
        {
            public string id { get; set; }
            public string value { get; set; }
        }


    }
}
