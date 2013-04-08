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
         public ActionResult PostByUid()
         {
             var id = Request.QueryString["id"];

             if (id.IsNullOrWhiteSpace())
             {
                 return HttpNotFound();
             }

             Guid guidId = Guid.Empty;
             if (Guid.TryParse(id, out guidId) == false)
             {
                 return HttpNotFound();
             }
             
             var post = CurrentSession.Query<Post>()
                 .Where(x => x.LegacyUniqueId != null)
                 .Where(x => x.LegacyUniqueId.Equals(guidId.ToString(), StringComparison.OrdinalIgnoreCase))
                 .FirstOrDefault();

             if (post != null)
             {
                 return RedirectToActionPermanent("Details", "PostDetails", post.ToRouteData());
             }

             return HttpNotFound();
         }
    }
}