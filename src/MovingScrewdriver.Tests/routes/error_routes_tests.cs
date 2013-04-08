using MovingScrewdriver.Web.Controllers.Error;
using MvcContrib.TestHelper;
using Xunit;

namespace MovingScrewdriver.Tests.routes
{
    public class error_routes_tests : routes_tests_base
    {
         [Fact]
         public void should_match_404()
         {
             "~/blad/404".ShouldMapTo<ErrorController>(a => a.Error404());
         }

         [Fact]
         public void should_match_ups()
         {
             "~/blad/ups".ShouldMapTo<ErrorController>(a => a.Ups());
         }
    }
}