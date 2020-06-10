using System.ComponentModel.DataAnnotations;

namespace ImprintCMS.Models
{

    [MetadataType(typeof(PersonImageMetadata))]
    public partial class PersonImage
    {
    }

    public class PersonImageMetadata
    {
        [Required(ErrorMessage = "*")]
        public int LargeImageId { get; set; }
    }

}