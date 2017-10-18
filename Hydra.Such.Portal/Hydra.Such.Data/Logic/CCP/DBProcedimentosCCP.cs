using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.CCP;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Hydra.Such.Data.Logic.CCP
{
    public static class DBProcedimentosCCP
    {
        private static SuchDBContext context = new SuchDBContext();

        #region parse ProcedimentosCcp
        public static List<ProcedimentosCcp> GetAllProcedimentosByTypeToList(int type)
        {
            try
            {
                return context.ProcedimentosCcp.Where(p => p.Tipo == type).ToList();
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public static List<ProcedimentosCcp> GetAllProcedimentosToList()
        {
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
        public static ProcedimentosCcp CreateProcedimento(ProcedimentoCCPView procedimento)
        {
            ProcedimentosCcp proc = CCPFunctions.CastProcCcpViewToProcCcp(procedimento);
            try
            {
                proc.DataHoraCriação = DateTime.Now;
                // inserir o utilizador da criação
                //proc.UtilizadorCriação = User.Identity.Name;
                context.Add(proc);
                context.SaveChanges();
                return proc;
            }
            catch(Exception e)
            {
                return null;
            }

            return proc;
        }
        public static ProcedimentosCcp CreateProcedimento(ProcedimentosCcp procedimento)
        {
            try
            {
                procedimento.DataHoraCriação = DateTime.Now;
                // inserir o utilizador da criação
                //proc.UtilizadorCriação = User.Identity.Name;
                context.Add(procedimento);
                context.SaveChanges();

                return procedimento;
            }
            catch(Exception e)
            {
                return null;
            }
            
        }
        #endregion

        #region parse ProcedimentosCCPView
        public static List<ProcedimentoCCPView> GetAllProcedimentosViewByTypeToList(int type)
        {
            List <ProcedimentosCcp> ProcList = GetAllProcedimentosByTypeToList(type);
            List<ProcedimentoCCPView> ProcViewList = new List<ProcedimentoCCPView>();

            if (ProcList.Count == 0)
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

            if (ProcList.Count == 0)
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
