using System.ComponentModel.DataAnnotations;

namespace ImprintCMS.Models
{
	[MetadataType(typeof(BindingMetadata))]
	public partial class Binding
	{
	}

	public class BindingMetadata
	{
		[Required(ErrorMessage = "*")]
		public string Name { get; set; }

	}
}