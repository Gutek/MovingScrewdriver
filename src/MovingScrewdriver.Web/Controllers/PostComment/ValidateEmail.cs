using System.Web.Mvc;
using MovingScrewdriver.Web.Extensions;

namespace MovingScrewdriver.Web.Controllers.PostComment
{
    public partial class PostCommentController : AbstractController
    {
        [HttpPost]
        public ActionResult ValidateEmail(string commenterEmail)
        {
            var user = CurrentSession.GetUserByEmail(commenterEmail);

            if (user != null && Request.IsAuthenticated == false)
            {
                return Json(new
                {
                    error = "Nie możesz użyć tego adresu email."
                });
            }
            return Json(new
            {
                success = "Wszystko OK."
            });
        }
    }
}