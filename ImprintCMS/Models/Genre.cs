using System.ComponentModel.DataAnnotations;

namespace ImprintCMS.Models
{
	[MetadataType(typeof(GenreMetadata))]
	public partial class Genre
	{
	}

	public class GenreMetadata
	{
        [Required(ErrorMessageResourceType = typeof(Phrases), ErrorMessageResourceName = "ValidationRequiredDefault")]
        public string Name { get; set; }
	}
}