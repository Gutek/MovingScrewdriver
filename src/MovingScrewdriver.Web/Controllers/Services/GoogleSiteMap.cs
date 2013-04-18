using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using MovingScrewdriver.Web.Extensions;
using MovingScrewdriver.Web.Infrastructure;
using MovingScrewdriver.Web.Models;
using Raven.Client;

namespace MovingScrewdriver.Web.Controllers.Services
{
    public partial class ServicesController : AbstractController
    {
        [OutputCache(CacheProfile = "StaticContent")]
        public ActionResult GoogleSiteMap()
        {
            // as per doc:  Google accepts RSS (Real Simple Syndication) 2.0 and Atom 1.0 feeds. 
            // If you have a blog with an RSS or Atom feed, you submit the feed's URL as a Sitemap. 
            // Most blog software creates your feed for you. Note that the feed may only provide 
            // information on recent URLs. In addition, you can use an mRSS (media RSS) feed to 
            // provide Google with details about video content on your site.
            RavenQueryStatistics stats;
            var posts = CurrentSession.Query<Post>()
                                      .Statistics(out stats)
                                      .WhereIsPublicPost()
                                      .Where(x => x.PublishAt < ApplicationTime.Current.AsMinutes())
                                      .OrderByDescending(x => x.PublishAt)
                                      .Take(1024)
                                      .ToList();

            var date = ApplicationTime.Current;
            date = date.AddDays(2 - date.Day);

            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var sitemap = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(ns + "urlset"

                    , new XElement(ns + "url",
                        new XElement(ns + "loc", Url.AbsoluteAction("Me", "About")),
                        new XElement(ns + "lastmod", date.ToString("yyyy-MM-dd")),
                        new XElement(ns + "changefreq", "monthly")
                    )
                    , 
                    from post in posts
                    let url = Url.AbsoluteAction("Details", "PostDetails", post.ToRouteData())
                    let pubDate = post.PublishAt.ToString("yyyy-MM-dd")
                    select
                        new XElement(ns + "url",
                            new XElement(ns + "loc", url),
                            new XElement(ns + "lastmod", pubDate),
                            new XElement(ns + "changefreq", "monthly")
                      )
                    )
                  );
            
            return XDoc(sitemap);
        }
    }
}