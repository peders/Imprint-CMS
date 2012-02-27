using System.Web.Mvc;
using ImprintCMS.Models;
using System.Threading;
using System.Globalization;

namespace ImprintCMS.Controllers
{
	public class ControllerBase : Controller
	{

		public Repository Repository { get; set; }

		public ControllerBase()
		{
			Repository = new Repository();
		}

	}

}
