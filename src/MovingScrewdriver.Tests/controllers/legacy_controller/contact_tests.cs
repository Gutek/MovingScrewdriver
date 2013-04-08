using System.Web.Mvc;
using Xunit;

namespace MovingScrewdriver.Tests.controllers.legacy_controller
{
    public class contact_tests : legacy_controller_test_base
    {
         [Fact]
         public void should_return_redirect()
         {
             var result = _controller.Contact();

             Assert.IsType<RedirectToRouteResult>(result);
         }

         [Fact]
         public void should_return_pernament_redirect()
         {
             var result = _controller.Contact() as RedirectToRouteResult;

             Assert.True(result.Permanent, "this url is not used any more, therefore we should redirect pernamently");
         }

         [Fact]
         public void should_redirecto_to_me_action_on_about_controller()
         {
             var result = _controller.Contact() as RedirectToRouteResult;

             Assert.Equal("About", result.RouteValues["controller"]);
             Assert.Equal("Me", result.RouteValues["action"]);
         }
    }
}