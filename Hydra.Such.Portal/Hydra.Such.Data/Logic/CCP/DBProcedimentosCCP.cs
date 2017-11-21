using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.CCP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.CCP
{
    public static class DBProcedimentosCCP
    {
        private const int _ElementoJuriFeature = 23;

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
                        Every time a Procedimento CCP is retrieved from the database every other entity related to it must be also retrieved:
                              Tempos PA CCP
                              Registo de Actas
                              Elementos Juri
                              Emails Procedimento CCP
                              Linhas Para Encomenda Procedimentos CCP
                              Notas Procedimento CCP
                              Workflow Procedimentos CCP
                              Fluxo de Trabalho Lista Controlo
                    */ 
                    
                    Procedimento.TemposPaCcp = _context.TemposPaCcp.Where(t => t.NºProcedimento == Procedimento.Nº).FirstOrDefault();
                    Procedimento.RegistoDeAtas = GetAllRegistoDeActasProcedimento(Procedimento.Nº);
                    Procedimento.ElementosJuri = GetAllElementosJuriProcedimento(Procedimento.Nº);
                    Procedimento.EmailsProcedimentosCcp = GetAllEmailsProcedimento(Procedimento.Nº);
                    Procedimento.LinhasPEncomendaProcedimentosCcp = GetAllLinhasParaEncomenda(Procedimento.Nº);
                    Procedimento.NotasProcedimentosCcp = GetAllNotasProcedimento(Procedimento.Nº);
                    Procedimento.WorkflowProcedimentosCcp = GetAllWorkflowsProcedimento(Procedimento.Nº);
                    Procedimento.FluxoTrabalhoListaControlo = GetAllCheklistControloProcedimento(Procedimento.Nº);
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

               
                //proc.TemposPaCcp = new TemposPaCcp()
                //{
                //    NºProcedimento = proc.Nº,
                //    Estado0 = 1,
                //    DataHoraCriação = proc.DataHoraCriação,
                //    UtilizadorCriação = proc.UtilizadorCriação
                //};

                //_context.Add(proc.TemposPaCcp);
                //_context.SaveChanges();

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
        public static ProcedimentosCcp __CreateProcedimento(int ProcedimentoType, string UserID)
        {
            try
            {
                ProcedimentoCCPView ProcedimentoView = new ProcedimentoCCPView
                {
                    TipoProcedimento = ProcedimentoType,
                    UtilizadorCriacao = UserID

                };

                ProcedimentosCcp Procedimento = __CreateProcedimento(ProcedimentoView);

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

                if (proc.ElementosJuri != null && proc.ElementosJuri.Count > 0)
                {
                    foreach (var ej in proc.ElementosJuri)
                    {
                        ej.DataHoraModificação = DateTime.Now;
                        ej.UtilizadorModificação = proc.UtilizadorModificação;

                        ElementosJuri Elemento = __UpdateElementoJuri(ej);
                    }

                }

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
                __DeleteAllEmailsRelatedToProcedimento(ProcedimentoID);
                __DeleteAllLinhasParaEncomendaRelatedToProcedimento(ProcedimentoID);
                __DeleteAllCheklistControloRelatedToProcedimento(ProcedimentoID);
                __DeleteTemposPaCcp(ProcedimentoID);
                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion

        #region CRUD TemposPaCcp
        //public static TemposPACCPView GetTemposPACcpView()
        public static bool __DeleteTemposPaCcp(string ProcedimentoID)
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                _context.TemposPaCcp.RemoveRange(_context.TemposPaCcp.Where(t => t.NºProcedimento == ProcedimentoID));

                _context.SaveChanges();

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
        public static List<RegistoDeAtas> GetAllRegistoDeActasProcedimento(string ProcedimentoID)
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
            foreach(var ra in Procedimento.RegistoDeAtas)
            {
                RegistoView.Add(CCPFunctions.CastRegistoActasToRegistoActasView(ra));
            }

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
        public static List<ElementosJuri> GetAllElementosJuriProcedimento(string ProcedimentoID)
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
        public static List<ElementosJuriView> GetAllElementosJuriViewProcedimento(ProcedimentosCcp Procedimento)
        {
            List<ElementosJuriView> ElementosView = new List<ElementosJuriView>();

            foreach(var e in Procedimento.ElementosJuri)
            {
                ElementosJuriView ElemJuriV = CCPFunctions.CastElementosJuriToElementosJuriView(e);
                if(e.Utilizador != null && e.Utilizador != "")
                    ElemJuriV.NomeEmpregado = GetUserName(ElemJuriV.Utilizador);

                ElementosView.Add(ElemJuriV);
            }

            return ElementosView;
        }

        public static ElementosJuri __CreateElementoJuri(ElementosJuriView ElementoView)
        {
            SuchDBContext _context = new SuchDBContext();

            if (ElementoView == null)
                return null;

            //ElementosJuri DuplicateElemento = _context.ElementosJuri.Where(ej => ej.NºProcedimento == ElementoView.NoProcedimento && ej.Utilizador == ElementoView.Utilizador).FirstOrDefault();

            //if (DuplicateElemento != null)
            //    return null;

            ElementosJuri Elemento = CCPFunctions.CastElementosJuriViewToElementosJuri(ElementoView);
            try
            {
                _context.Add(Elemento);
                _context.SaveChanges();

                return Elemento;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static ElementosJuri __UpdateElementoJuri(ElementosJuriView ElementoView)
        {
            SuchDBContext _context = new SuchDBContext();

            try
            {
                ElementosJuri Elemento = _context.ElementosJuri.Where(ej => ej.NºProcedimento == ElementoView.NoProcedimento && ej.NºLinha == ElementoView.NoLinha).FirstOrDefault();

                Elemento.Presidente = ElementoView.Presidente;
                Elemento.Vogal = ElementoView.Vogal;
                Elemento.Suplente = ElementoView.Suplente;
                Elemento.Email = ElementoView.Email;

                Elemento.DataHoraModificação = ElementoView.DataHoraModificacao;
                Elemento.UtilizadorModificação = ElementoView.UtilizadorModificacao;

                _context.Update(Elemento);
                _context.SaveChanges();

                return Elemento;
            }
            catch (Exception e)
            {

                return null;
            }

        }
        public static ElementosJuri __UpdateElementoJuri(ElementosJuri Elemento)
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                ElementosJuri Elemento2 = _context.ElementosJuri.Where(ej => ej.NºProcedimento == Elemento.NºProcedimento && ej.NºLinha == Elemento.NºLinha).FirstOrDefault();

                Elemento2.Presidente = Elemento.Presidente;
                Elemento2.Vogal = Elemento.Vogal;
                Elemento2.Suplente = Elemento.Suplente;
                Elemento.Email = Elemento.Email;

                Elemento2.DataHoraModificação = Elemento.DataHoraModificação;
                Elemento2.UtilizadorModificação = Elemento.UtilizadorModificação;

                _context.Update(Elemento2);
                _context.SaveChanges();

                return Elemento2;
            }
            catch (Exception e)
            {

                return null;
            }
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

        #region CRUD Emails Procedimentos CCP
        public static List<EmailsProcedimentosCcp> GetAllEmailsProcedimento(string ProcedimentoID)
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                return _context.EmailsProcedimentosCcp.Where(ep => ep.NºProcedimento == ProcedimentoID).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static List<EmailsProcedimentoCCPView> GetAllEmailsView(ProcedimentosCcp Procedimento)
        {
            List<EmailsProcedimentoCCPView> EmailsView = new List<EmailsProcedimentoCCPView>();
            foreach (var ep in Procedimento.EmailsProcedimentosCcp)
            {
                EmailsView.Add(CCPFunctions.CastEmailProcedimentoToEmailProcedimentoView(ep));
            }

            return EmailsView;
        }

        public static bool __DeleteAllEmailsRelatedToProcedimento(string ProcedimentoID)
        {
            SuchDBContext _context = new SuchDBContext();

            try
            {
                _context.EmailsProcedimentosCcp.RemoveRange(_context.EmailsProcedimentosCcp.Where(ep => ep.NºProcedimento == ProcedimentoID));
                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public static bool __DeleteEmailProcedimento(string ProcedimentoID, int LineNo)
        {
            SuchDBContext _context = new SuchDBContext();

            try
            {
                _context.EmailsProcedimentosCcp.RemoveRange(_context.EmailsProcedimentosCcp.Where(ep => ep.NºProcedimento == ProcedimentoID && ep.NºLinha == LineNo));
                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion

        #region CRUD Linhas Para Encomenda CCP
        public static List<LinhasPEncomendaProcedimentosCcp> GetAllLinhasParaEncomenda(string ProcedimentoID)
        {
            SuchDBContext _context = new SuchDBContext();

            try
            {
                return _context.LinhasPEncomendaProcedimentosCcp.Where(le => le.NºProcedimento == ProcedimentoID).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static List<LinhasParaEncomendaCCPView> GetAllLinhasParaEncomendaView(ProcedimentosCcp Procedimento)
        {
            List<LinhasParaEncomendaCCPView> LinhasParaEncView = new List<LinhasParaEncomendaCCPView>();
            foreach (var le in Procedimento.LinhasPEncomendaProcedimentosCcp)
            {
                LinhasParaEncView.Add(CCPFunctions.CastLinhaParaEncomendaProcediementoToLinhaEncomendaCCPView(le));
            }

            return LinhasParaEncView;
        }

        public static bool __DeleteAllLinhasParaEncomendaRelatedToProcedimento(string ProcedimentoID)
        {
            SuchDBContext _context = new SuchDBContext();

            try
            {
                _context.LinhasPEncomendaProcedimentosCcp.RemoveRange(_context.LinhasPEncomendaProcedimentosCcp.Where(le => le.NºProcedimento == ProcedimentoID));
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public static bool __DeleteLinhaParaEncomenda(string ProcedimentoID, int LineNo)
        {
            SuchDBContext _context = new SuchDBContext();

            try
            {
                _context.LinhasPEncomendaProcedimentosCcp.RemoveRange(_context.LinhasPEncomendaProcedimentosCcp.Where(le => le.NºProcedimento == ProcedimentoID && le.NºLinha == LineNo));
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }
        #endregion

        #region CRUD Notas Procedimentos CCP
        public static List<NotasProcedimentosCcp> GetAllNotasProcedimento(string ProcedimentoID)
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
        public static List<NotasProcedimentoCCPView> GetAllNotasProcedimentoView(ProcedimentosCcp Procedimento)
        {
            List<NotasProcedimentoCCPView> NotasView = new List<NotasProcedimentoCCPView>();

            foreach(var n in Procedimento.NotasProcedimentosCcp)
            {
                NotasView.Add(CCPFunctions.CastNotaProcedimentoToNotaProcedimentoView(n));
            }

            return NotasView;
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
        public static List<WorkflowProcedimentosCcp> GetAllWorkflowsProcedimento(string ProcedimentoID)
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
        public static List<WorkflowProcedimentosCCPView> GetAllWorkflowsView(ProcedimentosCcp Procedimento)
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

        #region CRUD Fluxo de Trabalho Lista Controlo
        public static List<FluxoTrabalhoListaControlo> GetAllCheklistControloProcedimento(string ProcedimentoID)
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                return _context.FluxoTrabalhoListaControlo.Where(f => f.No == ProcedimentoID).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static FluxoTrabalhoListaControlo __CreateFluxoTrabalho(string ProcedimentoID, DateTime SubmissionDate, string Comment, string UserID, bool Imob)
        {
            FluxoTrabalhoListaControlo Fluxo = new FluxoTrabalhoListaControlo()
            {
                No = ProcedimentoID,
                Estado = 0,
                Data = SubmissionDate,
                Hora = SubmissionDate.TimeOfDay,
                TipoEstado = 1,
                Comentario = Comment,
                User = UserID
            };

            Fluxo.EstadoSeguinte = Imob ? 1 : 4;

            return Fluxo;
        }
        public static bool __DeleteAllCheklistControloRelatedToProcedimento(string ProcedimentoID)
        {
            SuchDBContext _context = new SuchDBContext();

            try
            {
                _context.FluxoTrabalhoListaControlo.RemoveRange(_context.FluxoTrabalhoListaControlo.Where(f => f.No == ProcedimentoID));
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }
        public static bool __DeleteChecklistControlo(string ProcedimentoID, int State, DateTime Date, TimeSpan Time)
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                _context.FluxoTrabalhoListaControlo.RemoveRange(_context.FluxoTrabalhoListaControlo.Where(f => f.No == ProcedimentoID && f.Estado == State && f.Data == Date && f.Hora == Time));
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
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
        public static string GetUserName(string UserID)
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                var CU = _context.ConfigUtilizadores.Where(cu => cu.IdUtilizador == UserID).FirstOrDefault();
                return CU.Nome;
            }
            catch(Exception e)
            {
                return null;
            }

        }
        public static List<ConfigUtilizadores> GetAllUsersElementosJuri()
        {
            SuchDBContext _context = new SuchDBContext();
            List<AcessosUtilizador> UsersAccess = _context.AcessosUtilizador.Where(a => a.Funcionalidade == _ElementoJuriFeature).ToList();            

            List<ConfigUtilizadores> UsersElementosJuri = new List<ConfigUtilizadores>();

            try
            {
                foreach(var au in UsersAccess)
                {
                    UsersElementosJuri.Add(_context.ConfigUtilizadores.Where(u => u.IdUtilizador == au.IdUtilizador).FirstOrDefault());
                }
                return UsersElementosJuri;
            }
            catch(Exception e)
            {
                return null;
            }
        }        
        #endregion
    }
}
