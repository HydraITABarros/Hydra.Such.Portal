using System;
using System.Collections.Generic;
using System.Linq;
using Hydra.Such.Data;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Nutrition;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Nutrition;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Such.Portal.Controllers
{
    public class FichasTecnicasPratosController : Controller
    {
        #region View
        //Start Grid

        public IActionResult Index()
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.FichasTécnicasPratos);
            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UPermissions = userPermissions;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }
        // Record Technical Of Plates form by id

        public IActionResult FichaTecnica(string id)
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.FichasTécnicasPratos);
            if (userPermissions != null && userPermissions.Read.Value)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    ViewBag.RecTecPlatesId = id;
                }
                else
                {
                    ViewBag.RecTecPlatesId = "";
                }

                ViewBag.UPermissions = userPermissions;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }
        #endregion

        #region Get values
        // Get All Record Technical Of Plates by plateNo
        [HttpPost]

        public JsonResult GetAllRecTecPlatesBYPlate([FromBody]string plateNo)
        {
            List<RecordTechnicalOfPlatesModelView> result = DBRecordTechnicalOfPlates.GetByPlateNo(plateNo).ParseToViewModel();
            return Json(result);
        }
        // Get All Record Technical Of Plates without Image
        [HttpPost]

        public JsonResult GetAllRecTecPlates()
        {
            List<RecordTechnicalOfPlatesModelView> result = DBRecordTechnicalOfPlates.GetAll().ParseToViewModel();
            return Json(result);
        }
        // Get the Image from Record Technical Of Plates
        [HttpPost]

        public JsonResult GetImageRecTecPlates([FromBody]string plateNo)
        {
            RecordTechnicalOfPlatesModelView result = DBRecordTechnicalOfPlates.GetOnlyImageByPlateNo(plateNo);
            if (result.Image != null)
            {
                string imreBase64Data = Convert.ToBase64String(result.Image);
                string imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
                result.ImageToString = imgDataURL;
            }
            return Json(result);
        }
        // Get All Procedures Confection by plateNo
        [HttpPost]

        public JsonResult GetAllProceduresConfection([FromBody]string plateNo)
        {
            List<ProceduresConfectionViewModel> result = ProceduresConfection.GetAllbyPlateNo(plateNo).ParseToViewModel();
            return Json(result);
        }
        // Get all Lines Record Technical Of Plates by PlateNo
        [HttpPost]

        public JsonResult GetAllLinesRecTechnicPlates([FromBody] string plateNo)
        {
            List<LinesRecordTechnicalOfPlatesViewModel> result = DBLinesRecordTechnicalOfPlates.GetAllbyPlateNo(plateNo).ParseToViewModel();
            return Json(result);
        }
        #endregion
        [HttpPost]
        public JsonResult UpdateConfection([FromBody] List<ProceduresConfectionViewModel> data, string plateNo)
        {
            ErrorHandler result = new ErrorHandler();
            if (data != null)
            {
                List<ProcedimentosDeConfeção> previousList;
                // Get All
                previousList = ProceduresConfection.GetAllbyPlateNo(plateNo);
                foreach (ProcedimentosDeConfeção line in previousList)
                {
                    if (!data.Any(x => x.TechnicalSheetNo == line.NºPrato && x.actionNo == line.CódigoAção))
                    {
                        ProceduresConfection.Delete(line);
                    }
                }
                //Update
                try
                {
                    data.ForEach(x =>
                    {
                        List<ProcedimentosDeConfeção> dpObject = ProceduresConfection.GetAllbyActionNoAndPlateNo(x.actionNo, x.TechnicalSheetNo);
                        if (dpObject.Count > 0)
                        {
                            ProcedimentosDeConfeção newdp = dpObject.FirstOrDefault();
                            newdp.NºPrato = x.TechnicalSheetNo;
                            newdp.Descrição = x.description;
                            newdp.CódigoAção = x.actionNo;
                            newdp.NºOrdem = x.orderNo;
                            newdp.DataHoraCriação = x.CreateDateTime;
                            newdp.UtilizadorCriação = x.CreateUser;
                            newdp.DataHoraModificação = DateTime.Now;
                            newdp.UtilizadorModificação = User.Identity.Name;
                            newdp = ProceduresConfection.Update(newdp);
                            if (newdp != null)
                            {
                                result.eReasonCode = 1;
                                result.eMessage =
                                    "Registo editado com sucesso.";
                            }
                            else
                            {
                                result.eReasonCode = 2;
                                result.eMessage = "Ocorreu um erro ao editar o registo.";
                            }
                        }
                    });

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                result.eReasonCode = 2;
                result.eMessage = "Ocorreu um erro: a linha não pode ser nula.";
            }
            return Json(result);
        }


        [HttpPost]
        public JsonResult CreateConfection([FromBody] ProceduresConfectionViewModel data)
        {
            ErrorHandler result = new ErrorHandler();
            if (data != null)
            {
                data.CreateUser = User.Identity.Name;
                data.CreateDateTime = DateTime.Now;
                var createdItem = ProceduresConfection.Create(data.ParseToDatabase());
                if (createdItem != null)
                {
                    result.eReasonCode = 1;
                    result.eMessage = "Registo criado com sucesso.";
                }
                else
                {
                    result.eReasonCode = 2;
                    result.eMessage = "Ocorreu um erro ao criar o registo.";
                }
            }
            else
            {
                result.eReasonCode = 2;
                result.eMessage = "Ocorreu um erro: a linha não pode ser nula.";
            }
            return Json(result);
        }
        #region CREATE/UPDATE Record Technical Of Plates
        // Create Record Technical Of Plates Row
        [HttpPost]

        public JsonResult CreateRecordTechnicalOfPlates([FromBody] RecordTechnicalOfPlatesModelView data)
        {
            if (data != null)
            {
                //Get Numeration
                bool autoGenId = false;
                Configuração conf = DBConfigurations.GetById(1);
                int entityNumerationConfId = conf.NumeraçãoFichasTécnicasDePratos.Value;

                if (data.PlateNo == "" || data.PlateNo == null)
                {
                    autoGenId = true;
                    data.PlateNo = DBNumerationConfigurations.GetNextNumeration(entityNumerationConfId, autoGenId);
                }
                if (data.PlateNo != null)
                {
                    data.CreateUser = User.Identity.Name;
                    var createdItem = DBRecordTechnicalOfPlates.Create(data.ParseToDB());
                    if (createdItem != null)
                    {

                        data = createdItem.ParseToViewModel();

                        //Update Last Numeration Used
                        if (autoGenId)
                        {
                            ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(entityNumerationConfId);
                            ConfigNumerations.ÚltimoNºUsado = data.PlateNo;
                            ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                            DBNumerationConfigurations.Update(ConfigNumerations);
                        }

                        data.eReasonCode = 1;
                        data.eMessage = "Registo criado com sucesso.";
                    }
                    else
                    {

                        data = new RecordTechnicalOfPlatesModelView();
                        data.eReasonCode = 2;
                        data.eMessage = "Ocorreu um erro ao editar o registo.";
                    }
                }
                else
                {
                    data.eReasonCode = 2;
                    data.eMessage = "A numeração configurada não é compativel com a inserida.";
                }
            }
            else
            {
                data = new RecordTechnicalOfPlatesModelView();
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro: a linha não pode ser nula.";
            }
            return Json(data);
        }
        // Update Record Technical Of Plates Row
        [HttpPost]

        public JsonResult UpdateRecordTechnicalOfPlates([FromBody] RecordTechnicalOfPlatesModelView data)
        {
            if (data != null && !string.IsNullOrEmpty(data.PlateNo))
            {
                data.CreateUser = User.Identity.Name;
                var createdItem = DBRecordTechnicalOfPlates.Update(data.ParseToDB());
                if (createdItem != null)
                {

                    data = createdItem.ParseToViewModel();
                    data.eReasonCode = 1;
                    data.eMessage = "Registo Editado com sucesso.";
                }
                else
                {

                    data = new RecordTechnicalOfPlatesModelView();
                    data.eReasonCode = 2;
                    data.eMessage = "Ocorreu um erro ao Editado o registo.";
                }
            }
            else
            {
                data = new RecordTechnicalOfPlatesModelView();
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro: a linha não pode ser nula.";
            }
            return Json(data);
        }


        #endregion

        #region CREATE/UPDATE Lines Record Technical Of Plates
        // Create Lines Record Technical Of Plates Row
        [HttpPost]

        public JsonResult CreateLinesRecordTechnicalOfPlates([FromBody] LinesRecordTechnicalOfPlatesViewModel data)
        {
            ErrorHandler result = new ErrorHandler();
            if (data != null)
            {
                data.CreateUser = User.Identity.Name;
                var createdItem = DBLinesRecordTechnicalOfPlates.Create(data.ParseToDB());
                if (createdItem != null)
                {
                    result.eReasonCode = 1;
                    result.eMessage = "Registo criado com sucesso.";
                }
                else
                {
                    result.eReasonCode = 2;
                    result.eMessage = "Ocorreu um erro ao criar o registo.";
                }
            }
            else
            {
                result.eReasonCode = 2;
                result.eMessage = "Ocorreu um erro: a linha não pode ser nula.";
            }
            return Json(result);
        }

        // Update Lines Record Technical Of Plates Rows
        [HttpPost]

        public JsonResult UpdateLinesRecordTechnicalOfPlates([FromBody] List<LinesRecordTechnicalOfPlatesViewModel> data, string plateNo)
        {
            ErrorHandler result = new ErrorHandler();
            if (data != null)
            {
                List<LinhasFichasTécnicasPratos> previousList;
                // Get All
                previousList = DBLinesRecordTechnicalOfPlates.GetAllbyPlateNo(plateNo);
                foreach (LinhasFichasTécnicasPratos line in previousList)
                {
                    if (!data.Any(x => x.LineNo == line.NºLinha))
                    {
                        DBLinesRecordTechnicalOfPlates.Delete(line);
                    }
                }
                //Update
                try
                {
                    data.ForEach(x =>
                    {
                        List<LinhasFichasTécnicasPratos> dpObject = DBLinesRecordTechnicalOfPlates.GetByLineNo(x.LineNo);
                        if (dpObject.Count > 0)
                        {
                            LinhasFichasTécnicasPratos newdp = dpObject.FirstOrDefault();
                            newdp.NºPrato = x.PlateNo;
                            newdp.NºLinha = x.LineNo;
                            newdp.Tipo = x.Type;
                            newdp.Código = x.Code;
                            newdp.Descrição = x.Description;
                            newdp.Quantidade = x.Quantity;
                            newdp.CódUnidadeMedida = x.UnitMeasureCode;
                            newdp.QuantidadeDeProdução = x.QuantityOfProduction;
                            newdp.ValorEnergético = x.EnergeticValue;
                            newdp.Proteínas = x.Proteins;
                            newdp.HidratosDeCarbono = x.HydratesOfCarbon;
                            newdp.Lípidos = x.Lipids;
                            newdp.Fibras = x.Fibers;
                            newdp.PreçoCustoEsperado = x.ExpectedCostPrice;
                            newdp.PreçoCustoAtual = x.CurrentCostPrice;
                            newdp.TpreçoCustoEsperado = x.TimeExpectedCostPrice;
                            newdp.TpreçoCustoAtual = x.TimeCurrentCostPrice;
                            newdp.CódLocalização = x.LocalizationCode;
                            newdp.ProteínasPorQuantidade = x.ProteinsByQuantity;
                            newdp.GlícidosPorQuantidade = x.GlicansByQuantity;
                            newdp.LípidosPorQuantidade = x.LipidsByQuantity;
                            newdp.FibasPorQuantidade = x.FibersByQuantity;
                            newdp.ValorEnergético2 = x.EnergeticValue2;
                            newdp.VitaminaA = x.VitaminA;
                            newdp.VitaminaD = x.VitaminD;
                            newdp.Colesterol = x.Cholesterol;
                            newdp.Sódio = x.Sodium;
                            newdp.Potássio = x.Potassium;
                            newdp.Cálcio = x.Calcium;
                            newdp.Ferro = x.Iron;
                            newdp.Edivel = x.Edivel;
                            newdp.VitaminaAPorQuantidade = x.VitaminAByQuantity;
                            newdp.VitaminaDPorQuantidade = x.VitaminDByQuantity;
                            newdp.ColesterolPorQuantidade = x.CholesterolByQuantity;
                            newdp.SódioPorQuantidade = x.SodiumByQuantity;
                            newdp.PotássioPorQuantidade = x.PotassiumByQuantity;
                            newdp.FerroPorQuantidade = x.IronByQuantity;
                            newdp.CálcioPorQuantidade = x.CalciumByQuantity;
                            newdp.ÁcidosGordosSaturados = x.SaturatedFattyAcids;
                            newdp.Açucares = x.SugarCane;
                            newdp.Sal = x.Salt;
                            newdp.QuantidadePrato = x.QuantityPlates;
                            newdp.Preparação = x.Preparation;
                            newdp.DataHoraCriação = x.CreateDateTime;
                            newdp.UtilizadorCriação = x.CreateUser;
                            newdp.DataHoraModificação = DateTime.Now;
                            newdp.UtilizadorModificação = User.Identity.Name;
                            newdp = DBLinesRecordTechnicalOfPlates.Update(newdp);
                            if (newdp != null)
                            {
                                result.eReasonCode = 1;
                                result.eMessage =
                                    "Registo editado com sucesso.";
                            }
                            else
                            {
                                result.eReasonCode = 2;
                                result.eMessage = "Ocorreu um erro ao criar o registo.";
                            }
                        }
                    });

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                result.eReasonCode = 2;
                result.eMessage = "Ocorreu um erro: a linha não pode ser nula.";
            }
            return Json(result);
        }
        #endregion

        #region Upload Image
        // Record Technical Of Plates Updload And Save de Image
        [HttpPost]
        public IActionResult UploadFilesAjax(string id)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                byte[] fileBytes = null;
                FichasTécnicasPratos UpdateItem = new FichasTécnicasPratos();
                var files = Request.Form.Files;
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new System.IO.MemoryStream())
                        {
                            //System.IO.File.Create()
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                    }
                }
                if (fileBytes != null && id != null)
                {
                    List<RecordTechnicalOfPlatesModelView> GetAllrow =
                        DBRecordTechnicalOfPlates.GetWithImageByPlateNo(id).ParseToViewModel();

                    foreach (RecordTechnicalOfPlatesModelView rtp in GetAllrow)
                    {
                        rtp.Image = fileBytes;
                        rtp.CreateUser = User.Identity.Name;
                        UpdateItem = DBRecordTechnicalOfPlates.Update(rtp.ParseToDB());
                    }
                    if (UpdateItem != null)
                    {
                        result.eReasonCode = 1;
                        result.eMessage = "Imagem guardada com sucesso";
                    }
                    else
                    {
                        result.eReasonCode = 3;
                        result.eMessage = "Ocorreu um erro ao guardar a imagem.";
                    }
                }
                else
                {
                    result.eReasonCode = 2;
                    result.eMessage = "Não foi escolhida nenhuma imagem";
                }

                return Json(result);
            }
            catch (Exception e)
            {
                result.eReasonCode = 2;
                result.eMessage = "Ocorreu um problema com a imagem tente novamente";
                return Json(result);
            }
        }
        #endregion

        #region Classificação Fichas Técnicas

        public IActionResult ClassificacaoFichasTecnicas(string id, string option)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.DiárioProjeto);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ProjectNo = id ?? "";
                ViewBag.UPermissions = UPerm;

                if (option == "Grupos")
                {
                    @ViewBag.Option = "Grupos";
                    @ViewBag.Groups = "hidden";
                }
                else
                {
                    @ViewBag.Option = "linhas";
                    @ViewBag.Groups = "";
                }
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        //O : Lines of Groups
        //1 : Group
        public JsonResult GetClassificationFilesTechniques([FromBody] string option)
        {
            List<ClassificationFilesTechniquesViewModel> result;
            if (option == "Grupos")
                result = DBClassificationFilesTechniques.ParseToViewModel(DBClassificationFilesTechniques.GetTypeFiles(1));
            else
                result = DBClassificationFilesTechniques.ParseToViewModel(DBClassificationFilesTechniques.GetTypeFiles(0));

            return Json(result);
        }
        [HttpPost]
        public JsonResult CreateClassificationTechniques([FromBody] ClassificationFilesTechniquesViewModel data)
        {

            data.CreateUser = User.Identity.Name;
            if (DBClassificationFilesTechniques.Create(DBClassificationFilesTechniques.ParseToDatabase(data)) != null)
                return Json(data);
            else
                return null;
        }

        [HttpPost]
        public JsonResult DeleteClassificationTechniques([FromBody] ClassificationFilesTechniquesViewModel data)
        {

            //Delete lines of Groups
            if (data.Type == 1)
            {
                if (DBClassificationFilesTechniques.GetTypeFiles(0).Exists(x => x.Grupo == data.Code))
                {
                    return Json(null);
                }
            }
            // Delete Group
            var result = DBClassificationFilesTechniques.Delete(DBClassificationFilesTechniques.ParseToDatabase(data));

            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateClassificationTechniques([FromBody] List<ClassificationFilesTechniquesViewModel> data)
        {

            data.ForEach(x =>
            {
                x.UpdateUser = User.Identity.Name;
                DBClassificationFilesTechniques.Update(DBClassificationFilesTechniques.ParseToDatabase(x));
            });
            return Json(data);
        }
#endregion
    }
}