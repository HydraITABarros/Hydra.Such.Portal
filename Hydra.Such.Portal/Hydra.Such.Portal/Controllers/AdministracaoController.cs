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

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class AdministracaoController : Controller
    {
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
        #endregion

        public IActionResult Permicoes()
        {
            return View();
        }

        #region Configuracoes

        public IActionResult Configuracoes()
        {
            return View();
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
                ContactsNumeration = Cfg.NumeraçãoContactos
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
                        } else
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

        #endregion

        public UserAccessesViewModel GetPermissions(string id)
        {
            UserAccessesViewModel UPerm = new UserAccessesViewModel();
            if (id == "Engenharia")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 1, 18);
            }
            if (id == "Ambiente")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 2, 18);
            }
            if (id == "Nutricao")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 3, 18);
            }
            if (id == "Vendas")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 4, 18);
            }
            if (id == "Apoio")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 5, 18);
            }
            if (id == "PO")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 6, 18);
            }
            if (id == "NovasAreas")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 7, 18);
            }
            if (id == "Internacionalizacao")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 8, 18);
            }
            if (id == "Juridico")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 9, 18);
            }
            if (id == "Compras")
            {
                UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 10, 18);
            }

            return UPerm;
        }
    }
}