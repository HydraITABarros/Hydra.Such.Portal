using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBNumerationConfigurations
    {

        #region CRUD
        public static ConfiguraçãoNumerações GetById(int id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguraçãoNumerações.Where(x => x.Id == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ConfiguraçãoNumerações> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguraçãoNumerações.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConfiguraçãoNumerações Create(ConfiguraçãoNumerações ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.ConfiguraçãoNumerações.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConfiguraçãoNumerações Update(ConfiguraçãoNumerações ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //Check if need to clear Last Used Numeration
                    ConfiguraçãoNumerações OldNumerationObject = GetById(ObjectToUpdate.Id);
                    if (OldNumerationObject.Prefixo != ObjectToUpdate.Prefixo || OldNumerationObject.NºDígitosIncrementar != ObjectToUpdate.NºDígitosIncrementar)
                    {
                        ObjectToUpdate.ÚltimoNºUsado = "";
                    }

                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.ConfiguraçãoNumerações.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(ConfiguraçãoNumerações ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ConfiguraçãoNumerações.Remove(ObjectToDelete);
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


     
        public static string GetNextNumeration(int id, bool isAuto)
        {
            try
            {
                ConfiguraçãoNumerações ConfNumeration = null;
                using (var ctx = new SuchDBContext())
                {
                    ConfNumeration = ctx.ConfiguraçãoNumerações.Where(x => x.Id == id).FirstOrDefault();

                    if (ConfNumeration.Automático == isAuto)
                    {
                        string NextNumeration = ConfNumeration.Prefixo;

                        //Check if is first numeration
                        if (ConfNumeration.ÚltimoNºUsado != null && ConfNumeration.ÚltimoNºUsado != "")
                        {
                            int LastUsedNumber = int.Parse(ConfNumeration.ÚltimoNºUsado.Replace(ConfNumeration.Prefixo, ""));

                            LastUsedNumber += ConfNumeration.QuantidadeIncrementar.Value;

                            NextNumeration += LastUsedNumber.ToString().PadLeft(ConfNumeration.NºDígitosIncrementar.Value, '0');
                        }
                        else
                        {
                            NextNumeration += "1".PadLeft(ConfNumeration.NºDígitosIncrementar.Value, '0');
                        }

                        ConfNumeration.ÚltimoNºUsado = NextNumeration;
                        ctx.ConfiguraçãoNumerações.Update(ConfNumeration);
                        ctx.SaveChanges();

                        return NextNumeration;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }


    }
}
