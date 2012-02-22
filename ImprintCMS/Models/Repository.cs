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

		public void Add(UploadedFile file)
		{
			_db.UploadedFiles.InsertOnSubmit(file);
		}
		public void Add(Binding binding)
		{
			_db.Bindings.InsertOnSubmit(binding);
		}

		public void Delete(UploadedFile file)
		{
			_db.UploadedFiles.DeleteOnSubmit(file);
		}
		public void Delete(Binding binding)
		{
			_db.Bindings.DeleteOnSubmit(binding);
		}

		public void Save()
		{
			_db.SubmitChanges();
		}

	}

}