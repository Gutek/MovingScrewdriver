using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using MovingScrewdriver.Web.Infrastructure;
using Raven.Client.Connection;

namespace MovingScrewdriver.Web.Extensions
{
    public static class GeneralUtils
    {
        private static readonly Regex RegexHtml =
            new Regex(
                @"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>",
                RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex RegexTitle = new Regex(
            @"(?<=<title.*>)([\s\S]*)(?=</title>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static string GetClientIp()
        {
            var context = HttpContextFactory.GetHttpContext();

            if (context == null)
            {
                return string.Empty;
            }

            var request = context.Request;
            if (request == null)
            {
                return string.Empty;
            }

            var xff = request.Headers["X-Forwarded-For"];
            var clientIp = string.Empty;
            if (xff.IsNotNullOrEmpty())
            {
                var idx = xff.IndexOf(',');
                clientIp = idx > 0 ? xff.Substring(0, idx) : xff;
            }

            return clientIp.IsNullOrEmpty() ? request.UserHostAddress : clientIp;
        }

        public static bool ExistsOn(this string targetUrl, string sourceUrl)
        {
            try
            {
                var file = new RemoteFile(sourceUrl.ToUri());
                var html = file.GetFileAsString();
                
                return html.ToUpperInvariant().Contains(targetUrl.ToUpperInvariant());
            }
            catch (WebException)
            {
                return false;
            }
        }

        public static bool ExistsOn(this string targetUrl, string sourceUrl, out string title)
        {
            title = string.Empty;
            try
            {
                var file = new RemoteFile(sourceUrl.ToUri());
                var html = file.GetFileAsString();

                title = RegexTitle.Match(html).Value.Trim();
                var containsHtml = RegexHtml.IsMatch(title);

                return html.ToUpperInvariant().Contains(targetUrl.ToUpperInvariant()) && !containsHtml;
            }
            catch (WebException)
            {
                return false;
            }
        }


        public static string GetClientAgent()
        {
            var context = HttpContextFactory.GetHttpContext();

            if (context == null)
            {
                return string.Empty;
            }

            var request = context.Request;
            if (request == null)
            {
                return string.Empty;
            }

            return request.UserAgent;
        }

        public static string BlogUrl()
        {
            var context = HttpContextFactory.GetHttpContext();

            if (context == null)
            {
                return string.Empty;
            }

            var request = context.Request;
            if (request == null)
            {
                return string.Empty;
            }

            if (request.Url == null)
            {
                return string.Empty;
            }

            return request.Url.GetLeftPart(UriPartial.Authority);
        }

        public static string GetDomain(string sourceUrl)
        {
            var start = sourceUrl.IndexOf("://") + 3;
            var stop = sourceUrl.IndexOf("/", start);
            return sourceUrl.Substring(start, stop - start).Replace("www.", string.Empty);
        }

    }
}