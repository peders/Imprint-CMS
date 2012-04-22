using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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

		public string FullName
		{
			get
			{
				return !string.IsNullOrWhiteSpace(FirstName) ? FirstName + " " + LastName : LastName;
			}
		}
	}

	public class PersonMetadata
	{
		[Required(ErrorMessage = "*")]
		public string LastName { get; set; }
		[AllowHtml]
		public string Description { get; set; }
		[AllowHtml]
		public string Awards { get; set; }
		[AllowHtml]
		public string Translations { get; set; }
		[AllowHtml]
		public string Anthologies { get; set; }
	}
}