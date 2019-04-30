using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Database;
using Hydra.Such.Data;
using System;
using Hydra.Such.Portal.Configurations;
using Newtonsoft.Json.Linq;
using Hydra.Such.Data.NAV;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Linq;
using Hydra.Such.Data.Logic.Approvals;
using System.Net.Mail;
using System.Net;


namespace Hydra.Such.Portal.Controllers
{
    public class PedidosDEVController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly GeneralConfigurations _generalConfig;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PedidosDEVController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IOptions<GeneralConfigurations> appSettingsGeneral, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            _generalConfig = appSettingsGeneral.Value;
            this._hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult PedidosDEV_List()
        {
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminPedidosDEV);

            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.ReadPermissions = userPerm.Read.Value;
                ViewBag.CreatePermissions = userPerm.Create.Value;
                ViewBag.UpdatePermissions = userPerm.Update.Value;
                ViewBag.DeletePermissions = userPerm.Delete.Value;
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
                ViewBag.ReadPermissions = !userPerm.Read.Value;
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
        public JsonResult GetListPedidosDEV([FromBody] JObject requestParams)
        {
            int Archived = int.Parse(requestParams["Archived"].ToString());

            List<EnumData> AllEstados = EnumerablesFixed.DEV_Estados;
            List<PedidosDEVViewModel> result = DBPedidosDEV.GetAll().ParseToViewModel();

            if (Archived == 1)
            {
                result.RemoveAll(x => x.Estado == 0 || x.Estado == 1 || x.Estado == 3 || x.Estado == 4 || x.Estado == 5);
            }
            else
            {
                result.RemoveAll(x => x.Estado == 2 || x.Estado == 6);
            }

            result.ForEach(x => {
                x.EstadoText = x.Estado.HasValue ? AllEstados.Find(y => y.Id == x.Estado).Value : "";
            });

            return Json(result.OrderByDescending(x => x.ID));
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

                            //Envio automático de Email para ARomao@such.pt para conhecimento de novo Pedido
                            EmailsAprovações EmailApproval = new EmailsAprovações()
                            {
                                NºMovimento = data.ID,
                                EmailDestinatário = "ARomao@such.pt",
                                NomeDestinatário = "Amaro Romão",
                                Assunto = "e-SUCH - Foi criado um novo Pedido de Desenvolvimento",
                                DataHoraEmail = DateTime.Now,
                                TextoEmail = "Foi criado um novo Pedido de Desenvolvimento com o Nº " + data.ID.ToString() + " no Portal e-SUCH, com a seguinte descrição:" + Environment.NewLine + data.Descricao,
                                Enviado = false
                            };
                            SendEmailApprovals Email = new SendEmailApprovals
                            {
                                Subject = "e-SUCH - Foi criado um novo Pedido de Desenvolvimento",
                                From = "esuch@such.pt",
                                Body = "Foi criado um novo Pedido de Desenvolvimento com o Nº " + data.ID.ToString() + " no Portal e-SUCH, com a seguinte descrição:" + Environment.NewLine + data.Descricao,
                                IsBodyHtml = false,
                                DisplayName = "e-SUCH",
                                EmailApproval = EmailApproval
                            };
                            Email.To.Add("ARomao@such.pt");
                            Email.SendEmail();
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

                    if (DEV.Estado == 6 && !DEV.DataConclusao.HasValue) //6 = Concluído
                    {
                        DEV.DataConclusao = DateTime.Now;
                    }
                    if (DEV.Estado != 6 && DEV.DataConclusao.HasValue) //6 = Concluído
                    {
                        DEV.DataConclusao = null;
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

        [HttpPost]
        public async Task<JsonResult> ExportToExcel_PedidosDEV([FromBody] List<PedidosDEVViewModel> Lista)
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
                ISheet excelSheet = workbook.CreateSheet("Pedidos Desenvolvimento");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["id"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("ID"); Col = Col + 1; }
                if (dp["processo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Processo"); Col = Col + 1; }
                if (dp["descricao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descrição"); Col = Col + 1; }
                if (dp["url"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Link da Página"); Col = Col + 1; }
                if (dp["estadoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Estado"); Col = Col + 1; }
                if (dp["dataEstadoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data do Estado"); Col = Col + 1; }
                if (dp["dataPedidoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data do Pedido"); Col = Col + 1; }
                if (dp["pedidoPor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Pedido Por"); Col = Col + 1; }
                if (dp["dataConclusaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data da Conclusão"); Col = Col + 1; }
                if (dp["intervenientes"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Intervenientes"); Col = Col + 1; }
                if (dp["noHorasPrevistas"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº de Horas Previstas"); Col = Col + 1; }
                if (dp["noHorasRealizadas"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº de Horas Realizadas"); Col = Col + 1; }
                if (dp["criadoPor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Criado Por"); Col = Col + 1; }
                if (dp["dataCriacaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data de Criacao"); Col = Col + 1; }
                if (dp["alteradoPor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Alterado Por"); Col = Col + 1; }
                if (dp["dataAlteracaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data de Alteração"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (PedidosDEVViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["id"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ID); Col = Col + 1; }
                        if (dp["processo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Processo); Col = Col + 1; }
                        if (dp["descricao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Descricao); Col = Col + 1; }
                        if (dp["url"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.URL); Col = Col + 1; }
                        if (dp["estadoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.EstadoText); Col = Col + 1; }
                        if (dp["dataEstadoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataEstadoText); Col = Col + 1; }
                        if (dp["dataPedidoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataPedidoText); Col = Col + 1; }
                        if (dp["pedidoPor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.PedidoPor); Col = Col + 1; }
                        if (dp["dataConclusaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataConclusaoText); Col = Col + 1; }
                        if (dp["intervenientes"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Intervenientes); Col = Col + 1; }
                        if (dp["noHorasPrevistas"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NoHorasPrevistas.ToString()); Col = Col + 1; }
                        if (dp["noHorasRealizadas"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NoHorasRealizadas.ToString()); Col = Col + 1; }
                        if (dp["criadoPor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CriadoPor); Col = Col + 1; }
                        if (dp["dataCriacaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataCriacaoText); Col = Col + 1; }
                        if (dp["alteradoPor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.AlteradoPor); Col = Col + 1; }
                        if (dp["dataAlteracaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataAlteracaoText); Col = Col + 1; }

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

        public IActionResult ExportToExcelDownload_PedidosDEV(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Pedidos Desenvolvimento.xlsx");
        }



    }
}
