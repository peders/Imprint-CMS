using System.Collections.Generic;

namespace ImprintCMS.Models.ViewModels
{
	public class SearchResult
	{
		public string Query { get; set; }
		public IEnumerable<Book> Books { get; set; }
		public IEnumerable<Person> People { get; set; }
	}
}