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

		public bool HasExternalPublisher
		{
			get
			{
				return !String.IsNullOrWhiteSpace(ExternalPublisher);
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