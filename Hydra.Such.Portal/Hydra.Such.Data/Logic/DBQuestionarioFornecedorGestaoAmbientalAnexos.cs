using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Fornecedores;

namespace Hydra.Such.Data.Logic
{
    public static class DBQuestionarioFornecedorGestaoAmbientalAnexos
    {
        #region CRUD
        public static QuestionarioFornecedorGestaoAmbientalAnexos GetById(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.QuestionarioFornecedorGestaoAmbientalAnexos.Where(x => x.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static QuestionarioFornecedorGestaoAmbientalAnexos GetByCodigoAndVersao(string Codigo, int Versao)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.QuestionarioFornecedorGestaoAmbientalAnexos.Where(x => x.Codigo == Codigo && x.Versao == Versao).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<QuestionarioFornecedorGestaoAmbientalAnexos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.QuestionarioFornecedorGestaoAmbientalAnexos.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<QuestionarioFornecedorGestaoAmbientalAnexos> GetAllByFornecedor(string ID_Fornecedor)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.QuestionarioFornecedorGestaoAmbientalAnexos.Where(x => x.ID_Fornecedor == ID_Fornecedor).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static QuestionarioFornecedorGestaoAmbientalAnexos Create(QuestionarioFornecedorGestaoAmbientalAnexos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHora_Criacao = DateTime.Now;
                    ctx.QuestionarioFornecedorGestaoAmbientalAnexos.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<QuestionarioFornecedorGestaoAmbientalAnexos> Create(List<QuestionarioFornecedorGestaoAmbientalAnexos> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(x =>
                    {
                        x.DataHora_Criacao = DateTime.Now;
                        ctx.QuestionarioFornecedorGestaoAmbientalAnexos.Add(x);
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

        public static QuestionarioFornecedorGestaoAmbientalAnexos Update(QuestionarioFornecedorGestaoAmbientalAnexos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHora_Modificacao = DateTime.Now;
                    ctx.QuestionarioFornecedorGestaoAmbientalAnexos.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    QuestionarioFornecedorGestaoAmbientalAnexos Questionario = ctx.QuestionarioFornecedorGestaoAmbientalAnexos.Where(x => x.ID == ID).FirstOrDefault();
                    if (Questionario != null)
                    {
                        ctx.QuestionarioFornecedorGestaoAmbientalAnexos.Remove(Questionario);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public static bool Delete(QuestionarioFornecedorGestaoAmbientalAnexos item)
        {
            return Delete(new List<QuestionarioFornecedorGestaoAmbientalAnexos> { item });
        }

        public static bool Delete(List<QuestionarioFornecedorGestaoAmbientalAnexos> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.QuestionarioFornecedorGestaoAmbientalAnexos.RemoveRange(items);
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

        public static QuestionarioFornecedorGestaoAmbientalAnexosViewModel ParseToViewModel(this QuestionarioFornecedorGestaoAmbientalAnexos item)
        {
            if (item != null)
            {
                return new QuestionarioFornecedorGestaoAmbientalAnexosViewModel()
                {
                    ID = item.ID,
                    Codigo = item.Codigo,
                    Versao = item.Versao,
                    ID_Fornecedor = item.ID_Fornecedor,
                    URL_Anexo = item.URL_Anexo,
                    Visivel = item.Visivel,
                    Visivel_Texto = item.Visivel.HasValue ? item.Visivel == true ? "Sim" : "Não" : "Não",
                    DataHora_Criacao = item.DataHora_Criacao,
                    DataHora_Criacao_Texto = item.DataHora_Criacao.HasValue ? item.DataHora_Criacao.Value.ToString("yyyy-MM-dd") : "",
                    Utilizador_Criacao = item.Utilizador_Criacao,
                    Utilizador_Criacao_Texto = !string.IsNullOrEmpty(item.Utilizador_Criacao) ? DBUserConfigurations.GetById(item.Utilizador_Criacao).Nome : "",
                    DataHora_Modificacao = item.DataHora_Modificacao,
                    DataHora_Modificacao_Texto = item.DataHora_Modificacao.HasValue ? item.DataHora_Modificacao.Value.ToString("yyyy-MM-dd") : "",
                    Utilizador_Modificacao = item.Utilizador_Modificacao,
                    Utilizador_Modificacao_Texto = !string.IsNullOrEmpty(item.Utilizador_Modificacao) ? DBUserConfigurations.GetById(item.Utilizador_Modificacao).Nome : ""
                };
            }
            return null;
        }

        public static List<QuestionarioFornecedorGestaoAmbientalAnexosViewModel> ParseToViewModel(this List<QuestionarioFornecedorGestaoAmbientalAnexos> items)
        {
            List<QuestionarioFornecedorGestaoAmbientalAnexosViewModel> Questionarios = new List<QuestionarioFornecedorGestaoAmbientalAnexosViewModel>();
            if (items != null)
                items.ForEach(x =>
                    Questionarios.Add(x.ParseToViewModel()));
            return Questionarios;
        }

        public static QuestionarioFornecedorGestaoAmbientalAnexos ParseToDB(this QuestionarioFornecedorGestaoAmbientalAnexosViewModel item)
        {
            if (item != null)
            {
                return new QuestionarioFornecedorGestaoAmbientalAnexos()
                {
                    ID = item.ID,
                    Codigo = item.Codigo,
                    Versao = item.Versao,
                    ID_Fornecedor = item.ID_Fornecedor,
                    URL_Anexo = item.URL_Anexo,
                    DataHora_Criacao = item.DataHora_Criacao,
                    Utilizador_Criacao = item.Utilizador_Criacao,
                    DataHora_Modificacao = item.DataHora_Modificacao,
                    Utilizador_Modificacao = item.Utilizador_Modificacao
                };
            }
            return null;
        }

        public static List<QuestionarioFornecedorGestaoAmbientalAnexos> ParseToDB(this List<QuestionarioFornecedorGestaoAmbientalAnexosViewModel> items)
        {
            List<QuestionarioFornecedorGestaoAmbientalAnexos> Questionarios = new List<QuestionarioFornecedorGestaoAmbientalAnexos>();
            if (items != null)
                items.ForEach(x =>
                    Questionarios.Add(x.ParseToDB()));
            return Questionarios;
        }
    }
}
