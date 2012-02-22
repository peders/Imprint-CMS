using System.Web.Mvc;
using ImprintCMS.Models;
using System.Web.UI;

namespace ImprintCMS.Controllers
{

	public class UploadController : ControllerBase
	{
		[OutputCache(Duration = 60, Location=OutputCacheLocation.Any)]
		public ActionResult Display(string category, string fileName)
		{
			var vm = Repository.GetUploadedFile(category, fileName);
			if (vm == null) return HttpNotFound();
			return new FileContentResult(vm.Data.ToArray(), vm.ContentType);
		}

	}

}
