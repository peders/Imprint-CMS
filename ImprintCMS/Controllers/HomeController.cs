using System.Linq;
using System.Web.Mvc;
using ImprintCMS.Models;
using ImprintCMS.Models.ViewModels;
using System;

namespace ImprintCMS.Controllers
{
	public class HomeController : SiteControllerBase
	{

		public ActionResult Index()
		{
			var vm = new SiteFrontPage
			{
				BookLists = Repository.BookLists.Where(l => l.IsVisible).OrderBy(l => l.SequenceIdentifier),
				Articles = Repository.Articles.Where(a => a.IsVisible &&a.IsOnFrontPage).OrderByDescending(a => a.Date)
			};
			return View(vm);
		}

        public ActionResult Contact(int id)
        {
            var vm = Repository.GetArticleGroup(id);
            if (vm == null) return HttpNotFound();
            ViewBag.CurrentMenuTabId = id;
            return View(vm);
        }

        public ActionResult Article(int id) {
            var vm = Repository.GetArticle(id);
            if (vm == null) return HttpNotFound();
            if (!vm.IsVisible) return HttpNotFound();
            ViewBag.OpenGraph = vm.OpenGraph(Config.Name, Request.RequestContext);
            return View(vm);
        }

		public ActionResult Search(string q)
		{
			if (String.IsNullOrWhiteSpace(q))
			{
				return View(new SearchResult
				{
					Query = string.Empty,
					Books = Repository.Books.Where(b => false),
					People = Repository.People.Where(p => false)
				});
			}
			var cleanQuery = q.ToLower().Trim().Replace(" ", string.Empty);
			var vm = new SearchResult
			{
				Query = q.Trim(),
				Books = Repository.Books
					.Where(b =>
						b.IsVisible
						&& !b.HasExternalPublisher)
					.Where(b =>
						b.Title.ToLower().Replace(" ", string.Empty).Contains(cleanQuery)
						|| b.Relations.Select(r => r.Person).Any(p =>
							p.IsVisible
							&& p.HasPage
							&& (p.FirstName + p.LastName).ToLower().Replace(" ", string.Empty).Contains(cleanQuery)))
					.OrderBy(b => b.Title),
				People = Repository.People
					.Where(p => p.IsVisible && p.HasPage && (p.FirstName + p.LastName).ToLower().Replace(" ", string.Empty).Contains(cleanQuery))
					.OrderBy(p => p.LastName)
					.ThenBy(p => p.FirstName)
			};
			return View(vm);
		}

	}
}
