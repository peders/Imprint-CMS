using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using ImprintCMS.Models.ViewModels;

namespace ImprintCMS.Models
{

    public static class ExtensionMethods
    {
        public static string Sanitise(this string source)
        {
            var regex = new Regex(@"[^\w.]");
            return regex.Replace(source, "_");
        }

        public static HtmlString ToHtmlString(this string source)
        {
            return new HtmlString(source);
        }

        public static string ToFileSize(this int source)
        {
            if (source >= 1024 * 1024)
                return String.Format("{0:f1} {1}", (float)source / 1024 / 1024, Phrases.UnitMegaBytesShort);
            if (source >= 1024)
                return String.Format("{0} {1}", source / 1024, Phrases.UnitKiloBytesShort);
            if (source == 1)
                return String.Format("{0} {1}", source, Phrases.UnitBytesSingular);
            return String.Format("{0} {1}", source, Phrases.UnitBytesPlural);
        }

        public static string AsIsbn(this string source)
        {
            var clean = source.Replace("-", string.Empty).Trim();
            if (clean.Length == 13) return String.Format("{0}-{1}-{2}-{3}-{4}", clean.Substring(0, 3), clean.Substring(3, 2), clean.Substring(5, 2), clean.Substring(7, 5), clean.Substring(12, 1));
            if (clean.Length == 10) return String.Format("{0}-{1}-{2}-{3}", clean.Substring(0, 2), clean.Substring(2, 4), clean.Substring(4, 5), clean.Substring(9, 1));
            return clean;
        }

        public static string AsFileType(this string source)
        {
            if (source == "image/jpeg") return Phrases.FileTypeImage;
            if (source == "image/png") return Phrases.FileTypeImage;
            if (source == "application/pdf") return Phrases.FileTypePDF;
            if (source == "application/msword") return Phrases.FileTypeWordDocument;
            if (source == "application/x-javascript") return Phrases.FileTypeJavascript;
            if (source == "text/css") return Phrases.FileTypeCss;
            return source;
        }

        public static string UrlBase(this HttpRequestBase request)
        {
            var url = "http";
            if (request.IsSecureConnection) url += "s";
            url += "://" + request.ServerVariables["SERVER_NAME"];
            if (request.ServerVariables["SERVER_PORT"] != "80") url += ":" + request.ServerVariables["SERVER_PORT"];
            return url;
        }

        public static OpenGraph OpenGraph(this Person person, string siteName, RequestContext context)
        {
            if (person.UploadedFile == null) return null;
            var urlHelper = new UrlHelper(context);
            var imageUrl = context.HttpContext.Request.UrlBase() + urlHelper.Action("display", "upload", new { category = person.UploadedFile.Category, fileName = person.UploadedFile.FileName });
            var pageUrl = context.HttpContext.Request.UrlBase() + urlHelper.Action("details", "authors", new { id = person.Id });
            return new OpenGraph
            {
                Title = person.FullName,
                Type = "article",
                ImageUrl = imageUrl,
                Url = pageUrl,
                SiteName = siteName
            };
        }

        public static OpenGraph OpenGraph(this Book book, string siteName, RequestContext context)
        {
            if (book.HasExternalPublisher) return null;
            if (!book.Editions.Any()) return null;
            if (book.Editions.All(e => e.SmallCoverId == null)) return null;
            var coverEdition = book.Editions.Where(e => e.SmallCoverId != null).OrderBy(e => e.Number).Last();
            var urlHelper = new UrlHelper(context);
            var imageUrl = context.HttpContext.Request.UrlBase() + urlHelper.Action("display", "upload", new { category = coverEdition.UploadedFile.Category, fileName = coverEdition.UploadedFile.FileName });
            var pageUrl = context.HttpContext.Request.UrlBase() + urlHelper.Action("details", "books", new { id = book.Id });
            return new OpenGraph
            {
                Title = book.Title,
                Type = "article",
                ImageUrl = imageUrl,
                Url = pageUrl,
                SiteName = siteName
            };
        }

    }

}