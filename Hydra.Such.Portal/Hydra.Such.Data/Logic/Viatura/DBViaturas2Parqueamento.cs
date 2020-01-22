using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;


namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2Parqueamento
    {
        public static List<Viaturas2Parqueamento> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Parqueamento.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Parqueamento GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Parqueamento.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Parqueamento Create(Viaturas2Parqueamento ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2Parqueamento.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2Parqueamento ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2Parqueamento.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2Parqueamento Update(Viaturas2Parqueamento ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2Parqueamento.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2Parqueamento ParseToDB(Viaturas2ParqueamentoViewModel x)
        {
            Viaturas2Parqueamento Parqueamento = new Viaturas2Parqueamento()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDLocal = x.IDLocal,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) Parqueamento.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) Parqueamento.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return Parqueamento;
        }

        public static List<Viaturas2Parqueamento> ParseListToViewModel(List<Viaturas2ParqueamentoViewModel> x)
        {
            List<Viaturas2Parqueamento> Viaturas2Parqueamento = new List<Viaturas2Parqueamento>();

            x.ForEach(y => Viaturas2Parqueamento.Add(ParseToDB(y)));

            return Viaturas2Parqueamento;
        }

        public static Viaturas2ParqueamentoViewModel ParseToViewModel(Viaturas2Parqueamento x)
        {
            Viaturas2ParqueamentoViewModel Parqueamento = new Viaturas2ParqueamentoViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDLocal = x.IDLocal,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.DataCriacao != null) Parqueamento.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) Parqueamento.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return Parqueamento;
        }

        public static List<Viaturas2ParqueamentoViewModel> ParseListToViewModel(List<Viaturas2Parqueamento> x)
        {
            List<Viaturas2ParqueamentoViewModel> Viaturas2Parqueamento = new List<Viaturas2ParqueamentoViewModel>();

            x.ForEach(y => Viaturas2Parqueamento.Add(ParseToViewModel(y)));

            return Viaturas2Parqueamento;
        }
    }
}
