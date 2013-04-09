using System;
using Akismet.NET;
using MovingScrewdriver.Web.Controllers.Services;
using MovingScrewdriver.Web.Extensions;
using MovingScrewdriver.Web.Models;
using NLog;
using Raven.Client;

namespace MovingScrewdriver.Web.Services
{
    public interface IAkismetService
    {
        bool CheckForSpam(PostComments.Comment comment);
        void SubmitSpam(PostComments.Comment comment);
        void SubmitHam(PostComments.Comment comment);
    }

    public class AkismetService : IAkismetService
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        private IValidator get_api()
        {
            var api = new Validator(AksimetKey);

            if (api.VerifyKey(Domain) == false)
            {
                
                return null;
            }

            return api;
        }

        private Comment get_comment(PostComments.Comment comment)
        {
            return new Comment
            {
                blog = Domain,
                user_ip = comment.UserHostAddress,
                user_agent = comment.UserAgent,
                comment_author = comment.Author,
                comment_author_email = comment.Email,
                comment_author_url = comment.Url,
                comment_content = comment.Content,
                comment_type = comment.Type.ToString().ToLowerInvariant()
            };
        }

        public bool CheckForSpam(PostComments.Comment comment)
        {
            var api = get_api();
            if (api == null)
            {
                return false;
            }

            var ac = get_comment(comment);

#if DEBUG
            return false;
#else
            var isSpam = api.IsSpam(ac);

            _log.Trace("Comment: {0} is Spam: {1}", comment.ToJson(), isSpam);

            return isSpam;
#endif
        }

        public void SubmitSpam(PostComments.Comment comment)
        {
            var api = get_api();
            if (api == null)
            {
                return;
            }

            var ac = get_comment(comment);
            
#if !DEBUG
            api.SubmitSpam(ac);
#endif
        }

        public void SubmitHam(PostComments.Comment comment)
        {
            var api = get_api();
            if (api == null)
            {
                return;
            }

            var ac = get_comment(comment);

#if !DEBUG
            api.SubmitHam(ac);
#endif

        }

        private string _domain;
        private string _aksimetKey;

        public string Domain
        {
            get { return _domain ?? (_domain = GeneralUtils.BlogUrl()); }
        }

        public string AksimetKey
        {
            get
            {
                if (_aksimetKey.IsNullOrEmpty())
                {
                    //using(var session = _store.OpenSession())
                    {
                        _aksimetKey = _session.Load<ScrewdriverConfig>("Blog/Config").AkismetKey;
                    }
                }

                return _aksimetKey;
            }
        }
        
        private readonly IDocumentSession _session;
        public AkismetService(IDocumentSession session)
        {
            _session = session;
        }
    }
}