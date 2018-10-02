using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic.Encomendas;
using Microsoft.AspNetCore.Mvc;
using static Hydra.Such.Data.Enumerations;
using Newtonsoft.Json.Linq;
using Hydra.Such.Data.ViewModel.Encomendas;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Hydra.Such.Portal.Configurations;
using Microsoft.Extensions.Options;
using Hydra.Such.Data;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Portal.Services;
using Hydra.Such.Data.Logic.Request;

namespace Hydra.Such.Portal.Controllers
{
    public class LinhasPreEncomendaController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations configws;

        public LinhasPreEncomendaController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs)
        {
            _config = appSettings.Value;
            configws = NAVWSConfigs.Value;
        }

        // GET: LinhasPreEncomenda
        public IActionResult LinhasPreEncomenda()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.PréEncomendas);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult DetalheLinhasPreEncomenda(string numLinhaPreEncomenda)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.PréEncomendas);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.No = numLinhaPreEncomenda == null ? "" : numLinhaPreEncomenda;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetAllLinhas()
        {
            List<LinhasPreEncomenda> result = DBEncomendas.GetAllLinhasPreEncomendaToList().Where(linha => linha.Tratada == false).ToList();
            List<LinhasPreEncomendaView> list = new List<LinhasPreEncomendaView>();
            int _contador = -1;
            foreach (LinhasPreEncomenda lin in result)
            {
                _contador += 1;
                list.Add(DBEncomendas.CastLinhasPreEncomendaToView(lin));
                list[_contador].NomeFornecedor_Show = DBNAV2017Supplier.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, list[_contador].NumFornecedor).Count > 0 ? DBNAV2017Supplier.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, list[_contador].NumFornecedor).FirstOrDefault().Name : string.Empty;
            }

            return Json(list);
        }

        [HttpPost]
        public JsonResult GetLinhasPreEncomendaDetails([FromBody] LinhasPreEncomendaView data)
        {
            try
            {
                if (data != null)
                {
                    LinhasPreEncomenda Linhas = DBEncomendas.GetLinhasPreEncomenda(data.NumLinhaPreEncomenda);

                    if (Linhas != null)
                    {
                        LinhasPreEncomendaView linhasView = DBEncomendas.CastLinhasPreEncomendaToView(Linhas);
                        linhasView.NomeFornecedor_Show = DBNAV2017Supplier.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, linhasView.NumFornecedor).Count > 0 ? DBNAV2017Supplier.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, linhasView.NumFornecedor).FirstOrDefault().Name : string.Empty;
                        return Json(linhasView);
                    }

                    return Json(new LinhasPreEncomendaView());
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult MarcarComoConsultaMercado([FromBody] List<LinhasPreEncomendaView> Linhas)
        {
            ErrorHandler result = new ErrorHandler
            {
                eReasonCode = 0,
                eMessage = "Os Registos foram atualizados com sucesso, estando agora com o Documento a Criar como 'Consulta Mercado'."
            };

            try
            {
                if (Linhas != null && Linhas.Count() > 0)
                {
                    UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.PréEncomendas);
                    if (UPerm.Update == true)
                    {
                        Linhas.ForEach(LinhaPreEncomenda =>
                        {
                            LinhaPreEncomenda.DocumentoACriar = 0; //Consulta Mercado
                            LinhaPreEncomenda.DataHoraModificacao = DateTime.Now;
                            LinhaPreEncomenda.UtilizadorModificacao = User.Identity.Name;

                            if (DBEncomendas.Update(DBEncomendas.CastLinhasPreEncomendaToDB(LinhaPreEncomenda)) == null)
                            {
                                result.eReasonCode = 3;
                                result.eMessage = "Ocorreu um erro ao atualizar o registo.";
                            }
                        });
                    }
                    else
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Não tem permissões para alterar o registo.";
                    }
                }
                else
                {
                    result.eReasonCode = 1;
                    result.eMessage = "Não foi possivel ler o registo.";
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult MarcarComoEncomenda([FromBody] List<LinhasPreEncomendaView> Linhas)
        {
            ErrorHandler result = new ErrorHandler
            {
                eReasonCode = 0,
                eMessage = "Os Registos foram atualizados com sucesso, estando agora com o Documento a Criar como 'Encomenda'."
            };

            try
            {
                if (Linhas != null && Linhas.Count() > 0)
                {
                    UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.PréEncomendas);
                    if (UPerm.Update == true)
                    {
                        Linhas.ForEach(LinhaPreEncomenda =>
                        {
                            LinhaPreEncomenda.DocumentoACriar = 1; //Encomenda
                            LinhaPreEncomenda.DataHoraModificacao = DateTime.Now;
                            LinhaPreEncomenda.UtilizadorModificacao = User.Identity.Name;

                            if (DBEncomendas.Update(DBEncomendas.CastLinhasPreEncomendaToDB(LinhaPreEncomenda)) == null)
                            {
                                result.eReasonCode = 3;
                                result.eMessage = "Ocorreu um erro ao atualizar o registo.";
                            }
                        });
                    }
                    else
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Não tem permissões para alterar o registo.";
                    }
                }
                else
                {
                    result.eReasonCode = 1;
                    result.eMessage = "Não foi possivel ler o registo.";
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult MarcarComoCriarDocumento([FromBody] List<LinhasPreEncomendaView> Linhas)
        {
            ErrorHandler result = new ErrorHandler
            {
                eReasonCode = 0,
                eMessage = "Os Registos foram atualizados com sucesso, estando agora marcadas para Criar Documento."
            };

            try
            {
                if (Linhas != null && Linhas.Count() > 0)
                {
                    UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.PréEncomendas);
                    if (UPerm.Update == true)
                    {
                        Linhas.ForEach(LinhaPreEncomenda =>
                        {
                            LinhaPreEncomenda.CriarDocumento = true;
                            LinhaPreEncomenda.DataHoraModificacao = DateTime.Now;
                            LinhaPreEncomenda.UtilizadorModificacao = User.Identity.Name;

                            if (DBEncomendas.Update(DBEncomendas.CastLinhasPreEncomendaToDB(LinhaPreEncomenda)) == null)
                            {
                                result.eReasonCode = 3;
                                result.eMessage = "Ocorreu um erro ao atualizar o registo.";
                            }
                        });
                    }
                    else
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Não tem permissões para alterar o registo.";
                    }
                }
                else
                {
                    result.eReasonCode = 1;
                    result.eMessage = "Não foi possivel ler o registo.";
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult MarcarComoNaoCriarDocumento([FromBody] List<LinhasPreEncomendaView> Linhas)
        {
            ErrorHandler result = new ErrorHandler
            {
                eReasonCode = 0,
                eMessage = "Os Registos foram atualizados com sucesso, estando agora marcadas para Não Criar Documento."
            };

            try
            {
                if (Linhas != null && Linhas.Count() > 0)
                {
                    UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.PréEncomendas);
                    if (UPerm.Update == true)
                    {
                        Linhas.ForEach(LinhaPreEncomenda =>
                        {
                            LinhaPreEncomenda.CriarDocumento = false;
                            LinhaPreEncomenda.DataHoraModificacao = DateTime.Now;
                            LinhaPreEncomenda.UtilizadorModificacao = User.Identity.Name;

                            if (DBEncomendas.Update(DBEncomendas.CastLinhasPreEncomendaToDB(LinhaPreEncomenda)) == null)
                            {
                                result.eReasonCode = 3;
                                result.eMessage = "Ocorreu um erro ao atualizar o registo.";
                            }
                        });
                    }
                    else
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Não tem permissões para alterar o registo.";
                    }
                }
                else
                {
                    result.eReasonCode = 1;
                    result.eMessage = "Não foi possivel ler o registo.";
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(null);
        }


        [HttpPost]
        public JsonResult UpdateLinha([FromBody] LinhasPreEncomendaView Linha)
        {
            ErrorHandler result = new ErrorHandler
            {
                eReasonCode = 0,
                eMessage = "Fornecedor actualizado com sucesso."
            };

            try
            {
                if (Linha != null && Linha.NumFornecedor != string.Empty)
                {
                    UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.PréEncomendas);
                    if (UPerm.Update == true)
                    {
                        LinhasPreEncomenda LinhaPreEncomenda = DBEncomendas.GetLinhasPreEncomenda(Linha.NumLinhaPreEncomenda);

                        LinhaPreEncomenda.NºFornecedor = Linha.NumFornecedor;
                        LinhaPreEncomenda.DataHoraModificação = DateTime.Now;
                        LinhaPreEncomenda.UtilizadorModificação = User.Identity.Name;

                        if (DBEncomendas.Update(LinhaPreEncomenda) == null)
                        {
                            result.eReasonCode = 3;
                            result.eMessage = "Ocorreu um erro ao atualizar o registo.";
                        }
                    }
                    else
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Não tem permissões para alterar o registo.";
                    }
                }
                else
                {
                    result.eReasonCode = 1;
                    result.eMessage = "Não foi possivel ler o registo.";
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(null);
        }



        [HttpPost]
        public JsonResult CriarEncomendaCabimento([FromBody] List<LinhasPreEncomendaView> item)
        {
            //if (item != null)
            //{
            //    try
            //    {
            //        RequisitionService serv = new RequisitionService(configws, HttpContext.User.Identity.Name);
            //        item = serv.CreatePurchaseOrderFor(item);


            //    }
            //    catch (Exception ex)
            //    {
            //        item.eReasonCode = 2;
            //        item.eMessage = "Ocorreu um erro ao criar encomenda de compra (" + ex.Message + ")";
            //    }
            //}
            //else
            //{
            //    item = new LinhasPreEncomendaView()
            //    {
            //        eReasonCode = 3,
            //        eMessage = "Não é possivel criar encomenda de compra. A requisição não pode ser nula."
            //    };
            //}
            //return Json(item);



            /*
             1º - filtrar os itens que chegam aqui, ficando apenas com os que são:
                --> Documento a Criar = Encomenda (1)
                --> Criar Documento = true
                --> Nº Encomenda Aberto = ''
                --> Nº Linha Encomenda Aberto = ''
            2º - Criar Encomenda
            3º - As linhas da Requisição devem ficar marcadas  com o Nº da Encomenda criada (cada item tem um Nº Linha Requisição)
            4º - Colocar as linhas com Tratadas = 1
             */

            ErrorHandler resultado = new ErrorHandler
            {
                eReasonCode = 1,
                eMessage = "Criada Encomenda com sucesso."
            };

            if (item != null)
            {
                try
                {
                    var list = item.Where(it => it.DocumentoACriar == 1).Where(it => it.CriarDocumento == true).Where(it => it.NumEncomendaAberto == string.Empty).Where(it => !it.NumLinhaEncomendaAberto.HasValue).ToList();

                    List<PurchOrderDTO> purchOrders = new List<PurchOrderDTO>();
                    
                    try
                    {
                        purchOrders = list.GroupBy(x =>
                                x.NumFornecedor,
                                x => x,
                                (key, items) => new PurchOrderDTO
                                {
                                    SupplierId = key,
                                    RequisitionId = list.Where(f => f.NumFornecedor == key).FirstOrDefault().NumRequisicao,
                                    CenterResponsibilityCode = list.Where(f => f.NumFornecedor == key).FirstOrDefault().CodigoCentroResponsabilidade,
                                    FunctionalAreaCode = list.Where(f => f.NumFornecedor == key).FirstOrDefault().CodigoAreaFuncional,
                                    RegionCode = list.Where(f => f.NumFornecedor == key).FirstOrDefault().CodigoRegiao,
                                    LocalMarketRegion = list.Where(f => f.NumFornecedor == key).FirstOrDefault().CodigoLocalizacao,
                                    Lines = items.Select(line => new PurchOrderLineDTO()
                                    {
                                        LineId = line.NumLinhaPreEncomenda,
                                        Type = null,
                                        Code = line.CodigoProduto,
                                        Description = line.DescricaoProduto,
                                        ProjectNo = line.NumProjeto,
                                        QuantityRequired = line.QuantidadeDisponibilizada,
                                        UnitCost = line.CustoUnitario,
                                        LocationCode = line.CodigoLocalizacao,
                                        OpenOrderNo = line.NumEncomendaAberto,
                                        OpenOrderLineNo = line.NumLinhaEncomendaAberto,
                                        CenterResponsibilityCode = line.CodigoCentroResponsabilidade,
                                        FunctionalAreaCode = line.CodigoAreaFuncional,
                                        RegionCode = line.CodigoRegiao,
                                        UnitMeasureCode = line.CodigoUnidadeMedida,
                                        VATBusinessPostingGroup = string.Empty,
                                        VATProductPostingGroup = string.Empty
                                    })
                                    .ToList()
                                })
                        .ToList();
                    }
                    catch
                    {
                        throw new Exception("Ocorreu um erro ao agrupar as linhas.");
                    }

                    if (purchOrders.Count() > 0)
                    {
                        purchOrders.ForEach(purchOrder =>
                        {
                            RequisitionViewModel requisition = DBRequest.GetById(purchOrder.RequisitionId).ParseToViewModel();

                            try
                            {
                                var result = CreateNAVPurchaseOrderFor(purchOrder);
                                if (result.CompletedSuccessfully)
                                {
                                    //Update req
                                    requisition.OrderNo = result.ResultValue;

                                    //Update Requisition Lines
                                    requisition.Lines.ForEach(line =>
                                    {
                                        line.CreatedOrderNo = result.ResultValue;
                                        line.UpdateUser = User.Identity.Name;
                                    });
                                    //Commit to DB
                                    var updatedReq = DBRequest.Update(requisition.ParseToDB(), true);
                                    //bool linesUpdated = DBRequestLine.Update(requisition.Lines.ParseToDB());
                                    //if (linesUpdated)
                                    if (updatedReq != null)
                                    {
                                        resultado.eMessages.Add(new TraceInformation(TraceType.Success, "Criada encomenda para o fornecedor núm. " + purchOrder.SupplierId + "; "));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                resultado.eMessages.Add(new TraceInformation(TraceType.Error, "Ocorreu um erro ao criar encomenda para o fornecedor núm. " + purchOrder.SupplierId + "; "));
                                resultado.eMessages.Add(new TraceInformation(TraceType.Exception, purchOrder.SupplierId + " " + ex.Message));
                            }

                            if (resultado.eMessages.Any(x => x.Type == TraceType.Error))
                            {
                                resultado.eReasonCode = 2;
                                //resultado.eMessage = "Ocorreram erros ao criar encomenda de compra.";

                                resultado.eMessage = "Ocorreram erros ao criar encomenda de compra." + Environment.NewLine + resultado.eMessages[resultado.eMessages.Count() - 1].Message;
                            }
                            else
                            {
                                resultado.eReasonCode = 1;
                                resultado.eMessage = "Encomenda de compra criada com sucesso.";
                            }
                        });
                    }
                    else
                    {
                        resultado.eReasonCode = 3;
                        resultado.eMessage = "Não existem linhas que cumpram os requisitos de validação do mercado local.";
                    }
                }
                catch (Exception ex)
                {
                    resultado.eReasonCode = -1;
                    resultado.eMessage = "Erro desconhecido.";
                }
            }

            return Json(resultado);
        }


        private GenericResult CreateNAVPurchaseOrderFor(PurchOrderDTO purchOrder)
        {
            GenericResult createPrePurchOrderResult = new GenericResult();

            Task<WSPurchaseInvHeader.Create_Result> createPurchaseHeaderTask = NAVPurchaseHeaderIntermService.CreateAsync(purchOrder, configws);
            createPurchaseHeaderTask.Wait();
            if (createPurchaseHeaderTask.IsCompletedSuccessfully)
            {
                createPrePurchOrderResult.ResultValue = createPurchaseHeaderTask.Result.WSPurchInvHeaderInterm.No;
                purchOrder.NAVPrePurchOrderId = createPrePurchOrderResult.ResultValue;

                Task<WSPurchaseInvLine.CreateMultiple_Result> createPurchaseLinesTask = NAVPurchaseLineService.CreateMultipleAsync(purchOrder, configws);
                createPurchaseLinesTask.Wait();
                if (createPurchaseLinesTask.IsCompletedSuccessfully)
                {
                    try
                    {
                        /*
                         *  Swallow errors at this stage as they will be managed in NAV
                         */
                        //Task<WSGenericCodeUnit.FxCabimento_Result> createPurchOrderTask = WSGeneric.CreatePurchaseOrder(purchOrder.NAVPrePurchOrderId, configws);
                        //createPurchOrderTask.Wait();
                        //if (createPurchOrderTask.IsCompletedSuccessfully)
                        //{
                        //    createPrePurchOrderResult.CompletedSuccessfully = true;
                        //}
                    }
                    catch (Exception ex) { }
                }
            }
            return createPrePurchOrderResult;
        }
    }
}