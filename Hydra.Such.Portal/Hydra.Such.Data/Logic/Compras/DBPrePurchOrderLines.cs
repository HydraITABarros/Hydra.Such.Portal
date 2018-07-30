using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Data.Database;
using System.Linq;

namespace Hydra.Such.Data.Logic.ComprasML
{
    public static class DBPrePurchOrderLines
    {
        #region CRUD

        public static List<LinhasPreEncomenda> GetById(int lineNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasPreEncomenda.Where(x => x.NºLinhaPreEncomenda == lineNo).ToList();
                }
            }
            catch
            {
                return null;
            }
        }
        public static List<LinhasPreEncomenda> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasPreEncomenda.ToList();
                }
            }
            catch
            {
                return null;
            }
        }

        public static LinhasPreEncomenda Create(LinhasPreEncomenda item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraCriação = DateTime.Now;
                    ctx.LinhasPreEncomenda.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch
            {
                return null;
            }
        }

        public static List<LinhasPreEncomenda> Create(List<LinhasPreEncomenda> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(item => item.DataHoraCriação = DateTime.Now);
                    
                    ctx.LinhasPreEncomenda.AddRange(items);
                    ctx.SaveChanges();
                }
                return items;
            }
            catch
            {
                return null;
            }
        }

        public static List<LinhasPreEncomenda> CreateAndUpdateReqLines(List<LinhasPreEncomenda> prePurchOrderLines, List<LinhasRequisição> requisitionLines)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    prePurchOrderLines.ForEach(item => { item.DataHoraCriação = DateTime.Now;
                                                        item.NºEncomendaAberto = requisitionLines.Where(y => y.NºLinha == item.NºLinhaRequisição).FirstOrDefault().NºEncomendaAberto;
                                                        item.NºLinhaEncomendaAberto = requisitionLines.Where(y => y.NºLinha == item.NºLinhaRequisição).FirstOrDefault().NºLinhaEncomendaAberto; });
                    ctx.LinhasPreEncomenda.AddRange(prePurchOrderLines);

                    if (requisitionLines != null)
                    {
                        requisitionLines.ForEach(item => { item.DataHoraModificação = DateTime.Now;
                                                            /*item.EnviadoPréCompra = true;*/ });
                    }
                    ctx.LinhasRequisição.UpdateRange(requisitionLines);

                    ctx.SaveChanges();
                }
                return prePurchOrderLines;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static LinhasPreEncomenda Update(LinhasPreEncomenda item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraModificação = DateTime.Now;
                    ctx.LinhasPreEncomenda.Update(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch
            {
                return null;
            }
        }

        public static bool Update(List<LinhasPreEncomenda> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    if (items != null)
                    {
                        items.ForEach(item => item.DataHoraModificação = DateTime.Now);
                    }
                    ctx.LinhasPreEncomenda.UpdateRange(items);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool Delete(LinhasPreEncomenda item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasPreEncomenda.Remove(item);
                    ctx.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Parse Utilities
        public static PrePurchOrderLineViewModel ParseToViewModel(this LinhasPreEncomenda item)
        {
            if (item != null)
            {
                return new PrePurchOrderLineViewModel()
                {
                    PrePurchOrderNo = item.NºPreEncomenda,
                    PrePurchOrderLineNo = item.NºLinhaPreEncomenda,
                    RequisitionNo = item.NºRequisição,
                    RequisitionLineNo = item.NºLinhaRequisição,
                    ProductCode = item.CódigoProduto,
                    ProductDescription = item.DescriçãoProduto,
                    UnitOfMeasureCode = item.CódigoUnidadeMedida,
                    LocationCode = item.CódigoLocalização,
                    QuantityAvailable = item.QuantidadeDisponibilizada,
                    UnitCost = item.CustoUnitário,
                    ProjectNo = item.NºProjeto,
                    RegionCode = item.CódigoRegião,
                    FunctionalAreaCode = item.CódigoÁreaFuncional,
                    CenterResponsibilityCode = item.CódigoCentroResponsabilidade,
                    CreateDateTime = item.DataHoraCriação,
                    CreateUser = item.UtilizadorCriação,
                    UpdateDateTime = item.DataHoraModificação,
                    UpdateUser = item.UtilizadorModificação,
                    SupplierNo = item.NºFornecedor,
                };
            }
            return null;
        }

        public static List<PrePurchOrderLineViewModel> ParseToViewModel(this List<LinhasPreEncomenda> items)
        {
            List<PrePurchOrderLineViewModel> parsedItems = new List<PrePurchOrderLineViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }

        public static LinhasPreEncomenda ParseToDB(this PrePurchOrderLineViewModel item)
        {
            if (item != null)
            {
                return new LinhasPreEncomenda()
                {
                    NºPreEncomenda = item.PrePurchOrderNo,
                    NºLinhaPreEncomenda = item.PrePurchOrderLineNo,
                    NºRequisição = item.RequisitionNo,
                    NºLinhaRequisição = item.RequisitionLineNo,
                    CódigoProduto = item.ProductCode,
                    DescriçãoProduto = item.ProductDescription,
                    CódigoUnidadeMedida = item.UnitOfMeasureCode,
                    CódigoLocalização = item.LocationCode,
                    QuantidadeDisponibilizada = item.QuantityAvailable,
                    CustoUnitário = item.UnitCost,
                    NºProjeto = item.ProjectNo,
                    CódigoRegião = item.RegionCode,
                    CódigoÁreaFuncional = item.FunctionalAreaCode,
                    CódigoCentroResponsabilidade = item.CenterResponsibilityCode,
                    DataHoraCriação = item.CreateDateTime,
                    UtilizadorCriação = item.CreateUser,
                    DataHoraModificação = item.UpdateDateTime,
                    UtilizadorModificação = item.UpdateUser,
                    NºFornecedor = item.SupplierNo,
                };
            }
            return null;
        }

        public static List<LinhasPreEncomenda> ParseToDB(this List<PrePurchOrderLineViewModel> items)
        {
            List<LinhasPreEncomenda> parsedItems = new List<LinhasPreEncomenda>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
        #endregion
    }
}
