using System.ComponentModel.DataAnnotations;

namespace ImprintCMS.Models
{

    [MetadataType(typeof(ArticleGroupMetadata))]
	public partial class ArticleGroup
	{
	}

	public class ArticleGroupMetadata
	{
		[Required(ErrorMessage = "*")]
		public string Title { get; set; }
	}
}