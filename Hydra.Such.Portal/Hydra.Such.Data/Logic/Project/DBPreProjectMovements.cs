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
        public static List<PréMovimentosProjeto> GetPreRegisteredByProjectAndRegistadas(string ProjectNo, bool Registadas)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PréMovimentosProjeto.Where(x => x.NºProjeto == ProjectNo && x.Registado == Registadas).ToList();
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

        public static List<PréMovimentosProjeto> GetUnregisteredById(List<int> ids)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PréMovimentosProjeto.Where(x => ids.Contains(x.NºLinha) && x.Registado == false).ToList();
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
        public static List<PréMovimentosProjeto> Update(List<PréMovimentosProjeto> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    if (items != null)
                        items.ForEach(x => x.DataHoraModificação = DateTime.Now);
                    ctx.PréMovimentosProjeto.UpdateRange(items);
                    ctx.SaveChanges();
                }

                return items;
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

        public static bool Delete(PréMovimentosProjeto ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PréMovimentosProjeto.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

    }

}
