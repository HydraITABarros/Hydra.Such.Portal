using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2CartaVerdeSeguradora
    {
        public static List<Viaturas2CartaVerdeSeguradora> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2CartaVerdeSeguradora.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2CartaVerdeSeguradora GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2CartaVerdeSeguradora.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2CartaVerdeSeguradora Create(Viaturas2CartaVerdeSeguradora ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2CartaVerdeSeguradora.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2CartaVerdeSeguradora ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2CartaVerdeSeguradora.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2CartaVerdeSeguradora Update(Viaturas2CartaVerdeSeguradora ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2CartaVerdeSeguradora.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2CartaVerdeSeguradora ParseToDB(Viaturas2CartaVerdeSeguradoraViewModel x)
        {
            Viaturas2CartaVerdeSeguradora viatura = new Viaturas2CartaVerdeSeguradora()
            {
                ID = x.ID,
                Seguradora = x.Seguradora,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) viatura.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) viatura.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return viatura;
        }

        public static List<Viaturas2CartaVerdeSeguradora> ParseListToViewModel(List<Viaturas2CartaVerdeSeguradoraViewModel> x)
        {
            List<Viaturas2CartaVerdeSeguradora> Viaturas2CartaVerdeSeguradora = new List<Viaturas2CartaVerdeSeguradora>();

            x.ForEach(y => Viaturas2CartaVerdeSeguradora.Add(ParseToDB(y)));

            return Viaturas2CartaVerdeSeguradora;
        }

        public static Viaturas2CartaVerdeSeguradoraViewModel ParseToViewModel(Viaturas2CartaVerdeSeguradora x)
        {
            Viaturas2CartaVerdeSeguradoraViewModel viatura = new Viaturas2CartaVerdeSeguradoraViewModel()
            {
                ID = x.ID,
                Seguradora = x.Seguradora,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.DataCriacao != null) viatura.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) viatura.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return viatura;
        }

        public static List<Viaturas2CartaVerdeSeguradoraViewModel> ParseListToViewModel(List<Viaturas2CartaVerdeSeguradora> x)
        {
            List<Viaturas2CartaVerdeSeguradoraViewModel> Viaturas2CartaVerdeSeguradora = new List<Viaturas2CartaVerdeSeguradoraViewModel>();

            x.ForEach(y => Viaturas2CartaVerdeSeguradora.Add(ParseToViewModel(y)));

            return Viaturas2CartaVerdeSeguradora;
        }
    }
}
