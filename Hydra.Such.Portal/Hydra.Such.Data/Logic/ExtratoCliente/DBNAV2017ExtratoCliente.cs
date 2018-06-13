using AutoMapper;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Clients;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic.ExtratoCliente
{
    public class DBNAV2017ExtratoCliente
    {
        public static List<ClientExtractViewModel> GetByCustomerNo(string CustomerNo, string Data, int Movimentos)
        {
            using (var ctx = new SuchDBContextExtention())
            {
                var parameters = new[]{
                    new SqlParameter("@Cliente", CustomerNo),
                    new SqlParameter("@Data", Data),
                    new SqlParameter("@Movimentos", Movimentos)
                };

                var resultList = new List<ClientExtractViewModel>();

                try
                {
                    var data = ctx.execStoredProcedure("exec dw_MovClientesIntegrado @Cliente, @Data, @Movimentos", parameters);

                    foreach (var item in data)
                    {
                        resultList.Add(new ClientExtractViewModel {
                            Customer_No = (string)item.Customer_No,
                            Date = (DateTime)item.Date,
                            Due_Date = (DateTime)item.Due_Date,
                            Document_Type = (string)item.Document_Type,
                            Document_No = (string)item.Document_No,
                            Global_Dimension_2_Code = (string)item.Global_Dimension_2_Code,
                            Value = (Decimal)item.Value,
                            Factoring_Sem_Recurso = (string)item.Factoring_Sem_Recurso
                        });
                    }

                    return resultList;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

    }
}
