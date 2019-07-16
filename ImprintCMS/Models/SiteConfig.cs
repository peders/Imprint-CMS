using System.Web;
using System.Configuration;
using System.Linq;

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

        public SiteConfig(Repository repository)
        {
            var config = repository.Configurations.SingleOrDefault(_ => _.IsActive);
            if (config != null)
            {
                Name = config.SiteName;
                Footer = new HtmlString(config.SiteFooter);
                DistributionCostAmount = config.DistributionCostAmount;
                DistributionCostLimit = config.DistributionCostLimit;
                EmailSenderAddress = config.EmailSenderAddress;
                EmailSenderName = config.EmailSenderName;
                GoogleAnalyticsTrackingCode = config.GoogleAnalyticsTrackingCode;
                PersonImageDownloadNotice = config.PersonImageDownloadNotice;
                CoverImageDownloadNotice = config.CoverImageDownloadNotice;
                ShopIsVisible = config.ShopIsVisible;
                ShopEmailRecipient = config.ShopEmailRecipient;
                ShopDisclaimer = new HtmlString(config.ShopDisclaimer);
            }
            else
            {
                Name = "Imprint CMS";
                Footer = new HtmlString("<p>Powered by <em>Imprint CMS</em></p>");
                DistributionCostAmount = 50;
                DistributionCostLimit = 500;
                EmailSenderAddress = "address@server.com";
                EmailSenderName = "Imprint CMS";
                GoogleAnalyticsTrackingCode = string.Empty;
                PersonImageDownloadNotice = string.Empty;
                CoverImageDownloadNotice = string.Empty;
                ShopIsVisible = true;
                ShopEmailRecipient = "address@server.com";
                ShopDisclaimer = new HtmlString(string.Empty);

            }
        }

    }

}