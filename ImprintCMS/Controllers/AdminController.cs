using System.Web.Mvc;
using ImprintCMS.Models.ViewModels;
using ImprintCMS.Models;
using System;

namespace ImprintCMS.Controllers
{

	[Authorize]
	public class AdminController : ControllerBase
	{

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Uploads()
		{
			var vm = Repository.UploadedFiles;
			return View(vm);
		}

		public ActionResult CreateUpload()
		{
			var vm = new FileUpload
			{
				FileCategory = FileCategories.Attachment.ToString()
			};
			ViewBag.FileCategories = new SelectList(Enum.GetValues(typeof(FileCategories)), vm.FileCategory);
			return View(vm);
		}

		[HttpPost]
		public ActionResult CreateUpload(FileUpload vm)
		{
			var file = Request.Files[0];
			if (file.ContentLength == 0)
				ModelState.AddModelError("", Phrases.ValidationEmptyOrNoFile);
			if (file.ContentLength > 5242880)
				ModelState.AddModelError("", Phrases.ValidationFileTooLarge);
			if (Repository.GetUploadedFile(vm.FileCategory, file.FileName.Sanitise()) != null)
				ModelState.AddModelError("", Phrases.ValidationFileExists);
			if (!ModelState.IsValid)
			{
				ViewBag.FileCategories = new SelectList(Enum.GetValues(typeof(FileCategories)), vm.FileCategory);
				return View(vm);
			}
			var fileData = new byte[file.ContentLength];
			file.InputStream.Read(fileData, 0, fileData.Length);
			var upload = new UploadedFile
			{
				FileName = file.FileName.Sanitise(),
				ContentType = file.ContentType,
				ContentLength = file.ContentLength,
				Category = vm.FileCategory,
				Data = fileData
			};
			Repository.Add(upload);
			Repository.Save();
			return RedirectToAction("uploads");
		}

		public ActionResult DeleteUpload(int id)
		{
			var file = Repository.GetUploadedFile(id);
			if (file == null) return HttpNotFound();
			Repository.Delete(file);
			Repository.Save();
			return RedirectToAction("uploads");
		}

	}
}
