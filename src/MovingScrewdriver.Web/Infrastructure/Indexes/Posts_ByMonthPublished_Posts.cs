using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using MovingScrewdriver.Web.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace MovingScrewdriver.Web.Infrastructure.Indexes
{
    public class Posts_ByMonthPublished_Posts : AbstractMultiMapIndexCreationTask<Posts_ByMonthPublished_Posts.ReduceResult>
    {
        public class ReduceResult
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public IEnumerable<dynamic> Posts { get; set; }

        }

        public Posts_ByMonthPublished_Posts()
        {
            AddMap<Post>(posts => from post in posts
                           select new
                           {
                               Year = post.PublishAt.Year, 
                               Month = post.PublishAt.Month,
                               Posts = new List<dynamic>
                               {
                                   new
                                   {
                                       Id = post.Id,
                                       Slug = post.Slug,
                                       PublishAt = post.PublishAt,
                                       Title = post.Title,
                                       IsDeleted = post.IsDeleted
                                   }
                               }
                           });

            Reduce = results => from result in results
                                group result by new
                                {
                                    result.Year, 
                                    result.Month
                                }
                                into g
                                select new
                                {
                                    Year = g.Key.Year,
                                    Month = g.Key.Month,
                                    Posts = g.SelectMany(x => x.Posts)
                                };

            Sort(x=> x.Month, SortOptions.Int);
            Sort(x => x.Year, SortOptions.Int);
        }
    }
}
