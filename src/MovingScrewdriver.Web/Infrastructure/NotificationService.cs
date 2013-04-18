using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using MovingScrewdriver.Web.Models;
using NLog;
using Raven.Client;
using RestSharp;

namespace MovingScrewdriver.Web.Infrastructure
{
    public interface INotificationService
    {
        void Send(Post post, Uri itemUri);
        void SendAsync(Post post, Uri itemUri);
    }

    public class NotificationService : INotificationService
    {
        private static readonly Regex TrackbackLinkRegex = new Regex(
            "trackback:ping=\"([^\"]+)\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex UrlsRegex = new Regex(
            @"<a.*?href=[""'](?<url>.*?)[""'].*?>(?<name>.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Compiled);



        public void Send(Post post, Uri itemUri)
        {
            _log.Trace("sending notification for Post: {0}", post.Title);
            var uris = get_uris(post.Content);
            foreach (var uri in uris)
            {
                _log.Trace("Uri: {0} found in post content, trying to send trackback", uri);
                var trackback = false;

                // todo: send trackback

                if (!trackback)
                {
                    _log.Trace("Trackback didnt work, sending ping to: {0}", uri);
                    send_ping(itemUri, uri);
                }
            }
        }

        public void SendAsync(Post post, Uri itemUri)
        {
            new Task(() => Send(post, itemUri)).Start();
        }

        private void send_ping(Uri itemUri, Uri targetUri)
        {
            if (itemUri == null
                || targetUri == null)
            {
                return;
            }

            try
            {
                var request = new RestRequest(Method.GET);
                _restClient.BaseUrl = targetUri.AbsoluteUri;
                var response = _restClient.Execute(request);

                var pingback = response.Headers.Where(x => 
                    x.Name.Equals("x-pingback", StringComparison.OrdinalIgnoreCase)
                    || x.Name.Equals("pingback", StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.Value).FirstOrDefault() as string;

                Uri pingUri;
                if (pingback.IsNotNullOrWhiteSpace() 
                    && Uri.TryCreate(pingback, UriKind.Absolute, out pingUri))
                {
                    request = new RestRequest(Method.POST);
                    _restClient.BaseUrl = pingUri.AbsoluteUri;
                    request.RequestFormat = DataFormat.Xml;

                    request.AddBody(new RpcMethodCall
                    {
                        methodCall = new RpcMethodCall.Method
                        {
                            methodName = "pingback.ping",
                            @params = new List<RpcMethodCall.Method.Param>
                            {
                                new RpcMethodCall.Method.Param
                                {
                                    param = new RpcMethodCall.Method.Param.ParamInternal
                                    {
                                        value = new RpcMethodCall.Method.Param.ParamInternal.Value()
                                        {
                                            @string = itemUri.AbsolutePath
                                        }
                                    }
                                },
                                new RpcMethodCall.Method.Param
                                {
                                    param = new RpcMethodCall.Method.Param.ParamInternal
                                    {
                                        value = new RpcMethodCall.Method.Param.ParamInternal.Value()
                                        {
                                            @string = targetUri.AbsolutePath
                                        }
                                    }
                                },
                            }
                        }
                    });

                    // we do not care about error
                    var resp = _restClient.Execute(request);
                    _log.Trace("Response: {0}", resp.Content);
                }
            }
            catch (Exception ex)
            {
                _log.ErrorException("Error occured during sending pingback from: {0} to {1}".FormatWith(itemUri, targetUri), ex);
            }
        }

        private IEnumerable<Uri> get_uris(string content)
        {
            var urlsList = new List<Uri>();
            var matches = UrlsRegex
                .Matches(content)
                .Cast<Match>()
                .Select(myMatch => myMatch.Groups["url"].ToString().Trim())
                .Distinct();

            foreach (var url in matches)
            {
                Uri uri;
                if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                {
                    urlsList.Add(uri);
                }
            }

            return urlsList;
        }

        private readonly IRestClient _restClient;
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        
        public NotificationService(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public class RpcMethodCall
        {
            public Method methodCall { get; set; }

            public class Method
            {
                public string methodName { get; set; }
                public IList<Param> @params { get; set; }
                public class Param
                {
                    public ParamInternal @param { get; set; }
                    public class ParamInternal {

                        public Value value { get; set; }
                        public class Value
                        {
                            public string @string { get; set; }
                        }
                    }
                }
            }
        }
    }
}