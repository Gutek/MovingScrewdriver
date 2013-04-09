using System.Web.Mvc;
using System.Xml.Linq;
using MovingScrewdriver.Web.Extensions;

namespace MovingScrewdriver.Web.Controllers.Services
{
    public partial class ServicesController : AbstractController
    {
        [OutputCache(CacheProfile = "StaticContent")]
        public ActionResult Rsd()
        {
            var ns = XNamespace.Get("http://archipelago.phrasewise.com/rsd");

            return XDoc(new XDocument(
                        new XElement(ns + "service",
                                     new XElement(ns + "engineName", "Moving Screwdriver Blog"),
                                     new XElement(ns + "engineLink", "http://blog.gutek.pl"),
                                     new XElement(ns + "homePageLink", Url.AbsoluteAction("AllPosts", "Posts")),
                                     new XElement(ns + "apis",
                                                  new XElement(ns + "api",
                                                               new XAttribute("name", "MetaWeblog"),
                                                               new XAttribute("preferred", "true"),
                                                               new XAttribute("blogID", Url.AbsoluteAction("AllPosts", "Posts")),
                                                               new XAttribute("apiLink", Url.AbsoluteContent("~/services/metaweblogapi.ashx"))
                                                    )
                                        )
                            )
                        ), typeof(ServicesController).FullName);
        }
    }
}