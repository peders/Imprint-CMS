using System;
using System.Linq;
using System.Web.Mvc;
using ImprintCMS.Models;
using ImprintCMS.Models.ViewModels;

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

		public ActionResult Bindings()
		{
			var vm = Repository.Bindings.OrderBy(b => b.Name);
			return View(vm);
		}

		public ActionResult CreateBinding()
		{
			var vm = new Binding();
			return View(vm);
		}

		[HttpPost]
		public ActionResult CreateBinding(Binding vm)
		{
			if (!ModelState.IsValid) return View(vm);
			Repository.Add(vm);
			Repository.Save();
			return RedirectToAction("bindings");
		}

		public ActionResult EditBinding(int id)
		{
			var vm = Repository.GetBinding(id);
			return View(vm);
		}

		[HttpPost]
		public ActionResult EditBinding(Binding vm)
		{
			if (!ModelState.IsValid) return View(vm);
			UpdateModel(Repository.GetBinding(vm.Id));
			Repository.Save();
			return RedirectToAction("bindings");
		}

		public ActionResult DeleteBinding(int id)
		{
			var binding = Repository.GetBinding(id);
			if (binding == null) return HttpNotFound();
			Repository.Delete(binding);
			Repository.Save();
			return RedirectToAction("bindings");
		}

		public ActionResult Genres()
		{
			var vm = Repository.Genres.OrderBy(g => g.Name);
			return View(vm);
		}

		public ActionResult CreateGenre()
		{
			var vm = new Genre();
			return View(vm);
		}

		[HttpPost]
		public ActionResult CreateGenre(Genre vm)
		{
			if (!ModelState.IsValid) return View(vm);
			Repository.Add(vm);
			Repository.Save();
			return RedirectToAction("genres");
		}

		public ActionResult EditGenre(int id)
		{
			var vm = Repository.GetGenre(id);
			return View(vm);
		}

		[HttpPost]
		public ActionResult EditGenre(Genre vm)
		{
			if (!ModelState.IsValid) return View(vm);
			UpdateModel(Repository.GetGenre(vm.Id));
			Repository.Save();
			return RedirectToAction("genres");
		}

		public ActionResult DeleteGenre(int id)
		{
			var genre = Repository.GetGenre(id);
			if (genre == null) return HttpNotFound();
			Repository.Delete(genre);
			Repository.Save();
			return RedirectToAction("genres");
		}

		public ActionResult Roles()
		{
			var vm = Repository.Roles.OrderBy(r => r.Name);
			return View(vm);
		}

		public ActionResult CreateRole()
		{
			var vm = new Role();
			return View(vm);
		}

		[HttpPost]
		public ActionResult CreateRole(Role vm)
		{
			if (!ModelState.IsValid) return View(vm);
			Repository.Add(vm);
			Repository.Save();
			return RedirectToAction("roles");
		}

		public ActionResult EditRole(int id)
		{
			var vm = Repository.GetRole(id);
			return View(vm);
		}

		[HttpPost]
		public ActionResult EditRole(Role vm)
		{
			if (!ModelState.IsValid) return View(vm);
			UpdateModel(Repository.GetRole(vm.Id));
			Repository.Save();
			return RedirectToAction("roles");
		}

		public ActionResult DeleteRole(int id)
		{
			var role = Repository.GetRole(id);
			if (role == null) return HttpNotFound();
			Repository.Delete(role);
			Repository.Save();
			return RedirectToAction("roles");
		}

	}
}
