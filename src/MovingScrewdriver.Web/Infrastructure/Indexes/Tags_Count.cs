using System;
using System.Linq;
using MovingScrewdriver.Web.Models;
using Raven.Client.Indexes;

namespace MovingScrewdriver.Web.Infrastructure.Indexes
{
    public class Tags_Count : AbstractIndexCreationTask<Post, Tags_Count.ReduceResult>
    {
        public class ReduceResult
        {
            public string Name { get; set; }
            public string Slug { get; set; }
            public int Count { get; set; }
            public DateTimeOffset LastSeenAt { get; set; }
        }

        public Tags_Count()
        {
            Map = posts => from post in posts
                           from tag in post.Tags
                           select new
                           {
                               Name = tag.Title.ToLower(), 
                               Slug = tag.Slug,
                               Count = 1, 
                               LastSeenAt = post.PublishAt
                           };

            Reduce = results => from tagCount in results
                                group tagCount by tagCount.Name
                                into g
                                select new
                                {
                                    Name = g.Key, 
                                    Slug = g.Select(x => x.Slug).First(),
                                    Count = g.Sum(x => x.Count), 
                                    LastSeenAt = g.Max(x => x.LastSeenAt)
                                };
        }
    }
}