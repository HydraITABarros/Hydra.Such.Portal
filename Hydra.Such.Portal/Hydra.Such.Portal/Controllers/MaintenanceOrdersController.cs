﻿using System;
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

    //[Authorize]
    [AllowAnonymous]
    [Route("ordens-de-manutencao")]
    public class MaintenanceOrdersController : Controller
    {
        protected MaintenanceOrdersRepository MaintenanceOrdersRepository;
        protected MaintenanceOrdersLineRepository MaintenanceOrdersLineRepository;
        protected EquipamentoRepository EquipamentoRepository;
        protected EvolutionWEBContext evolutionWEBContext;
        protected SuchDBContext suchDBContext;
        private readonly ISession session;

        public MaintenanceOrdersController(
            MaintenanceOrdersRepository MaintenanceOrdersRepository,
            MaintenanceOrdersLineRepository MaintenanceOrdersLineRepository,
            EquipamentoRepository EquipamentoRepository,
            SuchDBContext suchDBContext,
            EvolutionWEBContext evolutionWEBContext, IOptions<NAVWSConfigurations> NAVWSConfigs,
            IHttpContextAccessor httpContextAccessor)
        {
            session = httpContextAccessor.HttpContext.Session;
            this.MaintenanceOrdersRepository = MaintenanceOrdersRepository;
            this.MaintenanceOrdersLineRepository = MaintenanceOrdersLineRepository;
            this.EquipamentoRepository = EquipamentoRepository;
            this.evolutionWEBContext = evolutionWEBContext;
            this.suchDBContext = suchDBContext;
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        [Route(""), HttpGet, AcceptHeader("application/json")]
        //[ResponseCache(Duration = 60000)]
        public ActionResult GetAll(ODataQueryOptions<MaintenanceOrder> queryOptions)
        {
            var pageSize = 30;

            var preventiveCodes = evolutionWEBContext.MaintenanceCatalog.Where(f => f.ManutPreventiva == 1).Select(f => f.Code).ToList();
            var curativeCodes = evolutionWEBContext.MaintenanceCatalog.Where(f => f.ManutCorrectiva == 1).Select(f => f.Code).ToList();
            var loggedUser = suchDBContext.AcessosUtilizador.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

            //if (loggedUser == null) { return NotFound(); }

            //var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name);

            //if (evolutionLoggedUser == null) { return NotFound(); }

            //var nivelAcesso = evolutionLoggedUser.NivelAcesso;

            IQueryable results;

            IQueryable<MaintenanceOrder> query = MaintenanceOrdersRepository.AsQueryable()
                .Where(o =>
                (preventiveCodes.Contains(o.OrderType) || curativeCodes.Contains(o.OrderType)) &&
                o.Status == 0 && (o.IdClienteEvolution != null && o.IdInstituicaoEvolution != null));

            var preventives = MaintenanceOrdersRepository.AsQueryable().Where(o =>
                   preventiveCodes.Contains(o.OrderType) && o.Status == 0 &&
                (o.IdClienteEvolution != null && o.IdInstituicaoEvolution != null));

            var curatives = MaintenanceOrdersRepository.AsQueryable().Where(o =>
                   curativeCodes.Contains(o.OrderType) && o.Status == 0 &&
                (o.IdClienteEvolution != null && o.IdInstituicaoEvolution != null));

            /*
             * filter MaintenanceOrder query based on user permissions
             * 1, 2, 8 = Podem ver tudo
             * 3, 4 = Filtrado por regiao
             * 5, 6, 7 = Filtrado por Cresp
             */
            //switch (nivelAcesso)
            //{
            //    case 3:
            //    case 4:
            //        preventives = preventives.Where(q => q.ShortcutDimension1Code == evolutionLoggedUser.Code1);
            //        curatives = curatives.Where(q => q.ShortcutDimension1Code == evolutionLoggedUser.Code1);
            //        query = query.Where(q => q.ShortcutDimension1Code == evolutionLoggedUser.Code1);
            //        break;
            //    case 5:
            //    case 6:
            //    case 7:
            //        preventives = preventives.Where(q => q.ShortcutDimension3Code == evolutionLoggedUser.Code3);
            //        curatives = curatives.Where(q => q.ShortcutDimension3Code == evolutionLoggedUser.Code3);
            //        query = query.Where(q => q.ShortcutDimension3Code == evolutionLoggedUser.Code3);
            //        break;
            //    default:
            //        break;
            //}

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
                    && item.IdServicoEvolution == p.IdServicoEvolution && p.Status == 0).Select(p => p.No).FirstOrDefault();
                }

            });

            var result = new PageResult<dynamic>(newList, nextLink, total);

            return Json(new
            {
                result,
                ordersCounts = new { preventive = preventives.Count(), curative = curatives.Count() }
            });
        }


        [AllowAnonymous]

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


        [AllowAnonymous]

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

        [AllowAnonymous]

        [Route("{orderId}"), HttpGet, AcceptHeader("application/json")]
        //[ResponseCache(Duration = 60000)]
        public ActionResult GetOrderDetails(string orderId, ODataQueryOptions<Equipamento> queryOptions)
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
                No = o.No,
                Description = o.Description,
                IdClienteEvolution = o.IdClienteEvolution,
                IdInstituicaoEvolution = o.IdInstituicaoEvolution,
                ContractNo = o.ContractNo,
                ClientName = o.CustomerName,
                InstitutionDescription = "",
                ShortcutDimension1Code = o.ShortcutDimension1Code,
                IdRegiao = 0,

            }).FirstOrDefault();

            if (order == null) { return NotFound(); }
            var cliente = evolutionWEBContext.Cliente.FirstOrDefault(i => i.IdCliente == order.IdClienteEvolution);
            order.ClientName = cliente.Nome;
            var instituicao = evolutionWEBContext.Instituicao.FirstOrDefault(i => i.IdInstituicao == order.IdInstituicaoEvolution);
            order.InstitutionDescription = instituicao.DescricaoTreePath;

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

            List<EquipamentoViewModel> newList;
            try
            {
                newList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EquipamentoViewModel>>(Newtonsoft.Json.JsonConvert.SerializeObject(list));
            }
            catch
            {
                newList = new List<EquipamentoViewModel>();
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
                item.haveCurative = evolutionWEBContext.MaintenanceOrder.Where(c => c.IdServicoEvolution == item.IdServico).Select(c => c.No).FirstOrDefault();
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

        public class OmHeaderViewModel
        {
            public string ContractNo;
            public string No;
            public string NomeCliente;
            public string Description;
            public int? IdClienteEvolution;
            public int? IdInstituicaoEvolution;
            public int IdRegiao;
            public string ShortcutDimension1Code;
            public string ClientName;
            public string InstitutionDescription;
        }










        [AllowAnonymous]
        [Route("{maintenanceSheet}"), HttpGet, AcceptHeader("application/json")]
        //[ResponseCache(Duration = 60000)]
        public ActionResult GetEquipmentDetails(List<int?> equipmentId, ODataQueryOptions<Equipamento> queryOptions, int? categoryId)
        {
            if (equipmentId == null) { return NotFound(); }
            if (categoryId == null) { return NotFound(); }

            var pageSize = 30;
            var maintenanceSheetCategories = evolutionWEBContext.FichaManutencao.Where(f => f.IdCategoria == categoryId).Select(c => c.IdCategoria).ToList();
            var maintenanceSheetCodigo = evolutionWEBContext.FichaManutencao.Where(f => f.IdCategoria == categoryId).Select(c => c.Codigo).FirstOrDefault();
            var headerTaskmaintenance = evolutionWEBContext.FichaManutencao.Where(f => maintenanceSheetCategories.Contains(f.IdCategoria)).Select(c => c.Codigo).FirstOrDefault();
            var headerTaskquantity = evolutionWEBContext.FichaManutencaoTestesQuantitativos.Where(q => q.Codigo == maintenanceSheetCodigo).Select(q => q.Codigo).FirstOrDefault();
            var headerTaskquality = evolutionWEBContext.FichaManutencaoTestesQualitativos.Where(q => q.Codigo == maintenanceSheetCodigo).Select(q => q.Codigo).FirstOrDefault();
            bool taskMaintenance;
            bool taskQuality;
            bool taskQuantity;
            if (headerTaskmaintenance != null) { taskMaintenance = true; }
            if (headerTaskquantity != null) { taskQuantity = true; }
            if (headerTaskquality != null) { taskQuality = true; }

            var equipmentHeader = evolutionWEBContext.Equipamento.AsQueryable().Where(e => equipmentId.Contains(e.IdEquipamento)).Select(e => new EquipmentHeaderViewModel()
            {
                IdEquipamento = e.IdEquipamento,
                Sala = e.Sala,
                IdServico = e.IdServico,
                IdCliente = e.IdCliente,
                Categoria = e.Categoria,
                Marca = e.Marca,
                Modelo = e.Modelo,
                NumSerie = e.NumSerie,
                NumInventario = e.NumInventario,
            }).FirstOrDefault();
            if (equipmentHeader == null) { return NotFound(); }


            IQueryable headerResults;
            headerResults = queryOptions.ApplyTo(evolutionWEBContext.Equipamento.Where(o => equipmentId.Contains(o.IdEquipamento)).Select(e => new Equipamento
            {
                IdEquipamento = e.IdEquipamento,
                Nome = e.Nome,
                Marca = e.Marca,
                NumSerie = e.NumSerie,
                NumInventario = e.NumInventario,
            }).Where(e => true), new ODataQuerySettings { PageSize = pageSize });
            var headerList = headerResults.Cast<dynamic>().AsEnumerable();
            var headerTotal = Request.ODataFeature().TotalCount;
            var headerNextLink = Request.GetNextPageLink(pageSize);

            List<EquipamentoViewModel> headerDataList;
            try
            {
                headerDataList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EquipamentoViewModel>>(Newtonsoft.Json.JsonConvert.SerializeObject(headerList));
            }
            catch
            {
                headerDataList = new List<EquipamentoViewModel>();
            }

            headerDataList.ForEach((item) =>
            {
                var marca = evolutionWEBContext.EquipMarca.FirstOrDefault(m => m.IdMarca == item.Marca);
                if (marca != null) { item.MarcaText = marca.Nome; }
                var modelo = evolutionWEBContext.Equipamento.Where(e => e.IdEquipamento == item.IdEquipamento).Select(m => m.Modelo).FirstOrDefault();
                var modeloType = evolutionWEBContext.EquipModelo.Where(m => m.IdModelo == modelo).Select(m => m.IdModelos).FirstOrDefault();
                var modeloNome = evolutionWEBContext.Modelos.Where(m => m.IdModelos == modeloType).Select(n => n.Nome).FirstOrDefault();
                if (modeloType != null)
                {
                    item.ModeloText = modeloNome;
                }

                item.NumSerie = evolutionWEBContext.Equipamento.Where(e => e.IdEquipamento == item.IdEquipamento).Select(m => m.NumSerie).FirstOrDefault();
                item.NumInventario = evolutionWEBContext.Equipamento.Where(e => e.IdEquipamento == item.IdEquipamento).Select(m => m.NumInventario).FirstOrDefault();
            });

            var headerResultLines = new PageResult<dynamic>(headerDataList, headerNextLink, headerTotal);

            var sheetCategories = evolutionWEBContext.FichaManutencao.Where(f => f.IdCategoria == equipmentHeader.Categoria).Select(c => c.IdCategoria).FirstOrDefault();
            var maintenanceCode = evolutionWEBContext.FichaManutencao.Where(o => o.IdCategoria == sheetCategories).Select(c => c.Codigo).FirstOrDefault();
            IQueryable maintenanceResults;
            maintenanceResults = evolutionWEBContext.FichaManutencaoManutencao.Where(o => o.Codigo == maintenanceCode).Select(f => new FichaManutencaoManutencao
            {
                IdManutencao = f.IdManutencao,
                Codigo = f.Codigo,
                Descricao = f.Descricao,
                Rotinas = f.Rotinas,
            }).Where(f => true);
            var maintenanceList = maintenanceResults.Cast<dynamic>().AsEnumerable();
            var maintenanceTotal = Request.ODataFeature().TotalCount;
            var maintenanceNextLink = Request.GetNextPageLink(pageSize);

            List<FichaManutencaoManutencaoViewModel> maintenanceDataList;
            try
            {
                maintenanceDataList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FichaManutencaoManutencaoViewModel>>(Newtonsoft.Json.JsonConvert.SerializeObject(maintenanceList));
            }
            catch
            {
                maintenanceDataList = new List<FichaManutencaoManutencaoViewModel>();
            }

            if (headerTaskmaintenance != null)
            {
                maintenanceDataList.ForEach((item) =>
                {
                    var description = evolutionWEBContext.FichaManutencaoManutencao.Where(d => d.Codigo == item.Codigo).Select(d => d.Descricao).FirstOrDefault();
                    if (description != null) { item.Descricao = description; }
                });
            }

            IQueryable quantityResults;
            quantityResults = evolutionWEBContext.FichaManutencaoTestesQuantitativos.Where(o => o.Codigo == maintenanceCode).Select(f => new FichaManutencaoTestesQuantitativos
            {
                IdTestesQuantitativos = f.IdTestesQuantitativos,
                Codigo = f.Codigo,
                Descricao = f.Descricao,
                Rotinas = f.Rotinas,
                Versao = f.Versao,
                TipoCampo1 = f.TipoCampo1,
                UnidadeCampo1 = f.UnidadeCampo1,
            }).Where(f => true);
            var quantityList = quantityResults.Cast<dynamic>().AsEnumerable();
            var quantityTotal = Request.ODataFeature().TotalCount;
            var quantityNextLink = Request.GetNextPageLink(pageSize);

            List<FichaManutencaoTestesQuantitativosViewModel> quantityDataList;
            try
            {
                quantityDataList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FichaManutencaoTestesQuantitativosViewModel>>(Newtonsoft.Json.JsonConvert.SerializeObject(quantityList));
            }
            catch
            {
                quantityDataList = new List<FichaManutencaoTestesQuantitativosViewModel>();
            }
            if (headerTaskquantity != null)
            {
                quantityDataList.ForEach((item) =>
                {
                    var description = evolutionWEBContext.FichaManutencaoTestesQuantitativos.Where(d => d.Codigo == item.Codigo).Select(d => d.Descricao).FirstOrDefault();
                    if (description != null) { item.Descricao = description; }
                });
            }

            IQueryable qualityResults;
            qualityResults = evolutionWEBContext.FichaManutencaoTestesQualitativos.Where(o => o.Codigo == maintenanceCode).Select(f => new FichaManutencaoTestesQualitativos
            {
                IdTesteQualitativos = f.IdTesteQualitativos,
                Codigo = f.Codigo,
                Descricao = f.Descricao,
                Rotinas = f.Rotinas,
                Versao = f.Versao,
            }).Where(f => true);
            var qualityList = qualityResults.Cast<dynamic>().AsEnumerable();
            var qualityTotal = Request.ODataFeature().TotalCount;
            var qualityNextLink = Request.GetNextPageLink(pageSize);

            List<FichaManutencaoTestesQualitativosViewModel> qualityDataList;
            try
            {
                qualityDataList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FichaManutencaoTestesQualitativosViewModel>>(Newtonsoft.Json.JsonConvert.SerializeObject(qualityList));
            }
            catch
            {
                qualityDataList = new List<FichaManutencaoTestesQualitativosViewModel>();
            }
            if (headerTaskquality != null)
            {
                qualityDataList.ForEach((item) =>
                {
                    var description = evolutionWEBContext.FichaManutencaoTestesQualitativos.Where(d => d.Codigo == item.Codigo).Select(d => d.Descricao).FirstOrDefault();
                    if (description != null) { item.Descricao = description; }
                });
            }

            return Json(new
            {
                headerTaskmaintenance,
                headerTaskquantity,
                headerTaskquality,
                equipmentHeader,
                headerResultLines,
                maintenanceResults,
                qualityDataList,
                quantityDataList,
            });
        }

        public class EquipmentHeaderViewModel
        {
            public int? IdEquipamento;
            public string Sala;
            public int? IdServico;
            public int? IdCliente;
            public int? Categoria;
            public int? Marca;
            public int? Modelo;
            public string NumSerie;
            public string NumInventario;
            public bool taskMaintenance;
            public bool taskQuality;
            public bool taskQuantity;
        }

        public class IconsTasksHeaderViewModel
        {
            public bool iconTaskMaintenance;
            public bool iconTaskQuality;
            public bool iconTaskQuantity;
            public List<int?> iconTaskEMM;
        }



        [Route("room"), HttpPut]
        public ActionResult RoomsPut(int? equipmentId, string room)
        {
            if (equipmentId == null || equipmentId == 0 || room == null || room == "") { return NotFound(); }
            if (room == null || room == "") { return NotFound(); }

            var equipmentToUpdate = EquipamentoRepository.AsQueryable().Where(m => m.IdEquipamento == equipmentId).FirstOrDefault();
            if (equipmentToUpdate == null) { return NotFound(); }

            var roomToUpdate = evolutionWEBContext.Equipamento.Where(u => u.IdEquipamento == equipmentId).Select(r => r.Sala).FirstOrDefault();
            equipmentToUpdate.Sala = room;

            try
            {
                evolutionWEBContext.Update(equipmentToUpdate);
                evolutionWEBContext.SaveChanges();
            }
            catch (Exception)
            {
                return Json(0);
            }

            return Json(equipmentToUpdate);
        }

        public class UpdateTechnicalsModel
        {
            public string orderId;
            public string[] technicalsId;
        }

        public class UpdateRoomsModel
        {
            public int? equipmentId;
            public string room;
        }

    }
}
