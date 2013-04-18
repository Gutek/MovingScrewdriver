using System;
using System.Linq;
using MovingScrewdriver.Web.Extensions;
using MovingScrewdriver.Web.Models;
using Raven.Client;
using Raven.Client.Linq;

namespace MovingScrewdriver.Web.Infrastructure
{
    public interface IScheduleStrategy
    {
        DateTimeOffset Schedule();
        DateTimeOffset Schedule(DateTimeOffset requestedDate);
    }

    public class ScheduleStrategy : IScheduleStrategy
    {
        private readonly IDocumentStore _store;

        public ScheduleStrategy(IDocumentStore store)
        {
            _store = store;
        }

        public DateTimeOffset Schedule()
        {
            using (var session = _store.OpenSession())
            {
                var p = session.Query<Post>()
                .OrderByDescending(post => post.PublishAt)
                .Select(post => new { post.PublishAt })
                .FirstOrDefault();

                var now = ApplicationTime.Current;
                var lastScheduledPostDate = p == null || p.PublishAt < now ? now : p.PublishAt;
                return lastScheduledPostDate
                    .AddDays(1)
                    .SkipToNextWorkDay()
                    .AtNoon();
            }
        }

        public DateTimeOffset Schedule(DateTimeOffset requestedDate)
        {
            using (var session = _store.OpenSession())
            {
                var now = ApplicationTime.Current;
                var postsQuery = from p in session.Query<Post>().Include(x => x.CommentsId)
                                 where p.PublishAt > requestedDate && p.PublishAt > now
                                 orderby p.PublishAt
                                 select p;

                var nextPostDate = requestedDate;
                foreach (var post in postsQuery)
                {
                    nextPostDate = nextPostDate
                            .AddDays(1)
                            .SkipToNextWorkDay()
                            .AtTime(post.PublishAt);

                    post.PublishAt = nextPostDate;

                    if (post.CommentsId != null)
                    {
                        session.Load<PostComments>(post.CommentsId).Post.Published = nextPostDate;
                    }
                }

                return requestedDate;
            }
        }
    }
}