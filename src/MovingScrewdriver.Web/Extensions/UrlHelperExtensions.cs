using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MovingScrewdriver.Web.Extensions;

namespace MovingScrewdriver.Web.Extensions
{
    public static class UrlHelperExtensions 
    {
        public static Uri GetBaseUrl(this UrlHelper url)
        {
            var uri = new Uri(
                url.RequestContext.HttpContext.Request.Url,
                url.RequestContext.HttpContext.Request.RawUrl
            );
            var builder = new UriBuilder(uri)
            {
                Path = url.RequestContext.HttpContext.Request.ApplicationPath,
                Query = null,
                Fragment = null
            };
            return builder.Uri;
        }

        public static string ActionResetPage(this UrlHelper url, string action, string controller)
        {
            return url.ActionResetPage(action, controller, null);
        }

        public static string ActionResetPage(this UrlHelper url, string action, string controller, object routeValues)
        {
            var routeDict = new RouteValueDictionary(routeValues);
            
            if (routeDict.ContainsKey("month"))
            {
                routeDict["month"] = "{0:00}".FormatWith(routeDict["month"]);
            }
            
            if (routeDict.ContainsKey("day"))
            {
                routeDict["day"] = "{0:00}".FormatWith(routeDict["day"]);
            }
            
            if (url.RequestContext.RouteData.Values.ContainsKey("page"))
            {
                url.RequestContext.RouteData.Values.Remove("page");
            }

            return url.Action(action, controller, routeDict);
        }

        public static string AbsoluteAction(this UrlHelper url, string actionName) {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;
            return url.Action(actionName, null, (RouteValueDictionary)null, requestUrl.Scheme, null);
            }

        public static string AbsoluteAction(this UrlHelper url, string actionName, object routeValues) {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;
            return url.Action(actionName, null, new RouteValueDictionary(routeValues), requestUrl.Scheme, null);
            }
        public static string AbsoluteAction(this UrlHelper url, string actionName, RouteValueDictionary routeValues) {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;
            return url.Action(actionName, null, routeValues, requestUrl.Scheme, null);
            }
        public static string AbsoluteAction(this UrlHelper url, string actionName, string controllerName) {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;
            return url.Action(actionName, controllerName, (RouteValueDictionary)null, requestUrl.Scheme, null);
            }
        public static string AbsoluteAction(this UrlHelper url, string actionName, string controllerName, object routeValues) {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;
            return url.Action(actionName, controllerName, new RouteValueDictionary(routeValues), requestUrl.Scheme, null);
            }
        public static string AbsoluteAction(this UrlHelper url, string actionName, string controllerName, RouteValueDictionary routeValues) {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;
            return url.Action(actionName, controllerName, routeValues, requestUrl.Scheme, null);
            }
        public static string AbsoluteAction(this UrlHelper url, string actionName, string controllerName, object routeValues, string protocol) {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;
            return url.Action(actionName, controllerName, new RouteValueDictionary(routeValues), protocol, null);
            }

        public static string AbsoluteContent(this UrlHelper url, string contentUrl)
        {
            return new Uri(url.GetBaseUrl(), url.Content(contentUrl)).AbsoluteUri;
        }

        }
}