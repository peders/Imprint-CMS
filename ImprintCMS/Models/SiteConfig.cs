using System.Web;
using System.Configuration;

namespace ImprintCMS.Models
{
    public class SiteConfig
    {
        public string Name { get; private set; }
        public HtmlString Footer { get; private set; }
        public decimal DistributionCostAmount { get; private set; }
        public decimal DistributionCostLimit { get; private set; }
        public string EmailSenderAddress { get; private set; }
        public string EmailSenderName { get; private set; }
        public string GoogleAnalyticsTrackingCode { get; set; }
        public string PersonImageDownloadNotice { get; set; }
        public string CoverImageDownloadNotice { get; set; }

        public SiteConfig()
        {
            Name = GetConfigValue<string>("ImprintCMSSiteName");
            Footer = new HtmlString(GetConfigValue<string>("ImprintCMSSiteFooter"));
            DistributionCostAmount = GetConfigValue<decimal>("ImprintCMSDistributionCostAmount");
            DistributionCostLimit = GetConfigValue<decimal>("ImprintCMSDistributionCostLimit");
            EmailSenderAddress = GetConfigValue<string>("ImprintCMSEmailSenderAddress");
            EmailSenderName = GetConfigValue<string>("ImprintCMSEmailSenderName");
            GoogleAnalyticsTrackingCode = GetConfigValue<string>("ImprintCMSGoogleAnalyticsTrackingCode");
            PersonImageDownloadNotice = GetConfigValue<string>("ImprintCMSPersonImageDownloadNotice");
            CoverImageDownloadNotice = GetConfigValue<string>("ImprintCMSCoverImageDownloadNotice");
        }

        private T GetConfigValue<T>(string name)
        {
            var reader = new AppSettingsReader();
            return (T)reader.GetValue(name, typeof(T));
        }
    }
}