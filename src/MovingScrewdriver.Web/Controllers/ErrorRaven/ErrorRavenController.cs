using System.Web.Mvc;

namespace MovingScrewdriver.Web.Controllers.ErrorRaven
{
    public class ErrorRavenController : Controller
    {
        public ActionResult RavenDb()
        {
            return View("ravendb");
        } 
    }
}