using ImageProcessor;
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
        public ActionResult Serve(int id)
        {
            var vm = Repository.GetUploadedFile(id);
            if (vm == null) return HttpNotFound();
            return new FileContentResult(vm.Data.ToArray(), vm.ContentType);
        }

        [OutputCache(Duration = int.MaxValue, Location = OutputCacheLocation.Any, SqlDependency = "ImprintCMS:UploadedFile")]
        public ActionResult CachedCover(int id)
        {
            var fileNameBase = Repository.GetUploadFileName(id);
            if (string.IsNullOrWhiteSpace(fileNameBase)) return HttpNotFound();
            var siteConfig = new SiteConfig(Repository);
            var cover = Repository.GetUploadedFile(FileCategories.CachedCover.ToString(), string.Format("cache{0}_{1}", siteConfig.CachedCoverWidth, fileNameBase));
            if (cover != null) return new FileContentResult(cover.Data.ToArray(), cover.ContentType);
            var largeCover = Repository.GetUploadedFile(id);
            if (largeCover == null) return HttpNotFound();
            if (largeCover.Category != FileCategories.LargeCover.ToString()) return HttpNotFound();
            var resizedData = ResizeToWidth(largeCover.Data.ToArray(), siteConfig.CachedCoverWidth);
            var resizedCover = new UploadedFile
            {
                FileName = string.Format("cache{0}_{1}", siteConfig.CachedCoverWidth, largeCover.FileName),
                ContentType = "image/jpeg",
                ContentLength = resizedData.Length,
                Category = FileCategories.CachedCover.ToString(),
                Data = resizedData
            };
            Repository.Add(resizedCover);
            Repository.Save();
            return new FileContentResult(resizedData, resizedCover.ContentType);
        }

        [OutputCache(Duration = int.MaxValue, Location = OutputCacheLocation.Any, SqlDependency = "ImprintCMS:UploadedFile")]
        public ActionResult CachedPortrait(int id)
        {
            var fileNameBase = Repository.GetUploadFileName(id);
            if (string.IsNullOrWhiteSpace(fileNameBase)) return HttpNotFound();
            var siteConfig = new SiteConfig(Repository);
            var portrait = Repository.GetUploadedFile(FileCategories.CachedPortrait.ToString(), string.Format("cache{0}_{1}", siteConfig.CachedPortraitWidth, fileNameBase));
            if (portrait != null) return new FileContentResult(portrait.Data.ToArray(), portrait.ContentType);
            var largePortrait = Repository.GetUploadedFile(id);
            if (largePortrait == null) return HttpNotFound();
            if (largePortrait.Category != FileCategories.LargePortrait.ToString()) return HttpNotFound();
            var resizedData = ResizeToWidth(largePortrait.Data.ToArray(), siteConfig.CachedPortraitWidth);
            var resizedPortrait = new UploadedFile
            {
                FileName = string.Format("cache{0}_{1}", siteConfig.CachedPortraitWidth, largePortrait.FileName),
                ContentType = "image/jpeg",
                ContentLength = resizedData.Length,
                Category = FileCategories.CachedPortrait.ToString(),
                Data = resizedData
            };
            Repository.Add(resizedPortrait);
            Repository.Save();
            return new FileContentResult(resizedData, resizedPortrait.ContentType);
        }

        [OutputCache(Duration = int.MaxValue, Location = OutputCacheLocation.Any, SqlDependency = "ImprintCMS:UploadedFile")]
        public ActionResult CachedThumbnail(int id)
        {
            var fileNameBase = Repository.GetUploadFileName(id);
            if (string.IsNullOrWhiteSpace(fileNameBase)) return HttpNotFound();
            var siteConfig = new SiteConfig(Repository);
            var thumbnail = Repository.GetUploadedFile(FileCategories.CachedThumbnail.ToString(), string.Format("cache{0}_{1}", siteConfig.CachedPortraitWidth, fileNameBase));
            if (thumbnail != null) return new FileContentResult(thumbnail.Data.ToArray(), thumbnail.ContentType);
            var largePortrait = Repository.GetUploadedFile(id);
            if (largePortrait == null) return HttpNotFound();
            if (largePortrait.Category != FileCategories.LargePortrait.ToString()) return HttpNotFound();
            var thumbData = CropToSquare(largePortrait.Data.ToArray(), siteConfig.CachedPortraitWidth);
            var thumbPortrait = new UploadedFile
            {
                FileName = string.Format("cache{0}_{1}", siteConfig.CachedPortraitWidth, largePortrait.FileName),
                ContentType = "image/jpeg",
                ContentLength = thumbData.Length,
                Category = FileCategories.CachedThumbnail.ToString(),
                Data = thumbData
            };
            Repository.Add(thumbPortrait);
            Repository.Save();
            return new FileContentResult(thumbData, thumbPortrait.ContentType);
        }

        private byte[] ResizeToWidth(byte[] original, int width)
        {
            byte[] outputBytes;
            using (var inStream = new MemoryStream(original))
            {
                using (var outStream = new MemoryStream())
                {
                    using (var imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        imageFactory.Load(inStream)
                            .Resize(new Size { Width = width, Height = 0 })
                            .Format(ImageFormat.Jpeg)
                            .Quality(100)
                            .Save(outStream);
                    }
                    outputBytes = outStream.ToArray();
                }
            }
            return outputBytes;
        }

        private byte[] CropToSquare(byte[] original, int side)
        {
            byte[] outputBytes;
            using (var inStream = new MemoryStream(original))
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
            return outputBytes;
        }

    }

}
