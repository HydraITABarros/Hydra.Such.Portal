using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.ProjectView;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017Products
    {
        public static List<NAVProductsViewModel> GetAllProducts(string NAVDatabaseName, string NAVCompanyName, string productNo)
        {
            try
            {
                List<NAVProductsViewModel> result = new List<NAVProductsViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoProduto", productNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Produtos @DBName, @CompanyName, @NoProduto", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVProductsViewModel()
                        {
                            Code = (string)temp.No_,
                            Name = (string)temp.Description,
                            MeasureUnit = (string)temp.Base_Unit_of_Measure,
                            ItemCategoryCode = (string)temp.Item_Category_Code,
                            ProductGroupCode = (string)temp.Product_Group_Code,
                            VendorProductNo = (string)temp.Vendor_Item_No_,
                            LastCostDirect = (decimal)temp.Last_Direct_Cost,
                            VendorNo = (string)temp.Vendor_No_

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

        public static List<NAVProductsViewModel> GetProductsForDimensions(string NAVDatabaseName, string NAVCompanyName, string allowedDimensions, string requisitionType)
        {
            try
            {
                List<NAVProductsViewModel> result = new List<NAVProductsViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@AllowedDimensions", allowedDimensions),
                        new SqlParameter("@RequisitionType", requisitionType)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure(@"exec NAV2017ProdutosDaArea @DBName, @CompanyName, @AllowedDimensions, @RequisitionType", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVProductsViewModel()
                        {
                            Code = (string)temp.No_,
                            Name = (string)temp.Description,
                            MeasureUnit = (string)temp.Base_Unit_of_Measure,
                            ItemCategoryCode = (string)temp.Item_Category_Code,
                            ProductGroupCode = (string)temp.Product_Group_Code,
                            VendorProductNo = (string)temp.Vendor_Item_No_,
                            LastCostDirect = (decimal)temp.Last_Direct_Cost,
                            VendorNo = (string)temp.Vendor_No_

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
