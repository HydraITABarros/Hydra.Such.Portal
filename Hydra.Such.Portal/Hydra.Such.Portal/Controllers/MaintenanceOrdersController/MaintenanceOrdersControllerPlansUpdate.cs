using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Portal.Filters;
using Hydra.Such.Data.Evolution.DatabaseReference;
using Hydra.Such.Portal.ViewModels;
using Hydra.Such.Portal.Extensions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;

namespace Hydra.Such.Portal.Controllers
{
	public partial class MaintenanceOrdersController : Controller
	{

		[Route("/ordens-de-manutencao/ficha-de-manutencao"), HttpPost, AcceptHeader("application/json")]
		//[ResponseCache(Duration = 86400)]
		public ActionResult PostMaintenancePlans([FromBody] List<EquipamentMaintenancePlanViewModel> plans, string orderId)
		{
			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

			if (loggedUser == null) { return NotFound(); }

			var loggedUserCresps = suchDBContext.AcessosDimensões.Where(o => o.Dimensão == 3 && o.IdUtilizador == loggedUser.IdUtilizador).ToList();

			var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name);

			if (evolutionLoggedUser == null) { return NotFound(); }

			if (string.IsNullOrEmpty(orderId)) { return NotFound(); }

			var order = evolutionWEBContext.MaintenanceOrder.Where(m => m.No == orderId).FirstOrDefault();
			if (order == null) { return NotFound(); }

			if (plans == null) { return NotFound(); }

			var Now = DateTime.Now;
			
			plans.ForEach((item) =>
			{
				var planReport = evolutionWEBContext.FichaManutencaoRelatorio
					.Where(r => r.Om == order.No && r.IdEquipamento == item.IdEquipamento && 
					            (item.Codigo != null ? r.Codigo == item.Codigo : true) && (item.Versao != null ? r.Versao == item.Versao: true))
					.OrderByDescending(o => o.Id).FirstOrDefault();
				if (planReport != null)
				{
					planReport.EstadoFinal = item.EstadoFinal;
					planReport.DataEstadoFinal = item.DataEstadoFinal;
					planReport.Observacao = item.Observacao;
					planReport.RelatorioTrabalho = item.RelatorioTrabalho;
					planReport.AssinaturaCliente = item.AssinaturaCliente;
					planReport.AssinaturaSie = item.AssinaturaSie;
					planReport.AssinaturaTecnico = item.AssinaturaTecnico;
					planReport.IdAssinaturaTecnico = item.UtilizadorAssinaturaTecnico!= null ?item.UtilizadorAssinaturaTecnico.Id : 0;
					planReport.AssinaturaClienteManual = item.AssinaturaClienteManual;
					planReport.AssinaturaSieIgualCliente = item.AssinaturaSieIgualCliente;
					planReport.CriadoPor = evolutionLoggedUser.Id;
					planReport.CriadoEm = Now;
					planReport.ActualizadoPor = evolutionLoggedUser.Id;
					planReport.ActualizadoEm = Now;
					planReport.Rotina = item.RotinaId != null ? (int) item.RotinaId : 1;
				}
				else
				{
					evolutionWEBContext.FichaManutencaoRelatorio.Add(new FichaManutencaoRelatorio
					{
						Om = order.No,
						IdEquipamento = item.IdEquipamento,
						Codigo = item.Codigo,
						Versao = item.Versao,
						Observacao = item.Observacao,
						RelatorioTrabalho = item.RelatorioTrabalho,
						EstadoFinal = item.EstadoFinal,			
						DataEstadoFinal = item.DataEstadoFinal,			
						CriadoPor = evolutionLoggedUser.Id,
						CriadoEm = Now,
						ActualizadoPor = evolutionLoggedUser.Id,
						ActualizadoEm = Now,
						AssinaturaCliente = item.AssinaturaCliente,
						AssinaturaSie = item.AssinaturaSie,
						AssinaturaTecnico = item.AssinaturaTecnico,
						IdAssinaturaTecnico = item.UtilizadorAssinaturaTecnico!= null ?item.UtilizadorAssinaturaTecnico.Id : 0,
						
						AssinaturaClienteManual = item.AssinaturaClienteManual,
						AssinaturaSieIgualCliente = item.AssinaturaSieIgualCliente,
						
						Rotina = item.RotinaId != null ? (int) item.RotinaId : 1
					});
					
					
			
					try
					{
						evolutionWEBContext.SaveChanges();
						planReport = evolutionWEBContext.FichaManutencaoRelatorio
							.Where(r => r.Om == order.No && r.IdEquipamento == item.IdEquipamento && 
							            (item.Codigo != null ? r.Codigo == item.Codigo : true) && (item.Versao != null ? r.Versao == item.Versao: true))
							.OrderByDescending(o => o.Id).FirstOrDefault();
					}
					catch{}
				}
				
				var planMaintenanceReport = evolutionWEBContext.FichaManutencaoRelatorioManutencao
					.Where(r =>  r.IdRelatorio == planReport.Id 
                );
				if(item.PlanMaintenance != null && item.PlanMaintenance.Count() > 0) {
					item.PlanMaintenance.ForEach(i =>
					{
						int IdManutencao;
						int.TryParse(i.IdManutencao.ToString(), out IdManutencao);

						var maintenanceReportLine = planMaintenanceReport
							.FirstOrDefault(r => r.IdRelatorio == planReport.Id && r.IdManutencao == IdManutencao);
						if (maintenanceReportLine == null)
						{
							evolutionWEBContext.FichaManutencaoRelatorioManutencao.Add(new FichaManutencaoRelatorioManutencao
							{       
								IdRelatorio = planReport.Id,
								IdManutencao = IdManutencao,
								ResultadoRotina = i.Resultado== null ? 0 : (int)i.Resultado,
								Observacoes = i.Observacoes
							});
						}
						else
						{
							maintenanceReportLine.ResultadoRotina = i.Resultado== null ? 0 : (int)i.Resultado;
							maintenanceReportLine.Observacoes = i.Observacoes;
						}
					});
				}
				var planQualityReport = evolutionWEBContext.FichaManutencaoRelatorioTestesQualitativos
					.Where(r => r.IdRelatorio == planReport.Id 
				);
				if(item.PlanQuality != null && item.PlanQuality.Count() > 0) {
					item.PlanQuality.ForEach(i =>
					{
						int IdTesteQualitativos;
						int.TryParse(i.IdTesteQualitativos.ToString(), out IdTesteQualitativos);

						var planQualityReportLine = planQualityReport
							.FirstOrDefault(r => r.IdRelatorio == planReport.Id && r.IdTesteQualitativos == IdTesteQualitativos);
						if (planQualityReportLine == null)
						{
							evolutionWEBContext.FichaManutencaoRelatorioTestesQualitativos.Add(new FichaManutencaoRelatorioTestesQualitativos
							{
								IdTesteQualitativos = IdTesteQualitativos,
								IdRelatorio = planReport.Id,
								ResultadoRotina = i.Resultado == null ? 0 : (int)i.Resultado,
								Observacoes = i.Observacoes
							});
						}
						else
						{
							planQualityReportLine.ResultadoRotina = i.Resultado== null ? 0 : (int)i.Resultado;
							planQualityReportLine.Observacoes = i.Observacoes;
						}
					});
				}

				if(item.Emms != null && item.Emms.Count() > 0) {
					var toRemove = evolutionWEBContext.FichaManutencaoRelatorioEquipamentosTeste
						.Where(r =>
							!item.Emms.Select(s => s.Id).Contains(r.IdEquipTeste)
							&& r.IdRelatorio == planReport.Id).ToList();
					
					if(toRemove!= null) {
						evolutionWEBContext.RemoveRange(
							toRemove	
						);
					}
				
					var planEmms = evolutionWEBContext.FichaManutencaoRelatorioEquipamentosTeste
						.Where(r => r.IdRelatorio == planReport.Id);
					
					item.Emms.ForEach(i =>
					{
						int IdEmm;
						int.TryParse(i.Id.ToString(), out IdEmm);

						var emmLine = planEmms
							.FirstOrDefault(r => r.IdRelatorio == planReport.Id && r.IdEquipTeste == IdEmm);
						if (emmLine == null)
						{
							evolutionWEBContext.FichaManutencaoRelatorioEquipamentosTeste.Add(new FichaManutencaoRelatorioEquipamentosTeste
							{
								IdEquipTeste = IdEmm,
								IdRelatorio = planReport.Id
							});
						}
						else
						{
							// update
						}
					});
				}
				
				if(item.Materials != null && item.Materials.Count() > 0) {
					evolutionWEBContext.RemoveRange(
						evolutionWEBContext.FichaManutencaoRelatorioMaterial
							.Where(r => !item.Materials.Select(s=> s.Id).Contains(r.Id)
							            && r.IdRelatorio == planReport.Id ).ToList()
					);
					var planMaterials = evolutionWEBContext.FichaManutencaoRelatorioMaterial
						.Where(r => r.IdRelatorio == planReport.Id);
					
					item.Materials.ForEach(i =>
					{
						int IdMaterial;
						int.TryParse(i.Id.ToString(), out IdMaterial);

						var materialLine = planMaterials
							.FirstOrDefault(r => r.IdRelatorio == planReport.Id && r.Id == IdMaterial);
						if (materialLine == null)
						{
							evolutionWEBContext.FichaManutencaoRelatorioMaterial.Add(new FichaManutencaoRelatorioMaterial
							{
								IdRelatorio = planReport.Id,
								Descricao = i.Descricao,
								Quantidade = i.Quantidade,
								FornecidoPor = i.FornecidoPor
							});
						}
						else
						{
							materialLine.Descricao = i.Descricao;
							materialLine.Quantidade = i.Quantidade;
							materialLine.FornecidoPor = i.FornecidoPor;
						}
					});
				}

				var planQuantityReport = evolutionWEBContext.FichaManutencaoRelatorioTestesQuantitativos
					.Where(r => r.IdRelatorio == planReport.Id 
                );
				if(item.PlanQuantity != null && item.PlanQuantity.Count() > 0) {
					item.PlanQuantity.ForEach(i =>
					{
						int IdTestesQuantitativos;
						int.TryParse(i.IdTestesQuantitativos.ToString(), out IdTestesQuantitativos);

						var planQuantityReportLine = planQuantityReport.FirstOrDefault(r => 
							r.IdRelatorio == planReport.Id && r.IdTestesQuantitativos == IdTestesQuantitativos);
						if (planQuantityReportLine == null)
						{
							evolutionWEBContext.FichaManutencaoRelatorioTestesQuantitativos.Add(new FichaManutencaoRelatorioTestesQuantitativos
							{
								IdTestesQuantitativos = IdTestesQuantitativos,
								IdRelatorio = planReport.Id,
								ResultadoRotina = i.Resultado,
								Observacoes = i.Observacoes
							});
						}
						else
						{
							planQuantityReportLine.ResultadoRotina = i.Resultado;
							planQuantityReportLine.Observacoes = i.Observacoes;
						}
					});
				}
				
			});
			
			try
			{
				evolutionWEBContext.SaveChanges();
			}
			catch (Exception ex)
			{

				var teste = ex;

			}

			return Json(true);
		}
		
	}

}

