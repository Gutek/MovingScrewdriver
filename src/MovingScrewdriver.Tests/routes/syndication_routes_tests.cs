using System;
using MovingScrewdriver.Web.Controllers.Screw;
using MovingScrewdriver.Web.Controllers.Syndication;
using MvcContrib.TestHelper;
using Xunit;

namespace MovingScrewdriver.Tests.routes
{
    public class syndication_routes_tests : routes_tests_base
    {
         [Fact]
         public void should_match_posts_rss()
         {
             "~/rss/posty".ShouldMapTo<SyndicationController>(a => a.Rss());
         }

         [Fact]
         public void should_match_comments_rss()
         {
             "~/rss/komentarze".ShouldMapTo<SyndicationController>(a => a.RssComments());
         }

         [Fact]
         public void should_match_tag_rss()
         {
             var tag = "test";
             "~/rss/tag/{0}".FormatWith(tag).ShouldMapTo<SyndicationController>(a => a.RssByTag(tag));
         }

         [Fact]
         public void should_match_category_rss()
         {
             var category = "test";
             "~/rss/kategoria/{0}".FormatWith(category).ShouldMapTo<SyndicationController>(a => a.RssByCategory(category));
         }
    }
}