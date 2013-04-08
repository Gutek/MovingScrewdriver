using System;
using System.Web.Mvc;
using MovingScrewdriver.Web.Controllers.Services;
using MvcContrib.TestHelper;
using Xunit;

namespace MovingScrewdriver.Tests.routes
{
    public class services_routes_tests : routes_tests_base
    {
         [Fact]
         public void should_match_rsd()
         {
             "~/services/rsd".ShouldMapTo<ServicesController>(a => a.Rsd());
         }

         [Fact]
         public void should_match_post_trackback()
         {
             var blog_name = "test";
             var url = "http://google";
             var title = "some_title";
             var excerpt = "some_text";
             var id = 10;

             var route = "~/trackback/{0}".FormatWith(id).WithMethod(HttpVerbs.Post);
             
             route.Values["blog_name"] = blog_name;
             route.Values["url"] = url;
             route.Values["title"] = title;
             route.Values["excerpt"] = excerpt;

             route.ShouldMapTo<ServicesController>(a => a.Trackback(
                 blog_name,
                 url,
                 title,
                 excerpt,
                 id
            ));
         }
         [Fact]
         public void should_match_put_trackback()
         {
             var blog_name = "test";
             var url = "http://google";
             var title = "some_title";
             var excerpt = "some_text";
             var id = 10;

             var route = "~/trackback/{0}".FormatWith(id).WithMethod(HttpVerbs.Put);
             
             route.Values["blog_name"] = blog_name;
             route.Values["url"] = url;
             route.Values["title"] = title;
             route.Values["excerpt"] = excerpt;

             route.ShouldMapTo<ServicesController>(a => a.Trackback(
                 blog_name,
                 url,
                 title,
                 excerpt,
                 id
            ));
         }
    }
}