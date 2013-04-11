using System;
using MovingScrewdriver.Web.Controllers.Legacy;
using MvcContrib.TestHelper;
using Xunit;

namespace MovingScrewdriver.Tests.routes
{
    public class legacy_routes_tests : routes_tests_base
    {
        private string _slug = Guid.NewGuid().ToString("n");
        private int _year = 2012;
        private int _month = 3;
        private int _day = 3;

        [Fact]
         public void should_match_about()
         {
             "~/page/about.aspx".ShouldMapTo<LegacyController>(a => a.About());
             "~/page/About.aspx".ShouldMapTo<LegacyController>(a => a.About());
             "~/author/gutek.aspx".ShouldMapTo<LegacyController>(a => a.About());
         }

        [Fact]
         public void should_match_contact()
         {
             "~/contact.aspx".ShouldMapTo<LegacyController>(a => a.Contact());
             "~/Contact.aspx".ShouldMapTo<LegacyController>(a => a.Contact());
         }
        [Fact]
         public void should_match_all_posts()
         {
             "~/default.aspx".ShouldMapTo<LegacyController>(a => a.AllPosts());
         }

        [Fact]
         public void should_match_post()
         {
             "~/post.aspx".ShouldMapTo<LegacyController>(a => a.PostByUid());
         }

        [Fact]
         public void should_match_post_by_slug()
         {
             "~/post/{0}/{1}/{2}/{3}.aspxSee".FormatWith(_year, _month, _day, _slug)
                 .ShouldMapTo<LegacyController>(a => a.PostBySlug(_year, _month, _day, _slug));
             "~/post/{0}/{1}/{2}/{3}.aspx".FormatWith(_year, _month, _day, _slug)
                 .ShouldMapTo<LegacyController>(a => a.PostBySlug(_year, _month, _day, _slug));
         }

        [Fact]
        public void should_match_posts_by_year()
        {

            "~/{0}/default.aspx".FormatWith(_year)
                .ShouldMapTo<LegacyController>(a => a.PostsByDate(_year, null, null));
        }

        [Fact]
        public void should_match_posts_by_year_month()
        {
            "~/{0}/{1}/default.aspx".FormatWith(_year, _month)
                .ShouldMapTo<LegacyController>(a => a.PostsByDate(_year, _month, null));
        }

        [Fact]
        public void should_match_posts_by_year_month_day()
        {
            "~/{0}/{1}/{2}/default.aspx".FormatWith(_year, _month, _day)
                .ShouldMapTo<LegacyController>(a => a.PostsByDate(_year, _month, _day));
        }
    }
}