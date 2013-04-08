using MovingScrewdriver.Web.Services;

namespace MovingScrewdriver.Web.Controllers.PostComment
{
    public partial class PostCommentController : AbstractController
    {
        private readonly IAkismetService _akismetService;

        public PostCommentController(IAkismetService akismetService)
        {
            _akismetService = akismetService;
        }
    }
}