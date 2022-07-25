using ImprintCMS.Models;
using ImprintCMS.Models.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ImprintCMS.Controllers
{

    [Authorize]
    public class AdminController : ControllerBase
    {

        public AdminConfig AdminConfig { get; private set; }

        public AdminController()
        {
            AdminConfig = new AdminConfig(Repository);
            ViewBag.AdminConfig = AdminConfig;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Configurations()
        {
            var vm = Repository.Configurations.OrderBy(_ => _.Name);
            return View(vm);
        }

        public ActionResult CreateConfiguration()
        {
            var vm = new Configuration();
            return View(vm);
        }

        [HttpPost]
        public ActionResult CreateConfiguration(Configuration vm)
        {
            if (!ModelState.IsValid) return View(vm);
            Repository.Add(vm);
            if (vm.IsActive)
            {
                foreach (var configuration in Repository.Configurations.Where(_ => _.IsActive))
                {
                    configuration.IsActive = false;
                }
            }
            Repository.Save();
            return RedirectToAction("configurations");
        }

        public ActionResult EditConfiguration(int id)
        {
            var vm = Repository.GetConfiguration(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditConfiguration(Configuration vm)
        {
            if (!ModelState.IsValid) return View(vm);
            UpdateModel(Repository.GetConfiguration(vm.Id));
            if (vm.IsActive)
            {
                foreach (var configuration in Repository.Configurations.Where(_ => _.IsActive && _.Id != vm.Id))
                {
                    configuration.IsActive = false;
                }
            }
            Repository.Save();
            return RedirectToAction("configurations");
        }

        public ActionResult DeleteConfiguration(int id)
        {
            var vm = Repository.GetConfiguration(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteConfiguration(Configuration vm)
        {
            Repository.Delete(Repository.GetConfiguration(vm.Id));
            Repository.Save();
            return RedirectToAction("configurations");
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
                Files = Repository.ListFiles.Where(u => u.Category == id),
                IsForCache = id.Equals(FileCategories.CachedCover.ToString(), StringComparison.InvariantCultureIgnoreCase)
                    || id.Equals(FileCategories.CachedPortrait.ToString(), StringComparison.InvariantCultureIgnoreCase)
                    || id.Equals(FileCategories.CachedThumbnail.ToString(), StringComparison.InvariantCultureIgnoreCase)
                    || id.Equals(FileCategories.CachedArticleImage.ToString(), StringComparison.InvariantCultureIgnoreCase)
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

        public ActionResult ReplaceUpload(int id)
        {
            var upload = Repository.GetUploadedFile(id);
            if (upload == null)
                return HttpNotFound();
            var vm = new FileUpload
            {
                FileCategory = upload.Category,
                ReplaceId = id
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult ReplaceUpload(FileUpload vm)
        {
            var upload = Repository.GetUploadedFile(vm.ReplaceId);
            if (upload == null)
                return HttpNotFound();
            var file = Request.Files[0];
            if (file.ContentLength == 0)
                ModelState.AddModelError("", Phrases.ValidationEmptyOrNoFile);
            if (file.ContentLength > 10485760)
                ModelState.AddModelError("", Phrases.ValidationFileTooLarge);
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var fileData = new byte[file.ContentLength];
            file.InputStream.Read(fileData, 0, fileData.Length);
            upload.FileName = file.FileName.Sanitise();
            upload.ContentType = file.ContentType;
            upload.ContentLength = file.ContentLength;
            upload.Data = fileData;
            var siteConfig = new SiteConfig(Repository);
            if (upload.Category.Equals(FileCategories.ArticleImage.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                var cachedImage = Repository.GetUploadedFile(FileCategories.CachedArticleImage.ToString(), string.Format("cache{0}_{1}", siteConfig.CachedPortraitWidth, upload.FileName));
                if (cachedImage != null) Repository.Delete(cachedImage);
            }
            if (upload.Category.Equals(FileCategories.LargeCover.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                var cachedCover = Repository.GetUploadedFile(FileCategories.CachedCover.ToString(), string.Format("cache{0}_{1}", siteConfig.CachedCoverWidth, upload.FileName));
                if (cachedCover != null) Repository.Delete(cachedCover);
            }
            if (upload.Category.Equals(FileCategories.LargePortrait.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                var cachedPortrait = Repository.GetUploadedFile(FileCategories.CachedPortrait.ToString(), string.Format("cache{0}_{1}", siteConfig.CachedPortraitWidth, upload.FileName));
                if (cachedPortrait != null) Repository.Delete(cachedPortrait);
                var cachedThumbnail = Repository.GetUploadedFile(FileCategories.CachedThumbnail.ToString(), string.Format("cache{0}_{1}", siteConfig.CachedPortraitWidth, upload.FileName));
                if (cachedThumbnail != null) Repository.Delete(cachedThumbnail);
            }
            Repository.Save();
            return RedirectToAction("uploadcategory", new { id = vm.FileCategory });
        }

        public ActionResult DeleteUpload(int id)
        {
            var vm = Repository.GetUploadedFile(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteUpload(UploadedFile vm)
        {
            var upload = Repository.GetUploadedFile(vm.Id);
            Repository.Delete(upload);
            var siteConfig = new SiteConfig(Repository);
            if (upload.Category.Equals(FileCategories.ArticleImage.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                var cachedImage = Repository.GetUploadedFile(FileCategories.CachedArticleImage.ToString(), string.Format("cache{0}_{1}", siteConfig.CachedPortraitWidth, upload.FileName));
                if (cachedImage != null) Repository.Delete(cachedImage);
            }
            if (upload.Category.Equals(FileCategories.LargeCover.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                var cachedCover = Repository.GetUploadedFile(FileCategories.CachedCover.ToString(), string.Format("cache{0}_{1}", siteConfig.CachedCoverWidth, upload.FileName));
                if (cachedCover != null) Repository.Delete(cachedCover);
            }
            if (upload.Category.Equals(FileCategories.LargePortrait.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                var cachedPortrait = Repository.GetUploadedFile(FileCategories.CachedPortrait.ToString(), string.Format("cache{0}_{1}", siteConfig.CachedPortraitWidth, upload.FileName));
                if (cachedPortrait != null) Repository.Delete(cachedPortrait);
                var cachedThumbnail = Repository.GetUploadedFile(FileCategories.CachedThumbnail.ToString(), string.Format("cache{0}_{1}", siteConfig.CachedPortraitWidth, upload.FileName));
                if (cachedThumbnail != null) Repository.Delete(cachedThumbnail);
            }
            Repository.Save();
            return RedirectToAction("uploadcategory", new { id = upload.Category });
        }

        public ActionResult DeleteCachedUploads(string id)
        {
            var vm = new UploadCategoryFiles
            {
                Name = id,
                Files = Repository.ListFiles.Where(u => u.Category == id),
                IsForCache = id.Equals(FileCategories.CachedCover.ToString(), StringComparison.InvariantCultureIgnoreCase)
                    || id.Equals(FileCategories.CachedPortrait.ToString(), StringComparison.InvariantCultureIgnoreCase)
                    || id.Equals(FileCategories.CachedThumbnail.ToString(), StringComparison.InvariantCultureIgnoreCase)
                    || id.Equals(FileCategories.CachedArticleImage.ToString(), StringComparison.InvariantCultureIgnoreCase)
            };
            if (!vm.IsForCache) return HttpNotFound();
            return View(vm);

        }

        [HttpPost]
        public ActionResult DeleteCachedUploads(string id, object seed)
        {
            Repository.PurgeUploadCategory(id);
            Repository.Save();
            return RedirectToAction("uploadcategory", new { id });
        }

        public ActionResult Bindings()
        {
            var vm = Repository.Bindings.OrderBy(b => b.Name);
            return View(vm);
        }

        public ActionResult CreateBinding()
        {
            var vm = new Binding
            {
                DeliveryFormat = BookDeliveryFormats.Physical.ToString()
            };
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
            var vm = Repository.GetBinding(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteBinding(Binding vm)
        {
            Repository.Delete(Repository.GetBinding(vm.Id));
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
            var vm = Repository.GetGenre(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteGenre(Genre vm)
        {
            Repository.Delete(Repository.GetGenre(vm.Id));
            Repository.Save();
            return RedirectToAction("genres");
        }

        public ActionResult Shortcuts()
        {
            var vm = Repository.MenuShortcuts.OrderBy(_ => _.SequenceIdentifier);
            return View(vm);
        }

        public ActionResult CreateShortcut()
        {
            var vm = new MenuShortcut
            {
                SequenceIdentifier = int.MaxValue
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult CreateShortcut(MenuShortcut vm)
        {
            if (!ModelState.IsValid) return View(vm);
            Repository.Add(vm);
            Repository.Save();
            return RedirectToAction("shortcuts");
        }

        public ActionResult EditShortcut(int id)
        {
            var vm = Repository.GetMenuShortcut(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditShortcut(MenuShortcut vm)
        {
            if (!ModelState.IsValid) return View(vm);
            UpdateModel(Repository.GetMenuShortcut(vm.Id));
            Repository.Save();
            return RedirectToAction("shortcuts");
        }

        public ActionResult DeleteShortcut(int id)
        {
            var vm = Repository.GetMenuShortcut(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteShortcut(MenuShortcut vm)
        {
            Repository.Delete(Repository.GetMenuShortcut(vm.Id));
            Repository.Save();
            return RedirectToAction("shortcuts");
        }

        public ActionResult ExternalStores()
        {
            var vm = Repository.ExternalStores.OrderBy(s => s.SequenceIdentifier);
            return View(vm);
        }

        public ActionResult CreateExternalStore()
        {
            var vm = new ExternalStore
            {
                SequenceIdentifier = int.MaxValue
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult CreateExternalStore(ExternalStore vm)
        {
            if (!ModelState.IsValid) return View(vm);
            Repository.Add(vm);
            Repository.Save();
            return RedirectToAction("externalstores");
        }

        public ActionResult EditExternalStore(int id)
        {
            var vm = Repository.GetExternalStore(id);
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditExternalStore(ExternalStore vm)
        {
            if (!ModelState.IsValid) return View(vm);
            UpdateModel(Repository.GetExternalStore(vm.Id));
            Repository.Save();
            return RedirectToAction("externalstores");
        }

        public ActionResult DeleteExternalStore(int id)
        {
            var vm = Repository.GetExternalStore(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteExternalStore(ExternalStore vm)
        {
            Repository.Delete(Repository.GetExternalStore(vm.Id));
            Repository.Save();
            return RedirectToAction("externalstores");
        }

        public ActionResult Roles()
        {
            var vm = Repository.Roles.OrderBy(r => r.SequenceIdentifier);
            return View(vm);
        }

        public ActionResult CreateRole()
        {
            var vm = new Role
            {
                SequenceIdentifier = int.MaxValue,
                ShowBookList = true
            };
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
            var vm = Repository.GetRole(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteRole(Role vm)
        {
            Repository.Delete(Repository.GetRole(vm.Id));
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
            return View(vm);
        }

        [HttpPost]
        public ActionResult CreatePerson(Person vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            Repository.Add(vm);
            Repository.Save();
            return RedirectToAction("people");
        }

        public ActionResult EditPerson(int id)
        {
            var vm = Repository.GetPerson(id);
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditPerson(Person vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            UpdateModel(Repository.GetPerson(vm.Id));
            Repository.Save();
            return RedirectToAction("people");
        }

        public ActionResult DeletePerson(int id)
        {
            var vm = Repository.GetPerson(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult DeletePerson(Person vm)
        {
            Repository.Delete(Repository.GetPerson(vm.Id));
            Repository.Save();
            return RedirectToAction("people");
        }

        public ActionResult CreatePersonImage(int id)
        {
            var person = Repository.GetPerson(id);
            if (person == null) return HttpNotFound();
            var vm = new PersonImage
            {
                PersonId = id,
                Person = person,
                SequenceIdentifier = int.MaxValue
            };
            ViewBag.LargeImages = FileList(FileCategories.LargePortrait, vm.LargeImageId);
            return View(vm);
        }

        [HttpPost]
        public ActionResult CreatePersonImage(PersonImage vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.LargeImages = FileList(FileCategories.LargePortrait, vm.LargeImageId);
                vm.Person = Repository.GetPerson(vm.PersonId);
                return View(vm);
            }
            Repository.Add(vm);
            Repository.Save();
            return RedirectToAction("editperson", new { id = vm.PersonId });
        }

        public ActionResult EditPersonImage(int id)
        {
            var vm = Repository.GetPersonImage(id);
            if (vm == null) return HttpNotFound();
            ViewBag.LargeImages = FileList(FileCategories.LargePortrait, vm.LargeImageId);
            vm.Person = Repository.GetPerson(vm.PersonId);
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditPersonImage(PersonImage vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.LargeImages = FileList(FileCategories.LargePortrait, vm.LargeImageId);
                vm.Person = Repository.GetPerson(vm.PersonId);
                return View(vm);
            }
            UpdateModel(Repository.GetPersonImage(vm.Id));
            Repository.Save();
            return RedirectToAction("editperson", new { id = vm.PersonId });
        }

        public ActionResult RemovePersonImage(int id)
        {
            var image = Repository.GetPersonImage(id);
            Repository.Delete(image);
            Repository.Save();
            return RedirectToAction("editperson", new { id = image.PersonId });
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
            ViewBag.AudioExcerpts = FileList(FileCategories.AudioExcerpt, vm.AudioExcerptId);
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
                ViewBag.AudioExcerpts = FileList(FileCategories.AudioExcerpt, vm.AudioExcerptId);
                ViewBag.Genres = GenreList(vm.GenreId);
                return View(vm);
            }
            vm.CachedReleaseYear = vm.GenerateReleaseYear();
            Repository.Add(vm);
            Repository.Save();
            return RedirectToAction("createrelation", new { id = vm.Id });
        }

        public ActionResult EditBook(int id)
        {
            var vm = Repository.GetBook(id);
            ViewBag.Excerpts = FileList(FileCategories.Excerpt, vm.ExcerptId);
            ViewBag.AudioExcerpts = FileList(FileCategories.AudioExcerpt, vm.AudioExcerptId);
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
                ViewBag.AudioExcerpts = FileList(FileCategories.AudioExcerpt, vm.AudioExcerptId);
                ViewBag.Genres = GenreList(vm.GenreId);
                return View(vm);
            }
            UpdateModel(Repository.GetBook(vm.Id));
            Repository.Save();
            return RedirectToAction("updatecachedbookfields", new { id = vm.Id });
        }

        public ActionResult DeleteBook(int id)
        {
            var vm = Repository.GetBook(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteBook(Book vm)
        {
            Repository.Delete(Repository.GetBook(vm.Id));
            Repository.Save();
            return RedirectToAction("books");
        }

        public ActionResult CreateEdition(int id)
        {
            var book = Repository.GetBook(id);
            if (book == null) return HttpNotFound();
            var vm = new Edition
            {
                Number = !book.Editions.Any() ? 1 : book.Editions.OrderBy(e => e.Number).Last().Number + 1,
                ReleaseDate = DateTime.Today,
                IsForSale = true,
                BookId = id,
                Book = book
            };
            ViewBag.LargeCovers = FileList(FileCategories.LargeCover, vm.LargeCoverId);
            ViewBag.Bindings = BindingList(vm.BindingId);
            return View(vm);
        }

        [HttpPost]
        public ActionResult CreateEdition(Edition vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.LargeCovers = FileList(FileCategories.LargeCover, vm.LargeCoverId);
                ViewBag.Bindings = BindingList(vm.BindingId);
                vm.Book = Repository.GetBook(vm.BookId);
                return View(vm);
            }
            Repository.Add(vm);
            Repository.Save();
            return RedirectToAction("updatereleaseyearcache", new { id = vm.BookId });
        }

        public ActionResult EditEdition(int id)
        {
            var vm = Repository.GetEdition(id);
            if (vm == null) return HttpNotFound();
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
                ViewBag.LargeCovers = FileList(FileCategories.LargeCover, vm.LargeCoverId);
                ViewBag.Bindings = BindingList(vm.BindingId);
                vm.Book = Repository.GetBook(vm.BookId);
                return View(vm);
            }
            UpdateModel(Repository.GetEdition(vm.Id));
            Repository.Save();
            return RedirectToAction("updatereleaseyearcache", new { id = vm.BookId });
        }

        public ActionResult DeleteEdition(int id)
        {
            var vm = Repository.GetEdition(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteEdition(Edition vm)
        {
            var edition = Repository.GetEdition(vm.Id);
            if (edition == null) return HttpNotFound();
            Repository.Delete(edition);
            Repository.Save();
            return RedirectToAction("updatereleaseyearcache", new { id = edition.BookId });
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
            if (Repository.Relations.Any(r => r.PersonId == vm.PersonId && r.BookId == vm.BookId)) ModelState.AddModelError("PersonId", Phrases.ValidationPersonHasRelationToBook);
            if (!ModelState.IsValid)
            {
                ViewBag.People = PeopleList(vm.PersonId);
                ViewBag.Roles = RoleList(vm.RoleId);
                vm.Book = Repository.GetBook(vm.BookId);
                return View(vm);
            }
            Repository.Add(vm);
            Repository.Save();
            return RedirectToAction("updaterelationcache", new { id = vm.BookId });
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
            if (Repository.Relations.Any(r => r.PersonId == vm.PersonId && r.BookId == vm.BookId && r.Id != vm.Id)) ModelState.AddModelError("PersonId", Phrases.ValidationPersonHasRelationToBook);
            if (!ModelState.IsValid)
            {
                ViewBag.People = PeopleList(vm.PersonId);
                ViewBag.Roles = RoleList(vm.RoleId);
                vm.Book = Repository.GetBook(vm.BookId);
                return View(vm);
            }
            UpdateModel(Repository.GetRelation(vm.Id));
            Repository.Save();
            return RedirectToAction("updaterelationcache", new { id = vm.BookId });
        }

        public ActionResult RemoveRelation(int id)
        {
            var vm = Repository.GetRelation(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult RemoveRelation(Relation vm)
        {
            var relation = Repository.GetRelation(vm.Id);
            Repository.Delete(relation);
            Repository.Save();
            return RedirectToAction("updaterelationcache", new { id = relation.BookId });
        }

        public ActionResult UpdateRelationCache(int id)
        {
            var vm = Repository.GetBook(id);
            if (vm == null) return HttpNotFound();
            vm.CachedRightsHoldersText = vm.GenerateRightsHoldersText();
            Repository.Save();
            return RedirectToAction("editbook", new { id = vm.Id });
        }

        public ActionResult UpdateReleaseYearCache(int id)
        {
            var vm = Repository.GetBook(id);
            if (vm == null) return HttpNotFound();
            vm.CachedReleaseYear = vm.GenerateReleaseYear();
            Repository.Save();
            return RedirectToAction("editbook", new { id = vm.Id });
        }

        public ActionResult UpdateCachedBookFields(int id)
        {
            var vm = Repository.GetBook(id);
            if (vm == null) return HttpNotFound();
            vm.CachedReleaseYear = vm.GenerateReleaseYear();
            vm.CachedRightsHoldersText = vm.GenerateRightsHoldersText();
            Repository.Save();
            return RedirectToAction("books");
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
            var vm = Repository.GetBookList(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteBookList(BookList vm)
        {
            Repository.Delete(Repository.GetBookList(vm.Id));
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

        public ActionResult RemoveBookListMembership(int id)
        {
            var vm = Repository.GetBookListMembership(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult RemoveBookListMembership(BookListMembership vm)
        {
            var membership = Repository.GetBookListMembership(vm.Id);
            Repository.Delete(membership);
            Repository.Save();
            return RedirectToAction("editbooklist", new { id = membership.BookListId });
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
                IsVisible = true,
                IsOnFrontPage = true
            };
            ViewBag.Images = FileList(FileCategories.ArticleImage, vm.ImageId);
            return View(vm);
        }

        [HttpPost]
        public ActionResult CreateArticle(Article vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Images = FileList(FileCategories.ArticleImage, vm.ImageId);
                return View(vm);
            }
            Repository.Add(vm);
            Repository.Save();
            return RedirectToAction("editarticle", new { id = vm.Id });
        }

        public ActionResult EditArticle(int id)
        {
            var vm = Repository.GetArticle(id);
            if (vm == null) return HttpNotFound();
            ViewBag.Images = FileList(FileCategories.ArticleImage, vm.ImageId);
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditArticle(Article vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Images = FileList(FileCategories.ArticleImage, vm.ImageId);
                return View(vm);
            }
            UpdateModel(Repository.GetArticle(vm.Id));
            Repository.Save();
            return RedirectToAction("articles");
        }

        public ActionResult DeleteArticle(int id)
        {
            var vm = Repository.GetArticle(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteArticle(Article vm)
        {
            Repository.Delete(Repository.GetArticle(vm.Id));
            Repository.Save();
            return RedirectToAction("articles");
        }

        public ActionResult ArticleGroups()
        {
            var vm = Repository.ArticleGroups.OrderBy(a => a.SequenceIdentifier);
            return View(vm);
        }

        public ActionResult CreateArticleGroup()
        {
            var vm = new ArticleGroup
            {
                SequenceIdentifier = int.MaxValue
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult CreateArticleGroup(ArticleGroup vm)
        {
            if (!ModelState.IsValid) return View(vm);
            Repository.Add(vm);
            Repository.Save();
            return RedirectToAction("articlegroups");
        }

        public ActionResult EditArticleGroup(int id)
        {
            var vm = Repository.GetArticleGroup(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditArticleGroup(ArticleGroup vm)
        {
            if (!ModelState.IsValid) return View(vm);
            UpdateModel(Repository.GetArticleGroup(vm.Id));
            Repository.Save();
            return RedirectToAction("articlegroups");
        }

        public ActionResult DeleteArticleGroup(int id)
        {
            var vm = Repository.GetArticleGroup(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteArticleGroup(ArticleGroup vm)
        {
            Repository.Delete(Repository.GetArticleGroup(vm.Id));
            Repository.Save();
            return RedirectToAction("articlegroups");
        }

        public ActionResult CreateContactArticle(int id)
        {
            var group = Repository.GetArticleGroup(id);
            if (group == null) return HttpNotFound();
            var vm = new ContactArticle
            {
                GroupId = id,
                ArticleGroup = group,
                SequenceIdentifier = int.MaxValue
            };
            ViewBag.Groups = ArticleGroupList(vm.GroupId);
            return View(vm);
        }

        [HttpPost]
        public ActionResult CreateContactArticle(ContactArticle vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Groups = ArticleGroupList(vm.GroupId);
                vm.ArticleGroup = Repository.GetArticleGroup(vm.GroupId);
                return View(vm);
            }
            Repository.Add(vm);
            Repository.Save();
            return RedirectToAction("editarticlegroup", new { id = vm.GroupId });
        }

        public ActionResult EditContactArticle(int id)
        {
            var vm = Repository.GetContactArticle(id);
            if (vm == null) return HttpNotFound();
            ViewBag.Groups = ArticleGroupList(vm.GroupId);
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditContactArticle(ContactArticle vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Groups = ArticleGroupList(vm.GroupId);
                vm.ArticleGroup = Repository.GetArticleGroup(vm.GroupId);
                return View(vm);
            }
            UpdateModel(Repository.GetContactArticle(vm.Id));
            Repository.Save();
            return RedirectToAction("editarticlegroup", new { id = vm.GroupId });
        }

        public ActionResult DeleteContactArticle(int id)
        {
            var vm = Repository.GetContactArticle(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteContactArticle(ContactArticle vm)
        {
            var article = Repository.GetContactArticle(vm.Id);
            Repository.Delete(article);
            Repository.Save();
            return RedirectToAction("editarticlegroup", new { id = article.GroupId });
        }

        [HttpPost]
        public ActionResult StorePersonImageOrder()
        {
            var data = Request.Form["sortitem[]"] as string;
            var ids = data.Split(',').Select(int.Parse);
            var sequenceIdentifier = 0;
            foreach (var id in ids)
            {
                var image = Repository.GetPersonImage(id);
                image.SequenceIdentifier = sequenceIdentifier;
                sequenceIdentifier++;
            }
            Repository.Save();
            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        public ActionResult StoreArticleGroupOrder()
        {
            var data = Request.Form["sortitem[]"] as string;
            var ids = data.Split(',').Select(int.Parse);
            var sequenceIdentifier = 0;
            foreach (var id in ids)
            {
                var group = Repository.GetArticleGroup(id);
                group.SequenceIdentifier = sequenceIdentifier;
                sequenceIdentifier++;
            }
            Repository.Save();
            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        public ActionResult StoreContactArticleOrder()
        {
            var data = Request.Form["sortitem[]"] as string;
            var ids = data.Split(',').Select(int.Parse);
            var sequenceIdentifier = 0;
            foreach (var id in ids)
            {
                var article = Repository.GetContactArticle(id);
                article.SequenceIdentifier = sequenceIdentifier;
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

        [HttpPost]
        public ActionResult StoreShortcutOrder()
        {
            var data = Request.Form["sortitem[]"] as string;
            var ids = data.Split(',').Select(int.Parse);
            var sequenceIdentifier = 0;
            foreach (var id in ids)
            {
                var shortcut = Repository.GetMenuShortcut(id);
                shortcut.SequenceIdentifier = sequenceIdentifier;
                sequenceIdentifier++;
            }
            Repository.Save();
            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        public ActionResult StoreRoleOrder()
        {
            var data = Request.Form["sortitem[]"] as string;
            var ids = data.Split(',').Select(int.Parse);
            var sequenceIdentifier = 0;
            foreach (var id in ids)
            {
                var role = Repository.GetRole(id);
                role.SequenceIdentifier = sequenceIdentifier;
                sequenceIdentifier++;
            }
            Repository.Save();
            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        public ActionResult StoreExternalStoreOrder()
        {
            var data = Request.Form["sortitem[]"] as string;
            var ids = data.Split(',').Select(int.Parse);
            var sequenceIdentifier = 0;
            foreach (var id in ids)
            {
                var store = Repository.GetExternalStore(id);
                store.SequenceIdentifier = sequenceIdentifier;
                sequenceIdentifier++;
            }
            Repository.Save();
            return new HttpStatusCodeResult(200);
        }

        public ActionResult CreatePersonToArticle(int id)
        {
            var article = Repository.GetArticle(id);
            if (article == null) return HttpNotFound();
            var vm = new PersonToArticle
            {
                ArticleId = id,
                Article = article,
                SequenceIdentifier = int.MaxValue
            };
            ViewBag.People = LinkablePeopleList(vm.PersonId);
            return View(vm);
        }

        [HttpPost]
        public ActionResult CreatePersonToArticle(PersonToArticle vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.People = LinkablePeopleList(vm.PersonId);
                vm.Article = Repository.GetArticle(vm.ArticleId);
                return View(vm);
            }
            Repository.Add(vm);
            Repository.Save();
            return RedirectToAction("editarticle", new { id = vm.ArticleId });
        }

        public ActionResult RemovePersonToArticle(int id)
        {
            var vm = Repository.GetPersonToArticle(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult RemovePersonToArticle(PersonToArticle vm)
        {
            var link = Repository.GetPersonToArticle(vm.Id);
            Repository.Delete(link);
            Repository.Save();
            return RedirectToAction("editarticle", new { id = link.ArticleId });
        }

        [HttpPost]
        public ActionResult StorePersonToArticleOrder()
        {
            var data = Request.Form["sortitem[]"] as string;
            var ids = data.Split(',').Select(int.Parse);
            var sequenceIdentifier = 0;
            foreach (var id in ids)
            {
                var relation = Repository.GetPersonToArticle(id);
                relation.SequenceIdentifier = sequenceIdentifier;
                sequenceIdentifier++;
            }
            Repository.Save();
            return new HttpStatusCodeResult(200);
        }

        public ActionResult CreateBookToArticle(int id)
        {
            var article = Repository.GetArticle(id);
            if (article == null) return HttpNotFound();
            var vm = new BookToArticle
            {
                ArticleId = id,
                Article = article,
                SequenceIdentifier = int.MaxValue
            };
            ViewBag.Books = LinkableBooksList(vm.BookId);
            return View(vm);
        }

        [HttpPost]
        public ActionResult CreateBookToArticle(BookToArticle vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Books = LinkableBooksList(vm.BookId);
                vm.Article = Repository.GetArticle(vm.ArticleId);
                return View(vm);
            }
            Repository.Add(vm);
            Repository.Save();
            return RedirectToAction("editarticle", new { id = vm.ArticleId });
        }

        public ActionResult RemoveBookToArticle(int id)
        {
            var vm = Repository.GetBookToArticle(id);
            if (vm == null) return HttpNotFound();
            return View(vm);
        }

        [HttpPost]
        public ActionResult RemoveBookToArticle(BookToArticle vm)
        {
            var link = Repository.GetBookToArticle(vm.Id);
            Repository.Delete(link);
            Repository.Save();
            return RedirectToAction("editarticle", new { id = link.ArticleId });
        }

        [HttpPost]
        public ActionResult StoreBookToArticleOrder()
        {
            var data = Request.Form["sortitem[]"] as string;
            var ids = data.Split(',').Select(int.Parse);
            var sequenceIdentifier = 0;
            foreach (var id in ids)
            {
                var relation = Repository.GetBookToArticle(id);
                relation.SequenceIdentifier = sequenceIdentifier;
                sequenceIdentifier++;
            }
            Repository.Save();
            return new HttpStatusCodeResult(200);
        }

        public ActionResult UpdateCacheFields()
        {
            foreach (var book in Repository.Books)
            {
                book.CachedRightsHoldersText = book.GenerateRightsHoldersText();
                book.CachedReleaseYear = book.GenerateReleaseYear();
            }
            Repository.Save();
            return RedirectToAction("index");
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
        private SelectList ArticleGroupList(int? selectedId)
        {
            return new SelectList(Repository.ArticleGroups.OrderBy(_ => _.Title), "Id", "Title", selectedId ?? default(int));
        }
        private SelectList PeopleList(int? selectedId)
        {
            return new SelectList(Repository.People.OrderBy(p => p.LastName).ThenBy(p => p.FirstName), "Id", "ReverseName", selectedId ?? default(int));
        }
        private SelectList LinkablePeopleList(int? selectedId)
        {
            return new SelectList(Repository.People.Where(p => p.IsVisible && p.HasPage).OrderBy(p => p.LastName).ThenBy(p => p.FirstName), "Id", "ReverseName", selectedId ?? default(int));
        }
        private SelectList LinkableBooksList(int? selectedId)
        {
            return new SelectList(Repository.Books.Where(b => b.IsVisible).OrderBy(b => b.FullTitle), "Id", "FullTitle", selectedId ?? default(int));
        }

    }
}
