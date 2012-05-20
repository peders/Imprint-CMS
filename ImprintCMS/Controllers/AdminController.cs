using System;
using System.Linq;
using System.Web.Mvc;
using ImprintCMS.Models;
using ImprintCMS.Models.ViewModels;

namespace ImprintCMS.Controllers
{

	[Authorize]
	public class AdminController : ControllerBase
	{

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Uploads()
		{
			var vm = Enum.GetNames(typeof(FileCategories)).Select(c => new UploadCategory
			{
				Name = c,
				FileCount = Repository.ListFiles.Count(u => u.Category == c)
			});
			return View(vm);
		}

		public ActionResult UploadCategory(string id)
		{
			var vm = new UploadCategoryFiles
			{
				Name = id,
				Files = Repository.ListFiles.Where(u => u.Category == id)
			};
			return View(vm);
		}

		public ActionResult CreateUpload(string id)
		{
			var vm = new FileUpload
			{
				FileCategory = id ?? string.Empty
			};
			return View(vm);
		}

		[HttpPost]
		public ActionResult CreateUpload(FileUpload vm)
		{
			var file = Request.Files[0];
			if (file.ContentLength == 0)
				ModelState.AddModelError("", Phrases.ValidationEmptyOrNoFile);
			if (file.ContentLength > 10485760)
				ModelState.AddModelError("", Phrases.ValidationFileTooLarge);
			if (Repository.GetUploadedFile(vm.FileCategory, file.FileName.Sanitise()) != null)
				ModelState.AddModelError("", Phrases.ValidationFileExists);
			if (!ModelState.IsValid)
			{
				return View(vm);
			}
			var fileData = new byte[file.ContentLength];
			file.InputStream.Read(fileData, 0, fileData.Length);
			var upload = new UploadedFile
			{
				FileName = file.FileName.Sanitise(),
				ContentType = file.ContentType,
				ContentLength = file.ContentLength,
				Category = vm.FileCategory,
				Data = fileData
			};
			Repository.Add(upload);
			Repository.Save();
			return RedirectToAction("uploadcategory", new { id = vm.FileCategory });
		}

		public ActionResult DeleteUpload(int id)
		{
			var file = Repository.GetUploadedFile(id);
			if (file == null) return HttpNotFound();
			if (file.Books.Any() || file.Editions.Any() || file.Editions1.Any() || file.Persons.Any() || file.Persons1.Any()) return View("NotEmpty");
			Repository.Delete(file);
			Repository.Save();
			return RedirectToAction("uploads");
		}

		public ActionResult Bindings()
		{
			var vm = Repository.Bindings.OrderBy(b => b.Name);
			return View(vm);
		}

		public ActionResult CreateBinding()
		{
			var vm = new Binding();
			return View(vm);
		}

		[HttpPost]
		public ActionResult CreateBinding(Binding vm)
		{
			if (!ModelState.IsValid) return View(vm);
			Repository.Add(vm);
			Repository.Save();
			return RedirectToAction("bindings");
		}

		public ActionResult EditBinding(int id)
		{
			var vm = Repository.GetBinding(id);
			return View(vm);
		}

		[HttpPost]
		public ActionResult EditBinding(Binding vm)
		{
			if (!ModelState.IsValid) return View(vm);
			UpdateModel(Repository.GetBinding(vm.Id));
			Repository.Save();
			return RedirectToAction("bindings");
		}

		public ActionResult DeleteBinding(int id)
		{
			var binding = Repository.GetBinding(id);
			if (binding == null) return HttpNotFound();
			if (binding.Editions.Any()) return View("NotEmpty");
			Repository.Delete(binding);
			Repository.Save();
			return RedirectToAction("bindings");
		}

		public ActionResult Genres()
		{
			var vm = Repository.Genres.OrderBy(g => g.SequenceIdentifier);
			return View(vm);
		}

		public ActionResult CreateGenre()
		{
			var vm = new Genre
			{
				SequenceIdentifier = int.MaxValue
			};
			return View(vm);
		}

		[HttpPost]
		public ActionResult CreateGenre(Genre vm)
		{
			if (!ModelState.IsValid) return View(vm);
			Repository.Add(vm);
			Repository.Save();
			return RedirectToAction("genres");
		}

		public ActionResult EditGenre(int id)
		{
			var vm = Repository.GetGenre(id);
			return View(vm);
		}

		[HttpPost]
		public ActionResult EditGenre(Genre vm)
		{
			if (!ModelState.IsValid) return View(vm);
			UpdateModel(Repository.GetGenre(vm.Id));
			Repository.Save();
			return RedirectToAction("genres");
		}

		public ActionResult DeleteGenre(int id)
		{
			var genre = Repository.GetGenre(id);
			if (genre == null) return HttpNotFound();
			if (genre.Books.Any()) return View("NotEmpty");
			Repository.Delete(genre);
			Repository.Save();
			return RedirectToAction("genres");
		}

		public ActionResult Roles()
		{
			var vm = Repository.Roles.OrderBy(r => r.Name);
			return View(vm);
		}

		public ActionResult CreateRole()
		{
			var vm = new Role();
			return View(vm);
		}

		[HttpPost]
		public ActionResult CreateRole(Role vm)
		{
			if (!ModelState.IsValid) return View(vm);
			Repository.Add(vm);
			Repository.Save();
			return RedirectToAction("roles");
		}

		public ActionResult EditRole(int id)
		{
			var vm = Repository.GetRole(id);
			return View(vm);
		}

		[HttpPost]
		public ActionResult EditRole(Role vm)
		{
			if (!ModelState.IsValid) return View(vm);
			UpdateModel(Repository.GetRole(vm.Id));
			Repository.Save();
			return RedirectToAction("roles");
		}

		public ActionResult DeleteRole(int id)
		{
			var role = Repository.GetRole(id);
			if (role == null) return HttpNotFound();
			if (role.Relations.Any()) return View("NotEmpty");
			Repository.Delete(role);
			Repository.Save();
			return RedirectToAction("roles");
		}

		public ActionResult People()
		{
			var vm = Repository.People.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
			return View(vm);
		}

		public ActionResult CreatePerson()
		{
			var vm = new Person
			{
				IsVisible = true,
				HasPage = true
			};
			ViewBag.SmallPortraits = FileList(FileCategories.SmallPortrait, vm.SmallImageId);
			ViewBag.LargePortraits = FileList(FileCategories.LargePortrait, vm.LargeImageId);
			return View(vm);
		}

		[HttpPost]
		public ActionResult CreatePerson(Person vm)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.SmallPortraits = FileList(FileCategories.SmallPortrait, vm.SmallImageId);
				ViewBag.LargePortraits = FileList(FileCategories.LargePortrait, vm.LargeImageId);
				return View(vm);
			}
			Repository.Add(vm);
			Repository.Save();
			return RedirectToAction("people");
		}

		public ActionResult EditPerson(int id)
		{
			var vm = Repository.GetPerson(id);
			ViewBag.SmallPortraits = FileList(FileCategories.SmallPortrait, vm.SmallImageId);
			ViewBag.LargePortraits = FileList(FileCategories.LargePortrait, vm.LargeImageId);
			return View(vm);
		}

		[HttpPost]
		public ActionResult EditPerson(Person vm)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.SmallPortraits = FileList(FileCategories.SmallPortrait, vm.SmallImageId);
				ViewBag.LargePortraits = FileList(FileCategories.LargePortrait, vm.LargeImageId);
				return View(vm);
			}
			UpdateModel(Repository.GetPerson(vm.Id));
			Repository.Save();
			return RedirectToAction("people");
		}

		public ActionResult DeletePerson(int id)
		{
			var person = Repository.GetPerson(id);
			if (person == null) return HttpNotFound();
			if (person.Relations.Any() || person.Articles.Any()) return View("NotEmpty");
			Repository.Delete(person);
			Repository.Save();
			return RedirectToAction("people");
		}

		public ActionResult Books()
		{
			var vm = Repository.Books.OrderBy(b => b.Title);
			return View(vm);
		}

		public ActionResult CreateBook()
		{
			var vm = new Book
			{
				IsVisible = true
			};
			ViewBag.Excerpts = FileList(FileCategories.Excerpt, vm.ExcerptId);
			ViewBag.Genres = GenreList(vm.GenreId);
			return View(vm);
		}

		[HttpPost]
		public ActionResult CreateBook(Book vm)
		{
			ValidateExternalPublisher(vm);
			if (!ModelState.IsValid)
			{
				ViewBag.Excerpts = FileList(FileCategories.Excerpt, vm.ExcerptId);
				ViewBag.Genres = GenreList(vm.GenreId);
				return View(vm);
			}
			Repository.Add(vm);
			Repository.Save();
			return RedirectToAction("editbook", new { id = vm.Id });
		}

		public ActionResult EditBook(int id)
		{
			var vm = Repository.GetBook(id);
			ViewBag.Excerpts = FileList(FileCategories.Excerpt, vm.ExcerptId);
			ViewBag.Genres = GenreList(vm.GenreId);
			return View(vm);
		}

		[HttpPost]
		public ActionResult EditBook(Book vm)
		{
			ValidateExternalPublisher(vm);
			if (!ModelState.IsValid)
			{
				ViewBag.Excerpts = FileList(FileCategories.Excerpt, vm.ExcerptId);
				ViewBag.Genres = GenreList(vm.GenreId);
				return View(vm);
			}
			UpdateModel(Repository.GetBook(vm.Id));
			Repository.Save();
			return RedirectToAction("books");
		}

		public ActionResult DeleteBook(int id)
		{
			var book = Repository.GetBook(id);
			if (book == null) return HttpNotFound();
			if (book.Relations.Any() || book.Editions.Any()) return View("NotEmpty");
			Repository.Delete(book);
			Repository.Save();
			return RedirectToAction("books");
		}

		public ActionResult CreateEdition(int id)
		{
			var book = Repository.GetBook(id);
			if (book == null) return HttpNotFound();
			var vm = new Edition
			{
				Number = 1,
				ReleaseDate = DateTime.Today,
				IsForSale = true,
				BookId = id,
				Book = book
			};
			ViewBag.SmallCovers = FileList(FileCategories.SmallCover, vm.SmallCoverId);
			ViewBag.LargeCovers = FileList(FileCategories.LargeCover, vm.LargeCoverId);
			ViewBag.Bindings = BindingList(vm.BindingId);
			return View(vm);
		}

		[HttpPost]
		public ActionResult CreateEdition(Edition vm)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.SmallCovers = FileList(FileCategories.SmallCover, vm.SmallCoverId);
				ViewBag.LargeCovers = FileList(FileCategories.LargeCover, vm.LargeCoverId);
				ViewBag.Bindings = BindingList(vm.BindingId);
				vm.Book = Repository.GetBook(vm.BookId);
				return View(vm);
			}
			Repository.Add(vm);
			Repository.Save();
			return RedirectToAction("editbook", new { id = vm.BookId });
		}

		public ActionResult EditEdition(int id)
		{
			var vm = Repository.GetEdition(id);
			if (vm == null) return HttpNotFound();
			ViewBag.SmallCovers = FileList(FileCategories.SmallCover, vm.SmallCoverId);
			ViewBag.LargeCovers = FileList(FileCategories.LargeCover, vm.LargeCoverId);
			ViewBag.Bindings = BindingList(vm.BindingId);
			vm.Book = Repository.GetBook(vm.BookId);
			return View(vm);
		}

		[HttpPost]
		public ActionResult EditEdition(Edition vm)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.SmallCovers = FileList(FileCategories.SmallCover, vm.SmallCoverId);
				ViewBag.LargeCovers = FileList(FileCategories.LargeCover, vm.LargeCoverId);
				ViewBag.Bindings = BindingList(vm.BindingId);
				vm.Book = Repository.GetBook(vm.BookId);
				return View(vm);
			}
			UpdateModel(Repository.GetEdition(vm.Id));
			Repository.Save();
			return RedirectToAction("editbook", new { id = vm.BookId });
		}

		public ActionResult DeleteEdition(int id)
		{
			var edition = Repository.GetEdition(id);
			if (edition == null) return HttpNotFound();
			if (edition.BookListMemberships.Any() || edition.OrderLines.Any()) return View("NotEmpty");
			Repository.Delete(edition);
			Repository.Save();
			return RedirectToAction("editbook", new { id = edition.BookId });
		}

		public ActionResult CreateRelation(int id)
		{
			var book = Repository.GetBook(id);
			if (book == null) return HttpNotFound();
			var vm = new Relation
			{
				BookId = id,
				Book = book,
				SequenceIdentifier = int.MaxValue
			};
			ViewBag.People = PeopleList(vm.PersonId);
			ViewBag.Roles = RoleList(vm.RoleId);
			return View(vm);
		}

		[HttpPost]
		public ActionResult CreateRelation(Relation vm)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.People = PeopleList(vm.PersonId);
				ViewBag.Roles = RoleList(vm.RoleId);
				vm.Book = Repository.GetBook(vm.BookId);
				return View(vm);
			}
			Repository.Add(vm);
			Repository.Save();
			return RedirectToAction("editbook", new { id = vm.BookId });
		}

		public ActionResult EditRelation(int id)
		{
			var vm = Repository.GetRelation(id);
			if (vm == null) return HttpNotFound();
			ViewBag.People = PeopleList(vm.PersonId);
			ViewBag.Roles = RoleList(vm.RoleId);
			vm.Book = Repository.GetBook(vm.BookId);
			return View(vm);
		}

		[HttpPost]
		public ActionResult EditRelation(Relation vm)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.People = PeopleList(vm.PersonId);
				ViewBag.Roles = RoleList(vm.RoleId);
				vm.Book = Repository.GetBook(vm.BookId);
				return View(vm);
			}
			UpdateModel(Repository.GetRelation(vm.Id));
			Repository.Save();
			return RedirectToAction("editbook", new { id = vm.BookId });
		}

		public ActionResult DeleteRelation(int id)
		{
			var vm = Repository.GetRelation(id);
			if (vm == null) return HttpNotFound();
			Repository.Delete(vm);
			Repository.Save();
			return RedirectToAction("editbook", new { id = vm.BookId });
		}

		public ActionResult BookLists()
		{
			var vm = Repository.BookLists.OrderBy(l => l.SequenceIdentifier);
			return View(vm);
		}

		public ActionResult CreateBookList()
		{
			var vm = new BookList
			{
				SequenceIdentifier = int.MaxValue
			};
			return View(vm);
		}

		[HttpPost]
		public ActionResult CreateBookList(BookList vm)
		{
			if (!ModelState.IsValid) return View(vm);
			Repository.Add(vm);
			Repository.Save();
			return RedirectToAction("booklists");
		}

		public ActionResult EditBookList(int id)
		{
			var vm = Repository.GetBookList(id);
			return View(vm);
		}

		[HttpPost]
		public ActionResult EditBookList(BookList vm)
		{
			if (!ModelState.IsValid) return View(vm);
			UpdateModel(Repository.GetBookList(vm.Id));
			Repository.Save();
			return RedirectToAction("booklists");
		}

		public ActionResult DeleteBookList(int id)
		{
			var list = Repository.GetBookList(id);
			if (list == null) return HttpNotFound();
			Repository.Delete(list);
			Repository.Save();
			return RedirectToAction("booklists");
		}

		public ActionResult CreateBookListMembership(int id)
		{
			var list = Repository.GetBookList(id);
			if (list == null) return HttpNotFound();
			var vm = new BookListMembership
			{
				BookListId = id,
				BookList = list,
				SequenceIdentifier = int.MaxValue
			};
			ViewBag.Editions = LinkableEditionsList(vm.EditionId);
			return View(vm);
		}

		[HttpPost]
		public ActionResult CreateBookListMembership(BookListMembership vm)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Editions = LinkableEditionsList(vm.EditionId);
				vm.BookList = Repository.GetBookList(vm.BookListId);
				return View(vm);
			}
			Repository.Add(vm);
			Repository.Save();
			return RedirectToAction("editbooklist", new { id = vm.BookListId });
		}

		public ActionResult DeleteBookListMembership(int id)
		{
			var vm = Repository.GetBookListMembership(id);
			if (vm == null) return HttpNotFound();
			Repository.Delete(vm);
			Repository.Save();
			return RedirectToAction("editbooklist", new { id = vm.BookListId });
		}

		public ActionResult Articles()
		{
			var vm = Repository.Articles.OrderByDescending(a => a.Date);
			return View(vm);
		}

		public ActionResult CreateArticle()
		{
			var vm = new Article
			{
				Date = DateTime.Today,
				IsVisible = true
			};
			ViewBag.People = LinkablePeopleList(vm.PersonId);
			return View(vm);
		}

		[HttpPost]
		public ActionResult CreateArticle(Article vm)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.People = LinkablePeopleList(vm.PersonId);
				return View(vm);
			}
			Repository.Add(vm);
			Repository.Save();
			return RedirectToAction("articles");
		}

		public ActionResult EditArticle(int id)
		{
			var vm = Repository.GetArticle(id);
			if (vm == null) return HttpNotFound();
			ViewBag.People = LinkablePeopleList(vm.PersonId);
			return View(vm);
		}

		[HttpPost]
		public ActionResult EditArticle(Article vm)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.People = LinkablePeopleList(vm.PersonId);
				return View(vm);
			}
			UpdateModel(Repository.GetArticle(vm.Id));
			Repository.Save();
			return RedirectToAction("articles");
		}

		public ActionResult DeleteArticle(int id)
		{
			var article = Repository.GetArticle(id);
			if (article == null) return HttpNotFound();
			Repository.Delete(article);
			Repository.Save();
			return RedirectToAction("articles");
		}

		public ActionResult ContactArticles()
		{
			var vm = Repository.ContactArticles.OrderBy(a => a.SequenceIdentifier);
			return View(vm);
		}

		public ActionResult CreateContactArticle()
		{
			var vm = new ContactArticle
			{
				SequenceIdentifier = int.MaxValue
			};
			return View(vm);
		}

		[HttpPost]
		public ActionResult CreateContactArticle(ContactArticle vm)
		{
			if (!ModelState.IsValid) return View(vm);
			Repository.Add(vm);
			Repository.Save();
			return RedirectToAction("contactarticles");
		}

		public ActionResult EditContactArticle(int id)
		{
			var vm = Repository.GetContactArticle(id);
			if (vm == null) return HttpNotFound();
			return View(vm);
		}

		[HttpPost]
		public ActionResult EditContactArticle(ContactArticle vm)
		{
			if (!ModelState.IsValid) return View(vm);
			UpdateModel(Repository.GetContactArticle(vm.Id));
			Repository.Save();
			return RedirectToAction("contactarticles");
		}

		public ActionResult DeleteContactArticle(int id)
		{
			var article = Repository.GetContactArticle(id);
			if (article == null) return HttpNotFound();
			Repository.Delete(article);
			Repository.Save();
			return RedirectToAction("contactarticles");
		}

		[HttpPost]
		public ActionResult StoreContactArticleOrder()
		{
			var data = Request.Form["sortitem[]"] as string;
			var ids = data.Split(',').Select(int.Parse);
			var sequenceIdentifier = 0;
			foreach (var id in ids)
			{
				var list = Repository.GetContactArticle(id);
				list.SequenceIdentifier = sequenceIdentifier;
				sequenceIdentifier++;
			}
			Repository.Save();
			return new HttpStatusCodeResult(200);
		}

		[HttpPost]
		public ActionResult StoreBookListOrder()
		{
			var data = Request.Form["sortitem[]"] as string;
			var ids = data.Split(',').Select(int.Parse);
			var sequenceIdentifier = 0;
			foreach (var id in ids)
			{
				var list = Repository.GetBookList(id);
				list.SequenceIdentifier = sequenceIdentifier;
				sequenceIdentifier++;
			}
			Repository.Save();
			return new HttpStatusCodeResult(200);
		}

		[HttpPost]
		public ActionResult StoreBookListMembershipOrder()
		{
			var data = Request.Form["sortitem[]"] as string;
			var ids = data.Split(',').Select(int.Parse);
			var sequenceIdentifier = 0;
			foreach (var id in ids)
			{
				var member = Repository.GetBookListMembership(id);
				member.SequenceIdentifier = sequenceIdentifier;
				sequenceIdentifier++;
			}
			Repository.Save();
			return new HttpStatusCodeResult(200);
		}

		[HttpPost]
		public ActionResult StoreRelationOrder()
		{
			var data = Request.Form["sortitem[]"] as string;
			var ids = data.Split(',').Select(int.Parse);
			var sequenceIdentifier = 0;
			foreach (var id in ids)
			{
				var relation = Repository.GetRelation(id);
				relation.SequenceIdentifier = sequenceIdentifier;
				sequenceIdentifier++;
			}
			Repository.Save();
			return new HttpStatusCodeResult(200);
		}

		[HttpPost]
		public ActionResult StoreGenreOrder()
		{
			var data = Request.Form["sortitem[]"] as string;
			var ids = data.Split(',').Select(int.Parse);
			var sequenceIdentifier = 0;
			foreach (var id in ids)
			{
				var genre = Repository.GetGenre(id);
				genre.SequenceIdentifier = sequenceIdentifier;
				sequenceIdentifier++;
			}
			Repository.Save();
			return new HttpStatusCodeResult(200);
		}

		private void ValidateExternalPublisher(Book vm)
		{
			if (!String.IsNullOrWhiteSpace(vm.ExternalPublisher) && vm.ExternalReleaseYear == null)
			{
				ModelState.AddModelError("ExternalReleaseYear", Phrases.ValidationExternalReleaseYear);
			}
		}

		private SelectList FileList(FileCategories category, int? selectedId)
		{
			return new SelectList(Repository.ListFiles.Where(f => f.Category == category.ToString()).OrderBy(f => f.FileName), "Id", "FileName", selectedId ?? default(int));
		}
		private SelectList GenreList(int? selectedId)
		{
			return new SelectList(Repository.Genres.OrderBy(g => g.Name), "Id", "Name", selectedId ?? default(int));
		}
		private SelectList BindingList(int? selectedId)
		{
			return new SelectList(Repository.Bindings.OrderBy(b => b.Name), "Id", "Name", selectedId ?? default(int));
		}
		private SelectList LinkableEditionsList(int? selectedId)
		{
			return new SelectList(Repository.Editions.Where(e => !e.Book.HasExternalPublisher).OrderBy(e => e.Name), "Id", "Name", selectedId ?? default(int));
		}
		private SelectList RoleList(int? selectedId)
		{
			return new SelectList(Repository.Roles.OrderBy(r => r.Name), "Id", "Name", selectedId ?? default(int));
		}
		private SelectList PeopleList(int? selectedId)
		{
			return new SelectList(Repository.People.OrderBy(p => p.LastName).ThenBy(p => p.FirstName), "Id", "ReverseName", selectedId ?? default(int));
		}
		private SelectList LinkablePeopleList(int? selectedId)
		{
			return new SelectList(Repository.People.Where(p => p.IsVisible && p.HasPage).OrderBy(p => p.LastName).ThenBy(p => p.FirstName), "Id", "ReverseName", selectedId ?? default(int));
		}

	}
}
