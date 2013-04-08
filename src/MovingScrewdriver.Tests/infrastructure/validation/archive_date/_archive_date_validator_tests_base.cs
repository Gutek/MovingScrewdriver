using MovingScrewdriver.Web.Infrastructure.Validation;

namespace MovingScrewdriver.Tests.infrastructure.validation.archive_date
{
    public class archive_date_validator_tests_base : ravendb_test_base
    {
        protected readonly ArchiveDateValidator _validator;

        public archive_date_validator_tests_base()
        {
            _validator = new ArchiveDateValidator(get_session());
        }
    }
}