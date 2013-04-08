using System;
using System.Linq;
using MovingScrewdriver.Web.Models;

namespace MovingScrewdriver.Web.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paging<T>(this IQueryable<T> query, int currentPage, int defaultPage, int pageSize)
        {
            return query
                .Skip((currentPage - defaultPage) * pageSize)
                .Take(pageSize);
        }

        public static IQueryable<Post> WhereIsPublicPost(this IQueryable<Post> query)
        {
            return query
                .Where(post => post.PublishAt < DateTimeOffset.Now.AsMinutes() && post.IsDeleted == false);
        }

        public static IQueryable<Post> PublishedAt(this IQueryable<Post> query, int year, int? month, int? day)
        {
            query = query.Where(x => x.PublishAt.Year == year);

            if (month.HasValue)
            {
                query = query.Where(x => x.PublishAt.Month == month.Value);
            }

            // if day has value, but month does not, we are doing it wrong
            if (day.HasValue && month.HasValue)
            {
                query = query.Where(x => x.PublishAt.Day == day.Value);
            }

            return query;
        }
        public static IQueryable<Post> WithSlug(this IQueryable<Post> query, string slug, bool legacy = false)
        {
            if (legacy)
            {
                return query.Where(x => x.LegacySlug.Equals(slug, StringComparison.OrdinalIgnoreCase));
            }

            return query.Where(x => x.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
        }

        public static IQueryable<Post> WithCategory(this IQueryable<Post> query, string slug)
        {
            return query.Where(x => x.CategoriesAsSlugs.Any(cat => cat == slug));
        }

        public static IQueryable<Post> WithTag(this IQueryable<Post> query, string slug)
        {
            return query.Where(x => x.TagsAsSlugs.Any(cat => cat == slug));
        }
    }
}