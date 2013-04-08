using System;
using System.Web.Mvc;

namespace MovingScrewdriver.Web.Infrastructure.AutoMapper.Resolvers
{
    public static class DescriptionResolver
    {
        public static string ResolveWithTrailling(MvcHtmlString content, int characters = 500)
        {
            string stripped;
            if (resolve(content, characters, out stripped))
            {
                return "{0}...".FormatWith(stripped);
            }

            return stripped;
        }

        public static string Resolve(MvcHtmlString content, int characters = 500)
        {
            string stripped;
            resolve(content, characters, out stripped);

            return stripped;
        }

        private static bool resolve(MvcHtmlString content, int characters, out string resolved)
        {
            var stripped = content == null ?
                        string.Empty : content.ToHtmlString().StripHtml();

            if (stripped.Length > characters)
            {
                resolved = stripped.FirstCharacters(characters);
                return true;
            }

            resolved = stripped;
            return false;
        }
    }
}