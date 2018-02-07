using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.ProjectView;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Project
{
    public class DBNAV2017DimensionValues
    {
        public static List<NAVDimValueViewModel> GetByDimType(string NAVDatabaseName, string NAVCompanyName, int NAVDimType)
        {
            try
            {
                List<NAVDimValueViewModel> result = new List<NAVDimValueViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@TipoDim", NAVDimType),
                        new SqlParameter("@RespCenter", "")
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017ValoresDimensao @DBName, @CompanyName, @TipoDim, @RespCenter", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVDimValueViewModel()
                        {
                            Code = (string)temp.Code,
                            Name = (string)temp.Name
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

        public static List<NAVDimValueViewModel> GetByDimTypeAndUserId(string NAVDatabaseName, string NAVCompanyName, int NAVDimType, string UserId)
        {
            try
            {
                List<NAVDimValueViewModel> result = new List<NAVDimValueViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@TipoDim", NAVDimType),
                        new SqlParameter("@RespCenter", "")
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017ValoresDimensao @DBName, @CompanyName, @TipoDim, @RespCenter", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVDimValueViewModel()
                        {
                            Code = (string)temp.Code,
                            Name = (string)temp.Name,
                            DimValueID = (int)temp.DimValueID
                        });
                    }


                    if (result.Count > 0)
                    {
                        var userDimensions = DBUserDimensions.GetByUserId(UserId);
                        if (userDimensions != null)
                        {
                            List<UserDimensionsViewModel> userDimensionsViewModel = userDimensions.ParseToViewModel();
                            userDimensionsViewModel.RemoveAll(x => x.Dimension != NAVDimType);
                            if (userDimensionsViewModel.Count > 0)
                            {
                                result.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.Code));
                            }
                        }
                    }
                }

                
                return result;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<NAVDimValueViewModel> GetById(string NAVDatabaseName, string NAVCompanyName, int NAVDimType, string UserId, string RespCenter)
        {
            try
            {
                List<NAVDimValueViewModel> result = new List<NAVDimValueViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@TipoDim", NAVDimType),
                        new SqlParameter("@RespCenter", RespCenter)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017ValoresDimensao @DBName, @CompanyName, @TipoDim, @RespCenter", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVDimValueViewModel()
                        {
                            Code = (string)temp.Code,
                            Name = (string)temp.Name,
                            DimValueID = (int)temp.DimValueID
                        });
                    }


                    if (result.Count > 0)
                    {
                        var userDimensions = DBUserDimensions.GetByUserId(UserId);
                        if (userDimensions != null)
                        {
                            List<UserDimensionsViewModel> userDimensionsViewModel = userDimensions.ParseToViewModel();
                            userDimensionsViewModel.RemoveAll(x => x.Dimension != NAVDimType);
                            if (userDimensionsViewModel.Count > 0)
                            {
                                result.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.Code));
                            }
                        }
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

