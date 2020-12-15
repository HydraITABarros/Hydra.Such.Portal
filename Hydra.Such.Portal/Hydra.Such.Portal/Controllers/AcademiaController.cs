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
        #region properties and class methods

        
        private readonly GeneralConfigurations _config;
        private readonly IHostingEnvironment _hostingEnvironment;

        private string UploadPath { get; set; }
        private const string SubjectImagePath = "\\TemaFormacao\\";
        private const string TrainingImagePath = "\\AccaoFormacao\\";
        private const string AttachmentPath = "\\PedidoFormacao\\";
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

        private bool DocumentTypeAllowed(string fileExt)
        {
            if (string.IsNullOrEmpty(fileExt))
                return false;

            if (fileExt.StartsWith("."))
                fileExt = fileExt.Substring(1).ToLower();

            switch (fileExt)
            {
                case "csv":
                case "doc":
                case "docx":
                case "dwf":
                case "dwg":
                case "dxf":
                case "odp":
                case "ods":
                case "odt":
                case "oth":
                case "ott":
                case "pdf":
                case "ppsx":
                case "ppt":
                case "pptx":
                case "rtf":
                case "txt":
                case "xls":
                case "xlsx":
                    return true;
                default: return false;
            }
        }

        private string TypeOfChangeToText(int type)
        {
            var items = EnumerablesFixed.TipoAlteracaoPedidoFormacao.OrderBy(t => t.Id).ToList();
            foreach (var i in items)
            {
                if (i.Id == type)
                {
                    return i.Value;
                }
            };

            return string.Empty;
        }
        #endregion

        [HttpPost]
        public JsonResult CriarPedido()
        {
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
            PedidoParticipacaoFormacao pedido = DBAcademia.__CriarPedidoFormacao(userConfig);

            if (pedido != null)
            {
                bool registoCriado = DBAcademia.__CriarRegistoAlteracaoPedidoFormacao(
                    pedido, 
                    (int)Enumerations.TipoAlteracaoPedidoFormacao.Criacao, 
                    TypeOfChangeToText((int)Enumerations.TipoAlteracaoPedidoFormacao.Criacao), 
                    User.Identity.Name);

                

                if (!registoCriado)
                {
                    DBAcademia.__CriarRegistoAlteracaoPedidoFormacao(
                        pedido, 
                        (int)Enumerations.TipoAlteracaoPedidoFormacao.Criacao,
                        TypeOfChangeToText((int)Enumerations.TipoAlteracaoPedidoFormacao.Criacao), 
                        User.Identity.Name);
                }

                return Json(pedido.IdPedido);
            }
            return Json("0");
        }

        [HttpPost]
        public JsonResult CriarPedidoApartirDeAccao([FromBody] AccaoFormacaoView accao)
        {
            if (accao != null)
            {
                ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
                Formando formando = new Formando(userConfig.EmployeeNo);
                formando.GetAllTrainingRequestsForTrainne();

                if (formando == null)
                {
                    ErrorHandler blankTrainee = new ErrorHandler()
                    {
                        eReasonCode = -1,
                        eMessage = "Formando inexistente"
                    };

                    return Json(blankTrainee);
                }

                string idPedido = string.Empty;


                if (formando.AlreadyRegisteredForCourse(accao.IdAccao, ref idPedido))
                {
                    ErrorHandler alreadyRegistered = new ErrorHandler()
                    {
                        eReasonCode = -2,
                        eMessage = idPedido
                    };

                    return Json(alreadyRegistered);
                }

                PedidoParticipacaoFormacao pedido = DBAcademia.__CriarPedidoFormacao(accao.ParseToDb(), formando, userConfig);
                if (pedido != null)
                {
                    bool registoCriado = DBAcademia.__CriarRegistoAlteracaoPedidoFormacao(
                        pedido, (int)Enumerations.TipoAlteracaoPedidoFormacao.Criacao,
                        TypeOfChangeToText((int)Enumerations.TipoAlteracaoPedidoFormacao.Criacao), 
                        User.Identity.Name);

                    if (!registoCriado)
                    {
                        DBAcademia.__CriarRegistoAlteracaoPedidoFormacao(
                            pedido, 
                            (int)Enumerations.TipoAlteracaoPedidoFormacao.Criacao,
                            TypeOfChangeToText((int)Enumerations.TipoAlteracaoPedidoFormacao.Criacao), 
                            User.Identity.Name);
                    }

                    ErrorHandler sucess = new ErrorHandler()
                    {
                        eReasonCode = 0,
                        eMessage = pedido.IdPedido
                    };

                    return Json(sucess);
                }
                
            }

            ErrorHandler blankCourse = new ErrorHandler()
            {
                eReasonCode = -9,
                eMessage = "Acção inexistente"
            };

            return Json(blankCourse);
        }

        [HttpPost]
        public JsonResult GetDetalhePedido([FromBody] JObject reqParam)
        {
            string idPedido = reqParam["idPedido"] == null ? string.Empty : (string)reqParam["idPedido"];

            if (!string.IsNullOrEmpty(idPedido))
            {
                PedidoParticipacaoFormacaoView pedido = new PedidoParticipacaoFormacaoView(DBAcademia.__GetDetailsPedidoFormacao(idPedido));

                return Json(pedido);
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult GetMeusPedidos([FromBody] JObject requestParams)
        {
            //ClickSource = 0;
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
            ConfiguracaoAprovacaoUtilizador cfgUser = new ConfiguracaoAprovacaoUtilizador(userConfig, TrainingRequestApprovalType);

            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            if(UPerm != null && UPerm.Read.Value)
            {
                bool apenasActivos = requestParams["apenasActivos"] == null ? false : (bool)requestParams["apenasActivos"];
                List<PedidoParticipacaoFormacao> meusPedidos = DBAcademia.__GetAllPedidosFormacao(User.Identity.Name, userConfig.EmployeeNo, apenasActivos);

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
            ConfiguracaoAprovacaoUtilizador cfgUser = new ConfiguracaoAprovacaoUtilizador(userConfig, TrainingRequestApprovalType);
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
        [Route("Academia/GetCatalogoToPedido")]
        public JsonResult GetCatalogoToPedido()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
          
            if (UPerm != null)
            {
                List<TemaFormacao> temas = DBAcademia.__GetCatalogo();
                List<TemaFormacaoView> temasV = new List<TemaFormacaoView>();

                foreach (var item in temas)
                {
                    temasV.Add(new TemaFormacaoView(item));
                }

                return Json(temasV);
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult GetCatalogo([FromBody] JObject requestParams)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
            bool apenasActivos = requestParams["apenasActivos"] == null ? true : (bool)requestParams["apenasActivos"];
            

            if (UPerm != null)
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

            TemaFormacao tema = DBAcademia.__GetDetailsTema(idTema);
            
            TemaFormacaoView temaV = new TemaFormacaoView(tema);
            temaV.AccoesDoTema(tema);

            try
            {
                if (fromCatalogo)
                {
                    if (temaV.AccoesTema != null && temaV.AccoesTema.Count > 0)
                    {
                        temaV.AccoesTema = temaV.AccoesTema.Where(a => a.Activa.Value == 1 && a.DataInicio > DateTime.Now).ToList();
                        temaV.Accoes = temaV.Accoes.Where(a => a.AccaoActiva && a.DataInicio > DateTime.Now).ToList();
                    }
                }
                else
                {
                    temaV.ImagensDoTema();
                }
            }
            catch (Exception ex)
            {

                return Json(null);
            }

            return Json(temaV);
        }

        [HttpPost]
        public JsonResult GetDetalhesTemaParaPedido([FromBody] JObject requestParams)
        {
            string idTema = requestParams["idTema"] == null ? null : (string)requestParams["idTema"];
            if (idTema != null)
            {
                TemaFormacaoView tema = DBAcademia.__GetDetailsTemaView(idTema);

                return Json(tema);
            }
            return Json(null);
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
                AccaoFormacao accao = DBAcademia.__GetDetailsAccaoFormacao(idAccao);
                AccaoFormacaoView accaoV = new AccaoFormacaoView(accao);
                accaoV.SessoesDaAccao(accao);
                accaoV.ImagensDaAccao();
                accaoV.DetalhesEntidade();

                return Json(accaoV);
            }
            catch (Exception ex)
            {

                return Json(null);
            }            
        }

       [HttpPost]
       public JsonResult GetEmpregadosHierarquia([FromBody] JObject obj)
        {
            if (obj != null)
            {
                bool isChief = obj["isChief"] == null ? false : (bool)obj["isChief"];
                bool isDirector = obj["isDirector"] == null ? false : (bool)obj["isDirector"];
                ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
                ConfiguracaoAprovacaoUtilizador cfgUser = new ConfiguracaoAprovacaoUtilizador(userConfig, TrainingRequestApprovalType);

                if (isDirector)
                {
                    List<Formando> formandos = DBAcademia.__GetAllFormandos(cfgUser, Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuDirector);
                    return Json(formandos);
                }

                if (isChief)
                {
                    List<Formando> formandos = DBAcademia.__GetAllFormandos(cfgUser, Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuChefia);
                    return Json(formandos);
                }
            }
            return Json(null);
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
                TemaFormacao tema = DBAcademia.__GetDetailsTema(image.DocNumber);
                TemaFormacaoView temaV = new TemaFormacaoView(tema);
                temaV.ImagensDoTema();

                if(temaV != null)
                {
                    if(temaV.ImagensTema != null && temaV.ImagensTema.Count > 0)
                    {
                       foreach(var i in temaV.ImagensTema)
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

                        temaV.UrlImagem = temaV.ImagensTema.Where(i => i.Visivel.Value).FirstOrDefault() != null ?
                             temaV.ImagensTema.Where(i => i.Visivel.Value).FirstOrDefault().Url : string.Empty;

                        return Json(DBAcademia.__UpdateTemaFormacao(temaV));
                    }

                }
            }

            if (image.DocType == TipoOrigemAnexos.AccaoFormacao && !string.IsNullOrEmpty(image.DocNumber))
            {
                AccaoFormacao accao = DBAcademia.__GetDetailsAccaoFormacao(image.DocNumber);
                AccaoFormacaoView accaoV = new AccaoFormacaoView(accao);
                accaoV.ImagensDaAccao();

                if (accaoV != null)
                {
                    if (accaoV.ImagensAccao != null && accaoV.ImagensAccao.Count() > 0)
                    {
                        foreach (var item in accaoV.ImagensAccao)
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

                        accaoV.UrlImagem = accaoV.ImagensAccao.Where(i => i.Visivel.Value).FirstOrDefault() != null ?
                            accaoV.ImagensAccao.Where(i => i.Visivel.Value).FirstOrDefault().Url : string.Empty;

                        return Json(DBAcademia.__UpdateAccaoFormacao(accaoV));
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

        [HttpPost]
        public JsonResult UpdatePedido([FromBody] PedidoParticipacaoFormacaoView pedido)
        {
            if (pedido != null)
            {
                pedido.UtilizadorUltimaModificacao = User.Identity.Name;
                pedido.DataHoraUltimaModificacao = DateTime.Now;

                return Json(DBAcademia.__UpdatePedidoFormacao(pedido.ParseToDb()));
            }
            return Json(false);
        }


        [HttpGet]
        [Route("Academia/DownloadImage/{imgPath}/{docId}/{id}")]
        public FileStreamResult DownloadImage(string imgPath, string docId, string id)
        {
            try
            {
                imgPath = "\\" + imgPath + "\\";
                string _path = UploadPath + imgPath + docId + "\\" + id;
                FileStream file = new FileStream(_path, FileMode.Open);

                if (IsImage(Path.GetExtension(file.Name)))
                {
                    string mimeType = GetMimeType(file.Name);

                    if (mimeType != "application/unknown")
                    {
                        return new FileStreamResult(file, mimeType);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {

                return null;
            }         
        }

        [HttpGet]
        [Route("Academia/DownloadDocument/{docId}/{id}")]
        public FileStreamResult DownloadDocument(string docId, string id)
        {
            try
            {
                string _path = UploadPath + AttachmentPath + docId + "\\" + id;

                FileStream file = new FileStream(_path, FileMode.Open);
                if (DocumentTypeAllowed(Path.GetExtension(file.Name)))
                {
                    string mimeType = GetMimeType(file.Name);
                    if (mimeType != "application/unknown")
                    {
                        return new FileStreamResult(file, mimeType);
                    }
                }
            }
            catch (Exception)
            {

                return null;
            }
            return null;
        }

        [HttpPost]       
        [Route("Academia/ImageUpload")]
        [Route("Academia/ImageUpload/{imgPath}/{id}")]
        public JsonResult ImageUpload(string imgPath, string id)
        {
            try
            {
                string imgTipo = imgPath.ToUpper();
                TipoOrigemAnexos tipo;
                switch (imgTipo)
                {
                    case "TEMAFORMACAO": tipo = TipoOrigemAnexos.TemaFormacao;
                        break;
                    case "ACCAOFORMACAO": tipo = TipoOrigemAnexos.AccaoFormacao;
                        break;
                    case "PEDIDOFORMACAO": tipo = TipoOrigemAnexos.PedidoFormacao;
                        break;
                    default: tipo = 0;
                        break;
                }

                imgPath = "\\" + imgPath + "\\";
                //string _uploadPath = UploadPath + SubjectImagePath;
                string _uploadPath = UploadPath + imgPath;
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
                                    TipoOrigem = tipo,
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
                    }
                    else
                    {
                        return Json("-1");
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
        [Route("Academia/DocumentUpload")]
        [Route("Academia/DocumentUpload/{id}")]
        public JsonResult DocumentUpload(string id)
        {
            try
            {
                string _uploadPath = UploadPath + AttachmentPath;
                var files = Request.Form.Files;
                
                foreach(var f in files)
                {
                    if (DocumentTypeAllowed(Path.GetExtension(f.FileName)))
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
                                    TipoOrigem = TipoOrigemAnexos.PedidoFormacao,
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
                    }
                    else
                    {
                        return Json("-1");
                    }
                }

                return Json("0");
            }
            catch (Exception ex)
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
                    case "PEDIDO": anexos = DBAttachments.GetById(TipoOrigemAnexos.PedidoFormacao, id);
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
                case TipoOrigemAnexos.PedidoFormacao: _uploadPath += AttachmentPath;
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
            ConfiguracaoAprovacaoUtilizador cfgUser = new ConfiguracaoAprovacaoUtilizador(userConfig, TrainingRequestApprovalType);

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

            ConfiguracaoAprovacaoUtilizador cfgUser = new ConfiguracaoAprovacaoUtilizador(userConfig, TrainingRequestApprovalType);

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

            if (UPerm != null && UPerm.Read.Value)
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

        public ActionResult DetalhePedido(string id)
        {
            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
            ConfiguracaoAprovacaoUtilizador cfgUser = new ConfiguracaoAprovacaoUtilizador(userConfig, TrainingRequestApprovalType);

            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AcademiaFormacao);
            if (UPerm != null && UPerm.Read.Value && cfgUser != null)
            {
                ViewBag.idPedido = id;
                ViewBag.isChief = cfgUser.IsChief();
                ViewBag.isDirector = cfgUser.IsDirector();
                ViewBag.isTrainingManager = userConfig.TipoUtilizadorFormacao.Value == (int)Enumerations.TipoUtilizadorFluxoPedidoFormacao.GestorFormacao;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            
        }

        // Test Action
        public ActionResult TestCatalog()
        {
            return View();
        }
        #endregion

    }
}