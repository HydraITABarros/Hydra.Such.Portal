using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public static class DBUserConfigurations
    {

        public static ConfigUtilizadores GetById(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id) && !id.Contains('@'))
                {
                    id += "@";
                }
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfigUtilizadores.Where(x => x.IdUtilizador.ToLower().StartsWith(id.ToLower())).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        
        public static ConfigUtilizadores GetByEmployeeNo(string EmployeeNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfigUtilizadores.Where(x => x.EmployeeNo.ToLower() == EmployeeNo.ToLower()).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ConfigUtilizadores> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfigUtilizadores.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConfigUtilizadores Create(ConfigUtilizadores ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.ConfigUtilizadores.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConfigUtilizadores Update(ConfigUtilizadores ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.ConfigUtilizadores.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        #region Parse Utilities
        public static UserConfigurationsViewModel ParseToViewModel(this ConfigUtilizadores item)
        {
            if (item != null)
            {

                return new UserConfigurationsViewModel()
                {
                    IdUser = item.IdUtilizador,
                    Name = item.Nome,
                    Active = item.Ativo,
                    Administrator = item.Administrador,
                    Area = item.AreaPorDefeito,
                    Cresp = item.CentroRespPorDefeito,
                    EmployeeNo = item.EmployeeNo,
                    ProcedimentosEmailEnvioParaArea = item.ProcedimentosEmailEnvioParaArea,
                    ProcedimentosEmailEnvioParaArea2 = item.ProcedimentosEmailEnvioParaArea2,
                    ProcedimentosEmailEnvioParaCA = item.ProcedimentosEmailEnvioParaCa,
                    ReceptionConfig = item.PerfilNumeraçãoRecDocCompras,
                    Regiao = item.RegiãoPorDefeito,
                    RFPerfil = item.Rfperfil.HasValue ? (Enumerations.BillingReceptionAreas)item.Rfperfil : (Enumerations.BillingReceptionAreas?)null,
                    RFPerfilVisualizacao = item.RfperfilVisualizacao.HasValue ? (Enumerations.BillingReceptionUserProfiles)item.RfperfilVisualizacao : (Enumerations.BillingReceptionUserProfiles?)null,
                    RFFiltroArea = item.RffiltroArea,
                    RFNomeAbreviado = item.RfnomeAbreviado,
                    RFRespostaContabilidade = item.RfrespostaContabilidade,
                    RFAlterarDestinatarios = item.RfalterarDestinatarios,
                    RFMailEnvio = item.RfmailEnvio,
                    Centroresp = item.CentroDeResponsabilidade,
                    NumSerieFaturas = item.NumSerieFaturas,
                    NumSerieNotasCredito = item.NumSerieNotasCredito,
                    NumSerieNotasDebito = item.NumSerieNotasDebito,
                    NumSeriePreFaturasCompraCP = item.NumSeriePreFaturasCompraCp,
                    NumSeriePreFaturasCompraCF = item.NumSeriePreFaturasCompraCf,
                    NumSerieNotasCreditoCompra = item.NumSerieNotasCreditoCompra
                };
            }
            return null;
        }

        public static List<UserConfigurationsViewModel> ParseToViewModel(this List<ConfigUtilizadores> items)
        {
            List<UserConfigurationsViewModel> parsedItems = new List<UserConfigurationsViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }

        public static ConfigUtilizadores ParseToDB(this UserConfigurationsViewModel item)
        {
            if (item != null)
            {
                return new ConfigUtilizadores()
                {
                    IdUtilizador = item.IdUser,
                    Nome = item.Name,
                    Ativo = item.Active,
                    Administrador = item.Administrator,
                    AreaPorDefeito = item.Area,
                    CentroRespPorDefeito = item.Cresp,
                    EmployeeNo = item.EmployeeNo,
                    ProcedimentosEmailEnvioParaArea = item.ProcedimentosEmailEnvioParaArea,
                    ProcedimentosEmailEnvioParaArea2 = item.ProcedimentosEmailEnvioParaArea2,
                    ProcedimentosEmailEnvioParaCa = item.ProcedimentosEmailEnvioParaCA,
                    PerfilNumeraçãoRecDocCompras = item.ReceptionConfig,
                    RegiãoPorDefeito = item.Regiao,
                    Rfperfil = item.RFPerfil.HasValue ? (int)item.RFPerfil : (int?)null,
                    RfperfilVisualizacao = item.RFPerfilVisualizacao.HasValue ? (int)item.RFPerfilVisualizacao : (int?)null,
                    RffiltroArea = item.RFFiltroArea,
                    RfnomeAbreviado = item.RFNomeAbreviado,
                    RfrespostaContabilidade = item.RFRespostaContabilidade,
                    RfalterarDestinatarios = item.RFAlterarDestinatarios,
                    RfmailEnvio = item.RFMailEnvio,
                    CentroDeResponsabilidade=item.Centroresp,
                    NumSerieFaturas = item.NumSerieFaturas,
                    NumSerieNotasCredito = item.NumSerieNotasCredito,
                    NumSerieNotasDebito = item.NumSerieNotasDebito,
                    NumSeriePreFaturasCompraCf = item.NumSeriePreFaturasCompraCF,
                    NumSeriePreFaturasCompraCp = item.NumSeriePreFaturasCompraCP,
                    NumSerieNotasCreditoCompra = item.NumSerieNotasCreditoCompra
                };
            }
            return null;
        }

        public static List<ConfigUtilizadores> ParseToDB(this List<UserConfigurationsViewModel> items)
        {
            List<ConfigUtilizadores> parsedItems = new List<ConfigUtilizadores>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
        #endregion
    }
}
