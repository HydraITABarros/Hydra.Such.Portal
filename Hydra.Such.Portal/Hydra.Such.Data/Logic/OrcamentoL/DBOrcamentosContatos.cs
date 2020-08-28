using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.OrcamentoVM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.OrcamentoL
{
    public static class DBOrcamentosContatos
    {
        #region CRUD
        public static OrcamentosContatos GetById(string ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.OrcamentosContatos.Where(x => x.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<OrcamentosContatos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.OrcamentosContatos.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static int GetMaxContato()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    string Max = ctx.OrcamentosContatos.Max(x => x.ID);

                    if (string.IsNullOrEmpty(Max))
                        return 0;
                    else
                        return Convert.ToInt32(Max.Substring(2));
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public static OrcamentosContatos Create(OrcamentosContatos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataCriacao = DateTime.Now;
                    ctx.OrcamentosContatos.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static OrcamentosContatos Update(OrcamentosContatos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataAlteracao = DateTime.Now;
                    ctx.OrcamentosContatos.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(string ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    OrcamentosContatos orcamento = ctx.OrcamentosContatos.Where(x => x.ID == ID).FirstOrDefault();
                    if (orcamento != null)
                    {
                        ctx.OrcamentosContatos.Remove(orcamento);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        #endregion

        public static OrcamentosContatosViewModel ParseToViewModel(this OrcamentosContatos item)
        {
            if (item != null)
            {
                return new OrcamentosContatosViewModel()
                {
                    ID = item.ID,
                    Organizacao = item.Organizacao,
                    Nome = item.Nome,
                    Telemovel = item.Telemovel,
                    Email = item.Email,
                    NIF = item.NIF,
                    Notas = item.Notas,
                    CriadoPor = item.CriadoPor,
                    DataCriacao = item.DataCriacao,
                    DataCriacaoText = item.DataCriacao.HasValue ? item.DataCriacao.Value.ToString("yyyy-MM-dd") : "",
                    AlteradoPor = item.AlteradoPor,
                    DataAlteracao = item.DataAlteracao,
                    DataAlteracaoText = item.DataAlteracao.HasValue ? item.DataAlteracao.Value.ToString("yyyy-MM-dd") : ""
                };
            }
            return null;
        }

        public static List<OrcamentosContatosViewModel> ParseToViewModel(this List<OrcamentosContatos> items)
        {
            List<OrcamentosContatosViewModel> contacts = new List<OrcamentosContatosViewModel>();
            if (items != null)
                items.ForEach(x =>
                    contacts.Add(x.ParseToViewModel()));
            return contacts;
        }

        public static OrcamentosContatos ParseToDB(this OrcamentosContatosViewModel item)
        {
            if (item != null)
            {
                return new OrcamentosContatos()
                {
                    ID = item.ID,
                    Organizacao = item.Organizacao,
                    Nome = item.Nome,
                    Telemovel = item.Telemovel,
                    Email = item.Email,
                    NIF = item.NIF,
                    Notas = item.Notas,
                    CriadoPor = item.CriadoPor,
                    DataCriacao = string.IsNullOrEmpty(item.DataCriacaoText) ? (DateTime?)null : DateTime.Parse(item.DataCriacaoText),
                    AlteradoPor = item.AlteradoPor,
                    DataAlteracao = string.IsNullOrEmpty(item.DataAlteracaoText) ? (DateTime?)null : DateTime.Parse(item.DataAlteracaoText),
                };
            }
            return null;
        }

        public static List<OrcamentosContatos> ParseToDB(this List<OrcamentosContatosViewModel> items)
        {
            List<OrcamentosContatos> contacts = new List<OrcamentosContatos>();
            if (items != null)
                items.ForEach(x =>
                    contacts.Add(x.ParseToDB()));
            return contacts;
        }
    }
}
