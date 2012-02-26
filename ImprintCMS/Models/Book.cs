using System.ComponentModel.DataAnnotations;
using System;

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
	}

	public class BookMetadata
	{
		[Required(ErrorMessage = "*")]
		public string Title { get; set; }
		[Required(ErrorMessage = "*")]
		public int? GenreId { get; set; }
	}
}