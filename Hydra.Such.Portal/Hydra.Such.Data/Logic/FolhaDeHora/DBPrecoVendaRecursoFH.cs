using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBPrecoVendaRecursoFH
    {
        public static List<PrecoVendaRecursoFh> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PrecoVendaRecursoFh.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static PrecoVendaRecursoFh Create(PrecoVendaRecursoFh ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.PrecoVendaRecursoFh.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static PrecoVendaRecursoFh Update(PrecoVendaRecursoFh ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraUltimaAlteracao = DateTime.Now;
                    ctx.PrecoVendaRecursoFh.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(PrecoVendaRecursoFh ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PrecoVendaRecursoFh.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static PrecoVendaRecursoFh ParseToDB(PrecoVendaRecursoFHViewModel x)
        {

            return new PrecoVendaRecursoFh()
            {
                Code = x.Code,
                Descricao = x.Descricao,
                CodTipoTrabalho = x.CodTipoTrabalho,
                PrecoUnitario = x.PrecoUnitario,
                CustoUnitario = x.CustoUnitario,
                StartingDate = (DateTime)x.StartingDate,
                EndingDate = x.EndingDate,
                FamiliaRecurso = x.FamiliaRecurso,
                CriadoPor = x.UtilizadorCriacao,
                DataHoraCriacao = x.DataHoraCriacao,
                AlteradoPor = x.UtilizadorModificacao,
                DataHoraUltimaAlteracao = x.DataHoraModificacao
            };
        }

        public static PrecoVendaRecursoFHViewModel ParseToViewModel(PrecoVendaRecursoFh x)
        {
            return new PrecoVendaRecursoFHViewModel()
            {
                Code = x.Code,
                Descricao = x.Descricao,
                CodTipoTrabalho = x.CodTipoTrabalho,
                PrecoUnitario = x.PrecoUnitario,
                CustoUnitario = x.CustoUnitario,
                StartingDate = x.StartingDate,
                StartingDateTexto = x.StartingDate.ToString("yyyy-MM-dd"),
                EndingDate = x.EndingDate,
                EndingDateTexto = x.EndingDate.Value.ToString("yyyy-MM-dd"),
                FamiliaRecurso = x.FamiliaRecurso,
                UtilizadorCriacao = x.CriadoPor,
                DataHoraCriacao = x.DataHoraCriacao,
                UtilizadorModificacao = x.AlteradoPor,
                DataHoraModificacao = x.DataHoraUltimaAlteracao
            };
        }

        public static List<PrecoVendaRecursoFHViewModel> ParseListToViewModel(List<PrecoVendaRecursoFh> x)
        {
            List<PrecoVendaRecursoFHViewModel> result = new List<PrecoVendaRecursoFHViewModel>();

            x.ForEach(y => result.Add(ParseToViewModel(y)));
            return result;
        }
    }
}