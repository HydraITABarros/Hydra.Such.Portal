using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.ExtratoCliente;
using Hydra.Such.Data.ViewModel.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class ExtratoClienteController : Controller
    {
        [AllowAnonymous]
        public ActionResult Get(string CustomerNo, string Data, int Movimentos)
        {
            var list = DBNAV2017ExtratoCliente.GetByCustomerNo(CustomerNo, Data, Movimentos);

            return Json(list);
        }

        [HttpPost]
        public JsonResult Get([FromBody] ClientExtractViewModel data)
        {
            return Json("");
        }

    }
}