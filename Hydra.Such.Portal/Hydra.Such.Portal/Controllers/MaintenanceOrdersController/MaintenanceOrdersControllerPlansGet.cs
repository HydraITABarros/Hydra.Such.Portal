using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Portal.Filters;
using Hydra.Such.Data.Evolution.DatabaseReference;
using Hydra.Such.Portal.ViewModels;
using Hydra.Such.Portal.Extensions;
using Microsoft.OData.Edm;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;

namespace Hydra.Such.Portal.Controllers
{
	public partial class MaintenanceOrdersController : Controller
	{

		[Route("/ordens-de-manutencao/ficha-de-manutencao"), HttpGet, AcceptHeader("application/json")]
		//[ResponseCache(Duration = 86400)]
		public ActionResult GetMaintenancePlans(int categoryId, string orderId, string equipmentIds, string marcaIds,
			string servicoIds)
		{
			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

			if (loggedUser == null)
			{
				return NotFound();
			}

			var loggedUserCresps = suchDBContext.AcessosDimensões
				.Where(o => o.Dimensão == 3 && o.IdUtilizador == loggedUser.IdUtilizador).ToList();

			var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name);

			if (evolutionLoggedUser == null)
			{
				return NotFound();
			}

			if (orderId == null || orderId == "")
			{
				return NotFound();
			}
			
			var order = GetOrderByOrderId(orderId);
			if (order == null) { return NotFound(); }
			
			var maintenanceOrderLine = evolutionWEBContext.MaintenanceOrderLine.Where(r => r.MoNo == order.No);
			var ordemManutencaoLinha = evolutionWEBContext.OrdemManutencaoLinha.Where(r => r.No == order.No);
			
			var OrderLines = maintenanceOrderLine
				.ToList().Select(s => new OrderLineRoutine(){ IdEquipamento = s.IdEquipamento, IdRotina = s.IdRotina }).ToList()
				.Concat(ordemManutencaoLinha.ToList().Select(s => new OrderLineRoutine() { IdEquipamento = s.IdEquipamento, IdRotina = s.IdRotina}).ToList());
			
			var availableEquipmentsIds = OrderLines.Select(s=>s.IdEquipamento);
			
			var isCurative = !order.isPreventive;
			var isSimplified = availableEquipmentsIds == null || availableEquipmentsIds.Count() == 0 || categoryId == 0;

			var planHeader = GetPlanHeader(order, categoryId);
			
			if (planHeader == null && !isCurative && !isSimplified) { return NotFound(); }

			var codigo = planHeader?.Codigo;
			
			var equipments = new List<EquipamentMaintenancePlanViewModel>();
			var planMaintenance = new List<FichaManutencaoRelatorioManutencaoViewModel>();
			var planQuality = new List<FichaManutencaoTestesQualitativosViewModel>();
			var planQuantity = new List<FichaManutencaoTestesQuantitativosViewModel>();
			
			if(!isCurative && !isSimplified) {
				var versao = planHeader.Versao;
				planMaintenance = getMaintenancePlanMaintenanceByCodigoVersao(codigo, versao);
				planQuality = getMaintenancePlanQualityByCodigoVersao(codigo, versao);
				planQuantity = getMaintenancePlanQuantityByCodigoVersao(codigo, versao);
				equipments = getEquipmentsByEquipmentIdsOrByMarcaIdsAndServicoIds(
					 equipmentIds, marcaIds, servicoIds, codigo, versao, categoryId, planMaintenance, planQuality, planQuantity, availableEquipmentsIds
					);
				
				//if (equipments == null || equipments.Count() < 1) { return NotFound(); }
				fillEquipmentsPlans( equipments, orderId, order, codigo, versao, OrderLines, evolutionLoggedUser );
			}
			
			if (equipments.Count() == 0)
			{
				equipments.Add(new EquipamentMaintenancePlanViewModel()
				{
					EstadoFinal = 0,
					IdEquipamento = 0,
					NumEquipamento = "",
					RelatorioTrabalho = "",
					Observacao = "",
					PlanMaintenance = new List<FichaManutencaoRelatorioManutencaoViewModel>(),
					PlanQuantity = new List<FichaManutencaoTestesQuantitativosViewModel>(),
					PlanQuality = new List<FichaManutencaoTestesQualitativosViewModel>(),
					Om = order.No
				});
				
				fillEquipmentsPlans( equipments, orderId, order, codigo, "", OrderLines, evolutionLoggedUser );
				isSimplified = true;
			}
			
			if (isCurative || isSimplified)
			{
				equipments.Splice(1, equipments.Count() - 1);
					
				var planReport = evolutionWEBContext.FichaManutencaoRelatorio
					.Where(r => r.Om == orderId && (r.IdEquipamento == null || r.IdEquipamento == 0 ))
					.OrderByDescending(o => o.Id).FirstOrDefault();
				
				if (planReport != null)
				{
					equipments[0].RelatorioTrabalho = planReport.RelatorioTrabalho ?? "";
					equipments[0].Observacao = planReport.Observacao ?? "";
					equipments[0].EstadoFinal = planReport.EstadoFinal ?? 0;
					equipments[0].IdEquipamento = planReport.IdEquipamento ?? 0;
					equipments[0].NumEquipamento = "";
				}
				else
				{
					planReport = new FichaManutencaoRelatorio
					{
						Om = order.No,
						EstadoFinal = 0,
						CriadoPor = evolutionLoggedUser.Id,
						CriadoEm = DateTime.Now,
						ActualizadoPor = evolutionLoggedUser.Id,
						ActualizadoEm = DateTime.Now,
						IdEquipamento = 0

					};
					evolutionWEBContext.FichaManutencaoRelatorio.Add(planReport);
					equipments[0].EstadoFinal = 0;
					equipments[0].RelatorioTrabalho = "";
					equipments[0].Observacao = "";
					equipments[0].IdEquipamento = 0;
					equipments[0].NumEquipamento = "";
				}

				if (isCurative)
				{
					equipments[0].TipoContactoClienteInicial = order.TipoContactoClienteInicial;
					equipments[0].NoDocumentoContactoInicial = order.NoDocumentoContactoInicial;
					equipments[0].DataPedidoReparação = order.DataPedidoReparação;
					equipments[0].NoCompromisso = order.NoCompromisso;
					equipments[0].ReferenciaEncomenda = order.ReferenciaEncomenda;
						
					equipments[0].MalfunctionDescription = order.Description;
				}
				
				try
				{
					evolutionWEBContext.SaveChanges();
				}
				catch (Exception ex)
				{
					var debug = ex;
				}
			}
			
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
				planMaintenance = planMaintenance/*.Where(i => i.RotinasList.Contains(rotinaId))*/,
				planQuality=planQuality/*.Where(i => i.RotinasList.Contains(rotinaId))*/,
				planQuantity=planQuantity/*.Where(i => i.RotinasList.Contains(rotinaId))*/,
                currentUser = evolutionLoggedUser,
                isCurative,
                isSimplified
            });
		}

		#region GetPlanHelperFunctions

		private FichaManutencao GetPlanHeader(MaintenanceOrderViewModel order, int categoryId)
		{
			var tipo = evolutionWEBContext.EquipCategoria.FirstOrDefault(e => e.IdCategoria == categoryId)?.IdTipo;
			//if (codigo == null && !isCurative && !isSimplified) { return NotFound(); }

			var planHeader = evolutionWEBContext.FichaManutencao
				.Where(f => f.IdTipo == tipo && order.OrderDate >= f.PeriodoInicio && order.OrderDate <= f.PeriodoFim)
				.OrderByDescending(f => f.Versao).FirstOrDefault();

			return planHeader;
		}
		
		private MaintenanceOrderViewModel GetOrderByOrderId(string orderId)
		{
			
			var preventiveCodes = evolutionWEBContext.MaintenanceCatalog.Where(f => f.ManutPreventiva == 1).Select(f => f.Code).ToList();
			
			return evolutionWEBContext.MaintenanceOrder.Where(m => m.No == orderId).ToList().Select(o =>
				new MaintenanceOrderViewModel()
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
					isPreventive = preventiveCodes.Contains(o.OrderType),
					ResponsibleEmployee = o.ResponsibleEmployee,
					MaintenanceResponsible = o.MaintenanceResponsible,
					OrderType = o.OrderType,
					Status = o.Status,
					TipoContactoClienteInicial = o.TipoContactoClienteInicial,
					NoDocumentoContactoInicial = o.NoDocumentoContactoInicial,
					DataPedidoReparação = o.DataPedidoReparação,
					NoCompromisso = o.NoCompromisso,
					ReferenciaEncomenda = o.ReferenciaEncomenda
				}).FirstOrDefault();
		}
		private List<FichaManutencaoRelatorioManutencaoViewModel> getMaintenancePlanMaintenanceByCodigoVersao(string codigo,string versao)
		{
			return evolutionWEBContext.FichaManutencaoManutencao
				.Where(m => m.Codigo == codigo && m.Versao == versao)
				.Select(p => new FichaManutencaoRelatorioManutencaoViewModel()
				{
					Descricao = p.Descricao,
					IdManutencao = p.IdManutencao,
					Codigo = p.Codigo,
					Versao = p.Versao,
					Rotinas = p.Rotinas
				}).ToList()
				.Select(p => new FichaManutencaoRelatorioManutencaoViewModel()
				{
					Descricao = p.Descricao,
					IdManutencao = p.IdManutencao,
					Codigo = p.Codigo,
					Versao = p.Versao,
					Rotinas = p.Rotinas,
					RotinasList = p.Rotinas.Split(";").Select(Int32.Parse).ToList()
				}).ToList();
		}
		
		private List<FichaManutencaoTestesQualitativosViewModel> getMaintenancePlanQualityByCodigoVersao(string codigo,string versao)
		{
			return evolutionWEBContext.FichaManutencaoTestesQualitativos
				.Where(m => m.Codigo == codigo && m.Versao == versao)
				.Select(p => new FichaManutencaoTestesQualitativosViewModel()
				{
					Descricao = p.Descricao,
					IdTesteQualitativos = p.IdTesteQualitativos,
					Codigo = p.Codigo,
					Versao = p.Versao,
					Rotinas = p.Rotinas
				}).ToList().Select(p => new FichaManutencaoTestesQualitativosViewModel()
				{
					Descricao = p.Descricao,
					IdTesteQualitativos = p.IdTesteQualitativos,
					Codigo = p.Codigo,
					Versao = p.Versao,
					Rotinas = p.Rotinas,
					RotinasList = p.Rotinas.Split(";").Select(Int32.Parse).ToList()
				}).ToList();
		}
		
		private List<FichaManutencaoTestesQuantitativosViewModel> getMaintenancePlanQuantityByCodigoVersao(string codigo,string versao)
		{
			return evolutionWEBContext.FichaManutencaoTestesQuantitativos.Where(m => m.Codigo == codigo && m.Versao == versao)
				.Select(p => new FichaManutencaoTestesQuantitativosViewModel()
				{
					Descricao = p.Descricao,
					IdTestesQuantitativos = p.IdTestesQuantitativos,
					UnidadeCampo1 = p.UnidadeCampo1,
					Codigo = p.Codigo,
					Versao = p.Versao,
					Rotinas = p.Rotinas
				}).ToList().Select(p => new FichaManutencaoTestesQuantitativosViewModel()
				{
					Descricao = p.Descricao,
					IdTestesQuantitativos = p.IdTestesQuantitativos,
					UnidadeCampo1 = p.UnidadeCampo1,
					Codigo = p.Codigo,
					Versao = p.Versao,
					Rotinas = p.Rotinas,
					RotinasList = p.Rotinas.Split(";").Select(Int32.Parse).ToList() 
				}).ToList();
		}

		private List<EquipamentMaintenancePlanViewModel> getEquipmentsByEquipmentIdsOrByMarcaIdsAndServicoIds(
			string equipmentIds, 
			string marcaIds, 
			string servicoIds, 
			string codigo, 
			string versao, 
			int categoryId,
			List<FichaManutencaoRelatorioManutencaoViewModel> planMaintenance, 
			List<FichaManutencaoTestesQualitativosViewModel> planQuality, 
			List<FichaManutencaoTestesQuantitativosViewModel> planQuantity, 
			IEnumerable<int?> availableEquipmentsIds
			)
		{
			
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
				if (_equipmentIds == null || _equipmentIds.Count() < 1)
				{
					return null;
				}

				equipments = evolutionWEBContext.Equipamento
				.Where(e => 
					availableEquipmentsIds.Contains(e.IdEquipamento) && 
					_equipmentIds.Contains(e.IdEquipamento) && 
					e.Categoria == categoryId)
				.Select(e => new EquipamentMaintenancePlanViewModel()
				{
					IdEquipamento = e.IdEquipamento,
					Sala = e.Sala,
					IdServico = e.IdServico,
					IdCliente = e.IdCliente,
					Categoria = e.Categoria,
					Marca = e.Marca,
					Modelo = e.Modelo,
					MarcaText = e.MarcaText,
					ModeloText = e.ModeloText,
					NumSerie = e.NumSerie,
					NumInventario = e.NumInventario,
					NumEquipamento = e.NumEquipamento,
					Codigo = codigo,
					Versao = versao,
					PlanMaintenance = planMaintenance.Select(s=>s.Clone()).ToList(),
					PlanQuality = planQuality.Select(s=>s.Clone()).ToList(),
					PlanQuantity = planQuantity.Select(s=>s.Clone()).ToList()
					
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
					MarcaText = e.MarcaText,
					ModeloText = e.ModeloText,
					Modelo = e.Modelo,
					NumSerie = e.NumSerie,
					NumInventario = e.NumInventario,
					NumEquipamento = e.NumEquipamento,
					Codigo = codigo,
					Versao = versao,
					PlanMaintenance = planMaintenance.Select(s=>s.Clone()).ToList(),
					PlanQuality = planQuality.Select(s=>s.Clone()).ToList(),
					PlanQuantity = planQuantity.Select(s=>s.Clone()).ToList()
				}).ToList();
			}

			return equipments;
		}

		private void fillEquipmentsPlans(
			List<EquipamentMaintenancePlanViewModel> equipments, 
			string orderId, 
			MaintenanceOrderViewModel order, 
			string codigo,
			string versao,
			IEnumerable<OrderLineRoutine> OrderLines,
			Utilizador evolutionLoggedUser
			)
		{
			
			int? rotinaId = 1;

			var index = 0;
			
			equipments.ForEach((item) =>
			{
				item.Om = orderId;
				
				if (item.Marca != 1 && (item.MarcaText == null || item.MarcaText.Length == 0))
				{
					var marca = evolutionWEBContext.EquipMarca.FirstOrDefault(m => m.IdMarca == item.Marca);
					if (marca != null)
					{
						item.MarcaText = marca.Nome;
					}
				}

				if (item.Modelo != 1 && (item.ModeloText == null || item.ModeloText.Length == 0))
				{
					int? modeloType = evolutionWEBContext.EquipModelo.Where(m => m.IdModelo == item.Modelo).Select(m => m.IdModelos).FirstOrDefault();
					if (modeloType != null)
					{
						item.ModeloText = evolutionWEBContext.Modelos.Where(m => m.IdModelos == modeloType).Select(n => n.Nome).FirstOrDefault();
					}
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
				
				var planReport = evolutionWEBContext.FichaManutencaoRelatorio
					.Where(r => r.Om == order.No && r.IdEquipamento == item.IdEquipamento && r.Codigo == codigo && r.Versao == versao)
					.OrderByDescending(o => o.Id).FirstOrDefault();
				
				if (index == 0 && planReport != null)
				{
					rotinaId = planReport.Rotina;
				} else if (index == 0 && planReport == null)
				{
					var line = OrderLines.FirstOrDefault(o => o.IdEquipamento == item.Id);
					rotinaId = line != null && line.IdRotina != null ? (int)line.IdRotina : 1;
				}
				index++;
				
				if (planReport != null)
				{
					item.Id = planReport.Id;
					item.EstadoFinal = planReport.EstadoFinal;
					item.Observacao = planReport.Observacao;
					item.AssinaturaCliente = planReport.AssinaturaCliente;
					item.AssinaturaSie = planReport.AssinaturaSie;
					item.AssinaturaTecnico = planReport.AssinaturaTecnico;
					item.UtilizadorAssinaturaTecnico = evolutionWEBContext.Utilizador.Where(u => u.Id == planReport.IdAssinaturaTecnico).FirstOrDefault();
					item.AssinaturaClienteManual = planReport.AssinaturaClienteManual;
					item.AssinaturaSieIgualCliente = planReport.AssinaturaSieIgualCliente;
					item.RotinaId = rotinaId;
				}
				else
				{
					planReport = new FichaManutencaoRelatorio
					{
						Om = order.No,
						IdEquipamento = item.IdEquipamento,
						Codigo = codigo,
						Versao = versao,
						EstadoFinal = 0,
						CriadoPor = evolutionLoggedUser.Id,
						CriadoEm = DateTime.Now,
						ActualizadoPor = evolutionLoggedUser.Id,
						ActualizadoEm = DateTime.Now,
						Rotina = rotinaId

					};
					evolutionWEBContext.FichaManutencaoRelatorio.Add(planReport);
					item.EstadoFinal = 0;
				}
				
				try
				{
					evolutionWEBContext.SaveChanges();
				}
				catch (Exception ex)
				{
					var debug = ex;
				}

				var planMaintenanceReport = evolutionWEBContext.FichaManutencaoRelatorioManutencao
					.Where(r => r.IdRelatorio == planReport.Id).ToList();
				item.PlanMaintenance.ForEach(i =>
				{
					/*
					if (!i.RotinasList.Contains(rotinaId))
					{
						return;
					}
					*/
					
					int IdManutencao;
					int.TryParse(i.IdManutencao.ToString(), out IdManutencao);
					
					FichaManutencaoRelatorioManutencao maintenanceReportLine = null;
					if(planMaintenanceReport != null) {
						maintenanceReportLine = planMaintenanceReport
											.FirstOrDefault(r => r.IdManutencao == IdManutencao && r.IdRelatorio == planReport.Id);
					}
					if (maintenanceReportLine == null)
					{
						evolutionWEBContext.FichaManutencaoRelatorioManutencao.Add(new FichaManutencaoRelatorioManutencao
						{
							IdRelatorio = planReport.Id,
							Observacoes = "",
							IdManutencao = IdManutencao,
							ResultadoRotina = 1
						});
						i.Resultado = 1;
					}
					else
					{
						i.Resultado = maintenanceReportLine.ResultadoRotina;
						i.Observacoes = maintenanceReportLine.Observacoes;
					}
				});

				var planQualityReport = evolutionWEBContext.FichaManutencaoRelatorioTestesQualitativos
					.Where(r => r.IdRelatorio == planReport.Id).ToList();
				item.PlanQuality.ForEach(i =>
				{
					/*	
					if (!i.RotinasList.Contains(rotinaId))
					{
						return;
					}
					*/
					
					int IdTesteQualitativos;
					int.TryParse(i.IdTesteQualitativos.ToString(), out IdTesteQualitativos);

					FichaManutencaoRelatorioTestesQualitativos planQualityReportLine = null;
					if (planQualityReport != null)
					{
						planQualityReportLine = planQualityReport
							.FirstOrDefault(r =>
								r.IdRelatorio == planReport.Id && r.IdTesteQualitativos == IdTesteQualitativos);
					}

					if (planQualityReportLine == null)
					{
						evolutionWEBContext.FichaManutencaoRelatorioTestesQualitativos.Add(new FichaManutencaoRelatorioTestesQualitativos
						{
							IdRelatorio = planReport.Id,
							Observacoes = "",
							IdTesteQualitativos = IdTesteQualitativos,
							ResultadoRotina = 1
						});
						i.Resultado = 1;
					}
					else
					{
						i.Resultado = planQualityReportLine.ResultadoRotina;
						i.Observacoes = planQualityReportLine.Observacoes;
					}
				});

				var planQuantityReport = evolutionWEBContext.FichaManutencaoRelatorioTestesQuantitativos
					.Where(r => r.IdRelatorio == planReport.Id).ToList();
				item.PlanQuantity.ForEach(i =>
				{
					/*
					if (!i.RotinasList.Contains(rotinaId))
					{
						return;
					}
					*/
					
					int IdTestesQuantitativos;
					int.TryParse(i.IdTestesQuantitativos.ToString(), out IdTestesQuantitativos);

					FichaManutencaoRelatorioTestesQuantitativos planQuantityReportLine = null;
					if (planQuantityReport != null)
					{
						planQuantityReportLine = planQuantityReport
						.FirstOrDefault(r => r.IdRelatorio == planReport.Id && r.IdTestesQuantitativos == IdTestesQuantitativos);
					}
					if (planQuantityReportLine == null)
					{
						evolutionWEBContext.FichaManutencaoRelatorioTestesQuantitativos.Add(new FichaManutencaoRelatorioTestesQuantitativos
						{
							IdRelatorio = planReport.Id,
							Observacoes = "",
							IdTestesQuantitativos = IdTestesQuantitativos,
							ResultadoRotina = ""
						});
						i.Resultado = "";
					}
					else
					{
						i.Resultado = planQuantityReportLine.ResultadoRotina;
						i.Observacoes = planQuantityReportLine.Observacoes;
					}
				});
				
				if (item.Emms == null)
				{
					item.Emms = new List<FichaManutencaoEmmEquipamentosViewModel>();
				}
				var emms = evolutionWEBContext.FichaManutencaoRelatorioEquipamentosTeste
					.Where(r => r.IdRelatorio == planReport.Id).ToList();
				emms.ForEach(i =>
				{
					var emm = evolutionWEBContext.EmmEquipamentos.Where(e => e.Id == i.IdEquipTeste).Select(s=>new FichaManutencaoEmmEquipamentosViewModel()
					{
						Id = s.Id,
						IdMarca = s.IdMarca,
						IdModelo = s.IdModelo,
						IdTipo = s.IdTipo,
						MarcaText = "",
						ModeloText = "",
						NumSerie = s.NumSerie,
						TipoDescricao = s.TipoDescricao
					}).FirstOrDefault();

					var marca = evolutionWEBContext.EquipMarca.FirstOrDefault(m => m.IdMarca == emm.IdMarca);
					
					emm.MarcaText = marca != null ? marca.Nome : "";
					
					var modeloType = evolutionWEBContext.EquipModelo.FirstOrDefault(m => m.IdModelo == emm.IdModelo);
					if(modeloType!= null) {
						var modelo = evolutionWEBContext.Modelos.FirstOrDefault(m => m.IdModelos == modeloType.IdModelos);
						emm.ModeloText = modelo != null ? modelo.Nome : "";
					}
					
					item.Emms.Add(emm);
				});

				item.Materials = new List<FichaManutencaoRelatorioMaterialViewModel>();
				
				item.Materials = evolutionWEBContext.FichaManutencaoRelatorioMaterial
					.Where(r => r.IdRelatorio == planReport.Id)
					.Select( s => new FichaManutencaoRelatorioMaterialViewModel()
						{
							Id = s.Id,
							Descricao = s.Descricao,
							Quantidade = s.Quantidade,
							FornecidoPor = s.FornecidoPor
						}
						)
					.ToList();
				
				try
				{
					evolutionWEBContext.SaveChanges();
				}
				catch (Exception ex)
				{
					var debug = ex;
				}

			});
		}
		
		#endregion
		
	}

	public class OrderLineRoutine
	{
		public int? IdEquipamento { get; set; }
		public int? IdRotina { get; set; }
	}
	
}

