using System.Linq;
using System.Web.Mvc;
using ImprintCMS.Models.ViewModels;
using System;

namespace ImprintCMS.Controllers
{
	public class BooksController : ControllerBase
	{

		public ActionResult Index(int? id)
		{
			var availableGenres = Repository.Books.Where(b => b.IsVisible && !b.HasExternalPublisher).Select(b => b.Genre).Distinct().OrderBy(g => g.SequenceIdentifier);
			var genreId = id ?? availableGenres.First().Id;
			var vm = new BookCatalog
			{
				Genres = availableGenres,
				CurrentGenre = Repository.GetGenre(genreId),
				CurrentBooks = Repository.Books.Where(b => b.GenreId == genreId && b.IsVisible && !b.HasExternalPublisher).Select(b => new ListBook(b, Url))
			};
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
