using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2Substituicao
    {
        public static List<Viaturas2Substituicao> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Substituicao.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Substituicao GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Substituicao.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2Substituicao> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Substituicao.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Substituicao GetByMatriculaRecent(string Matricula, DateTime Data)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Substituicao.Where(p => p.Matricula == Matricula && (p.DataInicio <= Data && p.DataFim.HasValue ? p.DataFim >= Data : DateTime.Now >= Data)).OrderByDescending(x => x.DataFim.HasValue ? x.DataFim : x.DataInicio).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Substituicao Create(Viaturas2Substituicao ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2Substituicao.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2Substituicao ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2Substituicao.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2Substituicao Update(Viaturas2Substituicao ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2Substituicao.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2Substituicao ParseToDB(Viaturas2SubstituicaoViewModel x)
        {
            Viaturas2Substituicao gestor = new Viaturas2Substituicao()
            {
                ID = x.ID,
                Matricula = x.Matricula,

                MatriculaSubstituicao = x.MatriculaSubstituicao,
                DataInicio = x.DataInicio,
                KmInicio = x.KmInicio,
                DataFim = x.DataFim,
                KmFim = x.KmFim,
                Observacoes = x.Observacoes,

                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (!string.IsNullOrEmpty(x.DataInicioTexto)) gestor.DataInicio = Convert.ToDateTime(x.DataInicioTexto);
            if (!string.IsNullOrEmpty(x.DataFimTexto)) gestor.DataFim = Convert.ToDateTime(x.DataFimTexto);
            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) gestor.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) gestor.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return gestor;
        }

        public static List<Viaturas2Substituicao> ParseListToViewModel(List<Viaturas2SubstituicaoViewModel> x)
        {
            List<Viaturas2Substituicao> Viaturas2Substituicao = new List<Viaturas2Substituicao>();

            x.ForEach(y => Viaturas2Substituicao.Add(ParseToDB(y)));

            return Viaturas2Substituicao;
        }

        public static Viaturas2SubstituicaoViewModel ParseToViewModel(Viaturas2Substituicao x)
        {
            Viaturas2SubstituicaoViewModel gestor = new Viaturas2SubstituicaoViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,

                MatriculaSubstituicao = x.MatriculaSubstituicao,
                DataInicio = x.DataInicio,
                KmInicio = x.KmInicio,
                DataFim = x.DataFim,
                KmFim = x.KmFim,
                Observacoes = x.Observacoes,

                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.DataInicio != null) gestor.DataInicioTexto = x.DataInicio.Value.ToString("yyyy-MM-dd");
            if (x.DataFim != null) gestor.DataFimTexto = x.DataFim.Value.ToString("yyyy-MM-dd");
            if (x.DataCriacao != null) gestor.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) gestor.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return gestor;
        }

        public static List<Viaturas2SubstituicaoViewModel> ParseListToViewModel(List<Viaturas2Substituicao> x)
        {
            List<Viaturas2SubstituicaoViewModel> Viaturas2Substituicao = new List<Viaturas2SubstituicaoViewModel>();

            x.ForEach(y => Viaturas2Substituicao.Add(ParseToViewModel(y)));

            return Viaturas2Substituicao;
        }
    }
}
