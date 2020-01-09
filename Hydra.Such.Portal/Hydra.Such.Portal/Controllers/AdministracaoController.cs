using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Office.Interop.Excel;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.Logic.ProjectDiary;
using Hydra.Such.Data.ViewModel.ProjectDiary;
using Hydra.Such.Data.ViewModel.ProjectView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Logic.Viatura;
using Hydra.Such.Data.ViewModel.FH;
using Hydra.Such.Data.Logic.FolhaDeHora;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Data.Logic.ComprasML;
using Hydra.Such.Data.Logic.Approvals;
using Hydra.Such.Data.ViewModel.Approvals;
using Microsoft.Extensions.Options;
using Hydra.Such.Data;
using System.IO;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Globalization;
using Hydra.Such.Data.Logic.Nutrition;
using Hydra.Such.Data.ViewModel.Nutrition;
using Hydra.Such.Data.ViewModel.Contracts;
using Hydra.Such.Data.Logic.Contracts;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using NPOI.HSSF.UserModel;
using Hydra.Such.Data.Logic.Request;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Hydra.Such.Data.ViewModel.CCP;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class AdministracaoController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly GeneralConfigurations _generalConfig;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AdministracaoController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IOptions<GeneralConfigurations> appSettingsGeneral, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            _generalConfig = appSettingsGeneral.Value;
            this._hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Utilizadores
        public IActionResult ConfiguracaoUtilizadores()
        {
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminGeral);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.UPermissions = userPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetListUsers()
        {
            List<UserConfigurationsViewModel> result = DBUserConfigurations.GetAll().ParseToViewModel();

            //if (result != null)
            //{
            //    result.ForEach(Utilizador =>
            //    {
            //        var nomeRegião = DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name, Utilizador.RegiãoPorDefeito).FirstOrDefault();
            //        Utilizador.RegiãoPorDefeito = nomeRegião == null ? "" : nomeRegião.Name;

            //        var nomeÁreaPorDefeito = DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 2, User.Identity.Name, Utilizador.AreaPorDefeito).FirstOrDefault();
            //        Utilizador.AreaPorDefeito = nomeÁreaPorDefeito == null ? "" : nomeÁreaPorDefeito.Name;

            //        var nomeCentroRespPorDefeito = DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 3, User.Identity.Name, Utilizador.CentroRespPorDefeito).FirstOrDefault();
            //        Utilizador.CentroRespPorDefeito = nomeCentroRespPorDefeito == null ? "" : nomeCentroRespPorDefeito.Name;
            //    });
            //};

            return Json(result);
        }


        public IActionResult ConfiguracaoUtilizadoresDetalhes(string id)
        {
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminGeral);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.UserId = id;
                ViewBag.UPermissions = userPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetUserConfigData([FromBody] UserConfigurationsViewModel data)
        {
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(data.IdUser);
            UserConfigurationsViewModel result = new UserConfigurationsViewModel()
            {
                IdUser = "",
                UserAccesses = new List<UserAccessesViewModel>(),
                UserProfiles = new List<ProfileModelsViewModel>()
            };

            if (userConfig != null)
            {
                result.IdUser = userConfig.IdUtilizador;
                result.Name = userConfig.Nome;
                result.Active = userConfig.Ativo;
                result.Administrator = userConfig.Administrador;
                result.Regiao = userConfig.RegiãoPorDefeito;
                result.Area = userConfig.AreaPorDefeito;
                result.Cresp = userConfig.CentroRespPorDefeito;
                result.EmployeeNo = userConfig.EmployeeNo;
                result.ProcedimentosEmailEnvioParaCA = userConfig.ProcedimentosEmailEnvioParaCa;
                result.ProcedimentosEmailEnvioParaArea = userConfig.ProcedimentosEmailEnvioParaArea;
                result.ProcedimentosEmailEnvioParaArea2 = userConfig.ProcedimentosEmailEnvioParaArea2;
                result.ReceptionConfig = userConfig.PerfilNumeraçãoRecDocCompras;
                if (userConfig.Rfperfil.HasValue)
                    result.RFPerfil = (Enumerations.BillingReceptionAreas)userConfig.Rfperfil;
                if (userConfig.RfperfilVisualizacao.HasValue)
                    result.RFPerfilVisualizacao = (Enumerations.BillingReceptionUserProfiles)userConfig.RfperfilVisualizacao;
                result.RFFiltroArea = userConfig.RffiltroArea;
                result.RFNomeAbreviado = userConfig.RfnomeAbreviado;
                result.RFRespostaContabilidade = userConfig.RfrespostaContabilidade;
                result.RFAlterarDestinatarios = userConfig.RfalterarDestinatarios;
                result.RFMailEnvio = userConfig.RfmailEnvio;
                result.NumSerieFaturas = userConfig.NumSerieFaturas;
                result.NumSeriePreFaturasCompraCF = userConfig.NumSeriePreFaturasCompraCf;
                result.NumSeriePreFaturasCompraCP = userConfig.NumSeriePreFaturasCompraCp;
                result.NumSerieNotasCreditoCompra = userConfig.NumSerieNotasCreditoCompra;
                result.NumSerieNotasCredito = userConfig.NumSerieNotasCredito;
                result.NumSerieNotasDebito = userConfig.NumSerieNotasDebito;
                result.Centroresp = userConfig.CentroDeResponsabilidade;
                result.SuperiorHierarquico = userConfig.SuperiorHierarquico;
                result.RequisicaoStock = userConfig.RequisicaoStock.HasValue ? userConfig.RequisicaoStock : false;
                result.AprovadorPedidoPag1 = userConfig.AprovadorPedidoPag1;
                result.AprovadorPedidoPag2 = userConfig.AprovadorPedidoPag2;
                result.AnulacaoPedidoPagamento = userConfig.AnulacaoPedidoPagamento.HasValue ? userConfig.AnulacaoPedidoPagamento : false;
                result.CriarProjetoSemAprovacao = userConfig.CriarProjetoSemAprovacao.HasValue ? userConfig.CriarProjetoSemAprovacao : false;
                result.CMHistoricoToActivo = userConfig.CMHistoricoToActivo.HasValue ? userConfig.CMHistoricoToActivo : false;
                result.ValidarPedidoPagamento = userConfig.ValidarPedidoPagamento.HasValue ? userConfig.ValidarPedidoPagamento : false;
                result.ArquivarREQPendentes = userConfig.ArquivarREQPendentes.HasValue ? userConfig.ArquivarREQPendentes : false;


                result.UserAccesses = DBUserAccesses.GetByUserId(data.IdUser).Select(x => new UserAccessesViewModel()
                {
                    IdUser = x.IdUtilizador,
                    //Area = x.Área,
                    Feature = x.Funcionalidade,
                    Create = x.Inserção,
                    Read = x.Leitura,
                    Update = x.Modificação,
                    Delete = x.Eliminação
                }).ToList();

                result.UserProfiles = DBProfileModels.GetByUserId(data.IdUser).Select(x => new ProfileModelsViewModel()
                {
                    Id = x.Id,
                    Description = x.Descrição
                }).ToList();

                result.AllowedUserDimensions = DBUserDimensions.GetByUserId(data.IdUser).ParseToViewModel();

                result.UserAcessosLocalizacoes = DBAcessosLocalizacoes.GetByUserId(data.IdUser).ParseToViewModel();

                List<GruposAprovação> AllGrupos = DBApprovalGroups.GetAll();
                List<ApprovalGroupViewModel> ListGrupoAprovacao = new List<ApprovalGroupViewModel>();
                foreach (GruposAprovação grupo in AllGrupos)
                {
                    List<UtilizadoresGruposAprovação> AllUsersByGrupo = DBApprovalUserGroup.GetByGroup(grupo.Código);
                    if (AllUsersByGrupo != null && AllUsersByGrupo.Where(x => x.UtilizadorAprovação.ToLower() == userConfig.IdUtilizador.ToLower()).Count() > 0)
                        ListGrupoAprovacao.Add(grupo.ParseToViewModel());
                }
                result.UserGruposAprovacao = ListGrupoAprovacao.OrderBy(x => x.Description).ToList();
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateUserConfig([FromBody] UserConfigurationsViewModel data)
        {
            ConfigUtilizadores ObjectCreated = DBUserConfigurations.Create(new ConfigUtilizadores()
            {
                IdUtilizador = data.IdUser,
                Nome = data.Name,
                Administrador = data.Administrator,
                Ativo = data.Active.HasValue ? data.Active.Value : false,
                RegiãoPorDefeito = data.Regiao,
                AreaPorDefeito = data.Area,
                CentroRespPorDefeito = data.Cresp,
                EmployeeNo = data.EmployeeNo,
                ProcedimentosEmailEnvioParaCa = data.ProcedimentosEmailEnvioParaCA,
                ProcedimentosEmailEnvioParaArea = data.ProcedimentosEmailEnvioParaArea,
                ProcedimentosEmailEnvioParaArea2 = data.ProcedimentosEmailEnvioParaArea2,
                UtilizadorCriação = User.Identity.Name,
                PerfilNumeraçãoRecDocCompras = data.ReceptionConfig,
                Rfperfil = data.RFPerfil.HasValue ? (int)data.RFPerfil : (int?)null,
                RfperfilVisualizacao = data.RFPerfilVisualizacao.HasValue ? (int)data.RFPerfilVisualizacao : (int?)null,
                RffiltroArea = data.RFFiltroArea,
                RfnomeAbreviado = data.RFNomeAbreviado,
                RfrespostaContabilidade = data.RFRespostaContabilidade,
                RfalterarDestinatarios = data.RFAlterarDestinatarios,
                RfmailEnvio = data.RFMailEnvio,
                NumSerieFaturas = data.NumSerieFaturas,
                NumSeriePreFaturasCompraCf = data.NumSeriePreFaturasCompraCF,
                NumSeriePreFaturasCompraCp = data.NumSeriePreFaturasCompraCP,
                NumSerieNotasCreditoCompra = data.NumSerieNotasCreditoCompra,
                NumSerieNotasCredito = data.NumSerieNotasCredito,
                NumSerieNotasDebito = data.NumSerieNotasDebito,
                CentroDeResponsabilidade=data.Centroresp,
                SuperiorHierarquico = data.SuperiorHierarquico,
                RequisicaoStock = data.RequisicaoStock.HasValue ? data.RequisicaoStock.Value : false,
                AprovadorPedidoPag1 = data.AprovadorPedidoPag1,
                AprovadorPedidoPag2 = data.AprovadorPedidoPag2,
                AnulacaoPedidoPagamento = data.AnulacaoPedidoPagamento.HasValue ? data.AnulacaoPedidoPagamento : false,
                CriarProjetoSemAprovacao = data.CriarProjetoSemAprovacao.HasValue ? data.CriarProjetoSemAprovacao : false,
                CMHistoricoToActivo = data.CMHistoricoToActivo.HasValue ? data.CMHistoricoToActivo : false,
                ArquivarREQPendentes = data.ArquivarREQPendentes.HasValue ? data.ArquivarREQPendentes : false
            });

            data.IdUser = ObjectCreated.IdUtilizador;

            //Add Accesses
            data.UserAccesses.ForEach(x =>
            {
                DBUserAccesses.Create(new AcessosUtilizador()
                {
                    IdUtilizador = ObjectCreated.IdUtilizador,
                    //Área = x.Area,
                    Funcionalidade = x.Feature,
                    Inserção = x.Create,
                    Leitura = x.Read,
                    Modificação = x.Update,
                    Eliminação = x.Delete,
                    UtilizadorCriação = User.Identity.Name
                });
            });

            //Add Profiles
            data.UserProfiles.ForEach(x =>
            {
                DBUserProfiles.Create(new PerfisUtilizador()
                {
                    IdUtilizador = ObjectCreated.IdUtilizador,
                    IdPerfil = x.Id,
                    UtilizadorCriação = User.Identity.Name
                });
            });

            return Json(data);
        }

        [HttpPost]
        public JsonResult UpdateUserConfig([FromBody] UserConfigurationsViewModel data)
        {
            //Update UserConfig
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(data.IdUser);
            if (userConfig == null)
            {
                data.eReasonCode = 1;
                data.eMessage = "Não foi possivel obter o utilizador.";
            }
            else
            {
                userConfig.IdUtilizador = data.IdUser;
                userConfig.Nome = data.Name;
                userConfig.Ativo = data.Active.HasValue ? data.Active.Value : false;
                userConfig.Administrador = data.Administrator;
                userConfig.RegiãoPorDefeito = data.Regiao;
                userConfig.AreaPorDefeito = data.Area;
                userConfig.CentroRespPorDefeito = data.Cresp;
                userConfig.EmployeeNo = data.EmployeeNo;
                userConfig.DataHoraModificação = DateTime.Now;
                userConfig.ProcedimentosEmailEnvioParaCa = data.ProcedimentosEmailEnvioParaCA;
                userConfig.ProcedimentosEmailEnvioParaArea = data.ProcedimentosEmailEnvioParaArea;
                userConfig.ProcedimentosEmailEnvioParaArea2 = data.ProcedimentosEmailEnvioParaArea2;
                userConfig.UtilizadorModificação = User.Identity.Name;
                userConfig.PerfilNumeraçãoRecDocCompras = data.ReceptionConfig;
                userConfig.Rfperfil = data.RFPerfil.HasValue ? (int)data.RFPerfil : (int?)null;
                userConfig.RfperfilVisualizacao = data.RFPerfilVisualizacao.HasValue ? (int)data.RFPerfilVisualizacao : (int?)null;
                userConfig.RffiltroArea = data.RFFiltroArea;
                userConfig.RfnomeAbreviado = data.RFNomeAbreviado;
                userConfig.RfrespostaContabilidade = data.RFRespostaContabilidade;
                userConfig.RfalterarDestinatarios = data.RFAlterarDestinatarios;
                userConfig.RfmailEnvio = data.RFMailEnvio;
                userConfig.NumSerieFaturas = data.NumSerieFaturas;
                userConfig.NumSeriePreFaturasCompraCf = data.NumSeriePreFaturasCompraCF;
                userConfig.NumSeriePreFaturasCompraCp = data.NumSeriePreFaturasCompraCP;
                userConfig.NumSerieNotasCreditoCompra = data.NumSerieNotasCreditoCompra;
                userConfig.NumSerieNotasCredito = data.NumSerieNotasCredito;
                userConfig.NumSerieNotasDebito = data.NumSerieNotasDebito;
                userConfig.CentroDeResponsabilidade = data.Centroresp;
                userConfig.SuperiorHierarquico = data.SuperiorHierarquico;
                userConfig.RequisicaoStock = data.RequisicaoStock.HasValue ? data.RequisicaoStock : false;
                userConfig.AprovadorPedidoPag1 = data.AprovadorPedidoPag1;
                userConfig.AprovadorPedidoPag2 = data.AprovadorPedidoPag2;
                userConfig.AnulacaoPedidoPagamento = data.AnulacaoPedidoPagamento.HasValue ? data.AnulacaoPedidoPagamento : false;
                userConfig.CriarProjetoSemAprovacao = data.CriarProjetoSemAprovacao.HasValue ? data.CriarProjetoSemAprovacao : false;
                userConfig.CMHistoricoToActivo = data.CMHistoricoToActivo.HasValue ? data.CMHistoricoToActivo : false;
                userConfig.ValidarPedidoPagamento = data.ValidarPedidoPagamento.HasValue ? data.ValidarPedidoPagamento : false;
                userConfig.ArquivarREQPendentes = data.ArquivarREQPendentes.HasValue ? data.ArquivarREQPendentes : false;

                DBUserConfigurations.Update(userConfig);

                #region Update Accesses

                //Get Existing from db
                var userAccesses = DBUserAccesses.GetByUserId(data.IdUser);

                //Get items to delete (for changed keys delete old, create new)
                var userAccessesToDelete = userAccesses
                    .Where(x => !data.UserAccesses
                        .Any(y => y.Feature == x.Funcionalidade))
                    .ToList();
                //Delete 
                if (userAccessesToDelete.Count > 0)
                {
                    bool uaSuccessfullyDeleted = DBUserAccesses.Delete(userAccessesToDelete);

                    if (!uaSuccessfullyDeleted)
                    {
                        data.eMessage = "Ocorreu um erro ao eliminar os acessos do utilizador.";
                    }
                }

                //Create (for changed keys) or Update existing
                data.UserAccesses.ForEach(userAccess =>
                    {
                        var updatedUA = userAccesses.SingleOrDefault(x => x.Funcionalidade == userAccess.Feature);

                        if (updatedUA == null)
                        {
                            //Create
                            updatedUA = new AcessosUtilizador()
                            {
                                IdUtilizador = data.IdUser,
                                //Área = userAccess.Area,
                                Funcionalidade = userAccess.Feature,
                                UtilizadorCriação = User.Identity.Name,
                                DataHoraCriação = DateTime.Now
                            };
                            updatedUA = DBUserAccesses.Create(updatedUA);
                        }
                        //Update
                        updatedUA.Eliminação = userAccess.Delete.HasValue ? userAccess.Delete.Value : false;
                        updatedUA.Inserção = userAccess.Create.HasValue ? userAccess.Create.Value : false;
                        updatedUA.Leitura = userAccess.Read.HasValue ? userAccess.Read.Value : false;
                        updatedUA.Modificação = userAccess.Update.HasValue ? userAccess.Update.Value : false;

                        updatedUA.UtilizadorModificação = User.Identity.Name;
                        updatedUA.DataHoraModificação = DateTime.Now;

                        DBUserAccesses.Update(updatedUA);
                    }
                );
                #endregion

                #region Update Profiles

                //Get Existing from db
                var userProfiles = DBUserProfiles.GetByUserId(data.IdUser);

                //Get items to delete (for changed keys delete old, create new)
                var userProfilesToDelete = userProfiles
                    .Where(x => !data.UserProfiles
                        .Any(y => y.Id == x.IdPerfil))
                    .ToList();

                //Delete 
                if (userProfilesToDelete.Count > 0)
                {
                    bool upSuccessfullyDeleted = DBUserProfiles.Delete(userProfilesToDelete);
                    if (!upSuccessfullyDeleted)
                    {
                        data.eMessage = "Ocorreu um erro ao eliminar os perfis do utilizador.";
                    }
                }

                //Create (for changed keys) or Update existing
                data.UserProfiles.ForEach(userProfile =>
                {
                    var updatedUP = userProfiles.SingleOrDefault(x => x.IdPerfil == userProfile.Id);

                    if (updatedUP == null)
                    {
                        //Create
                        updatedUP = new PerfisUtilizador()
                        {
                            IdUtilizador = data.IdUser,
                            IdPerfil = userProfile.Id,
                            UtilizadorCriação = User.Identity.Name,
                            DataHoraCriação = DateTime.Now
                        };
                        updatedUP = DBUserProfiles.Create(updatedUP);
                    }
                    //Update
                    updatedUP.UtilizadorModificação = User.Identity.Name;
                    updatedUP.DataHoraModificação = DateTime.Now;

                    DBUserProfiles.Update(updatedUP);
                });

                #endregion

                #region Update AllowedUserDimemsions

                //Get Existing from db
                var userDimensions = DBUserDimensions.GetByUserId(data.IdUser);

                //Get items to delete (for changed keys delete old, create new)
                var userDimensionsToDelete = userDimensions
                    .Where(x => !data.AllowedUserDimensions
                        .Any(y => y.Dimension == x.Dimensão &&
                            y.DimensionValue == x.ValorDimensão))
                    .ToList();

                //Delete 
                if (userDimensionsToDelete.Count > 0)
                {
                    bool udSuccessfullyDeleted = DBUserDimensions.Delete(userDimensionsToDelete);
                    if (!udSuccessfullyDeleted)
                    {
                        data.eMessage = "Ocorreu um erro ao eliminar as dimensões permitidas ao utilizador.";
                    }
                }

                //Create (for changed keys) or Update existing
                data.AllowedUserDimensions.ForEach(userDimension =>
                {
                    var updatedUD = userDimensions.SingleOrDefault(x => x.Dimensão == userDimension.Dimension &&
                        x.ValorDimensão == userDimension.DimensionValue);

                    if (updatedUD == null)
                    {
                        //Create
                        updatedUD = new AcessosDimensões()
                        {
                            IdUtilizador = data.IdUser,
                            Dimensão = userDimension.Dimension,
                            ValorDimensão = userDimension.DimensionValue,
                            UtilizadorCriação = User.Identity.Name,
                            DataHoraCriação = DateTime.Now
                        };
                        updatedUD = DBUserDimensions.Create(updatedUD);
                    }
                    //Update
                    updatedUD.UtilizadorModificação = User.Identity.Name;
                    updatedUD.DataHoraModificação = DateTime.Now;

                    DBUserDimensions.Update(updatedUD);
                });

                #endregion
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult DeleteUserConfig([FromBody] UserConfigurationsViewModel data)
        {
            ConfigUtilizadores UCObj = DBUserConfigurations.GetById(data.IdUser);

            //Remover os acessos os acessos
            DBUserAccesses.DeleteAllFromUser(data.IdUser);

            TabelaLog TabLog_AU = new TabelaLog
            {
                Tabela = "[Acessos Utilizador]",
                Descricao = "Delete - [Id Utilizador]: " + data.IdUser.ToString(),
                Utilizador = User.Identity.Name,
                DataHora = DateTime.Now
            };
            DBTabelaLog.Create(TabLog_AU);


            //Remover os acessos às dimensões
            DBUserDimensions.DeleteAllFromUser(data.IdUser);

            TabelaLog TabLog_UD = new TabelaLog
            {
                Tabela = "[dbo].[Acessos Dimensões]",
                Descricao = "Delete - [Id Utilizador]: " + data.IdUser.ToString(),
                Utilizador = User.Identity.Name,
                DataHora = DateTime.Now
            };
            DBTabelaLog.Create(TabLog_UD);

            UCObj.Ativo = false;

            DBUserConfigurations.Update(UCObj);
            return Json(data);
        }

        [HttpPost]
        public JsonResult CreateUserDimension([FromBody] UserDimensionsViewModel data)
        {
            bool result = false;
            try
            {
                AcessosDimensões userDimension = new AcessosDimensões();
                userDimension.UtilizadorCriação = User.Identity.Name;
                userDimension.DataHoraCriação = DateTime.Now;
                userDimension.IdUtilizador = data.UserId;
                userDimension.Dimensão = data.Dimension;
                userDimension.ValorDimensão = data.DimensionValue;

                var dbCreateResult = DBUserDimensions.Create(userDimension);
                result = dbCreateResult != null ? true : false;
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteUserDimension([FromBody] UserDimensionsViewModel data)
        {
            var userDimension = DBUserDimensions.GetById(data.UserId, data.Dimension, data.DimensionValue);
            return Json(userDimension != null ? DBUserDimensions.Delete(userDimension) : false);
        }

        [HttpPost]
        public JsonResult CreateUserAcessosLocalizacoes([FromBody] AcessosLocalizacoes data)
        {
            bool result = false;
            try
            {
                AcessosLocalizacoes userAcessosLocalizacoes = new AcessosLocalizacoes();
                userAcessosLocalizacoes.IdUtilizador = data.IdUtilizador;
                userAcessosLocalizacoes.Localizacao = data.Localizacao;
                userAcessosLocalizacoes.UtilizadorCriacao = User.Identity.Name;
                userAcessosLocalizacoes.DataHoraCriacao = DateTime.Now;

                var dbCreateResult = DBAcessosLocalizacoes.Create(userAcessosLocalizacoes);
                result = dbCreateResult != null ? true : false;
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteUserAcessosLocalizacoes([FromBody] AcessosLocalizacoes data)
        {
            var userAcessosLocalizacoes = DBAcessosLocalizacoes.GetById(data.IdUtilizador, data.Localizacao);
            return Json(userAcessosLocalizacoes != null ? DBAcessosLocalizacoes.Delete(userAcessosLocalizacoes) : false);
        }

        [HttpPost]
        public JsonResult CreateUserAccess([FromBody] UserAccessesViewModel data)
        {
            bool result = false;
            try
            {
                AcessosUtilizador userAccess = new AcessosUtilizador();
                userAccess.IdUtilizador = data.IdUser;
                userAccess.Área = 1;
                userAccess.Funcionalidade = data.Feature;
                userAccess.Eliminação = data.Delete;
                userAccess.Inserção = data.Create;
                userAccess.Leitura = data.Read;
                userAccess.Modificação = data.Update;
                userAccess.UtilizadorCriação = User.Identity.Name;
                userAccess.DataHoraCriação = DateTime.Now;

                var dbCreateResult = DBUserAccesses.Create(userAccess);
                result = dbCreateResult != null ? true : false;
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateUserGruposAprovacao([FromBody] ApprovalUserGroupViewModel data)
        {
            UtilizadoresGruposAprovação NewUserGrupo = new UtilizadoresGruposAprovação
            {
                DataHoraCriação = DateTime.Now,
                UtilizadorCriação = User.Identity.Name,
                EnviarEmailAlerta = data.EnviarEmailAlerta,
                GrupoAprovação = data.ApprovalGroup,
                UtilizadorAprovação = data.ApprovalUser
            };

            if (DBApprovalUserGroup.Create(NewUserGrupo) != null)
                return Json(true);
            else
                return Json(false);
        }

        [HttpPost]
        public JsonResult DeleteUserGruposAprovacao([FromBody] ApprovalUserGroupViewModel data)
        {
            UtilizadoresGruposAprovação UserGrupo = DBApprovalUserGroup.GetById(data.ApprovalGroup, data.ApprovalUser);
            bool result = DBApprovalUserGroup.Delete(UserGrupo);

            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteUserProfile([FromBody] UserProfileViewModel data)
        {
            var userProfile = DBUserProfiles.GetById(data.UserId, data.Id);
            return Json(userProfile != null ? DBUserProfiles.Delete(userProfile) : false);
        }

        [HttpPost]
        public JsonResult CreateUserProfile([FromBody] UserProfileViewModel data)
        {
            bool result = false;
            try
            {
                PerfisUtilizador userProfile = new PerfisUtilizador();
                userProfile.IdUtilizador = data.UserId;
                userProfile.IdPerfil = data.Id;
                userProfile.UtilizadorCriação = User.Identity.Name;
                userProfile.DataHoraCriação = DateTime.Now;

                var dbCreateResult = DBUserProfiles.Create(userProfile);
                result = dbCreateResult != null ? true : false;
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteUserAccess([FromBody] UserAccessesViewModel data)
        {
            var userAccess = DBUserAccesses.GetById(data.IdUser, data.Feature);
            return Json(userAccess != null ? DBUserAccesses.Delete(userAccess) : false);
        }

        [HttpPost]
        public JsonResult CopiarAcessosUtilizador([FromBody] AccessProfileModelView data)
        {
            ErrorHandler result = new ErrorHandler();
            int AcessosCopiados = 0;
            int AcessosDimensoesCopiados = 0;
            int LocalizacoesCopiados = 0;
            string IdUtilizadorOriginal = data.CreateUser; // "nunorato@such.pt";
            string IdUtilizadorDestino = data.UpdateUser; // "ARomao@such.pt";

            //COPIAR Acessos Utilizador
            List<AcessosUtilizador> ListaAcessosOriginal = DBUserAccesses.GetByUserId(IdUtilizadorOriginal);
            List<AcessosUtilizador> ListaAcessosDestino = DBUserAccesses.GetByUserId(IdUtilizadorDestino);

            ListaAcessosOriginal.ForEach(Acesso =>
            {
                if (ListaAcessosDestino.Where(x => x.Funcionalidade == Acesso.Funcionalidade).Count() == 0)
                {
                    AcessosUtilizador CopiarAcesso = new AcessosUtilizador();
                    CopiarAcesso.IdUtilizador = IdUtilizadorDestino;
                    CopiarAcesso.Área = Acesso.Área;
                    CopiarAcesso.Funcionalidade = Acesso.Funcionalidade;
                    CopiarAcesso.Leitura = Acesso.Leitura;
                    CopiarAcesso.Inserção = Acesso.Inserção;
                    CopiarAcesso.Modificação = Acesso.Modificação;
                    CopiarAcesso.Eliminação = Acesso.Eliminação;
                    CopiarAcesso.DataHoraCriação = DateTime.Now;
                    CopiarAcesso.DataHoraModificação = (DateTime?)null;
                    CopiarAcesso.UtilizadorCriação = User.Identity.Name;
                    CopiarAcesso.UtilizadorModificação = null;

                    if (DBUserAccesses.Create(CopiarAcesso) != null)
                        AcessosCopiados = AcessosCopiados + 1;
                }
            });

            //COPIAR Acessos Dimensões
            List<AcessosDimensões> ListaAcessosDimensoesOriginal = DBUserDimensions.GetByUserId(IdUtilizadorOriginal);
            List<AcessosDimensões> ListaAcessosDimensoesDestino = DBUserDimensions.GetByUserId(IdUtilizadorDestino);

            ListaAcessosDimensoesOriginal.ForEach(AcessoDimensao =>
            {
                if (ListaAcessosDimensoesDestino.Where(x => x.Dimensão == AcessoDimensao.Dimensão && x.ValorDimensão == AcessoDimensao.ValorDimensão).Count() == 0)
                {
                    AcessosDimensões CopiarAcessoDimensao = new AcessosDimensões();
                    CopiarAcessoDimensao.IdUtilizador = IdUtilizadorDestino;
                    CopiarAcessoDimensao.Dimensão = AcessoDimensao.Dimensão;
                    CopiarAcessoDimensao.ValorDimensão = AcessoDimensao.ValorDimensão;
                    CopiarAcessoDimensao.DataHoraCriação = DateTime.Now;
                    CopiarAcessoDimensao.UtilizadorCriação = User.Identity.Name;
                    CopiarAcessoDimensao.DataHoraModificação = (DateTime?)null;
                    CopiarAcessoDimensao.UtilizadorModificação = null;

                    if (DBUserDimensions.Create(CopiarAcessoDimensao) != null)
                        AcessosDimensoesCopiados = AcessosDimensoesCopiados + 1;
                }
            });

            //COPIAR Acessos Localizações
            List<AcessosLocalizacoes> ListaLocalizacoesOriginal = DBAcessosLocalizacoes.GetByUserId(IdUtilizadorOriginal);
            List<AcessosLocalizacoes> ListaLocalizacoesDestino = DBAcessosLocalizacoes.GetByUserId(IdUtilizadorDestino);

            ListaLocalizacoesOriginal.ForEach(Localizacao =>
            {
                if (ListaLocalizacoesDestino.Where(x => x.Localizacao == Localizacao.Localizacao).Count() == 0)
                {
                    AcessosLocalizacoes CopiarLocalizacao = new AcessosLocalizacoes();
                    CopiarLocalizacao.IdUtilizador = IdUtilizadorDestino;
                    CopiarLocalizacao.Localizacao = Localizacao.Localizacao;
                    CopiarLocalizacao.DataHoraCriacao = DateTime.Now;
                    CopiarLocalizacao.UtilizadorCriacao = User.Identity.Name;
                    CopiarLocalizacao.DataHoraModificacao = (DateTime?)null;
                    CopiarLocalizacao.UtilizadorModificacao = null;

                    if (DBAcessosLocalizacoes.Create(CopiarLocalizacao) != null)
                        LocalizacoesCopiados = LocalizacoesCopiados + 1;
                }
            });

            result.eReasonCode = 1;
            result.eMessage = "Foram copiados com sucesso " + AcessosCopiados.ToString() + " Acessos Utilizador, " + AcessosDimensoesCopiados.ToString() + " Acessos Dimensõess e " + LocalizacoesCopiados.ToString() + " Acessos Localizações.";

            return Json(result);
        }

        #endregion

        #region PerfisModelo
        public IActionResult PerfisModelo()
        {
            //UserAccessesViewModel UPerm = GetPermissions("Administracao");
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminGeral);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.UPermissions = userPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetListProfileModels()
        {
            List<ProfileModelsViewModel> result = DBProfileModels.GetAll().Select(x => new ProfileModelsViewModel()
            {
                Id = x.Id,
                Description = x.Descrição
            }).ToList();
            return Json(result);
        }


        public IActionResult PerfisModeloDetalhes(int id)
        {
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminGeral);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.ProfileModelId = id;
                ViewBag.UPermissions = userPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetProfileModelData([FromBody] ProfileModelsViewModel data)
        {
            PerfisModelo PM = DBProfileModels.GetById(data.Id);
            ProfileModelsViewModel result = new ProfileModelsViewModel()
            {
                Id = 0,
                Description = "",
                ProfileModelAccesses = new List<AccessProfileModelView>()
            };

            if (PM != null)
            {
                result.Id = PM.Id;
                result.Description = PM.Descrição;

                result.ProfileModelAccesses = DBAccessProfiles.GetByProfileModelId(data.Id).Select(x => new AccessProfileModelView()
                {
                    IdProfile = x.IdPerfil,
                    //Area = x.Área,
                    Feature = x.Funcionalidade,
                    Create = x.Inserção,
                    Read = x.Leitura,
                    Update = x.Modificação,
                    Delete = x.Eliminação
                }).ToList();
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateProfileModel([FromBody] ProfileModelsViewModel data)
        {
            PerfisModelo ObjectCreated = DBProfileModels.Create(new PerfisModelo()
            {
                Descrição = data.Description,
                UtilizadorCriação = User.Identity.Name
            });
            data.Id = ObjectCreated.Id;

            //Adicionar os acessos
            data.ProfileModelAccesses.ForEach(x =>
            {
                DBAccessProfiles.Create(new AcessosPerfil()
                {
                    IdPerfil = ObjectCreated.Id,
                    //Área = x.Area,
                    Funcionalidade = x.Feature,
                    Inserção = x.Create,
                    Leitura = x.Read,
                    Modificação = x.Update,
                    Eliminação = x.Delete,
                    UtilizadorCriação = User.Identity.Name
                });
            });
            return Json(data);
        }

        [HttpPost]
        public JsonResult UpdateProfileModel([FromBody] ProfileModelsViewModel data)
        {
            //Atualizar o elemento os acessos
            PerfisModelo PMObj = DBProfileModels.GetById(data.Id);
            PMObj.Descrição = data.Description;
            PMObj.UtilizadorModificação = User.Identity.Name;
            DBProfileModels.Update(PMObj);

            //Atualizar os acessos
            DBAccessProfiles.DeleteAllFromProfile(data.Id);
            data.ProfileModelAccesses.ForEach(x =>
            {
                DBAccessProfiles.Create(new AcessosPerfil()
                {
                    IdPerfil = data.Id,
                    //Área = x.Area,
                    Funcionalidade = x.Feature,
                    Inserção = x.Create,
                    Leitura = x.Read,
                    Modificação = x.Update,
                    Eliminação = x.Delete,
                    UtilizadorCriação = User.Identity.Name
                });
            });
            return Json(data);
        }

        [HttpPost]
        public JsonResult DeleteProfileModel([FromBody] ProfileModelsViewModel data)
        {
            PerfisModelo PMObj = DBProfileModels.GetById(data.Id);

            //Remover os acessos os acessos
            DBAccessProfiles.DeleteAllFromProfile(data.Id);

            if (DBProfileModels.Delete(PMObj))
            {
                data.Id = 0;
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult DeleteAccess([FromBody] AccessProfileModelView data)
        {
            if (data != null)
            {
                if (DBAccessProfiles.Delete(data.ParseToDB()))
                {
                    data.eReasonCode = 1;
                    data.eMessage = "Registo eliminado com sucesso.";
                }
                else
                {
                    data.eReasonCode = 2;
                    data.eMessage = "Ocorreu um erro ao eliminar o registo.";
                }
            }
            else
            {
                data = new AccessProfileModelView();
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao eliminar o registo.";
            }

            return Json(data);
        }
        #endregion

        public IActionResult Permicoes()
        {
            return View();
        }

        #region Configuracoes

        public IActionResult Configuracoes()
        {
            //UserAccessesViewModel UPerm= GetPermissions("Administracao");
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminGeral);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.UPermissions = userPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetListConfigurations()
        {
            Configuração Cfg = DBConfigurations.GetById(1);

            ConfigurationsViewModel result = new ConfigurationsViewModel()
            {
                Id = Cfg.Id,
                ProjectNumeration = Cfg.NumeraçãoProjetos,
                ContractNumeration = Cfg.NumeraçãoContratos,
                TimeSheetNumeration = Cfg.NumeraçãoFolhasDeHoras,
                OportunitiesNumeration = Cfg.NumeraçãoOportunidades,
                ProposalsNumeration = Cfg.NumeraçãoPropostas,
                ContactsNumeration = Cfg.NumeraçãoContactos,
                DishesTechnicalSheetsNumeration = Cfg.NumeraçãoFichasTécnicasDePratos,
                PreRequisitionNumeration = Cfg.NumeraçãoPréRequisições,
                PurchasingProceduresNumeration = Cfg.NumeraçãoProcedimentoAquisição,
                RequisitionNumeration = Cfg.NumeraçãoRequisições,
                SimplifiedProceduresNumeration = Cfg.NumeraçãoProcedimentoSimplificado,
                SimplifiedReqTemplatesNumeration = Cfg.NumeraçãoModReqSimplificadas,
                SimplifiedRequisitionNumeration = Cfg.NumeraçãoRequisiçõesSimplificada,
                ProdutosNumeration = Cfg.NumeracaoProdutos,
                ConsultaMercadoNumeration = Cfg.ConsultaMercado,
                DinnerEndTime = Cfg.FimHoraJantar,
                DinnerStartTime = Cfg.InicioHoraJantar,
                LunchEndTime = Cfg.FimHoraAlmoco,
                LunchStartTime = Cfg.InicioHoraAlmoco,
                WasteAreaId = Cfg.CodAreaResiduos,
                ReportUsername = Cfg.ReportUsername,
                ReportPassword = Cfg.ReportPassword,
                ArmazemCompraDireta = Cfg.ArmazemCompraDireta
            };
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateConfigurations([FromBody] ConfigurationsViewModel data)
        {
            Configuração configObj = DBConfigurations.GetById(data.Id);


            if (configObj == null)
            {
                configObj.DataHoraCriação = DateTime.Now;
                configObj.UtilizadorCriação = User.Identity.Name;
            }

            configObj.NumeraçãoProjetos = data.ProjectNumeration;
            configObj.NumeraçãoContratos = data.ContractNumeration;
            configObj.NumeraçãoFolhasDeHoras = data.TimeSheetNumeration;
            configObj.NumeraçãoOportunidades = data.OportunitiesNumeration;
            configObj.NumeraçãoPropostas = data.ProposalsNumeration;
            configObj.NumeraçãoContactos = data.ContactsNumeration;
            configObj.NumeraçãoFichasTécnicasDePratos = data.DishesTechnicalSheetsNumeration;
            configObj.NumeraçãoPréRequisições = data.PreRequisitionNumeration;
            configObj.NumeraçãoProcedimentoAquisição = data.PurchasingProceduresNumeration;
            configObj.NumeraçãoRequisições = data.RequisitionNumeration;
            configObj.NumeraçãoProcedimentoSimplificado = data.SimplifiedProceduresNumeration;
            configObj.NumeraçãoModReqSimplificadas = data.SimplifiedReqTemplatesNumeration;
            configObj.NumeraçãoRequisiçõesSimplificada = data.SimplifiedRequisitionNumeration;
            configObj.NumeracaoProdutos = data.ProdutosNumeration;
            configObj.ConsultaMercado = data.ConsultaMercadoNumeration;
            configObj.FimHoraJantar = data.DinnerEndTime;
            configObj.InicioHoraJantar = data.DinnerStartTime;
            configObj.InicioHoraAlmoco = data.LunchStartTime;
            configObj.FimHoraAlmoco = data.LunchEndTime;
            configObj.CodAreaResiduos = data.WasteAreaId;
            configObj.ReportUsername = data.ReportUsername;
            configObj.ReportPassword = data.ReportPassword;
            configObj.ArmazemCompraDireta = data.ArmazemCompraDireta;

            configObj.UtilizadorModificação = User.Identity.Name;
            //configObj.UtilizadorCriação = User.Identity.Name;
            //configObj.DataHoraCriação = DateTime.Now;
            configObj.UtilizadorModificação = User.Identity.Name;
            configObj.DataHoraModificação = DateTime.Now;

            DBConfigurations.Update(configObj);

            return Json(data);
        }

        #endregion

        #region ConfiguracaoNumeracoes

        public IActionResult ConfiguracaoNumeracoes()
        {
            //UserAccessesViewModel UPerm = GetPermissions("Administracao");
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminGeral);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.UPermissions = userPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetListConfigNumerations()
        {
            List<ConfigNumerationsViewModel> result = DBNumerationConfigurations.GetAll().Select(x => new ConfigNumerationsViewModel()
            {
                Id = x.Id,
                Auto = x.Automático,
                Manual = x.Manual,
                Prefix = x.Prefixo,
                Description = x.Descrição,
                TotalDigitIncrement = x.NºDígitosIncrementar,
                IncrementQuantity = x.QuantidadeIncrementar,
                LastNumerationUsed = x.ÚltimoNºUsado
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateNumerationConfigs([FromBody] List<ConfigNumerationsViewModel> data)
        {
            //Get All
            List<ConfiguraçãoNumerações> previousList = DBNumerationConfigurations.GetAll();
            //previousList.RemoveAll(x => !data.Any(u => u.Id == x.Id));
            //previousList.ForEach(x => DBNumerationConfigurations.Delete(x));

            foreach (ConfiguraçãoNumerações line in previousList)
            {
                if (!data.Any(x => x.Id == line.Id))
                {
                    DBNumerationConfigurations.Delete(line);
                }
            }

            data.ForEach(x =>
            {
                ConfiguraçãoNumerações CN = new ConfiguraçãoNumerações()
                {
                    Descrição = x.Description,
                    Automático = x.Auto,
                    Manual = x.Manual,
                    Prefixo = x.Prefix,
                    NºDígitosIncrementar = x.TotalDigitIncrement,
                    QuantidadeIncrementar = x.IncrementQuantity,
                    ÚltimoNºUsado = x.LastNumerationUsed
                };

                if (x.Id > 0)
                {
                    CN.Id = x.Id;
                    CN.UtilizadorModificação = User.Identity.Name;
                    CN.DataHoraModificação = DateTime.Now;
                    DBNumerationConfigurations.Update(CN);
                }
                else
                {
                    CN.UtilizadorCriação = User.Identity.Name;
                    CN.DataHoraCriação = DateTime.Now;
                    DBNumerationConfigurations.Create(CN);
                }
            });

            return Json(data);
        }
        #endregion


        #region ConfiguracaoMenus

        public IActionResult ConfiguracaoMenu()
        {
            //UserAccessesViewModel UPerm = GetPermissions("Administracao");
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminGeral);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.UPermissions = userPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public ActionResult GetListConfigMenu()
        {
            List<Data.Database.Menu> result = DBMenu.GetAllFull();
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateMenuConfigs([FromBody] List<Data.Database.Menu> data)
        {


            //Get All
            List<Data.Database.Menu> previousList = DBMenu.GetAll();
            //previousList.RemoveAll(x => !data.Any(u => u.Id == x.Id));
            //previousList.ForEach(x => DBNumerationConfigurations.Delete(x));

            foreach (Data.Database.Menu line in previousList)
            {
                if (!data.Any(x => x.Id == line.Id))
                {
                    DBMenu.Delete(line);
                }
            }

            data.ForEach(x =>
            {
                if (x.Id > 0)
                {
                    x.UpdatedBy = User.Identity.Name;
                    x.UpdatedAt = DateTime.Now;
                    DBMenu.Update(x);
                }
                else
                {
                    x.CreatedBy = User.Identity.Name;
                    x.CreatedAt = DateTime.Now;
                    DBMenu.Create(x);
                }
            });

            return Json(data);
        }
        #endregion


        #region TabelasAuxiliares

        #region TiposDeProjeto
        public IActionResult TiposProjetoDetalhes(string id)
        {

            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminProjetos);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetProjectTypeData()
        {
            List<ProjectTypesModelView> result = DBProjectTypes.GetAll().Select(x => new ProjectTypesModelView()
            {
                Code = x.Código,
                Description = x.Descrição
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateProjectType([FromBody] List<ProjectTypesModelView> data)
        {
            List<TipoDeProjeto> results = DBProjectTypes.GetAll();
            results.RemoveAll(x => data.Any(u => u.Code == x.Código));
            results.ForEach(x => DBProjectTypes.Delete(x));
            data.ForEach(x =>
            {
                TipoDeProjeto tpval = new TipoDeProjeto()
                {
                    Descrição = x.Description
                };
                if (x.Code > 0)
                {
                    tpval.Código = x.Code;
                    tpval.DataHoraModificação = DateTime.Now;
                    tpval.UtilizadorModificação = User.Identity.Name;
                    DBProjectTypes.Update(tpval);
                }
                else
                {
                    tpval.DataHoraCriação = DateTime.Now;
                    tpval.UtilizadorCriação = User.Identity.Name;
                    DBProjectTypes.Create(tpval);
                }
            });
            return Json(data);
        }
        #endregion

        #region TiposGrupoContabProjeto
        public IActionResult TiposGrupoContabProjeto(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminProjetos);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        //POPULATE GRID ContabGroupTypes
        public JsonResult GetTiposGrupoContabProjeto([FromBody] ContabGroupTypesProjectView data)
        {
            List<ContabGroupTypesProjectView> result = DBCountabGroupTypes.GetAll().Select(x => new ContabGroupTypesProjectView()
            {
                ID = x.Código,
                Description = x.Descrição,
                Description2 = x.Descricao2,
                FunctionalAreaCode = x.CódigoÁreaFuncional,
                DataHoraCriacao = x.DataHoraCriação,
                UtilizadorCriacao = x.UtilizadorCriação,
                DataHoraModificacao = x.DataHoraModificação,
                UtilizadorModificacao = x.UtilizadorModificação
            }).ToList();

            return Json(result);
        }

        //Create/Update/Delete 
        [HttpPost]
        public JsonResult UpdateTiposGrupoContabProjeto([FromBody] List<ContabGroupTypesProjectView> data)
        {
            //Get All
            List<TiposGrupoContabProjeto> previousList = DBCountabGroupTypes.GetAll();
            previousList.RemoveAll(x => data.Any(u => u.ID == x.Código));
            previousList.ForEach(x => DBCountabGroupTypes.DeleteAllFromProfile(x.Código));

            data.ForEach(x =>
            {
                TiposGrupoContabProjeto CN = new TiposGrupoContabProjeto()
                {
                    Descrição = x.Description,
                    Descricao2 = x.Description2,
                    CódigoÁreaFuncional = x.FunctionalAreaCode
                };

                if (x.ID > 0)
                {
                    CN.DataHoraCriação = x.DataHoraCriacao;
                    CN.UtilizadorCriação = x.UtilizadorCriacao;
                    CN.DataHoraModificação = DateTime.Now;
                    CN.UtilizadorModificação = User.Identity.Name;
                    CN.Código = x.ID;
                    DBCountabGroupTypes.Update(CN);
                }
                else
                {
                    CN.UtilizadorCriação = User.Identity.Name;
                    CN.DataHoraCriação = DateTime.Now;
                    DBCountabGroupTypes.Create(CN);
                }
            });

            return Json(data);
        }
        #endregion

        #region ObjetosDeServiço

        public IActionResult ObjetosDeServico(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminProjetos);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetServiceObjectsData()
        {
            List<ServiceObjectsViewModel> result = DBServiceObjects.GetAll().Select(x => new ServiceObjectsViewModel()
            {
                Code = x.Código,
                Description = x.Descrição,
                Blocked = x.Bloqueado,
                AreaCode = x.CódÁrea
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateServiceObjects([FromBody] List<ServiceObjectsViewModel> data)
        {
            List<ObjetosDeServiço> results = DBServiceObjects.GetAll();
            results.RemoveAll(x => data.Any(u => u.Code == x.Código));
            results.ForEach(x => DBServiceObjects.Delete(x));
            data.ForEach(x =>
            {
                ObjetosDeServiço OS = new ObjetosDeServiço()
                {
                    Descrição = x.Description,
                    Bloqueado = x.Blocked,
                    CódÁrea = x.AreaCode
                };
                if (x.Code > 0)
                {
                    OS.Código = x.Code;
                    OS.DataHoraModificação = DateTime.Now;
                    OS.UtilizadorModificação = User.Identity.Name;
                    DBServiceObjects.Update(OS);
                }
                else
                {

                    OS.DataHoraCriação = DateTime.Now;
                    OS.UtilizadorCriação = User.Identity.Name;
                    DBServiceObjects.Create(OS);
                }
            });
            return Json(data);
        }
        #endregion

        #region TiposGrupoContabOMProjeto

        public IActionResult TiposGrupoContabOMProjeto(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminProjetos);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public JsonResult GetTiposGrupoContabOMProjeto([FromBody] ContabGroupTypesOMProjectViewModel data)
        {
            List<ContabGroupTypesOMProjectViewModel> result = DBCountabGroupTypesOM.GetAll().Select(x => new ContabGroupTypesOMProjectViewModel()
            {
                Code = x.Código,
                Type = x.Tipo,
                Description = x.Descrição,
                CorrectiveMaintenance = x.ManutCorretiva,
                PreventiveMaintenance = x.ManutPreventiva,
                FailType = x.TipoRazãoFalha,
                ResponseTimeIndicator = x.IndicadorTempoResposta,
                StopTimeIndicator = x.IndicadorTempoImobilização,
                RepairEffectiveTimeIndicator = x.IndicadorTempoEfetivoReparação,
                ClosingWorksTimeIndicator = x.IndicadorTempoFechoObras,
                BillingTimeIndicator = x.IndicadorTempoFaturação,
                EmployeesOccupationTimeIndicator = x.IndicadorTempoOcupColaboradores,
                CostSaleValueIndicator = x.IndicadorValorCustoVenda,
                CATComplianceRateIndicator = x.IndicTaxaCumprimentoCat,
                CATCoverageRateIndicator = x.IndicadorTaxaCoberturaCat,
                MPRoutineFulfillmentRateIndicator = x.IndicTaxaCumprRotinasMp,
                BreakoutIncidentsIndicator = x.IndicIncidênciasAvarias,
                OrdernInProgressIndicator = x.IndicadorOrdensEmCurso
            }).ToList();

            return Json(result);
        }

        //Create/Update/Delete 
        [HttpPost]
        public JsonResult UpdateTiposGrupoContabProjetoOM([FromBody] List<ContabGroupTypesOMProjectViewModel> data)
        {
            //Get All
            List<TiposGrupoContabOmProjeto> previousList = DBCountabGroupTypesOM.GetAll();
            previousList.RemoveAll(x => data.Any(u => u.Code == x.Código));
            previousList.ForEach(x => DBCountabGroupTypesOM.DeleteAllFromProfile(x));
            data.ForEach(x =>
            {
                TiposGrupoContabOmProjeto CN = new TiposGrupoContabOmProjeto()
                {
                    Código = x.Code,
                    Tipo = x.Type,
                    Descrição = x.Description,
                    ManutCorretiva = x.CorrectiveMaintenance,
                    ManutPreventiva = x.PreventiveMaintenance,
                    TipoRazãoFalha = x.FailType,
                    IndicadorTempoResposta = x.ResponseTimeIndicator,
                    IndicadorTempoImobilização = x.StopTimeIndicator,
                    IndicadorTempoEfetivoReparação = x.RepairEffectiveTimeIndicator,
                    IndicadorTempoFechoObras = x.ClosingWorksTimeIndicator,
                    IndicadorTempoFaturação = x.BillingTimeIndicator,
                    IndicadorTempoOcupColaboradores = x.EmployeesOccupationTimeIndicator,
                    IndicadorValorCustoVenda = x.CostSaleValueIndicator,
                    IndicTaxaCumprimentoCat = x.CATComplianceRateIndicator,
                    IndicadorTaxaCoberturaCat = x.CATCoverageRateIndicator,
                    IndicTaxaCumprRotinasMp = x.MPRoutineFulfillmentRateIndicator,
                    IndicIncidênciasAvarias = x.BreakoutIncidentsIndicator,
                    IndicadorOrdensEmCurso = x.OrdernInProgressIndicator
                };

                if (x.Code > 0)
                {
                    CN.DataHoraModificação = DateTime.Now;
                    CN.UtilizadorModificação = User.Identity.Name;
                    CN.Código = x.Code;
                    DBCountabGroupTypesOM.Update(CN);
                }
                else
                {
                    CN.UtilizadorCriação = User.Identity.Name;
                    CN.DataHoraCriação = DateTime.Now;
                    DBCountabGroupTypesOM.Create(CN);
                }
            });

            return Json(data);
        }
        #endregion TiposGrupoContabOMProjeto

        #region TiposRefeicao
        public IActionResult TiposRefeicao(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminNutricao);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetMealTypesData()
        {
            List<MealTypesViewModel> result = DBMealTypes.GetAll().Select(x => new MealTypesViewModel()
            {
                Code = x.Código,
                Description = x.Descrição,
                GrupoContabProduto = x.GrupoContabProduto,
                GrupoContabProdutoText = DBNAV2017GruposContabilisticos.GetGruposContabProduto(_config.NAVDatabaseName, _config.NAVCompanyName).Where(y => y.Code == x.GrupoContabProduto).Count() > 0 ? DBNAV2017GruposContabilisticos.GetGruposContabProduto(_config.NAVDatabaseName, _config.NAVCompanyName).Where(y => y.Code == x.GrupoContabProduto).FirstOrDefault().Description : "",
            }).ToList();
            return Json(result);
        }
        [HttpPost]
        public JsonResult GeAlltMealTypes()
        {
            List<MealTypesViewModel> result = DBMealTypes.GetAll().Select(x => new MealTypesViewModel()
            {
                id = Convert.ToString(x.Código),
                Code = x.Código,
                Description = x.Descrição,
                GrupoContabProduto = x.GrupoContabProduto
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateMealTypes([FromBody] List<MealTypesViewModel> data)
        {
            List<TiposRefeição> results = DBMealTypes.GetAll();
            results.RemoveAll(x => data.Any(u => u.Code == x.Código));
            results.ForEach(x => DBMealTypes.Delete(x));
            data.ForEach(x =>
            {
                TiposRefeição TR = new TiposRefeição()
                {
                    Descrição = x.Description,
                    GrupoContabProduto = x.GrupoContabProduto
                };
                if (x.Code > 0)
                {
                    TR.Código = x.Code;
                    TR.DataHoraModificação = DateTime.Now;
                    TR.UtilizadorModificação = User.Identity.Name;
                    DBMealTypes.Update(TR);
                }
                else
                {
                    TR.DataHoraCriação = DateTime.Now;
                    TR.UtilizadorCriação = User.Identity.Name;
                    DBMealTypes.Create(TR);
                }
            });
            return Json(data);
        }


        #endregion

        #region DestinosFinaisResiduos
        public IActionResult DestinosFinaisResiduos(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminProjetos);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetFinalWasteDestinationsData()
        {
            List<FinalWasteDestinationsViewModel> result = DBFinalWasteDestinations.GetAll().Select(x => new FinalWasteDestinationsViewModel()
            {
                Code = x.Código,
                Description = x.Descrição
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateFinalWasteDestinations([FromBody] List<FinalWasteDestinationsViewModel> data)
        {
            List<DestinosFinaisResíduos> results = DBFinalWasteDestinations.GetAll();
            results.RemoveAll(x => data.Any(u => u.Code == x.Código));
            results.ForEach(x => DBFinalWasteDestinations.Delete(x));
            data.ForEach(x =>
            {
                DestinosFinaisResíduos DFR = new DestinosFinaisResíduos()
                {
                    Descrição = x.Description
                };
                if (x.Code > 0)
                {
                    DFR.Código = x.Code;
                    DFR.DataHoraModificação = DateTime.Now;
                    DFR.UtilizadorModificação = User.Identity.Name;
                    DBFinalWasteDestinations.Update(DFR);
                }
                else
                {
                    DFR.DataHoraCriação = DateTime.Now;
                    DFR.UtilizadorCriação = User.Identity.Name;
                    DBFinalWasteDestinations.Create(DFR);
                }
            });
            return Json(data);
        }


        #endregion

        #region Serviço
        public IActionResult Servicos(string id)
        {
            List<Enumerations.Features> features = new List<Enumerations.Features>()
            {
                //Enumerations.Features.AdminProjetos,
                //Enumerations.Features.AdminVendas,
                Enumerations.Features.AdminServicos
            };

            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, features);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetServices()
        {
            List<ProjectTypesModelViewStr> result = DBServices.GetAll().Select(x => new ProjectTypesModelViewStr()
            {
                Code = x.Código,
                Description = x.Descrição
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateServices([FromBody] ProjectTypesModelViewStr data)
        {
            List<Serviços> results = DBServices.GetAll();
            Serviços result = DBServices.GetById(data.Code);
            if (result == null)
            {
                Serviços tpval = new Serviços();
                tpval.Descrição = data.Description;
                tpval.Código = data.Code;
                tpval.UtilizadorCriação = User.Identity.Name;
                tpval.DataHoraCriação = DateTime.Now;
                DBServices.Create(tpval);
            }


            return Json(result);
        }
        public JsonResult UpdateServices([FromBody] List<ProjectTypesModelViewStr> data)
        {
            List<Serviços> results = DBServices.GetAll();
            results.RemoveAll(x => data.Any(u => u.Code == x.Código));
            results.ForEach(x => DBServices.Delete(x.Código));
            data.ForEach(x =>
            {
                Serviços tpval = new Serviços()
                {
                    Descrição = x.Description
                };
                if (x.Code != "" && x.Code != null)
                {
                    tpval.DataHoraModificação = DateTime.Now;
                    tpval.UtilizadorModificação = User.Identity.Name;
                    tpval.Código = x.Code;
                    DBServices.Update(tpval);
                }
                else
                {
                    tpval.UtilizadorCriação = User.Identity.Name;
                    tpval.DataHoraCriação = DateTime.Now;
                    DBServices.Create(tpval);
                }
            });
            return Json(data);
        }
        #endregion

        #region ServiçosCliente
        public IActionResult ServicosCliente(string id)
        {
            List<Enumerations.Features> features = new List<Enumerations.Features>()
            {
                //Enumerations.Features.AdminProjetos,
                //Enumerations.Features.AdminVendas,
                Enumerations.Features.AdminServicos
            };

            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, features);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetClientServices()
        {
            List<ClientServicesViewModel> result = DBClientServices.GetAll().Select(x => new ClientServicesViewModel()
            {
                ClientNumber = x.NºCliente,
                ClientName = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, x.NºCliente).Count() > 0 ? DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, x.NºCliente).FirstOrDefault().Name : "",
                ServiceCode = x.CódServiço,
                ServiceDescription = DBServices.GetById(x.CódServiço) != null ? DBServices.GetById(x.CódServiço).Descrição : "",
                //ServiceDescription = x.CódServiçoNavigation != null ? x.CódServiçoNavigation.Descrição : "",
                ServiceGroup = x.GrupoServiços,
                ServiceGroup_Show = x.GrupoServiços.HasValue ? x.GrupoServiços == true ? "Sim" : "Não" : "Não",
                CodGrupoServico = x.CodGrupoServico
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateClientServices([FromBody] List<ClientServicesViewModel> data)
        {
            foreach (var dt in data)
            {
                int param = 2;
                bool exist = CheckIfExist(dt.ClientNumber, dt.ServiceCode, dt.ServiceGroup, param);
                if (exist == false)
                {
                    ServiçosCliente tpval = new ServiçosCliente();
                    tpval.UtilizadorModificação = User.Identity.Name;
                    tpval.DataHoraModificação = DateTime.Now;
                    tpval.GrupoServiços = dt.ServiceGroup;
                    tpval.CódServiço = dt.ServiceCode;
                    tpval.CodGrupoServico = dt.CodGrupoServico;
                    tpval.NºCliente = dt.ClientNumber;

                    DBClientServices.Update(tpval);
                }
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult CreateClientServices([FromBody] List<ClientServicesViewModel> data)
        {
            try
            {

                int totalExists = 0;
                if (data != null)
                {
                    foreach (var dt in data)
                    {
                        int param = 1;
                        bool exist = CheckIfExist(dt.ClientNumber, dt.ServiceCode, dt.ServiceGroup, param);
                        if (exist == false)
                        {
                            ServiçosCliente tpval = new ServiçosCliente();
                            tpval.UtilizadorCriação = User.Identity.Name;
                            tpval.DataHoraCriação = DateTime.Now;
                            tpval.GrupoServiços = dt.ServiceGroup;
                            tpval.CódServiço = dt.ServiceCode;
                            tpval.CodGrupoServico = dt.CodGrupoServico;
                            tpval.NºCliente = dt.ClientNumber;

                            DBClientServices.Create(tpval);
                        }
                        else
                        {
                            totalExists++;
                        }
                    }
                }
                if (totalExists == data.Count())
                {
                    return Json(true);
                }
                return Json(false);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public JsonResult DeleteClientServices([FromBody] List<ClientServicesViewModel> data)
        {
            try
            {
                List<ServiçosCliente> results = DBClientServices.GetAll();
                results.RemoveAll(x => data.Any(u => u.ClientNumber == x.NºCliente && u.ServiceCode == x.CódServiço));
                results.ForEach(x => DBClientServices.Delete(x.CódServiço, x.NºCliente));
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool CheckIfExist(string ClientNumber, string ServiceCode, bool? ServiceGroup, int param)
        {
            List<ClientServicesViewModel> result = DBClientServices.GetAll().Select(x => new ClientServicesViewModel()
            {
                ClientNumber = x.NºCliente,
                ServiceCode = x.CódServiço,
                ServiceGroup = x.GrupoServiços,
                CodGrupoServico = x.CodGrupoServico
            }).ToList();

            bool exists = false;
            if (param == 1)
            {
                foreach (var res in result)
                {
                    if (res.ClientNumber == ClientNumber && res.ServiceCode == ServiceCode)
                    {
                        exists = true;
                    }
                }
            }

            if (param == 2)
            {
                foreach (var res in result)
                {
                    if (res.ClientNumber == ClientNumber && res.ServiceCode == ServiceCode && res.ServiceGroup == ServiceGroup)
                    {
                        exists = true;
                    }
                }
            }
            return exists;
        }
        #endregion

        #region TiposViaturas
        public IActionResult TiposViaturas(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminViaturasTelemoveis);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetTiposViaturas()
        {
            List<TiposViaturaViewModel> result = DBTiposViaturas.ParseListToViewModel(DBTiposViaturas.GetAll());
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateTiposViaturas([FromBody] TiposViaturaViewModel data)
        {
            TiposViatura tiposViatura = DBTiposViaturas.ParseToDB(data);
            tiposViatura.UtilizadorCriação = User.Identity.Name;
            DBTiposViaturas.Create(tiposViatura);

            return Json(data);
        }

        [HttpPost]
        public JsonResult DeleteTiposViaturas([FromBody] TiposViaturaViewModel data)
        {
            var result = DBTiposViaturas.Delete(DBTiposViaturas.ParseToDB(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateTiposViaturas([FromBody] List<TiposViaturaViewModel> data)
        {
            List<TiposViatura> results = DBTiposViaturas.GetAll();
            data.RemoveAll(x => results.Any(u => u.CódigoTipo == x.CodigoTipo && u.Descrição == x.Descricao));

            data.ForEach(x =>
            {
                TiposViatura tiposViatura = DBTiposViaturas.ParseToDB(x);
                tiposViatura.UtilizadorModificação = User.Identity.Name;
                DBTiposViaturas.Update(tiposViatura);
            });
            return Json(data);
        }


        #endregion

        #region Marcas
        public IActionResult Marcas(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminViaturasTelemoveis);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetMarcas()
        {
            List<MarcasViewModel> result = DBMarcas.ParseListToViewModel(DBMarcas.GetAll());
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateMarca([FromBody] MarcasViewModel data)
        {
            Marcas toCreate = DBMarcas.ParseToDB(data);
            toCreate.UtilizadorCriação = User.Identity.Name;
            DBMarcas.Create(toCreate);

            return Json(data);
        }

        [HttpPost]
        public JsonResult DeleteMarca([FromBody] MarcasViewModel data)
        {
            var result = DBMarcas.Delete(DBMarcas.ParseToDB(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateMarcas([FromBody] List<MarcasViewModel> data)
        {
            List<Marcas> results = DBMarcas.GetAll();
            data.RemoveAll(x => results.Any(u => u.CódigoMarca == x.CodigoMarca && u.Descrição == x.Descricao));

            data.ForEach(x =>
            {
                Marcas toUpdate = DBMarcas.ParseToDB(x);
                toUpdate.UtilizadorModificação = User.Identity.Name;
                DBMarcas.Update(toUpdate);
            });
            return Json(data);
        }

        #endregion

        #region Modelos
        public IActionResult Modelos(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminViaturasTelemoveis);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetModelos()
        {
            List<ModelosViewModel> result = DBModelos.ParseListToViewModel(DBModelos.GetAll());
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateModelo([FromBody] ModelosViewModel data)
        {
            Modelos toCreate = DBModelos.ParseToDB(data);
            toCreate.UtilizadorCriação = User.Identity.Name;
            DBModelos.Create(toCreate);

            return Json(data);
        }

        [HttpPost]
        public JsonResult DeleteModelo([FromBody] ModelosViewModel data)
        {
            var result = DBModelos.Delete(DBModelos.ParseToDB(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateModelos([FromBody] List<ModelosViewModel> data)
        {
            List<Modelos> results = DBModelos.GetAll();
            data.RemoveAll(x => results.Any(u => u.CódigoModelo == x.CodigoModelo && u.Descrição == x.Descricao));

            data.ForEach(x =>
            {
                Modelos toUpdate = DBModelos.ParseToDB(x);
                toUpdate.UtilizadorModificação = User.Identity.Name;
                DBModelos.Update(toUpdate);
            });
            return Json(data);
        }

        #endregion

        #region Cartoes E Apolices
        public IActionResult CartoesEApolices(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminViaturasTelemoveis);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetCartoesEApolices()
        {
            List<CartoesEApolicesViewModel> result = DBCartoesEApolices.ParseListToViewModel(DBCartoesEApolices.GetAll());
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateCartoesEApolices([FromBody] CartoesEApolicesViewModel data)
        {
            CartõesEApólices toCreate = DBCartoesEApolices.ParseToDB(data);
            toCreate.UtilizadorCriação = User.Identity.Name;
            DBCartoesEApolices.Create(toCreate);

            return Json(data);
        }

        [HttpPost]
        public JsonResult DeleteCartoesEApolices([FromBody] CartoesEApolicesViewModel data)
        {
            var result = DBCartoesEApolices.Delete(DBCartoesEApolices.ParseToDB(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateCArtoesEApolices([FromBody] List<CartoesEApolicesViewModel> data)
        {
            List<CartõesEApólices> results = DBCartoesEApolices.GetAll();

            data.RemoveAll(x => DBCartoesEApolices.ParseListToViewModel(results).Any(
                u =>
                    u.Tipo == x.Tipo &&
                    u.Numero == x.Numero &&
                    u.Descricao == x.Descricao &&
                    u.DataInicio == x.DataInicio &&
                    u.DataFim == x.DataFim &&
                    u.Fornecedor == x.Fornecedor
            ));

            data.ForEach(x =>
            {
                CartõesEApólices toUpdate = DBCartoesEApolices.ParseToDB(x);
                toUpdate.UtilizadorModificação = User.Identity.Name;
                DBCartoesEApolices.Update(toUpdate);
            });
            return Json(data);
        }
        #endregion

        #region Configuracao Ajuda De Custo
        public IActionResult ConfiguracaoAjudaCusto(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminFolhaHoras);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetConfiguracaoAjudaCusto()
        {
            List<ConfiguracaoAjudaCustoViewModel> result = DBConfiguracaoAjudaCusto.ParseListToViewModel(DBConfiguracaoAjudaCusto.GetAll());

            if (result != null)
            {
                result.ForEach(x =>
                {
                    x.CodigoTipoCustoTexto = x.CodigoTipoCusto.Trim() + " - " + DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, x.CodigoTipoCusto.Trim(), "", 0, "").FirstOrDefault().Name;
                });
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateConfiguracaoAjudaCusto([FromBody] ConfiguracaoAjudaCustoViewModel data)
        {

            ConfiguracaoAjudaCusto toCreate = DBConfiguracaoAjudaCusto.ParseToDB(data);
            toCreate.UtilizadorCriacao = User.Identity.Name;
            var result = DBConfiguracaoAjudaCusto.Create(toCreate);

            return Json(data);
        }

        [HttpPost]
        public JsonResult DeleteConfiguracaoAjudaCusto([FromBody] ConfiguracaoAjudaCustoViewModel data)
        {
            var result = DBConfiguracaoAjudaCusto.Delete(DBConfiguracaoAjudaCusto.ParseToDB(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateConfiguracaoAjudaCusto([FromBody] List<ConfiguracaoAjudaCustoViewModel> data)
        {
            List<ConfiguracaoAjudaCusto> results = DBConfiguracaoAjudaCusto.GetAll();

            data.RemoveAll(x => DBConfiguracaoAjudaCusto.ParseListToViewModel(results).Any(
                u =>
                    u.CodigoTipoCusto == x.CodigoTipoCusto &&
                    u.DistanciaMinima == x.DistanciaMinima &&
                    u.DataChegadaDataPartida == x.DataChegadaDataPartida &&
                    u.LimiteHoraPartida == x.LimiteHoraPartida &&
                    u.LimiteHoraChegada == x.LimiteHoraChegada &&
                    u.Prioritario == x.Prioritario &&
                    u.TipoCusto == x.TipoCusto &&
                    u.CodigoRefCusto == x.CodigoRefCusto &&
                    u.SinalHoraPartida == x.SinalHoraPartida &&
                    u.HoraPartida == x.HoraPartida &&
                    u.SinalHoraChegada == x.SinalHoraChegada &&
                    u.HoraChegada == x.HoraChegada
            ));

            data.ForEach(x =>
            {
                ConfiguracaoAjudaCusto toUpdate = DBConfiguracaoAjudaCusto.ParseToDB(x);
                toUpdate.UtilizadorModificacao = User.Identity.Name;
                DBConfiguracaoAjudaCusto.Update(toUpdate);
            });
            return Json(data);
        }
        #endregion

        #region Configuracao Tipo Trabalho FH
        public IActionResult ConfiguracaoTipoTrabalhoFH(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminFolhaHoras);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetConfiguracaoTipoTrabalhoFH()
        {
            List<TipoTrabalhoFHViewModel> result = DBTipoTrabalhoFH.ParseListToViewModel(DBTipoTrabalhoFH.GetAll());
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateConfiguracaoTipoTrabalhoFH([FromBody] TipoTrabalhoFHViewModel data)
        {
            int resultFinal = 0;

            TipoTrabalhoFh toCreate = DBTipoTrabalhoFH.ParseToDB(data);
            toCreate.CriadoPor = User.Identity.Name;
            var result = DBTipoTrabalhoFH.Create(toCreate);

            if (result == null)
                resultFinal = 0;
            else
                resultFinal = 1;
            //return Json(data);
            return Json(resultFinal);
        }

        [HttpPost]
        public JsonResult DeleteTipoTrabalhoFH([FromBody] TipoTrabalhoFHViewModel data)
        {
            var result = DBTipoTrabalhoFH.Delete(DBTipoTrabalhoFH.ParseToDB(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateTipoTrabalhoFH([FromBody] List<TipoTrabalhoFHViewModel> data)
        {
            List<TipoTrabalhoFh> results = DBTipoTrabalhoFH.GetAll();

            data.RemoveAll(x => DBTipoTrabalhoFH.ParseListToViewModel(results).Any(
                u =>
                    u.Codigo == x.Codigo &&
                    u.Descricao == x.Descricao &&
                    u.CodUnidadeMedida == x.CodUnidadeMedida &&
                    u.HoraViagem == x.HoraViagem &&
                    u.TipoHora == x.TipoHora &&
                    u.UtilizadorCriacao == x.UtilizadorCriacao &&
                    u.DataHoraCriacao == x.DataHoraCriacao
            ));

            data.ForEach(x =>
            {
                TipoTrabalhoFh toUpdate = DBTipoTrabalhoFH.ParseToDB(x);
                toUpdate.AlteradoPor = User.Identity.Name;
                DBTipoTrabalhoFH.Update(toUpdate);
            });
            return Json(data);
        }
        #endregion

        #region Configuração Preço Venda Recursos FH
        public IActionResult ConfiguracaoPrecoVendaRecursoFH(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminFolhaHoras);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetConfiguracaoPrecoVendaRecursoFH()
        {
            List<PrecoVendaRecursoFHViewModel> result = DBPrecoVendaRecursoFH.ParseListToViewModel(DBPrecoVendaRecursoFH.GetAll());

            if (result != null)
            {
                result.ForEach(x =>
                {
                    x.Descricao = x.Code; // + " - " + DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, x.Code, "", 0, "").FirstOrDefault().Name;
                    x.CodTipoTrabalhoTexto = x.CodTipoTrabalho; // + " - " + DBTipoTrabalhoFH.GetAll().Where(y => y.Codigo == x.CodTipoTrabalho).FirstOrDefault().Descricao;
                    //x.FamiliaRecurso = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, x.Code, "", 0, "").FirstOrDefault().ResourceGroup;
                });
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateConfiguracaoPrecoVendaRecursoFH([FromBody] PrecoVendaRecursoFHViewModel data)
        {
            int resultFinal = 0;

            PrecoVendaRecursoFh toCreate = DBPrecoVendaRecursoFH.ParseToDB(data);

            NAVResourcesViewModel resource = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, data.Code, "", 0, "").FirstOrDefault();

            toCreate.Descricao = resource.Name;
            toCreate.FamiliaRecurso = resource.ResourceGroup;

            toCreate.CriadoPor = User.Identity.Name;
            var result = DBPrecoVendaRecursoFH.Create(toCreate);

            if (result == null)
                resultFinal = 0;
            else
                resultFinal = 1;

            return Json(resultFinal);
        }

        [HttpPost]
        public JsonResult DeletePrecoVendaRecursoFH([FromBody] PrecoVendaRecursoFHViewModel data)
        {
            var result = DBPrecoVendaRecursoFH.Delete(DBPrecoVendaRecursoFH.ParseToDB(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdatePrecoVendaRecursoFH([FromBody] List<PrecoVendaRecursoFHViewModel> data)
        {
            List<PrecoVendaRecursoFh> results = DBPrecoVendaRecursoFH.GetAll();

            data.RemoveAll(x => DBPrecoVendaRecursoFH.ParseListToViewModel(results).Any(
                u =>
                    u.Code == x.Code &&
                    u.Descricao == x.Descricao &&
                    u.CodTipoTrabalho == x.CodTipoTrabalho &&
                    u.PrecoUnitario == x.PrecoUnitario &&
                    u.CustoUnitario == x.CustoUnitario &&
                    u.StartingDate == x.StartingDate &&
                    u.EndingDate == x.EndingDate &&
                    u.FamiliaRecurso == x.FamiliaRecurso &&
                    u.UtilizadorCriacao == x.UtilizadorCriacao &&
                    u.DataHoraCriacao == x.DataHoraCriacao
            ));

            data.ForEach(x =>
            {
                PrecoVendaRecursoFh toUpdate = DBPrecoVendaRecursoFH.ParseToDB(x);
                toUpdate.Descricao = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, x.Code, "", 0, "").FirstOrDefault().Name;
                toUpdate.FamiliaRecurso = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, x.Code, "", 0, "").FirstOrDefault().ResourceGroup;
                toUpdate.AlteradoPor = User.Identity.Name;
                DBPrecoVendaRecursoFH.Update(toUpdate);
            });
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetRecurso([FromBody] NAVResourcesViewModel data)
        {
            NAVResourcesViewModel result = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, data.Code, "", 0, "").FirstOrDefault();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetPrecoVendaRecursoFH_AnexosErros()
        {
            //ORIGEM = 3 » FH Preço Venda Recursos
            //TIPO = 2 » ERRO
            List<AnexosErrosViewModel> result = DBAnexosErros.GetByOrigemAndCodigo(3, "").Select(x => new AnexosErrosViewModel()
            {
                ID = x.Id,
                CodeTexto = x.Id.ToString(),
                Origem = (int)x.Origem,
                OrigemTexto = x.Origem == 0 ? "" : EnumerablesFixed.AE_Origem.Where(y => y.Id == x.Origem).SingleOrDefault().Value,
                Tipo = (int)x.Tipo,
                TipoTexto = x.Tipo == 0 ? "" : EnumerablesFixed.AE_Tipo.Where(y => y.Id == x.Tipo).SingleOrDefault().Value,
                Codigo = x.Codigo,
                NomeAnexo = x.NomeAnexo,
                Anexo = x.Anexo,
                CriadoPor = x.CriadoPor,
                CriadoPorNome = x.CriadoPor == null ? "" : DBUserConfigurations.GetById(x.CriadoPor).Nome,
                DataHora_Criacao = x.DataHoraCriacao,
                DataHora_CriacaoTexto = x.DataHoraCriacao == null ? "" : x.DataHoraCriacao.Value.ToString("yyyy-MM-dd"),
                AlteradoPor = x.AlteradoPor,
                AlteradoPorNome = x.AlteradoPor == null ? "" : DBUserConfigurations.GetById(x.AlteradoPor).Nome,
                DataHora_Alteracao = x.DataHoraAlteracao,
                DataHora_AlteracaoTexto = x.DataHoraAlteracao == null ? "" : x.DataHoraAlteracao.Value.ToString("yyyy-MM-dd")
            }).ToList();

            return Json(result);
        }

        [HttpGet]
        [Route("Administracao/DownloadPrecoVendaRecursoFHTemplate")]
        [Route("Administracao/DownloadPrecoVendaRecursoFH/{FileName}")]
        public FileStreamResult DownloadPrecoVendaRecursoFHTemplate(string FileName)
        {
            return new FileStreamResult(new FileStream(_generalConfig.FileUploadFolder + "Administracao\\" + FileName, FileMode.Open), "application /xlsx");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_PrecosVendaRecursosCliente([FromBody] List<PrecoVendaRecursoFHViewModel> dp)
        {
            string sWebRootFolder = _generalConfig.FileUploadFolder + "Administracao\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Preços Venda Recursos Cliente");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Código - Fam. Recurso");
                row.CreateCell(1).SetCellValue("Código - Tipo Trabalho");
                row.CreateCell(2).SetCellValue("Preço Unitário");
                row.CreateCell(3).SetCellValue("Custo Unitário");
                row.CreateCell(4).SetCellValue("Data Início");
                row.CreateCell(5).SetCellValue("Data Fim");
                row.CreateCell(6).SetCellValue("Criado Por");
                row.CreateCell(7).SetCellValue("Data-Hora Criação");

                if (dp != null)
                {
                    int count = 1;
                    foreach (PrecoVendaRecursoFHViewModel item in dp)
                    {
                        row = excelSheet.CreateRow(count);

                        row.CreateCell(0).SetCellValue(item.Code);
                        row.CreateCell(1).SetCellValue(item.CodTipoTrabalho);
                        row.CreateCell(2).SetCellValue(item.PrecoUnitario.HasValue ? item.PrecoUnitario.ToString() : "");
                        row.CreateCell(3).SetCellValue(item.CustoUnitario.HasValue ? item.CustoUnitario.ToString() : "");
                        row.CreateCell(4).SetCellValue(item.StartingDate.HasValue ? Convert.ToDateTime(item.StartingDate).ToShortDateString() : "");
                        row.CreateCell(5).SetCellValue(item.EndingDate.HasValue ? Convert.ToDateTime(item.EndingDate).ToShortDateString() : "");
                        row.CreateCell(6).SetCellValue(item.UtilizadorCriacao);
                        row.CreateCell(7).SetCellValue(item.DataHoraCriacao.ToString());

                        count++;
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_PrecosVendaRecursosCliente(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Administracao\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Preços Venda Recursos Cliente.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        //3
        [HttpPost]
        public JsonResult OnPostImport_PrecosVendaRecursosCliente()
        {
            var files = Request.Form.Files;
            List<PrecoVendaRecursoFHViewModel> ListToCreate = DBPrecoVendaRecursoFH.ParseListToViewModel(DBPrecoVendaRecursoFH.GetAll());
            PrecoVendaRecursoFHViewModel nrow = new PrecoVendaRecursoFHViewModel();
            for (int i = 0; i < files.Count; i++)
            {
                IFormFile file = files[i];
                string folderName = "Upload";
                string webRootPath = _generalConfig.FileUploadFolder + "Administracao\\" + "tmp\\";
                string newPath = Path.Combine(webRootPath, folderName);
                StringBuilder sb = new StringBuilder();
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, file.FileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                        }
                        for (int j = (sheet.FirstRowNum + 1); j <= sheet.LastRowNum; j++)
                        {
                            IRow row = sheet.GetRow(j);
                            if (row != null)
                            {
                                nrow = new PrecoVendaRecursoFHViewModel();

                                nrow.Code = row.GetCell(0) != null ? row.GetCell(0).ToString() : "";
                                nrow.CodTipoTrabalho = row.GetCell(1) != null ? row.GetCell(1).ToString() : "";
                                nrow.PrecoUnitarioTexto = row.GetCell(2) != null ? row.GetCell(2).ToString() : "";
                                nrow.CustoUnitarioTexto = row.GetCell(3) != null ? row.GetCell(3).ToString() : "";
                                nrow.StartingDateTexto = row.GetCell(4) != null ? row.GetCell(4).ToString() : "";
                                nrow.EndingDateTexto = row.GetCell(5) != null ? row.GetCell(5).ToString() : "";
                                nrow.UtilizadorCriacao = row.GetCell(6) != null ? row.GetCell(6).ToString() : "";
                                nrow.DataHoraCriacaoTexto = row.GetCell(7) != null ? row.GetCell(7).ToString() : "";

                                ListToCreate.Add(nrow);
                            }
                        }
                    }
                }
                if (ListToCreate.Count > 0)
                {
                    foreach (PrecoVendaRecursoFHViewModel item in ListToCreate)
                    {
                        if (!string.IsNullOrEmpty(item.PrecoUnitarioTexto))
                        {
                            item.PrecoUnitario = Convert.ToDecimal(item.PrecoUnitarioTexto);
                            item.PrecoUnitarioTexto = "";
                        }
                        if (!string.IsNullOrEmpty(item.CustoUnitarioTexto))
                        {
                            item.CustoUnitario = Convert.ToDecimal(item.CustoUnitarioTexto);
                            item.CustoUnitarioTexto = "";
                        }
                        if (!string.IsNullOrEmpty(item.StartingDateTexto))
                        {
                            item.StartingDate = Convert.ToDateTime(item.StartingDateTexto);
                            item.StartingDateTexto = "";
                        }
                        if (!string.IsNullOrEmpty(item.EndingDateTexto))
                        {
                            item.EndingDate = Convert.ToDateTime(item.EndingDateTexto);
                            item.EndingDateTexto = "";
                        }
                        if (!string.IsNullOrEmpty(item.DataHoraCriacaoTexto))
                        {
                            item.DataHoraCriacao = Convert.ToDateTime(item.DataHoraCriacaoTexto);
                            item.DataHoraCriacaoTexto = "";
                        }
                    }
                }
            }
            return Json(ListToCreate);
        }
        //4
        [HttpPost]
        public JsonResult UpdateCreatePrecosVendaRecursosCliente([FromBody] List<PrecoVendaRecursoFHViewModel> data)
        {
            List<PrecoVendaRecursoFh> results = DBPrecoVendaRecursoFH.GetAll();

            data.RemoveAll(x => results.Any(
                u =>
                    u.Code == x.Code &&
                    u.CodTipoTrabalho == x.CodTipoTrabalho &&
                    u.PrecoUnitario == x.PrecoUnitario &&
                    u.CustoUnitario == x.CustoUnitario &&
                    u.StartingDate == x.StartingDate &&
                    u.EndingDate == x.EndingDate
            ));

            data.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.Code) && !string.IsNullOrWhiteSpace(x.CodTipoTrabalho) && x.StartingDate != null)
                {
                    PrecoVendaRecursoFh toCreate = DBPrecoVendaRecursoFH.ParseToDB(x);
                    PrecoVendaRecursoFh toUpdate = DBPrecoVendaRecursoFH.ParseToDB(x);
                    PrecoVendaRecursoFh toSearch = DBPrecoVendaRecursoFH.GetByID(x.Code, x.CodTipoTrabalho, (DateTime)x.StartingDate);

                    NAVResourcesViewModel resource = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, x.Code, "", 0, "").FirstOrDefault();

                    if (toSearch == null)
                    {
                        toCreate.Code = x.Code;
                        toCreate.CodTipoTrabalho = x.CodTipoTrabalho;
                        toCreate.PrecoUnitario = x.PrecoUnitario;
                        toCreate.CustoUnitario = x.CustoUnitario;
                        toCreate.StartingDate = (DateTime)x.StartingDate;
                        toCreate.EndingDate = x.EndingDate;
                        if (resource != null)
                        {
                            toCreate.Descricao = resource.Name;
                            toCreate.FamiliaRecurso = resource.ResourceGroup;
                        }
                        else
                        {
                            toCreate.Descricao = null;
                            toCreate.FamiliaRecurso = null;
                        }
                        toCreate.CriadoPor = User.Identity.Name;
                        toCreate.DataHoraCriacao = DateTime.Now;

                        DBPrecoVendaRecursoFH.Create(toCreate);
                    }
                    else
                    {
                        toUpdate.Code = x.Code;
                        toUpdate.CodTipoTrabalho = x.CodTipoTrabalho;
                        toUpdate.PrecoUnitario = x.PrecoUnitario;
                        toUpdate.CustoUnitario = x.CustoUnitario;
                        toUpdate.StartingDate = (DateTime)x.StartingDate;
                        toUpdate.EndingDate = x.EndingDate;
                        if (resource != null)
                        {
                            toUpdate.Descricao = resource.Name;
                            toUpdate.FamiliaRecurso = resource.ResourceGroup;
                        }
                        else
                        {
                            toUpdate.Descricao = null;
                            toUpdate.FamiliaRecurso = null;
                        }
                        toUpdate.CriadoPor = x.UtilizadorCriacao;
                        toUpdate.DataHoraCriacao = x.DataHoraCriacao;
                        toUpdate.AlteradoPor = User.Identity.Name;
                        toUpdate.DataHoraUltimaAlteracao = DateTime.Now;

                        DBPrecoVendaRecursoFH.Update(toUpdate);
                    }
                }
            });
            return Json(data);
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_AcordoPrecos([FromBody] AcordoPrecosModelView dp)
        {
            string sWebRootFolder = _generalConfig.FileUploadFolder + "Administracao\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Acordo de Preços");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Nº Procedimento");
                row.CreateCell(1).SetCellValue("Nº Fornecedor");
                row.CreateCell(2).SetCellValue("Nome Fornecedor");
                row.CreateCell(3).SetCellValue("Cód. Produto");
                row.CreateCell(4).SetCellValue("Desc. Produto");
                row.CreateCell(5).SetCellValue("Data Validade Início");
                row.CreateCell(6).SetCellValue("Data Validade Fim");
                row.CreateCell(7).SetCellValue("Região");
                row.CreateCell(8).SetCellValue("Área");
                row.CreateCell(9).SetCellValue("Cresp");
                row.CreateCell(10).SetCellValue("Cód. Localização");
                row.CreateCell(11).SetCellValue("Custo Unitário");
                row.CreateCell(12).SetCellValue("UM");
                row.CreateCell(13).SetCellValue("Quantidade por UM");
                row.CreateCell(14).SetCellValue("Peso Unitário");
                row.CreateCell(15).SetCellValue("Cód. Produto Fornecedor");
                row.CreateCell(16).SetCellValue("Desc. Produto Fornecedor");
                row.CreateCell(17).SetCellValue("Forma Entrega");
                row.CreateCell(18).SetCellValue("Tipo Preço");
                row.CreateCell(19).SetCellValue("Grupo Registo IVA Produto");
                row.CreateCell(20).SetCellValue("Cód. Categoria Produto");
                row.CreateCell(21).SetCellValue("Criado Por");
                row.CreateCell(22).SetCellValue("Data-Hora Criação");

                if (dp.LinhasAcordoPrecos != null)
                {
                    int count = 1;
                    foreach (LinhasAcordoPrecosViewModel item in dp.LinhasAcordoPrecos)
                    {
                        row = excelSheet.CreateRow(count);

                        row.CreateCell(0).SetCellValue(item.NoProcedimento.ToString());
                        row.CreateCell(1).SetCellValue(item.NoFornecedor.ToString());
                        row.CreateCell(2).SetCellValue(item.NomeFornecedor.ToString());
                        row.CreateCell(3).SetCellValue(item.CodProduto.ToString());
                        row.CreateCell(4).SetCellValue(item.DescricaoProduto.ToString());
                        row.CreateCell(5).SetCellValue(Convert.ToDateTime(item.DtValidadeInicio).ToShortDateString());
                        row.CreateCell(6).SetCellValue(item.DtValidadeFim.HasValue ? Convert.ToDateTime(item.DtValidadeFim).ToShortDateString() : "");
                        row.CreateCell(7).SetCellValue(item.Regiao.ToString());
                        row.CreateCell(8).SetCellValue(item.Area.ToString());
                        row.CreateCell(9).SetCellValue(item.Cresp.ToString());
                        row.CreateCell(10).SetCellValue(item.Localizacao.ToString());
                        row.CreateCell(11).SetCellValue((double)(item.CustoUnitario.HasValue ? item.CustoUnitario : 0));
                        row.CreateCell(12).SetCellValue(item.Um.ToString());
                        row.CreateCell(13).SetCellValue(item.QtdPorUm.HasValue ? item.QtdPorUm.ToString() : "");
                        row.CreateCell(14).SetCellValue((double)(item.PesoUnitario.HasValue ? item.PesoUnitario : 0));
                        row.CreateCell(15).SetCellValue(item.CodProdutoFornecedor.ToString());
                        row.CreateCell(16).SetCellValue(item.DescricaoProdFornecedor.ToString());
                        row.CreateCell(17).SetCellValue(item.FormaEntrega.HasValue ? item.FormaEntrega.ToString() : "");
                        row.CreateCell(18).SetCellValue(item.TipoPreco.HasValue ? item.TipoPreco.ToString() : "");
                        row.CreateCell(19).SetCellValue(item.GrupoRegistoIvaProduto?.ToString());
                        row.CreateCell(20).SetCellValue(item.CodCategoriaProduto == null ? string.Empty : item.CodCategoriaProduto.ToString());
                        row.CreateCell(21).SetCellValue(item.UserId.ToString());
                        row.CreateCell(22).SetCellValue(item.DataCriacao.HasValue ? item.DataCriacao.ToString() : "");

                        count++;
                    }
                    excelSheet.SetColumnHidden(0, true);
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Json(sFileName);
        }
        //2
        [RequestSizeLimit(100_000_000)]
        public IActionResult ExportToExcelDownload_AcordoPrecos(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Administracao\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Acordo de Preços.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        //3
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public JsonResult OnPostImport_AcordoPrecos() //[FromBody] string NoProcedimento)
        {
            var files = Request.Form.Files;
            List<LinhasAcordoPrecosViewModel> ListToCreate = new List<LinhasAcordoPrecosViewModel>(); // DBLinhasAcordoPrecos.GetAllByNoProcedimento(NoProcedimento);
            LinhasAcordoPrecosViewModel nrow = new LinhasAcordoPrecosViewModel();
            for (int i = 0; i < files.Count; i++)
            {
                IFormFile file = files[i];
                string folderName = "Upload";
                string webRootPath = _generalConfig.FileUploadFolder + "Administracao\\" + "tmp\\";
                string newPath = Path.Combine(webRootPath, folderName);
                StringBuilder sb = new StringBuilder();
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, file.FileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                        }
                        for (int j = (sheet.FirstRowNum + 1); j <= sheet.LastRowNum; j++)
                        {
                            IRow row = sheet.GetRow(j);
                            if (row != null)
                            {
                                nrow = new LinhasAcordoPrecosViewModel();

                                nrow.NoProcedimento = row.GetCell(0) != null ? row.GetCell(0).ToString() : "";
                                nrow.NoFornecedor = row.GetCell(1) != null ? row.GetCell(1).ToString() : "";
                                nrow.NomeFornecedor = row.GetCell(2) != null ? row.GetCell(2).ToString() : "";
                                nrow.CodProduto = row.GetCell(3) != null ? row.GetCell(3).ToString() : "";
                                nrow.DescricaoProduto = row.GetCell(4) != null ? row.GetCell(4).ToString() : "";
                                nrow.DtValidadeInicioTexto = row.GetCell(5) != null ? row.GetCell(5).ToString() : "";
                                nrow.DtValidadeFimTexto = row.GetCell(6) != null ? row.GetCell(6).ToString() : "";
                                nrow.Regiao = row.GetCell(7) != null ? row.GetCell(7).ToString() : "";
                                nrow.Area = row.GetCell(8) != null ? row.GetCell(8).ToString() : "";
                                nrow.Cresp = row.GetCell(9) != null ? row.GetCell(9).ToString() : "";
                                nrow.Localizacao = row.GetCell(10) != null ? row.GetCell(10).ToString() : "";
                                nrow.CustoUnitarioTexto = row.GetCell(11) != null ? row.GetCell(11).ToString() : "";
                                nrow.Um = row.GetCell(12) != null ? row.GetCell(12).ToString() : "";
                                nrow.QtdPorUmTexto = row.GetCell(13) != null ? row.GetCell(13).ToString() : "";
                                nrow.PesoUnitarioTexto = row.GetCell(14) != null ? row.GetCell(14).ToString() : "";
                                nrow.CodProdutoFornecedor = row.GetCell(15) != null ? row.GetCell(15).ToString() : "";
                                nrow.DescricaoProdFornecedor = row.GetCell(16) != null ? row.GetCell(16).ToString() : "";
                                nrow.FormaEntregaTexto = row.GetCell(17) != null ? row.GetCell(17).ToString() : "";
                                nrow.TipoPrecoTexto = row.GetCell(18) != null ? row.GetCell(18).ToString() : "";
                                nrow.GrupoRegistoIvaProdutoTexto = row.GetCell(19) != null ? row.GetCell(19).ToString() : "";
                                nrow.CodCategoriaProduto = row.GetCell(20) != null ? row.GetCell(20).ToString() : "";
                                nrow.UserId = row.GetCell(21) != null ? row.GetCell(21).ToString() : "";
                                nrow.DataCriacaoTexto = row.GetCell(22) != null ? row.GetCell(22).ToString() : "";

                                ListToCreate.Add(nrow);
                            }
                        }
                    }
                }
                if (ListToCreate.Count > 0)
                {
                    foreach (LinhasAcordoPrecosViewModel item in ListToCreate)
                    {
                        if (!string.IsNullOrEmpty(item.DtValidadeInicioTexto))
                        {
                            item.DtValidadeInicio = Convert.ToDateTime(item.DtValidadeInicioTexto);
                            item.DtValidadeInicioTexto = "";
                        }
                        if (!string.IsNullOrEmpty(item.DtValidadeFimTexto))
                        {
                            item.DtValidadeFim = Convert.ToDateTime(item.DtValidadeFimTexto);
                            item.DtValidadeFimTexto = "";
                        }
                        if (!string.IsNullOrEmpty(item.CustoUnitarioTexto))
                        {
                            item.CustoUnitario = Convert.ToDecimal(item.CustoUnitarioTexto);
                            item.CustoUnitarioTexto = "";
                        }
                        if (!string.IsNullOrEmpty(item.QtdPorUmTexto))
                        {
                            item.QtdPorUm = Convert.ToDecimal(item.QtdPorUmTexto);
                            item.QtdPorUmTexto = "";
                        }
                        if (!string.IsNullOrEmpty(item.PesoUnitarioTexto))
                        {
                            item.PesoUnitario = Convert.ToDecimal(item.PesoUnitarioTexto);
                            item.PesoUnitarioTexto = "";
                        }
                        if (!string.IsNullOrEmpty(item.FormaEntregaTexto))
                        {
                            item.FormaEntrega = Convert.ToInt32(item.FormaEntregaTexto);
                            item.FormaEntregaTexto = "";
                        }
                        if (!string.IsNullOrEmpty(item.TipoPrecoTexto))
                        {
                            item.TipoPreco = Convert.ToInt32(item.TipoPrecoTexto);
                            item.TipoPrecoTexto = "";
                        }
                        if (!string.IsNullOrEmpty(item.DataCriacaoTexto))
                        {
                            item.DataCriacao = Convert.ToDateTime(item.DataCriacaoTexto);
                            item.DataCriacaoTexto = "";
                        }
                        if (!string.IsNullOrEmpty(item.GrupoRegistoIvaProdutoTexto))
                        {
                            item.GrupoRegistoIvaProduto = item.GrupoRegistoIvaProdutoTexto;
                            item.GrupoRegistoIvaProdutoTexto = "";
                        }
                    }
                }
            }
            return Json(ListToCreate);
        }
        //4
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public JsonResult UpdateCreate_AcordoPrecos([FromBody] List<LinhasAcordoPrecos> data)
        {
            List<LinhasAcordoPrecos> results = DBLinhasAcordoPrecos.GetAllByNoProcedimento(data[0].NoProcedimento);

            data.RemoveAll(x => results.Any(
                u =>
                    u.NoProcedimento == x.NoProcedimento &&
                    u.NoFornecedor == x.NoFornecedor &&
                    u.CodProduto == x.CodProduto &&
                    u.DtValidadeInicio == x.DtValidadeInicio &&
                    u.DtValidadeFim == x.DtValidadeFim &&
                    u.Regiao == x.Regiao &&
                    u.Area == x.Area &&
                    u.Cresp == x.Cresp &&
                    u.Localizacao == x.Localizacao &&
                    u.CustoUnitario == x.CustoUnitario &&
                    u.Um == x.Um &&
                    u.QtdPorUm == x.QtdPorUm &&
                    u.PesoUnitario == x.PesoUnitario &&
                    u.CodProdutoFornecedor == x.CodProdutoFornecedor &&
                    u.DescricaoProdFornecedor == x.DescricaoProdFornecedor &&
                    u.FormaEntrega == x.FormaEntrega &&
                    u.TipoPreco == x.TipoPreco &&
                    u.GrupoRegistoIvaProduto == x.GrupoRegistoIvaProduto
            ));

            List<LinhasAcordoPrecos> AllSearch = DBLinhasAcordoPrecos.GetAll().ToList();
            List<NAVVendorViewModel> AllVendor = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).ToList();
            List<NAVProductsViewModel> AllProduct = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, "").ToList();

            data.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.NoProcedimento) && !string.IsNullOrWhiteSpace(x.NoFornecedor) && !string.IsNullOrWhiteSpace(x.CodProduto) && x.DtValidadeInicio != null && !string.IsNullOrWhiteSpace(x.Cresp) && !string.IsNullOrWhiteSpace(x.Localizacao))
                {
                    LinhasAcordoPrecos toCreate = new LinhasAcordoPrecos();
                    LinhasAcordoPrecos toUpdate = new LinhasAcordoPrecos();
                    LinhasAcordoPrecos toSearch = new LinhasAcordoPrecos();

                    toSearch = AllSearch.Where(y => y.NoProcedimento == x.NoProcedimento && y.NoFornecedor == x.NoFornecedor && y.CodProduto == x.CodProduto && y.DtValidadeInicio == x.DtValidadeInicio && y.Cresp == x.Cresp && y.Localizacao == x.Localizacao).FirstOrDefault();
                    NAVVendorViewModel Vendor = AllVendor.Where(y => y.No_ == x.NoFornecedor).FirstOrDefault();
                    NAVProductsViewModel Product = AllProduct.Where(y => y.Code == x.CodProduto).FirstOrDefault();

                    if (toSearch == null)
                    {
                        if (Vendor != null && Product != null)
                        {
                            toCreate.NoProcedimento = x.NoProcedimento;
                            toCreate.NoFornecedor = x.NoFornecedor;
                            if (string.IsNullOrEmpty(x.NomeFornecedor))
                                toCreate.NomeFornecedor = Vendor.Name;
                            else
                                toCreate.NomeFornecedor = x.NomeFornecedor;
                            toCreate.CodProduto = x.CodProduto;
                            if (string.IsNullOrEmpty(x.DescricaoProduto))
                                toCreate.DescricaoProduto = Product.Name;
                            else
                                toCreate.DescricaoProduto = x.DescricaoProduto;
                            toCreate.DtValidadeInicio = x.DtValidadeInicio;
                            toCreate.DtValidadeFim = x.DtValidadeFim;
                            toCreate.Regiao = x.Regiao;
                            toCreate.Area = x.Area;
                            toCreate.Cresp = x.Cresp;
                            toCreate.Localizacao = x.Localizacao;
                            if (x.CustoUnitario == null)
                                toCreate.CustoUnitario = Product.UnitCost;
                            else
                                toCreate.CustoUnitario = x.CustoUnitario;
                            if (string.IsNullOrEmpty(x.Um))
                                toCreate.Um = Product.MeasureUnit;
                            else
                                toCreate.Um = x.Um;
                            toCreate.QtdPorUm = x.QtdPorUm;
                            toCreate.PesoUnitario = x.PesoUnitario;
                            toCreate.CodProdutoFornecedor = x.CodProdutoFornecedor;
                            toCreate.DescricaoProdFornecedor = x.DescricaoProdFornecedor;
                            toCreate.FormaEntrega = x.FormaEntrega;
                            toCreate.TipoPreco = x.TipoPreco;
                            toCreate.GrupoRegistoIvaProduto = x.GrupoRegistoIvaProduto;
                            if (string.IsNullOrEmpty(x.CodCategoriaProduto))
                                toCreate.CodCategoriaProduto = Product.ItemCategoryCode;
                            else
                                toCreate.CodCategoriaProduto = x.CodCategoriaProduto;

                            toCreate.UserId = User.Identity.Name;
                            toCreate.DataCriacao = DateTime.Now;

                            DBLinhasAcordoPrecos.Create(toCreate);
                        }
                    }
                    else
                    {
                        if (Vendor != null && Product != null)
                        {
                            toUpdate.NoProcedimento = x.NoProcedimento;
                            toUpdate.NoFornecedor = x.NoFornecedor;
                            if (string.IsNullOrEmpty(x.NomeFornecedor))
                                toUpdate.NomeFornecedor = Vendor.Name;
                            else
                                toUpdate.NomeFornecedor = x.NomeFornecedor;
                            toUpdate.CodProduto = x.CodProduto;
                            if (string.IsNullOrEmpty(x.DescricaoProduto))
                                toUpdate.DescricaoProduto = Product.Name;
                            else
                                toUpdate.DescricaoProduto = x.DescricaoProduto;
                            toUpdate.DtValidadeInicio = x.DtValidadeInicio;
                            toUpdate.DtValidadeFim = x.DtValidadeFim;
                            toUpdate.Regiao = x.Regiao;
                            toUpdate.Area = x.Area;
                            toUpdate.Cresp = x.Cresp;
                            toUpdate.Localizacao = x.Localizacao;
                            if (x.CustoUnitario == null)
                                toUpdate.CustoUnitario = Product.UnitCost;
                            else
                                toUpdate.CustoUnitario = x.CustoUnitario;
                            if (string.IsNullOrEmpty(x.Um))
                                toUpdate.Um = Product.MeasureUnit;
                            else
                                toUpdate.Um = x.Um;
                            toUpdate.QtdPorUm = x.QtdPorUm;
                            toUpdate.PesoUnitario = x.PesoUnitario;
                            toUpdate.CodProdutoFornecedor = x.CodProdutoFornecedor;
                            toUpdate.DescricaoProdFornecedor = x.DescricaoProdFornecedor;
                            toUpdate.FormaEntrega = x.FormaEntrega;
                            toUpdate.TipoPreco = x.TipoPreco;
                            toUpdate.GrupoRegistoIvaProduto = x.GrupoRegistoIvaProduto;
                            if (Product != null)
                                toUpdate.CodCategoriaProduto = Product.ItemCategoryCode;
                            else
                                toUpdate.CodCategoriaProduto = null;

                            toUpdate.UserId = x.UserId;
                            toUpdate.DataCriacao = x.DataCriacao;

                            DBLinhasAcordoPrecos.Update(toUpdate);
                        }
                    }
                }
            });
            return Json(data);
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_EmpregadoRecursos([FromBody] List<RHRecursosViewModel> dp)
        {
            string sWebRootFolder = _generalConfig.FileUploadFolder + "Administracao\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("FH Empregado Recursos");
                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("Nº Empregado");
                row.CreateCell(1).SetCellValue("Recurso");
                row.CreateCell(2).SetCellValue("Criado Por");
                row.CreateCell(3).SetCellValue("Data-Hora Criação");

                if (dp != null)
                {
                    int count = 1;
                    foreach (RHRecursosViewModel item in dp)
                    {
                        row = excelSheet.CreateRow(count);
                        row.CreateCell(0).SetCellValue(item.NoEmpregado);
                        row.CreateCell(1).SetCellValue(item.Recurso);
                        row.CreateCell(2).SetCellValue(item.UtilizadorCriacao);
                        row.CreateCell(3).SetCellValue(item.DataHoraCriacao.ToString());
                        count++;
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_EmpregadoRecursos(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Administracao\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FH Empregado Recursos.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        //3
        [HttpPost]
        public JsonResult OnPostImport_EmpregadoRecursos()
        {
            var files = Request.Form.Files;
            List<RHRecursosViewModel> ListToCreate = DBRHRecursosFH.ParseListToViewModel(DBRHRecursosFH.GetAll());
            RHRecursosViewModel nrow = new RHRecursosViewModel();
            for (int i = 0; i < files.Count; i++)
            {
                IFormFile file = files[i];
                string folderName = "Upload";
                string webRootPath = _generalConfig.FileUploadFolder + "Administracao\\" + "tmp\\";
                string newPath = Path.Combine(webRootPath, folderName);
                StringBuilder sb = new StringBuilder();
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, file.FileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                        }
                        for (int j = (sheet.FirstRowNum + 1); j <= sheet.LastRowNum; j++)
                        {
                            IRow row = sheet.GetRow(j);
                            if (row != null)
                            {
                                nrow = new RHRecursosViewModel();

                                nrow.NoEmpregado = row.GetCell(0) != null ? row.GetCell(0).ToString() : "";
                                nrow.Recurso = row.GetCell(1) != null ? row.GetCell(1).ToString() : "";
                                nrow.UtilizadorCriacao = row.GetCell(2) != null ? row.GetCell(2).ToString() : "";
                                nrow.DataHoraCriacaoTexto = row.GetCell(3) != null ? row.GetCell(3).ToString() : "";

                                ListToCreate.Add(nrow);
                            }
                        }
                    }
                }
                if (ListToCreate.Count > 0)
                {
                    foreach (RHRecursosViewModel item in ListToCreate)
                    {
                        if (!string.IsNullOrEmpty(item.NoEmpregado))
                        {
                            item.NomeEmpregado = DBNAV2009Employees.GetAll(item.NoEmpregado, _config.NAV2009DatabaseName, _config.NAV2009CompanyName).FirstOrDefault().Name;
                        }

                        if (!string.IsNullOrEmpty(item.Recurso))
                        {
                            NAVResourcesViewModel resource = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, item.Recurso, "", 0, "").FirstOrDefault();

                            if (resource != null)
                            {
                                item.NomeRecurso = resource.Name;
                                item.FamiliaRecurso = resource.ResourceGroup;
                            }
                        }

                        if (!string.IsNullOrEmpty(item.DataHoraCriacaoTexto))
                        {
                            item.DataHoraCriacao = Convert.ToDateTime(item.DataHoraCriacaoTexto);
                            item.DataHoraCriacaoTexto = "";
                        }
                    }
                }
            }
            return Json(ListToCreate);
        }

        //4
        [HttpPost]
        public JsonResult UpdateCreate_EmpregadoRecursos([FromBody] List<RHRecursosViewModel> data)
        {
            List<RhRecursosFh> results = DBRHRecursosFH.GetAll();

            data.RemoveAll(x => results.Any(
                u =>
                    u.NoEmpregado == x.NoEmpregado &&
                    u.Recurso == x.Recurso
            ));

            data.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.NoEmpregado) && !string.IsNullOrWhiteSpace(x.Recurso))
                {
                    RhRecursosFh toCreate = DBRHRecursosFH.ParseToDB(x);
                    RhRecursosFh toUpdate = DBRHRecursosFH.ParseToDB(x);
                    RhRecursosFh toSearch = DBRHRecursosFH.GetByID(x.NoEmpregado, x.Recurso);

                    NAVResourcesViewModel resource = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, x.Recurso, "", 0, "").FirstOrDefault();
                    NAVEmployeeViewModel employee = DBNAV2009Employees.GetAll(x.NoEmpregado, _config.NAV2009DatabaseName, _config.NAV2009CompanyName).FirstOrDefault();

                    if (toSearch == null)
                    {
                        toCreate.NoEmpregado = x.NoEmpregado;
                        toCreate.Recurso = x.Recurso;
                        if (resource != null)
                        {
                            toCreate.NomeRecurso = resource.Name;
                            toCreate.FamiliaRecurso = resource.ResourceGroup;
                        }
                        else
                        {
                            toCreate.NomeRecurso = null;
                            toCreate.FamiliaRecurso = null;
                        }
                        if (employee != null)
                            toCreate.NomeEmpregado = employee.Name;
                        else
                            toCreate.NomeEmpregado = null;
                        toCreate.CriadoPor = User.Identity.Name;
                        toCreate.DataHoraCriacao = DateTime.Now;

                        DBRHRecursosFH.Create(toCreate);
                    }
                    else
                    {
                        toCreate.NoEmpregado = x.NoEmpregado;
                        toCreate.Recurso = x.Recurso;
                        if (resource != null)
                        {
                            toCreate.NomeRecurso = resource.Name;
                            toCreate.FamiliaRecurso = resource.ResourceGroup;
                        }
                        else
                        {
                            toCreate.NomeRecurso = null;
                            toCreate.FamiliaRecurso = null;
                        }
                        if (employee != null)
                            toCreate.NomeEmpregado = employee.Name;
                        else
                            toCreate.NomeEmpregado = null;
                        toUpdate.CriadoPor = x.UtilizadorCriacao;
                        toUpdate.DataHoraCriacao = x.DataHoraCriacao;
                        toUpdate.AlteradoPor = User.Identity.Name;
                        toUpdate.DataHoraUltimaAlteracao = DateTime.Now;

                        DBRHRecursosFH.Update(toUpdate);
                    }
                }
            });
            return Json(data);
        }

        [HttpPost]
        [Route("Administracao/FileUpload_PrecoVendaRecursoFH")]
        public JsonResult FileUpload_PrecoVendaRecursoFH()
        {
            //TESTE COM DLL EPPlus
            var files = Request.Form.Files;
            bool global_result = true;
            foreach (var file in files)
            {
                try
                {
                    string name = Path.GetFileNameWithoutExtension(file.FileName);
                    string filename = Path.GetFileName(file.FileName);
                    var full_path = Path.Combine(_generalConfig.FileUploadFolder + "Administracao\\", User.Identity.Name + "_" + filename);
                    if (System.IO.File.Exists(full_path))
                        System.IO.File.Delete(full_path);
                    FileStream dd = new FileStream(full_path, FileMode.CreateNew);
                    file.CopyTo(dd);
                    dd.Dispose();
                    var existingFile = new FileInfo(full_path);

                    string filename_result = name + "_Resultado.xlsx";
                    var full_path_result = Path.Combine(_generalConfig.FileUploadFolder + "Administracao\\", User.Identity.Name + "_" + filename_result);
                    if (System.IO.File.Exists(full_path_result))
                        System.IO.File.Delete(full_path_result);
                    var existingFile_result = new FileInfo(full_path_result);

                    using (var excel = new ExcelPackage(existingFile))
                    {
                        var excel_result = new ExcelPackage(existingFile_result);
                        ExcelWorkbook workBook_result = excel_result.Workbook;

                        ExcelWorkbook workBook = excel.Workbook;
                        if (workBook != null)
                        {
                            if (workBook.Worksheets.Count > 0)
                            {
                                workBook_result = Criar_Excel_Worksheet_PrecoVendaRecursoFH(workBook_result, "ORIGINAL");
                                workBook_result = Criar_Excel_Worksheet_PrecoVendaRecursoFH(workBook_result, "SUCESSO");
                                workBook_result = Criar_Excel_Worksheet_PrecoVendaRecursoFH(workBook_result, "ERRO");

                                ExcelWorksheet currentWorksheet = workBook.Worksheets[0];
                                ExcelWorksheet currentWorksheet_ORIGINAL = workBook_result.Worksheets["ORIGINAL"];
                                ExcelWorksheet currentWorksheet_SUCESSO = workBook_result.Worksheets["SUCESSO"];
                                ExcelWorksheet currentWorksheet_ERRO = workBook_result.Worksheets["ERRO"];

                                if ((currentWorksheet.Dimension.End.Row > 1) &&
                                    (currentWorksheet.Cells[1, 1].Value.ToString() == "Cod Familia Recurso") &&
                                    (currentWorksheet.Cells[1, 2].Value.ToString() == "Cod Tipo Trabalho") &&
                                    (currentWorksheet.Cells[1, 3].Value.ToString() == "Preco Unitario") &&
                                    (currentWorksheet.Cells[1, 4].Value.ToString() == "Custo Unitario") &&
                                    (currentWorksheet.Cells[1, 5].Value.ToString() == "Data Inicio") &&
                                    (currentWorksheet.Cells[1, 6].Value.ToString() == "Data Fim"))
                                {
                                    //List<NAVResourcesViewModel> Lista_Resources, List< TipoTrabalhoFh > Lista_TipoTrabalhoFh, List<PrecoVendaRecursoFh> Lista_PrecoVendaRecursoFh


                                    List<NAVResourcesViewModel> Lista_Resources = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, "", "", 0, "");
                                    List<TipoTrabalhoFh> Lista_TipoTrabalhoFh = DBTipoTrabalhoFH.GetAll();
                                    int Linha_ORIGINAL = 2;
                                    int Linha_SUCESSO = 2;
                                    int Linha_ERRO = 2;
                                    var result_list = new List<bool>();
                                    for (int i = 1; i <= 8; i++)
                                    {
                                        result_list.Add(false);
                                    }

                                    string CodFamiliaRecurso = "";
                                    string CodTipoTrabalho = "";
                                    string PrecoUnitario = "";
                                    string CustoUnitario = "";
                                    string DataInicio = "";
                                    string DataFim = "";

                                    //VALIDAÇÃO DE TODOS OS CAMPOS
                                    for (int rowNumber = 2; rowNumber <= currentWorksheet.Dimension.End.Row; rowNumber++)
                                    {
                                        CodFamiliaRecurso = currentWorksheet.Cells[rowNumber, 1].Value == null ? "" : currentWorksheet.Cells[rowNumber, 1].Value.ToString();
                                        CodTipoTrabalho = currentWorksheet.Cells[rowNumber, 2].Value == null ? "" : currentWorksheet.Cells[rowNumber, 2].Value.ToString();
                                        PrecoUnitario = currentWorksheet.Cells[rowNumber, 3].Value == null ? "" : currentWorksheet.Cells[rowNumber, 3].Value.ToString();
                                        CustoUnitario = currentWorksheet.Cells[rowNumber, 4].Value == null ? "" : currentWorksheet.Cells[rowNumber, 4].Value.ToString();
                                        DataInicio = currentWorksheet.Cells[rowNumber, 5].Value == null ? "" : currentWorksheet.Cells[rowNumber, 5].Value.ToString();
                                        DataFim = currentWorksheet.Cells[rowNumber, 6].Value == null ? "" : currentWorksheet.Cells[rowNumber, 6].Value.ToString();

                                        result_list = Validar_LinhaExcel_PrecoVendaRecursoFH(CodFamiliaRecurso, CodTipoTrabalho, PrecoUnitario, CustoUnitario, DataInicio, DataFim,
                                            result_list, Lista_Resources, Lista_TipoTrabalhoFh);

                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 1].Value = CodFamiliaRecurso;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 2].Value = CodTipoTrabalho;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 3].Value = PrecoUnitario;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 4].Value = CustoUnitario;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 5].Value = DataInicio;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 6].Value = DataFim;

                                        Linha_ORIGINAL = Linha_ORIGINAL + 1;

                                        if (result_list[1] == false && result_list[2] == false && result_list[3] == false && result_list[4] == false
                                             && result_list[5] == false && result_list[6] == false)
                                        {
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 1].Value = CodFamiliaRecurso;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 2].Value = CodTipoTrabalho;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 3].Value = PrecoUnitario;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 4].Value = CustoUnitario;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 5].Value = DataInicio;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 6].Value = DataFim;

                                            if (result_list[7] == true)
                                            {
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 1].Style.Font.Color.SetColor(Color.Orange);
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 2].Style.Font.Color.SetColor(Color.Orange);
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 5].Style.Font.Color.SetColor(Color.Orange);
                                            }

                                            Linha_SUCESSO = Linha_SUCESSO + 1;
                                        }
                                        else
                                        {
                                            global_result = false;

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 1].Value = CodFamiliaRecurso;
                                            if (result_list[1] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 1].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 2].Value = CodTipoTrabalho;
                                            if (result_list[2] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 2].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 3].Value = PrecoUnitario;
                                            if (result_list[3] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 3].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 4].Value = CustoUnitario;
                                            if (result_list[4] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 4].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 5].Value = DataInicio;
                                            if (result_list[5] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 5].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 6].Value = DataFim;
                                            if (result_list[6] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 6].Style.Font.Color.SetColor(Color.Red);

                                            Linha_ERRO = Linha_ERRO + 1;
                                        }

                                        if (result_list.All(c => c == false))
                                        {
                                            PrecoVendaRecursoFh toCreate = DBPrecoVendaRecursoFH.Create(new PrecoVendaRecursoFh()
                                            {
                                                Code = CodFamiliaRecurso,
                                                Descricao = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, CodFamiliaRecurso, "", 0, "").SingleOrDefault().Name,
                                                CodTipoTrabalho = CodTipoTrabalho,
                                                PrecoUnitario = PrecoUnitario == "" ? (decimal?)null : Convert.ToDecimal(PrecoUnitario),
                                                CustoUnitario = CustoUnitario == "" ? (decimal?)null : Convert.ToDecimal(CustoUnitario),
                                                StartingDate = Convert.ToDateTime(DataInicio),
                                                EndingDate = DataFim == "" ? (DateTime?)null : Convert.ToDateTime(DataFim),
                                                FamiliaRecurso = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, CodFamiliaRecurso, "", 0, "").SingleOrDefault().ResourceGroup,
                                                CriadoPor = User.Identity.Name,
                                                DataHoraCriacao = DateTime.Now
                                            });
                                        }

                                        if (result_list[1] == false && result_list[2] == false && result_list[3] == false && result_list[4] == false
                                             && result_list[5] == false && result_list[6] == false && result_list[7] == true)
                                        {
                                            PrecoVendaRecursoFh toUpdate = DBPrecoVendaRecursoFH.Update(new PrecoVendaRecursoFh()
                                            {
                                                Code = CodFamiliaRecurso,
                                                Descricao = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, CodFamiliaRecurso, "", 0, "").SingleOrDefault().Name,
                                                CodTipoTrabalho = CodTipoTrabalho,
                                                PrecoUnitario = PrecoUnitario == "" ? (decimal?)null : Convert.ToDecimal(PrecoUnitario),
                                                CustoUnitario = CustoUnitario == "" ? (decimal?)null : Convert.ToDecimal(CustoUnitario),
                                                StartingDate = Convert.ToDateTime(DataInicio),
                                                EndingDate = DataFim == "" ? (DateTime?)null : Convert.ToDateTime(DataFim),
                                                FamiliaRecurso = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, CodFamiliaRecurso, "", 0, "").SingleOrDefault().ResourceGroup,
                                                CriadoPor = DBPrecoVendaRecursoFH.GetAll().Where(x => x.Code == CodFamiliaRecurso && x.CodTipoTrabalho == CodTipoTrabalho && x.StartingDate == Convert.ToDateTime(DataInicio)).SingleOrDefault().CriadoPor,
                                                DataHoraCriacao = DBPrecoVendaRecursoFH.GetAll().Where(x => x.Code == CodFamiliaRecurso && x.CodTipoTrabalho == CodTipoTrabalho && x.StartingDate == Convert.ToDateTime(DataInicio)).SingleOrDefault().DataHoraCriacao,
                                                AlteradoPor = User.Identity.Name,
                                                DataHoraUltimaAlteracao = DateTime.Now
                                            });
                                        }
                                    }

                                    excel_result.Save();

                                    byte[] Anexo_Result = System.IO.File.ReadAllBytes(full_path_result);

                                    AnexosErros newAnexo = new AnexosErros();
                                    newAnexo.Origem = 3; //Preco Venda Recurso FH
                                    if (global_result)
                                        newAnexo.Tipo = 1; //SUCESSO
                                    else
                                        newAnexo.Tipo = 2; //INSUCESSO
                                    newAnexo.Codigo = "";
                                    newAnexo.NomeAnexo = filename_result;
                                    newAnexo.Anexo = Anexo_Result;
                                    newAnexo.CriadoPor = User.Identity.Name;
                                    newAnexo.DataHoraCriacao = DateTime.Now;
                                    DBAnexosErros.Create(newAnexo);

                                    excel.Dispose();
                                    excel_result.Dispose();

                                    System.IO.File.Delete(full_path_result);
                                    System.IO.File.Delete(full_path);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return Json("");
        }

        public List<bool> Validar_LinhaExcel_PrecoVendaRecursoFH(string CodFamiliaRecurso, string CodTipoTrabalho, string PrecoUnitario, string CustoUnitario, string DataInicio, string DataFim,
            List<bool> result_list, List<NAVResourcesViewModel> Lista_Resources, List<TipoTrabalhoFh> Lista_TipoTrabalhoFh)
        {
            DateTime currectDate;
            decimal currectDecimal;

            for (int i = 1; i <= 7; i++)
            {
                result_list[i] = false;
            }

            if (Lista_Resources.Where(x => x.Code == CodFamiliaRecurso).Count() == 0)
                //if (DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, CodFamiliaRecurso, "", 0, "").Count() == 0)
                result_list[1] = true;


            if (Lista_TipoTrabalhoFh.Where(x => x.Codigo == CodTipoTrabalho).Count() == 0)
                //if (DBTipoTrabalhoFH.GetAll().Where(x => x.Codigo == CodTipoTrabalho).Count() == 0)
                result_list[2] = true;

            if (PrecoUnitario != "")
                if (!decimal.TryParse(PrecoUnitario, out currectDecimal))
                    result_list[3] = true;

            if (CustoUnitario != "")
                if (!decimal.TryParse(CustoUnitario, out currectDecimal))
                    result_list[4] = true;

            if (!DateTime.TryParse(DataInicio, out currectDate))
                result_list[5] = true;

            if (DataFim != "")
                if (!DateTime.TryParse(DataFim, out currectDate))
                    result_list[6] = true;

            if (result_list[1] == false && result_list[2] == false && result_list[5] == false)
            {
                if (DBPrecoVendaRecursoFH.GetAll().Where(x => x.Code == CodFamiliaRecurso && x.CodTipoTrabalho == CodTipoTrabalho && x.StartingDate == Convert.ToDateTime(DataInicio)).Count() > 0)
                    result_list[7] = true;
            }

            return result_list;
        }

        public ExcelWorkbook Criar_Excel_Worksheet_PrecoVendaRecursoFH(ExcelWorkbook workBook, string Nome)
        {
            workBook.Worksheets.Add(Nome);
            ExcelWorksheet currentWorksheet = workBook.Worksheets[Nome];

            currentWorksheet.Cells[1, 1].Value = "Cod Familia Recurso";
            currentWorksheet.Cells[1, 2].Value = "Cod Tipo Trabalho";
            currentWorksheet.Cells[1, 3].Value = "Preco Unitario";
            currentWorksheet.Cells[1, 4].Value = "Custo Unitario";
            currentWorksheet.Cells[1, 5].Value = "Data Inicio";
            currentWorksheet.Cells[1, 6].Value = "Data Fim";

            return workBook;
        }

        #endregion

        #region Configuração Preço Custo Recursos FH
        public IActionResult ConfiguracaoPrecoCustoRecursoFH(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminFolhaHoras);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetConfiguracaoPrecoCustoRecursoFH()
        {
            List<PrecoCustoRecursoViewModel> result = DBPrecoCustoRecursoFH.ParseListToViewModel(DBPrecoCustoRecursoFH.GetAll());
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateConfiguracaoPrecoCustoRecursoFH([FromBody] PrecoCustoRecursoViewModel data)
        {

            PrecoCustoRecursoFh toCreate = DBPrecoCustoRecursoFH.ParseToDB(data);
            toCreate.CriadoPor = User.Identity.Name;
            var result = DBPrecoCustoRecursoFH.Create(toCreate);

            return Json(data);
        }

        [HttpPost]
        public JsonResult DeletePrecoCustoRecursoFH([FromBody] PrecoCustoRecursoViewModel data)
        {
            var result = DBPrecoCustoRecursoFH.Delete(DBPrecoCustoRecursoFH.ParseToDB(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdatePrecoCustoRecursoFH([FromBody] List<PrecoCustoRecursoViewModel> data)
        {
            List<PrecoCustoRecursoFh> results = DBPrecoCustoRecursoFH.GetAll();

            data.RemoveAll(x => DBPrecoCustoRecursoFH.ParseListToViewModel(results).Any(
                u =>
                    u.Code == x.Code &&
                    u.Descricao == x.Descricao &&
                    u.CodTipoTrabalho == x.CodTipoTrabalho &&
                    u.CustoUnitario == x.CustoUnitario &&
                    u.StartingDate == x.StartingDate &&
                    u.EndingDate == x.EndingDate &&
                    u.FamiliaRecurso == x.FamiliaRecurso &&
                    u.UtilizadorCriacao == x.UtilizadorCriacao &&
                    u.DataHoraCriacao == x.DataHoraCriacao
            ));

            data.ForEach(x =>
            {
                PrecoCustoRecursoFh toUpdate = DBPrecoCustoRecursoFH.ParseToDB(x);
                toUpdate.AlteradoPor = User.Identity.Name;
                DBPrecoCustoRecursoFH.Update(toUpdate);
            });
            return Json(data);
        }
        #endregion

        #region Configuração RH Recursos FH
        public IActionResult ConfiguracaoRHRecursosFH(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminFolhaHoras);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult ConfiguracaoAutorizacaoFHRH(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminFolhaHoras);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetRHRecursosFH()
        {
            List<RHRecursosViewModel> result = DBRHRecursosFH.ParseListToViewModel(DBRHRecursosFH.GetAll());
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetRHRecursosFH_AnexosErros()
        {
            //ORIGEM = 2 » RH RECURSOS FH
            //TIPO = 2 » ERRO
            List<AnexosErrosViewModel> result = DBAnexosErros.GetByOrigemAndCodigo(2, "").Select(x => new AnexosErrosViewModel()
            {
                ID = x.Id,
                CodeTexto = x.Id.ToString(),
                Origem = (int)x.Origem,
                OrigemTexto = x.Origem == 0 ? "" : EnumerablesFixed.AE_Origem.Where(y => y.Id == x.Origem).SingleOrDefault().Value,
                Tipo = (int)x.Tipo,
                TipoTexto = x.Tipo == 0 ? "" : EnumerablesFixed.AE_Tipo.Where(y => y.Id == x.Tipo).SingleOrDefault().Value,
                Codigo = x.Codigo,
                NomeAnexo = x.NomeAnexo,
                Anexo = x.Anexo,
                CriadoPor = x.CriadoPor,
                CriadoPorNome = x.CriadoPor == null ? "" : DBUserConfigurations.GetById(x.CriadoPor).Nome,
                DataHora_Criacao = x.DataHoraCriacao,
                DataHora_CriacaoTexto = x.DataHoraCriacao == null ? "" : x.DataHoraCriacao.Value.ToString("yyyy-MM-dd"),
                AlteradoPor = x.AlteradoPor,
                AlteradoPorNome = x.AlteradoPor == null ? "" : DBUserConfigurations.GetById(x.AlteradoPor).Nome,
                DataHora_Alteracao = x.DataHoraAlteracao,
                DataHora_AlteracaoTexto = x.DataHoraAlteracao == null ? "" : x.DataHoraAlteracao.Value.ToString("yyyy-MM-dd")
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateRHRecursosFH([FromBody] RHRecursosViewModel data)
        {
            int resultFinal = 0;

            RhRecursosFh toCreate = DBRHRecursosFH.ParseToDB(data);

            NAVResourcesViewModel resource = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, data.Recurso, "", 0, "").FirstOrDefault();
            NAVEmployeeViewModel employee = DBNAV2009Employees.GetAll(data.NoEmpregado, _config.NAV2009DatabaseName, _config.NAV2009CompanyName).FirstOrDefault();

            toCreate.NomeRecurso = resource.Name;
            toCreate.FamiliaRecurso = resource.ResourceGroup;
            toCreate.NomeEmpregado = employee.Name;
            toCreate.CriadoPor = User.Identity.Name;

            var result = DBRHRecursosFH.Create(toCreate);

            if (result == null)
                resultFinal = 0;
            else
                resultFinal = 1;

            return Json(resultFinal);
        }

        [HttpPost]
        public JsonResult DeleteRHRecursosFH([FromBody] RHRecursosViewModel data)
        {
            var result = DBRHRecursosFH.Delete(DBRHRecursosFH.ParseToDB(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateRHRecursosFH([FromBody] List<RHRecursosViewModel> data)
        {
            List<RhRecursosFh> results = DBRHRecursosFH.GetAll();

            //data.RemoveAll(x => DBRHRecursosFH.ParseListToViewModel(results).Any(
            //    u =>
            //        u.NoEmpregado == x.NoEmpregado &&
            //        u.Recurso == x.Recurso &&
            //        u.NomeRecurso == x.NomeRecurso &&
            //        u.FamiliaRecurso == x.FamiliaRecurso &&
            //        u.NomeEmpregado == x.NomeEmpregado &&
            //        u.UtilizadorCriacao == x.UtilizadorCriacao &&
            //        u.DataHoraCriacao == x.DataHoraCriacao
            //));

            data.ForEach(x =>
            {
                RhRecursosFh toCreate = DBRHRecursosFH.ParseToDB(x);
                RhRecursosFh toDelete = DBRHRecursosFH.ParseToDB(x);

                NAVResourcesViewModel resource = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, x.Recurso, "", 0, "").FirstOrDefault();
                NAVEmployeeViewModel employee = DBNAV2009Employees.GetAll(x.NoEmpregado, _config.NAV2009DatabaseName, _config.NAV2009CompanyName).FirstOrDefault();

                toCreate.NoEmpregado = x.NoEmpregado;
                toCreate.Recurso = x.Recurso;
                toCreate.NomeRecurso = resource.Name;
                toCreate.FamiliaRecurso = resource.ResourceGroup;
                toCreate.NomeEmpregado = employee.Name;
                toCreate.CriadoPor = x.UtilizadorCriacao;
                toCreate.DataHoraCriacao = x.DataHoraCriacao;
                toCreate.AlteradoPor = User.Identity.Name;
                toCreate.DataHoraUltimaAlteracao = DateTime.Now;

                DBRHRecursosFH.Delete(toDelete);

                DBRHRecursosFH.Create(toCreate);

                //DBRHRecursosFH.Update(toUpdate);
            });
            return Json(data);
        }

        [HttpGet]
        [Route("Administracao/DownloadRHRecursosFHTemplate")]
        [Route("Administracao/DownloadRHRecursosFHTemplate/{FileName}")]
        public FileStreamResult DownloadRHRecursosFHTemplate(string FileName)
        {
            return new FileStreamResult(new FileStream(_generalConfig.FileUploadFolder + "Administracao\\" + FileName, FileMode.Open), "application /xlsx");
        }

        [HttpPost]
        [Route("Administracao/FileUpload_FHEmpregadoRecursos")]
        public JsonResult FileUpload_FHEmpregadoRecursos()
        {
            //TESTE COM DLL EPPlus
            var files = Request.Form.Files;
            bool global_result = true;
            foreach (var file in files)
            {
                try
                {
                    string name = Path.GetFileNameWithoutExtension(file.FileName);
                    string filename = Path.GetFileName(file.FileName);
                    var full_path = Path.Combine(_generalConfig.FileUploadFolder + "Administracao\\", User.Identity.Name + "_" + filename);
                    if (System.IO.File.Exists(full_path))
                        System.IO.File.Delete(full_path);
                    FileStream dd = new FileStream(full_path, FileMode.CreateNew);
                    file.CopyTo(dd);
                    dd.Dispose();
                    var existingFile = new FileInfo(full_path);

                    string filename_result = name + "_Resultado.xlsx";
                    var full_path_result = Path.Combine(_generalConfig.FileUploadFolder + "Administracao\\", User.Identity.Name + "_" + filename_result);
                    if (System.IO.File.Exists(full_path_result))
                        System.IO.File.Delete(full_path_result);
                    var existingFile_result = new FileInfo(full_path_result);

                    using (var excel = new ExcelPackage(existingFile))
                    {
                        var excel_result = new ExcelPackage(existingFile_result);
                        ExcelWorkbook workBook_result = excel_result.Workbook;

                        ExcelWorkbook workBook = excel.Workbook;
                        if (workBook != null)
                        {
                            if (workBook.Worksheets.Count > 0)
                            {
                                workBook_result = Criar_Excel_Worksheet_FHEmpregadoRecursos(workBook_result, "ORIGINAL");
                                workBook_result = Criar_Excel_Worksheet_FHEmpregadoRecursos(workBook_result, "SUCESSO");
                                workBook_result = Criar_Excel_Worksheet_FHEmpregadoRecursos(workBook_result, "ERRO");

                                ExcelWorksheet currentWorksheet = workBook.Worksheets[0];
                                ExcelWorksheet currentWorksheet_ORIGINAL = workBook_result.Worksheets["ORIGINAL"];
                                ExcelWorksheet currentWorksheet_SUCESSO = workBook_result.Worksheets["SUCESSO"];
                                ExcelWorksheet currentWorksheet_ERRO = workBook_result.Worksheets["ERRO"];

                                if ((currentWorksheet.Dimension.End.Row > 1) &&
                                    (currentWorksheet.Cells[1, 1].Value.ToString() == "Empregado") &&
                                    (currentWorksheet.Cells[1, 2].Value.ToString() == "Recurso"))
                                {
                                    List<NAVEmployeeViewModel> Lista_Employees = DBNAV2009Employees.GetAll("", _config.NAV2009DatabaseName, _config.NAV2009CompanyName);
                                    List<NAVResourcesViewModel> Lista_Resources = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, "", "", 0, "");
                                    int Linha_ORIGINAL = 2;
                                    int Linha_SUCESSO = 2;
                                    int Linha_ERRO = 2;
                                    var result_list = new List<bool>();
                                    for (int i = 1; i <= 4; i++)
                                    {
                                        result_list.Add(false);
                                    }

                                    string Empregado = "";
                                    string Recurso = "";

                                    //VALIDAÇÃO DE TODOS OS CAMPOS
                                    for (int rowNumber = 2; rowNumber <= currentWorksheet.Dimension.End.Row; rowNumber++)
                                    {
                                        Empregado = currentWorksheet.Cells[rowNumber, 1].Value == null ? "" : currentWorksheet.Cells[rowNumber, 1].Value.ToString();
                                        Recurso = currentWorksheet.Cells[rowNumber, 2].Value == null ? "" : currentWorksheet.Cells[rowNumber, 2].Value.ToString();

                                        result_list = Validar_LinhaExcel_FHEmpregadoRecursos(Empregado, Recurso,
                                            result_list, Lista_Employees, Lista_Resources);

                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 1].Value = Empregado;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 2].Value = Recurso;

                                        Linha_ORIGINAL = Linha_ORIGINAL + 1;

                                        if (result_list[1] == false && result_list[2] == false)
                                        {
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 1].Value = Empregado;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 2].Value = Recurso;

                                            if (result_list[3] == true)
                                            {
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 1].Style.Font.Color.SetColor(Color.Orange);
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 2].Style.Font.Color.SetColor(Color.Orange);
                                            }

                                            Linha_SUCESSO = Linha_SUCESSO + 1;
                                        }
                                        else
                                        {
                                            global_result = false;

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 1].Value = Empregado;
                                            if (result_list[1] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 1].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 2].Value = Recurso;
                                            if (result_list[2] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 2].Style.Font.Color.SetColor(Color.Red);

                                            Linha_ERRO = Linha_ERRO + 1;
                                        }

                                        if (result_list.All(c => c == false))
                                        {
                                            RhRecursosFh toCreate = DBRHRecursosFH.Create(new RhRecursosFh()
                                            {
                                                NoEmpregado = Empregado,
                                                Recurso = Recurso,
                                                NomeRecurso = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, Recurso, "", 0, "").SingleOrDefault().Name,
                                                FamiliaRecurso = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, Recurso, "", 0, "").SingleOrDefault().ResourceGroup,
                                                NomeEmpregado = DBNAV2009Employees.GetAll(Empregado, _config.NAV2009DatabaseName, _config.NAV2009CompanyName).SingleOrDefault().Name,
                                                CriadoPor = User.Identity.Name,
                                                DataHoraCriacao = DateTime.Now
                                            });
                                        }

                                        if (result_list[1] == false && result_list[2] == false && result_list[3] == true)
                                        {
                                            RhRecursosFh toUpdate = DBRHRecursosFH.Update(new RhRecursosFh()
                                            {
                                                NoEmpregado = Empregado,
                                                Recurso = Recurso,
                                                NomeRecurso = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, Recurso, "", 0, "").SingleOrDefault().Name,
                                                FamiliaRecurso = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, Recurso, "", 0, "").SingleOrDefault().ResourceGroup,
                                                NomeEmpregado = DBNAV2009Employees.GetAll(Empregado, _config.NAV2009DatabaseName, _config.NAV2009CompanyName).SingleOrDefault().Name,
                                                CriadoPor = DBRHRecursosFH.GetAll().Where(x => x.NoEmpregado == Empregado && x.Recurso == Recurso).SingleOrDefault().CriadoPor,
                                                DataHoraCriacao = DBRHRecursosFH.GetAll().Where(x => x.NoEmpregado == Empregado && x.Recurso == Recurso).SingleOrDefault().DataHoraCriacao,
                                                AlteradoPor = User.Identity.Name,
                                                DataHoraUltimaAlteracao = DateTime.Now
                                            });
                                        }
                                    }

                                    excel_result.Save();

                                    byte[] Anexo_Result = System.IO.File.ReadAllBytes(full_path_result);

                                    AnexosErros newAnexo = new AnexosErros();
                                    newAnexo.Origem = 2; //RH RECURSOS FH
                                    if (global_result)
                                        newAnexo.Tipo = 1; //SUCESSO
                                    else
                                        newAnexo.Tipo = 2; //INSUCESSO
                                    newAnexo.Codigo = "";
                                    newAnexo.NomeAnexo = filename_result;
                                    newAnexo.Anexo = Anexo_Result;
                                    newAnexo.CriadoPor = User.Identity.Name;
                                    newAnexo.DataHoraCriacao = DateTime.Now;
                                    DBAnexosErros.Create(newAnexo);

                                    excel.Dispose();
                                    excel_result.Dispose();

                                    System.IO.File.Delete(full_path_result);
                                    System.IO.File.Delete(full_path);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return Json("");
        }

        public List<bool> Validar_LinhaExcel_FHEmpregadoRecursos(string Empregado, string Recurso,
            List<bool> result_list, List<NAVEmployeeViewModel> Lista_Employees, List<NAVResourcesViewModel> Lista_Resources)
        {
            for (int i = 1; i <= 3; i++)
            {
                result_list[i] = false;
            }

            if (Lista_Employees.Where(x => x.No == Empregado).Count() == 0)
                //if (DBNAV2009Employees.GetAll(Empregado, _config.NAV2009DatabaseName, _config.NAV2009CompanyName).Count() == 0)
                result_list[1] = true;

            if (Lista_Resources.Where(x => x.Code == Recurso).Count() == 0)
                //if (DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, "", "", 0, "").Where(x => x.Code == Recurso).Count() == 0)
                result_list[2] = true;

            if (DBRHRecursosFH.GetAll().Where(x => x.NoEmpregado == Empregado && x.Recurso == Recurso).Count() > 0)
                result_list[3] = true;

            return result_list;
        }

        public ExcelWorkbook Criar_Excel_Worksheet_FHEmpregadoRecursos(ExcelWorkbook workBook, string Nome)
        {
            workBook.Worksheets.Add(Nome);
            ExcelWorksheet currentWorksheet = workBook.Worksheets[Nome];

            currentWorksheet.Cells[1, 1].Value = "Empregado";
            currentWorksheet.Cells[1, 2].Value = "Recurso";

            return workBook;
        }

        #endregion

        #region AutorizacaoFHRH
        [HttpPost]
        public JsonResult GetAutorizacaoFHRH()
        {
            List<AutorizacaoFHRHViewModel> result = DBAutorizacaoFHRH.ParseListToViewModel(DBAutorizacaoFHRH.GetAll());

            result.ForEach(x =>
            {
                x.NomeEmpregado = DBUserConfigurations.GetById(x.NoEmpregado) == null ? "" : DBUserConfigurations.GetById(x.NoEmpregado).Nome;
                x.NomeResponsavel1 = DBUserConfigurations.GetById(x.NoResponsavel1) == null ? "" : DBUserConfigurations.GetById(x.NoResponsavel1).Nome;
                x.NomeResponsavel2 = DBUserConfigurations.GetById(x.NoResponsavel2) == null ? "" : DBUserConfigurations.GetById(x.NoResponsavel2).Nome;
                x.NomeResponsavel3 = DBUserConfigurations.GetById(x.NoResponsavel3) == null ? "" : DBUserConfigurations.GetById(x.NoResponsavel3).Nome;
                x.NomeValidadorRH1 = DBUserConfigurations.GetById(x.ValidadorRH1) == null ? "" : DBUserConfigurations.GetById(x.ValidadorRH1).Nome;
                x.NomeValidadorRH2 = DBUserConfigurations.GetById(x.ValidadorRH2) == null ? "" : DBUserConfigurations.GetById(x.ValidadorRH2).Nome;
                x.NomeValidadorRH3 = DBUserConfigurations.GetById(x.ValidadorRH3) == null ? "" : DBUserConfigurations.GetById(x.ValidadorRH3).Nome;
                x.NomeValidadorRHKM1 = DBUserConfigurations.GetById(x.ValidadorRHKM1) == null ? "" : DBUserConfigurations.GetById(x.ValidadorRHKM1).Nome;
                x.NomeValidadorRHKM2 = DBUserConfigurations.GetById(x.ValidadorRHKM2) == null ? "" : DBUserConfigurations.GetById(x.ValidadorRHKM2).Nome;
            });
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateAutorizacaoFHRH([FromBody] AutorizacaoFHRHViewModel data)
        {
            int resultFinal = 0;
            try
            {
                AutorizacaoFhRh autorizacao = new AutorizacaoFhRh();

                autorizacao.NoEmpregado = data.NoEmpregado;
                autorizacao.NoResponsavel1 = data.NoResponsavel1;
                autorizacao.NoResponsavel2 = data.NoResponsavel2;
                autorizacao.NoResponsavel3 = data.NoResponsavel3;
                autorizacao.ValidadorRh1 = data.ValidadorRH1;
                autorizacao.ValidadorRh2 = data.ValidadorRH2;
                autorizacao.ValidadorRh3 = data.ValidadorRH3;
                autorizacao.ValidadorRhkm1 = data.ValidadorRHKM1;
                autorizacao.ValidadorRhkm2 = data.ValidadorRHKM2;
                autorizacao.CriadoPor = User.Identity.Name;
                autorizacao.DataHoraCriação = DateTime.Now;

                var dbCreateResult = DBAutorizacaoFHRH.Create(autorizacao);

                if (dbCreateResult == null)
                    resultFinal = 0;
                else
                    resultFinal = 1;
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(resultFinal);
        }

        [HttpPost]
        public JsonResult DeleteAutorizacaoFHRH([FromBody] AutorizacaoFHRHViewModel data)
        {
            var result = DBAutorizacaoFHRH.Delete(DBAutorizacaoFHRH.ParseToDB(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateAutorizacaoFHRH([FromBody] List<AutorizacaoFHRHViewModel> data)
        {
            List<AutorizacaoFhRh> results = DBAutorizacaoFHRH.GetAll();

            data.RemoveAll(x => DBAutorizacaoFHRH.ParseListToViewModel(results).Any(
                u =>
                    u.NoEmpregado == x.NoEmpregado &&
                    u.NoResponsavel1 == x.NoResponsavel1 &&
                    u.NoResponsavel2 == x.NoResponsavel2 &&
                    u.NoResponsavel3 == x.NoResponsavel3 &&
                    u.ValidadorRH1 == x.ValidadorRH1 &&
                    u.ValidadorRH2 == x.ValidadorRH2 &&
                    u.ValidadorRH3 == x.ValidadorRH3 &&
                    u.ValidadorRHKM1 == x.ValidadorRHKM1 &&
                    u.ValidadorRHKM2 == x.ValidadorRHKM2 &&
                    u.UtilizadorCriacao == x.UtilizadorCriacao &&
                    u.DataHoraCriacao == x.DataHoraCriacao
            ));

            data.ForEach(x =>
            {
                AutorizacaoFhRh toUpdate = DBAutorizacaoFHRH.ParseToDB(x);
                toUpdate.AlteradoPor = User.Identity.Name;
                DBAutorizacaoFHRH.Update(toUpdate);
            });
            return Json(data);
        }

        #endregion

        #region OrigemDestinoFH
        public IActionResult ConfiguracaoOrigemDestinoFH(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminFolhaHoras);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetOrigemDestinoFH()
        {
            List<OrigemDestinoFHViewModel> result = DBOrigemDestinoFh.ParseListToViewModel(DBOrigemDestinoFh.GetAll());
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateOrigemDestinoFH([FromBody] OrigemDestinoFHViewModel data)
        {
            int resultFinal = 0;
            try
            {
                OrigemDestinoFh OrigemDestinoFH = new OrigemDestinoFh();

                OrigemDestinoFH.Código = data.Codigo;
                OrigemDestinoFH.Descrição = data.Descricao;
                OrigemDestinoFH.CriadoPor = User.Identity.Name;
                OrigemDestinoFH.DataHoraCriação = DateTime.Now;

                var dbCreateResult = DBOrigemDestinoFh.Create(OrigemDestinoFH);

                if (dbCreateResult == null)
                    resultFinal = 0;
                else
                    resultFinal = 1;
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(resultFinal);
        }

        [HttpPost]
        public JsonResult DeleteOrigemDestinoFH([FromBody] OrigemDestinoFHViewModel data)
        {
            var result = DBOrigemDestinoFh.Delete(DBOrigemDestinoFh.ParseToDB(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateOrigemDestinoFH([FromBody] List<OrigemDestinoFHViewModel> data)
        {
            List<OrigemDestinoFh> results = DBOrigemDestinoFh.GetAll();

            data.RemoveAll(x => DBOrigemDestinoFh.ParseListToViewModel(results).Any(
                u =>
                    u.Codigo == x.Codigo &&
                    u.Descricao == x.Descricao &&
                    u.CriadoPor == x.CriadoPor &&
                    u.DataHoraCriacao == x.DataHoraCriacao
            ));

            data.ForEach(x =>
            {
                OrigemDestinoFh toUpdate = DBOrigemDestinoFh.ParseToDB(x);
                toUpdate.AlteradoPor = User.Identity.Name;
                DBOrigemDestinoFh.Update(toUpdate);
            });
            return Json(data);
        }

        #endregion

        #region DistanciaFH
        public IActionResult ConfiguracaoDistanciaFH(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminFolhaHoras);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetDistanciaFH()
        {
            List<DistanciaFHViewModel> result = DBDistanciaFh.ParseListToViewModel(DBDistanciaFh.GetAll());
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateDistanciaFH([FromBody] DistanciaFHViewModel data)
        {
            int resultFinal = 0;
            try
            {
                DistanciaFh DistanciaFH = new DistanciaFh();

                DistanciaFH.CódigoOrigem = data.Origem;
                DistanciaFH.CódigoDestino = data.Destino;
                DistanciaFH.Distância = data.Distancia;
                DistanciaFH.CriadoPor = User.Identity.Name;
                DistanciaFH.DataHoraCriação = DateTime.Now;

                var dbCreateResult = DBDistanciaFh.Create(DistanciaFH);

                if (dbCreateResult == null)
                    resultFinal = 0;
                else
                    resultFinal = 1;
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(resultFinal);
        }

        [HttpPost]
        public JsonResult DeleteDistanciaFH([FromBody] DistanciaFHViewModel data)
        {
            var result = DBDistanciaFh.Delete(DBDistanciaFh.ParseToDB(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateDistanciaFH([FromBody] List<DistanciaFHViewModel> data)
        {
            List<DistanciaFh> results = DBDistanciaFh.GetAll();

            data.RemoveAll(x => DBDistanciaFh.ParseListToViewModel(results).Any(
                u =>
                    u.Origem == x.Origem &&
                    u.Destino == x.Destino &&
                    u.Distancia == x.Distancia &&
                    u.CriadoPor == x.CriadoPor &&
                    u.DataHoraCriacao == x.DataHoraCriacao
            ));

            data.ForEach(x =>
            {
                DistanciaFh toUpdate = DBDistanciaFh.ParseToDB(x);
                toUpdate.AlteradoPor = User.Identity.Name;
                DBDistanciaFh.Update(toUpdate);
            });
            return Json(data);
        }

        #endregion

        #region Configuracao Recursos Folha Horas
        public IActionResult ConfiguracaoRecursosFolhaHoras(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminFolhaHoras);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetConfiguracaoRecursosFolhaHoras()
        {
            List<TabelaConfRecursosFHViewModel> result = DBTabelaConfRecursosFh.ParseListToViewModel(DBTabelaConfRecursosFh.GetAll());

            if (result != null)
            {
                result.ForEach(x =>
                {
                    x.Descricao = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, x.CodigoRecurso, "", 0, "").FirstOrDefault().Name;
                    x.UnidMedida = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, x.CodigoRecurso, "", 0, "").FirstOrDefault().MeasureUnit;
                });
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateConfiguracaoRecursosFolhaHoras([FromBody] TabelaConfRecursosFHViewModel data)
        {
            int resultFinal = 0;

            TabelaConfRecursosFh toCreate = DBTabelaConfRecursosFh.ParseToDB(data);
            //toCreate.UtilizadorCriacao = User.Identity.Name;
            var result = DBTabelaConfRecursosFh.Create(toCreate);

            if (result == null)
                resultFinal = 0;
            else
                resultFinal = 1;

            return Json(resultFinal);
        }

        [HttpPost]
        public JsonResult DeleteConfiguracaoRecursosFolhaHoras([FromBody] TabelaConfRecursosFHViewModel data)
        {
            var result = DBTabelaConfRecursosFh.Delete(DBTabelaConfRecursosFh.ParseToDB(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateConfiguracaoRecursosFolhaHoras([FromBody] List<TabelaConfRecursosFHViewModel> data)
        {
            List<TabelaConfRecursosFh> results = DBTabelaConfRecursosFh.GetAll();

            data.RemoveAll(x => DBTabelaConfRecursosFh.ParseListToViewModel(results).Any(
                u =>
                    u.Tipo == x.Tipo &&
                    u.CodigoRecurso == x.CodigoRecurso &&
                    u.Descricao == x.Descricao &&
                    u.UnidMedida == x.UnidMedida &&
                    u.PrecoUnitarioCusto == x.PrecoUnitarioCusto &&
                    u.PrecoUnitarioVenda == x.PrecoUnitarioVenda &&
                    u.RubricaSalarial == x.RubricaSalarial &&
                    u.CalculoAutomatico == x.CalculoAutomatico
            ));

            data.ForEach(x =>
            {
                TabelaConfRecursosFh toUpdate = DBTabelaConfRecursosFh.ParseToDB(x);
                //toUpdate.UtilizadorModificacao = User.Identity.Name;
                toUpdate.Descricao = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, x.CodigoRecurso, "", 0, "").FirstOrDefault().Name;
                toUpdate.UnidMedida = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, x.CodigoRecurso, "", 0, "").FirstOrDefault().MeasureUnit;
                DBTabelaConfRecursosFh.Update(toUpdate);
            });
            return Json(data);
        }
        #endregion

        #region Tipo Requisição
        public IActionResult TiposRequisicao(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminRequisicoes);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetRequisitionTypes()
        {
            List<RequesitionTypeViewModel> result = DBRequesitionType.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName).Select(x => new RequesitionTypeViewModel()
            {
                Code = x.Code,
                Description = x.Description
            }).ToList();
            return Json(result);
        }

        //[HttpPost]
        //public JsonResult UpdateRequesitionTypes([FromBody] List<RequesitionTypeViewModel> data)
        //{
        //    List<NAVRequisitionTypeViewModel> results = DBRequesitionType.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName);
        //    results.RemoveAll(x => data.Any(u => u.Code == x.Code));
        //    results.ForEach(x => DBRequesitionType.Delete(x));
        //    data.ForEach(x =>
        //    {
        //        TiposRequisições TR = new TiposRequisições()
        //        {
        //            Descrição = x.Description
        //        };
        //        if (x.Code > 0)
        //        {
        //            TR.Código = x.Code;
        //            TR.Frota = x.Fleet;
        //            TR.DataHoraModificação = DateTime.Now;
        //            TR.UtilizadorModificação = User.Identity.Name;
        //            DBRequesitionType.Update(TR);
        //        }
        //        else
        //        {
        //            TR.DataHoraCriação = DateTime.Now;
        //            TR.UtilizadorCriação = User.Identity.Name;
        //            TR.Frota = x.Fleet;
        //            DBRequesitionType.Create(TR);
        //        }
        //    });
        //    return Json(data);
        //}
        #endregion

        #region Config Comprador
        public IActionResult ConfigComprador(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminGeral);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetComprador()
        {
            List<CompradorViewModel> result = DBComprador.GetAll().Select(x => new CompradorViewModel()
            {
                CodComprador = x.CodComprador,
                NomeComprador = x.NomeComprador,
                DataHoraCriacao = x.DataHoraCriação,
                DataHoraModificacao = x.DataHoraModificação,
                UtilizadorCriacao = x.UtilizadorCriação,
                UtilizadorModificacao = x.UtilizadorModificação
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateComprador([FromBody] CompradorViewModel CPD)
        {
            if (CPD != null)
            {
                if (DBComprador.GetById(CPD.CodComprador) == null)
                {
                    CPD.DataHoraCriacao = DateTime.Now;
                    CPD.UtilizadorCriacao = User.Identity.Name;

                    if (DBComprador.Create(CPD.ParseToDB()) != null)
                    {
                        CPD.eReasonCode = 1;
                        CPD.eMessage = "O Comprador foi criado com sucesso.";
                    }
                    else
                    {
                        CPD.eReasonCode = 10;
                        CPD.eMessage = "Ocorreu um erro ao criar o Comprador.";
                    }
                }
                else
                {
                    CPD.eReasonCode = 11;
                    CPD.eMessage = "Já existe um comprador com esse Código.";
                }
            }
            else
            {
                CPD.eReasonCode = 12;
                CPD.eMessage = "Ocorreu um erro.";
            }

            return Json(CPD);
        }

        [HttpPost]
        public JsonResult UpdateComprador([FromBody] List<CompradorViewModel> CPD)
        {
            CompradorViewModel result = new CompradorViewModel();
            List<Comprador> results = DBComprador.GetAll();
            results.RemoveAll(x => CPD.Any(u => u.CodComprador == x.CodComprador));
            //results.ForEach(x => DBComprador.Delete(x));
            CPD.ForEach(x =>
            {
                if (x.NomeComprador != "")
                {
                    Comprador cpd = new Comprador()
                    {
                        NomeComprador = x.NomeComprador,
                        DataHoraCriação = x.DataHoraCriacao,
                        UtilizadorCriação = x.UtilizadorCriacao
                    };
                    if (x.CodComprador != "")
                    {
                        cpd.CodComprador = x.CodComprador;
                        cpd.DataHoraModificação = DateTime.Now;
                        cpd.UtilizadorModificação = User.Identity.Name;
                        if (DBComprador.Update(cpd) != null)
                        {
                            result.eReasonCode = 1;
                            result.eMessage = "O Comprador foi atualizado com sucesso.";
                        }
                        else
                        {
                            result.eReasonCode = 20;
                            result.eMessage = "Ocorreu um erro ao atualizar o Comprador.";
                        }
                    }
                    else
                    {
                        result.eReasonCode = 21;
                        result.eMessage = "O campo Código de Comprador não pode estar vazio.";
                    }
                }
                else
                {
                    result.eReasonCode = 22;
                    result.eMessage = "O campo Nome de Comprador não pode estar vazio.";
                }
            });
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteComprador([FromBody] CompradorViewModel CPD)
        {
            if (CPD != null)
            {
                if (DBComprador.GetById(CPD.CodComprador) != null)
                {
                    if (DBComprador.Delete(CPD.ParseToDB()) == true)
                    {
                        CPD.eReasonCode = 1;
                        CPD.eMessage = "O Comprador foi eliminado com sucesso.";
                    }
                    else
                    {
                        CPD.eReasonCode = 30;
                        CPD.eMessage = "Ocorreu um erro ao eliminar o Comprador.";
                    }
                }
                else
                {
                    CPD.eReasonCode = 31;
                    CPD.eMessage = "Não existe um comprador com esse Código.";
                }
            }
            else
            {
                CPD.eReasonCode = 32;
                CPD.eMessage = "Ocorreu um erro.";
            }

            return Json(CPD);
        }
        #endregion

        #region Config Email Fornecedores
        public IActionResult ConfigEmailFornecedores(string id)
        {
            //UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminGeral);
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.LinhasAcordosPrecos);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetEmailFornecedor()
        {
            List<ConfiguraçãoEmailFornecedores> result = DBConfigEmailFornecedores.GetAll().ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateEmailFornecedor([FromBody] ConfiguraçãoEmailFornecedores config)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 0;
            result.eMessage = "Ocorreu um erro.";

            if (config != null)
            {
                if (DBConfigEmailFornecedores.GetById(config.CodFornecedor, config.Cresp) == null)
                {
                    config.Nome = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Where(y => y.No_ == config.CodFornecedor).FirstOrDefault().Name;
                    config.DataHoraCriacao = DateTime.Now;
                    config.UtilizadorCriacao = User.Identity.Name;

                    if (DBConfigEmailFornecedores.Create(config) != null)
                    {
                        result.eReasonCode = 1;
                        result.eMessage = "A Configuração Email Fornecedor foi criada com sucesso.";
                    }
                    else
                    {
                        result.eReasonCode = 10;
                        result.eMessage = "Ocorreu um erro ao criar a Configuração Email Fornecedor.";
                    }
                }
                else
                {
                    result.eReasonCode = 11;
                    result.eMessage = "Já existe uma Configuração Email Fornecedor com esses Códigos.";
                }
            }
            else
            {
                result.eReasonCode = 12;
                result.eMessage = "Ocorreu um erro.";
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateEmailFornecedor([FromBody] List<ConfiguraçãoEmailFornecedores> item)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 0;
            result.eMessage = "Ocorreu um erro.";

            List<ConfiguraçãoEmailFornecedores> results = DBConfigEmailFornecedores.GetAll();
            results.RemoveAll(x => item.Any(u => u.CodFornecedor == x.CodFornecedor && u.Cresp == x.Cresp));
            item.ForEach(x =>
            {
                if (x.Email != "")
                {
                    ConfiguraçãoEmailFornecedores config = new ConfiguraçãoEmailFornecedores()
                    {
                        Email = x.Email,
                        DataHoraCriacao = x.DataHoraCriacao,
                        UtilizadorCriacao = x.UtilizadorCriacao
                    };
                    if (x.CodFornecedor != "")
                    {
                        if (x.Cresp != "")
                        {
                            config.CodFornecedor = x.CodFornecedor;
                            config.Nome = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Where(y => y.No_ == x.CodFornecedor).FirstOrDefault().Name;
                            config.Cresp = x.Cresp;
                            config.DataHoraModificacao = DateTime.Now;
                            config.UtilizadorModificacao = User.Identity.Name;
                            if (DBConfigEmailFornecedores.Update(config) != null)
                            {
                                result.eReasonCode = 1;
                                result.eMessage = "A Configuração Email Fornecedor foi atualizada com sucesso.";
                            }
                            else
                            {
                                result.eReasonCode = 20;
                                result.eMessage = "Ocorreu um erro ao atualizar a Configuração Email Fornecedor.";
                            }
                        }
                        else
                        {
                            result.eReasonCode = 21;
                            result.eMessage = "O campo Centro de Responsabilidade não pode estar vazio.";
                        }
                    }
                    else
                    {
                        result.eReasonCode = 22;
                        result.eMessage = "O campo Código de Fornecedor não pode estar vazio.";
                    }
                }
                else
                {
                    result.eReasonCode = 23;
                    result.eMessage = "O campo Email não pode estar vazio.";
                }
            });
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteEmailFornecedor([FromBody] ConfiguraçãoEmailFornecedores config)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 0;
            result.eMessage = "Ocorreu um erro.";

            if (config != null)
            {
                if (DBConfigEmailFornecedores.GetById(config.CodFornecedor, config.Cresp) != null)
                {
                    if (DBConfigEmailFornecedores.Delete(config) == true)
                    {
                        result.eReasonCode = 1;
                        result.eMessage = "A Configuração Email Fornecedor foi eliminada com sucesso.";
                    }
                    else
                    {
                        result.eReasonCode = 30;
                        result.eMessage = "Ocorreu um erro ao eliminar a Configuração Email Fornecedor.";
                    }
                }
                else
                {
                    result.eReasonCode = 31;
                    result.eMessage = "Não existe uma Configuração Email Fornecedor com esses Códigos.";
                }
            }
            else
            {
                result.eReasonCode = 32;
                result.eMessage = "Ocorreu um erro.";
            }

            return Json(result);
        }
        #endregion

        #region Configurações Aprovações

        public IActionResult ConfiguracaoAprovacoes(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminAprovacoes);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetApprovalConfig()
        {
            List<ApprovalConfigurationsViewModel> result = DBApprovalConfigurations.ParseToViewModel(DBApprovalConfigurations.GetAll());

            List<GruposAprovação> AllAprovGroups = DBApprovalGroups.GetAll();
            result.ForEach(x =>
            {
                if (x.ApprovalGroup > 0)
                {
                    x.ApprovalGroupText = AllAprovGroups.Where(y => y.Código == x.ApprovalGroup).FirstOrDefault().Descrição;
                }
            });

            return Json(result);
        }

        [HttpPost]
        public JsonResult DeteleApprovalConfig([FromBody] ApprovalConfigurationsViewModel data)
        {
            var result = DBApprovalConfigurations.Delete(DBApprovalConfigurations.ParseToDatabase(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateApprovalConfig([FromBody] List<ApprovalConfigurationsViewModel> data)
        {

            data.ForEach(x =>
            {
                ConfiguraçãoAprovações aprovConfig = new ConfiguraçãoAprovações()
                {
                    Tipo = x.Type,
                    NívelAprovação = x.Level,
                    ValorAprovação = x.ApprovalValue,
                    GrupoAprovação = x.ApprovalGroup,
                    UtilizadorAprovação = x.ApprovalUser,
                    //Área = x.Area,
                    CódigoÁreaFuncional = x.FunctionalArea,
                    CódigoCentroResponsabilidade = x.ResponsabilityCenter,
                    CódigoRegião = x.Region,
                    DataInicial = string.IsNullOrEmpty(x.StartDate) ? (DateTime?)null : DateTime.Parse(x.StartDate),
                    DataFinal = string.IsNullOrEmpty(x.EndDate) ? (DateTime?)null : DateTime.Parse(x.EndDate)
                };

                if (!aprovConfig.NívelAprovação.HasValue || aprovConfig.NívelAprovação.Value <= 0)
                    throw new Exception("O nível de aprovação tem que ser maior que zero.");

                if (x.Id > 0)
                {
                    aprovConfig.Id = x.Id;
                    aprovConfig.UtilizadorCriação = x.CreateUser;
                    aprovConfig.DataHoraCriação = x.CreateDate;
                    aprovConfig.CódigoÁreaFuncional = x.FunctionalArea;
                    aprovConfig.CódigoCentroResponsabilidade = x.ResponsabilityCenter;
                    aprovConfig.CódigoRegião = x.Region;
                    aprovConfig.DataHoraModificação = DateTime.Now;
                    aprovConfig.UtilizadorModificação = User.Identity.Name;
                    DBApprovalConfigurations.Update(aprovConfig);
                }
                else
                {
                    aprovConfig.DataHoraCriação = DateTime.Now;
                    aprovConfig.UtilizadorCriação = User.Identity.Name;
                    DBApprovalConfigurations.Create(aprovConfig);
                }
            });
            return Json(data);
        }
        #endregion

        #region Grupo Aprovações
        public IActionResult GruposAprovacoes(string id)
        {
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminAprovacoes);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.UPermissions = userPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        [HttpPost]
        public JsonResult GetApprovalGroup()
        {
            List<ApprovalGroupViewModel> result = DBApprovalGroups.ParseToViewModel(DBApprovalGroups.GetAll());
            return Json(result);
        }

        public JsonResult GetApprovalGroupID([FromBody] int id)
        {
            ApprovalGroupViewModel result = DBApprovalGroups.ParseToViewModel(DBApprovalGroups.GetById(id));
            return Json(result);
        }

        public JsonResult CreateApprovalGroup([FromBody] ApprovalGroupViewModel data)
        {
            ApprovalGroupViewModel result;
            //Create new 
            result = DBApprovalGroups.ParseToViewModel(DBApprovalGroups.Create(DBApprovalGroups.ParseToDatabase(data)));
            if (result != null)
            {
                result.eReasonCode = 100;
            }
            else
                result.eReasonCode = 101;
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateApprovalGroup([FromBody] ApprovalGroupViewModel item)
        {
            DBApprovalGroups.Update(DBApprovalGroups.ParseToDatabase(item));

            return Json(item);
        }

        public JsonResult DeleteApprovalGroup([FromBody] ApprovalGroupViewModel data)
        {
            string eReasonCode = "";
            if (!DBApprovalConfigurations.GetAll().Exists(x => x.GrupoAprovação == data.Code) && !DBApprovalConfigurations.GetAll().Exists(x => x.UtilizadorAprovação == data.Description))
            {
                List<UtilizadoresGruposAprovação> results2 = DBApprovalUserGroup.GetAll();
                results2.ForEach(x =>
                {
                    if (x.GrupoAprovação == data.Code)
                        DBApprovalUserGroup.Delete(x);
                });

                List<GruposAprovação> results = DBApprovalGroups.GetAll();
                results.ForEach(x =>
                {
                    if (x.Código == data.Code)
                        DBApprovalGroups.Delete(x);
                });
                eReasonCode = "100";
            }
            else
            {
                eReasonCode = "101";
            }
            return Json(eReasonCode);
        }

        #endregion

        #region Detalhes Grupos Aprovacoes

        public IActionResult DetalhesGruposAprovacoes(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminGeral);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.GroupApproval = "";
                ViewBag.IDGroupApproval = "";
                if (id != null)
                {
                    int IDGroup = Int32.Parse(id);
                    ViewBag.IDGroupApproval = IDGroup;
                }
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }


        public JsonResult GetDetailsApprovalGroup([FromBody] int id)
        {

            List<ApprovalUserGroupViewModel> result = DBApprovalUserGroup.ParseToViewModel(DBApprovalUserGroup.GetByGroup(id));
            return Json(result);

        }

        public JsonResult CreateDetailsApprovalGroup([FromBody] ApprovalUserGroupViewModel data)
        {
            string eReasonCode = "";
            //Create new 
            data.CreateUser = User.Identity.Name;
            eReasonCode = DBApprovalUserGroup.Create(DBApprovalUserGroup.ParseToDb(data)) == null ? "101" : "";

            if (String.IsNullOrEmpty(eReasonCode))
            {
                return Json(data);
            }
            else
            {
                return Json(eReasonCode);
            }

        }

        public JsonResult UpdateLinhaGrupoAprovacao([FromBody] ApprovalUserGroupViewModel data)
        {
            string eReasonCode = "";
            //Update 
            data.UpdateUser = User.Identity.Name;
            eReasonCode = DBApprovalUserGroup.Update(DBApprovalUserGroup.ParseToDb(data)) == null ? "101" : "";

            if (String.IsNullOrEmpty(eReasonCode))
            {
                return Json(data);
            }
            else
            {
                return Json(eReasonCode);
            }

        }

        [HttpPost]
        public JsonResult DeteleDetailsApprovalGroup([FromBody] ApprovalUserGroupViewModel data)
        {
            var result = DBApprovalUserGroup.Delete(DBApprovalUserGroup.ParseToDb(data));

            return Json(data);
        }
        #endregion

        #region Locais
        public IActionResult Locais(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminRequisicoes);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetPlace()
        {
            List<PlacesViewModel> result = DBPlaces.GetAll().Select(x => new PlacesViewModel()
            {
                Code = x.Código,
                Description = x.Descrição,
                Address = x.Endereço,
                Locality = x.Localidade,
                Postalcode = x.CódigoPostal,
                Contact = x.Contacto,
                Responsiblerecept = x.ResponsávelReceção,
                CreateDate = x.DataHoraCriação.HasValue ? x.DataHoraCriação.Value.ToString("yyyy-MM-dd hh:mm:ss.ff") : "",
                CreateUser = x.UtilizadorCriação
            }).ToList();
            return Json(result);
        }
        [HttpPost]
        public JsonResult DeletePlace([FromBody] PlacesViewModel data)
        {
            var result = DBPlaces.Delete(DBPlaces.ParseToDB(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdatePlace([FromBody] List<PlacesViewModel> data)
        {

            data.ForEach(x =>
            {
                Locais localval = new Locais()
                {
                    Descrição = x.Description,
                    CódigoPostal = x.Postalcode,
                    Endereço = x.Address,
                    Localidade = x.Locality,
                    Contacto = x.Contact,
                    ResponsávelReceção = x.Responsiblerecept
                };
                if (x.Code > 0)
                {
                    localval.Código = x.Code;
                    localval.UtilizadorCriação = x.CreateUser;
                    localval.DataHoraCriação = string.IsNullOrEmpty(x.CreateDate) ? (DateTime?)null : DateTime.Parse(x.CreateDate);
                    localval.DataHoraModificação = DateTime.Now;
                    localval.UtilizadorModificação = User.Identity.Name;
                    DBPlaces.Update(localval);
                }
                else
                {
                    localval.DataHoraCriação = DateTime.Now;
                    localval.UtilizadorCriação = User.Identity.Name;
                    DBPlaces.Create(localval);
                }
            });
            return Json(data);
        }
        #endregion

        #region Unidades Prestação
        public IActionResult UnidadePrestacao(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminVendas);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetFetcUnit()
        {
            List<FetcUnitViewModel> result = DBFetcUnit.GetAll().Select(x => new FetcUnitViewModel()
            {
                Code = x.Código,
                Description = x.Descrição,
                Email1 = x.Email1,
                Email2 = x.Email2,
                Email3 = x.Email3,
                EmailRegiao12 = x.EmailRegiao12,
                EmailRegiao23 = x.EmailRegiao23,
                EmailRegiao33 = x.EmailRegiao33,
                EmailRegiao43 = x.EmailRegiao43,
                CreateDate = x.DataHoraCriação.HasValue ? x.DataHoraCriação.Value.ToString("yyyy-MM-dd hh:mm:ss.ff") : "",
                CreateUser = x.UtilizadorCriação
            }).ToList();
            return Json(result);
        }
        [HttpPost]
        public JsonResult DeleteFetcUnit([FromBody] FetcUnitViewModel data)
        {
            var result = DBFetcUnit.Delete(DBFetcUnit.ParseToDB(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateFetcUnit([FromBody] List<FetcUnitViewModel> data)
        {

            data.ForEach(x =>
            {
                UnidadePrestação Unidadeval = new UnidadePrestação()
                {
                    Descrição = x.Description,
                    Email1 = x.Email1,
                    Email2 = x.Email2,
                    Email3 = x.Email3,
                    EmailRegiao12 = x.EmailRegiao12,
                    EmailRegiao23 = x.EmailRegiao23,
                    EmailRegiao33 = x.EmailRegiao33,
                    EmailRegiao43 = x.EmailRegiao43,
                };
                if (x.Code > 0)
                {
                    Unidadeval.Código = x.Code;
                    Unidadeval.UtilizadorCriação = x.CreateUser;
                    Unidadeval.DataHoraCriação = string.IsNullOrEmpty(x.CreateDate) ? (DateTime?)null : DateTime.Parse(x.CreateDate);
                    Unidadeval.DataHoraModificação = DateTime.Now;
                    Unidadeval.UtilizadorModificação = User.Identity.Name;
                    DBFetcUnit.Update(Unidadeval);
                }
                else
                {
                    Unidadeval.DataHoraCriação = DateTime.Now;
                    Unidadeval.UtilizadorCriação = User.Identity.Name;
                    DBFetcUnit.Create(Unidadeval);
                }
            });
            return Json(data);
        }
        #endregion

        #region Config Vendas Alertas
        public IActionResult ConfigVendasAlertas()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminGeral);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetConfigVendasAlertas()
        {
            ConfiguraçãoVendasAlertas result = DBConfigVendasAlertas.GetByNo(1);

            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateConfigVendasAlertas([FromBody] ConfiguracaoVendasAlertasViewModel data)
        {

            ConfiguraçãoVendasAlertas Compra = new ConfiguraçãoVendasAlertas()
            {
                Id = 1,
                Email1Regiao12 = data.Email1Regiao12,
                Email2Regiao12 = data.Email2Regiao12,
                Email3Regiao12 = data.Email3Regiao12,
                Email1Regiao23 = data.Email1Regiao23,
                Email2Regiao23 = data.Email2Regiao23,
                Email3Regiao23 = data.Email3Regiao23,
                Email1Regiao33 = data.Email1Regiao33,
                Email2Regiao33 = data.Email2Regiao33,
                Email3Regiao33 = data.Email3Regiao33,
                Email1Regiao43 = data.Email1Regiao43,
                Email2Regiao43 = data.Email2Regiao43,
                Email3Regiao43 = data.Email3Regiao43,
                DiasParaEnvioAlerta = data.DiasParaEnvioAlerta,
                UtilizadorCriacao = data.UtilizadorCriacao,
                DataHoraCriacao = data.DataHoraCriacao,
                UtilizadorModificacao = User.Identity.Name,
                DataHoraModificacao = DateTime.Now
            };

            DBConfigVendasAlertas.Update(Compra);

            return Json(data);
        }
        #endregion

        #region Unidade Medida
        public IActionResult UnidadeMedida(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminNutricao);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetUnidadeMedida()
        {
            List<UnidadeMedidaViewModel> result = DBUnidadeMedida.GetAll().Select(x => new UnidadeMedidaViewModel()
            {
                Code = x.Code,
                Description = x.Description,
                CreateDate = x.DataHoraCriação,
                CreateUser = x.UtilizadorCriação
            }).ToList();
            return Json(result);
        }
        [HttpPost]
        public JsonResult DeleteUnidadeMedida([FromBody] UnidadeMedidaViewModel data)
        {
            var result = DBUnidadeMedida.Delete(DBUnidadeMedida.ParseToDatabase(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateUnidadeMedida([FromBody] List<UnidadeMedidaViewModel> data)
        {

            data.ForEach(x =>
            {
                UnidadeMedida UnidadeMedida = new UnidadeMedida()
                {
                    Code = x.Code,
                    Description = x.Description,
                };

                if (DBUnidadeMedida.GetById(x.Code) != null)
                {
                    UnidadeMedida.UtilizadorCriação = x.CreateUser;
                    UnidadeMedida.DataHoraCriação = x.CreateDate.HasValue ? x.CreateDate : (DateTime?)null;
                    UnidadeMedida.DataHoraModificação = DateTime.Now;
                    UnidadeMedida.UtilizadorModificação = User.Identity.Name;
                    DBUnidadeMedida.Update(UnidadeMedida);
                }
                else
                {
                    UnidadeMedida.DataHoraCriação = DateTime.Now;
                    UnidadeMedida.UtilizadorCriação = User.Identity.Name;
                    DBUnidadeMedida.Create(UnidadeMedida);
                }
            });
            return Json(data);
        }
        #endregion

        #region Unidade Medida Produto
        public IActionResult UnidadeMedidaProduto(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminNutricao);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetUnidadeMedidaProduto()
        {
            List<UnitMeasureProductViewModel> result = DBUnitMeasureProduct.GetAll().Select(x => new UnitMeasureProductViewModel()
            {
                ProductNo = x.NºProduto,
                Code = x.Código,
                QtdUnitMeasure = x.QtdPorUnidadeMedida,
                Length = x.Comprimento,
                Width = x.Largura,
                Heigth = x.Altura,
                Cubage = x.Cubagem,
                Weight = x.Peso,
                CreateDate = x.DataHoraCriação,
                CreateUser = x.UtilizadorCriação,
                UpdateDate = x.DataHoraModificação,
                UpdateUser = x.UtilizadorModificação
            }).ToList();
            return Json(result);
        }
        [HttpPost]
        public JsonResult DeleteUnidadeMedidaProduto([FromBody] UnitMeasureProductViewModel data)
        {
            var result = DBUnitMeasureProduct.Delete(DBUnitMeasureProduct.ParseToDb(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateUnidadeMedidaProduto([FromBody] List<UnitMeasureProductViewModel> data)
        {

            data.ForEach(x =>
            {
                UnidadeMedidaProduto UnidadeMedidaProduto = new UnidadeMedidaProduto()
                {
                    NºProduto = x.ProductNo,
                    Código = x.Code,
                    QtdPorUnidadeMedida = x.QtdUnitMeasure,
                    Comprimento = x.Length,
                    Largura = x.Width,
                    Altura = x.Heigth,
                    Cubagem = x.Cubage,
                    Peso = x.Weight,
                    DataHoraCriação = x.CreateDate,
                    UtilizadorCriação = x.CreateUser,
                    DataHoraModificação = x.UpdateDate,
                    UtilizadorModificação = x.UpdateUser
                };

                if (DBUnitMeasureProduct.GetByProdutoCode(x.ProductNo, x.Code) != null)
                {
                    UnidadeMedidaProduto.UtilizadorCriação = x.CreateUser;
                    UnidadeMedidaProduto.DataHoraCriação = x.CreateDate.HasValue ? x.CreateDate : (DateTime?)null;
                    UnidadeMedidaProduto.DataHoraModificação = DateTime.Now;
                    UnidadeMedidaProduto.UtilizadorModificação = User.Identity.Name;
                    DBUnitMeasureProduct.Update(UnidadeMedidaProduto);
                }
                else
                {
                    UnidadeMedidaProduto.DataHoraCriação = DateTime.Now;
                    UnidadeMedidaProduto.UtilizadorCriação = User.Identity.Name;
                    DBUnitMeasureProduct.Create(UnidadeMedidaProduto);
                }
            });
            return Json(data);
        }
        #endregion

        #region Acordo de Preços

        public IActionResult AcordoPrecos_List()
        {
            //UserAccessesViewModel UPerm = GetPermissions("Administracao");
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminAcordosPrecos);

            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !userPerm.Create.Value;
                ViewBag.UpdatePermissions = !userPerm.Update.Value;
                ViewBag.DeletePermissions = !userPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult AcordoPrecos(string id)
        {
            ViewBag.NoProcedimento = id;

            //UserAccessesViewModel UPerm = GetPermissions("Administracao");
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminAcordosPrecos);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !userPerm.Create.Value;
                ViewBag.UpdatePermissions = !userPerm.Update.Value;
                ViewBag.DeletePermissions = !userPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult LinhasAcordosPrecos()
        {
            //UserAccessesViewModel UPerm= GetPermissions("Administracao");
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.LinhasAcordosPrecos);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.UPermissions = userPerm;
                ViewBag.reportServerURL = _config.ReportServerURL;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetAcordoPrecosConfigData([FromBody] AcordoPrecosModelView data)
        {
            AcordoPrecos AP = DBAcordoPrecos.GetById(data.NoProcedimento);

            AcordoPrecosModelView result = new AcordoPrecosModelView();

            if (AP != null)
            {
                result.NoProcedimento = AP.NoProcedimento;
                result.DtInicio = AP.DtInicio;
                result.DtInicioTexto = AP.DtInicio == null ? "" : AP.DtInicio.Value.ToString("yyyy-MM-dd");
                result.DtFim = AP.DtFim;
                result.DtFimTexto = AP.DtFim == null ? "" : AP.DtFim.Value.ToString("yyyy-MM-dd");
                result.ValorTotal = AP.ValorTotal;

                result.FornecedoresAcordoPrecos = DBFornecedoresAcordoPrecos.GetAllByNoProdimento(data.NoProcedimento).Select(x => new FornecedoresAcordoPrecosViewModel()
                {
                    NoProcedimento = x.NoProcedimento,
                    NoFornecedor = x.NoFornecedor,
                    NomeFornecedor = x.NomeFornecedor,
                    Valor = x.Valor,
                    ValorConsumido = x.ValorConsumido
                }).ToList();

                result.LinhasAcordoPrecos = DBLinhasAcordoPrecos.GetAllByNoProcedimento(data.NoProcedimento).Select(x => new LinhasAcordoPrecosViewModel()
                {
                    NoProcedimento = x.NoProcedimento,
                    NoFornecedor = x.NoFornecedor,
                    NomeFornecedor = x.NomeFornecedor,
                    CodProduto = x.CodProduto,
                    DescricaoProduto = x.DescricaoProduto,
                    DtValidadeInicio = x.DtValidadeInicio,
                    DtValidadeInicioTexto = x.DtValidadeInicio == null ? "" : Convert.ToDateTime(x.DtValidadeInicio).ToShortDateString(),
                    DtValidadeFim = x.DtValidadeFim,
                    DtValidadeFimTexto = x.DtValidadeFim == null ? "" : Convert.ToDateTime(x.DtValidadeFim).ToString("yyyy-MM-dd"),
                    Regiao = x.Regiao,
                    //RegiaoNome = x.Regiao == null ? "" : x.Regiao.ToString() + " - " + DBNAV2017DimensionValues.GetByDimType(_config.NAVDatabaseName, _config.NAVCompanyName, 1).Where(y => y.Code == x.Regiao).SingleOrDefault()?.Name,
                    Area = x.Area,
                    //AreaNome = x.Area == null ? "" : x.Area.ToString() + " - " + DBNAV2017DimensionValues.GetByDimType(_config.NAVDatabaseName, _config.NAVCompanyName, 2).Where(y => y.Code == x.Area).SingleOrDefault()?.Name,
                    Cresp = x.Cresp,
                    //CrespNome = x.Cresp == null ? "" : x.Cresp.ToString() + " - " + DBNAV2017DimensionValues.GetByDimType(_config.NAVDatabaseName, _config.NAVCompanyName, 3).Where(y => y.Code == x.Cresp).SingleOrDefault()?.Name,
                    Localizacao = x.Localizacao,
                    //LocalizacaoNome = x.Localizacao == null ? "" : x.Localizacao.ToString() + " - " + DBNAV2017Locations.GetAllLocations(_config.NAVDatabaseName, _config.NAVCompanyName).Where(y => y.Code == x.Localizacao).SingleOrDefault()?.Name,
                    CustoUnitario = x.CustoUnitario,
                    Um = x.Um,
                    QtdPorUm = x.QtdPorUm,
                    PesoUnitario = x.PesoUnitario,
                    CodProdutoFornecedor = x.CodProdutoFornecedor,
                    DescricaoProdFornecedor = x.DescricaoProdFornecedor,
                    FormaEntrega = x.FormaEntrega,
                    FormaEntregaTexto = x.FormaEntrega == null ? "" : EnumerablesFixed.AP_FormaEntrega.Where(y => y.Id == x.FormaEntrega).SingleOrDefault()?.Value,
                    TipoPreco = x.TipoPreco,
                    TipoPrecoTexto = x.TipoPreco == null ? "" : EnumerablesFixed.AP_TipoPreco.Where(y => y.Id == x.TipoPreco).SingleOrDefault()?.Value,
                    GrupoRegistoIvaProduto = x.GrupoRegistoIvaProduto,
                    CodCategoriaProduto = x.CodCategoriaProduto,

                    UserId = x.UserId,
                    DataCriacao = x.DataCriacao,
                    DataCriacaoTexto = x.DataCriacao == null ? "" : Convert.ToDateTime(x.DataCriacao).ToShortDateString(),
                    
                }).ToList();

                //ORIGEM = 1 » Acordo Preços
                //TIPO = 2 » ERRO
                //result.AnexosErros = DBAnexosErros.GetByOrigemAndCodigo(1, data.NoProcedimento).Select(x => new AnexosErrosViewModel()
                //{
                //    ID = x.Id,
                //    CodeTexto = x.Id.ToString(),
                //    Origem = (int)x.Origem,
                //    OrigemTexto = x.Origem == 0 ? "" : EnumerablesFixed.AE_Origem.Where(y => y.Id == x.Origem).SingleOrDefault().Value,
                //    Tipo = (int)x.Tipo,
                //    TipoTexto = x.Tipo == 0 ? "" : EnumerablesFixed.AE_Tipo.Where(y => y.Id == x.Tipo).SingleOrDefault().Value,
                //    Codigo = x.Codigo,
                //    NomeAnexo = x.NomeAnexo,
                //    Anexo = x.Anexo,
                //    CriadoPor = x.CriadoPor,
                //    CriadoPorNome = x.CriadoPor == null ? "" : DBUserConfigurations.GetById(x.CriadoPor).Nome,
                //    DataHora_Criacao = x.DataHoraCriacao,
                //    DataHora_CriacaoTexto = x.DataHoraCriacao == null ? "" : x.DataHoraCriacao.Value.ToString("yyyy-MM-dd"),
                //    AlteradoPor = x.AlteradoPor,
                //    AlteradoPorNome = x.AlteradoPor == null ? "" : DBUserConfigurations.GetById(x.AlteradoPor).Nome,
                //    DataHora_Alteracao = x.DataHoraAlteracao,
                //    DataHora_AlteracaoTexto = x.DataHoraAlteracao == null ? "" : x.DataHoraAlteracao.Value.ToString("yyyy-MM-dd")
                //}).ToList();
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetListAcordoPrecos()
        {
            List<AcordoPrecosModelView> result = DBAcordoPrecos.GetAll();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetAllLinhasAcordosPrecos()
        {
            List<LinhasAcordoPrecosViewModel> result = DBLinhasAcordoPrecos.GetAllByDimensionsUser(User.Identity.Name).Select(x => new LinhasAcordoPrecosViewModel()
            {
                NoProcedimento = x.NoProcedimento,
                NoFornecedor = x.NoFornecedor,
                CodProduto = x.CodProduto,
                DtValidadeInicio = x.DtValidadeInicio,
                DtValidadeInicioTexto = x.DtValidadeInicio == null ? "" : Convert.ToDateTime(x.DtValidadeInicio).ToShortDateString(),
                DtValidadeFim = x.DtValidadeFim,
                DtValidadeFimTexto = x.DtValidadeFim == null ? "" : Convert.ToDateTime(x.DtValidadeFim).ToShortDateString(),
                Cresp = x.Cresp,
                //CrespNome = x.Cresp == null ? "" : x.Cresp.ToString() + " - " + DBNAV2017DimensionValues.GetByDimType(_config.NAVDatabaseName, _config.NAVCompanyName, 3).Where(y => y.Code == x.Cresp).SingleOrDefault()?.Name,
                Area = x.Area,
                //AreaNome = x.Area == null ? "" : x.Area.ToString() + " - " + DBNAV2017DimensionValues.GetByDimType(_config.NAVDatabaseName, _config.NAVCompanyName, 2).Where(y => y.Code == x.Area).SingleOrDefault()?.Name,
                Regiao = x.Regiao,
                //RegiaoNome = x.Regiao == null ? "" : x.Regiao.ToString() + " - " + DBNAV2017DimensionValues.GetByDimType(_config.NAVDatabaseName, _config.NAVCompanyName, 1).Where(y => y.Code == x.Regiao).SingleOrDefault()?.Name,
                Localizacao = x.Localizacao,
                //LocalizacaoNome = x.Localizacao == null ? "" : x.Localizacao.ToString() + " - " + DBNAV2017Locations.GetAllLocations(_config.NAVDatabaseName, _config.NAVCompanyName).Where(y => y.Code == x.Localizacao).SingleOrDefault()?.Name,
                CustoUnitario = x.CustoUnitario,
                //NomeFornecedor = x.NoFornecedor == null ? "" : x.NoFornecedor.ToString() + " - " + DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Where(y => y.No_ == x.NoFornecedor).SingleOrDefault()?.Name,
                NomeFornecedor = x.NomeFornecedor,
                DescricaoProduto = x.DescricaoProduto,
                Um = x.Um,
                QtdPorUm = x.QtdPorUm,
                QtdPorUmTexto = x.QtdPorUm == null ? "" : x.QtdPorUm.ToString(),
                PesoUnitario = x.PesoUnitario,
                PesoUnitarioTexto = x.PesoUnitario == null ? "" : x.PesoUnitario.ToString(),
                CodProdutoFornecedor = x.CodProdutoFornecedor,
                DescricaoProdFornecedor = x.DescricaoProdFornecedor,
                FormaEntrega = x.FormaEntrega,
                FormaEntregaTexto = x.FormaEntrega == null ? "" : EnumerablesFixed.AP_FormaEntrega.Where(y => y.Id == x.FormaEntrega).SingleOrDefault()?.Value,
                UserId = x.UserId,
                DataCriacao = x.DataCriacao,
                DataCriacaoTexto = x.DataCriacao == null ? "" : Convert.ToDateTime(x.DataCriacao).ToShortDateString(),
                TipoPreco = x.TipoPreco,
                TipoPrecoTexto = x.TipoPreco == null ? "" : EnumerablesFixed.AP_TipoPreco.Where(y => y.Id == x.TipoPreco).SingleOrDefault()?.Value,
                GrupoRegistoIvaProduto = x.GrupoRegistoIvaProduto,
                CodCategoriaProduto = x.CodCategoriaProduto
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateAcordoPrecos([FromBody] AcordoPrecos data)
        {
            AcordoPrecos toCreate = DBAcordoPrecos.Create(new AcordoPrecos()
            {
                NoProcedimento = data.NoProcedimento,
                DtInicio = data.DtInicio,
                DtFim = data.DtFim,
                ValorTotal = data.ValorTotal
            });

            if (toCreate != null)
                return Json(0);
            else
                return Json(1);
        }

        [HttpPost]
        //Atualiza uma Presença
        public JsonResult UpdateLinhaAcordoPreco([FromBody] LinhasAcordoPrecos data)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                LinhasAcordoPrecos toUpdate = DBLinhasAcordoPrecos.GetById(data.NoProcedimento, data.NoFornecedor, data.CodProduto, data.DtValidadeInicio, data.Cresp, data.Localizacao);
                if (toUpdate != null)
                {
                    toUpdate.DtValidadeFim = data.DtValidadeFim;

                    if (DBLinhasAcordoPrecos.Update(toUpdate) != null)
                    {
                        result.eReasonCode = 1;
                        result.eMessage = "Linha Acordo de Preço atualizada com sucesso.";
                    }
                    else
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Ocorreu um erro ao atualizar a linha Acordo de Preço.";
                    }
                }
                else
                {
                    result.eReasonCode = 3;
                    result.eMessage = "Ocorreu um erro ao obter a linha Acordo de Preço.";
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteAcordoPreco([FromBody] AcordoPrecosModelView data)
        {
            int result = 0;
            bool dbDeleteLinhaResult = false;
            bool dbDeleteFornecedorResult = false;
            bool dbDeleteAcordoPrecoResult = false;

            try
            {
                dbDeleteLinhaResult = DBLinhasAcordoPrecos.DeleteByProcedimento(data.NoProcedimento);

                dbDeleteFornecedorResult = DBFornecedoresAcordoPrecos.DeleteByProcedimento(data.NoProcedimento);

                dbDeleteAcordoPrecoResult = DBAcordoPrecos.Delete(data.NoProcedimento);

                if (!dbDeleteAcordoPrecoResult)
                    result = 1;
            }
            catch (Exception ex)
            {
                result = 99;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteLinha([FromBody] LinhasAcordoPrecosViewModel data)
        {
            int result = 0;
            bool dbDeleteLinhaResult = false;

            try
            {
                dbDeleteLinhaResult = DBLinhasAcordoPrecos.Delete(data.NoProcedimento, data.NoFornecedor, data.CodProduto, data.DtValidadeInicio, data.Cresp, data.Localizacao);

                if (!dbDeleteLinhaResult)
                    result = 1;
            }
            catch (Exception ex)
            {
                result = 99;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteFornecedor([FromBody] FornecedoresAcordoPrecosViewModel data)
        {
            int result = 0;
            bool dbDeleteFornecedorResult = false;

            try
            {
                dbDeleteFornecedorResult = DBFornecedoresAcordoPrecos.Delete(data.NoProcedimento, data.NoFornecedor);

                if (!dbDeleteFornecedorResult)
                    result = 1;
            }
            catch (Exception ex)
            {
                result = 99;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateLinhaAcordoPrecos([FromBody] LinhasAcordoPrecos data)
        {
            LinhasAcordoPrecos toCreate = DBLinhasAcordoPrecos.Create(new LinhasAcordoPrecos()
            {
                NoProcedimento = data.NoProcedimento,
                NoFornecedor = data.NoFornecedor,
                NomeFornecedor = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Where(x => x.No_ == data.NoFornecedor).SingleOrDefault().Name,
                CodProduto = data.CodProduto,
                DescricaoProduto = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, data.CodProduto).SingleOrDefault().Name,
                DtValidadeInicio = data.DtValidadeInicio,
                DtValidadeFim = data.DtValidadeFim,
                Regiao = data.Regiao,
                Area = data.Area,
                Cresp = data.Cresp,
                Localizacao = data.Localizacao,
                CustoUnitario = data.CustoUnitario,
                Um = data.Um,
                QtdPorUm = data.QtdPorUm,
                PesoUnitario = data.PesoUnitario,
                CodProdutoFornecedor = data.CodProdutoFornecedor,
                DescricaoProdFornecedor = data.DescricaoProdFornecedor,
                FormaEntrega = data.FormaEntrega,
                TipoPreco = data.TipoPreco,
                GrupoRegistoIvaProduto = data.GrupoRegistoIvaProduto,
                CodCategoriaProduto = data.CodCategoriaProduto,

                UserId = User.Identity.Name,
                DataCriacao = DateTime.Now,
            });

            if (toCreate != null)
                return Json(0);
            else
                return Json(1);
        }

        [HttpPost]
        public JsonResult CreateFornecedorAcordoPrecos([FromBody] FornecedoresAcordoPrecos data)
        {
            FornecedoresAcordoPrecos toCreate = DBFornecedoresAcordoPrecos.Create(new FornecedoresAcordoPrecos()
            {
                NoProcedimento = data.NoProcedimento,
                NoFornecedor = data.NoFornecedor,
                NomeFornecedor = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Where(x => x.No_ == data.NoFornecedor).SingleOrDefault().Name,
                Valor = data.Valor,
                ValorConsumido = data.ValorConsumido
            });

            if (toCreate != null)
                return Json(0);
            else
                return Json(1);
        }

        [HttpPost]
        public JsonResult VerificarNoProcedimento([FromBody] AcordoPrecos data)
        {
            AcordoPrecos AcordoPrecos = DBAcordoPrecos.GetById(data.NoProcedimento);

            if (AcordoPrecos == null)
                return Json(0);
            else
                return Json(1);
        }

        [HttpPost]
        [Route("Administracao/FileUpload")]
        [Route("Administracao/FileUpload/{FormularioNoProcedimento}")]
        public JsonResult FileUpload(string FormularioNoProcedimento)
        {
            //TESTE COM DLL EPPlus
            var files = Request.Form.Files;
            bool global_result = true;
            foreach (var file in files)
            {
                try
                {
                    string name = Path.GetFileNameWithoutExtension(file.FileName);
                    string filename = Path.GetFileName(file.FileName);
                    var full_path = Path.Combine(_generalConfig.FileUploadFolder + "Administracao\\", User.Identity.Name + "_" + filename);
                    if (System.IO.File.Exists(full_path))
                        System.IO.File.Delete(full_path);
                    FileStream dd = new FileStream(full_path, FileMode.CreateNew);
                    file.CopyTo(dd);
                    dd.Dispose();
                    var existingFile = new FileInfo(full_path);

                    string filename_result = name + "_Resultado.xlsx";
                    var full_path_result = Path.Combine(_generalConfig.FileUploadFolder + "Administracao\\", User.Identity.Name + "_" + filename_result);
                    if (System.IO.File.Exists(full_path_result))
                        System.IO.File.Delete(full_path_result);
                    var existingFile_result = new FileInfo(full_path_result);

                    using (var excel = new ExcelPackage(existingFile))
                    {
                        var excel_result = new ExcelPackage(existingFile_result);
                        ExcelWorkbook workBook_result = excel_result.Workbook;

                        ExcelWorkbook workBook = excel.Workbook;
                        if (workBook != null)
                        {
                            if (workBook.Worksheets.Count > 0)
                            {
                                workBook_result = Criar_Excel_Worksheet(workBook_result, "ORIGINAL");
                                workBook_result = Criar_Excel_Worksheet(workBook_result, "SUCESSO");
                                workBook_result = Criar_Excel_Worksheet(workBook_result, "ERRO");

                                ExcelWorksheet currentWorksheet = workBook.Worksheets[0];
                                ExcelWorksheet currentWorksheet_ORIGINAL = workBook_result.Worksheets["ORIGINAL"];
                                ExcelWorksheet currentWorksheet_SUCESSO = workBook_result.Worksheets["SUCESSO"];
                                ExcelWorksheet currentWorksheet_ERRO = workBook_result.Worksheets["ERRO"];

                                if ((currentWorksheet.Dimension.End.Row > 1) &&
                                    (currentWorksheet.Cells[1, 1].Value.ToString() == "NoProcedimento") &&
                                    (currentWorksheet.Cells[1, 2].Value.ToString() == "NoFornecedor") &&
                                    (currentWorksheet.Cells[1, 3].Value.ToString() == "CodProduto") &&
                                    (currentWorksheet.Cells[1, 4].Value.ToString() == "DtValidadeInicio") &&
                                    (currentWorksheet.Cells[1, 5].Value.ToString() == "DtValidadeFim") &&
                                    (currentWorksheet.Cells[1, 6].Value.ToString() == "Regiao") &&
                                    (currentWorksheet.Cells[1, 7].Value.ToString() == "Area") &&
                                    (currentWorksheet.Cells[1, 8].Value.ToString() == "Cresp") &&
                                    (currentWorksheet.Cells[1, 9].Value.ToString() == "Localizacao") &&
                                    (currentWorksheet.Cells[1, 10].Value.ToString() == "CustoUnitario") &&
                                    (currentWorksheet.Cells[1, 11].Value.ToString() == "UM") &&
                                    (currentWorksheet.Cells[1, 12].Value.ToString() == "QtdPorUM") &&
                                    (currentWorksheet.Cells[1, 13].Value.ToString() == "PesoUnitario") &&
                                    (currentWorksheet.Cells[1, 14].Value.ToString() == "CodProdutoFornecedor") &&
                                    (currentWorksheet.Cells[1, 15].Value.ToString() == "FormaEntrega") &&
                                    (currentWorksheet.Cells[1, 16].Value.ToString() == "TipoPreco"))
                                {
                                    List<AcordoPrecosModelView> Lista_AcordoPrecos = DBAcordoPrecos.GetAll();
                                    List<NAVVendorViewModel> Lista_Vendor = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName);
                                    List<NAVProductsViewModel> Lista_Products = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, "");
                                    List<NAVDimValueViewModel> Lista_Regioes = DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name);
                                    List<NAVDimValueViewModel> Lista_Areas = DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 2, User.Identity.Name);
                                    List<NAVDimValueViewModel> Lista_Cresp = DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 3, User.Identity.Name);
                                    List<AcessosLocalizacoes> Lista_AcessosLocalizacoes = DBAcessosLocalizacoes.GetByUserId(User.Identity.Name);
                                    List<EnumData> Lista_FormaEntrega = EnumerablesFixed.AP_FormaEntrega;
                                    List<EnumData> Lista_TipoPreco = EnumerablesFixed.AP_TipoPreco;
                                    int Linha_ORIGINAL = 2;
                                    int Linha_SUCESSO = 2;
                                    int Linha_ERRO = 2;
                                    var result_list = new List<bool>();
                                    for (int i = 1; i <= 16; i++)
                                    {
                                        result_list.Add(false);
                                    }

                                    string NoProcedimento = "";
                                    string NoFornecedor = "";
                                    string CodProduto = "";
                                    string DtValidadeInicio = "";
                                    string DtValidadeFim = "";
                                    string Regiao = "";
                                    string Area = "";
                                    string Cresp = "";
                                    string Localizacao = "";
                                    string CustoUnitario = "";
                                    string UM = "";
                                    string QtdPorUM = "";
                                    string PesoUnitario = "";
                                    string CodProdutoFornecedor = "";
                                    string FormaEntrega = "";
                                    string TipoPreco = "";

                                    //VALIDAÇÃO DE TODOS OS CAMPOS
                                    for (int rowNumber = 2; rowNumber <= currentWorksheet.Dimension.End.Row; rowNumber++)
                                    {
                                        NoProcedimento = currentWorksheet.Cells[rowNumber, 1].Value == null ? "" : currentWorksheet.Cells[rowNumber, 1].Value.ToString();
                                        NoFornecedor = currentWorksheet.Cells[rowNumber, 2].Value == null ? "" : currentWorksheet.Cells[rowNumber, 2].Value.ToString();
                                        CodProduto = currentWorksheet.Cells[rowNumber, 3].Value == null ? "" : currentWorksheet.Cells[rowNumber, 3].Value.ToString();
                                        DtValidadeInicio = currentWorksheet.Cells[rowNumber, 4].Value == null ? "" : currentWorksheet.Cells[rowNumber, 4].Value.ToString();
                                        DtValidadeFim = currentWorksheet.Cells[rowNumber, 5].Value == null ? "" : currentWorksheet.Cells[rowNumber, 5].Value.ToString();
                                        Regiao = currentWorksheet.Cells[rowNumber, 6].Value == null ? "" : currentWorksheet.Cells[rowNumber, 6].Value.ToString();
                                        Area = currentWorksheet.Cells[rowNumber, 7].Value == null ? "" : currentWorksheet.Cells[rowNumber, 7].Value.ToString();
                                        Cresp = currentWorksheet.Cells[rowNumber, 8].Value == null ? "" : currentWorksheet.Cells[rowNumber, 8].Value.ToString();
                                        Localizacao = currentWorksheet.Cells[rowNumber, 9].Value == null ? "" : currentWorksheet.Cells[rowNumber, 9].Value.ToString();
                                        CustoUnitario = currentWorksheet.Cells[rowNumber, 10].Value == null ? "" : currentWorksheet.Cells[rowNumber, 10].Value.ToString();
                                        UM = currentWorksheet.Cells[rowNumber, 11].Value == null ? "" : currentWorksheet.Cells[rowNumber, 11].Value.ToString();
                                        QtdPorUM = currentWorksheet.Cells[rowNumber, 12].Value == null ? "" : currentWorksheet.Cells[rowNumber, 12].Value.ToString();
                                        PesoUnitario = currentWorksheet.Cells[rowNumber, 13].Value == null ? "" : currentWorksheet.Cells[rowNumber, 13].Value.ToString();
                                        CodProdutoFornecedor = currentWorksheet.Cells[rowNumber, 14].Value == null ? "" : currentWorksheet.Cells[rowNumber, 14].Value.ToString();
                                        FormaEntrega = currentWorksheet.Cells[rowNumber, 15].Value == null ? "" : currentWorksheet.Cells[rowNumber, 15].Value.ToString();
                                        TipoPreco = currentWorksheet.Cells[rowNumber, 16].Value == null ? "" : currentWorksheet.Cells[rowNumber, 16].Value.ToString();

                                        result_list = Validar_LinhaExcel(FormularioNoProcedimento, NoProcedimento, NoFornecedor, CodProduto, DtValidadeInicio, DtValidadeFim, Regiao, Area, Cresp,
                                            Localizacao, CustoUnitario, QtdPorUM, PesoUnitario, FormaEntrega, TipoPreco, result_list, Lista_AcordoPrecos, Lista_Vendor, Lista_Products,
                                            Lista_Regioes, Lista_Areas, Lista_Cresp, Lista_AcessosLocalizacoes, Lista_FormaEntrega, Lista_TipoPreco);

                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 1].Value = NoProcedimento;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 2].Value = NoFornecedor;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 3].Value = CodProduto;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 4].Value = DtValidadeInicio;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 5].Value = DtValidadeFim;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 6].Value = Regiao;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 7].Value = Area;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 8].Value = Cresp;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 9].Value = Localizacao;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 10].Value = CustoUnitario;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 11].Value = UM;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 12].Value = QtdPorUM;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 13].Value = PesoUnitario;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 14].Value = CodProdutoFornecedor;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 15].Value = FormaEntrega;
                                        currentWorksheet_ORIGINAL.Cells[Linha_ORIGINAL, 16].Value = TipoPreco;

                                        Linha_ORIGINAL = Linha_ORIGINAL + 1;

                                        if (result_list.All(c => c == false))
                                        {
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 1].Value = NoProcedimento;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 2].Value = NoFornecedor;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 3].Value = CodProduto;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 4].Value = DtValidadeInicio;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 5].Value = DtValidadeFim;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 6].Value = Regiao;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 7].Value = Area;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 8].Value = Cresp;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 9].Value = Localizacao;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 10].Value = CustoUnitario;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 11].Value = UM;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 12].Value = QtdPorUM;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 13].Value = PesoUnitario;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 14].Value = CodProdutoFornecedor;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 15].Value = FormaEntrega;
                                            currentWorksheet_SUCESSO.Cells[Linha_SUCESSO, 16].Value = TipoPreco;

                                            if (result_list[15] == true)
                                            {
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 1].Style.Font.Color.SetColor(Color.Orange);
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 2].Style.Font.Color.SetColor(Color.Orange);
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 3].Style.Font.Color.SetColor(Color.Orange);
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 4].Style.Font.Color.SetColor(Color.Orange);
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 8].Style.Font.Color.SetColor(Color.Orange);
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 9].Style.Font.Color.SetColor(Color.Orange);
                                            }

                                            Linha_SUCESSO = Linha_SUCESSO + 1;
                                        }
                                        else
                                        {
                                            global_result = false;

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 1].Value = NoProcedimento;
                                            if (result_list[1] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 1].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 2].Value = NoFornecedor;
                                            if (result_list[2] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 2].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 3].Value = CodProduto;
                                            if (result_list[3] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 3].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 4].Value = DtValidadeInicio;
                                            if (result_list[4] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 4].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 5].Value = DtValidadeFim;
                                            if (result_list[5] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 5].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 6].Value = Regiao;
                                            if (result_list[6] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 6].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 7].Value = Area;
                                            if (result_list[7] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 7].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 8].Value = Cresp;
                                            if (result_list[8] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 8].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 9].Value = Localizacao;
                                            if (result_list[9] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 9].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 10].Value = CustoUnitario;
                                            if (result_list[10] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 10].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 11].Value = UM;

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 12].Value = QtdPorUM;
                                            if (result_list[11] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 12].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 13].Value = PesoUnitario;
                                            if (result_list[12] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 13].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 14].Value = CodProdutoFornecedor;

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 15].Value = FormaEntrega;
                                            if (result_list[13] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 15].Style.Font.Color.SetColor(Color.Red);

                                            currentWorksheet_ERRO.Cells[Linha_ERRO, 16].Value = TipoPreco;
                                            if (result_list[14] == true)
                                                currentWorksheet_ERRO.Cells[Linha_ERRO, 16].Style.Font.Color.SetColor(Color.Red);

                                            Linha_ERRO = Linha_ERRO + 1;
                                        }

                                        if (result_list.All(c => c == false))
                                        {
                                            LinhasAcordoPrecos toCreate = DBLinhasAcordoPrecos.Create(new LinhasAcordoPrecos()
                                            {
                                                NoProcedimento = NoProcedimento,
                                                NoFornecedor = NoFornecedor,
                                                CodProduto = CodProduto,
                                                DtValidadeInicio = Convert.ToDateTime(DtValidadeInicio),
                                                DtValidadeFim = DtValidadeFim == "" ? (DateTime?)null : Convert.ToDateTime(DtValidadeFim),
                                                Cresp = Cresp,
                                                Area = Area == "" ? (string)null : Area,
                                                Regiao = Regiao == "" ? (string)null : Regiao,
                                                Localizacao = Localizacao,
                                                CustoUnitario = CustoUnitario == "" ? (decimal?)null : Convert.ToDecimal(CustoUnitario),
                                                NomeFornecedor = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Where(x => x.No_ == NoFornecedor).SingleOrDefault().Name,
                                                DescricaoProduto = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, CodProduto).SingleOrDefault().Name,
                                                Um = UM == "" ? (string)null : UM,
                                                QtdPorUm = QtdPorUM == "" ? (decimal?)null : Convert.ToDecimal(QtdPorUM),
                                                PesoUnitario = PesoUnitario == "" ? (decimal?)null : Convert.ToDecimal(PesoUnitario),
                                                CodProdutoFornecedor = CodProdutoFornecedor == "" ? (string)null : CodProdutoFornecedor,
                                                DescricaoProdFornecedor = (string)null,
                                                FormaEntrega = FormaEntrega == "" ? (int?)null : Convert.ToInt32(FormaEntrega),
                                                UserId = User.Identity.Name,
                                                DataCriacao = DateTime.Now,
                                                TipoPreco = TipoPreco == "" ? (int?)null : Convert.ToInt32(TipoPreco)
                                            });
                                        }

                                        if (result_list[1] == false && result_list[2] == false && result_list[3] == false && result_list[4] == false && result_list[5] == false &&
                                            result_list[6] == false && result_list[7] == false && result_list[8] == false && result_list[9] == false && result_list[10] == false &&
                                            result_list[11] == false && result_list[12] == false && result_list[13] == false && result_list[14] == false && result_list[15] == true)
                                        {
                                            LinhasAcordoPrecos toUpdate = DBLinhasAcordoPrecos.Update(new LinhasAcordoPrecos()
                                            {

                                                NoProcedimento = NoProcedimento,
                                                NoFornecedor = NoFornecedor,
                                                CodProduto = CodProduto,
                                                DtValidadeInicio = Convert.ToDateTime(DtValidadeInicio),
                                                DtValidadeFim = DtValidadeFim == "" ? (DateTime?)null : Convert.ToDateTime(DtValidadeFim),
                                                Cresp = Cresp,
                                                Area = Area == "" ? (string)null : Area,
                                                Regiao = Regiao == "" ? (string)null : Regiao,
                                                Localizacao = Localizacao,
                                                CustoUnitario = CustoUnitario == "" ? (decimal?)null : Convert.ToDecimal(CustoUnitario),
                                                NomeFornecedor = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Where(x => x.No_ == NoFornecedor).SingleOrDefault().Name,
                                                DescricaoProduto = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, CodProduto).SingleOrDefault().Name,
                                                Um = UM == "" ? (string)null : UM,
                                                QtdPorUm = QtdPorUM == "" ? (decimal?)null : Convert.ToDecimal(QtdPorUM),
                                                PesoUnitario = PesoUnitario == "" ? (decimal?)null : Convert.ToDecimal(PesoUnitario),
                                                CodProdutoFornecedor = CodProdutoFornecedor == "" ? (string)null : CodProdutoFornecedor,
                                                DescricaoProdFornecedor = (string)null,
                                                FormaEntrega = FormaEntrega == "" ? (int?)null : Convert.ToInt32(FormaEntrega),
                                                UserId = User.Identity.Name,
                                                DataCriacao = DBLinhasAcordoPrecos.GetAll().Where(x => x.NoProcedimento == NoProcedimento && x.NoFornecedor == NoFornecedor && x.CodProduto == CodProduto &&
                                                    x.DtValidadeInicio == Convert.ToDateTime(DtValidadeInicio) && x.Cresp == Cresp && x.Localizacao == Localizacao).FirstOrDefault().DataCriacao,
                                                TipoPreco = TipoPreco == "" ? (int?)null : Convert.ToInt32(TipoPreco)
                                            });
                                        }
                                    }

                                    excel_result.Save();

                                    byte[] Anexo_Result = System.IO.File.ReadAllBytes(full_path_result);

                                    AnexosErros newAnexo = new AnexosErros();
                                    newAnexo.Origem = 1; //ACORDO DE PREÇOS
                                    if (global_result)
                                        newAnexo.Tipo = 1; //SUCESSO
                                    else
                                        newAnexo.Tipo = 2; //INSUCESSO
                                    newAnexo.Codigo = FormularioNoProcedimento;
                                    newAnexo.NomeAnexo = filename_result;
                                    newAnexo.Anexo = Anexo_Result;
                                    newAnexo.CriadoPor = User.Identity.Name;
                                    newAnexo.DataHoraCriacao = DateTime.Now;
                                    DBAnexosErros.Create(newAnexo);

                                    excel.Dispose();
                                    excel_result.Dispose();

                                    System.IO.File.Delete(full_path_result);
                                    System.IO.File.Delete(full_path);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return Json("");
        }

        public List<bool> Validar_LinhaExcel(string FormularioNoProcedimento, string NoProcedimento, string NoFornecedor, string CodProduto, string DtValidadeInicio, string DtValidadeFim,
            string Regiao, string Area, string Cresp, string Localizacao, string CustoUnitario, string QtdPorUM, string PesoUnitario, string FormaEntrega, string TipoPreco,
            List<bool> result_list, List<AcordoPrecosModelView> Lista_AcordoPrecos, List<NAVVendorViewModel> Lista_Vendor, List<NAVProductsViewModel> Lista_Products,
            List<NAVDimValueViewModel> Lista_Regioes, List<NAVDimValueViewModel> Lista_Areas, List<NAVDimValueViewModel> Lista_Cresp, List<AcessosLocalizacoes> Lista_AcessosLocalizacoes,
            List<EnumData> Lista_FormaEntrega, List<EnumData> Lista_TipoPreco)
        {
            DateTime currectDate;
            decimal currectDecimal;
            int currectInt;

            for (int i = 1; i <= 15; i++)
            {
                result_list[i] = false;
            }

            if (Lista_AcordoPrecos.Where(x => x.NoProcedimento == NoProcedimento).Count() == 0 || FormularioNoProcedimento != NoProcedimento)
                //if (DBAcordoPrecos.GetAll().Where(x => x.NoProcedimento == NoProcedimento).Count() == 0 || FormularioNoProcedimento != NoProcedimento)
                result_list[1] = true;

            if (Lista_Vendor.Where(x => x.No_ == NoFornecedor).Count() == 0)
                //if (DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Where(x => x.No_ == NoFornecedor).Count() == 0)
                result_list[2] = true;

            if (Lista_Products.Where(x => x.Code == CodProduto).Count() == 0)
                //if (DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, CodProduto).Count() == 0)
                result_list[3] = true;

            if (!DateTime.TryParse(DtValidadeInicio, out currectDate))
                result_list[4] = true;

            if (DtValidadeFim != "")
                if (!DateTime.TryParse(DtValidadeFim, out currectDate))
                    result_list[5] = true;

            if (Regiao != "")
                if (Lista_Regioes.Where(x => x.Code == Regiao).Count() == 0)
                    //if (DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name).Where(x => x.Code == Regiao).Count() == 0)
                    result_list[6] = true;

            if (Area != "")
                if (Lista_Areas.Where(x => x.Code == Area).Count() == 0)
                    //if (DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 2, User.Identity.Name).Where(x => x.Code == Area).Count() == 0)
                    result_list[7] = true;

            if (Lista_Cresp.Where(x => x.Code == Cresp).Count() == 0)
                //if (DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 3, User.Identity.Name).Where(x => x.Code == Cresp).Count() == 0)
                result_list[8] = true;

            if (Lista_AcessosLocalizacoes.Where(x => x.Localizacao == Localizacao).Count() == 0)
                //if (DBAcessosLocalizacoes.GetByUserId(User.Identity.Name).Where(x => x.Localizacao == Localizacao).Count() == 0)
                result_list[9] = true;

            if (CustoUnitario != "")
                if (!decimal.TryParse(CustoUnitario, out currectDecimal))
                    result_list[10] = true;

            if (QtdPorUM != "")
                if (!decimal.TryParse(QtdPorUM, out currectDecimal))
                    result_list[11] = true;

            if (PesoUnitario != "")
                if (!decimal.TryParse(PesoUnitario, out currectDecimal))
                    result_list[12] = true;

            if (FormaEntrega != "")
            {
                if (int.TryParse(FormaEntrega, out currectInt))
                {
                    if (Lista_FormaEntrega.Where(x => x.Id == Convert.ToInt32(FormaEntrega)).Count() == 0)
                        //if (EnumerablesFixed.AP_FormaEntrega.Where(x => x.Id == Convert.ToInt32(FormaEntrega)).Count() == 0)
                        result_list[13] = true;
                }
                else
                    result_list[13] = true;
            }

            if (TipoPreco != "")
            {
                if (int.TryParse(TipoPreco, out currectInt))
                {
                    if (Lista_TipoPreco.Where(x => x.Id == Convert.ToInt32(TipoPreco)).Count() == 0)
                        //if (EnumerablesFixed.AP_TipoPreco.Where(x => x.Id == Convert.ToInt32(TipoPreco)).Count() == 0)
                        result_list[14] = true;
                }
                else
                    result_list[14] = true;
            }

            if (DBLinhasAcordoPrecos.GetAll().Where(x => x.NoProcedimento == NoProcedimento && x.NoFornecedor == NoFornecedor && x.CodProduto == CodProduto &&
                    x.DtValidadeInicio == Convert.ToDateTime(DtValidadeInicio) && x.Cresp == Cresp && x.Localizacao == Localizacao).Count() > 0)
                result_list[15] = true;


            return result_list;
        }

        public ExcelWorkbook Criar_Excel_Worksheet(ExcelWorkbook workBook, string Nome)
        {
            workBook.Worksheets.Add(Nome);
            ExcelWorksheet currentWorksheet = workBook.Worksheets[Nome];

            currentWorksheet.Cells[1, 1].Value = "NoProcedimento";
            currentWorksheet.Cells[1, 2].Value = "NoFornecedor";
            currentWorksheet.Cells[1, 3].Value = "CodProduto";
            currentWorksheet.Cells[1, 4].Value = "DtValidadeInicio";
            currentWorksheet.Cells[1, 5].Value = "DtValidadeFim";
            currentWorksheet.Cells[1, 6].Value = "Regiao";
            currentWorksheet.Cells[1, 7].Value = "Area";
            currentWorksheet.Cells[1, 8].Value = "Cresp";
            currentWorksheet.Cells[1, 9].Value = "Localizacao";
            currentWorksheet.Cells[1, 10].Value = "CustoUnitario";
            currentWorksheet.Cells[1, 11].Value = "UM";
            currentWorksheet.Cells[1, 12].Value = "QtdPorUM";
            currentWorksheet.Cells[1, 13].Value = "PesoUnitario";
            currentWorksheet.Cells[1, 14].Value = "CodProdutoFornecedor";
            currentWorksheet.Cells[1, 15].Value = "FormaEntrega";
            currentWorksheet.Cells[1, 16].Value = "TipoPreco";

            return workBook;
        }

        [HttpGet]
        public FileResult DownloadFileAnexosErros(string iD)
        {
            AnexosErros AnexoErro = DBAnexosErros.GetById(Convert.ToInt32(iD));

            return File(AnexoErro.Anexo, System.Net.Mime.MediaTypeNames.Application.Octet, AnexoErro.NomeAnexo);
        }

        [HttpGet]
        [Route("Administracao/DownloadAcordoPrecosTemplate")]
        [Route("Administracao/DownloadAcordoPrecosTemplate/{FileName}")]
        public FileStreamResult DownloadAcordoPrecosTemplate(string FileName)
        {
            return new FileStreamResult(new FileStream(_generalConfig.FileUploadFolder + "Administracao\\" + FileName, FileMode.Open), "application /xlsx");
        }


        [HttpPost]
        public JsonResult DeleteAnexosErros([FromBody] AnexosErrosViewModel AnexoErro)
        {
            int result = 0;
            bool dbDeleteAnexosErrosResult = false;

            try
            {
                dbDeleteAnexosErrosResult = DBAnexosErros.Delete(Convert.ToInt32(AnexoErro.CodeTexto));

                if (!dbDeleteAnexosErrosResult)
                    result = 1;
            }
            catch (Exception ex)
            {
                result = 99;
            }
            return Json(result);
        }




        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_LinhasAcordosPrecos([FromBody] List<LinhasAcordoPrecosViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Administracao\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Linhas dos Acordos de Preços");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["noProcedimento"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Procedimento"); Col = Col + 1; }
                if (dp["noFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Fornecedor"); Col = Col + 1; }
                if (dp["nomeFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nome Fornecedor"); Col = Col + 1; }
                if (dp["codProduto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Produto"); Col = Col + 1; }
                if (dp["descricaoProduto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descrição Produto"); Col = Col + 1; }
                if (dp["codCategoriaProduto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Categoria Produto"); Col = Col + 1; }
                if (dp["custoUnitarioTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Custo Unitário"); Col = Col + 1; }
                if (dp["um"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Unid. Medida"); Col = Col + 1; }
                if (dp["qtdPorUmTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Qtd. Por Unid. Medida"); Col = Col + 1; }
                if (dp["pesoUnitarioTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Peso Unitário"); Col = Col + 1; }
                if (dp["dtValidadeInicioTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Início Validade"); Col = Col + 1; }
                if (dp["dtValidadeFimTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Fim Validade"); Col = Col + 1; }
                if (dp["formaEntregaTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Forma Entrega"); Col = Col + 1; }
                if (dp["codProdutoFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Produto Fornecedor"); Col = Col + 1; }
                if (dp["descricaoProdFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descr. Produto Fornecedor"); Col = Col + 1; }
                if (dp["localizacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Localização"); Col = Col + 1; }
                if (dp["regiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Região"); Col = Col + 1; }
                if (dp["area"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Área Funcional"); Col = Col + 1; }
                if (dp["cresp"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Centro Resp."); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (LinhasAcordoPrecosViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["noProcedimento"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NoProcedimento); Col = Col + 1; }
                        if (dp["noFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NoFornecedor); Col = Col + 1; }
                        if (dp["nomeFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NomeFornecedor); Col = Col + 1; }
                        if (dp["codProduto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodProduto); Col = Col + 1; }
                        if (dp["descricaoProduto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DescricaoProduto); Col = Col + 1; }
                        if (dp["codCategoriaProduto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodCategoriaProduto == null ? string.Empty : item.CodCategoriaProduto.ToString()); Col = Col + 1; }
                        if (dp["custoUnitarioTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue((double)(item.CustoUnitario != null ? (decimal)item.CustoUnitario : 0)); Col = Col + 1; }
                        if (dp["um"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Um); Col = Col + 1; }
                        if (dp["qtdPorUmTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.QtdPorUmTexto); Col = Col + 1; }
                        if (dp["pesoUnitarioTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.PesoUnitarioTexto); Col = Col + 1; }
                        if (dp["dtValidadeInicioTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DtValidadeInicioTexto); Col = Col + 1; }
                        if (dp["dtValidadeFimTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DtValidadeFimTexto); Col = Col + 1; }
                        if (dp["formaEntregaTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.FormaEntregaTexto.ToString()); Col = Col + 1; }
                        if (dp["codProdutoFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodProdutoFornecedor.ToString()); Col = Col + 1; }
                        if (dp["descricaoProdFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DescricaoProdFornecedor); Col = Col + 1; }
                        if (dp["localizacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Localizacao); Col = Col + 1; }
                        if (dp["regiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Regiao); Col = Col + 1; }
                        if (dp["area"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Area.ToString()); Col = Col + 1; }
                        if (dp["cresp"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Cresp.ToString()); Col = Col + 1; }
                        
                        count++;
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_LinhasAcordoPrecos(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Administracao\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Linhas dos Acordos de Preços.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }




        #endregion Acordo de Preços


        #endregion

        #region Projetos



        #endregion

        #region Classificação Fichas Técnicas

        public IActionResult ClassificacaoFichasTecnicas(string id, string option)
        {
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminNutricao);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.ProjectNo = id ?? "";
                //ViewBag.UPermissions = userPerm;
                ViewBag.CreatePermissions = !userPerm.Create.Value;
                ViewBag.UpdatePermissions = !userPerm.Update.Value;
                ViewBag.DeletePermissions = !userPerm.Delete.Value;

                if (option == "Grupos")
                {
                    @ViewBag.Option = "Grupos";
                    @ViewBag.Groups = "hidden";
                }
                else
                {
                    @ViewBag.Option = "linhas";
                    @ViewBag.Groups = "";
                }
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        //O : Lines of Groups
        //1 : Group
        public JsonResult GetClassificationFilesTechniques([FromBody] string option)
        {
            List<ClassificationFilesTechniquesViewModel> result;
            if (option == "Grupos")
                result = DBClassificationFilesTechniques.ParseToViewModel(DBClassificationFilesTechniques.GetTypeFiles(1));
            else
                result = DBClassificationFilesTechniques.ParseToViewModel(DBClassificationFilesTechniques.GetTypeFiles(0));

            return Json(result);
        }
        [HttpPost]
        public JsonResult CreateClassificationTechniques([FromBody] ClassificationFilesTechniquesViewModel data)
        {

            data.CreateUser = User.Identity.Name;
            if (DBClassificationFilesTechniques.Create(DBClassificationFilesTechniques.ParseToDatabase(data)) != null)
                return Json(data);
            else
                return null;
        }

        [HttpPost]
        public JsonResult DeleteClassificationTechniques([FromBody] ClassificationFilesTechniquesViewModel data)
        {

            //Delete lines of Groups
            if (data.Type == 1)
            {
                if (DBClassificationFilesTechniques.GetTypeFiles(0).Exists(x => x.Grupo == data.Code))
                {
                    return Json(null);
                }
            }
            // Delete Group
            var result = DBClassificationFilesTechniques.Delete(DBClassificationFilesTechniques.ParseToDatabase(data));

            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateClassificationTechniques([FromBody] List<ClassificationFilesTechniquesViewModel> data)
        {

            data.ForEach(x =>
            {
                x.UpdateUser = User.Identity.Name;
                DBClassificationFilesTechniques.Update(DBClassificationFilesTechniques.ParseToDatabase(x));
            });
            return Json(data);
        }
        #endregion

        #region Procedimento Confeção

        public IActionResult ProcedimentoConfecao(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminNutricao);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ProjectNo = id ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public JsonResult GetConfectionProcedure()
        {
            List<ProceduresConfectionViewModel> result = ProceduresConfection.ParseToViewModel(ProceduresConfection.GetAll());
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateConfectionProcedure([FromBody] ProceduresConfectionViewModel data)
        {
            data.CreateUser = User.Identity.Name;
            string eReasonCode = "";
            //Create new 
            eReasonCode = ProceduresConfection.Create(ProceduresConfection.ParseToDatabase(data)) == null ? "101" : "";

            if (String.IsNullOrEmpty(eReasonCode))
            {
                return Json(data);
            }
            else
            {
                return Json(eReasonCode);
            }
        }

        [HttpPost]
        public JsonResult DeleteConfectionProcedure([FromBody] ProceduresConfectionViewModel data)
        {
            var result = ProceduresConfection.Delete(ProceduresConfection.ParseToDatabase(data));
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateConfectionProcedure([FromBody] List<ProceduresConfectionViewModel> data)
        {

            data.ForEach(x =>
            {
                x.UpdateUser = User.Identity.Name;
                ProceduresConfection.Update(ProceduresConfection.ParseToDatabase(x));
            });
            return Json(data);
        }
        #endregion

        #region Acções Confeção

        public IActionResult AccoesConfecao(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminNutricao);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ProjectNo = id ?? "";
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public JsonResult GetActionsConfection()
        {
            List<ActionsConfectionViewModel> result = DBActionsConfection.ParseToViewModel(DBActionsConfection.GetAll());
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateActionsConfection([FromBody] ActionsConfectionViewModel data)
        {
            data.CreateUser = User.Identity.Name;
            DBActionsConfection.Create(DBActionsConfection.ParseToDb(data));
            return Json(data);
        }

        [HttpPost]
        public JsonResult DeleteActionsConfection([FromBody] ActionsConfectionViewModel data)
        {

            if (ProceduresConfection.GetAllbyActionNo(data.Code).Count() == 0)
            {
                var result = DBActionsConfection.Delete(DBActionsConfection.ParseToDb(data));
                return Json(result);
            }
            else
                return Json(null);
        }

        [HttpPost]
        public JsonResult UpdateActionsConfection([FromBody] List<ActionsConfectionViewModel> data)
        {

            data.ForEach(x =>
            {
                x.UpdateUser = User.Identity.Name;
                DBActionsConfection.Update(DBActionsConfection.ParseToDb(x));
            });
            return Json(data);
        }
        #endregion

        public IActionResult Localizacoes()
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminExistencias);

            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UserPermissions = userPermissions;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        public IActionResult DetalhesLocalizacao(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminExistencias);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.LocationCode = id;
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        public UserAccessesViewModel GetPermissions(string id)
        {
            return DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Administração);
            //UserAccessesViewModel UPerm = new UserAccessesViewModel();
            //if (id == "Engenharia")
            //{
            //    UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Engenharia, Enumerations.Features.Administração);
            //}
            //if (id == "Ambiente")
            //{
            //    UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Ambiente, Enumerations.Features.Administração);
            //}
            //if (id == "Nutricao")
            //{
            //    UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Nutrição, Enumerations.Features.Administração);
            //}
            //if (id == "Vendas")
            //{
            //    UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Vendas, Enumerations.Features.Administração);
            //}
            //if (id == "Apoio")
            //{
            //    UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Apoio, Enumerations.Features.Administração);
            //}
            //if (id == "PO")
            //{
            //    UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.PO, Enumerations.Features.Administração);
            //}
            //if (id == "NovasAreas")
            //{
            //    UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.NovasÁreas, Enumerations.Features.Administração);
            //}
            //if (id == "Internacionalizacao")
            //{
            //    UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Internacional, Enumerations.Features.Administração);
            //}
            //if (id == "Juridico")
            //{
            //    UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Jurídico, Enumerations.Features.Administração);
            //}
            //if (id == "Compras")
            //{
            //    UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Compras, Enumerations.Features.Administração);
            //}
            //if (id == "Administracao")
            //{
            //    UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Administração, Enumerations.Features.Administração);
            //}

            //return UPerm;
        }

        #region ConfiguracaoCCP
        public IActionResult ConfiguracaoCCP()
        {
            //UserAccessesViewModel UPerm= GetPermissions("Administracao");
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminGeral);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.UPermissions = userPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult ConfiguracaoTemposCCP()
        {
            //UserAccessesViewModel UPerm= GetPermissions("Administracao");
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminGeral);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.UPermissions = userPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        //public IActionResult ConfiguracaoTiposProcedimento(int id)
        //{
        //    UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminGeral);
        //    if (userPerm != null && userPerm.Read.Value)
        //    {
        //        ViewBag.UPermissions = userPerm;
        //        ViewBag.No = id;
        //        ViewBag.reportServerURL = _config.ReportServerURL;

        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("AccessDenied", "Error");
        //    }
        //}
        public IActionResult ConfiguracaoTiposProcedimento(int id)
        {
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminGeral);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.UPermissions = userPerm;
                ViewBag.No = id;
                ViewBag.reportServerURL = _config.ReportServerURL;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        #region zpgm.ALT_CCP_#001.y2019
        public IActionResult ListaTiposProcedimento()
        {
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminGeral);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.UPermissions = userPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        #endregion


        [HttpPost]
        public JsonResult GetConfiguracaoCCP()
        {
            ConfiguracaoCcp CCP = DBConfiguracaoCCP.GetById(1);

            return Json(CCP);
        }

        [HttpPost]
        public JsonResult UpdateConfiguracaoCCP([FromBody] ConfiguracaoCcp data)
        {
            ConfiguracaoCcp CCP = new ConfiguracaoCcp();

            CCP.Id = 1;
            CCP.EmailJurididos = data.EmailJurididos;
            CCP.Email2Juridicos = data.Email2Juridicos;
            CCP.EmailFinanceiros = data.EmailFinanceiros;
            CCP.Email2Financeiros = data.Email2Financeiros;
            CCP.EmailCa = data.EmailCa;
            CCP.EmailContabilidade = data.EmailContabilidade;
            CCP.Email2Contabilidade = data.Email2Contabilidade;
            CCP.Email3Contabilidade = data.Email3Contabilidade;
            CCP.EmailCompras = data.EmailCompras;
            CCP.Email2Compras = data.Email2Compras;
            CCP.Email3Compras = data.Email3Compras;
            CCP.Email4Compras = data.Email4Compras;
            CCP.Email5Compras = data.Email5Compras;
            CCP.Email6Compras = data.Email6Compras;
            CCP.Email7Compras = data.Email7Compras;
            CCP.Email8Compras = data.Email8Compras;
            CCP.Email9Compras = data.Email9Compras;
            CCP.Email10Compras = data.Email10Compras;
            CCP.Email11Compras = data.Email11Compras;

            DBConfiguracaoCCP.Update(CCP);

            return Json(data);
        }

        [HttpPost]
        public JsonResult GetConfiguracaoTemposCcp()
        {
            List<ConfiguracaoTemposCcpView> result = DBConfiguracaoCCP.GetAllConfiguracaoTemposToView();
            List<EnumData> CCPTypes = EnumerablesFixed.ProcedimentosCcpType;
            foreach (var t in result)
            {
                t.TipoDescription = CCPTypes.Where(c => c.Id == t.Tipo).FirstOrDefault().Value;
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateConfigTempos([FromBody] ConfiguracaoTemposCcpView data)
        {
            ConfiguraçãoTemposCcp config = CCPFunctions.CastConfigTemposViewToConfigTempos(data);
            config.UtilizadorCriação = User.Identity.Name;
            config.DataHoraCriação = DateTime.Now;

            return Json(DBConfiguracaoCCP.CreateConfiguracaoTempo(config));
        }

        [HttpPost]
        public JsonResult UpdateConfigTempos([FromBody] ConfiguracaoTemposCcpView data)
        {
            ConfiguraçãoTemposCcp config = CCPFunctions.CastConfigTemposViewToConfigTempos(data);

            config.UtilizadorCriação = User.Identity.Name;
            config.DataHoraModificação = DateTime.Now;

            return Json(DBConfiguracaoCCP.UpdateConfiguracaoTempo(config));
        }

        public JsonResult DeleteConfigTempos([FromBody] ConfiguracaoTemposCcpView data)
        {
            if(data == null)
            {
                return Json(false);
            }

            return Json(DBConfiguracaoCCP.DeleteConfiguracaoTempo(data.Tipo));
        }

        #region zpgm.ALT_CCP_#001.y2019
        [HttpPost]
        public JsonResult GetTiposProcedimento()
        {
            List<TipoProcedimentoCcp> tipos = DBConfiguracaoCCP.GetAllTypes(false);
            return Json(tipos);
        }

        [HttpPost]
        public JsonResult GetTipoProcedimentoDetails([FromBody] JObject param)
        {
            try
            {
                if (param["id"] == null)
                    return Json(null);

                int id = (int)param["id"];

                TipoProcedimentoCcp result = DBConfiguracaoCCP.GetTypeById(id);
                return Json(result);
            }
            catch (Exception ex)
            {

                return Json(null);
            }


        }
        [HttpPost]
        public JsonResult CreateTipo()
        {
            TipoProcedimentoCcp newTipo = new TipoProcedimentoCcp()
            {
                Abreviatura = "",
                DescricaoTipo = "",
                UtilizadorCriacao = User.Identity.Name,
                DataCriacao = DateTime.Now
            };

            if (!DBConfiguracaoCCP.__CreateType(newTipo))
                return (Json(null));

            if (newTipo != null)
                return Json(newTipo.IdTipo);

            return Json(null);
        }

        [HttpPost]
        public JsonResult UpdateTipo([FromBody]TipoProcedimentoCcp data)
        {
            if (data == null)
                return Json(false);

            data.UtilizadorModificacao = User.Identity.Name;
            data.DataModificacao = DateTime.Now;

            if (data.FundamentoLegalTipoProcedimentoCcp != null)
            {
                foreach (var f in data.FundamentoLegalTipoProcedimentoCcp)
                {
                    if (f.UtilizadorCriacao == null || f.UtilizadorCriacao == "")
                    {
                        f.UtilizadorCriacao = User.Identity.Name;
                        f.DataCriacao = DateTime.Now;
                    }
                    else
                    {
                        f.UtilizadorModificacao = User.Identity.Name;
                        f.DataModificacao = DateTime.Now;
                    }
                }
            }

            return Json(DBConfiguracaoCCP.__UpdateType(data));
        }

        [HttpPost]
        public JsonResult CreateFundamento([FromBody]FundamentoLegalTipoProcedimentoCcp data)
        {
            if (data == null)
                return Json(false);

            data.UtilizadorCriacao = User.Identity.Name;
            data.DataCriacao = DateTime.Now;

            return Json(DBConfiguracaoCCP.__CreateReason(data));
        }

        [HttpPost]
        public JsonResult UpdateFundamento([FromBody]FundamentoLegalTipoProcedimentoCcp data)
        {
            if (data == null)
                return Json(false);

            return Json(DBConfiguracaoCCP.__UpdateReason(data));
        }
        #endregion

        #endregion

        #region Config. Mercado Local
        public IActionResult ConfiguracaoMLResponsaveis()
        {
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminGeral);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.UPermissions = userPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetConfigML()
        {
            List<ConfigMercadoLocal> result = DBConfigMercadoLocal.GetAll();
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateConfigML([FromBody] ConfigMercadoLocal linha)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 0;
            result.eMessage = "A linha foi Criada com sucesso.";

            try
            {
                if (DBConfigMercadoLocal.Create(linha) == null)
                {
                    result.eReasonCode = 1;
                    result.eMessage = "Ocorreu um erro ao Criar a linha Config Mercado Local.";
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";

                return Json(result);
            }
        }

        [HttpPost]
        public JsonResult UpdateLinhaConfigML([FromBody] ConfigMercadoLocal linha)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 0;
            result.eMessage = "A linha foi atualizada com sucesso.";

            try
            {
                if (DBConfigMercadoLocal.Update(linha) == null)
                {
                    result.eReasonCode = 1;
                    result.eMessage = "Ocorreu um erro ao atualizar a linha Config Mercado Local.";
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";

                return Json(result);
            }
        }

        [HttpPost]
        public JsonResult DeleteLinhaConfigML([FromBody] ConfigMercadoLocal linha)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 0;
            result.eMessage = "A linha foi Eliminada com sucesso.";

            try
            {
                if (DBConfigMercadoLocal.Delete(linha) == false)
                {
                    result.eReasonCode = 1;
                    result.eMessage = "Ocorreu um erro ao Eliminar a linha Config Mercado Local.";
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";

                return Json(result);
            }
        }
        #endregion

        #region Receção Faturação - Conf. Problemas
        public IActionResult ConfigProblemas()
        {
            //UserAccessesViewModel UPerm = GetPermissions("Administracao");
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminReceçãoFaturação);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.UPermissions = userPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetProblemConfigurations()
        {
            Services.BillingReceptionService billingReceptionService = new Services.BillingReceptionService();
            var result = billingReceptionService.GetAllProblems();
            return Json(result);
        }

        public IActionResult ConfigProblemasDetalhes([FromQuery] string id, [FromQuery] string type)
        {
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminReceçãoFaturação);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.ProblemId = id;
                ViewBag.ProblemType = type;
                ViewBag.UPermissions = userPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetProblemConfigDetails([FromBody] Newtonsoft.Json.Linq.JObject requestParams)
        {
            string problemId = string.Empty;
            string problemType = string.Empty;
            RecFacturasProblemas result = null;

            problemId = requestParams["id"].ToString();
            problemType = requestParams["type"].ToString();

            if (string.IsNullOrEmpty(problemId))
            {
                result = new RecFacturasProblemas();
            }
            else
            {
                Services.BillingReceptionService billingReceptionService = new Services.BillingReceptionService();
                result = billingReceptionService.GetQuestionID(problemId, problemType);
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateProblemConfig([FromBody] RecFacturasProblemas item)
        {
            Services.BillingReceptionService billingReceptionService = new Services.BillingReceptionService();
            var createdItem = billingReceptionService.CreateProblemConfig(item);

            ErrorHandler result = new ErrorHandler();
            if (createdItem != null)
            {
                result.eMessage = "Registo criado com sucesso.";
                result.eReasonCode = 1;
            }
            else
            {
                result.eMessage = "Ocorreu um erro ao criar o registo.";
                result.eReasonCode = 2;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateProblemConfig([FromBody] RecFacturasProblemas item)
        {
            Services.BillingReceptionService billingReceptionService = new Services.BillingReceptionService();
            var updatedItem = billingReceptionService.UpdateProblemConfig(item);

            ErrorHandler result = new ErrorHandler();
            if (updatedItem != null)
            {
                result.eMessage = "Registo atualizado com sucesso.";
                result.eReasonCode = 1;
            }
            else
            {
                result.eMessage = "Ocorreu um erro ao atualizar o registo.";
                result.eReasonCode = 2;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteProblemConfig([FromBody] RecFacturasProblemas item)
        {
            Services.BillingReceptionService billingReceptionService = new Services.BillingReceptionService();
            var updatedItem = billingReceptionService.DeleteProblemConfig(item);

            ErrorHandler result = new ErrorHandler();
            if (updatedItem != null)
            {
                result.eMessage = "Registo eliminado com sucesso.";
                result.eReasonCode = 1;
            }
            else
            {
                result.eMessage = "Ocorreu um erro ao eliminar o registo.";
                result.eReasonCode = 2;
            }
            return Json(result);
        }
        #endregion

        #region Receção Faturação - Conf. Destinatários
        public IActionResult ConfigDestinatarios()
        {
            //UserAccessesViewModel UPerm = GetPermissions("Administracao");
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminReceçãoFaturação);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.UPermissions = userPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult ConfigDestinatariosDetalhes([FromQuery] string id)
        {
            //UserAccessesViewModel UPerm = GetPermissions("Administracao");
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminReceçãoFaturação);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.ProblemId = id;
                ViewBag.UPermissions = userPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetConfAddressees()
        {
            var items = DBRFConfigDestinatarios.GetAll();
            return Json(items);
        }

        [HttpPost]
        public JsonResult GetConfAddressee([FromBody] string id)
        {
            var item = DBRFConfigDestinatarios.GetById(id);
            if (item == null)
                item = new RecFaturacaoConfigDestinatarios();
            return Json(item);
        }
        [HttpPost]
        public JsonResult CreateConfAddressee([FromBody] RecFaturacaoConfigDestinatarios item)
        {
            var createdItem = DBRFConfigDestinatarios.Create(item);

            ErrorHandler result = new ErrorHandler();
            if (createdItem != null)
            {
                result.eMessage = "Registo criado com sucesso.";
                result.eReasonCode = 1;
            }
            else
            {
                result.eMessage = "Ocorreu um erro ao criar o registo.";
                result.eReasonCode = 2;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateConfAddressee([FromBody] RecFaturacaoConfigDestinatarios item)
        {
            var updatedItem = DBRFConfigDestinatarios.Update(item);

            ErrorHandler result = new ErrorHandler();
            if (updatedItem != null)
            {
                result.eMessage = "Registo atualizado com sucesso.";
                result.eReasonCode = 1;
            }
            else
            {
                result.eMessage = "Ocorreu um erro ao atualizar o registo.";
                result.eReasonCode = 2;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteConfAddressee([FromBody] RecFaturacaoConfigDestinatarios item)
        {
            var deleted = DBRFConfigDestinatarios.Delete(item);

            ErrorHandler result = new ErrorHandler();
            if (deleted)
            {
                result.eMessage = "Registo eliminado com sucesso.";
                result.eReasonCode = 1;
            }
            else
            {
                result.eMessage = "Ocorreu um erro ao eliminar o registo.";
                result.eReasonCode = 2;
            }
            return Json(result);
        }
        #endregion

        #region Taxa Residuos
        public IActionResult TaxaResiduos()
        {
            UserAccessesViewModel userAccesses = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminTaxaResiduos);
            if (userAccesses != null && userAccesses.Read.Value)
            {
                ViewBag.UserPermissions = userAccesses;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        [HttpPost]
        public JsonResult GetAllWasteRate()
        {
            List<WasteRateViewModel> dp = DBWasteRate.ParseToViewModel(DBWasteRate.GetAll());
            List<DDMessageRelated> result = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, "", "", 0, "").Select(x => new DDMessageRelated()
            {
                id = x.Code,
                value = x.Name,
                extra = x.MeasureUnit
            }).ToList();
            foreach (WasteRateViewModel item in dp)
            {
                foreach (DDMessageRelated res in result)
                {
                    if (item.Recurso == res.id)
                    {
                        item.RecursoName = res.value;
                    }
                }
            }
            return Json(dp);
        }
        [HttpPost]
        public JsonResult UpdateWasteRate([FromBody] List<WasteRateViewModel> dp)
        {
            ErrorHandler responde = new ErrorHandler();
            responde.eReasonCode = 1;
            responde.eMessage = "Atualizado com sucesso";
            if (dp != null)
            {
                List<TaxaResiduos> getAllLines = DBWasteRate.GetAll();
                if (getAllLines != null && getAllLines.Count > 0)
                {
                    foreach (TaxaResiduos psc in getAllLines)
                    {
                        if (!dp.Any(x => x.Recurso == psc.Recurso))
                        {
                            DBWasteRate.Delete(psc);
                        }
                    }
                    dp.ForEach(x =>
                    {
                        TaxaResiduos dpObject = DBWasteRate.GetById(x.Recurso);
                        if (dpObject != null)
                        {
                            TaxaResiduos newdp = DBWasteRate.ParseToDatabase(x);
                            newdp.DataHoraModificação = DateTime.Now;
                            newdp.UtilizadorModificação = User.Identity.Name;
                            DBWasteRate.Update(newdp);
                        }
                        else
                        {
                            TaxaResiduos newdp = DBWasteRate.ParseToDatabase(x);
                            newdp.DataHoraCriação = DateTime.Now;
                            newdp.UtilizadorCriação = User.Identity.Name;
                            DBWasteRate.Create(newdp);
                        }
                    });
                }
                else
                {
                    dp.ForEach(x => {
                        TaxaResiduos newdp = DBWasteRate.ParseToDatabase(x);
                        newdp.DataHoraCriação = DateTime.Now;
                        newdp.UtilizadorCriação = User.Identity.Name;
                        DBWasteRate.Create(newdp);
                    });
                }

            }
            else
            {
                responde.eReasonCode = 2;
                responde.eMessage = "Ocorreu um erro ao atualizar";
            }
            return Json(responde);
        }
        #endregion

        #region Pedidos DEV

        public IActionResult PedidosDEV_List()
        {
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminPedidosDEV);

            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.ReadPermissions = userPerm.Read.Value;
                ViewBag.CreatePermissions = userPerm.Create.Value;
                ViewBag.UpdatePermissions = userPerm.Update.Value;
                ViewBag.DeletePermissions = userPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult PedidosDEV(string id)
        {
            ViewBag.NoPedidoDEV = id;

            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminPedidosDEV);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.ReadPermissions = !userPerm.Read.Value;
                ViewBag.CreatePermissions = !userPerm.Create.Value;
                ViewBag.UpdatePermissions = !userPerm.Update.Value;
                ViewBag.DeletePermissions = !userPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        #endregion

        #region Contactos Servicos
        public IActionResult ContactosServicos(string id)
        {

            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminContactos);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetContactosServicos()
        {
            List<ContactosServicos> result = DBContactsServicos.GetAll().Select(x => new ContactosServicos()
            {
                ID = x.ID,
                Servico = x.Servico,
                CriadoPor = x.CriadoPor,
                DataCriacao = x.DataCriacao,
                AlteradoPor = x.AlteradoPor,
                DataAlteracao = x.DataAlteracao
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateContactosServicos([FromBody] List<ContactosServicos> data)
        {
            List<ContactosServicos> results = DBContactsServicos.GetAll();
            results.RemoveAll(x => data.Any(u => u.ID == x.ID));
            results.ForEach(x => DBContactsServicos.Delete(x));

            data.ForEach(x =>
            {
                ContactosServicos servico = new ContactosServicos()
                {
                    Servico = x.Servico
                };
                if (x.ID > 0)
                {
                    servico.ID = x.ID;
                    servico.CriadoPor = x.CriadoPor;
                    servico.DataCriacao = x.DataCriacao;
                    servico.DataAlteracao = DateTime.Now;
                    servico.AlteradoPor = User.Identity.Name;
                    DBContactsServicos.Update(servico);
                }
                else
                {
                    servico.DataCriacao = DateTime.Now;
                    servico.CriadoPor = User.Identity.Name;
                    DBContactsServicos.Create(servico);
                }
            });
            return Json(data);
        }
        #endregion

        #region Contactos Funcoes
        public IActionResult ContactosFuncoes(string id)
        {

            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminContactos);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !UPerm.Create.Value;
                ViewBag.UpdatePermissions = !UPerm.Update.Value;
                ViewBag.DeletePermissions = !UPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetContactosFuncoes()
        {
            List<ContactosFuncoes> result = DBContactsFuncoes.GetAll().Select(x => new ContactosFuncoes()
            {
                ID = x.ID,
                Funcao = x.Funcao,
                CriadoPor = x.CriadoPor,
                DataCriacao = x.DataCriacao,
                AlteradoPor = x.AlteradoPor,
                DataAlteracao = x.DataAlteracao
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateContactosFuncoes([FromBody] List<ContactosFuncoes> data)
        {
            List<ContactosFuncoes> results = DBContactsFuncoes.GetAll();
            results.RemoveAll(x => data.Any(u => u.ID == x.ID));
            results.ForEach(x => DBContactsFuncoes.Delete(x));

            data.ForEach(x =>
            {
                ContactosFuncoes funcao = new ContactosFuncoes()
                {
                    Funcao = x.Funcao
                };
                if (x.ID > 0)
                {
                    funcao.ID = x.ID;
                    funcao.CriadoPor = x.CriadoPor;
                    funcao.DataCriacao = x.DataCriacao;
                    funcao.DataAlteracao = DateTime.Now;
                    funcao.AlteradoPor = User.Identity.Name;
                    DBContactsFuncoes.Update(funcao);
                }
                else
                {
                    funcao.DataCriacao = DateTime.Now;
                    funcao.CriadoPor = User.Identity.Name;
                    DBContactsFuncoes.Create(funcao);
                }
            });
            return Json(data);
        }
        #endregion

        #region Config Linhas Enc Fornecedor

        public IActionResult ConfigLinhasEncFornecedor()
        {
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminConfigLinhasEncFornecedor);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.UPermissions = userPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public ActionResult GetListLinhasEncFornecedor()
        {
            List<ConfigLinhasEncFornecedor> result = DBConfigLinhasEncFornecedor.GetAll();
            return Json(result.OrderBy(x => x.VendorNo).ThenBy(x => x.LineNo));
        }

        [HttpPost]
        public JsonResult UpdateListLinhasEncFornecedor([FromBody] List<ConfigLinhasEncFornecedor> data)
        {
            //Get All
            List<ConfigLinhasEncFornecedor> previousList = DBConfigLinhasEncFornecedor.GetAll();

            foreach (ConfigLinhasEncFornecedor line in previousList)
            {
                if (!data.Any(x => x.VendorNo == line.VendorNo && x.LineNo == line.LineNo))
                {
                    DBConfigLinhasEncFornecedor.Delete(line);
                }
            }

            data.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.VendorNo) && x.LineNo > 0)
                {
                    x.UtilizadorModificacao = User.Identity.Name;
                    x.DataHoraModificacao = DateTime.Now;
                    DBConfigLinhasEncFornecedor.Update(x);
                }
                else
                {
                    x.LineNo = DBConfigLinhasEncFornecedor.GetMaxLineNoByVendorNo(x.VendorNo) + 1;
                    x.UtilizadorCriacao = User.Identity.Name;
                    x.DataHoraCriacao = DateTime.Now;
                    DBConfigLinhasEncFornecedor.Create(x);
                }
            });

            return Json(data);
        }

        [HttpPost]
        //Verifica se já existe alguma Folha de Horas no mesmo períoda para o mesmo Empregado
        public JsonResult DeleteLinhasEncFornecedor([FromBody] ConfigLinhasEncFornecedor data)
        {
            ConfigLinhasEncFornecedorViewModel result = new ConfigLinhasEncFornecedorViewModel();
            result.eReasonCode = 99;
            result.eMessage = "Ocorreu um erro.";
            if (data != null)
            {
                ConfigLinhasEncFornecedor Linha = DBConfigLinhasEncFornecedor.GetByVendorNoAndLineNo(data.VendorNo, data.LineNo);
                if (Linha != null)
                {
                    if (DBConfigLinhasEncFornecedor.Delete(Linha) == true)
                    {
                        result.eReasonCode = 0;
                        result.eMessage = "Linha eliminada com sucesso.";
                    }
                    else
                    {
                        result.eReasonCode = 99;
                        result.eMessage = "Ocorreu um erro ao elinar a linha.";
                    }
                }
                else
                {
                    result.eReasonCode = 99;
                    result.eMessage = "Não foi possivel obter a informação da linha.";
                }
            }
            else
            {
                result.eReasonCode = 99;
                result.eMessage = "Não foi possivel obter a informação da linha.";
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProductInfoLinhasEncFornecedor([FromBody] ConfigLinhasEncFornecedor linha)
        {
            NAVProductsViewModel product = new NAVProductsViewModel();

            try
            {
                if (!string.IsNullOrEmpty(linha.No))
                {
                    product = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, linha.No).FirstOrDefault();
                    product.OpenOrderLines = false;

                    if (product.InventoryValueZero == 1)
                    {
                        product.LocationCode = DBConfigurations.GetById(1).ArmazemCompraDireta;
                    }
                    else
                    {
                        NAVStockKeepingUnitViewModel localizacao = DBNAV2017StockKeepingUnit.GetByProductsNo(_config.NAVDatabaseName, _config.NAVCompanyName, linha.No).FirstOrDefault();
                        product.UnitCost = localizacao.UnitCost;
                        product.LocationCode = localizacao.LocationCode;
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(null);
            }

            return Json(product);
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_LinhasEncFornecedor([FromBody] List<ConfigLinhasEncFornecedor> dp)
        {
            string sWebRootFolder = _generalConfig.FileUploadFolder + "Administracao\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Linhas Encomenda Fornecedor");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Nº Fornecedor");
                row.CreateCell(1).SetCellValue("Nº Linha");
                row.CreateCell(2).SetCellValue("Tipo");
                row.CreateCell(3).SetCellValue("Nº");
                row.CreateCell(4).SetCellValue("Descrição");
                row.CreateCell(5).SetCellValue("Descrição 2");
                row.CreateCell(6).SetCellValue("Unidade de Medida");
                row.CreateCell(7).SetCellValue("Quantidade");
                row.CreateCell(8).SetCellValue("Valor");

                if (dp != null)
                {
                    int count = 1;
                    foreach (ConfigLinhasEncFornecedor item in dp)
                    {
                        row = excelSheet.CreateRow(count);

                        row.CreateCell(0).SetCellValue(item.VendorNo);
                        row.CreateCell(1).SetCellValue(item.LineNo);
                        row.CreateCell(2).SetCellValue(item.Type.HasValue ? item.Type == 1 ? "Recurso" : "Produto" : "");
                        row.CreateCell(3).SetCellValue(item.No);
                        row.CreateCell(4).SetCellValue(item.Description);
                        row.CreateCell(5).SetCellValue(item.Description2);
                        row.CreateCell(6).SetCellValue(item.UnitOfMeasure);
                        row.CreateCell(7).SetCellValue(item.Quantity.HasValue ? item.Quantity.ToString() : "");
                        row.CreateCell(8).SetCellValue(item.Valor.HasValue ? item.Valor.ToString() : "");

                        count++;
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_LinhasEncFornecedor(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Administracao\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Linhas Encomenda Fornecedor.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        #endregion








        public JsonResult TesteARomao()
        {
                EmailsAprovações EmailApproval = new EmailsAprovações()
                {
                    NºMovimento = 999999,
                    EmailDestinatário = "ARomao@such.pt",
                    NomeDestinatário = "ARomao@such.pt",
                    Assunto = "eSUCH - Aprovação Pendente",
                    DataHoraEmail = DateTime.Now,
                    TextoEmail = "Existe uma nova tarefa pendente da sua aprovação no eSUCH!",
                    Enviado = false
                };


                SendEmailApprovals Email = new SendEmailApprovals
                {
                    Subject = "eSUCH - Aprovação Pendente",
                    From = "ARomao@such.pt"
                };

                Email.To.Add("ARomao@such.pt");

                Email.Body = MakeEmailBodyContent("Existe uma nova tarefa pendente da sua aprovação no eSUCH!");

                Email.IsBodyHtml = true;
                Email.EmailApproval = EmailApproval;

                Email.SendEmail();

            return Json(null);
        }
        public static string MakeEmailBodyContent(string BodyText)
        {
            string Body = @"<html>" +
                                "<head>" +
                                    "<style>" +
                                        "table{border:0;} " +
                                        "td{width:600px; vertical-align: top;}" +
                                    "</style>" +
                                "</head>" +
                                "<body>" +
                                    "<table>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Caro (a)," +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                BodyText +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Com os melhores cumprimentos," +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<i>SUCH - Serviço de Utilização Comum dos Hospitais</i>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                            "</html>";

            return Body;
        }



    }
}