using ImprintCMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImprintCMS.Models
{
    public class Repository
    {
        ImprintCMSDataContext _db;

        public Repository()
        {
            _db = new ImprintCMSDataContext();
        }

        public IEnumerable<UploadedFile> UploadedFiles
        {
            get
            {
                return _db.UploadedFiles;
            }
        }
        public UploadedFile GetUploadedFile(string category, string fileName)
        {
            return _db.UploadedFiles.SingleOrDefault(f => f.Category == category && f.FileName == fileName);
        }
        public UploadedFile GetUploadedFile(int id)
        {
            return _db.UploadedFiles.SingleOrDefault(f => f.Id == id);
        }
        public void Add(UploadedFile file)
        {
            _db.UploadedFiles.InsertOnSubmit(file);
        }
        public void Delete(UploadedFile file)
        {
            _db.UploadedFiles.DeleteOnSubmit(file);
        }

        public IEnumerable<ListFile> ListFiles
        {
            get
            {
                return _db.UploadedFiles.Select(u => new ListFile
                {
                    Id = u.Id,
                    FileName = u.FileName,
                    Category = u.Category,
                    ContentLength = u.ContentLength,
                    ContentType = u.ContentType
                });
            }
        }

        public IEnumerable<Binding> Bindings
        {
            get
            {
                return _db.Bindings;
            }
        }
        public Binding GetBinding(int id)
        {
            return _db.Bindings.SingleOrDefault(b => b.Id == id);
        }
        public void Add(Binding binding)
        {
            _db.Bindings.InsertOnSubmit(binding);
        }
        public void Delete(Binding binding)
        {
            _db.Bindings.DeleteOnSubmit(binding);
        }

        public IEnumerable<Genre> Genres
        {
            get
            {
                return _db.Genres;
            }
        }
        public Genre GetGenre(int id)
        {
            return _db.Genres.SingleOrDefault(g => g.Id == id);
        }
        public void Add(Genre genre)
        {
            _db.Genres.InsertOnSubmit(genre);
        }
        public void Delete(Genre genre)
        {
            _db.Genres.DeleteOnSubmit(genre);
        }

        public IEnumerable<Role> Roles
        {
            get
            {
                return _db.Roles;
            }
        }
        public Role GetRole(int id)
        {
            return _db.Roles.SingleOrDefault(r => r.Id == id);
        }
        public void Add(Role role)
        {
            _db.Roles.InsertOnSubmit(role);
        }
        public void Delete(Role role)
        {
            _db.Roles.DeleteOnSubmit(role);
        }

        public IEnumerable<Person> People
        {
            get
            {
                return _db.Persons;
            }
        }
        public Person GetPerson(int id)
        {
            return _db.Persons.SingleOrDefault(p => p.Id == id);
        }
        public void Add(Person person)
        {
            _db.Persons.InsertOnSubmit(person);
        }
        public void Delete(Person person)
        {
            _db.PersonToArticles.DeleteAllOnSubmit(person.PersonToArticles);
            _db.PersonImages.DeleteAllOnSubmit(person.PersonImages);
            _db.Persons.DeleteOnSubmit(person);
        }
        public PersonImage GetPersonImage(int id)
        {
            return _db.PersonImages.SingleOrDefault(i => i.Id == id);
        }
        public void Add(PersonImage image)
        {
            _db.PersonImages.InsertOnSubmit(image);
        }
        public void Delete(PersonImage image)
        {
            _db.PersonImages.DeleteOnSubmit(image);
        }

        public IEnumerable<Book> Books
        {
            get
            {
                return _db.Books;
            }
        }
        public Book GetBook(int id)
        {
            return _db.Books.SingleOrDefault(b => b.Id == id);
        }
        public void Add(Book book)
        {
            _db.Books.InsertOnSubmit(book);
        }
        public void Delete(Book book)
        {
            _db.Editions.DeleteAllOnSubmit(book.Editions);
            _db.Relations.DeleteAllOnSubmit(book.Relations);
            _db.BookToArticles.DeleteAllOnSubmit(book.BookToArticles);
            _db.Books.DeleteOnSubmit(book);
        }

        public IEnumerable<BookList> BookLists
        {
            get
            {
                return _db.BookLists;
            }
        }
        public BookList GetBookList(int id)
        {
            return _db.BookLists.SingleOrDefault(l => l.Id == id);
        }
        public void Add(BookList list)
        {
            _db.BookLists.InsertOnSubmit(list);
        }
        public void Delete(BookList list)
        {
            _db.BookListMemberships.DeleteAllOnSubmit(list.BookListMemberships);
            _db.BookLists.DeleteOnSubmit(list);
        }

        public IEnumerable<Article> Articles
        {
            get
            {
                return _db.Articles;
            }
        }
        public Article GetArticle(int id)
        {
            return _db.Articles.SingleOrDefault(a => a.Id == id);
        }
        public void Add(Article article)
        {
            _db.Articles.InsertOnSubmit(article);
        }
        public void Delete(Article article)
        {
            _db.PersonToArticles.DeleteAllOnSubmit(article.PersonToArticles);
            _db.BookToArticles.DeleteAllOnSubmit(article.BookToArticles);
            _db.Articles.DeleteOnSubmit(article);
        }

        public IEnumerable<ContactArticle> ContactArticles
        {
            get
            {
                return _db.ContactArticles;
            }
        }
        public ContactArticle GetContactArticle(int id)
        {
            return _db.ContactArticles.SingleOrDefault(a => a.Id == id);
        }
        public void Add(ContactArticle article)
        {
            _db.ContactArticles.InsertOnSubmit(article);
        }
        public void Delete(ContactArticle article)
        {
            _db.ContactArticles.DeleteOnSubmit(article);
        }

        public IEnumerable<Edition> Editions
        {
            get
            {
                return _db.Editions;
            }
        }
        public Edition GetEdition(int id)
        {
            return _db.Editions.SingleOrDefault(e => e.Id == id);
        }
        public void Add(Edition edition)
        {
            _db.Editions.InsertOnSubmit(edition);
        }
        public void Delete(Edition edition)
        {
            _db.BookListMemberships.DeleteAllOnSubmit(edition.BookListMemberships);
            _db.OrderLines.DeleteAllOnSubmit(edition.OrderLines.Where(l => l.Order.ClosedAt != null));
            _db.Editions.DeleteOnSubmit(edition);
        }

        public IEnumerable<Relation> Relations
        {
            get
            {
                return _db.Relations;
            }
        }
        public Relation GetRelation(int id)
        {
            return _db.Relations.SingleOrDefault(r => r.Id == id);
        }
        public void Add(Relation relation)
        {
            _db.Relations.InsertOnSubmit(relation);
        }
        public void Delete(Relation relation)
        {
            _db.Relations.DeleteOnSubmit(relation);
        }

        public PersonToArticle GetPersonToArticle(int id)
        {
            return _db.PersonToArticles.SingleOrDefault(p => p.Id == id);
        }
        public void Add(PersonToArticle link)
        {
            _db.PersonToArticles.InsertOnSubmit(link);
        }
        public void Delete(PersonToArticle link)
        {
            _db.PersonToArticles.DeleteOnSubmit(link);
        }

        public BookToArticle GetBookToArticle(int id)
        {
            return _db.BookToArticles.SingleOrDefault(b => b.Id == id);
        }
        public void Add(BookToArticle link)
        {
            _db.BookToArticles.InsertOnSubmit(link);
        }
        public void Delete(BookToArticle link)
        {
            _db.BookToArticles.DeleteOnSubmit(link);
        }

        public BookListMembership GetBookListMembership(int id)
        {
            return _db.BookListMemberships.SingleOrDefault(m => m.Id == id);
        }
        public void Add(BookListMembership membership)
        {
            _db.BookListMemberships.InsertOnSubmit(membership);
        }
        public void Delete(BookListMembership membership)
        {
            _db.BookListMemberships.DeleteOnSubmit(membership);
        }

        public IEnumerable<ExternalStore> ExternalStores
        {
            get
            {
                return _db.ExternalStores;
            }
        }
        public ExternalStore GetExternalStore(int id)
        {
            return _db.ExternalStores.SingleOrDefault(s => s.Id == id);
        }
        public void Add(ExternalStore store)
        {
            _db.ExternalStores.InsertOnSubmit(store);
        }
        public void Delete(ExternalStore store)
        {
            _db.ExternalStores.DeleteOnSubmit(store);
        }

        public IEnumerable<Order> Orders
        {
            get
            {
                return _db.Orders;
            }
        }
        public Order GetOrder(int id)
        {
            return _db.Orders.SingleOrDefault(o => o.Id == id);
        }
        public Order GetOrder(Guid guid)
        {
            return _db.Orders.SingleOrDefault(o => o.ExternalId == guid);
        }
        public void Add(Order order)
        {
            _db.Orders.InsertOnSubmit(order);
        }
        public void Delete(Order order)
        {
            _db.OrderLines.DeleteAllOnSubmit(order.OrderLines);
            _db.Orders.DeleteOnSubmit(order);
        }

        public OrderLine GetOrderLine(int id)
        {
            return _db.OrderLines.SingleOrDefault(l => l.Id == id);
        }
        public void Add(OrderLine line)
        {
            _db.OrderLines.InsertOnSubmit(line);
        }
        public void Delete(OrderLine line)
        {
            _db.OrderLines.DeleteOnSubmit(line);
        }

        public IEnumerable<Configuration> Configurations
        {
            get
            {
                return _db.Configurations;
            }
        }
        public Configuration GetConfiguration(int id)
        {
            return _db.Configurations.SingleOrDefault(_ => _.Id == id);
        }
        public void Add(Configuration configuration)
        {
            _db.Configurations.InsertOnSubmit(configuration);
        }
        public void Delete(Configuration configuration)
        {
            _db.Configurations.DeleteOnSubmit(configuration);
        }

        public IEnumerable<UploadedFile> Stylesheets
        {
            get
            {
                return _db.UploadedFiles.Where(u => u.Category == FileCategories.Layout.ToString() && u.ContentType == "text/css" && !u.FileName.StartsWith("legacy_"));
            }
        }

        public IEnumerable<UploadedFile> LegacyStylesheets
        {
            get
            {
                return _db.UploadedFiles.Where(u => u.Category == FileCategories.Layout.ToString() && u.ContentType == "text/css" && u.FileName.StartsWith("legacy_"));
            }
        }

        public IEnumerable<UploadedFile> Scripts
        {
            get
            {
                return _db.UploadedFiles.Where(u => u.Category == FileCategories.Script.ToString());
            }
        }

        public void Save()
        {
            _db.SubmitChanges();
        }

    }

}