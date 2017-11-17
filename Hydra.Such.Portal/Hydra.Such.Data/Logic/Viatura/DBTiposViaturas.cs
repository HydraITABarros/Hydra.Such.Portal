using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using Hydra.Such.Data.ViewModel.Viaturas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Viatura
{
    public class DBTiposViaturas
    {

        public static TiposViatura GetById(int? id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TiposViatura.FirstOrDefault(x => x.CódigoTipo == id);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static List<TiposViatura> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TiposViatura.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static TiposViatura Create(TiposViatura ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.TiposViatura.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static TiposViatura Update(TiposViatura ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.TiposViatura.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(TiposViatura ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.TiposViatura.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static TiposViatura ParseToDB(TiposViaturaViewModel x)
        {
            return new TiposViatura()
            {
                CódigoTipo = x.CodigoTipo,
                Descrição = x.Descricao,
                DataHoraCriação = x.DataHoraCriacao,
                UtilizadorCriação = x.UtilizadorCriacao,
                DataHoraModificação = x.DataHoraModificacao,
                UtilizadorModificação = x.UtilizadorModificacao
            };
        }

        public static TiposViaturaViewModel ParseToViewModel(TiposViatura x)
        {
            return new TiposViaturaViewModel()
            {
                CodigoTipo = x.CódigoTipo,
                Descricao = x.Descrição,
                DataHoraCriacao = x.DataHoraCriação,
                UtilizadorCriacao = x.UtilizadorCriação,
                DataHoraModificacao = x.DataHoraModificação,
                UtilizadorModificacao = x.UtilizadorModificação
            };
        }

        public static List<TiposViaturaViewModel> ParseListToViewModel(List<TiposViatura> x)
        {
            List<TiposViaturaViewModel> result = new List<TiposViaturaViewModel>();

            x.ForEach(y => result.Add(ParseToViewModel(y)));
            return result;
        }

    }
}
