using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2CarTrack
    {
        public static List<Viaturas2CarTrack> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2CarTrack.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2CarTrack GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2CarTrack.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2CarTrack> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2CarTrack.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2CarTrack Create(Viaturas2CarTrack ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2CarTrack.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2CarTrack ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2CarTrack.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2CarTrack Update(Viaturas2CarTrack ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2CarTrack.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2CarTrack ParseToDB(Viaturas2CarTrackViewModel x)
        {
            Viaturas2CarTrack viatura = new Viaturas2CarTrack()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDEmpresa = x.IDEmpresa,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
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

        public static List<Viaturas2CarTrack> ParseListToViewModel(List<Viaturas2CarTrackViewModel> x)
        {
            List<Viaturas2CarTrack> Viaturas2CarTrack = new List<Viaturas2CarTrack>();

            x.ForEach(y => Viaturas2CarTrack.Add(ParseToDB(y)));

            return Viaturas2CarTrack;
        }

        public static Viaturas2CarTrackViewModel ParseToViewModel(Viaturas2CarTrack x)
        {
            Viaturas2CarTrackViewModel viatura = new Viaturas2CarTrackViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDEmpresa = x.IDEmpresa,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
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

        public static List<Viaturas2CarTrackViewModel> ParseListToViewModel(List<Viaturas2CarTrack> x)
        {
            List<Viaturas2CarTrackViewModel> Viaturas2CarTrack = new List<Viaturas2CarTrackViewModel>();

            x.ForEach(y => Viaturas2CarTrack.Add(ParseToViewModel(y)));

            return Viaturas2CarTrack;
        }
    }
}