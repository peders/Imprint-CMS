using System.ComponentModel.DataAnnotations;

namespace ImprintCMS.Models
{
    [MetadataType(typeof(RoleMetadata))]
    public partial class Role
    {
    }

    public class RoleMetadata
    {
        [Required(ErrorMessageResourceType = typeof(Phrases), ErrorMessageResourceName = "ValidationRequiredDefault")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Phrases), ErrorMessageResourceName = "ValidationStringLength50")]
        public string BookListHeading { get; set; }
    }
}