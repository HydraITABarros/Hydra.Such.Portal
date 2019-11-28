using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Portal.Filters;
using Hydra.Such.Data.Evolution.DatabaseReference;
using Hydra.Such.Portal.ViewModels;
using JavaScriptEngineSwitcher.Core.Extensions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Hydra.Such.Portal.Controllers
{
	public partial class MaintenanceOrdersController : Controller
	{
		
		[Route("files/{orderId}/{planId}/{file}"), HttpGet]
		[ResponseCache(Duration = 2592000)]
		public ActionResult GetImage(string orderId,int planId, string file, Boolean thumb)
		{
			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

			if (loggedUser == null) { return NotFound(); }

			var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name && u.Activo == true);

			if (evolutionLoggedUser == null) { return NotFound(); }

			var nivelAcesso = evolutionLoggedUser.NivelAcesso;

			var document = evolutionWEBContext.FichaManutencaoRelatorioDocuments
				.FirstOrDefault(d => 
					planId == d.IdRelatorio && d.FileName == file
				);

			try
			{
				var path = document.Path;
				if (thumb)
				{
					path = path.Replace(".jpg", ".thumb.jpg");
				}
				var imageFileStream = System.IO.File.OpenRead(path);
				return File(imageFileStream, document.FileType);
			}
			catch (Exception e)
			{
				return NotFound();
			}
		}
		
		[Route("files/{orderId}"), HttpGet]
		public ActionResult GetAllByPlanIds(string orderId,string planIds, string documentType)
		{
			var _planIds = new List<int>();
			foreach (var s in planIds.Split(","))
			{
				int val = 0;
				int.TryParse(s, out val);
				_planIds.Add(val);
			}
			
			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

			if (loggedUser == null) { return NotFound(); }

			var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name && u.Activo == true);

			if (evolutionLoggedUser == null) { return NotFound(); }

			var nivelAcesso = evolutionLoggedUser.NivelAcesso;

			var reports = evolutionWEBContext.FichaManutencaoRelatorio
				.Where(r => _planIds.Contains(r.Id) )
				.OrderByDescending(o => o.Id).ToList();
			if (reports == null) { return NotFound(); }

			var files = evolutionWEBContext.FichaManutencaoRelatorioDocuments
				.Where(d => 
					_planIds.Contains(d.IdRelatorio) && d.DocumentType == documentType
				).ToList().Select(s =>
				new {
					s.Url,
					CreatedAt = s.CreatedAt.Date,
					s.IdRelatorio,
					Name = s.FileOriginalName,
					Size = s.FileSize
				}).ToList();

			return Json(files);
		}
		
		[Route("files/{orderId}/{planId}"), HttpPost]
		public async Task<IActionResult> OnPostUploadAsync(IFormFile file, string orderId, int planId, string documentType)
		{
			if (file == null) { return NotFound(); }
			if (orderId == null) { return NotFound(); }

			var loggedUser = suchDBContext.ConfigUtilizadores.FirstOrDefault(u => u.IdUtilizador == User.Identity.Name);

			if (loggedUser == null) { return NotFound(); }

			var evolutionLoggedUser = evolutionWEBContext.Utilizador.FirstOrDefault(u => u.Email == User.Identity.Name && u.Activo == true);

			if (evolutionLoggedUser == null) { return NotFound(); }

			var order = evolutionWEBContext.MaintenanceOrder.Where(m => m.No == orderId).FirstOrDefault();
			if (order == null) { return NotFound(); }

			// get equipment/plan
			var planReport = evolutionWEBContext.FichaManutencaoRelatorio
				.Where(r => r.Om == order.No && r.Id == planId )
				.OrderByDescending(o => o.Id).FirstOrDefault();
			if (planReport == null) { return NotFound(); }

			var equipamento = evolutionWEBContext.Equipamento.FirstOrDefault(e => e.IdEquipamento == planReport.IdEquipamento);
			
			var size = file.Length;

			var filePath = "";
			
			var fileExtension = file.FileName.Split(".")[1];
			
			Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			
			switch (fileExtension)
			{
				case "jpg":
				case "jpeg":
				case "pdf":
					break;
				default:
					return BadRequest(new { message = "." + filePath + " "});
			}

			var folder = $"{orderId}/";
			if (equipamento != null && equipamento.NumSerie != null && equipamento.NumSerie != "")
			{
				folder = $"{folder}{equipamento.NumSerie}/";
			}
				
			var path = Path.Combine(_host.ContentRootPath, "Uploads/OrdensDeManutencao/", $"{folder}");
			System.IO.Directory.CreateDirectory(path);

			var fileName= unixTimestamp + "_" + file.FileName;
			
			path = System.IO.Path.Combine(path, fileName);

			if (!System.IO.File.Exists(path))
			{
				using (var stream = System.IO.File.Create(path))
				{
					await file.CopyToAsync(stream);
				}
			}
			else
			{				
				return BadRequest(new { message = "File " + fileName+ " already exists."});
			}

			try
			{
				Stream _stream = file.OpenReadStream();
				Image newThumb = GetReducedImage(120,_stream);
				newThumb.Save(Path.ChangeExtension(path, "thumb.jpg"));
			}
			catch (Exception e)
			{
			}
			
			var newImage = new FichaManutencaoRelatorioDocuments()
			{
				Path = path,
				FileName = fileName,
				FileType = file.ContentType,
				FileSize = Convert.ToInt32(size),
				FileOriginalName = file.FileName,
				DocumentType = documentType == "document" ? documentType : "image",
				IdRelatorio = planId,
				Url = "/ordens-de-manutencao/files/"+order.No + "/" + planId + "/" + fileName,
				CreatedAt = DateTime.Now
			};

			evolutionWEBContext.FichaManutencaoRelatorioDocuments.Add(newImage);
			try
			{
				evolutionWEBContext.SaveChanges();
			}
			catch (Exception e)
			{
			}
			
			return Ok(new { count = 1, size, filePath, file.FileName });
		}
		
		public Image GetReducedImage(int width, Stream resourceImage)
        {
            try
            {
                Image image = Image.FromStream(resourceImage);
                Image thumb = image.GetThumbnailImage(width, ((image.Height * width)/ image.Width), () => false, IntPtr.Zero);
                return thumb;
            }
            catch (Exception e)
            {
                return null;
            }
        }
	}
}

