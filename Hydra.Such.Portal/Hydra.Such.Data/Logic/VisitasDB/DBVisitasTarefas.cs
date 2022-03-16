using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.VisitasVM;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.VisitasDB
{
    public class DBVisitasTarefas
    {
        public static List<VisitasTarefas> GetAllToList()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.VisitasTarefas.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static VisitasTarefas GetByVisita(string Visita)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.VisitasTarefas.FirstOrDefault(p => p.CodVisita == Visita);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static VisitasTarefas Create(VisitasTarefas ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.VisitasTarefas.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(VisitasTarefas ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.VisitasTarefas.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static VisitasTarefas Update(VisitasTarefas ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificacao = DateTime.Now;
                    ctx.VisitasTarefas.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static VisitasTarefas ParseToDB(VisitasTarefasViewModel x)
        {
            VisitasTarefas visita = new VisitasTarefas()
            {
                ID = x.ID,
                CodVisita = x.CodVisita,
                Ordem = x.Ordem,
                CodTarefa = x.CodTarefa,
                Tarefa = x.Tarefa,
                DataDuracao = x.DataDuracao,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataHoraCriacao = x.DataHoraCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataHoraModificacao = x.DataHoraModificacao,
            };

            if (!string.IsNullOrEmpty(x.DataHoraCriacaoTexto)) visita.DataHoraCriacao = Convert.ToDateTime(x.DataHoraCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataHoraModificacaoTexto)) visita.DataHoraModificacao = Convert.ToDateTime(x.DataHoraModificacaoTexto);

            return visita;
        }

        public static List<VisitasTarefas> ParseListToViewModel(List<VisitasTarefasViewModel> x)
        {
            List<VisitasTarefas> VisitasTarefas = new List<VisitasTarefas>();

            x.ForEach(y => VisitasTarefas.Add(ParseToDB(y)));

            return VisitasTarefas;
        }

        public static VisitasTarefasViewModel ParseToViewModel(VisitasTarefas x)
        {
            VisitasTarefasViewModel visita = new VisitasTarefasViewModel()
            {
                ID = x.ID,
                CodVisita = x.CodVisita,
                Ordem = x.Ordem,
                CodTarefa = x.CodTarefa,
                Tarefa = x.Tarefa,
                DataDuracao = x.DataDuracao,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataHoraCriacao = x.DataHoraCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataHoraModificacao = x.DataHoraModificacao,
            };

            if (x.DataHoraCriacao != null) visita.DataHoraCriacaoTexto = x.DataHoraCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataHoraModificacao != null) visita.DataHoraModificacaoTexto = x.DataHoraModificacao.Value.ToString("yyyy-MM-dd");

            return visita;
        }

        public static List<VisitasTarefasViewModel> ParseListToViewModel(List<VisitasTarefas> x)
        {
            List<VisitasTarefasViewModel> VisitasTarefas = new List<VisitasTarefasViewModel>();

            x.ForEach(y => VisitasTarefas.Add(ParseToViewModel(y)));

            return VisitasTarefas;
        }
    }
}
