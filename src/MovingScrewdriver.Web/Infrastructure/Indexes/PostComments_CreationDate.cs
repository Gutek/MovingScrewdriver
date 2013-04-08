using System;
using System.Linq;
using MovingScrewdriver.Web.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace MovingScrewdriver.Web.Infrastructure.Indexes
{
    public class PostComments_CreationDate : AbstractIndexCreationTask<PostComments, PostComments_CreationDate.ReduceResult>
    {
        public class ReduceResult
        {
            public DateTimeOffset Created { get; set; }
            public int CommentId { get; set; }
            public string PostCommentsId { get; set; }
            public string PostId { get; set; }
            public string Url { get; set; }
            public DateTimeOffset PostPublishAt { get; set; }
        }

        public PostComments_CreationDate()
        {
            Map = postComments => from postComment in postComments
                                  from comment in postComment.Comments
                                  where comment.IsSpam == false
                                  select new
                                  {
                                      Created = comment.Created,
                                      CommentId = comment.Id,
                                      PostCommentsId = postComment.Id,
                                      PostId = postComment.Post.Id,
                                      PostPublishAt = postComment.Post.Published,
                                      Url = comment.Url
                                  };

            Store(x => x.Created, FieldStorage.Yes);
            Store(x => x.CommentId, FieldStorage.Yes);
            Store(x => x.PostId, FieldStorage.Yes);
            Store(x => x.PostCommentsId, FieldStorage.Yes);
            Store(x => x.PostPublishAt, FieldStorage.Yes);
            Store(x => x.Url, FieldStorage.Yes);
        }
    }
}