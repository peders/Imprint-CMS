using System.Web.Mvc;
using ImprintCMS.Models;

namespace ImprintCMS.Controllers
{
	public class ControllerBase : Controller
	{

		protected Repository Repository { get; private set; }

		public ControllerBase()
		{
			Repository = new Repository();
		}

	}

}
