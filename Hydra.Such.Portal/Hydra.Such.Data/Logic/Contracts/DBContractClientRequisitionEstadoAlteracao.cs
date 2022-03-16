using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Contracts
{
    public static class DBContractClientRequisitionEstadoAlteracao
    {
        #region CRUD
        public static RequisiçõesClienteContratoEstadoAlteracao GetById(string ContractNo, int InvoiceGroup, string projectNo, DateTime startDate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RequisiçõesClienteContratoEstadoAlteracao.Where(x => x.NºContrato == ContractNo && x.GrupoFatura == InvoiceGroup && x.NºProjeto == projectNo && x.DataInícioCompromisso == startDate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<RequisiçõesClienteContratoEstadoAlteracao> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RequisiçõesClienteContratoEstadoAlteracao.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static RequisiçõesClienteContratoEstadoAlteracao Create(RequisiçõesClienteContratoEstadoAlteracao ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.RequisiçõesClienteContratoEstadoAlteracao.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static RequisiçõesClienteContratoEstadoAlteracao Update(RequisiçõesClienteContratoEstadoAlteracao ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.RequisiçõesClienteContratoEstadoAlteracao.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(RequisiçõesClienteContratoEstadoAlteracao ClientRequisition)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.RequisiçõesClienteContratoEstadoAlteracao.Remove(ClientRequisition);
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

        public static bool DeleteAllFromContract(string ContractNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.RequisiçõesClienteContratoEstadoAlteracao.RemoveRange(ctx.RequisiçõesClienteContratoEstadoAlteracao.Where(x => x.NºContrato == ContractNo));
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static List<RequisiçõesClienteContratoEstadoAlteracao> GetByContract(string ContractNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RequisiçõesClienteContratoEstadoAlteracao.Where(x => x.NºContrato == ContractNo).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static RequisiçõesClienteContratoEstadoAlteracao GetByContractAndGroup(string ContractNo, int? group)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RequisiçõesClienteContratoEstadoAlteracao.Where(x => x.NºContrato == ContractNo && x.GrupoFatura == group).OrderByDescending(x => x.DataInícioCompromisso).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static DateTime? GetLatsInvoiceDateFor(string contractNo, int? invoiceGroupNumber)
        {
            try
            {
                DateTime? lastInvoiceDate = null;
                using (var ctx = new SuchDBContext())
                {
                    var req = ctx.RequisiçõesClienteContratoEstadoAlteracao.Where(x => x.NºContrato == contractNo && x.GrupoFatura == invoiceGroupNumber && x.DataÚltimaFatura.HasValue).OrderByDescending(x => x.DataInícioCompromisso).FirstOrDefault();
                    if (req != null)
                    {
                        if (req.DataÚltimaFatura.HasValue)
                            lastInvoiceDate = req.DataÚltimaFatura.Value;
                        else
                            lastInvoiceDate = req.DataInícioCompromisso;
                    }
                    //}

                }
                return lastInvoiceDate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }



        public static RequisiçõesClienteContratoEstadoAlteracao ParseToDB(ContractClientRequisitionEstadoAlteracaoViewModel ObjectToParse)
        {
            return new RequisiçõesClienteContratoEstadoAlteracao()
            {
                NoLinha = ObjectToParse.NoLinha,
                NºContrato = ObjectToParse.ContractNo,
                GrupoFatura = ObjectToParse.InvoiceGroup,
                NºProjeto = ObjectToParse.ProjectNo,
                DataInícioCompromisso = DateTime.Parse(ObjectToParse.StartDate),
                DataFimCompromisso = ObjectToParse.EndDate != "" ? DateTime.Parse(ObjectToParse.EndDate) : (DateTime?)null,
                NºRequisiçãoCliente = ObjectToParse.ClientRequisitionNo,
                DataRequisição = ObjectToParse.RequisitionDate != "" ? DateTime.Parse(ObjectToParse.RequisitionDate) : (DateTime?)null,
                NºCompromisso = ObjectToParse.PromiseNo,
                DataÚltimaFatura = ObjectToParse.LastInvoiceDate != "" ? DateTime.Parse(ObjectToParse.LastInvoiceDate) : (DateTime?)null,
                NºFatura = ObjectToParse.InvoiceNo,
                ValorFatura = ObjectToParse.InvoiceValue,
                DataHoraCriação = ObjectToParse.CreateDate,
                UtilizadorCriação = ObjectToParse.CreateUser,
                DataHoraModificação = ObjectToParse.UpdateDate,
                UtilizadorModificação = ObjectToParse.UpdateUser
            };
        }

        public static RequisiçõesClienteContratoEstadoAlteracao ParseToDB(RequisiçõesClienteContrato ObjectToParse)
        {
            return new RequisiçõesClienteContratoEstadoAlteracao()
            {
                NoLinha = ObjectToParse.NoLinha,
                NºContrato = ObjectToParse.NºContrato,
                GrupoFatura = ObjectToParse.GrupoFatura,
                NºProjeto = ObjectToParse.NºProjeto,
                DataInícioCompromisso = ObjectToParse.DataInícioCompromisso,
                DataFimCompromisso = ObjectToParse.DataFimCompromisso,
                NºRequisiçãoCliente = ObjectToParse.NºRequisiçãoCliente,
                DataRequisição = ObjectToParse.DataRequisição,
                NºCompromisso = ObjectToParse.NºCompromisso,
                DataÚltimaFatura = ObjectToParse.DataÚltimaFatura,
                NºFatura = ObjectToParse.NºFatura,
                ValorFatura = ObjectToParse.ValorFatura,
                DataHoraCriação = ObjectToParse.DataHoraCriação,
                UtilizadorCriação = ObjectToParse.UtilizadorCriação,
                DataHoraModificação = ObjectToParse.DataHoraModificação,
                UtilizadorModificação = ObjectToParse.UtilizadorModificação
            };
        }

        public static ContractClientRequisitionEstadoAlteracaoViewModel ParseToViewModel(RequisiçõesClienteContratoEstadoAlteracao ObjectToParse)
        {
            return new ContractClientRequisitionEstadoAlteracaoViewModel()
            {
                NoLinha = ObjectToParse.NoLinha,
                ContractNo = ObjectToParse.NºContrato,
                InvoiceGroup = ObjectToParse.GrupoFatura,
                ProjectNo = ObjectToParse.NºProjeto,
                StartDate = ObjectToParse.DataInícioCompromisso.ToString("yyyy-MM-dd"),
                EndDate = ObjectToParse.DataFimCompromisso.HasValue ? ObjectToParse.DataFimCompromisso.Value.ToString("yyyy-MM-dd") : "",
                ClientRequisitionNo = ObjectToParse.NºRequisiçãoCliente,
                RequisitionDate = ObjectToParse.DataRequisição.HasValue ? ObjectToParse.DataRequisição.Value.ToString("yyyy-MM-dd") : "",
                PromiseNo = ObjectToParse.NºCompromisso,
                LastInvoiceDate = ObjectToParse.DataÚltimaFatura.HasValue ? ObjectToParse.DataÚltimaFatura.Value.ToString("yyyy-MM-dd") : "",
                InvoiceNo = ObjectToParse.NºFatura,
                InvoiceValue = ObjectToParse.ValorFatura,
                CreateDate = ObjectToParse.DataHoraCriação,
                CreateUser = ObjectToParse.UtilizadorCriação,
                UpdateDate = ObjectToParse.DataHoraModificação,
                UpdateUser = ObjectToParse.UtilizadorModificação
            };
        }
    }
}
