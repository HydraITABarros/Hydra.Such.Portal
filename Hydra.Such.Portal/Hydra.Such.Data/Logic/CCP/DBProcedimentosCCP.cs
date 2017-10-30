using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.CCP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.CCP
{
    public static class DBProcedimentosCCP
    {

        #region parse ProcedimentosCcp
        public static List<ProcedimentosCcp> GetAllProcedimentosByProcedimentoTypeToList(int type)
        {
            SuchDBContext context = new SuchDBContext();
            try
            {
                return context.ProcedimentosCcp.Where(p => p.TipoProcedimento == type).ToList();
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public static List<ProcedimentosCcp> GetAllProcedimentosToList()
        {
            SuchDBContext context = new SuchDBContext();
            try
            {
                return context.ProcedimentosCcp.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ProcedimentosCcp GetProcedimentoById(string proc_id)
        {
            var context = new SuchDBContext();

            try
            {
                return context.ProcedimentosCcp.Where(p => p.Nº == proc_id).FirstOrDefault();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        #endregion
        #region Create and Updates
        // zpgm - two overloaded methods to create ProcedimentosCcp: 
        //      the first uses a ProcedimentoCCPView object and returns aProcedimentosCcp object
        //      the second uses a ProcedimentosCcp object and returns an object of the same type
        public static ProcedimentosCcp __Create(ProcedimentoCCPView procedimento)
        {
            SuchDBContext context = new SuchDBContext();
            ProcedimentosCcp proc = CCPFunctions.CastProcCcpViewToProcCcp(procedimento);

            try
            {
                Configuração config = DBConfigurations.GetById(1);
                int NumeracaoProcedimento = 0;

                if(proc.TipoProcedimento == 1)
                {
                    NumeracaoProcedimento = config.NumeraçãoProcedimentoAquisição.Value;
                }
                else
                {
                    NumeracaoProcedimento = config.NumeraçãoProcedimentoSimplificado.Value;
                }

                proc.Nº = DBNumerationConfigurations.GetNextNumeration(NumeracaoProcedimento, true);
                proc.DataHoraCriação = DateTime.Now;

                proc.Nº1 = new TemposPaCcp()
                {
                    NºProcedimento = proc.Nº,
                    Estado0 = 1,
                    DataHoraCriação = proc.DataHoraCriação,
                    UtilizadorCriação =  proc.UtilizadorCriação
                };

                proc.NºNavigation = new RegistoDeAtas()
                {
                    // preencher o nº de acta
                    // NºAta
                    NºProcedimento = proc.Nº,
                    DataHoraCriação = proc.DataHoraCriação,
                    UtilizadorCriação = proc.UtilizadorCriação
                };

                context.Add(proc.Nº1);
                context.SaveChanges();

                context.Add(proc.NºNavigation);
                context.SaveChanges();
                
                context.Add(proc);
                context.SaveChanges();

                ConfiguraçãoNumerações ConfigNum = DBNumerationConfigurations.GetById(NumeracaoProcedimento);
                ConfigNum.ÚltimoNºUsado = proc.Nº;
                DBNumerationConfigurations.Update(ConfigNum);

                return proc;
            }
            catch(Exception e)
            {
                return null;
            }

        }

        public static ProcedimentosCcp __Create(ProcedimentosCcp procedimento)
        {
            try
            {
                ProcedimentoCCPView ProcCCPView = CCPFunctions.CastProcCcpToProcCcpView(procedimento);
                ProcedimentosCcp Proc =  __Create(ProcCCPView);

                procedimento.Nº = Proc.Nº;
                procedimento.DataHoraCriação = Proc.DataHoraCriação;
                procedimento.UtilizadorCriação = Proc.UtilizadorCriação;

                return procedimento;
            }
            catch(Exception e)
            {
                return null;
            }
            
        }

        public static ProcedimentosCcp __Update(ProcedimentoCCPView procedimento)
        {
            SuchDBContext context = new SuchDBContext();
            try
            {
                ProcedimentosCcp proc = CCPFunctions.CastProcCcpViewToProcCcp(procedimento);
                proc.DataHoraModificação = DateTime.Now;

                context.ProcedimentosCcp.Update(proc);
                context.SaveChanges();

                return proc;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public static bool __Delete(string procedimentoNo)
        {
            SuchDBContext context = new SuchDBContext();

            try
            {
                context.ProcedimentosCcp.RemoveRange(context.ProcedimentosCcp.Where(p => p.Nº == procedimentoNo));
                context.SaveChanges();

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        #endregion

        #region parse ProcedimentosCCPView
        public static List<ProcedimentoCCPView> GetAllProcedimentosViewByProcedimentoTypeToList(int type)
        {
            List <ProcedimentosCcp> ProcList = GetAllProcedimentosByProcedimentoTypeToList(type);
            List<ProcedimentoCCPView> ProcViewList = new List<ProcedimentoCCPView>();

            if (ProcList == null)
                return null;

            try
            {
                foreach (var x in ProcList)
                {
                    ProcViewList.Add(CCPFunctions.CastProcCcpToProcCcpView(x));
                }

                return ProcViewList;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public static List<ProcedimentoCCPView> GetAllProcedimentosByViewToList()
        {
            List<ProcedimentosCcp> ProcList = GetAllProcedimentosToList();
            List<ProcedimentoCCPView> ProcViewList = new List<ProcedimentoCCPView>();

            if (ProcList == null)
                return null;

            try
            {
                foreach (var x in ProcList)
                {
                    ProcViewList.Add(CCPFunctions.CastProcCcpToProcCcpView(x));
                }

                return ProcViewList;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ProcedimentoCCPView GetProcedimentoCCPViewById(string id)
        {
            return CCPFunctions.CastProcCcpToProcCcpView(GetProcedimentoById(id));
        }
        #endregion
    }
}
