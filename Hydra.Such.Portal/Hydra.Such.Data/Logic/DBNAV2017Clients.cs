using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017Clients
    {
        public static NAVClientsViewModel GetClientById(string NAVDatabaseName, string NAVCompanyName, string NAVClientNo)
        {
            List<NAVClientsViewModel> result = GetClients(NAVDatabaseName, NAVCompanyName, NAVClientNo);
            if (result != null)
                return result.FirstOrDefault();
            return null;
        }

        public static List<NAVClientsViewModel> GetClients(string NAVDatabaseName, string NAVCompanyName, IEnumerable<string> navCustomerIds)
        {
            string navCustomerIdsFilter = string.Join(",", navCustomerIds); 
            List<NAVClientsViewModel> customers = GetClients(NAVDatabaseName, NAVCompanyName, navCustomerIdsFilter);
            return customers;
        }

        public static List<NAVClientsViewModel> GetClients(string NAVDatabaseName, string NAVCompanyName, string NAVClientNo)
        {
            try
            {
                List<NAVClientsViewModel> result = new List<NAVClientsViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoCliente", NAVClientNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Clientes @DBName, @CompanyName, @NoCliente", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVClientsViewModel()
                        {
                            No_ = temp.No_.Equals(DBNull.Value) ? "" : (string)temp.No_,
                            Name = temp.Name.Equals(DBNull.Value) ? "" : (string)temp.Name,
                            VATRegistrationNo_ = temp.VATRegistrationNo.Equals(DBNull.Value) ? "" : (string)temp.VATRegistrationNo,
                            Address = temp.Address.Equals(DBNull.Value) ? "" : (string)temp.Address,
                            PostCode = temp.PostalCode.Equals(DBNull.Value) ? "" : (string)temp.PostalCode,
                            City = temp.PostalCode.Equals(DBNull.Value) ? "" : (string)temp.City,
                            Country_RegionCode = temp.Country_RegionCode.Equals(DBNull.Value) ? "" : (string)temp.Country_RegionCode,
                            UnderCompromiseLaw = temp.UnderCompromiseLaw.Equals(DBNull.Value) ? false : ((int)temp.UnderCompromiseLaw) == 0 ? false : true,
                            InternalClient = temp.InternalClient.Equals(DBNull.Value) ? false : ((int)temp.InternalClient) == 0 ? false : true,
                            National = temp.NationalCustomer.Equals(DBNull.Value) ? false : ((int)temp.NationalCustomer) == 0 ? false : true,
                            PaymentTermsCode = temp.PaymentTermsCode.Equals(DBNull.Value) ? "" : (string)temp.PaymentTermsCode,
                            PaymentMethodCode = temp.PaymentMethodCode.Equals(DBNull.Value) ? "" : (string)temp.PaymentMethodCode,
                            RegionCode = temp.RegionCode.Equals(DBNull.Value) ? "" : (string)temp.RegionCode,
                            FunctionalAreaCode = temp.FunctionalAreaCode.Equals(DBNull.Value) ? "" : (string)temp.FunctionalAreaCode,
                            ResponsabilityCenterCode = temp.ResponsabilityCenterCode.Equals(DBNull.Value) ? "" : (string)temp.ResponsabilityCenterCode,
                        });
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GetClientNameByNo(string NoClient, string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                string result = "";

                if (!string.IsNullOrEmpty(NoClient))
                {
                    using (var ctx = new SuchDBContextExtention())
                    {
                        var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoCliente", NoClient)
                    };

                        IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Clientes @DBName, @CompanyName, @NoCliente", parameters);

                        foreach (dynamic temp in data)
                        {
                            result = (string)temp.Name;

                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string GetClientVATByNo(string NoClient, string NAVDatabaseName, string NAVCompanyName)
            {
                try
                {
                    string result = "";
                    using (var ctx = new SuchDBContextExtention())
                    {
                        var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoCliente", NoClient)
                    };

                        IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Clientes @DBName, @CompanyName, @NoCliente", parameters);

                        foreach (dynamic temp in data)
                        {
                            result = (string)temp.VATRegistrationNo;
                        }
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    return "";
                }
            }

        public static string GetClientCurrencyByNo(string NoClient, string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                string result = "";
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoCliente", NoClient)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Clientes @DBName, @CompanyName, @NoCliente", parameters);

                    foreach (dynamic temp in data)
                    {
                        result = (string)temp.Currency;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static List<NAVClientesInvoicesViewModel> GetInvoices(string NAVDatabaseName, string NAVCompanyName, string NAVClientNo)
        {
            try
            {
                List<NAVClientesInvoicesViewModel> result = new List<NAVClientesInvoicesViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@CustomerNo", NAVClientNo ),
                        //new SqlParameter("@Regions", "''"),
                        //new SqlParameter("@FunctionalAreas", "''"),
                        //new SqlParameter("@RespCenters", "''"),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017ClientesInvoices @DBName, @CompanyName, @CustomerNo", parameters);
                    var minDate = new DateTime(2008, 1, 1);
                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVClientesInvoicesViewModel()
                        {
                            No_ = temp.No_.Equals(DBNull.Value) ? "" : (string)temp.No_,
                            ProjectNo = temp.ProjectNo.Equals(DBNull.Value) ? "" : (string)temp.ProjectNo,
                            DataServPrestado = temp.DataServPrestado.Equals(DBNull.Value) ? "" : (string)temp.DataServPrestado,
                            AmountIncludingVAT = temp.AmountIncludingVAT.Equals(DBNull.Value) ? "" : (string)temp.AmountIncludingVAT.ToString(),
                            BillToCustomerNo = temp.BillToCustomerNo.Equals(DBNull.Value) ? "" : (string)temp.BillToCustomerNo,
                            CreationDate = (DateTime?)temp.CreationDate,
                            DocumentDate = (DateTime?)temp.DocumentDate,
                            DueDate = (DateTime?)temp.DueDate,
                            FunctionalAreaId = temp.FunctionalAreaId.Equals(DBNull.Value) ? "" : (string)temp.FunctionalAreaId,
                            Paid = (bool)temp.Paid,
                            RegionId = temp.RegionId.Equals(DBNull.Value) ? "" : (string)temp.RegionId,
                            RespCenterId = temp.RespCenterId.Equals(DBNull.Value) ? "" : (string)temp.RespCenterId,
                            SellToCustomerNo = temp.SellToCustomerNo.Equals(DBNull.Value) ? "" : (string)temp.SellToCustomerNo,
                            Tipo = temp.Tipo.Equals(DBNull.Value) ? "" : (string)temp.Tipo,
                            ValorPendente = temp.ValorPendente.Equals(DBNull.Value) ? "" : (string)temp.ValorPendente.ToString()
                        });
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<NAVClientesBalanceControlViewModel> GetBalances(string NAVDatabaseName, string NAVCompanyName, string NAVClientNo)
        {
            try
            {
                var result = new List<NAVClientesBalanceControlViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@CustomerNo", NAVClientNo ),
                        //new SqlParameter("@Regions", "''"),
                        //new SqlParameter("@FunctionalAreas", "''"),
                        //new SqlParameter("@RespCenters", "''"),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017ClientesBalances @DBName, @CompanyName, @CustomerNo", parameters);
                    var minDate = new DateTime(2008, 1, 1);
                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVClientesBalanceControlViewModel()
                        {
                            EntryNo = (int)temp.EntryNo,
                            Amount = (decimal?)temp.Amount,
                            CustomerNo = temp.CustomerNo.Equals(DBNull.Value) ? "" : (string)temp.CustomerNo,
                            DataConcil = (DateTime?)temp.DataConcil != null && (DateTime?)temp.DataConcil > minDate ? (DateTime?)temp.DataConcil : null,
                            Description = temp.Description.Equals(DBNull.Value) ? "" : (string)temp.Description,
                            DocumentNo = temp.DocumentNo.Equals(DBNull.Value) ? "" : (string)temp.DocumentNo,
                            DocumentType = temp.DocumentType.Equals(DBNull.Value) ? "" : (string)temp.DocumentType.ToString(),
                            Obs = temp.Obs.Equals(DBNull.Value) ? "" : (string)temp.Obs,
                            PostingDate = (DateTime?)temp.PostingDate,
                            RemainingAmount = (decimal?)temp.RemainingAmount,
                            SinalizacaoRec = (int?)temp.SinalizacaoRec == 0 || (int?)temp.SinalizacaoRec == null ? false : true ,
                            DocumentDate = (DateTime?)temp.DocumentDate,
                            DueDate = (DateTime?)temp.DueDate,
                            FunctionalAreaId = temp.FunctionalAreaId.Equals(DBNull.Value) ? "" : (string)temp.FunctionalAreaId,
                            RegionId = temp.RegionId.Equals(DBNull.Value) ? "" : (string)temp.RegionId,
                            RespCenterId = temp.RespCenterId.Equals(DBNull.Value) ? "" : (string)temp.RespCenterId
                        });
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static int? UpdateBalance(string NAVDatabaseName, string NAVCompanyName, string NAVClientNo, string EntryNo, string SinalizacaoRec, string Obs)
        {
            try
            {
                var result = new List<NAVClientesBalanceControlViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var _parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@CustomerNo", NAVClientNo ),
                        new SqlParameter("@Obs", Obs ),
                        new SqlParameter("@SinalizacaoRec", SinalizacaoRec ),
                        new SqlParameter("@EntryNo", EntryNo )
                    };

                    var data = ctx.execStoredProcedureNQ("exec NAV2017ClientesBalancesUpdate @DBName, @CompanyName, @CustomerNo, @Obs, @SinalizacaoRec, @EntryNo", _parameters);

                    return data;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<NAVClientesInvoicesDetailsViewModel> GetInvoiceDetails(string NAVDatabaseName, string NAVCompanyName, string No)
        {
            try
            {
                List<NAVClientesInvoicesDetailsViewModel> result = new List<NAVClientesInvoicesDetailsViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@No", No ),
                        //new SqlParameter("@Regions", "''"),
                        //new SqlParameter("@FunctionalAreas", "''"),
                        //new SqlParameter("@RespCenters", "''"),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017ClientesInvoicesDetails @DBName, @CompanyName, @No", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVClientesInvoicesDetailsViewModel()
                        {
                            No = temp.No.Equals(DBNull.Value) ? "" : (string)temp.No.ToString(),
                            AmountIncludingVAT = temp.AmountIncludingVAT.Equals(DBNull.Value) ? "" : (string)temp.AmountIncludingVAT.ToString(),
                            FunctionalAreaId = temp.FunctionalAreaId.Equals(DBNull.Value) ? "" : (string)temp.FunctionalAreaId.ToString(),
                            RegionId = temp.RegionId.Equals(DBNull.Value) ? "" : (string)temp.RegionId.ToString(),
                            RespCenterId = temp.RespCenterId.Equals(DBNull.Value) ? "" : (string)temp.RespCenterId.ToString(),
                            SellToCustomerNo = temp.SellToCustomerNo.Equals(DBNull.Value) ? "" : (string)temp.SellToCustomerNo.ToString(),
                            Tipo = temp.Tipo.Equals(DBNull.Value) ? "" : (string)temp.Tipo.ToString(),
                            Amount = temp.Amount.Equals(DBNull.Value) ? "" : (string)temp.Amount.ToString(),
                            Description = temp.Description.Equals(DBNull.Value) ? "" : (string)temp.Description.ToString(),
                            Description2 = temp.Description2.Equals(DBNull.Value) ? "" : (string)temp.Description2.ToString(),
                            DimensionSetID = temp.DimensionSetID.Equals(DBNull.Value) ? "" : (string)temp.DimensionSetID.ToString(),
                            DocumentNo = temp.DocumentNo.Equals(DBNull.Value) ? "" : (string)temp.DocumentNo.ToString(),
                            LineNo = temp.LineNo_.Equals(DBNull.Value) ? "" : (string)temp.LineNo_.ToString(),
                            Quantity = temp.Quantity.Equals(DBNull.Value) ? "" : (string)temp.Quantity.ToString(),
                            ServiceContractNo = temp.ServiceContractNo.Equals(DBNull.Value) ? "" : (string)temp.ServiceContractNo.ToString(),
                            UnitOfMeasure = temp.UnitOfMeasure.Equals(DBNull.Value) ? "" : (string)temp.UnitOfMeasure.ToString(),
                            UnitPrice = temp.UnitPrice.Equals(DBNull.Value) ? "" : (string)temp.UnitPrice.ToString(),
                            VAT = temp.VAT.Equals(DBNull.Value) ? "" : (string)temp.VAT.ToString()

                        });
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public static List<NAVClientesInvoicesDetailsViewModel> GetCrMemoDetails(string NAVDatabaseName, string NAVCompanyName, string No)
        {
            try
            {
                List<NAVClientesInvoicesDetailsViewModel> result = new List<NAVClientesInvoicesDetailsViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@No", No ),
                        //new SqlParameter("@Regions", "''"),
                        //new SqlParameter("@FunctionalAreas", "''"),
                        //new SqlParameter("@RespCenters", "''"),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017ClientesCrMemoDetails @DBName, @CompanyName, @No", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVClientesInvoicesDetailsViewModel()
                        {
                            No = temp.No.Equals(DBNull.Value) ? "" : (string)temp.No.ToString(),
                            AmountIncludingVAT = temp.AmountIncludingVAT.Equals(DBNull.Value) ? "" : (string)temp.AmountIncludingVAT.ToString(),
                            FunctionalAreaId = temp.FunctionalAreaId.Equals(DBNull.Value) ? "" : (string)temp.FunctionalAreaId.ToString(),
                            RegionId = temp.RegionId.Equals(DBNull.Value) ? "" : (string)temp.RegionId.ToString(),
                            RespCenterId = temp.RespCenterId.Equals(DBNull.Value) ? "" : (string)temp.RespCenterId.ToString(),
                            SellToCustomerNo = temp.SellToCustomerNo.Equals(DBNull.Value) ? "" : (string)temp.SellToCustomerNo.ToString(),
                            Tipo = temp.Tipo.Equals(DBNull.Value) ? "" : (string)temp.Tipo.ToString(),
                            Amount = temp.Amount.Equals(DBNull.Value) ? "" : (string)temp.Amount.ToString(),
                            Description = temp.Description.Equals(DBNull.Value) ? "" : (string)temp.Description.ToString(),
                            Description2 = temp.Description2.Equals(DBNull.Value) ? "" : (string)temp.Description2.ToString(),
                            DimensionSetID = temp.DimensionSetID.Equals(DBNull.Value) ? "" : (string)temp.DimensionSetID.ToString(),
                            DocumentNo = temp.DocumentNo.Equals(DBNull.Value) ? "" : (string)temp.DocumentNo.ToString(),
                            LineNo = temp.LineNo_.Equals(DBNull.Value) ? "" : (string)temp.LineNo_.ToString(),
                            Quantity = temp.Quantity.Equals(DBNull.Value) ? "" : (string)temp.Quantity.ToString(),
                            ServiceContractNo = temp.ServiceContractNo.Equals(DBNull.Value) ? "" : (string)temp.ServiceContractNo.ToString(),
                            UnitOfMeasure = temp.UnitOfMeasure.Equals(DBNull.Value) ? "" : (string)temp.UnitOfMeasure.ToString(),
                            UnitPrice = temp.UnitPrice.Equals(DBNull.Value) ? "" : (string)temp.UnitPrice.ToString(),
                            VAT = temp.VAT.Equals(DBNull.Value) ? "" : (string)temp.VAT.ToString()

                        });
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
