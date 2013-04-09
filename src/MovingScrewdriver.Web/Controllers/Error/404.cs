using System.Net;
using System.Web.Mvc;

namespace MovingScrewdriver.Web.Controllers.Error
{
    public partial class ErrorController : AbstractController
    {
        [OutputCache(CacheProfile = "StaticContent")]
         public ActionResult Error404()
         {
             Response.StatusCode = (int)HttpStatusCode.NotFound;
             return View("404");
         }
    }
}