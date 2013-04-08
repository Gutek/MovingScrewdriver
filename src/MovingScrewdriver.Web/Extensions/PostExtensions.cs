using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using MovingScrewdriver.Web.Models;
using MovingScrewdriver.Web.ViewModels;

namespace MovingScrewdriver.Web.Extensions
{
    public static class PostExtensions
    {
        private static object ToRouteData(dynamic model)
        {
            var date = model.PublishAt;
            return new
            {
                year = date.Year,
                month = "{0:00}".FormatWith(date.Month as int?),
                day = "{0:00}".FormatWith(date.Day as int?),
                slug = @model.slug,
            };
        }

         public static object ToRouteData(this PostViewModel.PostDetails @this)
         {
             if (@this == null)
             {
                 throw new ArgumentNullException("this", "RouteData: Post cannot be null");
             }

             dynamic d = new
             {
                 PublishAt = @this.PublishedAt,
                 slug = @this.Slug
             };

             return ToRouteData(d);
         }

         public static object ToRouteData(this PostsViewModel.PostSummary @this)
         {
             if (@this == null)
             {
                 throw new ArgumentNullException("this", "RouteData: Post cannot be null");
             }

             dynamic d = new
             {
                 PublishAt = @this.PublishedAt,
                 slug = @this.Slug
             };

             return ToRouteData(d);
         }
         public static object ToRouteData(this RecentCommentViewModel @this)
         {
             if (@this == null)
             {
                 throw new ArgumentNullException("this", "RouteData: Post cannot be null");
             }

             dynamic d = new
             {
                 PublishAt = @this.PostPublishAt,
                 slug = @this.PostSlug
             };

             return ToRouteData(d);
         }
         public static object ToRouteData(this Post @this)
         {
             if (@this == null)
             {
                 throw new ArgumentNullException("this", "RouteData: Post cannot be null");
             }

             dynamic d = new
             {
                 PublishAt = @this.PublishAt,
                 slug = @this.Slug
             };

             return ToRouteData(d);
         }
         public static object ToRouteData(this PostReference @this)
         {
             if (@this == null)
             {
                 throw new ArgumentNullException("this", "RouteData: Post cannot be null");
             }

             dynamic d = new
             {
                 PublishAt = @this.PublishAt,
                 slug = @this.Slug
             };

             return ToRouteData(d);
         }

        public static bool TrackbackOrPingbackExists(this IList<PostComments.Comment> comments, string url)
        {
            var any = false;

            if (url.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("url", "Url cannot be null");
            }

            var ip = GeneralUtils.GetClientIp();

// ReSharper disable ReplaceWithSingleCallToAny
            any = comments
                .Where(x => x.Url.IsNotNullOrEmpty())
                .Where(x => x.UserHostAddress.IsNotNullOrEmpty())
                .Where(x => x.UserHostAddress.Equals(ip, StringComparison.Ordinal))
                .Where(x => x.Url.Equals(url, StringComparison.Ordinal))
                .Any();
// ReSharper restore ReplaceWithSingleCallToAny

            return any;
        }
    }
}