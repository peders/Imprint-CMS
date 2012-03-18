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

		public static string AsFileType(this string source)
		{
			if (source == "image/jpeg") return Phrases.FileTypeImage;
			if (source == "application/pdf") return Phrases.FileTypePDF;
			return source;
		}

		public static string ControllerName(this ViewContext source)
		{
			var name = source.RouteData.Values["controller"] as string;
			if (String.IsNullOrWhiteSpace(name)) return null;
			return name;
		}


	}

}