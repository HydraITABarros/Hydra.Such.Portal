using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2Gestores
    {
        public static List<Viaturas2Gestores> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Gestores.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Gestores GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Gestores.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2Gestores> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Gestores.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Gestores Create(Viaturas2Gestores ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2Gestores.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2Gestores ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2Gestores.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2Gestores Update(Viaturas2Gestores ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2Gestores.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2Gestores ParseToDB(Viaturas2GestoresViewModel x)
        {
            Viaturas2Gestores gestor = new Viaturas2Gestores()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDGestor = x.IDGestor,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
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

        public static List<Viaturas2Gestores> ParseListToViewModel(List<Viaturas2GestoresViewModel> x)
        {
            List<Viaturas2Gestores> Viaturas2Gestores = new List<Viaturas2Gestores>();

            x.ForEach(y => Viaturas2Gestores.Add(ParseToDB(y)));

            return Viaturas2Gestores;
        }

        public static Viaturas2GestoresViewModel ParseToViewModel(Viaturas2Gestores x)
        {
            Viaturas2GestoresViewModel gestor = new Viaturas2GestoresViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDGestor = x.IDGestor,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
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

        public static List<Viaturas2GestoresViewModel> ParseListToViewModel(List<Viaturas2Gestores> x)
        {
            List<Viaturas2GestoresViewModel> Viaturas2Gestores = new List<Viaturas2GestoresViewModel>();

            x.ForEach(y => Viaturas2Gestores.Add(ParseToViewModel(y)));

            return Viaturas2Gestores;
        }
    }
}
