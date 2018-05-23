﻿using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBNAV2017Projects
    {
        public static List<NAVProjectsViewModel> GetAll(string NAVDatabaseName, string NAVCompanyName, string ProjectNo)
        {
            try
            {
                List<NAVProjectsViewModel> result = new List<NAVProjectsViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@ProjectNo", ProjectNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Projetos @DBName, @CompanyName, @ProjectNo", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVProjectsViewModel()
                        {
                            No = (string)temp.No_,
                            Description = (string)temp.Description,
                            CustomerNo = (string)temp.BillToCustomerNo_,
                            GlobalDimension1 = (string)temp.GlobalDimension1Code,
                            GlobalDimension2 = (string)temp.GlobalDimension2Code,
                            AreaCode = (string)temp.AreaCode,
                            RegionCode = (string)temp.RegionCode,
                            CenterResponsibilityCode = (string)temp.CenterResponsibilityCode
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
