using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2Acidentes
    {
        public static List<Viaturas2Acidentes> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Acidentes.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Acidentes GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Acidentes.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2Acidentes> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Acidentes.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Acidentes Create(Viaturas2Acidentes ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2Acidentes.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2Acidentes ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2Acidentes.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2Acidentes Update(Viaturas2Acidentes ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2Acidentes.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2Acidentes ParseToDB(Viaturas2AcidentesViewModel x)
        {
            Viaturas2Acidentes viatura = new Viaturas2Acidentes()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                Local = x.Local,
                Data = x.Data,
                IDCondutor = x.IDCondutor,
                IDResponsabilidade = x.IDResponsabilidade,
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

        public static List<Viaturas2Acidentes> ParseListToViewModel(List<Viaturas2AcidentesViewModel> x)
        {
            List<Viaturas2Acidentes> Viaturas2Acidentes = new List<Viaturas2Acidentes>();

            x.ForEach(y => Viaturas2Acidentes.Add(ParseToDB(y)));

            return Viaturas2Acidentes;
        }

        public static Viaturas2AcidentesViewModel ParseToViewModel(Viaturas2Acidentes x)
        {
            Viaturas2AcidentesViewModel viatura = new Viaturas2AcidentesViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                Local = x.Local,
                Data = x.Data,
                IDCondutor = x.IDCondutor,
                IDResponsabilidade = x.IDResponsabilidade,
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

        public static List<Viaturas2AcidentesViewModel> ParseListToViewModel(List<Viaturas2Acidentes> x)
        {
            List<Viaturas2AcidentesViewModel> Viaturas2Acidentes = new List<Viaturas2AcidentesViewModel>();

            x.ForEach(y => Viaturas2Acidentes.Add(ParseToViewModel(y)));

            return Viaturas2Acidentes;
        }
    }
}
