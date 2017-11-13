using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Contracts
{
    public static class DBContractLines
    {
        #region CRUD
        public static LinhasContratos GetById(int contractType, string contractNo, int VersionNo, int LineNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasContratos.Where(x => x.TipoContrato == contractType && x.NºContrato == contractNo && x.NºVersão == VersionNo && x.NºLinha == LineNo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasContratos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasContratos.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasContratos> GetAllByNoTypeVersion(string contractNo, int type, int version, bool billable)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasContratos.Where(x => x.NºContrato == contractNo && x.TipoContrato == type && x.NºVersão == version && x.Faturável == billable).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static LinhasContratos Create(LinhasContratos ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.LinhasContratos.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static LinhasContratos Update(LinhasContratos ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.LinhasContratos.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(LinhasContratos ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasContratos.Remove(ObjectToDelete);
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





        public static List<LinhasContratos> GetAllByActiveContract(string contractNo, int versionNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasContratos.Where(x => x.NºContrato == contractNo && x.NºVersão == versionNo).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        public static bool DeleteAllFromContract(string contractNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasContratos.RemoveRange(ctx.LinhasContratos.Where(x => x.NºContrato == contractNo).ToList());
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static ContractLineViewModel ParseToViewModel(LinhasContratos x)
        {

            return new ContractLineViewModel() {
                ContractType = x.TipoContrato,
                ContractNo = x.NºContrato,
                VersionNo = x.NºVersão,
                LineNo = x.NºLinha,
                Type = x.Tipo,
                Code = x.Código,
                Description = x.Descrição,
                Quantity = x.Quantidade,
                CodeMeasureUnit = x.CódUnidadeMedida,
                UnitPrice = x.PreçoUnitário,
                LineDiscount = x.DescontoLinha,
                Billable = x.Faturável,
                CodeRegion = x.CódigoRegião,
                CodeFunctionalArea = x.CódigoÁreaFuncional,
                CodeResponsabilityCenter = x.CódigoCentroResponsabilidade,
                Frequency = x.Periodicidade,
                InterventionHours = x.NºHorasIntervenção,
                TotalTechinicians = x.NºTécnicos,
                ProposalType = x.TipoProposta,
                VersionStartDate = x.DataInícioVersão.HasValue ? x.DataInícioVersão.Value.ToString("yyyy-MM-dd") : "",
                VersionEndDate = x.DataFimVersão.HasValue ? x.DataFimVersão.Value.ToString("yyyy-MM-dd") : "",
                ResponsibleNo = x.NºResponsável,
                ServiceClientNo = x.CódServiçoCliente,
                InvoiceGroup = x.GrupoFatura,
                CreateContract = x.CriaContrato,
                CreateDate = x.DataHoraCriação,
                CreateUser = x.UtilizadorCriação,
                UpdateDate = x.DataHoraModificação,
                UpdateUser = x.UtilizadorModificação,
                Selected = false
            };
        }

        public static LinhasContratos ParseToDB(ContractLineViewModel x)
        {
            return new LinhasContratos()
            {
                TipoContrato = x.ContractType,
                NºContrato = x.ContractNo,
                NºVersão = x.VersionNo,
                NºLinha = x.LineNo,
                Tipo = x.Type,
                Código = x.Code,
                Descrição = x.Description,
                Quantidade = x.Quantity,
                CódUnidadeMedida = x.CodeMeasureUnit,
                PreçoUnitário = x.UnitPrice,
                DescontoLinha = x.LineDiscount,
                Faturável = x.Billable,
                CódigoRegião = x.CodeRegion,
                CódigoÁreaFuncional = x.CodeFunctionalArea,
                CódigoCentroResponsabilidade = x.CodeResponsabilityCenter,
                Periodicidade = x.Frequency,
                NºHorasIntervenção = x.InterventionHours,
                NºTécnicos = x.TotalTechinicians,
                TipoProposta = x.ProposalType,
                DataInícioVersão = x.VersionStartDate != null && x.VersionStartDate != "" ? DateTime.Parse(x.VersionStartDate) : (DateTime?)null,
                DataFimVersão = x.VersionEndDate != null && x.VersionEndDate != "" ? DateTime.Parse(x.VersionEndDate) : (DateTime?)null,
                NºResponsável = x.ResponsibleNo,
                CódServiçoCliente = x.ServiceClientNo,
                GrupoFatura = x.InvoiceGroup,
                CriaContrato = x.CreateContract,
                DataHoraCriação = x.CreateDate,
                UtilizadorCriação = x.CreateUser,
                DataHoraModificação = x.UpdateDate,
                UtilizadorModificação = x.UpdateUser
            };
        }
    }
}
