using System.Linq;
using System.Web;

namespace ImprintCMS.Models
{
    public class SiteConfig
    {
        public string Name { get; private set; }
        public HtmlString Footer { get; private set; }
        public string GoogleAnalyticsTrackingCode { get; private set; }
        public string PersonImageDownloadNotice { get; private set; }
        public string CoverImageDownloadNotice { get; private set; }
        public int CachedCoverWidth { get; private set; }
        public int CachedPortraitWidth { get; private set; }

        public SiteConfig(Repository repository)
        {
            var config = repository.Configurations.SingleOrDefault(_ => _.IsActive);
            if (config != null)
            {
                Name = config.SiteName;
                Footer = new HtmlString(config.SiteFooter);
                GoogleAnalyticsTrackingCode = config.GoogleAnalyticsTrackingCode;
                PersonImageDownloadNotice = config.PersonImageDownloadNotice;
                CoverImageDownloadNotice = config.CoverImageDownloadNotice;
                CachedCoverWidth = config.CachedCoverWidth;
                CachedPortraitWidth = config.CachedPortraitWidth;
            }
            else
            {
                Name = "Imprint CMS";
                Footer = new HtmlString("<p>Powered by <em>Imprint CMS</em></p>");
                GoogleAnalyticsTrackingCode = string.Empty;
                PersonImageDownloadNotice = string.Empty;
                CoverImageDownloadNotice = string.Empty;
                CachedCoverWidth = 250;
                CachedPortraitWidth = 250;
            }
        }

    }

}