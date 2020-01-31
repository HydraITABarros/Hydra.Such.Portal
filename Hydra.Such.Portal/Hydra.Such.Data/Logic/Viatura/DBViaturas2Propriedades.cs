using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2Propriedades
    {
        public static List<Viaturas2Propriedades> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Propriedades.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Propriedades GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Propriedades.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2Propriedades> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Propriedades.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Propriedades GetByMatriculaRecent(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Propriedades.Where(p => p.Matricula == Matricula).OrderByDescending(x => x.DataInicio).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Propriedades Create(Viaturas2Propriedades ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2Propriedades.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2Propriedades ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2Propriedades.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2Propriedades Update(Viaturas2Propriedades ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2Propriedades.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2Propriedades ParseToDB(Viaturas2PropriedadesViewModel x)
        {
            Viaturas2Propriedades Parqueamento = new Viaturas2Propriedades()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDTipoPropriedade = x.IDTipoPropriedade,
                IDPropriedade = x.IDPropriedade,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (!string.IsNullOrEmpty(x.DataInicioTexto)) Parqueamento.DataInicio = Convert.ToDateTime(x.DataInicioTexto);
            if (!string.IsNullOrEmpty(x.DataFimTexto)) Parqueamento.DataFim = Convert.ToDateTime(x.DataFimTexto);
            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) Parqueamento.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) Parqueamento.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return Parqueamento;
        }

        public static List<Viaturas2Propriedades> ParseListToViewModel(List<Viaturas2PropriedadesViewModel> x)
        {
            List<Viaturas2Propriedades> Viaturas2Propriedades = new List<Viaturas2Propriedades>();

            x.ForEach(y => Viaturas2Propriedades.Add(ParseToDB(y)));

            return Viaturas2Propriedades;
        }

        public static Viaturas2PropriedadesViewModel ParseToViewModel(Viaturas2Propriedades x)
        {
            Viaturas2PropriedadesViewModel Parqueamento = new Viaturas2PropriedadesViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDTipoPropriedade = x.IDTipoPropriedade,
                IDPropriedade = x.IDPropriedade,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.DataInicio != null) Parqueamento.DataInicioTexto = x.DataInicio.Value.ToString("yyyy-MM-dd");
            if (x.DataFim != null) Parqueamento.DataFimTexto = x.DataFim.Value.ToString("yyyy-MM-dd");
            if (x.DataCriacao != null) Parqueamento.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) Parqueamento.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return Parqueamento;
        }

        public static List<Viaturas2PropriedadesViewModel> ParseListToViewModel(List<Viaturas2Propriedades> x)
        {
            List<Viaturas2PropriedadesViewModel> Viaturas2Propriedades = new List<Viaturas2PropriedadesViewModel>();

            x.ForEach(y => Viaturas2Propriedades.Add(ParseToViewModel(y)));

            return Viaturas2Propriedades;
        }
    }
}
