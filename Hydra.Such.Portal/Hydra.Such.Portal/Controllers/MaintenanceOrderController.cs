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
using Hydra.Such.Data.Evolution;
using Hydra.Such.Data.Evolution.Repositories;
using SharpRepository.Repository;
using Hydra.Such.Data.Evolution.Database;
using SharpRepository.Repository.Queries;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Extensions;

namespace Hydra.Such.Portal.Controllers
{
    //[Authorize]
    [Route("Manutencao")]
    public class MaintenanceOrderController : Controller
    {
        protected IRepository<MaintenanceOrder, string> repository;
        protected EvolutionWEBContext evolutionWEBContext;
        private readonly ISession session;

        public MaintenanceOrderController(IRepository<MaintenanceOrder, string> repository, EvolutionWEBContext evolutionWEBContext, IOptions<NAVWSConfigurations> NAVWSConfigs, IHttpContextAccessor httpContextAccessor)
        {
            session = httpContextAccessor.HttpContext.Session;
            this.repository = repository;
            this.evolutionWEBContext = evolutionWEBContext;
        }

        //[Authorize]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        //[Authorize]
        [Route("getAll")]
        public PageResult<dynamic> GetAll(ODataQueryOptions<MaintenanceOrder> queryOptions)
        {
            var pageSize = 100;
            IQueryable results = queryOptions.ApplyTo(repository.AsQueryable(), new ODataQuerySettings { PageSize = pageSize });
            var list = results.Cast<dynamic>().AsEnumerable();
            var total = Request.ODataFeature().TotalCount;
            var nextLink = Request.GetNextPageLink(pageSize);

            return new PageResult<dynamic>(list, nextLink, total);
        }

    }
}
