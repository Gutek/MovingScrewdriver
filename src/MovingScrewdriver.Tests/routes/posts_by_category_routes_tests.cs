using System;
using MovingScrewdriver.Web.Controllers.Posts;
using MovingScrewdriver.Web.Controllers.PostsByCategory;
using MvcContrib.TestHelper;
using Xunit;

namespace MovingScrewdriver.Tests.routes
{
    public class posts_by_category_routes_tests : routes_tests_base
    {
        private string _slug = Guid.NewGuid().ToString("n");

         [Fact]
         public void should_match_by_category()
         {
             "~/kategoria/{0}".FormatWith(_slug).ShouldMapTo<PostsByCategoryController>(a => a.ByCategory(_slug));
         }
    }
}