using ImprintCMS.Models;
using System.Linq;

namespace ImprintCMS.Controllers
{
    public class SiteControllerBase : ControllerBase
    {

        protected SiteConfig Config { get; private set; }

        public SiteControllerBase()
        {
            Config = new SiteConfig(Repository);
            ViewBag.Config = Config;
            ViewBag.ShowShop = Config.ShopIsVisible;
            ViewBag.MenuShortcuts = Repository.MenuShortcuts.OrderBy(_ => _.SequenceIdentifier).ToList();
            ViewBag.StylesheetUrls = Repository.Stylesheets.Select(u => Url.Action("display", "upload", new { category = u.Category, fileName = u.FileName }));
            ViewBag.LegacyStylesheetUrls = Repository.LegacyStylesheets.Select(u => Url.Action("display", "upload", new { category = u.Category, fileName = u.FileName }));
            ViewBag.ScriptUrls = Repository.Scripts.Select(u => Url.Action("display", "upload", new { category = u.Category, fileName = u.FileName }));
        }

    }
}
