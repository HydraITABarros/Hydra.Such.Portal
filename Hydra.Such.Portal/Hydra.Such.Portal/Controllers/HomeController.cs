using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Projects;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.Logic;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Hydra.Such.Data.Logic.Approvals;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly NAVWSConfigurations _config;
        private readonly ISession session;

        public HomeController(IOptions<NAVWSConfigurations> NAVWSConfigs, IHttpContextAccessor httpContextAccessor)
        {
            this.session = httpContextAccessor.HttpContext.Session;
            _config = NAVWSConfigs.Value;
        }

        [Authorize]
        public IActionResult Index()
        {
            int totalPendingApprovals = DBApprovalMovements.GetAllAssignedToUserFilteredByStatus(User.Identity.Name, 1).Count;
            this.session.SetString("totalPendingApprovals", totalPendingApprovals.ToString());
            return View();
        }

        [Authorize]
        public IActionResult Login()
        {

            return View();
        }

    }
}
