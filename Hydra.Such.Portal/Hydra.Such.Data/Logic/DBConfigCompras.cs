using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Extensions;

namespace Hydra.Such.Data.Logic
{
    public static class DBConfigCompras
    {
        #region CRUD
        public static ConfiguraçãoCompras GetByNo(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguracaoCompras.Where(x => x.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConfiguraçãoCompras Create(ConfiguraçãoCompras ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.ConfiguracaoCompras.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConfiguraçãoCompras Update(ConfiguraçãoCompras ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificacao = DateTime.Now;
                    ctx.ConfiguracaoCompras.Update(ObjectToUpdate);
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

        public static ConfiguraçãoCompras ParseToDB(ConfiguracaoComprasViewModel x)
        {
            if (x == null)
                return null;

            ConfiguraçãoCompras result = new ConfiguraçãoCompras()
            {
                ID = x.ID,
                Email1Regiao12 = x.Email1Regiao12,
                Email2Regiao12 = x.Email2Regiao12,
                Email1Regiao23 = x.Email1Regiao23,
                Email2Regiao23 = x.Email2Regiao23,
                Email1Regiao33 = x.Email1Regiao33,
                Email2Regiao33 = x.Email2Regiao33,
                Email1Regiao43 = x.Email1Regiao43,
                Email2Regiao43 = x.Email2Regiao43,
                DiasParaEnvioAlerta = x.DiasParaEnvioAlerta,
                DataHoraCriacao = x.DataHoraCriacao,
                DataHoraModificacao = x.DataHoraModificacao,
                UtilizadorCriacao = x.UtilizadorCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
            };

            return result;
        }

        public static ConfiguracaoComprasViewModel ParseToViewModel(ConfiguraçãoCompras x)
        {
            if (x == null)
                return null;

            ConfiguracaoComprasViewModel result = new ConfiguracaoComprasViewModel()
            {
                ID = x.ID,
                Email1Regiao12 = x.Email1Regiao12,
                Email2Regiao12 = x.Email2Regiao12,
                Email1Regiao23 = x.Email1Regiao23,
                Email2Regiao23 = x.Email2Regiao23,
                Email1Regiao33 = x.Email1Regiao33,
                Email2Regiao33 = x.Email2Regiao33,
                Email1Regiao43 = x.Email1Regiao43,
                Email2Regiao43 = x.Email2Regiao43,
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
