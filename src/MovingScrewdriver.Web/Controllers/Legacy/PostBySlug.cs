using System;
using System.Linq;
using System.Web.Mvc;
using MovingScrewdriver.Web.Extensions;
using MovingScrewdriver.Web.Models;
using Raven.Client.Linq;

namespace MovingScrewdriver.Web.Controllers.Legacy
{
    public partial class LegacyController : AbstractController
    {
         public ActionResult PostBySlug(int year, int month, int day, string slug)
         {
             var oldPost = CurrentSession.Query<Post>()
                 .PublishedAt(year, month, day)
                 .WithSlug(slug)
                 .FirstOrDefault();

             if (oldPost != null)
             {
                 return RedirectToActionPermanent("Details", "PostDetails", oldPost.ToRouteData());
             }

             return HttpNotFound();
         }
    }
}