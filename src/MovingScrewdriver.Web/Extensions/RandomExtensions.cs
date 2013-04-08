using System;
using MovingScrewdriver.Web.Infrastructure.Indexes;

namespace MovingScrewdriver.Web.Extensions
{
    public static class RandomExtensions
    {
         public static string GetMonthName(this Posts_ByMonthPublished_Posts.ReduceResult @this)
         {
             var date = new DateTime(@this.Year, @this.Month, 1);

             return date.ToString("MMMM yyyy");
         }
    }
}