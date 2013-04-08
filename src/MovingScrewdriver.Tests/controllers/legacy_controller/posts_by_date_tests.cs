using System.Web.Mvc;
using Xunit;
using Xunit.Extensions;

namespace MovingScrewdriver.Tests.controllers.legacy_controller
{
    public class posts_by_date_tests : legacy_controller_test_base
    {
         [Fact]
         public void should_return_redirect()
         {
             Assert.IsType<RedirectToRouteResult>(_controller.PostsByDate(10, null, null));
             Assert.IsType<RedirectToRouteResult>(_controller.PostsByDate(10, 10, null));
             Assert.IsType<RedirectToRouteResult>(_controller.PostsByDate(10, 10, 10));
         }

         [Fact]
         public void should_return_pernament_redirect()
         {
             var result = _controller.PostsByDate(10, null, null) as RedirectToRouteResult;

             Assert.True(result.Permanent, "this url is not used any more, therefore we should redirect pernamently");
         }

         [Theory]
         [InlineData(10, null, null)]
         [InlineData(10, 20, null)]
         [InlineData(10, 20, 10)]
         public void should_redirect_to_by_date_action_on_posts_by_date_controller(int year, int? month, int? day)
         {
             var result = _controller.PostsByDate(year, month, day) as RedirectToRouteResult;

             Assert.Equal("PostsByDate", result.RouteValues["controller"]);
             Assert.Equal("ByDate", result.RouteValues["action"]);
             Assert.Equal(year, result.RouteValues["year"]);
             Assert.Equal(month, result.RouteValues["month"]);
             Assert.Equal(day, result.RouteValues["day"]);
         }
    }
}