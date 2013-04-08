using System;
using System.Web.Mvc;
using System.Xml.Linq;
using MovingScrewdriver.Web.Extensions;
using MovingScrewdriver.Web.Models;
using MovingScrewdriver.Web.ViewModels;

namespace MovingScrewdriver.Web.Controllers.Services
{
    public partial class ServicesController : AbstractController
    {
        // http://www.sixapart.com/labs/trackback/
        // http://www.movabletype.org/documentation/trackback_manual.html
        [AcceptVerbs("POST", "PUT")]
        public ActionResult Trackback(string blog_name, 
            string url,
            string title,
            string excerpt, 
            int id)
        {
            var model = new TrackbackInput
            {
                blog_name = blog_name,
                url = url,
                title = title,
                excerpt = excerpt
            };

            if (model.url.IsNullOrWhiteSpace())
            {
                return TrackbackError("URL Is Missing");
            }

            var post = CurrentSession
                .Include<Post>(x => x.CommentsId)
                .Load<Post>(id);

            if (post == null)
            {
                return RedirectToAction("AllPosts", "Posts");
            }

            var postUrl = Url.Action("Details", "PostDetails", post.ToRouteData());
            if (postUrl.ExistsOn(model.url) == false)
            {
                return TrackbackError("The source page does not link");
            }

            var comments = CurrentSession.Load<PostComments>(post.CommentsId);
            
            if (comments.AreCommentsClosed(post, BlogConfig.NumberOfDayToCloseComments) 
                || post.AllowComments == false)
            {
                return TrackbackError("Comments closed");
            }
            
            var trackbackExists = comments.Comments.TrackbackOrPingbackExists(model.url);

            if (trackbackExists)
            {
                return TrackbackError("Trackback already registered");
            }

            var comment = Mapper.Map<PostComments.Comment>(model);

            comment.Id = comments.GenerateNewCommentId();
            comment.IsSpam = _akismetService.CheckForSpam(comment);

            if (comment.IsSpam)
            {
                comments.Spam.Add(comment);
            }
            else
            {
                post.CommentsCount++;
                comments.Comments.Add(comment);
            }

            return TrackbackSuccess();
        }


        private ActionResult TrackbackError(string error)
        {
            var response = new XDocument(
                new XElement("response",
                    new XElement("error", error)
                )
            );

            return XDoc(response);
        }

        private ActionResult TrackbackSuccess()
        {
            var response = new XDocument(
                new XElement("response",
                             new XElement("error", "0")
                            )
            );

            return XDoc(response);
        }

    }
}