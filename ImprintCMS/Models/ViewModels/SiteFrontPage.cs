﻿using System.Collections.Generic;

namespace ImprintCMS.Models.ViewModels
{
	public class SiteFrontPage
	{
		public IEnumerable<BookList> BookLists { get; set; }
		public IEnumerable<Article> Articles { get; set; }
	}
}