using System.ComponentModel.DataAnnotations;

namespace ImprintCMS.Models
{
	[MetadataType(typeof(PersonToArticleMetadata))]
	public partial class PersonToArticle
	{
	}

	public class PersonToArticleMetadata
	{
		[Required(ErrorMessage = "*")]
		public int PersonId { get; set; }
	}

}