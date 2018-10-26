using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hydra.Such.Data.ViewModel.CCP;

namespace Hydra.Such.Data.Logic
{
    public static class DBConfiguracaoCCP
    {
        public static ConfiguracaoCcp GetById(int id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguracaoCcp.Where(x => x.Id == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ConfiguracaoCcp> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguracaoCcp.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConfiguracaoCcp Create(ConfiguracaoCcp ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ConfiguracaoCcp.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConfiguracaoCcp Update(ConfiguracaoCcp ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ConfiguracaoCcp.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        #region Configuração Tempos CCP
        public static List<ConfiguracaoTemposCcpView> GetAllConfiguracaoTemposToView()
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                List<ConfiguraçãoTemposCcp> ConfigTemposCccp = _context.ConfiguraçãoTemposCcp.ToList();
                List<ConfiguracaoTemposCcpView> ConfigView = new List<ConfiguracaoTemposCcpView>();
                if (ConfigTemposCccp != null)
                {
                    foreach (var c in ConfigTemposCccp)
                    {
                        ConfigView.Add(CCPFunctions.CastConfigTemposToConfigTemposView(c));
                    }
                }

                return ConfigView;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public static ConfiguracaoTemposCcpView GetConfiguracaoTemposToView(int type)
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                return CCPFunctions.CastConfigTemposToConfigTemposView(_context.ConfiguraçãoTemposCcp.Where(t => t.Tipo == type).FirstOrDefault());

            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool CreateConfiguracaoTempo(ConfiguraçãoTemposCcp config)
        {
            if (config == null)
                return false;

            SuchDBContext _context = new SuchDBContext();
            try
            {
                _context.ConfiguraçãoTemposCcp.Add(config);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public static bool UpdateConfiguracaoTempo(ConfiguraçãoTemposCcp config)
        {
            if (config == null)
                return false;

            SuchDBContext _context = new SuchDBContext();
            try
            {
                _context.ConfiguraçãoTemposCcp.Update(config);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool DeleteConfiguracaoTempo(int configID)
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                _context.ConfiguraçãoTemposCcp.RemoveRange(_context.ConfiguraçãoTemposCcp.Where(c => c.Tipo == configID));
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        #endregion

    }
}
