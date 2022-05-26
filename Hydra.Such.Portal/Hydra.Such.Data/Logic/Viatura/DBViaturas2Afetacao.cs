using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2Afetacao
    {
        public static List<Viaturas2Afetacao> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Afetacao.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Afetacao GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Afetacao.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2Afetacao> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Afetacao.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Afetacao GetByMatriculaRecent(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Afetacao.Where(p => p.Matricula == Matricula).OrderByDescending(x => x.DataInicio).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Afetacao Create(Viaturas2Afetacao ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2Afetacao.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2Afetacao ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2Afetacao.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2Afetacao Update(Viaturas2Afetacao ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2Afetacao.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2Afetacao ParseToDB(Viaturas2AfetacaoViewModel x)
        {
            Viaturas2Afetacao viatura = new Viaturas2Afetacao()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDAreaReal = x.IDAreaReal,
                LocalExato = x.LocalExato,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
                CodRegiao = x.CodRegiao,
                CodAreaFuncional = x.CodAreaFuncional,
                CodCentroResponsabilidade = x.CodCentroResponsabilidade,
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

        public static List<Viaturas2Afetacao> ParseListToViewModel(List<Viaturas2AfetacaoViewModel> x)
        {
            List<Viaturas2Afetacao> Viaturas2Afetacao = new List<Viaturas2Afetacao>();

            x.ForEach(y => Viaturas2Afetacao.Add(ParseToDB(y)));

            return Viaturas2Afetacao;
        }

        public static Viaturas2AfetacaoViewModel ParseToViewModel(Viaturas2Afetacao x)
        {
            Viaturas2AfetacaoViewModel viatura = new Viaturas2AfetacaoViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDAreaReal = x.IDAreaReal,
                LocalExato = x.LocalExato,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
                CodRegiao = x.CodRegiao,
                CodAreaFuncional = x.CodAreaFuncional,
                CodCentroResponsabilidade = x.CodCentroResponsabilidade,
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

        public static List<Viaturas2AfetacaoViewModel> ParseListToViewModel(List<Viaturas2Afetacao> x)
        {
            List<Viaturas2AfetacaoViewModel> Viaturas2Afetacao = new List<Viaturas2AfetacaoViewModel>();

            x.ForEach(y => Viaturas2Afetacao.Add(ParseToViewModel(y)));

            return Viaturas2Afetacao;
        }
    }
}