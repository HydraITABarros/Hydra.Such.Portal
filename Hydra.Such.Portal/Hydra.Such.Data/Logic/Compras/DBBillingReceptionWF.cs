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

        public static RececaoFaturacaoWorkflow GetById(int id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RececaoFaturacaoWorkflow
                        .SingleOrDefault(x => x.Id == id);
                        //.ParseToViewModel();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static RececaoFaturacaoWorkflow Create(RececaoFaturacaoWorkflow item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //item.Estado = BillingReceptionStates.Rececao;
                    item.DataCriacao = DateTime.Now;
                    //item.DataUltimaInteracao = DateTime.Now.ToString();
                    ctx.RececaoFaturacaoWorkflow.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static RececaoFaturacaoWorkflow Update(RececaoFaturacaoWorkflow item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataModificacao = DateTime.Now;
                    //item.DataUltimaInteracao = DateTime.Now.ToString();
                    ctx.RececaoFaturacaoWorkflow.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool Delete(RececaoFaturacaoWorkflow item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.RececaoFaturacaoWorkflow.Remove(item);
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

        #region Parse Utilities
        //public static BillingReceptionModel ParseToViewModel(this RececaoFaturacaoWorkflow item)
        //{
        //    if (item != null)
        //    {
        //        return new BillingReceptionModel()
        //        {
        //            Id = item.Id,
        //            AreaPendente = item.AreaPendente,
        //            CodAreaFuncional = item.CodAreaFuncional,
        //            CodCentroResponsabilidade = item.CodCentroResponsabilidade,
        //            CodFornecedor = item.CodFornecedor,
        //            CodLocalizacao = string.IsNullOrEmpty(item.CodLocalizacao) ? item.CodLocalizacao : item.CodLocalizacao.Trim(),
        //            CodRegiao = item.CodRegiao,
        //            CriadoPor = item.CriadoPor,
        //            DataCriacao = item.DataCriacao,
        //            DataDocFornecedor = !item.DataDocFornecedor.HasValue ? "" : item.DataDocFornecedor.Value.ToString("yyyy-MM-dd"),
        //            DataModificacao = item.DataModificacao,
        //            DataRececao = !item.DataRececao.HasValue ? "" : item.DataRececao.Value.ToString("yyyy-MM-dd"),
        //            DataUltimaInteracao = !item.DataUltimaInteracao.HasValue ? "" : item.DataUltimaInteracao.Value.ToString("yyyy-MM-dd"),
        //            Destinatario = item.Destinatario,
        //            Estado = (Enumerations.BillingReceptionStates)item.Estado,
        //            Local = item.Local,
        //            ModificadoPor = item.ModificadoPor,
        //            NumAcordoFornecedor = item.NumAcordoFornecedor,
        //            NumDocFornecedor = item.NumDocFornecedor,
        //            NumEncomenda = item.NumEncomenda,
        //            NumEncomendaManual = item.NumEncomendaManual,
        //            QuantidadeEncomenda = item.QuantidadeEncomenda,
        //            QuantidadeRecebida = item.QuantidadeRecebida,
        //            TipoDocumento = (Enumerations.BillingDocumentTypes)item.TipoDocumento,
        //            Valor = item.Valor,
        //            ValorEncomendaOriginal = item.ValorEncomendaOriginal,
        //            ValorRecebidoNaoContabilizado = item.ValorRecebidoNaoContabilizado
        //        };
        //    }
        //    return null;
        //}

        //public static List<BillingReceptionModel> ParseToViewModel(this List<RececaoFaturacaoWorkflow> items)
        //{
        //    List<BillingReceptionModel> parsedItems = new List<BillingReceptionModel>();
        //    if (items != null)
        //        items.ForEach(x =>
        //            parsedItems.Add(x.ParseToViewModel()));
        //    return parsedItems;
        //}

        //public static RececaoFaturacaoWorkflow ParseToDB(this BillingReceptionModel item)
        //{
        //    if (item != null)
        //    {
        //        return new RececaoFaturacaoWorkflow()
        //        {
        //            Id = item.Id,
        //            AreaPendente = item.AreaPendente,
        //            CodAreaFuncional = item.CodAreaFuncional,
        //            CodCentroResponsabilidade = item.CodCentroResponsabilidade,
        //            CodFornecedor = item.CodFornecedor,
        //            CodLocalizacao = string.IsNullOrEmpty(item.CodLocalizacao) ? item.CodLocalizacao : item.CodLocalizacao.Trim(),
        //            CodRegiao = item.CodRegiao,
        //            CriadoPor = item.CriadoPor,
        //            DataCriacao = item.DataCriacao,
        //            DataDocFornecedor = string.IsNullOrEmpty(item.DataDocFornecedor) ? (DateTime?)null : DateTime.Parse(item.DataDocFornecedor),
        //            DataModificacao = item.DataModificacao,
        //            DataRececao = string.IsNullOrEmpty(item.DataRececao) ? (DateTime?)null : DateTime.Parse(item.DataRececao),
        //            DataUltimaInteracao = string.IsNullOrEmpty(item.DataUltimaInteracao) ? (DateTime?)null : DateTime.Parse(item.DataUltimaInteracao),
        //            Destinatario = item.Destinatario,
        //            Estado = (int)item.Estado,
        //            Local = item.Local,
        //            ModificadoPor = item.ModificadoPor,
        //            NumAcordoFornecedor = item.NumAcordoFornecedor,
        //            NumDocFornecedor = item.NumDocFornecedor,
        //            NumEncomenda = item.NumEncomenda,
        //            NumEncomendaManual = item.NumEncomendaManual,
        //            QuantidadeEncomenda = item.QuantidadeEncomenda,
        //            QuantidadeRecebida = item.QuantidadeRecebida,
        //            TipoDocumento = (int)item.TipoDocumento,
        //            Valor = item.Valor,
        //            ValorEncomendaOriginal = item.ValorEncomendaOriginal,
        //            ValorRecebidoNaoContabilizado = item.ValorRecebidoNaoContabilizado
        //        };
        //    }
        //    return null;
        //}

        //public static List<RececaoFaturacaoWorkflow> ParseToDB(this List<BillingReceptionModel> items)
        //{
        //    List<RececaoFaturacaoWorkflow> parsedItems = new List<RececaoFaturacaoWorkflow>();
        //    if (items != null)
        //        items.ForEach(x =>
        //            parsedItems.Add(x.ParseToDB()));
        //    return parsedItems;
        //}
        #endregion

    }
}
