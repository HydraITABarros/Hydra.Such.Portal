using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Contracts
{
    public static class DBContractClientRequisition
    {
        #region CRUD
        public static RequisiçõesClienteContrato GetById(string ContractNo, int InvoiceGroup, string projectNo, DateTime startDate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RequisiçõesClienteContrato.Where(x => x.NºContrato == ContractNo && x.GrupoFatura == InvoiceGroup && x.NºProjeto == projectNo && x.DataInícioCompromisso == startDate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<RequisiçõesClienteContrato> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RequisiçõesClienteContrato.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static RequisiçõesClienteContrato Create(RequisiçõesClienteContrato ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.RequisiçõesClienteContrato.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static RequisiçõesClienteContrato Update(RequisiçõesClienteContrato ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.RequisiçõesClienteContrato.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(RequisiçõesClienteContrato ClientRequisition)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.RequisiçõesClienteContrato.Remove(ClientRequisition);
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
                    ctx.RequisiçõesClienteContrato.RemoveRange(ctx.RequisiçõesClienteContrato.Where(x => x.NºContrato == ContractNo));
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static List<RequisiçõesClienteContrato> GetByContract(string ContractNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RequisiçõesClienteContrato.Where(x => x.NºContrato == ContractNo).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static RequisiçõesClienteContrato GetByContractAndGroup(string ContractNo, int? group)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RequisiçõesClienteContrato.Where(x => x.NºContrato == ContractNo && x.GrupoFatura == group).OrderByDescending(x => x.DataInícioCompromisso).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }



        public static RequisiçõesClienteContrato ParseToDB(ContractClientRequisitionViewModel  ObjectToParse)
        {
            return new RequisiçõesClienteContrato()
            {
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

        public static ContractClientRequisitionViewModel ParseToViewModel(RequisiçõesClienteContrato ObjectToParse)
        {
            return new ContractClientRequisitionViewModel()
            {
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
