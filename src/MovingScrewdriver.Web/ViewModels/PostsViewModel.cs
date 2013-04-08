using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MovingScrewdriver.Web.ViewModels
{
    public class PostsViewModel : PagingViewModel
    {
        public IList<PostSummary> Posts { get; set; }

        public class PostSummary
        {
            public int Id { get; set; }
            public MvcHtmlString Title { get; set; }
            public string Slug { get; set; }
            public string Description { get; set; }
            
            public ICollection<TagDetails> Tags { get; set; }
            public ICollection<TagDetails> Categories { get; set; }
            public DateTimeOffset PublishedAt { get; set; }
            public int CommentsCount { get; set; }
            public AuthorDetails Author { get; set; }
        }

        public PostsViewModel()
        {
            Posts = new List<PostSummary>();
        }
    }
}