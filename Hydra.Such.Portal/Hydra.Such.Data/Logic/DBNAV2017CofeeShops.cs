using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Nutrition;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017CoffeeShops
    {
        public static decimal GetTotalMeals(string NAVDatabaseName, string NAVCompanyName, string projectNo)
        {
            decimal? totalMeals = null;
            try
            {   
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@JobNo", projectNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017CafetariasTotalRefeicoes @DBName, @CompanyName, @JobNo", parameters);

                    var item = data.FirstOrDefault();
                    if (item != null)
                    {
                        if(item.MealsTotal is decimal)
                            totalMeals = item.MealsTotal;
                    }
                }
            }
            catch
            {

            }
            return totalMeals.HasValue ? totalMeals.Value : 0;
        }
    }
}
