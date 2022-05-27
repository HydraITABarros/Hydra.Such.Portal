using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBNAV2017ContratosFornecedor
    {
        public static List<NAV2017ContratosFornecedorViewModel> GetContratosFornecedorByNoFornecedor(string DBName, string CompanyName, string NoFornecedor)
        {
            try
            {
                List<NAV2017ContratosFornecedorViewModel> result = new List<NAV2017ContratosFornecedorViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", DBName),
                        new SqlParameter("@CompanyName", CompanyName),
                        new SqlParameter("@NoFornecedor", NoFornecedor)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017ContratosFornecedor @DBName, @CompanyName, @NoFornecedor", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAV2017ContratosFornecedorViewModel()
                        {
                            NoContrato = (string)temp.NoContrato,
                            NameContrato = (string)temp.NameContrato,
                            NoFornecedor = (string)temp.NoFornecedor,
                            NameFornecedor = (string)temp.NameFornecedor,
                            DataCelebracao = (DateTime?)temp.DataCelebracao,
                            DataCelebracaoTexto = temp.DataCelebracao != null && temp.DataCelebracao.ToString("yyyy-MM-dd") != "1753-01-01" ? temp.DataCelebracao.ToString("yyyy-MM-dd") : "",
                            DataConclusaoInicial = (DateTime?)temp.DataConclusaoInicial,
                            DataConclusaoInicialTexto = temp.DataConclusaoInicial != null && temp.DataConclusaoInicial.ToString("yyyy-MM-dd") != "1753-01-01" ? temp.DataConclusaoInicial.ToString("yyyy-MM-dd") : "",
                            DataConclusaoRevista = (DateTime?)temp.DataConclusaoRevista,
                            DataConclusaoRevistaTexto = temp.DataConclusaoRevista != null && temp.DataConclusaoRevista.ToString("yyyy-MM-dd") != "1753-01-01" ? temp.DataConclusaoRevista.ToString("yyyy-MM-dd") : "",
                            PrecoBase = (decimal?)temp.PrecoBase,
                            PrecoContratual = (decimal?)temp.PrecoContratual,
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
