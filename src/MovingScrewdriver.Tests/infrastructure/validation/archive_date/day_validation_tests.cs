using System;
using MovingScrewdriver.Web.Infrastructure;
using MovingScrewdriver.Web.Infrastructure.Validation;
using Xunit;

namespace MovingScrewdriver.Tests.infrastructure.validation.archive_date
{
    public class day_validation_tests : archive_date_validator_tests_base
    {
        public override void Dispose()
        {
            base.Dispose();
            ApplicationTime._revertToDefaultLogic();
        }

         [Fact]
         public void should_not_throw_exception()
         {
             var currentDate = ApplicationTime.Current;

             Assert.DoesNotThrow(() => _validator.Validate(currentDate.Year, currentDate.Month, currentDate.Day));
             Assert.DoesNotThrow(() => _validator.Validate(currentDate.Year + 50, currentDate.Month + 50, currentDate.Day + 50));
             Assert.DoesNotThrow(() => _validator.Validate(currentDate.Year - 50, currentDate.Month - 50, currentDate.Day - 50));
         }


         [Fact]
         public void should_return_data_error_day_not_exists_for_day_outside_months_day()
         {
             ApplicationTime._replaceCurrentTimeLogic(() => new DateTimeOffset(2012, 1, 1, 1, 1, 1, TimeSpan.FromDays(0)));
             var currentDate = ApplicationTime.Current;

             var result1 = _validator.Validate(currentDate.Year, currentDate.Month, 0);
             var result2 = _validator.Validate(currentDate.Year, currentDate.Month, 32);
             
             Assert.Equal(DateError.DayNotExists, result1.Result);
             Assert.Equal(DateError.DayNotExists, result2.Result);
         }

         [Fact]
         public void should_return_data_error_future_date_for_day_older_then_current_date()
         {
             ApplicationTime._replaceCurrentTimeLogic(() => new DateTimeOffset(2012, 1, 1, 1, 1, 1, TimeSpan.FromDays(0)));
             var currentDate = ApplicationTime.Current;
             
             var result = _validator.Validate(currentDate.Year, currentDate.Month, currentDate.Day + 1);

             Assert.Equal(DateError.FutureDate, result.Result);

         }

         [Fact]
         public void should_return_success_for_current_year_and_month_and_day()
         {

             ApplicationTime._replaceCurrentTimeLogic(() => new DateTimeOffset(2012, 1, 1, 1, 1, 1, TimeSpan.FromDays(0)));
             var currentDate = ApplicationTime.Current;

             var result = _validator.Validate(currentDate.Year, currentDate.Month , currentDate.Day);
             
             Assert.True(result.Success, "checking for current year and month and day should always return true");
         }

         [Fact]
         public void should_return_data_error_none_for_current_year_and_month_and_day()
         {
             ApplicationTime._replaceCurrentTimeLogic(() => new DateTimeOffset(2012, 1, 1, 1, 1, 1, TimeSpan.FromDays(0)));
             var currentDate = ApplicationTime.Current;

             var result = _validator.Validate(currentDate.Year, currentDate.Month, currentDate.Day);

             ApplicationTime._revertToDefaultLogic();

             Assert.Equal(DateError.None, result.Result);
         }
    }
}