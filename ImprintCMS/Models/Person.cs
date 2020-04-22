using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        public PersonImage MainImage
        {
            get
            {
                if (!PersonImages.Any()) return null;
                return PersonImages.OrderBy(_ => _.SequenceIdentifier).First();
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
        [AllowHtml]
        public string MiscInfo { get; set; }
    }
}