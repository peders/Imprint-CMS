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
        public bool ShopIsVisible { get; set; }
        public string ShopEmailRecipient { get; set; }
        public HtmlString ShopDisclaimer { get; set; }

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
            ShopIsVisible = GetConfigValue<bool>("ImprintCMSShopIsVisible");
            ShopEmailRecipient = GetConfigValue<string>("ImprintCMSShopEmailRecipient");
            ShopDisclaimer = new HtmlString(GetConfigValue<string>("ImprintCMSShopDisclaimer"));
        }

        private static T GetConfigValue<T>(string name)
        {
            var reader = new AppSettingsReader();
            return (T)reader.GetValue(name, typeof(T));
        }
    }
}