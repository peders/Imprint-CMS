using System.ComponentModel.DataAnnotations;

namespace ImprintCMS.Models
{
	[MetadataType(typeof(BookToArticleMetadata))]
	public partial class BookToArticle
	{
	}

	public class BookToArticleMetadata
	{
		[Required(ErrorMessage = "*")]
		public int BookId { get; set; }
	}

}