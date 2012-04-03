using System.Collections.Generic;

namespace ImprintCMS.Models.ViewModels
{
	public class PersonPage
	{
		public Person Person { get; set; }
		public IEnumerable<ListBook> Books { get; set; }
	}
}