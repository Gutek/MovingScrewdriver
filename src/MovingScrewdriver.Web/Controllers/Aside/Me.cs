using System.Web.Mvc;

namespace MovingScrewdriver.Web.Controllers.Aside
{
    public partial class AsideController : AbstractController
    {
        [ChildActionOnly]
         public ActionResult Me()
         {
             return PartialView("me");
         }
    }
}