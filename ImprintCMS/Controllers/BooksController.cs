using System.Linq;
using System.Web.Mvc;
using ImprintCMS.Models;
using ImprintCMS.Models.ViewModels;

namespace ImprintCMS.Controllers
{
    public class BooksController : SiteControllerBase
    {

        public ActionResult Index(int? id)
        {
            var availableGenres = Repository.Books.Where(b => b.IsVisible && !b.HasExternalPublisher).Select(b => b.Genre).Distinct().OrderBy(g => g.SequenceIdentifier);
            var currentGenre = id != null ? Repository.GetGenre((int)id) : null;
            var currentBooks = id != null ? Repository.Books.Where(b => b.GenreId == id && b.IsVisible && !b.HasExternalPublisher) : Repository.Books.Where(b => b.IsVisible && !b.HasExternalPublisher);
            var vm = new BookCatalog
            {
                Title = currentGenre != null ? currentGenre.Name : SitePhrases.LabelAllBooks,
                Genres = availableGenres,
                CurrentGenre = currentGenre,
                CurrentBooks = currentBooks.OrderByDescending(b => b.Relations.Any()).ThenBy(b => b.RightsHoldersDisplayText()).ThenBy(b => b.ReleaseYear())
            };
            return View(vm);
        }

        public ActionResult Details(int id)
        {
            var vm = Repository.GetBook(id);
            if (vm == null) return HttpNotFound();
            if (!vm.IsVisible || vm.HasExternalPublisher) return HttpNotFound();
            ViewBag.OpenGraph = vm.OpenGraph(Config.Name, Request.RequestContext);
            return View(vm);
        }

    }
}
