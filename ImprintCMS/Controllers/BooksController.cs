using System.Linq;
using System.Web.Mvc;

namespace ImprintCMS.Controllers
{
	public class BooksController : ControllerBase
	{

		public ActionResult Index()
		{
			var vm = Repository.Books.Where(b => b.IsVisible);
			return View(vm);
		}

		public ActionResult Details(int id)
		{
			var vm = Repository.GetBook(id);
			if (vm == null) return HttpNotFound();
			if (!vm.IsVisible) return HttpNotFound();
			return View(vm);
		}

	}
}
