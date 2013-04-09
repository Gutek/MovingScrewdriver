using System;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using MovingScrewdriver.Web.Extensions;
using MovingScrewdriver.Web.Infrastructure;
using Raven.Client;
using Raven.Client.Linq;

namespace MovingScrewdriver.Web.Controllers.Syndication
{
    public partial class SyndicationController : AbstractController
    {
        [OutputCache(CacheProfile = "DynamicContent")]
        public ActionResult RssComments()
        {
            RavenQueryStatistics stats = null;
            var commentsTuples = CurrentSession.QueryForRecentComments(q =>
            {
                //if (id != null)
                //{
                //    var postId = CurrentSession.Advanced.GetDocumentId(id);
                //    q = q.Where(x => x.PostId == postId);
                //}
                return q.Statistics(out stats).Take(30);
            });

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
                                      new XElement("description", BlogConfig.MetaDescription ?? BlogConfig.Title),
                                      new XElement("copyright", String.Format("{0} (c) {1}", BlogConfig.MetaCopyright, ApplicationTime.Current.Year)),
                                      new XElement("ttl", "60"),
                                      from commentsTuple in commentsTuples
                                      let comment = commentsTuple.Item1
                                      let post = commentsTuple.Item2
                                      let link = Url.AbsoluteAction("Details", "PostDetails", post.ToRouteData()) + "#comment-" + comment.Id
                                      select new XElement("item",
                                                          new XElement("title", "Komentarz {0} do {1}".FormatWith(comment.Author, post.Title)),
                                                          new XElement("description", comment.Content),
                                                          new XElement("link", link),
                                                          new XElement("guid", link),
                                                          new XElement("pubDate", comment.Created.ToString("R"))
                                        )
                            )
                )
            );

            return XDoc(rss, responseETagHeader);
        }
    }
}