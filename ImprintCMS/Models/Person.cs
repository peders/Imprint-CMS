using System.ComponentModel.DataAnnotations;

namespace ImprintCMS.Models
{
	[MetadataType(typeof(PersonMetadata))]
	public partial class Person
	{
		public string ReverseName
		{
			get
			{
				return !string.IsNullOrWhiteSpace(FirstName) ? LastName + ", " + FirstName : LastName;
			}
		}
	}

	public class PersonMetadata
	{
		[Required(ErrorMessage = "*")]
		public string LastName { get; set; }
	}
}