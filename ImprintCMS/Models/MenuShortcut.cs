using System.ComponentModel.DataAnnotations;

namespace ImprintCMS.Models
{
    [MetadataType(typeof(MenuShortcutMetadata))]
    public partial class MenuShortcut
    {
    }

    public class MenuShortcutMetadata
    {
        [Required(ErrorMessageResourceType = typeof(Phrases), ErrorMessageResourceName = "ValidationRequiredDefault")]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Phrases), ErrorMessageResourceName = "ValidationRequiredDefault")]
        [StringLength(50, ErrorMessageResourceType = typeof(Phrases), ErrorMessageResourceName = "ValidationStringLength50")]
        public string Href { get; set; }
    }
}