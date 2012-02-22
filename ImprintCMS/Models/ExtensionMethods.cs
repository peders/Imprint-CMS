using System.Text.RegularExpressions;
using System.Web;

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
	}

}