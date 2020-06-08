using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImprintCMS.Models
{
    [MetadataType(typeof(ConfigurationMetadata))]
    public partial class Configuration
    {
    }

    public class ConfigurationMetadata
    {
        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessageResourceName = "ValidationStringLength50", ErrorMessageResourceType = typeof(Phrases))]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessageResourceName = "ValidationStringLength50", ErrorMessageResourceType = typeof(Phrases))]
        public string SiteName { get; set; }
        [Required(ErrorMessage = "*")]
        [AllowHtml]
        public string SiteFooter { get; set; }
        [StringLength(50, ErrorMessageResourceName = "ValidationStringLength50", ErrorMessageResourceType = typeof(Phrases))]
        public string GoogleAnalyticsTrackingCode { get; set; }

        public string PersonImageDownloadNotice { get; set; }
        public string CoverImageDownloadNotice { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessageResourceName = "ValidationStringLength50", ErrorMessageResourceType = typeof(Phrases))]
        public string ShopEmailRecipient { get; set; }
        [AllowHtml]
        public string ShopDisclaimer { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessageResourceName = "ValidationStringLength50", ErrorMessageResourceType = typeof(Phrases))]
        public string EmailSenderAddress { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessageResourceName = "ValidationStringLength50", ErrorMessageResourceType = typeof(Phrases))]
        public string EmailSenderName { get; set; }
        [Required(ErrorMessage = "*")]
        public decimal DistributionCostAmount { get; set; }
        [Required(ErrorMessage = "*")]
        public decimal DistributionCostLimit { get; set; }
        public bool ShopIsVisible { get; set; }
        [Required(ErrorMessage = "*")]
        public int CachedCoverWidth { get; set; }
        [Required(ErrorMessage = "*")]
        public int CachedPortraitWidth { get; set; }


    }
}