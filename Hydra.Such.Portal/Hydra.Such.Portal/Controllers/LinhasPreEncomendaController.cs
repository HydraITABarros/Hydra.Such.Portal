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

        
    }
}