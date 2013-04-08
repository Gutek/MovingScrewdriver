using System;
using MovingScrewdriver.Web.Controllers.PostsByTags;
using MvcContrib.TestHelper;
using Xunit;

namespace MovingScrewdriver.Tests.routes
{
    public class posts_by_tags_routes_tests : routes_tests_base
    {
        private string _slug = Guid.NewGuid().ToString("n");

         [Fact]
         public void should_match_by_tag()
         {
             "~/tag/{0}".FormatWith(_slug).ShouldMapTo<PostsByTagsController>(a => a.ByTags(_slug));
         }
    }
}