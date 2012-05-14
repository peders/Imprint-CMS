using ImprintCMS.Models;

namespace ImprintCMS.Controllers
{
	public class SiteControllerBase : ControllerBase
	{

		protected SiteConfig Config { get; private set; }

		public SiteControllerBase()
		{
			Config = new SiteConfig();
			ViewBag.Config = Config;
		}

	}
}
