using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Portal.Filters;
using Hydra.Such.Data.Evolution.DatabaseReference;
using Hydra.Such.Portal.ViewModels;
using JavaScriptEngineSwitcher.Core.Extensions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;

namespace Hydra.Such.Portal.Controllers
{
	public partial class MaintenanceOrdersController : Controller
	{
		
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
		
		[Route("emms/find"), HttpGet]
		public ActionResult FindEmmBySerial(string orderId, string serial)
		{
			//dropdown - ou emms disponíveis
			// através da cetegoryId vou buscar a ficha de manutenção
			// através da ficha de manutenção vou buscar fichademanutencaoequipamentosdeteste
			// relacionar equipamento_teste com id de EMM_tipo
			// retirar id
			
			if (orderId == null) { return NotFound(); }
			var order = evolutionWEBContext.MaintenanceOrder.FirstOrDefault(o => o.No == orderId);
			if (order == null) { return NotFound(); }
			
			var emm = evolutionWEBContext.EmmEquipamentos
				.FirstOrDefault(c => 
					c.Activo == true &&
					c.IdCresp == order.ShortcutDimension3Code &&
					c.NumSerie.Contains(serial.TrimStart().TrimEnd())
				);

			if (emm == null) { return NotFound(); }
			
			var marca = evolutionWEBContext.EquipMarca.FirstOrDefault(e => e.IdMarca == emm.IdMarca);
			var modeloCategoria = evolutionWEBContext.EquipModelo.FirstOrDefault(e => e.IdModelo == emm.IdModelo);
			var modelo = new Modelos();
			if(modeloCategoria != null) {
				modelo = evolutionWEBContext.Modelos.FirstOrDefault(e => e.IdModelos == modeloCategoria.IdModelos);
			}

			var emmViewModel = new FichaManutencaoEmmEquipamentosViewModel()
			{
				Id = emm.Id,
				IdMarca = emm.IdMarca,
				IdModelo = emm.IdModelo,
				MarcaText = marca.Nome,
				ModeloText = modelo.Nome,
				NumSerie = emm.NumSerie
			};
			
			return Json(emmViewModel);
		}
		
		[Route("emms"), HttpPost]
		public ActionResult AddEmmToPlans(string orderId, string equipmentsIds)
		{
			//dropdown - ou emms disponíveis
			// através da cetegoryId vou buscar a ficha de manutenção
			// através da ficha de manutenção vou buscar fichademanutencaoequipamentosdeteste
			// relacionar equipamento_teste com id de EMM_tipo
			// retirar id
			
			if (orderId == null) { return NotFound(); }
			var order = evolutionWEBContext.MaintenanceOrder.FirstOrDefault(o => o.No == orderId);
			if (order == null) { return NotFound(); }
		
			return Json(true);
		}

	}
}

