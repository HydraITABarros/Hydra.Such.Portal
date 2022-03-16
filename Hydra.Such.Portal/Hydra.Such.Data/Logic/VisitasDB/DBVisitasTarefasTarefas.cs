using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.VisitasVM;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.VisitasDB
{
    public class DBVisitasTarefasTarefas
    {
        public static List<VisitasTarefasTarefas> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.VisitasTarefasTarefas.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static VisitasTarefasTarefas GetByTarefa(int Tarefa)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.VisitasTarefasTarefas.FirstOrDefault(p => p.CodTarefa == Tarefa);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static VisitasTarefasTarefas Create(VisitasTarefasTarefas ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.VisitasTarefasTarefas.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(VisitasTarefasTarefas ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.VisitasTarefasTarefas.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static VisitasTarefasTarefas Update(VisitasTarefasTarefas ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificacao = DateTime.Now;
                    ctx.VisitasTarefasTarefas.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static VisitasTarefasTarefas ParseToDB(VisitasTarefasTarefasViewModel x)
        {
            VisitasTarefasTarefas visita = new VisitasTarefasTarefas()
            {
                ID = x.ID,
                CodTarefa = x.CodTarefa,
                Tarefa = x.Tarefa,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataHoraCriacao = x.DataHoraCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataHoraModificacao = x.DataHoraModificacao,
            };

            if (!string.IsNullOrEmpty(x.DataHoraCriacaoTexto)) visita.DataHoraCriacao = Convert.ToDateTime(x.DataHoraCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataHoraModificacaoTexto)) visita.DataHoraModificacao = Convert.ToDateTime(x.DataHoraModificacaoTexto);

            return visita;
        }

        public static List<VisitasTarefasTarefas> ParseListToViewModel(List<VisitasTarefasTarefasViewModel> x)
        {
            List<VisitasTarefasTarefas> VisitasTarefasTarefas = new List<VisitasTarefasTarefas>();

            x.ForEach(y => VisitasTarefasTarefas.Add(ParseToDB(y)));

            return VisitasTarefasTarefas;
        }

        public static VisitasTarefasTarefasViewModel ParseToViewModel(VisitasTarefasTarefas x)
        {
            VisitasTarefasTarefasViewModel visita = new VisitasTarefasTarefasViewModel()
            {
                ID = x.ID,
                CodTarefa = x.CodTarefa,
                Tarefa = x.Tarefa,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataHoraCriacao = x.DataHoraCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataHoraModificacao = x.DataHoraModificacao,
            };

            if (x.DataHoraCriacao != null) visita.DataHoraCriacaoTexto = x.DataHoraCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataHoraModificacao != null) visita.DataHoraModificacaoTexto = x.DataHoraModificacao.Value.ToString("yyyy-MM-dd");

            return visita;
        }

        public static List<VisitasTarefasTarefasViewModel> ParseListToViewModel(List<VisitasTarefasTarefas> x)
        {
            List<VisitasTarefasTarefasViewModel> VisitasTarefasTarefas = new List<VisitasTarefasTarefasViewModel>();

            x.ForEach(y => VisitasTarefasTarefas.Add(ParseToViewModel(y)));

            return VisitasTarefasTarefas;
        }
    }
}
