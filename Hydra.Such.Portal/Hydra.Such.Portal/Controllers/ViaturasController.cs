using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Logic;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.FolhaDeHora;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.NAV;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.Logic.Viatura;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class ViaturasController : Controller
    {

        [HttpPost]
        public JsonResult GetList()
        {

            List<ViaturasViewModel> result = DBViatura.ParseListToViewModel(DBViatura.GetAllToList());

            //Apply User Dimensions Validations
            List<AcessosDimensões> CUserDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (CUserDimensions.Where(y => y.Dimensão == 1).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == 1 && y.ValorDimensão == x.CodigoRegiao));
            //FunctionalAreas
            if (CUserDimensions.Where(y => y.Dimensão == 2).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == 2 && y.ValorDimensão == x.CodigoAreaFuncional));
            //ResponsabilityCenter
            if (CUserDimensions.Where(y => y.Dimensão == 3).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == 3 && y.ValorDimensão == x.CodigoCentroResponsabilidade));

            result.ForEach(x =>
            {

                if (x.Estado != null) x.EstadoDescricao = EnumerablesFixed.ViaturasEstado.Where(y => y.Id == x.Estado).FirstOrDefault().Value;
                if (x.TipoCombustivel != null) x.TipoCombustivelDescricao = EnumerablesFixed.ViaturasTipoCombustivel.Where(y => y.Id == x.TipoCombustivel).FirstOrDefault().Value;
                if (x.TipoPropriedade != null) x.TipoPropriedadeDescricao = EnumerablesFixed.ViaturasTipoPropriedade.Where(y => y.Id == x.TipoPropriedade).FirstOrDefault().Value;
                if (x.CodigoMarca != null) x.Marca = DBMarcas.ParseToViewModel(DBMarcas.GetById(Int32.Parse(x.CodigoMarca)));
                if (x.CodigoModelo != null) x.Modelo = DBModelos.ParseToViewModel(DBModelos.GetById(Int32.Parse(x.CodigoModelo)));
                if (x.CodigoTipoViatura != null) x.TipoViatura = DBTiposViaturas.ParseToViewModel(DBTiposViaturas.GetById(Int32.Parse(x.CodigoTipoViatura)));
            });

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetViaturaDetails([FromBody] ViaturasViewModel data)
        {

            if (data != null)
            {
                Viaturas viatura = DBViatura.GetByMatricula(data.Matricula);

                if (viatura != null)
                {
                    ViaturasViewModel result = DBViatura.ParseToViewModel(viatura);

                    return Json(result);
                }

                return Json(new ViaturasViewModel());
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult CreateViatura([FromBody] ViaturasViewModel data)
        {

            try
            {
                if (data != null)
                {

                    if (data.Matricula != null)
                    {
                        Viaturas viatura = DBViatura.ParseToDB(data);

                        viatura.UtilizadorCriação = User.Identity.Name;

                        viatura = DBViatura.Create(viatura);
                        data.eReasonCode = 1;

                        if (viatura == null)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Ocorreu um erro ao criar a viatura no portal.";
                        }
                    }

                }
            }
            catch (Exception e)
            {
                data.eReasonCode = 4;
                data.eMessage = "Ocorreu um erro ao criar a viatura";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult UpdateViatura([FromBody] ViaturasViewModel data)
        {

            if (data != null)
            {
                Viaturas viatura = DBViatura.ParseToDB(data);
                viatura.UtilizadorModificação = User.Identity.Name;

                DBViatura.Update(viatura);
                return Json(data);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult DeleteViatura([FromBody] ViaturasViewModel data)
        {

            if (data != null)
            {
                ErrorHandler result = new ErrorHandler();
                DBViatura.Delete(DBViatura.ParseToDB(data));
                result = new ErrorHandler()
                {
                    eReasonCode = 0,
                    eMessage = "Viatura removida com sucesso."
                };
                return Json(result);
            }
            return Json(false);
        }
    }
}
