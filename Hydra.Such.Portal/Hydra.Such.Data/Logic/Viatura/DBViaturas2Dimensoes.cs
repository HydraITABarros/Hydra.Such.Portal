using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2Dimensoes
    {
        public static List<Viaturas2Dimensoes> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Dimensoes.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Dimensoes GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Dimensoes.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2Dimensoes> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Dimensoes.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Dimensoes GetByMatriculaRecent(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Dimensoes.Where(p => p.Matricula == Matricula).OrderByDescending(x => x.DataInicio).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Dimensoes Create(Viaturas2Dimensoes ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2Dimensoes.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2Dimensoes ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2Dimensoes.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2Dimensoes Update(Viaturas2Dimensoes ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2Dimensoes.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2Dimensoes ParseToDB(Viaturas2DimensoesViewModel x)
        {
            Viaturas2Dimensoes Parqueamento = new Viaturas2Dimensoes()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDTipoDimensao = x.IDTipoDimensao,
                Dimensao = x.Dimensao,
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

        public static List<Viaturas2Dimensoes> ParseListToViewModel(List<Viaturas2DimensoesViewModel> x)
        {
            List<Viaturas2Dimensoes> Viaturas2Dimensoes = new List<Viaturas2Dimensoes>();

            x.ForEach(y => Viaturas2Dimensoes.Add(ParseToDB(y)));

            return Viaturas2Dimensoes;
        }

        public static Viaturas2DimensoesViewModel ParseToViewModel(Viaturas2Dimensoes x)
        {
            Viaturas2DimensoesViewModel Parqueamento = new Viaturas2DimensoesViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDTipoDimensao = x.IDTipoDimensao,
                Dimensao = x.Dimensao,
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

        public static List<Viaturas2DimensoesViewModel> ParseListToViewModel(List<Viaturas2Dimensoes> x)
        {
            List<Viaturas2DimensoesViewModel> Viaturas2Dimensoes = new List<Viaturas2DimensoesViewModel>();

            x.ForEach(y => Viaturas2Dimensoes.Add(ParseToViewModel(y)));

            return Viaturas2Dimensoes;
        }
    }
}
