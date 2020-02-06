using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2Manutencao
    {
        public static List<Viaturas2Manutencao> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Manutencao.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Manutencao GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Manutencao.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2Manutencao> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Manutencao.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Manutencao Create(Viaturas2Manutencao ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2Manutencao.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2Manutencao ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2Manutencao.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2Manutencao Update(Viaturas2Manutencao ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2Manutencao.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2Manutencao ParseToDB(Viaturas2ManutencaoViewModel x)
        {
            Viaturas2Manutencao viatura = new Viaturas2Manutencao()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                Local = x.Local,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
                Observacoes = x.Observacoes,
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

        public static List<Viaturas2Manutencao> ParseListToViewModel(List<Viaturas2ManutencaoViewModel> x)
        {
            List<Viaturas2Manutencao> Viaturas2Manutencao = new List<Viaturas2Manutencao>();

            x.ForEach(y => Viaturas2Manutencao.Add(ParseToDB(y)));

            return Viaturas2Manutencao;
        }

        public static Viaturas2ManutencaoViewModel ParseToViewModel(Viaturas2Manutencao x)
        {
            Viaturas2ManutencaoViewModel viatura = new Viaturas2ManutencaoViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                Local = x.Local,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
                Observacoes = x.Observacoes,
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

        public static List<Viaturas2ManutencaoViewModel> ParseListToViewModel(List<Viaturas2Manutencao> x)
        {
            List<Viaturas2ManutencaoViewModel> Viaturas2Manutencao = new List<Viaturas2ManutencaoViewModel>();

            x.ForEach(y => Viaturas2Manutencao.Add(ParseToViewModel(y)));

            return Viaturas2Manutencao;
        }
    }
}
