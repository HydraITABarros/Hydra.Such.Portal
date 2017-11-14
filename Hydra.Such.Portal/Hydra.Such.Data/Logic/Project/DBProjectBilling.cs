using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Project
{
    public static class DBProjectBilling
    {
        public static List<ProjetosFaturação> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ProjetosFaturação.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ProjetosFaturação Create(ProjetosFaturação ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.ProjetosFaturação.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ProjetosFaturação Update(ProjetosFaturação ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.ProjetosFaturação.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ProjetosFaturação GetById(int NºUnidadeProdutiva, string NºProjeto)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ProjetosFaturação.FirstOrDefault(x => x.NºUnidadeProdutiva == NºUnidadeProdutiva && x.NºProjeto == NºProjeto);
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(ProjetosFaturação ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ProjetosFaturação.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }


        public static List<ProjetosFaturação> GetByNUnidadeProdutiva(int NºUnidadeProdutiva)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ProjetosFaturação.Where(x => x.NºUnidadeProdutiva == NºUnidadeProdutiva).ToList(); ;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }



        public static ProjetosFaturação ParseToDB(DBProjectBillingViewModel x)
        {
            return new ProjetosFaturação()
            {
                NºUnidadeProdutiva = x.ProductivityUnitNo,
                NºProjeto = x.ProjectNo,
                Ativo = x.Active,
                DataHoraCriação = x.CreateDate,
                UtilizadorCriação = x.CreateUser,
                DataHoraModificação = x.UpdateDate,
                UtilizadorModificação = x.UpdateUser
            };
        }


        public static DBProjectBillingViewModel ParseToViewModel(ProjetosFaturação x)
        {
            return new DBProjectBillingViewModel()
            {
                ProductivityUnitNo = x.NºUnidadeProdutiva,
                ProjectNo = x.NºProjeto,
                Active = x.Ativo,
                CreateDate = x.DataHoraCriação,
                CreateUser = x.UtilizadorCriação,
                UpdateDate = x.DataHoraModificação,
                UpdateUser = x.UtilizadorModificação
            };
        }


        public static List<DBProjectBillingViewModel> ParseListToViewModel(List<ProjetosFaturação> x)
        {
            List<DBProjectBillingViewModel> result = new List<DBProjectBillingViewModel>();

            x.ForEach(y => result.Add(ParseToViewModel(y)));
            return result;
        }
    }
}
