using System;
using MovingScrewdriver.Web.Infrastructure;
using MovingScrewdriver.Web.Infrastructure.Validation;
using MovingScrewdriver.Web.Models;
using Xunit;

namespace MovingScrewdriver.Tests.infrastructure.validation.archive_date
{
    public class month_validation_tests : archive_date_validator_tests_base
    {
         [Fact]
         public void should_not_throw_exception()
         {
             var currentDate = ApplicationTime.Current;

             Assert.DoesNotThrow(() => _validator.Validate(currentDate.Year, currentDate.Month, null));
             Assert.DoesNotThrow(() => _validator.Validate(currentDate.Year + 50, currentDate.Month + 50, null));
             Assert.DoesNotThrow(() => _validator.Validate(currentDate.Year - 50, currentDate.Month - 50, null));
         }


         [Fact]
         public void should_return_data_error_month_not_exists_for_month_outside_values_1_12()
         {
             var currentDate = ApplicationTime.Current;
             var result1 = _validator.Validate(currentDate.Year, 50, null);
             var result2 = _validator.Validate(currentDate.Year, 0, null);
             var result3 = _validator.Validate(currentDate.Year, -2, null);

             Assert.Equal(DateError.MonthNotExists, result1.Result);
             Assert.Equal(DateError.MonthNotExists, result2.Result);
             Assert.Equal(DateError.MonthNotExists, result3.Result);
         }

         [Fact]
         public void should_return_data_error_future_date_for_month_older_then_current_date()
         {
             ApplicationTime._replaceCurrentTimeLogic(() => new DateTimeOffset(2012, 1, 1, 1, 1, 1, TimeSpan.FromDays(0)));
             var currentDate = ApplicationTime.Current;
             
             var result = _validator.Validate(currentDate.Year, currentDate.Month + 1, null);

             Assert.Equal(DateError.FutureDate, result.Result);

             ApplicationTime._revertToDefaultLogic();
         }

         [Fact]
         public void should_return_success_for_current_year_and_month()
         {

             ApplicationTime._replaceCurrentTimeLogic(() => new DateTimeOffset(2012, 1, 1, 1, 1, 1, TimeSpan.FromDays(0)));
             var currentDate = ApplicationTime.Current;

             var result = _validator.Validate(currentDate.Year, currentDate.Month , null);
             
             ApplicationTime._revertToDefaultLogic();

             Assert.True(result.Success, "checking for current year and month should always return true");
         }

         [Fact]
         public void should_return_data_error_none_for_current_year_and_month()
         {
             ApplicationTime._replaceCurrentTimeLogic(() => new DateTimeOffset(2012, 1, 1, 1, 1, 1, TimeSpan.FromDays(0)));
             var currentDate = ApplicationTime.Current;

             var result = _validator.Validate(currentDate.Year, currentDate.Month, null);

             ApplicationTime._revertToDefaultLogic();

             Assert.Equal(DateError.None, result.Result);
         }
    }
}