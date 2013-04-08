using System;
using System.Web.Mvc;
using MovingScrewdriver.Web.Infrastructure;
using MovingScrewdriver.Web.Models;
using Xunit;

namespace MovingScrewdriver.Tests.controllers.legacy_controller
{
    public class post_by_uid_tests : legacy_controller_test_base
    {
         [Fact]
         public void should_return_http_not_found_result_if_id_query_string_does_not_exists()
         {
             var result = _controller.PostByUid();

             Assert.IsType<HttpNotFoundResult>(result);
         }

         [Fact]
         public void should_return_http_not_found_result_if_id_is_not_guid()
         {
             _queryStrings.Add("id", "some_thing");
             var result = _controller.PostByUid();

             Assert.IsType<HttpNotFoundResult>(result);
         }

         [Fact]
         public void should_return_http_not_found_result_post_of_id_does_not_exists()
         {
             _queryStrings.Add("id", Guid.NewGuid().ToString());
             var result = _controller.PostByUid();

             Assert.IsType<HttpNotFoundResult>(result);
         }

         [Fact]
         public void should_return_redirect_result_if_post_exists()
         {
             var id = Guid.NewGuid();
             var post = new Post();
             post.Title = "title";
             post.Created = post.Modified = post.PublishAt = ApplicationTime.Current;
             post.LegacyUniqueId = id.ToString();

             set_data(session => session.Store(post));
             _queryStrings.Add("id", id.ToString());
             var result = _controller.PostByUid();

             Assert.IsType<RedirectToRouteResult>(result);
         }

         [Fact]
         public void should_return_redirect_pernament_result_if_post_exists()
         {
             var id = Guid.NewGuid();
             var post = new Post();
             post.Title = "title";
             post.Created = post.Modified = post.PublishAt = ApplicationTime.Current;
             post.LegacyUniqueId = id.ToString();

             set_data(session => session.Store(post));
             
             _queryStrings.Add("id", id.ToString());

             var result = _controller.PostByUid() as RedirectToRouteResult;

             Assert.True(result.Permanent, "this url is not used any more, therefore we should redirect pernamently");
         }

         [Fact]
         public void should_redirect_to_details_action_on_post_details_controller()
         {
             var id = Guid.NewGuid();
             var post = new Post();
             post.Title = "title";
             post.Created = post.Modified = post.PublishAt = ApplicationTime.Current;
             post.LegacyUniqueId = id.ToString();

             set_data(session => session.Store(post));

             _queryStrings.Add("id", id.ToString());
             var result = _controller.PostByUid() as RedirectToRouteResult;

             Assert.Equal("PostDetails", result.RouteValues["controller"]);
             Assert.Equal("Details", result.RouteValues["action"]);
             Assert.Equal(post.Created.Year, result.RouteValues["year"]);
             Assert.Equal(post.Created.Month, Convert.ToInt32(result.RouteValues["month"]));
             Assert.Equal(post.Created.Day, Convert.ToInt32(result.RouteValues["day"]));
             Assert.Equal(post.Slug, result.RouteValues["slug"]);
         }
    }
}