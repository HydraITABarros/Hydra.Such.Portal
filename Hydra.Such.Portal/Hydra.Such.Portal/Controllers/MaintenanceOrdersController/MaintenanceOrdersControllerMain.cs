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
	public partial class MaintenanceOrdersController : Controller
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

		[Route("{orderId}/curativa"), HttpGet, AcceptHeader("text/html")]
		public IActionResult FichaDeManutencaoCurativa(string orderId)
		{
			return Index();
		}

		[Route("arquivo"), HttpGet, AcceptHeader("text/html")]
		public IActionResult Arquivo()
		{
			return Index();
		}


		[Route(""), HttpGet, AcceptHeader("application/json")]
		[ResponseCache(Duration = 60000, VaryByHeader = "Cache")]
		public ActionResult GetAll(ODataQueryOptions<MaintenanceOrder> queryOptions, string v)
		{
			var UserName = User.Identity.Name;
			if (v == "1" || v == "2")
			{
				UserName = "MIGUELO@SUCH.PT"; /* equipa  43 */
				UserName = "HFREITAS@SUCH.PT";/* equipa  14 - HELDER JOSE HENRIQUE FREITAS - OM1907378*/
			}

			var pageSize = 150;

			var preventiveCodes = evolutionWEBContext.MaintenanceCatalog.Where(f => f.ManutPreventiva == 1).Select(f => f.Code).ToList();
			var curativeCodes = evolutionWEBContext.MaintenanceCatalog.Where(f => f.ManutCorrectiva == 1).Select(f => f.Code).ToList();
			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

			if (loggedUser == null) { return NotFound(); }

			var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == UserName && u.Activo == true);

			if (evolutionLoggedUser == null) { return NotFound(); }

			var nivelAcesso = evolutionLoggedUser.NivelAcesso;
			var utilizadorPermissao = evolutionWEBContext.UtilizadorPermissao.Where(r => r.IdUser == evolutionLoggedUser.Id).ToList();

			IQueryable results;

			var query = MaintenanceOrdersRepository.AsQueryable()
			    .Where(o =>
			    (preventiveCodes.Contains(o.OrderType) || curativeCodes.Contains(o.OrderType)) &&
			    (o.Status == 0 || o.Status == 6 || o.Status == 7) && (o.IdClienteEvolution != null && o.IdClienteEvolution != 0 && o.IdInstituicaoEvolution != null && o.IdInstituicaoEvolution != 0));

			var preventives = MaintenanceOrdersRepository.AsQueryable().Where(o =>
			       preventiveCodes.Contains(o.OrderType) && (o.Status == 0 || o.Status == 6 || o.Status == 7) &&
			    (o.IdClienteEvolution != null && o.IdInstituicaoEvolution != null));

			var curatives = MaintenanceOrdersRepository.AsQueryable().Where(o =>
			       curativeCodes.Contains(o.OrderType) && (o.Status == 0 || o.Status == 6 || o.Status == 7) &&
			    (o.IdClienteEvolution != null && o.IdInstituicaoEvolution != null));

			/* remove in production */
			if (v == "1")
			{
				nivelAcesso = 7;
			}
			else if (v == "2")
			{
				nivelAcesso = 4;
			}
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
					var userTeams = new List<int>();
					var equipas = utilizadorPermissao.Select(r => r.Equipa).ToList();

					try
					{
						equipas.ForEach((item) =>
						{
							var teste = item.Split(",").Select(s => int.Parse(s.Trim()));
							userTeams.AddRange(item.Split(",").Select(s => int.Parse(s.Trim())).ToList());
						});
					}
					catch /*(Exception ex)*/ { }
					var userDimensionCodes3 = evolutionWEBContext.Equipa.Where(e => userTeams.Contains(e.IdEquipa)).Select(s => s.Nome).ToList();

					preventives = preventives.Where(q => userDimensionCodes3.Contains(q.ShortcutDimension3Code));
					curatives = curatives.Where(q => userDimensionCodes3.Contains(q.ShortcutDimension3Code));
					query = query.Where(q => userDimensionCodes3.Contains(q.ShortcutDimension3Code));
					break;
				default:
					break;
			}
			/**/

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
				OrderType = o.OrderType,
				IdTecnico1 = o.IdTecnico1,
				IdTecnico2 = o.IdTecnico2,
				IdTecnico3 = o.IdTecnico3,
				IdTecnico4 = o.IdTecnico4,
				IdTecnico5 = o.IdTecnico5,
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

				var client = evolutionWEBContext.Cliente.FirstOrDefault(c => c.IdCliente == item.IdClienteEvolution && c.Activo == true);
				if (client != null) { item.ClientName = client.Nome; }

				var institution = evolutionWEBContext.Instituicao.FirstOrDefault(i => i.IdInstituicao == item.IdInstituicaoEvolution && i.Activo == true);
				if (institution != null) { item.InstitutionName = institution.DescricaoTreePath; }

				var service = evolutionWEBContext.Servico.FirstOrDefault(s => s.IdServico == item.IdServicoEvolution && s.Activo == true);
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


		[Route("institutions"), HttpGet, AcceptHeader("application/json")]
		[ResponseCache(Duration = 86400)] /*24h*/
		public ActionResult GetInstitutions()
		{
			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

			if (loggedUser == null) { return NotFound(); }

			var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name && u.Activo == true);

			if (evolutionLoggedUser == null) { return NotFound(); }

			var nivelAcesso = evolutionLoggedUser.NivelAcesso;

			var cliente = evolutionWEBContext.Cliente.Where(c => c.Activo == true);
			var instituicao = evolutionWEBContext.Instituicao.Where(i => i.Activo == true);
			/*
			 * filter MaintenanceOrder query based on user permissions
			 * 1, 2, 8 = Podem ver tudo
			 * 3, 4 = Filtrado por regiao
			 * 5, 6, 7 = Filtrado por Cresp
			 */

			/**/
			switch (nivelAcesso)
			{
				case 3:
				case 4:
					cliente = cliente.Where(c => c.RegiaoNav == evolutionLoggedUser.Code1).ToList().AsQueryable();
					instituicao = instituicao.Where(i => cliente.Select(c => c.IdCliente).Contains(i.Cliente)).ToList().AsQueryable();
					break;
				case 5:
				case 6:
				case 7:
					//cliente = cliente.Where(c => c.CrespNav == evolutionLoggedUser.Code3).ToList().AsQueryable();
					//instituicao = instituicao.Where(i => cliente.Select(c => c.IdCliente).Contains(i.Cliente)).ToList().AsQueryable();
					break;
				default:
					break;
			}
			/**/

			return Json(instituicao.Select(i => new { id = i.IdInstituicao, name = i.Nome }).ToList());
		}


		[Route("clients"), HttpGet, AcceptHeader("application/json")]
		[ResponseCache(Duration = 86400)] /*24h*/
		public ActionResult GetClients()
		{
			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

			if (loggedUser == null) { return NotFound(); }

			var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name && u.Activo == true);

			if (evolutionLoggedUser == null) { return NotFound(); }

			var nivelAcesso = evolutionLoggedUser.NivelAcesso;

			var clients = evolutionWEBContext.Cliente.Where(c => c.Activo == true);
			/*
			 * filter MaintenanceOrder query based on user permissions
			 * 1, 2, 8 = Podem ver tudo
			 * 3, 4 = Filtrado por regiao
			 * 5, 6, 7 = Filtrado por Cresp
			 */
			/**/
			switch (nivelAcesso)
			{
				case 3:
				case 4:
					clients = clients.Where(c => c.RegiaoNav == evolutionLoggedUser.Code1).ToList().AsQueryable();
					break;
				case 5:
				case 6:
				case 7:
					//clients = clients.Where(c => c.CrespNav == evolutionLoggedUser.Code3).ToList().AsQueryable();
					break;
				default:
					break;
			}
			/**/

			return Json(clients.Select(c => new { id = c.IdCliente, name = c.Nome }).ToList());
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


		[Route("{orderId}/technicals/logged"), HttpPut]
		public ActionResult SetTechnicals(string orderId)
		{
			if (orderId == null) { return NotFound(); }

			var order = evolutionWEBContext.MaintenanceOrder.FirstOrDefault(o => o.No == orderId);

			if (order == null) { return NotFound(); }

			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

			if (loggedUser == null) { return NotFound(); }

			var loggedUserCresps = suchDBContext.AcessosDimensões.Where(o => o.Dimensão == 3 && o.IdUtilizador == loggedUser.IdUtilizador).ToList();

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
			public string ShortcutDimension3Code;
			public string ClientName;
			public string InstitutionDescription;
			public string OrderType;
		}

		public class IconsTasksHeaderViewModel
		{
			public bool iconTaskMaintenance;
			public bool iconTaskQuality;
			public bool iconTaskQuantity;
			public List<int?> iconTaskEMM;
		}


		[Route("/ordens-de-manutencao/ficha-de-manutencao/curativa"), HttpGet, AcceptHeader("application/json")]
		//[ResponseCache(Duration = 60000)]
		public ActionResult GetCurativeMaintenancePlans(string orderId, int equipmentId)
		{
			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

			if (loggedUser == null) { return NotFound(); }

			var loggedUserCresps = suchDBContext.AcessosDimensões.Where(o => o.Dimensão == 3 && o.IdUtilizador == loggedUser.IdUtilizador).ToList();

			var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name);

			if (evolutionLoggedUser == null) { return NotFound(); }

			if (orderId == null || orderId == "") { return NotFound(); }

			//validar premissoes

			//obter ordem de manutencao
			var order = evolutionWEBContext.MaintenanceOrder.Where(m => m.No == orderId).Select(o => new MaintenanceOrderViewModel()
			{
				No = o.No,
				ClientName = o.ClientName,
				ContractNo = o.ContractNo,
				CustomerName = o.CustomerName,
				CustomerNo = o.CustomerNo,
				Description = o.Description,
				IdClienteEvolution = o.IdClienteEvolution,
				IdInstituicaoEvolution = o.IdInstituicaoEvolution,
				IdServicoEvolution = o.IdServicoEvolution,
				IdTecnico1 = o.IdTecnico1,
				IdTecnico2 = o.IdTecnico2,
				IdTecnico3 = o.IdTecnico3,
				IdTecnico4 = o.IdTecnico4,
				IdTecnico5 = o.IdTecnico5,
				InstitutionName = o.InstitutionName,
				isPreventive = o.isPreventive,
				ResponsibleEmployee = o.ResponsibleEmployee,
				MaintenanceResponsible = o.MaintenanceResponsible
			}).FirstOrDefault();
			if (order == null) { return NotFound(); }

			//obter campos de ficha de manutencao



			//obter fichas de manutencao (reports)

			var equipments = evolutionWEBContext.Equipamento.Where(e => e.IdEquipamento == equipmentId).Select(e => new EquipamentMaintenancePlanViewModel()
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
				NumEquipamento = e.NumEquipamento
			}).ToList();

			if (equipments == null || equipments.Count() < 1) { return NotFound(); }

			equipments.ForEach((item) =>
			{
				var marca = evolutionWEBContext.EquipMarca.FirstOrDefault(m => m.IdMarca == item.Marca);
				if (marca != null) { item.MarcaText = marca.Nome; }

				int? modeloType = evolutionWEBContext.EquipModelo.Where(m => m.IdModelo == item.Modelo).Select(m => m.IdModelos).FirstOrDefault();
				if (modeloType != null)
				{
					item.ModeloText = evolutionWEBContext.Modelos.Where(m => m.IdModelos == modeloType).Select(n => n.Nome).FirstOrDefault();
				}
				var servico = evolutionWEBContext.Servico.Where(s => s.IdServico == item.IdServico).FirstOrDefault();
				if (servico != null)
				{
					item.ServicoText = servico.Nome;
				}

				var categoria = evolutionWEBContext.EquipCategoria.Where(s => s.IdCategoria == item.Categoria).FirstOrDefault();
				if (categoria != null)
				{
					item.CategoriaText = categoria.Nome;
				}

				//todo get rotina (A or B)
				var rotina = "A";

				//var planSignatureReport = evolutionWEBContext.FichaManutencaoRelatorioAssinatura.Where(r => r.Om == order.No && r.IdEquipamento == item.IdEquipamento && r.Codigo == codigo && r.Versao == versao);

				//try { evolutionWEBContext.SaveChanges(); }
				//catch (Exception ex) { }

			});

			var institution = "";
			try { institution = evolutionWEBContext.Instituicao.FirstOrDefault(c => c.IdInstituicao == order.IdInstituicaoEvolution).Nome; }
			catch { institution = ""; }

			order.InstitutionName = institution;

			var client = "";
			try { client = evolutionWEBContext.Cliente.FirstOrDefault(c => c.IdCliente == order.IdClienteEvolution).Nome; }
			catch { client = ""; }

			order.ClientName = client;

			var technicals = GetTechnicals(order, null, null);
			if (technicals != null) { order.Technicals = technicals.ToList(); }

			order.ResponsibleEmployeeObj = evolutionWEBContext.Utilizador.Where(u => u.NumMec == order.ResponsibleEmployee).FirstOrDefault();
			order.MaintenanceResponsibleObj = evolutionWEBContext.Utilizador.Where(u => u.NumMec == order.MaintenanceResponsible).FirstOrDefault();

			return Json(new
			{
				order,
				equipments
			});
		}


		[Route("equipments"), HttpGet, AcceptHeader("application/json")]
		//[ResponseCache(Duration = 60000)]
		public ActionResult GetEquipments(string orderId, int? categoryId)
		{
			// validacao dos campos da funcao (orderId)
			if (orderId == null && orderId == "") { return NotFound(); }

			// validacao de premissoes
			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);
			if (loggedUser == null) { return Unauthorized(); }

			var userCresps = suchDBContext.AcessosDimensões.Where(o => o.Dimensão == 3 && o.IdUtilizador == loggedUser.IdUtilizador).Select(d => d.ValorDimensão).ToList();
			var userRegions = suchDBContext.AcessosDimensões.Where(o => o.Dimensão == 1 && o.IdUtilizador == loggedUser.IdUtilizador).Select(d => d.ValorDimensão).ToList();

			var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name && u.Activo == true);
			if (evolutionLoggedUser == null) { return Unauthorized(); }

			var orderQuery = evolutionWEBContext.MaintenanceOrder.AsQueryable().Where(o => o.No == orderId);
			/*
			switch (evolutionLoggedUser.NivelAcesso)
			{
			    case 3:
			    case 4:
				orderQuery = orderQuery.Where(o => userRegions.Contains(o.ShortcutDimension1Code));
				break;
			    case 5:
			    case 6:
			    case 7:
				orderQuery = orderQuery.Where(o => userCresps.Contains(o.ShortcutDimension3Code));
				break;
			    default:
				break;
			}*/

			var order = orderQuery.FirstOrDefault();
			if (order == null) { return NotFound(); }

			// obter equipamentos
			var equipas = evolutionWEBContext.Equipa.Where(e => e.Nome == order.ShortcutDimension3Code).Select(s => s.IdEquipa).ToList();
			if (equipas == null) { return NotFound(); }

			var equipments = evolutionWEBContext.Equipamento.Where(e => e.Activo == true)/*.Where(e => equipas.Contains((e.IdEquipa == null ? 0 : (int)e.IdEquipa)))*/;

			if (categoryId != null)
			{
				equipments = equipments.Where(e => e.Categoria == categoryId);
			}

			return Json(equipments);
		}


		[Route("{orderId}/equipments"), HttpPut]
		public ActionResult SetEquipments(string orderId, int? equipmentId)
		{
			if (orderId == null || equipmentId == null) { return NotFound(); }

			var order = evolutionWEBContext.MaintenanceOrder.FirstOrDefault(o => o.No == orderId);

			var orderDetails = evolutionWEBContext.MaintenanceOrderLine.FirstOrDefault(d => d.MoNo == orderId);

			if (order == null) { return NotFound(); }

			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

			if (loggedUser == null) { return NotFound(); }

			var loggedUserCresps = suchDBContext.AcessosDimensões.Where(o => o.Dimensão == 3 && o.IdUtilizador == loggedUser.IdUtilizador).ToList();

			var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name);

			if (evolutionLoggedUser == null) { return NotFound(); }

			try
			{
				MaintenanceOrderLine newLine = new MaintenanceOrderLine();
				newLine.MoNo = orderId;
				newLine.IdEquipamento = equipmentId;
				newLine.OrderStatus = order.Status;
				newLine.ObjectRefType = order.ObjectRefType;
				newLine.ObjectNo = order.ObjectRefNo;
				newLine.ObjectDescription = order.Description;
				newLine.ShortcutDimension1Code = order.ShortcutDimension1Code;
				newLine.ShortcutDimension2Code = order.ShortcutDimension2Code;
				newLine.ShortcutDimension3Code = order.ShortcutDimension3Code;
				newLine.CustomerNo = order.CustomerNo;
				newLine.OrderDate = order.OrderDate;
				evolutionWEBContext.MaintenanceOrderLine.Add(newLine);
				evolutionWEBContext.SaveChanges();
			}
			catch (Exception)
			{
				return Json(false);
			}

			return Json(true);
		}


		[Route("equipments"), HttpPost]
		public ActionResult CreateEquipment(int? equipmentId, int brand, int model, string serialNumber, string inventoryNumber)
		{
			if (equipmentId == null) { return NotFound(); }

			var previousEquipment = evolutionWEBContext.Equipamento.FirstOrDefault(o => o.IdEquipamento == equipmentId);

			if (previousEquipment == null) { return NotFound(); }

			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

			if (loggedUser == null) { return NotFound(); }

			var loggedUserCresps = suchDBContext.AcessosDimensões.Where(o => o.Dimensão == 3 && o.IdUtilizador == loggedUser.IdUtilizador).ToList();

			var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name);

			if (evolutionLoggedUser == null) { return NotFound(); }

			Equipamento newEquipment = new Equipamento();
			try
			{
				newEquipment.Nome = previousEquipment.Nome;
				newEquipment.Sala = previousEquipment.Sala;
				newEquipment.IdServico = previousEquipment.IdServico;
				newEquipment.Marca = brand;
				newEquipment.Modelo = model;
				newEquipment.Categoria = evolutionWEBContext.Equipamento.Select(m => m.Categoria).FirstOrDefault();
				newEquipment.IdCliente = evolutionWEBContext.Equipamento.Select(m => m.IdCliente).FirstOrDefault();
				newEquipment.NumSerie = serialNumber;
				newEquipment.NumInventario = inventoryNumber;
				newEquipment.DataAquisicao = new DateTime(1753, 1, 1);
				newEquipment.DataInstalacao = new DateTime(1753, 1, 1);
				newEquipment.DataInsercao = new DateTime(1753, 1, 1);
				newEquipment.DataValidacao = new DateTime(1753, 1, 1);
				newEquipment.DataEntradaContrato = new DateTime(1753, 1, 1);
				newEquipment.DataSaidaContrato = new DateTime(1753, 1, 1);
				evolutionWEBContext.Equipamento.AddRange(newEquipment);
				evolutionWEBContext.SaveChanges();
			}
			catch (Exception e)
			{
				return Json(e);
			}

			return Json(newEquipment);

		}


		[Route("emms"), HttpGet]
		public ActionResult GetEmms()
		{
			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

			if (loggedUser == null) { return NotFound(); }

			var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name && u.Activo == true);

			if (evolutionLoggedUser == null) { return NotFound(); }

			var nivelAcesso = evolutionLoggedUser.NivelAcesso;

			var emms = evolutionWEBContext.EmmEquipamentos.Where(c => c.Activo == true);

			switch (nivelAcesso)
			{
				case 3:
				case 4:
					emms = emms.Where(c => c.IdRegiao == evolutionLoggedUser.Code1).ToList().AsQueryable();
					break;
				case 5:
				case 6:
				case 7:
					emms = emms.Where(c => c.IdCresp == evolutionLoggedUser.Code3).ToList().AsQueryable();
					break;
				default:
					break;
			}

			return Json(emms.ToList());
		}


		[Route("equipamentos/{equipmentId}/room"), HttpPut]
		public ActionResult UpdateEquipmentRoom(int? equipmentId, string room)
		{
			if (equipmentId == null || room == null || room == "") { return NotFound(); }

			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

			if (loggedUser == null) { return NotFound(); }

			var loggedUserCresps = suchDBContext.AcessosDimensões.Where(o => o.Dimensão == 3 && o.IdUtilizador == loggedUser.IdUtilizador).ToList();

			var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name);

			if (evolutionLoggedUser == null) { return NotFound(); }

			var equipmentRepository = EquipamentoRepository.AsQueryable().Where(m => m.IdEquipamento == equipmentId).FirstOrDefault();
			if (equipmentRepository == null) { return NotFound(); }

			var equipment = evolutionWEBContext.Equipamento.Where(u => u.IdEquipamento == equipmentId).Select(r => r.Sala).FirstOrDefault();
			equipmentRepository.Sala = room;

			try
			{
				evolutionWEBContext.Update(equipmentRepository);
				evolutionWEBContext.SaveChanges();
			}
			catch (Exception)
			{
				return Json(false);
			}

			return Json(true);
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

