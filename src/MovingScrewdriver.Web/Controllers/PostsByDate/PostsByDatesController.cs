using MovingScrewdriver.Web.Infrastructure.Validation;

namespace MovingScrewdriver.Web.Controllers.PostsByDate
{
    public partial class PostsByDateController : PostsPagingController
    {
        private readonly IArchiveDateValidator _dateValidator;

        public PostsByDateController(IArchiveDateValidator dateValidator)
        {
            _dateValidator = dateValidator;
        }
    }
}