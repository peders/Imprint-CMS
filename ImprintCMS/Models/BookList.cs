using System.ComponentModel.DataAnnotations;

namespace ImprintCMS.Models
{
	[MetadataType(typeof(BookListMetadata))]
	public partial class BookList
	{
	}

	public class BookListMetadata
	{
		[Required(ErrorMessage = "*")]
		public string Title { get; set; }
	}
}