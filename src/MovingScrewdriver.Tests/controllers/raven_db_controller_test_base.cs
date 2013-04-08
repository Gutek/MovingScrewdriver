using System.Web;
using System.Web.Mvc;
using FakeItEasy;

namespace MovingScrewdriver.Tests.controllers
{
    public abstract class raven_db_controller_test_base : ravendb_test_base
    {
        protected readonly HttpResponseBase _responseBase;
        protected readonly HttpRequestBase _requestBase;
        protected readonly HttpContextBase _httpContext;
        protected ControllerContext _controllerContext;

        public raven_db_controller_test_base()
        {
            _responseBase = A.Fake<HttpResponseBase>();
            _requestBase = A.Fake<HttpRequestBase>();
            _httpContext = A.Fake<HttpContextBase>();

            A.CallTo(() => _httpContext.Response).Returns(_responseBase);
            A.CallTo(() => _httpContext.Request).Returns(_requestBase);
        }
    }
}