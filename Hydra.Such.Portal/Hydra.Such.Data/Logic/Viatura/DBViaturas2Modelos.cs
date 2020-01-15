using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2Modelos
    {
        public static List<Viaturas2Modelos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Modelos.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Modelos GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Modelos.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Modelos GetByModelo(string Modelo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Modelos.Where(p => p.Modelo == Modelo).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2Modelos> GetAllByMarca(int IDMarca)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Modelos.Where(p => p.IDMarca == IDMarca).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Modelos Create(Viaturas2Modelos ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2Modelos.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2Modelos ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2Modelos.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2Modelos Update(Viaturas2Modelos ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2Modelos.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2Modelos ParseToDB(Viaturas2ModelosViewModel x)
        {
            Viaturas2Modelos viatura = new Viaturas2Modelos()
            {
                ID = x.ID,
                IDMarca = x.IDMarca,
                Modelo = x.Modelo,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) viatura.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) viatura.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return viatura;
        }

        public static List<Viaturas2Modelos> ParseListToViewModel(List<Viaturas2ModelosViewModel> x)
        {
            List<Viaturas2Modelos> Viaturas2Modelos = new List<Viaturas2Modelos>();

            x.ForEach(y => Viaturas2Modelos.Add(ParseToDB(y)));

            return Viaturas2Modelos;
        }

        public static Viaturas2ModelosViewModel ParseToViewModel(Viaturas2Modelos x)
        {
            Viaturas2ModelosViewModel viatura = new Viaturas2ModelosViewModel()
            {
                ID = x.ID,
                IDMarca = x.IDMarca,
                Modelo = x.Modelo,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.DataCriacao != null) viatura.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) viatura.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return viatura;
        }

        public static List<Viaturas2ModelosViewModel> ParseListToViewModel(List<Viaturas2Modelos> x)
        {
            List<Viaturas2ModelosViewModel> Viaturas2Modelos = new List<Viaturas2ModelosViewModel>();

            x.ForEach(y => Viaturas2Modelos.Add(ParseToViewModel(y)));

            return Viaturas2Modelos;
        }
    }
}
