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
using NJsonSchema.Infrastructure;

namespace Hydra.Such.Portal.Controllers
{
	public partial class MaintenanceOrdersController : Controller
	{

		[Route("{orderId}"), HttpGet, AcceptHeader("application/json")]
		[ResponseCache(Duration = 1800, VaryByHeader = "Cache")]
		public ActionResult GetOrderDetails(string orderId, ODataQueryOptions<Equipamento> queryOptions, string v)
		{
			if (orderId == null) { return NotFound(); }

			var pageSize = 150;

			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

			if (loggedUser == null) { return NotFound(); }

			var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name && u.Activo == true);

			if (evolutionLoggedUser == null) { return NotFound(); }

			var nivelAcesso = evolutionLoggedUser.NivelAcesso;
			var utilizadorPermissao = evolutionWEBContext.UtilizadorPermissao.Where(r => r.IdUser == evolutionLoggedUser.Id).ToList();

			var orders = evolutionWEBContext.MaintenanceOrder.AsQueryable().Where(o => o.No == orderId).Select(o => new OmHeaderViewModel()
			{
				No = o.No,
				Description = o.Description,
				IdClienteEvolution = o.IdClienteEvolution,
				IdInstituicaoEvolution = o.IdInstituicaoEvolution,
				ContractNo = o.ContractNo,
				ClientName = o.CustomerName,
				InstitutionDescription = "",
				ShortcutDimension1Code = o.ShortcutDimension1Code,
				ShortcutDimension3Code = o.ShortcutDimension3Code,
				IdRegiao = 0,
				OrderType = o.OrderType,
				OrderDate = o.OrderDate
			});

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
					orders = orders.Where(q => q.ShortcutDimension1Code == evolutionLoggedUser.Code1);
					break;
				case 5:
				case 6:
				case 7:
					var userTeams = new List<int>();
					var equipas = utilizadorPermissao.Select(r => r.Equipa).ToList();
					try { equipas.ForEach((item) => { var teste = item.Split(",").Select(s => int.Parse(s.Trim())); userTeams.AddRange(item.Split(",").Select(s => int.Parse(s.Trim())).ToList()); }); }
					catch /*(Exception ex)*/ { }
					var userDimensionCodes3 = evolutionWEBContext.Equipa.Where(e => userTeams.Contains(e.IdEquipa)).Select(s => s.Nome).ToList();
					orders = orders.Where(q => userDimensionCodes3.Contains(q.ShortcutDimension3Code));
					break;
				default:
					break;
			}

			var order = orders.FirstOrDefault();
			if (order == null) { return NotFound(); }

			var maintenanceOrderLine = evolutionWEBContext.MaintenanceOrderLine.Where(r => r.MoNo == order.No);
			var ordemManutencaoLinha = evolutionWEBContext.OrdemManutencaoLinha.Where(r => r.No == order.No);

			var equipmentIds = maintenanceOrderLine.Select(s => s.IdEquipamento).ToList();
			equipmentIds.AddRange(ordemManutencaoLinha.Select(s => s.IdEquipamento).ToList());

			// hammered to force the list to return only equipment with defined maintenance.
			var availableTypes = evolutionWEBContext.FichaManutencao
				.Where(s => order.OrderDate >= s.PeriodoInicio && order.OrderDate <= s.PeriodoFim).Select(s => s.IdTipo)
				.Distinct().ToList();
			var availableCategories = evolutionWEBContext.EquipCategoria
				.Where(ec => availableTypes.Contains(ec.IdTipo)).Select(s => s.IdCategoria).ToList();

			
			var queryEquipments = evolutionWEBContext.Equipamento.OrderByDescending(o => o.IdEquipamento).Select(e => new Equipamento
			{
				Nome = e.Nome,
				Marca = e.Marca,
				MarcaText = e.MarcaText,
				ModeloText = e.ModeloText,
				IdEquipamento = e.IdEquipamento,
				Categoria = e.Categoria,
				NumSerie = e.NumSerie,
				NumInventario = e.NumInventario,
				IdServico = e.IdServico,
				NumEquipamento = e.NumEquipamento,
				IdRegiao = e.IdRegiao
			}).Where(e => (v == "2" ? true : equipmentIds.Contains(e.IdEquipamento)) && (v == "1" ? true : availableCategories.Contains(e.Categoria)));

			var preventiveCodes = evolutionWEBContext.MaintenanceCatalog.Where(f => f.ManutPreventiva == 1).Select(f => f.Code).ToList();
			var curativeCodes = evolutionWEBContext.MaintenanceCatalog.Where(f => f.ManutCorrectiva == 1).Select(f => f.Code).ToList();
			var orderPreventive = preventiveCodes.Contains(order.OrderType);
			var orderCurative = curativeCodes.Contains(order.OrderType);

			if (orderCurative) { return Json(false); }

			var cliente = evolutionWEBContext.Cliente.FirstOrDefault(i => i.IdCliente == order.IdClienteEvolution);
			order.ClientName = cliente.Nome;
			var instituicao = evolutionWEBContext.Instituicao.FirstOrDefault(i => i.IdInstituicao == order.IdInstituicaoEvolution);
			order.InstitutionDescription = instituicao.DescricaoTreePath;

			int.TryParse(order.ShortcutDimension1Code, out order.IdRegiao);

			IQueryable results;
			results = queryOptions.ApplyTo(queryEquipments, new ODataQuerySettings { PageSize = pageSize });

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

				if (item.Marca != 1 && (item.MarcaText == null || item.MarcaText.Length == 0))
				{
					item.MarcaText = marca != null ? marca.Nome : "";
				} 

				item.ServicoText = servico != null ? servico.Nome : "";
				item.haveCurative = evolutionWEBContext.MaintenanceOrder.Where(c => c.IdServicoEvolution == item.IdServico).Select(c => c.No).FirstOrDefault();

				var plan = evolutionWEBContext.FichaManutencaoRelatorio.FirstOrDefault(m =>
					m.Om == order.No && m.IdEquipamento == item.IdEquipamento);
				item.Estado = "0";
				if (plan != null)
				{
					item.Estado = plan.EstadoFinal.ToString();
					item.Signed = (plan.AssinaturaCliente != "" && plan.AssinaturaCliente != null) || 
					              (plan.AssinaturaSieIgualCliente == true && plan.AssinaturaSie != ""&& plan.AssinaturaSie != null) && item.Estado != "0" ? 2:item.Estado != "0" ? 1 : 0 ;
				}
				
			});

			var resultLines = new PageResult<dynamic>(newList, nextLink, total);
			var all = evolutionWEBContext.Equipamento.Where(e =>
				(v == "2" ? true : equipmentIds.Contains(e.IdEquipamento)) &&
				(v == "1" ? true : availableCategories.Contains(e.Categoria))).Count();
			var executed = evolutionWEBContext.FichaManutencaoRelatorio
				.Where(m =>
					m.Om == order.No && (m.AssinaturaCliente != null && m.AssinaturaCliente != "") ||
					(m.AssinaturaSieIgualCliente == true && (m.AssinaturaSie != "" && m.AssinaturaSie != null)) &&
					m.EstadoFinal != 0).Count();
			
			var ordersCountsLines = new
			{
				toExecute = all - executed,
				toSigning = evolutionWEBContext.FichaManutencaoRelatorio
					.Where(m=>
						m.Om == order.No && (m.AssinaturaCliente == null || m.AssinaturaCliente == "") && 
						(m.AssinaturaSieIgualCliente == false || (m.AssinaturaSie == "" || m.AssinaturaSie == null) ) &&
						m.EstadoFinal != 0).Count(),
				executed = executed
			};
			
			return Json(new
			{
				order,
				resultLines,
				ordersCountsLines,
				marcas,
				servicos,
				categorias = evolutionWEBContext.EquipCategoria.Where(m => availableCategories.Contains(m.IdCategoria)).Select(m => new
				{
					m.IdCategoria,
					m.Nome
				}).ToList()
			});
		}
	}
}

