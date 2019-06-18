using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Approvals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Approvals
{
    public static class DBApprovalMovements
    {
        #region CRUD
        public static MovimentosDeAprovação GetById(int NºMovimento)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosDeAprovação.Where(x => x.NºMovimento == NºMovimento).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<MovimentosDeAprovação> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosDeAprovação.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static MovimentosDeAprovação Create(MovimentosDeAprovação ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.NºMovimento = new Int32();
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.MovimentosDeAprovação.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static MovimentosDeAprovação Update(MovimentosDeAprovação ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.MovimentosDeAprovação.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(MovimentosDeAprovação ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.MovimentosDeAprovação.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion

        public static List<MovimentosDeAprovação> GetAllAssignedToUserFilteredByStatus(string userId, int status)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UtilizadoresMovimentosDeAprovação.Where(x => x.Utilizador.ToLower() == userId.ToLower()).Select(x => x.NºMovimentoNavigation).Where(x => x.Estado == status).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<MovimentosDeAprovação> GetAllREQAssignedToUserFilteredByStatus(string userId, int status)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.UtilizadoresMovimentosDeAprovação.Where(x => x.Utilizador.ToLower() == userId.ToLower()).Select(x => x.NºMovimentoNavigation).Where(x => x.Estado == status && x.Tipo == 1).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        #region Parses

        public static ApprovalMovementsViewModel ParseToViewModel(MovimentosDeAprovação x)
        {
            return new ApprovalMovementsViewModel()
            {
                MovementNo = x.NºMovimento,
                Type = x.Tipo,
                Area = x.Área,
                FunctionalArea = x.CódigoÁreaFuncional,
                ResponsabilityCenter = x.CódigoCentroResponsabilidade,
                Region = x.CódigoRegião,
                Number = x.Número,
                RequestUser = x.UtilizadorSolicitou,
                Value = x.Valor,
                DateTimeApprove = x.DataHoraAprovação,
                DateTimeCreate = x.DataHoraCriação,
                UserCreate = x.UtilizadorCriação,
                UserUpdate = x.UtilizadorModificação,
                DateTimeUpdate = x.DataHoraModificação,
                Status = x.Estado,
                ReproveReason = x.MotivoDeRecusa,
                Level = x.Nivel
            };
        }

        public static List<ApprovalMovementsViewModel> ParseToViewModel(this List<MovimentosDeAprovação> items)
        {
            List<ApprovalMovementsViewModel> places = new List<ApprovalMovementsViewModel>();
            if (items != null)
                items.ForEach(x =>
                    places.Add(ParseToViewModel(x)));
            return places;
        }

        public static MovimentosDeAprovação ParseToDatabase(ApprovalMovementsViewModel x)
        {
            return new MovimentosDeAprovação()
            {
                NºMovimento = x.MovementNo,
                Tipo = x.Type,
                Área = x.Area,
                CódigoÁreaFuncional = x.FunctionalArea,
                CódigoCentroResponsabilidade = x.ResponsabilityCenter,
                CódigoRegião = x.Region,
                Número = x.Number,
                UtilizadorSolicitou = x.RequestUser,
                Valor = x.Value,
                DataHoraAprovação = x.DateTimeApprove,
                DataHoraCriação = x.DateTimeCreate,
                UtilizadorCriação = x.UserCreate,
                UtilizadorModificação = x.UserUpdate,
                DataHoraModificação = x.DateTimeUpdate,
                Estado = x.Status,
                MotivoDeRecusa = x.ReproveReason,
                Nivel = x.Level
            };
        }

        public static List<MovimentosDeAprovação> ParseToDatabase(this List<ApprovalMovementsViewModel> items)
        {
            List<MovimentosDeAprovação> places = new List<MovimentosDeAprovação>();
            if (items != null)
                items.ForEach(x =>
                    places.Add(ParseToDatabase(x)));
            return places;
        }

        #endregion



    }
}
