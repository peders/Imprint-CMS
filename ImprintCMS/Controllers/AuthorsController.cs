using System.Linq;
using System.Web.Mvc;

namespace ImprintCMS.Controllers
{
	public class AuthorsController : ControllerBase
	{

		public ActionResult Index()
		{
			var vm = Repository.People.Where(p => p.IsVisible && p.HasPage).OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
			return View(vm);
		}

	}
}
