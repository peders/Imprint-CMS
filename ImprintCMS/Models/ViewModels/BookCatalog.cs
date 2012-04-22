using System.Collections.Generic;

namespace ImprintCMS.Models.ViewModels
{
	public class BookCatalog
	{
		public IEnumerable<Genre> Genres { get; set; }
		public Genre CurrentGenre { get; set; }
		public IEnumerable<ListBook> CurrentBooks { get; set; }
	}
}