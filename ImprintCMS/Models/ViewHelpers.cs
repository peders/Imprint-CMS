﻿using System;
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

        public static HtmlString PersonImage(this HtmlHelper helper, Person person)
        {
            if (person.SmallImageId == null) return null;
            var legend = helper.Encode(String.Format(SitePhrases.LabelAuthorImage, person.FullName));
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var source = urlHelper.Action("display", "upload", new { category = person.UploadedFile.Category, fileName = person.UploadedFile.FileName });
            return new HtmlString("<img src=\"" + source + "\" class=\"person\" alt=\"" + legend + "\" title=\"" + legend + "\" />");
        }

        public static HtmlString CoverImage(this HtmlHelper helper, Edition edition)
        {
            if (edition.SmallCoverId == null) return null;
            var legend = helper.Encode(String.Format(SitePhrases.LabelCoverImage, edition.Book.Title));
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var source = urlHelper.Action("display", "upload", new { category = edition.UploadedFile.Category, fileName = edition.UploadedFile.FileName });
            return new HtmlString("<img src=\"" + source + "\" class=\"cover\" alt=\"" + legend + "\" title=\"" + legend + "\" />");
        }

        public static HtmlString ImageDownload(this HtmlHelper helper, string url, string label, string notice)
        {
            if (String.IsNullOrWhiteSpace(url)) return null;
            if (String.IsNullOrWhiteSpace(notice)) return new HtmlString("<a href=\"" + url + "\" class=\"imagedownload\">" + helper.Encode(label) + "</a>");
            return new HtmlString("<a href=\"" + url + "\" class=\"imagedownload withnotice\" data-notice=\"" + notice + "\" data-acceptlabel=\"" + SitePhrases.LabelProceedToDownload + "\" data-cancellabel=\"" + SitePhrases.LabelCancel + "\">" + helper.Encode(label) + "</a>");
        }

        public static HtmlString DeleteConfirmation(this HtmlHelper helper, string name)
        {
            return new HtmlString(Phrases.LabelDeleteConfirmationPrefix + " <em>" + helper.Encode(name) + "</em>" + Phrases.LabelDeleteConfirmationSuffix);
        }

        public static HtmlString RemoveConfirmation(this HtmlHelper helper, string name, string container)
        {
            return new HtmlString(Phrases.LabelRemoveConfirmationPrefix + " <em>" + helper.Encode(name) + "</em> " + Phrases.LabelRemoveConfirmationMiddlePart + " <em>" + helper.Encode(container) + "</em>" + Phrases.LabelRemoveConfirmationSuffix);
        }

        public static HtmlString DeleteDependencies(this HtmlHelper helper, string name)
        {
            return new HtmlString(Phrases.LabelDeleteDependenciesPrefix + " <em>" + helper.Encode(name) + "</em>" + Phrases.LabelDeleteDependenciesSuffix);
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

        public static HtmlString GoogleAnalytics(this HtmlHelper helper, string trackingCode)
        {
            if (String.IsNullOrWhiteSpace(trackingCode)) return null;
            return new HtmlString("\n<script type=\"text/javascript\">\n\tvar gaJsHost = ((\"https:\" == document.location.protocol) ? \"https://ssl.\" : \"http://www.\");\n\tdocument.write(unescape(\"%3Cscript src='\" + gaJsHost + \"google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E\"));\n</script>\n<script type=\"text/javascript\">\n\ttry {\n\t\tvar pageTracker = _gat._getTracker(\"" + trackingCode + "\");\n\t\tpageTracker._trackPageview();\n\t} catch(err) {}\n</script>");
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