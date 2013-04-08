using System;
using System.IO;
using System.Net;

namespace MovingScrewdriver.Web.Infrastructure
{
    public class RemoteFile
    {
        private readonly Uri _url;
        private readonly int _timeoutLength;
        private WebRequest _webRequest;

        public RemoteFile(Uri filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }

            _url = filePath;
            _timeoutLength = 30000;
        }

        public WebResponse GetWebResponse()
        {
            var response = GetWebRequest().GetResponse();

            var contentLength = response.ContentLength;

            // WebResponse.ContentLength doesn't always know the value, it returns -1 in this case.
            if (contentLength == -1)
            {
                // Response headers may still have the Content-Length inside of it.
                string headerContentLength = response.Headers["Content-Length"];

                if (headerContentLength.IsNotNullOrEmpty())
                {
                    contentLength = long.Parse(headerContentLength);
                }

            }

            // -1 means an unknown ContentLength was found
            // Numbers any lower than that will always indicate that someone tampered with
            // the Content-Length header.
            if (contentLength <= -1)
            {
                response.Close();
                return null;
            }
            
            if (contentLength > 524288)
            {
                response.Close();
                return null;
            }

            return response;
        }


        public string GetFileAsString()
        {
            using (var response = GetWebResponse())
            {
                if (response == null)
                {
                    return string.Empty;
                }

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private WebRequest GetWebRequest()
        {
            if (_webRequest != null)
            {
                return _webRequest;
            }

            var request = (HttpWebRequest)WebRequest.Create(_url);
            request.Headers["Accept-Encoding"] = "gzip";
            request.Headers["Accept-Language"] = "en-us";
            request.Credentials = CredentialCache.DefaultNetworkCredentials;
            request.AutomaticDecompression = DecompressionMethods.GZip;

            if (_timeoutLength > 0)
            {
                request.Timeout = _timeoutLength;
            }

            return _webRequest = request;
        }
    }
}