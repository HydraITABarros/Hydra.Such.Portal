using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.OrcamentoVM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.OrcamentoL
{
    public static class DBOrcamentos
    {
        #region CRUD
        public static Orcamentos GetById(string No)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Orcamentos.Where(x => x.No == No).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Orcamentos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Orcamentos.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Orcamentos> GetAllByEstado(int IDEstado)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Orcamentos.Where(x => x.IDEstado == IDEstado).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Orcamentos> GetAllByMeusOrcamentos(string user)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Orcamentos.Where(x => x.UtilizadorCriacao.ToLower() == user.ToLower() || x.UtilizadorAceite.ToLower() == user.ToLower() ||
                    x.UtilizadorConcluido.ToLower() == user.ToLower() || x.UtilizadorModificacao.ToLower() == user.ToLower() ||
                    x.EmailUtilizadorEnvio.ToLower() == user.ToLower()).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Orcamentos Create(Orcamentos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataCriacao = DateTime.Now;
                    ctx.Orcamentos.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Orcamentos> Create(List<Orcamentos> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(x =>
                    {
                        x.DataCriacao = DateTime.Now;
                        ctx.Orcamentos.Add(x);
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

        public static Orcamentos Update(Orcamentos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataModificacao = DateTime.Now;
                    ctx.Orcamentos.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(string No)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    Orcamentos orcamento = ctx.Orcamentos.Where(x => x.No == No).FirstOrDefault();
                    if (orcamento != null)
                    {
                        ctx.Orcamentos.Remove(orcamento);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public static bool Delete(Orcamentos item)
        {
            return Delete(new List<Orcamentos> { item });
        }

        public static bool Delete(List<Orcamentos> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Orcamentos.RemoveRange(items);
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

        public static OrcamentosViewModel ParseToViewModel(this Orcamentos item)
        {
            if (item != null)
            {
                return new OrcamentosViewModel()
                {
                    No = item.No,
                    NoCliente = item.NoCliente,
                    NoContacto = item.NoContacto,
                    DataValidade = item.DataValidade,
                    DataValidadeText = item.DataValidade.HasValue ? item.DataValidade.Value.ToString("yyyy-MM-dd") : "",
                    IDEstado = item.IDEstado,
                    Descricao = item.Descricao,
                    CodRegiao = item.CodRegiao,
                    UnidadePrestacao = item.UnidadePrestacao,
                    TipoFaturacao = item.TipoFaturacao,
                    CondicoesPagamento = item.CondicoesPagamento,
                    TotalSemIVA = item.TotalSemIVA,
                    TotalComIVA = item.TotalComIVA,
                    NoProposta = item.NoProposta,
                    ProjetoAssociado = item.ProjetoAssociado,
                    Email = item.Email,
                    EmailAssunto = item.EmailAssunto,
                    EmailCorpo = item.EmailCorpo,
                    EmailDataEnvio = item.EmailDataEnvio,
                    EmailDataEnvioText = item.EmailDataEnvio.HasValue ? item.EmailDataEnvio.Value.ToString("yyyy-MM-dd") : "",
                    EmailUtilizadorEnvio = item.EmailUtilizadorEnvio,
                    DataCriacao = item.DataCriacao,
                    DataCriacaoText = item.DataCriacao.HasValue ? item.DataCriacao.Value.ToString("yyyy-MM-dd") : "",
                    UtilizadorCriacao = item.UtilizadorCriacao,
                    DataAceite = item.DataAceite,
                    DataAceiteText = item.DataAceite.HasValue ? item.DataAceite.Value.ToString("yyyy-MM-dd") : "",
                    UtilizadorAceite = item.UtilizadorAceite,
                    DataNaoAceite = item.DataNaoAceite,
                    DataNaoAceiteText = item.DataNaoAceite.HasValue ? item.DataNaoAceite.Value.ToString("yyyy-MM-dd") : "",
                    UtilizadorNaoAceite = item.UtilizadorNaoAceite,
                    DataConcluido = item.DataConcluido,
                    DataConcluidoText = item.DataConcluido.HasValue ? item.DataConcluido.Value.ToString("yyyy-MM-dd") : "",
                    UtilizadorConcluido = item.UtilizadorConcluido,
                    DataModificacao = item.DataModificacao,
                    DataModificacaoText = item.DataModificacao.HasValue ? item.DataModificacao.Value.ToString("yyyy-MM-dd") : "",
                    UtilizadorModificacao = item.UtilizadorModificacao
                };
            }
            return null;
        }

        public static List<OrcamentosViewModel> ParseToViewModel(this List<Orcamentos> items)
        {
            List<OrcamentosViewModel> contacts = new List<OrcamentosViewModel>();
            if (items != null)
                items.ForEach(x =>
                    contacts.Add(x.ParseToViewModel()));
            return contacts;
        }

        public static Orcamentos ParseToDB(this OrcamentosViewModel item)
        {
            if (item != null)
            {
                return new Orcamentos()
                {
                    No = item.No,
                    NoCliente = item.NoCliente,
                    NoContacto = item.NoContacto,
                    DataValidade = string.IsNullOrEmpty(item.DataValidadeText) ? (DateTime?)null : DateTime.Parse(item.DataValidadeText),
                    IDEstado = item.IDEstado,
                    Descricao = item.Descricao,
                    CodRegiao = item.CodRegiao,
                    UnidadePrestacao = item.UnidadePrestacao,
                    TipoFaturacao = item.TipoFaturacao,
                    CondicoesPagamento = item.CondicoesPagamento,
                    TotalSemIVA = item.TotalSemIVA,
                    TotalComIVA = item.TotalComIVA,
                    NoProposta = item.NoProposta,
                    ProjetoAssociado = item.ProjetoAssociado,
                    Email = item.Email,
                    EmailAssunto = item.EmailAssunto,
                    EmailCorpo = item.EmailCorpo,
                    EmailDataEnvio = string.IsNullOrEmpty(item.EmailDataEnvioText) ? (DateTime?)null : DateTime.Parse(item.EmailDataEnvioText),
                    EmailUtilizadorEnvio = item.EmailUtilizadorEnvio,
                    DataCriacao = string.IsNullOrEmpty(item.DataCriacaoText) ? (DateTime?)null : DateTime.Parse(item.DataCriacaoText),
                    UtilizadorCriacao = item.UtilizadorCriacao,
                    DataAceite = string.IsNullOrEmpty(item.DataAceiteText) ? (DateTime?)null : DateTime.Parse(item.DataAceiteText),
                    UtilizadorAceite = item.UtilizadorAceite,
                    DataNaoAceite = string.IsNullOrEmpty(item.DataNaoAceiteText) ? (DateTime?)null : DateTime.Parse(item.DataNaoAceiteText),
                    UtilizadorNaoAceite = item.UtilizadorNaoAceite,
                    DataConcluido = string.IsNullOrEmpty(item.DataConcluidoText) ? (DateTime?)null : DateTime.Parse(item.DataConcluidoText),
                    UtilizadorConcluido = item.UtilizadorConcluido,
                    DataModificacao = string.IsNullOrEmpty(item.DataModificacaoText) ? (DateTime?)null : DateTime.Parse(item.DataModificacaoText),
                    UtilizadorModificacao = item.UtilizadorModificacao
                };
            }
            return null;
        }

        public static List<Orcamentos> ParseToDB(this List<OrcamentosViewModel> items)
        {
            List<Orcamentos> contacts = new List<Orcamentos>();
            if (items != null)
                items.ForEach(x =>
                    contacts.Add(x.ParseToDB()));
            return contacts;
        }
    }
}
