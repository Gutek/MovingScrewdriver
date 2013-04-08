using System;
using System.Web.Mvc;

namespace MovingScrewdriver.Web.Controllers.Legacy
{
    public partial class LegacyController : AbstractController
    {
         public ActionResult Archive()
         {
             return RedirectToActionPermanent("Archive", "PostsByDate");
         }
    }
}