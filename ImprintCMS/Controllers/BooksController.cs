using System.Linq;
using System.Web.Mvc;
using ImprintCMS.Models.ViewModels;
using System;

namespace ImprintCMS.Controllers
{
	public class BooksController : ControllerBase
	{

		public ActionResult Index()
		{
			var vm = Repository.Books.Where(b => b.IsVisible && !b.HasExternalPublisher).Select(b => new ListBook(b, Url));
			return View(vm);
		}

		public ActionResult Details(int id)
		{
			var vm = Repository.GetBook(id);
			if (vm == null) return HttpNotFound();
			if (!vm.IsVisible || vm.HasExternalPublisher) return HttpNotFound();
			return View(vm);
		}

	}
}
