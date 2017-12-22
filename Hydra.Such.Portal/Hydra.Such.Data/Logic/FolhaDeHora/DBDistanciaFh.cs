using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBDistanciaFh
    {
        #region CRUD

        //public static DistanciaFh GetById(string id)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            DistanciaFh Distancia = ctx.DistanciaFh.FirstOrDefault(x => x.CódigoOrigem == id);

        //            if (Distancia == null)
        //                return ctx.DistanciaFh.FirstOrDefault(x => x.CódigoDestino == id);
        //            else
        //                return Distancia;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

        //public static List<DistanciaFh> GetAll()
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            return ctx.DistanciaFh.ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public static DistanciaFh Create(DistanciaFh ObjectToCreate)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            ObjectToCreate.DataHoraCriação = DateTime.Now;
        //            ctx.DistanciaFh.Add(ObjectToCreate);
        //            ctx.SaveChanges();
        //        }

        //        return ObjectToCreate;
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

        //public static DistanciaFh Update(DistanciaFh ObjectToUpdate)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            ObjectToUpdate.DataHoraÚltimaAlteração = DateTime.Now;
        //            ctx.DistanciaFh.Update(ObjectToUpdate);
        //            ctx.SaveChanges();
        //        }

        //        return ObjectToUpdate;
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

        //public static bool Delete(DistanciaFh ObjectToDelete)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            ctx.DistanciaFh.Remove(ObjectToDelete);
        //            ctx.SaveChanges();
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {

        //        return false;
        //    }
        //}

        //public static decimal GetDistanciaPrevista(string Origem, string Destino)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            DistanciaFh distancia;

        //            distancia = ctx.DistanciaFh.FirstOrDefault(x => x.CódigoOrigem == Origem && x.CódigoDestino == Destino);

        //            if (distancia == null)
        //                distancia = ctx.DistanciaFh.FirstOrDefault(x => x.CódigoOrigem == Destino && x.CódigoDestino == Origem);

        //            if (distancia != null)
        //                return Convert.ToDecimal(distancia.Distância);
        //            else
        //                return 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return 0;
        //    }
        //}

        public static DistanciaFh ParseToDB(DistanciaFHViewModel x)
        {
            try
            {
                return new DistanciaFh()
                {
                    CódigoOrigem = x.Origem,
                    CódigoDestino = x.Destino,
                    Distância = x.Distancia,
                    CriadoPor = x.CriadoPor,
                    DataHoraCriação = x.DataHoraCriacao,
                    AlteradoPor = x.AlteradoPor,
                    DataHoraÚltimaAlteração = x.DataHoraUltimaAlteracao
                };
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static DistanciaFHViewModel ParseToViewModel(DistanciaFh x)
        {
            return new DistanciaFHViewModel()
            {
                Origem = x.CódigoOrigem,
                Destino = x.CódigoDestino,
                Distancia = x.Distância,
                CriadoPor = x.CriadoPor,
                DataHoraCriacao = x.DataHoraCriação,
                AlteradoPor = x.AlteradoPor,
                DataHoraUltimaAlteracao = x.DataHoraÚltimaAlteração
            };
        }

        public static List<DistanciaFHViewModel> ParseListToViewModel(List<DistanciaFh> x)
        {
            List<DistanciaFHViewModel> result = new List<DistanciaFHViewModel>();

            x.ForEach(y => result.Add(ParseToViewModel(y)));
            return result;
        }

        #endregion
    }
}
