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
using NJsonSchema;
using NJsonSchema.Generation;
using Manatee.Json.Schema;
using System.Text.RegularExpressions;

namespace Hydra.Such.Portal.Controllers
{
    //[Authorize]
    [Route("ordens-de-manutencao")]
    public class MaintenanceOrdersController : Controller
    {

        protected MaintnenceOrdersRepository maintnenceOrdersRepository;
        protected EvolutionWEBContext evolutionWEBContext;
        private readonly ISession session;

        public MaintenanceOrdersController(MaintnenceOrdersRepository repository, EvolutionWEBContext evolutionWEBContext, IOptions<NAVWSConfigurations> NAVWSConfigs, IHttpContextAccessor httpContextAccessor)
        {
            session = httpContextAccessor.HttpContext.Session;
            this.maintnenceOrdersRepository = repository;
            this.evolutionWEBContext = evolutionWEBContext;
        }

        //[Authorize]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        
        [Route("all"), HttpGet]
        public ActionResult GetAll (ODataQueryOptions<MaintenanceOrder> queryOptions, DateTime? from, DateTime? to, int idCliente)
        {
            if(from== null || to == null) {  return NotFound(); }
            
            var pageSize = 100;

            IQueryable results = queryOptions.ApplyTo(maintnenceOrdersRepository.AsQueryable().Where(o => o.OrderDate >= from && o.OrderDate <= to && o.IsToExecute), new ODataQuerySettings { PageSize = pageSize });
            var list = results.Cast<dynamic>().AsEnumerable();
            var total = Request.ODataFeature().TotalCount;
            var nextLink = Request.GetNextPageLink(pageSize);

            var result = new PageResult<dynamic>(list, nextLink, total);

            var query = maintnenceOrdersRepository.AsQueryable().Where(o => o.OrderDate >= from && o.OrderDate <= to && o.IdClienteEvolution == idCliente).Select(m => new { m.IsToExecute, m.IsPreventive }).ToList();

            var ordersCounts = new {
                preventive = query.Where(o => o.IsPreventive).Count(),
                preventiveToExecute = query.Where(n => n.IsPreventive && n.IsToExecute).Count(),
                curative = query.Where(o => !o.IsPreventive).Count(),
                curativeToExecute = query.Where(n => !n.IsPreventive && n.IsToExecute).Count(),
            };
          
            return Json(new {
                result,
                ordersCounts,
                range = new { from,to}
            }); 
        }



        [Route("{orderId}/tecnicals"), HttpGet]
        public ActionResult GetTecnicalls(string orderId)
        {
            var order = maintnenceOrdersRepository.AsQueryable().Where(t => t.No == orderId)
                .Select(o => new { o.IdTecnico1, o.IdTecnico2, o.IdTecnico3, o.IdTecnico4, o.IdTecnico5 }).FirstOrDefault();
           
            int?[] technicalsIds = new int?[5];
            technicalsIds[0] = order.IdTecnico1;
            technicalsIds[1] = order.IdTecnico2;
            technicalsIds[2] = order.IdTecnico3;
            technicalsIds[3] = order.IdTecnico4;
            technicalsIds[4] = order.IdTecnico5;

            var technicals = evolutionWEBContext.Utilizador.Where(u => technicalsIds.Contains(u.Id) && u.Activo == true)
                .Select(u=> new { u.Id, u.Nome, u.NivelAcesso, u.Code1, u.Code2, u.Code3, u.SuperiorHierarquico, u.ChefeProjecto, u.ResponsavelProjecto } ).ToList();

            return Json(technicals);
        }


        [Route("{orderId}/all/{technicalid}"), HttpPut]
        public ActionResult TecnicallsPut(string orderId, int technicalid)
        {

            return Json(new { });
        }


        [Route("{orderId}/all/{technicalid}"), HttpDelete]
        public ActionResult TecnicallsDelete(string orderId, int technicalid)
        {

            return Json(new { });
        }












        //[Authorize]
        // Todo adds custom authorize filter (eSuchAuthorizationFilter)  (info: Authentication -> Authorization)
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

        [Route("getSchema")]
        [ResponseCache(Duration = 600)]
        public async Task<string> GetSchema()
        {
            var _schema = await JsonSchema4.FromTypeAsync<OrdemManutencao>();

            var settings = new JsonSchemaGeneratorSettings();
            //settings.GenerateKnownTypes = false;
            //settings.SchemaType = SchemaType.OpenApi3;

            var generator = new JsonSchemaGenerator(settings);
            var schema = await generator.GenerateAsync(typeof(IOrdemManutencao));

            var json = Regex.Replace(schema.ToJson(), @"\s+", string.Empty);

            return json.Replace(",\"format\":\"decimal\"", string.Empty).Replace(",\"format\":\"byte\"", string.Empty).Replace(",\"format\":\"int32\"", string.Empty).Replace(",\"format\":\"time-span\"", string.Empty);
        }

    }

    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private static string[] Summaries = new[]
       {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
    }
}
