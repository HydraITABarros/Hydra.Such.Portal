using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.ProjectView;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic.Project
{
    public class DBNAV2017MeasureUnit
    {
        public static List<NAVMeasureUnitViewModel> GetAllMeasureUnit (string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVMeasureUnitViewModel> result = new List<NAVMeasureUnitViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017UnidadesMedida @DBName, @CompanyName", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVMeasureUnitViewModel()
                        {
                            Code = (string)temp.Code,
                            Description = (string)temp.Description
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
