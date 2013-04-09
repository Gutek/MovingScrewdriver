using System.Globalization;
using System.Text.RegularExpressions;

namespace System
{
    public static class IntExtensions
    {
         public static string GetCommentsText(this int @this)
         {
             var regex = new Regex("[234]$");
             var regexTenths = new Regex("(12)|(13)|(14)$");
             var count = @this.ToString(CultureInfo.InvariantCulture);
    
            if (@this == 1)
            {
                return "komentarz";
            }

            if (regex.IsMatch(count) && regexTenths.IsMatch(count) == false)
            {
                return "komentarze";
            }

             return "komentarzy";
         }

        public static string ToPostId(this int @this)
        {
            return "posts/{0}".FormatWith(@this);
        }
    }
}