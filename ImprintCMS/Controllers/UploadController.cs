using ImageProcessor;
using ImageProcessor.Imaging;
using ImprintCMS.Models;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using System.Web.UI;

namespace ImprintCMS.Controllers
{

    public class UploadController : ControllerBase
    {

        [OutputCache(Duration = int.MaxValue, Location = OutputCacheLocation.Any, SqlDependency = "ImprintCMS:UploadedFile")]
        public ActionResult Display(string category, string fileName)
        {
            var vm = Repository.GetUploadedFile(category, fileName);
            if (vm == null) return HttpNotFound();
            return new FileContentResult(vm.Data.ToArray(), vm.ContentType);
        }

        [OutputCache(Duration = int.MaxValue, Location = OutputCacheLocation.Any, SqlDependency = "ImprintCMS:UploadedFile")]
        public ActionResult Thumbnail(string fileName, int side)
        {
            var vm = Repository.GetUploadedFile("SmallPortrait", fileName);
            if (vm == null) return HttpNotFound();
            if (vm.ContentType != "image/jpeg") return HttpNotFound();
            byte[] outputBytes;
            using (var inStream = new MemoryStream(vm.Data.ToArray()))
            {
                using (var outStream = new MemoryStream())
                {
                    using (var imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        imageFactory.Load(inStream)
                            .Crop(imageFactory.Image.CenterCropSquare())
                            .Constrain(new Size(side, side))
                            .Format(ImageFormat.Jpeg)
                            .Quality(100)
                            .Save(outStream);
                    }
                    outputBytes = outStream.ToArray();
                }
            }
            return new FileContentResult(outputBytes, vm.ContentType);
        }

    }

}
