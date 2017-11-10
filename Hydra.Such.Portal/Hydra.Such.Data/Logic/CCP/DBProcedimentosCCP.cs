using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.CCP;
//using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.CCP
{
    public static class DBProcedimentosCCP
    {

        #region CRUD Procedimentos
        public static List<ProcedimentosCcp> GetAllProcedimentosByProcedimentoTypeToList(int type)
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                return _context.ProcedimentosCcp.Where(p => p.TipoProcedimento == type).ToList();
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public static List<ProcedimentosCcp> GetAllProcedimentosToList()
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                return _context.ProcedimentosCcp.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ProcedimentosCcp GetProcedimentoById(string ProcedimentoID)
        {
            var _context = new SuchDBContext();

            try
            {
                ProcedimentosCcp Procedimento = _context.ProcedimentosCcp.Where(p => p.Nº == ProcedimentoID).FirstOrDefault();

                if(Procedimento != null)
                {
                    /*
                        Every time a Procedimento is retrieved from the database every other entity related to it must be also retrieved:
                              Tempos PA CCP
                              Registo de Actas
                              Elementos Juri
                              Emails Procedimento CCP
                              Linhas Para Encomenda Procedimentos CCP
                              Notas Procedimento CCP
                              Workflow Procedimentos CCP
                    */

                    Procedimento.Nº1 = _context.TemposPaCcp.Where(t => t.NºProcedimento == Procedimento.Nº).FirstOrDefault();

                    // zpgm. REGISTO DE ACTAS is missing until the data model is updated

                    Procedimento.ElementosJuri = GetElementosJuriProcedimento(Procedimento.Nº);

                    var emailsProcedimento = _context.EmailsProcedimentosCcp.Where(e => e.NºProcedimento == Procedimento.Nº);
                    foreach(EmailsProcedimentosCcp em in emailsProcedimento)
                    {
                        Procedimento.EmailsProcedimentosCcp.Add(em);
                    }

                    var linhasEncomendaProc = _context.LinhasPEncomendaProcedimentosCcp.Where(l => l.NºProcedimento == Procedimento.Nº);
                    foreach(LinhasPEncomendaProcedimentosCcp ln in linhasEncomendaProc)
                    {
                        Procedimento.LinhasPEncomendaProcedimentosCcp.Add(ln);
                    }
                    
                    Procedimento.NotasProcedimentosCcp = GetNotasProcedimento(Procedimento.Nº);
                    
                    var workflowsProcedimento = _context.WorkflowProcedimentosCcp.Where(w => w.NºProcedimento == Procedimento.Nº);
                    foreach(WorkflowProcedimentosCcp wf in workflowsProcedimento)
                    {
                        Procedimento.WorkflowProcedimentosCcp.Add(wf);
                    }
                }
                
                return Procedimento;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        
        // zpgm - two overloaded methods to create ProcedimentosCcp: 
        //      the first uses a ProcedimentoCCPView object and returns a ProcedimentosCcp object
        //      the second uses a ProcedimentosCcp object and returns an object of the same type
        public static ProcedimentosCcp __CreateProcedimento(ProcedimentoCCPView Procedimento)
        {
            SuchDBContext _context = new SuchDBContext();
            ProcedimentosCcp proc = CCPFunctions.CastProcedimentoCcpViewToProcedimentoCcp(Procedimento);

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
                proc.Estado = 0;
                proc.Nº1 = new TemposPaCcp()
                {
                    NºProcedimento = proc.Nº,
                    Estado0 = 1,
                    DataHoraCriação = proc.DataHoraCriação,
                    UtilizadorCriação = proc.UtilizadorCriação
                };

                proc.NºNavigation = __CreateRegistoDeAtas(proc, false);

                _context.Add(proc.NºNavigation);
                _context.SaveChanges();

                _context.Add(proc.Nº1);
                _context.SaveChanges();

                _context.Add(proc);
                _context.SaveChanges();

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
                ProcedimentoCCPView ProcCCPView = CCPFunctions.CastProcedimentoCcpToProcedimentoCcpView(Procedimento);
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
            SuchDBContext _context = new SuchDBContext();
            try
            {
                ProcedimentosCcp proc = CCPFunctions.CastProcedimentoCcpViewToProcedimentoCcp(Procedimento);
                proc.DataHoraModificação = DateTime.Now;

                _context.ProcedimentosCcp.Update(proc);
                _context.SaveChanges();

                return proc;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public static bool __DeleteProcedimento(string ProcedimentoID)
        {
            SuchDBContext _context = new SuchDBContext();

            try
            {
                _context.ProcedimentosCcp.RemoveRange(_context.ProcedimentosCcp.Where(p => p.Nº == ProcedimentoID));
                _context.SaveChanges();

                __DeleteAllElementosJuriRelatedToProcedimento(ProcedimentoID);
                __DeleteAllNotasProcedimentoRelatedToProcedimento(ProcedimentoID);
                __DeleteAllRegistoDeAtasRelatedToProcedimento(ProcedimentoID);
                __DeleteAllWorkflowsRelatedToProcedimento(ProcedimentoID);
                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion

        #region CRUD RegistoDeActas
        public static string GetActaNumber(string ProcedimentoID)
        {
            SuchDBContext _context = new SuchDBContext();

            int num = _context.RegistoDeAtas.Where(a => a.NºProcedimento == ProcedimentoID).Count();

            num += 1;

            return num.ToString().PadLeft(4, '0');
        }
        public static List<RegistoDeAtas> GetRegistoDeActasProcedimento(string ProcedimentoID)
        {
            SuchDBContext _context = new SuchDBContext();

            try
            {
                return _context.RegistoDeAtas.Where(a => a.NºProcedimento == ProcedimentoID).ToList();
            }
            catch (Exception e)
            {

                return null;
            }
        }
        public static List<RegistoActasView> GetRegistosActasViewProcedimento(ProcedimentosCcp Procedimento)
        {
            List<RegistoActasView> RegistoView = new List<RegistoActasView>();

            // zpgm this code must be updated after the data model update

            return RegistoView;
        }
        public static RegistoDeAtas __CreateRegistoDeAtas(ProcedimentosCcp Procedimento, bool SaveRecord)
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                RegistoDeAtas Acta = new RegistoDeAtas()
                {
                    NºProcedimento = Procedimento.Nº,
                    NºAta = GetActaNumber(Procedimento.Nº),
                    DataHoraCriação = Procedimento.DataHoraCriação,
                    UtilizadorCriação = Procedimento.UtilizadorCriação
                };

                if (SaveRecord)
                {
                    _context.Add(Acta);
                    _context.SaveChanges();
                }
                
                return Acta;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static RegistoDeAtas __UpdateRegistoDeAtas(string ProcedimentoID, string NoActa, DateTime DataActa, string Observacoes, string ModificationUser, DateTime ModificationDate)
        {
            SuchDBContext _context = new SuchDBContext();

            try
            {
                RegistoDeAtas Acta = _context.RegistoDeAtas.Where(a => a.NºProcedimento == ProcedimentoID && a.NºAta == NoActa).FirstOrDefault();

                Acta.Observações = Observacoes;
                Acta.DataDaAta = DataActa;
                Acta.DataHoraModificação = ModificationDate;
                Acta.UtilizadorModificação = ModificationUser;

                _context.RegistoDeAtas.Update(Acta);
                _context.SaveChanges();

                return Acta;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public static bool __DeleteAllRegistoDeAtasRelatedToProcedimento(string ProcedimentoID)
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                //RegistoDeAtas Acta = _context.RegistoDeAtas.Where(a => a.NºProcedimento == ProcedimentoID).FirstOrDefault();

                _context.RegistoDeAtas.RemoveRange(_context.RegistoDeAtas.Where(a => a.NºProcedimento == ProcedimentoID));
                _context.SaveChanges();

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public static bool __DeleteRegistoDeAtas(string ProcedimentoID, string NoActa)
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                _context.RegistoDeAtas.RemoveRange(_context.RegistoDeAtas.Where(a => a.NºProcedimento == ProcedimentoID && a.NºAta == NoActa));
                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion

        #region CRUD ElementosJuri
        public static List<ElementosJuri> GetElementosJuriProcedimento(string ProcedimentoID)
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                return _context.ElementosJuri.Where(ej => ej.NºProcedimento == ProcedimentoID).ToList();
            }
            catch(Exception e)
            {
                return null;
            }
        }
        public static List<ElementosJuriView> GetElementosJuriViewProcedimento(ProcedimentosCcp Procedimento)
        {
            List<ElementosJuriView> ElementosView = new List<ElementosJuriView>();

            foreach(var e in Procedimento.ElementosJuri)
            {
                ElementosView.Add(CCPFunctions.CastElementosJuriToElementosJuriView(e));
            }

            return ElementosView;
        }

        public static bool __DeleteAllElementosJuriRelatedToProcedimento(string ProcedimentoID)
        {
            SuchDBContext _context = new SuchDBContext();

            try
            {
                _context.ElementosJuri.RemoveRange(_context.ElementosJuri.Where(ej => ej.NºProcedimento == ProcedimentoID));
                _context.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public static bool __DeleteElementoJuri(string ProcedimentoID, int LineNo)
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                _context.ElementosJuri.RemoveRange(_context.ElementosJuri.Where(ej => ej.NºProcedimento == ProcedimentoID && ej.NºLinha == LineNo));
                _context.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        #endregion

        #region CRUD Notas Procedimentos CCP
        public static List<NotasProcedimentosCcp> GetNotasProcedimento(string ProcedimentoID)
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                return _context.NotasProcedimentosCcp.Where(n => n.NºProcedimento == ProcedimentoID).ToList();
            }
            catch(Exception e)
            {
                return null;
            }
        }
        public static List<NotasProcedimentoCCPView> GetNotasProcedimentoView(ProcedimentosCcp Procedimento)
        {
            List<NotasProcedimentoCCPView> NotasView = new List<NotasProcedimentoCCPView>();

            foreach(var n in Procedimento.NotasProcedimentosCcp)
            {
                NotasView.Add(CCPFunctions.CastNotaProcedimentoToNotaProcedimentoView(n));
            }

            return NotasView;
        }

        public static ElementosJuri __CreateElementoJuri(ElementosJuriView ElementoView)
        {
            SuchDBContext _context = new SuchDBContext();
            ElementosJuri Elemento = CCPFunctions.CastElementosJuriViewToElementosJuri(ElementoView);

            try
            {
                _context.Add(Elemento);
                _context.SaveChanges();

                return Elemento;
            }
            catch(Exception e)
            {
                return null;
            }
        }
        public static bool __DeleteAllNotasProcedimentoRelatedToProcedimento(string ProcedimentoID)
        {
            SuchDBContext _context = new SuchDBContext();

            try
            {
                _context.NotasProcedimentosCcp.RemoveRange(_context.NotasProcedimentosCcp.Where(n => n.NºProcedimento == ProcedimentoID));
                _context.SaveChanges();

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public static bool __DeleteNotaProcedimento(string ProcedimentoID, int LineNo)
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                _context.NotasProcedimentosCcp.RemoveRange(_context.NotasProcedimentosCcp.Where(n => n.NºProcedimento == ProcedimentoID && n.NºLinha == LineNo));
                _context.SaveChanges();

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        #endregion

        #region CRUD Workflow Procedimentos CCP
        public static List<WorkflowProcedimentosCcp> GetWorkflowsProcedimento(string ProcedimentoID)
        {
            SuchDBContext _context = new SuchDBContext();

            try
            {
                return _context.WorkflowProcedimentosCcp.Where(w => w.NºProcedimento == ProcedimentoID).ToList();
            }
            catch(Exception e)
            {
                return null;
            }
        }
        public static List<WorkflowProcedimentosCCPView> GetWorkflowsView(ProcedimentosCcp Procedimento)
        {
            List<WorkflowProcedimentosCCPView> WorkflowsView = new List<WorkflowProcedimentosCCPView>();

            foreach(var w in Procedimento.WorkflowProcedimentosCcp)
            {
                WorkflowsView.Add(CCPFunctions.CastWorkflowProcedimentoToWorkflowProcedimentoView(w));
            }

            return WorkflowsView;
        }

        public static bool __DeleteAllWorkflowsRelatedToProcedimento(string ProcedimentoID)
        {
            SuchDBContext _context = new SuchDBContext();

            try
            {
                _context.WorkflowProcedimentosCcp.RemoveRange(_context.WorkflowProcedimentosCcp.Where(w => w.NºProcedimento == ProcedimentoID));
                _context.SaveChanges();

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public static bool __DeleteWorkflowsProcedimento(string ProcedimentoID, int State, DateTime DateTimeProc)
        {
            SuchDBContext _context = new SuchDBContext();

            try
            {
                _context.WorkflowProcedimentosCcp.RemoveRange(_context.WorkflowProcedimentosCcp.Where(w => w.NºProcedimento == ProcedimentoID && w.Estado == State && w.DataHora == DateTimeProc));
                _context.SaveChanges();

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        #endregion

        #region CRUD Emails Procedimentos CCP
        #endregion

        #region CRUD Linhas Para Encomenda CCP
        #endregion

        #region CRUD TemposPaCcp
        public static bool __DeleteTemposPaCcp(string ProcedimentoID)
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                _context.TemposPaCcp.RemoveRange(_context.TemposPaCcp.Where(t => t.NºProcedimento == ProcedimentoID));

                _context.SaveChanges();

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
                    ProcViewList.Add(CCPFunctions.CastProcedimentoCcpToProcedimentoCcpView(x));
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
                    ProcViewList.Add(CCPFunctions.CastProcedimentoCcpToProcedimentoCcpView(x));
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
            return CCPFunctions.CastProcedimentoCcpToProcedimentoCcpView(GetProcedimentoById(id));
        }
        #endregion

        #region Users settings related to Procedimentos CCP
        //public static List<AcessosUtilizador> GetAllUsersElementosJuri()
        #endregion
    }
}
