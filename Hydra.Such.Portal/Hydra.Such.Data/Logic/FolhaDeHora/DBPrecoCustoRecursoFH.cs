using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBPrecoCustoRecursoFH
    {
        public static List<PrecoCustoRecursoFh> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PrecoCustoRecursoFh.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static PrecoCustoRecursoFh Create(PrecoCustoRecursoFh ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.PrecoCustoRecursoFh.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static PrecoCustoRecursoFh Update(PrecoCustoRecursoFh ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraUltimaAlteracao = DateTime.Now;
                    ctx.PrecoCustoRecursoFh.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(PrecoCustoRecursoFh ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PrecoCustoRecursoFh.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static PrecoCustoRecursoFh ParseToDB(PrecoCustoRecursoViewModel x)
        {
            return new PrecoCustoRecursoFh()
            {
                Code = x.Code,
                Descricao = x.Descricao,
                CodTipoTrabalho = x.CodTipoTrabalho,
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

        public static PrecoCustoRecursoViewModel ParseToViewModel(PrecoCustoRecursoFh x)
        {
            return new PrecoCustoRecursoViewModel()
            {
                Code = x.Code,
                Descricao = x.Descricao,
                CodTipoTrabalho = x.CodTipoTrabalho,
                CustoUnitario = x.CustoUnitario,
                StartingDate = x.StartingDate,
                EndingDate = x.EndingDate,
                FamiliaRecurso = x.FamiliaRecurso,
                UtilizadorCriacao = x.CriadoPor,
                DataHoraCriacao = x.DataHoraCriacao,
                UtilizadorModificacao = x.AlteradoPor,
                DataHoraModificacao = x.DataHoraUltimaAlteracao
            };
        }

        public static List<PrecoCustoRecursoViewModel> ParseListToViewModel(List<PrecoCustoRecursoFh> x)
        {
            List<PrecoCustoRecursoViewModel> result = new List<PrecoCustoRecursoViewModel>();

            x.ForEach(y => result.Add(ParseToViewModel(y)));
            return result;
        }
    }
}