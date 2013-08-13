using System.ComponentModel.DataAnnotations;

namespace ImprintCMS.Models
{

    [MetadataType(typeof(ExternalStoreMetadata))]
    public partial class ExternalStore
    {
    }

    public class ExternalStoreMetadata
    {
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*")]
        public string UrlPrefix { get; set; }
    }

}