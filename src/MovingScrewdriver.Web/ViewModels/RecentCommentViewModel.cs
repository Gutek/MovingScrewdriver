using System;

namespace MovingScrewdriver.Web.ViewModels
{
    public class RecentCommentViewModel
    {
        public string CommentId { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }

        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostSlug { get; set; }
        public DateTimeOffset PostPublishAt { get; set; }
    }
}