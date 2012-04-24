using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ImprintCMS.Models
{

	[MetadataType(typeof(ContactArticleMetadata))]
	public partial class ContactArticle
	{
	}

	public class ContactArticleMetadata
	{
		[Required(ErrorMessage = "*")]
		public string Title { get; set; }
		[AllowHtml]
		public string BodyText { get; set; }
	}
}