using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.NAV;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.Logic.Nutrition;
using System.Collections.Generic;
using Hydra.Such.Data.ViewModel.Nutrition;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Microsoft.AspNetCore.Http;
using System.Text;
using NPOI.HSSF.UserModel;
using System.Linq;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data;
using System;

using System.Web;
using System.Drawing;
using Hydra.Such.Data.Extensions;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class ProdutosController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProdutosController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult List()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Localizações);
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

        public IActionResult Detalhes(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Localizações);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.No = id ?? "";
                if (ViewBag.No != "")
                {
                    ViewBag.NoDisable = true;
                    ViewBag.ButtonHide = 0;
                }
                else
                {
                    ViewBag.NoDisable = false;
                    ViewBag.ButtonHide = 1;
                }

                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public JsonResult GetAllProdutos()
        {
            List<FichaProdutoViewModel> result = DBFichaProduto.ParseToViewModel(DBFichaProduto.GetAll());

            result.ForEach(x =>
            {
                x.ListaDeMateriaisText = x.ListaDeMateriais.HasValue ? x.ListaDeMateriais == true ? "Sim" : "Não" : "";
                x.TarasText = x.Taras.HasValue ? x.Taras == true ? "Sim" : "Não" : "";
                x.CereaisText = x.Cereais.HasValue ? x.Cereais == true ? "Sim" : "Não" : "";
                x.CrustaceosText = x.Crustaceos.HasValue ? x.Crustaceos == true ? "Sim" : "Não" : "";
                x.OvosText = x.Ovos.HasValue ? x.Ovos == true ? "Sim" : "Não" : "";
                x.PeixesText = x.Peixes.HasValue ? x.Peixes == true ? "Sim" : "Não" : "";
                x.AmendoinsText = x.Amendoins.HasValue ? x.Amendoins == true ? "Sim" : "Não" : "";
                x.SojaText = x.Soja.HasValue ? x.Soja == true ? "Sim" : "Não" : "";
                x.LeiteText = x.Leite.HasValue ? x.Leite == true ? "Sim" : "Não" : "";
                x.FrutasDeCascaRijaText = x.FrutasDeCascaRija.HasValue ? x.FrutasDeCascaRija == true ? "Sim" : "Não" : "";
                x.AipoText = x.Aipo.HasValue ? x.Aipo == true ? "Sim" : "Não" : "";
                x.MostardaText = x.Mostarda.HasValue ? x.Mostarda == true ? "Sim" : "Não" : "";
                x.SementesDeSesamoText = x.SementesDeSesamo.HasValue ? x.SementesDeSesamo == true ? "Sim" : "Não" : "";
                x.DioxidoDeEnxofreESulfitosText = x.DioxidoDeEnxofreESulfitos.HasValue ? x.DioxidoDeEnxofreESulfitos == true ? "Sim" : "Não" : "";
                x.TremocoText = x.Tremoco.HasValue ? x.Tremoco == true ? "Sim" : "Não" : "";
                x.MoluscosText = x.Moluscos.HasValue ? x.Moluscos == true ? "Sim" : "Não" : "";
                x.TipoText = x.Tipo.HasValue ? EnumerablesFixed.ProdutosTipo.Where(y => y.Id == x.Tipo).FirstOrDefault().Value : "";
            });

            return Json(result);
        }

        public JsonResult GetProdutoNo([FromBody]string noProduto)
        {
            FichaProdutoViewModel result = new FichaProdutoViewModel();

            if (!string.IsNullOrEmpty(noProduto))
            {
                result = DBFichaProduto.ParseToViewModel(DBFichaProduto.GetById(noProduto));
                result.ListUnidadeMedidaProduto = DBUnitMeasureProduct.ParseToViewModel(DBUnitMeasureProduct.GetByProduto(noProduto));
            }

            return Json(result);
        }

        [HttpPost]
        //Cria um Produto
        public JsonResult CreateProduto([FromBody] FichaProdutoViewModel Produto)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                Produto.No = Produto.Code;

                if (string.IsNullOrEmpty(Produto.No))
                {
                    //Get Ficha de Produto Numeration
                    int idNumeration = DBNumerationConfigurations.GetAll().Where(x => x.Descrição == "Numeração Produtos").FirstOrDefault().Id;
                    string ProdutoNo = DBNumerationConfigurations.GetNextNumeration(idNumeration, true, false);

                    //Update Last Numeration Used
                    ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(idNumeration);
                    ConfigNumerations.ÚltimoNºUsado = ProdutoNo;
                    DBNumerationConfigurations.Update(ConfigNumerations);

                    //New Produto
                    Produto.No = ProdutoNo;
                }

                Produto.DataHoraCriacao = DateTime.Now;
                Produto.UtilizadorCriacao = User.Identity.Name;

                if (DBFichaProduto.GetById(Produto.No) == null)
                {
                    if (string.IsNullOrEmpty(Produto.UnidadeMedidaBase))
                        Produto.UnidadeMedidaBase = null;
                    if (DBFichaProduto.Create(DBFichaProduto.ParseToDatabase(Produto)) != null)
                    {
                        result.aux = Produto.No;
                        result.eReasonCode = 0;
                        result.eMessage = "Foi criado com sucesso o Produto.";
                    }
                    else
                    {
                        result.eReasonCode = 20;
                        result.eMessage = "Ocorreu um erro ao criar o Produto.";
                    }
                }
                else
                {
                    result.eReasonCode = 10;
                    result.eMessage = "Já existe um Produto com esse Número.";
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(result);
        }

        [HttpPost]
        //Atualiza um Produto
        public JsonResult UpdateProduto([FromBody] FichaProdutoViewModel Produto)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                //Update Produto
                Produto.DataHoraModificacao = DateTime.Now;
                Produto.UtilizadorModificacao = User.Identity.Name;

                if (string.IsNullOrEmpty(Produto.UnidadeMedidaBase))
                    Produto.UnidadeMedidaBase = null;
                if (DBFichaProduto.Update(DBFichaProduto.ParseToDatabase(Produto)) != null)
                {
                    result.eReasonCode = 0;
                    result.eMessage = "Foi atualizado com sucesso o Produto.";
                }
                else
                {
                    result.eReasonCode = 10;
                    result.eMessage = "Ocorreu um erro ao atualizar o Produto.";
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(result);
        }

        [HttpPost]
        //Apaga um Produto
        public JsonResult DeleteProduto([FromBody] FichaProdutoViewModel Produto)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                if (DBUnitMeasureProduct.Delete(DBUnitMeasureProduct.GetByProduto(Produto.No)) == true)
                {
                    if (DBFichaProduto.Delete(DBFichaProduto.GetById(Produto.No)) == true)
                    {
                        result.eReasonCode = 0;
                        result.eMessage = "Foi eliminado com sucesso o Produto.";
                    }
                    else
                    {
                        result.eReasonCode = 10;
                        result.eMessage = "Ocorreu um erro ao eliminar o Produto.";
                    }
                }
                else
                {
                    result.eReasonCode = 20;
                    result.eMessage = "Ocorreu um erro ao eliminar as Unidades de Medidas de Produtos.";
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(result);
        }

        [HttpPost]
        //Cria uma nova Unidade Medida de Produto
        public JsonResult CreateUnidadeMedidaProduto([FromBody] UnitMeasureProductViewModel UMP)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                if (DBUnitMeasureProduct.GetByProdutoCode(UMP.ProductNo, UMP.Code) == null)
                {
                    //New Unidade Medida Produto
                    UMP.CreateDate = DateTime.Now;
                    UMP.CreateUser = User.Identity.Name;

                    if (DBUnitMeasureProduct.Create(DBUnitMeasureProduct.ParseToDb(UMP)) != null)
                    {
                        result.eReasonCode = 0;
                        result.eMessage = "Foi adicionado com sucesso a Unidade de Medida de Produto.";
                    }
                    else
                    {
                        result.eReasonCode = 10;
                        result.eMessage = "Ocorreu um erro ao criar a Unidade de Medida de Produto.";
                    }
                }
                else
                {
                    result.eReasonCode = 20;
                    result.eMessage = "Já existe uma Unidade de Medida de Produto com este Código de Medida.";
                }

            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(result);
        }

        [HttpPost]
        //Atualiza uma Unidade Medida Produto
        public JsonResult UpdateUnidadeMedidaProduto([FromBody] UnitMeasureProductViewModel UMP)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                //Update Unidade Medida Produto
                UMP.UpdateDate = DateTime.Now;
                UMP.UpdateUser = User.Identity.Name;

                if (DBUnitMeasureProduct.Update(DBUnitMeasureProduct.ParseToDb(UMP)) != null)
                {
                    result.eReasonCode = 0;
                    result.eMessage = "Foi atualizado com sucesso a Unidade de Medida de Produto.";
                }
                else
                {
                    result.eReasonCode = 10;
                    result.eMessage = "Ocorreu um erro ao atualizar a Unidade de Medida de Produto.";
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(result);
        }

        [HttpPost]
        //Apaga uma Unidade Medida Produto
        public JsonResult DeleteUnidadeMedidaProduto([FromBody] UnitMeasureProductViewModel UMP)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                if (DBUnitMeasureProduct.Delete(DBUnitMeasureProduct.ParseToDb(UMP)) == true)
                {
                    result.eReasonCode = 0;
                    result.eMessage = "Foi eliminado com sucesso a Unidade de Medida de Produto.";
                }
                else
                {
                    result.eReasonCode = 10;
                    result.eMessage = "Ocorreu um erro ao eliminar a Unidade de Medida de Produto.";
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(result);
        }

        public IActionResult Movimentos(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Localizações);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ProjectNo = id ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public bool SetSessionMovimentoProduto([FromBody] FichaProdutoViewModel data)
        {
            if (data.No != null)
            {
                HttpContext.Session.SetString("productNo", data.No);
                return true;
            }
            return false;
        }

        public JsonResult GetMovementProduct()
        {
            List<ProductMovementViewModel> result;
            if (HttpContext.Session.GetString("productNo") == null)
            {
                result = DBProductMovement.ParseToViewModel(DBProductMovement.GetAll());
            }
            else
            {
                string nProduct = HttpContext.Session.GetString("productNo");
                result = DBProductMovement.ParseToViewModel(DBProductMovement.GetByNoProduto(nProduct));
                HttpContext.Session.Remove("productNo");
            }
            return Json(result);
        }

        #region EXCEL
        //1
        [HttpPost]
        public async Task<JsonResult> ExportToExcel_Produtos([FromBody] List<FichaProdutoViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

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
                ISheet excelSheet = workbook.CreateSheet("Movimentos de Produtos");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["no"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº");
                    Col = Col + 1;
                }
                if (dp["descricao"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Descrição");
                    Col = Col + 1;
                }
                if (dp["listaDeMateriaisText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Lista De Materiais");
                    Col = Col + 1;
                }
                if (dp["unidadeMedidaBase"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Unidade Medida Base");
                    Col = Col + 1;
                }
                if (dp["noPrateleira"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Prateleira");
                    Col = Col + 1;
                }
                if (dp["precoUnitario"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Preço Unitário");
                    Col = Col + 1;
                }
                if (dp["custoUnitario"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Custo Unitário");
                    Col = Col + 1;
                }
                if (dp["inventario"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Inventário");
                    Col = Col + 1;
                }
                if (dp["valorEnergetico"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Valor Energético");
                    Col = Col + 1;
                }
                if (dp["valorEnergetico100g"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Valor Energético 100g");
                    Col = Col + 1;
                }
                if (dp["proteinas"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Proteínas");
                    Col = Col + 1;
                }
                if (dp["proteinas100g"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Proteínas 100g");
                    Col = Col + 1;
                }
                if (dp["glicidos"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Glícidos");
                    Col = Col + 1;
                }
                if (dp["glicidos100g"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Glícidos 100g");
                    Col = Col + 1;
                }
                if (dp["lipidos"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Lípidos");
                    Col = Col + 1;
                }
                if (dp["lipidos100g"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Lípidos 100g");
                    Col = Col + 1;
                }
                if (dp["fibraAlimentar"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Fibra Alimentar");
                    Col = Col + 1;
                }
                if (dp["fibraAlimentar100g"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Fibra Alimentar 100g");
                    Col = Col + 1;
                }
                if (dp["quantUnidadeMedida"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Quantidade Unidade Medida");
                    Col = Col + 1;
                }
                if (dp["gramasPorQuantUnidMedida"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Gramas Por Quantidade Unidade Medida");
                    Col = Col + 1;
                }
                if (dp["tipoRefeicao"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo Refeição");
                    Col = Col + 1;
                }
                if (dp["descricaoRefeicao"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Descrição Refeição");
                    Col = Col + 1;
                }
                if (dp["tarasText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Taras");
                    Col = Col + 1;
                }
                if (dp["acidosGordosSaturados"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Ácidos Gordos Saturados");
                    Col = Col + 1;
                }
                if (dp["acucares"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Açúcares");
                    Col = Col + 1;
                }
                if (dp["sal"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Sal");
                    Col = Col + 1;
                }
                if (dp["cereaisText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cereais");
                    Col = Col + 1;
                }
                if (dp["crustaceosText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Crustáceos");
                    Col = Col + 1;
                }
                if (dp["ovosText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Ovos");
                    Col = Col + 1;
                }
                if (dp["peixesText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Peixes");
                    Col = Col + 1;
                }
                if (dp["amendoinsText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Amendoins");
                    Col = Col + 1;
                }
                if (dp["sojaText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Soja");
                    Col = Col + 1;
                }
                if (dp["leiteText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Leite");
                    Col = Col + 1;
                }
                if (dp["frutasDeCascaRijaText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Frutas De Casca Rija");
                    Col = Col + 1;
                }
                if (dp["aipoText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Aipo");
                    Col = Col + 1;
                }
                if (dp["mostardaText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Mostarda");
                    Col = Col + 1;
                }
                if (dp["sementesDeSesamoText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Sementes De Sésamo");
                    Col = Col + 1;
                }
                if (dp["dioxidoDeEnxofreESulfitosText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Dióxido De Enxofre E Súlfitos");
                    Col = Col + 1;
                }
                if (dp["tremocoText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tremoço");
                    Col = Col + 1;
                }
                if (dp["moluscosText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Moluscos");
                    Col = Col + 1;
                }
                if (dp["tipoText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo");
                    Col = Col + 1;
                }
                if (dp["vitaminaA"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Vitamina A");
                    Col = Col + 1;
                }
                if (dp["vitaminaD"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Vitamina D");
                    Col = Col + 1;
                }
                if (dp["colesterol"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Colesterol");
                    Col = Col + 1;
                }
                if (dp["sodio"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Sódio");
                    Col = Col + 1;
                }
                if (dp["potacio"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Potácio");
                    Col = Col + 1;
                }
                if (dp["calcio"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cálcio");
                    Col = Col + 1;
                }
                if (dp["ferro"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Ferro");
                    Col = Col + 1;
                }
                if (dp["edivel"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Edível");
                    Col = Col + 1;
                }
                if (dp["alcool"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Alcool");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (FichaProdutoViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["no"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.No);
                            Col = Col + 1;
                        }
                        if (dp["descricao"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Descricao);
                            Col = Col + 1;
                        }
                        if (dp["listaDeMateriaisText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ListaDeMateriaisText);
                            Col = Col + 1;
                        }
                        if (dp["unidadeMedidaBase"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.UnidadeMedidaBase);
                            Col = Col + 1;
                        }
                        if (dp["noPrateleira"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.NoPrateleira);
                            Col = Col + 1;
                        }
                        if (dp["precoUnitario"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.PrecoUnitario.ToString());
                            Col = Col + 1;
                        }
                        if (dp["custoUnitario"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CustoUnitario.ToString());
                            Col = Col + 1;
                        }
                        if (dp["inventario"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Inventario.ToString());
                            Col = Col + 1;
                        }
                        if (dp["valorEnergetico"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ValorEnergetico.ToString());
                            Col = Col + 1;
                        }
                        if (dp["valorEnergetico100g"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ValorEnergetico100g.ToString());
                            Col = Col + 1;
                        }
                        if (dp["proteinas"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Proteinas.ToString());
                            Col = Col + 1;
                        }
                        if (dp["proteinas100g"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Proteinas100g.ToString());
                            Col = Col + 1;
                        }
                        if (dp["glicidos"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Glicidos.ToString());
                            Col = Col + 1;
                        }
                        if (dp["glicidos100g"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Glicidos100g.ToString());
                            Col = Col + 1;
                        }
                        if (dp["lipidos"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Lipidos.ToString());
                            Col = Col + 1;
                        }
                        if (dp["lipidos100g"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Lipidos100g.ToString());
                            Col = Col + 1;
                        }
                        if (dp["fibraAlimentar"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.FibraAlimentar.ToString());
                            Col = Col + 1;
                        }
                        if (dp["fibraAlimentar100g"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.FibraAlimentar100g.ToString());
                            Col = Col + 1;
                        }
                        if (dp["quantUnidadeMedida"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.QuantUnidadeMedida.ToString());
                            Col = Col + 1;
                        }
                        if (dp["gramasPorQuantUnidMedida"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.GramasPorQuantUnidMedida.ToString());
                            Col = Col + 1;
                        }
                        if (dp["tipoRefeicao"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TipoRefeicao);
                            Col = Col + 1;
                        }
                        if (dp["descricaoRefeicao"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DescricaoRefeicao);
                            Col = Col + 1;
                        }
                        if (dp["tarasText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TarasText);
                            Col = Col + 1;
                        }
                        if (dp["acidosGordosSaturados"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.AcidosGordosSaturados.ToString());
                            Col = Col + 1;
                        }
                        if (dp["acucares"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Acucares.ToString());
                            Col = Col + 1;
                        }
                        if (dp["sal"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Sal.ToString());
                            Col = Col + 1;
                        }
                        if (dp["cereaisText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CereaisText);
                            Col = Col + 1;
                        }
                        if (dp["crustaceosText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CrustaceosText);
                            Col = Col + 1;
                        }
                        if (dp["ovosText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.OvosText);
                            Col = Col + 1;
                        }
                        if (dp["peixesText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.PeixesText);
                            Col = Col + 1;
                        }
                        if (dp["amendoinsText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.AmendoinsText);
                            Col = Col + 1;
                        }
                        if (dp["sojaText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.SojaText);
                            Col = Col + 1;
                        }
                        if (dp["leiteText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.LeiteText);
                            Col = Col + 1;
                        }
                        if (dp["frutasDeCascaRijaText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.FrutasDeCascaRijaText);
                            Col = Col + 1;
                        }
                        if (dp["aipoText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.AipoText);
                            Col = Col + 1;
                        }
                        if (dp["mostardaText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.MostardaText);
                            Col = Col + 1;
                        }
                        if (dp["sementesDeSesamoText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.SementesDeSesamoText);
                            Col = Col + 1;
                        }
                        if (dp["dioxidoDeEnxofreESulfitosText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DioxidoDeEnxofreESulfitosText);
                            Col = Col + 1;
                        }
                        if (dp["tremocoText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TremocoText);
                            Col = Col + 1;
                        }
                        if (dp["moluscosText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.MoluscosText);
                            Col = Col + 1;
                        }
                        if (dp["tipoText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TipoText);
                            Col = Col + 1;
                        }
                        if (dp["vitaminaA"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.VitaminaA.ToString());
                            Col = Col + 1;
                        }
                        if (dp["vitaminaD"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.VitaminaD.ToString());
                            Col = Col + 1;
                        }
                        if (dp["colesterol"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Colesterol.ToString());
                            Col = Col + 1;
                        }
                        if (dp["sodio"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Sodio.ToString());
                            Col = Col + 1;
                        }
                        if (dp["potacio"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Potacio.ToString());
                            Col = Col + 1;
                        }
                        if (dp["calcio"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Calcio.ToString());
                            Col = Col + 1;
                        }
                        if (dp["ferro"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Ferro.ToString());
                            Col = Col + 1;
                        }
                        if (dp["edivel"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Edivel.ToString());
                            Col = Col + 1;
                        }
                        if (dp["alcool"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Alcool.ToString());
                            Col = Col + 1;
                        }

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
        public IActionResult ExportToExcelDownload_Produtos(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Artigos Produtos.xlsx");
        }

        //3
        [HttpPost]
        public JsonResult OnPostImport_Produtos()
        {
            var files = Request.Form.Files;
            List<FichaProdutoViewModel> ListToCreate = DBFichaProduto.ParseToViewModel(DBFichaProduto.GetAll());
            FichaProdutoViewModel nrow = new FichaProdutoViewModel();
            for (int i = 0; i < files.Count; i++)
            {
                IFormFile file = files[i];
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.WebRootPath + "\\Upload\\temp";
                string newPath = Path.Combine(webRootPath, folderName);
                StringBuilder sb = new StringBuilder();
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, file.FileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                        }
                        for (int j = (sheet.FirstRowNum + 1); j <= sheet.LastRowNum; j++)
                        {
                            IRow row = sheet.GetRow(j);
                            if (row != null)
                            {
                                nrow = new FichaProdutoViewModel();

                                nrow.No = row.GetCell(0) != null ? row.GetCell(0).ToString() : "";
                                nrow.Descricao = row.GetCell(1) != null ? row.GetCell(1).ToString() : "";
                                nrow.ListaDeMateriaisText = row.GetCell(2) != null ? row.GetCell(2).ToString() : "";
                                nrow.UnidadeMedidaBase = row.GetCell(3) != null ? row.GetCell(3).ToString() : "";
                                nrow.NoPrateleira = row.GetCell(4) != null ? row.GetCell(4).ToString() : "";
                                nrow.PrecoUnitarioText = row.GetCell(5) != null ? row.GetCell(5).ToString() : "";
                                nrow.CustoUnitarioText = row.GetCell(6) != null ? row.GetCell(6).ToString() : "";
                                nrow.InventarioText = row.GetCell(7) != null ? row.GetCell(7).ToString() : "";
                                nrow.ValorEnergeticoText = row.GetCell(8) != null ? row.GetCell(8).ToString() : "";
                                nrow.ValorEnergetico100gText = row.GetCell(9) != null ? row.GetCell(9).ToString() : "";
                                nrow.ProteinasText = row.GetCell(10) != null ? row.GetCell(10).ToString() : "";
                                nrow.Proteinas100gText = row.GetCell(11) != null ? row.GetCell(11).ToString() : "";
                                nrow.GlicidosText = row.GetCell(12) != null ? row.GetCell(12).ToString() : "";
                                nrow.Glicidos100gText = row.GetCell(13) != null ? row.GetCell(13).ToString() : "";
                                nrow.LipidosText = row.GetCell(14) != null ? row.GetCell(14).ToString() : "";
                                nrow.Lipidos100gText = row.GetCell(15) != null ? row.GetCell(15).ToString() : "";
                                nrow.FibraAlimentarText = row.GetCell(16) != null ? row.GetCell(16).ToString() : "";
                                nrow.FibraAlimentar100gText = row.GetCell(17) != null ? row.GetCell(17).ToString() : "";
                                nrow.QuantUnidadeMedidaText = row.GetCell(18) != null ? row.GetCell(18).ToString() : "";
                                nrow.GramasPorQuantUnidMedidaText = row.GetCell(19) != null ? row.GetCell(19).ToString() : "";
                                nrow.TipoRefeicao = row.GetCell(20) != null ? row.GetCell(20).ToString() : "";
                                nrow.DescricaoRefeicao = row.GetCell(21) != null ? row.GetCell(21).ToString() : "";
                                nrow.TarasText = row.GetCell(22) != null ? row.GetCell(22).ToString() : "";
                                nrow.AcidosGordosSaturadosText = row.GetCell(23) != null ? row.GetCell(23).ToString() : "";
                                nrow.AcucaresText = row.GetCell(24) != null ? row.GetCell(24).ToString() : "";
                                nrow.SalText = row.GetCell(25) != null ? row.GetCell(25).ToString() : "";
                                nrow.CereaisText = row.GetCell(26) != null ? row.GetCell(26).ToString() : "";
                                nrow.CrustaceosText = row.GetCell(27) != null ? row.GetCell(27).ToString() : "";
                                nrow.OvosText = row.GetCell(28) != null ? row.GetCell(28).ToString() : "";
                                nrow.PeixesText = row.GetCell(29) != null ? row.GetCell(29).ToString() : "";
                                nrow.AmendoinsText = row.GetCell(30) != null ? row.GetCell(30).ToString() : "";
                                nrow.SojaText = row.GetCell(31) != null ? row.GetCell(31).ToString() : "";
                                nrow.LeiteText = row.GetCell(32) != null ? row.GetCell(32).ToString() : "";
                                nrow.FrutasDeCascaRijaText = row.GetCell(33) != null ? row.GetCell(33).ToString() : "";
                                nrow.AipoText = row.GetCell(34) != null ? row.GetCell(34).ToString() : "";
                                nrow.MostardaText = row.GetCell(35) != null ? row.GetCell(35).ToString() : "";
                                nrow.SementesDeSesamoText = row.GetCell(36) != null ? row.GetCell(36).ToString() : "";
                                nrow.DioxidoDeEnxofreESulfitosText = row.GetCell(37) != null ? row.GetCell(37).ToString() : "";
                                nrow.TremocoText = row.GetCell(38) != null ? row.GetCell(38).ToString() : "";
                                nrow.MoluscosText = row.GetCell(39) != null ? row.GetCell(39).ToString() : "";
                                nrow.TipoText = row.GetCell(40) != null ? row.GetCell(40).ToString() : "";
                                nrow.VitaminaAText = row.GetCell(41) != null ? row.GetCell(41).ToString() : "";
                                nrow.VitaminaDText = row.GetCell(42) != null ? row.GetCell(42).ToString() : "";
                                nrow.ColesterolText = row.GetCell(43) != null ? row.GetCell(43).ToString() : "";
                                nrow.SodioText = row.GetCell(44) != null ? row.GetCell(44).ToString() : "";
                                nrow.PotacioText = row.GetCell(45) != null ? row.GetCell(45).ToString() : "";
                                nrow.CalcioText = row.GetCell(46) != null ? row.GetCell(46).ToString() : "";
                                nrow.FerroText = row.GetCell(47) != null ? row.GetCell(47).ToString() : "";
                                nrow.EdivelText = row.GetCell(48) != null ? row.GetCell(48).ToString() : "";
                                nrow.AlcoolText = row.GetCell(49) != null ? row.GetCell(49).ToString() : "";

                                ListToCreate.Add(nrow);
                            }
                        }
                    }
                }
                if (ListToCreate.Count > 0)
                {
                    foreach (FichaProdutoViewModel item in ListToCreate)
                    {
                        if (!string.IsNullOrEmpty(item.ListaDeMateriaisText))
                        {
                            item.ListaDeMateriais = item.ListaDeMateriaisText.ToLower() == "sim" ? true : false;
                            item.ListaDeMateriaisText = "";
                        }
                        if (!string.IsNullOrEmpty(item.PrecoUnitarioText))
                        {
                            item.PrecoUnitario = Convert.ToDecimal(item.PrecoUnitarioText);
                            item.PrecoUnitarioText = "";
                        }
                        if (!string.IsNullOrEmpty(item.CustoUnitarioText))
                        {
                            item.CustoUnitario = Convert.ToDecimal(item.CustoUnitarioText);
                            item.CustoUnitarioText = "";
                        }
                        if (!string.IsNullOrEmpty(item.InventarioText))
                        {
                            item.Inventario = Convert.ToDecimal(item.InventarioText);
                            item.InventarioText = "";
                        }
                        if (!string.IsNullOrEmpty(item.ValorEnergeticoText))
                        {
                            item.ValorEnergetico = Convert.ToDecimal(item.ValorEnergeticoText);
                            item.ValorEnergeticoText = "";
                        }
                        if (!string.IsNullOrEmpty(item.ValorEnergetico100gText))
                        {
                            item.ValorEnergetico100g = Convert.ToDecimal(item.ValorEnergetico100gText);
                            item.ValorEnergetico100gText = "";
                        }
                        if (!string.IsNullOrEmpty(item.ProteinasText))
                        {
                            item.Proteinas = Convert.ToDecimal(item.ProteinasText);
                            item.ProteinasText = "";
                        }
                        if (!string.IsNullOrEmpty(item.Proteinas100gText))
                        {
                            item.Proteinas100g = Convert.ToDecimal(item.Proteinas100gText);
                            item.Proteinas100gText = "";
                        }
                        if (!string.IsNullOrEmpty(item.GlicidosText))
                        {
                            item.Glicidos = Convert.ToDecimal(item.GlicidosText);
                            item.GlicidosText = "";
                        }
                        if (!string.IsNullOrEmpty(item.Glicidos100gText))
                        {
                            item.Glicidos100g = Convert.ToDecimal(item.Glicidos100gText);
                            item.Glicidos100gText = "";
                        }
                        if (!string.IsNullOrEmpty(item.LipidosText))
                        {
                            item.Lipidos = Convert.ToDecimal(item.LipidosText);
                            item.LipidosText = "";
                        }
                        if (!string.IsNullOrEmpty(item.Lipidos100gText))
                        {
                            item.Lipidos100g = Convert.ToDecimal(item.Lipidos100gText);
                            item.Lipidos100gText = "";
                        }
                        if (!string.IsNullOrEmpty(item.FibraAlimentarText))
                        {
                            item.FibraAlimentar = Convert.ToDecimal(item.FibraAlimentarText);
                            item.FibraAlimentarText = "";
                        }
                        if (!string.IsNullOrEmpty(item.FibraAlimentar100gText))
                        {
                            item.FibraAlimentar100g = Convert.ToDecimal(item.FibraAlimentar100gText);
                            item.FibraAlimentar100gText = "";
                        }
                        if (!string.IsNullOrEmpty(item.QuantUnidadeMedidaText))
                        {
                            item.QuantUnidadeMedida = Convert.ToDecimal(item.QuantUnidadeMedidaText);
                            item.QuantUnidadeMedidaText = "";
                        }
                        if (!string.IsNullOrEmpty(item.GramasPorQuantUnidMedidaText))
                        {
                            item.GramasPorQuantUnidMedida = Convert.ToDecimal(item.GramasPorQuantUnidMedidaText);
                            item.GramasPorQuantUnidMedidaText = "";
                        }
                        if (!string.IsNullOrEmpty(item.TarasText))
                        {
                            item.Taras = item.TarasText.ToLower() == "sim" ? true : false;
                            item.TarasText = "";
                        }
                        if (!string.IsNullOrEmpty(item.AcidosGordosSaturadosText))
                        {
                            item.AcidosGordosSaturados = Convert.ToDecimal(item.AcidosGordosSaturadosText);
                            item.AcidosGordosSaturadosText = "";
                        }
                        if (!string.IsNullOrEmpty(item.AcucaresText))
                        {
                            item.Acucares = Convert.ToDecimal(item.AcucaresText);
                            item.AcucaresText = "";
                        }
                        if (!string.IsNullOrEmpty(item.SalText))
                        {
                            item.Sal = Convert.ToDecimal(item.SalText);
                            item.SalText = "";
                        }
                        if (!string.IsNullOrEmpty(item.CereaisText))
                        {
                            item.Cereais = item.CereaisText.ToLower() == "sim" ? true : false;
                            item.CereaisText = "";
                        }
                        if (!string.IsNullOrEmpty(item.CrustaceosText))
                        {
                            item.Crustaceos = item.CrustaceosText.ToLower() == "sim" ? true : false;
                            item.CrustaceosText = "";
                        }
                        if (!string.IsNullOrEmpty(item.OvosText))
                        {
                            item.Ovos = item.OvosText.ToLower() == "sim" ? true : false;
                            item.OvosText = "";
                        }
                        if (!string.IsNullOrEmpty(item.PeixesText))
                        {
                            item.Peixes = item.PeixesText.ToLower() == "sim" ? true : false;
                            item.PeixesText = "";
                        }
                        if (!string.IsNullOrEmpty(item.AmendoinsText))
                        {
                            item.Amendoins = item.AmendoinsText.ToLower() == "sim" ? true : false;
                            item.AmendoinsText = "";
                        }
                        if (!string.IsNullOrEmpty(item.SojaText))
                        {
                            item.Soja = item.SojaText.ToLower() == "sim" ? true : false;
                            item.SojaText = "";
                        }
                        if (!string.IsNullOrEmpty(item.LeiteText))
                        {
                            item.Leite = item.LeiteText.ToLower() == "sim" ? true : false;
                            item.LeiteText = "";
                        }
                        if (!string.IsNullOrEmpty(item.FrutasDeCascaRijaText))
                        {
                            item.FrutasDeCascaRija = item.FrutasDeCascaRijaText.ToLower() == "sim" ? true : false;
                            item.FrutasDeCascaRijaText = "";
                        }
                        if (!string.IsNullOrEmpty(item.AipoText))
                        {
                            item.Aipo = item.AipoText.ToLower() == "sim" ? true : false;
                            item.AipoText = "";
                        }
                        if (!string.IsNullOrEmpty(item.MostardaText))
                        {
                            item.Mostarda = item.MostardaText.ToLower() == "sim" ? true : false;
                            item.MostardaText = "";
                        }
                        if (!string.IsNullOrEmpty(item.SementesDeSesamoText))
                        {
                            item.SementesDeSesamo = item.SementesDeSesamoText.ToLower() == "sim" ? true : false;
                            item.SementesDeSesamoText = "";
                        }
                        if (!string.IsNullOrEmpty(item.DioxidoDeEnxofreESulfitosText))
                        {
                            item.DioxidoDeEnxofreESulfitos = item.DioxidoDeEnxofreESulfitosText.ToLower() == "sim" ? true : false;
                            item.DioxidoDeEnxofreESulfitosText = "";
                        }
                        if (!string.IsNullOrEmpty(item.TremocoText))
                        {
                            item.Tremoco = item.TremocoText.ToLower() == "sim" ? true : false;
                            item.TremocoText = "";
                        }
                        if (!string.IsNullOrEmpty(item.MoluscosText))
                        {
                            item.Moluscos = item.MoluscosText.ToLower() == "sim" ? true : false;
                            item.MoluscosText = "";
                        }
                        if (!string.IsNullOrEmpty(item.TipoText))
                        {
                            item.Tipo = item.TipoText.ToLower() == "alimento" ? 1 : item.TipoText.ToLower() == "prato" ? 2 : 0;
                            item.TipoText = "";
                        }
                        if (!string.IsNullOrEmpty(item.VitaminaAText))
                        {
                            item.VitaminaA = Convert.ToDecimal(item.VitaminaAText);
                            item.VitaminaAText = "";
                        }
                        if (!string.IsNullOrEmpty(item.VitaminaDText))
                        {
                            item.VitaminaD = Convert.ToDecimal(item.VitaminaDText);
                            item.VitaminaDText = "";
                        }
                        if (!string.IsNullOrEmpty(item.ColesterolText))
                        {
                            item.Colesterol = Convert.ToDecimal(item.ColesterolText);
                            item.ColesterolText = "";
                        }
                        if (!string.IsNullOrEmpty(item.SodioText))
                        {
                            item.Sodio = Convert.ToDecimal(item.SodioText);
                            item.SodioText = "";
                        }
                        if (!string.IsNullOrEmpty(item.PotacioText))
                        {
                            item.Potacio = Convert.ToDecimal(item.PotacioText);
                            item.PotacioText = "";
                        }
                        if (!string.IsNullOrEmpty(item.CalcioText))
                        {
                            item.Calcio = Convert.ToDecimal(item.CalcioText);
                            item.CalcioText = "";
                        }
                        if (!string.IsNullOrEmpty(item.FerroText))
                        {
                            item.Ferro = Convert.ToDecimal(item.FerroText);
                            item.FerroText = "";
                        }
                        if (!string.IsNullOrEmpty(item.EdivelText))
                        {
                            item.Edivel = Convert.ToDecimal(item.EdivelText);
                            item.EdivelText = "";
                        }
                        if (!string.IsNullOrEmpty(item.AlcoolText))
                        {
                            item.Alcool = Convert.ToDecimal(item.AlcoolText);
                            item.AlcoolText = "";
                        }
                    }
                }
            }
            return Json(ListToCreate);
        }

        //4
        [HttpPost]
        public JsonResult UpdateCreate_Produtos([FromBody] List<FichaProdutoViewModel> data)
        {
            List<FichaProdutoViewModel> results = DBFichaProduto.ParseToViewModel(DBFichaProduto.GetAll());

            data.RemoveAll(x => results.Any(
                u =>
                    u.No == x.No &&
                    u.Descricao == x.Descricao &&
                    u.ListaDeMateriais == x.ListaDeMateriais &&
                    u.UnidadeMedidaBase == x.UnidadeMedidaBase &&
                    u.NoPrateleira == x.NoPrateleira &&
                    u.PrecoUnitario == x.PrecoUnitario &&
                    u.CustoUnitario == x.CustoUnitario &&
                    u.Inventario == x.Inventario &&
                    u.ValorEnergetico == x.ValorEnergetico &&
                    u.ValorEnergetico100g == x.ValorEnergetico100g &&
                    u.Proteinas == x.Proteinas &&
                    u.Proteinas100g == x.Proteinas100g &&
                    u.Glicidos == x.Glicidos &&
                    u.Glicidos100g == x.Glicidos100g &&
                    u.Lipidos == x.Lipidos &&
                    u.Lipidos100g == x.Lipidos100g &&
                    u.FibraAlimentar == x.FibraAlimentar &&
                    u.FibraAlimentar100g == x.FibraAlimentar100g &&
                    u.QuantUnidadeMedida == x.QuantUnidadeMedida &&
                    u.GramasPorQuantUnidMedida == x.GramasPorQuantUnidMedida &&
                    u.TipoRefeicao == x.TipoRefeicao &&
                    u.DescricaoRefeicao == x.DescricaoRefeicao &&
                    u.Taras == x.Taras &&
                    u.AcidosGordosSaturados == x.AcidosGordosSaturados &&
                    u.Acucares == x.Acucares &&
                    u.Sal == x.Sal &&
                    u.Cereais == x.Cereais &&
                    u.Crustaceos == x.Crustaceos &&
                    u.Ovos == x.Ovos &&
                    u.Peixes == x.Peixes &&
                    u.Amendoins == x.Amendoins &&
                    u.Soja == x.Soja &&
                    u.Leite == x.Leite &&
                    u.FrutasDeCascaRija == x.FrutasDeCascaRija &&
                    u.Aipo == x.Aipo &&
                    u.Mostarda == x.Mostarda &&
                    u.SementesDeSesamo == x.SementesDeSesamo &&
                    u.DioxidoDeEnxofreESulfitos == x.DioxidoDeEnxofreESulfitos &&
                    u.Tremoco == x.Tremoco &&
                    u.Moluscos == x.Moluscos &&
                    u.Tipo == x.Tipo &&
                    u.VitaminaA == x.VitaminaA &&
                    u.VitaminaD == x.VitaminaD &&
                    u.Colesterol == x.Colesterol &&
                    u.Sodio == x.Sodio &&
                    u.Potacio == x.Potacio &&
                    u.Calcio == x.Calcio &&
                    u.Ferro == x.Ferro &&
                    u.Edivel == x.Edivel &&
                    u.Alcool == x.Alcool
            ));

            data.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.No))
                {
                    FichaProduto toCreate = DBFichaProduto.ParseToDatabase(x);
                    FichaProduto toUpdate = DBFichaProduto.ParseToDatabase(x);
                    FichaProduto toSearch = DBFichaProduto.GetById(x.No);

                    UnidadeMedidaViewModel unidadeMedida = DBUnidadeMedida.ParseToViewModel(DBUnidadeMedida.GetById(x.UnidadeMedidaBase));

                    if (toSearch == null)
                    {
                        toCreate.Nº = x.No;
                        toCreate.Descrição = x.Descricao;
                        toCreate.ListaDeMateriais = x.ListaDeMateriais.HasValue ? x.ListaDeMateriais.ToString().ToLower() == "sim" ? true : false : false;
                        if (unidadeMedida != null)
                            toCreate.UnidadeMedidaBase = x.UnidadeMedidaBase;
                        else
                            toUpdate.UnidadeMedidaBase = null;
                        toCreate.NºPrateleira = x.NoPrateleira;
                        toCreate.PreçoUnitário = x.PrecoUnitario;
                        toCreate.CustoUnitário = x.CustoUnitario;
                        toCreate.Inventário = x.Inventario;
                        toCreate.ValorEnergético = x.ValorEnergetico;
                        toCreate.ValorEnergético100g = x.ValorEnergetico100g;
                        toCreate.Proteínas = x.Proteinas;
                        toCreate.Proteínas100g = x.Proteinas100g;
                        toCreate.Glícidos = x.Glicidos;
                        toCreate.Glícidos100g = x.Glicidos100g;
                        toCreate.Lípidos = x.Lipidos;
                        toCreate.Lípidos100g = x.Lipidos100g;
                        toCreate.FibraAlimentar = x.FibraAlimentar;
                        toCreate.FibraAlimentar100g = x.FibraAlimentar100g;
                        toCreate.QuantUnidadeMedida = x.QuantUnidadeMedida;
                        toCreate.GramasPorQuantUnidMedida = x.GramasPorQuantUnidMedida;
                        toCreate.TipoRefeição = x.TipoRefeicao;
                        toCreate.DescriçãoRefeição = x.DescricaoRefeicao;
                        toCreate.Taras = x.Taras.HasValue ? x.Taras.ToString().ToLower() == "sim" ? true : false : false;
                        toCreate.ÁcidosGordosSaturados = x.AcidosGordosSaturados;
                        toCreate.Açucares = x.Acucares;
                        toCreate.Sal = x.Sal;
                        toCreate.Cereais = x.Cereais.HasValue ? x.Cereais.ToString().ToLower() == "sim" ? true : false : false;
                        toCreate.Crustáceos = x.Crustaceos.HasValue ? x.Crustaceos.ToString().ToLower() == "sim" ? true : false : false;
                        toCreate.Ovos = x.Ovos.HasValue ? x.Ovos.ToString().ToLower() == "sim" ? true : false : false;
                        toCreate.Peixes = x.Peixes.HasValue ? x.Peixes.ToString().ToLower() == "sim" ? true : false : false;
                        toCreate.Amendoins = x.Amendoins.HasValue ? x.Amendoins.ToString().ToLower() == "sim" ? true : false : false;
                        toCreate.Soja = x.Soja.HasValue ? x.Soja.ToString().ToLower() == "sim" ? true : false : false;
                        toCreate.Leite = x.Leite.HasValue ? x.Leite.ToString().ToLower() == "sim" ? true : false : false;
                        toCreate.FrutasDeCascaRija = x.FrutasDeCascaRija.HasValue ? x.FrutasDeCascaRija.ToString().ToLower() == "sim" ? true : false : false;
                        toCreate.Aipo = x.Aipo.HasValue ? x.Aipo.ToString().ToLower() == "sim" ? true : false : false;
                        toCreate.Mostarda = x.Mostarda.HasValue ? x.Mostarda.ToString().ToLower() == "sim" ? true : false : false;
                        toCreate.SementesDeSésamo = x.SementesDeSesamo.HasValue ? x.SementesDeSesamo.ToString().ToLower() == "sim" ? true : false : false;
                        toCreate.DióxidoDeEnxofreESulfitos = x.DioxidoDeEnxofreESulfitos.HasValue ? x.DioxidoDeEnxofreESulfitos.ToString().ToLower() == "sim" ? true : false : false;
                        toCreate.Tremoço = x.Tremoco.HasValue ? x.Tremoco.ToString().ToLower() == "sim" ? true : false : false;
                        toCreate.Moluscos = x.Moluscos.HasValue ? x.Moluscos.ToString().ToLower() == "sim" ? true : false : false;
                        toCreate.Tipo = x.Tipo;
                        toCreate.VitaminaA = x.VitaminaA;
                        toCreate.VitaminaD = x.VitaminaD;
                        toCreate.Colesterol = x.Colesterol;
                        toCreate.Sodio = x.Sodio;
                        toCreate.Potacio = x.Potacio;
                        toCreate.Calcio = x.Calcio;
                        toCreate.Ferro = x.Ferro;
                        toCreate.Edivel = x.Edivel;
                        toCreate.Alcool = x.Alcool;
                        toCreate.DataHoraCriação = DateTime.Now;
                        toCreate.UtilizadorCriação = User.Identity.Name;

                        DBFichaProduto.Create(toCreate);
                    }
                    else
                    {
                        toUpdate.Nº = x.No;
                        toUpdate.Descrição = x.Descricao;
                        toUpdate.ListaDeMateriais = x.ListaDeMateriais.HasValue ? x.ListaDeMateriais.ToString().ToLower() == "sim" ? true : false : false;
                        if (unidadeMedida != null)
                            toUpdate.UnidadeMedidaBase = x.UnidadeMedidaBase;
                        else
                            toUpdate.UnidadeMedidaBase = null;
                        toUpdate.NºPrateleira = x.NoPrateleira;
                        toUpdate.PreçoUnitário = x.PrecoUnitario;
                        toUpdate.CustoUnitário = x.CustoUnitario;
                        toUpdate.Inventário = x.Inventario;
                        toUpdate.ValorEnergético = x.ValorEnergetico;
                        toUpdate.ValorEnergético100g = x.ValorEnergetico100g;
                        toUpdate.Proteínas = x.Proteinas;
                        toUpdate.Proteínas100g = x.Proteinas100g;
                        toUpdate.Glícidos = x.Glicidos;
                        toUpdate.Glícidos100g = x.Glicidos100g;
                        toUpdate.Lípidos = x.Lipidos;
                        toUpdate.Lípidos100g = x.Lipidos100g;
                        toUpdate.FibraAlimentar = x.FibraAlimentar;
                        toUpdate.FibraAlimentar100g = x.FibraAlimentar100g;
                        toUpdate.QuantUnidadeMedida = x.QuantUnidadeMedida;
                        toUpdate.GramasPorQuantUnidMedida = x.GramasPorQuantUnidMedida;
                        toUpdate.TipoRefeição = x.TipoRefeicao;
                        toUpdate.DescriçãoRefeição = x.DescricaoRefeicao;
                        toUpdate.Taras = x.Taras.HasValue ? x.Taras.ToString().ToLower() == "sim" ? true : false : false;
                        toUpdate.ÁcidosGordosSaturados = x.AcidosGordosSaturados;
                        toUpdate.Açucares = x.Acucares;
                        toUpdate.Sal = x.Sal;
                        toUpdate.Cereais = x.Cereais.HasValue ? x.Cereais.ToString().ToLower() == "sim" ? true : false : false;
                        toUpdate.Crustáceos = x.Crustaceos.HasValue ? x.Crustaceos.ToString().ToLower() == "sim" ? true : false : false;
                        toUpdate.Ovos = x.Ovos.HasValue ? x.Ovos.ToString().ToLower() == "sim" ? true : false : false;
                        toUpdate.Peixes = x.Peixes.HasValue ? x.Peixes.ToString().ToLower() == "sim" ? true : false : false;
                        toUpdate.Amendoins = x.Amendoins.HasValue ? x.Amendoins.ToString().ToLower() == "sim" ? true : false : false;
                        toUpdate.Soja = x.Soja.HasValue ? x.Soja.ToString().ToLower() == "sim" ? true : false : false;
                        toUpdate.Leite = x.Leite.HasValue ? x.Leite.ToString().ToLower() == "sim" ? true : false : false;
                        toUpdate.FrutasDeCascaRija = x.FrutasDeCascaRija.HasValue ? x.FrutasDeCascaRija.ToString().ToLower() == "sim" ? true : false : false;
                        toUpdate.Aipo = x.Aipo.HasValue ? x.Aipo.ToString().ToLower() == "sim" ? true : false : false;
                        toUpdate.Mostarda = x.Mostarda.HasValue ? x.Mostarda.ToString().ToLower() == "sim" ? true : false : false;
                        toUpdate.SementesDeSésamo = x.SementesDeSesamo.HasValue ? x.SementesDeSesamo.ToString().ToLower() == "sim" ? true : false : false;
                        toUpdate.DióxidoDeEnxofreESulfitos = x.DioxidoDeEnxofreESulfitos.HasValue ? x.DioxidoDeEnxofreESulfitos.ToString().ToLower() == "sim" ? true : false : false;
                        toUpdate.Tremoço = x.Tremoco.HasValue ? x.Tremoco.ToString().ToLower() == "sim" ? true : false : false;
                        toUpdate.Moluscos = x.Moluscos.HasValue ? x.Moluscos.ToString().ToLower() == "sim" ? true : false : false;
                        toUpdate.Tipo = x.Tipo;
                        toUpdate.VitaminaA = x.VitaminaA;
                        toUpdate.VitaminaD = x.VitaminaD;
                        toUpdate.Colesterol = x.Colesterol;
                        toUpdate.Sodio = x.Sodio;
                        toUpdate.Potacio = x.Potacio;
                        toUpdate.Calcio = x.Calcio;
                        toUpdate.Ferro = x.Ferro;
                        toUpdate.Edivel = x.Edivel;
                        toUpdate.Alcool = x.Alcool;
                        toUpdate.DataHoraCriação = x.DataHoraCriacao;
                        toUpdate.UtilizadorCriação = x.UtilizadorCriacao;
                        toUpdate.DataHoraModificação = DateTime.Now;
                        toUpdate.UtilizadorModificação = User.Identity.Name;

                        DBFichaProduto.Update(toUpdate);
                    }
                }
            });
            return Json(data);
        }
        #endregion




        public ActionResult ShowImage(string ProdutoNo)
        {
            try
            {
                byte[] fileBytes = null;
                string contentType = null;

                FichaProduto Produto = DBFichaProduto.GetById(ProdutoNo);
                fileBytes = Produto.Imagem;
                contentType = "image/jpeg";

                return File(fileBytes, contentType);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public JsonResult Upload_Imagem()
        {
            FichaProdutoViewModel Produto = new FichaProdutoViewModel();
            var files = Request.Form.Files;
            IFormFile file = files[0];
            byte[] imagem = null;

            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    imagem = fileBytes;
                }
            }
            return Json(imagem);
        }



















    }
}
