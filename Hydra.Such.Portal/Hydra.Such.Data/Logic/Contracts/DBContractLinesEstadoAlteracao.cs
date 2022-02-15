using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Contracts
{
    public static class DBContractLinesEstadoAlteracao
    {
        #region CRUD
        public static LinhasContratosEstadoAlteracao GetById(int contractType, string contractNo, int VersionNo, int LineNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasContratosEstadoAlteracao.Where(x => x.TipoContrato == contractType && x.NºContrato == contractNo && x.NºVersão == VersionNo && x.NºLinha == LineNo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static LinhasContratosEstadoAlteracao GetByLineNoId(int LineNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasContratosEstadoAlteracao.Where(x => x.NºLinha == LineNo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasContratosEstadoAlteracao> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasContratosEstadoAlteracao.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasContratosEstadoAlteracao> GetAllByNoTypeVersion(string contractNo, int type, int version, bool billable)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasContratosEstadoAlteracao.Where(x => x.NºContrato == contractNo && x.TipoContrato == type && x.NºVersão == version && x.Faturável == billable).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<LinhasContratosEstadoAlteracao> GetAllByNoTypeVersion(string contractNo, int type, int version)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasContratosEstadoAlteracao.Where(x => x.NºContrato == contractNo && x.TipoContrato == type && x.NºVersão == version).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static LinhasContratosEstadoAlteracao Create(LinhasContratosEstadoAlteracao ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //ObjectToCreate.CódServiçoCliente = ObjectToCreate.CódServiçoCliente == "" || ObjectToCreate.CódServiçoCliente == "0" ? null : ObjectToCreate.CódServiçoCliente;
                    //ObjectToCreate.DataHoraCriação = DateTime.Now;
                    //ObjectToCreate.NºLinha = 0;
                    
                    ctx.SaveChanges();

                    LinhasContratosEstadoAlteracao l = new LinhasContratosEstadoAlteracao();

                    l.NºContrato = "TESTE";
                    l.NºVersão = 1;
                    l.NºLinha = 1;
                    l.TipoContrato = 1;
                    ctx.LinhasContratosEstadoAlteracao.Add(l);

                    //ctx.LinhasContratosEstadoAlteracao.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasContratosEstadoAlteracao> Create(List<LinhasContratosEstadoAlteracao> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(item =>
                    {
                        item.CódServiçoCliente = item.CódServiçoCliente == "" || item.CódServiçoCliente == "0" ? null : item.CódServiçoCliente;
                        item.DataHoraCriação = DateTime.Now;
                        item.NºLinha = 0;
                        ctx.LinhasContratosEstadoAlteracao.Add(item);
                    });
                    ctx.SaveChanges();
                }
                return items;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static List<LinhasContratosEstadoAlteracao> GetbyContractId(string ContractId, string ResourceCod)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasContratosEstadoAlteracao.Where(x => x.NºContrato == ContractId && x.Código == ResourceCod).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static LinhasContratosEstadoAlteracao Update(LinhasContratosEstadoAlteracao ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.CódServiçoCliente = ObjectToUpdate.CódServiçoCliente == "" || ObjectToUpdate.CódServiçoCliente == "0" ? null : ObjectToUpdate.CódServiçoCliente;
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.LinhasContratosEstadoAlteracao.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(LinhasContratosEstadoAlteracao ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasContratosEstadoAlteracao.Remove(ObjectToDelete);
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

        public static List<LinhasContratosEstadoAlteracao> GetAllByActiveContract(string contractNo, int versionNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasContratosEstadoAlteracao.Where(x => x.NºContrato == contractNo && x.NºVersão == versionNo).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasContratosEstadoAlteracao> GetAllByContractAndVersionAndGroup(string contractNo, int versionNo, int Group)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasContratosEstadoAlteracao.Where(x => x.NºContrato == contractNo && x.NºVersão == versionNo && x.GrupoFatura == Group).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasContratosEstadoAlteracao> GetAllByContractLinesTypeAndType(ContractType contractType, int Type)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {

                    return ctx.LinhasContratosEstadoAlteracao.Where(x => x.TipoContrato == (int)contractType && x.Tipo == Type).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasContratosEstadoAlteracao> GetAllBySClient(string contractNo, int versionNo, string SClient)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasContratosEstadoAlteracao.Where(x => x.NºContrato == contractNo && x.NºVersão == versionNo && x.CódServiçoCliente == SClient).ToList();
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
                    ctx.LinhasContratosEstadoAlteracao.RemoveRange(ctx.LinhasContratosEstadoAlteracao.Where(x => x.NºContrato == contractNo).ToList());
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static ContractLineEstadoAlteracaoViewModel ParseToViewModel(LinhasContratosEstadoAlteracao x)
        {

            return new ContractLineEstadoAlteracaoViewModel()
            {
                ContractType = x.TipoContrato,
                ContractNo = x.NºContrato,
                VersionNo = x.NºVersão,
                LineNo = x.NºLinha,
                Type = x.Tipo,
                Code = x.Código,
                Description = x.Descrição,
                Description2 = x.Descricao2,
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
                ServiceClientNo = x.CódServiçoCliente == "" || x.CódServiçoCliente == "0" ? null : x.CódServiçoCliente,
                InvoiceGroup = x.GrupoFatura == null ? 0 : x.GrupoFatura,
                ProjectNo = x.NºProjeto,
                CreateContract = x.CriaContrato,
                CreateDate = x.DataHoraCriação,
                CreateUser = x.UtilizadorCriação,
                UpdateDate = x.DataHoraModificação,
                UpdateUser = x.UtilizadorModificação,
                Selected = false
            };
        }

        public static List<ContractLineEstadoAlteracaoViewModel> ParseToViewModel(List<LinhasContratosEstadoAlteracao> items)
        {
            List<ContractLineEstadoAlteracaoViewModel> parsedItems = new List<ContractLineEstadoAlteracaoViewModel>();
            if (items != null && items.Count > 0)
            {
                items.ForEach(x =>
                    parsedItems.Add(ParseToViewModel(x))
                );
            }
            return parsedItems;
        }

        public static LinhasContratosEstadoAlteracao ParseToDB(ContractLineEstadoAlteracaoViewModel x)
        {
            return new LinhasContratosEstadoAlteracao()
            {
                TipoContrato = x.ContractType,
                NºContrato = x.ContractNo,
                NºVersão = x.VersionNo,
                NºLinha = x.LineNo,
                Tipo = x.Type,
                Código = x.Code,
                Descrição = x.Description,
                Descricao2 = x.Description2,
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
                CódServiçoCliente = x.ServiceClientNo == "" || x.ServiceClientNo == "0" ? null : x.ServiceClientNo,
                GrupoFatura = x.InvoiceGroup == null ? 0 : x.InvoiceGroup,
                CriaContrato = x.CreateContract,
                NºProjeto = x.ProjectNo,
                DataHoraCriação = x.CreateDate,
                UtilizadorCriação = x.CreateUser,
                DataHoraModificação = x.UpdateDate,
                UtilizadorModificação = x.UpdateUser
            };
        }

        public static LinhasContratosEstadoAlteracao ParseToDB(LinhasContratos x)
        {
            return new LinhasContratosEstadoAlteracao()
            {
                TipoContrato = x.TipoContrato,
                NºContrato = x.NºContrato,
                NºVersão = x.NºVersão,
                NºLinha = x.NºLinha,
                Tipo = x.Tipo,
                Código = x.Código,
                Descrição = x.Descrição,
                Descricao2 = x.Descricao2,
                Quantidade = x.Quantidade,
                CódUnidadeMedida = x.CódUnidadeMedida,
                PreçoUnitário = x.PreçoUnitário,
                DescontoLinha = x.DescontoLinha,
                Faturável = x.Faturável,
                CódigoRegião = x.CódigoRegião,
                CódigoÁreaFuncional = x.CódigoÁreaFuncional,
                CódigoCentroResponsabilidade = x.CódigoCentroResponsabilidade,
                Periodicidade = x.Periodicidade,
                NºHorasIntervenção = x.NºHorasIntervenção,
                NºTécnicos = x.NºTécnicos,
                TipoProposta = x.TipoProposta,
                DataInícioVersão = x.DataInícioVersão,
                DataFimVersão = x.DataFimVersão,
                NºResponsável = x.NºResponsável,
                CódServiçoCliente = x.CódServiçoCliente,
                GrupoFatura = x.GrupoFatura,
                CriaContrato = x.CriaContrato,
                DataHoraCriação = x.DataHoraCriação,
                UtilizadorCriação = x.UtilizadorCriação,
                DataHoraModificação = x.DataHoraModificação,
                UtilizadorModificação = x.UtilizadorModificação,
                NºProjeto = x.NºProjeto
            };
        }


    }
}
