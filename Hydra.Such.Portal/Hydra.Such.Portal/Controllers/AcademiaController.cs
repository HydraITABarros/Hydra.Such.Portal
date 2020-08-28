using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Hydra.Such.Data.Extensions;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data;

namespace Hydra.Such.Portal.Controllers
{
    public class AcademiaController : Controller
    {
        //private const int _UserCA = (int)Enumerations.Features.MembroCA;
        //private const int _UserAdminAcademia = (int)Enumerations.Features.AcademiaAdmin;
        //private const int _UserAcademia = (int)Enumerations.Features.AcademiaBase;
        //private const int _UserChefia = (int)Enumerations.Features.AcademiaChefia;
        //private const int _UserDirector = (int)Enumerations.Features.AcademiaDirector;

        // GET: Academia
        public ActionResult Index()
        {
            return View();
        }

        // GET: Academia/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Academia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Academia/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Academia/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Academia/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Academia/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Academia/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}