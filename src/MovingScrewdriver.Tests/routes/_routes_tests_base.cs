using System;
using System.Web.Routing;
using MovingScrewdriver.Web;

namespace MovingScrewdriver.Tests.routes
{
    public class routes_tests_base : IDisposable
    {
        public routes_tests_base()
        {
            new RouteConfig(RouteTable.Routes).Configure();
        }

        public void Dispose()
        {
            RouteTable.Routes.Clear();
        }
    }
}
