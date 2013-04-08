using System;
using System.Web.Mvc;
using System.Xml.Linq;
using MovingScrewdriver.Web.Controllers.Syndication;
using MovingScrewdriver.Web.Extensions;

namespace MovingScrewdriver.Web.Controllers.Services
{
    public partial class ServicesController : AbstractController
    {
        public ActionResult GoogleSiteMap()
        {
            // as per doc:  Google accepts RSS (Real Simple Syndication) 2.0 and Atom 1.0 feeds. 
            // If you have a blog with an RSS or Atom feed, you submit the feed's URL as a Sitemap. 
            // Most blog software creates your feed for you. Note that the feed may only provide 
            // information on recent URLs. In addition, you can use an mRSS (media RSS) feed to 
            // provide Google with details about video content on your site.
            throw new NotImplementedException();
        }
    }
}