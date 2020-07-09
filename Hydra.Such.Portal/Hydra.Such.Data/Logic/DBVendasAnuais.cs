using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Hydra.Such.Data.Logic
{
    public static class DBVendasAnuais
    {
        public static List<VendasAnuais> GetAllByFilterToList(int Ano, string Regiao)
        {
            try
            {
                List<VendasAnuais> result = new List<VendasAnuais>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@Ano", Ano),
                        new SqlParameter("@Regiao", Regiao)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017MapaVendasAnuais @Ano, @Regiao", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new VendasAnuais()
                        {
                            Ano = temp.Ano.Equals(DBNull.Value) ? 0 : (int)temp.Ano,
                            Regiao = temp.Regiao.Equals(DBNull.Value) ? "" : temp.Regiao,
                            NoAssociado = temp.NoAssociado.Equals(DBNull.Value) ? "" : Convert.ToString(temp.NoAssociado).Equals("x") ? "" : (string)temp.NoAssociado,
                            NomeAssociado = temp.NomeAssociado.Equals(DBNull.Value) ? "" : (string)temp.NomeAssociado,
                            Jan = temp.Jan.Equals(DBNull.Value) ? 0 : (decimal)temp.Jan,
                            Fev = temp.Fev.Equals(DBNull.Value) ? 0 : (decimal)temp.Fev,
                            Mar = temp.Mar.Equals(DBNull.Value) ? 0 : (decimal)temp.Mar,
                            Abr = temp.Abr.Equals(DBNull.Value) ? 0 : (decimal)temp.Abr,
                            Mai = temp.Mai.Equals(DBNull.Value) ? 0 : (decimal)temp.Mai,
                            Jun = temp.Jun.Equals(DBNull.Value) ? 0 : (decimal)temp.Jun,
                            Jul = temp.Jul.Equals(DBNull.Value) ? 0 : (decimal)temp.Jul,
                            Ago = temp.Ago.Equals(DBNull.Value) ? 0 : (decimal)temp.Ago,
                            Set = temp.Set.Equals(DBNull.Value) ? 0 : (decimal)temp.Set,
                            Out = temp.Out.Equals(DBNull.Value) ? 0 : (decimal)temp.Out,
                            Nov = temp.Nov.Equals(DBNull.Value) ? 0 : (decimal)temp.Nov,
                            Dez = temp.Dez.Equals(DBNull.Value) ? 0 : (decimal)temp.Dez
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

        public static List<DDMessage> GetAnos()
        {
            try
            {
                List<DDMessage> result = new List<DDMessage>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@Ano", 2020),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017MapaVendasAnuais_Anos @Ano", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new DDMessage()
                        {
                            id = (int)temp.ID,
                            value = (string)temp.Value
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

        public class DDMessage
        {
            public int id { get; set; }
            public string value { get; set; }
        }
    }
}
