using MovingScrewdriver.Web.Infrastructure;
using Raven.Client;

namespace MovingScrewdriver.Web.Controllers.PostDetails
{
    public partial class PostDetailsController : AbstractController
    {
        private readonly INotificationService _notification;


        //public INotificationService Notification { get; set; }

        public PostDetailsController(INotificationService notification)
        {
            _notification = notification;
        }
    }
}