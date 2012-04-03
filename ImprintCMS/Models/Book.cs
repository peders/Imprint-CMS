using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace ImprintCMS.Models
{
	[MetadataType(typeof(BookMetadata))]
	public partial class Book
	{
		public string FullTitle
		{
			get
			{
				if (String.IsNullOrWhiteSpace(Subtitle)) return Title;
				return String.Format("{0} : {1}", Title, Subtitle);
			}
		}

		public UploadedFile Cover
		{
			get
			{
				if (!Editions.Any(e => e.SmallCoverId != null)) return null;
				return Editions.Where(e => e.SmallCoverId != null).OrderBy(e => e.Number).Last().UploadedFile;
			}
		}

		public int? ReleaseYear
		{
			get
			{
				if (!String.IsNullOrWhiteSpace(ExternalPublisher)) return ExternalReleaseYear;
				if (Editions.Any()) return Editions.OrderBy(e => e.Number).First().ReleaseDate.Year;
				return null;
			}
		}

		public string PersonSortString
		{
			get
			{
				if (Relations.Any()) return Relations.First().ReversePersonName;
				return null;
			}
		}

	}

	public class BookMetadata
	{
		[Required(ErrorMessage = "*")]
		public string Title { get; set; }
		[Required(ErrorMessage = "*")]
		public int? GenreId { get; set; }
		[AllowHtml]
		public string Description { get; set; }
	}
}