﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ImprintCMS.Models
{
    public static class ViewHelpers
    {

        public static string ControllerName(this ViewContext source)
        {
            var name = source.RouteData.Values["controller"] as string;
            if (string.IsNullOrWhiteSpace(name)) return null;
            return name;
        }

        public static string ActionName(this ViewContext source)
        {
            var name = source.RouteData.Values["action"] as string;
            if (string.IsNullOrWhiteSpace(name)) return null;
            return name;
        }

        public static bool IsForController(this ViewContext context, string name)
        {
            return context.ControllerName().Equals(name, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsForAction(this ViewContext context, string name)
        {
            return context.ActionName().Equals(name, StringComparison.InvariantCultureIgnoreCase);
        }

        public static HtmlString ToRichText(this string source)
        {
            source = source.Replace("[score]", "<span class=\"score level").Replace("[/score]", "\">[score]</span>");
            return new HtmlString(source);
        }

        public static string ToBookListDate(this DateTime date)
        {
            if (date <= DateTime.Today) return SitePhrases.LabelIsReleased;
            var formatString = SitePhrases.LabelToBeReleased + " {0} " + date.ToString("MMMM").ToLowerInvariant();
            if (date.Day < 11) return string.Format(formatString, SitePhrases.UnitMonthPrimo);
            if (date.Day < 21) return string.Format(formatString, SitePhrases.UnitMonthMedio);
            return string.Format(formatString, SitePhrases.UnitMonthUltimo);
        }

        public static HtmlString PersonLinkCard(this HtmlHelper helper, Person person)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var buffer = "<a href=\"" + urlHelper.Action("details", "authors", new { id = person.Id }) + "\" class=\"linkcard person\">";
            buffer += "\n\t" + PersonThumbnail(helper, person);
            buffer += "\n\t<p class=\"name\">" + person.FullName + "</p>";
            buffer += "\n</a>";
            return new HtmlString(buffer);
        }

        public static HtmlString PersonLinkCardList(this HtmlHelper helper, IEnumerable<Person> people)
        {
            if (!people.Any()) return null;
            var buffer = "<ul class=\"linkcardlist people\">";
            foreach (var person in people)
            {
                buffer += "\n<li>";
                buffer += "\n" + PersonLinkCard(helper, person);
                buffer += "\n</li>";
            }
            buffer += "</ul>";
            return new HtmlString(buffer);
        }

        public static HtmlString PeopleAsGroupedCards(this HtmlHelper helper, IEnumerable<Person> people)
        {
            if (!people.Any()) return null;
            var buffer = "<ul class=\"linkcardlist people\">";
            foreach (var peopleUnderLetter in people.GroupBy(_ => _.LastName[0]))
            {
                buffer += "\n<li class=\"groupingkey\">" + peopleUnderLetter.Key + "</li>";
                foreach (var person in peopleUnderLetter)
                {
                    buffer += "\n<li>";
                    buffer += "\n" + PersonLinkCard(helper, person);
                    buffer += "\n</li>";
                }
            }
            buffer += "</ul>";
            return new HtmlString(buffer);
        }

        public static HtmlString EditionLinkCard(this HtmlHelper helper, Edition edition, bool showReleaseDate = false, bool showBlurb = false)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var buffer = "<a href=\"" + urlHelper.Action("details", "books", new { id = edition.BookId }) + "\" class=\"linkcard book\">";
            buffer += "\n\t" + CoverImage(helper, edition);
            buffer += "\n\t<p class=\"people\">" + edition.Book.RelationNames() + "</p>";
            buffer += "\n\t<p class=\"title\">" + edition.Book.Title + "</p>";
            if (!string.IsNullOrWhiteSpace(edition.Book.Subtitle))
            {
                buffer += "\n\t<p class=\"subtitle\">" + edition.Book.Subtitle + "</p>";
            }
            if (showReleaseDate)
            {
                buffer += "\n\t<p class=\"" + string.Format("date{0}", edition.ReleaseDate > DateTime.Today ? " tobereleased" : string.Empty) + "\">" + edition.ReleaseDate.ToBookListDate() + "</p>";
            }
            if (!string.IsNullOrWhiteSpace(edition.Blurb) && showBlurb)
            {
                buffer += "\n\t<p class=\"blurb\">" + edition.Blurb.ToRichText() + "</p>";
            }
            if (edition.Book.IsDebut)
            {
                buffer += "\n\t<p class=\"isdebut\">" + SitePhrases.LabelIsDebut + "</p>";
            }
            buffer += "\n</a>";
            return new HtmlString(buffer);
        }

        public static HtmlString EditionLinkCardList(this HtmlHelper helper, IEnumerable<Edition> editions, bool showReleaseDate = false, bool showBlurb = false)
        {
            if (!editions.Any()) return null;
            var buffer = "<ul class=\"linkcardlist editions\">";
            foreach (var edition in editions)
            {
                buffer += "\n<li>";
                buffer += "\n" + EditionLinkCard(helper, edition, showReleaseDate, showBlurb);
                buffer += "\n</li>";
            }
            buffer += "</ul>";
            return new HtmlString(buffer);
        }

        public static HtmlString BookListAsCards(this HtmlHelper helper, BookList list)
        {
            return EditionLinkCardList(helper, list.BookListMemberships.OrderBy(m => m.SequenceIdentifier).Where(m => m.Edition.LargeCoverId != null).Select(m => m.Edition), showReleaseDate: true, showBlurb: true);
        }

        public static HtmlString BooksAsCards(this HtmlHelper helper, IEnumerable<Book> books)
        {
            var editions = books.Where(_ => _.Editions.Any(e => e.LargeCoverId != null)).Select(_ => _.Editions.Where(e => e.LargeCoverId != null).OrderBy(e => e.Number).Last());
            return EditionLinkCardList(helper, editions, showReleaseDate: false, showBlurb: false);
        }

        public static HtmlString PersonThumbnail(this HtmlHelper helper, int ImageId, string name)
        {
            var legend = helper.Encode(string.Format(SitePhrases.LabelAuthorImage, name));
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var source = urlHelper.Action("cachedthumbnail", "upload", new { id = ImageId });
            return new HtmlString("<img src=\"" + source + "\" class=\"person thumbnail\" alt=\"" + legend + "\" title=\"" + legend + "\" />");
        }

        public static HtmlString PersonThumbnail(this HtmlHelper helper, Person person)
        {
            if (person.MainImage == null) return null;
            return PersonThumbnail(helper, person.MainImage.LargeImageId, person.FullName);
        }

        public static HtmlString PersonImage(this HtmlHelper helper, Person person)
        {
            if (person.MainImage == null) return null;
            var legend = helper.Encode(string.Format(SitePhrases.LabelAuthorImage, person.FullName));
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var source = urlHelper.Action("cachedportrait", "upload", new { id = person.MainImage.LargeImageId });
            return new HtmlString("<img src=\"" + source + "\" class=\"person\" alt=\"" + legend + "\" title=\"" + legend + "\" />");
        }

        public static HtmlString CoverImage(this HtmlHelper helper, Edition edition)
        {
            if (edition.LargeCoverId == null) return null;
            var legend = helper.Encode(string.Format(SitePhrases.LabelCoverImage, edition.Book.Title));
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var source = urlHelper.Action("cachedcover", "upload", new { id = edition.LargeCoverId });
            return new HtmlString("<img src=\"" + source + "\" class=\"cover\" alt=\"" + legend + "\" title=\"" + legend + "\" />");
        }

        public static HtmlString ArticleImage(this HtmlHelper helper, Article article)
        {
            if (article.ImageId == null) return null;
            var legend = helper.Encode(string.Format(SitePhrases.LabelArticleImage, article.Title));
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var source = urlHelper.Action("cachedarticleimage", "upload", new { id = article.ImageId });
            return new HtmlString("<img src=\"" + source + "\" class=\"article\" alt=\"" + legend + "\" title=\"" + legend + "\" />");
        }

        public static HtmlString ArticleChosenImage(this HtmlHelper helper, Article article)
        {
            if (article.ImageId != null) return ArticleImage(helper, article);
            if (article.ImagePerson != null) return PersonThumbnail(helper, article.ImagePerson);
            if (article.ImageEdition != null) return CoverImage(helper, article.ImageEdition);
            return null;
        }

        public static HtmlString ArticleChosenImageInLink(this HtmlHelper helper, Article article)
        {
            if (ArticleChosenImage(helper, article) != null)
            {
                var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
                return new HtmlString("<a href=\"" + urlHelper.Action("article", "home", new { id = article.Id }) + "\">" + ArticleChosenImage(helper, article) + "</a>");
            }
            return null;
        }

        public static HtmlString ArticleImageCredit(this HtmlHelper helper, Article article)
        {
            if (article.ImageId == null && article.ImagePerson != null)
            {
                if (!string.IsNullOrWhiteSpace(article.ImagePerson.MainImage.Photographer))
                {
                    return new HtmlString("<p class=\"photographer\">" + string.Format(SitePhrases.LabelPhotoCredit, article.ImagePerson.MainImage.Photographer) + "</p>");
                }
            }
            return null;
        }

        public static HtmlString ArticleLinks(this HtmlHelper helper, Article article)
        {
            var people = article.PersonToArticles.Where(p => p.Person.IsVisible && p.Person.HasPage).OrderBy(p => p.SequenceIdentifier).Select(p => p.Person);
            var books = article.BookToArticles.Where(b => b.Book.IsVisible).OrderBy(b => b.SequenceIdentifier).Select(b => b.Book);
            if (!people.Any() && !books.Any()) return null;
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var buffer = "<ul class=\"links\">";
            foreach (var person in people)
            {
                buffer += "\n\t<li>";
                buffer += "\n\t\t<a href=\"" + urlHelper.Action("details", "authors", new { id = person.Id }) + "\">" + person.FullName + "</a>";
                buffer += "\n\t</li>";
            }
            foreach (var book in books)
            {
                buffer += "\n\t<li>";
                buffer += "\n\t\t<a href=\"" + urlHelper.Action("details", "books", new { id = book.Id }) + "\">" + book.Title + "</a>";
                buffer += "\n\t</li>";
            }
            buffer += "\n</ul>";
            return new HtmlString(buffer);
        }

        public static string RelationNames(this Book book)
        {
            return string.Join(" / ", book.Relations.OrderBy(r => r.SequenceIdentifier).Select(r => r.PersonName));
        }

        public static HtmlString ImageDownload(this HtmlHelper helper, string url, string label, string notice)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;
            if (string.IsNullOrWhiteSpace(notice)) return new HtmlString("<a href=\"" + url + "\" class=\"imagedownload\">" + helper.Encode(label) + "</a>");
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

        public static HtmlString Twitter(this HtmlHelper helper, string message, string hashtags)
        {
            return new HtmlString("<a href=\"https://twitter.com/share\" class=\"twitter-share-button\" data-text=\"" + HttpUtility.HtmlEncode(message) + "\" data-lang=\"en\" data-hashtags=\"" + hashtags + "\">Tweet</a>\n<script>!function(d,s,id){var js,fjs=d.getElementsByTagName(s)[0];if(!d.getElementById(id)){js=d.createElement(s);js.id=id;js.src='//platform.twitter.com/widgets.js';fjs.parentNode.insertBefore(js,fjs);}}(document,'script','twitter-wjs');</script>");
        }

        public static HtmlString Facebook(this HtmlHelper helper)
        {
            var url = helper.ViewContext.RequestContext.HttpContext.Request.Url;
            return new HtmlString("<iframe src=\"https://www.facebook.com/plugins/like.php?href=" + url + "&amp;send=false&amp;layout=button_count&amp;width=450&amp;show_faces=false&amp;action=like&amp;colorscheme=light&amp;font&amp;height=21\" scrolling=\"no\" frameborder=\"0\" style=\"border:none; overflow:hidden; width:450px; height:21px;\" allowTransparency=\"true\"></iframe>");
        }

        public static HtmlString GoogleAnalytics(this HtmlHelper helper, string trackingCode)
        {
            if (string.IsNullOrWhiteSpace(trackingCode)) return null;
            return new HtmlString("\n<script type=\"text/javascript\">\n\tvar gaJsHost = ((\"https:\" == document.location.protocol) ? \"https://ssl.\" : \"http://www.\");\n\tdocument.write(unescape(\"%3Cscript src='\" + gaJsHost + \"google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E\"));\n</script>\n<script type=\"text/javascript\">\n\ttry {\n\t\tvar pageTracker = _gat._getTracker(\"" + trackingCode + "\");\n\t\tpageTracker._trackPageview();\n\t} catch(err) {}\n</script>");
        }

        public static HtmlString SearchForm(this HtmlHelper helper, string action, string query)
        {
            return new HtmlString(string.Format(
                @"<form method=""get"" action=""{0}"" id=""searchform"">
<fieldset>
	<p>
		<input type=""search"" name=""q"" id=""query"" value=""{1}"" placeholder=""{2}"" />
		<input type=""submit"" id=""button"" value=""{3}"" /></p>
</fieldset>
</form>", action, query, SitePhrases.LabelSearchPlaceholder, SitePhrases.LabelSearch));
        }

        public static HtmlString Legend(this HtmlHelper helper, string text)
        {
            return new HtmlString("<span class=\"widget legend\" title=\"" + helper.Encode(text) + "\">" + SitePhrases.LabelLegendPlaceholder + "</span>");
        }

        public static HtmlString Email(this HtmlHelper helper, string email)
        {
            return new HtmlString("<a href=\"mailto:" + email + "\">" + email + "</a>");
        }

        public static HtmlString EditShortcut(this HtmlHelper helper)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var actionUrl = urlHelper.Action("index", "admin");
            var actionLabel = Phrases.PagenameAdmin;
            if (helper.ViewContext.IsForController("books") && helper.ViewContext.IsForAction("details"))
            {
                actionUrl = urlHelper.Action("editbook", "admin", new { id = helper.ViewContext.RouteData.Values["id"] as string }, null);
                actionLabel = Phrases.PagenameEditBook;
            }
            if (helper.ViewContext.IsForController("authors") && helper.ViewContext.IsForAction("details"))
            {
                actionUrl = urlHelper.Action("editperson", "admin", new { id = helper.ViewContext.RouteData.Values["id"] as string }, null);
                actionLabel = Phrases.PagenameEditPerson;
            }
            if (helper.ViewContext.HttpContext.Request.Url == null) return null;
            return new HtmlString(String.Format(
                @"
<div id=""editshortcut"">
    <a href=""{0}"">{1}</a>
    |
    <a href=""{2}"">{3}</a>
</div>", actionUrl, actionLabel, urlHelper.Action("logout", "account", new { ReturnUrl = helper.ViewContext.HttpContext.Request.Url.AbsolutePath }, null), Phrases.LabelLogOutSimple));
        }

        public static HtmlString RelationLinkOrName(this HtmlHelper helper, Relation relation)
        {
            return relation.Person.HasPage
                ? helper.ActionLink(relation.ReversePersonName, "details", "authors", new { id = relation.PersonId }, null)
                : new HtmlString(relation.ReversePersonName);
        }

        public static HtmlString BookPersonPageEntry(this HtmlHelper helper, Book book)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            if (book.HasExternalPublisher)
            {
                return new HtmlString(String.Format("{0}{1}. {2}, {3}",
                      book.Title,
                      !String.IsNullOrWhiteSpace(book.Subtitle) ? String.Format(". {0}", book.Subtitle) : string.Empty,
                      book.ExternalPublisher,
                      book.CachedReleaseYear));
            }
            return new HtmlString(String.Format("{0}{1}, {2}",
                "<a href=\"" + urlHelper.Action("details", "books", new { id = book.Id }, null) + "\">" + book.Title + "</a>",
                !String.IsNullOrWhiteSpace(book.Subtitle) ? String.Format(". {0}", book.Subtitle) : string.Empty,
                book.CachedReleaseYear));
        }

        public static string ToWord(this bool source)
        {
            return source ? Phrases.UnitYes : Phrases.UnitNo;
        }

    }

}