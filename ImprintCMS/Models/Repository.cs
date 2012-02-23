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
		public IEnumerable<Binding> Bindings
		{
			get
			{
				return _db.Bindings;
			}
		}
		public IEnumerable<Genre> Genres
		{
			get
			{
				return _db.Genres;
			}
		}
		public IEnumerable<Role> Roles
		{
			get
			{
				return _db.Roles;
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
		public Binding GetBinding(int id)
		{
			return _db.Bindings.SingleOrDefault(b => b.Id == id);
		}
		public Genre GetGenre(int id)
		{
			return _db.Genres.SingleOrDefault(g => g.Id == id);
		}
		public Role GetRole(int id)
		{
			return _db.Roles.SingleOrDefault(r => r.Id == id);
		}

		public void Add(UploadedFile file)
		{
			_db.UploadedFiles.InsertOnSubmit(file);
		}
		public void Add(Binding binding)
		{
			_db.Bindings.InsertOnSubmit(binding);
		}
		public void Add(Genre genre)
		{
			_db.Genres.InsertOnSubmit(genre);
		}
		public void Add(Role role)
		{
			_db.Roles.InsertOnSubmit(role);
		}

		public void Delete(UploadedFile file)
		{
			_db.UploadedFiles.DeleteOnSubmit(file);
		}
		public void Delete(Binding binding)
		{
			_db.Bindings.DeleteOnSubmit(binding);
		}
		public void Delete(Genre genre)
		{
			_db.Genres.DeleteOnSubmit(genre);
		}
		public void Delete(Role role)
		{
			_db.Roles.DeleteOnSubmit(role);
		}

		public void Save()
		{
			_db.SubmitChanges();
		}

	}

}