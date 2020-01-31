using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2Estados
    {
        public static List<Viaturas2Estados> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Estados.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Estados GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Estados.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2Estados> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Estados.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Estados GetByMatriculaRecent(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Estados.Where(p => p.Matricula == Matricula).OrderByDescending(x => x.DataInicio).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Estados Create(Viaturas2Estados ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2Estados.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2Estados ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2Estados.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2Estados Update(Viaturas2Estados ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2Estados.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2Estados ParseToDB(Viaturas2EstadosViewModel x)
        {
            Viaturas2Estados Parqueamento = new Viaturas2Estados()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDEstado = x.IDEstado,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (!string.IsNullOrEmpty(x.DataInicioTexto)) Parqueamento.DataInicio = Convert.ToDateTime(x.DataInicioTexto);
            if (!string.IsNullOrEmpty(x.DataFimTexto)) Parqueamento.DataFim = Convert.ToDateTime(x.DataFimTexto);
            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) Parqueamento.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) Parqueamento.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return Parqueamento;
        }

        public static List<Viaturas2Estados> ParseListToViewModel(List<Viaturas2EstadosViewModel> x)
        {
            List<Viaturas2Estados> Viaturas2Estados = new List<Viaturas2Estados>();

            x.ForEach(y => Viaturas2Estados.Add(ParseToDB(y)));

            return Viaturas2Estados;
        }

        public static Viaturas2EstadosViewModel ParseToViewModel(Viaturas2Estados x)
        {
            Viaturas2EstadosViewModel Parqueamento = new Viaturas2EstadosViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDEstado = x.IDEstado,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.DataInicio != null) Parqueamento.DataInicioTexto = x.DataInicio.Value.ToString("yyyy-MM-dd");
            if (x.DataFim != null) Parqueamento.DataFimTexto = x.DataFim.Value.ToString("yyyy-MM-dd");
            if (x.DataCriacao != null) Parqueamento.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) Parqueamento.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return Parqueamento;
        }

        public static List<Viaturas2EstadosViewModel> ParseListToViewModel(List<Viaturas2Estados> x)
        {
            List<Viaturas2EstadosViewModel> Viaturas2Estados = new List<Viaturas2EstadosViewModel>();

            x.ForEach(y => Viaturas2Estados.Add(ParseToViewModel(y)));

            return Viaturas2Estados;
        }
    }
}
