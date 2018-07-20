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
    public static class DBBillingReception
    {
        #region CRUD

        //public static List<BillingReceptionModel> GetAll()
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            return ctx.RececaoFaturacao
        //                .OrderByDescending(x => x.Id)
        //                .ToList()
        //                .ParseToViewModel();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public static BillingReceptionModel GetById(string id)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            return ctx.RececaoFaturacao
        //                .SingleOrDefault(x => x.Id == id)
        //                .ParseToViewModel();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public static BillingReceptionModel Create(BillingReceptionModel item)
        //{
        //    if (item == null)
        //        throw new ArgumentNullException("item");
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            item.Estado = BillingReceptionStates.Rececao;
        //            item.DataCriacao = DateTime.Now;
        //            item.DataUltimaInteracao = DateTime.Now.ToString();
        //            ctx.RececaoFaturacao.Add(item.ParseToDB());

        //            RececaoFaturacaoWorkflow wf = new RececaoFaturacaoWorkflow();
        //            wf.IdRecFaturacao = item.Id;
        //            wf.AreaWorkflow = "Contabilidade";//TODO: Identificar áres possivels
        //            wf.Descricao = "Entrada fatura em receção";
        //            wf.CriadoPor = item.CriadoPor;
        //            wf.Data = DateTime.Now;
        //            wf.DataCriacao = DateTime.Now;
        //            wf.Estado = (int)BillingReceptionStates.Rececao;//TODO: Identificar estados possivels “Receção/Conferência”
        //            wf.Utilizador = item.CriadoPor;
        //            ctx.RececaoFaturacaoWorkflow.Add(wf);

        //            ctx.SaveChanges();
        //        }
        //        return item;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public static BillingReceptionModel Update(BillingReceptionModel item)
        //{
        //    if (item == null)
        //        throw new ArgumentNullException("item");
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            item.DataModificacao = DateTime.Now;
        //            item.DataUltimaInteracao = DateTime.Now.ToString();
        //            ctx.RececaoFaturacao.Update(item.ParseToDB());
        //            ctx.SaveChanges();
        //        }

        //        return item;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public static bool Delete(BillingReceptionModel item)
        //{
        //    if (item == null)
        //        throw new ArgumentNullException("item");
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            ctx.RececaoFaturacao.Remove(item.ParseToDB());
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
        public static BillingReceptionModel ParseToViewModel(this RececaoFaturacao item)
        {
            if (item != null)
            {
                var parsedItem = new BillingReceptionModel();
                parsedItem.Id = item.Id;
                parsedItem.IdAreaPendente = item.IdAreaPendente.HasValue ? (BillingReceptionAreas)item.IdAreaPendente : (BillingReceptionAreas?)null;
                parsedItem.AreaPendente = item.AreaPendente;
                parsedItem.AreaPendente2 = item.AreaPendente2;
                parsedItem.CodAreaFuncional = item.CodAreaFuncional;
                parsedItem.CodCentroResponsabilidade = item.CodCentroResponsabilidade;
                parsedItem.CodFornecedor = item.CodFornecedor;
                parsedItem.CodLocalizacao = string.IsNullOrEmpty(item.CodLocalizacao) ? item.CodLocalizacao : item.CodLocalizacao.Trim();
                parsedItem.CodRegiao = item.CodRegiao;
                parsedItem.CriadoPor = item.CriadoPor;
                parsedItem.DataCriacao = item.DataCriacao;
                parsedItem.DataDocFornecedor = !item.DataDocFornecedor.HasValue ? "" : item.DataDocFornecedor.Value.ToString("yyyy-MM-dd");
                parsedItem.DataModificacao = item.DataModificacao;
                parsedItem.DataRececao = !item.DataRececao.HasValue ? "" : item.DataRececao.Value.ToString("yyyy-MM-dd");
                parsedItem.DataUltimaInteracao = !item.DataUltimaInteracao.HasValue ? "" : item.DataUltimaInteracao.Value.ToString("yyyy-MM-dd");
                parsedItem.Destinatario = item.Destinatario;
                parsedItem.Estado = (Enumerations.BillingReceptionStates)item.Estado;
                parsedItem.Local = item.Local;
                parsedItem.ModificadoPor = item.ModificadoPor;
                parsedItem.NumAcordoFornecedor = item.NumAcordoFornecedor;
                parsedItem.NumDocFornecedor = item.NumDocFornecedor;
                parsedItem.NumEncomenda = item.NumEncomenda;
                parsedItem.NumEncomendaManual = item.NumEncomendaManual;
                parsedItem.QuantidadeEncomenda = item.QuantidadeEncomenda;
                parsedItem.QuantidadeRecebida = item.QuantidadeRecebida;
                parsedItem.TipoDocumento = (Enumerations.BillingDocumentTypes)item.TipoDocumento;
                parsedItem.Valor = item.Valor;
                parsedItem.ValorEncomendaOriginal = item.ValorEncomendaOriginal;
                parsedItem.ValorRecebidoNaoContabilizado = item.ValorRecebidoNaoContabilizado;
                parsedItem.DocumentoCriadoEm = item.DocumentoCriadoEm;
                parsedItem.DocumentoCriadoPor = item.DocumentoCriadoPor;
                parsedItem.DataPassaPendente = !item.DataPassaPendente.HasValue ? "" : item.DataPassaPendente.Value.ToString("dd-MM-yyyy");
                parsedItem.Descricao = item.Descricao;
                parsedItem.DescricaoProblema = item.DescricaoProblema;
                parsedItem.TipoProblema = item.TipoProblema;
                parsedItem.WorkflowItems = item.RececaoFaturacaoWorkflow.ToList().ParseToViewModel();
                parsedItem.DataResolucao= !item.DataResolucao.HasValue ? "" : item.DataResolucao.Value.ToString("yyyy-MM-dd");

                return parsedItem;
            }
            return null;
        }

        public static List<BillingReceptionModel> ParseToViewModel(this List<RececaoFaturacao> items)
        {
            List<BillingReceptionModel> parsedItems = new List<BillingReceptionModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }

        public static RececaoFaturacao ParseToDB(this BillingReceptionModel item)
        {
            if (item != null)
            {

                var parsedItem = new RececaoFaturacao();
                parsedItem.Id = item.Id;
                parsedItem.IdAreaPendente = (int)item.IdAreaPendente;
                parsedItem.AreaPendente = item.AreaPendente;
                parsedItem.AreaPendente2 = item.AreaPendente2;
                parsedItem.CodAreaFuncional = item.CodAreaFuncional;
                parsedItem.CodCentroResponsabilidade = item.CodCentroResponsabilidade;
                parsedItem.CodFornecedor = item.CodFornecedor;
                parsedItem.CodLocalizacao = string.IsNullOrEmpty(item.CodLocalizacao) ? item.CodLocalizacao : item.CodLocalizacao.Trim();
                parsedItem.CodRegiao = item.CodRegiao;
                parsedItem.CriadoPor = item.CriadoPor;
                parsedItem.DataCriacao = item.DataCriacao;
                parsedItem.DataDocFornecedor = string.IsNullOrEmpty(item.DataDocFornecedor) ? (DateTime?)null : DateTime.Parse(item.DataDocFornecedor);
                parsedItem.DataModificacao = item.DataModificacao;
                parsedItem.DataRececao = string.IsNullOrEmpty(item.DataRececao) ? (DateTime?)null : DateTime.Parse(item.DataRececao);
                parsedItem.DataUltimaInteracao = string.IsNullOrEmpty(item.DataUltimaInteracao) ? (DateTime?)null : DateTime.Parse(item.DataUltimaInteracao);
                parsedItem.Destinatario = item.Destinatario;
                parsedItem.Estado = (int)item.Estado;
                parsedItem.Local = item.Local;
                parsedItem.ModificadoPor = item.ModificadoPor;
                parsedItem.NumAcordoFornecedor = item.NumAcordoFornecedor;
                parsedItem.NumDocFornecedor = item.NumDocFornecedor;
                parsedItem.NumEncomenda = item.NumEncomenda;
                parsedItem.NumEncomendaManual = item.NumEncomendaManual;
                parsedItem.QuantidadeEncomenda = item.QuantidadeEncomenda;
                parsedItem.QuantidadeRecebida = item.QuantidadeRecebida;
                parsedItem.TipoDocumento = (int)item.TipoDocumento;
                parsedItem.Valor = item.Valor;
                parsedItem.ValorEncomendaOriginal = item.ValorEncomendaOriginal;
                parsedItem.ValorRecebidoNaoContabilizado = item.ValorRecebidoNaoContabilizado;
                parsedItem.DocumentoCriadoEm = item.DocumentoCriadoEm;
                parsedItem.DocumentoCriadoPor = item.DocumentoCriadoPor;
                parsedItem.DataPassaPendente = string.IsNullOrEmpty(item.DataPassaPendente) ? (DateTime?)null : DateTime.Parse(item.DataPassaPendente);
                parsedItem.Descricao = item.Descricao;
                parsedItem.DescricaoProblema = item.DescricaoProblema;
                parsedItem.TipoProblema = item.TipoProblema;
                parsedItem.DataResolucao = string.IsNullOrEmpty(item.DataResolucao) ? (DateTime?)null : DateTime.Parse(item.DataResolucao);

                return parsedItem;
               
              
            }
            return null;
        }

        public static List<RececaoFaturacao> ParseToDB(this List<BillingReceptionModel> items)
        {
            List<RececaoFaturacao> parsedItems = new List<RececaoFaturacao>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
        #endregion

    }
}
