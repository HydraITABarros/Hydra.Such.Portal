using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;


namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2ParqueamentoLocal
    {
        public static List<Viaturas2ParqueamentoLocal> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2ParqueamentoLocal.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2ParqueamentoLocal GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2ParqueamentoLocal.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2ParqueamentoLocal Create(Viaturas2ParqueamentoLocal ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2ParqueamentoLocal.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2ParqueamentoLocal ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2ParqueamentoLocal.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2ParqueamentoLocal Update(Viaturas2ParqueamentoLocal ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2ParqueamentoLocal.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2ParqueamentoLocal ParseToDB(Viaturas2ParqueamentoLocalViewModel x)
        {
            Viaturas2ParqueamentoLocal Parqueamento = new Viaturas2ParqueamentoLocal()
            {
                ID = x.ID,
                Local = x.Local,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) Parqueamento.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) Parqueamento.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return Parqueamento;
        }

        public static List<Viaturas2ParqueamentoLocal> ParseListToViewModel(List<Viaturas2ParqueamentoLocalViewModel> x)
        {
            List<Viaturas2ParqueamentoLocal> Viaturas2ParqueamentoLocal = new List<Viaturas2ParqueamentoLocal>();

            x.ForEach(y => Viaturas2ParqueamentoLocal.Add(ParseToDB(y)));

            return Viaturas2ParqueamentoLocal;
        }

        public static Viaturas2ParqueamentoLocalViewModel ParseToViewModel(Viaturas2ParqueamentoLocal x)
        {
            Viaturas2ParqueamentoLocalViewModel Parqueamento = new Viaturas2ParqueamentoLocalViewModel()
            {
                ID = x.ID,
                Local = x.Local,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.DataCriacao != null) Parqueamento.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) Parqueamento.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return Parqueamento;
        }

        public static List<Viaturas2ParqueamentoLocalViewModel> ParseListToViewModel(List<Viaturas2ParqueamentoLocal> x)
        {
            List<Viaturas2ParqueamentoLocalViewModel> Viaturas2ParqueamentoLocal = new List<Viaturas2ParqueamentoLocalViewModel>();

            x.ForEach(y => Viaturas2ParqueamentoLocal.Add(ParseToViewModel(y)));

            return Viaturas2ParqueamentoLocal;
        }
    }
}
