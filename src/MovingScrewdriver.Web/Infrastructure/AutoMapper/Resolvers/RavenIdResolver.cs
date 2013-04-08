using System;
using System.Text.RegularExpressions;

namespace MovingScrewdriver.Web.Infrastructure.AutoMapper.Resolvers
{
    public class RavenIdResolver
    {
        public static int Resolve(string ravenId)
        {
            var match = Regex.Match(ravenId, @"\d+");
            var idStr = match.Value;
            int id = int.Parse(idStr);
            if (id == 0)
            {
                throw new InvalidOperationException("Id cannot be zero.");
            }

            return id;
        }
    }
}
