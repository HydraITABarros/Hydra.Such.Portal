using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBTabelaConfRecursosFH
    {

        public static List<TabelaConfRecursosFH> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TabelaConfRecursosFH.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static TabelaConfRecursosFH Create(TabelaConfRecursosFH ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.TabelaConfRecursosFH.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static TabelaConfRecursosFH Update(TabelaConfRecursosFH ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //ObjectToUpdate.DataHoraModificacao = DateTime.Now;
                    ctx.TabelaConfRecursosFH.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(TabelaConfRecursosFH ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.TabelaConfRecursosFH.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static TabelaConfRecursosFH ParseToDB(TabelaConfRecursosFHViewModel x)
        {

            return new TabelaConfRecursosFH()
            {
                Tipo = x.Tipo.ToString(),
                CodRecurso = x.CodigoRecurso,
                Descricao = x.Descricao,
                PrecoUnitarioCusto = x.PrecoUnitarioCusto,
                PrecoUnitarioVenda = x.PrecoUnitarioVenda,
                UnidMedida = x.UnidMedida,
                RubricaSalarial = x.RubricaSalarial
            };
        }

        public static TabelaConfRecursosFHViewModel ParseToViewModel(TabelaConfRecursosFH x)
        {
            return new TabelaConfRecursosFHViewModel()
            {
                Tipo = Int32.Parse(x.Tipo),
                CodigoRecurso = x.CodRecurso,
                Descricao = x.Descricao,
                PrecoUnitarioCusto = x.PrecoUnitarioCusto,
                PrecoUnitarioVenda = x.PrecoUnitarioVenda,
                UnidMedida = x.UnidMedida,
                RubricaSalarial = x.RubricaSalarial
            };
        }

        public static List<TabelaConfRecursosFHViewModel> ParseListToViewModel(List<TabelaConfRecursosFH> x)
        {
            List<TabelaConfRecursosFHViewModel> result = new List<TabelaConfRecursosFHViewModel>();

            x.ForEach(y => result.Add(ParseToViewModel(y)));
            return result;
        }

        public static decimal GetPrecoUnitarioCusto(string Tipo, string CodRecurso)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    TabelaConfRecursosFH recurso;

                    recurso = ctx.TabelaConfRecursosFH.FirstOrDefault(x => x.Tipo == Tipo && x.CodRecurso == CodRecurso);

                    if (recurso == null)
                        return 0;
                    else
                        return Convert.ToDecimal(recurso.PrecoUnitarioCusto);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}