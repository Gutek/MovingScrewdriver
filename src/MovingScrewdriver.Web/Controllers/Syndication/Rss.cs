using System;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using MovingScrewdriver.Web.Extensions;
using MovingScrewdriver.Web.Infrastructure;
using MovingScrewdriver.Web.Models;
using Raven.Client;

namespace MovingScrewdriver.Web.Controllers.Syndication
{
    public partial class SyndicationController : AbstractController
    {
        [OutputCache(CacheProfile = "DynamicContent")]
        public ActionResult Rss()
        {
            RavenQueryStatistics stats;

            var posts = CurrentSession.Query<Post>()
                                      .Statistics(out stats)
                                      .WhereIsPublicPost()
                                      .Where(x => x.PublishAt < ApplicationTime.Current.AsMinutes())
                                      .OrderByDescending(x => x.PublishAt)
                                      .Take(20)
                                      .ToList();

            string responseETagHeader;
            if (CheckEtag(stats, out responseETagHeader))
            {
                return new HttpStatusCodeResult(304);
            }

            var rss = new XDocument(
                new XElement("rss",
                             new XAttribute("version", "2.0"),
                             new XElement("channel",
                                          new XElement("title", BlogConfig.MetaApplicationName),
                                          new XElement("link", Url.AbsoluteAction("AllPosts", "Posts")),
                                          new XElement("description", BlogConfig.MetaDescription ?? BlogConfig.MetaApplicationName),
                                          new XElement("copyright", String.Format("{0} (c) {1}", BlogConfig.MetaCopyright, ApplicationTime.Current.Year)),
                                          new XElement("ttl", "60"),
                                          from post in posts
                                          let postLink = Url.AbsoluteAction("Details", "PostDetails", post.ToRouteData())
                                          select new XElement("item",
                                                              new XElement("title", post.Title),
                                                              new XElement("description", MvcHtmlString.Create(post.Content),
                                                              new XElement("link", postLink),
                                                                new XElement("guid", postLink),
                                                              new XElement("pubDate", post.PublishAt.ToString("R"))
                                            )
                                )
                    )
                )
            );

            return XDoc(rss, responseETagHeader, contentType: "application/rss+xml");
        }

    }
}