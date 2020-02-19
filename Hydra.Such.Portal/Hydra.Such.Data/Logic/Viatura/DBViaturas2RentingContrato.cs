using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2RentingContrato
    {
        public static List<Viaturas2RentingContrato> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2RentingContrato.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2RentingContrato GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2RentingContrato.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2RentingContrato> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2RentingContrato.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2RentingContrato Create(Viaturas2RentingContrato ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2RentingContrato.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2RentingContrato ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2RentingContrato.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2RentingContrato Update(Viaturas2RentingContrato ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2RentingContrato.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2RentingContrato ParseToDB(Viaturas2RentingContratoViewModel x)
        {
            Viaturas2RentingContrato viatura = new Viaturas2RentingContrato()
            {
                ID = x.ID,
                Matricula = x.Matricula,

                IDFornecedor = x.IDFornecedor,
                NoContrato = x.NoContrato,
                KmInicio = x.KmInicio,
                KmContratados = x.KmContratados,
                PrecoKmAdicionalSemIVA = x.PrecoKmAdicionalSemIVA,
                PrecoKmNaoPercorridoSemIVA = x.PrecoKmNaoPercorridoSemIVA,
                KmMaximo = x.KmMaximo,
                DataInicio = x.DataInicio,
                DataTermo = x.DataTermo,
                DuracaoContratoMensal = x.DuracaoContratoMensal,
                InicioPagamento = x.InicioPagamento,
                NoPagamentos = x.NoPagamentos,
                IDPeriodicidade = x.IDPeriodicidade,
                TotalSemIVA = x.TotalSemIVA,
                IVA = x.IVA,
                TotalComIVA = x.TotalComIVA,
                FranquiaSemIVA = x.FranquiaSemIVA,
                ResponsavelContrato = x.ResponsavelContrato,
                Observacoes = x.Observacoes,

                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (!string.IsNullOrEmpty(x.DataInicioTexto)) viatura.DataInicio = Convert.ToDateTime(x.DataInicioTexto);
            if (!string.IsNullOrEmpty(x.DataTermoTexto)) viatura.DataTermo = Convert.ToDateTime(x.DataTermoTexto);
            if (!string.IsNullOrEmpty(x.InicioPagamentoTexto)) viatura.InicioPagamento = Convert.ToDateTime(x.InicioPagamentoTexto);
            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) viatura.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) viatura.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return viatura;
        }

        public static List<Viaturas2RentingContrato> ParseListToViewModel(List<Viaturas2RentingContratoViewModel> x)
        {
            List<Viaturas2RentingContrato> Viaturas2RentingContrato = new List<Viaturas2RentingContrato>();

            x.ForEach(y => Viaturas2RentingContrato.Add(ParseToDB(y)));

            return Viaturas2RentingContrato;
        }

        public static Viaturas2RentingContratoViewModel ParseToViewModel(Viaturas2RentingContrato x)
        {
            Viaturas2RentingContratoViewModel viatura = new Viaturas2RentingContratoViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,

                IDFornecedor = x.IDFornecedor,
                NoContrato = x.NoContrato,
                KmInicio = x.KmInicio,
                KmContratados = x.KmContratados,
                PrecoKmAdicionalSemIVA = x.PrecoKmAdicionalSemIVA,
                PrecoKmNaoPercorridoSemIVA = x.PrecoKmNaoPercorridoSemIVA,
                KmMaximo = x.KmMaximo,
                DataInicio = x.DataInicio,
                DataTermo = x.DataTermo,
                DuracaoContratoMensal = x.DuracaoContratoMensal,
                InicioPagamento = x.InicioPagamento,
                NoPagamentos = x.NoPagamentos,
                IDPeriodicidade = x.IDPeriodicidade,
                TotalSemIVA = x.TotalSemIVA,
                IVA = x.IVA,
                TotalComIVA = x.TotalComIVA,
                FranquiaSemIVA = x.FranquiaSemIVA,
                ResponsavelContrato = x.ResponsavelContrato,
                Observacoes = x.Observacoes,

                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.DataInicio != null) viatura.DataInicioTexto = x.DataInicio.Value.ToString("yyyy-MM-dd");
            if (x.DataTermo != null) viatura.DataTermoTexto = x.DataTermo.Value.ToString("yyyy-MM-dd");
            if (x.InicioPagamento != null) viatura.InicioPagamentoTexto = x.InicioPagamento.Value.ToString("yyyy-MM-dd");
            if (x.DataCriacao != null) viatura.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) viatura.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return viatura;
        }

        public static List<Viaturas2RentingContratoViewModel> ParseListToViewModel(List<Viaturas2RentingContrato> x)
        {
            List<Viaturas2RentingContratoViewModel> Viaturas2RentingContrato = new List<Viaturas2RentingContratoViewModel>();

            x.ForEach(y => Viaturas2RentingContrato.Add(ParseToViewModel(y)));

            return Viaturas2RentingContrato;
        }
    }
}
