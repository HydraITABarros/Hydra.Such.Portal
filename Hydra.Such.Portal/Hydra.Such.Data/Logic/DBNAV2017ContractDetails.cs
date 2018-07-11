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



        public static List<NAVContractInvoiceHeaderViewModel> GetContractInvoiceHeaderByNo(string ContractNo, string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVContractInvoiceHeaderViewModel> result = new List<NAVContractInvoiceHeaderViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoContrato", ContractNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2009FaturasContrato @DBName, @CompanyName, @NoContrato", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVContractInvoiceHeaderViewModel()
                        {
                            No_ = (string)temp.No_,
                            ValorContrato = (decimal?)temp.ValorContrato
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

        public static List<NAVContractInvoiceLinesViewModel> GetContractInvoiceLinesByNo(string ContractNo, string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVContractInvoiceLinesViewModel> result = new List<NAVContractInvoiceLinesViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoContrato", ContractNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2009LinhasFaturaContrato @DBName, @CompanyName, @NoContrato", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVContractInvoiceLinesViewModel()
                        {
                            No_ = (string)temp.No_,
                            DocNo = (string)temp.DocNo,
                            Description = (string)temp.Description,
                            Description2 = (string)temp.Description2,
                            UnitOfMeasure = (string)temp.UnitOfMeasure,
                            Quantity = (decimal?)temp.Quantity,
                            UnitPrice = (decimal?)temp.UnitPrice,
                            Amount = (decimal?)temp.Amount,
                            AmountIncludingVAT = (decimal?)temp.AmountIncludingVAT,
                            JobNo = (string)temp.JobNo,
                            ExternalShipmentNo_ = (string)temp.ExternalShipmentNo_,
                            DataRegistoDiario = (string)temp.DataRegistoDiario,
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
