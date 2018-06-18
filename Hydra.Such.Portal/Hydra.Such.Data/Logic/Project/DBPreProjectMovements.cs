using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Project
{
    public static class DBPreProjectMovements
    {
        public static List<PréMovimentosProjeto> GetPreRegistered(string ProjectNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PréMovimentosProjeto.Where(x => x.NºProjeto == ProjectNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
           public static PréMovimentosProjeto GetByLine(int Line)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PréMovimentosProjeto.Where(x => x.NºLinha == Line && x.Registado == false).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
          public static PréMovimentosProjeto Update(PréMovimentosProjeto ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.PréMovimentosProjeto.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static PréMovimentosProjeto CreatePreRegist(PréMovimentosProjeto ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.PréMovimentosProjeto.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
      
}
