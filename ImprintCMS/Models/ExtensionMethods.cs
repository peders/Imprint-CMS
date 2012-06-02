using System.Text.RegularExpressions;
using System.Web;
using System;
using System.Web.Mvc;

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

    }

}