using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MovingScrewdriver.Web.Extensions;
using MovingScrewdriver.Web.Infrastructure;
using MovingScrewdriver.Web.Models;
using MovingScrewdriver.Web.ViewModels;
using Raven.Client;

namespace MovingScrewdriver.Web.Controllers.PostDetails
{
    public partial class PostDetailsController : AbstractController
    {
        public ActionResult Details(int year, int month, int day, string slug)
        {
            var post = CurrentSession
                .Query<Post>()
                .Include<Post>(x => x.CommentsId)
                .PublishedAt(year, month, day)
                .WithSlug(slug)
                .WhereIsPublicPost()
                .FirstOrDefault();

            if (post == null)
            {
                return HttpNotFound();
            }
            
            //if (post.NotificationSend == false)
            //{
            //    _notification.SendAsync(post, new Uri(Url.AbsoluteAction("Details", "PostDetails", post.ToRouteData())));
            //    post.NotificationSend = true;
            //}

            var comments = CurrentSession.Load<PostComments>(post.CommentsId) ?? new PostComments();
            var vm = new PostViewModel
            {
                Post = Mapper.Map<PostViewModel.PostDetails>(post),
                Comments = Mapper.Map<IList<PostViewModel.Comment>>(comments.Comments),
                NextPost = CurrentSession.GetNextPrevPost(post, true),
                PreviousPost = CurrentSession.GetNextPrevPost(post, false),
                AreCommentsClosed = comments.AreCommentsClosed(post, BlogConfig.NumberOfDayToCloseComments),
            };

            vm.Post.Author = Mapper.Map<AuthorDetails>(BlogOwner);

            return View("Details", vm);
        }
    }
}