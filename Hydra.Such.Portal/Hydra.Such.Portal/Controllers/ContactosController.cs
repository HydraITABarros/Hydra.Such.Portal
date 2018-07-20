using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Contracts;
using Hydra.Such.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Hydra.Such.Portal.Controllers
{
    public class ContactosController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly GeneralConfigurations _generalConfig;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ContactosController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IOptions<GeneralConfigurations> appSettingsGeneral, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            _generalConfig = appSettingsGeneral.Value;
            this._hostingEnvironment = _hostingEnvironment;
        }

        // GET: Contactos
        public ActionResult Index()
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Contactos);
            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UserPermissions = userPermissions;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [Route("Contactos/Detalhes")]
        [Route("Contactos/Detalhes/{id}")]
        public ActionResult Details(string id)
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Contactos);
            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.ContactId = string.IsNullOrEmpty(id) ? string.Empty : id;
                ViewBag.UserPermissions = userPermissions;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult ValidateNumeration([FromBody] ContactViewModel item)
        {
            //Get Project Numeration
            Configuração conf = DBConfigurations.GetById(1);
            if (conf != null)
            {
                int contactsNumerationConfId = conf.NumeraçãoContactos.Value;

                ConfiguraçãoNumerações numConf = DBNumerationConfigurations.GetById(contactsNumerationConfId);

                //Validate if id is valid
                if (!(item.Id == "" || item.Id == null) && !numConf.Manual.Value)
                {
                    return Json("A numeração configurada para contactos não permite inserção manual.");
                }
                else if (item.Id == "" && !numConf.Automático.Value)
                {
                    return Json("É obrigatório inserir o Nº de Contacto.");
                }
            }
            else
            {
                return Json("Não foi possivel obter as configurações base de numeração.");
            }
            return Json("");
        }

        [HttpPost]
        public JsonResult GetById([FromBody] ContactViewModel item)
        {
            ContactViewModel result = new ContactViewModel();
            if (item != null && !string.IsNullOrEmpty(item.Id))
                result = DBContacts.GetById(item.Id).ParseToViewModel();
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateContact([FromBody] ContactViewModel item)
        {
            if (item != null)
            {
                //Get Numeration
                bool autoGenId = false;
                Configuração conf = DBConfigurations.GetById(1);
                int entityNumerationConfId = conf.NumeraçãoContactos.Value;
                
                if (item.Id == "" || item.Id == null)
                {
                    autoGenId = true;
                    item.Id = DBNumerationConfigurations.GetNextNumeration(entityNumerationConfId, autoGenId, false);
                }

                if (item.Id != null)
                {
                    //Ensure contact Id doesn't exist
                    var existingContact = DBContacts.GetById(item.Id);
                    if (existingContact == null)
                    {
                        item.CreateUser = User.Identity.Name;

                        var newItem = DBContacts.Create(item.ParseToDB()).ParseToViewModel();
                        if (newItem != null)
                        {
                            //Inserted, update item to return
                            item = newItem;
                            
                            Task<WSContacts.Create_Result> createContactTask = NAVContactsService.CreateAsync(item, _configws);
                            try
                            {
                                createContactTask.Wait();
                            }
                            catch (Exception ex)
                            {
                                item.eReasonCode = 3;
                                item.eMessage = "Ocorreu um erro ao criar o contacto no NAV.";
                                item.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                            }


                            if (!createContactTask.IsCompletedSuccessfully)
                            {
                                //Delete Created Project on Database
                                DBContacts.Delete(item.Id);

                                item.eReasonCode = 3;
                                item.eMessage = "Ocorreu um erro ao criar o contacto no NAV.";
                            }
                            else
                            {
                                //Update Last Numeration Used
                                ConfiguraçãoNumerações configNumerations = DBNumerationConfigurations.GetById(entityNumerationConfId);
                                if (configNumerations != null && autoGenId)
                                {
                                    configNumerations.ÚltimoNºUsado = item.Id;
                                    configNumerations.UtilizadorModificação = User.Identity.Name;
                                    DBNumerationConfigurations.Update(configNumerations);
                                }
                                item.eReasonCode = 1;
                                item.eMessage = "Contacto criado com sucesso.";
                            }
                        }
                        else
                        {
                            item.eReasonCode = 3;
                            item.eMessage = "Ocorreu um erro ao criar o contacto no portal.";
                        }
                    }
                    else
                    {
                        item.eReasonCode = 4;
                        item.eMessage = "Já existe um contacto com o id " + item.Id;
                    }
                }
                else
                {
                    item.eReasonCode = 5;
                    item.eMessage = "A numeração configurada não é compativel com a inserida.";
                }
            }
            return Json(item);
        }

        [HttpPost]
        public JsonResult UpdateContact([FromBody] ContactViewModel item)
        {
            if (item != null)
            {
                item.UpdateUser = User.Identity.Name;
                var updatedItem = DBContacts.Update(item.ParseToDB()).ParseToViewModel();
                if (updatedItem != null)
                {
                    item = updatedItem;
                    item.eReasonCode = 1;
                    item.eMessage = "Contacto atualizado com sucesso.";
                }
                else
                {
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao atualizar o contacto.";
                }

                Task<WSContacts.Update_Result> updateContactTask = NAVContactsService.UpdateAsync(item, _configws);

                try
                {
                    updateContactTask.Wait();
                }
                catch (Exception ex)
                {
                    item.eReasonCode = 4;
                    item.eMessage = "Ocorreu um erro ao atualizar o contacto no NAV.";
                }
            }
            else
            {
                item = new ContactViewModel()
                {
                    eReasonCode = 3,
                    eMessage = "Ocorreu um erro ao atualizar. O contacto não pode ser nulo."
                };
            }
            return Json(item);
        }

        [HttpPost]
        public JsonResult DeleteContact([FromBody] ContactViewModel item)
        {
            bool sucess = false;

            if (item != null)
                sucess = DBContacts.Delete(item.Id);

            return Json(sucess);
        }

        [HttpPost]
        public JsonResult GetContacts()
        {
            List<ContactViewModel> result = DBContacts.GetAll().ParseToViewModel();
            return Json(result);
        }

        //1
        [HttpPost]
        public async Task<JsonResult> ExportToExcel_Contactos([FromBody] List<ContactViewModel> dp)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\Upload\\temp";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + ".xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Contactos");
                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("ID");
                row.CreateCell(1).SetCellValue("Nome");

                if (dp != null)
                {
                    int count = 1;
                    foreach (ContactViewModel item in dp)
                    {
                        row = excelSheet.CreateRow(count);
                        row.CreateCell(0).SetCellValue(item.Id);
                        row.CreateCell(1).SetCellValue(item.Name);
                        count++;
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_Contactos(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Contactos.xlsx");
        }
    }
}