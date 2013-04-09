using System;
using System.Web.Mvc;
using MovingScrewdriver.Web.Infrastructure.ActionAttributes;
using MovingScrewdriver.Web.Models;
using MovingScrewdriver.Web.ViewModels;

namespace MovingScrewdriver.Web.Controllers.PostComment
{
    public partial class PostCommentController : AbstractController
    {
         [HttpPost]
         [AjaxOnly]
         [ValidateAntiForgeryToken]
         public ActionResult Add(CommentInput input, int id)
         {
             if (!ModelState.IsValid)
             {
                 return Json(new
                 {
                     error = ValidationErrors
                 });
             }

             if (BlogOwner.Email.Equals(input.CommenterEmail, StringComparison.OrdinalIgnoreCase)
                 && Request.IsAuthenticated == false)
             {
                 ModelState.AddModelError("CommenterEmail", "Nie możesz użyć tego adresu e-mail, należy on do właściciela bloga.");

                 return Json(new
                 {
                     error = ValidationErrors
                 });
             }

             var post = CurrentSession
                 .Include<Post>(x => x.CommentsId)
                 .Load(id);
             var comments = CurrentSession.Load<PostComments>(post.CommentsId);

             if (comments.AreCommentsClosed(post, BlogConfig.NumberOfDayToCloseComments)
                || post.AllowComments == false)
             {
                 return Json(new
                 {
                     error = "Komentarze zamknięte"
                 });
             }

             var comment = Mapper.Map<PostComments.Comment>(input);

             comment.Id = comments.GenerateNewCommentId();
             comment.IsSpam = _akismetService.CheckForSpam(comment);

             if (Request.IsAuthenticated && BlogOwner.Email.Equals(comment.Email, StringComparison.OrdinalIgnoreCase))
             {
                 //_akismetService.SubmitHam(comment);
                 comment.IsSpam = false;
                 // this is single user blog!
                 comment.Important = true;
             }

             if (comment.IsSpam && Request.IsAuthenticated == false)
             {
                 // add spam
                 comments.Spam.Add(comment);
             }
             else
             {
                 post.CommentsCount++;
                 comments.Comments.Add(comment);
             }

             var viewComment = Mapper.Map<PostViewModel.Comment>(comment);

             return PartialView(
                 comment.IsSpam ? "_spam" : "_comment"
                 , viewComment);
         }
    }
}