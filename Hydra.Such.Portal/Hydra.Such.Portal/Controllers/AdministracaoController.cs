using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
using Hydra.Such.Data.Logic.Compras;
using Hydra.Such.Data.Logic.Approvals;
using Hydra.Such.Data.ViewModel.Approvals;
using Microsoft.Extensions.Options;
using Hydra.Such.Data;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class AdministracaoController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;

        public AdministracaoController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Utilizadores
        public IActionResult ConfiguracaoUtilizadores()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetListUsers()
        {
            List<ConfigUtilizadores> result = DBUserConfigurations.GetAll();

            if (result != null)
            {
                result.ForEach(Utilizador =>
                {
                    Utilizador.RegiãoPorDefeito = Utilizador.RegiãoPorDefeito == null ? "" : DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name, Utilizador.RegiãoPorDefeito).FirstOrDefault().Name;
                    Utilizador.AreaPorDefeito = Utilizador.AreaPorDefeito == null ? "" : DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 2, User.Identity.Name, Utilizador.AreaPorDefeito).FirstOrDefault().Name;
                    Utilizador.CentroRespPorDefeito = Utilizador.CentroRespPorDefeito == null ? "" : DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 3, User.Identity.Name, Utilizador.CentroRespPorDefeito).FirstOrDefault().Name;
                });
            };

            return Json(result);
        }


        public IActionResult ConfiguracaoUtilizadoresDetalhes(string id)
        {
            ViewBag.UserId = id;
            return View();
        }

        [HttpPost]
        public JsonResult GetUserConfigData([FromBody] UserConfigurationsViewModel data)
        {
            ConfigUtilizadores CU = DBUserConfigurations.GetById(data.IdUser);
            UserConfigurationsViewModel result = new UserConfigurationsViewModel()
            {
                IdUser = "",
                UserAccesses = new List<UserAccessesViewModel>(),
                UserProfiles = new List<ProfileModelsViewModel>()
            };

            if (CU != null)
            {
                result.IdUser = CU.IdUtilizador;
                result.Name = CU.Nome;
                result.Active = CU.Ativo;
                result.Administrator = CU.Administrador;
                result.Regiao = CU.RegiãoPorDefeito;
                result.Area = CU.AreaPorDefeito;
                result.Cresp = CU.CentroRespPorDefeito;

                result.UserAccesses = DBUserAccesses.GetByUserId(data.IdUser).Select(x => new UserAccessesViewModel()
                {
                    IdUser = x.IdUtilizador,
                    Area = x.Área,
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
                Ativo = data.Active,
                RegiãoPorDefeito = data.Regiao,
                AreaPorDefeito = data.Area,
                CentroRespPorDefeito = data.Cresp,
                UtilizadorCriação = User.Identity.Name,
            });

            data.IdUser = ObjectCreated.IdUtilizador;

            //Add Accesses
            data.UserAccesses.ForEach(x =>
            {
                DBUserAccesses.Create(new AcessosUtilizador()
                {
                    IdUtilizador = ObjectCreated.IdUtilizador,
                    Área = x.Area,
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
                userConfig.Ativo = data.Active;
                userConfig.Administrador = data.Administrator;
                userConfig.RegiãoPorDefeito = data.Regiao;
                userConfig.AreaPorDefeito = data.Area;
                userConfig.CentroRespPorDefeito = data.Cresp;
                userConfig.DataHoraModificação = DateTime.Now;
                userConfig.UtilizadorModificação = User.Identity.Name;
                DBUserConfigurations.Update(userConfig);

                #region Update Accesses

                //Get Existing from db
                var userAccesses = DBUserAccesses.GetByUserId(data.IdUser);

                //Get items to delete (for changed keys delete old, create new)
                var userAccessesToDelete = userAccesses
                    .Where(x => !data.UserAccesses
                        .Any(y => y.Area == x.Área &&
                            y.Feature == x.Funcionalidade))
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
                        var updatedUA = userAccesses.SingleOrDefault(x => x.Área == userAccess.Area &&
                            x.Funcionalidade == userAccess.Feature);

                        if (updatedUA == null)
                        {
                            //Create
                            updatedUA = new AcessosUtilizador()
                            {
                                IdUtilizador = data.IdUser,
                                Área = userAccess.Area,
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

            //Remover os acessos às dimensões
            DBUserDimensions.DeleteAllFromUser(data.IdUser);

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
        public JsonResult CreateUserAccess([FromBody] UserAccessesViewModel data)
        {
            bool result = false;
            try
            {
                AcessosUtilizador userAccess = new AcessosUtilizador();
                userAccess.IdUtilizador = data.IdUser;
                userAccess.Área = data.Area;
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
            var userAccess = DBUserAccesses.GetById(data.IdUser, data.Area, data.Feature);
            return Json(userAccess != null ? DBUserAccesses.Delete(userAccess) : false);
        }

        #endregion

        #region PerfisModelo
        public IActionResult PerfisModelo()
        {
            return View();
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
            ViewBag.ProfileModelId = id;

            return View();
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
                    Area = x.Área,
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
                    Área = x.Area,
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
                    Área = x.Area,
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
            UserAccessesViewModel UPerm = GetPermissions("Administracao");
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
                DinnerEndTime = Cfg.FimHoraJantar,
                DinnerStartTime = Cfg.InicioHoraJantar,
                LunchEndTime = Cfg.FimHoraAlmoco,
                LunchStartTime = Cfg.InicioHoraAlmoco
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
            configObj.FimHoraJantar = data.DinnerEndTime;
            configObj.InicioHoraJantar = data.DinnerStartTime;
            configObj.InicioHoraAlmoco = data.LunchStartTime;
            configObj.FimHoraAlmoco = data.LunchEndTime;

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
            return View();
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

        #region TabelasAuxiliares

        #region TiposDeProjeto
        public IActionResult TiposProjetoDetalhes(string id)
        {

            UserAccessesViewModel UPerm = GetPermissions(id);
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
                FunctionalAreaCode = x.CódigoÁreaFuncional,
                Region = x.CódigoRegião,
                ResponsabilityCenter = x.CódigoCentroResponsabilidade
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
                    CódigoCentroResponsabilidade = x.ResponsabilityCenter,
                    CódigoRegião = x.Region,
                    CódigoÁreaFuncional = x.FunctionalAreaCode
                };

                if (x.ID > 0)
                {
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
            List<ProjectTypesModelView> result = DBServices.GetAll().Select(x => new ProjectTypesModelView()
            {
                Code = x.Código,
                Description = x.Descrição
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateServices([FromBody] List<ProjectTypesModelView> data)
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
                if (x.Code > 0)
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
                ServiceCode = x.CódServiço,
                ServiceGroup = x.GrupoServiços
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

        public bool CheckIfExist(string ClientNumber, int ServiceCode, bool? ServiceGroup, int param)
        {
            List<ClientServicesViewModel> result = DBClientServices.GetAll().Select(x => new ClientServicesViewModel()
            {
                ClientNumber = x.NºCliente,
                ServiceCode = x.CódServiço,
                ServiceGroup = x.GrupoServiços
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
                    x.Descricao = x.Code + " - " + DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, x.Code, "", 0, "").FirstOrDefault().Name;
                    x.CodTipoTrabalhoTexto = x.CodTipoTrabalho + " - " + DBTipoTrabalhoFH.GetAll().Where(y => y.Codigo == x.CodTipoTrabalho).FirstOrDefault().Descricao;
                    x.FamiliaRecurso = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, x.Code, "", 0, "").FirstOrDefault().ResourceGroup;
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
        #endregion

        #region Configuração Preço Custo Recursos FH
        public IActionResult ConfiguracaoPrecoCustoRecursoFH(string id)
        {
            UserAccessesViewModel UPerm = GetPermissions(id);
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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

            data.RemoveAll(x => DBRHRecursosFH.ParseListToViewModel(results).Any(
                u =>
                    u.NoEmpregado == x.NoEmpregado &&
                    u.Recurso == x.Recurso &&
                    u.NomeRecurso == x.NomeRecurso &&
                    u.FamiliaRecurso == x.FamiliaRecurso &&
                    u.NomeEmpregado == x.NomeEmpregado &&
                    u.UtilizadorCriacao == x.UtilizadorCriacao &&
                    u.DataHoraCriacao == x.DataHoraCriacao
            ));

            data.ForEach(x =>
            {
                RhRecursosFh toUpdate = DBRHRecursosFH.ParseToDB(x);

                NAVResourcesViewModel resource = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, x.Recurso, "", 0, "").FirstOrDefault();
                toUpdate.NomeRecurso = resource.Name;
                toUpdate.FamiliaRecurso = resource.ResourceGroup;

                toUpdate.AlteradoPor = User.Identity.Name;
                DBRHRecursosFH.Update(toUpdate);
            });
            return Json(data);
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
                    u.RubricaSalarial == x.RubricaSalarial
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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

        #region Configurações Aprovações

        public IActionResult ConfiguracaoAprovacoes(string id)
        {
            UserAccessesViewModel UPerm = GetPermissions(id);
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
        public JsonResult GetApprovalConfig()
        {
            List<ApprovalConfigurationsViewModel> result = DBApprovalConfigurations.ParseToViewModel(DBApprovalConfigurations.GetAll());
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
                    Área = x.Area,
                    CódigoÁreaFuncional = x.FunctionalArea,
                    CódigoCentroResponsabilidade = x.ResponsabilityCenter,
                    CódigoRegião = x.Region,
                    DataInicial = string.IsNullOrEmpty(x.StartDate) ? (DateTime?)null : DateTime.Parse(x.StartDate),
                    DataFinal = string.IsNullOrEmpty(x.EndDate) ? (DateTime?)null : DateTime.Parse(x.EndDate)
                };
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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
            UserAccessesViewModel UPerm = GetPermissions("Administracao");
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
            UserAccessesViewModel UPerm = GetPermissions(id);
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

        #endregion

        public UserAccessesViewModel GetPermissions(string id)
        {
            UserAccessesViewModel UPerm = new UserAccessesViewModel();
            if (id == "Engenharia")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Engenharia, Enumerations.Features.Administração);
            }
            if (id == "Ambiente")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Ambiente, Enumerations.Features.Administração);
            }
            if (id == "Nutricao")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Nutrição, Enumerations.Features.Administração);
            }
            if (id == "Vendas")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Vendas, Enumerations.Features.Administração);
            }
            if (id == "Apoio")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Apoio, Enumerations.Features.Administração);
            }
            if (id == "PO")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.PO, Enumerations.Features.Administração);
            }
            if (id == "NovasAreas")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.NovasÁreas, Enumerations.Features.Administração);
            }
            if (id == "Internacionalizacao")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Internacional, Enumerations.Features.Administração);
            }
            if (id == "Juridico")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Jurídico, Enumerations.Features.Administração);
            }
            if (id == "Compras")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Compras, Enumerations.Features.Administração);
            }
            if (id == "Administracao")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Administração, Enumerations.Features.Administração);
            }

            return UPerm;
        }
    }
}