using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.VisitasVM;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.VisitasDB
{
    public class DBVisitasContratos
    {
        public static List<VisitasContratos> GetAllToList()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.VisitasContratos.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static VisitasContratos GetByID(string Visita, string NoContrato)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.VisitasContratos.FirstOrDefault(p => p.CodVisita == Visita && p.NoContrato == NoContrato);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<VisitasContratos> GetByVisita(string Visita)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.VisitasContratos.Where(p => p.CodVisita == Visita).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static VisitasContratos Create(VisitasContratos ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.VisitasContratos.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(VisitasContratos ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.VisitasContratos.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static VisitasContratos Update(VisitasContratos ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificacao = DateTime.Now;
                    ctx.VisitasContratos.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static VisitasContratos ParseToDB(VisitasContratosViewModel x)
        {
            VisitasContratos visita = new VisitasContratos()
            {
                CodVisita = x.CodVisita,
                NoContrato = x.NoContrato,
                AmbitoServico = x.AmbitoServico,
                NoCliente = x.NoCliente,
                NomeCliente = x.NomeCliente,
                CodArea = x.CodArea,
                NomeArea = x.NomeArea,
                CodCresp = x.CodCresp,
                NomeCresp = x.NomeCresp,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataHoraCriacao = x.DataHoraCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataHoraModificacao = x.DataHoraModificacao,
            };

            if (!string.IsNullOrEmpty(x.DataHoraCriacaoTexto)) visita.DataHoraCriacao = Convert.ToDateTime(x.DataHoraCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataHoraModificacaoTexto)) visita.DataHoraModificacao = Convert.ToDateTime(x.DataHoraModificacaoTexto);

            return visita;
        }

        public static List<VisitasContratos> ParseListToViewModel(List<VisitasContratosViewModel> x)
        {
            List<VisitasContratos> VisitasContratos = new List<VisitasContratos>();

            x.ForEach(y => VisitasContratos.Add(ParseToDB(y)));

            return VisitasContratos;
        }

        public static VisitasContratosViewModel ParseToViewModel(VisitasContratos x)
        {
            VisitasContratosViewModel visita = new VisitasContratosViewModel()
            {
                CodVisita = x.CodVisita,
                NoContrato = x.NoContrato,
                AmbitoServico = x.AmbitoServico,
                NoCliente = x.NoCliente,
                NomeCliente = x.NomeCliente,
                CodArea = x.CodArea,
                NomeArea = x.NomeArea,
                CodCresp = x.CodCresp,
                NomeCresp = x.NomeCresp,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataHoraCriacao = x.DataHoraCriacao,
                DataHoraCriacaoTexto = x.DataHoraCriacao.HasValue ? x.DataHoraCriacao.Value.ToString("yyyy-MM-dd") : "",
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataHoraModificacao = x.DataHoraModificacao,
                DataHoraModificacaoTexto = x.DataHoraModificacao.HasValue ? x.DataHoraModificacao.Value.ToString("yyyy-MM-dd") : "",
            };

            return visita;
        }

        public static List<VisitasContratosViewModel> ParseListToViewModel(List<VisitasContratos> x)
        {
            List<VisitasContratosViewModel> VisitasContratos = new List<VisitasContratosViewModel>();

            x.ForEach(y => VisitasContratos.Add(ParseToViewModel(y)));

            return VisitasContratos;
        }
    }
}
