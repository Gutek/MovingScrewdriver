using MovingScrewdriver.Web.Controllers.Aside;
using MvcContrib.TestHelper;
using Xunit;

namespace MovingScrewdriver.Tests.routes
{
    public class aside_routes_tests : routes_tests_base
    {
         [Fact]
         public void should_match_me()
         {
             "~/aside/o-mnie".ShouldMapTo<AsideController>(a => a.Me());
         }

         [Fact]
         public void should_match_recent_comments()
         {
             "~/aside/ostatnie-komentarze".ShouldMapTo<AsideController>(a => a.RecentComments());
         }

         [Fact]
         public void should_match_twitter()
         {
             "~/aside/twitter".ShouldMapTo<AsideController>(a => a.Twitter());
         }
    }
}