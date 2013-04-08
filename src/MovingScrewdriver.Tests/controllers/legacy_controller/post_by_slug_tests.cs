using System;
using System.Web.Mvc;
using MovingScrewdriver.Web.Infrastructure;
using MovingScrewdriver.Web.Models;
using Xunit;

namespace MovingScrewdriver.Tests.controllers.legacy_controller
{
    public class post_by_slug_tests : legacy_controller_test_base
    {
         [Fact]
         public void should_return_http_not_found_result()
         {
             var result = _controller.PostBySlug(2012, 10, 10, "some-slug");

             Assert.IsType<HttpNotFoundResult>(result);
         }

         [Fact]
         public void should_return_redirect_result_if_post_exists()
         {
             var post = new Post();
             post.Title = "title";
             post.Created = post.Modified = post.PublishAt = ApplicationTime.Current;
             post.LegacySlug = post.Slug;
             set_data(session => session.Store(post));

             var result = _controller.PostBySlug(post.Created.Year
                 , post.Created.Month
                 , post.Created.Day
                 , post.Slug);

             Assert.IsType<RedirectToRouteResult>(result);
         }

         [Fact]
         public void should_return_redirect_pernament_result_if_post_exists()
         {
             var post = new Post();
             post.Title = "title";
             post.Created = post.Modified = post.PublishAt = ApplicationTime.Current;
             post.LegacySlug = post.Slug;
             set_data(session => session.Store(post));

             var result = _controller.PostBySlug(post.Created.Year
                 , post.Created.Month
                 , post.Created.Day
                 , post.Slug) as RedirectToRouteResult;

             Assert.True(result.Permanent, "this url is not used any more, therefore we should redirect pernamently");
         }

         [Fact]
         public void should_redirect_to_details_action_on_post_details_controller()
         {
             var post = new Post();
             post.Title = "title";
             post.Created = post.Modified = post.PublishAt = ApplicationTime.Current;
             post.LegacySlug = post.Slug;
             set_data(session => session.Store(post));

             var result = _controller.PostBySlug(post.Created.Year
                 , post.Created.Month
                 , post.Created.Day
                 , post.Slug) as RedirectToRouteResult;

             Assert.Equal("PostDetails", result.RouteValues["controller"]);
             Assert.Equal("Details", result.RouteValues["action"]);
             Assert.Equal(post.Created.Year, result.RouteValues["year"]);
             Assert.Equal(post.Created.Month, Convert.ToInt32(result.RouteValues["month"]));
             Assert.Equal(post.Created.Day, Convert.ToInt32(result.RouteValues["day"]));
             Assert.Equal(post.Slug, result.RouteValues["slug"]);
         }
    }
}