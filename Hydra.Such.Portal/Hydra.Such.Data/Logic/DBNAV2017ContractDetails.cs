using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBNAV2017ContractDetails
    {
        public static NAVContractDetailsViewModel GetContractByNo(string ContractNo, string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                NAVContractDetailsViewModel result = new NAVContractDetailsViewModel();
                string currency = "";
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoCliente", ContractNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017DadosContrato @DBName, @CompanyName, @NoCliente", parameters);

                    foreach (dynamic temp in data)
                    {
                        //result.NBilledInvoices = temp.NFaturasRegistadas != DBNull.Value ? (decimal?)temp.NFaturasRegistadas : null;
                        //result.NCreditNotes = temp.NNotasCreditoRegistadas != DBNull.Value ? (decimal?)temp.NNotasCreditoRegistadas : null;
                        //result.VBilled = temp.VFaturado != DBNull.Value ? (decimal?)temp.VFaturado : null;
                        //result.VCreditNotes = temp.VNotasCredito != DBNull.Value ? (decimal?)temp.VNotasCredito : null;

                        result.NBilledInvoices = (decimal?)temp.NFaturasRegistadas;
                        result.NCreditNotes = (decimal?)temp.NNotasCreditoRegistadas;
                        result.VBilled = (decimal?)temp.VFaturado;
                        result.VCreditNotes = (decimal?)temp.VNotasCredito;
                        result.VPeriod = (decimal?)temp.VPeriodo;
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
