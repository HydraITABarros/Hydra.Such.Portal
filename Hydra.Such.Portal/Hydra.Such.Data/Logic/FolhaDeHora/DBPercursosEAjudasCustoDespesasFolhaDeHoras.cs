using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FolhasDeHoras;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBPercursosEAjudasCustoDespesasFolhaDeHoras
    {
        #region CRUD
        public static PercursosEAjudasCustoDespesasFolhaDeHoras GetByPercursoNo(int PercursoNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //TipoCusto = 1 = PERCURSO
                    return ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Where(x => x.NºLinha == PercursoNo && x.TipoCusto == 1).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<PercursosEAjudasCustoDespesasFolhaDeHoras> GetByFolhaHoraNo(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //TipoCusto = 1 = PERCURSO
                    return ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Where(x => x.NºFolhaDeHoras == FolhaHoraNo && x.TipoCusto == 1).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<PercursosEAjudasCustoDespesasFolhaDeHoras> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static PercursosEAjudasCustoDespesasFolhaDeHoras Create(PercursosEAjudasCustoDespesasFolhaDeHoras ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //TipoCusto = 1 = PERCURSO
                    ObjectToCreate.TipoCusto = 1;
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static PercursosEAjudasCustoDespesasFolhaDeHoras Update(PercursosEAjudasCustoDespesasFolhaDeHoras ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //TipoCusto = 1 = PERCURSO
                    ObjectToUpdate.TipoCusto = 1;
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(int PercursoNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.RemoveRange(ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Where(x => x.NºLinha == PercursoNo));
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

        public static List<PercursosEAjudasCustoDespesasFolhaDeHorasListItemViewModel> GetAllByPercursoToList(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Where(x => x.NºFolhaDeHoras == FolhaHoraNo && x.TipoCusto == 1).Select(x => new PercursosEAjudasCustoDespesasFolhaDeHorasListItemViewModel()
                    {
                        FolhaDeHorasNo = x.NºFolhaDeHoras,
                        CostType = x.TipoCusto,
                        LineNo = x.NºLinha,
                        Description = x.Descrição,
                        Source = x.Origem,
                        Destiny = x.Destino,
                        DateTravel = x.DataViagem,
                        DateTravelText = x.DataViagem.Value.ToShortDateString(),//.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Distance = Convert.ToDecimal(x.Distância),
                        Amount = Convert.ToDecimal(x.Quantidade),
                        UnitCost = Convert.ToDecimal(x.CustoUnitário),
                        TotalCost = Convert.ToDecimal(x.CustoTotal),
                        UnitPrice = Convert.ToDecimal(x.PreçoUnitário),
                        Justification = x.Justificação,
                        Payroll = x.RúbricaSalarial,
                        DateTimeCreation = x.DataHoraCriação,
                        DateTimeCreationText = x.DataHoraCriação.Value.ToShortDateString(),//.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        UserCreation = x.UtilizadorCriação,
                        DateTimeModification = x.DataHoraModificação,
                        DateTimeModificationText = x.DataHoraModificação.Value.ToShortDateString(),//.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        UserModification = x.UtilizadorModificação
                    }).ToList(); ;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<PercursosEAjudasCustoDespesasFolhaDeHorasListItemViewModel> GetAllByAjudaToList(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Where(x => x.NºFolhaDeHoras == FolhaHoraNo && x.TipoCusto == 2).Select(x => new PercursosEAjudasCustoDespesasFolhaDeHorasListItemViewModel()
                    {
                        FolhaDeHorasNo = x.NºFolhaDeHoras,
                        CostType = x.TipoCusto,
                        LineNo = x.NºLinha,
                        Description = x.Descrição,
                        Source = x.Origem,
                        Destiny = x.Destino,
                        DateTravel = x.DataViagem,
                        DateTravelText = x.DataViagem.Value.ToShortDateString(),//.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Distance = Convert.ToDecimal(x.Distância),
                        Amount = Convert.ToDecimal(x.Quantidade),
                        UnitCost = Convert.ToDecimal(x.CustoUnitário),
                        TotalCost = Convert.ToDecimal(x.CustoTotal),
                        UnitPrice = Convert.ToDecimal(x.PreçoUnitário),
                        Justification = x.Justificação,
                        Payroll = x.RúbricaSalarial,
                        DateTimeCreation = x.DataHoraCriação,
                        DateTimeCreationText = x.DataHoraCriação.Value.ToShortDateString(),//.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        UserCreation = x.UtilizadorCriação,
                        DateTimeModification = x.DataHoraModificação,
                        DateTimeModificationText = x.DataHoraModificação.Value.ToShortDateString(),//.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
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
