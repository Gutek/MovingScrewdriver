using System.Linq;
using System.Web.Mvc;
using MovingScrewdriver.Web.Extensions;
using MovingScrewdriver.Web.Infrastructure.Indexes;
using MovingScrewdriver.Web.Models;
using MovingScrewdriver.Web.ViewModels;
using Raven.Client;
using Raven.Client.Linq;

namespace MovingScrewdriver.Web.Controllers.PostsByDate
{
    public partial class PostsByDateController : PostsPagingController
    {
        public ActionResult Archive()
        {
            RavenQueryStatistics stats;
            var query = CurrentSession
                .Query<Posts_ByMonthPublished_Posts.ReduceResult, Posts_ByMonthPublished_Posts>()
                .Statistics(out stats)
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .AsProjection<Posts_ByMonthPublished_Posts.ReduceResult>()
                .Paging(CurrentPage, DefaultPage, PageSize)
                .ToList();

            var model = new PostsArchiveViewModel
            {
                CurrentPage = CurrentPage,
                PostsCount = stats.TotalResults,
                ByYearAndMonth = query.Select(x => new PostsArchiveViewModel.PostsInMonth
                {
                    Month = x.Month,
                    Year = x.Year,
                    Posts = x.Posts.Where(y => y.IsDeleted == false)
                    .OrderByDescending(y => y.PublishAt)
                    .Select(y => new PostReference
                    {
                        Id = y.Id,
                        Slug = y.Slug,
                        Title = y.Title,
                        PublishAt = y.PublishAt
                    }).ToList()
                }).ToList()
            };

            if (stats.TotalResults > 0 && model.ByYearAndMonth.Count == 0)
            {
                return View("archive_not_exists");
            }

            return View("archive", model);
        }
    }
}