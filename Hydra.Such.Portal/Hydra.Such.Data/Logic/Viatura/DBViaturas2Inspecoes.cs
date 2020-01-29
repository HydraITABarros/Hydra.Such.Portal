using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2Inspecoes
    {
        public static List<Viaturas2Inspecoes> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Inspecoes.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Inspecoes GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Inspecoes.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2Inspecoes> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Inspecoes.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Inspecoes Create(Viaturas2Inspecoes ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2Inspecoes.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2Inspecoes ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2Inspecoes.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2Inspecoes Update(Viaturas2Inspecoes ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2Inspecoes.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2Inspecoes ParseToDB(Viaturas2InspecoesViewModel x)
        {
            Viaturas2Inspecoes viatura = new Viaturas2Inspecoes()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                DataInspecao = x.DataInspecao,
                KmInspecao = x.KmInspecao,
                IDResultado = x.IDResultado,
                ProximaInspecao = x.ProximaInspecao,
                Observacao = x.Observacao,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (!string.IsNullOrEmpty(x.DataInspecaoTexto)) viatura.DataInspecao = Convert.ToDateTime(x.DataInspecaoTexto);
            if (!string.IsNullOrEmpty(x.ProximaInspecaoTexto)) viatura.ProximaInspecao = Convert.ToDateTime(x.ProximaInspecaoTexto);
            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) viatura.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) viatura.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return viatura;
        }

        public static List<Viaturas2Inspecoes> ParseListToViewModel(List<Viaturas2InspecoesViewModel> x)
        {
            List<Viaturas2Inspecoes> Viaturas2Inspecoes = new List<Viaturas2Inspecoes>();

            x.ForEach(y => Viaturas2Inspecoes.Add(ParseToDB(y)));

            return Viaturas2Inspecoes;
        }

        public static Viaturas2InspecoesViewModel ParseToViewModel(Viaturas2Inspecoes x)
        {
            Viaturas2InspecoesViewModel viatura = new Viaturas2InspecoesViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                DataInspecao = x.DataInspecao,
                KmInspecao = x.KmInspecao,
                IDResultado = x.IDResultado,
                ProximaInspecao = x.ProximaInspecao,
                Observacao = x.Observacao,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.DataInspecao != null) viatura.DataInspecaoTexto = x.DataInspecao.Value.ToString("yyyy-MM-dd");
            if (x.ProximaInspecao != null) viatura.ProximaInspecaoTexto = x.ProximaInspecao.Value.ToString("yyyy-MM-dd");
            if (x.DataCriacao != null) viatura.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) viatura.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return viatura;
        }

        public static List<Viaturas2InspecoesViewModel> ParseListToViewModel(List<Viaturas2Inspecoes> x)
        {
            List<Viaturas2InspecoesViewModel> Viaturas2Inspecoes = new List<Viaturas2InspecoesViewModel>();

            x.ForEach(y => Viaturas2Inspecoes.Add(ParseToViewModel(y)));

            return Viaturas2Inspecoes;
        }
    }
}
