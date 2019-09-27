using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017Fornecedores
    {
        public static List<NAVFornecedoresViewModel> GetFornecedores(string NAVDatabaseName, string NAVCompanyName, string NAVFornecedorNo)
        {
            try
            {
                List<NAVFornecedoresViewModel> result = new List<NAVFornecedoresViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoFornecedor", NAVFornecedorNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017FornecedoresLista @DBName, @CompanyName, @NoFornecedor", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVFornecedoresViewModel()
                        {
                            No = temp.No.Equals(DBNull.Value) ? "" : (string)temp.No,
                            Name = temp.Name.Equals(DBNull.Value) ? "" : (string)temp.Name,
                            FullAddress = temp.FullAddress.Equals(DBNull.Value) ? "" : (string)temp.FullAddress,
                            PostCode = temp.PostCode.Equals(DBNull.Value) ? "" : (string)temp.PostCode,
                            City = temp.City.Equals(DBNull.Value) ? "" : (string)temp.City,
                            Country = temp.Country.Equals(DBNull.Value) ? "" : (string)temp.Country,
                            Phone = temp.Phone.Equals(DBNull.Value) ? "" : (string)temp.Phone,
                            Email = temp.Email.Equals(DBNull.Value) ? "" : (string)temp.Email,
                            Fax = temp.Fax.Equals(DBNull.Value) ? "" : (string)temp.Fax,
                            HomePage = temp.HomePage.Equals(DBNull.Value) ? "" : (string)temp.HomePage,
                            VATRegistrationNo = temp.VATRegistrationNo.Equals(DBNull.Value) ? "" : (string)temp.VATRegistrationNo,
                            PaymentTermsCode = temp.PaymentTermsCode.Equals(DBNull.Value) ? "" : (string)temp.PaymentTermsCode,
                            PaymentMethodCode = temp.PaymentMethodCode.Equals(DBNull.Value) ? "" : (string)temp.PaymentMethodCode,
                            NoClienteAssociado = temp.NoClienteAssociado.Equals(DBNull.Value) ? "" : (string)temp.NoClienteAssociado,
                            Blocked = (int)temp.Blocked,
                            BlockedText = temp.BlockedText.Equals(DBNull.Value) ? "" : (string)temp.BlockedText,
                            Address = temp.Address.Equals(DBNull.Value) ? "" : (string)temp.Address,
                            Address2 = temp.Address2.Equals(DBNull.Value) ? "" : (string)temp.Address2,
                            Distrito = temp.Distrito.Equals(DBNull.Value) ? "" : (string)temp.Distrito,
                            Criticidade = (int)temp.Criticidade,
                            CriticidadeText = temp.CriticidadeText.Equals(DBNull.Value) ? "" : (string)temp.CriticidadeText,
                            Observacoes = temp.Observacoes.Equals(DBNull.Value) ? "" : (string)temp.Observacoes,
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
