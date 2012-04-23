using System.Collections.Generic;

namespace ImprintCMS.Models.ViewModels
{
	public class ContactPage
	{
		public IEnumerable<Article> Articles { get; set; }
		public Article CurrentArticle { get; set; }
	}
}