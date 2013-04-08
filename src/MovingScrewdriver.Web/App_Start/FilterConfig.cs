using System.Web.Mvc;
using MovingScrewdriver.Web.Infrastructure.Filters;

namespace MovingScrewdriver.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new LogFilterAttribute());
        }
    }
}