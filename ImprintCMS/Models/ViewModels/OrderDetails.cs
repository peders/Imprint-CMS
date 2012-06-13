using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ImprintCMS.Models.ViewModels
{
    public class OrderDetails
    {
        [Required(ErrorMessageResourceType = typeof(SitePhrases), ErrorMessageResourceName = "ValidationRequired")]
        public string Name { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessageResourceType = typeof(SitePhrases), ErrorMessageResourceName = "ValidationRequired")]
        [RegularExpression("[a-z0-9!#$%&\'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&\'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessageResourceType = typeof(SitePhrases), ErrorMessageResourceName = "ValidationInvalidEmailFormat")]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessageResourceType = typeof(SitePhrases), ErrorMessageResourceName = "ValidationRequired")]
        public string City { get; set; }
        [Required(ErrorMessageResourceType = typeof(SitePhrases), ErrorMessageResourceName = "ValidationRequired")]
        public string Postcode { get; set; }
    }
}