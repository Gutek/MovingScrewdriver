using System.Web.Mvc;

namespace MovingScrewdriver.Web.Controllers.Legacy
{
    public partial class LegacyController : AbstractController
    {
         public ActionResult PostsByCategory(string slug)
         {
             return RedirectToActionPermanent("ByCategory", "PostsByCategory", new { slug = slug });
         }
    }
}