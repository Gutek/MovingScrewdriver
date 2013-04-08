using System.Web.Mvc;
using Xunit;

namespace MovingScrewdriver.Tests.controllers.legacy_controller
{
    public class archive_tests : legacy_controller_test_base
    {
         [Fact]
         public void should_return_redirect()
         {
             var result = _controller.Archive();

             Assert.IsType<RedirectToRouteResult>(result);
         }

         [Fact]
         public void should_return_pernament_redirect()
         {
             var result = _controller.Archive() as RedirectToRouteResult;

             Assert.True(result.Permanent, "this url is not used any more, therefore we should redirect pernamently");
         }

         [Fact]
         public void should_redirecto_to_archive_action_on_post_by_date_controller()
         {
             var result = _controller.Archive() as RedirectToRouteResult;

             Assert.Equal("PostsByDate", result.RouteValues["controller"]);
             Assert.Equal("Archive", result.RouteValues["action"]);
         }
    }
}