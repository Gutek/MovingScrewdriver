using MovingScrewdriver.Web.Controllers.ErrorRaven;
using MvcContrib.TestHelper;
using Xunit;

namespace MovingScrewdriver.Tests.routes
{
    public class error_raven_tests : routes_tests_base
    {
        [Fact]
        public void should_match_raven_db()
        {
            "~/blad/ravendb".ShouldMapTo<ErrorRavenController>(a => a.RavenDb());
        }
    }
}