using System.Web.Mvc;

namespace MovingScrewdriver.Web.Controllers.ErrorRaven
{
    public class ErrorRavenController : Controller
    {
        [OutputCache(CacheProfile = "StaticContent")]
        public ActionResult RavenDb()
        {
            return View("ravendb");
        } 
    }
}