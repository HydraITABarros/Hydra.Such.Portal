using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.ProjectDiary
{
    public class DBServices
    {
        public static Serviços GetById(string Codigo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Serviços.Where(x => x.Código == Codigo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static List<Serviços> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Serviços.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Serviços Create(Serviços ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Serviços.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Serviços Update(Serviços ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Serviços.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(string ProfileId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<Serviços> ProfileAccessesToDelete = ctx.Serviços.Where(x => x.Código == ProfileId).ToList();
                    ctx.Serviços.RemoveRange(ProfileAccessesToDelete);
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
