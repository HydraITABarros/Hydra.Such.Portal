using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Projects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic.Project
{
    public class DBNAV2017JobLedgerEntries
    {
        public static List<NAVJobLedgerEntryViewModel> GetAll(string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVJobLedgerEntryViewModel> result = new List<NAVJobLedgerEntryViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoProjeto", ""),
                        new SqlParameter("@NoMovimento", null)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017MovimentosProjeto @DBName, @CompanyName, @NoCliente, @NoMovimento", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVJobLedgerEntryViewModel()
                        {
                            EntryNo = (string)temp.EntryNo,
                            JobNo = (string)temp.JobNo,
                            PostingDate = (string)temp.PostingDate,
                            DocumentDate = (string)temp.DocumentDate,
                            EntryType = (string)temp.EntryType,
                            DocumentNo = (string)temp.DocumentNo,
                            Type = (string)temp.Type,
                            No = (string)temp.No,
                            Description = (string)temp.Description,
                            Quantity = (decimal)temp.Quantity,
                            UnitOfMeasureCode = (string)temp.UnitOfMeasureCode,
                            UnitCost = (decimal)temp.UnitCost,
                            TotalCost = (decimal)temp.TotalCost,
                            UnitPrice = (decimal)temp.UnitPrice,
                            TotalPrice = (decimal)temp.TotalPrice,
                            LineAmount = (decimal)temp.LineAmount,
                            LocationCode = (string)temp.LocationCode,
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

        public static List<NAVJobLedgerEntryViewModel> GetFiltered(string projectNo, int? MovementNo, string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVJobLedgerEntryViewModel> result = new List<NAVJobLedgerEntryViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoProjeto", projectNo),
                        new SqlParameter("@NoMovimento", MovementNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017MovimentosProjeto @DBName, @CompanyName, @NoCliente, @NoMovimento", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVJobLedgerEntryViewModel()
                        {
                            EntryNo = (string)temp.EntryNo,
                            JobNo = (string)temp.JobNo,
                            PostingDate = (string)temp.PostingDate,
                            DocumentDate = (string)temp.DocumentDate,
                            EntryType = (string)temp.EntryType,
                            DocumentNo = (string)temp.DocumentNo,
                            Type = (string)temp.Type,
                            No = (string)temp.No,
                            Description = (string)temp.Description,
                            Quantity = (decimal)temp.Quantity,
                            UnitOfMeasureCode = (string)temp.UnitOfMeasureCode,
                            UnitCost = (decimal)temp.UnitCost,
                            TotalCost = (decimal)temp.TotalCost,
                            UnitPrice = (decimal)temp.UnitPrice,
                            TotalPrice = (decimal)temp.TotalPrice,
                            LineAmount = (decimal)temp.LineAmount,
                            LocationCode = (string)temp.LocationCode,
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
