using System;
using MovingScrewdriver.Web.Controllers.PostDetails;
using MvcContrib.TestHelper;
using Xunit;

namespace MovingScrewdriver.Tests.routes
{
    public class posts_by_date_routes_tests : routes_tests_base
    {
        private string _slug = Guid.NewGuid().ToString("n");
        private int _year = 2012;
        private int _month = 3;
        private int _day = 3;

         [Fact]
         public void should_match_post_details()
        {
            "~/{0}/{1}/{2}/{3}".FormatWith(_year, _month, _day, _slug)
                .ShouldMapTo<PostDetailsController>(a => a.Details(_year, _month, _day, _slug));
         }
    }
}