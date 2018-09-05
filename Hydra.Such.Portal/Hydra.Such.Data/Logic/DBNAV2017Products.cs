using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Data.ViewModel.ProjectView;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
                            VendorNo = (string)temp.Vendor_No_,
                            VATProductPostingGroup = (string)temp.VATProductPostingGroup,
                            UnitCost = (decimal)temp.UnitCost
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

        public static List<NAVProductsViewModel> GetAllProductsCompras(string NAVDatabaseName, string NAVCompanyName, string productNo)
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

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017ProdutosCompras @DBName, @CompanyName, @NoProduto", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVProductsViewModel()
                        {
                            Code = (string)temp.No_,
                            Name = (string)temp.Description,
                            Name2 = (string)temp.Description2,
                            MeasureUnit = (string)temp.Base_Unit_of_Measure,
                            ItemCategoryCode = (string)temp.Item_Category_Code,
                            ProductGroupCode = (string)temp.Product_Group_Code,
                            VendorProductNo = (string)temp.Vendor_Item_No_,
                            LastCostDirect = (decimal)temp.Last_Direct_Cost,
                            VendorNo = (string)temp.Vendor_No_,
                            VATProductPostingGroup = (string)temp.VATProductPostingGroup,
                            UnitCost = (decimal)temp.UnitCost
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

        public static List<NAVProductsViewModel> GetProductsById(string navDatabaseName, string navCompanyName, List<string> productsId)
        {
            string productsIds = string.Join(",", productsId);
            return GetAllProducts(navDatabaseName, navCompanyName, productsIds);
        }
        public static List<NAVProductsViewModel> GetProductsForDimensions(string NAVDatabaseName, string NAVCompanyName, string allowedDimensions, string requisitionType, string locationCode)
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
                        new SqlParameter("@RequisitionType", requisitionType),
                        new SqlParameter("@LocationCode", locationCode)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure(@"exec NAV2017ProdutosDaArea @DBName, @CompanyName, @AllowedDimensions, @RequisitionType, @LocationCode", parameters);

                    foreach (dynamic temp in data)
                    {
                        var item = new NAVProductsViewModel();
                        item.Code = (string)temp.No_;
                        item.Name = (string)temp.Description;
                        item.Name2 = (string)temp.Description2;
                        item.MeasureUnit = (string)temp.Base_Unit_of_Measure;
                        item.ItemCategoryCode = (string)temp.Item_Category_Code;
                        item.ProductGroupCode = (string)temp.Product_Group_Code;
                        item.VendorProductNo = (string)temp.Vendor_Item_No_;
                        item.LastCostDirect = (decimal)temp.Last_Direct_Cost;
                        item.VendorNo = (string)temp.Vendor_No_;
                        item.UnitCost = (decimal)temp.UnitCost;
                        item.LocationCode = temp.Location_Code.Equals(DBNull.Value) ? "" : (string)temp.Location_Code;
                        item.VATProductPostingGroup = (string)temp.VATProductPostingGroup;

                        result.Add(item);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<NAVProductsViewModel> GetProductsForPreRequisitions(string NAVDatabaseName, string NAVCompanyName, string allowedDimensions, string requisitionType, string locationCode)
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
                        new SqlParameter("@RequisitionType", requisitionType),
                        new SqlParameter("@LocationCode", locationCode)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure(@"exec NAV2017ProdutosPreRequisicao @DBName, @CompanyName, @AllowedDimensions, @RequisitionType, @LocationCode", parameters);

                    foreach (dynamic temp in data)
                    {
                        var item = new NAVProductsViewModel();
                        item.Code = (string)temp.No_;
                        item.Name = (string)temp.Description;
                        item.Name2 = (string)temp.Description2;
                        item.MeasureUnit = (string)temp.Base_Unit_of_Measure;
                        item.ItemCategoryCode = (string)temp.Item_Category_Code;
                        item.ProductGroupCode = (string)temp.Product_Group_Code;
                        item.VendorProductNo = (string)temp.Vendor_Item_No_;
                        item.LastCostDirect = temp.Last_Direct_Cost.Equals(DBNull.Value) ? 0 : (decimal)temp.Last_Direct_Cost;
                        item.VendorNo = (string)temp.Vendor_No_;
                        item.UnitCost = temp.UnitCost.Equals(DBNull.Value) ? 0 : (decimal)temp.UnitCost;
                        item.LocationCode = temp.Location_Code.Equals(DBNull.Value) ? "" : (string)temp.Location_Code;
                        item.VATProductPostingGroup = (string)temp.VATProductPostingGroup;

                        result.Add(item);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Check for blocked or Nonexisting products for requisition
        /// </summary>
        /// <param name="requisition"></param>
        /// <param name="navDatabaseName"></param>
        /// <param name="navCompanyName"></param>
        /// <returns></returns>
        public static ErrorHandler CheckProductsAvailability(RequisitionViewModel requisition, string navDatabaseName, string navCompanyName)
        {
            //garantir que todos os produtos estão desbloqueados
            var productsIds = requisition.Lines.Select(x => x.Code).Distinct().ToList();
            var products = GetProductsById(navDatabaseName, navCompanyName, productsIds);
            ErrorHandler result = new ErrorHandler(1, "Os produtos não estão bloqueados.");
            if (products != null)
            {
                if (products.Count < productsIds.Count)
                {
                    var existingIds = products.Select(x => x.Code).Distinct();
                    var blockedOrUnexisting = requisition.Lines.Where(x => !existingIds.Contains(x.Code)).ToList();

                    result.eReasonCode = 2;
                    result.eMessage = "Os seguintes produtos não existem ou estão bloqueados: " + string.Join(", ", blockedOrUnexisting.Select(x => x.Code + " - " + x.Description).ToArray());
                }
            }
            return result;
        }
    }
}
