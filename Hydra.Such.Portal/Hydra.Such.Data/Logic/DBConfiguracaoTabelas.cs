using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;
using Hydra.Such.Data.ViewModel.Viaturas;

namespace Hydra.Such.Data.Logic
{
    public static class DBConfiguracaoTabelas
    {
        public static List<ConfiguracaoTabelas> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguracaoTabelas.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ConfiguracaoTabelas GetByTabelaAndID(string Tabela, int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguracaoTabelas.Where(p => p.Tabela == Tabela && p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<ConfiguracaoTabelas> GetAllByTabela(string Tabela)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguracaoTabelas.Where(p => p.Tabela == Tabela).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static int GetMaxByTabela(string Tabela)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguracaoTabelas.Where(p => p.Tabela == Tabela).OrderByDescending(x => x.ID).FirstOrDefault() != null ? ctx.ConfiguracaoTabelas.Where(p => p.Tabela == Tabela).OrderByDescending(x => x.ID).FirstOrDefault().ID : 0;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static ConfiguracaoTabelas Create(ConfiguracaoTabelas ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.ConfiguracaoTabelas.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(ConfiguracaoTabelas ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ConfiguracaoTabelas.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static ConfiguracaoTabelas Update(ConfiguracaoTabelas ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ConfiguracaoTabelas.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConfiguracaoTabelas ParseToDB(ConfiguracaoTabelasViewModel x)
        {
            ConfiguracaoTabelas viatura = new ConfiguracaoTabelas()
            {
                Tabela = x.Tabela,
                ID = x.ID,
                Descricao = x.Descricao,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
            };

            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) viatura.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);

            return viatura;
        }

        public static List<ConfiguracaoTabelas> ParseListToViewModel(List<ConfiguracaoTabelasViewModel> x)
        {
            List<ConfiguracaoTabelas> ConfiguracaoTabelas = new List<ConfiguracaoTabelas>();

            x.ForEach(y => ConfiguracaoTabelas.Add(ParseToDB(y)));

            return ConfiguracaoTabelas;
        }

        public static ConfiguracaoTabelasViewModel ParseToViewModel(ConfiguracaoTabelas x)
        {
            ConfiguracaoTabelasViewModel viatura = new ConfiguracaoTabelasViewModel()
            {
                Tabela = x.Tabela,
                ID = x.ID,
                Descricao = x.Descricao,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
            };

            if (x.DataCriacao != null) viatura.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");

            return viatura;
        }

        public static List<ConfiguracaoTabelasViewModel> ParseListToViewModel(List<ConfiguracaoTabelas> x)
        {
            List<ConfiguracaoTabelasViewModel> ConfiguracaoTabelas = new List<ConfiguracaoTabelasViewModel>();

            x.ForEach(y => ConfiguracaoTabelas.Add(ParseToViewModel(y)));

            return ConfiguracaoTabelas;
        }
    }
}
