using System;
using MovingScrewdriver.Web.Controllers.Posts;
using MvcContrib.TestHelper;
using Xunit;

namespace MovingScrewdriver.Tests.routes
{
    public class post_details_routes_tests : routes_tests_base
    {
        private string _slug = Guid.NewGuid().ToString("n");
        private int _year = 2012;
        private int _month = 3;
        private int _day = 3;

         [Fact]
         public void should_match_post_details()
         {
             "~/".ShouldMapTo<PostsController>(a => a.AllPosts());
         }
    }
}