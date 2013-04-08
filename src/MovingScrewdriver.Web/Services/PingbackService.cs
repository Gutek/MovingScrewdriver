using System;
using System.Linq;
using Autofac;
using CookComputing.XmlRpc;
using MovingScrewdriver.Web.Extensions;
using MovingScrewdriver.Web.Models;
using NLog;
using Raven.Client;

namespace MovingScrewdriver.Web.Services
{
    // http://www.hixie.ch/specs/pingback/pingback
    public interface IPingbackService
    {
        [XmlRpcMethod("pingback.ping")]
        string pingback(string sourceUri, string targetUri);
    }

    public class PingbackService : XmlRpcService, IPingbackService
    {
        private readonly IDocumentStore _store;
        private readonly IAkismetService _akismetService;
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public PingbackService()
        {
            _store = AutofacConfig.IoC.Resolve<IDocumentStore>();
            _akismetService = AutofacConfig.IoC.Resolve<IAkismetService>();
        }

        string IPingbackService.pingback(string sourceUri, string targetUri)
        {
            if (sourceUri.IsNullOrEmpty())
            {
                _log.Trace("Wrong param for pinback: sourceUri");
                throw new XmlRpcFaultException(16, "The source URI does not exist.");
            }
            
            if (targetUri.IsNullOrEmpty())
            {
                _log.Trace("Wrong param for pinback: targetUri");
                throw new XmlRpcFaultException(32, "The specified target URI does not exist.");
            }

            var title = string.Empty;
            if (targetUri.ExistsOn(sourceUri, out title) == false)
            {
                _log.Warn("SPAM: someone tried to add pinback when without linking to our site. Source URI: {0}. Pinback to: {1}", sourceUri, targetUri);
                throw new XmlRpcFaultException(17, "The source URI does not contain a link to the target URI, and so cannot be used as a source.");
            }

            var slug = get_slug(targetUri);

            _log.Trace("Target uri: {0}, slug: {1}", targetUri, slug);

            using (var session = _store.OpenSession())
            {
                var config = session.Load<ScrewdriverConfig>("Blog/Config");
                var post = session.Query<Post>()
                                  .Include(x => x.CommentsId)
                                  .WhereIsPublicPost()
                                  .WithSlug(slug)
                                  .FirstOrDefault();

                if (post == null)
                {
                    _log.Trace("The specified target URI cannot be used as a target. It either doesn't exist, or it is not a pingback-enabled resource.");
                    throw new XmlRpcFaultException(33, "The specified target URI cannot be used as a target. It either doesn't exist, or it is not a pingback-enabled resource.");
                }

                var comments = session.Load<PostComments>(post.CommentsId);

                if (comments.AreCommentsClosed(post, config.NumberOfDayToCloseComments)
                    || post.AllowComments == false)
                {
                    _log.Trace("Comments closed, pinback blocked for target URI: {0}", targetUri);
                    throw new XmlRpcFaultException(33, "The specified target URI cannot be used as a target. It either doesn't exist, or it is not a pingback-enabled resource.");
                }

                var exists = comments.Comments.TrackbackOrPingbackExists(sourceUri);

                if (exists)
                {
                    _log.Trace("Pingback from {0} already exists on {1}", sourceUri, targetUri);
                    throw new XmlRpcFaultException(48, "The pingback has already been registered.");
                }

                var comment = new PostComments.Comment();
                comment.Id = comments.GenerateNewCommentId();
                comment.Author = GeneralUtils.GetDomain(sourceUri);
                comment.Email = CommentType.Pingback.ToString();
                comment.Type = CommentType.Pingback;
                comment.Url = sourceUri;
                comment.UserAgent = GeneralUtils.GetClientAgent();
                comment.UserHostAddress = GeneralUtils.GetClientIp();
                comment.Content = "Pingback z {0} - {1}".FormatWith(comment.Author, title);

                var isSpam = _akismetService.CheckForSpam(comment);

                if (isSpam)
                {
                    comment.IsSpam = true;
                    comments.Spam.Add(comment);
                }
                else
                {
                    post.CommentsCount++;
                    comments.Comments.Add(comment);
                }

                session.SaveChanges();
            }

            return "Your ping request has been received successfully.!";
        }

        private string get_slug(string url)
        {
            var start = url.LastIndexOf("/") + 1;
            var slug = url.Substring(start).ToLowerInvariant();

            return slug;
        }
    }
}