using MovingScrewdriver.Web.Services;

namespace MovingScrewdriver.Web.Controllers.PostComment
{
    public partial class PostCommentController : AbstractController
    {
        private IAkismetService _akismetService;
        //public IAkismetService AkismetService { get; set; }

        //public PostCommentController()
        public PostCommentController(IAkismetService akismetService)
        {
            _akismetService = akismetService;
        }
    }
}