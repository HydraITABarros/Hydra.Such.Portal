using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.VisitasVM;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.VisitasDB
{
    public class DBVisitasEstados
    {
        public static List<VisitasEstados> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.VisitasEstados.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static VisitasEstados GetByEstado(int Estado)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.VisitasEstados.FirstOrDefault(p => p.CodEstado == Estado);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static VisitasEstados Create(VisitasEstados ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.VisitasEstados.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(VisitasEstados ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.VisitasEstados.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static VisitasEstados Update(VisitasEstados ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificacao = DateTime.Now;
                    ctx.VisitasEstados.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static VisitasEstados ParseToDB(VisitasEstadosViewModel x)
        {
            VisitasEstados visita = new VisitasEstados()
            {
                ID = x.ID,
                CodEstado = x.CodEstado,
                Estado = x.Estado,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataHoraCriacao = x.DataHoraCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataHoraModificacao = x.DataHoraModificacao,
            };

            if (!string.IsNullOrEmpty(x.DataHoraCriacaoTexto)) visita.DataHoraCriacao = Convert.ToDateTime(x.DataHoraCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataHoraModificacaoTexto)) visita.DataHoraModificacao = Convert.ToDateTime(x.DataHoraModificacaoTexto);

            return visita;
        }

        public static List<VisitasEstados> ParseListToViewModel(List<VisitasEstadosViewModel> x)
        {
            List<VisitasEstados> VisitasEstados = new List<VisitasEstados>();

            x.ForEach(y => VisitasEstados.Add(ParseToDB(y)));

            return VisitasEstados;
        }

        public static VisitasEstadosViewModel ParseToViewModel(VisitasEstados x)
        {
            VisitasEstadosViewModel visita = new VisitasEstadosViewModel()
            {
                ID = x.ID,
                CodEstado = x.CodEstado,
                Estado = x.Estado,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataHoraCriacao = x.DataHoraCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataHoraModificacao = x.DataHoraModificacao,
            };

            if (x.DataHoraCriacao != null) visita.DataHoraCriacaoTexto = x.DataHoraCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataHoraModificacao != null) visita.DataHoraModificacaoTexto = x.DataHoraModificacao.Value.ToString("yyyy-MM-dd");

            return visita;
        }

        public static List<VisitasEstadosViewModel> ParseListToViewModel(List<VisitasEstados> x)
        {
            List<VisitasEstadosViewModel> VisitasEstados = new List<VisitasEstadosViewModel>();

            x.ForEach(y => VisitasEstados.Add(ParseToViewModel(y)));

            return VisitasEstados;
        }
    }
}
