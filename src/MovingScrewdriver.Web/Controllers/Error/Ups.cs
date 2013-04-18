using System.Net;
using System.Web.Mvc;

namespace MovingScrewdriver.Web.Controllers.Error
{
    public partial class ErrorController : AbstractController
    {
        [OutputCache(CacheProfile = "StaticContent")]
         public ActionResult Ups()
         {
             Response.StatusCode = (int)HttpStatusCode.InternalServerError;
             return View("ups");
         }
    }
}