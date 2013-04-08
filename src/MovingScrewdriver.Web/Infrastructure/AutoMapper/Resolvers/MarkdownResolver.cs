using System;
using System.Web;
using System.Web.Mvc;
using MarkdownSharp;

namespace MovingScrewdriver.Web.Infrastructure.AutoMapper.Resolvers
{
    public class MarkdownResolver
    {
        public static MvcHtmlString Resolve(string inputBody)
        {
            var html = FormatMarkdown(inputBody);
            html = SanitizeHtml.Sanitize(html);
            return MvcHtmlString.Create(html);
        }

        private static string FormatMarkdown(string content)
        {
            var md = new Markdown();
            
            string result;
            
            try
            {
                result = md.Transform(content);
            }
            catch (Exception)
            {
                result = "<pre>{0}</pre>".FormatWith(HttpUtility.HtmlEncode(content));
            }

            return result;
        }
    }
}