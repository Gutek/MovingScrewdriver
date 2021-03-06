﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MovingScrewdriver.Web.Extensions;
using NLog;

namespace MovingScrewdriver.Web
{
    public class MvcApplication : HttpApplication
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            
            AutofacConfig.Configure();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var urlHelper = new UrlHelper(Context.Request.RequestContext);
            Context.Response.AddHeader("X-Pingback", urlHelper.AbsoluteContent("~/services/pingback.ashx"));
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //using (var session = AutofacConfig.IoC.Resolve<IDocumentSession>())
            //{
            //    if (Server.GetLastError() != null)
            //    {
            //        return;
            //    }

            //    session.SaveChanges();
            //}
        }
        
        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            _log.ErrorException("Unhandled exception occured", ex);

            var webEx = InnerExLoop<WebException>(ex);

            // don't like is here but in most cases this will not be socket ex, so no point
            // of ifing and figuring out how to write different code
            if (webEx != null && webEx.InnerException is SocketException)
            {
                var socketEx = webEx.InnerException as SocketException;
                switch (socketEx.SocketErrorCode)
                {
                    case SocketError.AddressNotAvailable:
                    case SocketError.NetworkDown:
                    case SocketError.NetworkUnreachable:
                    case SocketError.ConnectionAborted:
                    case SocketError.ConnectionReset:
                    case SocketError.TimedOut:
                    case SocketError.ConnectionRefused:
                    case SocketError.HostDown:
                    case SocketError.HostUnreachable:
                    case SocketError.HostNotFound:
                        Context.Response.Redirect("~/blad/ravendb");
                        break;
                    default:
                        break;
                }
            }
        }

        private static TTarget InnerExLoop<TTarget>(Exception ex)
            where TTarget : Exception
        {
            if (ex == null)
            {
                return null;
            }

            var current = ex as TTarget;

            if (current != null)
            {
                return current;
            }
            else
            {
                return InnerExLoop<TTarget>(ex.InnerException);
            }
        }
    }
}