using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using MovingScrewdriver.Web.ViewModels;

namespace MovingScrewdriver.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        private static string get_link_with_route_data(HtmlHelper html, int page, bool absolute = true)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var action = html.ViewContext.Controller.ValueProvider.GetValue("action").RawValue.ToString();
            var controller = html.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();
            var routeData = new RouteValueDictionary(html.ViewContext.RouteData.Values);

            routeData["page"] = page;

            routeData.Remove("action");
            routeData.Remove("controller");

            if (absolute)
            {
                return urlHelper.AbsoluteAction(action, controller, routeData);
            }
            else
            {
                return urlHelper.Action(action, controller, routeData);
            }
        }

        public static string NextPageUrl(this HtmlHelper html, int page, bool absolute = true)
        {
            return get_link_with_route_data(html, page + 1, absolute);
        }

        public static string PrevPageUrl(this HtmlHelper html, int page, bool absolute = true)
        {
            return get_link_with_route_data(html, page - 1, absolute);
        }

        public static MvcHtmlString TrackbackRdf(this HtmlHelper html, PostViewModel post)
        {
            if (post.AreCommentsClosed || post.Post.IsCommentAllowed == false)
            {
                return MvcHtmlString.Create(string.Empty);
            }

            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var postUrl = urlHelper.AbsoluteAction("Details", "PostDetails", post.Post.ToRouteData());
            var trackbackUrl = urlHelper.AbsoluteAction("Trackback", "Services", new
            {
                id = post.Post.Id
            });

            return MvcHtmlString.Create(GetTrackbackRdf(postUrl, post.Post.Title, trackbackUrl));
        }

        private static string GetTrackbackRdf(string postUrl, MvcHtmlString postTitle, string trackbackUrl)
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"<!--");
            sb.AppendLine(@"<rdf:RDF xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#"" ");
            sb.AppendLine(@"xmlns:dc=""http://purl.org/dc/elements/1.1/"" ");
            sb.AppendLine(@"xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"">");
            sb.AppendLine(@"<rdf:Description ");
            sb.AppendLine(@"rdf:about=""{0}"" ".FormatWith(postUrl));
            sb.AppendLine(@"dc:identifier=""{0}"" ".FormatWith(postUrl));
            sb.AppendLine(@"dc:title=""{0}"" ".FormatWith(postTitle));
            sb.AppendLine(@"trackback:ping=""{0}"" />".FormatWith(trackbackUrl));
            sb.AppendLine(@"</rdf:RDF>");
            sb.AppendLine("-->");

            return sb.ToString();
        }
    }
}