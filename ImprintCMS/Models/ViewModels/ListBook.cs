using System.Web;
using System.Web.Mvc;
using System.Linq;
using System;

namespace ImprintCMS.Models.ViewModels
{
	public class ListBook
	{
		public string Title { get; set; }
		public string Subtitle { get; set; }
		public Genre Genre { get; set; }
		public int ReleaseYear { get; set; }
		public string RightsHolders { get; set; }
		public string Url { get; set; }
		public HtmlString PersonPageEntry { get; set; }
		public Edition CoverEdition { get; set; }

		public ListBook(Book book, UrlHelper url)
		{
			Title = book.Title;
			Subtitle = book.Subtitle;
			Genre = book.Genre;
			Url = url.Action("details", "books", new { id = book.Id });
			if (!book.Relations.Any())
			{
				RightsHolders = "–";
			}
			else
			{
				RightsHolders = String.Join(" / ", book.Relations.OrderBy(r => r.SequenceIdentifier).Select(r => r.ReversePersonName));
			}
			if (!book.Editions.Any())
			{
				ReleaseYear = book.ExternalReleaseYear ?? default(int);
			}
			else
			{
				ReleaseYear = book.Editions.OrderBy(e => e.Number).First().ReleaseDate.Year;
			}
			if (book.HasExternalPublisher)
			{
				PersonPageEntry = new HtmlString(String.Format("{0}{1}. {2}, {3}",
					book.Title,
					!String.IsNullOrWhiteSpace(book.Subtitle) ? String.Format(". {0}", book.Subtitle) : string.Empty,
					book.ExternalPublisher,
					ReleaseYear));
			}
			else
			{
				PersonPageEntry = new HtmlString(String.Format("{0}{1}, {2}",
					"<a href=\"" + Url + "\">" + book.Title + "</a>",
					!String.IsNullOrWhiteSpace(book.Subtitle) ? String.Format(". {0}", book.Subtitle) : string.Empty,
					ReleaseYear));
			}
			CoverEdition = book.Editions.Where(e => e.SmallCoverId != null).OrderBy(e => e.Number).Last();
		}

		public string SubtitleWithSeparator
		{
			get
			{
				if (String.IsNullOrWhiteSpace(Subtitle)) return null;
				return String.Format("{0} {1}", !(Title.EndsWith(".") ||Title.EndsWith("!") ||Title.EndsWith("?")) ? "." : string.Empty, Subtitle);
			}
		}
	}
}