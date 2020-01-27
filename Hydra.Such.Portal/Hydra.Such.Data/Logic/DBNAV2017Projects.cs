using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBNAV2017Projects
    {
        public static List<NAVProjectsViewModel> GetAll(string NAVDatabaseName, string NAVCompanyName, string ProjectNo)
        {
            try
            {
                List<NAVProjectsViewModel> result = new List<NAVProjectsViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@ProjectNo", ProjectNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Projetos @DBName, @CompanyName, @ProjectNo", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVProjectsViewModel()
                        {
                            No = (string)temp.No_,
                            Description = (string)temp.Description,
                            CustomerNo = (string)temp.BillToCustomerNo_,
                            CustomerName = (string)temp.BillToCustomerName,
                            GlobalDimension1 = (string)temp.GlobalDimension1Code,
                            GlobalDimension2 = (string)temp.GlobalDimension2Code,
                            AreaCode = (string)temp.AreaCode,
                            RegionCode = (string)temp.RegionCode,
                            CenterResponsibilityCode = (string)temp.CenterResponsibilityCode
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

        public static List<NAVProjectsViewModel> GetAllInDB(string NAVDatabaseName, string NAVCompanyName, string ProjectNo)
        {
            try
            {
                List<NAVProjectsViewModel> result = new List<NAVProjectsViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@ProjectNo", ProjectNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017ProjetosGetAll @DBName, @CompanyName, @ProjectNo", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVProjectsViewModel()
                        {
                            No = (string)temp.No_,
                            Description = (string)temp.Description,
                            CustomerNo = (string)temp.BillToCustomerNo_,
                            CustomerName = (string)temp.BillToCustomerName,
                            GlobalDimension1 = (string)temp.GlobalDimension1Code,
                            GlobalDimension2 = (string)temp.GlobalDimension2Code,
                            AreaCode = (string)temp.AreaCode,
                            RegionCode = (string)temp.RegionCode,
                            CenterResponsibilityCode = (string)temp.CenterResponsibilityCode
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

        public static decimal? GetTotalInvoiceValue(string NAVDatabaseName, string NAVCompanyName, string projectNo)
        {
            try
            {
                decimal result = 0;
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@ProjectNo", projectNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017ValorFaturasDoProjeto @DBName, @CompanyName, @ProjectNo", parameters);
                    
                    foreach (dynamic temp in data)
                    {
                        result += temp.ValorFaturas.Equals(DBNull.Value) ? 0 : (decimal)temp.ValorFaturas;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<NAVProjectsMovementsViaturasViewModel> GetAllMovimentsByViatura(string NAVDatabaseName, string NAVCompanyName, string NoProjeto)
        {
            try
            {
                List<NAVProjectsMovementsViaturasViewModel> result = new List<NAVProjectsMovementsViaturasViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoProjeto", NoProjeto)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017MovimentosProjetoViaturas @DBName, @CompanyName, @NoProjeto", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVProjectsMovementsViaturasViewModel()
                        {
                            Data = (string)temp.Data,
                            Tipo = (string)temp.Tipo,
                            Codigo = (string)temp.Codigo,
                            Descricao = (string)temp.Descricao,
                            Quantidade = (decimal)temp.Quantidade,
                            CodigoUnidadeMedida = (string)temp.CodigoUnidadeMedida,
                            CustoUnitario = (decimal)temp.CustoUnitario,
                            CustoTotal = (decimal)temp.CustoTotal,
                            Regiao = (string)temp.Regiao,
                            Area = (string)temp.Area,
                            Cresp = (string)temp.Cresp,
                            DocumentoNo = (string)temp.DocumentoNo,
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

        public static List<NAVProjectsMovementsViaturasViewModel> GetAllCustosByViatura(string NAVDatabaseName, string NAVCompanyName, string NoProjeto)
        {
            try
            {
                List<NAVProjectsMovementsViaturasViewModel> result = new List<NAVProjectsMovementsViaturasViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoProjeto", NoProjeto)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017CustosProjetoViaturas @DBName, @CompanyName, @NoProjeto", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVProjectsMovementsViaturasViewModel()
                        {
                            Data = (string)temp.Data,
                            DocumentoNo = (string)temp.DocumentoNo,
                            Codigo = (string)temp.Codigo,
                            Descricao = (string)temp.Descricao,
                            CustoTotal = (decimal)temp.CustoTotal,
                            Regiao = (string)temp.Regiao,
                            Area = (string)temp.Area,
                            Cresp = (string)temp.Cresp,
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
