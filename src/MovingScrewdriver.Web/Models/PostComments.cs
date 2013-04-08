using System;
using System.Collections.Generic;
using System.Linq;
using MovingScrewdriver.Web.Infrastructure;

namespace MovingScrewdriver.Web.Models
{
    public class PostComments
    {
        public string Id { get; set; }
        public PostReference Post { get; set; }

        public List<Comment> Comments { get; set; }
        public List<Comment> Spam { get; set; }

        public PostComments()
        {
            Comments = new List<Comment>();
            Spam = new List<Comment>();
        }

        public int LastCommentId { get; set; }
        public int GenerateNewCommentId()
        {
            return ++LastCommentId;
        }

        public bool AreCommentsClosed(Post post, int numberOfDayToCloseComments)
        {
            if (numberOfDayToCloseComments < 1)
            {
                return false;
            }
            
            var lastCommentDate = Comments.Count == 0 ? post.PublishAt : Comments.Max(x => x.Created);
            return ApplicationTime.Current - lastCommentDate > TimeSpan.FromDays(numberOfDayToCloseComments);
        }

        public class PostReference
        {
            public string Id { get; set; }
            public DateTimeOffset Published { get; set; }
            public string Slug { get; set; }
        }

        public class Comment
        {
            public int Id { get; set; }
            public string Content { get; set; }
            public string Author { get; set; }
            public string Email { get; set; }
            public string Url { get; set; }

            public bool Important { get; set; }
            public bool IsSpam { get; set; }
            public CommentType Type { get; set; }

            public DateTimeOffset Created { get; set; }

            public string UserHostAddress { get; set; }
            public string UserAgent { get; set; }
        }
    }
}