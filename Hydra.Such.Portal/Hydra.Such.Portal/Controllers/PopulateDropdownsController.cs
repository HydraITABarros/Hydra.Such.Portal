using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.Logic;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.Logic.Project;

namespace Hydra.Such.Portal.Controllers
{
    public class PopulateDropdownsController : Controller
    {
        [HttpPost]
        public JsonResult GetNumerations()
        {
            List<DDMessage> result = DBNumerationConfigurations.GetAll().Select(x => new DDMessage()
            {
                id = x.Id,
                value = x.Descrição
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetAreas()
        {
            List<EnumData> result = EnumerablesFixed.Areas;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetFeatures()
        {
            List<EnumData> result = EnumerablesFixed.Features;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProjectStatus()
        {
            List<EnumData> result = EnumerablesFixed.ProjectStatus;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProjectCategories()
        {
            List<EnumData> result = EnumerablesFixed.ProjectCategories;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProjectTypes()
        {
            List<DDMessage> result = DBProjectTypes.GetAll().Select(x => new DDMessage()
            {
                id = x.Código,
                value = x.Descrição
            }).ToList(); ;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetUserProfileModels()
        {
            List<DDMessage> result = DBProfileModels.GetAll().Select(x => new DDMessage()
            {
                id = x.Id,
                value = x.Descrição
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetRegionCode()
        {
            List<DDMessage> Region = new List<DDMessage>(){
                new DDMessage()
                {
                    id = 1,
                    value = "Minho"
                },

                new DDMessage()
                {
                    id = 2,
                    value = "Douro Litoral"
                },

                new DDMessage()
                {
                    id = 3,
                    value = "Algarve"
                },
            };

            return Json(Region);
        }

        [HttpPost]
        public JsonResult GetFunctionalAreaCode()
        {
            List<DDMessage> FunctionalArea = new List<DDMessage>(){
                new DDMessage()
                {
                    id = 1,
                    value = "Area 1"
                },

                new DDMessage()
                {
                    id = 2,
                    value = "Area 2"
                },

                new DDMessage()
                {
                    id = 3,
                    value = "Area 3"
                },
            };

            return Json(FunctionalArea);
        }

        [HttpPost]
        public JsonResult GetResponsabilityCenterCode()
        {
            List<DDMessage> ResponsabilityCenter = new List<DDMessage>(){
                new DDMessage()
                {
                    id = 1,
                    value = "Responsabilidade 1"
                },

                new DDMessage()
                {
                    id = 2,
                    value = "Responsabilidade 2"
                },

                new DDMessage()
                {
                    id = 3,
                    value = "Responsabilidade 3"
                },
            };

            return Json(ResponsabilityCenter);
        }


        [HttpPost]
        public JsonResult GetMoveType()
        {
            List<DDMessage> ResponsabilityCenter = new List<DDMessage>(){
                new DDMessage()
                {
                    id = 1,
                    value = "Consumo"
                },

                new DDMessage()
                {
                    id = 2,
                    value = "Venda"
                },
            };

            return Json(ResponsabilityCenter);
        }

        [HttpPost]
        public JsonResult GetProjectType()
        {
            List<DDMessage> ResponsabilityCenter = new List<DDMessage>(){
                new DDMessage()
                {
                    id = 1,
                    value = "Recurso"
                },

                new DDMessage()
                {
                    id = 2,
                    value = "Produto"
                },

                new DDMessage()
                {
                    id = 3,
                    value = "Conta CG"
                },
            };

            return Json(ResponsabilityCenter);
        }

        [HttpPost]
        public JsonResult GetProjectList()
        {
            List<DDMessageString> result = DBProjects.GetAll().Select(x => new DDMessageString()
            {
                id = x.NºProjeto,
                value = x.NºProjeto
            }).ToList();

            return Json(result);
        }
        [HttpPost]
        public JsonResult GetAreaCode()
        {
            List<DDMessageString> Area = new List<DDMessageString>(){
                new DDMessageString()
                {
                    id = "1",
                    value = "Code 1"
                },

                new DDMessageString()
                {
                    id = "2",
                    value = "Code 2"
                },

                new DDMessageString()
                {
                    id = "3",
                    value = "Code 3"
                },
            };

            return Json(Area);
        }
    }

    public class DDMessage
    {
        public int id { get; set; }
        public string value { get; set; }
    }

    public class DDMessageString
    {
        public string id { get; set; }
        public string value { get; set; }
    }
}