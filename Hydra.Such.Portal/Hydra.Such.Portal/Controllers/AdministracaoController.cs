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

                result.AllowedUserDimensions = DBUserDimensions.GetByUserId(data.IdUser).ToList();
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
                UtilizadorCriação = User.Identity.Name
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
                //Get items to delete
                var userAccessesToDelete = userAccesses
                    .Where(x => !data.UserAccesses
                        .Any(y => y.IdUser == data.IdUser &&
                            y.Area == x.Área &&
                            y.Feature == x.Funcionalidade))
                    .ToList();
                //Delete 
                bool uaSuccessfullyDeleted = DBUserAccesses.Delete(userAccessesToDelete);
                if (!uaSuccessfullyDeleted)
                {
                    data.eMessage = "Ocorreu um erro ao eliminar os acessos do utilizador.";
                }

                //Create or update existing
                data.UserAccesses.ForEach(userAccess =>
                    {
                        var updatedUA = userAccesses.SingleOrDefault(x => x.IdUtilizador == data.IdUser &&
                            x.Área == userAccess.Area &&
                            x.Funcionalidade == userAccess.Feature);

                        if (updatedUA == null)
                        {
                            //Create
                            updatedUA = new AcessosUtilizador()
                            {
                                IdUtilizador = data.IdUser,
                                Área = userAccess.Area,
                                Funcionalidade = userAccess.Feature
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

                DBUserProfiles.DeleteAllFromUser(data.IdUser);
                data.UserProfiles.ForEach(x =>
                {
                    DBUserProfiles.Create(new PerfisUtilizador()
                    {
                        IdUtilizador = userConfig.IdUtilizador,
                        IdPerfil = x.Id,
                        UtilizadorCriação = User.Identity.Name
                    });
                });
                
                //Update AllowedUserDimemsions
                DBUserDimensions.DeleteAllFromUser(data.IdUser);
                DBUserDimensions.Create(data.IdUser, data.AllowedUserDimensions.ParseToDB());
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
                TimeSheetNumeration = Cfg.NumeraçãoFolhasDeHoras
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
                IncrementQuantity = x.NºDígitosIncrementar,
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
                    return Json(exist);
                }
                else
                {
                    return Json(exist);
                }
            }

            return Json(data);
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


        #endregion

        public UserAccessesViewModel GetPermissions(string id)
        {
            UserAccessesViewModel UPerm = new UserAccessesViewModel();
            if (id== "Engenharia")
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