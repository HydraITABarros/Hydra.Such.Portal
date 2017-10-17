using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.ProjectView;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.Logic.ProjectDiary;
using Hydra.Such.Data.ViewModel.ProjectDiary;
using Microsoft.AspNetCore.Authorization;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class TabelasAuxiliaresController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region TiposDeProjeto
        public IActionResult TiposProjetoDetalhes()
        {
            return View();
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
        public IActionResult TiposGrupoContabProjeto(int id)
        {
            ViewBag.GroupContabTypes = id;
            return View();
        }

        //POPULATE GRID ContabGroupTypes
        public JsonResult GetTiposGrupoContabProjeto([FromBody] ContabGroupTypesProjectView data)
        {
            List<ContabGroupTypesProjectView> result = DBCountabGroupTypes.GetAll().Select(x=> new ContabGroupTypesProjectView() {
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

        public IActionResult ObjetosDeServico()
        {
            return View();
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

        public IActionResult TiposGrupoContabOMProjeto()
        {
            return View();
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
            previousList.ForEach(x => DBCountabGroupTypesOM.DeleteAllFromProfile(x.Código));

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
        public IActionResult TiposRefeicao()
        {
            return View();
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
        public IActionResult DestinosFinaisResiduos()
        {
            return View();
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
        public JsonResult UpdateFinalWasteDestinations ([FromBody] List<FinalWasteDestinationsViewModel> data)
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
        public IActionResult Servicos()
        {
            return View();
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
        public IActionResult ServicosCliente()
        {
            return View();
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
            if(param == 1)
            {
                foreach (var res in result)
                {
                    if (res.ClientNumber == ClientNumber && res.ServiceCode == ServiceCode)
                    {
                        exists = true;
                    }
                }
            }
           
            if(param == 2)
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
    }
}