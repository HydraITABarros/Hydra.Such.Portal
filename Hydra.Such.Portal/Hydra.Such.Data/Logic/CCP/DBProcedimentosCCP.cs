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

        public static ProcedimentosCcp GetProcedimentoById(string ProcedimentoID)
        {
            var context = new SuchDBContext();

            try
            {
                return context.ProcedimentosCcp.Where(p => p.Nº == ProcedimentoID).FirstOrDefault();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        #endregion


        #region Create, Update and Delete Procedimentos
        // zpgm - two overloaded methods to create ProcedimentosCcp: 
        //      the first uses a ProcedimentoCCPView object and returns a ProcedimentosCcp object
        //      the second uses a ProcedimentosCcp object and returns an object of the same type
        public static ProcedimentosCcp __CreateProcedimento(ProcedimentoCCPView Procedimento)
        {
            SuchDBContext context = new SuchDBContext();
            ProcedimentosCcp proc = CCPFunctions.CastProcCcpViewToProcCcp(Procedimento);

            try
            {
                Configuração config = DBConfigurations.GetById(1);
                int NumeracaoProcedimento = 0;

                if (proc.TipoProcedimento == 1)
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
                    UtilizadorCriação = proc.UtilizadorCriação
                };

                proc.NºNavigation = __CreateRegistoDeAtas(proc);

                context.Add(proc.NºNavigation);
                context.SaveChanges();

                context.Add(proc.Nº1);
                context.SaveChanges();

                context.Add(proc);
                context.SaveChanges();

                ConfiguraçãoNumerações ConfigNum = DBNumerationConfigurations.GetById(NumeracaoProcedimento);
                ConfigNum.ÚltimoNºUsado = proc.Nº;
                DBNumerationConfigurations.Update(ConfigNum);

                return proc;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public static ProcedimentosCcp __CreateProcedimento(ProcedimentosCcp Procedimento)
        {
            try
            {
                ProcedimentoCCPView ProcCCPView = CCPFunctions.CastProcCcpToProcCcpView(Procedimento);
                ProcedimentosCcp Proc = __CreateProcedimento(ProcCCPView);

                Procedimento.Nº = Proc.Nº;
                Procedimento.DataHoraCriação = Proc.DataHoraCriação;
                Procedimento.UtilizadorCriação = Proc.UtilizadorCriação;

                return Procedimento;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public static ProcedimentosCcp __UpdateProcedimento(ProcedimentoCCPView Procedimento)
        {
            SuchDBContext context = new SuchDBContext();
            try
            {
                ProcedimentosCcp proc = CCPFunctions.CastProcCcpViewToProcCcp(Procedimento);
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

        public static bool __DeleteProcedimento(string ProcedimentoID)
        {
            SuchDBContext context = new SuchDBContext();

            try
            {
                context.ProcedimentosCcp.RemoveRange(context.ProcedimentosCcp.Where(p => p.Nº == ProcedimentoID));
                context.SaveChanges();

                if (!__DeleteAllRegistoDeAtasRelatedToProcedimento(ProcedimentoID))
                    return false;

                if (!__DeleteTemposPaCcp(ProcedimentoID))
                    return false;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion

        #region Create, Update and Delete RegistoDeActas
        public static string GetActaNumber(string ProcedimentoID)
        {
            SuchDBContext context = new SuchDBContext();

            int num = context.RegistoDeAtas.Where(a => a.NºProcedimento == ProcedimentoID).Count();

            num += 1;

            return num.ToString().PadLeft(4, '0');
        }
        public static RegistoDeAtas __CreateRegistoDeAtas(ProcedimentosCcp Procedimento)
        {
            //SuchDBContext context = new SuchDBContext();
            try
            {
                RegistoDeAtas Acta = new RegistoDeAtas()
                {
                    NºProcedimento = Procedimento.Nº,
                    NºAta = GetActaNumber(Procedimento.Nº),
                    DataHoraCriação = Procedimento.DataHoraCriação,
                    UtilizadorCriação = Procedimento.UtilizadorCriação
                };

                //context.Add(Acta);
                //context.SaveChanges();

                return Acta;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static RegistoDeAtas __UpdateRegistoDeAtas(string ProcedimentoID, string NoActa, DateTime DataActa, string Observacoes, string ModificationUser, DateTime ModificationDate)
        {
            SuchDBContext context = new SuchDBContext();

            try
            {
                RegistoDeAtas Acta = context.RegistoDeAtas.Where(a => a.NºProcedimento == ProcedimentoID && a.NºAta == NoActa).FirstOrDefault();

                Acta.Observações = Observacoes;
                Acta.DataDaAta = DataActa;
                Acta.DataHoraModificação = ModificationDate;
                Acta.UtilizadorModificação = ModificationUser;

                context.RegistoDeAtas.Update(Acta);
                context.SaveChanges();

                return Acta;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public static bool __DeleteAllRegistoDeAtasRelatedToProcedimento(string ProcedimentoID)
        {
            SuchDBContext context = new SuchDBContext();
            try
            {
                RegistoDeAtas Acta = context.RegistoDeAtas.Where(a => a.NºProcedimento == ProcedimentoID).FirstOrDefault();

                context.RegistoDeAtas.RemoveRange(context.RegistoDeAtas.Where(a => a.NºProcedimento == ProcedimentoID));
                context.SaveChanges();

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public static bool __DeleteRegistoDeAtas(string ProcedimentoID, string NoActa)
        {
            SuchDBContext context = new SuchDBContext();
            try
            {
                context.RegistoDeAtas.RemoveRange(context.RegistoDeAtas.Where(a => a.NºProcedimento == ProcedimentoID && a.NºAta == NoActa));
                context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion

        #region Delete TemposPaCcp
        public static bool __DeleteTemposPaCcp(string ProcedimentoID)
        {
            SuchDBContext context = new SuchDBContext();
            try
            {
                context.TemposPaCcp.RemoveRange(context.TemposPaCcp.Where(t => t.NºProcedimento == ProcedimentoID));

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
