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

	}
}
