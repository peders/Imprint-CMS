using System;
using System.Web.Mvc;
using ImprintCMS.Models;
using ImprintCMS.Models.ViewModels;

namespace ImprintCMS.Controllers
{

	[Authorize]
	public class UploadController : ControllerBase
	{
		public ActionResult Index()
		{
			var vm = Repository.UploadedFiles;
			return View(vm);
		}

		public ActionResult New()
		{
			var vm = new FileUpload
			{
				FileCategory = FileCategories.LargeCover.ToString()
			};
			ViewBag.FileCategories = new SelectList(Enum.GetValues(typeof(FileCategories)), vm.FileCategory);
			return View(vm);
		}

		[HttpPost]
		public ActionResult New(FileUpload vm)
		{
			var file = Request.Files[0];
			if (file.ContentLength == 0)
				ModelState.AddModelError("", "No file or file is empty");
			if (Repository.GetUploadedFile(vm.FileCategory, file.FileName) != null)
				ModelState.AddModelError("", "File already exists");
			if (!ModelState.IsValid)
			{
				ViewBag.FileCategories = new SelectList(Enum.GetValues(typeof(FileCategories)), vm.FileCategory);
				return View(vm);
			}
			var fileData = new byte[file.ContentLength];
			file.InputStream.Read(fileData, 0, fileData.Length);
			var upload = new UploadedFile
			{
				FileName = file.FileName,
				ContentType = file.ContentType,
				Category = vm.FileCategory,
				Data = fileData
			};
			Repository.Add(upload);
			Repository.Save();
			return RedirectToAction("index");
		}

		[OutputCache(Duration = 60)]
		public ActionResult Display(string category, string fileName)
		{
			var vm = Repository.GetUploadedFile(category, fileName);
			if (vm == null) return HttpNotFound();
			return new FileContentResult(vm.Data.ToArray(), vm.ContentType);
		}

	}
}
