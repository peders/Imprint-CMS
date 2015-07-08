using System.Collections.Generic;

namespace ImprintCMS.Models.ViewModels
{
	public class ContactPage
	{
		public IEnumerable<ContactArticle> Articles { get; set; }
		public ContactArticle CurrentArticle { get; set; }
	}
}