using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2AfetacaoAreaReal
    {
        public static List<Viaturas2AfetacaoAreaReal> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2AfetacaoAreaReal.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2AfetacaoAreaReal GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2AfetacaoAreaReal.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2AfetacaoAreaReal Create(Viaturas2AfetacaoAreaReal ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2AfetacaoAreaReal.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2AfetacaoAreaReal ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2AfetacaoAreaReal.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2AfetacaoAreaReal Update(Viaturas2AfetacaoAreaReal ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2AfetacaoAreaReal.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2AfetacaoAreaReal ParseToDB(Viaturas2AfetacaoAreaRealViewModel x)
        {
            Viaturas2AfetacaoAreaReal gestor = new Viaturas2AfetacaoAreaReal()
            {
                ID = x.ID,
                AreaReal = x.AreaReal,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) gestor.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) gestor.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return gestor;
        }

        public static List<Viaturas2AfetacaoAreaReal> ParseListToViewModel(List<Viaturas2AfetacaoAreaRealViewModel> x)
        {
            List<Viaturas2AfetacaoAreaReal> Viaturas2AfetacaoAreaReal = new List<Viaturas2AfetacaoAreaReal>();

            x.ForEach(y => Viaturas2AfetacaoAreaReal.Add(ParseToDB(y)));

            return Viaturas2AfetacaoAreaReal;
        }

        public static Viaturas2AfetacaoAreaRealViewModel ParseToViewModel(Viaturas2AfetacaoAreaReal x)
        {
            Viaturas2AfetacaoAreaRealViewModel gestor = new Viaturas2AfetacaoAreaRealViewModel()
            {
                ID = x.ID,
                AreaReal = x.AreaReal,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.DataCriacao != null) gestor.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) gestor.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return gestor;
        }

        public static List<Viaturas2AfetacaoAreaRealViewModel> ParseListToViewModel(List<Viaturas2AfetacaoAreaReal> x)
        {
            List<Viaturas2AfetacaoAreaRealViewModel> Viaturas2AfetacaoAreaReal = new List<Viaturas2AfetacaoAreaRealViewModel>();

            x.ForEach(y => Viaturas2AfetacaoAreaReal.Add(ParseToViewModel(y)));

            return Viaturas2AfetacaoAreaReal;
        }
    }
}
