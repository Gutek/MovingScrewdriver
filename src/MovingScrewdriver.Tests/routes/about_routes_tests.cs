using MovingScrewdriver.Web.Controllers.About;
using MvcContrib.TestHelper;
using Xunit;

namespace MovingScrewdriver.Tests.routes
{
    public class about_routes_tests : routes_tests_base
    {
         [Fact]
         public void should_match_about()
         {
             "~/o-mnie".ShouldMapTo<AboutController>(a => a.Me());
         }
    }
}