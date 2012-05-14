using System.Linq;
using System.Web.Mvc;
using ImprintCMS.Models.ViewModels;

namespace ImprintCMS.Controllers
{
	public class AuthorsController : SiteControllerBase
	{

		public ActionResult Index()
		{
			var vm = Repository.People.Where(p => p.IsVisible && p.HasPage).OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
			return View(vm);
		}

		public ActionResult Details(int id)
		{
			var person = Repository.GetPerson(id);
			if (person == null) return HttpNotFound();
			if (!person.IsVisible || !person.HasPage) return HttpNotFound();
			var vm = new PersonPage
			{
				Person = person,
				Books = person.Relations.Where(r => r.Book.IsVisible).Select(r => new ListBook(r.Book, Url)).OrderBy(b => b.ReleaseYear)
			};
			return View(vm);
		}

	}
}
