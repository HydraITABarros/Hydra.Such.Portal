using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2Marcas
    {
        public static List<Viaturas2Marcas> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Marcas.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Marcas GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Marcas.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Marcas GetByMarca(string Marca)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Marcas.Where(p => p.Marca == Marca).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Marcas Create(Viaturas2Marcas ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2Marcas.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2Marcas ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2Marcas.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2Marcas Update(Viaturas2Marcas ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2Marcas.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2Marcas ParseToDB(Viaturas2MarcasViewModel x)
        {
            Viaturas2Marcas viatura = new Viaturas2Marcas()
            {
                ID = x.ID,
                Marca = x.Marca,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) viatura.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) viatura.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return viatura;
        }

        public static List<Viaturas2Marcas> ParseListToViewModel(List<Viaturas2MarcasViewModel> x)
        {
            List<Viaturas2Marcas> Viaturas2Marcas = new List<Viaturas2Marcas>();

            x.ForEach(y => Viaturas2Marcas.Add(ParseToDB(y)));

            return Viaturas2Marcas;
        }

        public static Viaturas2MarcasViewModel ParseToViewModel(Viaturas2Marcas x)
        {
            Viaturas2MarcasViewModel viatura = new Viaturas2MarcasViewModel()
            {
                ID = x.ID,
                Marca = x.Marca,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.DataCriacao != null) viatura.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) viatura.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return viatura;
        }

        public static List<Viaturas2MarcasViewModel> ParseListToViewModel(List<Viaturas2Marcas> x)
        {
            List<Viaturas2MarcasViewModel> Viaturas2Marcas = new List<Viaturas2MarcasViewModel>();

            x.ForEach(y => Viaturas2Marcas.Add(ParseToViewModel(y)));

            return Viaturas2Marcas;
        }
    }
}
