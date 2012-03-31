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
			source = source.Replace("[i]", "<em>").Replace("[/i]", "</em>");
			source = source.Replace("[b]", "<strong>").Replace("[/b]", "</strong>");
			source = source.Replace("\n", "<br />");
			return new HtmlString(source);
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

	}
}