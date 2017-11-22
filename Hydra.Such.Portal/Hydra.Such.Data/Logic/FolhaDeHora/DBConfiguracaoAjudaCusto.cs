using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBConfiguracaoAjudaCusto
    {

        public static List<ConfiguracaoAjudaCusto> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguracaoAjudaCusto.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ConfiguracaoAjudaCusto Create(ConfiguracaoAjudaCusto ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.ConfiguracaoAjudaCusto.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static ConfiguracaoAjudaCusto Update(ConfiguracaoAjudaCusto ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificacao = DateTime.Now;
                    ctx.ConfiguracaoAjudaCusto.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(ConfiguracaoAjudaCusto ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ConfiguracaoAjudaCusto.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static ConfiguracaoAjudaCusto ParseToDB(ConfiguracaoAjudaCustoViewModel x)
        {

            return new ConfiguracaoAjudaCusto()
            {
                CodigoTipoCusto = x.CodigoTipoCusto,
                CodigoRefCusto = x.CodigoRefCusto,
                DataChegadaDataPartida = x.DataChegadaDataPartida,
                DistanciaMinima = x.DistanciaMinima,
                LimiteHoraPartida = TimeSpan.TryParse(x.LimiteHoraPartida, out TimeSpan LimiteHoraPartida) ? LimiteHoraPartida : (TimeSpan?)null,
                LimiteHoraChegada = TimeSpan.TryParse(x.LimiteHoraChegada, out TimeSpan LimiteHoraChegada) ? LimiteHoraChegada : (TimeSpan?)null,
                Prioritario = x.Prioritario,
                TipoCusto = x.TipoCusto,
                SinalHoraPartida = x.SinalHoraPartida,
                HoraPartida = TimeSpan.TryParse(x.HoraPartida, out TimeSpan HoraPartida) ? HoraPartida : (TimeSpan?)null,
                SinalHoraChegada = x.SinalHoraChegada,
                HoraChegada = TimeSpan.TryParse(x.HoraChegada, out TimeSpan HoraChegada) ? HoraChegada : (TimeSpan?)null,
                TipoEstadia = x.TipoEstadia,
                DataHoraCriacao = x.DataHoraCriacao,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataHoraModificacao = x.DataHoraModificacao,
                UtilizadorModificacao = x.UtilizadorModificacao
            };
        }

        public static ConfiguracaoAjudaCustoViewModel ParseToViewModel(ConfiguracaoAjudaCusto x)
        {
            return new ConfiguracaoAjudaCustoViewModel()
            {
                CodigoTipoCusto = x.CodigoTipoCusto,
                CodigoRefCusto = x.CodigoRefCusto,
                DataChegadaDataPartida = x.DataChegadaDataPartida,
                DistanciaMinima = x.DistanciaMinima,
                LimiteHoraPartida = x.LimiteHoraPartida.ToString(),
                LimiteHoraChegada = x.LimiteHoraChegada.ToString(),
                Prioritario = x.Prioritario,
                TipoCusto = x.TipoCusto,
                SinalHoraPartida = x.SinalHoraPartida,
                HoraPartida = x.HoraPartida.ToString(),
                SinalHoraChegada = x.SinalHoraChegada,
                HoraChegada = x.HoraChegada.ToString(),
                TipoEstadia = x.TipoEstadia,
                DataHoraCriacao = x.DataHoraCriacao,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataHoraModificacao = x.DataHoraModificacao,
                UtilizadorModificacao = x.UtilizadorModificacao
            };
        }

        public static List<ConfiguracaoAjudaCustoViewModel> ParseListToViewModel(List<ConfiguracaoAjudaCusto> x)
        {
            List<ConfiguracaoAjudaCustoViewModel> result = new List<ConfiguracaoAjudaCustoViewModel>();

            x.ForEach(y => result.Add(ParseToViewModel(y)));
            return result;
        }
    }
}