using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Contracts
{
    public static class DBContractInvoiceText
    {
        #region CRUD
        public static TextoFaturaContrato GetById(string ContractNo, int InvoiceGroup, string projectNo, DateTime startDate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TextoFaturaContrato.Where(x => x.NºContrato == ContractNo && x.GrupoFatura == InvoiceGroup && x.NºProjeto == projectNo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<TextoFaturaContrato> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TextoFaturaContrato.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static TextoFaturaContrato Create(TextoFaturaContrato ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.TextoFaturaContrato.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static TextoFaturaContrato Update(TextoFaturaContrato ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.TextoFaturaContrato.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(TextoFaturaContrato InvoiceText)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.TextoFaturaContrato.Remove(InvoiceText);
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


        public static List<TextoFaturaContrato> GetByContract(string ContractNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TextoFaturaContrato.Where(x => x.NºContrato == ContractNo).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }



        public static TextoFaturaContrato ParseToDB(ContractInvoiceTextViewModel ObjectToParse)
        {
            return new TextoFaturaContrato()
            {
                NºContrato = ObjectToParse.ContractNo,
                GrupoFatura = ObjectToParse.InvoiceGroup,
                NºProjeto = ObjectToParse.ProjectNo,
                TextoFatura = ObjectToParse.InvoiceText,
                DataHoraCriação = ObjectToParse.CreateDate,
                UtilizadorCriação = ObjectToParse.CreateUser,
                DataHoraModificação = ObjectToParse.UpdateDate,
                UtilizadorModificação = ObjectToParse.UpdateUser
            };
        }

        public static ContractInvoiceTextViewModel ParseToViewModel(TextoFaturaContrato ObjectToParse)
        {
            return new ContractInvoiceTextViewModel()
            {
                ContractNo = ObjectToParse.NºContrato,
                InvoiceGroup = ObjectToParse.GrupoFatura,
                ProjectNo = ObjectToParse.NºProjeto,
                InvoiceText = ObjectToParse.TextoFatura,
                CreateDate = ObjectToParse.DataHoraCriação,
                CreateUser = ObjectToParse.UtilizadorCriação,
                UpdateDate = ObjectToParse.DataHoraModificação,
                UpdateUser = ObjectToParse.UtilizadorModificação
            };
        }
    }
}
