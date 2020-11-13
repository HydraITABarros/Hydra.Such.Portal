using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.ViewModel.Academia;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Hydra.Such.Portal.Controllers
{
    public class AcademiaController : Controller
    {
        private readonly GeneralConfigurations _config;
        private readonly IHostingEnvironment _hostingEnvironment;

        private string UploadPath { get; set; }
        private const string SubjectImagePath = "\\TemaFormacao\\";
        private const string TrainingImagePath = "\\AccaoFormacao\\";
        private const string RequestAttachmentsPath = "\\PedidoFormacao\\";
        protected int RequestOrigin { get; set; }
        protected readonly int TrainingRequestApprovalType = EnumerablesFixed.ApprovalTypes.Where(e => e.Value.Contains("Pedido Formação")).FirstOrDefault().Id;

        public AcademiaController(IOptions<GeneralConfigurations> appSettings, IHostingEnvironment hostingEnvironment)
        {
            _config = appSettings.Value;
            _hostingEnvironment = hostingEnvironment;
            UploadPath = _config.FileUploadFolder + "Academia";
        }

        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();

            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);

            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        private bool IsImage(string fileExt)
        {
            if (string.IsNullOrWhiteSpace(fileExt))
                return false;

            if (fileExt.StartsWith("."))
                fileExt = fileExt.Substring(1).ToLower();

            switch (fileExt)
            {
                case "bmp":
                case "gif":
                case "jpeg":
                case "jpg":
                case "png":
                    return true;
                default: return false;
            }
        }


        [HttpPost]
        public JsonResult GetMeusPedidos([FromBody] JObject requestParams)
        {
            //ClickSource = 0;
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
            ConfiguracaoAprovacaoUtilizador cfgUser = new ConfiguracaoAprovacaoUtilizador(User.Identity.Name, userConfig.TipoUtilizadorFormacao.Value, TrainingRequestApprovalType);
            List<Formando> formandos = DBAcademia.__GetAllFormandos(cfgUser, Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuChefia);

            Formando formando = DBAcademia.__GetDetailsFormando("27960");


            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            if(UPerm != null && UPerm.Read.Value)
            {
                bool apenasActivos = requestParams["apenasActivos"] == null ? false : (bool)requestParams["apenasActivos"];
                List<PedidoParticipacaoFormacao> meusPedidos = DBAcademia.__GetAllPedidosFormacao(User.Identity.Name, apenasActivos);

                List<PedidoParticipacaoFormacaoView> meusPedidosView = new List<PedidoParticipacaoFormacaoView>();

                foreach(var p in meusPedidos)
                {
                    meusPedidosView.Add(new PedidoParticipacaoFormacaoView(p));
                }

                return Json(meusPedidosView);
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult GetMeusPedidosAprovacao([FromBody] JObject requestParams)
        {
            bool apenasCompletos = requestParams["apenasCompletos"] == null ? false : (bool)requestParams["apenasCompletos"];
            try
            {
                RequestOrigin = requestParams["requestOrigin"] == null ? 0 : (int)requestParams["requestOrigin"];
            }
            catch (Exception)
            {
                return Json(-1);
            }            

            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
            ConfiguracaoAprovacaoUtilizador cfgUser = new ConfiguracaoAprovacaoUtilizador(User.Identity.Name, userConfig.TipoUtilizadorFormacao.Value, TrainingRequestApprovalType);
            List<PedidoParticipacaoFormacaoView> meusPedidosView = new List<PedidoParticipacaoFormacaoView>();

            switch (RequestOrigin)
            {
                case (int)Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuChefia:
                    if(UPerm != null && UPerm.Read.Value && cfgUser.IsChief())
                    {
                        List<PedidoParticipacaoFormacao> meusPedidos = DBAcademia.__GetAllPedidosFormacao(cfgUser, Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuChefia, apenasCompletos);
                        if (meusPedidos != null && meusPedidos.Count > 0)
                        {
                            foreach (var p in meusPedidos)
                            {
                                meusPedidosView.Add(new PedidoParticipacaoFormacaoView(p));
                            }
                        }
                    }
                    break;
                case (int)Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuDirector:
                    if(UPerm != null && UPerm.Read.Value && cfgUser.IsDirector())
                    {
                        List<PedidoParticipacaoFormacao> meusPedidos = DBAcademia.__GetAllPedidosFormacao(cfgUser, Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuDirector, apenasCompletos);
                        if (meusPedidos != null && meusPedidos.Count > 0)
                        {
                            foreach (var p in meusPedidos)
                            {
                                meusPedidosView.Add(new PedidoParticipacaoFormacaoView(p));
                            }

                            return Json(meusPedidosView);
                        }
                    }
                    break;
            }

            return Json(meusPedidosView);
        }

        [HttpPost]
        public JsonResult GetCatalogo([FromBody] JObject requestParams)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
            bool apenasActivos = requestParams["apenasActivos"] == null ? false : (bool)requestParams["apenasActivos"];
            

            if (UPerm != null && userConfig.TipoUtilizadorFormacao == (int)Enumerations.TipoUtilizadorFluxoPedidoFormacao.GestorFormacao)
            {
                List<TemaFormacao> temas = DBAcademia.__GetCatalogo(apenasActivos);
                List<TemaFormacaoView> temasV = new List<TemaFormacaoView>();
                foreach (var item in temas)
                {
                    TemaFormacaoView t = new TemaFormacaoView();
                    temasV.Add(t.ParseToView(item, apenasActivos));
                }

                return Json(temasV);
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult GetDetalhesTema([FromBody] JObject requestParams)
        {
            string idTema = requestParams["idTema"] == null ? null : (string)requestParams["idTema"];
            bool fromCatalogo = requestParams["fromCatalogo"] == null ? false : (bool)requestParams["fromCatalogo"];
            if (idTema == null)
                return Json(null);

            TemaFormacaoView tema = new TemaFormacaoView(DBAcademia.__GetDetailsTema(idTema));

            if (fromCatalogo)
            {
                tema.AccoesTema = tema.AccoesTema.Where(a => a.Activa.Value == 1 && a.DataInicio > DateTime.Now).ToList();
                tema.Accoes = tema.Accoes.Where(a => a.AccaoActiva).ToList();
            }
            return Json(tema);
        }

        [HttpPost]
        public JsonResult GetDetalhesAccao([FromBody] JObject requestParams)
        {
            string idAccao = requestParams["idAccao"] == null ? null : (string)requestParams["idAccao"];
            if (idAccao == null)
            {
                return Json(null);
            }

            try
            {
                AccaoFormacaoView accao = new AccaoFormacaoView(DBAcademia.__GetDetailsAccaoFormacao(idAccao));
                return Json(accao);
            }
            catch (Exception ex)
            {

                return Json(null);
            }            
        }

        [HttpPost]
        public JsonResult UpdateTema([FromBody] TemaFormacaoView data)
        {
            if(data == null)
            {
                return Json(false);
            }

            try
            {
                AttachmentsViewModel imagemActiva = data.ImagensTema.Where(i => i.Visivel.Value).FirstOrDefault();
                data.UrlImagem = imagemActiva != null ? imagemActiva.Url : string.Empty;
            }
            catch (Exception ex)
            {

                return Json(false);
            }
            
            

            bool updated = DBAcademia.__UpdateTemaFormacao(data);

            return Json(updated);
        }

        [HttpPost]
        public JsonResult UpdateImageStatus([FromBody] AttachmentsViewModel image)
        {
            if(image.DocType == TipoOrigemAnexos.TemaFormacao && !string.IsNullOrEmpty(image.DocNumber))
            {
                TemaFormacaoView tema = new TemaFormacaoView(DBAcademia.__GetDetailsTema(image.DocNumber));
                if(tema != null)
                {
                    if(tema.ImagensTema != null && tema.ImagensTema.Count > 0)
                    {
                       foreach(var i in tema.ImagensTema)
                        {
                            if(i.DocLineNo == image.DocLineNo)
                            {
                                i.Visivel = image.Visivel;
                            }
                            else
                            {
                                if (image.Visivel.Value)
                                {
                                    i.Visivel = false;
                                }
                            }
                        }

                        tema.UrlImagem = tema.ImagensTema.Where(i => i.Visivel.Value).FirstOrDefault() != null ?
                             tema.ImagensTema.Where(i => i.Visivel.Value).FirstOrDefault().Url : string.Empty;

                        return Json(DBAcademia.__UpdateTemaFormacao(tema));
                    }

                }
            }

            if (image.DocType == TipoOrigemAnexos.AccaoFormacao && !string.IsNullOrEmpty(image.DocNumber))
            {
                AccaoFormacaoView accao = new AccaoFormacaoView(DBAcademia.__GetDetailsAccaoFormacao(image.DocNumber));
                if (accao != null)
                {
                    if (accao.ImagensAccao != null && accao.ImagensAccao.Count() > 0)
                    {
                        foreach (var item in accao.ImagensAccao)
                        {
                            if (item.DocLineNo == image.DocLineNo)
                            {
                                item.Visivel = image.Visivel;
                            }
                            else
                            {
                                if (image.Visivel.Value)
                                {
                                    image.Visivel = false;
                                }
                            }
                        }

                        accao.UrlImagem = accao.ImagensAccao.Where(i => i.Visivel.Value).FirstOrDefault() != null ?
                            accao.ImagensAccao.Where(i => i.Visivel.Value).FirstOrDefault().Url : string.Empty;

                        return Json(DBAcademia.__UpdateAccaoFormacao(accao));
                    }

                }
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult UpdateAccao([FromBody] AccaoFormacaoView accao)
        {
            if (accao != null)
            {
                return Json(DBAcademia.__UpdateAccaoFormacao(accao.ParseToDb()));
            }
            return Json(false);
        }


        [HttpGet]
        [Route("Academia/DownloadImage/{docId}/{id}")]
        public FileStreamResult DownloadImage(string docId, string id)
        {
            try
            {
                string _path = UploadPath + SubjectImagePath + docId + "\\" + id;
                FileStream file = new FileStream(_path, FileMode.Open);

                if (IsImage(Path.GetExtension(file.Name)))
                {
                    string mimeType = GetMimeType(file.Name);

                    if (mimeType != "application/unknown")
                    {
                        return new FileStreamResult(file, mimeType);
                    }
                }
            }
            catch (Exception ex)
            {

                return null;
            }
            
            return null;
        }

        [HttpPost]       
        [Route("Academia/SubjectImageUpload")]
        [Route("Academia/SubjectImageUpload/{id}")]
        public JsonResult SubjectImageUpload(string id)
        {
            try
            {
                string _uploadPath = UploadPath + SubjectImagePath;
                var files = Request.Form.Files;

                foreach(var f in files)
                {
                    if (IsImage(Path.GetExtension(f.FileName)))
                    {
                        string full_filename = Path.GetFileName(f.FileName);
                        _uploadPath += id;

                        if (!System.IO.Directory.Exists(_uploadPath))
                        {
                            System.IO.Directory.CreateDirectory(_uploadPath);
                        }

                        var path = Path.Combine(_uploadPath, full_filename);
                        if (!System.IO.File.Exists(path))
                        {
                            using (FileStream dd = new FileStream(path, FileMode.CreateNew))
                            {
                                f.CopyTo(dd);
                                dd.Dispose();

                                Anexos newFile = new Anexos()
                                {
                                    NºOrigem = id,
                                    UrlAnexo = full_filename,
                                    Visivel = false,
                                    TipoOrigem = TipoOrigemAnexos.TemaFormacao,
                                    DataHoraCriação = DateTime.Now,
                                    UtilizadorCriação = User.Identity.Name
                                };

                                DBAttachments.Create(newFile);
                                if(newFile.NºLinha == 0)
                                {
                                    System.IO.File.Delete(path);
                                }
                            }
                        }
                        else
                        {
                            return Json("-1");
                        }                        
                    }
                }

                return Json("0");

            }
            catch (Exception e)
            {
                return Json("-10");
            }
        }

        [HttpPost]
        [Route("Academia/CourseImageUpload")]
        [Route("Academia/CourseImageUpload/{id}")]
        public JsonResult CourseImageUpload(string id)
        {
            try
            {
                string _uploadPath = UploadPath + SubjectImagePath;
                var files = Request.Form.Files;

                foreach (var f in files)
                {
                    if (IsImage(Path.GetExtension(f.FileName)))
                    {
                        string full_filename = Path.GetFileName(f.FileName);
                        _uploadPath += id;

                        if (!System.IO.Directory.Exists(_uploadPath))
                        {
                            System.IO.Directory.CreateDirectory(_uploadPath);
                        }

                        var path = Path.Combine(_uploadPath, full_filename);
                        if (!System.IO.File.Exists(path))
                        {
                            using (FileStream dd = new FileStream(path, FileMode.CreateNew))
                            {
                                f.CopyTo(dd);
                                dd.Dispose();

                                Anexos newFile = new Anexos()
                                {
                                    NºOrigem = id,
                                    UrlAnexo = full_filename,
                                    Visivel = false,
                                    TipoOrigem = TipoOrigemAnexos.AccaoFormacao,
                                    DataHoraCriação = DateTime.Now,
                                    UtilizadorCriação = User.Identity.Name
                                };

                                DBAttachments.Create(newFile);
                                if (newFile.NºLinha == 0)
                                {
                                    System.IO.File.Delete(path);
                                }
                            }
                        }
                        else
                        {
                            return Json("-1");
                        }
                    }
                }

                return Json("0");

            }
            catch (Exception e)
            {
                return Json("-10");
            }
        }

        [HttpPost]
        public JsonResult LoadAttachments([FromBody] JObject requestParams)
        {
            if(requestParams == null)
            {
                return Json("No data!");
            }

            string id = requestParams["id"] == null ? string.Empty : (string)requestParams["id"];
            string origem = requestParams["origem"] == null ? string.Empty : (string)requestParams["origem"];
            origem = origem.ToUpper();

            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(origem))
            {
                List<Anexos> anexos = new List<Anexos>();
                switch (origem)
                {
                    case "TEMA": anexos = DBAttachments.GetById(TipoOrigemAnexos.TemaFormacao, id);
                        break;
                    case "PEDIDOS": anexos = DBAttachments.GetById(TipoOrigemAnexos.PedidoFormacao, id);
                        break;
                    case "ACCAO": anexos = DBAttachments.GetById(TipoOrigemAnexos.AccaoFormacao, id);
                        break;
                }

                List<AttachmentsViewModel> attachments = new List<AttachmentsViewModel>();
                anexos.ForEach(x => attachments.Add(DBAttachments.ParseToViewModel(x)));

                if(attachments != null && attachments.Count() > 0)
                {
                    return Json(attachments);
                }

                return Json("Sem Anexos");

            }
            return Json("Erro");
        }

        [HttpPost]
        public JsonResult DeleteAttachment([FromBody] AttachmentsViewModel attach)
        {
            if (attach == null)
                return Json(false);

            string _uploadPath = UploadPath;

            switch (attach.DocType)
            {
                case TipoOrigemAnexos.PedidoFormacao: _uploadPath += RequestAttachmentsPath;
                    break;
                case TipoOrigemAnexos.TemaFormacao: _uploadPath += SubjectImagePath;
                    break;
                case TipoOrigemAnexos.AccaoFormacao: _uploadPath += TrainingImagePath;
                    break;
                default:
                    return Json(false);
            }

            _uploadPath += (attach.DocNumber + '\\' + attach.Url);
            try
            {
                System.IO.File.Delete(_uploadPath);
                DBAttachments.Delete(DBAttachments.ParseToDB(attach));
                return Json(true);
            }
            catch (Exception ex)
            {

                return Json(false);
            }
        }
                

        #region SGPPF actions
        public ActionResult Catalogo()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);

            if (UPerm != null && UPerm.Read.Value)
            {
                return View(); 
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        public ActionResult MeusPedidos()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
            

            if (UPerm != null && UPerm.Read.Value)
            {
                if(userConfig.TipoUtilizadorFormacao == null)
                {                    
                    // os utilizadores deverão ter um tipo de utilizador definido. 
                    // O tipo por defeito deverá ser o de Formando (1-Formando)
                    return RedirectToAction("AccessDenied", "Error");
                }

                ViewBag.OnlyActive = true;
                ViewBag.RequestOrigin = (int)Enumerations.AcademiaOrigemAcessoFuncionalidade.MeusPedidos;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public ActionResult AprovacaoChefia()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
            ConfiguracaoAprovacaoUtilizador cfgUser = new ConfiguracaoAprovacaoUtilizador(User.Identity.Name, userConfig.TipoUtilizadorFormacao.Value, TrainingRequestApprovalType);

            if (UPerm != null && UPerm.Read.Value && cfgUser.IsChief())
            {
                ViewBag.OnlyCompleted = true;
                ViewBag.RequestOrigin = (int)Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuChefia;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public ActionResult AprovacaoDireccao()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);

            ConfiguracaoAprovacaoUtilizador cfgUser = new ConfiguracaoAprovacaoUtilizador(User.Identity.Name, userConfig.TipoUtilizadorFormacao.Value, TrainingRequestApprovalType);

            bool isDirector = cfgUser.IsDirector();

            if (UPerm != null && UPerm.Read.Value && isDirector)
            {
                ViewBag.OnlyCompleted = false;
                ViewBag.RequestOrigin = (int)Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuDirector;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public ActionResult AutorizacaoCA()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);

            if (UPerm != null && UPerm.Read.Value &&
                userConfig.TipoUtilizadorFormacao.Value == (int)Enumerations.TipoUtilizadorFluxoPedidoFormacao.ConselhoAdministracao)
            {
                ViewBag.OnlyCompleted = false;
                ViewBag.RequestOrigin = (int)Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuCA;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public ActionResult GestaoPedidos()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);

            if (UPerm != null && UPerm.Read.Value &&
                userConfig.TipoUtilizadorFormacao.Value == (int)Enumerations.TipoUtilizadorFluxoPedidoFormacao.GestorFormacao)
            {
                ViewBag.OnlyCompleted = false;
                ViewBag.RequestOrigin = (int)Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuGestao;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public ActionResult GestaoCatalogo()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);

            if (UPerm != null && UPerm.Read.Value &&
                userConfig.TipoUtilizadorFormacao.Value == (int)Enumerations.TipoUtilizadorFluxoPedidoFormacao.GestorFormacao)
            {
                ViewBag.onlyActives = false;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public ActionResult DetalhesTema(string id, string codInterno, bool fromCatalogo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);

            if (UPerm != null && UPerm.Read.Value &&
                userConfig.TipoUtilizadorFormacao.Value == (int)Enumerations.TipoUtilizadorFluxoPedidoFormacao.GestorFormacao)
            {
                TemaFormacao tema = DBAcademia.__GetDetailsTema(id);
                if (fromCatalogo)
                {
                    tema.AccoesTema = tema.AccoesTema.Where(a => a.Activa.Value == 1).ToList();
                }
                if(tema != null)
                {
                    ViewBag.idTema = id;
                    ViewBag.descricaoTema = tema.DescricaoTema;
                    ViewBag.codInterno = codInterno;
                    ViewBag.fromCatalogo = fromCatalogo;

                    return View();
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Error");
                }
                
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public ActionResult DetalhesAccao(string id, string codInterno, bool fromTema)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);

            if (UPerm != null && UPerm.Read.Value)
            {
                AccaoFormacao accao = DBAcademia.__GetDetailsAccaoFormacao(id);
               
                if (accao == null)
                {
                    return RedirectToAction("AccessDenied", "Error");
                }

                ViewBag.idAccao = id;
                ViewBag.codInterno = codInterno;
                ViewBag.designacaoAccao = accao.DesignacaoAccao;
                ViewBag.isAcademiaUser = userConfig.TipoUtilizadorFormacao.Value == (int)Enumerations.TipoUtilizadorFluxoPedidoFormacao.GestorFormacao;
                ViewBag.fromTema = fromTema;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        #endregion

    }
}