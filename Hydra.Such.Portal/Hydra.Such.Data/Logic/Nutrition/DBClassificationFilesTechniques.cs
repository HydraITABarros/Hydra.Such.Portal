using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Nutrition;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public static class DBClassificationFilesTechniques
    {
        #region CRUD
        public static List<ClassificaçãoFichasTécnicas> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ClassificaçãoFichasTécnicas.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ClassificaçãoFichasTécnicas Create(ClassificaçãoFichasTécnicas ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.ClassificaçãoFichasTécnicas.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ClassificaçãoFichasTécnicas Update(ClassificaçãoFichasTécnicas ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.ClassificaçãoFichasTécnicas.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static bool Delete(ClassificaçãoFichasTécnicas ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ClassificaçãoFichasTécnicas.Remove(ObjectToDelete);
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

        public static List<ClassificaçãoFichasTécnicas> GetAllFiles()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ClassificaçãoFichasTécnicas.Where(x => x.Tipo == 1).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<ClassificaçãoFichasTécnicas> GetTypeFiles(int Type)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ClassificaçãoFichasTécnicas.Where(x => x.Tipo == Type).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ClassificaçãoFichasTécnicas ParseToDatabase(this ClassificationFilesTechniquesViewModel x)
        {
            if (x != null)
            {
                return new ClassificaçãoFichasTécnicas()
                {

                    Código = x.Code,
                    Tipo = x.Type,
                    Descrição = x.Description,
                    Grupo = x.Group,
                    DataHoraCriação = x.CreateDate,
                    DataHoraModificação = x.UpdateDate,
                    UtilizadorCriação = x.CreateUser,
                    UtilizadorModificação = x.UpdateUser
                };
            }
            return null;
        }
        public static List<ClassificaçãoFichasTécnicas> ParseToDatabase(this List<ClassificationFilesTechniquesViewModel> items)
        {
            List<ClassificaçãoFichasTécnicas> itemsParse = new List<ClassificaçãoFichasTécnicas>();
            if (items != null)
                items.ForEach(x =>
                    itemsParse.Add(ParseToDatabase(x)));
            return itemsParse;
        }

        public static ClassificationFilesTechniquesViewModel ParseToViewModel(this ClassificaçãoFichasTécnicas item)
        {
            if (item != null)
            {
                return new ClassificationFilesTechniquesViewModel()
                {
                
                    Code = item.Código,
                    Type = item.Tipo,
                    Description = item.Descrição,
                    Group = item.Grupo,
                    CreateDate = item.DataHoraCriação,
                    UpdateDate = item.DataHoraModificação,
                    CreateUser = item.UtilizadorCriação,
                    UpdateUser=item.UtilizadorModificação

                };
            }
            return null;
        }

        public static List<ClassificationFilesTechniquesViewModel> ParseToViewModel(this List<ClassificaçãoFichasTécnicas> items)
        {
            List<ClassificationFilesTechniquesViewModel> parsedItems = new List<ClassificationFilesTechniquesViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(ParseToViewModel(x)));
            return parsedItems;
        }
    }
}
