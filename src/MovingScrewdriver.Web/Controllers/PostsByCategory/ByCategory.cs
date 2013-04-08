using System;
using System.Linq;
using System.Web.Mvc;
using MovingScrewdriver.Web.Extensions;
using MovingScrewdriver.Web.Infrastructure;
using MovingScrewdriver.Web.Models;
using Raven.Client;

namespace MovingScrewdriver.Web.Controllers.PostsByCategory
{
    public partial class PostsByCategoryController : PostsPagingController
    {
        public ActionResult ByCategory(string slug)
        {
            RavenQueryStatistics stats;
            // we need query to get suggestions
            var query = CurrentSession.Query<Post>()
                // this will cause that Load<BlogOwner> will not go to server! :)
               .Include(x => x.AuthorId)
               .Statistics(out stats)
               .WhereIsPublicPost()
               .WithCategory(slug)
               .OrderByDescending(post => post.PublishAt)
               .Paging(CurrentPage, DefaultPage, PageSize)
            ;

            Func<Post, string> resolveSlug = (post) =>
            {
                if (post == null)
                {
                    post = CurrentSession
                        .Query<Post>()
                        .WithCategory(slug)
                        .First();
                }

                return post.Categories.First(x => x.Slug == slug).Title;
            };

            return FilteredPagedResult(
                stats
                , query
                , slug
                , resolveSlug
            );
        }
    }
}