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
using SharpRepository.Repository.Queries;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Extensions;
using NJsonSchema;
using NJsonSchema.Generation;
using Manatee.Json.Schema;
using System.Text.RegularExpressions;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data;
using Hydra.Such.Portal.Filters;
using Hydra.Such.Data.Evolution.DatabaseReference;
using StackExchange.Redis;

namespace Hydra.Such.Portal.Controllers
{
    [AllowAnonymous]
    [Route("ordens-de-manutencao")]
    public class MaintenanceOrdersController : Controller
    {
        protected MaintenanceOrdersRepository MaintenanceOrdersRepository;
        protected MaintenanceOrdersLineRepository MaintenanceOrdersLineRepository;
        protected EvolutionWEBContext evolutionWEBContext;
        private readonly ISession session;

        public MaintenanceOrdersController(
            MaintenanceOrdersRepository MaintenanceOrdersRepository, 
            MaintenanceOrdersLineRepository MaintenanceOrdersLineRepository, 
            EvolutionWEBContext evolutionWEBContext, IOptions<NAVWSConfigurations> NAVWSConfigs, 
            IHttpContextAccessor httpContextAccessor)
        {
            session = httpContextAccessor.HttpContext.Session;
            this.MaintenanceOrdersRepository = MaintenanceOrdersRepository;
            this.MaintenanceOrdersLineRepository = MaintenanceOrdersLineRepository;
            this.evolutionWEBContext = evolutionWEBContext;
        }

        [Route("{orderId}"), Route("{orderId}/ficha-de-manutencao"),
        Route(""), HttpGet, AcceptHeader("text/html")]
        [ResponseCache(Duration = 60000)]
        public IActionResult Index(string orderId)
        {            
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.MaintenanceOrders);
            UserConfigurationsViewModel userConfig = DBUserConfigurations.GetById(User.Identity.Name).ParseToViewModel();
            if (UPerm != null && UPerm.Read.Value)
            {
                return View("Index");
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        [Route("arquivo"), HttpGet, AcceptHeader("text/html")]
        public IActionResult Arquivo()
        {
            return Index("");
        }

        [Route(""), HttpGet, AcceptHeader("application/json")]
        [ResponseCache(Duration = 60000)]
        public ActionResult GetAll(ODataQueryOptions<MaintenanceOrder> queryOptions, DateTime? from, DateTime? to)
        {
            if (from == null || to == null) { return NotFound(); }

            var pageSize = 30;

            IQueryable results = queryOptions.ApplyTo(MaintenanceOrdersRepository.AsQueryable()
                .Where(o => o.OrderDate >= from && o.OrderDate <= to && !(o.DataFecho > new DateTime(1753, 1, 1)) &&
                (o.OrderType == "DMNALMP" || o.OrderType == "DMNCATE" || o.OrderType == "DMNLVMP" || o.OrderType == "DMNPRVE" || o.OrderType == "DMNCREE" || o.OrderType == "DMNDBI" || o.OrderType == "DMNORCE"))
                .OrderByDescending(o=>o.OrderDate), new ODataQuerySettings { PageSize = pageSize });

            var list = results.Cast<dynamic>().AsEnumerable();
            var total = Request.ODataFeature().TotalCount;
            var nextLink = Request.GetNextPageLink(pageSize);

            List<MaintenanceOrder> newList;
            try
            {
                newList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MaintenanceOrder>>(Newtonsoft.Json.JsonConvert.SerializeObject(list));
            }
            catch
            {
                newList = new List<MaintenanceOrder>();
            }
            newList = newList.Where(l => l.IsPreventive != null).ToList();
            newList.ForEach((item) =>
            {
                var technicals = GetTechnicals(item, null, null);
                if (technicals != null)
                {
                    item.Technicals = technicals.ToList();
                }
            });

            var result = new PageResult<dynamic>(newList, nextLink, total);

            var query = MaintenanceOrdersRepository.AsQueryable().Where(o => o.OrderDate >= from && o.OrderDate <= to
            && (o.OrderType == "DMNALMP" || o.OrderType == "DMNCATE" || o.OrderType == "DMNLVMP" || o.OrderType == "DMNPRVE" || o.OrderType == "DMNCREE" || o.OrderType == "DMNDBI" || o.OrderType == "DMNORCE"))
                .Select(m => new { m.IsToExecute, m.IsPreventive, m.OrderType, m.FinishingDate }).ToList();

            var ordersCounts = new
            {
                preventive = query.Where(o => o.IsPreventive == true && !o.IsToExecute).Count(),
                preventiveToExecute = query.Where(o => o.IsPreventive == true && o.IsToExecute).Count(),
                curative = query.Where(o => o.IsPreventive == false && !o.IsToExecute).Count(),
                curativeToExecute = query.Where(o => o.IsPreventive == false && o.IsToExecute).Count(),
            };

            return Json(new
            {
                result,
                ordersCounts,
                range = new { from, to }
            });
        }


        [Route("technicals"), HttpGet]
        public ActionResult HttpGetTecnicalls(string orderId, string technicalid)
        {
            if ((orderId == null || orderId == "") && (technicalid == null || technicalid == "") ) { return NotFound(); }
            return Json(new { technicals = GetTechnicals( null, orderId, technicalid).OrderBy(o=>o.Nome) });
        }


        private IQueryable<Utilizador> GetTechnicals(MaintenanceOrder order, string orderId, string technicalid) {

            if ((order== null ) && (orderId == null || orderId == "") && (technicalid == null || technicalid == "")) { return (new List<Utilizador>()).AsQueryable(); }

            IQueryable<Utilizador> technicals;
            if (technicalid != null && technicalid != "")
            {
                technicals = evolutionWEBContext.Utilizador.Where(u => u.NumMec == technicalid);
                return technicals;
            }

            if (order != null)
            {
                var technicalsId = new List<int>();

                for (int i = 1; i <= 5; i++)
                {
                    var prop = order.GetType().GetProperty("IdTecnico" + i.ToString());
                    int? name = (int?)(prop.GetValue(order, null));
                    if (name != null)
                    {
                        technicalsId.Add((int)name);
                    }
                }

                technicals = evolutionWEBContext.Utilizador.Where(u => technicalsId.Contains(u.Id));
                return technicals;
            }

            if (orderId != null && orderId!= "")
            {
                var _order = evolutionWEBContext.MaintenanceOrder.FirstOrDefault(o => o.No == orderId);
                if(_order!= null)
                {
                    technicals = evolutionWEBContext.Utilizador.Where(u => u.Code3 == _order.ShortcutDimension3Code);
                    return technicals;
                }
            }
            technicals = (new List<Utilizador>()).AsQueryable();
            return technicals;
        }



        [Route("technicals"), HttpPut]
        public ActionResult TecnicallsPut([FromBody] UpdateTechnicalsModel data)
        {
            if (data.orderId == null || data.orderId == "" || data.technicalsId == null) { return NotFound(); }

            var orderToUpdate = MaintenanceOrdersRepository.AsQueryable().Where(m => m.No == data.orderId).FirstOrDefault();

            if (orderToUpdate == null) { return NotFound(); }

            var technicalsToUpdate = evolutionWEBContext.Utilizador.Where(u => data.technicalsId.Contains(u.NumMec)).ToArray();

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


        public class UpdateTechnicalsModel
        {
            public string orderId;
            public string[] technicalsId;
        }



        //[AllowAnonymous]
        [Route("{orderId}"), HttpGet]
        [ResponseCache(Duration = 60000)]
        public ActionResult GetDetails(string orderId, ODataQueryOptions<Equipamento> queryOptions)
        {
            if (orderId == null) { return NotFound(); }

            var pageSize = 30;
            var order = evolutionWEBContext.MaintenanceOrder.AsQueryable().Where(o => o.No == orderId).Select(o => new OmHeaderViewModel() {
                Description = o.Description,
                IdClienteEvolution = o.IdClienteEvolution,
                IdInstituicaoEvolution = o.IdInstituicaoEvolution,
                CustomerName = o.CustomerName,
                NomeInstituicao = ""
            } ).FirstOrDefault();

            if (order == null) { return NotFound(); }

            var instituicao = evolutionWEBContext.Instituicao.FirstOrDefault(i=>i.IdInstituicao == order.IdInstituicaoEvolution);
            var cliente = evolutionWEBContext.Cliente.FirstOrDefault(i=>i.IdCliente == order.IdClienteEvolution);
            if(cliente != null)
            {
                order.CustomerName = cliente.Nome;
            }

            if (instituicao != null)
            {
                order.NomeInstituicao = instituicao.Nome;
            }

            IQueryable results;
            results = queryOptions.ApplyTo(evolutionWEBContext.Equipamento.Select(e => new Equipamento() {
                Nome = e.Nome, Marca = e.Marca, IdEquipamento = e.IdEquipamento,
                Categoria = e.Categoria, NumSerie = e.NumSerie, NumInventario = e.NumInventario, NumEquipamento = e.NumEquipamento
            }), new ODataQuerySettings { PageSize = pageSize });

            var list = results.Cast<dynamic>().AsEnumerable();
            var total = Request.ODataFeature().TotalCount;
            var nextLink = Request.GetNextPageLink(pageSize);

            List<Equipamento> newList;
            try
            {
                newList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Equipamento>>(Newtonsoft.Json.JsonConvert.SerializeObject(list));
            }
            catch
            {
                newList = new List<Equipamento>();
            }

            newList.ForEach((item) => {
                var categoria = evolutionWEBContext.EquipCategoria.FirstOrDefault(m => m.IdCategoria == item.Categoria);
                var marca = evolutionWEBContext.EquipMarca.FirstOrDefault(m => m.IdMarca == item.Marca);
                item.CategoriaText = categoria != null ? categoria.Nome : "";
                item.MarcaText = marca != null ? marca.Nome : "";
            });

            var resultLines = new PageResult<dynamic>(newList, nextLink, total);


            var ordersCountsLines = new
            {
                toExecute = evolutionWEBContext.Equipamento.Count(),
                toSigning = 0,
                executed = 0
            };

            return Json(new
            {
                order,
                resultLines,
                ordersCountsLines,
            });
        }

        public class OmHeaderViewModel  {
            public string Description;
            public int? IdClienteEvolution;
            public int? IdInstituicaoEvolution;
            public string CustomerName;
            public string NomeInstituicao;
        }

        //[Authorize]
        // Todo adds custom authorize filter (eSuchAuthorizationFilter)  (info: Authentication -> Authorization)
        [Route("getAll")]
        public PageResult<dynamic> GetAll(ODataQueryOptions<MaintenanceOrder> queryOptions)
        {
            var pageSize = 100;
            IQueryable results = queryOptions.ApplyTo(MaintenanceOrdersRepository.AsQueryable(), new ODataQuerySettings { PageSize = pageSize });
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
}
