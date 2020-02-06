using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2CartaVerde
    {
        public static List<Viaturas2CartaVerde> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2CartaVerde.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2CartaVerde GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2CartaVerde.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2CartaVerde> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2CartaVerde.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2CartaVerde Create(Viaturas2CartaVerde ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2CartaVerde.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2CartaVerde ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2CartaVerde.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2CartaVerde Update(Viaturas2CartaVerde ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2CartaVerde.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2CartaVerde ParseToDB(Viaturas2CartaVerdeViewModel x)
        {
            Viaturas2CartaVerde viatura = new Viaturas2CartaVerde()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                NoApolice = x.NoApolice,
                IDSeguradora = x.IDSeguradora,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
                ValorPremioSeguro = x.ValorPremioSeguro,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (!string.IsNullOrEmpty(x.DataInicioTexto)) viatura.DataInicio = Convert.ToDateTime(x.DataInicioTexto);
            if (!string.IsNullOrEmpty(x.DataFimTexto)) viatura.DataFim = Convert.ToDateTime(x.DataFimTexto);
            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) viatura.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) viatura.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return viatura;
        }

        public static List<Viaturas2CartaVerde> ParseListToViewModel(List<Viaturas2CartaVerdeViewModel> x)
        {
            List<Viaturas2CartaVerde> Viaturas2CartaVerde = new List<Viaturas2CartaVerde>();

            x.ForEach(y => Viaturas2CartaVerde.Add(ParseToDB(y)));

            return Viaturas2CartaVerde;
        }

        public static Viaturas2CartaVerdeViewModel ParseToViewModel(Viaturas2CartaVerde x)
        {
            Viaturas2CartaVerdeViewModel viatura = new Viaturas2CartaVerdeViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                NoApolice = x.NoApolice,
                IDSeguradora = x.IDSeguradora,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
                ValorPremioSeguro = x.ValorPremioSeguro,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.DataInicio != null) viatura.DataInicioTexto = x.DataInicio.Value.ToString("yyyy-MM-dd");
            if (x.DataFim != null) viatura.DataFimTexto = x.DataFim.Value.ToString("yyyy-MM-dd");
            if (x.DataCriacao != null) viatura.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) viatura.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return viatura;
        }

        public static List<Viaturas2CartaVerdeViewModel> ParseListToViewModel(List<Viaturas2CartaVerde> x)
        {
            List<Viaturas2CartaVerdeViewModel> Viaturas2CartaVerde = new List<Viaturas2CartaVerdeViewModel>();

            x.ForEach(y => Viaturas2CartaVerde.Add(ParseToViewModel(y)));

            return Viaturas2CartaVerde;
        }
    }
}
