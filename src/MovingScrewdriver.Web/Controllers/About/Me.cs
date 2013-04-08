using System.Web.Mvc;

namespace MovingScrewdriver.Web.Controllers.About
{
    public partial class AboutController : AbstractController
    {
         public ActionResult Me()
         {
             return View("me");
         }
    }
}