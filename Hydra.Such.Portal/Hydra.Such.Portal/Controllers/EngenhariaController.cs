using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Project;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.NAV;
using Hydra.Such.Portal.Configurations;
using Microsoft.Extensions.Options;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class EngenhariaController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;

        public EngenhariaController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Contratos()
        {
            return View();
        }

        public IActionResult Requisicoes()
        {
            return View();
        }

        public IActionResult TabelasAuxiliares()
        {
            return View();
        }

        public IActionResult DiarioProjeto()
        {
            return View();
        }

        #region DiárioDeProjetos
        [HttpPost]
        public JsonResult GetAllProjectDiary()
        {
            List<ProjectDiaryViewModel> dp = DBProjectDiary.GetAll().Select(x => new ProjectDiaryViewModel()
            {
                LineNo = x.NºLinha,
                ProjectNo = x.NºProjeto,
                Date = x.Data == null ? String.Empty : x.Data.Value.ToString("yyyy-MM-dd"),
                MovementType = x.TipoMovimento,
                Type = x.Tipo,
                Code = x.Código,
                Description = x.Descrição,
                Quantity = x.Quantidade,
                MeasurementUnitCode = x.CódUnidadeMedida,
                LocationCode = x.CódLocalização,
                ProjectContabGroup = x.GrupoContabProjeto,
                RegionCode = x.CódigoRegião,
                FunctionalAreaCode = x.CódigoÁreaFuncional,
                ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                User = x.Utilizador,
                UnitCost = x.CustoUnitário,
                TotalCost = x.CustoTotal,
                UnitPrice = x.PreçoUnitário,
                TotalPrice = x.PreçoTotal,
                Billable = x.Faturável,
                InvoiceToClientNo = x.FaturaANºCliente
            }).ToList();
            return Json(dp);
        }

        [HttpPost]
        public JsonResult UpdateProjectDiary([FromBody] List<ProjectDiaryViewModel> dp)
        {
            // Get All
            List<DiárioDeProjeto> previousList = DBProjectDiary.GetAll();
            //previousList.RemoveAll(x => !dp.Any(u => u.LineNo == x.NºLinha));
            //previousList.ForEach(x => DBProjectDiary.Delete(x));

            foreach (DiárioDeProjeto line in previousList)
            {
                if (!dp.Any(x => x.LineNo == line.NºLinha))
                {
                    DBProjectDiary.Delete(line);
                }
            }

            //Update or Create
            dp.ForEach(x =>
            {
                DiárioDeProjeto newdp = new DiárioDeProjeto()
                {
                    NºLinha = x.LineNo,
                    NºProjeto = x.ProjectNo,
                    Data = x.Date == "" || x.Date == String.Empty ? (DateTime?)null : DateTime.Parse(x.Date),
                    TipoMovimento = x.MovementType,
                    Tipo = x.Type,
                    Código = x.Code,
                    Descrição = x.Description,
                    Quantidade = x.Quantity,
                    CódUnidadeMedida = x.MeasurementUnitCode,
                    CódLocalização = x.LocationCode,
                    GrupoContabProjeto = x.ProjectContabGroup,
                    CódigoRegião = x.RegionCode,
                    CódigoÁreaFuncional = x.FunctionalAreaCode,
                    CódigoCentroResponsabilidade = x.ResponsabilityCenterCode,
                    Utilizador = User.Identity.Name,
                    CustoUnitário = x.UnitCost,
                    CustoTotal = x.TotalCost,
                    PreçoUnitário = x.UnitPrice,
                    PreçoTotal = x.TotalPrice,
                    Faturável = x.Billable,
                    //FaturaANºCliente = x.InvoiceToClientNo
                };

                if (x.LineNo > 0)
                {
                    newdp.DataHoraModificação = DateTime.Now;
                    newdp.UtilizadorModificação = User.Identity.Name;
                    DBProjectDiary.Update(newdp);
                }
                else
                {
                    newdp.DataHoraCriação = DateTime.Now;
                    newdp.UtilizadorCriação = User.Identity.Name;
                    DBProjectDiary.Create(newdp);
                }
            });

            return Json(dp);
        }

        [HttpPost]
        public JsonResult GetRelatedProjectInfo([FromBody] string projectNo)
        {
            //Get Project Info
            Projetos proj = DBProjects.GetById(projectNo);

            if (proj != null)
            {
                ProjectInfo pi = new ProjectInfo
                {
                    //ProjectNo = proj.NºProjeto,
                    ContabGroup = proj.GrupoContabObra,
                    RegionCode = proj.CódigoRegião,
                    FuncAreaCode = proj.CódigoÁreaFuncional,
                    ResponsabilityCenter = proj.CódigoCentroResponsabilidade
                };

                return Json(pi);
            }
            else
            {
                return Json(null);
            }
        }

        [HttpPost]
        public JsonResult RegisterDiaryLines([FromBody]  List<ProjectDiaryViewModel> dp)
        {
            Guid transactID = Guid.NewGuid();

            //Create Lines in NAV
            //Task<WSCreateProjectDiaryLine.CreateMultiple_Result> TCreateNavDiaryLine = WSProjectDiaryLine.CreateNavDiaryLines(dp, transactID, _configws);
            //TCreateNavDiaryLine.Wait();

            ////Register Lines in NAV
            //Task<WSGenericCodeUnit.FxPostJobJrnlLines_Result> TRegisterNavDiaryLine = WSProjectDiaryLine.RegsiterNavDiaryLines(transactID, _configws);
            //TRegisterNavDiaryLine.Wait();

            //SET INTEGRATED IN DB
            //dp.ForEach(x =>
            //{
            //    DiárioDeProjeto newdp = new DiárioDeProjeto()
            //    {
            //        //Bit = true
            //    };

            //    DBProjectDiary.Update(newdp);
            //});

            return Json(dp);
        }


        public class ProjectInfo
        {
            public string ProjectNo { get; set; }
            public string ContabGroup { get; set; }
            public string Description { get; set; }
            public string RegionCode { get; set; }
            public string FuncAreaCode { get; set; }
            public string ResponsabilityCenter { get; set; }
        }
        #endregion
    }
}