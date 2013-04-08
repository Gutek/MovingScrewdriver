using System.Web;
using System.Web.Mvc;
using FakeItEasy;

namespace MovingScrewdriver.Tests.controllers
{
    public class controller_tests_base
    {
        protected readonly HttpResponseBase _responseBase;
        protected readonly HttpRequestBase _requestBase;
        protected readonly HttpContextBase _httpContext;
        protected readonly ControllerContext _controllerContext;

        public controller_tests_base()
        {
            _responseBase = A.Fake<HttpResponseBase>();
            _requestBase = A.Fake<HttpRequestBase>();
            _httpContext = A.Fake<HttpContextBase>();
            _controllerContext = A.Fake<ControllerContext>();

            A.CallTo(() => _controllerContext.HttpContext).Returns(_httpContext);
            A.CallTo(() => _httpContext.Response).Returns(_responseBase);
            A.CallTo(() => _httpContext.Request).Returns(_requestBase);
        }
    }
}