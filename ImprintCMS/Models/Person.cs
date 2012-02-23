using System.ComponentModel.DataAnnotations;

namespace ImprintCMS.Models
{
	[MetadataType(typeof(PersonMetadata))]
	public partial class Person
	{
	}

	public class PersonMetadata
	{
		[Required(ErrorMessage = "*")]
		public string LastName { get; set; }
	}
}