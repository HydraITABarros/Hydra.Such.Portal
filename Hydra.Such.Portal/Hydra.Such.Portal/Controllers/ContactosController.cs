using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class ContactosController : Controller
    {
        // GET: Contactos
        public ActionResult Index()
        {
            return View();
        }

        [Route("Contactos/Detalhes/{id}")]
        public ActionResult Details(string id)
        {
            ViewBag.Id = id;
            return View();
        }
        
        [HttpPost]
        public JsonResult GetById(string id)
        {
            ContactViewModel result = DBContacts.GetById(id).ParseToViewModel();
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteContact(string id)
        {
            return Json(DBContacts.Delete(id));
        }

        [HttpPost]
        public JsonResult GetContacts()
        {
            List<ContactViewModel> result = DBContacts.GetAll().ParseToViewModel();
            return Json(result);
        }
    }
}