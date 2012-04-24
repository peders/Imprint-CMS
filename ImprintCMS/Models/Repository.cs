using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
			_db.Persons.DeleteOnSubmit(person);
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
			_db.Editions.DeleteOnSubmit(edition);
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

		public void Save()
		{
			_db.SubmitChanges();
		}

	}

}