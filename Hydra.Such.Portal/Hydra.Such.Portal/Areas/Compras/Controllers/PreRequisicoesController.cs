using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Data.Logic.Compras;
using Microsoft.Extensions.Options;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Portal.Areas.Compras.Controllers
{
    public class PreRequisicoesController : Controller
    {
        [Area("Compras")]
        public IActionResult Index(string PreRequesitionNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 10, 3);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.PreRequesitionNo = PreRequesitionNo ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetPreRequesitionDetails([FromBody] PreRequesitionsViewModel data)
        {
            if (data != null)
            {
                PreRequesitionsViewModel result = new PreRequesitionsViewModel();
                if (data.PreRequesitionsNo != "")
                {
                    PréRequisição PreRequisition = DBPreRequesition.GetByNo(data.PreRequesitionsNo);
                    result = DBPreRequesition.ParseToViewModel(PreRequisition);
                    
                }
                return Json(result);

            }
            return Json(false);
        }

        #region Populate CB
        public JsonResult CBVehicleFleetBool([FromBody] int id)
        {
            bool? FleetBool = DBRequesitionType.GetById(id).Frota;
            return Json(FleetBool);
        }

        public JsonResult GetPlaceData([FromBody] int placeId)
        {
            PlacesViewModel PlacesData = DBPlaces.ParseToViewModel(DBPlaces.GetById(placeId));
            return Json(PlacesData);
        }
        #endregion
        
        #region Numeração
        [HttpPost]
        public JsonResult ValidateNumeration([FromBody] PreRequesitionsViewModel data)
        {
            //Get Project Numeration
            Configuração Cfg = DBConfigurations.GetById(1);
            int ProjectNumerationConfigurationId = 0;
            ProjectNumerationConfigurationId = Cfg.NumeraçãoPréRequisições.Value;

            ConfiguraçãoNumerações CfgNumeration = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);

            //Validate if ProjectNo is valid
            if (!(data.PreRequesitionsNo == "" || data.PreRequesitionsNo == null) && !CfgNumeration.Manual.Value)
            {
                return Json("A numeração configurada para contratos não permite inserção manual.");
            }
            else if (data.PreRequesitionsNo == "" && !CfgNumeration.Automático.Value)
            {
                return Json("É obrigatório inserir o Nº de Contrato.");
            }

            return Json("");
        }
        #endregion

        #region CRUD
        [HttpPost]
        public JsonResult CreatePreRequesition([FromBody] PreRequesitionsViewModel data)
        {
            try
            {
                if (data != null)
                {
                    //Get Contract Numeration
                    Configuração Configs = DBConfigurations.GetById(1);
                    int ProjectNumerationConfigurationId = 0;
                    ProjectNumerationConfigurationId = Configs.NumeraçãoPréRequisições.Value;
                

                    data.PreRequesitionsNo = DBNumerationConfigurations.GetNextNumeration(ProjectNumerationConfigurationId, (data.PreRequesitionsNo == "" || data.PreRequesitionsNo == null));

                    if (data.PreRequesitionsNo != null)
                    {
                        PréRequisição pPreRequisicao = DBPreRequesition.ParseToDB(data);
                        pPreRequisicao.UtilizadorCriação = User.Identity.Name;
                        pPreRequisicao.DataHoraCriação = DateTime.Now;

                        //Create Contract On Database
                       
                        pPreRequisicao = DBPreRequesition.Create(pPreRequisicao);

                        if (pPreRequisicao == null)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Ocorreu um erro ao criar o contrato.";
                        }
                        else
                        {
                            ////Create Client Contract Requisitions
                            //data.ClientRequisitions.ForEach(r =>
                            //{
                            //    r.ContractNo = cContract.NºDeContrato;
                            //    r.CreateUser = User.Identity.Name;
                            //    DBContractClientRequisition.Create(DBContractClientRequisition.ParseToDB(r));
                            //});

                            ////Create Contract Invoice Texts
                            //data.InvoiceTexts.ForEach(r =>
                            //{
                            //    r.ContractNo = cContract.NºDeContrato;
                            //    r.CreateUser = User.Identity.Name;
                            //    DBContractInvoiceText.Create(DBContractInvoiceText.ParseToDB(r));
                            //});

                            //Update Last Numeration Used
                            ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);
                            ConfigNumerations.ÚltimoNºUsado = data.PreRequesitionsNo;
                            ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                            DBNumerationConfigurations.Update(ConfigNumerations);

                            data.eReasonCode = 1;
                        }
                    }
                    else
                    {
                        data.eReasonCode = 5;
                        data.eMessage = "A numeração configurada não é compativel com a inserida.";
                    }
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 4;
                data.eMessage = "Ocorreu um erro ao criar o contrato";
            }
            return Json(data);

        }
        #endregion
    }
}