using System;
using System.IO;
using System.Text;
using System.Web.Optimization;
using ICSharpCode.SharpZipLib.GZip;

namespace MovingScrewdriver.Web.Infrastructure
{
    public class GZipTransform : IBundleTransform
    {
        readonly string _contentType;

        public GZipTransform(string contentType)
        {
            _contentType = contentType;
        }

        public void Process(BundleContext context, BundleResponse response)
        {
            var contentBytes = new UTF8Encoding().GetBytes(response.Content);

            var outputStream = new MemoryStream();
            var gzipOutputStream = new GZipOutputStream(outputStream);
            gzipOutputStream.Write(contentBytes, 0, contentBytes.Length);

            var outputBytes = outputStream.GetBuffer();
            response.Content = Convert.ToBase64String(outputBytes);


            // NOTE: this part is broken -> http://stackoverflow.com/a/11353652
            context.HttpContext.Response.Headers["Content-Encoding"] = "gzip";
            response.ContentType = _contentType;
        }
    }
}