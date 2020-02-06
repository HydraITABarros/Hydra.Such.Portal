using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2ContraOrdenacoes
    {
        public static List<Viaturas2ContraOrdenacoes> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2ContraOrdenacoes.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2ContraOrdenacoes GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2ContraOrdenacoes.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2ContraOrdenacoes> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2ContraOrdenacoes.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2ContraOrdenacoes Create(Viaturas2ContraOrdenacoes ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2ContraOrdenacoes.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2ContraOrdenacoes ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2ContraOrdenacoes.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2ContraOrdenacoes Update(Viaturas2ContraOrdenacoes ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2ContraOrdenacoes.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2ContraOrdenacoes ParseToDB(Viaturas2ContraOrdenacoesViewModel x)
        {
            Viaturas2ContraOrdenacoes viatura = new Viaturas2ContraOrdenacoes()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                Local = x.Local,
                Data = x.Data,
                IDCondutor = x.IDCondutor,
                IDResponsabilidade = x.IDResponsabilidade,
                Valor = x.Valor,
                Observacoes = x.Observacoes,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (!string.IsNullOrEmpty(x.DataTexto)) viatura.Data = Convert.ToDateTime(x.DataTexto);
            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) viatura.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) viatura.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return viatura;
        }

        public static List<Viaturas2ContraOrdenacoes> ParseListToViewModel(List<Viaturas2ContraOrdenacoesViewModel> x)
        {
            List<Viaturas2ContraOrdenacoes> Viaturas2ContraOrdenacoes = new List<Viaturas2ContraOrdenacoes>();

            x.ForEach(y => Viaturas2ContraOrdenacoes.Add(ParseToDB(y)));

            return Viaturas2ContraOrdenacoes;
        }

        public static Viaturas2ContraOrdenacoesViewModel ParseToViewModel(Viaturas2ContraOrdenacoes x)
        {
            Viaturas2ContraOrdenacoesViewModel viatura = new Viaturas2ContraOrdenacoesViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                Local = x.Local,
                Data = x.Data,
                IDCondutor = x.IDCondutor,
                IDResponsabilidade = x.IDResponsabilidade,
                Valor = x.Valor,
                Observacoes = x.Observacoes,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.Data != null) viatura.DataTexto = x.Data.Value.ToString("yyyy-MM-dd");
            if (x.DataCriacao != null) viatura.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) viatura.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return viatura;
        }

        public static List<Viaturas2ContraOrdenacoesViewModel> ParseListToViewModel(List<Viaturas2ContraOrdenacoes> x)
        {
            List<Viaturas2ContraOrdenacoesViewModel> Viaturas2ContraOrdenacoes = new List<Viaturas2ContraOrdenacoesViewModel>();

            x.ForEach(y => Viaturas2ContraOrdenacoes.Add(ParseToViewModel(y)));

            return Viaturas2ContraOrdenacoes;
        }
    }
}
