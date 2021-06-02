using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.Logic
{ 
    public static class DBNAV2017SalesHeader
    {
        public static NAVSalesHeaderViewModel GetSalesHeader(string NAVDatabaseName, string NAVCompanyName, string NAVContractNo, NAVBaseDocumentTypes documentType)
        {
            try
            {
                List<NAVSalesHeaderViewModel> result = new List<NAVSalesHeaderViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@DocumentType",(int)documentType),
                        new SqlParameter("@ContractNo_", NAVContractNo)                                         
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017CabecalhoVendas @DBName, @CompanyName, @DocumentType, @ContractNo_", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVSalesHeaderViewModel()
                        {
                            No = (string)temp.DocumentNo,
                            ContractNo = (string)temp.ContractNo_,
                            DocumentType = (int)temp.DocumentType,
                           
                        });
                    }
                }
                return result[0];
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
