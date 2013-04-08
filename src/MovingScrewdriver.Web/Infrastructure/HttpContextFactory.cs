using System;
using System.Web;

namespace MovingScrewdriver.Web.Infrastructure
{
    public static class HttpContextFactory
    {
        [ThreadStatic]
        private static HttpContextBase _mockHttpContext;

        public static void SetHttpContext(HttpContextBase httpContextBase)
        {
            _mockHttpContext = httpContextBase;
        }

        public static void ResetHttpContext()
        {
            _mockHttpContext = null;
        }

        public static HttpContextBase GetHttpContext()
        {
            if(_mockHttpContext != null)
            {
                return _mockHttpContext;
            }

            if(HttpContext.Current != null)
            {
                return new HttpContextWrapper(HttpContext.Current);
            }

            return null;
        }
    }
}