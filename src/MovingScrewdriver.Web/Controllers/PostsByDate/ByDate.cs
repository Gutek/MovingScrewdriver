using System;
using System.Linq;
using System.Web.Mvc;
using MovingScrewdriver.Web.Extensions;
using MovingScrewdriver.Web.Infrastructure.Validation;
using MovingScrewdriver.Web.Models;
using Raven.Client;

namespace MovingScrewdriver.Web.Controllers.PostsByDate
{
    public partial class PostsByDateController : PostsPagingController
    {
        public ActionResult ByDate(int year, int? month, int? day)
        {
            var validateResult = _dateValidator.Validate(year, month, day);

            ViewBag.Year = year;

            if (validateResult.Success == false)
            {
                return return_validate_false_view(validateResult.Result);
            }

            RavenQueryStatistics stats;
            var query = CurrentSession.Query<Post>()
                .Include(x => x.AuthorId)
                .Statistics(out stats)
                .WhereIsPublicPost()
                .PublishedAt(year, month, day);

            set_view_bag_data(year, month, day);

            var posts = query
                .OrderByDescending(post => post.PublishAt)
                .Paging(CurrentPage, DefaultPage, PageSize)
                .ToList();

            return PagedResult(stats.TotalResults, posts);
        }

        private ActionResult return_validate_false_view(DateError error)
        {

            switch (error)
            {
                case DateError.FutureDate:
                    return View("future_posts");
                case DateError.MonthNotExists:
                case DateError.DayNotExists:
                    return View("date_not_exists");
                case DateError.BeforeBloging:
                    return View("past_posts");
                default:
                    return HttpNotFound();
            }
        }

        private void set_view_bag_data(int year, int? month, int? day)
        {
            if (month.HasValue && day.HasValue)
            {
                var dt = new DateTime(year, month.Value, day.Value);
                var monthName = dt.ToString("MMMM");

                ViewBag.Title = "{0} | {1} | {2}"
                    .FormatWith(day.Value, monthName, year);

                ViewBag.ArchiveTitle = "Wszystkie posty z dnia: {0}"
                    .FormatWith(dt.ToShortDateString());
            }
            else if (month.HasValue)
            {
                var dt = new DateTime(year, month.Value, 1);
                var monthName = dt.ToString("MMMM");

                ViewBag.Title = "{0} | {1}"
                    .FormatWith(monthName, year);

                ViewBag.ArchiveTitle = "Wszystkie posty z miesiąca: {0}"
                    .FormatWith(dt.ToString("MMMM yyyy"));
            }
            else
            {
                ViewBag.Title = "{0}"
                    .FormatWith(year);

                ViewBag.ArchiveTitle = "Wszystkie posty z roku: {0:0000}"
                    .FormatWith(year);
            }

            ViewBag.Year = year;
            ViewBag.Month = month;
            ViewBag.Day = day;
        }
    }
}