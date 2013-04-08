using System.Web.Mvc;

namespace MovingScrewdriver.Web.Controllers.Screw
{
    public partial class ScrewController : AbstractController
    {
         public ActionResult Done()
         {
             return View("done");
         }
    }
}