using System;
using System.Web.Mvc;

namespace MovingScrewdriver.Web.Controllers.Legacy
{
    public partial class LegacyController : AbstractController
    {
         public ActionResult PostsByDate(int year, int? month, int? day)
         {
            return RedirectToActionPermanent("ByDate", "PostsByDate", new
            {
                year = year,
                month = month,
                day = day
            });
         }
    }
}