using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MovingScrewdriver.Web.Models;
using MovingScrewdriver.Web.ViewModels;
using Raven.Client;

namespace MovingScrewdriver.Web.Controllers
{
    public abstract class PostsPagingController : AbstractController
    {
        private const string PageKey = "page";

        private IDisposable _aggressivelyCacheFor;

        public const int DefaultPage = 1;
        public const int PageSize = 25;

        protected TimeSpan CacheDuration
        {
            get
            {
                return TimeSpan.FromMinutes(5);
            }
        }

        protected int CurrentPage
        {
            get
            {
                var pageRouteValue = RouteData.Values[PageKey] as string;
                int result;

                if (int.TryParse(pageRouteValue, out result))
                {
                    return Math.Max(DefaultPage, result);
                }

                return DefaultPage;
            }
        }

        protected ActionResult PagedResult(
            int totalRecords
            , IEnumerable<Post> posts
        )
        {
            var summaries = Mapper.Map<IEnumerable<PostsViewModel.PostSummary>>(posts).ToList();
            var author = Mapper.Map<AuthorDetails>(BlogOwner);

            foreach (var postSummary in summaries)
            {
                postSummary.Author = author;
            }

            if (summaries.Count == 0)
            {
                return View("wrong_page", new PostsViewModel
                {
                    CurrentPage = CurrentPage,
                    PostsCount = totalRecords
                });
            }

            return View("list", new PostsViewModel
            {
                CurrentPage = CurrentPage,
                PostsCount = totalRecords,
                Posts = summaries
            });
        }

        protected ActionResult FilteredPagedResult(
            RavenQueryStatistics stats
            , IQueryable<Post> query
            , string slug
            , Func<Post, string> resolveSlug)
        {
            ViewBag.Slug = slug;

            var posts = query.ToList();

             // nothing returned by filter, this means that it does not exists!
             // if posts are empty, but TotalResults contains values
             // it means that we are on page that will not display anything
             if (stats.TotalResults == 0)
             {
                 var suggestions = query.Suggest();

                 if (suggestions.Suggestions.Length > 0)
                 {
                     return View("suggest", suggestions.Suggestions);
                 }

                 return HttpNotFound();
             }
            
            var resolvedSlug = resolveSlug(posts.FirstOrDefault());
            ViewBag.ResolvedSlug = resolvedSlug;
            return PagedResult(stats.TotalResults, posts);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            _aggressivelyCacheFor = CurrentSession.Advanced.DocumentStore.AggressivelyCacheFor(CacheDuration);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (_aggressivelyCacheFor != null)
            {
                _aggressivelyCacheFor.Dispose();
                _aggressivelyCacheFor = null;
            }
        }
    }
}