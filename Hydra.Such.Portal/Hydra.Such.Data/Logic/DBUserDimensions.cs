using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.Logic
{
    class DBUserDimensions
    {

        //#region CRUD
        //public static AcessosDimensões GetById(string IdUtilizador, int Área, int Funcionalidade)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            return ctx.AcessosDimensões.Where(x => x.IdUtilizador == IdUtilizador && x. == Área && x.Funcionalidade == Funcionalidade).FirstOrDefault();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

        //public static List<AcessosDimensões> GetAll()
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            return ctx.AcessosDimensões.ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

        //public static AcessosDimensões Create(AcessosDimensões ObjectToCreate)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            ObjectToCreate.DataHoraCriação = DateTime.Now;
        //            ctx.AcessosDimensões.Add(ObjectToCreate);
        //            ctx.SaveChanges();
        //        }

        //        return ObjectToCreate;
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

        //public static AcessosDimensões Update(AcessosDimensões ObjectToUpdate)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            ObjectToUpdate.DataHoraModificação = DateTime.Now;
        //            ctx.AcessosDimensões.Update(ObjectToUpdate);
        //            ctx.SaveChanges();
        //        }

        //        return ObjectToUpdate;
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

        //public static bool DeleteAllFromUser(string UserId)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            List<AcessosDimensões> UserAccessesToDelete = ctx.AcessosDimensões.Where(x => x.IdUtilizador == UserId).ToList();
        //            ctx.AcessosDimensões.RemoveRange(UserAccessesToDelete);
        //            ctx.SaveChanges();
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {

        //        return false;
        //    }
        //}
        //#endregion

        //public static List<AcessosDimensões> GetByUserId(string UserId)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            return ctx.AcessosDimensões.Where(x => x.IdUtilizador == UserId).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}
    }
}
