using System;
using System.IO.Compression;
using System.Web;

namespace MovingScrewdriver.Web.Infrastructure
{
    public class GzipModule : IHttpModule
    {
        public void Init(HttpApplication application)
        {
#if !DEBUG
            application.BeginRequest += Application_BeginRequest;
#endif
        }

        public void Dispose()
        {
        }

        private void Application_BeginRequest(Object source, EventArgs e)
        {
            var context = HttpContextFactory.GetHttpContext();
            var request = context.Request;
            var response = context.Response;

            if (request.AcceptTypes == null)
            {
                return;
            }

            var accept = string.Join(" ", request.AcceptTypes);
            
            if (accept.IsNullOrWhiteSpace())
            {
                return;
            }

            accept = accept.ToUpperInvariant();
            
            if (accept.DoesNotContain("TEXT/")
                || accept.DoesNotContain("MESSAGE/")
                || accept.DoesNotContain("APPLICATION/X-JAVASCRIPT")
                || accept.DoesNotContain("APPLICATION/JAVASCRIPT")
                || accept.DoesNotContain("APPLICATION/JSON")
                )
            {
                return;
            }

            string acceptEncoding = request.Headers["Accept-Encoding"];

            if (acceptEncoding.IsNullOrWhiteSpace())
            {
                return;
            }
                
            acceptEncoding = acceptEncoding.ToUpperInvariant();

            if (acceptEncoding.Contains("GZIP"))
            {
                response.AppendHeader("Content-Encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains("DEFLATE"))
            {
                response.AppendHeader("Content-Encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
        }
    }
}