using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace ImprintCMS.Models
{

	[MetadataType(typeof(ArticleMetadata))]
	public partial class Article
	{
		public Person ImagePerson
		{
			get
			{
				var personToArticle = PersonToArticles.Where(p => p.Person.IsVisible && p.Person.HasPage && p.Person.SmallImageId != null).OrderBy(p => p.SequenceIdentifier).FirstOrDefault();
				return personToArticle == null ? null : personToArticle.Person;
			}
		}

		public Edition ImageEdition
		{
			get
			{
				var bookToArticle = BookToArticles.Where(b => b.Book.IsVisible && b.Book.Editions.Any(e => e.SmallCoverId != null)).OrderBy(b => b.SequenceIdentifier).FirstOrDefault();
				return bookToArticle == null ? null : bookToArticle.Book.Editions.Where(e => e.SmallCoverId != null).OrderBy(e => e.Number).Last();
			}
		}

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