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
using Hydra.Such.Portal.ViewModels;
using System.Reflection;

namespace Hydra.Such.Portal.Controllers
{
    
    [Authorize]
    [Route("ordens-de-manutencao")]
    public class MaintenanceOrdersController : Controller
    {
        protected MaintenanceOrdersRepository MaintenanceOrdersRepository;
        protected MaintenanceOrdersLineRepository MaintenanceOrdersLineRepository;
        protected EvolutionWEBContext evolutionWEBContext;
        protected SuchDBContext suchDBContext;
        private readonly ISession session;

        public MaintenanceOrdersController(
            MaintenanceOrdersRepository MaintenanceOrdersRepository,
            MaintenanceOrdersLineRepository MaintenanceOrdersLineRepository,
            SuchDBContext suchDBContext,
            EvolutionWEBContext evolutionWEBContext, IOptions<NAVWSConfigurations> NAVWSConfigs,
            IHttpContextAccessor httpContextAccessor)
        {
            session = httpContextAccessor.HttpContext.Session;
            this.MaintenanceOrdersRepository = MaintenanceOrdersRepository;
            this.MaintenanceOrdersLineRepository = MaintenanceOrdersLineRepository;
            this.evolutionWEBContext = evolutionWEBContext;
            this.suchDBContext = suchDBContext;
        }

        [Route(""), HttpGet, AcceptHeader("text/html")]
        //[ResponseCache(Duration = 60000)]
        public IActionResult Index()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.MaintenanceOrders);
            UserConfigurationsViewModel userConfig = DBUserConfigurations.GetById(User.Identity.Name).ParseToViewModel();
            if (UPerm != null && UPerm.Read.Value)
            {
                return View("Index");
            }
            return RedirectToAction("AccessDenied", "Error");
        }
      
        [Route("{orderId}"), HttpGet, AcceptHeader("text/html")]
        public IActionResult OrdemDeManutencao(string orderId)
        {
            return Index();
        }
   
        [Route("{orderId}/ficha-de-manutencao"), HttpGet, AcceptHeader("text/html")]
        public IActionResult FichaDeManutencao(string orderId)
        {
            return Index();
        }
       
        [Route("arquivo"), HttpGet, AcceptHeader("text/html")]
        public IActionResult Arquivo()
        {
            return Index();
        }

       
        [Route(""), HttpGet, AcceptHeader("application/json")]
        //[ResponseCache(Duration = 60000)]
        public ActionResult GetAll(ODataQueryOptions<MaintenanceOrder> queryOptions)
        {
            var pageSize = 30;

            var preventiveCodes = evolutionWEBContext.MaintenanceCatalog.Where(f => f.ManutPreventiva == 1).Select(f => f.Code).ToList();
            var curativeCodes = evolutionWEBContext.MaintenanceCatalog.Where(f => f.ManutCorrectiva == 1).Select(f => f.Code).ToList();
            var loggedUser = suchDBContext.AcessosUtilizador.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

            if (loggedUser == null) { return NotFound(); }

            var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name);

            if (evolutionLoggedUser == null) { return NotFound(); }

            var nivelAcesso = evolutionLoggedUser.NivelAcesso;

            IQueryable results;

            IQueryable<MaintenanceOrder> query = MaintenanceOrdersRepository.AsQueryable()
                .Where(o =>
                (preventiveCodes.Contains(o.OrderType) || curativeCodes.Contains(o.OrderType)) &&
                o.Status == 0 && (o.IdClienteEvolution != null || o.IdInstituicaoEvolution != null));

            var preventives = MaintenanceOrdersRepository.AsQueryable().Where(o =>
                   preventiveCodes.Contains(o.OrderType) && o.Status == 0 &&
                (o.IdClienteEvolution != null || o.IdInstituicaoEvolution != null));

            var curatives = MaintenanceOrdersRepository.AsQueryable().Where(o =>
                   curativeCodes.Contains(o.OrderType) && o.Status == 0 &&
                (o.IdClienteEvolution != null || o.IdInstituicaoEvolution != null));

            /*
             * filter MaintenanceOrder query based on user permissions
             * 1, 2, 8 = Podem ver tudo
             * 3, 4 = Filtrado por regiao
             * 5, 6, 7 = Filtrado por Cresp
             */
            switch (nivelAcesso)
            {
                case 3:
                case 4:
                    preventives = preventives.Where(q => q.ShortcutDimension1Code == evolutionLoggedUser.Code1);
                    curatives = curatives.Where(q => q.ShortcutDimension1Code == evolutionLoggedUser.Code1);
                    query = query.Where(q => q.ShortcutDimension1Code == evolutionLoggedUser.Code1);
                    break;
                case 5:
                case 6:
                case 7:
                    preventives = preventives.Where(q => q.ShortcutDimension3Code == evolutionLoggedUser.Code3);
                    curatives = curatives.Where(q => q.ShortcutDimension3Code == evolutionLoggedUser.Code3);
                    query = query.Where(q => q.ShortcutDimension3Code == evolutionLoggedUser.Code3);
                    break;
                default:
                    break;
            }

            results = queryOptions.ApplyTo(query.Select(o => new MaintenanceOrder
                {
                    No = o.No,
                    Description = o.Description,
                    IdClienteEvolution = o.IdClienteEvolution,
                    IdInstituicaoEvolution = o.IdInstituicaoEvolution,
                    IdServicoEvolution = o.IdServicoEvolution,
                    OrderDate = o.OrderDate,
                    ClientName = o.ClientName,
                    InstitutionName = o.InstitutionName,
                    ServiceName = o.ServiceName,
                    OrderType = o.OrderType
                }), new ODataQuerySettings { PageSize = pageSize });
            
            var list = results.Cast<dynamic>().AsEnumerable();
            long? total = Request.ODataFeature().TotalCount;
            var nextLink = Request.GetNextPageLink(pageSize);

            List<MaintenanceOrderViewModel> newList;
            try
            {
                newList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MaintenanceOrderViewModel>>(Newtonsoft.Json.JsonConvert.SerializeObject(list));
            }
            catch
            {
                newList = new List<MaintenanceOrderViewModel>();
            }
           
            newList.ForEach((item) =>
            {
                var technicals = GetTechnicals(item, null, null);
                if (technicals != null) { item.Technicals = technicals.ToList(); }

                var client = evolutionWEBContext.Cliente.FirstOrDefault(c => c.IdCliente == item.IdClienteEvolution);
                if (client != null) { item.ClientName = client.Nome; }

                var institution = evolutionWEBContext.Instituicao.FirstOrDefault(i => i.IdInstituicao == item.IdInstituicaoEvolution);
                if (institution != null) { item.InstitutionName = institution.DescricaoTreePath; }

                var service = evolutionWEBContext.Servico.FirstOrDefault(s => s.IdServico == item.IdServicoEvolution);
                if (service != null) { item.ServiceName = service.Nome; }

                var date = item.OrderDate;

                item.isPreventive = preventiveCodes.Contains(item.OrderType);


                var orderCurative = curativeCodes.Contains(item.OrderType);
                var orderPreventive = preventiveCodes.Contains(item.OrderType);
                if (orderCurative)
                {
                    item.havePreventive = evolutionWEBContext.MaintenanceOrder.Where(p => preventiveCodes.Contains(p.OrderType)
                    && item.IdServicoEvolution == p.IdServicoEvolution).Select(p => p.No).FirstOrDefault();
                    Console.WriteLine(item.havePreventive);
                }
           
            });

            var result = new PageResult<dynamic>(newList, nextLink, total);
                        
            return Json(new
            {
                result,
                ordersCounts = new { preventive = preventives.Count(), curative = curatives.Count() }
            });
        }


        
        [Route("technicals"), HttpGet]
        private IQueryable<Utilizador> GetTechnicals(MaintenanceOrderViewModel order, string orderId, string technicalid)
        {
            if ((order == null) && (orderId == null || orderId == "") && (technicalid == null || technicalid == "")) { return (new List<Utilizador>()).AsQueryable(); }

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

            if (orderId != null && orderId != "")
            {
                var _order = evolutionWEBContext.MaintenanceOrder.FirstOrDefault(o => o.No == orderId);
                if (_order != null)
                {
                    technicals = evolutionWEBContext.Utilizador.Where(u => u.Code3 == _order.ShortcutDimension3Code);
                    return technicals;
                }
            }
            technicals = (new List<Utilizador>()).AsQueryable();
            return technicals;
        }


        
        [Route("{orderId}/technical"), HttpPost]
        public ActionResult AddTechnicalToOrder(string orderId)
        {
            if (orderId == null) { return NotFound(); }

            var order = evolutionWEBContext.MaintenanceOrder.FirstOrDefault(o => o.No == orderId);

            if (order == null) { return NotFound(); }

            var loggedUser = suchDBContext.AcessosUtilizador.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

            if (loggedUser == null) { return NotFound(); }

            var loggedUserTechnical = suchDBContext.AcessosDimensões.Where(o => o.Dimensão == 5 && o.Dimensão == 6 && o.Dimensão == 7 && o.IdUtilizador == loggedUser.IdUtilizador).ToList();

            var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name);

            if (evolutionLoggedUser == null) { return NotFound(); }

            if (evolutionLoggedUser.NivelAcesso == 6 || evolutionLoggedUser.NivelAcesso == 7)
            {
                if (order.IdTecnico1 == evolutionLoggedUser.Id || order.IdTecnico2 == evolutionLoggedUser.Id ||
                order.IdTecnico3 == evolutionLoggedUser.Id || order.IdTecnico4 == evolutionLoggedUser.Id ||
                order.IdTecnico5 == evolutionLoggedUser.Id) { return Json(true); }

                if (order.IdTecnico1 == null) { order.IdTecnico1 = evolutionLoggedUser.Id; }

                else if (order.IdTecnico2 == null) { order.IdTecnico2 = evolutionLoggedUser.Id; }

                else if (order.IdTecnico3 == null) { order.IdTecnico3 = evolutionLoggedUser.Id; }

                else if (order.IdTecnico4 == null) { order.IdTecnico4 = evolutionLoggedUser.Id; }

                else { order.IdTecnico5 = evolutionLoggedUser.Id; }

                try
                {
                    evolutionWEBContext.Update(order);
                    evolutionWEBContext.SaveChanges();
                }
                catch (Exception)
                {
                    return Json(false);
                }
                return Json(true);
            }
            return Json(false);
        }

             
        [Route("{orderId}"), HttpGet, AcceptHeader("application/json")]
        //[ResponseCache(Duration = 60000)]
        public ActionResult GetDetails(string orderId, ODataQueryOptions<Equipamento> queryOptions)
        {
            var orderType = evolutionWEBContext.MaintenanceOrder.Where(o => o.No == orderId).FirstOrDefault();
            var preventiveCodes = evolutionWEBContext.MaintenanceCatalog.Where(f => f.ManutPreventiva == 1).Select(f => f.Code).ToList();
            var curativeCodes = evolutionWEBContext.MaintenanceCatalog.Where(f => f.ManutCorrectiva == 1).Select(f => f.Code).ToList();
            var orderPreventive = preventiveCodes.Contains(orderType.OrderType);
            var orderCurative = curativeCodes.Contains(orderType.OrderType);

            if (orderCurative)
            {
                return Json(false);
            }

            if (orderId == null) { return NotFound(); }

            var pageSize = 30;
            var order = evolutionWEBContext.MaintenanceOrder.AsQueryable().Where(o => o.No == orderId).Select(o => new OmHeaderViewModel()
            {
                Description = o.Description,
                IdClienteEvolution = o.IdClienteEvolution,
                IdInstituicaoEvolution = o.IdInstituicaoEvolution,
                CustomerName = o.CustomerName,
                NomeInstituicao = "",
                ShortcutDimension1Code = o.ShortcutDimension1Code,
                IdRegiao = 0,
                Contrato = o.ContractNo
            }).FirstOrDefault();

            if (order == null) { return NotFound(); }

            var instituicao = evolutionWEBContext.Instituicao.FirstOrDefault(i => i.IdInstituicao == order.IdInstituicaoEvolution);
            var cliente = evolutionWEBContext.Cliente.FirstOrDefault(i => i.IdCliente == order.IdClienteEvolution);
            if (cliente != null)
            {
                order.CustomerName = cliente.Nome;
            }

            if (instituicao != null)
            {
                order.NomeInstituicao = instituicao.Nome;
            }
            int.TryParse(order.ShortcutDimension1Code, out order.IdRegiao);

            IQueryable results;
            results = queryOptions.ApplyTo(evolutionWEBContext.Equipamento.OrderByDescending(o => o.IdEquipamento).Select(e => new Equipamento
            {
                Nome = e.Nome,
                Marca = e.Marca,
                IdEquipamento = e.IdEquipamento,
                Categoria = e.Categoria,
                NumSerie = e.NumSerie,
                NumInventario = e.NumInventario,
                IdServico = e.IdServico,
                NumEquipamento = e.NumEquipamento,
                IdRegiao = e.IdRegiao
            }).Where(e => true/*e.IdServico == 7231*//*&& e.NumEquipamento == "ni1102821"*/), new ODataQuerySettings { PageSize = pageSize });

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

            var marcas = evolutionWEBContext.EquipMarca.Where(m => m.Activo == true).Select(m => new
            {
                m.IdMarca,
                m.Nome
            }).ToList();
            var servicos = evolutionWEBContext.Servico.Where(s => s.Activo == true).Select(s => new
            {
                s.IdServico,
                s.Nome
            }).ToList();

            newList.ForEach((item) =>
            {
                var categoria = evolutionWEBContext.EquipCategoria.FirstOrDefault(m => m.IdCategoria == item.Categoria);
                var marca = marcas.FirstOrDefault(m => m.IdMarca == item.Marca);
                var servico = servicos.FirstOrDefault(m => m.IdServico == item.IdServico);
                item.CategoriaText = categoria != null ? categoria.Nome : "";
                item.MarcaText = marca != null ? marca.Nome : "";
                item.ServicoText = servico != null ? servico.Nome : "";
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
                marcas,
                servicos
            });
        }




        //[Route("maintenanceSheet"), HttpGet]
        ////[ResponseCache(Duration = 60000)]
        //public ActionResult GetMaintenanceSheet(string orderId, ODataQueryOptions<Equipamento> queryOptions)
        //{
        //}




        public class OmHeaderViewModel
        {
            public string Description;
            public int? IdClienteEvolution;
            public int? IdInstituicaoEvolution;
            public int IdRegiao;
            public string ShortcutDimension1Code;
            public string CustomerName;
            public string NomeInstituicao;
            public string Contrato;
        }


    }
}
