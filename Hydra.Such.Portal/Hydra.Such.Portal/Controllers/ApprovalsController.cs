using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Logic.Approvals;
using Hydra.Such.Data.ViewModel.Approvals;
using Hydra.Such.Portal.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Such.Portal.Controllers
{
    public class ApprovalsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetListApprovals()
        {
            List<ApprovalMovementsViewModel> result = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetAllAssignedToUserFilteredByStatus(User.Identity.Name,1));

            result.ForEach(x => {
                x.TypeText = EnumerablesFixed.ApprovalTypes.Where(y => y.Id == x.Type).FirstOrDefault().Value;
            });
            return Json(result);
        }
    }
}