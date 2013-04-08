using MovingScrewdriver.Web.Services;

namespace MovingScrewdriver.Web.Controllers.Services
{
    public partial class ServicesController : AbstractController
    {
        private readonly IAkismetService _akismetService;

        public ServicesController(IAkismetService akismetService)
        {
            _akismetService = akismetService;
        }
    }
}