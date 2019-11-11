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
	public partial class MaintenanceOrdersController : Controller
	{

		[Route("/ordens-de-manutencao/ficha-de-manutencao"), HttpGet, AcceptHeader("application/json")]
		//[ResponseCache(Duration = 86400)]
		public ActionResult GetMaintenancePlans(int categoryId, string orderId, string equipmentIds, string marcaIds, string servicoIds)
		{
			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

			if (loggedUser == null) { return NotFound(); }

			var loggedUserCresps = suchDBContext.AcessosDimensões.Where(o => o.Dimensão == 3 && o.IdUtilizador == loggedUser.IdUtilizador).ToList();

			var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name);

			if (evolutionLoggedUser == null) { return NotFound(); }

			if (orderId == null || orderId == "") { return NotFound(); }
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
				MaintenanceResponsible = o.MaintenanceResponsible,
                OrderType = o.OrderType,
                Status = o.Status
            }).FirstOrDefault();
			if (order == null) { return NotFound(); }

			var maintenanceOrderLine = evolutionWEBContext.MaintenanceOrderLine.Where(r => r.MoNo == order.No);
			var ordemManutencaoLinha = evolutionWEBContext.OrdemManutencaoLinha.Where(r => r.No == order.No);

			var availableEquipmentsIds = maintenanceOrderLine.Select(s => s.IdEquipamento).ToList();
			availableEquipmentsIds.AddRange(ordemManutencaoLinha.Select(s => s.IdEquipamento).ToList());

			//obter campos de ficha de manutencao
			var codigo = evolutionWEBContext.FichaManutencao.Where(f => f.IdCategoria == categoryId).Select(f => f.Codigo).FirstOrDefault();
			if (codigo == null) { return NotFound(); }

			var planHeader = evolutionWEBContext.FichaManutencao.Where(f => f.Codigo == codigo).OrderByDescending(f => f.Versao).FirstOrDefault();
			if (planHeader == null) { return NotFound(); }

			var versao = planHeader.Versao;

			var planMaintenance = evolutionWEBContext.FichaManutencaoManutencao.Where(m => m.Codigo == codigo && m.Versao == versao)
			    .Select(p => new FichaManutencaoRelatorioManutencaoViewModel()
			    {
				    Descricao = p.Descricao,
				    IdManutencao = p.IdManutencao,
				    Codigo = p.Codigo,
				    Versao = p.Versao
			    }).ToList();
			var planQuality = evolutionWEBContext.FichaManutencaoTestesQualitativos.Where(m => m.Codigo == codigo && m.Versao == versao)
			    .Select(p => new FichaManutencaoTestesQualitativosViewModel()
			    {
				    Descricao = p.Descricao,
				    IdTesteQualitativos = p.IdTesteQualitativos,
				    Codigo = p.Codigo,
				    Versao = p.Versao
			    }).ToList();
			var planQuantity = evolutionWEBContext.FichaManutencaoTestesQuantitativos.Where(m => m.Codigo == codigo && m.Versao == versao)
			    .Select(p => new FichaManutencaoTestesQuantitativosViewModel()
			    {
				    Descricao = p.Descricao,
				    IdTestesQuantitativos = p.IdTestesQuantitativos,
				    UnidadeCampo1 = p.UnidadeCampo1,
				    Codigo = p.Codigo,
				    Versao = p.Versao
			    }).ToList();

			List<EquipamentMaintenancePlanViewModel> equipments;

			if (equipmentIds != null)
			{
				var equipmentIdsSplited = equipmentIds.Split(',');
				var _equipmentIds = new List<int>();
				equipmentIdsSplited.ToList().ForEach(item =>
				{
					int number;
					Int32.TryParse(item, out number);

					_equipmentIds.Add(number);
				});
				if (_equipmentIds == null || _equipmentIds.Count() < 1) { return NotFound(); }

				equipments = evolutionWEBContext.Equipamento
				.Where(e => availableEquipmentsIds.Contains(e.IdEquipamento) && _equipmentIds.Contains(e.IdEquipamento) && e.Categoria == categoryId).Select(e => new EquipamentMaintenancePlanViewModel()
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
					NumEquipamento = e.NumEquipamento,
					Codigo = codigo,
					Versao = versao,
					PlanMaintenance = planMaintenance,
					PlanQuality = planQuality,
					PlanQuantity = planQuantity,


				}).ToList();
			}
			else
			{
				var _marcaIds = new List<int>();
				if (marcaIds != null)
				{
					var marcaIdsSplited = marcaIds.Split(',');
					marcaIdsSplited.ToList().ForEach(item =>
					{
						int number;
						Int32.TryParse(item, out number);
						_marcaIds.Add(number);
					});
				}
				var _servicoIds = new List<int>();
				if (servicoIds != null)
				{
					var servicoIdsSplited = servicoIds.Split(',');
					servicoIdsSplited.ToList().ForEach(item =>
					{
						int number;
						Int32.TryParse(item, out number);
						_servicoIds.Add(number);
					});
				}

				equipments = evolutionWEBContext.Equipamento.Where(e => e.Categoria == categoryId
				&& (marcaIds == null ? true : _marcaIds.Contains(e.Marca))
				&& (servicoIds == null ? true : _servicoIds.Contains(e.IdServico))
				&& availableEquipmentsIds.Contains(e.IdEquipamento)).Select(e => new EquipamentMaintenancePlanViewModel()
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
					NumEquipamento = e.NumEquipamento,
					Codigo = codigo,
					Versao = versao,
					PlanMaintenance = planMaintenance,
					PlanQuality = planQuality,
					PlanQuantity = planQuantity
				}).ToList();
			}
			//validar premissoes

			//obter fichas de manutencao (reports)
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

				var planMaintenanceReport = evolutionWEBContext.FichaManutencaoRelatorioManutencao.Where(r => r.Om == order.No && r.IdEquipamento == item.IdEquipamento);
				item.PlanMaintenance.ForEach(i =>
				{
					int IdManutencao;
					int.TryParse(i.IdManutencao.ToString(), out IdManutencao);

					var maintenanceReportLine = planMaintenanceReport.FirstOrDefault(r => r.RotinaTipo == rotina && r.IdManutencao == IdManutencao);
					if (maintenanceReportLine == null)
					{
						evolutionWEBContext.FichaManutencaoRelatorioManutencao.Add(new FichaManutencaoRelatorioManutencao
						{
							Om = order.No,
							IdEquipamento = item.IdEquipamento,
							Codigo = i.Codigo,
							Versao = i.Versao,
							IdUtilizador = evolutionLoggedUser.Id,
							Data = DateTime.Now,
							IdManutencao = IdManutencao,
							RotinaTipo = rotina,
							ResultadoRotina = 0
						});
						i.Resultado = 0;
					}
					else
					{
						i.Resultado = maintenanceReportLine.ResultadoRotina;
					}
				});

				var planQualityReport = evolutionWEBContext.FichaManutencaoRelatorioTestesQualitativos.Where(r => r.Om == order.No && r.IdEquipamento == item.IdEquipamento && r.Codigo == codigo && r.Versao == versao);
				item.PlanQuality.ForEach(i =>
				{
					int IdTesteQualitativos;
					int.TryParse(i.IdTesteQualitativos.ToString(), out IdTesteQualitativos);

					var planQualityReportLine = planQualityReport.FirstOrDefault(r => r.RotinaTipo == rotina && r.IdTesteQualitativos == IdTesteQualitativos);
					if (planQualityReportLine == null)
					{
						evolutionWEBContext.FichaManutencaoRelatorioTestesQualitativos.Add(new FichaManutencaoRelatorioTestesQualitativos
						{
							Om = order.No,
							IdEquipamento = item.IdEquipamento,
							Codigo = codigo,
							Versao = versao,
							IdUtilizador = evolutionLoggedUser.Id,
							Data = DateTime.Now,
							IdTesteQualitativos = IdTesteQualitativos,
							RotinaTipo = rotina,
							ResultadoRotina = 0
						});
						i.Resultado = 0;
					}
					else
					{
						i.Resultado = planQualityReportLine.ResultadoRotina;
					}
				});

				var planQuantityReport = evolutionWEBContext.FichaManutencaoRelatorioTestesQuantitativos.Where(r => r.Om == order.No && r.IdEquipamento == item.IdEquipamento && r.Codigo == codigo && r.Versao == versao);
				item.PlanQuantity.ForEach(i =>
				{
					int IdTestesQuantitativos;
					int.TryParse(i.IdTestesQuantitativos.ToString(), out IdTestesQuantitativos);

					var planQuantityReportLine = planQuantityReport.FirstOrDefault(r => r.RotinaTipo == rotina && r.IdTestesQuantitativos == IdTestesQuantitativos);
					if (planQuantityReportLine == null)
					{
						evolutionWEBContext.FichaManutencaoRelatorioTestesQuantitativos.Add(new FichaManutencaoRelatorioTestesQuantitativos
						{
							Om = order.No,
							IdEquipamento = item.IdEquipamento,
							Codigo = codigo,
							Versao = versao,
							IdUtilizador = evolutionLoggedUser.Id,
							Data = DateTime.Now,
							IdTestesQuantitativos = IdTestesQuantitativos,
							RotinaTipo = rotina,
							//ResultadoRotina = true,
							ResultadoCampo1 = ""
						});
						i.Resultado = "";
					}
					else
					{
						i.Resultado = planQuantityReportLine.ResultadoCampo1;
					}
				});

				var planFinalStateReport = evolutionWEBContext.FichaManutencaoRelatorioRelatorio.Where(r => r.Om == order.No && r.IdEquipamento == item.IdEquipamento && r.Codigo == codigo && r.Versao == versao).OrderByDescending(o => o.Id).FirstOrDefault();
				if (planFinalStateReport != null)
				{
					item.EstadoFinal = planFinalStateReport.EstadoFinal;
                    item.Observacao = planFinalStateReport.Observacao;
                    item.AssinaturaCliente = planFinalStateReport.AssinaturaCliente;
                    item.AssinaturaSie = planFinalStateReport.AssinaturaSie;
                    item.AssinaturaTecnico = planFinalStateReport.AssinaturaTecnico;
                    item.UtilizadorAssinaturaTecnico = evolutionWEBContext.Utilizador.Where(u => u.Id == planFinalStateReport.IdAssinaturaTecnico).FirstOrDefault();
                }
				else
				{
					evolutionWEBContext.FichaManutencaoRelatorioRelatorio.Add(new FichaManutencaoRelatorioRelatorio
					{
						Om = order.No,
						IdEquipamento = item.IdEquipamento,
						Codigo = codigo,
						Versao = versao,
						Data = DateTime.Now,
						EstadoFinal = 0,
						Relatorio = "",
                        CriadoPor = evolutionLoggedUser.Id,
                        CriadoEm = DateTime.Now,
                        ActualizadoPor = evolutionLoggedUser.Id,
                        ActualizadoEm = DateTime.Now

                    });
					item.EstadoFinal = 0;
				}

				try { evolutionWEBContext.SaveChanges(); }
				catch (Exception ex) { }

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
				equipments,
				planMaintenance,
				planQuality,
				planQuantity,
                currentUser = evolutionLoggedUser
            });
		}
	}
}

