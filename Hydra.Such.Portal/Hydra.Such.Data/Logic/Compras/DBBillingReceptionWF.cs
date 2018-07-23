using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Compras;
using Microsoft.EntityFrameworkCore;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.Logic.Request
{
    public static class DBBillingReceptionWf
    {
        #region CRUD

        public static List<RececaoFaturacaoWorkflow> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RececaoFaturacaoWorkflow
                        .OrderByDescending(x => x.Id)
                        .ToList();
                    //.ParseToViewModel();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static RececaoFaturacaoWorkflow GetLastById(string id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RececaoFaturacaoWorkflow.Where(x => x.IdRecFaturacao == id).LastOrDefault();
                    //.ParseToViewModel();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
          }

            //public static RececaoFaturacaoWorkflow Create(RececaoFaturacaoWorkflow item)
            //{
            //    if (item == null)
            //        throw new ArgumentNullException("item");
            //    try
            //    {
            //        using (var ctx = new SuchDBContext())
            //        {
            //            //item.Estado = BillingReceptionStates.Rececao;
            //            item.DataCriacao = DateTime.Now;
            //            //item.DataUltimaInteracao = DateTime.Now.ToString();
            //            ctx.RececaoFaturacaoWorkflow.Add(item);
            //            ctx.SaveChanges();
            //        }
            //        return item;
            //    }
            //    catch (Exception ex)
            //    {
            //        return null;
            //    }
            //}

            //public static RececaoFaturacaoWorkflow Update(RececaoFaturacaoWorkflow item)
            //{
            //    if (item == null)
            //        throw new ArgumentNullException("item");
            //    try
            //    {
            //        using (var ctx = new SuchDBContext())
            //        {
            //            item.DataModificacao = DateTime.Now;
            //            //item.DataUltimaInteracao = DateTime.Now.ToString();
            //            ctx.RececaoFaturacaoWorkflow.Update(item);
            //            ctx.SaveChanges();
            //        }

            //        return item;
            //    }
            //    catch (Exception ex)
            //    {
            //        return null;
            //    }
            //}

            //public static bool Delete(RececaoFaturacaoWorkflow item)
            //{
            //    if (item == null)
            //        throw new ArgumentNullException("item");
            //    try
            //    {
            //        using (var ctx = new SuchDBContext())
            //        {
            //            ctx.RececaoFaturacaoWorkflow.Remove(item);
            //            ctx.SaveChanges();
            //        }
            //        return true;
            //    }
            //    catch (Exception ex)
            //    {
            //        return false;
            //    }
            //}

            #endregion

            #region Parse Utilities
            public static BillingRecWorkflowModel ParseToViewModel(this RececaoFaturacaoWorkflow item)
        {
            if (item != null)
            {
                return new BillingRecWorkflowModel()
                {
                    Id = item.Id,
                    CriadoPor = item.CriadoPor,
                    DataCriacao = item.DataCriacao,
                    DataModificacao = item.DataModificacao,
                    Estado = (Enumerations.BillingReceptionStates)item.Estado,
                    ModificadoPor = item.ModificadoPor,
                    AreaWorkflow = item.AreaWorkflow,
                    Data = item.Data,
                    Descricao = item.Descricao,
                    IdRecFaturacao = item.IdRecFaturacao,
                    Utilizador = item.Utilizador,
                    CodTipoProblema = item.CodTipoProblema,
                    CodProblema = item.CodProblema,
                    EnderecoEnvio = item.EnderecoEnvio,
                    EnderecoFornecedor = item.EnderecoFornecedor,
                    Comentario = item.Comentario
                };
    
            }
            return null;
        }

        public static List<BillingRecWorkflowModel> ParseToViewModel(this List<RececaoFaturacaoWorkflow> items)
        {
            List<BillingRecWorkflowModel> parsedItems = new List<BillingRecWorkflowModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }

        public static RececaoFaturacaoWorkflow ParseToDB(this BillingRecWorkflowModel item)
        {
            if (item != null)
            {
                return new RececaoFaturacaoWorkflow()
                {
                    Id = item.Id,
                    CriadoPor = item.CriadoPor,
                    DataCriacao = item.DataCriacao,
                    DataModificacao = item.DataModificacao,
                    Estado = (int)item.Estado,
                    ModificadoPor = item.ModificadoPor,
                    AreaWorkflow = item.AreaWorkflow,
                    Data = item.Data,
                    Descricao = item.Descricao,
                    IdRecFaturacao = item.IdRecFaturacao,
                    Utilizador = item.Utilizador,
                    CodTipoProblema = item.CodTipoProblema,
                    CodProblema = item.CodProblema,
                    EnderecoEnvio = item.EnderecoEnvio,
                    EnderecoFornecedor = item.EnderecoFornecedor,
                    Comentario = item.Comentario
                };
            }
            return null;
        }

        public static List<RececaoFaturacaoWorkflow> ParseToDB(this List<BillingRecWorkflowModel> items)
        {
            List<RececaoFaturacaoWorkflow> parsedItems = new List<RececaoFaturacaoWorkflow>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
        #endregion

    }
}
