using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Projects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Project
{
    public static class DBProjects
    {
        #region CRUD
        public static Projetos GetById(string NProjeto)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Projetos.Where(x => x.NºProjeto == NProjeto).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<Projetos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Projetos.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Projetos Create(Projetos ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.Projetos.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Projetos Update(Projetos ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.Projetos.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(Projetos ProjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Projetos.RemoveRange(ctx.Projetos.Where(x => x.NºProjeto == ProjectToDelete.NºProjeto));
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

        public static List<Projetos> GetAllByArea(int AreaId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Projetos.Where(x => x.Área == AreaId-1 && x.Estado != 3).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ProjectListItemViewModel> GetAllByAreaToList(int AreaId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Projetos.Where(x => x.Área == AreaId - 1).Select(x => new ProjectListItemViewModel()
                    {
                        ProjectNo = x.NºProjeto,
                        Date = x.Data,
                        DateText = x.Data.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Status = x.Estado,
                        Description = x.Descrição,
                        ClientNo = x.NºCliente,
                        RegionCode = x.CódigoRegião,
                        FunctionalAreaCode = x.CódigoÁreaFuncional,
                        ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                        ContractoNo = x.NºContrato,
                        ProjectTypeCode = x.CódTipoProjeto,
                        ProjectTypeDescription = x.CódTipoProjetoNavigation.Descrição
                    }).ToList(); ;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
