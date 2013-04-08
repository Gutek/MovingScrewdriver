using System;
using System.Linq;
using MovingScrewdriver.Web.Models;
using Raven.Client;

namespace MovingScrewdriver.Web.Infrastructure.Validation
{
    public interface IArchiveDateValidator
    {
        ValidationResult<DateError> Validate(int year, int? month, int? day);
    }

    public class ArchiveDateValidator : IArchiveDateValidator
    {
        public ValidationResult<DateError> Validate(int year, int? month, int? day)
        {
            //var result = new ValidationResult<DateError>();
            var currentDate = ApplicationTime.Current;

            Func<DateError, ValidationResult<DateError>> error = err =>
            {
                var result = new ValidationResult<DateError>();
                result.Success = false;
                result.Result = err;

                return result;
            };

            // not doing this DateTime.TryParse() as I want to show different messages depending
            // on the date

            if (year > currentDate.Year)
            {
                return error(DateError.FutureDate);
            }
            
            if (month.HasValue)
            {
                if (month.Value > 12 
                    || month.Value < 1)
                {
                    return error(DateError.MonthNotExists);
                }

                if (month.Value > currentDate.Month
                    && year == currentDate.Year)
                {
                    return error(DateError.FutureDate);
                }
            }

            if (month.HasValue
                && day.HasValue)
            {
                var daysInMonth = DateTime.DaysInMonth(year, month.Value);

                if (day.Value < 1 
                    || day.Value > daysInMonth)
                {
                    return error(DateError.DayNotExists);
                }

                if (day.Value > currentDate.Day
                    && year == currentDate.Year
                    && month.Value == currentDate.Month)
                {
                    return error(DateError.FutureDate);
                }
            }

            var firstPost = _currentSession.Query<Post>()
                .Where(post => post.PublishAt.Year != 0)
                .OrderBy(post => post.PublishAt)
                .Select(post => post.PublishAt)
                .FirstOrDefault();
            
            if (firstPost.Year > year)
            {
                return error(DateError.BeforeBloging);
            }

            return new ValidationResult<DateError>
            {
                Success = true
            };
        }

        private readonly IDocumentSession _currentSession;

        public ArchiveDateValidator(IDocumentSession currentSession)
        {
            _currentSession = currentSession;
        }
    }

    public class ValidationResult<T>
        where T:struct
    {
        public bool Success { get; set; }
        public T Result { get; set; }
    }

    public enum DateError
    {
        None,
        FutureDate,
        MonthNotExists,
        DayNotExists,
        BeforeBloging
    }
}