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
    public class DBModelos
    {

        public static Modelos GetById(int? id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Modelos.FirstOrDefault(x => x.CódigoModelo == id);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static List<Modelos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Modelos.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Modelos> GetAllByMarca(int marca)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Modelos.Where(m => m.CódigoMarca == marca).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Modelos Create(Modelos ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.Modelos.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static Modelos Update(Modelos ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.Modelos.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Modelos ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Modelos.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Modelos ParseToDB(ModelosViewModel x)
        {
            return new Modelos()
            {
                CódigoMarca = x.CodigoMarca,
                CódigoModelo = x.CodigoModelo,
                Descrição = x.Descricao,
                DataHoraCriação = x.DataHoraCriacao,
                UtilizadorCriação = x.UtilizadorCriacao,
                DataHoraModificação = x.DataHoraModificacao,
                UtilizadorModificação = x.UtilizadorModificacao
            };
        }

        public static ModelosViewModel ParseToViewModel(Modelos x)
        {
            return new ModelosViewModel()
            {
                CodigoMarca = x.CódigoMarca,
                CodigoModelo = x.CódigoModelo,
                Descricao = x.Descrição,
                DataHoraCriacao = x.DataHoraCriação,
                UtilizadorCriacao = x.UtilizadorCriação,
                DataHoraModificacao = x.DataHoraModificação,
                UtilizadorModificacao = x.UtilizadorModificação
            };
        }

        public static List<ModelosViewModel> ParseListToViewModel(List<Modelos> x)
        {
            List<ModelosViewModel> result = new List<ModelosViewModel>();

            x.ForEach(y => result.Add(ParseToViewModel(y)));
            return result;
        }

    }
}
