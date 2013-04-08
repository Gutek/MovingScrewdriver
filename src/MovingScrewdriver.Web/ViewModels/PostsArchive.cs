using System;
using System.Collections.Generic;
using System.Globalization;
using MovingScrewdriver.Web.Models;

namespace MovingScrewdriver.Web.ViewModels
{
    public class PostsArchiveViewModel : PagingViewModel
    {
        public IList<PostsInMonth> ByYearAndMonth { get; set; }
        
        public class PostsInMonth
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public string MonthInYearName
            {
                get
                {
                    return "{0} {1}".FormatWith(
                        CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month),
                        Year
                    );
                }
            }

            public IList<PostReference> Posts { get; set; }
        }
    }
}