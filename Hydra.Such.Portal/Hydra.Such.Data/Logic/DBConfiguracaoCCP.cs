using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hydra.Such.Data.ViewModel.CCP;
using System.Diagnostics;

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

        #region Configuração Tipos Procedimento
        public static TipoProcedimentoCcp GetTypeById(int id)
        {
            try
            {
                   using(var _ctx = new SuchDBContext())
                {
                    TipoProcedimentoCcp tipo   = _ctx.TipoProcedimentoCcp.Where(t => t.IdTipo == id).FirstOrDefault();
                    
                    tipo.Fundamentos = _ctx.FundamentoLegalTipoProcedimentoCcp.Where(f => f.IdTipo == id).ToList();

                    /* 
                     * /!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\
                     *  zpgm.    This loop is needed to overcome the exception:
                     *      (...)
                     *          Newtonsoft.Json.JsonSerializationException: 
                     *          Self referencing loop detected for property 'tipoNavigation' with type 'Hydra.Such.Data.Database.TipoProcedimentoCcp'. Path 'fundamentos[0]'.
                     *      (...)
                     * /!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\     
                     */
                    foreach (var f in tipo.Fundamentos)
                    {
                        f.TipoNavigation = null;
                    }
                    return tipo;
                }
            }
            catch (Exception ex)
            {
                //Debug.Write(ex.Message);
                return null;
            }
        }

        public static List<TipoProcedimentoCcp> GetAllTypes(bool onlyActives)
        {
            SuchDBContext _context = new SuchDBContext();

            try
            {
                List<TipoProcedimentoCcp> types = _context.TipoProcedimentoCcp.ToList();
                if (onlyActives)
                {
                    return types.Where(t => t.Activo == true).ToList();
                }

                return types;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool __CreateReason(FundamentoLegalTipoProcedimentoCcp fundamento)
        {
            if (fundamento == null)
                return false;

            SuchDBContext _context = new SuchDBContext();
            try
            {
                FundamentoLegalTipoProcedimentoCcp LastFundamento = _context.FundamentoLegalTipoProcedimentoCcp
                    .Where(f => f.IdTipo == fundamento.IdTipo)
                    .OrderBy(t => t.IdTipo)
                    .ThenBy(f => f.IdFundamento)
                    .LastOrDefault();

                if(LastFundamento == null)
                {
                    fundamento.IdFundamento = 1;
                }
                else
                {
                    fundamento.IdFundamento = LastFundamento.IdFundamento + 1;
                }

                _context.Add(fundamento);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        public static bool __CreateType(TipoProcedimentoCcp tipo)
        {
            SuchDBContext _context = new SuchDBContext();

            try
            {
                TipoProcedimentoCcp LastTipo = _context.TipoProcedimentoCcp.OrderBy(t => t.IdTipo).LastOrDefault();
                if (LastTipo == null)
                    tipo.IdTipo = 1;
                else
                    tipo.IdTipo = LastTipo.IdTipo + 1;

                _context.TipoProcedimentoCcp.Add(tipo);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        #endregion

    }
}
