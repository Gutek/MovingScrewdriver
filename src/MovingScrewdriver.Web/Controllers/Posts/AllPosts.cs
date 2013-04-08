using System.Linq;
using System.Web.Mvc;
using MovingScrewdriver.Web.Extensions;
using MovingScrewdriver.Web.Models;
using Raven.Client;

namespace MovingScrewdriver.Web.Controllers.Posts
{
    public partial class PostsController : PostsPagingController
    {
        public ActionResult AllPosts()
        {
            RavenQueryStatistics stats;
            var posts = CurrentSession.Query<Post>()
                .Include(x => x.AuthorId)
                .Statistics(out stats)
                .WhereIsPublicPost()
                .OrderByDescending(post => post.PublishAt)
                .Paging(CurrentPage, DefaultPage, PageSize)
                .ToList();

            return PagedResult(stats.TotalResults, posts);
        }
    }
}