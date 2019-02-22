using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Extensions;

namespace Hydra.Such.Data.Logic
{
    public static class DBConfigVendasAlertas
    {
        #region CRUD
        public static ConfiguraçãoVendasAlertas GetByNo(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguraçãoVendasAlertas.Where(x => x.Id == ID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConfiguraçãoVendasAlertas Create(ConfiguraçãoVendasAlertas ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.ConfiguraçãoVendasAlertas.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConfiguraçãoVendasAlertas Update(ConfiguraçãoVendasAlertas ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificacao = DateTime.Now;
                    ctx.ConfiguraçãoVendasAlertas.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        #endregion

        public static ConfiguraçãoVendasAlertas ParseToDB(ConfiguracaoVendasAlertasViewModel x)
        {
            if (x == null)
                return null;

            ConfiguraçãoVendasAlertas result = new ConfiguraçãoVendasAlertas()
            {
                Id = x.ID,
                Email1Regiao12 = x.Email1Regiao12,
                Email2Regiao12 = x.Email2Regiao12,
                Email3Regiao12 = x.Email3Regiao12,
                Email1Regiao23 = x.Email1Regiao23,
                Email2Regiao23 = x.Email2Regiao23,
                Email3Regiao23 = x.Email3Regiao23,
                Email1Regiao33 = x.Email1Regiao33,
                Email2Regiao33 = x.Email2Regiao33,
                Email3Regiao33 = x.Email3Regiao33,
                Email1Regiao43 = x.Email1Regiao43,
                Email2Regiao43 = x.Email2Regiao43,
                Email3Regiao43 = x.Email3Regiao43,
                DiasParaEnvioAlerta = x.DiasParaEnvioAlerta,
                DataHoraCriacao = x.DataHoraCriacao,
                DataHoraModificacao = x.DataHoraModificacao,
                UtilizadorCriacao = x.UtilizadorCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
            };

            return result;
        }

        public static ConfiguracaoVendasAlertasViewModel ParseToViewModel(ConfiguraçãoVendasAlertas x)
        {
            if (x == null)
                return null;

            ConfiguracaoVendasAlertasViewModel result = new ConfiguracaoVendasAlertasViewModel()
            {
                ID = x.Id,
                Email1Regiao12 = x.Email1Regiao12,
                Email2Regiao12 = x.Email2Regiao12,
                Email3Regiao12 = x.Email3Regiao12,
                Email1Regiao23 = x.Email1Regiao23,
                Email2Regiao23 = x.Email2Regiao23,
                Email3Regiao23 = x.Email3Regiao23,
                Email1Regiao33 = x.Email1Regiao33,
                Email2Regiao33 = x.Email2Regiao33,
                Email3Regiao33 = x.Email3Regiao33,
                Email1Regiao43 = x.Email1Regiao43,
                Email2Regiao43 = x.Email2Regiao43,
                Email3Regiao43 = x.Email3Regiao43,
                DiasParaEnvioAlerta = x.DiasParaEnvioAlerta,
                DataHoraCriacao = x.DataHoraCriacao,
                DataHoraModificacao = x.DataHoraModificacao,
                UtilizadorCriacao = x.UtilizadorCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
            };

            return result;
        }
    }
}
