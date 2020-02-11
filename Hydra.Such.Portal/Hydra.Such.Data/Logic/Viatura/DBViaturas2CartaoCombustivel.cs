using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2CartaoCombustivel
    {
        public static List<Viaturas2CartaoCombustivel> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2CartaoCombustivel.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2CartaoCombustivel GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2CartaoCombustivel.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2CartaoCombustivel> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2CartaoCombustivel.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2CartaoCombustivel Create(Viaturas2CartaoCombustivel ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2CartaoCombustivel.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2CartaoCombustivel ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2CartaoCombustivel.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2CartaoCombustivel Update(Viaturas2CartaoCombustivel ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2CartaoCombustivel.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2CartaoCombustivel ParseToDB(Viaturas2CartaoCombustivelViewModel x)
        {
            Viaturas2CartaoCombustivel viatura = new Viaturas2CartaoCombustivel()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDEmpresa = x.IDEmpresa,
                NoCartao = x.NoCartao,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
                Plafon = x.Plafon,
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

        public static List<Viaturas2CartaoCombustivel> ParseListToViewModel(List<Viaturas2CartaoCombustivelViewModel> x)
        {
            List<Viaturas2CartaoCombustivel> Viaturas2CartaoCombustivel = new List<Viaturas2CartaoCombustivel>();

            x.ForEach(y => Viaturas2CartaoCombustivel.Add(ParseToDB(y)));

            return Viaturas2CartaoCombustivel;
        }

        public static Viaturas2CartaoCombustivelViewModel ParseToViewModel(Viaturas2CartaoCombustivel x)
        {
            Viaturas2CartaoCombustivelViewModel viatura = new Viaturas2CartaoCombustivelViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDEmpresa = x.IDEmpresa,
                NoCartao = x.NoCartao,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
                Plafon = x.Plafon,
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

        public static List<Viaturas2CartaoCombustivelViewModel> ParseListToViewModel(List<Viaturas2CartaoCombustivel> x)
        {
            List<Viaturas2CartaoCombustivelViewModel> Viaturas2CartaoCombustivel = new List<Viaturas2CartaoCombustivelViewModel>();

            x.ForEach(y => Viaturas2CartaoCombustivel.Add(ParseToViewModel(y)));

            return Viaturas2CartaoCombustivel;
        }
    }
}