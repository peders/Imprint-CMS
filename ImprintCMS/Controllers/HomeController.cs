using System.Linq;
using System.Web.Mvc;
using ImprintCMS.Models.ViewModels;
using System;

namespace ImprintCMS.Controllers
{
	public class HomeController : ControllerBase
	{

		public ActionResult Index()
		{
			var vm = new SiteFrontPage
			{
				BookLists = Repository.BookLists.Where(l => l.IsVisible).OrderBy(l => l.SequenceIdentifier),
				Articles = Repository.Articles.Where(a => a.IsVisible).OrderByDescending(a => a.Date)
			};
			return View(vm);
		}

		public ActionResult Contact(int? id)
		{
			var articles = Repository.ContactArticles.OrderBy(a => a.SequenceIdentifier);
			var vm = new ContactPage
			{
				Articles = articles,
				CurrentArticle = id == null ? articles.First() : Repository.GetContactArticle((int)id)
			};
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
			var cleanQuery = q.ToLower().Trim();
			var vm = new SearchResult
			{
				Query = q.Trim(),
				Books = Repository.Books
					.Where(b =>
						b.IsVisible
						&& !b.HasExternalPublisher)
					.Where(b =>
						b.Title.ToLower().Contains(cleanQuery)
						|| b.Relations.Select(r => r.Person).Any(p =>
							p.IsVisible
							&& p.HasPage
							&& p.FirstName.ToLower().Contains(cleanQuery) || p.LastName.ToLower().Contains(cleanQuery)))
					.OrderBy(b => b.Title),
				People = Repository.People
					.Where(p =>
						p.IsVisible
						&& p.HasPage
						&& p.FirstName.ToLower().Contains(cleanQuery) || p.LastName.ToLower().Contains(cleanQuery))
					.OrderBy(p => p.LastName)
					.ThenBy(p => p.FirstName)
			};
			return View(vm);
		}

	}
}
