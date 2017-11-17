using Hydra.Such.Data.Database;

using Hydra.Such.Data.ViewModel.Viaturas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Viatura
{
    public class DBMarcas
    {

        public static Marcas GetById(int? id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Marcas.FirstOrDefault(x => x.CódigoMarca == id);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static List<Marcas> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Marcas.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Marcas Create(Marcas ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.Marcas.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static Marcas Update(Marcas ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.Marcas.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Marcas ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Marcas.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Marcas ParseToDB(MarcasViewModel x)
        {
            return new Marcas()
            {
                CódigoMarca = x.CodigoMarca,
                Tipo = x.Tipo,
                Descrição = x.Descricao,
                DataHoraCriação = x.DataHoraCriacao,
                UtilizadorCriação = x.UtilizadorCriacao,
                DataHoraModificação = x.DataHoraModificacao,
                UtilizadorModificação = x.UtilizadorModificacao
            };
        }

        public static MarcasViewModel ParseToViewModel(Marcas x)
        {
            return new MarcasViewModel()
            {
                CodigoMarca = x.CódigoMarca,
                Tipo = x.Tipo,
                Descricao = x.Descrição,
                DataHoraCriacao = x.DataHoraCriação,
                UtilizadorCriacao = x.UtilizadorCriação,
                DataHoraModificacao = x.DataHoraModificação,
                UtilizadorModificacao = x.UtilizadorModificação
            };
        }

        public static List<MarcasViewModel> ParseListToViewModel(List<Marcas> x)
        {
            List<MarcasViewModel> result = new List<MarcasViewModel>();

            x.ForEach(y => result.Add(ParseToViewModel(y)));
            return result;
        }

    }
}
