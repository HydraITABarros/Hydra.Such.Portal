using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.Logic.Project
{
    public class DBProjectDiary
    {
        #region CRUD

        public static List<DiárioDeProjeto> GetAll(string user)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                        return ctx.DiárioDeProjeto.Where(x => x.Utilizador == user && x.Registado != true).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<DiárioDeProjeto> GetNonInvoiced()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioDeProjeto.Where(x => x.Faturável == true && x.FaturaçãoAutorizada == false).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static DiárioDeProjeto Create(DiárioDeProjeto ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.DiárioDeProjeto.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static DiárioDeProjeto Update(DiárioDeProjeto ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.DiárioDeProjeto.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(DiárioDeProjeto ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.DiárioDeProjeto.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        #endregion

        public static List<DiárioDeProjeto> GetByProjectNo(string ProjectNo, string user)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioDeProjeto.Where(x => x.NºProjeto == ProjectNo && x.Utilizador == user).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<DiárioDeProjeto> GetRegisteredDiary(string ProjectNo, string user)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioDeProjeto.Where(x => x.NºProjeto == ProjectNo && x.Utilizador == user && x.Registado == true).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
