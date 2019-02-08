using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using static Hydra.Such.Data.Enumerations;
using Hydra.Such.Data.ViewModel.Encomendas;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017Encomendas
    {

        public static List<EncomendasViewModel> ListByDimListAndNoFilter(string NAVDatabaseName, string NAVCompanyName, List<AcessosDimensões> Dimensions, string No_FilterExpression, string from = null, string to = null, string fornecedor = null)
        {
            try
            {
                List<EncomendasViewModel> result = new List<EncomendasViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {

                    var regions = Dimensions.Where(d => d.Dimensão == (int)Enumerations.Dimensions.Region).Select(s => s.ValorDimensão);
                    var functionalAreas = Dimensions.Where(d => d.Dimensão == (int)Enumerations.Dimensions.FunctionalArea).Select(s => s.ValorDimensão);
                    var responsabilityCenters = Dimensions.Where(d => d.Dimensão == (int)Enumerations.Dimensions.ResponsabilityCenter).Select(s => s.ValorDimensão);

                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@Regions", regions != null && regions.Count() > 0 ? string.Join(',', regions) : null),
                        new SqlParameter("@FunctionalAreas",functionalAreas != null && functionalAreas.Count() > 0 ?  string.Join(',', functionalAreas): null),
                        new SqlParameter("@RespCenters", responsabilityCenters != null && responsabilityCenters.Count() > 0 ? '\'' + string.Join("',\'",responsabilityCenters) + '\'': null),
                        new SqlParameter("@CodFornecedor", fornecedor),
                        new SqlParameter("@From", from),
                        new SqlParameter("@To", to),
                        new SqlParameter("@No_FilterExpression", No_FilterExpression )
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017EncomendasList @DBName, @CompanyName, @Regions, @FunctionalAreas, @RespCenters, @CodFornecedor, @From, @To, @No_FilterExpression", parameters);

                    foreach (dynamic temp in data)
                    {
                        if (!temp.NoFirstTwoInt.Equals(DBNull.Value) && ((int)temp.NoFirstTwoInt >= 18))
                        {
                            DateTime? ExpectedReceiptDate = (DateTime)temp.ExpectedReceiptDate;
                            var minDate = new DateTime(2008, 1, 1);
        
                            result.Add(new EncomendasViewModel()
                            {
                                No = temp.No.Equals(DBNull.Value) ? "" : (string)temp.No,
                                PayToVendorNo = temp.PayToVendorNo.Equals(DBNull.Value) ? "" : (string)temp.PayToVendorNo,
                                PayToName = temp.PayToName.Equals(DBNull.Value) ? "" : (string)temp.PayToName,
                                YourReference = temp.YourReference.Equals(DBNull.Value) ? "" : (string)temp.YourReference,
                                OrderDate = (DateTime)temp.OrderDate,
                                NoConsulta = temp.NConsulta.Equals(DBNull.Value) ? "" : (string)temp.NConsulta,
                                ExpectedReceiptDate = ExpectedReceiptDate != null && ExpectedReceiptDate > minDate ? ExpectedReceiptDate : null,
                                RequisitionNo = temp.RequisitionNo.Equals(DBNull.Value) ? "" : (string)temp.RequisitionNo,
                                RegionId = temp.RegionId.Equals(DBNull.Value) ? "" : (string)temp.RegionId,
                                FunctionalAreaId = temp.FunctionalAreaId.Equals(DBNull.Value) ? "" : (string)temp.FunctionalAreaId,
                                RespCenterId = temp.RespCenterId.Equals(DBNull.Value) ? "" : (string)temp.RespCenterId,
                                HasAnAdvance = (bool)temp.HasAnAdvance,
                                Total = temp.Total.Equals(DBNull.Value) ? 0 : (decimal)temp.Total,
                                VlrRececionadoComIVA = temp.VlrRececionadoComIVA.Equals(DBNull.Value) ? 0 : (decimal)temp.VlrRececionadoComIVA,        
                                VlrRececionadoSemIVA = temp.VlrRececionadoSemIVA.Equals(DBNull.Value) ? 0 : (decimal)temp.VlrRececionadoSemIVA,
                            });
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

        public static EncomendasViewModel GetDetailsByNo(string NAVDatabaseName, string NAVCompanyName, string No, string No_FilterExpression)
        {
            try
            {
                var result = new EncomendasViewModel();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@No", No ),
                        new SqlParameter("@No_FilterExpression", No_FilterExpression )
                    };

                    dynamic data = ctx.execStoredProcedure("exec NAV2017EncomendasDetails @DBName, @CompanyName, @No", parameters).FirstOrDefault();

                    result = new EncomendasViewModel()
                    {
                        No = data.No.Equals(DBNull.Value) ? "" : (string)data.No,
                        PayToVendorNo = data.PayToVendorNo.Equals(DBNull.Value) ? "" : (string)data.PayToVendorNo,
                        PayToName = data.PayToName.Equals(DBNull.Value) ? "" : (string)data.PayToName,
                        YourReference = data.YourReference.Equals(DBNull.Value) ? "" : (string)data.YourReference,
                        OrderDate = (DateTime)data.OrderDate,
                        NoConsulta = data.NConsulta.Equals(DBNull.Value) ? "" : (string)data.NConsulta,
                        ExpectedReceiptDate = (DateTime)data.ExpectedReceiptDate,
                        RequisitionNo = data.RequisitionNo.Equals(DBNull.Value) ? "" : (string)data.RequisitionNo,
                        RegionId = data.RegionId.Equals(DBNull.Value) ? "" : (string)data.RegionId,
                        FunctionalAreaId = data.FunctionalAreaId.Equals(DBNull.Value) ? "" : (string)data.FunctionalAreaId,
                        RespCenterId = data.RespCenterId.Equals(DBNull.Value) ? "" : (string)data.RespCenterId,
                        HasAnAdvance = (bool)data.HasAnAdvance,
                    };

                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<EncomendasLinhasViewModel> ListLinesByNo(string NAVDatabaseName, string NAVCompanyName, string No, string No_FilterExpression)
        {
            try
            {
                List<EncomendasLinhasViewModel> result = new List<EncomendasLinhasViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@No", No ),
                        new SqlParameter("@No_FilterExpression", No_FilterExpression )
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017EncomendasLinhasList @DBName, @CompanyName, @No", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new EncomendasLinhasViewModel()
                        {
                            No = temp.No.Equals(DBNull.Value) ? "" : (string)temp.No,
                            AllocationNo = temp.AllocationNo.Equals(DBNull.Value) ? "" : (string)temp.AllocationNo,
                            LocationCode = temp.LocationCode.Equals(DBNull.Value) ? "" : (string)temp.LocationCode,
                            Amount = (decimal?)temp.Amount,
                            AmountIncludingVAT = (decimal?)temp.AmountIncludingVAT,
                            Description = temp.Description.Equals(DBNull.Value) ? "" : (string)temp.Description,
                            Description2 = temp.Description2.Equals(DBNull.Value) ? "" : (string)temp.Description2,
                            DirectUnitCost = (decimal?)temp.DirectUnitCost,
                            FunctionalAreaId = temp.FunctionalAreaId.Equals(DBNull.Value) ? "" : (string)temp.FunctionalAreaId,
                            JobNo = temp.JobNo.Equals(DBNull.Value) ? "" : (string)temp.JobNo,
                            LineNo = (int?)temp.LineN,
                            Quantity = (decimal?)temp.Quantity,
                            QuantityInvoiced = (decimal?)temp.QuantityInvoiced,
                            QuantityReceived = (decimal?)temp.QuantityReceived,
                            RegionId = temp.RegionId.Equals(DBNull.Value) ? "" : (string)temp.RegionId,
                            RespCenterId = temp.RespCenterId.Equals(DBNull.Value) ? "" : (string)temp.RespCenterId,
                            UnitOfMeasure = temp.UnitOfMeasure.Equals(DBNull.Value) ? "" : (string)temp.UnitOfMeasure,
                            VAT = (decimal)temp.VAT,
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

        public static List<EncomendasViewModel> EncomendasPorRequisicao(string NAVDatabaseName, string NAVCompanyName, string Requisicao, int Tipo)
        {
            try
            {
                List<EncomendasViewModel> result = new List<EncomendasViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@OrderId", Requisicao ),
                        new SqlParameter("@Type", Tipo ),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017EncomendasPorRequisicao @DBName, @CompanyName, @OrderId, @Type", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new EncomendasViewModel()
                        {
                            No = temp.NoEncomenda.Equals(DBNull.Value) ? "" : (string)temp.NoEncomenda,
                            PayToVendorNo = temp.Fornecedor.Equals(DBNull.Value) ? "" : (string)temp.Fornecedor,
                            OrderDate = (DateTime)temp.DataEncomenda,
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
