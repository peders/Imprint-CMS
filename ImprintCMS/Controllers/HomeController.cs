using System.Linq;
using System.Web.Mvc;
using ImprintCMS.Models.ViewModels;

namespace ImprintCMS.Controllers
{
	public class HomeController : ControllerBase
	{

		public ActionResult Index()
		{
			var vm = new SiteFrontPage
			{
				BookLists = Repository.BookLists.Where(l => l.IsVisible).OrderBy(l => l.SequenceIdentifier),
				Articles = Repository.NewsArticles.Where(a => a.IsVisible).OrderByDescending(a => a.Date)
			};
			return View(vm);
		}

	}
}
