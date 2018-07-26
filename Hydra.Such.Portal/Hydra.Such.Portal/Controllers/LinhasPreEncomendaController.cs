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

namespace Hydra.Such.Portal.Controllers
{
    public class LinhasPreEncomendaController : Controller
    {
        private readonly NAVConfigurations _config;

        public LinhasPreEncomendaController(IOptions<NAVConfigurations> appSettings)
        {
            _config = appSettings.Value;
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
            List<LinhasPreEncomenda> result = DBEncomendas.GetAllLinhasPreEncomendaToList();
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
                            LinhaPreEncomenda.DocumentoaCriar = 0; //Consulta Mercado
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
                            LinhaPreEncomenda.DocumentoaCriar = 1; //Encomenda
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


    }
}