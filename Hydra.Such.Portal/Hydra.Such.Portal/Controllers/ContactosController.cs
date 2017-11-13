using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



//using Hydra.Such.Data.ViewModel;
//using Hydra.Such.Data.Logic;
//using Hydra.Such.Data.Database;
//using Hydra.Such.Data.Logic.Project;
//using Hydra.Such.Data.Logic.ProjectDiary;
//using Hydra.Such.Data.ViewModel.ProjectDiary;
//using Hydra.Such.Data.ViewModel.ProjectView;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.CodeAnalysis.CSharp.Syntax;

using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Contracts;

namespace Hydra.Such.Portal.Controllers
{
    public class ContactosController : Controller
    {
        private UserAccessesViewModel userPermissions = new UserAccessesViewModel()
        {
            Create = true,
            Delete = true,
            Update = true,
            Read = true,
        };
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;

        public ContactosController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
        }

        // GET: Contactos
        public ActionResult Index()
        {
            //UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 3, 1);
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
            //UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 3, 1);
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
                    return Json("É obrigatório inserir o Nº de Projeto.");
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
                string entityId = "";
                bool autoGenId = false;

                //Get Numeration
                Configuração conf = DBConfigurations.GetById(1);
                int entityNumerationConfId = conf.NumeraçãoContactos.Value;
                
                if (item.Id == "" || item.Id == null)
                {
                    autoGenId = true;
                    entityId = DBNumerationConfigurations.GetNextNumeration(entityNumerationConfId, autoGenId);
                    item.Id = entityId;
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
    }
}