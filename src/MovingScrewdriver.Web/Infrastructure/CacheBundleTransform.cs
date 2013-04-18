using System;
using System.Web;
using System.Web.Optimization;

namespace MovingScrewdriver.Web.Infrastructure
{
    public class CacheBundleTransform : IBundleTransform 
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            context.UseServerCache = true;
            HttpCachePolicyBase cachePolicy = context.HttpContext.Response.Cache;
            cachePolicy.SetCacheability(response.Cacheability);
            cachePolicy.SetOmitVaryStar(true);
            cachePolicy.SetExpires(ApplicationTime.Current.UtcDateTime.AddYears(1));
            cachePolicy.SetValidUntilExpires(true);
            cachePolicy.SetLastModified(ApplicationTime.Current.UtcDateTime);
            cachePolicy.VaryByHeaders["User-Agent"] = true;
        }
    }
}