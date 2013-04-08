using MovingScrewdriver.Web.Controllers.Screw;
using MvcContrib.TestHelper;
using Xunit;

namespace MovingScrewdriver.Tests.routes
{
    public class screw_routes_tests : routes_tests_base
    {
         [Fact]
         public void should_match_on()
         {
             "~/przykrec-srubke".ShouldMapTo<ScrewController>(a => a.On());
         }

         [Fact]
         public void should_match_done()
         {
             "~/srubka-przykrecona".ShouldMapTo<ScrewController>(a => a.Done());
         }
    }
}