using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Portal.Filters;
using Hydra.Such.Data.Evolution.DatabaseReference;
using Hydra.Such.Portal.ViewModels;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;

namespace Hydra.Such.Portal.Controllers
{
	public partial class MaintenanceOrdersController : Controller
	{
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

			var equipments = evolutionWEBContext.Equipamento
				.Where(e => e.Activo == true && e.IdCliente == order.IdClienteEvolution);

			if (categoryId != null || categoryId != 0)
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

			if (order == null) { return NotFound(); }

			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

			if (loggedUser == null) { return NotFound(); }

			var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name);

			if (evolutionLoggedUser == null) { return NotFound(); }
			
			return Json(GetOrCreateOmLinha(orderId, equipmentId));
		}


		[Route("{orderId}/equipments"), HttpPost]
		public ActionResult CreateEquipment( string orderId, [FromBody]PostNewEquipment postNewEquipment)
		{
			var model = postNewEquipment.Model;
			var brand = postNewEquipment.Brand;
			var serialNumber = postNewEquipment.SerialNumber;
			var equipmentNumber = postNewEquipment.EquipmentNumber;
			var previousEquipmentId = postNewEquipment.PreviousEquipmentId;
			
			if (orderId == null || brand == null || model == null) { return NotFound(); }

			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

			if (loggedUser == null) { return NotFound(); }

			var loggedUserCresps = suchDBContext.AcessosDimensões.Where(o => o.Dimensão == 3 && o.IdUtilizador == loggedUser.IdUtilizador).ToList();

			var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name);

			if (evolutionLoggedUser == null) { return NotFound(); }

			var order = evolutionWEBContext.MaintenanceOrder.FirstOrDefault(o => o.No == orderId);
			if (order == null) { return NotFound(); }
			
			var previousEquipment = evolutionWEBContext.Equipamento
				.FirstOrDefault(o => o.IdEquipamento == previousEquipmentId );

			if (previousEquipment == null) { return NotFound(); }

			var _brand = evolutionWEBContext.EquipMarca
				.FirstOrDefault(m => m.Nome == brand.ToUpper().TrimStart().TrimEnd() && m.Activo == true);
			var marcaId = 1;
			if (_brand != null)
			{
				marcaId = _brand.IdMarca;
			}
			
			var modeloIds = evolutionWEBContext.EquipModelo
				.Where(m =>
					m.IdCategoria == previousEquipment.Categoria &&
					m.IdMarca == marcaId &&
					m.Activo == true).Select(m => m.IdModelos);
			
			var _modelo = evolutionWEBContext.Modelos
				.FirstOrDefault(m => modeloIds.Contains(m.IdModelos) && m.Nome == brand.ToUpper().TrimStart().TrimEnd() && m.Activo == true);
			
			var modeloId = 1;
			if (_modelo != null)
			{
				modeloId = _modelo.IdModelos;
			}

			Equipamento newEquipment = evolutionWEBContext.Equipamento
				.FirstOrDefault(e=> 
					e.Marca == marcaId && 
					e.Modelo == modeloId && 
					e.NumSerie == serialNumber &&
					e.NumEquipamento == equipmentNumber &&
					e.Categoria == previousEquipment.Categoria &&
					e.IdCliente == previousEquipment.IdCliente);
			
			//Equipamento newEquipment = new Equipamento();
			if(newEquipment == null) {
				newEquipment = new Equipamento();
				try
				{
					newEquipment.Nome = previousEquipment.Nome;
					newEquipment.Sala = previousEquipment.Sala;
					newEquipment.IdServico = previousEquipment.IdServico;
					newEquipment.IdRegiao = previousEquipment.IdRegiao;
					newEquipment.Categoria = previousEquipment.Categoria;
					newEquipment.IdCliente = previousEquipment.IdCliente;
					newEquipment.NumInventario = "";
					newEquipment.NumEquipamento = "";
					newEquipment.Marca = marcaId;
					newEquipment.Modelo = modeloId;
					newEquipment.MarcaText = brand;
					newEquipment.ModeloText = model;
					newEquipment.NumSerie = serialNumber;
					newEquipment.NumEquipamento = equipmentNumber;
					newEquipment.PorValidar = true;
					newEquipment.DataAquisicao = new DateTime(1753, 1, 1);
					newEquipment.DataInstalacao = new DateTime(1753, 1, 1);
					newEquipment.DataInsercao = DateTime.Now;
					newEquipment.DataValidacao = new DateTime(1753, 1, 1);
					newEquipment.DataEntradaContrato = new DateTime(1753, 1, 1);
					newEquipment.DataSaidaContrato = new DateTime(1753, 1, 1);
					newEquipment.Activo = true;
					
					evolutionWEBContext.Equipamento.AddRange(newEquipment);
					evolutionWEBContext.SaveChanges();
				}
				catch (Exception e)
				{
					return Json(/*e*/false);
				}
			}
			GetOrCreateOmLinha(orderId, newEquipment.IdEquipamento);
			return Json(newEquipment.IdEquipamento);
		}

		private bool GetOrCreateOmLinha(string orderId, int? equipmentId)
		{
			var omLinha = evolutionWEBContext.OrdemManutencaoLinha.FirstOrDefault(r =>
				r.No == orderId && r.IdEquipamento == equipmentId);

			if (omLinha != null)
			{
				return true;
			}
			
			var moLine = evolutionWEBContext.MaintenanceOrderLine.FirstOrDefault(r =>
				r.MoNo == orderId && r.IdEquipamento == equipmentId);

			if (moLine != null)
			{
				return true;
			}

			var lastLine = evolutionWEBContext.OrdemManutencaoLinha.Where(r => r.No == orderId)
				.OrderByDescending(o => o.NumLinha).FirstOrDefault();
			try
			{
				OrdemManutencaoLinha newLine = new OrdemManutencaoLinha();
				newLine.No = orderId;
				newLine.NumLinha = lastLine.NumLinha + 1;
				newLine.IdEquipamento = equipmentId;
				evolutionWEBContext.OrdemManutencaoLinha.Add(newLine);
				evolutionWEBContext.SaveChanges();
			}
			catch (Exception ex)
			{
				return false;
			}

			return true;
		}
		
	}

	public class PostNewEquipment
	{
		public int PreviousEquipmentId; 
		public string Brand;
		public string Model;
		public string SerialNumber;
		public string EquipmentNumber;
	} 
	
}

