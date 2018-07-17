using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic.Telemoveis;
using Microsoft.AspNetCore.Mvc;
using static Hydra.Such.Data.Enumerations;
using Newtonsoft.Json.Linq;
using Hydra.Such.Data.ViewModel.Telemoveis;

namespace Hydra.Such.Portal.Controllers
{
    public class TelemoveisEquipamentosController : Controller
    {
        public IActionResult TelemoveisEquipamentos()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.Telemoveis);

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

        public IActionResult DetalheTelemoveisEquipamentos([FromQuery] string tipo, string imei)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.Telemoveis);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.tipo = tipo;
                ViewBag.imei = imei;
                ViewBag.tipo_desc = int.Parse(tipo != null ? tipo : "0") == 0 ? "Equipamento" : "Placa de Rede";
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetAllEquipamentos()
        {
            List<TelemoveisEquipamentos> result = DBTelemoveis.GetAllTelemoveisEquipamentosToList();
            List<TelemoveisEquipamentosView> list = new List<TelemoveisEquipamentosView>();

            foreach (TelemoveisEquipamentos tel in result)
            {
                list.Add(DBTelemoveis.CastTelemoveisEquipamentosToView(tel));
            }

            //return Json(result);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetEquipamentosPorTipo([FromBody] JObject requestParams)
        {
            int tipo = int.Parse(requestParams["tipo"].ToString());

            List<TelemoveisEquipamentos> result = DBTelemoveis.GetAllTelemoveisEquipamentosTypeToList(tipo);

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetEquipamentoDetails([FromBody] TelemoveisEquipamentosView data)
        {
            try
            {
                if (data != null)
                {
                    TelemoveisEquipamentos telemoveisEquipamentos = DBTelemoveis.GetTelemoveisEquipamentos(data.Tipo, data.Imei);
                    
                    if (telemoveisEquipamentos != null)
                    {
                        TelemoveisEquipamentosView telemoveisEquipamentosView = DBTelemoveis.CastTelemoveisEquipamentosToView(telemoveisEquipamentos);
                        
                        return Json(telemoveisEquipamentosView);
                    }

                    return Json(new TelemoveisEquipamentosView());
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return Json(false);
        }


        [HttpPost]
        public JsonResult CreateTelemoveisEquipamentos([FromBody] TelemoveisEquipamentosView item)
        {
            if (item != null)
            {
                //Verificar se existe chave única tipo + imei
                TelemoveisEquipamentos telemoveisEquipamentos = DBTelemoveis.GetTelemoveisEquipamentos(item.Tipo, item.Imei);

                if (telemoveisEquipamentos != null)
                {
                    string Tipo_Desc = item.Tipo == 0 ? "Equipamento" : "Placa de Rede";
                    item.eReasonCode = -1;
                    item.eMessage = string.Format("Já existe um equipamento do tipo '{0}' com o IMEI/Nº Série '{1}'", Tipo_Desc, item.Imei);
                }
                else
                {
                    TelemoveisEquipamentos novo = new TelemoveisEquipamentos()
                    {
                        Tipo = item.Tipo,
                        Imei = item.Imei,
                        Marca = item.Marca,
                        Modelo = item.Modelo,
                        Estado = item.Estado,
                        Cor = item.Cor,
                        Observacoes = item.Observacoes,
                        DataRecepcao = item.DataRecepcao,
                        Documento = item.Documento,
                        DocumentoRecepcao = item.DocumentoRecepcao,
                        Utilizador = item.Utilizador,
                        DataAlteracao = item.DataAlteracao,
                        DevolvidoBk = item.DevolvidoBk,
                        NumEmpregadoComprador = item.NumEmpregadoComprador,
                        NomeComprador = item.NomeComprador,
                        Devolvido = item.Devolvido,
                        UtilizadorCriacao = User.Identity.Name,
                        DataHoraCriacao = DateTime.Now
                    };

                    try
                    {
                        DBTelemoveis.Create(novo);
                    }
                    catch
                    {
                        item.eReasonCode = -1;
                        item.eMessage = "Ocorreu um erro ao criar o Equipamento!";
                        return Json(item);
                    }

                    telemoveisEquipamentos = DBTelemoveis.GetTelemoveisEquipamentos(item.Tipo, item.Imei);
                    item = DBTelemoveis.CastTelemoveisEquipamentosToView(telemoveisEquipamentos);

                    item.eReasonCode = 1;
                    item.eMessage = "Equipamento criado com sucesso!";
                }
            }

            return Json(item);
        }


        [HttpPost]
        public JsonResult UpdateTelemoveisEquipamentos([FromBody] TelemoveisEquipamentosView item)
        {
            if (item != null)
            {
                //Verificar se existe chave única tipo + imei
                TelemoveisEquipamentos telemoveisEquipamentos = DBTelemoveis.GetTelemoveisEquipamentos(item.Tipo, item.Imei);

                if (telemoveisEquipamentos != null)
                {
                    telemoveisEquipamentos.Marca = item.Marca;
                    telemoveisEquipamentos.Modelo = item.Modelo;
                    telemoveisEquipamentos.Estado = item.Estado;
                    telemoveisEquipamentos.Cor = item.Cor;
                    telemoveisEquipamentos.Observacoes = item.Observacoes;
                    telemoveisEquipamentos.DataRecepcao = item.DataRecepcao;
                    telemoveisEquipamentos.Documento = item.Documento;
                    telemoveisEquipamentos.DocumentoRecepcao = item.DocumentoRecepcao;
                    telemoveisEquipamentos.Utilizador = User.Identity.Name;
                    telemoveisEquipamentos.DataAlteracao = DateTime.Now;
                    telemoveisEquipamentos.DevolvidoBk = item.DevolvidoBk;
                    telemoveisEquipamentos.NumEmpregadoComprador = item.NumEmpregadoComprador;
                    telemoveisEquipamentos.NomeComprador = item.NomeComprador;
                    telemoveisEquipamentos.Devolvido = item.Devolvido;
                    telemoveisEquipamentos.UtilizadorModificacao = User.Identity.Name;
                    telemoveisEquipamentos.DataHoraModificacao = DateTime.Now;

                    try
                    {
                        DBTelemoveis.Update(telemoveisEquipamentos);

                        telemoveisEquipamentos = DBTelemoveis.GetTelemoveisEquipamentos(item.Tipo, item.Imei);
                        item = DBTelemoveis.CastTelemoveisEquipamentosToView(telemoveisEquipamentos);

                        item.eReasonCode = 1;
                        item.eMessage = "Equipamento actualizado com sucesso!";
                    }
                    catch
                    {
                        item.eReasonCode = -1;
                        item.eMessage = "Ocorreu um erro ao gravar o Equipamento!";
                        return Json(item);
                    }
                }
                else
                {
                    item.eReasonCode = -1;
                    item.eMessage = "Ocorreu um erro!";
                    return Json(item);
                }
            }

            return Json(item);
        }
    }
}