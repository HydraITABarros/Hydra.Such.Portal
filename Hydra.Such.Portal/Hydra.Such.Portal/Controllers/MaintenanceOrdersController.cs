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

            IQueryable results = queryOptions.ApplyTo(maintnenceOrdersRepository.AsQueryable().Where(o => o.OrderDate >= from && o.OrderDate <= to), new ODataQuerySettings { PageSize = pageSize });
            var list = results.Cast<dynamic>().AsEnumerable();
            var total = Request.ODataFeature().TotalCount;
            var nextLink = Request.GetNextPageLink(pageSize);

            var result = new PageResult<dynamic>(list, nextLink, total);  

            var query = maintnenceOrdersRepository.AsQueryable().Where(o => o.OrderDate >= from && o.OrderDate <= to)
                .Select(m => new {m.IsToExecute, m.IsPreventive }).ToList();

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

        
        [Route("technicals"), HttpGet]
        public ActionResult GetTecnicalls(string local, string orderId, string technicalid)
        {
           if ((local == null || local == "") && (orderId == null|| orderId == "") && (technicalid == null || technicalid == "")) { return NotFound(); }

           IQueryable<Utilizador> technicals;
           if (technicalid != null && technicalid != "" )
           {
                technicals = evolutionWEBContext.Utilizador.Where(u => u.NumMec == technicalid);
                return Json(new { technicals });
           }

           if (orderId != null && orderId != "")
           {
                var order = maintnenceOrdersRepository.AsQueryable().Where(m => m.No == orderId).FirstOrDefault();
                var technicalsId = new List<int>();

                for (int i = 1; i <= 5; i++)
                {
                    var prop = order.GetType().GetProperty("IdTecnico" + i.ToString());
                    int? name = (int?)(prop.GetValue(order, null));
                    if(name != null)
                    {
                        technicalsId.Add((int)name);
                    }
                }

                technicals = evolutionWEBContext.Utilizador.Where(u => technicalsId.Contains(u.Id));
                return Json(new { technicals });
            }

            technicals = evolutionWEBContext.Utilizador.Where(a => a.Code1 == local);
            return Json(new { technicals });
        }
        


        [Route("technicals"), HttpPut]
        public ActionResult TecnicallsPut(string orderId, string[] technicalsid)
        {
            if (orderId == null || orderId == "" || technicalsid == null) { return NotFound(); }

            var orderToUpdate = maintnenceOrdersRepository.AsQueryable().Where(m => m.No == orderId).FirstOrDefault();

            if (orderToUpdate == null) { return NotFound(); }

            var technicalsToUpdate = evolutionWEBContext.Utilizador.Where(u => technicalsid.Contains(u.NumMec)).ToArray();

            orderToUpdate.IdTecnico1 = technicalsToUpdate.Count() > 0 ? (int?)technicalsToUpdate[0].Id : null;
            orderToUpdate.IdTecnico2 = technicalsToUpdate.Count() > 1 ? (int?)technicalsToUpdate[1].Id : null;
            orderToUpdate.IdTecnico3 = technicalsToUpdate.Count() > 2 ? (int?)technicalsToUpdate[2].Id : null;
            orderToUpdate.IdTecnico4 = technicalsToUpdate.Count() > 3 ? (int?)technicalsToUpdate[3].Id : null;
            orderToUpdate.IdTecnico5 = technicalsToUpdate.Count() > 4 ? (int?)technicalsToUpdate[4].Id : null;

            try
            {
                evolutionWEBContext.Update(orderToUpdate);
                evolutionWEBContext.SaveChanges();
            }
            catch (Exception)
            {
                return Json(0);
            }
            
            
            return Json(orderToUpdate);
        }



        //[Authorize]
        // Todo adds custom authorize filter (eSuchAuthorizationFilter)  (info: Authentication -> Authorization)
        [Route("getAll")]
        public PageResult<dynamic> GetAll(ODataQueryOptions<MaintenanceOrder> queryOptions)
        {
            var pageSize = 100;
            IQueryable results = queryOptions.ApplyTo(maintnenceOrdersRepository.AsQueryable(), new ODataQuerySettings { PageSize = pageSize });
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
