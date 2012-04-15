using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ImprintCMS.Models
{

	[MetadataType(typeof(ArticleMetadata))]
	public partial class Article
	{
	}

	public class ArticleMetadata
	{
		[Required(ErrorMessage = "*")]
		[StringLength(50, ErrorMessageResourceName = "ValidationStringLength50", ErrorMessageResourceType = typeof(Phrases))]
		public string Title { get; set; }
		[Required(ErrorMessage = "*")]
		[AllowHtml]
		public string Description { get; set; }
	}
}