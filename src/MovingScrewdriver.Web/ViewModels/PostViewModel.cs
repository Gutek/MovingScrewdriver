using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MovingScrewdriver.Web.Models;

namespace MovingScrewdriver.Web.ViewModels
{
    public class PostViewModel
    {
        public PostReference PreviousPost { get; set; }
        public PostReference NextPost { get; set; }

        public PostDetails Post { get; set; }
        public IList<Comment> Comments { get; set; }

        public bool AreCommentsClosed { get; set; }

        public class Comment
        {
            public int Id { get; set; }
            public MvcHtmlString Content { get; set; }
            public string Author { get; set; }
            public CommentType Type { get; set; }
            public string Url { get; set; }    // Look for HTML injection.
            public string EmailHash { get; set; }
            public DateTimeOffset CreatedAt { get; set; }

            public bool IsImportant { get; set; }

            public bool IsTrackback { get { return Type == CommentType.Trackback; } }
            public bool IsPingback { get { return Type == CommentType.Pingback; } }
        }

        public class PostDetails
        {
            public int Id { get; set; }
            public MvcHtmlString Title { get; set; }
            public string Slug { get; set; }
            public MvcHtmlString Content { get; set; }
            public string Description { get; set; }

            public DateTimeOffset CreatedAt { get; set; }
            public DateTimeOffset PublishedAt { get; set; }
            public bool IsCommentAllowed { get; set; }

            public ICollection<TagDetails> Tags { get; set; }
            public ICollection<TagDetails> Categories { get; set; }

            public AuthorDetails Author { get; set; }
        }
    }
}