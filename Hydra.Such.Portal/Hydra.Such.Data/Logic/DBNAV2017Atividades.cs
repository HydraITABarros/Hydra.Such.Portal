using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.PedidoCotacao;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017Atividades
    {
        public static List<ActividadesView> GetAtividades(string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<ActividadesView> result = new List<ActividadesView>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Actividades @DBName, @CompanyName", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new ActividadesView()
                        {
                            CodActividade = (string)temp.CodActividade,
                            Descricao = (string)temp.Descricao
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
