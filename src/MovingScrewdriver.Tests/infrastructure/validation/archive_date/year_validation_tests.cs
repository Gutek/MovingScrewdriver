using MovingScrewdriver.Web.Infrastructure;
using MovingScrewdriver.Web.Infrastructure.Validation;
using MovingScrewdriver.Web.Models;
using Xunit;

namespace MovingScrewdriver.Tests.infrastructure.validation.archive_date
{
    public class year_validation_tests : archive_date_validator_tests_base
    {
         [Fact]
         public void should_not_throw_exception()
         {
             var currentDate = ApplicationTime.Current;

             Assert.DoesNotThrow(() => _validator.Validate(currentDate.Year, null, null));
             Assert.DoesNotThrow(() => _validator.Validate(currentDate.Year + 50, null, null));
             Assert.DoesNotThrow(() => _validator.Validate(currentDate.Year - 50, null, null));
         }

         [Fact]
         public void should_return_success_for_current_year()
         {
             var currentDate = ApplicationTime.Current;
             var result = _validator.Validate(currentDate.Year, null, null);

             Assert.True(result.Success, "checking for current year only should always return true");
         }

         [Fact]
         public void should_return_data_error_none_for_current_year()
         {
             var currentDate = ApplicationTime.Current;
             var result = _validator.Validate(currentDate.Year, null, null);

             Assert.Equal(DateError.None, result.Result);
         }
         [Fact]
         public void should_faild_for_future_year()
         {
             var currentDate = ApplicationTime.Current;
             var result = _validator.Validate(currentDate.Year + 1, null, null);

             Assert.False(result.Success, "checking for future year should always return false");
         }

         [Fact]
         public void should_return_data_error_future_year_for_future_year()
         {
             var currentDate = ApplicationTime.Current;
             var result = _validator.Validate(currentDate.Year + 1, null, null);

             Assert.Equal(DateError.FutureDate, result.Result);
         }

        [Fact]
        public void should_return_data_error_before_bloging_if_year_is_earlier_then_first_post()
        {
            var currentDate = ApplicationTime.Current;
            var post = new Post();
            post.Title = "test";
            post.Created = post.Modified = post.PublishAt = currentDate;
            set_data(session => session.Store(post));

            var result = _validator.Validate(currentDate.Year - 1, null, null);

            Assert.Equal(DateError.BeforeBloging, result.Result);
        }
    }
}