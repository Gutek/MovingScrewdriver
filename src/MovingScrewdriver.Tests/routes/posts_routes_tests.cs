using MovingScrewdriver.Web.Controllers.Posts;
using MvcContrib.TestHelper;
using Xunit;

namespace MovingScrewdriver.Tests.routes
{
    public class posts_routes_tests : routes_tests_base
    {
         [Fact]
         public void should_match_all_posts()
         {
             "~/".ShouldMapTo<PostsController>(a => a.AllPosts());
         }
    }
}