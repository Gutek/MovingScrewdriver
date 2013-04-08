using System;
using Raven.Client;

namespace MovingScrewdriver.Web.Controllers.Syndication
{
    public partial class SyndicationController : AbstractController
    {
        private static readonly string EtagInitValue = Guid.NewGuid().ToString();
        private bool CheckEtag(RavenQueryStatistics stats, out string responseETagHeader)
        {
            string requestETagHeader = Request.Headers["If-None-Match"] ?? string.Empty;
            responseETagHeader = stats.Timestamp.ToString("o") + EtagInitValue;
            return requestETagHeader == responseETagHeader;
        }
    }
}