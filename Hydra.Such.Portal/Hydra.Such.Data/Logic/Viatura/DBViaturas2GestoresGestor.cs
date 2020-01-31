using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2GestoresGestor
    {
        public static List<Viaturas2GestoresGestor> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2GestoresGestor.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2GestoresGestor GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2GestoresGestor.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2GestoresGestor> GetByTipo(int Tipo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2GestoresGestor.Where(p => p.IDTipo == Tipo || p.IDTipo == 3).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2GestoresGestor Create(Viaturas2GestoresGestor ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2GestoresGestor.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2GestoresGestor ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2GestoresGestor.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2GestoresGestor Update(Viaturas2GestoresGestor ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2GestoresGestor.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2GestoresGestor ParseToDB(Viaturas2GestoresGestorViewModel x)
        {
            Viaturas2GestoresGestor gestor = new Viaturas2GestoresGestor()
            {
                ID = x.ID,
                Gestor = x.Gestor,
                NoMecanografico = x.NoMecanografico,
                Mail = x.Mail,
                IDTipo = x.IDTipo,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) gestor.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) gestor.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return gestor;
        }

        public static List<Viaturas2GestoresGestor> ParseListToViewModel(List<Viaturas2GestoresGestorViewModel> x)
        {
            List<Viaturas2GestoresGestor> Viaturas2GestoresGestor = new List<Viaturas2GestoresGestor>();

            x.ForEach(y => Viaturas2GestoresGestor.Add(ParseToDB(y)));

            return Viaturas2GestoresGestor;
        }

        public static Viaturas2GestoresGestorViewModel ParseToViewModel(Viaturas2GestoresGestor x)
        {
            Viaturas2GestoresGestorViewModel gestor = new Viaturas2GestoresGestorViewModel()
            {
                ID = x.ID,
                Gestor = x.Gestor,
                NoMecanografico = x.NoMecanografico,
                Mail = x.Mail,
                IDTipo = x.IDTipo,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.DataCriacao != null) gestor.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) gestor.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return gestor;
        }

        public static List<Viaturas2GestoresGestorViewModel> ParseListToViewModel(List<Viaturas2GestoresGestor> x)
        {
            List<Viaturas2GestoresGestorViewModel> Viaturas2GestoresGestor = new List<Viaturas2GestoresGestorViewModel>();

            x.ForEach(y => Viaturas2GestoresGestor.Add(ParseToViewModel(y)));

            return Viaturas2GestoresGestor;
        }
    }
}
