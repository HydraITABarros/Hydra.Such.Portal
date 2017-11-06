using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FolhasDeHoras;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBMaoDeObraFolhaDeHoras
    {
        #region CRUD
        public static MãoDeObraFolhaDeHoras GetByMaoDeObraNo(int MaoDeObraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MãoDeObraFolhaDeHoras.Where(x => x.NºLinha == MaoDeObraNo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<MãoDeObraFolhaDeHoras> GetByFolhaHoraNo(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MãoDeObraFolhaDeHoras.Where(x => x.NºFolhaDeHoras == FolhaHoraNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<MãoDeObraFolhaDeHoras> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MãoDeObraFolhaDeHoras.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static MãoDeObraFolhaDeHoras Create(MãoDeObraFolhaDeHoras ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.MãoDeObraFolhaDeHoras.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static MãoDeObraFolhaDeHoras Update(MãoDeObraFolhaDeHoras ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.MãoDeObraFolhaDeHoras.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(int MaoDeObraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.MãoDeObraFolhaDeHoras.RemoveRange(ctx.MãoDeObraFolhaDeHoras.Where(x => x.NºLinha == MaoDeObraNo));
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

        public static List<MaoDeObraFolhaDeHorasListItemViewModel> GetAllByMaoDeObraToList(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MãoDeObraFolhaDeHoras.Where(x => x.NºFolhaDeHoras == FolhaHoraNo).Select(x => new MaoDeObraFolhaDeHorasListItemViewModel()
                    {
                        FolhaDeHorasNo = x.NºFolhaDeHoras,
                        LineNo = x.NºLinha,
                        Date = x.Date,
                        EmployedNo = x.NºEmpregado,
                        ProjectNo = x.NºProjeto,
                        WorkTypeCode = x.CódigoTipoTrabalho,
                        //StartTime = Convert.ToDateTime(x.HoraInício).ToLocalTime(),
                        StartTimeText = Convert.ToString(x.HoraInício),
                        //EndTime = Convert.ToDateTime(x.HoraFim).ToLocalTime(),
                        EndTimeText = Convert.ToString(x.HoraFim),
                        LunchTime = x.HorárioAlmoço,
                        DinnerTime = x.HorárioJantar,
                        FamilyCodeResource = x.CódigoFamíliaRecurso,
                        ResourceNo = x.NºRecurso,
                        UnitCodeMeasure = x.CódUnidadeMedida,
                        OMTypeCode = x.CódigoTipoOm,
                        //HoursNo = Convert.ToDateTime(x.NºDeHotas).ToLocalTime(),
                        HoursNoText = Convert.ToString(x.NºDeHotas),
                        DirectUnitCost = Convert.ToDecimal(x.CustoUnitárioDireto),
                        CostPrice = Convert.ToDecimal(x.PreçoDeCusto),
                        SalePrice = Convert.ToDecimal(x.PreçoDeVenda),
                        TotalPrice = Convert.ToDecimal(x.PreçoTotal),
                        DateTimeCreation = x.DataHoraCriação,
                        UserCreation = x.UtilizadorCriação,
                        DateTimeModification = x.DataHoraModificação,
                        UserModification = x.UtilizadorModificação
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
