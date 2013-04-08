using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MovingScrewdriver.Web.Infrastructure;
using MovingScrewdriver.Web.Infrastructure.Indexes;
using MovingScrewdriver.Web.Models;
using Raven.Client;
using Raven.Client.Linq;

namespace MovingScrewdriver.Web.Extensions
{
    public static class DocumentSessionExtensions
    {
        public static IList<Tuple<PostComments.Comment, Post>> QueryForRecentComments(
            this IDocumentSession documentSession,
            Func<IRavenQueryable<PostComments_CreationDate.ReduceResult>, IQueryable<PostComments_CreationDate.ReduceResult>> processQuery)
        {
            var query = documentSession
                .Query<PostComments_CreationDate.ReduceResult, PostComments_CreationDate>()
                .Include(comment => comment.PostCommentsId)
                .Include(comment => comment.PostId)
                .OrderByDescending(x => x.Created)
                .Where(x => x.Created <= ApplicationTime.Current)
                .AsProjection<PostComments_CreationDate.ReduceResult>();

            var commentsIdentifiers = processQuery(query)
                .ToList();

            return (from commentIdentifier in commentsIdentifiers
                    let comments = documentSession.Load<PostComments>(commentIdentifier.PostCommentsId)
                    let post = documentSession.Load<Post>(commentIdentifier.PostId)
                    let comment = comments.Comments.FirstOrDefault(x => x.Id == commentIdentifier.CommentId)
                    where comment != null && post.IsDeleted == false
                    select Tuple.Create(comment, post))
                .ToList();
        }

        public static PostReference GetNextPrevPost(this IDocumentSession session, Post compareTo, bool isNext)
        {
            var queryable = session.Query<Post>()
                .WhereIsPublicPost();

            if (isNext)
            {
                queryable = queryable
                    .Where(post => post.PublishAt >= compareTo.PublishAt && post.Id != compareTo.Id)
                    .OrderBy(post => post.PublishAt);
            }
            else
            {
                queryable = queryable
                    .Where(post => post.PublishAt <= compareTo.PublishAt && post.Id != compareTo.Id)
                    .OrderByDescending(post => post.PublishAt);
            }

            var postReference = queryable
                .Select(p => new PostReference
                {
                    Id = p.Id,
                    Title = p.Title,
                    PublishAt = p.PublishAt,
                })
                .FirstOrDefault();

            return postReference;
        }

        public static BlogOwner GetCurrentUser(this IDocumentSession session)
        {
            if (HttpContext.Current.Request.IsAuthenticated == false)
            {
                return null;
            }
            
            var email = HttpContext.Current.User.Identity.Name;
            var user = session.GetUserByEmail(email);
            return user;
        }

        public static BlogOwner GetUserByEmail(this IDocumentSession session, string email)
        {
            return session.Query<BlogOwner>().FirstOrDefault(u => u.Email == email);
        }
    }
}