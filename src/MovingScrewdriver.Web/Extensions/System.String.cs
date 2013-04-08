using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace System
{
    public static class StringExtensions
    {
        private static readonly Regex RegexStripHtml = new Regex("<[^>]*>", RegexOptions.Compiled);
        private static readonly Regex RegexSubstring = new Regex(@"^.{1,497}\b(?<!\s)", RegexOptions.Compiled);
        private static readonly Regex RegexStripPreAndCode = new Regex("<(pre|code)[^>]*>[^<]+</(pre|code)>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static string StripHtml(this string html)
        {
            if (html.IsNullOrWhiteSpace())
            {
                return string.Empty;
            }

            var strippedFromCode = RegexStripPreAndCode.Replace(html, string.Empty);
            return RegexStripHtml.Replace(strippedFromCode, string.Empty);
        }
        public static string FirstCharacters(this string html, int characters = 500)
        {
            if (html.IsNullOrWhiteSpace())
            {
                return string.Empty;
            }

            var regex = "^.{{1,{0}}}\\b(?<!\\s)".FormatWith(characters);
            return Regex.Match(html, regex).Value;
            //return RegexSubstring.Match(html).Value;
            //Regex.Match(html, @"^.{1,497}\b(?<!\s)").Value
        }

        public static bool IsNullOrEmpty(this string @this)
        {
            return string.IsNullOrEmpty(@this);
        }

        public static bool IsNotNullOrEmpty(this string @this)
        {
            return !@this.IsNullOrEmpty();
        }

        public static bool IsNullOrWhiteSpace(this string @this)
        {
            return string.IsNullOrWhiteSpace(@this);
        }

        public static bool IsNotNullOrWhiteSpace(this string @this)
        {
            return !@this.IsNullOrWhiteSpace();
        }

        public static string FormatWith(this string @this, params object[] args)
        {
            return string.Format(@this, args);
        }

        public static bool DoesNotContain(this string @this, string value)
        {
            if(@this == null)
            {
                throw new ArgumentNullException("this", "String on which method is called should not be null");
            }

            return !@this.Contains(value);
        }

        public static TEnum ToEnum<TEnum>(this string @this) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), @this, true);
        }

        public static TType FromJson<TType>(this string source)
            where TType : class
        {
            return JsonConvert.DeserializeObject<TType>(source);
        }

        public static string OrDefault(this string @this, string defaultValue)
        {
            if(@this.IsNullOrEmpty())
            {
                return defaultValue ?? string.Empty;
            }

            return @this;
        }
    }
}