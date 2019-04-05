using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Database;
using Hydra.Such.Data;
using System;

namespace Hydra.Such.Portal.Controllers
{
    public class PedidosDEVController : Controller
    {
        public IActionResult PedidosDEV_List()
        {
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminPedidosDEV);

            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !userPerm.Create.Value;
                ViewBag.UpdatePermissions = !userPerm.Update.Value;
                ViewBag.DeletePermissions = !userPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult PedidosDEV(string id)
        {
            ViewBag.NoPedidoDEV = id;

            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminPedidosDEV);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !userPerm.Create.Value;
                ViewBag.UpdatePermissions = !userPerm.Update.Value;
                ViewBag.DeletePermissions = !userPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetListPedidosDEV()
        {
            List<PedidosDEVViewModel> result = DBPedidosDEV.GetAll().ParseToViewModel();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetPedidoDesenvolvimento([FromBody] PedidosDEVViewModel data)
        {
            PedidosDEV DEV = new PedidosDEV();
            PedidosDEVViewModel result = new PedidosDEVViewModel();

            if (data != null)
            {
                DEV = DBPedidosDEV.GetById(data.ID);
                if (DEV != null)
                {
                    result = DEV.ParseToViewModel();
                }
                else
                {
                    if (data.ID == 0)
                    {
                        result.Estado = 0;
                        result.DataEstado = DateTime.Now;
                        result.DataEstadoText = DateTime.Now.ToString("yyyy-MM-dd");
                        result.CriadoPor = User.Identity.Name;
                        result.DataCriacao = DateTime.Now;
                    }
                }
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult CreatePedidoDesenvolvimento([FromBody] PedidosDEVViewModel data)
        {
            try
            {
                if (data != null)
                {
                    PedidosDEV DEV = new PedidosDEV();
                    DEV = data.ParseToDB();

                    if (DEV != null)
                    {
                        DEV.Estado = 0;
                        DEV.DataEstado = DateTime.Now;
                        DEV.CriadoPor = User.Identity.Name;
                        if (DBPedidosDEV.Create(DEV) != null)
                        {
                            data.ID = DEV.ID;
                            data.eReasonCode = 1;
                            data.eMessage = "Pedido de Desenvolvimento criado com sucesso.";
                        }
                        else
                        {
                            data.eReasonCode = 2;
                            data.eMessage = "Erro ao criar o Pedido de Desenvolvimento.";
                        }
                    }
                    else
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "Erro ao converter os dados do Pedido de Desenvolvimento.";
                    }
                }
                else
                {
                    data.eReasonCode = 5;
                    data.eMessage = "Não foi possível ler os dados do Pedido de Desenvolvimento.";
                }
            }
            catch
            {
                data.eReasonCode = 99;
                data.eMessage = "Ocorreu um erro.";
            }

            return Json(data);
        }

        [HttpPost]
        public JsonResult UpdatePedidoDesenvolvimento([FromBody] PedidosDEVViewModel data)
        {
            try
            {
                if (data != null)
                {
                    PedidosDEV DEV = new PedidosDEV();
                    PedidosDEV DEV_OLD = new PedidosDEV();

                    DEV = data.ParseToDB();
                    DEV_OLD = DBPedidosDEV.GetById(data.ID);

                    if (DEV.Estado != DEV_OLD.Estado)
                    {
                        DEV.DataEstado = DateTime.Now;
                    }

                    if (DEV != null)
                    {
                        DEV.AlteradoPor = User.Identity.Name;
                        if (DBPedidosDEV.Update(DEV) != null)
                        {
                            data.eReasonCode = 1;
                            data.eMessage = "Pedido de Desenvolvimento atualizado com sucesso.";
                        }
                        else
                        {
                            data.eReasonCode = 2;
                            data.eMessage = "Erro ao atualizar o Pedido de Desenvolvimento.";
                        }
                    }
                    else
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "Erro ao converter os dados do Pedido de Desenvolvimento.";
                    }
                }
                else
                {
                    data.eReasonCode = 5;
                    data.eMessage = "Não foi possível ler os dados do Pedido de Desenvolvimento.";
                }
            }
            catch
            {
                data.eReasonCode = 99;
                data.eMessage = "Ocorreu um erro.";
            }

            return Json(data);
        }

        [HttpPost]
        public JsonResult DeletePedidoDEV([FromBody] PedidosDEVViewModel data)
        {
            try
            {
                if (data != null)
                {
                    if (DBPedidosDEV.Delete(data.ID) == true)
                    {
                        data.eReasonCode = 1;
                        data.eMessage = "Pedido de Desenvolvimento eliminado com sucesso.";
                    }
                    else
                    {
                        data.eReasonCode = 2;
                        data.eMessage = "Erro ao eliminar o Pedido de Desenvolvimento.";
                    }
                }
                else
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Não foi possível ler os dados do Pedido de Desenvolvimento.";
                }
            }
            catch
            {
                data.eReasonCode = 99;
                data.eMessage = "Ocorreu um erro.";
            }

            return Json(data);
        }




    }
}
