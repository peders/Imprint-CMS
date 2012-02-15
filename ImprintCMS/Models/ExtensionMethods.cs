using System.Text.RegularExpressions;

namespace ImprintCMS.Models
{

	public static class ExtensionMethods
	{
		public static string Sanitise(this string source)
		{
			var regex = new Regex(@"[^\w]");
			return regex.Replace(source, "_");
		}
	}

}