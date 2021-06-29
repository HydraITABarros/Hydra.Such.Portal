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
using Hydra.Such.Data.ViewModel.Approvals;
using static Hydra.Such.Data.Enumerations;

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

            List<ApprovalMovementsViewModel> result = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetAllAssignedToUserFilteredByStatus(User.Identity.Name, 1));

            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.Region));
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.FunctionalArea));
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.ResponsabilityCenter));

            int totalPendingApprovals = result != null ? result.Count : 0;
            this.session.SetString("totalPendingApprovals", totalPendingApprovals.ToString());
            return View();

            //var items = DBApprovalMovements.GetAllAssignedToUserFilteredByStatus(User.Identity.Name, 1);

            //int totalPendingApprovals = items != null ? items.Count : 0;
            //this.session.SetString("totalPendingApprovals", totalPendingApprovals.ToString());
            //return View();
        }

        [Authorize]
        public IActionResult Login()
        {

            return View();
        }

    }
}
