using System.Web.Mvc;
using System.Web.WebPages;

namespace MovingScrewdriver.Web.Extensions
{
    public static class WebPageExtensions
    {
        public static HtmlHelper GetPageHelper(this System.Web.WebPages.Html.HtmlHelper html)
        {
            return ((WebViewPage)WebPageContext.Current.Page).Html;
        }
    }
}