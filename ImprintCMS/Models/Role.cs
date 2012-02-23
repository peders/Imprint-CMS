using System.ComponentModel.DataAnnotations;

namespace ImprintCMS.Models
{
	[MetadataType(typeof(RoleMetadata))]
	public partial class Role
	{
	}

	public class RoleMetadata
	{
		[Required(ErrorMessage = "*")]
		public string Name { get; set; }
		[Required(ErrorMessage = "*")]
		public string ShortName { get; set; }
	}
}