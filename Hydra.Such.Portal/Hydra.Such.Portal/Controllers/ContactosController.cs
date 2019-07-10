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
using Newtonsoft.Json.Linq;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.ViewModel.ProjectView;

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
                if (!(item.No == "" || item.No == null) && !numConf.Manual.Value)
                {
                    return Json("A numeração configurada para contactos não permite inserção manual.");
                }
                else if (item.No == "" && !numConf.Automático.Value)
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

            if (item != null && !string.IsNullOrEmpty(item.No))
                result = DBContacts.GetById(item.No).ParseToViewModel();

            if (!string.IsNullOrEmpty(result.NoCliente))
            {
                NAVClientsViewModel cliente = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, result.NoCliente);
                if (cliente != null)
                {
                    result.ClienteNome = cliente.Name;
                    result.ClienteNIF = cliente.VATRegistrationNo_;
                    result.ClienteEndereco = cliente.Address;
                    result.ClienteCodigoPostal = cliente.PostCode;
                    result.ClienteCidade = cliente.City;
                    result.ClienteRegiao = cliente.RegionCode;
                    result.ClienteTelefone = cliente.PhoneNo;
                    result.ClienteEmail = cliente.EMail;
                }
            }

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
                
                if (item.No == "" || item.No == null)
                {
                    autoGenId = true;
                    item.No = DBNumerationConfigurations.GetNextNumeration(entityNumerationConfId, autoGenId, false);
                }

                if (item.No != null)
                {
                    //Ensure contact Id doesn't exist
                    var existingContact = DBContacts.GetById(item.No);
                    if (existingContact == null)
                    {
                        item.CriadoPor = User.Identity.Name;

                        var newItem = DBContacts.Create(item.ParseToDB()).ParseToViewModel();
                        if (newItem != null)
                        {
                            //Inserted, update item to return
                            item = newItem;
                            
                            //Task<WSContacts.Create_Result> createContactTask = NAVContactsService.CreateAsync(item, _configws);
                            //try
                            //{
                            //    createContactTask.Wait();
                            //}
                            //catch (Exception ex)
                            //{
                            //    item.eReasonCode = 3;
                            //    item.eMessage = "Ocorreu um erro ao criar o contacto no NAV.";
                            //    item.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                            //}


                            //if (!createContactTask.IsCompletedSuccessfully)
                            //{
                            //    //Delete Created Project on Database
                            //    DBContacts.Delete(item.No);

                            //    item.eReasonCode = 3;
                            //    item.eMessage = "Ocorreu um erro ao criar o contacto no NAV.";
                            //}
                            //else
                            //{
                                //Update Last Numeration Used
                                ConfiguraçãoNumerações configNumerations = DBNumerationConfigurations.GetById(entityNumerationConfId);
                                if (configNumerations != null && autoGenId)
                                {
                                    configNumerations.ÚltimoNºUsado = item.No;
                                    configNumerations.UtilizadorModificação = User.Identity.Name;
                                    DBNumerationConfigurations.Update(configNumerations);
                                }
                                item.eReasonCode = 1;
                                item.eMessage = "Contacto criado com sucesso.";
                            //}
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
                        item.eMessage = "Já existe um contacto com o Nº " + item.No;
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
                item.AlteradoPor = User.Identity.Name;
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

                //Task<WSContacts.Update_Result> updateContactTask = NAVContactsService.UpdateAsync(item, _configws);

                //try
                //{
                //    updateContactTask.Wait();
                //}
                //catch (Exception ex)
                //{
                //    item.eReasonCode = 4;
                //    item.eMessage = "Ocorreu um erro ao atualizar o contacto no NAV.";
                //}
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
                sucess = DBContacts.Delete(item.No);

            return Json(sucess);
        }

        [HttpPost]
        public JsonResult GetContacts()
        {
            List<ContactViewModel> result = DBContacts.GetAll().ParseToViewModel();

            List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");
            List<ContactosServicos> AllServicos = DBContactsServicos.GetAll();
            List<ContactosFuncoes> AllFuncoes = DBContactsFuncoes.GetAll();

            result.ForEach(CT =>
            {
                NAVClientsViewModel cliente = AllClients.Where(x => x.No_ == CT.NoCliente).FirstOrDefault() != null ? AllClients.Where(x => x.No_ == CT.NoCliente).FirstOrDefault() : null;
                CT.ClienteNome = cliente != null ? cliente.Name : "";
                CT.ClienteNIF = cliente != null ? cliente.VATRegistrationNo_ : "";
                CT.ClienteEndereco = cliente != null ? cliente.Address : "";
                CT.ClienteCodigoPostal = cliente != null ? cliente.PostCode : "";
                CT.ClienteCidade = cliente != null ? cliente.City : "";
                CT.ClienteRegiao = cliente != null ? cliente.RegionCode : "";
                CT.ClienteTelefone = cliente != null ? cliente.PhoneNo : "";
                CT.ClienteEmail = cliente != null ? cliente.EMail : "";

                CT.ServicoNome = AllServicos.Where(x => x.ID == CT.NoServico).FirstOrDefault() != null ? AllServicos.Where(x => x.ID == CT.NoServico).FirstOrDefault().Servico : "";
                CT.FuncaoNome = AllFuncoes.Where(x => x.ID == CT.NoFuncao).FirstOrDefault() != null ? AllFuncoes.Where(x => x.ID == CT.NoFuncao).FirstOrDefault().Funcao : "";
            });

            return Json(result.OrderByDescending(x => x.No));
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_Contactos([FromBody] List<ContactViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\Upload\\temp";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Contactos");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["no"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº"); Col = Col + 1; }
                if (dp["noCliente"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Cliente"); Col = Col + 1; }
                if (dp["clienteNome"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cliente Nome"); Col = Col + 1; }
                if (dp["clienteNIF"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cliente NIF"); Col = Col + 1; }
                if (dp["clienteEndereco"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cliente Endereço"); Col = Col + 1; }
                if (dp["clienteCodigoPostal"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cliente Código Postal"); Col = Col + 1; }
                if (dp["clienteCidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cliente Cidade"); Col = Col + 1; }
                if (dp["clienteRegiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cliente Região"); Col = Col + 1; }
                if (dp["clienteTelefone"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cliente Telefone"); Col = Col + 1; }
                if (dp["clienteEmail"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cliente Email"); Col = Col + 1; }
                //if (dp["noServico"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Serviço"); Col = Col + 1; }
                if (dp["servicoNome"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Serviço"); Col = Col + 1; }
                //if (dp["noFuncao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Função"); Col = Col + 1; }
                if (dp["funcaoNome"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Função"); Col = Col + 1; }
                if (dp["nome"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nome"); Col = Col + 1; }
                if (dp["telefone"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Telefone"); Col = Col + 1; }
                if (dp["telemovel"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Telemóvel"); Col = Col + 1; }
                if (dp["fax"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Fax"); Col = Col + 1; }
                if (dp["email"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Email"); Col = Col + 1; }
                if (dp["pessoa"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Pessoa"); Col = Col + 1; }
                if (dp["notas"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Notas"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ContactViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["no"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.No); Col = Col + 1; }
                        if (dp["noCliente"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NoCliente); Col = Col + 1; }
                        if (dp["clienteNome"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ClienteNome); Col = Col + 1; }
                        if (dp["clienteNIF"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ClienteNIF); Col = Col + 1; }
                        if (dp["clienteEndereco"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ClienteEndereco); Col = Col + 1; }
                        if (dp["clienteCodigoPostal"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ClienteCodigoPostal); Col = Col + 1; }
                        if (dp["clienteCidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ClienteCidade); Col = Col + 1; }
                        if (dp["clienteRegiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ClienteRegiao); Col = Col + 1; }
                        if (dp["clienteTelefone"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ClienteTelefone); Col = Col + 1; }
                        if (dp["clienteEmail"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ClienteEmail); Col = Col + 1; }
                        //if (dp["noServico"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NoServico.ToString()); Col = Col + 1; }
                        if (dp["servicoNome"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ServicoNome); Col = Col + 1; }
                        //if (dp["noFuncao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NoFuncao.ToString()); Col = Col + 1; }
                        if (dp["funcaoNome"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.FuncaoNome); Col = Col + 1; }
                        if (dp["nome"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Nome); Col = Col + 1; }
                        if (dp["telefone"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Telefone); Col = Col + 1; }
                        if (dp["telemovel"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Telemovel); Col = Col + 1; }
                        if (dp["fax"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Fax); Col = Col + 1; }
                        if (dp["email"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Email); Col = Col + 1; }
                        if (dp["pessoa"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Pessoa); Col = Col + 1; }
                        if (dp["notas"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Notas); Col = Col + 1; }
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