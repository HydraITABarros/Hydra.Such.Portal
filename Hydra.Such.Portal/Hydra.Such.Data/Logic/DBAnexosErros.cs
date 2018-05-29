using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public static class DBAnexosErros
    {
        #region CRUD
        public static AnexosErrosViewModel GetById(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AnexosErros.Select(AnexosErros => new AnexosErrosViewModel()
                    {
                        ID = AnexosErros.ID,
                        Origem = AnexosErros.Origem,
                        OrigemTexto = AnexosErros.Origem.ToString(),
                        Tipo = AnexosErros.Tipo,
                        TipoTexto = AnexosErros.Tipo.ToString(),
                        Codigo = AnexosErros.Codigo,
                        NomeAnexo = AnexosErros.NomeAnexo,
                        Anexo = AnexosErros.Anexo,
                        CriadoPor = AnexosErros.CriadoPor,
                        CriadoPorNome = AnexosErros.CriadoPor == null ? "" : DBUserConfigurations.GetById(AnexosErros.CriadoPor).Nome,
                        DataHora_Criacao = AnexosErros.DataHora_Criacao,
                        DataHora_CriacaoTexto = AnexosErros.DataHora_Criacao.HasValue ? AnexosErros.DataHora_Criacao.Value.ToString("yyyy-MM-dd") : "",
                        AlteradoPor = AnexosErros.AlteradoPor,
                        AlteradoPorNome = AnexosErros.AlteradoPor == null ? "" : DBUserConfigurations.GetById(AnexosErros.AlteradoPor).Nome,
                        DataHora_Alteracao = AnexosErros.DataHora_Alteracao,
                        DataHora_AlteracaoTexto = AnexosErros.DataHora_Alteracao.HasValue ? AnexosErros.DataHora_Alteracao.Value.ToString("yyyy-MM-dd") : ""
                    }).Where(x => x.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static AnexosErrosViewModel GetByOrigemAndTipoAndCodigo(int Origem, int Tipo, string Codigo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AnexosErros.Select(AnexosErros => new AnexosErrosViewModel()
                    {
                        ID = AnexosErros.ID,
                        Origem = AnexosErros.Origem,
                        OrigemTexto = AnexosErros.Origem.ToString(),
                        Tipo = AnexosErros.Tipo,
                        TipoTexto = AnexosErros.Tipo.ToString(),
                        Codigo = AnexosErros.Codigo,
                        NomeAnexo = AnexosErros.NomeAnexo,
                        Anexo = AnexosErros.Anexo,
                        CriadoPor = AnexosErros.CriadoPor,
                        CriadoPorNome = AnexosErros.CriadoPor == null ? "" : DBUserConfigurations.GetById(AnexosErros.CriadoPor).Nome,
                        DataHora_Criacao = AnexosErros.DataHora_Criacao,
                        DataHora_CriacaoTexto = AnexosErros.DataHora_Criacao.HasValue ? AnexosErros.DataHora_Criacao.Value.ToString("yyyy-MM-dd") : "",
                        AlteradoPor = AnexosErros.AlteradoPor,
                        AlteradoPorNome = AnexosErros.AlteradoPor == null ? "" : DBUserConfigurations.GetById(AnexosErros.AlteradoPor).Nome,
                        DataHora_Alteracao = AnexosErros.DataHora_Alteracao,
                        DataHora_AlteracaoTexto = AnexosErros.DataHora_Alteracao.HasValue ? AnexosErros.DataHora_Alteracao.Value.ToString("yyyy-MM-dd") : ""
                    }).Where(x => x.Origem == Origem && x.Tipo == Tipo && x.Codigo == Codigo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<AnexosErrosViewModel> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AnexosErros.Select(AnexosErros => new AnexosErrosViewModel()
                    {
                        ID = AnexosErros.ID,
                        Origem = AnexosErros.Origem,
                        OrigemTexto = AnexosErros.Origem.ToString(),
                        Tipo = AnexosErros.Tipo,
                        TipoTexto = AnexosErros.Tipo.ToString(),
                        Codigo = AnexosErros.Codigo,
                        NomeAnexo = AnexosErros.NomeAnexo,
                        Anexo = AnexosErros.Anexo,
                        CriadoPor = AnexosErros.CriadoPor,
                        CriadoPorNome = AnexosErros.CriadoPor == null ? "" : DBUserConfigurations.GetById(AnexosErros.CriadoPor).Nome,
                        DataHora_Criacao = AnexosErros.DataHora_Criacao,
                        DataHora_CriacaoTexto = AnexosErros.DataHora_Criacao.HasValue ? AnexosErros.DataHora_Criacao.Value.ToString("yyyy-MM-dd") : "",
                        AlteradoPor = AnexosErros.AlteradoPor,
                        AlteradoPorNome = AnexosErros.AlteradoPor == null ? "" : DBUserConfigurations.GetById(AnexosErros.AlteradoPor).Nome,
                        DataHora_Alteracao = AnexosErros.DataHora_Alteracao,
                        DataHora_AlteracaoTexto = AnexosErros.DataHora_Alteracao.HasValue ? AnexosErros.DataHora_Alteracao.Value.ToString("yyyy-MM-dd") : ""
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static AnexosErros Create(AnexosErros item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHora_Criacao = DateTime.Now;
                    ctx.AnexosErros.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static AnexosErros Update(AnexosErros item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHora_Alteracao = DateTime.Now;
                    ctx.AnexosErros.Update(item);
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
                    AnexosErros userAnexosErros = ctx.AnexosErros.Where(x => x.ID == ID).FirstOrDefault();
                    if (userAnexosErros != null)
                    {
                        ctx.AnexosErros.Remove(userAnexosErros);
                        ctx.SaveChanges();

                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        #endregion
    }
}
