using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.OrcamentoVM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.OrcamentoL
{
    public static class DBLinhasOrcamentos
    {
        #region CRUD
        public static LinhasOrcamentos GetById(int NoLinha, string NoOrcamento)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasOrcamentos.Where(x => x.NoLinha == NoLinha && x.OrcamentosNo == NoOrcamento).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<LinhasOrcamentos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasOrcamentos.OrderBy(x => x.Ordem).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<LinhasOrcamentos> GetAllByOrcamento(string NoOrcamento)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasOrcamentos.Where(x => x.OrcamentosNo == NoOrcamento).OrderBy(x => x.Ordem).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static int GetMaxOrdemByOrcamento(string NoOrcamento)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<LinhasOrcamentos> AllLinhas = ctx.LinhasOrcamentos.Where(x => x.OrcamentosNo == NoOrcamento).ToList();
                    return (int)AllLinhas.Max(x => x.Ordem);
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public static LinhasOrcamentos Create(LinhasOrcamentos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataCriacao = DateTime.Now;
                    ctx.LinhasOrcamentos.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<LinhasOrcamentos> Create(List<LinhasOrcamentos> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(x =>
                    {
                        x.DataCriacao = DateTime.Now;
                        ctx.LinhasOrcamentos.Add(x);
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

        public static LinhasOrcamentos Update(LinhasOrcamentos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataModificacao = DateTime.Now;
                    ctx.LinhasOrcamentos.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(int NoLinha)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    LinhasOrcamentos linha = ctx.LinhasOrcamentos.Where(x => x.NoLinha == NoLinha).FirstOrDefault();
                    if (linha != null)
                    {
                        ctx.LinhasOrcamentos.Remove(linha);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public static bool Delete(LinhasOrcamentos item)
        {
            return Delete(new List<LinhasOrcamentos> { item });
        }

        public static bool Delete(List<LinhasOrcamentos> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasOrcamentos.RemoveRange(items);
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

        public static LinhasOrcamentosViewModel ParseToViewModel(this LinhasOrcamentos item)
        {
            if (item != null)
            {
                return new LinhasOrcamentosViewModel()
                {
                    NoLinha = item.NoLinha,
                    NoOrcamento = item.OrcamentosNo,
                    Ordem = item.Ordem,
                    Descricao = item.Descricao,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario,
                    TaxaIVA = item.TaxaIVA,
                    TotalLinha = item.TotalLinha,
                    DataCriacao = item.DataCriacao,
                    DataCriacaoText = item.DataCriacao.HasValue ? item.DataCriacao.Value.ToString("yyyy-MM-dd") : "",
                    UtilizadorCriacao = item.UtilizadorCriacao,
                    DataModificacao = item.DataModificacao,
                    DataModificacaoText = item.DataModificacao.HasValue ? item.DataModificacao.Value.ToString("yyyy-MM-dd") : "",
                    UtilizadorModificacao = item.UtilizadorModificacao
                };
            }
            return null;
        }

        public static List<LinhasOrcamentosViewModel> ParseToViewModel(this List<LinhasOrcamentos> items)
        {
            List<LinhasOrcamentosViewModel> contacts = new List<LinhasOrcamentosViewModel>();
            if (items != null)
                items.ForEach(x =>
                    contacts.Add(x.ParseToViewModel()));
            return contacts;
        }

        public static LinhasOrcamentos ParseToDB(this LinhasOrcamentosViewModel item)
        {
            if (item != null)
            {
                return new LinhasOrcamentos()
                {
                    NoLinha = item.NoLinha,
                    OrcamentosNo = item.NoOrcamento,
                    Ordem = item.Ordem,
                    Descricao = item.Descricao,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario,
                    TaxaIVA = item.TaxaIVA,
                    TotalLinha = item.TotalLinha,
                    DataCriacao = item.DataCriacao,
                    UtilizadorCriacao = item.UtilizadorCriacao,
                    DataModificacao = item.DataModificacao,
                    UtilizadorModificacao = item.UtilizadorModificacao
                };
            }
            return null;
        }

        public static List<LinhasOrcamentos> ParseToDB(this List<LinhasOrcamentosViewModel> items)
        {
            List<LinhasOrcamentos> contacts = new List<LinhasOrcamentos>();
            if (items != null)
                items.ForEach(x =>
                    contacts.Add(x.ParseToDB()));
            return contacts;
        }
    }
}
