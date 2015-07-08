using System.ComponentModel.DataAnnotations;

namespace ImprintCMS.Models
{

	[MetadataType(typeof(BookListMembershipMetadata))]
	public partial class BookListMembership
	{
	}

	public class BookListMembershipMetadata
	{
		[Required(ErrorMessage = "*")]
		public int EditionId { get; set; }
	}

}
