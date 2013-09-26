using System.ComponentModel.DataAnnotations;
using System;
using System.Web.Mvc;

namespace ImprintCMS.Models
{
    [MetadataType(typeof(EditionMetadata))]
    public partial class Edition
    {
        public string Name
        {
            get
            {
                return String.Format("{0} ({1} – {2})", Book.Title, Number, Binding.Name);
            }
        }
    }

    public class EditionMetadata
    {
        [Required(ErrorMessageResourceType = typeof(Phrases), ErrorMessageResourceName = "ValidationRequiredDefault")]
        public int BookId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Phrases), ErrorMessageResourceName = "ValidationRequiredDefault")]
        public int BindingId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Phrases), ErrorMessageResourceName = "ValidationRequiredDefault")]
        public DateTime ReleaseDate { get; set; }
        [Required(ErrorMessageResourceType = typeof(Phrases), ErrorMessageResourceName = "ValidationRequiredDefault")]
        public int Number { get; set; }
        [Required(ErrorMessageResourceType = typeof(Phrases), ErrorMessageResourceName = "ValidationRequiredDefault")]
        public string Isbn { get; set; }
        [AllowHtml]
        public string Blurb { get; set; }
        [StringLength(50, ErrorMessageResourceType = typeof(Phrases), ErrorMessageResourceName = "ValidationStringLength50")]
        public string AlternativeNotForSaleMessage { get; set; }
    }
}