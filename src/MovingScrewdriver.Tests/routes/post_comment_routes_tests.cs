using System;
using System.Web.Mvc;
using MovingScrewdriver.Web.Controllers.PostComment;
using MovingScrewdriver.Web.ViewModels;
using MvcContrib.TestHelper;
using Xunit;

namespace MovingScrewdriver.Tests.routes
{
    public class post_comment_routes_tests : routes_tests_base
    {
        private string _slug = Guid.NewGuid().ToString("n");
        private int _year = 2012;
        private int _month = 3;
        private int _day = 3;

         [Fact]
         public void should_match_post_validate_email()
         {
             var email = "test@test.pl";
             var id = 10;

             var route = "~/{0}/komentarz/sprawdz-email".FormatWith(id).WithMethod(HttpVerbs.Post);
             route.Values["commenterEmail"] = email;

             route.ShouldMapTo<PostCommentController>(a => a.ValidateEmail(email));
         }

         [Fact]
         public void should_match_post_add()
         {
             var comment = new CommentInput();
             var id = 10;

             var route = "~/{0}/komentarz/".FormatWith(id).WithMethod(HttpVerbs.Post);
             route.Values["input"] = comment;

             route.ShouldMapTo<PostCommentController>(a => a.Add(comment, id));
         }
    }
}