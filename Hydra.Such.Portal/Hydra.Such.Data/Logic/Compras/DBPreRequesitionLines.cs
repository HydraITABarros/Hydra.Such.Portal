using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Compras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.ComprasML
{
    public class DBPreRequesitionLines
    {
        #region CRUD
        public static LinhasPréRequisição GetById(string PreRequisicionNo, int LineNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasPréRequisição.Where(x => x.NºPréRequisição == PreRequisicionNo && x.NºLinha == LineNo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasPréRequisição> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasPréRequisição.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasPréRequisição> GetAllByNo(string PreRequisicionNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasPréRequisição.Where(x => x.NºPréRequisição == PreRequisicionNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static LinhasPréRequisição Create(LinhasPréRequisição ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.LinhasPréRequisição.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool CreateMultiple(List<LinhasPréRequisição> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(x => x.DataHoraCriação = DateTime.Now);
                    ctx.LinhasPréRequisição.AddRange(items);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static LinhasPréRequisição Update(LinhasPréRequisição ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.LinhasPréRequisição.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(LinhasPréRequisição ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasPréRequisição.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        #endregion

        public static bool DeleteAllFromPreReqNo(string PreRequisicionNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasPréRequisição.RemoveRange(ctx.LinhasPréRequisição.Where(x => x.NºPréRequisição == PreRequisicionNo).ToList());
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static PreRequisitionLineViewModel ParseToViewModel(LinhasPréRequisição x)
        {
            return new PreRequisitionLineViewModel()
            {
                PreRequisitionLineNo = x.NºPréRequisição,
                LineNo = x.NºLinha,
                Type = x.Tipo,
                Code = x.Código,
                Description = x.Descrição,
                Description2 = x.Descrição2,
                LocalCode = x.CódigoLocalização,
                UnitMeasureCode = x.CódigoUnidadeMedida,
                QuantityToRequire = x.QuantidadeARequerer,
                RegionCode = x.CódigoRegião,
                FunctionalAreaCode = x.CódigoÁreaFuncional,
                CenterResponsibilityCode = x.CódigoCentroResponsabilidade,
                ProjectNo = x.NºProjeto,
                CreateDateTime = x.DataHoraCriação.HasValue ? x.DataHoraCriação.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                CreateUser = x.UtilizadorCriação,
                UpdateDateTime = x.DataHoraModificação.HasValue ? x.DataHoraModificação.Value.ToString("yyyy-MM-dd") : "",
                UpdateUser = x.UtilizadorModificação,
                QtyByUnitOfMeasure = x.QtdPorUnidadeMedida,
                QuantityRequired = x.QuantidadeRequerida,
                QuantityPending = x.QuantidadePendente,
                UnitCost = x.CustoUnitário,
                SellUnityPrice = x.PreçoUnitárioVenda,
                BudgetValue = x.ValorOrçamento,
                ExpectedReceivingDate = x.DataReceçãoEsperada.HasValue ? x.DataReceçãoEsperada.Value.ToString("yyyy-MM-dd") : "",
                Billable = x.Faturável,
                MaintenanceOrderLineNo = x.NºLinhaOrdemManutenção,
                EmployeeNo = x.NºFuncionário,
                Vehicle = x.Viatura,
                SupplierNo = x.NºFornecedor,
                SupplierProductCode = x.CódigoProdutoFornecedor,
                UnitNutritionProduction = x.UnidadeProdutivaNutrição,
                CustomerNo = x.NºCliente,
                OpenOrderNo = x.NºEncomendaAberto,
                OpenOrderLineNo = x.NºLinhaEncomendaAberto,
                Selected = false,
                TotalCost = x.CustoUnitário * x.QuantidadeARequerer,
                ArmazemCDireta = x.LocalCompraDireta

            };
        }

        public static LinhasPréRequisição ParseToDB(PreRequisitionLineViewModel x)
        {
            return new LinhasPréRequisição()
            {
                NºPréRequisição = x.PreRequisitionLineNo,
                NºLinha = x.LineNo,
                Tipo = x.Type,
                Código = x.Code,
                Descrição = x.Description,
                Descrição2 = x.Description2,
                CódigoLocalização = x.LocalCode,
                CódigoUnidadeMedida = x.UnitMeasureCode,
                QuantidadeARequerer = x.QuantityToRequire,
                CódigoRegião = x.RegionCode,
                CódigoÁreaFuncional = x.FunctionalAreaCode,
                CódigoCentroResponsabilidade = x.CenterResponsibilityCode,
                NºProjeto = x.ProjectNo,
                DataHoraCriação = x.CreateDateTime != null && x.CreateDateTime != "" ? DateTime.Parse(x.CreateDateTime) : (DateTime?)null,
                UtilizadorCriação = x.CreateUser,
                DataHoraModificação = x.UpdateDateTime != null && x.UpdateDateTime != "" ? DateTime.Parse(x.UpdateDateTime) : (DateTime?)null,
                UtilizadorModificação = x.UpdateUser,
                QtdPorUnidadeMedida = x.QtyByUnitOfMeasure,
                QuantidadeRequerida = x.QuantityRequired,
                QuantidadePendente = x.QuantityPending,
                CustoUnitário = x.UnitCost,
                PreçoUnitárioVenda = x.SellUnityPrice,
                ValorOrçamento = x.BudgetValue,
                DataReceçãoEsperada = x.ExpectedReceivingDate != null && x.ExpectedReceivingDate != "" ? DateTime.Parse(x.ExpectedReceivingDate) : (DateTime?)null,
                Faturável = x.Billable,
                NºLinhaOrdemManutenção = x.MaintenanceOrderLineNo,
                NºFuncionário = x.EmployeeNo,
                Viatura = x.Vehicle,
                NºFornecedor = x.SupplierNo,
                CódigoProdutoFornecedor = x.SupplierProductCode,
                UnidadeProdutivaNutrição = x.UnitNutritionProduction,
                NºCliente = x.CustomerNo,
                NºEncomendaAberto = x.OpenOrderNo,
                NºLinhaEncomendaAberto = x.OpenOrderLineNo,
                LocalCompraDireta = x.ArmazemCDireta
                
            };
        }
    }
}
