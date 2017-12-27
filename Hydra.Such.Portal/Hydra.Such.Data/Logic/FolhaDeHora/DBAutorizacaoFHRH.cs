using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBAutorizacaoFHRH
    {
        public static List<AutorizacaoFhRh> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AutorizacaoFhRh.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static AutorizacaoFhRh Create(AutorizacaoFhRh ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.AutorizacaoFhRh.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static AutorizacaoFhRh Update(AutorizacaoFhRh ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraÚltimaAlteração = DateTime.Now;
                    ctx.AutorizacaoFhRh.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(AutorizacaoFhRh ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.AutorizacaoFhRh.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static AutorizacaoFhRh ParseToDB(AutorizacaoFHRHViewModel x)
        {
            try
            {
                return new AutorizacaoFhRh()
                {
                    NoEmpregado = x.NoEmpregado,
                    NoResponsavel1 = x.NoResponsavel1,
                    NoResponsavel2 = x.NoResponsavel2,
                    NoResponsavel3 = x.NoResponsavel3,
                    ValidadorRh1 = x.ValidadorRH1,
                    ValidadorRh2 = x.ValidadorRH2,
                    ValidadorRh3 = x.ValidadorRH3,
                    ValidadorRhkm1 = x.ValidadorRHKM1,
                    ValidadorRhkm2 = x.ValidadorRHKM2,
                    CriadoPor = x.UtilizadorCriacao,
                    DataHoraCriação = x.DataHoraCriacao,
                    AlteradoPor = x.UtilizadorModificacao,
                    DataHoraÚltimaAlteração = x.DataHoraModificacao
                };
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static AutorizacaoFHRHViewModel ParseToViewModel(AutorizacaoFhRh x)
        {
            return new AutorizacaoFHRHViewModel()
            {
                NoEmpregado = x.NoEmpregado,
                NoResponsavel1 = x.NoResponsavel1,
                NoResponsavel2 = x.NoResponsavel2,
                NoResponsavel3 = x.NoResponsavel3,
                ValidadorRH1 = x.ValidadorRh1,
                ValidadorRH2 = x.ValidadorRh2,
                ValidadorRH3 = x.ValidadorRh3,
                ValidadorRHKM1 = x.ValidadorRhkm1,
                ValidadorRHKM2 = x.ValidadorRhkm2,
                UtilizadorCriacao = x.CriadoPor,
                DataHoraCriacao = x.DataHoraCriação,
                UtilizadorModificacao = x.AlteradoPor,
                DataHoraModificacao = x.DataHoraÚltimaAlteração
            };
        }

        public static List<AutorizacaoFHRHViewModel> ParseListToViewModel(List<AutorizacaoFhRh> x)
        {
            List<AutorizacaoFHRHViewModel> result = new List<AutorizacaoFHRHViewModel>();

            x.ForEach(y => result.Add(ParseToViewModel(y)));
            return result;
        }

    }
}