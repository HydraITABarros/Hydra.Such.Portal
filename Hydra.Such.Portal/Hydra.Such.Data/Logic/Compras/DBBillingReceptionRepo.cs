using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Compras;
using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static Hydra.Such.Data.Enumerations;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic.ComprasML
{
    public class BillingReceptionRepo : IDisposable
    {
        private SuchDBContext ctx;

        public BillingReceptionRepo()
        {
            ctx = new SuchDBContext();
        }

        public void SaveChanges()
        {
            ctx.SaveChanges();
        }

        #region BillingReception

        public BillingReceptionModel Create(BillingReceptionModel item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            item.DataUltimaInteracao = DateTime.Now.ToString("");
            item.Destinatario = ExtractUserNameFromEmail(item.Destinatario);
            ctx.RececaoFaturacao.Add(item.ParseToDB());

            return item;
        }

        public BillingReceptionModel Update(BillingReceptionModel item)
        {
            item.DataUltimaInteracao = DateTime.Now.ToString("");
            item.Destinatario = ExtractUserNameFromEmail(item.Destinatario);
            ctx.RececaoFaturacao.Update(item.ParseToDB());
            return item;
        }

        public List<BillingReceptionModel> GetAll()
        {
            try
            {
                return ctx.RececaoFaturacao
                    .OrderByDescending(x => x.Id)
                    .ToList()
                    .ParseToViewModel();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string ExtractUserNameFromEmail(string emailAddress)
        {
            string userName = emailAddress;
            if (!string.IsNullOrEmpty(emailAddress))
            {
                int pos = emailAddress.IndexOf('@');
                if (pos > -1)
                    userName = emailAddress.Substring(0, pos);
            }
            return userName;
        }

        public List<BillingReceptionModel> GetAllFor(UserConfigurationsViewModel userConfig)
        {
            List<BillingReceptionModel> billingReceptions = new List<BillingReceptionModel>();
            if (userConfig != null)
            {
                try
                {
                    var items = ctx.RececaoFaturacao.AsQueryable();
                    if (userConfig.RFPerfilVisualizacao.HasValue && userConfig.RFPerfilVisualizacao.Value == BillingReceptionUserProfiles.Tudo)
                    {
                        //Apply User Dimensions Validations
                        List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(userConfig.IdUser);
                        //Regions
                        if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.Region).Count() > 0)
                            items = items.Where(x => userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CodRegiao));
                        //ResponsabilityCenter
                        if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                            items = items.Where(x => userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CodCentroResponsabilidade));
                    }
                    else if (userConfig.RFPerfilVisualizacao.HasValue && userConfig.RFPerfilVisualizacao.Value == BillingReceptionUserProfiles.Perfil)
                    {
                        if (userConfig.RFPerfil.HasValue && userConfig.RFPerfil.Value != BillingReceptionAreas.Contabilidade)
                        {
                            items = items.Where(x => x.Estado.HasValue && (x.Estado.Value == (int)BillingReceptionStates.Pendente || x.Estado.Value == (int)BillingReceptionStates.Rececao));
                            List<string> areasFilter = string.IsNullOrEmpty(userConfig.RFFiltroArea) ? new List<string>() : userConfig.RFFiltroArea.Split('|').ToList();
                            if (userConfig.RFPerfil.HasValue && userConfig.RFPerfil.Value == BillingReceptionAreas.Aprovisionamento)
                            {
                                items = items.Where(x => x.AreaPendente == "Aprovisionamento");
                                if (!string.IsNullOrEmpty(userConfig.RFFiltroArea))
                                    items = items.Where(x => areasFilter.Contains(x.AreaPendente2));
                                //var test = ctx.RececaoFaturacao.Where(x => areasFilter.Contains(x.AreaPendente2)).ToList();
                            }
                            else
                            {
                                items = items.Where(x => x.AreaPendente != BillingReceptionAreas.Contabilidade.ToString() &&
                                                         x.AreaPendente != BillingReceptionAreas.Aprovisionamento.ToString() &&
                                                         x.AreaPendente != "Fornecedor");
                                if (!string.IsNullOrEmpty(userConfig.RFFiltroArea))
                                    items = items.Where(x => areasFilter.Contains(x.AreaPendente));
                            }
                        }
                    }
                    else if (userConfig.RFPerfilVisualizacao.HasValue && userConfig.RFPerfilVisualizacao.Value == BillingReceptionUserProfiles.Utilizador)
                    {
                        string userName = ExtractUserNameFromEmail(userConfig.IdUser);
                        items = items.Where(x => x.Destinatario == userName);
                    }
                    billingReceptions = items.ToList().ParseToViewModel();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return billingReceptions;
        }

        public List<BillingReceptionModel> GetPendingForUser(BillingReceptionAreas? userAreaProfile, string userName)
        {
            try
            {
                string shortUserName = ExtractUserNameFromEmail(userName);
                if(userAreaProfile.HasValue && userAreaProfile.Value == BillingReceptionAreas.Contabilidade)
                {   
                    return ctx.RececaoFaturacao.Where(x => x.AreaPendente == userAreaProfile.Value.ToString()).OrderByDescending(x => x.Id).ToList().ParseToViewModel();
                }
                else
                    return ctx.RececaoFaturacao.Where(x => x.Destinatario == shortUserName).OrderByDescending(x => x.Id).ToList().ParseToViewModel();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<BillingReceptionModel> GetPendingOnAreas(BillingReceptionAreas? userAreaProfile, string area, BillingReceptionUserProfiles? userViewProfile)
        {
            try
            {
                if (userAreaProfile.HasValue && userAreaProfile.Value == BillingReceptionAreas.Aprovisionamento)
                {
                    var items = ctx.RececaoFaturacao.Where(x => x.Estado == (int)BillingReceptionStates.Pendente);

                    if (!string.IsNullOrEmpty(area))
                    {
                        items = items.Where(x => x.AreaPendente == area);
                    }
                    else
                    {
                        List<string> areasToExclude = new List<string>
                        {
                            BillingReceptionAreas.Aprovisionamento.ToString(),
                            BillingReceptionAreas.Contabilidade.ToString(),
                            string.Empty,
                        };
                        items = items.Where(x => !areasToExclude.Contains(x.AreaPendente));
                    }
                    return items.OrderByDescending(x => x.Id).ToList().ParseToViewModel();
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public List<BillingReceptionModel> GetChangeableDestination(string userName, BillingReceptionAreas? userAreaProfile, BillingReceptionUserProfiles? userViewProfile)
        {
            try
            {
                if (userAreaProfile.HasValue && userAreaProfile.Value == BillingReceptionAreas.Aprovisionamento)
                {
                    var items = ctx.RececaoFaturacao
                        .Where(x => x.Estado == (int)BillingReceptionStates.Pendente &&
                        x.AreaPendente != BillingReceptionAreas.Contabilidade.ToString());

                    if (userViewProfile.HasValue && userViewProfile.Value == BillingReceptionUserProfiles.Utilizador)
                    {
                        string shortUserName = ExtractUserNameFromEmail(userName);
                        items = items.Where(x => x.Destinatario == shortUserName);
                    }
                    return items.OrderByDescending(x => x.Id).ToList().ParseToViewModel();
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<BillingReceptionModel> GetAllHistory()
        {
            try
            {
                //history
                return ctx.RececaoFaturacao.Where(x => string.IsNullOrEmpty(x.AreaPendente)).OrderByDescending(x => x.Id).ToList().ParseToViewModel();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public BillingReceptionModel GetById(string id)
        {
            try
            {
                return ctx.RececaoFaturacao
                    .Include(x => x.RececaoFaturacaoWorkflow)
                    .SingleOrDefault(x => x.Id == id)
                    .ParseToViewModel();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<BillingReceptionModel> GetByExternalDoc(string externalDocNo, int year, string supplierId)
        {
            try
            {
                return ctx.RececaoFaturacao
                    .Where(x => x.CodFornecedor == supplierId && x.NumDocFornecedor == externalDocNo && x.DataDocFornecedor.Value.Year == year)
                    .ToList()
                    .ParseToViewModel();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void Delete(BillingReceptionModel item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            ctx.RececaoFaturacao.Remove(item.ParseToDB());
        }
        
        #endregion

        #region WF

        public RececaoFaturacaoWorkflow Create(RececaoFaturacaoWorkflow item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            item.DataCriacao = DateTime.Now;
            item.Destinatario = ExtractUserNameFromEmail(item.Destinatario);
            item.Utilizador = ExtractUserNameFromEmail(item.Utilizador);
            var item1 = ctx.RececaoFaturacaoWorkflow.Add(item);

            return item;
        }

        public RececaoFaturacaoWorkflow Update(RececaoFaturacaoWorkflow item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            item.DataModificacao = DateTime.Now;
            item.Destinatario = ExtractUserNameFromEmail(item.Destinatario);
            item.Utilizador = ExtractUserNameFromEmail(item.Utilizador);
            ctx.RececaoFaturacaoWorkflow.Update(item);

            return item;
        }

        public void Delete(RececaoFaturacaoWorkflow item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            ctx.RececaoFaturacaoWorkflow.Remove(item);
        }

        #endregion

        #region WFA
        public RececaoFaturacaoWorkflowAnexo Create(RececaoFaturacaoWorkflowAnexo item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            var item1 = ctx.RececaoFaturacaoWorkflowAnexo.Add(item);
            return item;
        }
        #endregion

        #region GETS
        
        public List<RecFacturasProblemas> GetQuestionsProblem(string Type)
        {
            return ctx.RecFacturasProblemas.Where(x => x.Codigo == Type && x.Tipo != "" && x.Bloqueado != true).ToList();
        }
       
        public List<RecFacturasProblemas> GetQuestionsID(string id,string type)
        {
            return ctx.RecFacturasProblemas.Where(x => x.Codigo == id && x.Tipo == type).ToList();
        }
        public List<RecFacturasProblemas> GetQuestionIDByDesc(string id, string desc)
        {
            return ctx.RecFacturasProblemas.Where(x => x.Codigo == id && x.Descricao == desc).ToList();
        }
        
        public List<RecFacturasProblemas> GetQuestionsReason()
        {
            return ctx.RecFacturasProblemas.Where(x => x.Codigo == "RF4P").ToList();
        }
        public List<RecFacturasProblemas> GetAllProblems()
        {
            return ctx.RecFacturasProblemas.ToList();
        }
        public List<RecFaturacaoConfigDestinatarios> GetAreas()
        {
            return ctx.RecFaturacaoConfigDestinatarios.Where(x => x.Codigo.StartsWith("1A") && x.Mostra == true).ToList();
        }
        public List<RecFaturacaoConfigDestinatarios> GetAreasUPUAS(string respCenterId)
        {
            if (string.IsNullOrEmpty(respCenterId))
                return ctx.RecFaturacaoConfigDestinatarios
                    .Where(x => x.Codigo.Length == 5 && 
                                x.Codigo.StartsWith("3A-") && 
                                x.Mostra == true)
                    .ToList();
            else
                return ctx.RecFaturacaoConfigDestinatarios
                    .Where(x => x.Codigo.StartsWith("3A-") &&
                                x.Mostra == true &&
                                x.CodCentroResponsabilidade == respCenterId &&
                                !string.IsNullOrEmpty(x.Destinatario))
                    .ToList();
        }
        public List<RecFaturacaoConfigDestinatarios> GetDimensionsForArea(string areaId)
        {
            var dimensions = ctx.RecFaturacaoConfigDestinatarios.Where(x => x.Codigo.StartsWith("3A-") && x.CodArea == areaId && x.Mostra == true && x.CodCentroResponsabilidade != string.Empty).ToList();
            dimensions.ForEach(dim =>
                dim.Destinatario = ctx.RecFaturacaoConfigDestinatarios
                                    .Where(x => x.Codigo.StartsWith("3A-") &&
                                        x.Mostra == false &&
                                        x.CodCentroResponsabilidade == dim.CodCentroResponsabilidade &&
                                        !string.IsNullOrEmpty(x.Destinatario))
                                    .FirstOrDefault()?.Destinatario
            );
            return dimensions;// ctx.RecFaturacaoConfigDestinatarios.Where(x => x.Codigo.StartsWith("3A-") && x.CodArea == areaId && x.Mostra == true && x.CodCentroResponsabilidade != string.Empty).ToList();
        }
        public List<RecFaturacaoConfigDestinatarios> GetUsersToResendByAreaName(string areaId, string respCenterId)
        {
            List<RecFaturacaoConfigDestinatarios> users = new List<RecFaturacaoConfigDestinatarios>();

            if (string.IsNullOrEmpty(respCenterId))
            {
                users = ctx.RecFaturacaoConfigDestinatarios
                    .Where(x => x.Codigo.StartsWith("3A-") &&
                                x.CodArea == areaId &&
                                x.Mostra == false
                                && string.IsNullOrEmpty(x.CodCentroResponsabilidade))
                    .ToList();
            }
            else
            {
                users = ctx.RecFaturacaoConfigDestinatarios
                    .Where(x => x.Codigo.StartsWith("3A-") &&
                                x.CodArea == areaId &&
                                x.Mostra == false
                                && x.CodCentroResponsabilidade == respCenterId)
                    .ToList();
            }
            var areas = GetAreasUPUAS(respCenterId);
            if (areas.Count > 0)
            {
                var selectedArea = areas.FirstOrDefault(x => x.CodArea == areaId);
                if (selectedArea != null && !string.IsNullOrEmpty(selectedArea.Destinatario) && !users.Any(x => x.Destinatario == selectedArea.Destinatario))
                {
                    users.Add(selectedArea);
                }
            }
            return users;
        }

        public List<RecFaturacaoConfigDestinatarios> GetUsersToResendByAreaNumber(string areaNumber)
        {
            List<RecFaturacaoConfigDestinatarios> users = new List<RecFaturacaoConfigDestinatarios>();

            users = ctx.RecFaturacaoConfigDestinatarios
                .Where(x => x.Codigo.StartsWith("V" + areaNumber) &&
                            !string.IsNullOrEmpty(x.Destinatario))
                .ToList();
            
            return users;
        }

        public List<RecFaturacaoConfigDestinatarios> GetDestination()
        {
            return ctx.RecFaturacaoConfigDestinatarios.ToList();
        }

        public RecFacturasProblemas Update(RecFacturasProblemas item)
        {
            ctx.RecFacturasProblemas.Update(item);
            return item;
        }

        public void Create(RecFacturasProblemas item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (string.IsNullOrEmpty(item.Tipo))
                item.Tipo = string.Empty;

            ctx.RecFacturasProblemas.Add(item);
        }

        public void Delete(RecFacturasProblemas item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            ctx.RecFacturasProblemas.Remove(item);
        }
        #endregion

        void IDisposable.Dispose()
        {
            if(ctx != null)
                ctx.Dispose();
        }
    }
}
