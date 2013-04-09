using System;
using System.Web.Mvc;

namespace MovingScrewdriver.Web.Controllers.Syndication
{
    public partial class SyndicationController : AbstractController
    {
        [OutputCache(CacheProfile = "DynamicContent")]
        public ActionResult RssByCategory(string slug)
        {
            throw new NotImplementedException();
        }
    }
}