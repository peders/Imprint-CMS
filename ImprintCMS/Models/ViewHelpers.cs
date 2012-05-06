using System;
using System.Web.Mvc;
using System.Web;

namespace ImprintCMS.Models
{
	public static class ViewHelpers
	{

		public static string ControllerName(this ViewContext source)
		{
			var name = source.RouteData.Values["controller"] as string;
			if (String.IsNullOrWhiteSpace(name)) return null;
			return name;
		}

		public static string ActionName(this ViewContext source)
		{
			var name = source.RouteData.Values["action"] as string;
			if (String.IsNullOrWhiteSpace(name)) return null;
			return name;
		}

		public static HtmlString ToRichText(this string source)
		{
			source = source.Replace("[score]", "<span class=\"score level").Replace("[/score]", "\">[score]</span>");
			return new HtmlString(source);
		}

		public static string ToBookListDate(this DateTime source)
		{
			if (source <= DateTime.Today) return SitePhrases.LabelIsReleased;
			var formatString = SitePhrases.LabelToBeReleased + " {0} " + source.ToString("MMMM");
			if (source.Day < 11) return String.Format(formatString, SitePhrases.UnitMonthPremio);
			if (source.Day < 21) return String.Format(formatString, SitePhrases.UnitMonthMedio);
			return String.Format(formatString, SitePhrases.UnitMonthUltimo);
		}

		public static HtmlString Twitter(this HtmlHelper helper)
		{
			return new HtmlString("<a href=\"http://twitter.com/share\" class=\"twitter\">" + Phrases.LabelShareOnTwitter + "</a>");
		}

		public static HtmlString Facebook(this HtmlHelper helper)
		{
			var url = helper.ViewContext.RequestContext.HttpContext.Request.Url;
			return new HtmlString("<iframe src=\"http://www.facebook.com/plugins/like.php?href=" + url + "&amp;send=false&amp;layout=button_count&amp;width=450&amp;show_faces=false&amp;action=like&amp;colorscheme=light&amp;font&amp;height=21\" scrolling=\"no\" frameborder=\"0\" style=\"border:none; overflow:hidden; width:450px; height:21px;\" allowTransparency=\"true\"></iframe>");
		}

		public static HtmlString SearchForm(this HtmlHelper helper, string action, string query)
		{
			return new HtmlString(String.Format(
				@"<form method=""get"" action=""{0}"" id=""searchform"">
<fieldset>
	<p>
		<input type=""text"" name=""q"" id=""query"" value=""{1}"" placeholder=""{2}"" />
		<input type=""submit"" id=""button"" value=""{3}"" /></p>
</fieldset>
</form>", action, query, SitePhrases.LabelSearchPlaceholder, SitePhrases.LabelSearch));
		}

	}

}