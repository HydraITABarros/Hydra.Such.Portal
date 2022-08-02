﻿using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBRHRecursosFH
    {
        public static List<RhRecursosFh> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RhRecursosFh.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static RhRecursosFh GetByID(string NoEmpregado, string Recurso)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RhRecursosFh.FirstOrDefault(x => x.NoEmpregado == NoEmpregado && x.Recurso == Recurso);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static RhRecursosFh Create(RhRecursosFh ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.RhRecursosFh.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static RhRecursosFh Update(RhRecursosFh ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraUltimaAlteracao = DateTime.Now;
                    ctx.RhRecursosFh.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(RhRecursosFh ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.RhRecursosFh.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static RhRecursosFh ParseToDB(RHRecursosViewModel x)
        {
            return new RhRecursosFh()
            {
                NoEmpregado = x.NoEmpregado,
                Recurso = x.Recurso,
                NomeRecurso = x.NomeRecurso,
                FamiliaRecurso = x.FamiliaRecurso,
                NomeEmpregado = x.NomeEmpregado,
                CriadoPor = x.UtilizadorCriacao,
                DataHoraCriacao = x.DataHoraCriacao,
                AlteradoPor = x.UtilizadorModificacao,
                DataHoraUltimaAlteracao = x.DataHoraModificacao
            };
        }

        public static RHRecursosViewModel ParseToViewModel(RhRecursosFh x)
        {
            return new RHRecursosViewModel()
            {
                NoEmpregado = x.NoEmpregado,
                Recurso = x.Recurso,
                NomeRecurso = x.NomeRecurso,
                FamiliaRecurso = x.FamiliaRecurso,
                NomeEmpregado = x.NomeEmpregado,
                UtilizadorCriacao = x.CriadoPor,
                DataHoraCriacao = x.DataHoraCriacao,
                UtilizadorModificacao = x.AlteradoPor,
                DataHoraModificacao = x.DataHoraUltimaAlteracao
            };
        }

        public static List<RHRecursosViewModel> ParseListToViewModel(List<RhRecursosFh> x)
        {
            List<RHRecursosViewModel> result = new List<RHRecursosViewModel>();

            x.ForEach(y => result.Add(ParseToViewModel(y)));
            return result;
        }
    }
}